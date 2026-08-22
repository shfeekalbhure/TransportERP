using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Geo;
using TransportERP.Contracts.Wave1;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Wave1;

public static class Wave1ReferenceApiModule
{
    public static IEndpointRouteBuilder MapWave1ReferenceMasters(this IEndpointRouteBuilder app)
    {
        var languages = app.MapGroup("/api/v1/general/languages").RequireAuthorization("Authenticated");

        languages.MapGet("", async ([AsParameters] LanguageQueryRequest request, HttpContext h, Wave1ReferenceService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "GEN014.View")) return Forbidden(h);
            try { return Results.Ok(await service.ListLanguagesAsync(request, ct)); }
            catch (Wave1ReferenceRuleException ex) { return Rule(ex, h); }
        });

        languages.MapGet("/{id:guid}", async (Guid id, HttpContext h, Wave1ReferenceService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "GEN014.View")) return Forbidden(h);
            var row = await service.GetLanguageAsync(id, ct);
            return row is null ? NotFound(h) : Results.Ok(row);
        });

        languages.MapPost("", async (CreateLanguageRequest request, HttpContext h, Wave1ReferenceService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "GEN014.Create")) return Forbidden(h);
            if (!TryContext(h, out var context)) return ScopeDenied(h);
            try { return Results.Ok(await service.CreateLanguageAsync(context, request, ct)); }
            catch (Wave1ReferenceRuleException ex) { return Rule(ex, h); }
        });

        languages.MapPut("/{id:guid}", async (Guid id, UpdateLanguageRequest request, HttpContext h, Wave1ReferenceService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "GEN014.Edit")) return Forbidden(h);
            if (!TryContext(h, out var context)) return ScopeDenied(h);
            try
            {
                var row = await service.UpdateLanguageAsync(context, id, request, ct);
                return row is null ? NotFound(h) : Results.Ok(row);
            }
            catch (DbUpdateConcurrencyException) { return ConcurrencyConflict(h); }
            catch (Wave1ReferenceRuleException ex) { return Rule(ex, h); }
        });

        languages.MapPost("/{id:guid}/disable", async (Guid id, DisableRequest request, HttpContext h, Wave1ReferenceService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "GEN014.Disable")) return Forbidden(h);
            if (!TryContext(h, out var context)) return ScopeDenied(h);
            try
            {
                var row = await service.DisableLanguageAsync(context, id, request, ct);
                return row is null ? NotFound(h) : Results.Ok(row);
            }
            catch (DbUpdateConcurrencyException) { return ConcurrencyConflict(h); }
            catch (Wave1ReferenceRuleException ex) { return Rule(ex, h); }
        });

        // ACC-036 is intentionally not registered while its exact entity/DTO field contract is HOLD.
        return app;
    }

    private static bool TryContext(HttpContext h, out OperationContext context)
    {
        context = default!;
        if (!Guid.TryParse(h.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? h.User.FindFirstValue("sub"), out var userId)) return false;
        if (!Guid.TryParse(h.User.FindFirstValue("company_id"), out var companyId)) return false;
        if (!Guid.TryParse(h.User.FindFirstValue("branch_id"), out var branchId)) return false;
        context = new OperationContext(userId, companyId, branchId, Correlation(h));
        return true;
    }

    private static bool HasPermission(ClaimsPrincipal p, string permission)
        => p.Claims.Any(x => x.Type is "permission" or ClaimTypes.Role && string.Equals(x.Value, permission, StringComparison.OrdinalIgnoreCase));

    private static IResult Rule(Wave1ReferenceRuleException ex, HttpContext h)
        => ex.Code == "CONFLICT"
            ? Results.Conflict(new { ErrorCode = "CONFLICT", CorrelationId = Correlation(h) })
            : Results.BadRequest(new { ErrorCode = ex.Code, CorrelationId = Correlation(h) });

    private static Guid Correlation(HttpContext h)
        => Guid.TryParse(h.Request.Headers["X-Correlation-Id"].FirstOrDefault(), out var id) ? id : Guid.NewGuid();
    private static IResult Forbidden(HttpContext h) => Results.Json(new { ErrorCode = "PERMISSION_DENIED", CorrelationId = Correlation(h) }, statusCode: 403);
    private static IResult ScopeDenied(HttpContext h) => Results.Json(new { ErrorCode = "SCOPE_DENIED", CorrelationId = Correlation(h) }, statusCode: 403);
    private static IResult NotFound(HttpContext h) => Results.NotFound(new { ErrorCode = "NOT_FOUND", CorrelationId = Correlation(h) });
    private static IResult ConcurrencyConflict(HttpContext h) => Results.Conflict(new { ErrorCode = "CONCURRENCY_CONFLICT", CorrelationId = Correlation(h) });
}
