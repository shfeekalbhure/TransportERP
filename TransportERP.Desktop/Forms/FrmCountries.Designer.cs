using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop;

public partial class FrmCountries
{
    private System.ComponentModel.IContainer? components;
    private TableLayoutPanel tblRoot = null!;
    private TableLayoutPanel tblContent = null!;
    private Panel pnlHeader = null!;
    private Panel pnlForm = null!;
    private Panel pnlActions = null!;
    private Panel pnlSearch = null!;
    private Panel pnlGrid = null!;
    private Panel pnlAudit = null!;
    private Panel pnlCounters = null!;
    private RequiredTextBox txtCountryCode = null!;
    private RequiredTextBox txtCountryNameAr = null!;
    private RequiredTextBox txtCountryNameEn = null!;
    private RequiredTextBox txtIsoCode = null!;
    private RequiredTextBox txtDialCode = null!;
    private LookupComboBox cboCurrency = null!;
    private LookupComboBox cboRegion = null!;
    private LookupComboBox cboLanguage = null!;
    private LookupComboBox cboStatus = null!;
    private TextBox txtNotes = null!;
    private TextBox txtSearchAll = null!;
    private TextBox txtSearchCode = null!;
    private TextBox txtSearchName = null!;
    private ComboBox cboSearchStatus = null!;
    private Button btnNew = null!;
    private Button btnSave = null!;
    private Button btnEdit = null!;
    private Button btnDelete = null!;
    private Button btnPrint = null!;
    private Button btnRefresh = null!;
    private Button btnClose = null!;
    private Button btnSearch = null!;
    private Button btnResetSearch = null!;
    private DataGridView dgvCountries = null!;
    private Label lblResultCount = null!;
    private Label lblCreatedAtValue = null!;
    private Label lblCreatedByValue = null!;
    private Label lblUpdatedAtValue = null!;
    private Label lblUpdatedByValue = null!;
    private Label lblViewCountValue = null!;
    private Label lblSaveCountValue = null!;
    private Label lblEditCountValue = null!;
    private Label lblPrintCountValue = null!;
    private Label lblLastPrintAtValue = null!;
    private Label lblLastPrintByValue = null!;
    private TransportStatusBar statusBar = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        tblRoot = new TableLayoutPanel();
        tblContent = new TableLayoutPanel();
        pnlHeader = new Panel();
        pnlForm = new Panel();
        pnlActions = new Panel();
        pnlSearch = new Panel();
        pnlGrid = new Panel();
        pnlAudit = new Panel();
        pnlCounters = new Panel();
        statusBar = new TransportStatusBar();

        SuspendLayout();
        BackColor = UiTheme.WindowBackground;
        Font = UiTheme.CreateRegularFont(10F);
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        WindowState = FormWindowState.Maximized;
        MinimumSize = new Size(1280, 800);
        Text = "TransportERP - الدول";

        tblRoot.Dock = DockStyle.Fill;
        tblRoot.ColumnCount = 1;
        tblRoot.RowCount = 2;
        tblRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        tblRoot.Padding = new Padding(14, 10, 14, 0);
        tblRoot.Controls.Add(tblContent, 0, 0);
        tblRoot.Controls.Add(statusBar, 0, 1);

