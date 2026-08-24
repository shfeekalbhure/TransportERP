using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Wave1;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Wave1;

public static class Wave1NumberingApiModule
{
    public static IEndpointRouteBuilder MapWave1Numbering(this IEndpointRouteBuilder app)
    {
        var sequences = app.MapGroup("/api/v1/general/number-sequences").RequireAuthorization("Authenticated");

        sequences.MapGet("", async (HttpContext h, Wave1NumberingService s, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "GEN013.View")) return Forbidden(h);
            if (!TryContext(h, out var context)) return ScopeDenied(h);
            return Results.Ok(await s.ListAsync(context, ct));
        });

        sequences.MapPut("/{id:guid}", async (Guid id, UpdateNumberSequenceRequest r, HttpContext h, Wave1NumberingService s, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "GEN013.Edit")) return Forbidden(h);
            if (!TryContext(h, out var context)) return ScopeDenied(h);
            try
            {
                var row = await s.UpdateAsync(context, id, r, ct);
                return row is null ? NotFound(h) : Results.Ok(row);
            }
            catch (DbUpdateConcurrencyException) { return Conflict(h); }
            catch (ArgumentException ex) { return Unprocessable(ex.Message, h); }
        });

        sequences.MapPost("/{id:guid}/reservations", async (Guid id, NumberReservationCommandRequest r, HttpContext h, Wave1NumberingService s, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "GEN013.Reserve")) return Forbidden(h);
            if (!TryContext(h, out var context)) return ScopeDenied(h);
            try { return Results.Ok(await s.ReserveAsync(context, id, r, ct)); }
            catch (WaybillPersistenceException ex) { return PersistenceError(ex, h); }
            catch (ArgumentException ex) { return Unprocessable(ex.Message, h); }
        });

        sequences.MapPost("/{id:guid}/protected-action", async (Guid id, ProtectedNumberSequenceActionRequest r, HttpContext h, Wave1NumberingService s, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "GEN013.Override")) return Forbidden(h);
            if (!TryContext(h, out var context)) return ScopeDenied(h);
            try
            {
                var row = await s.ProtectedActionAsync(context, id, r, ct);
                return row is null ? NotFound(h) : Results.Ok(row);
            }
            catch (DbUpdateConcurrencyException) { return Conflict(h); }
            catch (InvalidOperationException ex) { return Unprocessable(ex.Message, h); }
            catch (ArgumentException ex) { return Unprocessable(ex.Message, h); }
        });

        var reservations = app.MapGroup("/api/v1/general/number-reservations").RequireAuthorization("Authenticated");
        reservations.MapPost("/{reservationId:guid}/commit", async (Guid reservationId, NumberReservationTransitionCommandRequest r, HttpContext h, Wave1NumberingService s, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "GEN013.Commit")) return Forbidden(h);
            if (!TryContext(h, out var context)) return ScopeDenied(h);
            try { return Results.Ok(await s.CommitAsync(context, reservationId, r, ct)); }
            catch (WaybillPersistenceException ex) { return PersistenceError(ex, h); }
            catch (ArgumentException ex) { return Unprocessable(ex.Message, h); }
        });

        reservations.MapPost("/{reservationId:guid}/cancel", async (Guid reservationId, NumberReservationTransitionCommandRequest r, HttpContext h, Wave1NumberingService s, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "GEN013.Cancel")) return Forbidden(h);
            if (!TryContext(h, out var context)) return ScopeDenied(h);
            try { return Results.Ok(await s.CancelAsync(context, reservationId, r, ct)); }
            catch (WaybillPersistenceException ex) { return PersistenceError(ex, h); }
            catch (ArgumentException ex) { return Unprocessable(ex.Message, h); }
        });

        return app;
    }

    private static bool TryContext(HttpContext h, out OperationContext context)
    {
        var userId = TryGuid(h.User, ClaimTypes.NameIdentifier) ?? TryGuid(h.User, "sub");
        var companyId = TryGuid(h.User, "company_id");
        var branchId = TryGuid(h.User, "branch_id");
        if (!userId.HasValue || !companyId.HasValue || !branchId.HasValue)
        {
            context = null!;
            return false;
        }
        context = new OperationContext(userId.Value, companyId.Value, branchId.Value, Correlation(h));
        return true;
    }

    private static bool HasPermission(ClaimsPrincipal p, string permission)
        => p.Claims.Any(x => x.Type is "permission" or ClaimTypes.Role && string.Equals(x.Value, permission, StringComparison.OrdinalIgnoreCase));

    private static Guid? TryGuid(ClaimsPrincipal p, string type)
        => Guid.TryParse(p.FindFirstValue(type), out var value) ? value : null;

    private static Guid Correlation(HttpContext h)
        => Guid.TryParse(h.Request.Headers["X-Correlation-Id"].FirstOrDefault(), out var value) ? value : Guid.NewGuid();

    private static IResult PersistenceError(WaybillPersistenceException ex, HttpContext h)
        => ex.Code is "NUMBERING_CONCURRENCY" or "IDEMPOTENCY_CONFLICT"
            ? Results.Conflict(new { ErrorCode = ex.Code, CorrelationId = Correlation(h) })
            : Results.UnprocessableEntity(new { ErrorCode = ex.Code, CorrelationId = Correlation(h) });

    private static IResult Forbidden(HttpContext h)
        => Results.Json(new { ErrorCode = "PERMISSION_DENIED", CorrelationId = Correlation(h) }, statusCode: StatusCodes.Status403Forbidden);

    private static IResult ScopeDenied(HttpContext h)
        => Results.Json(new { ErrorCode = "SCOPE_DENIED", CorrelationId = Correlation(h) }, statusCode: StatusCodes.Status403Forbidden);

    private static IResult Conflict(HttpContext h)
        => Results.Conflict(new { ErrorCode = "CONCURRENCY_CONFLICT", CorrelationId = Correlation(h) });

    private static IResult NotFound(HttpContext h)
        => Results.NotFound(new { ErrorCode = "NOT_FOUND", CorrelationId = Correlation(h) });

    private static IResult Unprocessable(string code, HttpContext h)
        => Results.UnprocessableEntity(new { ErrorCode = code, CorrelationId = Correlation(h) });
}
