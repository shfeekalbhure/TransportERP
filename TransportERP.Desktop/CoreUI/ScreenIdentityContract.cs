using TransportERP.Desktop.CoreUI.Architecture;

namespace TransportERP.Desktop.CoreUI;

/// <summary>W0 identity core only; it is not the complete DEC-015 Screen Contract.</summary>
public sealed record ScreenIdentityContract(
    string ScreenId, string ArabicName, string LocalizationKey,
    TransportScreenProfile Profile, IReadOnlyList<string> PermissionIds)
{
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(ScreenId) || string.IsNullOrWhiteSpace(ArabicName) || string.IsNullOrWhiteSpace(LocalizationKey))
            throw new InvalidOperationException("Screen identity and localization key are required.");
    }
}
