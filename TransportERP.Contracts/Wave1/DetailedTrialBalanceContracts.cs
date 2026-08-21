namespace TransportERP.Contracts.Wave1;

public sealed record DetailedTrialBalanceQueryRequest(
    DateTime From,
    DateTime To,
    Guid? BranchId = null,
    Guid? CurrencyId = null,
    Guid? AccountId = null,
    Guid? FinancialDimensionId = null,
    int Skip = 0,
    int Take = 100);

public sealed record DetailedTrialBalanceRow(
    Guid AccountId,
    string AccountCode,
    string AccountNameAr,
    decimal OpeningDebit,
    decimal OpeningCredit,
    decimal PeriodDebit,
    decimal PeriodCredit,
    decimal ClosingDebit,
    decimal ClosingCredit);

public sealed record DetailedTrialBalanceResponse(
    IReadOnlyList<DetailedTrialBalanceRow> Items,
    int Total,
    decimal TotalOpeningDebit,
    decimal TotalOpeningCredit,
    decimal TotalPeriodDebit,
    decimal TotalPeriodCredit,
    decimal TotalClosingDebit,
    decimal TotalClosingCredit,
    int Skip,
    int Take);

public sealed record DetailedTrialBalanceDrillDownRequest(
    Guid AccountId,
    DateTime From,
    DateTime To,
    Guid? BranchId = null,
    Guid? CurrencyId = null,
    Guid? FinancialDimensionId = null,
    int Skip = 0,
    int Take = 100);

public sealed record DetailedTrialBalanceDrillDownRow(
    Guid JournalEntryId,
    string DocumentNo,
    DateTime EntryDate,
    int LineNo,
    string? Description,
    decimal Debit,
    decimal Credit,
    Guid CurrencyId,
    Guid? FinancialDimensionId);

public sealed record DetailedTrialBalanceDrillDownResponse(
    IReadOnlyList<DetailedTrialBalanceDrillDownRow> Items,
    int Total,
    int Skip,
    int Take);
