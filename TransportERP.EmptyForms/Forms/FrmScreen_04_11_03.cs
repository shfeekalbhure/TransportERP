using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
public partial class UcScreen_04_11_03 : UserControl, IWorkspaceChangeState, IBatchFiveScreen
{
    public BatchFiveUiSession Binding { get; }
    public bool HasUnsavedChanges => Binding.HasPendingChanges;
    public event EventHandler? CloseRequested;
    public UcScreen_04_11_03()
    {
        InitializeComponent();
        ScreenProperties.Apply(this);
        SharedScreenProperties.Apply(this);
        Binding = new BatchFiveUiSession(this,lblStatus,validationErrors,
        new Dictionary<string,BatchFiveField>
        {
            ["documentNumber"] = new(field_documentNumber, false, false, true),
            ["accountingDate"] = new(field_accountingDate, true, false, false),
            ["reference"] = new(field_reference, false, false, false),
            ["description"] = new(field_description, true, false, false),
            ["currencyRef"] = new(field_currencyRef, true, false, false),
            ["exchangeRate"] = new(field_exchangeRate, true, true, false),
            ["state"] = new(field_state, false, false, true),
        }, dgvLines,new Dictionary<string,Button>
        {
            ["View"] = btnView,
            ["Create"] = btnCreate,
            ["Edit"] = btnEdit,
            ["Cancel"] = btnCancel,
            ["Post"] = btnPost,
            ["Reverse"] = btnReverse,
        }, tabs,btnClear,btnAddRow,btnRemoveRow);
        btnClose.Click += (_,_) => { CloseRequested?.Invoke(this,EventArgs.Empty); };

    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
}
