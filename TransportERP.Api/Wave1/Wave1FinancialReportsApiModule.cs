using System.Security.Claims;
using TransportERP.Contracts.Wave1;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Wave1;

public static class Wave1FinancialReportsApiModule
{
    public static IEndpointRouteBuilder MapWave1FinancialReports(this IEndpointRouteBuilder app)
    {
        MapAging(app, "customer-aging", "ACC058", true);
        MapAging(app, "supplier-aging", "ACC059", false);
        MapBalanceSheet(app);
        MapCashFlow(app);
        return app;
    }

    private static void MapAging(IEndpointRouteBuilder app, string route, string permissionPrefix, bool customer)
    {
        var group = app.MapGroup($"/api/v1/accounting/reports/{route}").RequireAuthorization("Authenticated");
        group.MapPost("/query", async (AgingQueryRequest request, HttpContext h, Wave1FinancialReportService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, $"{permissionPrefix}.View")) return Forbidden("PERMISSION_DENIED", h);
            if (!TryScope(h.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", h);
            try { return Results.Ok(customer ? await service.QueryCustomerAgingAsync(companyId, branchId, request, ct) : await service.QuerySupplierAgingAsync(companyId, branchId, request, ct)); }
            catch (ArgumentException ex) { return Bad(ex.Message, h); }
        });
        group.MapPost("/drill-down", async (AgingDrillDownRequest request, HttpContext h, Wave1FinancialReportService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, $"{permissionPrefix}.DrillDown")) return Forbidden("PERMISSION_DENIED", h);
            if (!TryScope(h.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", h);
            try { return Results.Ok(customer ? await service.DrillCustomerAgingAsync(companyId, branchId, request, ct) : await service.DrillSupplierAgingAsync(companyId, branchId, request, ct)); }
            catch (ArgumentException ex) { return Bad(ex.Message, h); }
        });
        group.MapPost("/export", async (AgingQueryRequest request, HttpContext h, Wave1FinancialReportService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, $"{permissionPrefix}.Export")) return Forbidden("PERMISSION_DENIED", h);
            if (!TryScope(h.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", h);
            var report = customer ? await service.QueryCustomerAgingAsync(companyId, branchId, request, ct) : await service.QuerySupplierAgingAsync(companyId, branchId, request, ct);
            return Results.Ok(Wave1FinancialReportService.ExportAging(customer ? "customer" : "supplier", report));
        });
        group.MapPost("/print", async (AgingQueryRequest request, HttpContext h, Wave1FinancialReportService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, $"{permissionPrefix}.Print")) return Forbidden("PERMISSION_DENIED", h);
            if (!TryScope(h.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", h);
            var report = customer ? await service.QueryCustomerAgingAsync(companyId, branchId, request, ct) : await service.QuerySupplierAgingAsync(companyId, branchId, request, ct);
            return Results.Ok(Wave1FinancialReportService.Print(customer ? "أعمار الذمم المدينة" : "أعمار الذمم الدائنة", Wave1FinancialReportService.AgingHtml(report)));
        });
    }

    private static void MapBalanceSheet(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/accounting/reports/balance-sheet").RequireAuthorization("Authenticated");
        group.MapPost("/query", async (BalanceSheetQueryRequest request, HttpContext h, Wave1FinancialReportService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "ACC049.View")) return Forbidden("PERMISSION_DENIED", h);
            if (!TryScope(h.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", h);
            return Results.Ok(await service.QueryBalanceSheetAsync(companyId, branchId, request, ct));
        });
        group.MapPost("/drill-down", async (BalanceSheetDrillDownRequest request, HttpContext h, Wave1FinancialReportService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "ACC049.DrillDown")) return Forbidden("PERMISSION_DENIED", h);
            if (!TryScope(h.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", h);
            return Results.Ok(await service.DrillBalanceSheetAsync(companyId, branchId, request, ct));
        });
        group.MapPost("/export", async (BalanceSheetQueryRequest request, HttpContext h, Wave1FinancialReportService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "ACC049.Export")) return Forbidden("PERMISSION_DENIED", h);
            if (!TryScope(h.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", h);
            return Results.Ok(Wave1FinancialReportService.ExportBalanceSheet(await service.QueryBalanceSheetAsync(companyId, branchId, request, ct)));
        });
        group.MapPost("/print", async (BalanceSheetQueryRequest request, HttpContext h, Wave1FinancialReportService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "ACC049.Print")) return Forbidden("PERMISSION_DENIED", h);
            if (!TryScope(h.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", h);
            var report = await service.QueryBalanceSheetAsync(companyId, branchId, request, ct);
            return Results.Ok(Wave1FinancialReportService.Print("الميزانية العمومية", Wave1FinancialReportService.BalanceSheetHtml(report)));
        });
    }

