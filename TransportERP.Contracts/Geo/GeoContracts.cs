namespace TransportERP.Contracts.Geo;

public sealed record PagedQueryRequest(int Page = 1, int PageSize = 100, string? SearchText = null, string? Sort = null, string? Direction = null, Guid? ParentId = null, bool? IsActive = null);
public sealed record PagedResponse<T>(IReadOnlyList<T> Items, int Page, int PageSize, int TotalCount);
public sealed record DisableRequest(int ExpectedVersion, string Reason);

public abstract record GeoDto(Guid Id, string Code, string ArabicName, string? EnglishName, bool IsActive, int Version);
public sealed record CountryDto(Guid Id, string Code, string ArabicName, string? EnglishName, string? NationalityName, bool IsActive, int Version) : GeoDto(Id, Code, ArabicName, EnglishName, IsActive, Version);
public sealed record GovernorateDto(Guid Id, Guid CountryId, string Code, string ArabicName, string? EnglishName, bool IsActive, int Version) : GeoDto(Id, Code, ArabicName, EnglishName, IsActive, Version);
public sealed record DirectorateDto(Guid Id, Guid GovernorateId, string Code, string ArabicName, string? EnglishName, bool IsActive, int Version) : GeoDto(Id, Code, ArabicName, EnglishName, IsActive, Version);
public sealed record CityDto(Guid Id, Guid DirectorateId, string Code, string ArabicName, string? EnglishName, bool IsActive, int Version) : GeoDto(Id, Code, ArabicName, EnglishName, IsActive, Version);
public sealed record AreaDto(Guid Id, Guid CityId, string Code, string ArabicName, string? EnglishName, bool IsActive, int Version) : GeoDto(Id, Code, ArabicName, EnglishName, IsActive, Version);

public sealed record CreateCountryRequest(string Code, string ArabicName, string? EnglishName, string? NationalityName);
public sealed record UpdateCountryRequest(string Code, string ArabicName, string? EnglishName, string? NationalityName, int ExpectedVersion);
public sealed record CreateGovernorateRequest(Guid CountryId, string Code, string ArabicName, string? EnglishName);
public sealed record UpdateGovernorateRequest(Guid CountryId, string Code, string ArabicName, string? EnglishName, int ExpectedVersion);
public sealed record CreateDirectorateRequest(Guid GovernorateId, string Code, string ArabicName, string? EnglishName);
public sealed record UpdateDirectorateRequest(Guid GovernorateId, string Code, string ArabicName, string? EnglishName, int ExpectedVersion);
public sealed record CreateCityRequest(Guid DirectorateId, string Code, string ArabicName, string? EnglishName);
public sealed record UpdateCityRequest(Guid DirectorateId, string Code, string ArabicName, string? EnglishName, int ExpectedVersion);
public sealed record CreateAreaRequest(Guid CityId, string Code, string ArabicName, string? EnglishName);
public sealed record UpdateAreaRequest(Guid CityId, string Code, string ArabicName, string? EnglishName, int ExpectedVersion);
