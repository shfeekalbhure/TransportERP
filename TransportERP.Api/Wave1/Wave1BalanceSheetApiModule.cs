using System.Security.Claims;
using TransportERP.Contracts.Geo;
using TransportERP.Contracts.Wave1;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Wave1;

public static class Wave1BalanceSheetApiModule
{
    public static IEndpointRouteBuilder MapWave1BalanceSheet(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/accounting/reports/balance-sheet")
            .RequireAuthorization("Authenticated");

        group.MapPost("/query", async (ACC049QueryRequest request, HttpContext context, Wave1BalanceSheetService service, CancellationToken cancellationToken) =>
        {
            if (!HasPermission(context.User, "ACC049.View")) return Forbidden("PERMISSION_DENIED", context);
            if (!TryResolveScope(context.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", context);
            try
            {
                var paging = Paging(request.Page, request.PageSize);
                var report = await service.QueryAsync(companyId, branchId, new BalanceSheetQueryRequest(request.AsOf, branchId, request.CurrencyId), cancellationToken);
                var rows = report.Assets.Concat(report.Liabilities).Concat(report.Equity).Select(ToRow).ToList();
                return Results.Ok(new PagedResponse<ACC049RowDto>(rows.Skip(paging.Skip).Take(paging.Take).ToArray(), request.Page, request.PageSize, rows.Count));
            }
            catch (ArgumentException ex) { return Bad(ex, context); }
        });

        group.MapPost("/drill-down", async (ACC049DrillDownRequest request, HttpContext context, Wave1BalanceSheetService service, CancellationToken cancellationToken) =>
        {
            if (!HasPermission(context.User, "ACC049.DrillDown")) return Forbidden("PERMISSION_DENIED", context);
            if (!TryResolveScope(context.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", context);
            try
            {
                var paging = Paging(request.Page, request.PageSize);
                var report = await service.DrillDownAsync(companyId, branchId,
                    new BalanceSheetDrillDownRequest(request.AccountId, request.AsOf, branchId, request.CurrencyId, paging.Skip, paging.Take), cancellationToken);
                return Results.Ok(new PagedResponse<ACC049DetailDto>(report.Items.Select(ToDetail).ToArray(), request.Page, request.PageSize, report.Total));
            }
            catch (ArgumentException ex) { return Bad(ex, context); }
        });

        group.MapPost("/export", async (ACC049ExportRequest request, HttpContext context, Wave1BalanceSheetService service, CancellationToken cancellationToken) =>
        {
            if (!HasPermission(context.User, "ACC049.Export")) return Forbidden("PERMISSION_DENIED", context);
            if (!TryResolveScope(context.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", context);
            try
            {
                _ = Paging(request.Page, request.PageSize);
                var report = await service.QueryAsync(companyId, branchId, new BalanceSheetQueryRequest(request.AsOf, branchId, request.CurrencyId), cancellationToken);
                var export = Wave1BalanceSheetService.Export(report);
                return Results.Ok(new ExportJobOrFileResponse(export.FileName, export.ContentType, export.Content));
            }
            catch (ArgumentException ex) { return Bad(ex, context); }
        });

        group.MapPost("/print", async (ACC049PrintRequest request, HttpContext context, Wave1BalanceSheetService service, CancellationToken cancellationToken) =>
        {
            if (!HasPermission(context.User, "ACC049.Print")) return Forbidden("PERMISSION_DENIED", context);
            if (!TryResolveScope(context.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", context);
            try
            {
                _ = Paging(request.Page, request.PageSize);
                var report = await service.QueryAsync(companyId, branchId, new BalanceSheetQueryRequest(request.AsOf, branchId, request.CurrencyId), cancellationToken);
                var print = Wave1BalanceSheetService.Print(report);
                return Results.Ok(new PrintPayloadOrJobResponse(print.Title, print.ContentType, print.Content));
            }
            catch (ArgumentException ex) { return Bad(ex, context); }
        });

        return app;
    }

    private static (int Skip, int Take) Paging(int page, int pageSize)
    {
        if (page < 1 || pageSize is < 1 or > 200) throw new ArgumentException("INVALID_PAGING");
        return ((page - 1) * pageSize, pageSize);
    }

    private static ACC049RowDto ToRow(BalanceSheetLine x) => new(x.AccountId, x.AccountCode, x.AccountNameAr, x.AccountType, x.Balance);
    private static ACC049DetailDto ToDetail(FinancialDrillDownRow x) => new(x.JournalEntryId, x.DocumentNo, x.EntryDate, x.LineNo, x.Description, x.Debit, x.Credit, x.CurrencyId);

    private static bool TryResolveScope(ClaimsPrincipal principal, Guid? requestedBranchId, out Guid companyId, out Guid? branchId)
    {
        companyId = default;
        branchId = null;
        if (!Guid.TryParse(principal.FindFirstValue("company_id"), out companyId)) return false;
        if (Guid.TryParse(principal.FindFirstValue("branch_id"), out var claimedBranchId))
        {
            if (requestedBranchId.HasValue && requestedBranchId.Value != claimedBranchId) return false;
            branchId = claimedBranchId;
            return true;
        }
        branchId = requestedBranchId;
        return true;
    }

    private static bool HasPermission(ClaimsPrincipal principal, string permission)
        => principal.Claims.Any(x => x.Type is "permission" or ClaimTypes.Role && string.Equals(x.Value, permission, StringComparison.OrdinalIgnoreCase));

    private static IResult Forbidden(string errorCode, HttpContext context)
        => Results.Json(new { ErrorCode = errorCode, CorrelationId = Correlation(context) }, statusCode: StatusCodes.Status403Forbidden);

    private static IResult Bad(ArgumentException ex, HttpContext context)
        => Results.BadRequest(new { ErrorCode = "INVALID_FILTER", Message = ex.Message, CorrelationId = Correlation(context) });

    private static Guid Correlation(HttpContext context)
        => Guid.TryParse(context.Request.Headers["X-Correlation-Id"].FirstOrDefault(), out var id) ? id : Guid.NewGuid();
}
