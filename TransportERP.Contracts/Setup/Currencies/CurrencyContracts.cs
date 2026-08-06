namespace TransportERP.Contracts.Setup.Currencies;

public enum CurrencyStatus { Active = 1, Suspended = 2 }

public sealed record CurrencyDto(Guid Id, string Code, string ArabicName, string? EnglishName, string IsoCode, string? Symbol, int DecimalPlaces, bool IsLocal, CurrencyStatus Status, string? Notes, string CreatedBy, DateTimeOffset CreatedAt, string? ModifiedBy, DateTimeOffset? ModifiedAt, int EditCount, int PrintCount);
public sealed record CurrencySearchRequest(string? Query, CurrencyStatus? Status, int Page = 1, int PageSize = 25);
public sealed record CreateCurrencyRequest(string Code, string ArabicName, string? EnglishName, string IsoCode, string? Symbol, int DecimalPlaces, bool IsLocal, CurrencyStatus Status, string? Notes);
public sealed record UpdateCurrencyRequest(string ArabicName, string? EnglishName, string IsoCode, string? Symbol, int DecimalPlaces, bool IsLocal, CurrencyStatus Status, string? Notes);
public sealed record CurrencySearchResponse(IReadOnlyList<CurrencyDto> Items, int TotalCount, int Page, int PageSize, bool StorageAvailable, string? BlockerCode, string? Message);
public sealed record CurrencyCommandResponse(bool Succeeded, bool StorageAvailable, string? ErrorCode, string? Message, CurrencyDto? Item);

public static class CurrencyBlockers { public const string ApprovedStorageUnavailable = "APPROVED_STORAGE_UNAVAILABLE"; }
