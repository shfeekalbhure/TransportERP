namespace TransportERP.Desktop.Views.Setup.Geographic;

partial class UcCities
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
    private Label lblGovernorate = null!;
    private ComboBox cmbGovernorate = null!;
    private Label lblDirectorate = null!;
    private ComboBox cmbDirectorate = null!;
    private Label lblCityCode = null!;
    private TextBox txtCityCode = null!;
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

    private DataGridView dgvCities = null!;
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
        lblCountry = new Label();
        cmbCountry = new ComboBox();
        lblGovernorate = new Label();
        cmbGovernorate = new ComboBox();
        lblDirectorate = new Label();
        cmbDirectorate = new ComboBox();
        lblCityCode = new Label();
        txtCityCode = new TextBox();
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
        dgvCities = new DataGridView();
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

        tblRoot.SuspendLayout();
        flpToolbar.SuspendLayout();
        grpData.SuspendLayout();
        tblData.SuspendLayout();
        grpSearch.SuspendLayout();
        flpSearch.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvCities).BeginInit();
        flpPagination.SuspendLayout();
        tblAudit.SuspendLayout();
        SuspendLayout();

        // tblRoot
        tblRoot.BackColor = Color.FromArgb(247, 249, 252);
        tblRoot.ColumnCount = 1;
        tblRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tblRoot.Controls.Add(flpToolbar, 0, 0);
        tblRoot.Controls.Add(grpData, 0, 1);
        tblRoot.Controls.Add(grpSearch, 0, 2);
        tblRoot.Controls.Add(dgvCities, 0, 3);
        tblRoot.Controls.Add(flpPagination, 0, 4);
        tblRoot.Controls.Add(tblAudit, 0, 5);
        tblRoot.Dock = DockStyle.Fill;
        tblRoot.Padding = new Padding(16);
        tblRoot.RowCount = 6;
        tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
        tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 252F));
        tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
        tblRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
        tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));

        // toolbar
        flpToolbar.BackColor = Color.FromArgb(235, 241, 249);
        flpToolbar.Controls.Add(btnNew);
        flpToolbar.Controls.Add(btnSave);
        flpToolbar.Controls.Add(btnEdit);
        flpToolbar.Controls.Add(btnStop);
        flpToolbar.Controls.Add(btnDelete);
        flpToolbar.Controls.Add(btnPrint);
        flpToolbar.Controls.Add(btnClose);
        flpToolbar.Dock = DockStyle.Fill;
        flpToolbar.FlowDirection = FlowDirection.RightToLeft;
        flpToolbar.Padding = new Padding(8, 7, 8, 5);
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

        // data container
        grpData.Controls.Add(tblData);
        grpData.Dock = DockStyle.Fill;
        grpData.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        grpData.Padding = new Padding(12);
        grpData.RightToLeft = RightToLeft.Yes;
        grpData.Text = "البيانات الرئيسية";

        tblData.ColumnCount = 4;
        tblData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
        tblData.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tblData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
        tblData.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tblData.Dock = DockStyle.Fill;
        tblData.RightToLeft = RightToLeft.Yes;
        tblData.RowCount = 5;
        tblData.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        tblData.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        tblData.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        tblData.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        tblData.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        tblData.Controls.Add(lblCountry, 0, 0);
        tblData.Controls.Add(cmbCountry, 1, 0);
        tblData.Controls.Add(lblGovernorate, 2, 0);
        tblData.Controls.Add(cmbGovernorate, 3, 0);
        tblData.Controls.Add(lblDirectorate, 0, 1);
        tblData.Controls.Add(cmbDirectorate, 1, 1);
        tblData.Controls.Add(lblCityCode, 2, 1);
        tblData.Controls.Add(txtCityCode, 3, 1);
        tblData.Controls.Add(lblNameAr, 0, 2);
        tblData.Controls.Add(txtNameAr, 1, 2);
        tblData.Controls.Add(lblNameEn, 2, 2);
        tblData.Controls.Add(txtNameEn, 3, 2);
        tblData.Controls.Add(lblStatus, 0, 3);
        tblData.Controls.Add(cmbStatus, 1, 3);
        tblData.Controls.Add(lblNotes, 0, 4);
        tblData.Controls.Add(txtNotes, 1, 4);
        tblData.SetColumnSpan(txtNotes, 3);

        ConfigureFieldLabel(lblCountry, "الدولة *");
        ConfigureFieldLabel(lblGovernorate, "المحافظة *");
        ConfigureFieldLabel(lblDirectorate, "المديرية *");
        ConfigureFieldLabel(lblCityCode, "كود المدينة *");
        ConfigureFieldLabel(lblNameAr, "الاسم العربي *");
        ConfigureFieldLabel(lblNameEn, "الاسم الإنجليزي");
        ConfigureFieldLabel(lblStatus, "الحالة");
        ConfigureFieldLabel(lblNotes, "الملاحظات");
        ConfigureCombo(cmbCountry);
        ConfigureCombo(cmbGovernorate);
        ConfigureCombo(cmbDirectorate);
        ConfigureTextBox(txtCityCode, true);
        ConfigureTextBox(txtNameAr, true);
        ConfigureTextBox(txtNameEn, false);
        ConfigureCombo(cmbStatus);
        cmbStatus.Items.AddRange(new object[] { "نشط", "موقوف" });
        ConfigureTextBox(txtNotes, false);
        txtNotes.Multiline = true;
        txtNotes.ScrollBars = ScrollBars.Vertical;

        // search
        grpSearch.Controls.Add(flpSearch);
        grpSearch.Dock = DockStyle.Fill;
        grpSearch.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        grpSearch.RightToLeft = RightToLeft.Yes;
        grpSearch.Text = "البحث والتصفية";
        flpSearch.Controls.Add(lblSearch);
        flpSearch.Controls.Add(txtSearch);
        flpSearch.Controls.Add(lblStatusFilter);
        flpSearch.Controls.Add(cmbStatusFilter);
        flpSearch.Dock = DockStyle.Fill;
        flpSearch.FlowDirection = FlowDirection.RightToLeft;
        flpSearch.Padding = new Padding(8, 8, 8, 4);
        flpSearch.RightToLeft = RightToLeft.Yes;
        flpSearch.WrapContents = false;
        ConfigureInlineLabel(lblSearch, "بحث:");
        txtSearch.Margin = new Padding(6);
        txtSearch.PlaceholderText = "ابحث بالكود أو الاسم...";
        txtSearch.RightToLeft = RightToLeft.Yes;
        txtSearch.Size = new Size(320, 32);
        txtSearch.TextAlign = HorizontalAlignment.Right;
        txtSearch.TextChanged += txtSearch_TextChanged;
        ConfigureInlineLabel(lblStatusFilter, "الحالة:");
        cmbStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbStatusFilter.Items.AddRange(new object[] { "الكل", "نشط", "موقوف" });
        cmbStatusFilter.Margin = new Padding(6);
        cmbStatusFilter.RightToLeft = RightToLeft.Yes;
        cmbStatusFilter.Size = new Size(150, 32);

        // grid
        dgvCities.AllowUserToAddRows = false;
        dgvCities.AllowUserToDeleteRows = false;
        dgvCities.AllowUserToResizeRows = false;
        dgvCities.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvCities.BackgroundColor = Color.White;
        dgvCities.BorderStyle = BorderStyle.Fixed3D;
        dgvCities.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        dgvCities.ColumnHeadersHeight = 36;
        dgvCities.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        dgvCities.Dock = DockStyle.Fill;
        dgvCities.MultiSelect = false;
        dgvCities.ReadOnly = true;
        dgvCities.RightToLeft = RightToLeft.Yes;
        dgvCities.RowHeadersVisible = false;
        dgvCities.RowTemplate.Height = 34;
        dgvCities.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvCities.Columns.Add("colCode", "كود المدينة");
        dgvCities.Columns.Add("colNameAr", "اسم المدينة");
        dgvCities.Columns.Add("colDirectorate", "المديرية");
        dgvCities.Columns.Add("colGovernorate", "المحافظة");
        dgvCities.Columns.Add("colCountry", "الدولة");
        dgvCities.Columns.Add("colStatus", "الحالة");

        // pagination
        flpPagination.Controls.Add(btnFirst);
        flpPagination.Controls.Add(btnPrevious);
        flpPagination.Controls.Add(lblPage);
        flpPagination.Controls.Add(btnNext);
        flpPagination.Controls.Add(btnLast);
        flpPagination.Dock = DockStyle.Fill;
        flpPagination.FlowDirection = FlowDirection.RightToLeft;
        flpPagination.Padding = new Padding(0, 7, 0, 0);
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

        // audit
        tblAudit.BackColor = Color.White;
        tblAudit.ColumnCount = 8;
        for (var i = 0; i < 8; i++)
        {
            tblAudit.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
        }
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
        tblAudit.RowCount = 1;
        tblAudit.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        ConfigureAuditLabel(lblCreatedCaption, "الإنشاء:", true);
        ConfigureAuditLabel(lblCreatedValue, "—", false);
        ConfigureAuditLabel(lblModifiedCaption, "آخر تعديل:", true);
        ConfigureAuditLabel(lblModifiedValue, "—", false);
        ConfigureAuditLabel(lblEditCountCaption, "مرات التعديل:", true);
        ConfigureAuditLabel(lblEditCountValue, "0", false);
        ConfigureAuditLabel(lblPrintCountCaption, "مرات الطباعة:", true);
        ConfigureAuditLabel(lblPrintCountValue, "0", false);

        // UcCities
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(247, 249, 252);
        Controls.Add(tblRoot);
        Font = new Font("Segoe UI", 10F);
        Name = "UcCities";
        RightToLeft = RightToLeft.Yes;
        Size = new Size(1280, 760);

        tblRoot.ResumeLayout(false);
        flpToolbar.ResumeLayout(false);
        grpData.ResumeLayout(false);
        tblData.ResumeLayout(false);
        tblData.PerformLayout();
        grpSearch.ResumeLayout(false);
        flpSearch.ResumeLayout(false);
        flpSearch.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvCities).EndInit();
        flpPagination.ResumeLayout(false);
        tblAudit.ResumeLayout(false);
        ResumeLayout(false);
    }

    private static void ConfigureToolbarButton(Button button, string text)
    {
        button.Text = text;
        button.Size = new Size(88, 38);
        button.Margin = new Padding(4);
        button.UseVisualStyleBackColor = true;
    }

    private static void ConfigureFieldLabel(Label label, string text)
    {
        label.Text = text;
        label.Dock = DockStyle.Fill;
        label.TextAlign = ContentAlignment.MiddleRight;
        label.Margin = new Padding(4);
    }

    private static void ConfigureTextBox(TextBox textBox, bool required)
    {
        textBox.Dock = DockStyle.Fill;
        textBox.Margin = new Padding(8, 5, 8, 5);
        textBox.RightToLeft = RightToLeft.Yes;
        textBox.TextAlign = HorizontalAlignment.Right;
        textBox.BackColor = required ? Color.FromArgb(255, 250, 220) : Color.White;
    }

    private static void ConfigureCombo(ComboBox combo)
    {
        combo.Dock = DockStyle.Fill;
        combo.DropDownStyle = ComboBoxStyle.DropDownList;
        combo.Margin = new Padding(8, 5, 8, 5);
        combo.RightToLeft = RightToLeft.Yes;
    }

    private static void ConfigureInlineLabel(Label label, string text)
    {
        label.Text = text;
        label.AutoSize = false;
        label.Margin = new Padding(6);
        label.Size = new Size(72, 32);
        label.TextAlign = ContentAlignment.MiddleRight;
    }

    private static void ConfigurePagerButton(Button button, string text)
    {
        button.Text = text;
        button.Margin = new Padding(4);
        button.Size = new Size(88, 32);
    }

    private static void ConfigureAuditLabel(Label label, string text, bool bold)
    {
        label.Text = text;
        label.Dock = DockStyle.Fill;
        label.TextAlign = ContentAlignment.MiddleRight;
        label.Font = new Font("Segoe UI", 9F, bold ? FontStyle.Bold : FontStyle.Regular);
    }
}
