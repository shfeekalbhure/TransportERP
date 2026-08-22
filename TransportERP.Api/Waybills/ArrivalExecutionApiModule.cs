using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TransportERP.Application.Waybills;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Waybills;
using TransportERP.Domain.Waybills;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Waybills;

public static class ArrivalExecutionApiModule
{
    public static IServiceCollection AddP2C01DArrivalExecution(this IServiceCollection services)
    {
        services.AddScoped<IArrivalExecutionStore, EfArrivalExecutionStore>();
        services.AddScoped<ArrivalExecutionApplicationService>();
        return services;
    }

    public static IEndpointRouteBuilder MapP2C01DArrivalExecution(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1").RequireAuthorization("Authenticated");

        group.MapPost("/trips/{tripId:guid}/arrivals", async Task<IResult> (
            Guid tripId, RecordArrivalRequest request, HttpContext http,
            ArrivalExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, ArrivalExecutionPermissionCodes.RecordArrival,
                context => service.RecordArrivalAsync(context, tripId, request, ct)));

        group.MapPost("/arrivals/{arrivalId:guid}/lines:unload", async Task<IResult> (
            Guid arrivalId, RecordUnloadRequest request, HttpContext http,
            ArrivalExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, ArrivalExecutionPermissionCodes.RecordUnload,
                context => service.RecordUnloadAsync(context, arrivalId, request, ct)));

        group.MapPost("/holdings/{holdingId:guid}:allocate", async Task<IResult> (
            Guid holdingId, ReallocateTransitRequest request, HttpContext http,
            ArrivalExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, ArrivalExecutionPermissionCodes.Reallocate,
                context => service.ReallocateTransitAsync(context, holdingId, request, ct)));

        group.MapPost("/arrivals/{arrivalId:guid}:finalize", async Task<IResult> (
            Guid arrivalId, FinalizeArrivalRequest request, HttpContext http,
            ArrivalExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, ArrivalExecutionPermissionCodes.FinalizeArrival,
                context => service.FinalizeArrivalAsync(context, arrivalId, request, ct)));

        group.MapPost("/trips/{tripId:guid}:close", async Task<IResult> (
            Guid tripId, CloseTripRequest request, HttpContext http,
            ArrivalExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, ArrivalExecutionPermissionCodes.TripClose,
                context => service.CloseTripAsync(context, tripId, request, ct)));

        group.MapGet("/waybills/{waybillId:guid}/movement", async Task<IResult> (
            Guid waybillId, [AsParameters] MovementQueryRequest request, HttpContext http,
            ArrivalExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, ArrivalExecutionPermissionCodes.WaybillMovementView,
                context => service.GetWaybillMovementAsync(context, waybillId, request, ct)));

        group.MapGet("/waybills/{waybillId:guid}/items/{itemId:guid}/movement", async Task<IResult> (
            Guid waybillId, Guid itemId, [AsParameters] MovementQueryRequest request, HttpContext http,
            ArrivalExecutionApplicationService service, CancellationToken ct) =>
            await Authorized(http, ArrivalExecutionPermissionCodes.ItemMovementView,
                context => service.GetItemMovementAsync(context, waybillId, itemId, request, ct)));

        return app;
    }

    private static async Task<IResult> Authorized<T>(HttpContext http, string permission, Func<OperationContext, Task<T>> action)
    {
        if (!TryContext(http, out var context, out var failure)) return failure!;
        if (!HasPermission(http.User, permission)) return Forbidden(context.CorrelationId);
        try { return Results.Ok(await action(context)); }
        catch (ArrivalExecutionApplicationException ex) { return MapError(ex.Code, context.CorrelationId); }
        catch (ArrivalExecutionRuleException ex) { return MapError(ex.Code, context.CorrelationId); }
        catch (WaybillPersistenceException ex) { return MapError(ex.Code, context.CorrelationId); }
        catch (ArgumentException) { return Results.BadRequest(new { ErrorCode = "VALIDATION_ERROR", CorrelationId = context.CorrelationId }); }
    }

    private static IResult MapError(string code, Guid correlationId) => code switch
    {
        "NOT_FOUND" => Results.NotFound(new { ErrorCode = code, CorrelationId = correlationId }),
        "SCOPE_DENIED" => Forbidden(correlationId),
        "INVALID_STATE" or "LOCATION_INVALID" or "DUPLICATE_OPERATION" or "IDEMPOTENCY_CONFLICT" or
        "CONCURRENCY_CONFLICT" or "QUANTITY_EXCEEDS_IN_TRANSIT" or "QUANTITY_EXCEEDS_AVAILABLE" or
        "ROUTE_INCOMPATIBLE" or "HOLD_BLOCKED" or "UNVALIDATED_LINES" or "DIFFERENCE_REQUIRES_EVIDENCE" or
        "CARGO_UNACCOUNTED" or "CUSTODY_OPEN" or "EXCEPTION_BLOCKED" =>
            Results.Json(new { ErrorCode = code, CorrelationId = correlationId }, statusCode: StatusCodes.Status409Conflict),
        _ => Results.BadRequest(new { ErrorCode = code, CorrelationId = correlationId })
    };

    private static bool TryContext(HttpContext http, out OperationContext context, out IResult? failure)
    {
        context = default!; failure = null;
        if (http.User.Identity?.IsAuthenticated != true) { failure = Results.Unauthorized(); return false; }
        if (!TryGuid(http.User, ClaimTypes.NameIdentifier, "sub", out var userId) ||
            !TryGuid(http.User, "company_id", null, out var companyId) ||
            !TryGuid(http.User, "branch_id", null, out var branchId))
        {
            failure = Results.Unauthorized(); return false;
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
