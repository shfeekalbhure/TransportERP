using System.Drawing.Printing;
using TransportERP.Contracts.Waybills;
using TransportERP.Desktop.CoreUI.Architecture;

namespace TransportERP.Desktop.Waybills;

/// <summary>SHP-027 — load quantity input with explicit resource-risk confirmation.</summary>
 
/// <summary>Compatibility window hosting the reusable business surface.</summary>
public sealed partial class ManifestLoadingForm : ShippingRtlForm
{
    public UcManifestLoading Content { get; } = new()
    {
        Dock = DockStyle.Fill
    };

    public ManifestLoadingForm() : base("تحميل الرحلة — SHP-027", 1000, 600)
    {
        Controls.Add(Content);
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }

    public event Action<Guid, Guid, LoadManifestLineRequest>? LoadRequested { add => Content.LoadRequested += value; remove => Content.LoadRequested -= value; }

    public void Bind(ManifestResponse manifest, IReadOnlyList<ManifestLoadingRow> rows) => Content.Bind(manifest, rows);
    public void Bind(ManifestResponse manifest) => Content.Bind(manifest);
    public ManifestLoadingRow? SelectedLine => Content.SelectedLine;
}
