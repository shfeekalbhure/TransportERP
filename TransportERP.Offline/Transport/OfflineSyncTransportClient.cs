using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using TransportERP.Application.Sync;

namespace TransportERP.Offline.Transport;

public sealed record OfflineSyncTransportOptions(
    Uri BatchEndpoint,
    string DeviceId,
    Guid RegisteredDeviceId,
    Guid CompanyId,
    Guid BranchId,
    Guid UserId,
    string WorkerId,
    TimeSpan? LeaseDuration = null,
    int MaximumBatchOperations = 100,
    TimeSpan? AcceptedPollInterval = null,
    int MaximumRequestBodyBytes = 2_097_152,
    int MaximumPayloadBytes = 16_384,
    BuildIdentityV1? BuildIdentity = null)
{
    public TimeSpan EffectiveLeaseDuration => LeaseDuration ?? TimeSpan.FromMinutes(2);
    public TimeSpan EffectiveAcceptedPollInterval => AcceptedPollInterval ?? TimeSpan.FromSeconds(5);
}

public sealed record OfflineSyncTransportRunResult(
    int Claimed,
    int Succeeded,
    int Conflicted,
    int Rejected,
    int RetryScheduled,
    int AcceptedPending = 0);

/// <summary>
/// sync-v1 client transport. Authentication artifacts exist only for one HTTP request and are
/// never written to the outbox. The body is serialized once and reused byte-for-byte for nonce
/// acquisition and every signed send in the same local attempt.
/// </summary>
public sealed class OfflineSyncTransportClient
{
    private const string ProtocolVersion = "sync-v1";
    private const string CorrelationHeader = "X-Correlation-Id";
    private const string NonceHeader = "DPoP-Nonce";
    private readonly HttpClient _httpClient;
    private readonly OfflineOperationStore _store;
    private readonly IInMemoryBearerTokenProvider _bearerTokens;
    private readonly SyncDpopProofFactory _proofs;
    private readonly OfflineSyncTransportOptions _options;
    private readonly string _canonicalHtu;

    public OfflineOperationScope Scope { get; }

    public OfflineSyncTransportClient(
        HttpClient httpClient,
        OfflineOperationStore store,
        IInMemoryBearerTokenProvider bearerTokens,
        IDeviceProofSigningKey signingKey,
        OfflineSyncTransportOptions options,
        TimeProvider? timeProvider = null)
    {
        _httpClient = httpClient;
        _store = store;
        _bearerTokens = bearerTokens;
        _options = options;
        _canonicalHtu = CanonicalizeBatchEndpoint(options.BatchEndpoint);
        Scope = new OfflineOperationScope(
            options.CompanyId, options.BranchId, options.UserId, options.RegisteredDeviceId);
        Scope.Validate();
        _proofs = new SyncDpopProofFactory(signingKey, timeProvider ?? TimeProvider.System);

        if (string.IsNullOrWhiteSpace(options.DeviceId) || options.DeviceId.Any(char.IsWhiteSpace) ||
            options.RegisteredDeviceId == Guid.Empty || options.CompanyId == Guid.Empty ||
            options.BranchId == Guid.Empty || options.UserId == Guid.Empty ||
            string.IsNullOrWhiteSpace(options.WorkerId) ||
            options.EffectiveLeaseDuration <= TimeSpan.Zero ||
            options.EffectiveAcceptedPollInterval <= TimeSpan.Zero ||
            options.EffectiveAcceptedPollInterval > TimeSpan.FromHours(1) ||
            options.MaximumBatchOperations is < 1 or > 100 ||
            options.MaximumRequestBodyBytes is < 1 or > 2_097_152 ||
            options.MaximumPayloadBytes is < 1 or > 16_384 ||
            options.MaximumPayloadBytes > options.MaximumRequestBodyBytes ||
            options.BuildIdentity is not { IsValid: true })
            throw new ArgumentException("The sync transport options are invalid.", nameof(options));
    }

