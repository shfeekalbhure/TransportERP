namespace TransportERP.Desktop.Views.Setup.Geographic;

partial class UcGovernorates
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
    private Label lblCountry = null!;
    private ComboBox cmbCountry = null!;
    private Label lblGovernorateCode = null!;
    private TextBox txtGovernorateCode = null!;
    private Label lblNameAr = null!;
    private TextBox txtNameAr = null!;
    private Label lblNameEn = null!;
    private TextBox txtNameEn = null!;
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
    private DataGridView dgvGovernorates = null!;
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
        if (disposing) components?.Dispose();
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
        lblCountry = new Label();
        cmbCountry = new ComboBox();
        lblGovernorateCode = new Label();
        txtGovernorateCode = new TextBox();
        lblNameAr = new Label();
        txtNameAr = new TextBox();
        lblNameEn = new Label();
        txtNameEn = new TextBox();
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
        dgvGovernorates = new DataGridView();
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

        SuspendLayout();

        tblRoot.BackColor = Color.FromArgb(247, 249, 252);
        tblRoot.ColumnCount = 1;
        tblRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tblRoot.Dock = DockStyle.Fill;
        tblRoot.Padding = new Padding(16);
        tblRoot.RightToLeft = RightToLeft.Yes;
        tblRoot.RowCount = 6;
        tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
        tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 220F));
        tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
        tblRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
        tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
        Controls.Add(tblRoot);

        flpToolbar.Dock = DockStyle.Fill;
        flpToolbar.FlowDirection = FlowDirection.RightToLeft;
        flpToolbar.RightToLeft = RightToLeft.Yes;
        flpToolbar.WrapContents = false;
        flpToolbar.Padding = new Padding(0, 4, 0, 4);
        tblRoot.Controls.Add(flpToolbar, 0, 0);

        ConfigureToolbarButton(btnNew, "جديد");
        ConfigureToolbarButton(btnSave, "حفظ");
        ConfigureToolbarButton(btnEdit, "تعديل");
        ConfigureToolbarButton(btnStop, "إيقاف");
        ConfigureToolbarButton(btnDelete, "حذف");
        ConfigureToolbarButton(btnPrint, "طباعة");
        ConfigureToolbarButton(btnClose, "إغلاق");
        btnNew.Click += btnNew_Click;
        flpToolbar.Controls.AddRange(new Control[] { btnNew, btnSave, btnEdit, btnStop, btnDelete, btnPrint, btnClose });

        grpData.Dock = DockStyle.Fill;
        grpData.Text = "البيانات الرئيسية";
        grpData.RightToLeft = RightToLeft.Yes;
        grpData.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        grpData.Padding = new Padding(12);
        tblRoot.Controls.Add(grpData, 0, 1);

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
        grpData.Controls.Add(tblData);

        ConfigureFieldLabel(lblCountry, "الدولة *");
        ConfigureCombo(cmbCountry);
        ConfigureFieldLabel(lblGovernorateCode, "كود المحافظة *");
        ConfigureTextBox(txtGovernorateCode, true);
        ConfigureFieldLabel(lblNameAr, "الاسم العربي *");
        ConfigureTextBox(txtNameAr, true);
        ConfigureFieldLabel(lblNameEn, "الاسم الإنجليزي");
        ConfigureTextBox(txtNameEn, false);
        ConfigureFieldLabel(lblStatus, "الحالة");
        ConfigureCombo(cmbStatus);
        cmbStatus.Items.AddRange(new object[] { "نشط", "موقوف" });
        ConfigureFieldLabel(lblNotes, "الملاحظات");
        ConfigureTextBox(txtNotes, false);
        txtNotes.Multiline = true;
        txtNotes.ScrollBars = ScrollBars.Vertical;

        tblData.Controls.Add(lblCountry, 0, 0);
        tblData.Controls.Add(cmbCountry, 1, 0);
        tblData.Controls.Add(lblGovernorateCode, 2, 0);
        tblData.Controls.Add(txtGovernorateCode, 3, 0);
        tblData.Controls.Add(lblNameAr, 0, 1);
        tblData.Controls.Add(txtNameAr, 1, 1);
        tblData.Controls.Add(lblNameEn, 2, 1);
        tblData.Controls.Add(txtNameEn, 3, 1);
        tblData.Controls.Add(lblStatus, 0, 2);
        tblData.Controls.Add(cmbStatus, 1, 2);
        tblData.Controls.Add(lblNotes, 0, 3);
        tblData.Controls.Add(txtNotes, 1, 3);
        tblData.SetColumnSpan(txtNotes, 3);

        grpSearch.Dock = DockStyle.Fill;
        grpSearch.Text = "البحث والتصفية";
        grpSearch.RightToLeft = RightToLeft.Yes;
        grpSearch.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        tblRoot.Controls.Add(grpSearch, 0, 2);

        flpSearch.Dock = DockStyle.Fill;
        flpSearch.FlowDirection = FlowDirection.RightToLeft;
        flpSearch.RightToLeft = RightToLeft.Yes;
        flpSearch.WrapContents = false;
        flpSearch.Padding = new Padding(8, 8, 8, 4);
        grpSearch.Controls.Add(flpSearch);

        ConfigureInlineLabel(lblSearch, "بحث:");
        txtSearch.Width = 320;
        txtSearch.Margin = new Padding(6);
        txtSearch.PlaceholderText = "ابحث بالكود أو الاسم...";
        txtSearch.RightToLeft = RightToLeft.Yes;
        txtSearch.TextAlign = HorizontalAlignment.Right;
        txtSearch.TextChanged += txtSearch_TextChanged;
        ConfigureInlineLabel(lblStatusFilter, "الحالة:");
        cmbStatusFilter.Width = 150;
        cmbStatusFilter.Margin = new Padding(6);
        cmbStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbStatusFilter.RightToLeft = RightToLeft.Yes;
        cmbStatusFilter.Items.AddRange(new object[] { "الكل", "نشط", "موقوف" });
        flpSearch.Controls.AddRange(new Control[] { lblSearch, txtSearch, lblStatusFilter, cmbStatusFilter });

        dgvGovernorates.Dock = DockStyle.Fill;
        dgvGovernorates.AllowUserToAddRows = false;
        dgvGovernorates.AllowUserToDeleteRows = false;
        dgvGovernorates.AllowUserToResizeRows = false;
        dgvGovernorates.ReadOnly = true;
        dgvGovernorates.MultiSelect = false;
        dgvGovernorates.RowHeadersVisible = false;
        dgvGovernorates.RightToLeft = RightToLeft.Yes;
        dgvGovernorates.BackgroundColor = Color.White;
        dgvGovernorates.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvGovernorates.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvGovernorates.ColumnHeadersHeight = 36;
        dgvGovernorates.RowTemplate.Height = 34;
        dgvGovernorates.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        dgvGovernorates.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        dgvGovernorates.Columns.Add("colCode", "كود المحافظة");
        dgvGovernorates.Columns.Add("colNameAr", "اسم المحافظة");
        dgvGovernorates.Columns.Add("colCountry", "الدولة");
        dgvGovernorates.Columns.Add("colStatus", "الحالة");
        tblRoot.Controls.Add(dgvGovernorates, 0, 3);

        flpPagination.Dock = DockStyle.Fill;
        flpPagination.FlowDirection = FlowDirection.RightToLeft;
        flpPagination.RightToLeft = RightToLeft.Yes;
        flpPagination.WrapContents = false;
        flpPagination.Padding = new Padding(0, 8, 0, 0);
        tblRoot.Controls.Add(flpPagination, 0, 4);
        ConfigurePagerButton(btnFirst, "الأول");
        ConfigurePagerButton(btnPrevious, "السابق");
        lblPage.AutoSize = false;
        lblPage.Size = new Size(70, 30);
        lblPage.Margin = new Padding(8);
        lblPage.TextAlign = ContentAlignment.MiddleCenter;
        lblPage.Text = "1 / 1";
        ConfigurePagerButton(btnNext, "التالي");
        ConfigurePagerButton(btnLast, "الأخير");
        flpPagination.Controls.AddRange(new Control[] { btnFirst, btnPrevious, lblPage, btnNext, btnLast });

        tblAudit.Dock = DockStyle.Fill;
        tblAudit.BackColor = Color.White;
        tblAudit.RightToLeft = RightToLeft.Yes;
        tblAudit.ColumnCount = 8;
        tblAudit.RowCount = 1;
        for (var i = 0; i < 8; i++) tblAudit.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
        tblRoot.Controls.Add(tblAudit, 0, 5);
        ConfigureAuditLabel(lblCreatedCaption, "الإنشاء:", true);
        ConfigureAuditLabel(lblCreatedValue, "—", false);
        ConfigureAuditLabel(lblModifiedCaption, "آخر تعديل:", true);
        ConfigureAuditLabel(lblModifiedValue, "—", false);
        ConfigureAuditLabel(lblEditCountCaption, "مرات التعديل:", true);
        ConfigureAuditLabel(lblEditCountValue, "0", false);
        ConfigureAuditLabel(lblPrintCountCaption, "مرات الطباعة:", true);
        ConfigureAuditLabel(lblPrintCountValue, "0", false);
        tblAudit.Controls.Add(lblCreatedCaption, 0, 0);
        tblAudit.Controls.Add(lblCreatedValue, 1, 0);
        tblAudit.Controls.Add(lblModifiedCaption, 2, 0);
        tblAudit.Controls.Add(lblModifiedValue, 3, 0);
        tblAudit.Controls.Add(lblEditCountCaption, 4, 0);
        tblAudit.Controls.Add(lblEditCountValue, 5, 0);
        tblAudit.Controls.Add(lblPrintCountCaption, 6, 0);
        tblAudit.Controls.Add(lblPrintCountValue, 7, 0);

        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(247, 249, 252);
        Font = new Font("Segoe UI", 10F);
        Name = "UcGovernorates";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(1280, 760);
        ResumeLayout(false);
    }

    private static void ConfigureToolbarButton(Button button, string text)
    {
        button.Text = text;
        button.Size = new Size(88, 38);
        button.Margin = new Padding(4);
        button.TextAlign = ContentAlignment.MiddleCenter;
        button.UseVisualStyleBackColor = true;
    }

    private static void ConfigureFieldLabel(Label label, string text)
    {
        label.Text = text;
        label.Dock = DockStyle.Fill;
        label.TextAlign = ContentAlignment.MiddleRight;
        label.AutoSize = false;
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

    private static void ConfigureCombo(ComboBox comboBox)
    {
        comboBox.Dock = DockStyle.Fill;
        comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        comboBox.Margin = new Padding(6, 5, 6, 5);
        comboBox.RightToLeft = RightToLeft.Yes;
    }

    private static void ConfigureInlineLabel(Label label, string text)
    {
        label.Text = text;
        label.AutoSize = true;
        label.Margin = new Padding(6, 10, 6, 6);
        label.TextAlign = ContentAlignment.MiddleRight;
    }

    private static void ConfigurePagerButton(Button button, string text)
    {
        button.Text = text;
        button.Size = new Size(78, 30);
        button.Margin = new Padding(4);
    }

    private static void ConfigureAuditLabel(Label label, string text, bool bold)
    {
        label.Text = text;
        label.Dock = DockStyle.Fill;
        label.TextAlign = ContentAlignment.MiddleRight;
        label.Font = new Font("Segoe UI", 9F, bold ? FontStyle.Bold : FontStyle.Regular);
    }
}
