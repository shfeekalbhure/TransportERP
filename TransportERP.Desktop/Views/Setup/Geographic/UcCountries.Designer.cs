using TransportERP.Desktop.Controls;
using TransportERP.Desktop.CoreUI.Controls;

namespace TransportERP.Desktop.Views.Setup.Geographic;

partial class UcCountries
{
    private System.ComponentModel.IContainer? components = null;

    // القالب العام يحتوي الأزرار والتنبيه والبحث والجدول والتصفح والتدقيق.
    private TransportReferenceScreenShell screenShell = null!;

    // هذا الجدول فقط لترتيب حقول شاشة الدول الخاصة بها.
    private TableLayoutPanel tblData = null!;

    // الحقول التالية خاصة بالدول، لذلك تبقى داخل هذه الشاشة وليست داخل القالب العام.
    private RequiredTextBox txtCountryCode = null!;
    private RequiredTextBox txtNameAr = null!;
    private TextBox txtNameEn = null!;
    private TextBox txtIso2 = null!;
    private TextBox txtIso3 = null!;
    private TextBox txtDialCode = null!;
    private TextBox txtCurrencyCode = null!;
    private ComboBox cmbStatus = null!;
    private TextBox txtNotes = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        screenShell = new TransportReferenceScreenShell();
        tblData = new TableLayoutPanel();
        txtCountryCode = new RequiredTextBox();
        txtNameAr = new RequiredTextBox();
        txtNameEn = new TextBox();
        txtIso2 = new TextBox();
        txtIso3 = new TextBox();
        txtDialCode = new TextBox();
        txtCurrencyCode = new TextBox();
        cmbStatus = new ComboBox();
        txtNotes = new TextBox();

        SuspendLayout();

        // القالب المشترك يملأ الشاشة بالكامل، لذلك لا نكرر الحاويات العامة هنا.
        screenShell.Dock = DockStyle.Fill;
        screenShell.Name = "screenShell";
        screenShell.DataGroupTitle = "البيانات الرئيسية";
        screenShell.RightToLeft = RightToLeft.Yes;

        // جدول الحقول يتكون من عمودين للبيانات، وكل Label يقع يمين الحقل الخاص به.
        tblData.ColumnCount = 4;
        tblData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
        tblData.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tblData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
        tblData.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tblData.Dock = DockStyle.Fill;
        tblData.Padding = new Padding(4);
        tblData.RightToLeft = RightToLeft.Yes;
        tblData.RowCount = 5;
        tblData.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        tblData.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        tblData.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        tblData.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        tblData.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        // إضافة حقول الدول فقط. بقية الأدوات تأتي من screenShell.
        AddField(tblData, "كود الدولة *", txtCountryCode, 0, 0);
        AddField(tblData, "الاسم العربي *", txtNameAr, 2, 0);
        AddField(tblData, "الاسم الإنجليزي", txtNameEn, 0, 1);
        AddField(tblData, "ISO2", txtIso2, 2, 1);
        AddField(tblData, "ISO3", txtIso3, 0, 2);
        AddField(tblData, "مفتاح الاتصال", txtDialCode, 2, 2);
        AddField(tblData, "رمز العملة", txtCurrencyCode, 0, 3);
        AddField(tblData, "الحالة", cmbStatus, 2, 3);

        var lblNotes = CreateLabel("الملاحظات");
        tblData.Controls.Add(lblNotes, 0, 4);
        ConfigureTextBox(txtNotes, false);
        txtNotes.Multiline = true;
        txtNotes.ScrollBars = ScrollBars.Vertical;
        tblData.Controls.Add(txtNotes, 1, 4);
        tblData.SetColumnSpan(txtNotes, 3);

        ConfigureRequiredTextBox(txtCountryCode);
        ConfigureRequiredTextBox(txtNameAr);
        ConfigureTextBox(txtNameEn, false);
        ConfigureTextBox(txtIso2, true);
        ConfigureTextBox(txtIso3, true);
        ConfigureTextBox(txtDialCode, true);
        ConfigureTextBox(txtCurrencyCode, true);
        ConfigureComboBox(cmbStatus);
        cmbStatus.Items.AddRange(new object[] { "نشط", "موقوف" });

        // إضافة جدول الحقول إلى المكان المخصص للبيانات داخل القالب المشترك.
        screenShell.DataHost.Controls.Add(tblData);

        // أعمدة جدول الدول فقط هي التي تختلف عن بقية الشاشات.
        screenShell.Grid.AutoGenerateColumns = false;
        screenShell.Grid.Columns.Add("colCountryCode", "كود الدولة");
        screenShell.Grid.Columns.Add("colNameAr", "الاسم العربي");
        screenShell.Grid.Columns.Add("colNameEn", "الاسم الإنجليزي");
        screenShell.Grid.Columns.Add("colIso2", "ISO2");
        screenShell.Grid.Columns.Add("colIso3", "ISO3");
        screenShell.Grid.Columns.Add("colDialCode", "مفتاح الاتصال");
        screenShell.Grid.Columns.Add("colCurrencyCode", "رمز العملة");
        screenShell.Grid.Columns.Add("colStatus", "الحالة");

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(247, 249, 252);
        Controls.Add(screenShell);
        Font = new Font("Segoe UI", 10F);
        Name = "UcCountries";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(1280, 760);
        ResumeLayout(false);
    }

    /// <summary>
    /// إضافة Label وحقل إلى جدول البيانات مع الحفاظ على نفس الترتيب والمسافات.
    /// </summary>
    private static void AddField(TableLayoutPanel table, string labelText, Control field, int labelColumn, int row)
    {
        table.Controls.Add(CreateLabel(labelText), labelColumn, row);
        table.Controls.Add(field, labelColumn + 1, row);
    }

    /// <summary>
    /// إنشاء عنوان حقل عربي بمحاذاة يمين ثابتة.
    /// </summary>
    private static Label CreateLabel(string text) => new()
    {
        Dock = DockStyle.Fill,
        Margin = new Padding(4),
        Text = text,
        TextAlign = ContentAlignment.MiddleRight
    };

    /// <summary>
    /// تجهيز الحقل الإلزامي الموحد الموجود في Toolbox.
    /// </summary>
    private static void ConfigureRequiredTextBox(RequiredTextBox textBox)
    {
        textBox.Dock = DockStyle.Fill;
        textBox.Margin = new Padding(4, 5, 8, 5);
        textBox.RightToLeft = RightToLeft.Yes;
        textBox.TextAlign = HorizontalAlignment.Right;
    }

    /// <summary>
    /// تجهيز الحقول النصية العادية، مع السماح بالمحاذاة اليسرى للقيم اللاتينية الصرفة.
    /// </summary>
    private static void ConfigureTextBox(TextBox textBox, bool latinValue)
    {
        textBox.Dock = DockStyle.Fill;
        textBox.Margin = new Padding(4, 5, 8, 5);
        textBox.RightToLeft = latinValue ? RightToLeft.No : RightToLeft.Yes;
        textBox.TextAlign = latinValue ? HorizontalAlignment.Left : HorizontalAlignment.Right;
    }

    /// <summary>
    /// تجهيز ComboBox بنفس المقاس والاتجاه العربي في جميع حقول الشاشة.
    /// </summary>
    private static void ConfigureComboBox(ComboBox comboBox)
    {
        comboBox.Dock = DockStyle.Fill;
        comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        comboBox.Margin = new Padding(4, 5, 8, 5);
        comboBox.RightToLeft = RightToLeft.Yes;
    }
}
