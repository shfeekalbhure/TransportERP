namespace TransportERP.Contracts.Wave1;

public sealed record ACC074QueryRequest(DateTime AsOf, Guid? BranchId = null, Guid? CurrencyId = null, Guid? PartyId = null, int Page = 1, int PageSize = 100, string? SearchText = null, string? Sort = null, string? Direction = null);
public sealed record ACC074DrillDownRequest(Guid PartyId, DateTime AsOf, Guid? BranchId = null, Guid? CurrencyId = null, int Page = 1, int PageSize = 100, string? Sort = null, string? Direction = null);
public sealed record ACC074ExportRequest(DateTime AsOf, Guid? BranchId = null, Guid? CurrencyId = null, Guid? PartyId = null, int Page = 1, int PageSize = 100, string? SearchText = null, string? Sort = null, string? Direction = null);
public sealed record ACC074PrintRequest(DateTime AsOf, Guid? BranchId = null, Guid? CurrencyId = null, Guid? PartyId = null, int Page = 1, int PageSize = 100, string? SearchText = null, string? Sort = null, string? Direction = null);
public sealed record ACC074RowDto(Guid PartyId, string PartyCode, string PartyName, Guid CurrencyId, decimal Current, decimal Days1To30, decimal Days31To60, decimal Days61To90, decimal Over90, decimal TotalOutstanding);
public sealed record ACC074DetailDto(Guid Id, string DocumentNo, string SourceType, DateTime DocumentDate, DateTime DueDate, Guid CurrencyId, decimal OriginalAmount, decimal SettledAmount, decimal OutstandingAmount, int AgeDays);

public sealed record ACC075QueryRequest(DateTime AsOf, Guid? BranchId = null, Guid? CurrencyId = null, Guid? PartyId = null, int Page = 1, int PageSize = 100, string? SearchText = null, string? Sort = null, string? Direction = null);
public sealed record ACC075DrillDownRequest(Guid PartyId, DateTime AsOf, Guid? BranchId = null, Guid? CurrencyId = null, int Page = 1, int PageSize = 100, string? Sort = null, string? Direction = null);
public sealed record ACC075ExportRequest(DateTime AsOf, Guid? BranchId = null, Guid? CurrencyId = null, Guid? PartyId = null, int Page = 1, int PageSize = 100, string? SearchText = null, string? Sort = null, string? Direction = null);
public sealed record ACC075PrintRequest(DateTime AsOf, Guid? BranchId = null, Guid? CurrencyId = null, Guid? PartyId = null, int Page = 1, int PageSize = 100, string? SearchText = null, string? Sort = null, string? Direction = null);
public sealed record ACC075RowDto(Guid PartyId, string PartyCode, string PartyName, Guid CurrencyId, decimal Current, decimal Days1To30, decimal Days31To60, decimal Days61To90, decimal Over90, decimal TotalOutstanding);
public sealed record ACC075DetailDto(Guid Id, string DocumentNo, string SourceType, DateTime DocumentDate, DateTime DueDate, Guid CurrencyId, decimal OriginalAmount, decimal SettledAmount, decimal OutstandingAmount, int AgeDays);

public sealed record ACC049QueryRequest(DateTime AsOf, Guid? BranchId = null, Guid? CurrencyId = null, int Page = 1, int PageSize = 100, string? SearchText = null, string? Sort = null, string? Direction = null);
public sealed record ACC049DrillDownRequest(Guid AccountId, DateTime AsOf, Guid? BranchId = null, Guid? CurrencyId = null, int Page = 1, int PageSize = 100, string? Sort = null, string? Direction = null);
public sealed record ACC049ExportRequest(DateTime AsOf, Guid? BranchId = null, Guid? CurrencyId = null, int Page = 1, int PageSize = 100, string? SearchText = null, string? Sort = null, string? Direction = null);
public sealed record ACC049PrintRequest(DateTime AsOf, Guid? BranchId = null, Guid? CurrencyId = null, int Page = 1, int PageSize = 100, string? SearchText = null, string? Sort = null, string? Direction = null);
public sealed record ACC049RowDto(Guid AccountId, string AccountCode, string AccountNameAr, string AccountType, decimal Balance);
public sealed record ACC049DetailDto(Guid JournalEntryId, string DocumentNo, DateTime EntryDate, int LineNo, string? Description, decimal Debit, decimal Credit, Guid CurrencyId);

public sealed record ACC050QueryRequest(DateTime From, DateTime To, Guid? BranchId = null, Guid? CurrencyId = null, int Page = 1, int PageSize = 100, string? SearchText = null, string? Sort = null, string? Direction = null);
public sealed record ACC050DrillDownRequest(string Activity, DateTime From, DateTime To, Guid? BranchId = null, Guid? CurrencyId = null, int Page = 1, int PageSize = 100, string? Sort = null, string? Direction = null);
public sealed record ACC050ExportRequest(DateTime From, DateTime To, Guid? BranchId = null, Guid? CurrencyId = null, int Page = 1, int PageSize = 100, string? SearchText = null, string? Sort = null, string? Direction = null);
public sealed record ACC050PrintRequest(DateTime From, DateTime To, Guid? BranchId = null, Guid? CurrencyId = null, int Page = 1, int PageSize = 100, string? SearchText = null, string? Sort = null, string? Direction = null);
public sealed record ACC050RowDto(string Activity, string SourceType, string DocumentNo, DateTime Date, Guid CurrencyId, decimal Inflow, decimal Outflow, decimal Net);
public sealed record ACC050DetailDto(string Activity, string SourceType, string DocumentNo, DateTime Date, Guid CurrencyId, decimal Inflow, decimal Outflow, decimal Net);

public sealed record ACC058QueryRequest(DateTime From, DateTime To, Guid? BranchId = null, Guid? CurrencyId = null, Guid? AccountId = null, Guid? FinancialDimensionId = null, int Page = 1, int PageSize = 100, string? SearchText = null, string? Sort = null, string? Direction = null);
public sealed record ACC058DrillDownRequest(Guid AccountId, DateTime From, DateTime To, Guid? BranchId = null, Guid? CurrencyId = null, Guid? FinancialDimensionId = null, int Page = 1, int PageSize = 100, string? Sort = null, string? Direction = null);
public sealed record ACC058ExportRequest(DateTime From, DateTime To, Guid? BranchId = null, Guid? CurrencyId = null, Guid? AccountId = null, Guid? FinancialDimensionId = null, int Page = 1, int PageSize = 100, string? SearchText = null, string? Sort = null, string? Direction = null);
public sealed record ACC058PrintRequest(DateTime From, DateTime To, Guid? BranchId = null, Guid? CurrencyId = null, Guid? AccountId = null, Guid? FinancialDimensionId = null, int Page = 1, int PageSize = 100, string? SearchText = null, string? Sort = null, string? Direction = null);
public sealed record ACC058RowDto(Guid AccountId, string AccountCode, string AccountNameAr, decimal OpeningDebit, decimal OpeningCredit, decimal PeriodDebit, decimal PeriodCredit, decimal ClosingDebit, decimal ClosingCredit);
public sealed record ACC058DetailDto(Guid JournalEntryId, string DocumentNo, DateTime EntryDate, int LineNo, string? Description, decimal Debit, decimal Credit, Guid CurrencyId, Guid? FinancialDimensionId);

public sealed record ExportJobOrFileResponse(string FileName, string ContentType, string Content);
public sealed record PrintPayloadOrJobResponse(string Title, string ContentType, string Content);
