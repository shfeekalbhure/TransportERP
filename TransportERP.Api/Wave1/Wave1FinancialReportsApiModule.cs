using System.Security.Claims;
using TransportERP.Contracts.Geo;
using TransportERP.Contracts.Wave1;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Wave1;

public static class Wave1FinancialReportsApiModule
{
    public static IEndpointRouteBuilder MapWave1FinancialReports(this IEndpointRouteBuilder app)
    {
        MapCustomerAging(app);
        MapSupplierAging(app);
        MapBalanceSheet(app);
        MapCashFlow(app);
        return app;
    }

    private static void MapCustomerAging(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/accounting/reports/customer-aging").RequireAuthorization("Authenticated");
        group.MapPost("/query", async (ACC074QueryRequest request, HttpContext h, Wave1FinancialReportService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "ACC074.View")) return Forbidden("PERMISSION_DENIED", h);
            if (!TryScope(h.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", h);
            try
            {
                var paging = Paging(request.Page, request.PageSize);
                var report = await service.QueryCustomerAgingAsync(companyId, branchId,
                    new AgingQueryRequest(request.AsOf, branchId, request.CurrencyId, request.PartyId, paging.Skip, paging.Take), ct);
                var rows = report.Items.Select(ToACC074).ToArray();
                return Results.Ok(new PagedResponse<ACC074RowDto>(rows, request.Page, request.PageSize, report.Total));
            }
            catch (ArgumentException ex) { return Bad(ex.Message, h); }
        });
        group.MapPost("/drill-down", async (ACC074DrillDownRequest request, HttpContext h, Wave1FinancialReportService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "ACC074.DrillDown")) return Forbidden("PERMISSION_DENIED", h);
            if (!TryScope(h.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", h);
            try
            {
                var paging = Paging(request.Page, request.PageSize);
                var report = await service.DrillCustomerAgingAsync(companyId, branchId,
                    new AgingDrillDownRequest(request.PartyId, request.AsOf, branchId, request.CurrencyId, paging.Skip, paging.Take), ct);
                return Results.Ok(new PagedResponse<ACC074DetailDto>(report.Items.Select(ToACC074Detail).ToArray(), request.Page, request.PageSize, report.Total));
            }
            catch (ArgumentException ex) { return Bad(ex.Message, h); }
        });
        group.MapPost("/export", async (ACC074ExportRequest request, HttpContext h, Wave1FinancialReportService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "ACC074.Export")) return Forbidden("PERMISSION_DENIED", h);
            if (!TryScope(h.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", h);
            try
            {
                var paging = Paging(request.Page, request.PageSize);
                var report = await service.QueryCustomerAgingAsync(companyId, branchId,
                    new AgingQueryRequest(request.AsOf, branchId, request.CurrencyId, request.PartyId, paging.Skip, paging.Take), ct);
                return Results.Ok(ToExport(Wave1FinancialReportService.ExportAging("customer", report)));
            }
            catch (ArgumentException ex) { return Bad(ex.Message, h); }
        });
        group.MapPost("/print", async (ACC074PrintRequest request, HttpContext h, Wave1FinancialReportService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "ACC074.Print")) return Forbidden("PERMISSION_DENIED", h);
            if (!TryScope(h.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", h);
            try
            {
                var paging = Paging(request.Page, request.PageSize);
                var report = await service.QueryCustomerAgingAsync(companyId, branchId,
                    new AgingQueryRequest(request.AsOf, branchId, request.CurrencyId, request.PartyId, paging.Skip, paging.Take), ct);
                return Results.Ok(ToPrint(Wave1FinancialReportService.Print("أعمار الذمم المدينة", Wave1FinancialReportService.AgingHtml(report))));
            }
            catch (ArgumentException ex) { return Bad(ex.Message, h); }
        });
    }

