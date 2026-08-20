using TransportERP.Contracts.Geo;

namespace TransportERP.Contracts.Party;

public enum PartyRole
{
    Sender,
    Receiver,
    Payer,
    Other
}

/// <summary>
/// Operational party snapshot. An operational party is not automatically an accounting account.
/// Address data uses the single governed GeoAddressSnapshot contract.
/// </summary>
public sealed record OperationalPartySnapshot(
    Guid? PartyId,
    string? PartyNo,
    string Name,
    string Mobile,
    string? IdentityType,
    string? IdentityNo,
    GeoAddressSnapshot Address)
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

        Address.EnsureUsable();
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
