namespace TransportERP.Contracts.Party;

public enum PartyRole
{
    Sender,
    Receiver,
    Payer,
    Other
}

/// <summary>
/// Immutable address snapshot captured on a business document.
/// Lookup identities remain optional so imported or field-entered addresses can still be preserved.
/// </summary>
public sealed record AddressSnapshot(
    Guid? CountryId,
    Guid? GovernorateId,
    Guid? DirectorateId,
    Guid? CityId,
    Guid? AreaId,
    string? AddressLine);

/// <summary>
/// Operational party snapshot. An operational party is not automatically an accounting account.
/// </summary>
public sealed record OperationalPartySnapshot(
    Guid? PartyId,
    string? PartyNo,
    string Name,
    string Mobile,
    string? IdentityType,
    string? IdentityNo,
    AddressSnapshot Address)
{
    public void EnsureValid()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            throw new ArgumentException("Party name is required.", nameof(Name));
        }

        if (string.IsNullOrWhiteSpace(Mobile))
        {
            throw new ArgumentException("Party mobile is required.", nameof(Mobile));
        }

        if (!string.IsNullOrWhiteSpace(IdentityNo) && string.IsNullOrWhiteSpace(IdentityType))
        {
            throw new ArgumentException("Identity type is required when an identity number is supplied.", nameof(IdentityType));
        }
    }
}

public sealed record WaybillPartySnapshot(PartyRole Role, OperationalPartySnapshot Party)
{
    public void EnsureValid()
    {
        if (!Enum.IsDefined(Role))
        {
            throw new ArgumentException("A defined party role is required.", nameof(Role));
        }
        Party.EnsureValid();
    }
}
