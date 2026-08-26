using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Net;
using Microsoft.AspNetCore.HttpOverrides;
using TransportERP.Api.Identity;
using TransportERP.Api.Security;
using TransportERP.Application.Sync;
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
    public string? CanonicalHtuForPath(string absolutePath)
    {
        if (!IsValid || PublicHost is null || string.IsNullOrEmpty(absolutePath) ||
            absolutePath[0] != '/' || absolutePath.Contains('?') || absolutePath.Contains('#'))
            return null;
        var portText = PublicPort == 443 ? string.Empty : $":{PublicPort}";
        return $"https://{PublicHost}{portText}{absolutePath}";
    }

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

    public static async Task<IResult> HandleBatchAsync(
        HttpContext http,
        ISyncPopHttpRequestAuthenticator authenticator,
        SyncOperationService sync,
        IEffectivePermissionResolver permissions,
        CancellationToken cancellationToken)
    {
        var authentication = await authenticator.AuthenticateAsync(
            http, "/api/v1/sync/operations:batch", TryReadEnvelopeDeviceId, cancellationToken);
        if (authentication.Failure is not null) return authentication.Failure;
        var acceptedRequest = authentication.Accepted!;
        var attemptCorrelationId = acceptedRequest.AttemptCorrelationId;
        var proofSecurity = acceptedRequest.Security;
        var acceptedProof = acceptedRequest.Proof;
        var rawBody = acceptedRequest.RawBody;

        SyncBatchRequest? request;
        try { request = SyncBatchJsonContract.Deserialize(rawBody); }
        catch (JsonException) { return Results.BadRequest(new { ErrorCode = "REQUEST_SCHEMA_INVALID", CorrelationId = attemptCorrelationId }); }
        var envelopeError = SyncBatchEnvelopeContract.Validate(request, proofSecurity.DeviceId, MaximumBatchOperations);
        if (envelopeError is not null)
            return Results.BadRequest(new { ErrorCode = envelopeError, CorrelationId = attemptCorrelationId });

        var validRequest = request!;
        var operations = validRequest.Operations!;
        var results = new List<SyncBatchOperationResult>(operations.Count);
        foreach (var item in operations)
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
                var definition = SyncActionCatalog.Definitions.Single(x =>
                    string.Equals(x.ActionCodeValue, validItem.ActionCode, StringComparison.Ordinal));
                if (!await permissions.HasPermissionAsync(
                        acceptedProof.UserId, acceptedProof.CompanyId, acceptedProof.BranchId,
                        definition.RequiredPermission, cancellationToken))
                {
                    // Permission is checked before persistence so a denied item creates neither a
                    // SyncOperation row nor a queued audit event. The response does not disclose
                    // whether any target entity exists.
                    results.Add(SyncBatchOperationResult.Rejected(item, "SCOPE_DENIED", serverTime));
                    continue;
                }
                if (definition.RuntimeAvailability != SyncActionRuntimeAvailability.Available)
                {
                    results.Add(SyncBatchOperationResult.Rejected(
                        item, "ACTION_RUNTIME_UNAVAILABLE", serverTime));
                    continue;
                }
                _ = TryParseClientOccurredAt(validItem.ClientOccurredAt, out var clientOccurredAt);
                var operation = await sync.EnqueueAcceptedSyncOperationAsync(
                    new EnqueueAcceptedSyncOperationCommand(
                        validRequest.ProtocolVersion, validItem.ActionCode, validItem.OperationType, validItem.EntityType,
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
            validRequest.ProtocolVersion, results, DateTimeOffset.UtcNow, attemptCorrelationId));
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
        return SyncActionCatalog.ValidateShape(item.ActionCode, item.OperationType, item.EntityType,
            item.EntityId, item.BaseVersion).ErrorCode;
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

    private static bool TryReadEnvelopeDeviceId(byte[] rawBody, out string? deviceId)
        => SyncBatchJsonContract.TryReadDeviceId(rawBody, out deviceId);
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

public static class SyncBatchEnvelopeContract
{
    public static string? Validate(SyncBatchRequest? request, string currentDeviceId, int maximumBatchOperations)
    {
        if (request is null || request.Operations is null ||
            string.IsNullOrEmpty(request.DeviceId) ||
            !string.Equals(request.DeviceId, currentDeviceId, StringComparison.Ordinal))
            return "REQUEST_SCHEMA_INVALID";
        if (request.Operations.Count is < 1 || request.Operations.Count > maximumBatchOperations)
            return "BATCH_SIZE_INVALID";
        if (!string.Equals(request.ProtocolVersion, "sync-v1", StringComparison.Ordinal))
            return "PROTOCOL_VERSION_UNSUPPORTED";
        return null;
    }
}
