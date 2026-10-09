using System.Drawing.Printing;
using TransportERP.Contracts.Waybills;
using TransportERP.Desktop.CoreUI.Architecture;

namespace TransportERP.Desktop.Waybills;

/// <summary>SHP-016 — W3 allocation planning with released remaining, route and quantity input.</summary>
 
/// <summary>Compatibility window hosting the reusable business surface.</summary>
public sealed partial class TripAllocationForm : ShippingRtlForm
{
    public UcTripAllocation Content { get; } = new()
    {
        Dock = DockStyle.Fill
    };

    public TripAllocationForm() : base("توزيع الأصناف على الرحلات — SHP-016", 980, 600)
    {
        Controls.Add(Content);
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }

    public event Action<Guid, AllocateItemRequest>? AllocateRequested { add => Content.AllocateRequested += value; remove => Content.AllocateRequested -= value; }

    public event Action<Guid, UnallocateRequest>? UnallocateRequested { add => Content.UnallocateRequested += value; remove => Content.UnallocateRequested -= value; }

    public void Bind(IReadOnlyList<TripAllocationPlanningRow> rows) => Content.Bind(rows);
    public TripAllocationPlanningRow? Selected => Content.Selected;
}
