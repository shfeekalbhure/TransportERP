using TransportERP.Contracts.Geo;

namespace TransportERP.Contracts.Waybills;

public static class WaybillOperationalStatuses
{
    public const string Draft = "DRAFT";
    public const string ReadyForApproval = "READY_FOR_APPROVAL";
    public const string Approved = "APPROVED";
    public const string Cancelled = "CANCELLED";

    public static bool IsKnown(string? value) => value is Draft or ReadyForApproval or Approved or Cancelled;
}

public static class WaybillFinancialStatuses
{
    public const string Unpaid = "UNPAID";
    public const string PartiallyPaid = "PARTIALLY_PAID";
    public const string Paid = "PAID";
    public const string Overpaid = "OVERPAID";
    public const string FinanciallyClosed = "FINANCIALLY_CLOSED";
}

public sealed record CreateOperationalPartyRequest(
    string Name,
    string Mobile,
    string? IdentityType,
    string? IdentityNo,
    GeoAddressSnapshot Address,
    string? ClientOperationId = null);

public sealed record OperationalPartyResponse(
    Guid Id,
    string PartyNo,
    string Name,
    string Mobile,
    string? IdentityType,
    string? IdentityNo,
    GeoAddressSnapshot Address,
    string Status,
    string Version);

public sealed record PartySearchRequest(string? Query, int Skip = 0, int Take = 50);
public sealed record PagedOperationalPartyResponse(IReadOnlyList<OperationalPartyResponse> Items, int Total, int Skip, int Take);

public sealed record WaybillPartyInput(
    string Role,
    Guid? OperationalPartyId,
    string Name,
    string Mobile,
    string? IdentityType,
    string? IdentityNo,
    GeoAddressSnapshot Address);

public sealed record WaybillItemInput(
    int LineNo,
    string? ItemCode,
    string ItemType,
    string Contents,
    decimal Quantity,
    int? Pieces,
    decimal? Weight,
    decimal? Length,
    decimal? Width,
    decimal? Height,
    decimal? Volume,
    decimal? DeclaredValue,
    Guid? OriginCountryId,
    decimal? ItemFreight,
    string? RiskFlagsJson,
    string? Notes);

public sealed record CreateWaybillDraftRequest(
    Guid? ServicePointId,
    Guid? AgentId,
    DateTimeOffset WaybillDateTime,
    DateTimeOffset? RequestDateTime,
    DateTimeOffset? ExpectedArrivalAt,
    string ServiceType,
    string Priority,
    Guid OriginId,
    Guid DestinationId,
    Guid CurrencyId,
    decimal ExchangeRate,
    decimal FreightTotal,
    decimal DiscountTotal,
    IReadOnlyList<WaybillPartyInput> Parties,
    IReadOnlyList<WaybillItemInput> Items,
    string ClientOperationId);

public sealed record UpdateWaybillDraftRequest(
    Guid? ServicePointId,
    Guid? AgentId,
    DateTimeOffset WaybillDateTime,
    DateTimeOffset? RequestDateTime,
    DateTimeOffset? ExpectedArrivalAt,
    string ServiceType,
    string Priority,
    Guid OriginId,
    Guid DestinationId,
    Guid CurrencyId,
    decimal ExchangeRate,
    decimal FreightTotal,
    decimal DiscountTotal,
    IReadOnlyList<WaybillPartyInput> Parties,
    IReadOnlyList<WaybillItemInput> Items,
    string ExpectedVersion,
    string ClientOperationId);

public sealed record ValidateWaybillRequest(string? ExpectedVersion = null);
public sealed record SubmitWaybillRequest(string ExpectedVersion, string ClientOperationId);
public sealed record ApproveWaybillRequest(Guid SequenceId, string IdempotencyKey, string ExpectedVersion);
public sealed record ReturnWaybillRequest(string Reason, string ExpectedVersion, string ClientOperationId);
public sealed record CancelWaybillRequest(string Reason, string ExpectedVersion, string ClientOperationId);

public sealed record WaybillValidationIssue(string Code, string Field, string Message, bool Blocking);
public sealed record WaybillValidationResponse(Guid WaybillId, bool IsValid, IReadOnlyList<WaybillValidationIssue> Issues, string Version);

