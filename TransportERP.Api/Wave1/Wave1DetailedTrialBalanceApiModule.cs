using System.Globalization;
using System.Security.Claims;
using System.Text;
using TransportERP.Contracts.Wave1;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Wave1;

public static class Wave1DetailedTrialBalanceApiModule
{
    public static IEndpointRouteBuilder MapWave1DetailedTrialBalance(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/accounting/reports/detailed-trial-balance")
            .RequireAuthorization("Authenticated");

        group.MapPost("/query", async (DetailedTrialBalanceQueryRequest request, HttpContext httpContext, Wave1DetailedTrialBalanceService service, CancellationToken cancellationToken) =>
        {
            if (!HasPermission(httpContext.User, "ACC058.View")) return Forbidden("PERMISSION_DENIED", httpContext);
            if (!TryResolveScope(httpContext.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", httpContext);
            try { return Results.Ok(await service.QueryAsync(companyId, branchId, request, cancellationToken)); }
            catch (ArgumentException ex) { return Bad(ex, httpContext); }
        });

        group.MapPost("/drill-down", async (DetailedTrialBalanceDrillDownRequest request, HttpContext httpContext, Wave1DetailedTrialBalanceService service, CancellationToken cancellationToken) =>
        {
            if (!HasPermission(httpContext.User, "ACC058.DrillDown")) return Forbidden("PERMISSION_DENIED", httpContext);
            if (!TryResolveScope(httpContext.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", httpContext);
            try { return Results.Ok(await service.DrillDownAsync(companyId, branchId, request, cancellationToken)); }
            catch (ArgumentException ex) { return Bad(ex, httpContext); }
        });

        group.MapPost("/export", async (DetailedTrialBalanceQueryRequest request, HttpContext httpContext, Wave1DetailedTrialBalanceService service, CancellationToken cancellationToken) =>
        {
            if (!HasPermission(httpContext.User, "ACC058.Export")) return Forbidden("PERMISSION_DENIED", httpContext);
            if (!TryResolveScope(httpContext.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", httpContext);
            var report = await service.QueryAsync(companyId, branchId, request, cancellationToken);
            var sb = new StringBuilder("AccountCode,AccountName,CurrencyId,Opening,Debit,Credit,Closing\n");
            foreach (var x in report.Items)
                sb.AppendLine(string.Join(',', Csv(x.AccountCode), Csv(x.AccountNameAr), x.CurrencyId, N(x.OpeningBalance), N(x.PeriodDebit), N(x.PeriodCredit), N(x.ClosingBalance)));
            return Results.Ok(new ReportExportResponse("detailed-trial-balance.csv", "text/csv; charset=utf-8", sb.ToString()));
        });

        group.MapPost("/print", async (DetailedTrialBalanceQueryRequest request, HttpContext httpContext, Wave1DetailedTrialBalanceService service, CancellationToken cancellationToken) =>
        {
            if (!HasPermission(httpContext.User, "ACC058.Print")) return Forbidden("PERMISSION_DENIED", httpContext);
            if (!TryResolveScope(httpContext.User, request.BranchId, out var companyId, out var branchId)) return Forbidden("SCOPE_DENIED", httpContext);
            var report = await service.QueryAsync(companyId, branchId, request, cancellationToken);
            var rows = string.Concat(report.Items.Select(x => $"<tr><td>{Html(x.AccountCode)}</td><td>{Html(x.AccountNameAr)}</td><td>{N(x.OpeningBalance)}</td><td>{N(x.PeriodDebit)}</td><td>{N(x.PeriodCredit)}</td><td>{N(x.ClosingBalance)}</td></tr>"));
            var html = $"<!doctype html><html dir=\"rtl\"><head><meta charset=\"utf-8\"><title>ميزان المراجعة التفصيلي</title></head><body><h1>ميزان المراجعة التفصيلي</h1><table><thead><tr><th>الحساب</th><th>الاسم</th><th>افتتاحي</th><th>مدين</th><th>دائن</th><th>ختامي</th></tr></thead><tbody>{rows}</tbody></table></body></html>";
            return Results.Ok(new ReportPrintResponse("ميزان المراجعة التفصيلي", "text/html; charset=utf-8", html));
        });

        return app;
    }

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
