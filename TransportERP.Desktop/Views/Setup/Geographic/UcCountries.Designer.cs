namespace TransportERP.Desktop.Views.Setup.Geographic;

partial class UcCountries
{
    private System.ComponentModel.IContainer? components = null;

    private TableLayoutPanel tblRoot = null!;
    private FlowLayoutPanel flpToolbar = null!;
    private Button btnNew = null!;
    private Button btnSave = null!;
    private Button btnEdit = null!;
    private Button btnStop = null!;
    private Button btnDelete = null!;
    private Button btnPrint = null!;
    private Button btnClose = null!;
    private GroupBox grpData = null!;
    private TableLayoutPanel tblData = null!;
    private Label lblCountryCode = null!;
    private TextBox txtCountryCode = null!;
    private Label lblNameAr = null!;
    private TextBox txtNameAr = null!;
    private Label lblNameEn = null!;
    private TextBox txtNameEn = null!;
    private Label lblIso2 = null!;
    private TextBox txtIso2 = null!;
    private Label lblIso3 = null!;
    private TextBox txtIso3 = null!;
    private Label lblDialCode = null!;
    private TextBox txtDialCode = null!;
    private Label lblCurrencyCode = null!;
    private TextBox txtCurrencyCode = null!;
    private Label lblStatus = null!;
    private ComboBox cmbStatus = null!;
    private Label lblNotes = null!;
    private TextBox txtNotes = null!;
    private GroupBox grpSearch = null!;
    private FlowLayoutPanel flpSearch = null!;
    private Label lblSearch = null!;
    private TextBox txtSearch = null!;
    private Label lblStatusFilter = null!;
    private ComboBox cmbStatusFilter = null!;
    private DataGridView dgvCountries = null!;
    private FlowLayoutPanel flpPagination = null!;
    private Button btnFirst = null!;
    private Button btnPrevious = null!;
    private Label lblPage = null!;
    private Button btnNext = null!;
    private Button btnLast = null!;
    private TableLayoutPanel tblAudit = null!;
    private Label lblCreatedCaption = null!;
    private Label lblCreatedValue = null!;
    private Label lblModifiedCaption = null!;
    private Label lblModifiedValue = null!;
    private Label lblEditCountCaption = null!;
    private Label lblEditCountValue = null!;
    private Label lblPrintCountCaption = null!;
    private Label lblPrintCountValue = null!;

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
        tblRoot = new TableLayoutPanel();
        flpToolbar = new FlowLayoutPanel();
        btnNew = new Button();
        btnSave = new Button();
        btnEdit = new Button();
        btnStop = new Button();
        btnDelete = new Button();
        btnPrint = new Button();
        btnClose = new Button();
        grpData = new GroupBox();
        tblData = new TableLayoutPanel();
        lblCountryCode = new Label();
        txtCountryCode = new TextBox();
        lblNameAr = new Label();
        txtNameAr = new TextBox();
        lblNameEn = new Label();
        txtNameEn = new TextBox();
        lblIso2 = new Label();
        txtIso2 = new TextBox();
        lblIso3 = new Label();
        txtIso3 = new TextBox();
        lblDialCode = new Label();
        txtDialCode = new TextBox();
        lblCurrencyCode = new Label();
        txtCurrencyCode = new TextBox();
        lblStatus = new Label();
        cmbStatus = new ComboBox();
        lblNotes = new Label();
        txtNotes = new TextBox();
        grpSearch = new GroupBox();
        flpSearch = new FlowLayoutPanel();
        lblSearch = new Label();
        txtSearch = new TextBox();
        lblStatusFilter = new Label();
        cmbStatusFilter = new ComboBox();
        dgvCountries = new DataGridView();
        flpPagination = new FlowLayoutPanel();
        btnFirst = new Button();
        btnPrevious = new Button();
        lblPage = new Label();
        btnNext = new Button();
        btnLast = new Button();
        tblAudit = new TableLayoutPanel();
        lblCreatedCaption = new Label();
        lblCreatedValue = new Label();
        lblModifiedCaption = new Label();
        lblModifiedValue = new Label();
        lblEditCountCaption = new Label();
        lblEditCountValue = new Label();
        lblPrintCountCaption = new Label();
        lblPrintCountValue = new Label();
        ((System.ComponentModel.ISupportInitialize)dgvCountries).BeginInit();
        SuspendLayout();

