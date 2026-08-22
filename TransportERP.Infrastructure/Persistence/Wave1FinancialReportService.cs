using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TransportERP.Contracts.Wave1;

namespace TransportERP.Infrastructure.Persistence;

public sealed class Wave1FinancialReportService(
    TransportErpDbContext accounting,
    Wave1ReferenceDbContext reference)
{
    public Task<AgingResponse> QueryCustomerAgingAsync(Guid companyId, Guid? branchId, AgingQueryRequest request, CancellationToken ct = default)
        => QueryAgingAsync(companyId, branchId, request, "RECEIVABLE", ct);

    public Task<AgingResponse> QuerySupplierAgingAsync(Guid companyId, Guid? branchId, AgingQueryRequest request, CancellationToken ct = default)
        => QueryAgingAsync(companyId, branchId, request, "PAYABLE", ct);

    public Task<AgingDrillDownResponse> DrillCustomerAgingAsync(Guid companyId, Guid? branchId, AgingDrillDownRequest request, CancellationToken ct = default)
        => DrillAgingAsync(companyId, branchId, request, "RECEIVABLE", ct);

    public Task<AgingDrillDownResponse> DrillSupplierAgingAsync(Guid companyId, Guid? branchId, AgingDrillDownRequest request, CancellationToken ct = default)
        => DrillAgingAsync(companyId, branchId, request, "PAYABLE", ct);

    public async Task<BalanceSheetResponse> QueryBalanceSheetAsync(
        Guid companyId,
        Guid? branchId,
        BalanceSheetQueryRequest request,
        CancellationToken ct = default)
    {
        var asOfExclusive = request.AsOf.Date.AddDays(1);
        var entries = accounting.JournalEntries.AsNoTracking()
            .Where(x => x.CompanyId == companyId && x.Status == "POSTED" && x.EntryDate < asOfExclusive);
        if (branchId.HasValue) entries = entries.Where(x => x.BranchId == branchId.Value);
        if (request.CurrencyId.HasValue) entries = entries.Where(x => x.CurrencyId == request.CurrencyId.Value);

        var raw = await (
            from entry in entries
            join line in accounting.JournalEntryLines.AsNoTracking() on entry.Id equals line.JournalEntryId
            join account in accounting.ChartOfAccounts.AsNoTracking() on line.AccountId equals account.Id
            where account.CompanyId == companyId
            select new { account.Id, account.Code, account.NameAr, account.AccountType, line.Debit, line.Credit })
            .ToListAsync(ct);

        var grouped = raw.GroupBy(x => new { x.Id, x.Code, x.NameAr, x.AccountType })
            .Select(g => new
            {
                g.Key.Id,
                g.Key.Code,
                g.Key.NameAr,
                Type = g.Key.AccountType,
                Debit = g.Sum(x => x.Debit),
                Credit = g.Sum(x => x.Credit)
            })
            .OrderBy(x => x.Code, StringComparer.Ordinal)
            .ToList();

        var assets = grouped.Where(x => x.Type == "ASSET")
            .Select(x => new BalanceSheetLine(x.Id, x.Code, x.NameAr, x.Type, x.Debit - x.Credit)).ToList();
        var liabilities = grouped.Where(x => x.Type == "LIABILITY")
            .Select(x => new BalanceSheetLine(x.Id, x.Code, x.NameAr, x.Type, x.Credit - x.Debit)).ToList();
        var equity = grouped.Where(x => x.Type == "EQUITY")
            .Select(x => new BalanceSheetLine(x.Id, x.Code, x.NameAr, x.Type, x.Credit - x.Debit)).ToList();

        var revenue = grouped.Where(x => x.Type == "REVENUE").Sum(x => x.Credit - x.Debit);
        var expense = grouped.Where(x => x.Type == "EXPENSE").Sum(x => x.Debit - x.Credit);
        var currentEarnings = revenue - expense;
        var assetsTotal = assets.Sum(x => x.Balance);
        var liabilitiesTotal = liabilities.Sum(x => x.Balance);
        var equityTotal = equity.Sum(x => x.Balance);
        var difference = assetsTotal - (liabilitiesTotal + equityTotal + currentEarnings);

        return new BalanceSheetResponse(
            assets, liabilities, equity,
            assetsTotal, liabilitiesTotal, equityTotal, currentEarnings, difference);
    }

    public async Task<FinancialDrillDownResponse> DrillBalanceSheetAsync(
        Guid companyId,
        Guid? branchId,
        BalanceSheetDrillDownRequest request,
        CancellationToken ct = default)
    {
        ValidatePage(request.Skip, request.Take);
        var asOfExclusive = request.AsOf.Date.AddDays(1);
        var entries = accounting.JournalEntries.AsNoTracking()
            .Where(x => x.CompanyId == companyId && x.Status == "POSTED" && x.EntryDate < asOfExclusive);
        if (branchId.HasValue) entries = entries.Where(x => x.BranchId == branchId.Value);
        if (request.CurrencyId.HasValue) entries = entries.Where(x => x.CurrencyId == request.CurrencyId.Value);

        var query =
            from entry in entries
            join line in accounting.JournalEntryLines.AsNoTracking().Where(x => x.AccountId == request.AccountId)
                on entry.Id equals line.JournalEntryId
            orderby entry.EntryDate, entry.DocumentNo, line.LineNo
            select new FinancialDrillDownRow(
                entry.Id, entry.DocumentNo, entry.EntryDate, line.LineNo, line.Description,
                line.Debit, line.Credit, line.CurrencyId);

        var total = await query.CountAsync(ct);
        var items = await query.Skip(request.Skip).Take(request.Take).ToListAsync(ct);
        return new FinancialDrillDownResponse(items, total, request.Skip, request.Take);
    }

    public async Task<CashFlowResponse> QueryCashFlowAsync(
        Guid companyId,
        Guid? branchId,
        CashFlowQueryRequest request,
        CancellationToken ct = default)
    {
        if (request.To.Date < request.From.Date) throw new ArgumentException("INVALID_DATE_RANGE");
        var from = request.From.Date;
        var toExclusive = request.To.Date.AddDays(1);

        var receipts = accounting.ReceiptVouchers.AsNoTracking()
            .Where(x => x.CompanyId == companyId && x.Status == "POSTED" && x.VoucherDate >= from && x.VoucherDate < toExclusive);
        var payments = accounting.PaymentVouchers.AsNoTracking()
            .Where(x => x.CompanyId == companyId && x.Status == "POSTED" && x.VoucherDate >= from && x.VoucherDate < toExclusive);
        if (branchId.HasValue)
        {
            receipts = receipts.Where(x => x.BranchId == branchId.Value);
            payments = payments.Where(x => x.BranchId == branchId.Value);
        }
        if (request.CurrencyId.HasValue)
        {
            receipts = receipts.Where(x => x.CurrencyId == request.CurrencyId.Value);
            payments = payments.Where(x => x.CurrencyId == request.CurrencyId.Value);
        }

        var receiptRows = await receipts.Select(x => new
        {
            x.ReferenceType, x.VoucherNo, Date = x.VoucherDate, x.CurrencyId, x.Amount
        }).ToListAsync(ct);
        var paymentRows = await payments.Select(x => new
        {
            x.ReferenceType, x.VoucherNo, Date = x.VoucherDate, x.CurrencyId, x.Amount
        }).ToListAsync(ct);

        var items = receiptRows.Select(x => new CashFlowLine(
                ClassifyActivity(x.ReferenceType), x.ReferenceType, x.VoucherNo, x.Date, x.CurrencyId, x.Amount, 0m, x.Amount))
            .Concat(paymentRows.Select(x => new CashFlowLine(
                ClassifyActivity(x.ReferenceType), x.ReferenceType, x.VoucherNo, x.Date, x.CurrencyId, 0m, x.Amount, -x.Amount)))
            .OrderBy(x => x.Date).ThenBy(x => x.DocumentNo, StringComparer.Ordinal)
            .ToList();

        decimal Net(string activity) => items.Where(x => x.Activity == activity).Sum(x => x.Net);
        return new CashFlowResponse(
            items,
            Net("OPERATING"), Net("INVESTING"), Net("FINANCING"), Net("UNCLASSIFIED"), items.Sum(x => x.Net));
    }

    public async Task<CashFlowResponse> DrillCashFlowAsync(
        Guid companyId,
        Guid? branchId,
        CashFlowDrillDownRequest request,
        CancellationToken ct = default)
    {
        ValidatePage(request.Skip, request.Take);
        var full = await QueryCashFlowAsync(companyId, branchId,
            new CashFlowQueryRequest(request.From, request.To, request.BranchId, request.CurrencyId), ct);
        var filtered = full.Items.Where(x => string.Equals(x.Activity, request.Activity, StringComparison.OrdinalIgnoreCase)).ToList();
        var page = filtered.Skip(request.Skip).Take(request.Take).ToList();
        decimal Net(string activity) => page.Where(x => x.Activity == activity).Sum(x => x.Net);
        return new CashFlowResponse(page, Net("OPERATING"), Net("INVESTING"), Net("FINANCING"), Net("UNCLASSIFIED"), page.Sum(x => x.Net));
    }

    public static ReportExportResponse ExportAging(string side, AgingResponse report)
    {
        var sb = new StringBuilder("PartyCode,PartyName,CurrencyId,Current,Days1To30,Days31To60,Days61To90,Over90,Total\n");
        foreach (var x in report.Items)
            sb.AppendLine(string.Join(',', Csv(x.PartyCode), Csv(x.PartyName), x.CurrencyId,
                N(x.Current), N(x.Days1To30), N(x.Days31To60), N(x.Days61To90), N(x.Over90), N(x.TotalOutstanding)));
        return new ReportExportResponse($"{side.ToLowerInvariant()}-aging.csv", "text/csv; charset=utf-8", sb.ToString());
    }

    public static ReportExportResponse ExportBalanceSheet(BalanceSheetResponse report)
    {
        var sb = new StringBuilder("AccountCode,AccountName,AccountType,Balance\n");
        foreach (var x in report.Assets.Concat(report.Liabilities).Concat(report.Equity))
            sb.AppendLine(string.Join(',', Csv(x.AccountCode), Csv(x.AccountNameAr), x.AccountType, N(x.Balance)));
        sb.AppendLine($"CURRENT_EARNINGS,,EQUITY,{N(report.CurrentEarnings)}");
        return new ReportExportResponse("balance-sheet.csv", "text/csv; charset=utf-8", sb.ToString());
    }

    public static ReportExportResponse ExportCashFlow(CashFlowResponse report)
    {
        var sb = new StringBuilder("Activity,SourceType,DocumentNo,Date,CurrencyId,Inflow,Outflow,Net\n");
        foreach (var x in report.Items)
            sb.AppendLine(string.Join(',', x.Activity, Csv(x.SourceType), Csv(x.DocumentNo), x.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), x.CurrencyId, N(x.Inflow), N(x.Outflow), N(x.Net)));
        return new ReportExportResponse("cash-flow.csv", "text/csv; charset=utf-8", sb.ToString());
    }

    public static ReportPrintResponse Print(string title, string body)
        => new(title, "text/html; charset=utf-8", $"<!doctype html><html dir=\"rtl\"><head><meta charset=\"utf-8\"><title>{Html(title)}</title></head><body><h1>{Html(title)}</h1>{body}</body></html>");

    public static string AgingHtml(AgingResponse report)
        => "<table><thead><tr><th>الطرف</th><th>الحالي</th><th>1-30</th><th>31-60</th><th>61-90</th><th>أكثر من 90</th><th>الإجمالي</th></tr></thead><tbody>" +
           string.Concat(report.Items.Select(x => $"<tr><td>{Html(x.PartyName)}</td><td>{N(x.Current)}</td><td>{N(x.Days1To30)}</td><td>{N(x.Days31To60)}</td><td>{N(x.Days61To90)}</td><td>{N(x.Over90)}</td><td>{N(x.TotalOutstanding)}</td></tr>")) + "</tbody></table>";

    public static string BalanceSheetHtml(BalanceSheetResponse report)
        => $"<p>الأصول: {N(report.AssetsTotal)} | الالتزامات: {N(report.LiabilitiesTotal)} | حقوق الملكية: {N(report.EquityTotal)} | نتيجة الفترة: {N(report.CurrentEarnings)} | الفرق: {N(report.EquationDifference)}</p>";

    public static string CashFlowHtml(CashFlowResponse report)
        => $"<p>تشغيلي: {N(report.OperatingNet)} | استثماري: {N(report.InvestingNet)} | تمويلي: {N(report.FinancingNet)} | غير مصنف: {N(report.UnclassifiedNet)} | صافي الحركة: {N(report.NetCashMovement)}</p>";

    private async Task<AgingResponse> QueryAgingAsync(Guid companyId, Guid? branchId, AgingQueryRequest request, string side, CancellationToken ct)
    {
        ValidatePage(request.Skip, request.Take);
        var asOf = request.AsOf.Date;
        var q = reference.OpenItems.AsNoTracking()
            .Where(x => x.CompanyId == companyId && x.Side == side && x.Status == "OPEN" && x.DocumentDate <= asOf && x.OriginalAmount > x.SettledAmount);
        if (branchId.HasValue) q = q.Where(x => x.BranchId == branchId.Value);
        if (request.CurrencyId.HasValue) q = q.Where(x => x.CurrencyId == request.CurrencyId.Value);
        if (request.PartyId.HasValue) q = q.Where(x => x.PartyId == request.PartyId.Value);
        var raw = await q.ToListAsync(ct);

        var rows = raw.GroupBy(x => new { x.PartyId, x.PartyCode, x.PartyName, x.CurrencyId })
            .Select(g =>
            {
                decimal Bucket(Func<int, bool> predicate) => g.Where(x => predicate(Age(asOf, x.DueDate))).Sum(x => x.OriginalAmount - x.SettledAmount);
                var current = g.Where(x => x.DueDate.Date > asOf).Sum(x => x.OriginalAmount - x.SettledAmount);
                var b1 = Bucket(a => a is >= 0 and <= 30);
                var b2 = Bucket(a => a is >= 31 and <= 60);
                var b3 = Bucket(a => a is >= 61 and <= 90);
                var b4 = Bucket(a => a > 90);
                return new AgingRow(g.Key.PartyId, g.Key.PartyCode, g.Key.PartyName, g.Key.CurrencyId, current, b1, b2, b3, b4, current + b1 + b2 + b3 + b4);
            })
            .OrderBy(x => x.PartyCode, StringComparer.Ordinal)
            .ToList();
        var page = rows.Skip(request.Skip).Take(request.Take).ToList();
        return new AgingResponse(page, rows.Count, rows.Sum(x => x.TotalOutstanding), request.Skip, request.Take);
    }

    private async Task<AgingDrillDownResponse> DrillAgingAsync(Guid companyId, Guid? branchId, AgingDrillDownRequest request, string side, CancellationToken ct)
    {
        ValidatePage(request.Skip, request.Take);
        var asOf = request.AsOf.Date;
        var q = reference.OpenItems.AsNoTracking()
            .Where(x => x.CompanyId == companyId && x.Side == side && x.PartyId == request.PartyId && x.Status == "OPEN" && x.DocumentDate <= asOf && x.OriginalAmount > x.SettledAmount);
        if (branchId.HasValue) q = q.Where(x => x.BranchId == branchId.Value);
        if (request.CurrencyId.HasValue) q = q.Where(x => x.CurrencyId == request.CurrencyId.Value);
        var total = await q.CountAsync(ct);
        var raw = await q.OrderBy(x => x.DueDate).ThenBy(x => x.DocumentNo).Skip(request.Skip).Take(request.Take).ToListAsync(ct);
        var rows = raw.Select(x => new AgingOpenItemRow(x.Id, x.DocumentNo, x.SourceType, x.DocumentDate, x.DueDate, x.CurrencyId, x.OriginalAmount, x.SettledAmount, x.OriginalAmount - x.SettledAmount, Age(asOf, x.DueDate))).ToList();
        return new AgingDrillDownResponse(rows, total, request.Skip, request.Take);
    }

    private static int Age(DateTime asOf, DateTime due) => Math.Max(0, (asOf - due.Date).Days);
    private static void ValidatePage(int skip, int take) { if (skip < 0 || take is < 1 or > 500) throw new ArgumentOutOfRangeException(nameof(take)); }
    private static string ClassifyActivity(string? referenceType)
    {
        var x = (referenceType ?? string.Empty).Trim().ToUpperInvariant();
        if (x.Contains("ASSET") || x.Contains("CAPEX") || x.Contains("INVEST")) return "INVESTING";
        if (x.Contains("LOAN") || x.Contains("CAPITAL") || x.Contains("EQUITY") || x.Contains("BORROW") || x.Contains("FINANC")) return "FINANCING";
        if (x.Contains("CUSTOMER") || x.Contains("COLLECTION") || x.Contains("SALES") || x.Contains("SUPPLIER") || x.Contains("EXPENSE") || x.Contains("PURCHASE") || x.Contains("PAYROLL") || x.Contains("OPERAT")) return "OPERATING";
        return "UNCLASSIFIED";
    }
    private static string N(decimal x) => x.ToString("0.####", CultureInfo.InvariantCulture);
    private static string Csv(string? x) => $"\"{(x ?? string.Empty).Replace("\"", "\"\"")}\"";
    private static string Html(string? x) => System.Net.WebUtility.HtmlEncode(x ?? string.Empty);
}
