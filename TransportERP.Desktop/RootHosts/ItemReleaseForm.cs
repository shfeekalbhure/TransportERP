using System.Drawing.Printing;
using TransportERP.Contracts.Waybills;
using TransportERP.Desktop.CoreUI.Architecture;

namespace TransportERP.Desktop.Waybills;

/// <summary>SHP-015 — W3 release identities, quantities and hold state.</summary>
 
/// <summary>Compatibility window hosting the reusable business surface.</summary>
public sealed partial class ItemReleaseForm : ShippingRtlForm
{
    public UcItemRelease Content { get; } = new()
    {
        Dock = DockStyle.Fill
    };

    public ItemReleaseForm() : base("إطلاق كميات الأصناف — SHP-015", 760, 520)
    {
        Controls.Add(Content);
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }

    public event Action<Guid, Guid, ReleaseItemRequest>? ReleaseRequested { add => Content.ReleaseRequested += value; remove => Content.ReleaseRequested -= value; }

    public void Bind(ItemReleaseScreenState state) => Content.Bind(state);
    public void Bind(ItemQuantityStateResponse value) => Content.Bind(value);
}
