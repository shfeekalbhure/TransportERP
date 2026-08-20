using TransportERP.Contracts.Geo;

namespace TransportERP.Contracts.Waybills;

public sealed record WaybillPartyInput(
    string Role,
    Guid? OperationalPartyId,
    string Name,
    string Mobile,
    string? IdentityType,
    string? IdentityNo,
    GeoAddressSnapshot Address);

public sealed record WaybillItemInput(
    Guid? Id,
    int LineNo,
    string ItemType,
    string Contents,
    decimal Quantity,
    int? Pieces,
    decimal? Weight,
    decimal? Length,
    decimal? Width,
    decimal? Height,
    decimal? DeclaredValue,
    Guid? OriginCountryId,
    IReadOnlyList<string>? RiskFlags,
    string? Notes);

public sealed record CreateWaybillDraftRequest(
    Guid BranchId,
    DateTimeOffset WaybillDateTime,
    Guid OriginId,
    Guid DestinationId,
    Guid CurrencyId,
    decimal ExchangeRate,
    string? ServiceType,
    string? Priority,
    string ClientOperationId);

public sealed record UpdateWaybillDraftRequest(
    long ExpectedVersion,
    DateTimeOffset WaybillDateTime,
    Guid OriginId,
    Guid DestinationId,
    Guid CurrencyId,
    decimal ExchangeRate,
    decimal FreightTotal,
    decimal DiscountTotal,
    string ServiceType,
    string Priority,
    IReadOnlyList<WaybillPartyInput> Parties,
    IReadOnlyList<WaybillItemInput> Items,
    string ClientOperationId);

public sealed record ValidateWaybillRequest(long? ExpectedVersion = null);
public sealed record SubmitWaybillRequest(long ExpectedVersion, string ClientOperationId);
public sealed record ApproveWaybillRequest(long ExpectedVersion, Guid NumberSequenceId, string IdempotencyKey);
public sealed record ReturnWaybillRequest(long ExpectedVersion, string Reason, string ClientOperationId);
public sealed record CancelWaybillRequest(long ExpectedVersion, string Reason, string ClientOperationId);

public sealed record WaybillValidationResponse(
    Guid WaybillId,
    bool IsValid,
    IReadOnlyList<string> BlockingErrors,
    long Version,
    Guid CorrelationId);

public sealed record WaybillPartyResponse(
    string Role,
    Guid? OperationalPartyId,
    string Name,
    string Mobile,
    string? IdentityType,
    string? MaskedIdentityNo,
    GeoAddressSnapshot Address);

public sealed record WaybillItemResponse(
    Guid Id,
    int LineNo,
    string ItemType,
    string Contents,
    decimal Quantity,
    int? Pieces,
    decimal? Weight,
    decimal? Length,
    decimal? Width,
    decimal? Height,
    decimal? DeclaredValue,
    Guid? OriginCountryId,
    IReadOnlyList<string> RiskFlags,
    string? Notes);

public sealed record WaybillResponse(
    Guid Id,
    string DraftNo,
    string? WaybillNo,
    Guid CompanyId,
    Guid BranchId,
    DateTimeOffset WaybillDateTime,
    Guid OriginId,
    Guid DestinationId,
    Guid CurrencyId,
    decimal ExchangeRate,
    decimal FreightTotal,
    decimal DiscountTotal,
    decimal NetAmount,
    string ServiceType,
    string Priority,
    string Status,
    long Version,
    IReadOnlyList<WaybillPartyResponse> Parties,
    IReadOnlyList<WaybillItemResponse> Items,
    Guid CorrelationId);

public sealed record OperationalPartyCreateRequest(
    string Name,
    string Mobile,
    string? IdentityType,
    string? IdentityNo,
    GeoAddressSnapshot Address,
    string ClientOperationId);

public sealed record OperationalPartySearchRequest(string? Query, int Skip = 0, int Take = 50);

public sealed record OperationalPartyResponse(
    Guid Id,
    string PartyNo,
    string Name,
    string Mobile,
    string? IdentityType,
    string? MaskedIdentityNo,
    GeoAddressSnapshot Address,
    string Status,
    long Version);

public sealed record PagedOperationalPartyResponse(
    IReadOnlyList<OperationalPartyResponse> Items,
    long Total,
    int Skip,
    int Take,
    Guid CorrelationId);

public static class WaybillPermissionCodes
{
    public const string View = "waybill.view";
    public const string Create = "waybill.create";
    public const string Edit = "waybill.edit";
    public const string Validate = "waybill.validate";
    public const string Submit = "waybill.submit";
    public const string Approve = "waybill.approve";
    public const string Return = "waybill.approval.return";
    public const string Cancel = "waybill.cancel";
    public const string PartyView = "party.view";
    public const string PartyCreate = "party.create";
}
