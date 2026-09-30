using TransportERP.Contracts.Core;

namespace TransportERP.Contracts.Waybills;


public static class WaybillPayerRoleCodes
{
    public const string Sender = "SENDER";
    public const string Receiver = "RECEIVER";
    public const string OtherParty = "OTHER_PARTY";
}

public static class WaybillCollectorTypeCodes
{
    public const string Branch = "BRANCH";
    public const string Driver = "DRIVER";
    public const string Agent = "AGENT";
    public const string Bank = "BANK";
    public const string Other = "OTHER";
}

public sealed record PaymentPlanLineInput(
    int LineNo,
    string PayerRole,
    Guid? PartyId,
    string PaymentMethodCode,
    MoneyAmount? Amount,
    decimal? Percent,
    string DueTrigger,
    DateTimeOffset? DueAt);

public sealed record SetPaymentPlanRequest(
    long ExpectedVersion,
    IReadOnlyList<PaymentPlanLineInput> Lines,
    string ClientOperationId);

public sealed record PaymentPlanLineResponse(
    Guid Id,
    int LineNo,
    string PayerRole,
    Guid? PartyId,
    string PaymentMethodCode,
    MoneyAmount? Amount,
    decimal? Percent,
    string DueTrigger,
    DateTimeOffset? DueAt,
    string Status);

public sealed record PaymentPlanResponse(
    Guid WaybillId,
    Guid CurrencyId,
    decimal NetAmount,
    long WaybillVersion,
    IReadOnlyList<PaymentPlanLineResponse> Lines,
    Guid CorrelationId);

public sealed record RecordCollectionRequest(
    string PayerRole,
    Guid? PartyId,
    string PaymentMethodCode,
    MoneyAmount Amount,
    decimal ExchangeRate,
    string CollectedByType,
    Guid CollectedById,
    DateTimeOffset CollectedAt,
    string ClientOperationId,
    Guid? AccountingReferenceId = null,
    string? AccountingDocumentType = null);

public sealed record ReverseCollectionRequest(
    string Reason,
    string ClientOperationId,
    Guid? AccountingReferenceId = null,
    string? AccountingDocumentType = null);

public sealed record CollectionResponse(
    Guid Id,
    Guid WaybillId,
    string PayerRole,
    Guid? PartyId,
    string PaymentMethodCode,
    MoneyAmount Amount,
    decimal ExchangeRate,
    string CollectedByType,
    Guid CollectedById,
    Guid BranchId,
    DateTimeOffset CollectedAt,
    string Status,
    Guid? ReversalOfId,
    Guid? AccountingReferenceId,
    Guid CorrelationId);

public sealed record WaybillFinancialStatusResponse(
    Guid WaybillId,
    MoneyAmount NetAmount,
    MoneyAmount PaidEquivalent,
    MoneyAmount RemainingEquivalent,
    string FinancialStatus,
    long WaybillVersion,
    Guid CorrelationId);

public static class WaybillFinancePermissionCodes
{
    public const string PaymentPlan = "waybill.payment.plan";
    public const string CollectionCreate = "waybill.collection.create";
    public const string CollectionReverse = "waybill.collection.reverse";
}
