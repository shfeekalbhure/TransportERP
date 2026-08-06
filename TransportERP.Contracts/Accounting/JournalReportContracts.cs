namespace TransportERP.Contracts.Accounting;

public sealed record JournalReportQuery(DateOnly FromDate, DateOnly ToDate, string? AccountCode, string? BranchCode, string? CostCenterCode, string? Status, string? CurrencyCode, string? JournalType, string? Search, int Page = 1, int PageSize = 100);
public sealed record JournalReportRow(DateOnly Date, string JournalNumber, string AccountCode, string AccountName, string Description, decimal Debit, decimal Credit, string Status, string CurrencyCode, string JournalType);
public sealed record JournalReportResponse(IReadOnlyList<JournalReportRow> Items, int TotalCount, bool StorageAvailable, string? BlockerCode, string? BlockerMessage);
