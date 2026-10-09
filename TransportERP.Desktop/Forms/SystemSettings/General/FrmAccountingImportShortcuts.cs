namespace TransportERP.Desktop.Forms.SystemSettings.General;

// PDF29: navigation-only shortcuts. This dialog has no save payload or service dependency.
internal sealed partial class FrmAccountingImportShortcuts : Form
{
    public FrmAccountingImportShortcuts()
    {
        InitializeComponent();
        shortcutsMenu.Rows.Add("استيراد من ملف إكسل");
        shortcutsMenu.Rows.Add("استيراد دليل حسابات رئيسي");
        shortcutsMenu.Rows[1].DefaultCellStyle.ForeColor = SystemColors.GrayText;
        global::TransportERP.ScreenRootProperties.Attach(this);
    }

    private void OpenSelectedShortcut()
    {
        if (shortcutsMenu.CurrentCell?.RowIndex != 0) return;
        using var preview = new FrmAccountingImportPreview();
        preview.ShowDialog(this);
    }

    private void shortcutsMenu_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex == 0) OpenSelectedShortcut();
    }

    private void shortcutsMenu_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;
        e.Handled = true;
        e.SuppressKeyPress = true;
        OpenSelectedShortcut();
    }

    private void shortcutsMenu_Paint(object? sender, PaintEventArgs e)
    {
        if (shortcutsMenu.Rows.Count != 2) return;
        using var pen = new Pen(shortcutsMenu.GridColor);
        int top = shortcutsMenu.GetRowDisplayRectangle(1, false).Bottom;
        for (int y = top; y < shortcutsMenu.ClientSize.Height; y += shortcutsMenu.RowTemplate.Height)
            e.Graphics.DrawLine(pen, 0, y, shortcutsMenu.ClientSize.Width, y);
    }
}
