using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TransportERP.Contracts.Wave1;

namespace TransportERP.Infrastructure.Persistence;

public sealed class Wave1DetailedTrialBalanceService(TransportErpDbContext db)
{
    public async Task<DetailedTrialBalanceResponse> QueryAsync(
        Guid companyId,
        Guid? branchId,
        DetailedTrialBalanceQueryRequest request,
        CancellationToken cancellationToken = default)
    {
        ValidateRange(request.From, request.To, request.Skip, request.Take);

        var from = request.From.Date;
        var toExclusive = request.To.Date.AddDays(1);

        var entries = db.JournalEntries.AsNoTracking()
            .Where(x => x.CompanyId == companyId && x.Status == "POSTED" && x.EntryDate < toExclusive);
        if (branchId.HasValue)
            entries = entries.Where(x => x.BranchId == branchId.Value);
        if (request.CurrencyId.HasValue)
            entries = entries.Where(x => x.CurrencyId == request.CurrencyId.Value);

        var lines = db.JournalEntryLines.AsNoTracking();
        if (request.AccountId.HasValue)
            lines = lines.Where(x => x.AccountId == request.AccountId.Value);
        if (request.FinancialDimensionId.HasValue)
            lines = lines.Where(x => x.FinancialDimensionId == request.FinancialDimensionId.Value);

        var query =
            from entry in entries
            join line in lines on entry.Id equals line.JournalEntryId
            join account in db.ChartOfAccounts.AsNoTracking() on line.AccountId equals account.Id
            where account.CompanyId == companyId
            select new
            {
                entry.EntryDate,
                line.AccountId,
                account.Code,
                account.NameAr,
                line.Debit,
                line.Credit
            };

        var opening = await query
            .Where(x => x.EntryDate < from)
            .GroupBy(x => new { x.AccountId, x.Code, x.NameAr })
            .Select(g => new Aggregate(
                g.Key.AccountId,
                g.Key.Code,
                g.Key.NameAr,
                g.Sum(x => x.Debit),
                g.Sum(x => x.Credit)))
            .ToListAsync(cancellationToken);

        var period = await query
            .Where(x => x.EntryDate >= from && x.EntryDate < toExclusive)
            .GroupBy(x => new { x.AccountId, x.Code, x.NameAr })
            .Select(g => new Aggregate(
                g.Key.AccountId,
                g.Key.Code,
                g.Key.NameAr,
                g.Sum(x => x.Debit),
                g.Sum(x => x.Credit)))
            .ToListAsync(cancellationToken);

        var openingByAccount = opening.ToDictionary(x => x.AccountId);
        var periodByAccount = period.ToDictionary(x => x.AccountId);
        var accountIds = openingByAccount.Keys.Union(periodByAccount.Keys).ToArray();

        var rows = accountIds
            .Select(accountId => BuildRow(accountId, openingByAccount, periodByAccount))
            .OrderBy(x => x.AccountCode, StringComparer.Ordinal)
            .ToList();

        var page = rows.Skip(request.Skip).Take(request.Take).ToList();

        return new DetailedTrialBalanceResponse(
            page,
            rows.Count,
            rows.Sum(x => x.OpeningDebit),
            rows.Sum(x => x.OpeningCredit),
            rows.Sum(x => x.PeriodDebit),
            rows.Sum(x => x.PeriodCredit),
            rows.Sum(x => x.ClosingDebit),
            rows.Sum(x => x.ClosingCredit),
            request.Skip,
            request.Take);
    }

    public async Task<DetailedTrialBalanceDrillDownResponse> DrillDownAsync(
        Guid companyId,
        Guid? branchId,
        DetailedTrialBalanceDrillDownRequest request,
        CancellationToken cancellationToken = default)
    {
        ValidateRange(request.From, request.To, request.Skip, request.Take);

        var from = request.From.Date;
        var toExclusive = request.To.Date.AddDays(1);

        var entries = db.JournalEntries.AsNoTracking()
            .Where(x => x.CompanyId == companyId
                        && x.Status == "POSTED"
                        && x.EntryDate >= from
                        && x.EntryDate < toExclusive);
        if (branchId.HasValue)
            entries = entries.Where(x => x.BranchId == branchId.Value);
        if (request.CurrencyId.HasValue)
            entries = entries.Where(x => x.CurrencyId == request.CurrencyId.Value);

        var lines = db.JournalEntryLines.AsNoTracking()
            .Where(x => x.AccountId == request.AccountId);
        if (request.FinancialDimensionId.HasValue)
            lines = lines.Where(x => x.FinancialDimensionId == request.FinancialDimensionId.Value);

        var query =
            from entry in entries
            join line in lines on entry.Id equals line.JournalEntryId
            orderby entry.EntryDate, entry.DocumentNo, line.LineNo
            select new DetailedTrialBalanceDrillDownRow(
                entry.Id,
                entry.DocumentNo,
                entry.EntryDate,
                line.LineNo,
                line.Description,
                line.Debit,
                line.Credit,
                line.CurrencyId,
                line.FinancialDimensionId);

        var total = await query.CountAsync(cancellationToken);
        var page = await query.Skip(request.Skip).Take(request.Take).ToListAsync(cancellationToken);
        return new DetailedTrialBalanceDrillDownResponse(page, total, request.Skip, request.Take);
    }

