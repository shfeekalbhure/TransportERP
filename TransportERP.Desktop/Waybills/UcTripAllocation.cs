using System.Drawing.Printing;
using TransportERP.Contracts.Waybills;

namespace TransportERP.Desktop.Waybills;

public sealed partial class UcTripAllocation : ShippingRtlControl
{

    public UcTripAllocation()
    {
        InitializeComponent();
        TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    public event Action<Guid, AllocateItemRequest>? AllocateRequested;
    public event Action<Guid, UnallocateRequest>? UnallocateRequested;

    public void Bind(IReadOnlyList<TripAllocationPlanningRow> rows)
    {
        _rows.DataSource = rows.ToList();
        SyncSelected();
    }

    public TripAllocationPlanningRow? Selected => _rows.CurrentRow?.DataBoundItem as TripAllocationPlanningRow;

    private void SyncSelected()
    {
        var selected = Selected;
        if (selected is null)
        {
            _allocateQty.Maximum = 0m;
            _allocateQty.Value = 0m;
            return;
        }

        _allocateQty.Maximum = Math.Max(0m, selected.ReleasedRemaining);
        if (_allocateQty.Value > _allocateQty.Maximum)
            _allocateQty.Value = _allocateQty.Maximum;
        _message.Text = $"المسار: {selected.Route} — الحالة: {selected.AllocationStatus}";
    }

    private void RequestAllocate()
    {
        var selected = Selected;
        if (selected is null || _allocateQty.Value <= 0m)
        {
            _message.Text = "اختر سطراً وأدخل كمية تخصيص موجبة.";
            return;
        }

        AllocateRequested?.Invoke(
            selected.TripId,
            new AllocateItemRequest(
                selected.WaybillItemId,
                selected.ReleaseId,
                _allocateQty.Value,
                OperationId("desktop-allocate")));
    }

    private void RequestUnallocate()
    {
        var selected = Selected;
        if (selected?.AllocationId is not Guid allocationId)
        {
            _message.Text = "السطر المحدد لا يمثل تخصيصاً قابلاً للفك.";
            return;
        }

        if (string.IsNullOrWhiteSpace(_reason.Text))
        {
            _message.Text = "سبب فك التخصيص مطلوب.";
            return;
        }

        UnallocateRequested?.Invoke(
            allocationId,
            new UnallocateRequest(_reason.Text.Trim(), OperationId("desktop-unallocate")));
    }

    private void btnAllocate_Click(object? sender, EventArgs e) => RequestAllocate();

    private void btnUnallocate_Click(object? sender, EventArgs e) => RequestUnallocate();

    private void Grid_SelectionChanged(object? sender, EventArgs e) => SyncSelected();
}
