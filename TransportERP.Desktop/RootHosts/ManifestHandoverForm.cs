using System.Drawing.Printing;
using TransportERP.Contracts.Waybills;
using TransportERP.Desktop.CoreUI.Architecture;

namespace TransportERP.Desktop.Waybills;

/// <summary>SHP-029 — custody summary with driver/vehicle/totals/acceptance time and governed handover request.</summary>
 
/// <summary>Compatibility window hosting the reusable business surface.</summary>
public sealed partial class ManifestHandoverForm : ShippingRtlForm
{
    public UcManifestHandover Content { get; } = new()
    {
        Dock = DockStyle.Fill
    };

    public ManifestHandoverForm() : base("تسليم عهدة الحمولة للسائق — SHP-029", 800, 460)
    {
        Controls.Add(Content);
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }

    public event Action<Guid, HandoverManifestRequest>? AcceptRequested { add => Content.AcceptRequested += value; remove => Content.AcceptRequested -= value; }

    public void Bind(ManifestScreenState state) => Content.Bind(state);
}
