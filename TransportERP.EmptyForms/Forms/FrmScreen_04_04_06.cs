using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
public partial class UcScreen_04_04_06 : UserControl, IWorkspaceChangeState, IBatchFiveScreen
{
    public BatchFiveUiSession Binding { get; }
    public bool HasUnsavedChanges => Binding.HasPendingChanges;
    public event EventHandler? CloseRequested;
    public UcScreen_04_04_06()
    {
        InitializeComponent();
        ScreenProperties.Apply(this);
        SharedScreenProperties.Apply(this);
        Binding = new BatchFiveUiSession(this,lblStatus,validationErrors,
        new Dictionary<string,BatchFiveField>
        {
            ["voucherNumber"] = new(field_voucherNumber, false, false, true),
            ["voucherDate"] = new(field_voucherDate, true, false, false),
            ["partyRef"] = new(field_partyRef, false, false, false),
            ["sourceCashBankRef"] = new(field_sourceCashBankRef, false, false, false),
            ["destinationCashBankRef"] = new(field_destinationCashBankRef, false, false, false),
            ["currencyRef"] = new(field_currencyRef, true, false, false),
            ["amount"] = new(field_amount, true, true, false),
            ["exchangeRate"] = new(field_exchangeRate, true, true, false),
            ["counterAccountRef"] = new(field_counterAccountRef, true, false, false),
            ["description"] = new(field_description, true, false, false),
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
