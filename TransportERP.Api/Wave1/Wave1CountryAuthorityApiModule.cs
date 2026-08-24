using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TransportERP.Contracts.Geo;
using TransportERP.Contracts.Wave1;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Wave1;

public static class Wave1CountryAuthorityApiModule
{
    public static IEndpointRouteBuilder MapWave1Countries(this IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/api/v1/general/countries").RequireAuthorization("Authenticated");
        g.MapGet("", async ([AsParameters] PagedQueryRequest q, HttpContext h, Wave1CountryAuthorityService s, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "GEN003.View")) return Forbidden(h);
            try { return Results.Ok(await s.ListAsync(q, ct)); }
            catch (ArgumentOutOfRangeException ex) { return Results.BadRequest(new { ErrorCode = "INVALID_FILTER", Message = ex.Message, CorrelationId = Correlation(h) }); }
        });
        g.MapGet("/{id:guid}", async (Guid id, HttpContext h, Wave1CountryAuthorityService s, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "GEN003.View")) return Forbidden(h);
            var row = await s.GetAsync(id, ct);
            return row is null ? Results.NotFound(new { ErrorCode = "NOT_FOUND", CorrelationId = Correlation(h) }) : Results.Ok(row);
        });
        g.MapPost("", async (CreateCountryRequest r, HttpContext h, Wave1CountryAuthorityService s, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "GEN003.Create")) return Forbidden(h);
            try { return Results.Ok(await s.CreateAsync(r, Context(h), ct)); }
            catch (ArgumentException ex) { return Rule(ex.Message, h); }
        });
        g.MapPut("/{id:guid}", async (Guid id, UpdateCountryRequest r, HttpContext h, Wave1CountryAuthorityService s, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "GEN003.Edit")) return Forbidden(h);
            try { var row = await s.UpdateAsync(id, r, Context(h), ct); return row is null ? Results.NotFound(new { ErrorCode = "NOT_FOUND", CorrelationId = Correlation(h) }) : Results.Ok(row); }
            catch (DbUpdateConcurrencyException) { return Results.Conflict(new { ErrorCode = "CONCURRENCY_CONFLICT", CorrelationId = Correlation(h) }); }
            catch (ArgumentException ex) { return Rule(ex.Message, h); }
        });
        g.MapPost("/{id:guid}/disable", async (Guid id, DisableRequest r, HttpContext h, Wave1CountryAuthorityService s, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "GEN003.Disable")) return Forbidden(h);
            try { var row = await s.DisableAsync(id, r, Context(h), ct); return row is null ? Results.NotFound(new { ErrorCode = "NOT_FOUND", CorrelationId = Correlation(h) }) : Results.Ok(row); }
            catch (DbUpdateConcurrencyException) { return Results.Conflict(new { ErrorCode = "CONCURRENCY_CONFLICT", CorrelationId = Correlation(h) }); }
            catch (ArgumentException ex) { return Rule(ex.Message, h); }
        });
        g.MapPost("/print", async (GEN003PrintRequest r, HttpContext h, Wave1CountryAuthorityService s, Wave1DeliveryAuditWriter audit, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "GEN003.Print")) return Forbidden(h);
            var list = await s.ListAsync(new PagedQueryRequest(1, 200, r.SearchText, r.Sort, r.Direction, IsActive: r.IsActive), ct);
            var sb = new StringBuilder("<!doctype html><html dir=\"rtl\"><meta charset=\"utf-8\"><body><h1>الدول</h1><table>");
            foreach (var x in list.Items) sb.Append($"<tr><td>{System.Net.WebUtility.HtmlEncode(x.Code)}</td><td>{System.Net.WebUtility.HtmlEncode(x.ArabicName)}</td><td>{System.Net.WebUtility.HtmlEncode(x.ISO2)}</td><td>{System.Net.WebUtility.HtmlEncode(x.ISO3)}</td><td>{System.Net.WebUtility.HtmlEncode(x.DialingCode)}</td></tr>");
            sb.Append("</table></body></html>");
            var correlation = Correlation(h);
            await audit.AppendSuccessAsync("GEN-003", "Print", new { r.SearchText, r.Sort, r.Direction, r.IsActive }, DeliveryContext(h, correlation), ct);
            return Results.Ok(new PrintPayloadOrJobResponse("الدول", "text/html; charset=utf-8", sb.ToString()));
        });
        return app;
    }

    private static Wave1GeoOperationContext Context(HttpContext h)
        => new(TryGuid(h.User, ClaimTypes.NameIdentifier) ?? TryGuid(h.User, "sub"), TryGuid(h.User, "company_id"), TryGuid(h.User, "branch_id"), Correlation(h), h.User.FindFirstValue("device_id"), h.Connection.RemoteIpAddress?.ToString());
    private static Wave1DeliveryAuditContext DeliveryContext(HttpContext h, Guid correlation)
        => new(TryGuid(h.User, ClaimTypes.NameIdentifier) ?? TryGuid(h.User, "sub"), TryGuid(h.User, "company_id"), TryGuid(h.User, "branch_id"), correlation, h.User.FindFirstValue("device_id"), h.Connection.RemoteIpAddress?.ToString());
    private static Guid? TryGuid(ClaimsPrincipal p, string type) => Guid.TryParse(p.FindFirstValue(type), out var v) ? v : null;
    private static bool HasPermission(ClaimsPrincipal p, string permission) => p.Claims.Any(x => x.Type is "permission" or ClaimTypes.Role && string.Equals(x.Value, permission, StringComparison.OrdinalIgnoreCase));
    private static Guid Correlation(HttpContext h) => Guid.TryParse(h.Request.Headers["X-Correlation-Id"].FirstOrDefault(), out var v) ? v : Guid.NewGuid();
    private static IResult Forbidden(HttpContext h) => Results.Json(new { ErrorCode = "PERMISSION_DENIED", CorrelationId = Correlation(h) }, statusCode: StatusCodes.Status403Forbidden);
    private static IResult Rule(string code, HttpContext h) => code.StartsWith("DUPLICATE_", StringComparison.Ordinal)
        ? Results.Conflict(new { ErrorCode = code, CorrelationId = Correlation(h) })
        : Results.UnprocessableEntity(new { ErrorCode = code, CorrelationId = Correlation(h) });
}
