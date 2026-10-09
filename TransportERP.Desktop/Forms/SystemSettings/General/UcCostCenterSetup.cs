using System.ComponentModel;
using TransportERP.Desktop.CoreUI;
namespace TransportERP.Desktop.Forms.SystemSettings.General;

// Visual reference: supplied PDF p.31 (types); group fields retained from the existing contract. Existing field keys and editable drafts retained.
public sealed partial class UcCostCenterSetup : UserControl, IFoundationScreen, IExplicitScreenLayout
{
    private readonly FoundationUiSession foundation;
    private bool updatingChecks;
    public UcCostCenterSetup()
    {
        InitializeComponent();
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
    private void btnAdd_Click(object? sender, EventArgs e)
    {
        gridCostCenterTypes.Enabled = true;
        gridCostCenterGroups.Enabled = true;


        commandBar.Commands[StandardCommand.Add].Enabled = false;
        tabsSetup.SelectedTab?.SelectNextControl(null, true, true, true, false);
    }
    private void grid_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
    {
        if (sender is DataGridView { IsCurrentCellDirty: true, CurrentCell: DataGridViewCheckBoxCell } grid)
            grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }
    private void grid_EmptyRulingPaint(object? sender, PaintEventArgs e)
    {
        // Empty reference ruling only; never create data rows or intercept editing.
        if (sender is not DataGridView grid || grid.DataSource != null
            || grid.Rows.Cast<DataGridViewRow>().Any(r => !r.IsNewRow)) return;
        int rowHeight = grid.RowTemplate.Height;
        int top = grid.ColumnHeadersHeight + rowHeight;
        using var pen = new Pen(grid.GridColor);
        for (int row = 1; row < 6 && top + rowHeight <= grid.ClientSize.Height; row++, top += rowHeight)
        {
            foreach (DataGridViewColumn col in grid.Columns)
            {
                var header = grid.GetColumnDisplayRectangle(col.Index, false);
                var bounds = new Rectangle(header.Left, top, header.Width, rowHeight);
                var color = col.DefaultCellStyle.BackColor.IsEmpty ? grid.BackgroundColor : col.DefaultCellStyle.BackColor;
                using var brush = new SolidBrush(color);
                e.Graphics.FillRectangle(brush, bounds);
                e.Graphics.DrawRectangle(pen, bounds.X, bounds.Y, Math.Max(0,bounds.Width-1), bounds.Height-1);

            }
        }
    }
    private void gridCostCenterTypes_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (updatingChecks || e.RowIndex < 0 || e.ColumnIndex != colAffectedByTransactions.Index
            || gridCostCenterTypes.Rows[e.RowIndex].Cells[e.ColumnIndex].Value is not true) return;
        updatingChecks = true;
        try
        {
            foreach (DataGridViewRow row in gridCostCenterTypes.Rows)
                if (!row.IsNewRow && row.Index != e.RowIndex) row.Cells[e.ColumnIndex].Value = false;
        }
        finally { updatingChecks = false; }
    }
}