    public async Task<OfflineSyncTransportRunResult> ProcessNextBatchAsync(
        int? maximumOperations = null,
        CancellationToken cancellationToken = default)
    {
        var limit = maximumOperations ?? _options.MaximumBatchOperations;
        if (limit is < 1 or > 100 || limit > _options.MaximumBatchOperations)
            throw new ArgumentOutOfRangeException(nameof(maximumOperations));

        var claimed = new List<OfflineOperation>(limit);
        var claimedLocalOperationIds = new HashSet<Guid>();
        for (var index = 0; index < limit; index++)
        {
            var operation = await _store.ClaimNextExcludingAsync(
                _options.WorkerId,
                _options.EffectiveLeaseDuration,
                Scope,
                claimedLocalOperationIds,
                cancellationToken);
            if (operation is null) break;
            claimed.Add(operation);
            claimedLocalOperationIds.Add(operation.LocalOperationId);
        }

        if (claimed.Count == 0) return new(0, 0, 0, 0, 0);

        var eligible = new List<OfflineOperation>(claimed.Count);
        var rejectedBeforeSend = 0;
        foreach (var operation in claimed)
        {
            if (operation.AttemptCorrelationId is null)
                throw new OfflineStoreException("LOCAL_STORE_CORRUPT", "A claimed operation has no attempt identity.");
            var scopeInvalid = operation.RegisteredDeviceId != _options.RegisteredDeviceId ||
                operation.CompanyId != _options.CompanyId || operation.BranchId != _options.BranchId ||
                operation.UserId != _options.UserId;
            var payloadInvalid = operation.PayloadJson is null ||
                Encoding.UTF8.GetByteCount(operation.PayloadJson) > _options.MaximumPayloadBytes;
            if (scopeInvalid || payloadInvalid)
            {
                await _store.MarkRejectedAsync(
                    operation.LocalOperationId,
                    operation.AttemptCorrelationId.Value,
                    scopeInvalid ? "LOCAL_SCOPE_INVALID" : "PAYLOAD_TOO_LARGE",
                    cancellationToken);
                rejectedBeforeSend++;
                continue;
            }
            eligible.Add(operation);
        }

        if (eligible.Count == 0)
            return new(claimed.Count, 0, 0, rejectedBeforeSend, 0);

        var body = CreateBody(eligible);
        if (body.Length > _options.MaximumRequestBodyBytes)
        {
            var oversized = await CompleteRequestFailureAsync(
                eligible, "REQUEST_BODY_TOO_LARGE", retryable: false, cancellationToken);
            return oversized with
            {
                Claimed = claimed.Count,
                Rejected = oversized.Rejected + rejectedBeforeSend
            };
        }
        SyncV1BatchResponse? response = null;
        string? requestError = null;
        var requestErrorRetryable = false;
        // Local claim-attempt identity is only the encrypted-store CAS token. Every HTTP request
        // below receives an independent wire AttemptCorrelationId and, when signed, matching cid.
        try
        {
            var bearer = await _bearerTokens.GetBearerTokenAsync(cancellationToken);
            ValidateBearer(bearer);
            var nonceResult = await AcquireNonceAsync(body, bearer, cancellationToken);
            if (nonceResult.ErrorCode is not null)
            {
                requestError = nonceResult.ErrorCode;
                requestErrorRetryable = nonceResult.Retryable;
            }
            else
            {
                var signedResult = await SendSignedAsync(
                    body, bearer, nonceResult.Nonce!, cancellationToken);
                response = signedResult.Response;
                requestError = signedResult.ErrorCode;
                requestErrorRetryable = signedResult.Retryable;
            }
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            requestError = "TIMEOUT";
            requestErrorRetryable = true;
        }
        catch (HttpRequestException)
        {
            requestError = "NO_RESPONSE";
            requestErrorRetryable = true;
        }
        catch (JsonException)
        {
            requestError = "INTERNAL_ERROR";
            requestErrorRetryable = true;
        }
        catch (SyncTransportException exception)
        {
            requestError = exception.Code;
            requestErrorRetryable = exception.Retryable;
        }
        catch (CryptographicException)
        {
            requestError = "DEVICE_PROOF_KEY_INVALID";
            requestErrorRetryable = false;
        }

        if (response is null)
        {
            var outcome = await CompleteRequestFailureAsync(
                eligible, requestError ?? "INTERNAL_ERROR", requestErrorRetryable, cancellationToken);
            return outcome with { Claimed = claimed.Count, Rejected = outcome.Rejected + rejectedBeforeSend };
        }

        var mapped = await ApplyResultsAsync(eligible, response, cancellationToken);
        return mapped with { Claimed = claimed.Count, Rejected = mapped.Rejected + rejectedBeforeSend };
    }

