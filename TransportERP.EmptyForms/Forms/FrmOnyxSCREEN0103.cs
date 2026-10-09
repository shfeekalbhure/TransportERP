using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
public partial class UcOnyxSCREEN0103 : UserControl, IWorkspaceChangeState, IBatchFiveScreen
{
    public BatchFiveUiSession Binding { get; }
    public bool HasUnsavedChanges => Binding.HasPendingChanges;
    public event EventHandler? CloseRequested;
    public UcOnyxSCREEN0103()
    {
        InitializeComponent();
        ScreenProperties.Apply(this);
        SharedScreenProperties.Apply(this);
        Binding = new BatchFiveUiSession(this,lblStatus,validationErrors,
        new Dictionary<string,BatchFiveField>
        {
            ["bankRef"] = new(field_bankRef, false, false, false),
            ["fromDate"] = new(field_fromDate, false, false, false),
            ["toDate"] = new(field_toDate, false, false, false),
        }, dgvLines,new Dictionary<string,Button>
        {
            ["View"] = btnView,
            ["Save"] = btnSave,
        }, tabs,btnClear,null,null);
        btnClose.Click += (_,_) => { CloseRequested?.Invoke(this,EventArgs.Empty); };

    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
}
