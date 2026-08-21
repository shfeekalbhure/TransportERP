using System.Security.Claims;
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
            ShippingExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, ShippingExecutionPermissionCodes.Release,
                context => service.ReleaseItemAsync(context, waybillId, itemId, request, ct)));

        group.MapPost("/trips", async Task<IResult> (
            CreateTripRequest request, HttpContext http, ShippingExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, ShippingExecutionPermissionCodes.TripCreate,
                context => service.CreateTripAsync(context, request, ct)));

        group.MapPost("/trips/{tripId:guid}/allocations", async Task<IResult> (
            Guid tripId, AllocateItemRequest request, HttpContext http, ShippingExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, ShippingExecutionPermissionCodes.Allocate,
                context => service.AllocateAsync(context, tripId, request, ct)));

        group.MapPost("/allocations/{allocationId:guid}:reverse", async Task<IResult> (
            Guid allocationId, UnallocateRequest request, HttpContext http, ShippingExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, ShippingExecutionPermissionCodes.Unallocate,
                context => service.UnallocateAsync(context, allocationId, request, ct)));

        group.MapPost("/trips/{tripId:guid}/manifests", async Task<IResult> (
            Guid tripId, GenerateManifestRequest request, HttpContext http, ShippingExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, ShippingExecutionPermissionCodes.ManifestCreate,
                context => service.GenerateManifestAsync(context, tripId, request, ct)));

        group.MapPost("/manifests/{manifestId:guid}/lines/{lineId:guid}:load", async Task<IResult> (
            Guid manifestId, Guid lineId, LoadManifestLineRequest request, HttpContext http, ShippingExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, ShippingExecutionPermissionCodes.ManifestLoad,
                context => service.LoadManifestLineAsync(context, manifestId, lineId, request, ct)));

        group.MapPost("/manifests/{manifestId:guid}:finalize", async Task<IResult> (
            Guid manifestId, FinalizeManifestRequest request, HttpContext http, ShippingExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, ShippingExecutionPermissionCodes.ManifestFinalize,
                context => service.FinalizeManifestAsync(context, manifestId, request, ct)));

        group.MapPost("/manifests/{manifestId:guid}:handover", async Task<IResult> (
            Guid manifestId, HandoverManifestRequest request, HttpContext http, ShippingExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, ShippingExecutionPermissionCodes.ManifestHandover,
                context => service.HandoverManifestAsync(context, manifestId, request, ct)));

        group.MapPost("/trips/{tripId:guid}:start", async Task<IResult> (
            Guid tripId, StartTripRequest request, HttpContext http, ShippingExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, ShippingExecutionPermissionCodes.TripStart,
                context => service.StartTripAsync(context, tripId, request, ct)));

        return app;
    }

    private static async Task<IResult> Authorized<T>(
        HttpContext http, string permission, Func<OperationContext, Task<T>> action)
    {
        if (!TryContext(http, out var context, out var failure)) return failure!;
        if (!HasPermission(http.User, permission)) return Forbidden(context.CorrelationId);
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
        "DUPLICATE_TRIP_NO" or "INVALID_STATE" or "ALREADY_LOADED" or "HOLD_BLOCKED" or
        "MANIFEST_NOT_ACCEPTED" or "DRIVER_MISMATCH" =>
            Results.Json(new { ErrorCode = code, CorrelationId = correlationId }, statusCode: StatusCodes.Status409Conflict),
        _ => Results.BadRequest(new { ErrorCode = code, CorrelationId = correlationId })
    };

    private static bool TryContext(HttpContext http, out OperationContext context, out IResult? failure)
    {
        context = default!;
        failure = null;
        if (http.User.Identity?.IsAuthenticated != true)
        {
            failure = Results.Unauthorized();
            return false;
        }
        if (!TryGuid(http.User, ClaimTypes.NameIdentifier, "sub", out var userId) ||
            !TryGuid(http.User, "company_id", null, out var companyId) ||
            !TryGuid(http.User, "branch_id", null, out var branchId))
        {
            failure = Results.Unauthorized();
            return false;
        }
        var correlationId = Guid.TryParse(http.Request.Headers["X-Correlation-Id"].FirstOrDefault(), out var parsed)
            ? parsed : Guid.NewGuid();
        context = new OperationContext(userId, companyId, branchId, correlationId);
        return true;
    }

    private static bool TryGuid(ClaimsPrincipal principal, string first, string? second, out Guid value)
    {
        var raw = principal.FindFirstValue(first) ?? (second is null ? null : principal.FindFirstValue(second));
        return Guid.TryParse(raw, out value);
    }

    private static bool HasPermission(ClaimsPrincipal principal, string permission)
        => principal.Claims.Any(x => x.Type is "permission" or ClaimTypes.Role &&
            string.Equals(x.Value, permission, StringComparison.OrdinalIgnoreCase));

    private static IResult Forbidden(Guid correlationId)
        => Results.Json(new { ErrorCode = "SCOPE_DENIED", CorrelationId = correlationId }, statusCode: StatusCodes.Status403Forbidden);
}
