namespace TransportERP.Desktop.Forms.SystemSettings.General;

// Sole source: https://www.scribd.com/document/973153595/ONYX-ERP-v8-%D8%AA%D9%87%D9%8A%D8%A6%D8%A9-%D8%A7%D9%84%D9%86%D8%B8%D8%A7%D9%85
// Printed pp.46–47: main fields, explicit tax-type choices, derived exemption,
// conditional transition method, tax types and branches tabs with activation/transition dates.
// Date cells are temporary text; tax calculation, date validation and data links are unconnected.
public sealed partial class UcTaxCalculationMethods : UserControl, TransportERP.Desktop.CoreUI.IFoundationScreen, TransportERP.Desktop.CoreUI.IExplicitScreenLayout
{
    public UcTaxCalculationMethods()
    {
        InitializeComponent();
        TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
        Load += (_, _) => OnyxPhaseOneProperties.Apply(this);
        foundation = new TransportERP.Desktop.CoreUI.FoundationUiSession(layout, bindDisabledActions: false);
        var type = (ComboBox)Controls.Find("cboTaxType", true)[0];
        var exempt = (CheckBox)Controls.Find("chkExempt", true)[0];
        type.SelectedIndexChanged += (_, _) => exempt.Checked = type.SelectedIndex > 0;
        var transition = (CheckBox)Controls.Find("chkTransition", true)[0];
        void UpdateTransition()
        {
            Controls.Find("cboTransitionMethod", true)[0].Visible = transition.Checked;
            Controls.Find("lblcboTransitionMethod", true)[0].Visible = transition.Checked;
        }
        transition.CheckedChanged += (_, _) => UpdateTransition();
        UpdateTransition();
    
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
        gridTaxTypes.Enabled = true;
        gridBranches.Enabled = true;
        standardCommandAdd.Enabled = false;
        var firstEditor = fields.Controls.Cast<Control>().FirstOrDefault(c => c.Enabled && (c is TextBox { ReadOnly: false } || c is CheckBox));
        if (firstEditor != null) firstEditor.Focus();
        else gridTaxTypes.Focus();
    }
    private void grid_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
    {
        if (sender is DataGridView { IsCurrentCellDirty: true, CurrentCell: DataGridViewCheckBoxCell } grid)
            grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }}
