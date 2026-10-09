using System.Drawing.Printing;
using TransportERP.Contracts.Waybills;
using TransportERP.Desktop.CoreUI.Architecture;

namespace TransportERP.Desktop.Waybills;

/// <summary>SHP-024 — typed capacity/allocation plan. Weight and volume are server-proportional allocation measures.</summary>
 
/// <summary>Compatibility window hosting the reusable business surface.</summary>
public sealed partial class LoadPlanningForm : ShippingRtlForm
{
    public UcLoadPlanning Content { get; } = new()
    {
        Dock = DockStyle.Fill
    };

    public LoadPlanningForm() : base("تخطيط الحمولة — SHP-024", 1120, 620)
    {
        Controls.Add(Content);
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }

    public event Action<Guid, AllocateItemRequest>? AllocateRequested { add => Content.AllocateRequested += value; remove => Content.AllocateRequested -= value; }

    public event Action<Guid, GenerateManifestRequest>? GenerateManifestRequested { add => Content.GenerateManifestRequested += value; remove => Content.GenerateManifestRequested -= value; }

    public void Bind(IReadOnlyList<LoadPlanningRow> rows) => Content.Bind(rows);
    public LoadPlanningRow? Selected => Content.Selected;
}
