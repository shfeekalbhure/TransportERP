using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
public partial class UcScreen_04_11_05 : UserControl, IWorkspaceChangeState, IBatchFiveScreen
{
    public ApprovalQueuePicker Queue => approvalQueue;
    public BatchFiveUiSession Binding { get; }
    public bool HasUnsavedChanges => Binding.HasPendingChanges;
    public event EventHandler? CloseRequested;
    public UcScreen_04_11_05()
    {
        InitializeComponent();
        ScreenProperties.Apply(this);
        SharedScreenProperties.Apply(this);
        Binding = new BatchFiveUiSession(this,lblStatus,validationErrors,
        new Dictionary<string,BatchFiveField>
        {
            ["requestNumber"] = new(field_requestNumber, false, false, true),
            ["documentTypeRef"] = new(field_documentTypeRef, false, false, true),
            ["linkedDocumentRef"] = new(field_linkedDocumentRef, false, false, true),
            ["requestedBy"] = new(field_requestedBy, false, false, true),
            ["requestedAt"] = new(field_requestedAt, false, false, true),
            ["documentAmount"] = new(field_documentAmount, false, false, true),
            ["currency"] = new(field_currency, false, false, true),
            ["requestReasonNote"] = new(field_requestReasonNote, false, false, true),
            ["approvalLevel"] = new(field_approvalLevel, false, false, true),
            ["decision"] = new(field_decision, false, false, false),
            ["decisionReason"] = new(field_decisionReason, false, false, false),
        }, null,new Dictionary<string,Button>
        {
            ["View"] = btnView,
            ["Execute"] = btnExecute,
            ["Approve"] = btnApprove,
            ["Reject"] = btnReject,
            ["Return"] = btnReturn,
        }, tabs,btnClear,null,null);
        approvalQueue.ConfirmNavigation = () => !Binding.IsBusy;
        approvalQueue.RequestSelected += async id => await Binding.ViewItemAsync(id);
        Binding.ValidateCommand = action =>
        {
            if(action is "Reject" or "Return" && string.IsNullOrWhiteSpace(field_decisionReason.Text))
            {
                tabs.SelectedTab=tp2;field_decisionReason.Focus();
                return "سبب القرار مطلوب للرفض أو الإرجاع";
            }
            return null;
        };
        btnClose.Click += (_,_) => { CloseRequested?.Invoke(this,EventArgs.Empty); };

    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
}
