using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TransportERP.Contracts.Wave1;

namespace TransportERP.Infrastructure.Persistence;

public sealed class Wave1AgingAuthorityService(Wave1AccountingAuthorityDbContext authority, TransportErpDbContext accounting)
{
    private sealed record SourceDocument(string Number, DateTime Date);
    private sealed record ComputedItem(Wave1OpenItemRecord Item, string PartyCode, string PartyName, decimal Outstanding, SourceDocument Source);
    private sealed record AgingAggregate(Guid PartyId, string Code, string Name, Guid CurrencyId, decimal Current, decimal B1, decimal B2, decimal B3, decimal B4, decimal Total);

    public async Task<PagedResponse<ACC074RowDto>> QueryCustomerAsync(Guid companyId, Guid? branchId, ACC074QueryRequest request, CancellationToken ct = default)
    {
        var result = await BuildAsync(companyId, branchId, "CUSTOMER", request.AsOf, request.CurrencyId, request.PartyId, request.SearchText, request.Sort, request.Direction, ct);
        ValidatePage(request.Page, request.PageSize);
        return new(result.Aggregates.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .Select(x => new ACC074RowDto(x.PartyId, x.Code, x.Name, x.CurrencyId, x.Current, x.B1, x.B2, x.B3, x.B4, x.Total)).ToArray(), request.Page, request.PageSize, result.Aggregates.Count);
    }

    public async Task<PagedResponse<ACC075RowDto>> QuerySupplierAsync(Guid companyId, Guid? branchId, ACC075QueryRequest request, CancellationToken ct = default)
    {
        var result = await BuildAsync(companyId, branchId, "SUPPLIER", request.AsOf, request.CurrencyId, request.PartyId, request.SearchText, request.Sort, request.Direction, ct);
        ValidatePage(request.Page, request.PageSize);
        return new(result.Aggregates.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
            .Select(x => new ACC075RowDto(x.PartyId, x.Code, x.Name, x.CurrencyId, x.Current, x.B1, x.B2, x.B3, x.B4, x.Total)).ToArray(), request.Page, request.PageSize, result.Aggregates.Count);
    }

