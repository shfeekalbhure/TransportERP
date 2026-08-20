namespace TransportERP.Contracts.Geo;

/// <summary>
/// Document-safe address snapshot aligned with the closed P2-C01 W1 party contract.
/// Lookup IDs are optional snapshots; the governed Geo catalog remains the source for full hierarchy resolution.
/// </summary>
public sealed record GeoAddressSnapshot(
    Guid? CountryId,
    Guid? GovernorateId,
    Guid? CityId,
    Guid? AreaId,
    string? AddressLine)
{
    public bool HasStructuredLocation =>
        CountryId.HasValue || GovernorateId.HasValue || CityId.HasValue || AreaId.HasValue;

    public void EnsureUsable()
    {
        if (!HasStructuredLocation && string.IsNullOrWhiteSpace(AddressLine))
        {
            throw new ArgumentException("A structured location or address line is required.");
        }

        // Area is a child-level selector and must identify the city context captured on the document.
        if (AreaId.HasValue && !CityId.HasValue)
        {
            throw new ArgumentException("Area requires a city reference.", nameof(AreaId));
        }

        // If a governorate snapshot is supplied, preserve its country context as well.
        if (GovernorateId.HasValue && !CountryId.HasValue)
        {
            throw new ArgumentException("Governorate requires a country reference.", nameof(GovernorateId));
        }
    }
}
