using System.Drawing.Printing;
using TransportERP.Contracts.Waybills;
using TransportERP.Desktop.CoreUI.Architecture;

namespace TransportERP.Desktop.Waybills;

/// <summary>SHP-028 — manifest, driver/vehicle, proportional physical totals and printable RTL representation.</summary>
 
/// <summary>Compatibility window hosting the reusable business surface.</summary>
public sealed partial class ManifestForm : ShippingRtlForm
{
    public UcManifest Content { get; } = new()
    {
        Dock = DockStyle.Fill
    };

    public ManifestForm() : base("كشف الحمولة Manifest — SHP-028", 1040, 700)
    {
        Controls.Add(Content);
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }

    public event Action<Guid, FinalizeManifestRequest>? FinalizeRequested { add => Content.FinalizeRequested += value; remove => Content.FinalizeRequested -= value; }

    public void Bind(ManifestScreenState state) => Content.Bind(state);
    public void Bind(ManifestResponse manifest) => Content.Bind(manifest);
}
