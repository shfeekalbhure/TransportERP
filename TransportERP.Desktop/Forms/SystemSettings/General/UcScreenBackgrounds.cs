using System.ComponentModel;
using TransportERP.Desktop.CoreUI;
namespace TransportERP.Desktop.Forms.SystemSettings.General;

// Visual authority: original screenshots PDF p.17. No theme assets or records are seeded.
public sealed partial class UcScreenBackgrounds : UserControl, IFoundationScreen, IExplicitScreenLayout
{
    private readonly FoundationUiSession foundation;
    private bool updatingChecks;
    public UcScreenBackgrounds()
    {
        InitializeComponent();
        foreach (var pair in commandBar.Commands)
        {
            pair.Value.Visible = pair.Key is StandardCommand.Edit or StandardCommand.Last or StandardCommand.Next
                or StandardCommand.Previous or StandardCommand.First or StandardCommand.Save or StandardCommand.Close;
            pair.Value.Enabled = false;
        }
        var edit = commandBar.Commands[StandardCommand.Edit];
        edit.Name = "btnEdit";
        edit.Enabled = true;
        edit.Click += btnEdit_Click;
        commandBar.Commands[StandardCommand.Save].Name = "btnSave";
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
    private void btnEdit_Click(object? sender, EventArgs e)
    {
        gridThemes.Enabled = true;
        commandBar.Commands[StandardCommand.Edit].Enabled = false;
        gridThemes.Focus();
    }
    private void gridThemes_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
    {
        if (gridThemes.IsCurrentCellDirty && gridThemes.CurrentCell is DataGridViewCheckBoxCell)
            gridThemes.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }
    private void gridThemes_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (updatingChecks || e.RowIndex < 0 || e.ColumnIndex != colSelected.Index
            || gridThemes.Rows[e.RowIndex].Cells[e.ColumnIndex].Value is not true) return;
        updatingChecks = true;
        try
        {
            foreach (DataGridViewRow row in gridThemes.Rows)
                if (!row.IsNewRow && row.Index != e.RowIndex) row.Cells[e.ColumnIndex].Value = false;
        }
        finally { updatingChecks = false; }
    }
}
