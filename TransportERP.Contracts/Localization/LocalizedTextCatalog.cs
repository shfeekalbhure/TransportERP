namespace TransportERP.Contracts.Localization;

/// <summary>Resource-backed text lookup; technical values remain values, never translated business data.</summary>
public interface ILocalizedTextCatalog
{
    string Get(string key, string cultureName);
}