    public async Task<PagedResponse<ACC074DetailDto>> DrillCustomerAsync(Guid companyId, Guid? branchId, ACC074DrillDownRequest request, CancellationToken ct = default)
    {
        var result = await BuildAsync(companyId, branchId, "CUSTOMER", request.AsOf, request.CurrencyId, request.PartyId, null, request.Sort, request.Direction, ct);
        ValidatePage(request.Page, request.PageSize);
        var ordered = result.Items.OrderBy(x => x.Item.DueDate).ThenBy(x => x.Source.Number, StringComparer.Ordinal).ToArray();
        return new(ordered.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).Select(x =>
            new ACC074DetailDto(x.Item.Id, x.Source.Number, x.Item.SourceDocumentType, x.Source.Date, x.Item.DueDate,
                x.Item.CurrencyId, x.Item.OriginalAmount, x.Item.OriginalAmount - x.Outstanding, x.Outstanding,
                (request.AsOf.Date - x.Item.DueDate.Date).Days)).ToArray(), request.Page, request.PageSize, ordered.Length);
    }

    public async Task<PagedResponse<ACC075DetailDto>> DrillSupplierAsync(Guid companyId, Guid? branchId, ACC075DrillDownRequest request, CancellationToken ct = default)
    {
        var result = await BuildAsync(companyId, branchId, "SUPPLIER", request.AsOf, request.CurrencyId, request.PartyId, null, request.Sort, request.Direction, ct);
        ValidatePage(request.Page, request.PageSize);
        var ordered = result.Items.OrderBy(x => x.Item.DueDate).ThenBy(x => x.Source.Number, StringComparer.Ordinal).ToArray();
        return new(ordered.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).Select(x =>
            new ACC075DetailDto(x.Item.Id, x.Source.Number, x.Item.SourceDocumentType, x.Source.Date, x.Item.DueDate,
                x.Item.CurrencyId, x.Item.OriginalAmount, x.Item.OriginalAmount - x.Outstanding, x.Outstanding,
                (request.AsOf.Date - x.Item.DueDate.Date).Days)).ToArray(), request.Page, request.PageSize, ordered.Length);
    }

    public async Task<ExportJobOrFileResponse> ExportCustomerAsync(Guid companyId, Guid? branchId, ACC074ExportRequest request, CancellationToken ct = default)
        => Export("customer-aging.csv", (await BuildAsync(companyId, branchId, "CUSTOMER", request.AsOf, request.CurrencyId, request.PartyId, request.SearchText, request.Sort, request.Direction, ct)).Aggregates);

    public async Task<ExportJobOrFileResponse> ExportSupplierAsync(Guid companyId, Guid? branchId, ACC075ExportRequest request, CancellationToken ct = default)
        => Export("supplier-aging.csv", (await BuildAsync(companyId, branchId, "SUPPLIER", request.AsOf, request.CurrencyId, request.PartyId, request.SearchText, request.Sort, request.Direction, ct)).Aggregates);

    public async Task<PrintPayloadOrJobResponse> PrintCustomerAsync(Guid companyId, Guid? branchId, ACC074PrintRequest request, CancellationToken ct = default)
        => Print("أعمار الديون للعملاء", (await BuildAsync(companyId, branchId, "CUSTOMER", request.AsOf, request.CurrencyId, request.PartyId, request.SearchText, request.Sort, request.Direction, ct)).Aggregates);

    public async Task<PrintPayloadOrJobResponse> PrintSupplierAsync(Guid companyId, Guid? branchId, ACC075PrintRequest request, CancellationToken ct = default)
        => Print("أعمار الالتزامات للموردين", (await BuildAsync(companyId, branchId, "SUPPLIER", request.AsOf, request.CurrencyId, request.PartyId, request.SearchText, request.Sort, request.Direction, ct)).Aggregates);

    private async Task<(List<ComputedItem> Items, List<AgingAggregate> Aggregates)> BuildAsync(
        Guid companyId, Guid? branchId, string partyType, DateTime asOf, Guid? currencyId, Guid? partyId,
        string? searchText, string? sort, string? direction, CancellationToken ct)
    {
        var query = authority.OpenItems.AsNoTracking().Where(x => x.CompanyId == companyId && x.PartyType == partyType && x.Status == "OPEN");
        if (branchId.HasValue) query = query.Where(x => x.BranchId == branchId.Value);
        if (currencyId.HasValue) query = query.Where(x => x.CurrencyId == currencyId.Value);
        if (partyId.HasValue)
            query = partyType == "CUSTOMER" ? query.Where(x => x.CustomerId == partyId.Value) : query.Where(x => x.SupplierId == partyId.Value);

        var openItems = await query.ToListAsync(ct);
        var openItemIds = openItems.Select(x => x.Id).ToArray();
        var applied = await authority.PaymentAllocations.AsNoTracking()
            .Where(x => openItemIds.Contains(x.TargetOpenItemId) && x.Status == "APPLIED")
            .GroupBy(x => x.TargetOpenItemId)
            .Select(g => new { OpenItemId = g.Key, Amount = g.Sum(x => x.Amount) })
            .ToDictionaryAsync(x => x.OpenItemId, x => x.Amount, ct);

        var ids = openItems.Select(x => partyType == "CUSTOMER" ? x.CustomerId!.Value : x.SupplierId!.Value).Distinct().ToArray();
        Dictionary<Guid, (string Code, string Name)> parties;
        if (partyType == "CUSTOMER")
            parties = (await authority.Customers.AsNoTracking().Where(x => x.CompanyId == companyId && ids.Contains(x.Id)).ToListAsync(ct))
                .ToDictionary(x => x.Id, x => (x.Code, x.ArabicName));
        else
            parties = (await authority.Suppliers.AsNoTracking().Where(x => x.CompanyId == companyId && ids.Contains(x.Id)).ToListAsync(ct))
                .ToDictionary(x => x.Id, x => (x.Code, x.ArabicName));

        var computed = new List<ComputedItem>();
        foreach (var item in openItems)
        {
            var source = await ResolveSourceDocumentAsync(companyId, branchId, item.SourceDocumentType, item.SourceDocumentId, ct);
            if (source.Date.Date > asOf.Date) continue;
            var outstanding = item.OriginalAmount - applied.GetValueOrDefault(item.Id);
            if (outstanding <= 0) continue;
            var id = partyType == "CUSTOMER" ? item.CustomerId!.Value : item.SupplierId!.Value;
            if (!parties.TryGetValue(id, out var party)) throw new InvalidOperationException("PARTY_SOURCE_NOT_FOUND");
            computed.Add(new(item, party.Code, party.Name, outstanding, source));
        }

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            var term = searchText.Trim();
            computed = computed.Where(x => x.PartyCode.Contains(term, StringComparison.OrdinalIgnoreCase)
                || x.PartyName.Contains(term, StringComparison.OrdinalIgnoreCase)
                || x.Source.Number.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        var aggregates = computed
            .GroupBy(x => new { PartyId = partyType == "CUSTOMER" ? x.Item.CustomerId!.Value : x.Item.SupplierId!.Value, x.PartyCode, x.PartyName, x.Item.CurrencyId })
            .Select(g =>
            {
                decimal Bucket(Func<int, bool> predicate) => g.Where(x => predicate((asOf.Date - x.Item.DueDate.Date).Days)).Sum(x => x.Outstanding);
                var current = g.Where(x => x.Item.DueDate.Date > asOf.Date).Sum(x => x.Outstanding);
                var b1 = Bucket(a => a is >= 0 and <= 30); var b2 = Bucket(a => a is >= 31 and <= 60);
                var b3 = Bucket(a => a is >= 61 and <= 90); var b4 = Bucket(a => a > 90);
                return new AgingAggregate(g.Key.PartyId, g.Key.PartyCode, g.Key.PartyName, g.Key.CurrencyId, current, b1, b2, b3, b4, current + b1 + b2 + b3 + b4);
            }).ToList();

        var descending = string.Equals(direction, "desc", StringComparison.OrdinalIgnoreCase);
        aggregates = (sort?.Trim().ToLowerInvariant(), descending) switch
        {
            ("total", true) => aggregates.OrderByDescending(x => x.Total).ToList(),
            ("total", false) => aggregates.OrderBy(x => x.Total).ToList(),
            ("name", true) => aggregates.OrderByDescending(x => x.Name, StringComparer.Ordinal).ToList(),
            ("name", false) => aggregates.OrderBy(x => x.Name, StringComparer.Ordinal).ToList(),
            _ => aggregates.OrderBy(x => x.Code, StringComparer.Ordinal).ToList()
        };
        return (computed, aggregates);
    }

    private async Task<SourceDocument> ResolveSourceDocumentAsync(Guid companyId, Guid? branchId, string type, Guid id, CancellationToken ct)
    {
        switch (type.Trim().ToUpperInvariant())
        {
            case "RECEIPT_VOUCHER":
            {
                var x = await accounting.ReceiptVouchers.AsNoTracking()
                    .Where(x => x.Id == id && x.CompanyId == companyId && (!branchId.HasValue || x.BranchId == branchId.Value))
                    .Select(x => new { x.VoucherNo, x.VoucherDate }).SingleOrDefaultAsync(ct);
                return x is null ? throw new InvalidOperationException("SOURCE_DOCUMENT_NOT_FOUND") : new(x.VoucherNo, x.VoucherDate);
            }
            case "PAYMENT_VOUCHER":
            {
                var x = await accounting.PaymentVouchers.AsNoTracking()
                    .Where(x => x.Id == id && x.CompanyId == companyId && (!branchId.HasValue || x.BranchId == branchId.Value))
                    .Select(x => new { x.VoucherNo, x.VoucherDate }).SingleOrDefaultAsync(ct);
                return x is null ? throw new InvalidOperationException("SOURCE_DOCUMENT_NOT_FOUND") : new(x.VoucherNo, x.VoucherDate);
            }
            case "JOURNAL_ENTRY":
            {
                var x = await accounting.JournalEntries.AsNoTracking()
                    .Where(x => x.Id == id && x.CompanyId == companyId && (!branchId.HasValue || x.BranchId == branchId.Value))
                    .Select(x => new { x.DocumentNo, x.EntryDate }).SingleOrDefaultAsync(ct);
                return x is null ? throw new InvalidOperationException("SOURCE_DOCUMENT_NOT_FOUND") : new(x.DocumentNo, x.EntryDate);
            }
            case "WAYBILL":
            {
                var x = await accounting.Set<WaybillEntity>().AsNoTracking()
                    .Where(x => x.Id == id && x.CompanyId == companyId && (!branchId.HasValue || x.BranchId == branchId.Value))
                    .Select(x => new { x.DraftNo, x.WaybillNo, x.WaybillDateTime }).SingleOrDefaultAsync(ct);
                return x is null ? throw new InvalidOperationException("SOURCE_DOCUMENT_NOT_FOUND") : new(x.WaybillNo ?? x.DraftNo, x.WaybillDateTime.UtcDateTime);
            }
            default: throw new InvalidOperationException("SOURCE_DOCUMENT_UNSUPPORTED");
        }
    }

    private static ExportJobOrFileResponse Export(string fileName, IEnumerable<AgingAggregate> rows)
    {
        var sb = new StringBuilder("PartyCode,PartyName,CurrencyId,Current,Days1To30,Days31To60,Days61To90,Over90,Total\n");
        foreach (var x in rows) sb.AppendLine($"{Csv(x.Code)},{Csv(x.Name)},{x.CurrencyId},{N(x.Current)},{N(x.B1)},{N(x.B2)},{N(x.B3)},{N(x.B4)},{N(x.Total)}");
        return new(fileName, "text/csv; charset=utf-8", sb.ToString());
    }

    private static PrintPayloadOrJobResponse Print(string title, IEnumerable<AgingAggregate> rows)
        => new(title, "text/html; charset=utf-8", $"<!doctype html><html dir=\"rtl\"><meta charset=\"utf-8\"><body><h1>{Html(title)}</h1><table>{string.Concat(rows.Select(x => $"<tr><td>{Html(x.Name)}</td><td>{N(x.Total)}</td></tr>"))}</table></body></html>");

    private static void ValidatePage(int page, int pageSize) { if (page < 1 || pageSize is < 1 or > 200) throw new ArgumentOutOfRangeException(nameof(pageSize)); }
    private static string Csv(string value) => $"\"{value.Replace("\"", "\"\"")}\"";
    private static string Html(string value) => System.Net.WebUtility.HtmlEncode(value);
    private static string N(decimal value) => value.ToString("0.######", CultureInfo.InvariantCulture);
}
