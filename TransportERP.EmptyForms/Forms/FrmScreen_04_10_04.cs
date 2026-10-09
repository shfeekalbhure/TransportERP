using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
public partial class UcScreen_04_10_04 : UserControl, IWorkspaceChangeState, IBatchFiveScreen
{
    public BatchFiveUiSession Binding { get; }
    public bool HasUnsavedChanges => Binding.HasPendingChanges;
    public event EventHandler? CloseRequested;
    public UcScreen_04_10_04()
    {
        InitializeComponent();
        ScreenProperties.Apply(this);
        SharedScreenProperties.Apply(this);
        Binding = new BatchFiveUiSession(this,lblStatus,validationErrors,
        new Dictionary<string,BatchFiveField>
        {
            ["T04_EV_0399"] = new(field_T04_EV_0399, false, false, false),
            ["T04_EV_0400"] = new(field_T04_EV_0400, false, false, true),
            ["T04_EV_0401"] = new(field_T04_EV_0401, false, false, false),
            ["T04_EV_0402"] = new(field_T04_EV_0402, false, false, false),
            ["T04_EV_0403"] = new(field_T04_EV_0403, false, false, false),
            ["T04_EV_0404"] = new(field_T04_EV_0404, false, false, false),
            ["T04_EV_0407"] = new(field_T04_EV_0407, false, false, true),
            ["T04_EV_0408"] = new(field_T04_EV_0408, false, false, true),
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