        tblRoot.BackColor = Color.FromArgb(247, 249, 252);
        tblRoot.ColumnCount = 1;
        tblRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tblRoot.Controls.Add(flpToolbar, 0, 0);
        tblRoot.Controls.Add(grpData, 0, 1);
        tblRoot.Controls.Add(grpSearch, 0, 2);
        tblRoot.Controls.Add(dgvCountries, 0, 3);
        tblRoot.Controls.Add(flpPagination, 0, 4);
        tblRoot.Controls.Add(tblAudit, 0, 5);
        tblRoot.Dock = DockStyle.Fill;
        tblRoot.Padding = new Padding(12);
        tblRoot.RowCount = 6;
        tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
        tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 250F));
        tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 76F));
        tblRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
        tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F));

        flpToolbar.Controls.AddRange(new Control[] { btnNew, btnSave, btnEdit, btnStop, btnDelete, btnPrint, btnClose });
        flpToolbar.Dock = DockStyle.Fill;
        flpToolbar.FlowDirection = FlowDirection.RightToLeft;
        flpToolbar.Padding = new Padding(4);
        flpToolbar.RightToLeft = RightToLeft.Yes;
        flpToolbar.WrapContents = false;
        ConfigureToolbarButton(btnNew, "جديد");
        ConfigureToolbarButton(btnSave, "حفظ");
        ConfigureToolbarButton(btnEdit, "تعديل");
        ConfigureToolbarButton(btnStop, "إيقاف");
        ConfigureToolbarButton(btnDelete, "حذف");
        ConfigureToolbarButton(btnPrint, "طباعة");
        ConfigureToolbarButton(btnClose, "إغلاق");
        btnNew.Click += btnNew_Click;

        grpData.Controls.Add(tblData);
        grpData.Dock = DockStyle.Fill;
        grpData.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        grpData.Padding = new Padding(12);
        grpData.RightToLeft = RightToLeft.Yes;
        grpData.Text = "البيانات الرئيسية";

        tblData.ColumnCount = 4;
        tblData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
        tblData.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tblData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
        tblData.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tblData.Dock = DockStyle.Fill;
        tblData.RightToLeft = RightToLeft.Yes;
        tblData.RowCount = 5;
        for (var i = 0; i < 4; i++) tblData.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        tblData.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        AddField(tblData, lblCountryCode, "كود الدولة *", txtCountryCode, 0, 0, true);
        AddField(tblData, lblNameAr, "الاسم العربي *", txtNameAr, 2, 0, true);
        AddField(tblData, lblNameEn, "الاسم الإنجليزي", txtNameEn, 0, 1, false);
        AddField(tblData, lblIso2, "ISO2", txtIso2, 2, 1, false);
        AddField(tblData, lblIso3, "ISO3", txtIso3, 0, 2, false);
        AddField(tblData, lblDialCode, "مفتاح الاتصال", txtDialCode, 2, 2, false);
        AddField(tblData, lblCurrencyCode, "رمز العملة", txtCurrencyCode, 0, 3, false);
        ConfigureFieldLabel(lblStatus, "الحالة");
        tblData.Controls.Add(lblStatus, 2, 3);
        ConfigureCombo(cmbStatus);
        cmbStatus.Items.AddRange(new object[] { "نشطة", "غير نشطة" });
        tblData.Controls.Add(cmbStatus, 3, 3);
        ConfigureFieldLabel(lblNotes, "الملاحظات");
        tblData.Controls.Add(lblNotes, 0, 4);
        ConfigureTextBox(txtNotes, false);
        txtNotes.Multiline = true;
        txtNotes.ScrollBars = ScrollBars.Vertical;
        tblData.Controls.Add(txtNotes, 1, 4);
        tblData.SetColumnSpan(txtNotes, 3);

        grpSearch.Controls.Add(flpSearch);
        grpSearch.Dock = DockStyle.Fill;
        grpSearch.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        grpSearch.RightToLeft = RightToLeft.Yes;
        grpSearch.Text = "البحث والتصفية";
        flpSearch.Controls.AddRange(new Control[] { lblSearch, txtSearch, lblStatusFilter, cmbStatusFilter });
        flpSearch.Dock = DockStyle.Fill;
        flpSearch.FlowDirection = FlowDirection.RightToLeft;
        flpSearch.Padding = new Padding(8, 10, 8, 6);
        flpSearch.RightToLeft = RightToLeft.Yes;
        flpSearch.WrapContents = false;
        ConfigureInlineLabel(lblSearch, "بحث:");
        txtSearch.Margin = new Padding(6);
        txtSearch.PlaceholderText = "ابحث بالكود أو الاسم...";
        txtSearch.RightToLeft = RightToLeft.Yes;
        txtSearch.Size = new Size(320, 30);
        txtSearch.TextAlign = HorizontalAlignment.Right;
        txtSearch.TextChanged += txtSearch_TextChanged;
        ConfigureInlineLabel(lblStatusFilter, "الحالة:");
        cmbStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbStatusFilter.Items.AddRange(new object[] { "الكل", "نشطة", "غير نشطة" });
        cmbStatusFilter.Margin = new Padding(6);
        cmbStatusFilter.RightToLeft = RightToLeft.Yes;
        cmbStatusFilter.Size = new Size(150, 30);

        dgvCountries.AllowUserToAddRows = false;
        dgvCountries.AllowUserToDeleteRows = false;
        dgvCountries.AllowUserToResizeRows = false;
        dgvCountries.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvCountries.BackgroundColor = Color.White;
        dgvCountries.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        dgvCountries.ColumnHeadersHeight = 36;
        dgvCountries.Dock = DockStyle.Fill;
        dgvCountries.MultiSelect = false;
        dgvCountries.ReadOnly = true;
        dgvCountries.RightToLeft = RightToLeft.Yes;
        dgvCountries.RowHeadersVisible = false;
        dgvCountries.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvCountries.Columns.Add("colCode", "كود الدولة");
        dgvCountries.Columns.Add("colNameAr", "الاسم العربي");
        dgvCountries.Columns.Add("colNameEn", "الاسم الإنجليزي");
        dgvCountries.Columns.Add("colIso2", "ISO2");
        dgvCountries.Columns.Add("colIso3", "ISO3");
        dgvCountries.Columns.Add("colDial", "مفتاح الاتصال");
        dgvCountries.Columns.Add("colCurrency", "رمز العملة");
        dgvCountries.Columns.Add("colStatus", "الحالة");

        flpPagination.Controls.AddRange(new Control[] { btnFirst, btnPrevious, lblPage, btnNext, btnLast });
        flpPagination.Dock = DockStyle.Fill;
        flpPagination.FlowDirection = FlowDirection.LeftToRight;
        flpPagination.Padding = new Padding(0, 8, 0, 0);
        flpPagination.RightToLeft = RightToLeft.Yes;
        flpPagination.WrapContents = false;
        ConfigurePagerButton(btnFirst, "الأول");
        ConfigurePagerButton(btnPrevious, "السابق");
        lblPage.AutoSize = false;
        lblPage.Margin = new Padding(8);
        lblPage.Size = new Size(70, 30);
        lblPage.Text = "1 / 1";
        lblPage.TextAlign = ContentAlignment.MiddleCenter;
        ConfigurePagerButton(btnNext, "التالي");
        ConfigurePagerButton(btnLast, "الأخير");

        tblAudit.BackColor = Color.White;
        tblAudit.ColumnCount = 8;
        for (var i = 0; i < 8; i++) tblAudit.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
        tblAudit.Controls.Add(lblCreatedCaption, 0, 0);
        tblAudit.Controls.Add(lblCreatedValue, 1, 0);
        tblAudit.Controls.Add(lblModifiedCaption, 2, 0);
        tblAudit.Controls.Add(lblModifiedValue, 3, 0);
        tblAudit.Controls.Add(lblEditCountCaption, 4, 0);
        tblAudit.Controls.Add(lblEditCountValue, 5, 0);
        tblAudit.Controls.Add(lblPrintCountCaption, 6, 0);
        tblAudit.Controls.Add(lblPrintCountValue, 7, 0);
        tblAudit.Dock = DockStyle.Fill;
        tblAudit.Padding = new Padding(8);
        tblAudit.RightToLeft = RightToLeft.Yes;
        ConfigureAuditLabel(lblCreatedCaption, "الإنشاء:", true);
        ConfigureAuditLabel(lblCreatedValue, "—", false);
        ConfigureAuditLabel(lblModifiedCaption, "آخر تعديل:", true);
        ConfigureAuditLabel(lblModifiedValue, "—", false);
        ConfigureAuditLabel(lblEditCountCaption, "مرات التعديل:", true);
        ConfigureAuditLabel(lblEditCountValue, "0", false);
        ConfigureAuditLabel(lblPrintCountCaption, "مرات الطباعة:", true);
        ConfigureAuditLabel(lblPrintCountValue, "0", false);

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(247, 249, 252);
        Controls.Add(tblRoot);
        Font = new Font("Segoe UI", 10F);
        Name = "UcCountries";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(1280, 760);
        ((System.ComponentModel.ISupportInitialize)dgvCountries).EndInit();
        ResumeLayout(false);
    }

    private static void AddField(TableLayoutPanel layout, Label label, string caption, TextBox textBox, int labelColumn, int row, bool required)
    {
        ConfigureFieldLabel(label, caption);
        ConfigureTextBox(textBox, required);
        layout.Controls.Add(label, labelColumn, row);
        layout.Controls.Add(textBox, labelColumn + 1, row);
    }

    private static void ConfigureToolbarButton(Button button, string text)
    {
        button.Text = text;
        button.Size = new Size(92, 38);
        button.Margin = new Padding(4);
        button.FlatStyle = FlatStyle.Flat;
        button.TextAlign = ContentAlignment.MiddleCenter;
        button.RightToLeft = RightToLeft.Yes;
    }

    private static void ConfigureFieldLabel(Label label, string text)
    {
        label.Text = text;
        label.Dock = DockStyle.Fill;
        label.TextAlign = ContentAlignment.MiddleRight;
        label.Margin = new Padding(6, 3, 6, 3);
    }

    private static void ConfigureTextBox(TextBox textBox, bool required)
    {
        textBox.Dock = DockStyle.Fill;
        textBox.Margin = new Padding(6, 5, 6, 5);
        textBox.RightToLeft = RightToLeft.Yes;
        textBox.TextAlign = HorizontalAlignment.Right;
        textBox.BackColor = required ? Color.FromArgb(255, 250, 220) : Color.White;
    }

    private static void ConfigureCombo(ComboBox combo)
    {
        combo.Dock = DockStyle.Fill;
        combo.DropDownStyle = ComboBoxStyle.DropDownList;
        combo.Margin = new Padding(6, 5, 6, 5);
        combo.RightToLeft = RightToLeft.Yes;
    }

    private static void ConfigureInlineLabel(Label label, string text)
    {
        label.AutoSize = false;
        label.Size = new Size(80, 30);
        label.Margin = new Padding(6);
        label.Text = text;
        label.TextAlign = ContentAlignment.MiddleRight;
    }

    private static void ConfigurePagerButton(Button button, string text)
    {
        button.Text = text;
        button.Size = new Size(82, 30);
        button.Margin = new Padding(4);
        button.TextAlign = ContentAlignment.MiddleCenter;
    }

    private static void ConfigureAuditLabel(Label label, string text, bool bold)
    {
        label.Dock = DockStyle.Fill;
        label.Text = text;
        label.TextAlign = ContentAlignment.MiddleRight;
        label.Font = new Font("Segoe UI", 9F, bold ? FontStyle.Bold : FontStyle.Regular);
    }
}
