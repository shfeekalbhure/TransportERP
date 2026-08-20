namespace TransportERP.Contracts.Geo;

/// <summary>
/// Document-safe geographic snapshot. IDs point to governed Geo lookups while AddressLine preserves field detail.
/// </summary>
public sealed record GeoAddressSnapshot(
    Guid? CountryId,
    Guid? GovernorateId,
    Guid? DirectorateId,
    Guid? CityId,
    Guid? AreaId,
    string? AddressLine)
{
    public bool HasStructuredLocation => CountryId.HasValue || GovernorateId.HasValue || DirectorateId.HasValue || CityId.HasValue || AreaId.HasValue;

    public void EnsureUsable()
    {
        if (!HasStructuredLocation && string.IsNullOrWhiteSpace(AddressLine))
        {
            throw new ArgumentException("A structured location or address line is required.");
        }
        if (AreaId.HasValue && !CityId.HasValue)
        {
            throw new ArgumentException("Area requires a city reference.", nameof(AreaId));
        }
        if (CityId.HasValue && !DirectorateId.HasValue)
        {
            throw new ArgumentException("City requires a directorate reference.", nameof(CityId));
        }
        if (DirectorateId.HasValue && !GovernorateId.HasValue)
        {
            throw new ArgumentException("Directorate requires a governorate reference.", nameof(DirectorateId));
        }
        if (GovernorateId.HasValue && !CountryId.HasValue)
        {
            throw new ArgumentException("Governorate requires a country reference.", nameof(GovernorateId));
        }
    }
}
