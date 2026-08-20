namespace TransportERP.Infrastructure.Persistence;

public sealed class OperationalParty : P1Entity
{
    public Guid CompanyId { get; set; }
    public Guid? BranchId { get; set; }
    public string PartyNo { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Mobile { get; set; } = string.Empty;
    public string? IdentityType { get; set; }
    public string? IdentityNo { get; set; }
    public Guid? CountryId { get; set; }
    public Guid? GovernorateId { get; set; }
    public Guid? DirectorateId { get; set; }
    public Guid? CityId { get; set; }
    public Guid? AreaId { get; set; }
    public string? AddressLine { get; set; }
    public string Status { get; set; } = "ACTIVE";
}

public sealed class Waybill : P1Entity
{
    public Guid CompanyId { get; set; }
    public Guid BranchId { get; set; }
    public string DraftNo { get; set; } = string.Empty;
    public string? WaybillNo { get; set; }
    public Guid? ServicePointId { get; set; }
    public Guid? AgentId { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid? ApprovedBy { get; set; }
    public DateTimeOffset WaybillDateTime { get; set; }
    public DateTimeOffset? RequestDateTime { get; set; }
    public DateTimeOffset? ExpectedArrivalAt { get; set; }
    public string ServiceType { get; set; } = string.Empty;
    public string Priority { get; set; } = "NORMAL";
    public Guid OriginId { get; set; }
    public Guid DestinationId { get; set; }
    public Guid CurrencyId { get; set; }
    public decimal ExchangeRate { get; set; } = 1m;
    public decimal FreightTotal { get; set; }
    public decimal DiscountTotal { get; set; }
    public decimal NetAmount { get; set; }
    public string OperationalStatus { get; set; } = "DRAFT";
    public string FinancialStatus { get; set; } = "UNPAID";
    public string? LastReason { get; set; }
    public DateTimeOffset? ApprovedAt { get; set; }
    public DateTimeOffset? CancelledAt { get; set; }
    public ICollection<WaybillParty> Parties { get; set; } = new List<WaybillParty>();
    public ICollection<WaybillItem> Items { get; set; } = new List<WaybillItem>();
}

public sealed class WaybillParty
{
    public Guid Id { get; set; }
    public Guid WaybillId { get; set; }
    public string Role { get; set; } = string.Empty;
    public Guid? OperationalPartyId { get; set; }
    public string NameSnapshot { get; set; } = string.Empty;
    public string MobileSnapshot { get; set; } = string.Empty;
    public string? IdentityTypeSnapshot { get; set; }
    public string? IdentityNoSnapshot { get; set; }
    public Guid? CountryId { get; set; }
    public Guid? GovernorateId { get; set; }
    public Guid? DirectorateId { get; set; }
    public Guid? CityId { get; set; }
    public Guid? AreaId { get; set; }
    public string? AddressLine { get; set; }
    public Waybill? Waybill { get; set; }
    public OperationalParty? OperationalParty { get; set; }
}

public sealed class WaybillItem : P1Entity
{
    public Guid WaybillId { get; set; }
    public int LineNo { get; set; }
    public string? ItemCode { get; set; }
    public string ItemType { get; set; } = string.Empty;
    public string Contents { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public int? Pieces { get; set; }
    public decimal? Weight { get; set; }
    public decimal? Length { get; set; }
    public decimal? Width { get; set; }
    public decimal? Height { get; set; }
    public decimal? Volume { get; set; }
    public decimal? DeclaredValue { get; set; }
    public Guid? OriginCountryId { get; set; }
    public decimal? ItemFreight { get; set; }
    public string? RiskFlagsJson { get; set; }
    public string? Notes { get; set; }
    public Waybill? Waybill { get; set; }
}

public sealed class NumberSequence : P1Entity
{
    public Guid CompanyId { get; set; }
    public Guid? BranchId { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string? Prefix { get; set; }
    public ulong NextValue { get; set; } = 1;
    public string ResetPolicy { get; set; } = "NONE";
    public string Status { get; set; } = "ACTIVE";
}

public sealed class NumberReservation : P1Entity
{
    public Guid SequenceId { get; set; }
    public Guid WaybillId { get; set; }
    public Guid CompanyId { get; set; }
    public Guid? BranchId { get; set; }
    public string IdempotencyKey { get; set; } = string.Empty;
    public ulong NumberValue { get; set; }
    public string RenderedNumber { get; set; } = string.Empty;
    public DateTimeOffset ReservedAt { get; set; }
    public DateTimeOffset? CommittedAt { get; set; }
    public DateTimeOffset? VoidedAt { get; set; }
    public string? VoidReason { get; set; }
    public string State { get; set; } = "RESERVED";
    public NumberSequence? Sequence { get; set; }
    public Waybill? Waybill { get; set; }
}
