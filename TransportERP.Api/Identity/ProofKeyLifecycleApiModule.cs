using System.Text.Json;
using System.Text.Json.Serialization;
using TransportERP.Api.Security;
using TransportERP.Api.Sync;
using TransportERP.Contracts.Identity;

namespace TransportERP.Api.Identity;

public static class ProofKeyLifecycleApiModule
{
    public const int MaximumChangeRequestBodyBytes = 16_384;
    private static readonly JsonSerializerOptions ChangeJson = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = false,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
        MaxDepth = 16
    };
    private static readonly HashSet<string> ChangeMembers = new(
        ["challengeId", "changeRequestId", "changeType", "expectedProofKeyVersion", "newPublicJwk", "reason"],
        StringComparer.Ordinal);
    private static readonly HashSet<string> RequiredChangeMembers = new(
        ["challengeId", "changeRequestId", "changeType", "expectedProofKeyVersion", "newPublicJwk"],
        StringComparer.Ordinal);

    public static IEndpointRouteBuilder MapRegisteredDeviceProofKeys(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/devices/{deviceId:guid}/proof-key-challenges", CreateChallengeAsync)
            .RequireAuthorization(SecurityPolicies.Permission(RegisteredDevicePermissionCodes.Manage));
        app.MapPost("/api/v1/devices/{deviceId:guid}:bind-proof-key",
                (Guid deviceId, HttpContext http, ICurrentSecurityContext security,
                    ProofKeyLifecycleService lifecycle, SyncPopDeploymentProfile deployment, CancellationToken ct) =>
                    ChangeAsync(deviceId, "BIND", http, security, lifecycle, deployment, ct))
            .RequireAuthorization(SecurityPolicies.Permission(RegisteredDevicePermissionCodes.Manage));
        app.MapPost("/api/v1/devices/{deviceId:guid}:rotate-proof-key",
                (Guid deviceId, HttpContext http, ICurrentSecurityContext security,
                    ProofKeyLifecycleService lifecycle, SyncPopDeploymentProfile deployment, CancellationToken ct) =>
                    ChangeAsync(deviceId, "ROTATE", http, security, lifecycle, deployment, ct))
            .RequireAuthorization(SecurityPolicies.Permission(RegisteredDevicePermissionCodes.Manage));
        app.MapPost("/api/v1/devices/{deviceId:guid}:recover-proof-key",
                (Guid deviceId, HttpContext http, ICurrentSecurityContext security,
                    ProofKeyLifecycleService lifecycle, SyncPopDeploymentProfile deployment, CancellationToken ct) =>
                    ChangeAsync(deviceId, "RECOVER", http, security, lifecycle, deployment, ct))
            .RequireAuthorization(SecurityPolicies.Permission(RegisteredDevicePermissionCodes.Manage));
        return app;
    }

    private static async Task<IResult> CreateChallengeAsync(
        Guid deviceId,
        CreateProofKeyChallengeRequest request,
        HttpContext http,
        ICurrentSecurityContext security,
        ProofKeyLifecycleService lifecycle,
        CancellationToken ct)
    {
        var current = await security.ResolveAsync(http.User, ct);
        if (current is null) return Results.Unauthorized();
        var correlationId = CorrelationId(http);
        try
        {
            return Results.Ok(await lifecycle.CreateChallengeAsync(
                deviceId, current, request, correlationId, ct));
        }
        catch (ProofKeyLifecycleException exception)
        {
            return Error(exception.Code, correlationId);
        }
    }

    private static async Task<IResult> ChangeAsync(
        Guid deviceId,
        string changeType,
        HttpContext http,
        ICurrentSecurityContext security,
        ProofKeyLifecycleService lifecycle,
        SyncPopDeploymentProfile deployment,
        CancellationToken ct)
    {
        var current = await security.ResolveAsync(http.User, ct);
        if (current is null) return Results.Unauthorized();
        var correlationId = CorrelationId(http);
        if (!deployment.IsValid || deployment.CanonicalHtu is null || !RequestTopologyMatches(http.Request, deployment))
            return Results.Json(new { ErrorCode = "PROOF_KEY_CONFIGURATION_INVALID", CorrelationId = correlationId },
                statusCode: StatusCodes.Status503ServiceUnavailable);
        var metadataError = ValidateChangeRequestMetadata(http.Request);
        if (metadataError is not null) return Error(metadataError, correlationId);

        byte[] rawBody;
        ChangeProofKeyRequest request;
        try
        {
            rawBody = await ReadBoundedBodyAsync(http.Request, ct);
            ValidateChangeJson(rawBody);
            request = JsonSerializer.Deserialize<ChangeProofKeyRequest>(rawBody, ChangeJson)
                ?? throw new ProofKeyLifecycleException("REQUEST_SCHEMA_INVALID");
        }
        catch (ProofKeyLifecycleException exception)
        {
            return Error(exception.Code, correlationId);
        }
        catch (JsonException)
        {
            return Error("REQUEST_SCHEMA_INVALID", correlationId);
        }

        if (!TryReadSingleHeader(http.Request, "Device-Key-Proof-New", out var newProof) ||
            !TryReadOptionalSingleHeader(http.Request, "Device-Key-Proof-Current", out var currentProof) ||
            !TryReadBearer(http.Request, out var bearer))
            return Error("PROOF_KEY_PROOF_INVALID", correlationId);

        var origin = new Uri(deployment.CanonicalHtu).GetLeftPart(UriPartial.Authority);
        var canonicalHtu = $"{origin}/api/v1/devices/{deviceId:D}:{changeType.ToLowerInvariant()}-proof-key";
        try
        {
            return Results.Ok(await lifecycle.ChangeAsync(deviceId, changeType, current, request,
                currentProof, newProof!, bearer!, rawBody, canonicalHtu, correlationId, ct));
        }
        catch (ProofKeyLifecycleException exception)
        {
            return Error(exception.Code, correlationId);
        }
    }

    private static async Task<byte[]> ReadBoundedBodyAsync(HttpRequest request, CancellationToken ct)
    {
        if (request.ContentLength is > MaximumChangeRequestBodyBytes)
            throw new ProofKeyLifecycleException("REQUEST_BODY_TOO_LARGE");
        using var buffer = new MemoryStream();
        var chunk = new byte[4096];
        while (true)
        {
            var count = await request.Body.ReadAsync(chunk, ct);
            if (count == 0) break;
            if (buffer.Length + count > MaximumChangeRequestBodyBytes)
                throw new ProofKeyLifecycleException("REQUEST_BODY_TOO_LARGE");
            buffer.Write(chunk, 0, count);
        }
        if (buffer.Length == 0) throw new ProofKeyLifecycleException("REQUEST_SCHEMA_INVALID");
        return buffer.ToArray();
    }

    public static string? ValidateChangeRequestMetadata(HttpRequest request)
    {
        var contentTypes = request.Headers["Content-Type"];
        if (contentTypes.Count != 1 || string.IsNullOrWhiteSpace(contentTypes[0]))
            return "UNSUPPORTED_MEDIA_TYPE";
        var rawContentType = contentTypes[0]!;
        if (rawContentType.Contains(',')) return "UNSUPPORTED_MEDIA_TYPE";
        var parameterStart = rawContentType.IndexOf(';');
        var mediaType = (parameterStart < 0 ? rawContentType : rawContentType[..parameterStart]).Trim();
        if (!string.Equals(mediaType, "application/json", StringComparison.OrdinalIgnoreCase))
            return "UNSUPPORTED_MEDIA_TYPE";

        if (!request.Headers.TryGetValue("Content-Encoding", out var encodings)) return null;
        if (encodings.Count != 1 || string.IsNullOrWhiteSpace(encodings[0]) || encodings[0]!.Contains(',') ||
            !string.Equals(encodings[0]!.Trim(), "identity", StringComparison.OrdinalIgnoreCase))
            return "UNSUPPORTED_CONTENT_ENCODING";
        return null;
    }

    private static void ValidateChangeJson(byte[] rawBody)
    {
        using var document = JsonDocument.Parse(rawBody, new JsonDocumentOptions
        {
            AllowTrailingCommas = false,
            CommentHandling = JsonCommentHandling.Disallow,
            MaxDepth = 16
        });
        if (document.RootElement.ValueKind != JsonValueKind.Object)
            throw new ProofKeyLifecycleException("REQUEST_SCHEMA_INVALID");
        var names = new HashSet<string>(StringComparer.Ordinal);
        foreach (var property in document.RootElement.EnumerateObject())
            if (!names.Add(property.Name) || !ChangeMembers.Contains(property.Name))
                throw new ProofKeyLifecycleException("REQUEST_SCHEMA_INVALID");
        if (!RequiredChangeMembers.IsSubsetOf(names))
            throw new ProofKeyLifecycleException("REQUEST_SCHEMA_INVALID");
    }

    private static bool TryReadSingleHeader(HttpRequest request, string name, out string? value)
    {
        var values = request.Headers[name];
        value = values.Count == 1 ? values[0] : null;
        return value is not null && value.Length > 0;
    }

    private static bool TryReadOptionalSingleHeader(HttpRequest request, string name, out string? value)
    {
        if (!request.Headers.TryGetValue(name, out var values))
        {
            value = null;
            return true;
        }
        value = values.Count == 1 ? values[0] : null;
        return value is not null && value.Length > 0;
    }

    private static bool TryReadBearer(HttpRequest request, out string? bearer)
    {
        bearer = null;
        var values = request.Headers.Authorization;
        if (values.Count != 1 || values[0] is null || !values[0]!.StartsWith("Bearer ", StringComparison.Ordinal))
            return false;
        var token = values[0]![7..];
        if (token.Length == 0 || token.Any(c => c > 0x7f || char.IsWhiteSpace(c))) return false;
        bearer = token;
        return true;
    }

    private static bool RequestTopologyMatches(HttpRequest request, SyncPopDeploymentProfile deployment)
    {
        if (!request.IsHttps || deployment.PublicHost is null ||
            !string.Equals(request.Host.Host, deployment.PublicHost, StringComparison.OrdinalIgnoreCase)) return false;
        var requestPort = request.Host.Port ?? 443;
        return requestPort == deployment.PublicPort;
    }

    private static IResult Error(string code, Guid correlationId) => code switch
    {
        "DEVICE_NOT_FOUND" => Results.NotFound(new { ErrorCode = code, CorrelationId = correlationId }),
        "LOCAL_SESSION_REQUIRED" => Results.Json(new { ErrorCode = code, CorrelationId = correlationId },
            statusCode: StatusCodes.Status403Forbidden),
        "DEVICE_REVOKED" or "DEVICE_PROOF_KEY_STATE_INVALID" or "PROOF_KEY_CHALLENGE_MISMATCH" or
            "PROOF_KEY_CHANGE_MISMATCH" or "DEVICE_PROOF_KEY_CONFLICT" or "PROOF_KEY_CHALLENGE_CONFLICT" or
            "PROOF_KEY_CHANGE_CONFLICT" => Results.Json(new { ErrorCode = code, CorrelationId = correlationId },
                statusCode: StatusCodes.Status409Conflict),
        "REQUEST_BODY_TOO_LARGE" => Results.Json(new { ErrorCode = code, CorrelationId = correlationId },
            statusCode: StatusCodes.Status413PayloadTooLarge),
        "UNSUPPORTED_MEDIA_TYPE" or "UNSUPPORTED_CONTENT_ENCODING" => Results.Json(
            new { ErrorCode = code, CorrelationId = correlationId },
            statusCode: StatusCodes.Status415UnsupportedMediaType),
        _ => Results.BadRequest(new { ErrorCode = code, CorrelationId = correlationId })
    };

    private static Guid CorrelationId(HttpContext context)
        => Guid.TryParse(context.Request.Headers["X-Correlation-Id"].FirstOrDefault(), out var id)
            ? id
            : Guid.NewGuid();
}
