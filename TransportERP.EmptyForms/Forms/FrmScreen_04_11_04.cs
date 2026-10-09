using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
public partial class UcScreen_04_11_04 : UserControl, IWorkspaceChangeState, IBatchFiveScreen
{
    public BatchFiveUiSession Binding { get; }
    public bool HasUnsavedChanges => Binding.HasPendingChanges;
    public event EventHandler? CloseRequested;
    public void SetReversingRows(IEnumerable<IReadOnlyDictionary<string,object?>> rows)
    {
        var items=rows.ToArray();
        foreach(var row in items)foreach(string key in row.Keys)
            if(!dgvReversing.Columns.Contains(key))throw new ArgumentException("Unknown reversal column: "+key);
        dgvReversing.Rows.Clear();
        foreach(var row in items)
        {
            int i=dgvReversing.Rows.Add();
            foreach(var cell in row)dgvReversing.Rows[i].Cells[cell.Key].Value=cell.Value;
        }
    }
    public UcScreen_04_11_04()
    {
        InitializeComponent();
        ScreenProperties.Apply(this);
        SharedScreenProperties.Apply(this);
        Binding = new BatchFiveUiSession(this,lblStatus,validationErrors,
        new Dictionary<string,BatchFiveField>
        {
            ["reversalRequestNumber"] = new(field_reversalRequestNumber, false, false, true),
            ["originalEntryRef"] = new(field_originalEntryRef, true, false, false),
            ["originalEntryNumber"] = new(field_originalEntryNumber, false, false, true),
            ["originalEntryDate"] = new(field_originalEntryDate, false, false, true),
            ["reversalReason"] = new(field_reversalReason, true, false, false),
            ["reversalDate"] = new(field_reversalDate, true, false, false),
            ["reversalPolicy"] = new(field_reversalPolicy, true, false, false),
            ["reversingEntryRef"] = new(field_reversingEntryRef, false, false, true),
            ["reversingEntryNumber"] = new(field_reversingEntryNumber, false, false, true),
            ["reversalState"] = new(field_reversalState, false, false, true),
        }, dgvLines,new Dictionary<string,Button>
        {
            ["View"] = btnView,
            ["Create"] = btnCreate,
            ["Edit"] = btnEdit,
            ["Cancel"] = btnCancel,
            ["Reverse"] = btnReverse,
        }, tabs,btnClear,null,null);
        Binding.DocumentLoaded += () => dgvReversing.Rows.Clear();
        btnClose.Click += (_,_) => { CloseRequested?.Invoke(this,EventArgs.Empty); };

    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
}
