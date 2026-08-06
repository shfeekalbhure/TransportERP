namespace TransportERP.Contracts.Setup.ExchangeRates;

public enum ExchangeRateStatus { Active = 1, Suspended = 2 }

public sealed record ExchangeRateDto(Guid Id, string ForeignCurrencyCode, string LocalCurrencyCode, DateOnly EffectiveDate, decimal ReferenceRate, decimal MinimumRate, decimal MaximumRate, string? Source, ExchangeRateStatus Status, string? Notes, string CreatedBy, DateTimeOffset CreatedAt, string? ModifiedBy, DateTimeOffset? ModifiedAt, int EditCount, int PrintCount);
public sealed record ExchangeRateSearchRequest(string? Query, ExchangeRateStatus? Status, int Page = 1, int PageSize = 25);
public sealed record CreateExchangeRateRequest(string ForeignCurrencyCode, string LocalCurrencyCode, DateOnly EffectiveDate, decimal ReferenceRate, decimal MinimumRate, decimal MaximumRate, string? Source, ExchangeRateStatus Status, string? Notes);
public sealed record UpdateExchangeRateRequest(string ForeignCurrencyCode, string LocalCurrencyCode, DateOnly EffectiveDate, decimal ReferenceRate, decimal MinimumRate, decimal MaximumRate, string? Source, ExchangeRateStatus Status, string? Notes);
public sealed record ExchangeRateSearchResponse(IReadOnlyList<ExchangeRateDto> Items, int TotalCount, int Page, int PageSize, bool StorageAvailable, string? BlockerCode, string? Message);
public sealed record ExchangeRateCommandResponse(bool Succeeded, bool StorageAvailable, string? ErrorCode, string? Message, ExchangeRateDto? Item);
public static class ExchangeRateBlockers { public const string ApprovedStorageUnavailable = "APPROVED_STORAGE_UNAVAILABLE"; }
