using System.ComponentModel;
using TransportERP.Desktop.CoreUI;
namespace TransportERP.Desktop.Forms.SystemSettings.General;

// Visual reference: supplied PDF pp.18–21. Existing field keys and editable drafts retained.
public sealed partial class UcChartSetup : UserControl, IFoundationScreen, IExplicitScreenLayout
{
    private readonly FoundationUiSession foundation;
    private bool updatingChecks;
    public UcChartSetup()
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
        gridAccountTypes.Enabled = true;
        gridAccountGroups.Enabled = true;
        gridAccountClassifications.Enabled = true;
        gridReportTypes.Enabled = true;
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
                if (col is ReferenceLookupTextColumn)
                {
                    int width = Math.Max(12, (int)Math.Round(14 * e.Graphics.DpiX / 96F));
                    ControlPaint.DrawComboButton(e.Graphics, new Rectangle(bounds.Right-width-1,bounds.Top+1,width,bounds.Height-2), ButtonState.Inactive);
                }
            }
        }
    }
    private void gridAccountTypes_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (updatingChecks || e.RowIndex < 0 || e.ColumnIndex != colAffectedByTransactions.Index
            || gridAccountTypes.Rows[e.RowIndex].Cells[e.ColumnIndex].Value is not true) return;
        updatingChecks = true;
        try
        {
            foreach (DataGridViewRow row in gridAccountTypes.Rows)
                if (!row.IsNewRow && row.Index != e.RowIndex) row.Cells[e.ColumnIndex].Value = false;
        }
        finally { updatingChecks = false; }
    }
}

// A disabled lookup affordance inside the existing text cell: no additional payload column,
// option list, click action or service call. Padding preserves ordinary text editing.
public sealed class ReferenceLookupTextColumn : DataGridViewTextBoxColumn
{
    public ReferenceLookupTextColumn()
    {
        CellTemplate = new ReferenceLookupTextCell();
        DefaultCellStyle.Padding = new Padding(0, 0, 16, 0);
        ToolTipText = "البحث غير متاح؛ لم تُربط خدمة البحث.";
    }
    [Browsable(false)] public bool LookupButtonEnabled => false;
}
public sealed class ReferenceLookupTextCell : DataGridViewTextBoxCell
{
    protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds,
        int rowIndex, DataGridViewElementStates cellState, object? value, object? formattedValue,
        string? errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle,
        DataGridViewPaintParts paintParts)
    {
        base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue,
            errorText, cellStyle, advancedBorderStyle, paintParts);
        if ((paintParts & DataGridViewPaintParts.ContentForeground) == 0) return;
        int width = Math.Max(12, (int)Math.Round(14 * graphics.DpiX / 96F));
        var button = new Rectangle(cellBounds.Right - width - 1, cellBounds.Top + 1, width, cellBounds.Height - 2);
        var state = graphics.Save();
        graphics.SetClip(Rectangle.Intersect(clipBounds, button));
        ControlPaint.DrawComboButton(graphics, button, ButtonState.Inactive);
        graphics.Restore(state);
    }
}
