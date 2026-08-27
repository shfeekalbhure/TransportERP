using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace TransportERP.Offline.Transport;

public enum OfflineConflictDecision { KeepServer, Reapply }

public sealed class OfflineSyncConflictClient
{
    private const string KeepServerDecision = "KEEP_SERVER_AND_REJECT_LOCAL";
    private const string ReapplyDecision = "REAPPLY_AS_NEW";
    private static readonly string[] ForbiddenReasonFragments =
    [
        "authorization", "bearer ", "token=", "password", "clientsecret", "devicecredential",
        "dpop", "privatekey", "refresh_token", "access_token"
    ];
    private readonly HttpClient _httpClient;
    private readonly OfflineOperationStore _store;
    private readonly IInMemoryBearerTokenProvider _bearerTokens;
    private readonly SyncDpopProofFactory _proofs;
    private readonly OfflineSyncTransportOptions _options;

    public OfflineSyncConflictClient(
        HttpClient httpClient,
        OfflineOperationStore store,
        IInMemoryBearerTokenProvider bearerTokens,
        IDeviceProofSigningKey signingKey,
        OfflineSyncTransportOptions options,
        TimeProvider? timeProvider = null)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _store = store ?? throw new ArgumentNullException(nameof(store));
        _bearerTokens = bearerTokens ?? throw new ArgumentNullException(nameof(bearerTokens));
        _proofs = new SyncDpopProofFactory(signingKey ?? throw new ArgumentNullException(nameof(signingKey)),
            timeProvider ?? TimeProvider.System);
        _options = options ?? throw new ArgumentNullException(nameof(options));
        if (!options.BatchEndpoint.IsAbsoluteUri || options.BatchEndpoint.Scheme != Uri.UriSchemeHttps ||
            !string.Equals(options.BatchEndpoint.AbsolutePath, "/api/v1/sync/operations:batch", StringComparison.Ordinal) ||
            !string.IsNullOrEmpty(options.BatchEndpoint.UserInfo) || !string.IsNullOrEmpty(options.BatchEndpoint.Query) ||
            !string.IsNullOrEmpty(options.BatchEndpoint.Fragment) || options.CompanyId == Guid.Empty ||
            options.BranchId == Guid.Empty || options.UserId == Guid.Empty || options.RegisteredDeviceId == Guid.Empty)
            throw new ArgumentException("The sync conflict transport options are invalid.", nameof(options));
    }

    [Obsolete("A distinct reviewed user reason is required for conflict resolution.")]
    public Task ResolveAsync(
        Guid localOperationId,
        OfflineConflictDecision decision,
        long? reapplyBaseVersion = null,
        CancellationToken cancellationToken = default) =>
        Task.FromException(new OfflineStoreException(
            "CONFLICT_REASON_REQUIRED", "A distinct reviewed user reason is required for conflict resolution."));

    public async Task ResolveAsync(
        Guid localOperationId,
        OfflineConflictDecision decision,
        string reason,
        long? reapplyBaseVersion = null,
        CancellationToken cancellationToken = default)
    {
        ValidateReason(reason);
        var operation = await _store.GetAsync(localOperationId,
                new OfflineOperationScope(_options.CompanyId, _options.BranchId, _options.UserId,
                    _options.RegisteredDeviceId), cancellationToken)
            ?? throw new OfflineStoreException("LOCAL_OPERATION_NOT_FOUND", "The local operation does not exist.");
        Validate(operation, decision, reapplyBaseVersion);

        var conflictCaseId = operation.ConflictCaseId!.Value;
        var body = CreateBody(operation, decision, reason, reapplyBaseVersion);
        var endpoint = ConflictEndpoint(conflictCaseId);
        var htu = endpoint.AbsoluteUri;
        var bearer = await _bearerTokens.GetBearerTokenAsync(cancellationToken);
        ValidateBearer(bearer);

        var nonce = await AcquireNonceAsync(endpoint, body, bearer, cancellationToken);
        var signed = await SendSignedAsync(endpoint, htu, body, bearer, nonce, cancellationToken);
        var response = signed.Response;
        if (response.ConflictCaseId != conflictCaseId ||
            response.OriginalOperationId != operation.ServerOperationId ||
            response.CorrelationId != signed.CorrelationId ||
            !string.Equals(response.ConflictStatus, "RESOLVED", StringComparison.Ordinal) ||
            !string.Equals(response.Decision,
                decision == OfflineConflictDecision.KeepServer ? KeepServerDecision : ReapplyDecision,
                StringComparison.Ordinal) || response.ResolvedAt == default ||
            (decision == OfflineConflictDecision.KeepServer &&
                (response.OriginalOperationStatus != "REJECTED" || response.ReplacedByOperationId.HasValue)) ||
            (decision == OfflineConflictDecision.Reapply &&
                (response.OriginalOperationStatus != "RESOLVED" ||
                 response.ReplacedByOperationId is not { } replacement || replacement == Guid.Empty)))
            throw new SyncTransportException("CONFLICT_RESPONSE_INVALID", retryable: true);

        var scope = new OfflineOperationScope(_options.CompanyId, _options.BranchId, _options.UserId,
            _options.RegisteredDeviceId);
        await _store.MarkResolvedAsync(localOperationId,
            decision == OfflineConflictDecision.KeepServer ? "KEEP_SERVER" : "REAPPLY_AS_NEW",
            new OfflineConflictResolutionOutcome(response.Decision, response.ConflictStatus, true,
                response.ResolvedAt, response.ReplacedByOperationId),
            scope,
            cancellationToken);
    }

    private byte[] CreateBody(
        OfflineOperation operation,
        OfflineConflictDecision decision,
        string reason,
        long? baseVersion)
    {
        SyncV1ConflictReapplyRequest? reapply = null;
        if (decision == OfflineConflictDecision.Reapply)
        {
            var stableOperation = StableGuid(operation.ConflictCaseId!.Value, "replacement-operation");
            var stableCorrelation = StableGuid(operation.ConflictCaseId.Value, "replacement-correlation");
            reapply = new SyncV1ConflictReapplyRequest(
                stableOperation.ToString("D"), stableCorrelation, operation.ActionCode,
                operation.OperationType, operation.EntityType, operation.EntityId, baseVersion,
                operation.ClientOccurredAt, operation.PayloadJson!, operation.PayloadHash);
        }

        return SyncV1Json.Serialize(new SyncV1ConflictResolutionRequest(
            decision == OfflineConflictDecision.KeepServer ? KeepServerDecision : ReapplyDecision,
            reason,
            reapply,
            _options.BuildIdentity!));
    }

    private async Task<string> AcquireNonceAsync(Uri endpoint, byte[] body, string bearer,
        CancellationToken cancellationToken)
    {
        var correlationId = Guid.NewGuid();
        using var request = Request(endpoint, body, bearer, correlationId, proof: null);
        using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead,
            cancellationToken);
        var error = await ErrorAsync(response, correlationId, cancellationToken);
        if (response.StatusCode == HttpStatusCode.Unauthorized &&
            string.Equals(error.Code, "use_dpop_nonce", StringComparison.Ordinal))
        {
            if (response.Headers.TryGetValues("DPoP-Nonce", out var values))
            {
                var nonceValues = values.ToArray();
                if (nonceValues.Length == 1 && !string.IsNullOrEmpty(nonceValues[0]))
                    return nonceValues[0];
            }
            throw new SyncTransportException("NONCE_CHALLENGE_INVALID", retryable: false);
        }
        throw error;
    }

    private async Task<(SyncV1ConflictResolutionResponse Response, Guid CorrelationId)> SendSignedAsync(
        Uri endpoint, string htu, byte[] body, string bearer, string nonce,
        CancellationToken cancellationToken)
    {
        for (var attempt = 0; attempt < 2; attempt++)
        {
            var correlationId = Guid.NewGuid();
            var proof = await _proofs.CreateAsync(htu, bearer, body, nonce, correlationId, cancellationToken);
            using var request = Request(endpoint, body, bearer, correlationId, proof);
            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead,
                cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);
                var parsed = SyncV1Json.Deserialize<SyncV1ConflictResolutionResponse>(bytes)
                    ?? throw new SyncTransportException("CONFLICT_RESPONSE_INVALID", retryable: true);
                return (parsed, correlationId);
            }

            var error = await ErrorAsync(response, correlationId, cancellationToken);
            if (attempt == 0 && response.StatusCode == HttpStatusCode.Unauthorized &&
                string.Equals(error.Code, "use_dpop_nonce", StringComparison.Ordinal) &&
                response.Headers.TryGetValues("DPoP-Nonce", out var refreshed))
            {
                var values = refreshed.ToArray();
                if (values.Length == 1 && !string.IsNullOrEmpty(values[0]))
                {
                    nonce = values[0];
                    continue;
                }
            }
            throw error;
        }
        throw new SyncTransportException("NONCE_CHALLENGE_INVALID", retryable: false);
    }

    private static HttpRequestMessage Request(Uri endpoint, byte[] body, string bearer,
        Guid correlationId, string? proof)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = new ByteArrayContent(body)
        };
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json") { CharSet = "utf-8" };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearer);
        request.Headers.TryAddWithoutValidation("X-Correlation-Id", correlationId.ToString("D"));
        if (proof is not null) request.Headers.TryAddWithoutValidation("DPoP", proof);
        return request;
    }

    private Uri ConflictEndpoint(Guid conflictCaseId)
    {
        var origin = new Uri(_options.BatchEndpoint.GetLeftPart(UriPartial.Authority));
        return new Uri(origin, $"/api/v1/sync/conflicts/{conflictCaseId:D}:resolve");
    }

    private void Validate(OfflineOperation operation, OfflineConflictDecision decision, long? baseVersion)
    {
        if (operation.Status != OfflineOperationStatus.Conflict || operation.ConflictCaseId is null ||
            operation.ConflictCaseId == Guid.Empty || operation.ServerOperationId is null ||
            operation.ServerOperationId == Guid.Empty || operation.ConflictReview is not { IsDecisionReady: true })
            throw new OfflineStoreException("LOCAL_STATE_CONFLICT", "Only a server-bound conflict can be resolved.");
        if (operation.CompanyId != _options.CompanyId || operation.BranchId != _options.BranchId ||
            operation.UserId != _options.UserId || operation.RegisteredDeviceId != _options.RegisteredDeviceId)
            throw new OfflineStoreException("LOCAL_SCOPE_DENIED", "The conflict does not match the authenticated scope.");
        if (decision == OfflineConflictDecision.Reapply &&
            (baseVersion is null or < 0 || string.IsNullOrEmpty(operation.PayloadJson)))
            throw new OfflineStoreException("REAPPLY_INPUT_REQUIRED", "A current base version and retained payload are required.");
        if (decision == OfflineConflictDecision.KeepServer && baseVersion is not null)
            throw new OfflineStoreException("RESOLUTION_INVALID", "KEEP_SERVER cannot include a reapply base version.");
    }

    private static async Task<SyncTransportException> ErrorAsync(
        HttpResponseMessage response, Guid expectedCorrelationId,
        CancellationToken cancellationToken)
    {
        try
        {
            var bytes = await SyncV1Json.ReadBoundedErrorBytesAsync(
                response.Content, cancellationToken);
            var error = SyncV1Json.Deserialize<SyncV1ErrorResponse>(bytes);
            if (error?.CorrelationId != expectedCorrelationId)
                return new SyncTransportException(
                    "CLIENT_HTTP_CORRELATION_MISMATCH", retryable: true);
            var code = string.IsNullOrEmpty(error?.ErrorCode) ? "HTTP_REJECTED" : error.ErrorCode;
            return new SyncTransportException(code, retryable:
                code is "INTERNAL_ERROR" or "RATE_LIMITED" or "TIMEOUT" or "NO_RESPONSE" ||
                response.StatusCode == HttpStatusCode.RequestTimeout ||
                response.StatusCode == HttpStatusCode.TooManyRequests ||
                (int)response.StatusCode >= 500);
        }
        catch (JsonException)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return new SyncTransportException("HTTP_REJECTED", retryable:
                response.StatusCode == HttpStatusCode.RequestTimeout ||
                response.StatusCode == HttpStatusCode.TooManyRequests ||
                (int)response.StatusCode >= 500);
        }
    }

    private static Guid StableGuid(Guid conflictCaseId, string purpose)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes($"TransportERP|sync-v1|{purpose}|{conflictCaseId:D}"));
        Span<byte> value = bytes.AsSpan(0, 16);
        value[7] = (byte)((value[7] & 0x0f) | 0x40);
        value[8] = (byte)((value[8] & 0x3f) | 0x80);
        return new Guid(value);
    }

    private static void ValidateBearer(string bearer)
    {
        if (string.IsNullOrEmpty(bearer) || bearer.Any(character => character > 0x7f || char.IsWhiteSpace(character)))
            throw new SyncTransportException("SESSION_TOKEN_INVALID", retryable: false);
    }

    private static void ValidateReason(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason) || reason.Length > 500 || reason.Any(char.IsControl) ||
            ForbiddenReasonFragments.Any(fragment => reason.Contains(fragment, StringComparison.OrdinalIgnoreCase)))
            throw new OfflineStoreException(
                "CONFLICT_REASON_INVALID",
                "Conflict reasons must contain 1..500 safe text characters and no authentication material.");
    }
}
