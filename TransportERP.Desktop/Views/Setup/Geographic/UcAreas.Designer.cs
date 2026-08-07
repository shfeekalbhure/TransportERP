using TransportERP.Desktop.Controls;
using TransportERP.Desktop.CoreUI.Controls;

namespace TransportERP.Desktop.Views.Setup.Geographic;

partial class UcAreas
{
    private System.ComponentModel.IContainer? components = null;
    private TransportReferenceScreenShell screenShell = null!;
    private TableLayoutPanel tblData = null!;

    // الحقول التالية خاصة بالمناطق فقط، لذلك لا توضع داخل القالب العام.
    private ComboBox cmbCountry = null!;
    private ComboBox cmbGovernorate = null!;
    private ComboBox cmbDirectorate = null!;
    private ComboBox cmbCity = null!;
    private RequiredTextBox txtAreaCode = null!;
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
        cmbDirectorate = new ComboBox();
        cmbCity = new ComboBox();
        txtAreaCode = new RequiredTextBox();
        txtNameAr = new RequiredTextBox();
        txtNameEn = new TextBox();
        cmbStatus = new ComboBox();
        txtNotes = new TextBox();

        SuspendLayout();

        // القالب المشترك يملأ الشاشة ويحتوي الأوامر والتنبيه والبحث والجدول والتنقل والتدقيق.
        screenShell.Dock = DockStyle.Fill;
        screenShell.Name = "screenShell";
        screenShell.RightToLeft = RightToLeft.Yes;
        screenShell.DataGroupTitle = "البيانات الرئيسية";

        // هذا الجدول يرتب فقط حقول المنطقة. يبدأ من اليمين ويقسم البيانات إلى عمودين.
        tblData.ColumnCount = 4;
        tblData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
        tblData.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tblData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
        tblData.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tblData.Dock = DockStyle.Fill;
        tblData.Padding = new Padding(4);
        tblData.RightToLeft = RightToLeft.Yes;
        tblData.RowCount = 6;
        tblData.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        tblData.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        tblData.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        tblData.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        tblData.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        tblData.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        ConfigureComboBox(cmbCountry);
        ConfigureComboBox(cmbGovernorate);
        ConfigureComboBox(cmbDirectorate);
        ConfigureComboBox(cmbCity);
        ConfigureRequiredTextBox(txtAreaCode);
        ConfigureRequiredTextBox(txtNameAr);
        ConfigureTextBox(txtNameEn);
        ConfigureComboBox(cmbStatus);
        cmbStatus.Items.AddRange(new object[] { "نشط", "موقوف" });
        ConfigureTextBox(txtNotes);
        txtNotes.Multiline = true;
        txtNotes.ScrollBars = ScrollBars.Vertical;

        AddField(tblData, "الدولة *", cmbCountry, 0, 0);
        AddField(tblData, "المحافظة *", cmbGovernorate, 2, 0);
        AddField(tblData, "المديرية", cmbDirectorate, 0, 1);
        AddField(tblData, "المدينة *", cmbCity, 2, 1);
        AddField(tblData, "كود المنطقة *", txtAreaCode, 0, 2);
        AddField(tblData, "الاسم العربي *", txtNameAr, 2, 2);
        AddField(tblData, "الاسم الإنجليزي", txtNameEn, 0, 3);
        AddField(tblData, "الحالة", cmbStatus, 2, 3);

        tblData.Controls.Add(CreateLabel("الملاحظات"), 0, 4);
        tblData.Controls.Add(txtNotes, 1, 4);
        tblData.SetColumnSpan(txtNotes, 3);

        // يضاف محتوى المنطقة إلى DataHost فقط؛ بقية الحاويات تأتي من القالب العام.
        screenShell.DataHost.Controls.Add(tblData);

        // أعمدة جدول المناطق الخاصة بهذه الشاشة.
        screenShell.Grid.AutoGenerateColumns = false;
        screenShell.Grid.Columns.Add("colAreaCode", "كود المنطقة");
        screenShell.Grid.Columns.Add("colNameAr", "اسم المنطقة");
        screenShell.Grid.Columns.Add("colCity", "المدينة");
        screenShell.Grid.Columns.Add("colDirectorate", "المديرية");
        screenShell.Grid.Columns.Add("colGovernorate", "المحافظة");
        screenShell.Grid.Columns.Add("colCountry", "الدولة");
        screenShell.Grid.Columns.Add("colStatus", "الحالة");

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(247, 249, 252);
        Controls.Add(screenShell);
        Font = new Font("Segoe UI", 10F);
        Name = "UcAreas";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(1280, 760);
        ResumeLayout(false);
    }

    /// <summary>يضيف تسمية وحقلًا بنفس الترتيب الثابت من اليمين.</summary>
    private static void AddField(TableLayoutPanel table, string labelText, Control field, int labelColumn, int row)
    {
        table.Controls.Add(CreateLabel(labelText), labelColumn, row);
        table.Controls.Add(field, labelColumn + 1, row);
    }

    /// <summary>ينشئ عنوانًا عربيًا بمحاذاة يمين.</summary>
    private static Label CreateLabel(string text) => new()
    {
        Dock = DockStyle.Fill,
        Margin = new Padding(4),
        Text = text,
        TextAlign = ContentAlignment.MiddleRight
    };

    /// <summary>يجهز الحقل الإلزامي ويثبت الكتابة داخله جهة اليمين.</summary>
    private static void ConfigureRequiredTextBox(RequiredTextBox textBox)
    {
        textBox.Dock = DockStyle.Fill;
        textBox.Margin = new Padding(4, 5, 8, 5);
        textBox.RightToLeft = RightToLeft.Yes;
        textBox.TextAlign = HorizontalAlignment.Right;
    }

    /// <summary>يجهز TextBox عاديًا ويثبت النص جهة اليمين.</summary>
    private static void ConfigureTextBox(TextBox textBox)
    {
        textBox.Dock = DockStyle.Fill;
        textBox.Margin = new Padding(4, 5, 8, 5);
        textBox.RightToLeft = RightToLeft.Yes;
        textBox.TextAlign = HorizontalAlignment.Right;
    }

    /// <summary>يجهز ComboBox موحدًا من اليمين إلى اليسار.</summary>
    private static void ConfigureComboBox(ComboBox comboBox)
    {
        comboBox.Dock = DockStyle.Fill;
        comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        comboBox.Margin = new Padding(4, 5, 8, 5);
        comboBox.RightToLeft = RightToLeft.Yes;
    }
}
