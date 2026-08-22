using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Wave1;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Wave1;

public static class Wave1ReferenceApiModule
{
    public static IEndpointRouteBuilder MapWave1ReferenceMasters(this IEndpointRouteBuilder app)
    {
        var languages = app.MapGroup("/api/v1/general/languages").RequireAuthorization("Authenticated");
        languages.MapGet("", async (int? skip, int? take, HttpContext h, Wave1ReferenceService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "GEN014.View")) return Forbidden(h);
            try { return Results.Ok(await service.ListLanguagesAsync(skip ?? 0, take ?? 100, ct)); }
            catch (ArgumentOutOfRangeException ex) { return Bad(ex.Message, h); }
        });
        languages.MapPost("", async (CreateLanguageRequest request, HttpContext h, Wave1ReferenceService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "GEN014.Create")) return Forbidden(h);
            if (!TryContext(h, out var context)) return ScopeDenied(h);
            try { return Results.Ok(await service.CreateLanguageAsync(context, request, ct)); }
            catch (ArgumentException ex) { return Unprocessable(ex.Message, h); }
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
            catch (DbUpdateConcurrencyException) { return Conflict(h); }
            catch (ArgumentException ex) { return Unprocessable(ex.Message, h); }
        });
        languages.MapPost("/{id:guid}/disable", async (Guid id, DisableReferenceRequest request, HttpContext h, Wave1ReferenceService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "GEN014.Disable")) return Forbidden(h);
            if (!TryContext(h, out var context)) return ScopeDenied(h);
            try
            {
                var row = await service.DisableLanguageAsync(context, id, request, ct);
                return row is null ? NotFound(h) : Results.Ok(row);
            }
            catch (DbUpdateConcurrencyException) { return Conflict(h); }
            catch (ArgumentException ex) { return Unprocessable(ex.Message, h); }
        });
        languages.MapPut("/{languageId:guid}/translations", async (Guid languageId, UpsertTranslationRequest request, HttpContext h, Wave1ReferenceService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "GEN014.Edit")) return Forbidden(h);
            if (!TryContext(h, out var context)) return ScopeDenied(h);
            try { return Results.Ok(await service.UpsertTranslationAsync(context, languageId, request, ct)); }
            catch (DbUpdateConcurrencyException) { return Conflict(h); }
            catch (ArgumentException ex) { return Unprocessable(ex.Message, h); }
        });

        var classifications = app.MapGroup("/api/v1/accounting/account-classifications").RequireAuthorization("Authenticated");
        classifications.MapGet("", async (int? skip, int? take, HttpContext h, Wave1ReferenceService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "ACC036.View")) return Forbidden(h);
            if (!TryContext(h, out var context)) return ScopeDenied(h);
            try { return Results.Ok(await service.ListClassificationsAsync(context.CompanyId, skip ?? 0, take ?? 100, ct)); }
            catch (ArgumentOutOfRangeException ex) { return Bad(ex.Message, h); }
        });
        classifications.MapPost("", async (CreateAccountClassificationRequest request, HttpContext h, Wave1ReferenceService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "ACC036.Create")) return Forbidden(h);
            if (!TryContext(h, out var context)) return ScopeDenied(h);
            try { return Results.Ok(await service.CreateClassificationAsync(context, request, ct)); }
            catch (ArgumentException ex) { return Unprocessable(ex.Message, h); }
        });
        classifications.MapPut("/{id:guid}", async (Guid id, UpdateAccountClassificationRequest request, HttpContext h, Wave1ReferenceService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "ACC036.Edit")) return Forbidden(h);
            if (!TryContext(h, out var context)) return ScopeDenied(h);
            try
            {
                var row = await service.UpdateClassificationAsync(context, id, request, ct);
                return row is null ? NotFound(h) : Results.Ok(row);
            }
            catch (DbUpdateConcurrencyException) { return Conflict(h); }
            catch (ArgumentException ex) { return Unprocessable(ex.Message, h); }
        });
        classifications.MapPost("/{id:guid}/disable", async (Guid id, DisableReferenceRequest request, HttpContext h, Wave1ReferenceService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "ACC036.Disable")) return Forbidden(h);
            if (!TryContext(h, out var context)) return ScopeDenied(h);
            try
            {
                var row = await service.DisableClassificationAsync(context, id, request, ct);
                return row is null ? NotFound(h) : Results.Ok(row);
            }
            catch (DbUpdateConcurrencyException) { return Conflict(h); }
            catch (ArgumentException ex) { return Unprocessable(ex.Message, h); }
        });

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
    private static Guid Correlation(HttpContext h)
        => Guid.TryParse(h.Request.Headers["X-Correlation-Id"].FirstOrDefault(), out var id) ? id : Guid.NewGuid();
    private static IResult Forbidden(HttpContext h) => Results.Json(new { ErrorCode = "PERMISSION_DENIED", CorrelationId = Correlation(h) }, statusCode: 403);
    private static IResult ScopeDenied(HttpContext h) => Results.Json(new { ErrorCode = "SCOPE_DENIED", CorrelationId = Correlation(h) }, statusCode: 403);
    private static IResult NotFound(HttpContext h) => Results.NotFound(new { ErrorCode = "NOT_FOUND", CorrelationId = Correlation(h) });
    private static IResult Conflict(HttpContext h) => Results.Conflict(new { ErrorCode = "CONCURRENCY_CONFLICT", CorrelationId = Correlation(h) });
    private static IResult Bad(string message, HttpContext h) => Results.BadRequest(new { ErrorCode = "INVALID_FILTER", Message = message, CorrelationId = Correlation(h) });
    private static IResult Unprocessable(string message, HttpContext h) => Results.UnprocessableEntity(new { ErrorCode = message, CorrelationId = Correlation(h) });
}
