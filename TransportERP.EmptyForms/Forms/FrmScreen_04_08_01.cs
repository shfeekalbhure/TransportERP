using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
public partial class UcScreen_04_08_01 : UserControl, IWorkspaceChangeState, IBatchFiveScreen
{
    public BatchFiveUiSession Binding { get; }
    public bool HasUnsavedChanges => Binding.HasPendingChanges;
    public event EventHandler? CloseRequested;
    public UcScreen_04_08_01()
    {
        InitializeComponent();
        // Preserve this screen's designer font and background after shared setup.
        var designerFont = Font;
        var designerBackColor = BackColor;
        ScreenProperties.Apply(this);
        Font = designerFont;
        BackColor = designerBackColor;
        Binding = new BatchFiveUiSession(this,lblStatus,validationErrors,
        new Dictionary<string,BatchFiveField>
        {
            ["noteNumber"] = new(field_noteNumber, false, false, true),
            ["noteDate"] = new(field_noteDate, false, false, false),
            ["partyRef"] = new(field_partyRef, false, false, false),
            ["originalDocumentRef"] = new(field_originalDocumentRef, false, false, false),
            ["reason"] = new(field_reason, true, false, false),
            ["currencyRef"] = new(field_currencyRef, false, false, false),
            ["state"] = new(field_state, false, false, true),
            ["baseAmount"] = new(field_baseAmount, false, true, false),
            ["taxAmount"] = new(field_taxAmount, false, true, false),
            ["totalAmount"] = new(field_totalAmount, false, false, true),
            ["partyAccountRef"] = new(field_partyAccountRef, false, false, false),
            ["counterAccountRef"] = new(field_counterAccountRef, false, false, false),
            ["costCenterRef"] = new(field_costCenterRef, false, false, false),
            ["allocationContext"] = new(field_allocationContext, false, false, true),
            ["balanceContext"] = new(field_balanceContext, false, false, true),
        }, dgvLines,new Dictionary<string,Button>
        {
            ["View"] = btnView,
            ["Create"] = btnCreate,
            ["Edit"] = btnEdit,
            ["Cancel"] = btnCancel,
            ["Post"] = btnPost,
            ["Reverse"] = btnReverse,
        }, mainLayout,btnClear,btnAddRow,btnRemoveRow);
        btnClose.Click += (_,_) => { CloseRequested?.Invoke(this,EventArgs.Empty); };

    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
}
