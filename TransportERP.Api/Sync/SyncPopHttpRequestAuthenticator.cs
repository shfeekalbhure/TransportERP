using TransportERP.Api.Security;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Sync;

public delegate bool TryReadSyncRequestDeviceId(byte[] rawBody, out string? deviceId);

public sealed record AcceptedSyncHttpRequest(
    CurrentSecurityContext Current,
    SyncProofSecurityContext Security,
    AcceptedSyncProofContext Proof,
    byte[] RawBody,
    Guid AttemptCorrelationId,
    EffectiveSyncPolicy? EffectivePolicy = null);

public sealed record SyncHttpAuthenticationResult(AcceptedSyncHttpRequest? Accepted, IResult? Failure);

public interface ISyncPopHttpRequestAuthenticator
{
    Task<SyncHttpAuthenticationResult> AuthenticateAsync(
        HttpContext http,
        string canonicalPath,
        TryReadSyncRequestDeviceId? tryReadBodyDeviceId,
        CancellationToken cancellationToken);
}

/// <summary>
/// The single HTTP boundary for sync-v1 bearer, raw-body and DPoP/nonce validation.
/// The runtime gate deliberately runs before body reads, nonce issuance or replay persistence.
/// </summary>
public sealed class SyncPopHttpRequestAuthenticator(
    ICurrentSecurityContext currentSecurity,
    ISyncRuntimeGate runtimeGate,
    SyncPopProofValidator proofValidator,
    ISyncProofRuntime proofRuntime,
    SyncPopDeploymentProfile deployment) : ISyncPopHttpRequestAuthenticator
{
    public async Task<SyncHttpAuthenticationResult> AuthenticateAsync(
        HttpContext http,
        string canonicalPath,
        TryReadSyncRequestDeviceId? tryReadBodyDeviceId,
        CancellationToken cancellationToken)
    {
        var current = await currentSecurity.ResolveAsync(http.User, cancellationToken);
        if (current is null) return Failed(Results.Unauthorized());
        var correlationId = ReadAttemptCorrelationId(http);
        if (correlationId is null)
            return Failed(Results.BadRequest(new
                { ErrorCode = "ATTEMPT_CORRELATION_REQUIRED", CorrelationId = Guid.Empty }));

        var effectivePolicy = await runtimeGate.ResolveAsync(current, cancellationToken);
        if (!effectivePolicy.Enabled)
            return Failed(Error(StatusCodes.Status403Forbidden, "OFFLINE_DISABLED", correlationId.Value));
        if (!TrySecurityContext(current, out var security))
            return Failed(Error(StatusCodes.Status403Forbidden, "DEVICE_NOT_REGISTERED", correlationId.Value));

        var metadataError = ValidateRequestMetadata(http.Request, effectivePolicy.MaximumRequestBodyBytes);
        if (metadataError is not null) return Failed(RequestLevelError(metadataError, correlationId.Value));
        byte[] rawBody;
        try
        {
            rawBody = await ReadBoundedBodyAsync(
                http.Request, effectivePolicy.MaximumRequestBodyBytes, cancellationToken);
        }
        catch (SyncRequestException exception)
        {
            return Failed(RequestLevelError(exception.Code, correlationId.Value));
        }

        var canonicalHtu = deployment.CanonicalHtuForPath(canonicalPath);
        if (canonicalHtu is null || !RequestTopologyMatches(http.Request, deployment, canonicalPath))
            return Failed(Error(StatusCodes.Status503ServiceUnavailable,
                "SYNC_POP_CONFIGURATION_INVALID", correlationId.Value));

        var proofHeaders = http.Request.Headers["DPoP"];
        if (proofHeaders.Count == 0 || (proofHeaders.Count == 1 && string.IsNullOrEmpty(proofHeaders[0])))
            return Failed(await NonceRequiredAsync(http, security, correlationId.Value, cancellationToken));
        if (proofHeaders.Count != 1) return Failed(InvalidProof(correlationId.Value));
        if (!TryReadBearer(http.Request, out var bearer)) return Failed(Results.Unauthorized());

        VerifiedSyncProofMaterial verified;
        try
        {
            verified = proofValidator.Validate(new SyncPopProofValidationInput(
                proofHeaders[0]!, bearer, rawBody, canonicalHtu, correlationId.Value, DateTimeOffset.UtcNow));
        }
        catch (SyncPopNonceRequiredException)
        {
            return Failed(await NonceRequiredAsync(http, security, correlationId.Value, cancellationToken));
        }
        catch (SyncPopProofValidationException)
        {
            return Failed(InvalidProof(correlationId.Value));
        }

        if (tryReadBodyDeviceId is not null && tryReadBodyDeviceId(rawBody, out var bodyDeviceId) &&
            !string.Equals(bodyDeviceId, current.DeviceId, StringComparison.Ordinal))
            return Failed(Error(StatusCodes.Status403Forbidden, "DEVICE_NOT_REGISTERED", correlationId.Value));

        try
        {
            var proof = await proofRuntime.ClaimAsync(security, verified, cancellationToken);
            return new SyncHttpAuthenticationResult(
                new AcceptedSyncHttpRequest(
                    current, security, proof, rawBody, correlationId.Value, effectivePolicy), null);
        }
        catch (SyncProofRuntimeException exception) when (exception.Code == "use_dpop_nonce")
        {
            return Failed(await NonceRequiredAsync(http, security, correlationId.Value, cancellationToken));
        }
        catch (SyncProofRuntimeException exception) when (exception.Code == "invalid_dpop_proof")
        {
            return Failed(InvalidProof(correlationId.Value));
        }
        catch (SyncProofRuntimeException exception) when (exception.Code == "DEVICE_PROOF_KEY_REQUIRED")
        {
            return Failed(Error(StatusCodes.Status403Forbidden, exception.Code, correlationId.Value));
        }
        catch (SyncProofRuntimeException)
        {
            return Failed(Error(StatusCodes.Status403Forbidden, "DEVICE_NOT_REGISTERED", correlationId.Value));
        }
    }

    private async Task<IResult> NonceRequiredAsync(HttpContext http, SyncProofSecurityContext security,
        Guid correlationId, CancellationToken cancellationToken)
    {
        try
        {
            var nonce = await proofRuntime.IssueNonceAsync(security, cancellationToken);
            http.Response.Headers.WWWAuthenticate = "DPoP error=\"use_dpop_nonce\"";
            http.Response.Headers["DPoP-Nonce"] = nonce.Value;
            http.Response.Headers.CacheControl = "no-store";
            return Error(StatusCodes.Status401Unauthorized, "use_dpop_nonce", correlationId);
        }
        catch (SyncProofRuntimeException exception) when (exception.Code == "DEVICE_PROOF_KEY_REQUIRED")
        {
            return Error(StatusCodes.Status403Forbidden, exception.Code, correlationId);
        }
        catch (SyncProofRuntimeException exception) when (exception.Code == "NONCE_GENERATION_FAILED")
        {
            return Error(StatusCodes.Status500InternalServerError, "INTERNAL_ERROR", correlationId);
        }
        catch (SyncProofRuntimeException)
        {
            return Error(StatusCodes.Status403Forbidden, "DEVICE_NOT_REGISTERED", correlationId);
        }
    }

    private static string? ValidateRequestMetadata(HttpRequest request, int maximumRequestBodyBytes)
    {
        if (request.ContentLength > maximumRequestBodyBytes) return "REQUEST_BODY_TOO_LARGE";
        if (request.ContentType is null ||
            !request.ContentType.Split(';', 2)[0].Trim().Equals("application/json", StringComparison.OrdinalIgnoreCase))
            return "CONTENT_TYPE_UNSUPPORTED";
        var encoding = request.Headers["Content-Encoding"];
        if (encoding.Count > 1 || (encoding.Count == 1 &&
            !string.Equals(encoding[0], "identity", StringComparison.OrdinalIgnoreCase)))
            return "CONTENT_ENCODING_UNSUPPORTED";
        return null;
    }

    private static async Task<byte[]> ReadBoundedBodyAsync(
        HttpRequest request,
        int maximumRequestBodyBytes,
        CancellationToken cancellationToken)
    {
        await using var output = new MemoryStream();
        var buffer = new byte[16 * 1024];
        while (true)
        {
            var read = await request.Body.ReadAsync(buffer.AsMemory(), cancellationToken);
            if (read == 0) break;
            if (output.Length + read > maximumRequestBodyBytes)
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

    private static bool RequestTopologyMatches(
        HttpRequest request,
        SyncPopDeploymentProfile profile,
        string canonicalPath)
    {
        if (!string.Equals(request.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase)) return false;
        if (!string.Equals(request.Path.Value, canonicalPath, StringComparison.Ordinal) ||
            request.QueryString.HasValue)
            return false;
        string host;
        try { host = new System.Globalization.IdnMapping().GetAscii(request.Host.Host).ToLowerInvariant(); }
        catch (ArgumentException) { return false; }
        return string.Equals(host, profile.PublicHost, StringComparison.Ordinal) &&
               (request.Host.Port ?? 443) == profile.PublicPort;
    }

    private static SyncHttpAuthenticationResult Failed(IResult failure) => new(null, failure);
    private static IResult InvalidProof(Guid correlationId)
        => Error(StatusCodes.Status401Unauthorized, "invalid_dpop_proof", correlationId);
    private static IResult Error(int statusCode, string code, Guid correlationId)
        => Results.Json(new { ErrorCode = code, CorrelationId = correlationId }, statusCode: statusCode);
    private static IResult RequestLevelError(string code, Guid correlationId) => code switch
    {
        "REQUEST_BODY_TOO_LARGE" => Error(StatusCodes.Status413PayloadTooLarge, code, correlationId),
        "CONTENT_ENCODING_UNSUPPORTED" or "CONTENT_TYPE_UNSUPPORTED" =>
            Error(StatusCodes.Status415UnsupportedMediaType, code, correlationId),
        _ => Results.BadRequest(new { ErrorCode = code, CorrelationId = correlationId })
    };

    private sealed class SyncRequestException(string code) : InvalidOperationException(code)
    {
        public string Code { get; } = code;
    }
}