    public static ReportExportResponse Export(DetailedTrialBalanceResponse report)
    {
        var sb = new StringBuilder("AccountCode,AccountName,OpeningDebit,OpeningCredit,PeriodDebit,PeriodCredit,ClosingDebit,ClosingCredit\n");
        foreach (var x in report.Items)
            sb.AppendLine(string.Join(',', Csv(x.AccountCode), Csv(x.AccountNameAr), Number(x.OpeningDebit), Number(x.OpeningCredit), Number(x.PeriodDebit), Number(x.PeriodCredit), Number(x.ClosingDebit), Number(x.ClosingCredit)));
        return new ReportExportResponse("detailed-trial-balance.csv", "text/csv; charset=utf-8", sb.ToString());
    }

    public static ReportPrintResponse Print(DetailedTrialBalanceResponse report)
    {
        var rows = string.Concat(report.Items.Select(x => $"<tr><td>{Html(x.AccountCode)}</td><td>{Html(x.AccountNameAr)}</td><td>{Number(x.OpeningDebit)}</td><td>{Number(x.OpeningCredit)}</td><td>{Number(x.PeriodDebit)}</td><td>{Number(x.PeriodCredit)}</td><td>{Number(x.ClosingDebit)}</td><td>{Number(x.ClosingCredit)}</td></tr>"));
        var html = $"<!doctype html><html dir=\"rtl\"><head><meta charset=\"utf-8\"><title>ميزان المراجعة التفصيلي</title></head><body><h1>ميزان المراجعة التفصيلي</h1><table><thead><tr><th>الحساب</th><th>الاسم</th><th>افتتاحي مدين</th><th>افتتاحي دائن</th><th>حركة مدين</th><th>حركة دائن</th><th>ختامي مدين</th><th>ختامي دائن</th></tr></thead><tbody>{rows}</tbody></table></body></html>";
        return new ReportPrintResponse("ميزان المراجعة التفصيلي", "text/html; charset=utf-8", html);
    }

    private static DetailedTrialBalanceRow BuildRow(
        Guid accountId,
        IReadOnlyDictionary<Guid, Aggregate> opening,
        IReadOnlyDictionary<Guid, Aggregate> period)
    {
        opening.TryGetValue(accountId, out var openingValue);
        period.TryGetValue(accountId, out var periodValue);
        var identity = openingValue ?? periodValue!;

        var openingNet = (openingValue?.Debit ?? 0m) - (openingValue?.Credit ?? 0m);
        var periodDebit = periodValue?.Debit ?? 0m;
        var periodCredit = periodValue?.Credit ?? 0m;
        var closingNet = openingNet + periodDebit - periodCredit;

        return new DetailedTrialBalanceRow(
            accountId,
            identity.Code,
            identity.NameAr,
            openingNet >= 0m ? openingNet : 0m,
            openingNet < 0m ? -openingNet : 0m,
            periodDebit,
            periodCredit,
            closingNet >= 0m ? closingNet : 0m,
            closingNet < 0m ? -closingNet : 0m);
    }

    private static void ValidateRange(DateTime from, DateTime to, int skip, int take)
    {
        if (to.Date < from.Date)
            throw new ArgumentException("To date must be on or after From date.");
        if (skip < 0 || take is < 1 or > 200)
            throw new ArgumentOutOfRangeException(nameof(take), "Skip must be non-negative and Take must be between 1 and 200.");
    }

    private static string Number(decimal value) => value.ToString("0.####", CultureInfo.InvariantCulture);
    private static string Csv(string? value) => $"\"{(value ?? string.Empty).Replace("\"", "\"\"")}\"";
    private static string Html(string? value) => System.Net.WebUtility.HtmlEncode(value ?? string.Empty);

    private sealed record Aggregate(Guid AccountId, string Code, string NameAr, decimal Debit, decimal Credit);
}
