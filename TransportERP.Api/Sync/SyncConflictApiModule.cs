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
            ResolveSyncConflictRequest request,
            HttpContext http,
            ICurrentSecurityContext security,
            ISyncConflictResolutionService service,
            CancellationToken cancellationToken) =>
        {
            var correlationId = ReadCorrelationId(http);
            var current = await security.ResolveAsync(http.User, cancellationToken);
            if (current is null)
                return Results.Unauthorized();
            if (!current.IsLocalSession || !current.BranchId.HasValue ||
                !current.RegisteredDeviceId.HasValue || !current.DeviceCredentialVersion.HasValue ||
                string.IsNullOrWhiteSpace(current.DeviceId))
                return Error(StatusCodes.Status403Forbidden, "DEVICE_NOT_REGISTERED", correlationId);

            try
            {
                var result = await service.ResolveAsync(conflictCaseId, request,
                    new SyncConflictResolutionContext(
                        current.UserId, current.CompanyId, current.BranchId.Value,
                        current.RegisteredDeviceId.Value, current.DeviceCredentialVersion.Value,
                        current.DeviceId, correlationId), cancellationToken);
                return Results.Ok(result);
            }
            catch (SyncRuleException exception)
            {
                return MapError(exception.Code, correlationId);
            }
        }).RequireAuthorization(SecurityPolicies.Permission(SyncConflictPermissionCodes.Resolve));

        return app;
    }

    private static IResult MapError(string code, Guid correlationId) => code switch
    {
        // Do not expose whether a conflict id belongs to another scope or does not exist.
        "CONFLICT_NOT_FOUND" or "OPERATION_NOT_FOUND" or "SCOPE_DENIED" or "PERMISSION_DENIED" or
        "DEVICE_NOT_REGISTERED" or "ORIGINAL_ACTION_UNAVAILABLE" =>
            Error(StatusCodes.Status403Forbidden, "SCOPE_DENIED", correlationId),
        "CONFLICT_ALREADY_RESOLVED" or "REAPPLY_ID_REUSE" or "REAPPLY_SCOPE_MISMATCH" or
        "REAPPLY_PROOF_REQUIRED" or "ACTION_CONTRACT_MISMATCH" =>
            Error(StatusCodes.Status409Conflict, code, correlationId),
        "RESOLUTION_INVALID" or "RESOLUTION_REQUIRED" or "REASON_REQUIRED" or
        "REAPPLY_REQUEST_INVALID" or "PAYLOAD_INVALID" or "ONLINE_REQUIRED" =>
            Error(StatusCodes.Status400BadRequest, code, correlationId),
        "CONFLICT_STORE_UNSUPPORTED" =>
            Error(StatusCodes.Status503ServiceUnavailable, code, correlationId),
        _ => Error(StatusCodes.Status400BadRequest, code, correlationId)
    };

    private static IResult Error(int statusCode, string code, Guid correlationId)
        => Results.Json(new { ErrorCode = code, CorrelationId = correlationId }, statusCode: statusCode);

    private static Guid ReadCorrelationId(HttpContext http)
        => http.Request.Headers.TryGetValue("X-Correlation-Id", out var values) && values.Count == 1 &&
           Guid.TryParseExact(values[0], "D", out var parsed) && parsed != Guid.Empty
            ? parsed
            : Guid.NewGuid();
}
