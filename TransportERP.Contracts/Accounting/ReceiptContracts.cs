namespace TransportERP.Contracts.Accounting;

public sealed record ReceiptLine(Guid AccountId, Guid CurrencyId, decimal Amount, decimal? Rate,
    string Description, string? ChequeNumber = null, DateTime? ChequeDueDate = null, Guid? WaybillId = null,
    Dictionary<string, string?>? Additional = null);
public sealed record ReceiptDraft(Guid Id, string? ExpectedVersion, DateTime Date, string Method,
    Guid CurrencyId, decimal Amount, decimal ExchangeRate, Guid DestinationId, Guid TypeId,
    Guid CollectorId, string Description, List<ReceiptLine> Lines,
    Dictionary<string, string?> Additional);
public sealed record ReceiptDocument(ReceiptDraft Draft, string Number, string State, string Version,
    Guid? JournalId, string? PostingPolicy, List<ReceiptAuditItem>? Audit = null,
    List<ReceiptAttachmentInfo>? Attachments = null, List<ReceiptAccountRow>? AccountRows = null,
    LedgerWorkflowSnapshot? Workflow = null, string? WorkflowMessage = null);
public sealed record ReceiptAuditItem(string Action, string Actor, DateTimeOffset At, string? Reason);
public sealed record ReceiptDestination(Guid Id, string Label, string Kind, Guid AccountId);
public sealed record ReceiptType(Guid Id, string Label, bool RequiresWaybill);
// No implicit cheque policy, rounding rule or account is supplied.
public sealed record ReceiptConfiguration(string? ChequeTreatment, Guid? ChequesReceivableAccountId,
    string? Rounding, Guid? NumberSequenceId, List<ReceiptDestination> Destinations, List<ReceiptType> Types,
    List<Guid> CollectorIds, List<Guid> SalespersonIds,
    Guid? DefaultCurrencyId = null, Guid? DefaultCostCenterId = null,
    string? CostCenterDimensionCode = null, string? ProjectDimensionCode = null, string? ActivityDimensionCode = null,
    string? PostingDimension = null);
public sealed record ReceiptChoice(Guid Id, string Label);
public sealed record ReceiptBootstrap(ReceiptConfiguration Configuration, List<ReceiptChoice> Accounts,
    List<ReceiptChoice> Currencies, List<ReceiptChoice> Users, List<ReceiptChoice> Waybills,
    List<ReceiptChoice> Sequences, List<string> Actions, string BranchName, string? ConfigurationVersion = null,
    List<ReceiptDimension>? Dimensions = null, List<ReceiptLinkedDocument>? Documents = null, Guid? BaseCurrencyId = null,
    LedgerWorkflowSnapshot? Workflow = null);
public sealed record ReceiptConfigurationUpdate(ReceiptConfiguration Configuration, string? ExpectedVersion);
public sealed record ReceiptListItem(Guid Id, string Number, DateTime Date, string State, decimal Amount);
public sealed record ReceiptDimension(Guid Id, string DimensionCode, string Label, DateTime ValidFrom, DateTime? ValidTo);
public sealed record ReceiptLinkedDocument(string Kind, Guid Id, string Label);
public sealed record ReceiptAttachmentInfo(Guid Id, string FileName, string MediaType, long Length, string Hash, DateTimeOffset AddedAt);
public sealed record ReceiptAttachmentUpload(Guid Id, string Version, string FileName, byte[] Content);
public sealed record ReceiptAttachmentDownload(ReceiptAttachmentInfo Info, byte[] Content);
public sealed record ReceiptAccountRow(string Account, string Currency, decimal ForeignAmount, decimal Debit, decimal Credit, string Description);
