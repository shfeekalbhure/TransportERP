using System.ComponentModel;
using TransportERP.Desktop.CoreUI;
namespace TransportERP.Desktop.Forms.SystemSettings.General;

// Visual authority: original screenshots PDF pp.4-5 (GENS003).
// Existing monthly/user-period modes are retained. Generation and persistence remain unavailable.
public sealed partial class UcSystemPeriods : UserControl, IFoundationScreen, IExplicitScreenLayout
{
    private readonly FoundationUiSession foundation;
    public UcSystemPeriods()
    {
        InitializeComponent();
        foreach (var pair in commandBar.Commands)
        {
            pair.Value.Visible = pair.Key <= StandardCommand.Close
                && pair.Key is not (StandardCommand.Cancel or StandardCommand.View or StandardCommand.Print);
            pair.Value.Enabled = false;
        }
        var add = commandBar.Commands[StandardCommand.Add];
        add.Name = "btnAdd"; add.Enabled = true; add.Click += btnAdd_Click;
        commandBar.Commands[StandardCommand.Save].Name = "btnSave";
        UpdatePeriodMode();
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
    private void btnAdd_Click(object? sender, EventArgs e)
    {
        referenceFields.Enabled = true;
        commandBar.Commands[StandardCommand.Add].Enabled = false;
        UpdatePeriodMode();
        txtPeriodCount.Focus();
    }
    private void periodType_SelectedIndexChanged(object? sender, EventArgs e) => UpdatePeriodMode();
    private void UpdatePeriodMode()
    {
        // The initial unselected state also shows the boundary inputs in PDF p.4.
        bool monthly = cboPeriodType.SelectedIndex <= 0;
        txtFromMonth.Visible = lbltxtFromMonth.Visible = monthly;
        txtToMonth.Visible = lbltxtToMonth.Visible = monthly;
        txtFromYear.Visible = lbltxtFromYear.Visible = monthly;
        txtToYear.Visible = lbltxtToYear.Visible = monthly;
        gridPeriods.ReadOnly = cboPeriodType.SelectedIndex != 1;
        gridPeriods.Enabled = cboPeriodType.SelectedIndex >= 0;
    }
    private void gridPeriods_Paint(object? sender, PaintEventArgs e)
    {
        // Empty reference ruling, not period records. Real data uses native grid rendering.
        if (gridPeriods.Rows.Count != 0 || gridPeriods.DataSource != null) return;
        using var line = new Pen(gridPeriods.GridColor);
        using var background = new SolidBrush(gridPeriods.DefaultCellStyle.BackColor);
        using var selected = new SolidBrush(gridPeriods.DefaultCellStyle.SelectionBackColor);
        int top = gridPeriods.ColumnHeadersHeight;
        for (int row = 0; top + gridPeriods.RowTemplate.Height <= gridPeriods.ClientSize.Height; row++, top += gridPeriods.RowTemplate.Height)
        {
            foreach (DataGridViewColumn column in gridPeriods.Columns)
            {
                var header = gridPeriods.GetCellDisplayRectangle(column.Index, -1, true);
                var cell = new Rectangle(header.X, top, Math.Max(1, header.Width - 1), gridPeriods.RowTemplate.Height - 1);
                e.Graphics.FillRectangle(row == 0 ? selected : background, cell);
                e.Graphics.DrawRectangle(line, cell);
            }
        }
    }}
