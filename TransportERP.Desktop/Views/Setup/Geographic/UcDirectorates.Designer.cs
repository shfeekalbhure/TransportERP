using TransportERP.Desktop.Controls;
using TransportERP.Desktop.CoreUI.Controls;

namespace TransportERP.Desktop.Views.Setup.Geographic;

partial class UcDirectorates
{
    private System.ComponentModel.IContainer? components = null;
    private TransportReferenceScreenShell screenShell = null!;
    private TableLayoutPanel tblData = null!;

    // هذه الحقول فقط تخص المديرية، لذلك تبقى داخل هذه الشاشة.
    private ComboBox cmbCountry = null!;
    private ComboBox cmbGovernorate = null!;
    private RequiredTextBox txtDirectorateCode = null!;
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
        cmbGovernorate = new ComboBox();
        txtDirectorateCode = new RequiredTextBox();
        txtNameAr = new RequiredTextBox();
        txtNameEn = new TextBox();
        cmbStatus = new ComboBox();
        txtNotes = new TextBox();
        SuspendLayout();

        // القالب الواحد يحتوي الأدوات العامة الثابتة لكل شاشة مرجعية.
        screenShell.Dock = DockStyle.Fill;
        screenShell.Name = "screenShell";
        screenShell.RightToLeft = RightToLeft.Yes;

        // تنظيم الحقول الخاصة بالمديرية فقط داخل عمودين.
        tblData.ColumnCount = 4;
        tblData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
        tblData.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tblData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
        tblData.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tblData.Dock = DockStyle.Fill;
        tblData.RightToLeft = RightToLeft.Yes;
        tblData.RowCount = 4;
        tblData.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        tblData.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        tblData.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        tblData.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        ConfigureComboBox(cmbCountry);
        ConfigureComboBox(cmbGovernorate);
        ConfigureRequiredTextBox(txtDirectorateCode);
        ConfigureRequiredTextBox(txtNameAr);
        ConfigureTextBox(txtNameEn);
        ConfigureComboBox(cmbStatus);
        cmbStatus.Items.AddRange(new object[] { "نشط", "موقوف" });
        ConfigureTextBox(txtNotes);
        txtNotes.Multiline = true;
        txtNotes.ScrollBars = ScrollBars.Vertical;

        AddField(tblData, "الدولة *", cmbCountry, 0, 0);
        AddField(tblData, "المحافظة *", cmbGovernorate, 2, 0);
        AddField(tblData, "كود المديرية *", txtDirectorateCode, 0, 1);
        AddField(tblData, "الاسم العربي *", txtNameAr, 2, 1);
        AddField(tblData, "الاسم الإنجليزي", txtNameEn, 0, 2);
        AddField(tblData, "الحالة", cmbStatus, 2, 2);
        tblData.Controls.Add(CreateLabel("الملاحظات"), 0, 3);
        tblData.Controls.Add(txtNotes, 1, 3);
        tblData.SetColumnSpan(txtNotes, 3);

        screenShell.DataHost.Controls.Add(tblData);

        // أعمدة قائمة المديريات فقط.
        screenShell.Grid.AutoGenerateColumns = false;
        screenShell.Grid.Columns.Add("colDirectorateCode", "كود المديرية");
        screenShell.Grid.Columns.Add("colNameAr", "اسم المديرية");
        screenShell.Grid.Columns.Add("colGovernorate", "المحافظة");
        screenShell.Grid.Columns.Add("colCountry", "الدولة");
        screenShell.Grid.Columns.Add("colStatus", "الحالة");

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(247, 249, 252);
        Controls.Add(screenShell);
        Font = new Font("Segoe UI", 10F);
        Name = "UcDirectorates";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(1280, 760);
        ResumeLayout(false);
    }

    /// <summary>إضافة Label وحقل بنفس المسافات الثابتة.</summary>
    private static void AddField(TableLayoutPanel table, string labelText, Control field, int labelColumn, int row)
    {
        table.Controls.Add(CreateLabel(labelText), labelColumn, row);
        table.Controls.Add(field, labelColumn + 1, row);
    }

    /// <summary>إنشاء تسمية عربية ثابتة ومحاذاة يمين.</summary>
    private static Label CreateLabel(string text) => new()
    {
        Dock = DockStyle.Fill,
        Margin = new Padding(4),
        Text = text,
        TextAlign = ContentAlignment.MiddleRight
    };

    /// <summary>تهيئة الحقول الإلزامية الموحدة.</summary>
    private static void ConfigureRequiredTextBox(RequiredTextBox textBox)
    {
        textBox.Dock = DockStyle.Fill;
        textBox.Margin = new Padding(4, 5, 8, 5);
        textBox.RightToLeft = RightToLeft.Yes;
        textBox.TextAlign = HorizontalAlignment.Right;
    }

    /// <summary>تهيئة TextBox عربي قياسي.</summary>
    private static void ConfigureTextBox(TextBox textBox)
    {
        textBox.Dock = DockStyle.Fill;
        textBox.Margin = new Padding(4, 5, 8, 5);
        textBox.RightToLeft = RightToLeft.Yes;
        textBox.TextAlign = HorizontalAlignment.Right;
    }

    /// <summary>تهيئة ComboBox قياسي RTL.</summary>
    private static void ConfigureComboBox(ComboBox comboBox)
    {
        comboBox.Dock = DockStyle.Fill;
        comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        comboBox.Margin = new Padding(4, 5, 8, 5);
        comboBox.RightToLeft = RightToLeft.Yes;
    }
}
