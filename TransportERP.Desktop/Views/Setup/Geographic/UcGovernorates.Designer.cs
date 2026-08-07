using TransportERP.Desktop.Controls;
using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop.Views.Setup.Geographic;

partial class UcGovernorates
{
    private System.ComponentModel.IContainer? components = null;
    private TransportReferenceScreenShell screenShell = null!;
    private TableLayoutPanel tblData = null!;

    // هذه فقط حقول المحافظة الخاصة، أما بقية الأدوات فتأتي من القالب المشترك.
    private ComboBox cmbCountry = null!;
    private RequiredTextBox txtGovernorateCode = null!;
    private RequiredTextBox txtNameAr = null!;
    private TextBox txtNameEn = null!;
    private ComboBox cmbStatus = null!;
    private TextBox txtNotes = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        screenShell = new TransportReferenceScreenShell();
        tblData = new TableLayoutPanel();
        cmbCountry = new ComboBox();
        txtGovernorateCode = new RequiredTextBox();
        txtNameAr = new RequiredTextBox();
        txtNameEn = new TextBox();
        cmbStatus = new ComboBox();
        txtNotes = new TextBox();

        SuspendLayout();

        screenShell.Dock = DockStyle.Fill;
        screenShell.Name = "screenShell";
        screenShell.RightToLeft = RightToLeft.Yes;

        // ستة حقول = عمودان وفق القرار الحاكم. الارتفاع يأتي من المحتوى ولا يملأ مساحة زائدة.
        tblData.ColumnCount = 4;
        tblData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, TransportUiMetrics.MainDataLabelWidth));
        tblData.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tblData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, TransportUiMetrics.MainDataLabelWidth));
        tblData.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tblData.Dock = DockStyle.Top;
        tblData.AutoSize = true;
        tblData.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tblData.AutoScroll = false;
        tblData.Margin = Padding.Empty;
        tblData.Padding = Padding.Empty;
        tblData.RightToLeft = RightToLeft.Yes;
        tblData.RowCount = 4;
        tblData.RowStyles.Add(new RowStyle(SizeType.Absolute, TransportUiMetrics.MainDataRowHeight));
        tblData.RowStyles.Add(new RowStyle(SizeType.Absolute, TransportUiMetrics.MainDataRowHeight));
        tblData.RowStyles.Add(new RowStyle(SizeType.Absolute, TransportUiMetrics.MainDataRowHeight));
        tblData.RowStyles.Add(new RowStyle(SizeType.Absolute, TransportUiMetrics.MainDataMultilineMinHeight + TransportUiMetrics.MainDataRowGap));

        ConfigureComboBox(cmbCountry);
        ConfigureRequiredTextBox(txtGovernorateCode);
        ConfigureRequiredTextBox(txtNameAr);
        ConfigureTextBox(txtNameEn);
        ConfigureComboBox(cmbStatus);
        cmbStatus.Items.AddRange(new object[] { "نشط", "موقوف" });
        ConfigureTextBox(txtNotes);
        txtNotes.Multiline = true;
        txtNotes.ScrollBars = ScrollBars.Vertical;

        AddField(tblData, "الدولة *", cmbCountry, 0, 0);
        AddField(tblData, "كود المحافظة *", txtGovernorateCode, 2, 0);
        AddField(tblData, "الاسم العربي *", txtNameAr, 0, 1);
        AddField(tblData, "الاسم الإنجليزي", txtNameEn, 2, 1);
        AddField(tblData, "الحالة", cmbStatus, 0, 2);
        tblData.Controls.Add(CreateLabel("الملاحظات"), 0, 3);
        tblData.Controls.Add(txtNotes, 1, 3);
        tblData.SetColumnSpan(txtNotes, 3);

        screenShell.DataHost.Controls.Add(tblData);

        // الأعمدة التالية خاصة بالمحافظات فقط وتبقى محلية لأنها مرتبطة بمحتوى الشاشة.
        screenShell.Grid.AutoGenerateColumns = false;
        screenShell.Grid.Columns.Add("colGovernorateCode", "كود المحافظة");
        screenShell.Grid.Columns.Add("colNameAr", "اسم المحافظة");
        screenShell.Grid.Columns.Add("colCountry", "الدولة");
        screenShell.Grid.Columns.Add("colStatus", "الحالة");

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = UiTheme.WorkspaceBackground;
        Controls.Add(screenShell);
        Name = "UcGovernorates";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(1280, 760);
        ResumeLayout(false);
    }

    private static void AddField(TableLayoutPanel table, string labelText, Control field, int labelColumn, int row)
    {
        table.Controls.Add(CreateLabel(labelText), labelColumn, row);
        table.Controls.Add(field, labelColumn + 1, row);
    }

    private static Label CreateLabel(string text) => new()
    {
        Dock = DockStyle.Fill,
        Text = text,
        TextAlign = ContentAlignment.MiddleRight,
        RightToLeft = RightToLeft.Yes
    };

    private static void ConfigureRequiredTextBox(RequiredTextBox textBox)
    {
        textBox.Dock = DockStyle.Fill;
        textBox.RightToLeft = RightToLeft.Yes;
        textBox.TextAlign = HorizontalAlignment.Right;
    }

    private static void ConfigureTextBox(TextBox textBox)
    {
        textBox.Dock = DockStyle.Fill;
        textBox.RightToLeft = RightToLeft.Yes;
        textBox.TextAlign = HorizontalAlignment.Right;
    }

    private static void ConfigureComboBox(ComboBox comboBox)
    {
        comboBox.Dock = DockStyle.Fill;
        comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        comboBox.RightToLeft = RightToLeft.Yes;
    }
}
