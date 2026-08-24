using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Geo;
using TransportERP.Contracts.Wave1;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Wave1;

public static class Wave1AccountingAuthorityApiModule
{
    public static IEndpointRouteBuilder MapWave1AuthorizedAccounting(this IEndpointRouteBuilder app)
    {
        MapACC036(app); MapACC074(app); MapACC075(app); MapACC050(app); return app;
    }

    private static void MapACC036(IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/api/v1/accounting/account-classifications").RequireAuthorization("Authenticated");
        g.MapGet("", async ([AsParameters] PagedQueryRequest q, HttpContext h, Wave1AccountClassificationAuthorityService s, CancellationToken ct) =>
        {
            if (!Has(h, "ACC036.View")) return Denied(h); if (!Context(h, out var c)) return ScopeDenied(h);
            try { return Results.Ok(await s.ListAsync(c, q, ct)); }
            catch (Exception ex) when (ex is ArgumentException or ArgumentOutOfRangeException) { return Bad(ex.Message, h); }
        });
        g.MapGet("/{id:guid}", async (Guid id, HttpContext h, Wave1AccountClassificationAuthorityService s, CancellationToken ct) =>
        {
            if (!Has(h, "ACC036.View")) return Denied(h); if (!Context(h, out var c)) return ScopeDenied(h);
            var row = await s.GetAsync(c, id, ct); return row is null ? NotFound(h) : Results.Ok(row);
        });
        g.MapPost("", async (CreateACC036Request r, HttpContext h, Wave1AccountClassificationAuthorityService s, CancellationToken ct) =>
        {
            if (!Has(h, "ACC036.Create")) return Denied(h); if (!Context(h, out var c)) return ScopeDenied(h);
            try { return Results.Ok(await s.CreateAsync(c, r, ct)); } catch (ArgumentException ex) { return Rule(ex.Message, h); }
        });
        g.MapPut("/{id:guid}", async (Guid id, UpdateACC036Request r, HttpContext h, Wave1AccountClassificationAuthorityService s, CancellationToken ct) =>
        {
            if (!Has(h, "ACC036.Edit")) return Denied(h); if (!Context(h, out var c)) return ScopeDenied(h);
            try { var row = await s.UpdateAsync(c, id, r, ct); return row is null ? NotFound(h) : Results.Ok(row); }
            catch (DbUpdateConcurrencyException) { return Conflict("CONCURRENCY_CONFLICT", h); }
            catch (ArgumentException ex) { return Rule(ex.Message, h); }
        });
        g.MapPost("/{id:guid}/disable", async (Guid id, DisableReferenceRequest r, HttpContext h, Wave1AccountClassificationAuthorityService s, CancellationToken ct) =>
        {
            if (!Has(h, "ACC036.Disable")) return Denied(h); if (!Context(h, out var c)) return ScopeDenied(h);
            try { var row = await s.DisableAsync(c, id, r, ct); return row is null ? NotFound(h) : Results.Ok(row); }
            catch (DbUpdateConcurrencyException) { return Conflict("CONCURRENCY_CONFLICT", h); }
            catch (ArgumentException ex) { return Rule(ex.Message, h); }
        });
    }

    private static void MapACC074(IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/api/v1/accounting/reports/customer-aging").RequireAuthorization("Authenticated");
        g.MapPost("/query", (ACC074QueryRequest r, HttpContext h, Wave1AgingAuthorityService s, CancellationToken ct) => Report(h, "ACC074.View", r.BranchId, (c,b) => s.QueryCustomerAsync(c,b,r,ct)));
        g.MapPost("/drill-down", (ACC074DrillDownRequest r, HttpContext h, Wave1AgingAuthorityService s, CancellationToken ct) => Report(h, "ACC074.DrillDown", r.BranchId, (c,b) => s.DrillCustomerAsync(c,b,r,ct)));
        g.MapPost("/export", (ACC074ExportRequest r, HttpContext h, Wave1AgingAuthorityService s, Wave1DeliveryAuditWriter a, CancellationToken ct) => Report(h, "ACC074.Export", r.BranchId, (c,b) => s.ExportCustomerAsync(c,b,r,ct), a, "ACC-074", r, ct));
        g.MapPost("/print", (ACC074PrintRequest r, HttpContext h, Wave1AgingAuthorityService s, CancellationToken ct) => Report(h, "ACC074.Print", r.BranchId, (c,b) => s.PrintCustomerAsync(c,b,r,ct)));
    }

    private static void MapACC075(IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/api/v1/accounting/reports/supplier-aging").RequireAuthorization("Authenticated");
        g.MapPost("/query", (ACC075QueryRequest r, HttpContext h, Wave1AgingAuthorityService s, CancellationToken ct) => Report(h, "ACC075.View", r.BranchId, (c,b) => s.QuerySupplierAsync(c,b,r,ct)));
        g.MapPost("/drill-down", (ACC075DrillDownRequest r, HttpContext h, Wave1AgingAuthorityService s, CancellationToken ct) => Report(h, "ACC075.DrillDown", r.BranchId, (c,b) => s.DrillSupplierAsync(c,b,r,ct)));
        g.MapPost("/export", (ACC075ExportRequest r, HttpContext h, Wave1AgingAuthorityService s, Wave1DeliveryAuditWriter a, CancellationToken ct) => Report(h, "ACC075.Export", r.BranchId, (c,b) => s.ExportSupplierAsync(c,b,r,ct), a, "ACC-075", r, ct));
        g.MapPost("/print", (ACC075PrintRequest r, HttpContext h, Wave1AgingAuthorityService s, CancellationToken ct) => Report(h, "ACC075.Print", r.BranchId, (c,b) => s.PrintSupplierAsync(c,b,r,ct)));
    }

