namespace TransportERP.Domain.Geo;

public abstract class GeoEntity
{
    public Guid Id { get; set; }
    public required string Code { get; set; }
    public required string ArabicName { get; set; }
    public string? EnglishName { get; set; }
    public bool IsActive { get; set; } = true;
    public int Version { get; set; } = 1;
}

public sealed class Country : GeoEntity
{
    public string? NationalityName { get; set; }
    public ICollection<Governorate> Governorates { get; } = [];
}

public sealed class Governorate : GeoEntity
{
    public Guid CountryId { get; set; }
    public Country? Country { get; set; }
    public ICollection<Directorate> Directorates { get; } = [];
}

public sealed class Directorate : GeoEntity
{
    public Guid GovernorateId { get; set; }
    public Governorate? Governorate { get; set; }
    public ICollection<City> Cities { get; } = [];
}

public sealed class City : GeoEntity
{
    public Guid DirectorateId { get; set; }
    public Directorate? Directorate { get; set; }
    public ICollection<Area> Areas { get; } = [];
}

public sealed class Area : GeoEntity
{
    public Guid CityId { get; set; }
    public City? City { get; set; }
}
