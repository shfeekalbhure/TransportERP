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
        var group = app.MapGroup("/api/v1").RequireAuthorization("Authenticated");

        group.MapPost("/waybills/{waybillId:guid}/items/{itemId:guid}/releases", async Task<IResult> (
            Guid waybillId, Guid itemId, ReleaseItemRequest request, HttpContext http,
            ICurrentRequestSecurityResolver securityResolver, ShippingExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, securityResolver, ShippingExecutionPermissionCodes.Release, ct,
                context => service.ReleaseItemAsync(context, waybillId, itemId, request, ct)));

        group.MapPost("/trips", async Task<IResult> (
            CreateTripRequest request, HttpContext http, ICurrentRequestSecurityResolver securityResolver,
            ShippingExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, securityResolver, ShippingExecutionPermissionCodes.TripCreate, ct,
                context => service.CreateTripAsync(context, request, ct)));

        group.MapPost("/trips/{tripId:guid}/allocations", async Task<IResult> (
            Guid tripId, AllocateItemRequest request, HttpContext http, ICurrentRequestSecurityResolver securityResolver,
            ShippingExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, securityResolver, ShippingExecutionPermissionCodes.Allocate, ct,
                context => service.AllocateAsync(context, tripId, request, ct)));

        group.MapPost("/allocations/{allocationId:guid}:reverse", async Task<IResult> (
            Guid allocationId, UnallocateRequest request, HttpContext http, ICurrentRequestSecurityResolver securityResolver,
            ShippingExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, securityResolver, ShippingExecutionPermissionCodes.Unallocate, ct,
                context => service.UnallocateAsync(context, allocationId, request, ct)));

        group.MapPost("/trips/{tripId:guid}/manifests", async Task<IResult> (
            Guid tripId, GenerateManifestRequest request, HttpContext http, ICurrentRequestSecurityResolver securityResolver,
            ShippingExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, securityResolver, ShippingExecutionPermissionCodes.ManifestCreate, ct,
                context => service.GenerateManifestAsync(context, tripId, request, ct)));

        group.MapPost("/manifests/{manifestId:guid}/lines/{lineId:guid}:load", async Task<IResult> (
            Guid manifestId, Guid lineId, LoadManifestLineRequest request, HttpContext http,
            ICurrentRequestSecurityResolver securityResolver, ShippingExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, securityResolver, ShippingExecutionPermissionCodes.ManifestLoad, ct,
                context => service.LoadManifestLineAsync(context, manifestId, lineId, request, ct)));

        group.MapPost("/manifests/{manifestId:guid}:finalize", async Task<IResult> (
            Guid manifestId, FinalizeManifestRequest request, HttpContext http,
            ICurrentRequestSecurityResolver securityResolver, ShippingExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, securityResolver, ShippingExecutionPermissionCodes.ManifestFinalize, ct,
                context => service.FinalizeManifestAsync(context, manifestId, request, ct)));

        group.MapPost("/manifests/{manifestId:guid}:handover", async Task<IResult> (
            Guid manifestId, HandoverManifestRequest request, HttpContext http,
            ICurrentRequestSecurityResolver securityResolver, ShippingExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, securityResolver, ShippingExecutionPermissionCodes.ManifestHandover, ct,
                context => service.HandoverManifestAsync(context, manifestId, request, ct)));

        group.MapPost("/trips/{tripId:guid}:start", async Task<IResult> (
            Guid tripId, StartTripRequest request, HttpContext http,
            ICurrentRequestSecurityResolver securityResolver, ShippingExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, securityResolver, ShippingExecutionPermissionCodes.TripStart, ct,
                context => service.StartTripAsync(context, tripId, request, ct)));

        return app;
    }

    private static async Task<IResult> Authorized<T>(
        HttpContext http,
        ICurrentRequestSecurityResolver securityResolver,
        string permission,
        CancellationToken cancellationToken,
        Func<OperationContext, Task<T>> action)
    {
        var security = await securityResolver.ResolveAsync(http, permission, cancellationToken);
        if (!security.Succeeded) return security.Failure!;
        var context = security.Context!;
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

    private static IResult Forbidden(Guid correlationId)
        => Results.Json(new { ErrorCode = "SCOPE_DENIED", CorrelationId = correlationId }, statusCode: StatusCodes.Status403Forbidden);
}
