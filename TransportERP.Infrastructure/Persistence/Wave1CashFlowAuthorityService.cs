using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TransportERP.Contracts.Wave1;

namespace TransportERP.Infrastructure.Persistence;

public sealed class Wave1CashFlowAuthorityService(Wave1AccountingAuthorityDbContext authority, TransportErpDbContext accounting)
{
    public async Task<PagedResponse<ACC050RowDto>> QueryAsync(Guid companyId, Guid? branchId, ACC050QueryRequest request, CancellationToken ct = default)
    {
        var rows = await BuildAsync(companyId, branchId, request.From, request.To, request.CurrencyId, request.SearchText, request.Sort, request.Direction, ct);
        ValidatePage(request.Page, request.PageSize);
        return new(rows.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToArray(), request.Page, request.PageSize, rows.Count);
    }

    public async Task<PagedResponse<ACC050DetailDto>> DrillAsync(Guid companyId, Guid? branchId, ACC050DrillDownRequest request, CancellationToken ct = default)
    {
        if (!Wave1CashFlowActivities.IsKnown(request.Activity?.Trim().ToUpperInvariant())) throw new ArgumentException("INVALID_ACTIVITY");
        var rows = (await BuildAsync(companyId, branchId, request.From, request.To, request.CurrencyId, null, request.Sort, request.Direction, ct))
            .Where(x => string.Equals(x.Activity, request.Activity, StringComparison.OrdinalIgnoreCase)).ToArray();
        ValidatePage(request.Page, request.PageSize);
        return new(rows.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .Select(x => new ACC050DetailDto(x.Activity, x.SourceType, x.DocumentNo, x.Date, x.CurrencyId, x.Inflow, x.Outflow, x.Net)).ToArray(), request.Page, request.PageSize, rows.Length);
    }

    public async Task<ExportJobOrFileResponse> ExportAsync(Guid companyId, Guid? branchId, ACC050ExportRequest request, CancellationToken ct = default)
    {
        var rows = await BuildAsync(companyId, branchId, request.From, request.To, request.CurrencyId, request.SearchText, request.Sort, request.Direction, ct);
        var sb = new StringBuilder("Activity,SourceType,DocumentNo,Date,CurrencyId,Inflow,Outflow,Net\n");
        foreach (var x in rows)
            sb.AppendLine($"{x.Activity},{x.SourceType},{Csv(x.DocumentNo)},{x.Date:yyyy-MM-dd},{x.CurrencyId},{N(x.Inflow)},{N(x.Outflow)},{N(x.Net)}");
        return new("cash-flow.csv", "text/csv; charset=utf-8", sb.ToString());
    }

    public async Task<PrintPayloadOrJobResponse> PrintAsync(Guid companyId, Guid? branchId, ACC050PrintRequest request, CancellationToken ct = default)
    {
        var rows = await BuildAsync(companyId, branchId, request.From, request.To, request.CurrencyId, request.SearchText, request.Sort, request.Direction, ct);
        decimal Net(string activity) => rows.Where(x => x.Activity == activity).Sum(x => x.Net);
        var html = $"<!doctype html><html dir=\"rtl\"><meta charset=\"utf-8\"><body><h1>قائمة التدفقات النقدية</h1><p>تشغيلي: {N(Net(Wave1CashFlowActivities.Operating))} | استثماري: {N(Net(Wave1CashFlowActivities.Investing))} | تمويلي: {N(Net(Wave1CashFlowActivities.Financing))} | غير مصنف: {N(Net(Wave1CashFlowActivities.Unclassified))}</p></body></html>";
        return new("قائمة التدفقات النقدية", "text/html; charset=utf-8", html);
    }

