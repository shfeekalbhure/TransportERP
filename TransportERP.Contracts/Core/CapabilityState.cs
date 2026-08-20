namespace TransportERP.Contracts.Core;

/// <summary>
/// Presentation state supplied by an authorized operation. It is not a permission model.
/// </summary>
public sealed record CapabilityState(bool IsVisible, bool IsEnabled, string? ReasonCode)
{
    public static CapabilityState Hidden { get; } = new(false, false, null);
    public static CapabilityState Enabled { get; } = new(true, true, null);

    public static CapabilityState Disabled(string reasonCode)
    {
        if (string.IsNullOrWhiteSpace(reasonCode))
        {
            throw new ArgumentException("A disabled capability requires a reason code.", nameof(reasonCode));
        }

        return new CapabilityState(true, false, reasonCode);
    }
}
