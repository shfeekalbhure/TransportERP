using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Wave1;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Wave1;

public static class Wave1NumberingAuthorityApiModule
{
    public static IEndpointRouteBuilder MapWave1AuthorizedNumbering(this IEndpointRouteBuilder app)
    {
        var sequences = app.MapGroup("/api/v1/general/number-sequences").RequireAuthorization("Authenticated");
        sequences.MapGet("", async (HttpContext h, Wave1NumberingAuthorityService s, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "GEN013.View")) return Forbidden(h);
            if (!TryContext(h, out var context)) return ScopeDenied(h);
            return Results.Ok(await s.ListAsync(context, ct));
        });
        sequences.MapPut("/{id:guid}", async (Guid id, UpdateNumberSequenceRequest r, HttpContext h, Wave1NumberingAuthorityService s, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "GEN013.Edit")) return Forbidden(h);
            if (!TryContext(h, out var context)) return ScopeDenied(h);
            try { var row = await s.UpdateAsync(context, id, r, ct); return row is null ? NotFound(h) : Results.Ok(row); }
            catch (DbUpdateConcurrencyException) { return Conflict("CONCURRENCY_CONFLICT", h); }
            catch (ArgumentException ex) { return Rule(ex.Message, h); }
        });
        sequences.MapPost("/{id:guid}/reservations", async (Guid id, NumberReservationCommandRequest r, HttpContext h, Wave1NumberingAuthorityService s, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "GEN013.Reserve")) return Forbidden(h);
            if (!TryContext(h, out var context)) return ScopeDenied(h);
            try { return Results.Ok(await s.ReserveAsync(context, id, r, ct)); }
            catch (KeyNotFoundException) { return NotFound(h); }
            catch (InvalidOperationException ex) { return Conflict(ex.Message, h); }
            catch (ArgumentException ex) { return Rule(ex.Message, h); }
        });
        sequences.MapPost("/{id:guid}/protected-action", async (Guid id, ProtectedNumberSequenceActionRequest r, HttpContext h, Wave1NumberingAuthorityService s, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "GEN013.Override")) return Forbidden(h);
            if (!TryContext(h, out var context)) return ScopeDenied(h);
            try { var row = await s.ProtectedActionAsync(context, id, r, ct); return row is null ? NotFound(h) : Results.Ok(row); }
            catch (DbUpdateConcurrencyException) { return Conflict("CONCURRENCY_CONFLICT", h); }
            catch (InvalidOperationException ex) { return Rule(ex.Message, h); }
            catch (ArgumentException ex) { return Rule(ex.Message, h); }
        });

        var reservations = app.MapGroup("/api/v1/general/number-reservations").RequireAuthorization("Authenticated");
        reservations.MapPost("/{reservationId:guid}/commit", async (Guid reservationId, NumberReservationTransitionCommandRequest r, HttpContext h, Wave1NumberingAuthorityService s, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "GEN013.Commit")) return Forbidden(h);
            if (!TryContext(h, out var context)) return ScopeDenied(h);
            try { return Results.Ok(await s.CommitAsync(context, reservationId, r, ct)); }
            catch (KeyNotFoundException) { return NotFound(h); }
            catch (InvalidOperationException ex) { return Rule(ex.Message, h); }
            catch (ArgumentException ex) { return Rule(ex.Message, h); }
        });
        reservations.MapPost("/{reservationId:guid}/cancel", async (Guid reservationId, NumberReservationTransitionCommandRequest r, HttpContext h, Wave1NumberingAuthorityService s, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "GEN013.Cancel")) return Forbidden(h);
            if (!TryContext(h, out var context)) return ScopeDenied(h);
            try { return Results.Ok(await s.CancelAsync(context, reservationId, r, ct)); }
            catch (KeyNotFoundException) { return NotFound(h); }
            catch (InvalidOperationException ex) { return Rule(ex.Message, h); }
            catch (ArgumentException ex) { return Rule(ex.Message, h); }
        });
        return app;
    }

    private static bool TryContext(HttpContext h, out OperationContext context)
    {
        context = default!;
        var user = TryGuid(h.User, ClaimTypes.NameIdentifier) ?? TryGuid(h.User, "sub");
        var company = TryGuid(h.User, "company_id"); var branch = TryGuid(h.User, "branch_id");
        if (!user.HasValue || !company.HasValue || !branch.HasValue) return false;
        context = new OperationContext(user.Value, company.Value, branch.Value, Correlation(h)); return true;
    }
    private static Guid? TryGuid(ClaimsPrincipal p, string type) => Guid.TryParse(p.FindFirstValue(type), out var v) ? v : null;
    private static bool HasPermission(ClaimsPrincipal p, string permission)
        => p.Claims.Any(x => x.Type is "permission" or ClaimTypes.Role && string.Equals(x.Value, permission, StringComparison.OrdinalIgnoreCase));
    private static Guid Correlation(HttpContext h) => Guid.TryParse(h.Request.Headers["X-Correlation-Id"].FirstOrDefault(), out var v) ? v : Guid.NewGuid();
    private static IResult Forbidden(HttpContext h) => Results.Json(new { ErrorCode = "PERMISSION_DENIED", CorrelationId = Correlation(h) }, statusCode: 403);
    private static IResult ScopeDenied(HttpContext h) => Results.Json(new { ErrorCode = "SCOPE_DENIED", CorrelationId = Correlation(h) }, statusCode: 403);
    private static IResult NotFound(HttpContext h) => Results.NotFound(new { ErrorCode = "NOT_FOUND", CorrelationId = Correlation(h) });
    private static IResult Conflict(string code, HttpContext h) => Results.Conflict(new { ErrorCode = code, CorrelationId = Correlation(h) });
    private static IResult Rule(string code, HttpContext h)
        => code is "IDEMPOTENCY_CONFLICT" or "CONCURRENCY_CONFLICT"
            ? Conflict(code, h)
            : Results.UnprocessableEntity(new { ErrorCode = code, CorrelationId = Correlation(h) });
}