    private byte[] CreateBody(IReadOnlyList<OfflineOperation> operations)
    {
        var requests = operations.Select(operation => new SyncV1OperationRequest(
            operation.ActionCode,
            operation.OperationType,
            operation.EntityType,
            operation.EntityId,
            operation.ClientOperationId,
            operation.PayloadJson!,
            operation.PayloadHash,
            FormatClientTime(operation.ClientOccurredAt),
            operation.OperationCorrelationId,
            operation.BaseVersion)).ToArray();
        return SyncV1Json.Serialize(new SyncV1BatchRequest(
            _options.DeviceId, ProtocolVersion, requests, _options.BuildIdentity!));
    }

    private async Task<(string? Nonce, string? ErrorCode, bool Retryable)> AcquireNonceAsync(
        byte[] body,
        string bearer,
        CancellationToken cancellationToken)
    {
        var attemptCorrelationId = Guid.NewGuid();
        using var request = CreateRequest(body, bearer, attemptCorrelationId, proof: null);
        using var response = await _httpClient.SendAsync(
            request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        if (response.StatusCode == HttpStatusCode.Unauthorized &&
            response.Headers.TryGetValues(NonceHeader, out var values))
        {
            var nonces = values.ToArray();
            if (nonces.Length == 1 && !string.IsNullOrEmpty(nonces[0])) return (nonces[0], null, false);
            return (null, "NONCE_CHALLENGE_INVALID", false);
        }

        var errorCode = await ReadErrorCodeAsync(response, attemptCorrelationId, cancellationToken);
        return (null, errorCode, IsRetryableCode(errorCode) || IsRetryableStatus(response.StatusCode));
    }

    private async Task<(SyncV1BatchResponse? Response, string? ErrorCode, bool Retryable)> SendSignedAsync(
        byte[] body,
        string bearer,
        string nonce,
        CancellationToken cancellationToken)
    {
        // One nonce refresh is accepted. Each signed HTTP attempt still receives a new jti and signature.
        for (var signedAttempt = 0; signedAttempt < 2; signedAttempt++)
        {
            var attemptCorrelationId = Guid.NewGuid();
            var proof = await _proofs.CreateAsync(
                _canonicalHtu, bearer, body, nonce, attemptCorrelationId, cancellationToken);
            using var request = CreateRequest(body, bearer, attemptCorrelationId, proof);
            using var response = await _httpClient.SendAsync(
                request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var responseBytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);
                var envelope = SyncV1Json.Deserialize<SyncV1BatchResponse>(responseBytes)
                    ?? throw new JsonException("The sync response was empty.");
                if (!string.Equals(envelope.ProtocolVersion, ProtocolVersion, StringComparison.Ordinal) ||
                    envelope.AttemptCorrelationId != attemptCorrelationId || envelope.Results is null)
                    throw new JsonException("The sync response envelope did not match the request.");
                return (envelope, null, false);
            }

            var errorCode = await ReadErrorCodeAsync(response, attemptCorrelationId, cancellationToken);
            if (signedAttempt == 0 && response.StatusCode == HttpStatusCode.Unauthorized &&
                string.Equals(errorCode, "use_dpop_nonce", StringComparison.Ordinal) &&
                response.Headers.TryGetValues(NonceHeader, out var refreshValues))
            {
                var refreshed = refreshValues.ToArray();
                if (refreshed.Length == 1 && !string.IsNullOrEmpty(refreshed[0]))
                {
                    nonce = refreshed[0];
                    continue;
                }
            }
            return (null, errorCode, IsRetryableCode(errorCode) || IsRetryableStatus(response.StatusCode));
        }

        return (null, "NONCE_CHALLENGE_INVALID", false);
    }

