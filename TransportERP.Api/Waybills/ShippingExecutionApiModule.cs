using TransportERP.Api.Security;
using TransportERP.Application.Waybills;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Waybills;
using TransportERP.Domain.Waybills;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Waybills;

public static class ShippingExecutionApiModule
{
    public static IServiceCollection AddP2C01CShippingExecution(this IServiceCollection services)
    {
        services.AddScoped<IShippingExecutionStore, EfShippingExecutionStore>();
        services.AddScoped<ShippingExecutionApplicationService>();
        return services;
    }

    public static IEndpointRouteBuilder MapP2C01CShippingExecution(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1");

        group.MapPost("/waybills/{waybillId:guid}/items/{itemId:guid}/releases", async Task<IResult> (
            Guid waybillId, Guid itemId, ReleaseItemRequest request, HttpContext http,
            ICurrentSecurityContext security, ShippingExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, security, context => service.ReleaseItemAsync(context, waybillId, itemId, request, ct)))
            .RequireAuthorization(SecurityPolicies.Permission(ShippingExecutionPermissionCodes.Release));

        group.MapPost("/trips", async Task<IResult> (
            CreateTripRequest request, HttpContext http, ICurrentSecurityContext security, ShippingExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, security, context => service.CreateTripAsync(context, request, ct)))
            .RequireAuthorization(SecurityPolicies.Permission(ShippingExecutionPermissionCodes.TripCreate));

        group.MapPost("/trips/{tripId:guid}/allocations", async Task<IResult> (
            Guid tripId, AllocateItemRequest request, HttpContext http, ICurrentSecurityContext security, ShippingExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, security, context => service.AllocateAsync(context, tripId, request, ct)))
            .RequireAuthorization(SecurityPolicies.Permission(ShippingExecutionPermissionCodes.Allocate));

        group.MapPost("/allocations/{allocationId:guid}:reverse", async Task<IResult> (
            Guid allocationId, UnallocateRequest request, HttpContext http, ICurrentSecurityContext security, ShippingExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, security, context => service.UnallocateAsync(context, allocationId, request, ct)))
            .RequireAuthorization(SecurityPolicies.Permission(ShippingExecutionPermissionCodes.Unallocate));

        group.MapPost("/trips/{tripId:guid}/manifests", async Task<IResult> (
            Guid tripId, GenerateManifestRequest request, HttpContext http, ICurrentSecurityContext security, ShippingExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, security, context => service.GenerateManifestAsync(context, tripId, request, ct)))
            .RequireAuthorization(SecurityPolicies.Permission(ShippingExecutionPermissionCodes.ManifestCreate));

        group.MapPost("/manifests/{manifestId:guid}/lines/{lineId:guid}:load", async Task<IResult> (
            Guid manifestId, Guid lineId, LoadManifestLineRequest request, HttpContext http,
            ICurrentSecurityContext security, ShippingExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, security, context => service.LoadManifestLineAsync(context, manifestId, lineId, request, ct)))
            .RequireAuthorization(SecurityPolicies.Permission(ShippingExecutionPermissionCodes.ManifestLoad));

        group.MapPost("/manifests/{manifestId:guid}:finalize", async Task<IResult> (
            Guid manifestId, FinalizeManifestRequest request, HttpContext http,
            ICurrentSecurityContext security, ShippingExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, security, context => service.FinalizeManifestAsync(context, manifestId, request, ct)))
            .RequireAuthorization(SecurityPolicies.Permission(ShippingExecutionPermissionCodes.ManifestFinalize));

        group.MapPost("/manifests/{manifestId:guid}:handover", async Task<IResult> (
            Guid manifestId, HandoverManifestRequest request, HttpContext http,
            ICurrentSecurityContext security, ShippingExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, security, context => service.HandoverManifestAsync(context, manifestId, request, ct)))
            .RequireAuthorization(SecurityPolicies.Permission(ShippingExecutionPermissionCodes.ManifestHandover));

        group.MapPost("/trips/{tripId:guid}:start", async Task<IResult> (
            Guid tripId, StartTripRequest request, HttpContext http,
            ICurrentSecurityContext security, ShippingExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, security, context => service.StartTripAsync(context, tripId, request, ct)))
            .RequireAuthorization(SecurityPolicies.Permission(ShippingExecutionPermissionCodes.TripStart));

        return app;
    }

    private static async Task<IResult> Authorized<T>(
        HttpContext http, ICurrentSecurityContext security, Func<OperationContext, Task<T>> action)
    {
        var (context, failure) = await TryContextAsync(http, security, http.RequestAborted);
        if (failure is not null) return failure;
        return await Execute(context, () => action(context));
    }

    private static async Task<IResult> Execute<T>(OperationContext context, Func<Task<T>> action)
    {
        try
        {
            return Results.Ok(await action());
        }
        catch (ShippingExecutionApplicationException ex)
        {
            return MapError(ex.Code, context.CorrelationId);
        }
        catch (ShippingExecutionRuleException ex)
        {
            return MapError(ex.Code, context.CorrelationId);
        }
        catch (WaybillPersistenceException ex)
        {
            return MapError(ex.Code, context.CorrelationId);
        }
        catch (ArgumentException)
        {
            return Results.BadRequest(new { ErrorCode = "VALIDATION_ERROR", CorrelationId = context.CorrelationId });
        }
    }

    private static IResult MapError(string code, Guid correlationId) => code switch
    {
        "NOT_FOUND" => Results.NotFound(new { ErrorCode = code, CorrelationId = correlationId }),
        "SCOPE_DENIED" => Forbidden(correlationId),
        "CONCURRENCY_CONFLICT" or "IDEMPOTENCY_CONFLICT" or "DUPLICATE_OPERATION" or
        "DUPLICATE_TRIP_NO" or "INVALID_STATE" or "HOLD_BLOCKED" or "ALREADY_LOADED" or
        "QUANTITY_EXCEEDS_REMAINING" or "QUANTITY_EXCEEDS_RELEASED" or "QUANTITY_EXCEEDS_ALLOCATION" or
        "ROUTE_INCOMPATIBLE" or "NO_ALLOCATIONS" or "RESOURCE_CONSTRAINT" or "MANIFEST_LINE_INVALID" or
        "MANIFEST_NOT_ACCEPTED" or "DRIVER_MISMATCH" =>
            Results.Json(new { ErrorCode = code, CorrelationId = correlationId }, statusCode: StatusCodes.Status409Conflict),
        _ => Results.BadRequest(new { ErrorCode = code, CorrelationId = correlationId })
    };

    private static async Task<(OperationContext Context, IResult? Failure)> TryContextAsync(
        HttpContext http, ICurrentSecurityContext security, CancellationToken ct)
    {
        var current = await security.ResolveAsync(http.User, ct);
        if (current is null || !current.BranchId.HasValue) return (default!, Results.Unauthorized());
        var correlationId = Guid.TryParse(http.Request.Headers["X-Correlation-Id"].FirstOrDefault(), out var parsed)
            ? parsed : Guid.NewGuid();
        return (current.ToOperationContext(correlationId), null);
    }

    private static IResult Forbidden(Guid correlationId)
        => Results.Json(new { ErrorCode = "SCOPE_DENIED", CorrelationId = correlationId }, statusCode: StatusCodes.Status403Forbidden);
}
