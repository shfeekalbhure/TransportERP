using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
public partial class UcScreen_04_11_02 : UserControl, IWorkspaceChangeState, IBatchFiveScreen
{
    public BatchFiveUiSession Binding { get; }
    public bool HasUnsavedChanges => Binding.HasPendingChanges;
    public event EventHandler? CloseRequested;
    public UcScreen_04_11_02()
    {
        InitializeComponent();
        ScreenProperties.Apply(this);
        SharedScreenProperties.Apply(this);
        Binding = new BatchFiveUiSession(this,lblStatus,validationErrors,
        new Dictionary<string,BatchFiveField>
        {
            ["fiscalYearRef"] = new(field_fiscalYearRef, true, false, false),
            ["periodRef"] = new(field_periodRef, true, false, false),
            ["requestedAction"] = new(field_requestedAction, true, false, false),
            ["reason"] = new(field_reason, true, false, false),
            ["effectiveFrom"] = new(field_effectiveFrom, true, false, false),
            ["currentState"] = new(field_currentState, false, false, true),
            ["requestedBy"] = new(field_requestedBy, false, false, true),
            ["approvedBy"] = new(field_approvedBy, false, false, true),
            ["unpostedCheckResult"] = new(field_unpostedCheckResult, false, false, true),
        }, null,new Dictionary<string,Button>
        {
            ["View"] = btnView,
            ["Execute"] = btnExecute,
            ["Approve"] = btnApprove,
            ["Reject"] = btnReject,
            ["Return"] = btnReturn,
            ["Reopen"] = btnReopen,
        }, tabs,btnClear,null,null);
        btnClose.Click += (_,_) => { CloseRequested?.Invoke(this,EventArgs.Empty); };

    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
}
