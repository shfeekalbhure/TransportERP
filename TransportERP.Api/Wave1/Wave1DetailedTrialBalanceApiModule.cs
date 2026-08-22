using System.Globalization;
using System.Security.Claims;
using System.Text;
using TransportERP.Contracts.Geo;
using TransportERP.Contracts.Wave1;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Wave1;

public static class Wave1DetailedTrialBalanceApiModule
{
    public static IEndpointRouteBuilder MapWave1DetailedTrialBalance(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/accounting/reports/detailed-trial-balance")
            .RequireAuthorization("Authenticated");

        group.MapPost("/query", async (ACC058QueryRequest request, HttpContext httpContext, Wave1DetailedTrialBalanceService service, CancellationToken cancellationToken) =>
        {
            if (!HasPermission(httpContext.User, "ACC058.View")) return Forbidden("PERMISSION_DENIED", httpContext);
            if (!TryResolveScope(httpContext.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", httpContext);
            try
            {
                var paging = Paging(request.Page, request.PageSize);
                var result = await service.QueryAsync(companyId, branchId,
                    new DetailedTrialBalanceQueryRequest(request.From, request.To, branchId, request.CurrencyId, request.AccountId, request.FinancialDimensionId, paging.Skip, paging.Take), cancellationToken);
                return Results.Ok(new PagedResponse<ACC058RowDto>(result.Items.Select(ToRow).ToArray(), request.Page, request.PageSize, result.Total));
            }
            catch (ArgumentException ex) { return Bad(ex, httpContext); }
        });

        group.MapPost("/drill-down", async (ACC058DrillDownRequest request, HttpContext httpContext, Wave1DetailedTrialBalanceService service, CancellationToken cancellationToken) =>
        {
            if (!HasPermission(httpContext.User, "ACC058.DrillDown")) return Forbidden("PERMISSION_DENIED", httpContext);
            if (!TryResolveScope(httpContext.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", httpContext);
            try
            {
                var paging = Paging(request.Page, request.PageSize);
                var result = await service.DrillDownAsync(companyId, branchId,
                    new DetailedTrialBalanceDrillDownRequest(request.AccountId, request.From, request.To, branchId, request.CurrencyId, request.FinancialDimensionId, paging.Skip, paging.Take), cancellationToken);
                return Results.Ok(new PagedResponse<ACC058DetailDto>(result.Items.Select(ToDetail).ToArray(), request.Page, request.PageSize, result.Total));
            }
            catch (ArgumentException ex) { return Bad(ex, httpContext); }
        });

        group.MapPost("/export", async (ACC058ExportRequest request, HttpContext httpContext, Wave1DetailedTrialBalanceService service, CancellationToken cancellationToken) =>
        {
            if (!HasPermission(httpContext.User, "ACC058.Export")) return Forbidden("PERMISSION_DENIED", httpContext);
            if (!TryResolveScope(httpContext.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", httpContext);
            try
            {
                var paging = Paging(request.Page, request.PageSize);
                var report = await service.QueryAsync(companyId, branchId,
                    new DetailedTrialBalanceQueryRequest(request.From, request.To, branchId, request.CurrencyId, request.AccountId, request.FinancialDimensionId, paging.Skip, paging.Take), cancellationToken);
                var sb = new StringBuilder("AccountCode,AccountName,OpeningDebit,OpeningCredit,PeriodDebit,PeriodCredit,ClosingDebit,ClosingCredit\n");
                foreach (var x in report.Items)
                    sb.AppendLine(string.Join(',', Csv(x.AccountCode), Csv(x.AccountNameAr), N(x.OpeningDebit), N(x.OpeningCredit), N(x.PeriodDebit), N(x.PeriodCredit), N(x.ClosingDebit), N(x.ClosingCredit)));
                return Results.Ok(new ExportJobOrFileResponse("detailed-trial-balance.csv", "text/csv; charset=utf-8", sb.ToString()));
            }
            catch (ArgumentException ex) { return Bad(ex, httpContext); }
        });

        group.MapPost("/print", async (ACC058PrintRequest request, HttpContext httpContext, Wave1DetailedTrialBalanceService service, CancellationToken cancellationToken) =>
        {
            if (!HasPermission(httpContext.User, "ACC058.Print")) return Forbidden("PERMISSION_DENIED", httpContext);
            if (!TryResolveScope(httpContext.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", httpContext);
            try
            {
                var paging = Paging(request.Page, request.PageSize);
                var report = await service.QueryAsync(companyId, branchId,
                    new DetailedTrialBalanceQueryRequest(request.From, request.To, branchId, request.CurrencyId, request.AccountId, request.FinancialDimensionId, paging.Skip, paging.Take), cancellationToken);
                var rows = string.Concat(report.Items.Select(x => $"<tr><td>{Html(x.AccountCode)}</td><td>{Html(x.AccountNameAr)}</td><td>{N(x.OpeningDebit)}</td><td>{N(x.OpeningCredit)}</td><td>{N(x.PeriodDebit)}</td><td>{N(x.PeriodCredit)}</td><td>{N(x.ClosingDebit)}</td><td>{N(x.ClosingCredit)}</td></tr>"));
                var html = $"<!doctype html><html dir=\"rtl\"><head><meta charset=\"utf-8\"><title>ميزان المراجعة التفصيلي</title></head><body><h1>ميزان المراجعة التفصيلي</h1><table><thead><tr><th>الحساب</th><th>الاسم</th><th>افتتاحي مدين</th><th>افتتاحي دائن</th><th>حركة مدين</th><th>حركة دائن</th><th>ختامي مدين</th><th>ختامي دائن</th></tr></thead><tbody>{rows}</tbody></table></body></html>";
                return Results.Ok(new PrintPayloadOrJobResponse("ميزان المراجعة التفصيلي", "text/html; charset=utf-8", html));
            }
            catch (ArgumentException ex) { return Bad(ex, httpContext); }
        });

        return app;
    }

    private static (int Skip, int Take) Paging(int page, int pageSize)
    {
        if (page < 1 || pageSize is < 1 or > 500) throw new ArgumentException("INVALID_PAGING");
        return ((page - 1) * pageSize, pageSize);
    }

    private static ACC058RowDto ToRow(DetailedTrialBalanceRow x) => new(x.AccountId, x.AccountCode, x.AccountNameAr, x.OpeningDebit, x.OpeningCredit, x.PeriodDebit, x.PeriodCredit, x.ClosingDebit, x.ClosingCredit);
    private static ACC058DetailDto ToDetail(DetailedTrialBalanceDrillDownRow x) => new(x.JournalEntryId, x.DocumentNo, x.EntryDate, x.LineNo, x.Description, x.Debit, x.Credit, x.CurrencyId, x.FinancialDimensionId);

    private static bool TryResolveScope(ClaimsPrincipal principal, Guid? requestedBranchId, out Guid companyId, out Guid? branchId)
    {
        companyId = default; branchId = null;
        if (!Guid.TryParse(principal.FindFirstValue("company_id"), out companyId)) return false;
        var branchClaim = principal.FindFirstValue("branch_id");
        if (Guid.TryParse(branchClaim, out var claimedBranchId))
        {
            if (requestedBranchId.HasValue && requestedBranchId.Value != claimedBranchId) return false;
            branchId = claimedBranchId; return true;
        }
        branchId = requestedBranchId; return true;
    }

    private static bool HasPermission(ClaimsPrincipal principal, string permission)
        => principal.Claims.Any(x => x.Type is "permission" or ClaimTypes.Role && string.Equals(x.Value, permission, StringComparison.OrdinalIgnoreCase));
    private static IResult Forbidden(string errorCode, HttpContext context)
        => Results.Json(new { ErrorCode = errorCode, CorrelationId = GetCorrelationId(context) }, statusCode: StatusCodes.Status403Forbidden);
    private static IResult Bad(ArgumentException ex, HttpContext context)
        => Results.BadRequest(new { ErrorCode = "INVALID_FILTER", Message = ex.Message, CorrelationId = GetCorrelationId(context) });
    private static Guid GetCorrelationId(HttpContext context)
        => Guid.TryParse(context.Request.Headers["X-Correlation-Id"].FirstOrDefault(), out var id) ? id : Guid.NewGuid();
    private static string N(decimal x) => x.ToString("0.####", CultureInfo.InvariantCulture);
    private static string Csv(string? x) => $"\"{(x ?? string.Empty).Replace("\"", "\"\"")}\"";
    private static string Html(string? x) => System.Net.WebUtility.HtmlEncode(x ?? string.Empty);
}
