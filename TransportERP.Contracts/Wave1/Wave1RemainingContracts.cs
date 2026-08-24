namespace TransportERP.Contracts.Wave1;

public sealed record Wave1ReferencePage<T>(IReadOnlyList<T> Items, int Total, int Skip, int Take);

// GEN-014 current governing contract: SRC-048/SRC-049/SRC-057.
public sealed record LanguageQueryRequest(
    string? SearchText = null,
    string? Status = null,
    string? Direction = null,
    int Page = 1,
    int PageSize = 100,
    string? SortBy = null,
    string? SortDirection = null);

public sealed record LanguageDto(
    Guid Id,
    string Code,
    string CultureCode,
    string Direction,
    string Status,
    int Version);

public sealed record LanguageListItemDto(
    Guid Id,
    string Code,
    string CultureCode,
    string Direction,
    string Status,
    int Version);

public sealed record CreateLanguageRequest(
    string Code,
    string CultureCode,
    string Direction);

public sealed record UpdateLanguageRequest(
    string Code,
    string CultureCode,
    string Direction,
    int ExpectedVersion);

public sealed record DisableReferenceRequest(long ExpectedVersion, string Reason);

public sealed record AccountClassificationDto(
    Guid Id,
    Guid CompanyId,
    string Code,
    string ArabicName,
    string? EnglishName,
    string AccountType,
    bool IsActive,
    long Version);

public sealed record CreateAccountClassificationRequest(
    string Code,
    string ArabicName,
    string? EnglishName,
    string AccountType);

public sealed record UpdateAccountClassificationRequest(
    string Code,
    string ArabicName,
    string? EnglishName,
    string AccountType,
    long ExpectedVersion);

public sealed record AgingQueryRequest(
    DateTime AsOf,
    Guid? BranchId = null,
    Guid? CurrencyId = null,
    Guid? PartyId = null,
    int Skip = 0,
    int Take = 100);

public sealed record AgingRow(
    Guid PartyId,
    string PartyCode,
    string PartyName,
    Guid CurrencyId,
    decimal Current,
    decimal Days1To30,
    decimal Days31To60,
    decimal Days61To90,
    decimal Over90,
    decimal TotalOutstanding);

public sealed record AgingResponse(
    IReadOnlyList<AgingRow> Items,
    int Total,
    decimal GrandTotal,
    int Skip,
    int Take);

public sealed record AgingDrillDownRequest(
    Guid PartyId,
    DateTime AsOf,
    Guid? BranchId = null,
    Guid? CurrencyId = null,
    int Skip = 0,
    int Take = 100);

public sealed record AgingOpenItemRow(
    Guid Id,
    string DocumentNo,
    string SourceType,
    DateTime DocumentDate,
    DateTime DueDate,
    Guid CurrencyId,
    decimal OriginalAmount,
    decimal SettledAmount,
    decimal OutstandingAmount,
    int AgeDays);

public sealed record AgingDrillDownResponse(
    IReadOnlyList<AgingOpenItemRow> Items,
    int Total,
    int Skip,
    int Take);

public sealed record BalanceSheetQueryRequest(
    DateTime AsOf,
    Guid? BranchId = null,
    Guid? CurrencyId = null);

public sealed record BalanceSheetLine(
    Guid AccountId,
    string AccountCode,
    string AccountNameAr,
    string AccountType,
    decimal Balance);

public sealed record BalanceSheetResponse(
    IReadOnlyList<BalanceSheetLine> Assets,
    IReadOnlyList<BalanceSheetLine> Liabilities,
    IReadOnlyList<BalanceSheetLine> Equity,
    decimal AssetsTotal,
    decimal LiabilitiesTotal,
    decimal EquityTotal,
    decimal CurrentEarnings,
    decimal EquationDifference);

public sealed record BalanceSheetDrillDownRequest(
    Guid AccountId,
    DateTime AsOf,
    Guid? BranchId = null,
    Guid? CurrencyId = null,
    int Skip = 0,
    int Take = 100);

public sealed record FinancialDrillDownRow(
    Guid JournalEntryId,
    string DocumentNo,
    DateTime EntryDate,
    int LineNo,
    string? Description,
    decimal Debit,
    decimal Credit,
    Guid CurrencyId);

public sealed record FinancialDrillDownResponse(
    IReadOnlyList<FinancialDrillDownRow> Items,
    int Total,
    int Skip,
    int Take);

public sealed record CashFlowQueryRequest(
    DateTime From,
    DateTime To,
    Guid? BranchId = null,
    Guid? CurrencyId = null);

public sealed record CashFlowLine(
    string Activity,
    string SourceType,
    string DocumentNo,
    DateTime Date,
    Guid CurrencyId,
    decimal Inflow,
    decimal Outflow,
    decimal Net);

public sealed record CashFlowResponse(
    IReadOnlyList<CashFlowLine> Items,
    decimal OperatingNet,
    decimal InvestingNet,
    decimal FinancingNet,
    decimal UnclassifiedNet,
    decimal NetCashMovement);

public sealed record CashFlowDrillDownRequest(
    string Activity,
    DateTime From,
    DateTime To,
    Guid? BranchId = null,
    Guid? CurrencyId = null,
    int Skip = 0,
    int Take = 100);

public sealed record ReportExportResponse(string FileName, string ContentType, string Content);
public sealed record ReportPrintResponse(string Title, string ContentType, string Content);