        tblContent.Dock = DockStyle.Fill;
        tblContent.ColumnCount = 1;
        tblContent.RowCount = 7;
        tblContent.RowStyles.Add(new RowStyle(SizeType.Absolute, 74F));
        tblContent.RowStyles.Add(new RowStyle(SizeType.Absolute, 250F));
        tblContent.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F));
        tblContent.RowStyles.Add(new RowStyle(SizeType.Absolute, 102F));
        tblContent.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tblContent.RowStyles.Add(new RowStyle(SizeType.Absolute, 130F));
        tblContent.RowStyles.Add(new RowStyle(SizeType.Absolute, 8F));

        ConfigureHeader();
        ConfigureForm();
        ConfigureActions();
        ConfigureSearch();
        ConfigureGrid();
        ConfigureBottomPanels();

        tblContent.Controls.Add(pnlHeader, 0, 0);
        tblContent.Controls.Add(pnlForm, 0, 1);
        tblContent.Controls.Add(pnlActions, 0, 2);
        tblContent.Controls.Add(pnlSearch, 0, 3);
        tblContent.Controls.Add(pnlGrid, 0, 4);

        var bottom = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, Margin = Padding.Empty };
        bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        bottom.Controls.Add(pnlAudit, 0, 0);
        bottom.Controls.Add(pnlCounters, 1, 0);
        tblContent.Controls.Add(bottom, 0, 5);

        Controls.Add(tblRoot);
        ResumeLayout(false);
    }

    private void ConfigureHeader()
    {
        pnlHeader.Dock = DockStyle.Fill;
        pnlHeader.BackColor = Color.White;
        pnlHeader.Padding = new Padding(18, 8, 18, 8);
        pnlHeader.Margin = new Padding(0, 0, 0, 8);
        var title = new Label { Dock = DockStyle.Top, Height = 34, Text = "الدول", Font = UiTheme.CreateBoldFont(22F), ForeColor = UiTheme.HeadingText, TextAlign = ContentAlignment.MiddleRight };
        var trail = new Label { Dock = DockStyle.Fill, Text = "التهيئة العامة  ‹  البيانات الجغرافية  ‹  الدول", ForeColor = UiTheme.SecondaryText, TextAlign = ContentAlignment.MiddleRight };
        pnlHeader.Controls.Add(trail);
        pnlHeader.Controls.Add(title);
    }

    private void ConfigureForm()
    {
        pnlForm.Dock = DockStyle.Fill;
        pnlForm.BackColor = Color.White;
        pnlForm.Padding = new Padding(18, 12, 18, 12);
        pnlForm.Margin = new Padding(0, 0, 0, 8);
        var grid = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 5, RightToLeft = RightToLeft.Yes };
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145F));
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145F));
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        for (int i = 0; i < 5; i++) grid.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));

        txtCountryCode = RequiredBox(); txtCountryCode.Text = "SAU";
        txtCountryNameAr = RequiredBox(); txtCountryNameAr.Text = "المملكة العربية السعودية";
        txtCountryNameEn = RequiredBox(); txtCountryNameEn.Text = "Kingdom of Saudi Arabia";
        txtIsoCode = RequiredBox(); txtIsoCode.Text = "SA";
        txtDialCode = RequiredBox(); txtDialCode.Text = "966";
        cboCurrency = RequiredCombo("ريال سعودي (SAR)", "درهم إماراتي (AED)", "دولار أمريكي (USD)");
        cboRegion = RequiredCombo("آسيا", "أفريقيا", "أوروبا", "أمريكا الشمالية");
        cboLanguage = OptionalCombo("العربية", "English");
        cboStatus = RequiredCombo("نشط", "غير نشط");
        txtNotes = new TextBox { Dock = DockStyle.Fill, Multiline = true, BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Text = "ملاحظات اختيارية", RightToLeft = RightToLeft.Yes };

        AddField(grid, 0, 0, "كود الدولة *", txtCountryCode);
        AddField(grid, 0, 2, "العملة الافتراضية *", cboCurrency);
        AddField(grid, 1, 0, "اسم الدولة (عربي) *", txtCountryNameAr);
        AddField(grid, 1, 2, "المنطقة الجغرافية *", cboRegion);
        AddField(grid, 2, 0, "اسم الدولة (إنجليزي) *", txtCountryNameEn);
        AddField(grid, 2, 2, "اللغة الافتراضية", cboLanguage);
        AddField(grid, 3, 0, "رمز ISO *", txtIsoCode);
        AddField(grid, 3, 2, "الحالة *", cboStatus);
        AddField(grid, 4, 0, "مفتاح الاتصال *", txtDialCode);
        AddField(grid, 4, 2, "ملاحظات", txtNotes);
        pnlForm.Controls.Add(grid);
    }

    private void ConfigureActions()
    {
        pnlActions.Dock = DockStyle.Fill;
        pnlActions.BackColor = Color.Transparent;
        var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(0, 4, 0, 4), WrapContents = false };
        btnNew = ActionButton("جديد", Color.FromArgb(28, 105, 225)); btnNew.Click += btnNew_Click;
        btnSave = ActionButton("حفظ", Color.FromArgb(45, 157, 83));
        btnEdit = ActionButton("تعديل", Color.FromArgb(245, 145, 20));
        btnDelete = ActionButton("حذف", Color.FromArgb(225, 55, 55));
        btnPrint = ActionButton("طباعة", Color.White, Color.Black);
        btnRefresh = ActionButton("تحديث", Color.White, Color.Black);
        btnClose = ActionButton("إغلاق", Color.White, Color.Black); btnClose.Click += btnClose_Click;
        flow.Controls.AddRange(new Control[] { btnNew, btnSave, btnEdit, btnDelete, btnPrint, btnRefresh, btnClose });
        pnlActions.Controls.Add(flow);
    }

    private void ConfigureSearch()
    {
        pnlSearch.Dock = DockStyle.Fill;
        pnlSearch.BackColor = Color.White;
        pnlSearch.Padding = new Padding(14, 8, 14, 8);
        pnlSearch.Margin = new Padding(0, 0, 0, 8);
        var grid = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 7, RowCount = 2, RightToLeft = RightToLeft.Yes };
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17F));
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14F));
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
        grid.Controls.Add(new Label { Text = "بحث وتصفية", Font = UiTheme.CreateBoldFont(11F), ForeColor = UiTheme.PrimaryBlue, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight }, 0, 0);
        grid.SetColumnSpan(grid.GetControlFromPosition(0, 0)!, 7);
        txtSearchAll = SearchBox("ابحث في جميع الحقول...");
        txtSearchCode = SearchBox("كود الدولة");
        txtSearchName = SearchBox("اسم الدولة");
        cboSearchStatus = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, RightToLeft = RightToLeft.Yes };
        cboSearchStatus.Items.AddRange(new object[] { "الكل", "نشط", "غير نشط" }); cboSearchStatus.SelectedIndex = 0;
        btnSearch = ActionButton("بحث", UiTheme.PrimaryBlue); btnSearch.Click += btnSearch_Click;
        btnResetSearch = ActionButton("إعادة تعيين", Color.White, Color.Black); btnResetSearch.Click += btnResetSearch_Click;
        lblResultCount = new Label { Dock = DockStyle.Fill, Text = "0", BackColor = Color.FromArgb(245, 248, 252), TextAlign = ContentAlignment.MiddleCenter, Font = UiTheme.CreateBoldFont(11F) };
        grid.Controls.Add(txtSearchAll, 0, 1); grid.Controls.Add(txtSearchCode, 1, 1); grid.Controls.Add(txtSearchName, 2, 1); grid.Controls.Add(cboSearchStatus, 3, 1); grid.Controls.Add(btnSearch, 4, 1); grid.Controls.Add(btnResetSearch, 5, 1); grid.Controls.Add(lblResultCount, 6, 1);
        pnlSearch.Controls.Add(grid);
    }

    private void ConfigureGrid()
    {
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.BackColor = Color.White;
        pnlGrid.Padding = new Padding(10);
        pnlGrid.Margin = new Padding(0, 0, 0, 8);
        var title = new Label { Dock = DockStyle.Top, Height = 32, Text = "قائمة الدول", Font = UiTheme.CreateBoldFont(12F), ForeColor = UiTheme.PrimaryBlue, TextAlign = ContentAlignment.MiddleRight };
        dgvCountries = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, RowHeadersVisible = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, SelectionMode = DataGridViewSelectionMode.FullRowSelect, BackgroundColor = Color.White, BorderStyle = BorderStyle.None, RightToLeft = RightToLeft.Yes };
        string[] headers = { "#", "كود الدولة", "اسم الدولة (عربي)", "اسم الدولة (إنجليزي)", "رمز ISO", "مفتاح الاتصال", "العملة الافتراضية", "المنطقة الجغرافية", "الحالة" };
        foreach (var h in headers) dgvCountries.Columns.Add(h.Replace(" ", ""), h);
        pnlGrid.Controls.Add(dgvCountries);
        pnlGrid.Controls.Add(title);
    }

    private void ConfigureBottomPanels()
    {
        pnlAudit.Dock = DockStyle.Fill; pnlAudit.BackColor = Color.White; pnlAudit.Padding = new Padding(12); pnlAudit.Margin = new Padding(0, 0, 6, 0);
        pnlCounters.Dock = DockStyle.Fill; pnlCounters.BackColor = Color.White; pnlCounters.Padding = new Padding(12); pnlCounters.Margin = new Padding(6, 0, 0, 0);

        var audit = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 3, RightToLeft = RightToLeft.Yes };
        audit.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F)); audit.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F)); audit.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F)); audit.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        audit.Controls.Add(SectionTitle("بيانات الإنشاء والتعديل"), 0, 0); audit.SetColumnSpan(audit.GetControlFromPosition(0, 0)!, 4);
        lblCreatedAtValue = ValueLabel(); lblCreatedByValue = ValueLabel(); lblUpdatedAtValue = ValueLabel(); lblUpdatedByValue = ValueLabel();
        AddReadOnly(audit, 1, 0, "تاريخ الإنشاء", lblCreatedAtValue); AddReadOnly(audit, 1, 2, "أنشئ بواسطة", lblCreatedByValue); AddReadOnly(audit, 2, 0, "آخر تعديل", lblUpdatedAtValue); AddReadOnly(audit, 2, 2, "آخر تعديل بواسطة", lblUpdatedByValue);
        pnlAudit.Controls.Add(audit);

        var counters = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 6, RowCount = 3, RightToLeft = RightToLeft.Yes };
        for (int i = 0; i < 6; i++) counters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.66F));
        counters.Controls.Add(SectionTitle("العدادات والإحصاءات"), 0, 0); counters.SetColumnSpan(counters.GetControlFromPosition(0, 0)!, 6);
        lblViewCountValue = Counter(counters, 0, "عدد مرات العرض"); lblSaveCountValue = Counter(counters, 1, "عدد مرات الحفظ"); lblEditCountValue = Counter(counters, 2, "عدد مرات التعديل"); lblPrintCountValue = Counter(counters, 3, "عدد مرات الطباعة");
        lblLastPrintAtValue = ValueLabel(); lblLastPrintByValue = ValueLabel();
        counters.Controls.Add(new Label { Text = "آخر طباعة", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight }, 4, 1); counters.Controls.Add(lblLastPrintAtValue, 4, 2);
        counters.Controls.Add(new Label { Text = "آخر طباعة بواسطة", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight }, 5, 1); counters.Controls.Add(lblLastPrintByValue, 5, 2);
        pnlCounters.Controls.Add(counters);
    }

    private static RequiredTextBox RequiredBox() => new() { Dock = DockStyle.Fill, IsRequired = true, Margin = new Padding(4), BackColor = Color.FromArgb(255, 250, 214) };
    private static LookupComboBox RequiredCombo(params string[] items) { var c = new LookupComboBox { Dock = DockStyle.Fill, IsRequired = true, Margin = new Padding(4), BackColor = Color.FromArgb(255, 250, 214) }; c.Items.AddRange(items); if (items.Length > 0) c.SelectedIndex = 0; return c; }
    private static LookupComboBox OptionalCombo(params string[] items) { var c = new LookupComboBox { Dock = DockStyle.Fill, IsRequired = false, Margin = new Padding(4), BackColor = Color.White }; c.Items.AddRange(items); if (items.Length > 0) c.SelectedIndex = 0; return c; }
    private static TextBox SearchBox(string placeholder) => new() { Dock = DockStyle.Fill, PlaceholderText = placeholder, RightToLeft = RightToLeft.Yes, BorderStyle = BorderStyle.FixedSingle, Margin = new Padding(4) };
    private static Button ActionButton(string text, Color back, Color? fore = null) => new() { Text = text, Width = 118, Height = 40, Margin = new Padding(5), FlatStyle = FlatStyle.Flat, BackColor = back, ForeColor = fore ?? Color.White, Font = UiTheme.CreateBoldFont(10F), Cursor = Cursors.Hand };
    private static void AddField(TableLayoutPanel grid, int row, int labelColumn, string label, Control control) { grid.Controls.Add(new Label { Text = label, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight, ForeColor = label.Contains('*') ? Color.FromArgb(170, 40, 40) : UiTheme.HeadingText }, labelColumn, row); grid.Controls.Add(control, labelColumn + 1, row); }
    private static Label SectionTitle(string text) => new() { Text = text, Dock = DockStyle.Fill, Font = UiTheme.CreateBoldFont(11F), ForeColor = UiTheme.PrimaryBlue, TextAlign = ContentAlignment.MiddleRight };
    private static Label ValueLabel() => new() { Dock = DockStyle.Fill, BackColor = Color.FromArgb(245, 248, 252), TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(4) };
    private static void AddReadOnly(TableLayoutPanel grid, int row, int labelColumn, string text, Label value) { grid.Controls.Add(new Label { Text = text, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight }, labelColumn, row); grid.Controls.Add(value, labelColumn + 1, row); }
    private static Label Counter(TableLayoutPanel grid, int column, string title) { var label = ValueLabel(); grid.Controls.Add(new Label { Text = title, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }, column, 1); grid.Controls.Add(label, column, 2); return label; }
}
