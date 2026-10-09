using System.Drawing.Printing;
using TransportERP.Contracts.Waybills;

namespace TransportERP.Desktop.Waybills;

public sealed partial class UcReadyToLoadWaybills : ShippingRtlControl
{

    public UcReadyToLoadWaybills()
    {
        InitializeComponent();
        TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    }

    public event Action<Guid, GenerateManifestRequest>? GenerateManifestRequested;

    public void Bind(IReadOnlyList<ReadyToLoadRow> rows)
    {
        _rows.DataSource = rows.ToList();
        SyncSelected();
    }

    private ReadyToLoadRow? Selected => _rows.CurrentRow?.DataBoundItem as ReadyToLoadRow;

    private void SyncSelected()
    {
        var selected = Selected;
        _message.Text = selected is null
            ? "لا يوجد سطر محدد."
            : $"الأولوية: {selected.Priority} — المخاطر: {RiskText(selected.RiskFlags)}";
    }

    private void RequestManifest()
    {
        var selected = Selected;
        if (selected is null) return;
        GenerateManifestRequested?.Invoke(
            selected.TripId,
            new GenerateManifestRequest(null, OperationId("desktop-manifest")));
    }

    private void btnManifest_Click(object? sender, EventArgs e) => RequestManifest();

    private void Grid_SelectionChanged(object? sender, EventArgs e) => SyncSelected();
}
