using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
public partial class UcScreen_04_10_05 : UserControl, IWorkspaceChangeState, IBatchFiveScreen
{
    public BatchFiveUiSession Binding { get; }
    public bool HasUnsavedChanges => Binding.HasPendingChanges;
    public event EventHandler? CloseRequested;
    public UcScreen_04_10_05()
    {
        InitializeComponent();
        ScreenProperties.Apply(this);
        SharedScreenProperties.Apply(this);
        Binding = new BatchFiveUiSession(this,lblStatus,validationErrors,
        new Dictionary<string,BatchFiveField>
        {
            ["T04_EV_0413"] = new(field_T04_EV_0413, false, false, true),
            ["T04_EV_0415"] = new(field_T04_EV_0415, false, false, false),
            ["T04_EV_0416"] = new(field_T04_EV_0416, false, false, false),
            ["T04_EV_0417"] = new(field_T04_EV_0417, false, false, false),
            ["R05_AT_0304"] = new(field_R05_AT_0304, false, false, false),
            ["R05_AT_0305"] = new(field_R05_AT_0305, false, false, true),
        }, dgvLines,new Dictionary<string,Button>
        {
            ["Save"] = btnSave,
        }, tabs,btnClear,btnAddRow,btnRemoveRow);
        btnClose.Click += (_,_) => { CloseRequested?.Invoke(this,EventArgs.Empty); };

    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
}
