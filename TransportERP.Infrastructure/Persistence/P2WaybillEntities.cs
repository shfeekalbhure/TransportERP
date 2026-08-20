namespace TransportERP.Infrastructure.Persistence;

public abstract class P2Entity
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public long Version { get; set; } = 1;
}

public sealed class OperationalPartyEntity : P2Entity
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
    public string ClientOperationId { get; set; } = string.Empty;
}

public sealed class WaybillEntity : P2Entity
{
    public Guid CompanyId { get; set; }
    public Guid BranchId { get; set; }
    public string DraftNo { get; set; } = string.Empty;
    public string? WaybillNo { get; set; }
    public DateTimeOffset WaybillDateTime { get; set; }
    public string ServiceType { get; set; } = "STANDARD";
    public string Priority { get; set; } = "NORMAL";
    public Guid OriginId { get; set; }
    public Guid DestinationId { get; set; }
    public Guid CurrencyId { get; set; }
    public decimal ExchangeRate { get; set; }
    public decimal FreightTotal { get; set; }
    public decimal DiscountTotal { get; set; }
    public string Status { get; set; } = "DRAFT";
    public string CreateClientOperationId { get; set; } = string.Empty;
    public string LastClientOperationId { get; set; } = string.Empty;
    public ICollection<WaybillPartyEntity> Parties { get; set; } = new List<WaybillPartyEntity>();
    public ICollection<WaybillItemEntity> Items { get; set; } = new List<WaybillItemEntity>();
}

public sealed class WaybillPartyEntity
{
    public Guid Id { get; set; }
    public Guid WaybillId { get; set; }
    public int Sequence { get; set; }
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
    public string? AddressLineSnapshot { get; set; }
    public WaybillEntity? Waybill { get; set; }
}

public sealed class WaybillItemEntity
{
    public Guid Id { get; set; }
    public Guid WaybillId { get; set; }
    public int LineNo { get; set; }
    public string ItemType { get; set; } = string.Empty;
    public string Contents { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public int? Pieces { get; set; }
    public decimal? Weight { get; set; }
    public decimal? Length { get; set; }
    public decimal? Width { get; set; }
    public decimal? Height { get; set; }
    public decimal? DeclaredValue { get; set; }
    public Guid? OriginCountryId { get; set; }
    public string RiskFlagsJson { get; set; } = "[]";
    public string? Notes { get; set; }
    public WaybillEntity? Waybill { get; set; }
}

public sealed class NumberSequenceEntity : P2Entity
{
    public Guid CompanyId { get; set; }
    public Guid? BranchId { get; set; }
    public string DocumentType { get; set; } = "WAYBILL";
    public string? Prefix { get; set; }
    public ulong NextValue { get; set; } = 1;
    public string ResetPolicy { get; set; } = "NONE";
    public string Status { get; set; } = "ACTIVE";
}

public sealed class NumberReservationEntity
{
    public Guid Id { get; set; }
    public Guid SequenceId { get; set; }
    public Guid? WaybillId { get; set; }
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
    public string? LastTransitionKey { get; set; }
    public NumberSequenceEntity? Sequence { get; set; }
    public WaybillEntity? Waybill { get; set; }
}
