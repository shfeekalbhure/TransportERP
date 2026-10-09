namespace TransportERP.Desktop.Forms.SystemSettings.General;

// Sole source: Scribd 973153595, ONYX ERP v8 تهيئة النظام, printed pp.23–24.
public sealed partial class UcCompanyData : UserControl, TransportERP.Desktop.CoreUI.IFoundationScreen, TransportERP.Desktop.CoreUI.IExplicitScreenLayout
{
    public UcCompanyData()
    {
        InitializeComponent();
        TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
        Load += (_, _) => OnyxPhaseOneProperties.Apply(this);
        foundation = new TransportERP.Desktop.CoreUI.FoundationUiSession(layout, bindDisabledActions: false);

        var tabs = (TabControl)Controls.Find("tabsFields", true)[0];
        var invoice = tabs.TabPages[2];
        // Keep the documented section visible in the preview, but unavailable without its prerequisite.
        invoice.Enabled = false;
        var main = (CheckBox)Controls.Find("chkMain", true)[0];
        var tax = (CheckBox)Controls.Find("chkTaxGroup", true)[0];
        tax.Enabled = false;
        main.CheckedChanged += (_, _) => { tax.Enabled = main.Checked; if (!main.Checked) tax.Checked = false; };
        TransportERP.Desktop.CoreUI.SharedScreenProperties.ApplyContent(this);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
    private readonly TransportERP.Desktop.CoreUI.FoundationUiSession foundation;
    [System.ComponentModel.Browsable(false), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public TransportERP.Desktop.CoreUI.FoundationUiSession Foundation => foundation;
    [System.ComponentModel.Browsable(false), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public bool HasUnsavedChanges => foundation.HasUnsavedChanges;
    [System.ComponentModel.Browsable(false), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public bool IsBusy => foundation.IsBusy;
    public bool ConfirmLeave() => foundation.ConfirmLeave();
    private void draftAction_Click(object? sender, EventArgs e)
    {
        fields.Enabled = true;
        sectionFields0.Enabled = true; sectionFields1.Enabled = true; sectionFields2.Enabled = true;

        standardCommandAdd.Enabled = false;
        var firstEditor = fields.Controls.Cast<Control>().FirstOrDefault(c => c.Enabled && (c is TextBox { ReadOnly: false } || c is CheckBox));
        if (firstEditor != null) firstEditor.Focus();

    }
    private void grid_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
    {
        if (sender is DataGridView { IsCurrentCellDirty: true, CurrentCell: DataGridViewCheckBoxCell } grid)
            grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }}
