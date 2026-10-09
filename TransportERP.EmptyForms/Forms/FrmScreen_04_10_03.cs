using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
public partial class UcScreen_04_10_03 : UserControl, IWorkspaceChangeState, IBatchFiveScreen
{
    public BatchFiveUiSession Binding { get; }
    public bool HasUnsavedChanges => Binding.HasPendingChanges;
    public event EventHandler? CloseRequested;
    public UcScreen_04_10_03()
    {
        InitializeComponent();
        ScreenProperties.Apply(this);
        SharedScreenProperties.Apply(this);
        Binding = new BatchFiveUiSession(this,lblStatus,validationErrors,
        new Dictionary<string,BatchFiveField>
        {
            ["T04_EV_0383"] = new(field_T04_EV_0383, false, false, false),
            ["T04_EV_0384"] = new(field_T04_EV_0384, false, false, true),
            ["T04_EV_0385"] = new(field_T04_EV_0385, false, false, false),
            ["T04_EV_0386"] = new(field_T04_EV_0386, false, false, false),
            ["T04_EV_0387"] = new(field_T04_EV_0387, false, false, false),
            ["T04_EV_0388"] = new(field_T04_EV_0388, false, false, false),
            ["T04_EV_0391"] = new(field_T04_EV_0391, false, false, false),
            ["T04_EV_0392"] = new(field_T04_EV_0392, false, false, true),
            ["T04_EV_0393"] = new(field_T04_EV_0393, false, false, true),
            ["actualAmount"] = new(field_actualAmount, false, false, true),
            ["actualCurrency"] = new(field_actualCurrency, false, false, true),
        }, dgvLines,new Dictionary<string,Button>
        {
            ["Save"] = btnSave,
            ["ReviewDocuments"] = btnReviewDocuments,
            ["LoadSource"] = btnLoadSource,
        }, tabs,btnClear,btnAddRow,btnRemoveRow);
        btnClose.Click += (_,_) => { CloseRequested?.Invoke(this,EventArgs.Empty); };

    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
}
