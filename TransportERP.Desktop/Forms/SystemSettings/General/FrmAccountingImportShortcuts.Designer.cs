#nullable enable
namespace TransportERP.Desktop.Forms.SystemSettings.General;

partial class FrmAccountingImportShortcuts
{
    private System.ComponentModel.IContainer? components;
    private DataGridView shortcutsMenu = null!;
    private DataGridViewTextBoxColumn colShortcut = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        SuspendLayout();
        shortcutsMenu = new DataGridView();
        colShortcut = new DataGridViewTextBoxColumn();
        ((System.ComponentModel.ISupportInitialize)shortcutsMenu).BeginInit();
        shortcutsMenu.Name = "shortcutsMenu";
        shortcutsMenu.AccessibleDescription = "استيراد دليل حسابات رئيسي غير متاح في هذه المعاينة.";
        shortcutsMenu.Dock = DockStyle.Fill;
        shortcutsMenu.BackgroundColor = Color.White;
        shortcutsMenu.GridColor = Color.Silver;
        shortcutsMenu.ColumnHeadersVisible = false;
        shortcutsMenu.RowHeadersVisible = false;
        shortcutsMenu.AllowUserToAddRows = false;
        shortcutsMenu.AllowUserToDeleteRows = false;
        shortcutsMenu.AllowUserToResizeRows = false;
        shortcutsMenu.ReadOnly = true;
        shortcutsMenu.MultiSelect = false;
        shortcutsMenu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        shortcutsMenu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        shortcutsMenu.RowTemplate.Height = 24;
        shortcutsMenu.CellDoubleClick += shortcutsMenu_CellDoubleClick;
        shortcutsMenu.KeyDown += shortcutsMenu_KeyDown;
        shortcutsMenu.Paint += shortcutsMenu_Paint;
        colShortcut.Name = "colShortcut";
        colShortcut.HeaderText = "الاختصارات";
        colShortcut.SortMode = DataGridViewColumnSortMode.NotSortable;
        shortcutsMenu.Columns.Add(colShortcut);
        Controls.Add(shortcutsMenu);
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Font = new Font("Tahoma", 9F);
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        Name = "FrmAccountingImportShortcuts";
        Text = "الاختصارات";
        ClientSize = new Size(340, 230);
        MinimumSize = new Size(280, 180);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.SizableToolWindow;
        ShowInTaskbar = false;
        MaximizeBox = false;
        MinimizeBox = false;
        ((System.ComponentModel.ISupportInitialize)shortcutsMenu).EndInit();
        ResumeLayout(true);
    }
}
