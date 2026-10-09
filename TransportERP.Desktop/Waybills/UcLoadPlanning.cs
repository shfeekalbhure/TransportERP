using System.Drawing.Printing;
using TransportERP.Contracts.Waybills;

namespace TransportERP.Desktop.Waybills;

public sealed partial class UcLoadPlanning : ShippingRtlControl
{

    public UcLoadPlanning()
    {
        InitializeComponent();
        TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    public event Action<Guid, AllocateItemRequest>? AllocateRequested;
    public event Action<Guid, GenerateManifestRequest>? GenerateManifestRequested;

    public void Bind(IReadOnlyList<LoadPlanningRow> rows)
    {
        _rows.DataSource = rows.ToList();
        SyncSelected();
    }

    public LoadPlanningRow? Selected => _rows.CurrentRow?.DataBoundItem as LoadPlanningRow;

    private void SyncSelected()
    {
        var selected = Selected;
        if (selected is null)
        {
            _capacity.Text = "لا توجد معلومات سعة/مخاطر محددة.";
            _allocateQty.Maximum = 0m;
            _allocateQty.Value = 0m;
            return;
        }

        _allocateQty.Maximum = Math.Max(0m, selected.ReleasedRemaining);
        if (_allocateQty.Value > _allocateQty.Maximum)
            _allocateQty.Value = _allocateQty.Maximum;

        _capacity.Text =
            $"السعة: {selected.Capacity} — الوزن المخصص: {selected.AllocatedWeight:N3}/{CapacityValue(selected.CapacityWeight)}" +
            $" — الحجم المخصص: {selected.AllocatedVolume:N3}/{CapacityValue(selected.CapacityVolume)}" +
            $" — {selected.CapacityStatus} — الأولوية: {selected.Priority} — المخاطر: {RiskText(selected.RiskFlags)}";
    }

    private static string CapacityValue(decimal value) => value > 0m ? value.ToString("N3") : "غير محددة";

    private void RequestAllocate()
    {
        var selected = Selected;
        if (selected is null || _allocateQty.Value <= 0m) return;
        AllocateRequested?.Invoke(
            selected.TripId,
            new AllocateItemRequest(
                selected.WaybillItemId,
                selected.ReleaseId,
                _allocateQty.Value,
                OperationId("desktop-plan-allocate")));
    }

    private void RequestManifest()
    {
        var selected = Selected;
        if (selected is null) return;
        GenerateManifestRequested?.Invoke(
            selected.TripId,
            new GenerateManifestRequest(null, OperationId("desktop-plan-manifest")));
    }

    private void btnManifest_Click(object? sender, EventArgs e) => RequestManifest();

    private void btnAllocate_Click(object? sender, EventArgs e) => RequestAllocate();

    private void Grid_SelectionChanged(object? sender, EventArgs e) => SyncSelected();
}
