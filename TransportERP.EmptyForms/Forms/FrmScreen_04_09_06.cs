using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
public partial class UcScreen_04_09_06 : UserControl, IWorkspaceChangeState, IBatchFiveScreen, IExplicitScreenLayout
{
    public BatchFiveUiSession Binding { get; }
    public bool HasUnsavedChanges => Binding.HasPendingChanges;
    public event EventHandler? CloseRequested;
    public UcScreen_04_09_06()
    {
        InitializeComponent();
        ScreenProperties.Apply(this);
        SharedScreenProperties.Apply(this);
        Binding = new BatchFiveUiSession(this,lblStatus,validationErrors,
        new Dictionary<string,BatchFiveField>
        {
            ["bankAccountRef"] = new(field_bankAccountRef, true, false, false),
            ["fromDate"] = new(field_fromDate, true, false, false),
            ["toDate"] = new(field_toDate, true, false, false),
            ["statementBalance"] = new(field_statementBalance, true, true, false),
            ["bookBalance"] = new(field_bookBalance, false, false, true),
            ["difference"] = new(field_difference, false, false, true),
            ["unmatchedItems"] = new(field_unmatchedItems, false, false, true),
        }, dgvLines,new Dictionary<string,Button>
        {
            ["View"] = btnView,
            ["Create"] = btnCreate,
            ["Edit"] = btnEdit,
            ["Cancel"] = btnCancel,
            ["Match"] = btnMatch,
            ["Finalize"] = btnFinalize,
            ["Reopen"] = btnReopen,
        }, tabs,btnClear,null,null, new[] { "analyticalAccount", "accountName", "lineRatio", "foreignDebit", "foreignCredit" });
        Binding.ValidateCommand = action =>
        {
            if(field_fromDate.Checked && field_toDate.Checked && field_fromDate.Value.Date>field_toDate.Value.Date)
                return "تاريخ البداية يجب ألا يتجاوز تاريخ النهاية";
            return null;
        };
        btnClose.Click += (_,_) => { CloseRequested?.Invoke(this,EventArgs.Empty); };

    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
}