    private static void MapSupplierAging(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/accounting/reports/supplier-aging").RequireAuthorization("Authenticated");
        group.MapPost("/query", async (ACC075QueryRequest request, HttpContext h, Wave1FinancialReportService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "ACC075.View")) return Forbidden("PERMISSION_DENIED", h);
            if (!TryScope(h.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", h);
            try
            {
                var paging = Paging(request.Page, request.PageSize);
                var report = await service.QuerySupplierAgingAsync(companyId, branchId,
                    new AgingQueryRequest(request.AsOf, branchId, request.CurrencyId, request.PartyId, paging.Skip, paging.Take), ct);
                return Results.Ok(new PagedResponse<ACC075RowDto>(report.Items.Select(ToACC075).ToArray(), request.Page, request.PageSize, report.Total));
            }
            catch (ArgumentException ex) { return Bad(ex.Message, h); }
        });
        group.MapPost("/drill-down", async (ACC075DrillDownRequest request, HttpContext h, Wave1FinancialReportService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "ACC075.DrillDown")) return Forbidden("PERMISSION_DENIED", h);
            if (!TryScope(h.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", h);
            try
            {
                var paging = Paging(request.Page, request.PageSize);
                var report = await service.DrillSupplierAgingAsync(companyId, branchId,
                    new AgingDrillDownRequest(request.PartyId, request.AsOf, branchId, request.CurrencyId, paging.Skip, paging.Take), ct);
                return Results.Ok(new PagedResponse<ACC075DetailDto>(report.Items.Select(ToACC075Detail).ToArray(), request.Page, request.PageSize, report.Total));
            }
            catch (ArgumentException ex) { return Bad(ex.Message, h); }
        });
        group.MapPost("/export", async (ACC075ExportRequest request, HttpContext h, Wave1FinancialReportService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "ACC075.Export")) return Forbidden("PERMISSION_DENIED", h);
            if (!TryScope(h.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", h);
            try
            {
                var paging = Paging(request.Page, request.PageSize);
                var report = await service.QuerySupplierAgingAsync(companyId, branchId,
                    new AgingQueryRequest(request.AsOf, branchId, request.CurrencyId, request.PartyId, paging.Skip, paging.Take), ct);
                return Results.Ok(ToExport(Wave1FinancialReportService.ExportAging("supplier", report)));
            }
            catch (ArgumentException ex) { return Bad(ex.Message, h); }
        });
        group.MapPost("/print", async (ACC075PrintRequest request, HttpContext h, Wave1FinancialReportService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "ACC075.Print")) return Forbidden("PERMISSION_DENIED", h);
            if (!TryScope(h.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", h);
            try
            {
                var paging = Paging(request.Page, request.PageSize);
                var report = await service.QuerySupplierAgingAsync(companyId, branchId,
                    new AgingQueryRequest(request.AsOf, branchId, request.CurrencyId, request.PartyId, paging.Skip, paging.Take), ct);
                return Results.Ok(ToPrint(Wave1FinancialReportService.Print("أعمار الذمم الدائنة", Wave1FinancialReportService.AgingHtml(report))));
            }
            catch (ArgumentException ex) { return Bad(ex.Message, h); }
        });
    }