    private static void MapCashFlow(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/accounting/reports/cash-flow").RequireAuthorization("Authenticated");
        group.MapPost("/query", async (CashFlowQueryRequest request, HttpContext h, Wave1FinancialReportService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "ACC050.View")) return Forbidden("PERMISSION_DENIED", h);
            if (!TryScope(h.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", h);
            try { return Results.Ok(await service.QueryCashFlowAsync(companyId, branchId, request, ct)); }
            catch (ArgumentException ex) { return Bad(ex.Message, h); }
        });
        group.MapPost("/drill-down", async (CashFlowDrillDownRequest request, HttpContext h, Wave1FinancialReportService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "ACC050.DrillDown")) return Forbidden("PERMISSION_DENIED", h);
            if (!TryScope(h.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", h);
            try { return Results.Ok(await service.DrillCashFlowAsync(companyId, branchId, request, ct)); }
            catch (ArgumentException ex) { return Bad(ex.Message, h); }
        });
        group.MapPost("/export", async (CashFlowQueryRequest request, HttpContext h, Wave1FinancialReportService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "ACC050.Export")) return Forbidden("PERMISSION_DENIED", h);
            if (!TryScope(h.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", h);
            return Results.Ok(Wave1FinancialReportService.ExportCashFlow(await service.QueryCashFlowAsync(companyId, branchId, request, ct)));
        });
        group.MapPost("/print", async (CashFlowQueryRequest request, HttpContext h, Wave1FinancialReportService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "ACC050.Print")) return Forbidden("PERMISSION_DENIED", h);
            if (!TryScope(h.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", h);
            var report = await service.QueryCashFlowAsync(companyId, branchId, request, ct);
            return Results.Ok(Wave1FinancialReportService.Print("التدفقات النقدية", Wave1FinancialReportService.CashFlowHtml(report)));
        });
    }

    private static bool TryScope(ClaimsPrincipal principal, Guid? requestedBranchId, out Guid companyId, out Guid? branchId)
    {
        companyId = default; branchId = null;
        if (!Guid.TryParse(principal.FindFirstValue("company_id"), out companyId)) return false;
        if (Guid.TryParse(principal.FindFirstValue("branch_id"), out var claimedBranch))
        {
            if (requestedBranchId.HasValue && requestedBranchId.Value != claimedBranch) return false;
            branchId = claimedBranch; return true;
        }
        branchId = requestedBranchId; return true;
    }
    private static bool HasPermission(ClaimsPrincipal p, string permission)
        => p.Claims.Any(x => x.Type is "permission" or ClaimTypes.Role && string.Equals(x.Value, permission, StringComparison.OrdinalIgnoreCase));
    private static Guid Correlation(HttpContext h) => Guid.TryParse(h.Request.Headers["X-Correlation-Id"].FirstOrDefault(), out var id) ? id : Guid.NewGuid();
    private static IResult Forbidden(string code, HttpContext h) => Results.Json(new { ErrorCode = code, CorrelationId = Correlation(h) }, statusCode: 403);
    private static IResult Bad(string message, HttpContext h) => Results.BadRequest(new { ErrorCode = "INVALID_FILTER", Message = message, CorrelationId = Correlation(h) });
}
