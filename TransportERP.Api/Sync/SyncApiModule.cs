using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Net;
using Microsoft.AspNetCore.HttpOverrides;
using TransportERP.Api.Identity;
using TransportERP.Api.Security;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Sync;

public interface ISyncRuntimeGate
{
    Task<bool> IsOpenAsync(Guid companyId, CancellationToken cancellationToken);
}

/// <summary>Production registration has no configuration path and is always closed before G5.</summary>
public sealed class ClosedSyncRuntimeGate : ISyncRuntimeGate
{
    public Task<bool> IsOpenAsync(Guid companyId, CancellationToken cancellationToken)
        => Task.FromResult(false);
}

public sealed record SyncPopDeploymentProfile(
    bool IsValid,
    string? CanonicalHtu,
    string? PublicHost,
    int PublicPort,
    bool ForwardedHeadersEnabled,
    int ForwardLimit,
    IReadOnlyList<string> KnownProxies,
    IReadOnlyList<string> KnownNetworks,
    IReadOnlyList<string> AllowedHosts)
{
    public static SyncPopDeploymentProfile Load(IConfiguration configuration)
    {
        var origin = configuration["Sync:Proof:PublicOrigin"] ??
                     Environment.GetEnvironmentVariable("TRANSPORTERP_SYNC_PROOF_PUBLIC_ORIGIN");
        var pastSeconds = configuration.GetValue<int?>("Sync:Proof:MaximumPastSeconds");
        var futureSeconds = configuration.GetValue<int?>("Sync:Proof:MaximumFutureSeconds");
        var nonceSeconds = configuration.GetValue<int?>("Sync:Proof:NonceLifetimeSeconds");
        var replaySeconds = configuration.GetValue<int?>("Sync:Proof:ReplayRetentionSeconds");
        var bodyBytes = configuration.GetValue<int?>("Sync:Proof:MaximumRequestBodyBytes");
        var payloadBytes = configuration.GetValue<int?>("Sync:Proof:MaximumPayloadBytes");
        if (pastSeconds != 120 || futureSeconds != 30 || nonceSeconds != 300 || replaySeconds != 600 ||
            bodyBytes != 2_097_152 || payloadBytes != 16_384 ||
            !Uri.TryCreate(origin, UriKind.Absolute, out var uri) || uri.Scheme != Uri.UriSchemeHttps ||
            uri.AbsolutePath != "/" || !string.IsNullOrEmpty(uri.Query) || !string.IsNullOrEmpty(uri.Fragment) ||
            !string.IsNullOrEmpty(uri.UserInfo))
            return Invalid();
        var idn = new System.Globalization.IdnMapping();
        string host;
        try { host = idn.GetAscii(uri.DnsSafeHost).ToLowerInvariant(); }
        catch (ArgumentException) { return Invalid(); }
        var port = uri.IsDefaultPort || uri.Port == 443 ? 443 : uri.Port;
        var portText = port == 443 ? "" : $":{port}";
        var forwardedEnabled = configuration.GetValue("Sync:Proof:ForwardedHeadersEnabled", false);
        var configuredForwardLimit = configuration.GetValue<int?>("Sync:Proof:ForwardLimit");
        var forwardLimit = configuredForwardLimit ?? 1;
        var knownProxies = configuration.GetSection("Sync:Proof:KnownProxies").Get<string[]>() ?? [];
        var knownNetworks = configuration.GetSection("Sync:Proof:KnownNetworks").Get<string[]>() ?? [];
        var allowedHosts = (configuration["AllowedHosts"] ?? "").Split(';',
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (forwardedEnabled && (!configuredForwardLimit.HasValue || forwardLimit is < 1 or > 5 ||
            (knownProxies.Length == 0 && knownNetworks.Length == 0) ||
            allowedHosts.Length == 0 || allowedHosts.Any(x => x is "*" or "+") ||
            !allowedHosts.Contains(host, StringComparer.OrdinalIgnoreCase) ||
            knownProxies.Any(x => !IPAddress.TryParse(x, out _)) ||
            knownNetworks.Any(x => !TryParseNetwork(x, out _, out _))))
            return Invalid();
        return new(true, $"https://{host}{portText}/api/v1/sync/operations:batch", host, port,
            forwardedEnabled, forwardLimit, knownProxies, knownNetworks, allowedHosts);
    }

    public void ConfigureForwardedHeaders(ForwardedHeadersOptions options)
    {
        options.ForwardedHeaders = ForwardedHeaders.None;
        options.KnownProxies.Clear();
        options.KnownNetworks.Clear();
        options.AllowedHosts.Clear();
        if (!IsValid || !ForwardedHeadersEnabled) return;
        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                                   ForwardedHeaders.XForwardedProto |
                                   ForwardedHeaders.XForwardedHost;
        options.ForwardLimit = ForwardLimit;
        foreach (var value in KnownProxies) options.KnownProxies.Add(IPAddress.Parse(value));
        foreach (var value in KnownNetworks)
        {
            _ = TryParseNetwork(value, out var address, out var prefixLength);
            options.KnownNetworks.Add(new Microsoft.AspNetCore.HttpOverrides.IPNetwork(address!, prefixLength));
        }
        foreach (var value in AllowedHosts) options.AllowedHosts.Add(value);
    }

    private static bool TryParseNetwork(string value, out IPAddress? address, out int prefixLength)
    {
        address = null; prefixLength = 0;
        var parts = value.Split('/', 2);
        return parts.Length == 2 && IPAddress.TryParse(parts[0], out address) &&
               int.TryParse(parts[1], out prefixLength) && prefixLength >= 0 &&
               prefixLength <= (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork ? 32 : 128);
    }

    private static SyncPopDeploymentProfile Invalid()
        => new(false, null, null, 443, false, 1, [], [], []);
}

public static class SyncApiModule
{
    public const int MaximumRequestBodyBytes = 2_097_152;
    public const int MaximumPayloadBytes = 16_384;
    public const int MaximumBatchOperations = 100;
    private static readonly Regex CanonicalClientTime = new(
        "^[0-9]{4}-[0-9]{2}-[0-9]{2}T[0-9]{2}:[0-9]{2}:[0-9]{2}(?:\\.[0-9]{1,6})?Z$",
        RegexOptions.CultureInvariant | RegexOptions.NonBacktracking);

    public static IEndpointRouteBuilder MapTransportSync(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/v1/sync/operations:batch", HandleBatchAsync)
            .RequireAuthorization(SecurityPolicies.Permission("sync.operations.execute"));
        return endpoints;
    }

    private static async Task<IResult> HandleBatchAsync(
        HttpContext http,
        ICurrentSecurityContext currentSecurity,
        ISyncRuntimeGate runtimeGate,
        SyncPopProofValidator proofValidator,
        SyncProofRuntimeService proofRuntime,
        SyncOperationService sync,
        SyncPopDeploymentProfile deployment,
        CancellationToken cancellationToken)
    {
        var current = await currentSecurity.ResolveAsync(http.User, cancellationToken);
        if (current is null) return Results.Unauthorized();
        var attemptCorrelationId = ReadAttemptCorrelationId(http);
        if (attemptCorrelationId is null)
            return Results.BadRequest(new { ErrorCode = "ATTEMPT_CORRELATION_REQUIRED", CorrelationId = Guid.Empty });

        // This is the production invariant until G4 evidence and the separate G5 owner decision.
        // It intentionally precedes body reads, nonce issuance and jti persistence.
        if (!await runtimeGate.IsOpenAsync(current.CompanyId, cancellationToken))
            return Results.Json(new { ErrorCode = "OFFLINE_DISABLED", CorrelationId = attemptCorrelationId },
                statusCode: StatusCodes.Status403Forbidden);

        if (!TrySecurityContext(current, out var proofSecurity))
            return Results.Json(new { ErrorCode = "DEVICE_NOT_REGISTERED", CorrelationId = attemptCorrelationId },
                statusCode: StatusCodes.Status403Forbidden);

        var requestLevelError = ValidateRequestMetadata(http.Request);
        if (requestLevelError is not null) return RequestLevelError(requestLevelError, attemptCorrelationId.Value);
        byte[] rawBody;
        try { rawBody = await ReadBoundedBodyAsync(http.Request, cancellationToken); }
        catch (SyncRequestException exception) { return RequestLevelError(exception.Code, attemptCorrelationId.Value); }

        if (!deployment.IsValid || !RequestTopologyMatches(http.Request, deployment))
            return Results.Json(new { ErrorCode = "SYNC_POP_CONFIGURATION_INVALID", CorrelationId = attemptCorrelationId },
                statusCode: StatusCodes.Status503ServiceUnavailable);

        var proofHeaders = http.Request.Headers["DPoP"];
        if (proofHeaders.Count == 0 || (proofHeaders.Count == 1 && string.IsNullOrEmpty(proofHeaders[0])))
            return await NonceRequiredAsync(http, proofRuntime, proofSecurity, attemptCorrelationId.Value, cancellationToken);
        if (proofHeaders.Count != 1) return InvalidProof(attemptCorrelationId.Value);
        if (!TryReadBearer(http.Request, out var bearer))
            return Results.Unauthorized();

        VerifiedSyncProofMaterial verified;
        try
        {
            verified = proofValidator.Validate(new SyncPopProofValidationInput(
                proofHeaders[0]!, bearer, rawBody, deployment.CanonicalHtu!, attemptCorrelationId.Value,
                DateTimeOffset.UtcNow));
        }
        catch (SyncPopNonceRequiredException)
        {
            return await NonceRequiredAsync(http, proofRuntime, proofSecurity,
                attemptCorrelationId.Value, cancellationToken);
        }
        catch (SyncPopProofValidationException)
        {
            return InvalidProof(attemptCorrelationId.Value);
        }

        if (TryReadEnvelopeDeviceId(rawBody, out var envelopeDeviceId) &&
            !string.Equals(envelopeDeviceId, current.DeviceId, StringComparison.Ordinal))
            return Results.Json(new { ErrorCode = "DEVICE_NOT_REGISTERED", CorrelationId = attemptCorrelationId },
                statusCode: StatusCodes.Status403Forbidden);

        AcceptedSyncProofContext acceptedProof;
        try
        {
            acceptedProof = await proofRuntime.ClaimAsync(proofSecurity, verified, cancellationToken);
        }
        catch (SyncProofRuntimeException exception) when (exception.Code == "use_dpop_nonce")
        {
            return await NonceRequiredAsync(http, proofRuntime, proofSecurity, attemptCorrelationId.Value, cancellationToken);
        }
        catch (SyncProofRuntimeException exception) when (exception.Code == "invalid_dpop_proof")
        {
            return InvalidProof(attemptCorrelationId.Value);
        }
        catch (SyncProofRuntimeException exception) when (exception.Code == "DEVICE_PROOF_KEY_REQUIRED")
        {
            return Results.Json(new { ErrorCode = exception.Code, CorrelationId = attemptCorrelationId },
                statusCode: StatusCodes.Status403Forbidden);
        }
        catch (SyncProofRuntimeException)
        {
            return Results.Json(new { ErrorCode = "DEVICE_NOT_REGISTERED", CorrelationId = attemptCorrelationId },
                statusCode: StatusCodes.Status403Forbidden);
        }

        SyncBatchRequest? request;
        try { request = SyncBatchJsonContract.Deserialize(rawBody); }
        catch (JsonException) { return Results.BadRequest(new { ErrorCode = "REQUEST_SCHEMA_INVALID", CorrelationId = attemptCorrelationId }); }
        if (request is null || request.Operations is null || request.Operations.Count is < 1 or > MaximumBatchOperations ||
            !string.Equals(request.ProtocolVersion, "sync-v1", StringComparison.Ordinal) ||
            !string.Equals(request.DeviceId, current.DeviceId, StringComparison.Ordinal))
            return Results.BadRequest(new { ErrorCode = "REQUEST_SCHEMA_INVALID", CorrelationId = attemptCorrelationId });

        var results = new List<SyncBatchOperationResult>(request.Operations.Count);
        foreach (var item in request.Operations)
        {
            var serverTime = DateTimeOffset.UtcNow;
            var validationCode = ValidateOperation(item);
            if (validationCode is not null)
            {
                results.Add(SyncBatchOperationResult.Rejected(item, validationCode, serverTime));
                continue;
            }
            try
            {
                var validItem = item!;
                _ = TryParseClientOccurredAt(validItem.ClientOccurredAt, out var clientOccurredAt);
                var operation = await sync.EnqueueAcceptedSyncOperationAsync(
                    new EnqueueAcceptedSyncOperationCommand(
                        request.ProtocolVersion, validItem.ActionCode, validItem.OperationType, validItem.EntityType,
                        validItem.EntityId, validItem.ClientOperationId, validItem.PayloadJson, validItem.PayloadHash,
                        clientOccurredAt, validItem.OperationCorrelationId, validItem.BaseVersion),
                    acceptedProof, cancellationToken);
                results.Add(SyncBatchOperationResult.From(operation, serverTime));
            }
            catch (SyncRuleException exception)
            {
                results.Add(SyncBatchOperationResult.Rejected(item, exception.Code, serverTime));
            }
            catch
            {
                results.Add(SyncBatchOperationResult.Rejected(item, "INTERNAL_ERROR", serverTime, "FAILED"));
            }
        }

        return Results.Ok(new SyncBatchResponse(
            request.ProtocolVersion, results, DateTimeOffset.UtcNow, attemptCorrelationId.Value));
    }

    private static string? ValidateOperation(SyncBatchOperationRequest? item)
    {
        if (item is null || item.OperationCorrelationId == Guid.Empty ||
            string.IsNullOrEmpty(item.ActionCode) || string.IsNullOrEmpty(item.OperationType) ||
            string.IsNullOrEmpty(item.EntityType) || string.IsNullOrEmpty(item.ClientOperationId) ||
            string.IsNullOrEmpty(item.PayloadJson) || string.IsNullOrEmpty(item.PayloadHash) ||
            !TryParseClientOccurredAt(item.ClientOccurredAt, out _))
            return "PAYLOAD_INVALID";
        var payloadBytes = Encoding.UTF8.GetByteCount(item.PayloadJson);
        if (payloadBytes > MaximumPayloadBytes) return "PAYLOAD_TOO_LARGE";
        try
        {
            using var payload = JsonDocument.Parse(item.PayloadJson, new JsonDocumentOptions
            {
                AllowTrailingCommas = false, CommentHandling = JsonCommentHandling.Disallow, MaxDepth = 32
            });
        }
        catch (JsonException) { return "PAYLOAD_INVALID"; }
        return SyncActionContract.Validate(item);
    }

    private static string? ValidateRequestMetadata(HttpRequest request)
    {
        if (request.ContentLength > MaximumRequestBodyBytes) return "REQUEST_BODY_TOO_LARGE";
        if (request.ContentType is null ||
            !request.ContentType.Split(';', 2)[0].Trim().Equals("application/json", StringComparison.OrdinalIgnoreCase))
            return "CONTENT_TYPE_UNSUPPORTED";
        var encoding = request.Headers["Content-Encoding"];
        if (encoding.Count > 1 || (encoding.Count == 1 &&
            !string.Equals(encoding[0], "identity", StringComparison.OrdinalIgnoreCase)))
            return "CONTENT_ENCODING_UNSUPPORTED";
        return null;
    }

    private static async Task<byte[]> ReadBoundedBodyAsync(HttpRequest request, CancellationToken cancellationToken)
    {
        await using var output = new MemoryStream();
        var buffer = new byte[16 * 1024];
        while (true)
        {
            var read = await request.Body.ReadAsync(buffer.AsMemory(), cancellationToken);
            if (read == 0) break;
            if (output.Length + read > MaximumRequestBodyBytes)
                throw new SyncRequestException("REQUEST_BODY_TOO_LARGE");
            await output.WriteAsync(buffer.AsMemory(0, read), cancellationToken);
        }
        return output.ToArray();
    }

    private static bool TryReadBearer(HttpRequest request, out string bearer)
    {
        bearer = string.Empty;
        var values = request.Headers["Authorization"];
        if (values.Count != 1 || values[0] is null || !values[0]!.StartsWith("Bearer ", StringComparison.Ordinal))
            return false;
        bearer = values[0]!["Bearer ".Length..];
        return bearer.Length > 0 && !bearer.Any(char.IsWhiteSpace);
    }

    private static bool TryParseClientOccurredAt(string? value, out DateTimeOffset timestamp)
    {
        timestamp = default;
        return value is not null && CanonicalClientTime.IsMatch(value) &&
               DateTimeOffset.TryParseExact(value,
                   ["yyyy-MM-dd'T'HH:mm:ss'Z'", "yyyy-MM-dd'T'HH:mm:ss.FFFFFF'Z'"],
                   CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                   out timestamp);
    }

    private static Guid? ReadAttemptCorrelationId(HttpContext context)
    {
        var values = context.Request.Headers["X-Correlation-Id"];
        return values.Count == 1 && Guid.TryParseExact(values[0], "D", out var value) && value != Guid.Empty
            ? value : null;
    }

    private static bool TrySecurityContext(CurrentSecurityContext current, out SyncProofSecurityContext security)
    {
        security = null!;
        if (!current.IsLocalSession || !current.BranchId.HasValue || !current.RegisteredDeviceId.HasValue ||
            string.IsNullOrEmpty(current.DeviceId)) return false;
        security = new SyncProofSecurityContext(current.UserId, current.CompanyId, current.BranchId.Value,
            current.RegisteredDeviceId.Value, current.DeviceId);
        return true;
    }

    private static bool TryReadEnvelopeDeviceId(byte[] rawBody, out string? deviceId)
        => SyncBatchJsonContract.TryReadDeviceId(rawBody, out deviceId);

    private static bool RequestTopologyMatches(HttpRequest request, SyncPopDeploymentProfile deployment)
    {
        if (!string.Equals(request.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase)) return false;
        string host;
        try { host = new System.Globalization.IdnMapping().GetAscii(request.Host.Host).ToLowerInvariant(); }
        catch (ArgumentException) { return false; }
        return string.Equals(host, deployment.PublicHost, StringComparison.Ordinal) &&
               (request.Host.Port ?? 443) == deployment.PublicPort;
    }

    private static async Task<IResult> NonceRequiredAsync(HttpContext http,
        SyncProofRuntimeService runtime, SyncProofSecurityContext security, Guid correlationId, CancellationToken ct)
    {
        try
        {
            var nonce = await runtime.IssueNonceAsync(security, ct);
            http.Response.Headers.WWWAuthenticate = "DPoP error=\"use_dpop_nonce\"";
            http.Response.Headers["DPoP-Nonce"] = nonce.Value;
            http.Response.Headers.CacheControl = "no-store";
            return Results.Json(new { ErrorCode = "use_dpop_nonce", CorrelationId = correlationId },
                statusCode: StatusCodes.Status401Unauthorized);
        }
        catch (SyncProofRuntimeException exception) when (exception.Code == "DEVICE_PROOF_KEY_REQUIRED")
        {
            return Results.Json(new { ErrorCode = exception.Code, CorrelationId = correlationId },
                statusCode: StatusCodes.Status403Forbidden);
        }
        catch (SyncProofRuntimeException exception) when (exception.Code == "NONCE_GENERATION_FAILED")
        {
            return Results.Json(new { ErrorCode = "INTERNAL_ERROR", CorrelationId = correlationId },
                statusCode: StatusCodes.Status500InternalServerError);
        }
        catch (SyncProofRuntimeException)
        {
            return Results.Json(new { ErrorCode = "DEVICE_NOT_REGISTERED", CorrelationId = correlationId },
                statusCode: StatusCodes.Status403Forbidden);
        }
    }

    private static IResult InvalidProof(Guid correlationId)
        => Results.Json(new { ErrorCode = "invalid_dpop_proof", CorrelationId = correlationId },
            statusCode: StatusCodes.Status401Unauthorized);

    private static IResult RequestLevelError(string code, Guid correlationId) => code switch
    {
        "REQUEST_BODY_TOO_LARGE" => Results.Json(new { ErrorCode = code, CorrelationId = correlationId },
            statusCode: StatusCodes.Status413PayloadTooLarge),
        "CONTENT_ENCODING_UNSUPPORTED" or "CONTENT_TYPE_UNSUPPORTED" => Results.Json(
            new { ErrorCode = code, CorrelationId = correlationId }, statusCode: StatusCodes.Status415UnsupportedMediaType),
        _ => Results.BadRequest(new { ErrorCode = code, CorrelationId = correlationId })
    };

    private sealed class SyncRequestException(string code) : InvalidOperationException(code)
    {
        public string Code { get; } = code;
    }
}

/// <summary>
/// The single wire codec for the sync-v1 envelope. It intentionally uses the
/// ASP.NET Web JSON naming convention while retaining exact, case-sensitive
/// property matching and rejecting unmapped properties.
/// </summary>
public static class SyncBatchJsonContract
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = false,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
        MaxDepth = 64
    };

    public static SyncBatchRequest? Deserialize(byte[] utf8Json)
        => JsonSerializer.Deserialize<SyncBatchRequest>(utf8Json, Options);

    public static bool TryReadDeviceId(byte[] utf8Json, out string? deviceId)
    {
        deviceId = null;
        try
        {
            using var document = JsonDocument.Parse(utf8Json, new JsonDocumentOptions
            {
                AllowTrailingCommas = false,
                CommentHandling = JsonCommentHandling.Disallow,
                MaxDepth = 64
            });
            if (document.RootElement.ValueKind != JsonValueKind.Object ||
                !document.RootElement.TryGetProperty("deviceId", out var property) ||
                property.ValueKind != JsonValueKind.String) return false;
            deviceId = property.GetString();
            return true;
        }
        catch (JsonException) { return false; }
    }
}