    private static void MapBalanceSheet(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/accounting/reports/balance-sheet").RequireAuthorization("Authenticated");
        group.MapPost("/query", async (ACC049QueryRequest request, HttpContext h, Wave1FinancialReportService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "ACC049.View")) return Forbidden("PERMISSION_DENIED", h);
            if (!TryScope(h.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", h);
            try
            {
                var paging = Paging(request.Page, request.PageSize);
                var report = await service.QueryBalanceSheetAsync(companyId, branchId, new BalanceSheetQueryRequest(request.AsOf, branchId, request.CurrencyId), ct);
                var rows = report.Assets.Concat(report.Liabilities).Concat(report.Equity).Select(ToACC049).ToList();
                var page = rows.Skip(paging.Skip).Take(paging.Take).ToArray();
                return Results.Ok(new PagedResponse<ACC049RowDto>(page, request.Page, request.PageSize, rows.Count));
            }
            catch (ArgumentException ex) { return Bad(ex.Message, h); }
        });
        group.MapPost("/drill-down", async (ACC049DrillDownRequest request, HttpContext h, Wave1FinancialReportService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "ACC049.DrillDown")) return Forbidden("PERMISSION_DENIED", h);
            if (!TryScope(h.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", h);
            try
            {
                var paging = Paging(request.Page, request.PageSize);
                var report = await service.DrillBalanceSheetAsync(companyId, branchId,
                    new BalanceSheetDrillDownRequest(request.AccountId, request.AsOf, branchId, request.CurrencyId, paging.Skip, paging.Take), ct);
                return Results.Ok(new PagedResponse<ACC049DetailDto>(report.Items.Select(ToACC049Detail).ToArray(), request.Page, request.PageSize, report.Total));
            }
            catch (ArgumentException ex) { return Bad(ex.Message, h); }
        });
        group.MapPost("/export", async (ACC049ExportRequest request, HttpContext h, Wave1FinancialReportService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "ACC049.Export")) return Forbidden("PERMISSION_DENIED", h);
            if (!TryScope(h.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", h);
            var report = await service.QueryBalanceSheetAsync(companyId, branchId, new BalanceSheetQueryRequest(request.AsOf, branchId, request.CurrencyId), ct);
            return Results.Ok(ToExport(Wave1FinancialReportService.ExportBalanceSheet(report)));
        });
        group.MapPost("/print", async (ACC049PrintRequest request, HttpContext h, Wave1FinancialReportService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "ACC049.Print")) return Forbidden("PERMISSION_DENIED", h);
            if (!TryScope(h.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", h);
            var report = await service.QueryBalanceSheetAsync(companyId, branchId, new BalanceSheetQueryRequest(request.AsOf, branchId, request.CurrencyId), ct);
            return Results.Ok(ToPrint(Wave1FinancialReportService.Print("الميزانية العمومية", Wave1FinancialReportService.BalanceSheetHtml(report))));
        });
    }