public sealed record WaybillPartyResponse(
    Guid Id,
    string Role,
    Guid? OperationalPartyId,
    string Name,
    string Mobile,
    string? IdentityType,
    string? IdentityNo,
    GeoAddressSnapshot Address);

public sealed record WaybillItemResponse(
    Guid Id,
    int LineNo,
    string? ItemCode,
    string ItemType,
    string Contents,
    decimal Quantity,
    int? Pieces,
    decimal? Weight,
    decimal? Length,
    decimal? Width,
    decimal? Height,
    decimal? Volume,
    decimal? DeclaredValue,
    Guid? OriginCountryId,
    decimal? ItemFreight,
    string? RiskFlagsJson,
    string? Notes);

public record WaybillResponse(
    Guid Id,
    string DraftNo,
    string? WaybillNo,
    Guid CompanyId,
    Guid BranchId,
    Guid? ServicePointId,
    Guid? AgentId,
    DateTimeOffset WaybillDateTime,
    DateTimeOffset? RequestDateTime,
    DateTimeOffset? ExpectedArrivalAt,
    string ServiceType,
    string Priority,
    Guid OriginId,
    Guid DestinationId,
    Guid CurrencyId,
    decimal ExchangeRate,
    decimal FreightTotal,
    decimal DiscountTotal,
    decimal NetAmount,
    string OperationalStatus,
    string FinancialStatus,
    IReadOnlyList<WaybillPartyResponse> Parties,
    IReadOnlyList<WaybillItemResponse> Items,
    string Version);

public sealed record WaybillDraftResponse(
    Guid Id,
    string DraftNo,
    string? WaybillNo,
    Guid CompanyId,
    Guid BranchId,
    Guid? ServicePointId,
    Guid? AgentId,
    DateTimeOffset WaybillDateTime,
    DateTimeOffset? RequestDateTime,
    DateTimeOffset? ExpectedArrivalAt,
    string ServiceType,
    string Priority,
    Guid OriginId,
    Guid DestinationId,
    Guid CurrencyId,
    decimal ExchangeRate,
    decimal FreightTotal,
    decimal DiscountTotal,
    decimal NetAmount,
    string OperationalStatus,
    string FinancialStatus,
    IReadOnlyList<WaybillPartyResponse> Parties,
    IReadOnlyList<WaybillItemResponse> Items,
    string Version)
    : WaybillResponse(Id, DraftNo, WaybillNo, CompanyId, BranchId, ServicePointId, AgentId, WaybillDateTime,
        RequestDateTime, ExpectedArrivalAt, ServiceType, Priority, OriginId, DestinationId, CurrencyId, ExchangeRate,
        FreightTotal, DiscountTotal, NetAmount, OperationalStatus, FinancialStatus, Parties, Items, Version);

public sealed record ApprovedWaybillResponse(
    Guid Id,
    string DraftNo,
    string? WaybillNo,
    Guid CompanyId,
    Guid BranchId,
    Guid? ServicePointId,
    Guid? AgentId,
    DateTimeOffset WaybillDateTime,
    DateTimeOffset? RequestDateTime,
    DateTimeOffset? ExpectedArrivalAt,
    string ServiceType,
    string Priority,
    Guid OriginId,
    Guid DestinationId,
    Guid CurrencyId,
    decimal ExchangeRate,
    decimal FreightTotal,
    decimal DiscountTotal,
    decimal NetAmount,
    string OperationalStatus,
    string FinancialStatus,
    IReadOnlyList<WaybillPartyResponse> Parties,
    IReadOnlyList<WaybillItemResponse> Items,
    string Version,
    Guid NumberReservationId)
    : WaybillResponse(Id, DraftNo, WaybillNo, CompanyId, BranchId, ServicePointId, AgentId, WaybillDateTime,
        RequestDateTime, ExpectedArrivalAt, ServiceType, Priority, OriginId, DestinationId, CurrencyId, ExchangeRate,
        FreightTotal, DiscountTotal, NetAmount, OperationalStatus, FinancialStatus, Parties, Items, Version);