public sealed record SyncBatchRequest(
    string DeviceId,
    string ProtocolVersion,
    IReadOnlyList<SyncBatchOperationRequest?> Operations);

public sealed record SyncBatchOperationRequest(
    string ActionCode,
    string OperationType,
    string EntityType,
    Guid? EntityId,
    string ClientOperationId,
    string PayloadJson,
    string PayloadHash,
    string ClientOccurredAt,
    Guid OperationCorrelationId,
    long? BaseVersion = null);

public sealed record SyncBatchOperationResult(
    string? ClientOperationId,
    Guid? OperationCorrelationId,
    Guid? ServerOperationId,
    string? ActionCode,
    Guid? ResultEntityId,
    string Status,
    long? ResultVersion,
    string? ErrorCode,
    Guid? ConflictCaseId,
    DateTimeOffset ServerTime)
{
    public static SyncBatchOperationResult From(SyncOperation operation, DateTimeOffset serverTime)
        => new(operation.ClientOperationId, operation.OperationCorrelationId, operation.Id, operation.ActionCode,
            operation.ResultEntityId, operation.Status, operation.ResultVersion, operation.ErrorCode,
            operation.ConflictCase?.Id, serverTime);

    public static SyncBatchOperationResult Rejected(SyncBatchOperationRequest? item, string code,
        DateTimeOffset serverTime, string status = "REJECTED")
        => new(item?.ClientOperationId, item?.OperationCorrelationId, null, item?.ActionCode, null,
            status, null, code, null, serverTime);
}