    private HttpRequestMessage CreateRequest(
        byte[] body,
        string bearer,
        Guid attemptCorrelationId,
        string? proof)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, _options.BatchEndpoint)
        {
            Content = new ByteArrayContent(body)
        };
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json") { CharSet = "utf-8" };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearer);
        request.Headers.TryAddWithoutValidation(CorrelationHeader, attemptCorrelationId.ToString("D"));
        if (proof is not null) request.Headers.TryAddWithoutValidation("DPoP", proof);
        return request;
    }

    private async Task<OfflineSyncTransportRunResult> ApplyResultsAsync(
        IReadOnlyList<OfflineOperation> operations,
        SyncV1BatchResponse response,
        CancellationToken cancellationToken)
    {
        var expected = operations.ToDictionary(
            operation => (operation.ClientOperationId, operation.OperationCorrelationId));
        var returned = new Dictionary<(string, Guid), SyncV1OperationResult>();
        var responseInvalid = false;
        foreach (var result in response.Results)
        {
            if (string.IsNullOrEmpty(result.ClientOperationId) || result.OperationCorrelationId is null ||
                !expected.TryGetValue((result.ClientOperationId, result.OperationCorrelationId.Value), out var operation) ||
                !string.Equals(result.ActionCode, operation.ActionCode, StringComparison.Ordinal) ||
                !returned.TryAdd((result.ClientOperationId, result.OperationCorrelationId.Value), result))
            {
                responseInvalid = true;
                break;
            }
        }

        if (responseInvalid)
            return await CompleteRequestFailureAsync(operations, "INTERNAL_ERROR", true, cancellationToken);

        var succeeded = 0;
        var conflicted = 0;
        var rejected = 0;
        var retryScheduled = 0;
        var acceptedPending = 0;
        foreach (var operation in operations)
        {
            if (!returned.TryGetValue((operation.ClientOperationId, operation.OperationCorrelationId), out var result))
            {
                var missingResult = await _store.MarkTransportFailureAsync(operation.LocalOperationId,
                    operation.AttemptCorrelationId!.Value, true, "INTERNAL_ERROR", cancellationToken);
                CountFailure(missingResult, ref retryScheduled, ref rejected);
                continue;
            }

            switch (result.Status)
            {
                case "QUEUED":
                case "SENDING":
                    if (result.ServerOperationId is not { } serverOperationId || serverOperationId == Guid.Empty)
                    {
                        var malformedPending = await _store.MarkTransportFailureAsync(operation.LocalOperationId,
                            operation.AttemptCorrelationId!.Value, true, "INTERNAL_ERROR", cancellationToken);
                        CountFailure(malformedPending, ref retryScheduled, ref rejected);
                        break;
                    }
                    await _store.MarkAcceptedPendingAsync(operation.LocalOperationId,
                        operation.AttemptCorrelationId!.Value, serverOperationId, result.Status,
                        _options.EffectiveAcceptedPollInterval, cancellationToken);
                    acceptedPending++;
                    break;
                case "SUCCEEDED":
                    if (result.ResultEntityId is not { } resultEntityId || resultEntityId == Guid.Empty)
                    {
                        var malformedSuccess = await _store.MarkTransportFailureAsync(operation.LocalOperationId,
                            operation.AttemptCorrelationId!.Value, true, "INTERNAL_ERROR", cancellationToken);
                        CountFailure(malformedSuccess, ref retryScheduled, ref rejected);
                        break;
                    }
                    await _store.MarkSucceededAsync(operation.LocalOperationId,
                        operation.AttemptCorrelationId!.Value, resultEntityId, result.ResultVersion,
                        result.ServerOperationId, cancellationToken);
                    succeeded++;
                    break;
                case "CONFLICT" when result.ConflictCaseId is { } conflictCaseId && conflictCaseId != Guid.Empty:
                    if (!TryCreateConflictReview(operation, result, out var conflictReview))
                    {
                        var malformedConflict = await _store.MarkTransportFailureAsync(operation.LocalOperationId,
                            operation.AttemptCorrelationId!.Value, true, "INTERNAL_ERROR", cancellationToken);
                        CountFailure(malformedConflict, ref retryScheduled, ref rejected);
                        break;
                    }
                    await _store.MarkConflictAsync(operation.LocalOperationId,
                        operation.AttemptCorrelationId!.Value, conflictCaseId,
                        result.ErrorCode ?? "BASE_VERSION_CONFLICT", conflictReview, result.ServerOperationId,
                        cancellationToken);
                    conflicted++;
                    break;
                case "CONFLICT":
                    var conflictFailure = await _store.MarkTransportFailureAsync(operation.LocalOperationId,
                        operation.AttemptCorrelationId!.Value, true, "INTERNAL_ERROR", cancellationToken);
                    CountFailure(conflictFailure, ref retryScheduled, ref rejected);
                    break;
                case "FAILED" when IsRetryableCode(result.ErrorCode):
                case "REJECTED" when IsRetryableCode(result.ErrorCode):
                    var retryFailure = await _store.MarkTransportFailureAsync(operation.LocalOperationId,
                        operation.AttemptCorrelationId!.Value, true, result.ErrorCode!, cancellationToken);
                    CountFailure(retryFailure, ref retryScheduled, ref rejected);
                    break;
                case "FAILED":
                case "REJECTED":
                    await _store.MarkRejectedAsync(operation.LocalOperationId,
                        operation.AttemptCorrelationId!.Value, result.ErrorCode ?? "SYNC_REJECTED", cancellationToken);
                    rejected++;
                    break;
                default:
                    await _store.MarkRejectedAsync(operation.LocalOperationId,
                        operation.AttemptCorrelationId!.Value, "RESPONSE_STATUS_INVALID", cancellationToken);
                    rejected++;
                    break;
            }
        }

        return new(operations.Count, succeeded, conflicted, rejected, retryScheduled, acceptedPending);
    }

    private static bool TryCreateConflictReview(
        OfflineOperation operation,
        SyncV1OperationResult result,
        out OfflineConflictReview review)
    {
        review = null!;
        var source = result.ConflictReview;
        if (result.ServerOperationId is not { } serverOperationId || serverOperationId == Guid.Empty ||
            source is null || source.BaseVersion is not { } baseVersion || baseVersion <= 0 ||
            operation.BaseVersion != baseVersion || !IsSafeReviewCode(source.ConflictReason) ||
            !string.Equals(result.ErrorCode, source.ConflictReason, StringComparison.Ordinal) ||
            source.LocalSnapshot is not { } local ||
            source.ServerSnapshot is not { } server || source.Status != "OPEN" ||
            source.Resolution is not null || source.ResolvedByAuthorizedUser || source.ResolvedAt.HasValue ||
            source.ReplacedByOperationId.HasValue || local.RequestedBaseVersion != baseVersion ||
            local.ActionCode != operation.ActionCode || local.EntityType != operation.EntityType ||
            local.EntityId != operation.EntityId || server.EntityType != operation.EntityType ||
            server.EntityId != operation.EntityId || server.Exists is null)
            return false;

        review = new OfflineConflictReview(
            baseVersion,
            source.ConflictReason!,
            new OfflineConflictLocalSnapshot(local.ActionCode, local.EntityType, local.EntityId, baseVersion),
            new OfflineConflictServerSnapshot(server.EntityType, server.EntityId, server.Exists.Value,
                server.CurrentVersion),
            source.Status);
        return review.IsDecisionReady;
    }

    private static bool IsSafeReviewCode(string? value) =>
        !string.IsNullOrWhiteSpace(value) && value.Length <= 120 &&
        value.All(character => char.IsAsciiLetterOrDigit(character) || character is '_' or '-' or '.');

    private async Task<OfflineSyncTransportRunResult> CompleteRequestFailureAsync(
        IReadOnlyList<OfflineOperation> operations,
        string errorCode,
        bool retryable,
        CancellationToken cancellationToken)
    {
        var retryScheduled = 0;
        var rejected = 0;
        foreach (var operation in operations)
        {
            var disposition = await _store.MarkTransportFailureAsync(operation.LocalOperationId,
                operation.AttemptCorrelationId!.Value, retryable, errorCode, cancellationToken);
            CountFailure(disposition, ref retryScheduled, ref rejected);
        }
        return new(operations.Count, 0, 0, rejected, retryScheduled);
    }

    private static async Task<string> ReadErrorCodeAsync(
        HttpResponseMessage response,
        Guid expectedCorrelationId,
        CancellationToken cancellationToken)
    {
        try
        {
            var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);
            var error = SyncV1Json.Deserialize<SyncV1ErrorResponse>(bytes);
            if (error?.CorrelationId is not null && error.CorrelationId != expectedCorrelationId)
                return "INTERNAL_ERROR";
            return string.IsNullOrEmpty(error?.ErrorCode) ? "HTTP_REJECTED" : error.ErrorCode;
        }
        catch (JsonException)
        {
            return response.StatusCode == HttpStatusCode.TooManyRequests ? "RATE_LIMITED" : "HTTP_REJECTED";
        }
    }

    private static bool IsRetryableCode(string? code) =>
        string.Equals(code, "INTERNAL_ERROR", StringComparison.Ordinal) ||
        string.Equals(code, "RATE_LIMITED", StringComparison.Ordinal) ||
        string.Equals(code, "TIMEOUT", StringComparison.Ordinal) ||
        string.Equals(code, "NO_RESPONSE", StringComparison.Ordinal);

    private static bool IsRetryableStatus(HttpStatusCode statusCode) =>
        statusCode == HttpStatusCode.RequestTimeout ||
        statusCode == HttpStatusCode.TooManyRequests ||
        (int)statusCode >= 500;

    private static void CountFailure(
        OfflineTransportFailureDisposition disposition,
        ref int retryScheduled,
        ref int rejected)
    {
        if (disposition == OfflineTransportFailureDisposition.RetryScheduled) retryScheduled++;
        else rejected++;
    }

    private static void ValidateBearer(string bearer)
    {
        if (string.IsNullOrEmpty(bearer) || bearer.Any(character => character > 0x7f || char.IsWhiteSpace(character)))
            throw new SyncTransportException("SESSION_TOKEN_INVALID", retryable: false);
    }

    private static string FormatClientTime(DateTimeOffset value) =>
        value.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.ffffff'Z'",
            System.Globalization.CultureInfo.InvariantCulture);

    private static string CanonicalizeBatchEndpoint(Uri endpoint)
    {
        if (!endpoint.IsAbsoluteUri || endpoint.Scheme != Uri.UriSchemeHttps ||
            !string.IsNullOrEmpty(endpoint.UserInfo) || !string.IsNullOrEmpty(endpoint.Query) ||
            !string.IsNullOrEmpty(endpoint.Fragment) ||
            !string.Equals(endpoint.AbsolutePath, "/api/v1/sync/operations:batch", StringComparison.Ordinal))
            throw new ArgumentException("A canonical HTTPS sync-v1 batch endpoint is required.", nameof(endpoint));
        var host = new System.Globalization.IdnMapping().GetAscii(endpoint.DnsSafeHost).ToLowerInvariant();
        var port = endpoint.IsDefaultPort || endpoint.Port == 443 ? string.Empty : $":{endpoint.Port}";
        return $"https://{host}{port}/api/v1/sync/operations:batch";
    }
}
