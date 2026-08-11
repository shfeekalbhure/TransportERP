namespace TransportERP.Domain.Org;

/// <summary>Shared persistence fields for the approved W1-SETUP-ORG aggregates.</summary>
public abstract class OrgEntity
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; } = true;
    public int Version { get; set; } = 1;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
}

public sealed class Currency : OrgEntity
{
    public required string Code { get; set; }
    public required string ArabicName { get; set; }
    public required string EnglishName { get; set; }
    public string? Symbol { get; set; }
    public byte DecimalPlaces { get; set; }
}

public sealed class Company : OrgEntity
{
    public required string Code { get; set; }
    public required string ArabicName { get; set; }
    public required string EnglishName { get; set; }
    public required string LegalName { get; set; }
    public string? TaxNumber { get; set; }
    public Guid BaseCurrencyId { get; set; }
    public string? LogoUri { get; set; }
    public string? Notes { get; set; }
}

public sealed class Branch : OrgEntity
{
    public Guid CompanyId { get; set; }
    public required string Code { get; set; }
    public required string ArabicName { get; set; }
    public required string EnglishName { get; set; }
    public string? TimeZone { get; set; }
    public string? Notes { get; set; }
}

public sealed class ExchangeRate : OrgEntity
{
    public Guid CompanyId { get; set; }
    public Guid BaseCurrencyId { get; set; }
    public Guid QuoteCurrencyId { get; set; }
    public decimal Rate { get; set; }
    public DateOnly EffectiveFrom { get; set; }
    public DateOnly? EffectiveTo { get; set; }
    public decimal MinimumRate { get; set; }
    public decimal MaximumRate { get; set; }
    public required string Source { get; set; }
}

public enum FiscalYearStatus { Draft, Open, Closed }

public sealed class FiscalYear : OrgEntity
{
    public Guid CompanyId { get; set; }
    public required string Code { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public FiscalYearStatus Status { get; set; } = FiscalYearStatus.Draft;
}

public sealed class NumberSequence : OrgEntity
{
    public required string Code { get; set; }
    public required string ArabicName { get; set; }
    public required string EnglishName { get; set; }
    public required string ScopeType { get; set; }
    public string? DocumentType { get; set; }
    public Guid? CompanyId { get; set; }
    public Guid? BranchId { get; set; }
    public Guid? FiscalYearId { get; set; }
    public string? Prefix { get; set; }
    public ulong LastNumber { get; set; }
    public string? ResetPolicy { get; set; }
}

public enum NumberReservationState { Reserved, Committed, Cancelled }

public sealed class NumberReservation
{
    public Guid Id { get; set; }
    public Guid SequenceId { get; set; }
    public ulong NumberValue { get; set; }
    public required string RenderedNumber { get; set; }
    public NumberReservationState State { get; set; } = NumberReservationState.Reserved;
    public required string IdempotencyKey { get; set; }
    public string? Reason { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}

public sealed class Language : OrgEntity
{
    public required string LanguageCode { get; set; }
    public required string ArabicName { get; set; }
    public required string EnglishName { get; set; }
    public required string Direction { get; set; }
}

public sealed class SettingDefinition
{
    public Guid Id { get; set; }
    public required string PropertyCode { get; set; }
    public required string Group { get; set; }
    public required string ValueType { get; set; }
    public required string BuiltInDefault { get; set; }
    public required string AllowedScopes { get; set; }
    public required string ResolutionPolicy { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
}

public sealed class SettingOverride : OrgEntity
{
    public Guid DefinitionId { get; set; }
    public required string ScopeType { get; set; }
    public Guid? ScopeId { get; set; }
    public required string TypedValue { get; set; }
    public DateOnly? EffectiveFrom { get; set; }
    public DateOnly? EffectiveTo { get; set; }
}
