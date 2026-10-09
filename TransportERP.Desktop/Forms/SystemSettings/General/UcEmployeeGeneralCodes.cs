namespace TransportERP.Desktop.Forms.SystemSettings.General;

// Sole source: https://www.scribd.com/document/973153595/ONYX-ERP-v8-%D8%AA%D9%87%D9%8A%D8%A6%D8%A9-%D8%A7%D9%84%D9%86%D8%B8%D8%A7%D9%85
// Printed pp.38–39: code-type selection, subcode/name/foreign name, conditional employee-number
// bounds, ordering, notes, stopped flag/reason. System-provided code types are not fabricated.
public sealed partial class UcEmployeeGeneralCodes : UserControl, TransportERP.Desktop.CoreUI.IFoundationScreen, TransportERP.Desktop.CoreUI.IExplicitScreenLayout
{
    public UcEmployeeGeneralCodes()
    {
        InitializeComponent();
        TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
        Load += (_, _) => OnyxPhaseOneProperties.Apply(this);
        foundation = new TransportERP.Desktop.CoreUI.FoundationUiSession(layout, bindDisabledActions: false);
        // Source makes these dependent on both the general numbering mode and selected code type.
        // Neither prerequisite is connected, so do not pretend the conditional fields are active.
        var grid = (DataGridView)Controls.Find("gridEmployeeCodes", true)[0];
        grid.Columns["colEmployeeSequenceStart"]!.Visible = false;
        grid.Columns["colEmployeeSequenceEnd"]!.Visible = false;
    
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
        gridEmployeeCodes.Enabled = true;
        standardCommandAdd.Enabled = false;
        var firstEditor = fields.Controls.Cast<Control>().FirstOrDefault(c => c.Enabled && (c is TextBox { ReadOnly: false } || c is CheckBox));
        if (firstEditor != null) firstEditor.Focus();
        else gridEmployeeCodes.Focus();
    }
    private void grid_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
    {
        if (sender is DataGridView { IsCurrentCellDirty: true, CurrentCell: DataGridViewCheckBoxCell } grid)
            grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }}
