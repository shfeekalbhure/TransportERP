namespace TransportERP.Desktop.Views.Setup.Geographic;

partial class UcDirectorates
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
    private Label lblDirectorateCode = null!;
    private TextBox txtDirectorateCode = null!;
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
    private DataGridView dgvDirectorates = null!;
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
        lblDirectorateCode = new Label();
        txtDirectorateCode = new TextBox();
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
        dgvDirectorates = new DataGridView();
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
        ((System.ComponentModel.ISupportInitialize)dgvDirectorates).BeginInit();
        flpPagination.SuspendLayout();
        tblAudit.SuspendLayout();
        SuspendLayout();
        //
        // tblRoot
        //
        tblRoot.BackColor = Color.FromArgb(247, 249, 252);
        tblRoot.ColumnCount = 1;
        tblRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tblRoot.Controls.Add(flpToolbar, 0, 0);
        tblRoot.Controls.Add(grpData, 0, 1);
        tblRoot.Controls.Add(grpSearch, 0, 2);
        tblRoot.Controls.Add(dgvDirectorates, 0, 3);
        tblRoot.Controls.Add(flpPagination, 0, 4);
        tblRoot.Controls.Add(tblAudit, 0, 5);
        tblRoot.Dock = DockStyle.Fill;
        tblRoot.Padding = new Padding(12);
        tblRoot.RowCount = 6;
        tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
        tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 230F));
        tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 76F));
        tblRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
        tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F));
        //
        // flpToolbar
        //
        flpToolbar.AutoScroll = true;
        flpToolbar.Controls.Add(btnNew);
        flpToolbar.Controls.Add(btnSave);
        flpToolbar.Controls.Add(btnEdit);
        flpToolbar.Controls.Add(btnStop);
        flpToolbar.Controls.Add(btnDelete);
        flpToolbar.Controls.Add(btnPrint);
        flpToolbar.Controls.Add(btnClose);
        flpToolbar.Dock = DockStyle.Fill;
        flpToolbar.FlowDirection = FlowDirection.RightToLeft;
        flpToolbar.Padding = new Padding(4);
        flpToolbar.RightToLeft = RightToLeft.Yes;
        flpToolbar.WrapContents = false;
        //
        // toolbar buttons
        //
        ConfigureToolbarButton(btnNew, "جديد");
        ConfigureToolbarButton(btnSave, "حفظ");
        ConfigureToolbarButton(btnEdit, "تعديل");
        ConfigureToolbarButton(btnStop, "إيقاف");
        ConfigureToolbarButton(btnDelete, "حذف");
        ConfigureToolbarButton(btnPrint, "طباعة");
        ConfigureToolbarButton(btnClose, "إغلاق");
        btnNew.Click += btnNew_Click;
        //
        // grpData
        //
        grpData.Controls.Add(tblData);
        grpData.Dock = DockStyle.Fill;
        grpData.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        grpData.Padding = new Padding(12);
        grpData.RightToLeft = RightToLeft.Yes;
        grpData.Text = "البيانات الرئيسية";
        //
        // tblData
        //
        tblData.ColumnCount = 4;
        tblData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
        tblData.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tblData.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
        tblData.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tblData.Controls.Add(lblCountry, 0, 0);
        tblData.Controls.Add(cmbCountry, 1, 0);
        tblData.Controls.Add(lblGovernorate, 2, 0);
        tblData.Controls.Add(cmbGovernorate, 3, 0);
        tblData.Controls.Add(lblDirectorateCode, 0, 1);
        tblData.Controls.Add(txtDirectorateCode, 1, 1);
        tblData.Controls.Add(lblNameAr, 2, 1);
        tblData.Controls.Add(txtNameAr, 3, 1);
        tblData.Controls.Add(lblNameEn, 0, 2);
        tblData.Controls.Add(txtNameEn, 1, 2);
        tblData.Controls.Add(lblStatus, 2, 2);
        tblData.Controls.Add(cmbStatus, 3, 2);
        tblData.Controls.Add(lblNotes, 0, 3);
        tblData.Controls.Add(txtNotes, 1, 3);
        tblData.Dock = DockStyle.Fill;
        tblData.RightToLeft = RightToLeft.Yes;
        tblData.RowCount = 4;
        tblData.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        tblData.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        tblData.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        tblData.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tblData.SetColumnSpan(txtNotes, 3);
        //
        // labels and fields
        //
        ConfigureFieldLabel(lblCountry, "الدولة *");
        ConfigureFieldLabel(lblGovernorate, "المحافظة *");
        ConfigureFieldLabel(lblDirectorateCode, "كود المديرية *");
        ConfigureFieldLabel(lblNameAr, "الاسم العربي *");
        ConfigureFieldLabel(lblNameEn, "الاسم الإنجليزي");
        ConfigureFieldLabel(lblStatus, "الحالة");
        ConfigureFieldLabel(lblNotes, "الملاحظات");
        ConfigureCombo(cmbCountry);
        ConfigureCombo(cmbGovernorate);
        ConfigureTextBox(txtDirectorateCode, true);
        ConfigureTextBox(txtNameAr, true);
        ConfigureTextBox(txtNameEn, false);
        ConfigureCombo(cmbStatus);
        cmbStatus.Items.AddRange(new object[] { "نشط", "موقوف" });
        ConfigureTextBox(txtNotes, false);
        txtNotes.Multiline = true;
        txtNotes.ScrollBars = ScrollBars.Vertical;
        //
        // grpSearch
        //
        grpSearch.Controls.Add(flpSearch);
        grpSearch.Dock = DockStyle.Fill;
        grpSearch.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        grpSearch.RightToLeft = RightToLeft.Yes;
        grpSearch.Text = "البحث والتصفية";
        //
        // flpSearch
        //
        flpSearch.Controls.Add(lblSearch);
        flpSearch.Controls.Add(txtSearch);
        flpSearch.Controls.Add(lblStatusFilter);
        flpSearch.Controls.Add(cmbStatusFilter);
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
        cmbStatusFilter.Items.AddRange(new object[] { "الكل", "نشط", "موقوف" });
        cmbStatusFilter.Margin = new Padding(6);
        cmbStatusFilter.RightToLeft = RightToLeft.Yes;
        cmbStatusFilter.Size = new Size(150, 30);
        cmbStatusFilter.SelectedIndex = 0;
        //
        // dgvDirectorates
        //
        dgvDirectorates.AllowUserToAddRows = false;
        dgvDirectorates.AllowUserToDeleteRows = false;
        dgvDirectorates.AllowUserToResizeRows = false;
        dgvDirectorates.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvDirectorates.BackgroundColor = Color.White;
        dgvDirectorates.BorderStyle = BorderStyle.Fixed3D;
        dgvDirectorates.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        dgvDirectorates.ColumnHeadersHeight = 36;
        dgvDirectorates.Dock = DockStyle.Fill;
        dgvDirectorates.EnableHeadersVisualStyles = false;
        dgvDirectorates.MultiSelect = false;
        dgvDirectorates.ReadOnly = true;
        dgvDirectorates.RightToLeft = RightToLeft.Yes;
        dgvDirectorates.RowHeadersVisible = false;
        dgvDirectorates.RowTemplate.Height = 34;
        dgvDirectorates.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvDirectorates.Columns.Add("colCode", "كود المديرية");
        dgvDirectorates.Columns.Add("colNameAr", "اسم المديرية");
        dgvDirectorates.Columns.Add("colGovernorate", "المحافظة");
        dgvDirectorates.Columns.Add("colCountry", "الدولة");
        dgvDirectorates.Columns.Add("colStatus", "الحالة");
        //
        // flpPagination
        //
        flpPagination.Controls.Add(btnFirst);
        flpPagination.Controls.Add(btnPrevious);
        flpPagination.Controls.Add(lblPage);
        flpPagination.Controls.Add(btnNext);
        flpPagination.Controls.Add(btnLast);
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
        //
        // tblAudit
        //
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
        //
        // UcDirectorates
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(247, 249, 252);
        Controls.Add(tblRoot);
        Font = new Font("Segoe UI", 10F);
        Name = "UcDirectorates";
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
        ((System.ComponentModel.ISupportInitialize)dgvDirectorates).EndInit();
        flpPagination.ResumeLayout(false);
        tblAudit.ResumeLayout(false);
        ResumeLayout(false);
    }

    private static void ConfigureToolbarButton(Button button, string text)
    {
        button.AutoSize = false;
        button.FlatStyle = FlatStyle.System;
        button.Margin = new Padding(5, 4, 5, 4);
        button.Size = new Size(92, 38);
        button.Text = text;
        button.UseVisualStyleBackColor = true;
    }

    private static void ConfigureFieldLabel(Label label, string text)
    {
        label.Dock = DockStyle.Fill;
        label.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        label.Text = text;
        label.TextAlign = ContentAlignment.MiddleRight;
    }

    private static void ConfigureInlineLabel(Label label, string text)
    {
        label.AutoSize = false;
        label.Margin = new Padding(6);
        label.Size = new Size(72, 30);
        label.Text = text;
        label.TextAlign = ContentAlignment.MiddleRight;
    }

    private static void ConfigureTextBox(TextBox textBox, bool required)
    {
        textBox.Dock = DockStyle.Fill;
        textBox.Margin = new Padding(6);
        textBox.RightToLeft = RightToLeft.Yes;
        textBox.TextAlign = HorizontalAlignment.Right;
        if (required)
        {
            textBox.BackColor = Color.LightYellow;
        }
    }

    private static void ConfigureCombo(ComboBox comboBox)
    {
        comboBox.Dock = DockStyle.Fill;
        comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        comboBox.Margin = new Padding(6);
        comboBox.RightToLeft = RightToLeft.Yes;
    }

    private static void ConfigurePagerButton(Button button, string text)
    {
        button.Margin = new Padding(5);
        button.Size = new Size(82, 32);
        button.Text = text;
        button.UseVisualStyleBackColor = true;
    }

    private static void ConfigureAuditLabel(Label label, string text, bool bold)
    {
        label.Dock = DockStyle.Fill;
        label.Font = new Font("Segoe UI", 8.5F, bold ? FontStyle.Bold : FontStyle.Regular);
        label.Text = text;
        label.TextAlign = ContentAlignment.MiddleRight;
    }
}