    private static void MapACC050(IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/api/v1/accounting/reports/cash-flow").RequireAuthorization("Authenticated");
        g.MapPost("/query", (ACC050QueryRequest r, HttpContext h, Wave1CashFlowAuthorityService s, CancellationToken ct) => Report(h, "ACC050.View", r.BranchId, (c,b) => s.QueryAsync(c,b,r,ct)));
        g.MapPost("/drill-down", (ACC050DrillDownRequest r, HttpContext h, Wave1CashFlowAuthorityService s, CancellationToken ct) => Report(h, "ACC050.DrillDown", r.BranchId, (c,b) => s.DrillAsync(c,b,r,ct)));
        g.MapPost("/export", (ACC050ExportRequest r, HttpContext h, Wave1CashFlowAuthorityService s, Wave1DeliveryAuditWriter a, CancellationToken ct) => Report(h, "ACC050.Export", r.BranchId, (c,b) => s.ExportAsync(c,b,r,ct), a, "ACC-050", r, ct));
        g.MapPost("/print", (ACC050PrintRequest r, HttpContext h, Wave1CashFlowAuthorityService s, CancellationToken ct) => Report(h, "ACC050.Print", r.BranchId, (c,b) => s.PrintAsync(c,b,r,ct)));
    }

    private static async Task<IResult> Report<T>(
        HttpContext h,
        string permission,
        Guid? requestedBranch,
        Func<Guid, Guid?, Task<T>> action,
        Wave1DeliveryAuditWriter? audit = null,
        string? screenId = null,
        object? filters = null,
        CancellationToken ct = default)
    {
        if (!Has(h, permission)) return Denied(h);
        if (!ReportContext(h, requestedBranch, out var companyId, out var branchId)) return ScopeDenied(h);
        try
        {
            var result = await action(companyId, branchId);
            if (audit is not null)
            {
                var correlation = Correlation(h);
                await audit.AppendSuccessAsync(screenId ?? throw new InvalidOperationException("AUDIT_SCREEN_REQUIRED"), "Export", filters ?? new { },
                    DeliveryContext(h, companyId, branchId, correlation), ct);
            }
            return Results.Ok(result);
        }
        catch (Exception ex) when (ex is ArgumentException or ArgumentOutOfRangeException or InvalidOperationException) { return Bad(ex.Message, h); }
    }

    private static bool Context(HttpContext h, out OperationContext context)
    {
        context = default!; var user = GuidClaim(h.User, ClaimTypes.NameIdentifier) ?? GuidClaim(h.User, "sub");
        var company = GuidClaim(h.User, "company_id"); var branch = GuidClaim(h.User, "branch_id");
        if (!user.HasValue || !company.HasValue || !branch.HasValue) return false;
        context = new(user.Value, company.Value, branch.Value, Correlation(h)); return true;
    }
    private static bool ReportContext(HttpContext h, Guid? requestedBranch, out Guid companyId, out Guid? branchId)
    {
        companyId = Guid.Empty; branchId = null; var company = GuidClaim(h.User, "company_id"); var claimBranch = GuidClaim(h.User, "branch_id");
        if (!company.HasValue || !claimBranch.HasValue) return false;
        if (requestedBranch.HasValue && requestedBranch.Value != claimBranch.Value) return false;
        companyId = company.Value; branchId = requestedBranch ?? claimBranch.Value; return true;
    }
    private static Wave1DeliveryAuditContext DeliveryContext(HttpContext h, Guid companyId, Guid? branchId, Guid correlation)
        => new(GuidClaim(h.User, ClaimTypes.NameIdentifier) ?? GuidClaim(h.User, "sub"), companyId, branchId, correlation, h.User.FindFirstValue("device_id"), h.Connection.RemoteIpAddress?.ToString());
    private static Guid? GuidClaim(ClaimsPrincipal p, string type) => Guid.TryParse(p.FindFirstValue(type), out var v) ? v : null;
    private static bool Has(HttpContext h, string permission) => h.User.Claims.Any(x => (x.Type == "permission" || x.Type == ClaimTypes.Role) && string.Equals(x.Value, permission, StringComparison.OrdinalIgnoreCase));
    private static Guid Correlation(HttpContext h) => Guid.TryParse(h.Request.Headers["X-Correlation-Id"].FirstOrDefault(), out var v) ? v : Guid.NewGuid();
    private static IResult Denied(HttpContext h) => Results.Json(new { ErrorCode = "PERMISSION_DENIED", CorrelationId = Correlation(h) }, statusCode: 403);
    private static IResult ScopeDenied(HttpContext h) => Results.Json(new { ErrorCode = "SCOPE_DENIED", CorrelationId = Correlation(h) }, statusCode: 403);
    private static IResult NotFound(HttpContext h) => Results.NotFound(new { ErrorCode = "NOT_FOUND", CorrelationId = Correlation(h) });
    private static IResult Conflict(string code, HttpContext h) => Results.Conflict(new { ErrorCode = code, CorrelationId = Correlation(h) });
    private static IResult Bad(string code, HttpContext h) => Results.BadRequest(new { ErrorCode = code, CorrelationId = Correlation(h) });
    private static IResult Rule(string code, HttpContext h) => code.StartsWith("DUPLICATE_", StringComparison.Ordinal) ? Conflict(code, h) : Results.UnprocessableEntity(new { ErrorCode = code, CorrelationId = Correlation(h) });
}
