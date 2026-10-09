namespace TransportERP.Desktop.Forms.SystemSettings.General;

// PDF27: a read-only visual import template. No file is read and no records are imported.
internal sealed partial class FrmAccountingImportPreview : Form
{
    public FrmAccountingImportPreview()
    {
        InitializeComponent();
        global::TransportERP.ScreenRootProperties.Attach(this);
    }

    private void btnClose_Click(object? sender, EventArgs e) => Close();

    private void previewGrid_Paint(object? sender, PaintEventArgs e)
    {
        // Show the empty worksheet rules pictured in PDF27 without fabricating data rows.
        if (previewGrid.Rows.Count != 0) return;
        using var pen = new Pen(previewGrid.GridColor);
        int top = previewGrid.ColumnHeadersHeight;
        int rowHeight = Math.Max(24, previewGrid.RowTemplate.Height);
        for (int y = top; y < previewGrid.ClientSize.Height; y += rowHeight)
            e.Graphics.DrawLine(pen, 0, y, previewGrid.ClientSize.Width, y);
        foreach (DataGridViewColumn column in previewGrid.Columns)
        {
            var bounds = previewGrid.GetColumnDisplayRectangle(column.Index, true);
            e.Graphics.DrawLine(pen, bounds.Left, top, bounds.Left, previewGrid.ClientSize.Height);
            e.Graphics.DrawLine(pen, bounds.Right, top, bounds.Right, previewGrid.ClientSize.Height);
        }
    }
}