    private static void MapCashFlow(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/accounting/reports/cash-flow").RequireAuthorization("Authenticated");
        group.MapPost("/query", async (ACC050QueryRequest request, HttpContext h, Wave1FinancialReportService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "ACC050.View")) return Forbidden("PERMISSION_DENIED", h);
            if (!TryScope(h.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", h);
            try
            {
                var paging = Paging(request.Page, request.PageSize);
                var report = await service.QueryCashFlowAsync(companyId, branchId, new CashFlowQueryRequest(request.From, request.To, branchId, request.CurrencyId), ct);
                var page = report.Items.Skip(paging.Skip).Take(paging.Take).Select(ToACC050).ToArray();
                return Results.Ok(new PagedResponse<ACC050RowDto>(page, request.Page, request.PageSize, report.Items.Count));
            }
            catch (ArgumentException ex) { return Bad(ex.Message, h); }
        });
        group.MapPost("/drill-down", async (ACC050DrillDownRequest request, HttpContext h, Wave1FinancialReportService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "ACC050.DrillDown")) return Forbidden("PERMISSION_DENIED", h);
            if (!TryScope(h.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", h);
            try
            {
                var paging = Paging(request.Page, request.PageSize);
                var full = await service.QueryCashFlowAsync(companyId, branchId, new CashFlowQueryRequest(request.From, request.To, branchId, request.CurrencyId), ct);
                var filtered = full.Items.Where(x => string.Equals(x.Activity, request.Activity, StringComparison.OrdinalIgnoreCase)).ToList();
                return Results.Ok(new PagedResponse<ACC050DetailDto>(filtered.Skip(paging.Skip).Take(paging.Take).Select(ToACC050Detail).ToArray(), request.Page, request.PageSize, filtered.Count));
            }
            catch (ArgumentException ex) { return Bad(ex.Message, h); }
        });
        group.MapPost("/export", async (ACC050ExportRequest request, HttpContext h, Wave1FinancialReportService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "ACC050.Export")) return Forbidden("PERMISSION_DENIED", h);
            if (!TryScope(h.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", h);
            try { return Results.Ok(ToExport(Wave1FinancialReportService.ExportCashFlow(await service.QueryCashFlowAsync(companyId, branchId, new CashFlowQueryRequest(request.From, request.To, branchId, request.CurrencyId), ct)))); }
            catch (ArgumentException ex) { return Bad(ex.Message, h); }
        });
        group.MapPost("/print", async (ACC050PrintRequest request, HttpContext h, Wave1FinancialReportService service, CancellationToken ct) =>
        {
            if (!HasPermission(h.User, "ACC050.Print")) return Forbidden("PERMISSION_DENIED", h);
            if (!TryScope(h.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", h);
            try
            {
                var report = await service.QueryCashFlowAsync(companyId, branchId, new CashFlowQueryRequest(request.From, request.To, branchId, request.CurrencyId), ct);
                return Results.Ok(ToPrint(Wave1FinancialReportService.Print("التدفقات النقدية", Wave1FinancialReportService.CashFlowHtml(report))));
            }
            catch (ArgumentException ex) { return Bad(ex.Message, h); }
        });
    }

    private static (int Skip, int Take) Paging(int page, int pageSize)
    {
        if (page < 1 || pageSize is < 1 or > 500) throw new ArgumentException("INVALID_PAGING");
        return ((page - 1) * pageSize, pageSize);
    }

    private static ACC074RowDto ToACC074(AgingRow x) => new(x.PartyId, x.PartyCode, x.PartyName, x.CurrencyId, x.Current, x.Days1To30, x.Days31To60, x.Days61To90, x.Over90, x.TotalOutstanding);
    private static ACC075RowDto ToACC075(AgingRow x) => new(x.PartyId, x.PartyCode, x.PartyName, x.CurrencyId, x.Current, x.Days1To30, x.Days31To60, x.Days61To90, x.Over90, x.TotalOutstanding);
    private static ACC074DetailDto ToACC074Detail(AgingOpenItemRow x) => new(x.Id, x.DocumentNo, x.SourceType, x.DocumentDate, x.DueDate, x.CurrencyId, x.OriginalAmount, x.SettledAmount, x.OutstandingAmount, x.AgeDays);
    private static ACC075DetailDto ToACC075Detail(AgingOpenItemRow x) => new(x.Id, x.DocumentNo, x.SourceType, x.DocumentDate, x.DueDate, x.CurrencyId, x.OriginalAmount, x.SettledAmount, x.OutstandingAmount, x.AgeDays);
    private static ACC049RowDto ToACC049(BalanceSheetLine x) => new(x.AccountId, x.AccountCode, x.AccountNameAr, x.AccountType, x.Balance);
    private static ACC049DetailDto ToACC049Detail(FinancialDrillDownRow x) => new(x.JournalEntryId, x.DocumentNo, x.EntryDate, x.LineNo, x.Description, x.Debit, x.Credit, x.CurrencyId);
    private static ACC050RowDto ToACC050(CashFlowLine x) => new(x.Activity, x.SourceType, x.DocumentNo, x.Date, x.CurrencyId, x.Inflow, x.Outflow, x.Net);
    private static ACC050DetailDto ToACC050Detail(CashFlowLine x) => new(x.Activity, x.SourceType, x.DocumentNo, x.Date, x.CurrencyId, x.Inflow, x.Outflow, x.Net);
    private static ExportJobOrFileResponse ToExport(ReportExportResponse x) => new(x.FileName, x.ContentType, x.Content);
    private static PrintPayloadOrJobResponse ToPrint(ReportPrintResponse x) => new(x.Title, x.ContentType, x.Content);

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
