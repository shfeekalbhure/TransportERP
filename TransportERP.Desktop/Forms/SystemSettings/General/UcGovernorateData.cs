using System.ComponentModel;
using TransportERP.Desktop.CoreUI;
namespace TransportERP.Desktop.Forms.SystemSettings.General;

// Visual authority: original screenshots PDF p.13 (GENS006).
public sealed partial class UcGovernorateData : UserControl, IFoundationScreen, IExplicitScreenLayout
{
    private readonly FoundationUiSession foundation;
    public UcGovernorateData()
    {
        InitializeComponent();
        UpdateRegionPresentation();
        foreach (var pair in commandBar.Commands)
        {
            pair.Value.Visible = pair.Key <= StandardCommand.Close;
            pair.Value.Enabled = false;
        }
        var add = commandBar.Commands[StandardCommand.Add];
        add.Name = "btnAdd";
        add.Enabled = true;
        add.Click += btnAdd_Click;
        commandBar.Commands[StandardCommand.Save].Name = "btnSave";
        // Audit display editors are not business fields and must not enter save/dirty snapshots.
        foundation = new FoundationUiSession(referenceLayout, bindDisabledActions: false);
        global::TransportERP.ScreenRootProperties.Attach(this);
    }
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public FoundationUiSession Foundation => foundation;
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool HasUnsavedChanges => foundation.HasUnsavedChanges;
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsBusy => foundation.IsBusy;
    public bool ConfirmLeave() => foundation.ConfirmLeave();
    private void txtRegionNumber_TextChanged(object? sender, EventArgs e) => UpdateRegionPresentation();
    private void UpdateRegionPresentation()
    {
        // Display only the existing derived value, never fabricate lookup choices.
        cboRegionDisplay.Items.Clear();
        if (txtRegionNumber.Text.Length > 0)
        {
            cboRegionDisplay.Items.Add(txtRegionNumber.Text);
            cboRegionDisplay.SelectedIndex = 0;
        }
    }
    private void btnAdd_Click(object? sender, EventArgs e)
    {
        referenceFields.Enabled = true;
        commandBar.Commands[StandardCommand.Add].Enabled = false;
        txtGovernorateNumber.Focus();
    }
}

