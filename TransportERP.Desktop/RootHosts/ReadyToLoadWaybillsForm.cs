using System.Drawing.Printing;
using TransportERP.Contracts.Waybills;
using TransportERP.Desktop.CoreUI.Architecture;

namespace TransportERP.Desktop.Waybills;

/// <summary>SHP-023 — typed ready-to-load selection with text-first priority/risk information.</summary>
 
/// <summary>Compatibility window hosting the reusable business surface.</summary>
public sealed partial class ReadyToLoadWaybillsForm : ShippingRtlForm
{
    public UcReadyToLoadWaybills Content { get; } = new()
    {
        Dock = DockStyle.Fill
    };

    public ReadyToLoadWaybillsForm() : base("البوالص الجاهزة للتحميل — SHP-023", 980, 560)
    {
        Controls.Add(Content);
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }

    public event Action<Guid, GenerateManifestRequest>? GenerateManifestRequested { add => Content.GenerateManifestRequested += value; remove => Content.GenerateManifestRequested -= value; }

    public void Bind(IReadOnlyList<ReadyToLoadRow> rows) => Content.Bind(rows);
}
