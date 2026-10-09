namespace TransportERP.Desktop.Forms.SystemSettings.General;

// Text-only reconstruction, pp.71–85 of the sole Scribd source referenced in the Fields partial.
public sealed partial class UcEmployeeData : UserControl, TransportERP.Desktop.CoreUI.IFoundationScreen, TransportERP.Desktop.CoreUI.IExplicitScreenLayout
{
    public UcEmployeeData()
    {
        InitializeComponent();
        TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
        Load += (_, _) => OnyxPhaseOneProperties.Apply(this);
        foundation = new TransportERP.Desktop.CoreUI.FoundationUiSession(layout, bindDisabledActions: false);
        var tabs = tabsFields;
        var grids = new Dictionary<string, DataGridView> { ["gridPermanent"] = gridPermanent, ["gridLeave"] = gridLeave, ["gridDocuments"] = gridDocuments, ["gridDependents"] = gridDependents, ["gridGuarantors"] = gridGuarantors, ["gridSponsors"] = gridSponsors, ["gridBanks"] = gridBanks, ["gridItemChanges"] = gridItemChanges, ["gridAssets"] = gridAssets, ["gridCompanions"] = gridCompanions, ["gridQualifications"] = gridQualifications };
        // The source requires employee insurance and configured dependents before editing this flag.
        grids["gridDependents"].Columns["colInsurance"]!.ReadOnly = true;
        tabs.TabPages[10].Enabled = false; // Sponsor-data global prerequisite is not connected.
        ((TextBox)Controls.Find("txtMobilePassword", true)[0]).UseSystemPasswordChar = true;



        void Visibility(string name, bool visible)
        {
            Controls.Find(name, true)[0].Visible = visible;
            Controls.Find("lbl" + name, true)[0].Visible = visible;
        }
        void UpdateDependencies()
        {
            Visibility("lstLocations", ((ComboBox)Controls.Find("cboLocations", true)[0]).SelectedIndex == 2);
            bool tickets = ((CheckBox)Controls.Find("chkTickets", true)[0]).Checked;
            foreach (var field in Sections[15].Fields.Skip(1)) Visibility(field.Name, tickets);
            Visibility("txtLastTickets", tickets && ((ComboBox)Controls.Find("cboTicketMethod", true)[0]).SelectedIndex == 1);
            grids["gridCompanions"].Visible = tickets;
            bool rest = ((CheckBox)Controls.Find("chkRestPolicy", true)[0]).Checked;
            foreach (var field in Sections[24].Fields.Skip(1)) Visibility(field.Name, rest);
        }
        foreach (var name in new[] { "chkTickets", "chkRestPolicy" })
            ((CheckBox)Controls.Find(name, true)[0]).CheckedChanged += (_, _) => UpdateDependencies();
        foreach (var name in new[] { "cboLocations", "cboTicketMethod" })
            ((ComboBox)Controls.Find(name, true)[0]).SelectedIndexChanged += (_, _) => UpdateDependencies();
        UpdateDependencies();
        // Full-name order depends on the unconnected title-position setting, so leave it derived/empty.
        AutoScroll = true;
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
        gridDocuments.Enabled = true;
        gridDependents.Enabled = true;
        gridGuarantors.Enabled = true;
        gridSponsors.Enabled = true;
        gridBanks.Enabled = true;
        gridCompanions.Enabled = true;
        gridQualifications.Enabled = true;
        sectionFields0.Enabled = true;         sectionFields1.Enabled = true;         sectionFields2.Enabled = true;         sectionFields3.Enabled = true;         sectionFields4.Enabled = true;         sectionFields5.Enabled = true;         sectionFields6.Enabled = true;         sectionFields7.Enabled = true;         sectionFields8.Enabled = true;         sectionFields9.Enabled = true;         sectionFields10.Enabled = true;         sectionFields11.Enabled = true;         sectionFields12.Enabled = true;         sectionFields13.Enabled = true;         sectionFields14.Enabled = true;         sectionFields15.Enabled = true;         sectionFields16.Enabled = true;         sectionFields17.Enabled = true;         sectionFields18.Enabled = true;         sectionFields19.Enabled = true;         sectionFields20.Enabled = true;         sectionFields21.Enabled = true;         sectionFields22.Enabled = true;         sectionFields23.Enabled = true;         sectionFields24.Enabled = true;         sectionFields25.Enabled = true;
        standardCommandAdd.Enabled = false;
        var firstEditor = fields.Controls.Cast<Control>().FirstOrDefault(c => c.Enabled && (c is TextBox { ReadOnly: false } || c is CheckBox));
        if (firstEditor != null) firstEditor.Focus();
        else gridPermanent.Focus();
        tabsFields.SelectNextControl(null, true, true, true, false);
    }
    private void grid_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
    {
        if (sender is DataGridView { IsCurrentCellDirty: true, CurrentCell: DataGridViewCheckBoxCell } grid)
            grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }
    private enum FieldKind { Text, ForeignText, Lookup, Derived, Check, SingleCheck, Choice, DerivedCheck, MultiLookup }
    private sealed record Field(string Name, string Label, FieldKind Kind = FieldKind.Text, string[]? Choices = null);
    private sealed record Table(string Name, string? Label, bool ReadOnly, params Field[] Columns);
}
