using TransportERP.Contracts.Waybills;

namespace TransportERP.Desktop.Waybills;

public sealed partial class UcWaybillCollections : UserControl
{
    public UcWaybillCollections()
    {
        InitializeComponent();
        TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    public event EventHandler? RecordRequested;

    public event EventHandler? ReverseRequested;

    public void Bind(IReadOnlyList<CollectionResponse> items)
    {
        _collections.DataSource = items.ToList();
        _state.Text = items.Count == 0 ? "لا توجد تحصيلات" : $"عدد الحركات: {items.Count}";
    }

    public CollectionResponse? SelectedCollection
        => _collections.CurrentRow?.DataBoundItem as CollectionResponse;

    private void record_Click(object? sender, EventArgs e) => RecordRequested?.Invoke(this, EventArgs.Empty);
    private void reverse_Click(object? sender, EventArgs e) => ReverseRequested?.Invoke(this, EventArgs.Empty);
}