public sealed record SyncBatchResponse(
    string ProtocolVersion,
    IReadOnlyList<SyncBatchOperationResult> Results,
    DateTimeOffset ServerTime,
    Guid AttemptCorrelationId);

internal static class SyncActionContract
{
    private sealed record Rule(string OperationType, string EntityType, bool EntityRequired, bool BaseVersionRequired);

    private static readonly IReadOnlyDictionary<string, Rule> Rules = new Dictionary<string, Rule>(StringComparer.Ordinal)
    {
        ["CreateJournalEntry"] = new("CREATE", "JournalEntry", false, false),
        ["CreateReceiptVoucher"] = new("CREATE", "ReceiptVoucher", false, false),
        ["CreatePaymentVoucher"] = new("CREATE", "PaymentVoucher", false, false),
        ["CreateWaybillDraft"] = new("CREATE", "Waybill", false, false),
        ["UpdateWaybillDraft"] = new("UPDATE", "Waybill", true, true),
        ["CreateOperationalParty"] = new("CREATE", "OperationalParty", false, false),
        ["AddWaybillAttachment"] = new("CREATE", "Waybill", true, false),
        ["RecordCollection"] = new("COMMAND", "Waybill", true, false),
        ["LoadAllocatedQuantity"] = new("COMMAND", "ManifestLine", true, false),
        ["RecordArrival"] = new("COMMAND", "Trip", true, false),
        ["RecordUnload"] = new("COMMAND", "ArrivalReceipt", true, false),
        ["DeliverQuantity"] = new("COMMAND", "Waybill", true, false),
        ["RecordProofOfDelivery"] = new("CREATE", "Delivery", true, false),
        ["CreateShipmentException"] = new("COMMAND", "Waybill", true, false)
    };

    public static string Validate(SyncBatchOperationRequest operation)
    {
        if (operation.OperationType == "DELETE" || !Rules.TryGetValue(operation.ActionCode, out var rule))
            return "ONLINE_REQUIRED";
        if (operation.OperationType != rule.OperationType || operation.EntityType != rule.EntityType ||
            (rule.EntityRequired && !operation.EntityId.HasValue) ||
            (rule.BaseVersionRequired && !operation.BaseVersion.HasValue) ||
            (!rule.BaseVersionRequired && operation.BaseVersion.HasValue))
            return "ACTION_CONTRACT_MISMATCH";
        // No baseline action has an approved offline dispatcher yet. Keeping this explicit prevents
        // an allowlisted name from becoming executable merely because the transport Runtime exists.
        return "ACTION_RUNTIME_UNAVAILABLE";
    }
}
