namespace TransportERP.Desktop.Forms.SystemSettings.General;

// Sole source: https://www.scribd.com/document/973153595/ONYX-ERP-v8-%D8%AA%D9%87%D9%8A%D8%A6%D8%A9-%D8%A7%D9%84%D9%86%D8%B8%D8%A7%D9%85
// Printed pp.62–63. Data-entry screen, distinct from the type/group setup on pp.30–31.
// Configurable extra fields, project linking and Excel import have no connected data/service.
public sealed partial class UcCostCenterData : UserControl, TransportERP.Desktop.CoreUI.IFoundationScreen, TransportERP.Desktop.CoreUI.IExplicitScreenLayout
{
    public UcCostCenterData()
    {
        InitializeComponent();
        TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
        Load += (_, _) => OnyxPhaseOneProperties.Apply(this);
        lstbtnSelectProjects.Name = string.Empty;
        foundation = new TransportERP.Desktop.CoreUI.FoundationUiSession(layout, bindDisabledActions: false);
        lstbtnSelectProjects.Name = "lstbtnSelectProjects";

        Controls.Find("cboUseProjects", true)[0].AccessibleDescription =
            "اختيار مؤقت لطبيعة الربط؛ تفعيل الربط العام وقوائم المشاريع غير مرتبطين ولا تُطبّق القيم على النظام.";
        var tabs = (TabControl)Controls.Find("tabsFields", true)[0];
        tabs.Multiline = true;
        tabs.Padding = new Point(12, 6);
        btnSelectProjects.Click += (_, _) => grpbtnSelectProjects.Visible = !grpbtnSelectProjects.Visible;
        SourceSetupLayout.AddToolbarAction(this, "btnSelectProjects", "المشاريع");
        SourceSetupLayout.ApplyReviewedProperties(this);
    
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
