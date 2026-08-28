using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using TransportERP.Api.Security;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Sync;

public static class SyncConflictApiModule
{
    public static IServiceCollection AddSyncConflictRuntime(this IServiceCollection services)
    {
        services.AddScoped<ISyncConflictResolutionService, SyncConflictResolutionService>();
        return services;
    }

    public static IEndpointRouteBuilder MapSyncConflictRuntime(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/sync/conflicts/{conflictCaseId:guid}:resolve", async Task<IResult> (
            Guid conflictCaseId,
            HttpContext http,
            ISyncPopHttpRequestAuthenticator authenticator,
            ISyncConflictResolutionService service,
            IOptions<SyncRuntimePolicyOptions> runtimePolicy,
            CancellationToken cancellationToken) =>
        {
            var canonicalPath = $"/api/v1/sync/conflicts/{conflictCaseId:D}:resolve";
            var authentication = await authenticator.AuthenticateAsync(
                http, canonicalPath, null, cancellationToken);
            if (authentication.Failure is not null) return authentication.Failure;
            var accepted = authentication.Accepted!;
            ResolveSyncConflictRequest? request;
            try { request = SyncConflictJsonContract.Deserialize(accepted.RawBody); }
            catch (JsonException)
            {
                return Error(StatusCodes.Status400BadRequest, "REQUEST_SCHEMA_INVALID",
                    accepted.AttemptCorrelationId);
            }
            if (request is null)
                return Error(StatusCodes.Status400BadRequest, "REQUEST_SCHEMA_INVALID",
                    accepted.AttemptCorrelationId);
            if (!SyncBuildIdentityAuthority.MatchesAuthorized(request.BuildIdentity, runtimePolicy))
                return Error(StatusCodes.Status403Forbidden, "BUILD_IDENTITY_MISMATCH",
                    accepted.AttemptCorrelationId);

            try
            {
                var result = await service.ResolveAsync(conflictCaseId, request,
                    new SyncConflictResolutionContext(
                        accepted.Proof.UserId, accepted.Proof.CompanyId, accepted.Proof.BranchId,
                        accepted.Proof.RegisteredDeviceId, accepted.Proof.DeviceCredentialVersion,
                        accepted.Proof.DeviceId, accepted.AttemptCorrelationId),
                    accepted.Proof, cancellationToken);
                return Results.Ok(result);
            }
            catch (SyncRuleException exception)
            {
                return MapError(exception.Code, accepted.AttemptCorrelationId);
            }
        }).RequireAuthorization(SecurityPolicies.Permission(SyncConflictPermissionCodes.Resolve));

        return app;
    }

    private static IResult MapError(string code, Guid correlationId) => code switch
    {
        "invalid_dpop_proof" =>
            Error(StatusCodes.Status401Unauthorized, code, correlationId),
        // Do not expose whether a conflict id belongs to another scope or does not exist.
        "CONFLICT_NOT_FOUND" or "OPERATION_NOT_FOUND" or "SCOPE_DENIED" or "PERMISSION_DENIED" or
        "DEVICE_NOT_REGISTERED" or "ORIGINAL_ACTION_UNAVAILABLE" =>
            Error(StatusCodes.Status403Forbidden, "SCOPE_DENIED", correlationId),
        "CONFLICT_ALREADY_RESOLVED" or "REAPPLY_ID_REUSE" or "REAPPLY_SCOPE_MISMATCH" or
        "ACTION_CONTRACT_MISMATCH" =>
            Error(StatusCodes.Status409Conflict, code, correlationId),
        "RESOLUTION_INVALID" or "RESOLUTION_REQUIRED" or "REASON_REQUIRED" or
        "REAPPLY_REQUEST_INVALID" or "PAYLOAD_INVALID" or "HASH_MISMATCH" or "ONLINE_REQUIRED" =>
            Error(StatusCodes.Status400BadRequest, code, correlationId),
        "CONFLICT_STORE_UNSUPPORTED" =>
            Error(StatusCodes.Status503ServiceUnavailable, code, correlationId),
        _ => Error(StatusCodes.Status400BadRequest, code, correlationId)
    };

    private static IResult Error(int statusCode, string code, Guid correlationId)
        => Results.Json(new { ErrorCode = code, CorrelationId = correlationId }, statusCode: statusCode);

}

public static class SyncConflictJsonContract
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = false,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
        MaxDepth = 64
    };

    public static ResolveSyncConflictRequest? Deserialize(byte[] utf8Json)
        => JsonSerializer.Deserialize<ResolveSyncConflictRequest>(utf8Json, Options);
}
