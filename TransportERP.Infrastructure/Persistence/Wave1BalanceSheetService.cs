using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TransportERP.Contracts.Wave1;

namespace TransportERP.Infrastructure.Persistence;

public sealed class Wave1BalanceSheetService(TransportErpDbContext db)
{
    public async Task<BalanceSheetResponse> QueryAsync(
        Guid companyId,
        Guid? branchId,
        BalanceSheetQueryRequest request,
        CancellationToken cancellationToken = default)
    {
        var asOfExclusive = request.AsOf.Date.AddDays(1);
        var entries = db.JournalEntries.AsNoTracking()
            .Where(x => x.CompanyId == companyId && x.Status == "POSTED" && x.EntryDate < asOfExclusive);
        if (branchId.HasValue)
            entries = entries.Where(x => x.BranchId == branchId.Value);
        if (request.CurrencyId.HasValue)
            entries = entries.Where(x => x.CurrencyId == request.CurrencyId.Value);

        var raw = await (
            from entry in entries
            join line in db.JournalEntryLines.AsNoTracking() on entry.Id equals line.JournalEntryId
            join account in db.ChartOfAccounts.AsNoTracking() on line.AccountId equals account.Id
            where account.CompanyId == companyId
            select new { account.Id, account.Code, account.NameAr, account.AccountType, line.Debit, line.Credit })
            .ToListAsync(cancellationToken);

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

    public async Task<FinancialDrillDownResponse> DrillDownAsync(
        Guid companyId,
        Guid? branchId,
        BalanceSheetDrillDownRequest request,
        CancellationToken cancellationToken = default)
    {
        ValidatePage(request.Skip, request.Take);
        var asOfExclusive = request.AsOf.Date.AddDays(1);
        var entries = db.JournalEntries.AsNoTracking()
            .Where(x => x.CompanyId == companyId && x.Status == "POSTED" && x.EntryDate < asOfExclusive);
        if (branchId.HasValue)
            entries = entries.Where(x => x.BranchId == branchId.Value);
        if (request.CurrencyId.HasValue)
            entries = entries.Where(x => x.CurrencyId == request.CurrencyId.Value);

        var query =
            from entry in entries
            join line in db.JournalEntryLines.AsNoTracking().Where(x => x.AccountId == request.AccountId)
                on entry.Id equals line.JournalEntryId
            orderby entry.EntryDate, entry.DocumentNo, line.LineNo
            select new FinancialDrillDownRow(
                entry.Id, entry.DocumentNo, entry.EntryDate, line.LineNo, line.Description,
                line.Debit, line.Credit, line.CurrencyId);

        var total = await query.CountAsync(cancellationToken);
        var items = await query.Skip(request.Skip).Take(request.Take).ToListAsync(cancellationToken);
        return new FinancialDrillDownResponse(items, total, request.Skip, request.Take);
    }

    public static ReportExportResponse Export(BalanceSheetResponse report)
    {
        var sb = new StringBuilder("AccountCode,AccountName,AccountType,Balance\n");
        foreach (var x in report.Assets.Concat(report.Liabilities).Concat(report.Equity))
            sb.AppendLine(string.Join(',', Csv(x.AccountCode), Csv(x.AccountNameAr), x.AccountType, Number(x.Balance)));
        sb.AppendLine($"CURRENT_EARNINGS,,EQUITY,{Number(report.CurrentEarnings)}");
        return new ReportExportResponse("balance-sheet.csv", "text/csv; charset=utf-8", sb.ToString());
    }

    public static ReportPrintResponse Print(BalanceSheetResponse report)
    {
        var body = $"<p>الأصول: {Number(report.AssetsTotal)} | الالتزامات: {Number(report.LiabilitiesTotal)} | حقوق الملكية: {Number(report.EquityTotal)} | نتيجة الفترة: {Number(report.CurrentEarnings)} | الفرق: {Number(report.EquationDifference)}</p>";
        var html = $"<!doctype html><html dir=\"rtl\"><head><meta charset=\"utf-8\"><title>الميزانية العمومية</title></head><body><h1>الميزانية العمومية</h1>{body}</body></html>";
        return new ReportPrintResponse("الميزانية العمومية", "text/html; charset=utf-8", html);
    }

    private static void ValidatePage(int skip, int take)
    {
        if (skip < 0 || take is < 1 or > 200)
            throw new ArgumentOutOfRangeException(nameof(take), "Skip must be non-negative and Take must be between 1 and 200.");
    }

    private static string Number(decimal value) => value.ToString("0.####", CultureInfo.InvariantCulture);
    private static string Csv(string? value) => $"\"{(value ?? string.Empty).Replace("\"", "\"\"")}\"";
}