    private async Task<List<ACC050RowDto>> BuildAsync(Guid companyId, Guid? branchId, DateTime from, DateTime to, Guid? currencyId, string? searchText, string? sort, string? direction, CancellationToken ct)
    {
        if (to.Date < from.Date) throw new ArgumentException("INVALID_DATE_RANGE");
        var endExclusive = to.Date.AddDays(1);
        var receipts = accounting.ReceiptVouchers.AsNoTracking().Where(x => x.CompanyId == companyId && x.Status == "POSTED" && x.VoucherDate >= from.Date && x.VoucherDate < endExclusive);
        var payments = accounting.PaymentVouchers.AsNoTracking().Where(x => x.CompanyId == companyId && x.Status == "POSTED" && x.VoucherDate >= from.Date && x.VoucherDate < endExclusive);
        if (branchId.HasValue) { receipts = receipts.Where(x => x.BranchId == branchId.Value); payments = payments.Where(x => x.BranchId == branchId.Value); }
        if (currencyId.HasValue) { receipts = receipts.Where(x => x.CurrencyId == currencyId.Value); payments = payments.Where(x => x.CurrencyId == currencyId.Value); }

        var receiptRows = await receipts.ToListAsync(ct); var paymentRows = await payments.ToListAsync(ct);
        var movementIds = receiptRows.Select(x => x.Id).Concat(paymentRows.Select(x => x.Id)).Distinct().ToArray();
        var overrides = await authority.CashFlowMovementOverrides.AsNoTracking()
            .Where(x => x.CompanyId == companyId && x.IsActive && movementIds.Contains(x.MovementId)).ToListAsync(ct);
        var journalEntries = await accounting.JournalEntries.AsNoTracking()
            .Where(x => x.CompanyId == companyId && x.Status == "POSTED" && x.SourceId.HasValue && movementIds.Contains(x.SourceId.Value)).ToListAsync(ct);
        var entryIds = journalEntries.Select(x => x.Id).ToArray();
        var journalLines = await accounting.JournalEntryLines.AsNoTracking().Where(x => entryIds.Contains(x.JournalEntryId)).ToListAsync(ct);
        var accountIds = journalLines.Select(x => x.AccountId).Distinct().ToArray();
        var mappings = await authority.CashFlowAccountMappings.AsNoTracking()
            .Where(x => x.CompanyId == companyId && x.IsActive && accountIds.Contains(x.AccountId))
            .ToDictionaryAsync(x => x.AccountId, x => x.Activity, ct);

        string Classify(string movementType, Guid movementId)
        {
            var activeOverride = overrides.SingleOrDefault(x => x.MovementType == movementType && x.MovementId == movementId);
            if (activeOverride is not null) return activeOverride.Activity;
            var linkedEntryIds = journalEntries.Where(x => x.SourceId == movementId).Select(x => x.Id).ToHashSet();
            var activities = journalLines.Where(x => linkedEntryIds.Contains(x.JournalEntryId) && mappings.ContainsKey(x.AccountId))
                .Select(x => mappings[x.AccountId]).Distinct(StringComparer.Ordinal).ToArray();
            return activities.Length == 1 ? activities[0] : Wave1CashFlowActivities.Unclassified;
        }

        var rows = receiptRows.Select(x => new ACC050RowDto(Classify("RECEIPT_VOUCHER", x.Id), "RECEIPT_VOUCHER", x.VoucherNo, x.VoucherDate, x.CurrencyId, x.Amount, 0m, x.Amount))
            .Concat(paymentRows.Select(x => new ACC050RowDto(Classify("PAYMENT_VOUCHER", x.Id), "PAYMENT_VOUCHER", x.VoucherNo, x.VoucherDate, x.CurrencyId, 0m, x.Amount, -x.Amount))).ToList();

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            var term = searchText.Trim();
            rows = rows.Where(x => x.DocumentNo.Contains(term, StringComparison.OrdinalIgnoreCase)
                || x.SourceType.Contains(term, StringComparison.OrdinalIgnoreCase)
                || x.Activity.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        var descending = string.Equals(direction, "desc", StringComparison.OrdinalIgnoreCase);
        return (sort?.Trim().ToLowerInvariant(), descending) switch
        {
            ("activity", false) => rows.OrderBy(x => x.Activity, StringComparer.Ordinal).ThenBy(x => x.Date).ToList(),
            ("activity", true) => rows.OrderByDescending(x => x.Activity, StringComparer.Ordinal).ThenByDescending(x => x.Date).ToList(),
            ("documentno", false) => rows.OrderBy(x => x.DocumentNo, StringComparer.Ordinal).ToList(),
            ("documentno", true) => rows.OrderByDescending(x => x.DocumentNo, StringComparer.Ordinal).ToList(),
            (_, true) => rows.OrderByDescending(x => x.Date).ThenByDescending(x => x.DocumentNo, StringComparer.Ordinal).ToList(),
            _ => rows.OrderBy(x => x.Date).ThenBy(x => x.DocumentNo, StringComparer.Ordinal).ToList()
        };
    }

    private static void ValidatePage(int page, int pageSize) { if (page < 1 || pageSize is < 1 or > 200) throw new ArgumentOutOfRangeException(nameof(pageSize)); }
    private static string Csv(string value) => $"\"{value.Replace("\"", "\"\"")}\"";
    private static string N(decimal value) => value.ToString("0.######", CultureInfo.InvariantCulture);
}
