using System.ComponentModel;
using System.Drawing;

namespace TransportERP.Desktop.Forms.Security;

/// <summary>SEC-001 — المستخدمون. نموذج RTL مستقل يطابق بطاقة الشاشة المعتمدة.</summary>
public sealed class FrmUsers : Form
{
    private readonly BindingList<UserRow> _users = new();
    private readonly BindingSource _source = new();
    private readonly TextBox _code = RequiredTextBox();
    private readonly TextBox _nameAr = RequiredTextBox();
    private readonly TextBox _nameEn = PlainTextBox();
    private readonly TextBox _username = RequiredTextBox();
    private readonly TextBox _email = PlainTextBox();
    private readonly TextBox _mobile = PlainTextBox();
    private readonly ComboBox _status = RequiredCombo("نشط", "موقوف");
    private readonly TextBox _searchUser = PlainTextBox();
    private readonly TextBox _searchRole = PlainTextBox();
    private readonly TextBox _searchCompany = PlainTextBox();
    private readonly TextBox _searchBranch = PlainTextBox();
    private readonly ComboBox _searchStatus = RequiredCombo("الكل", "نشط", "موقوف");
    private readonly Label _audit = new() { Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight, ForeColor = Color.DimGray };
    private readonly Label _counter = new() { Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, ForeColor = Color.DimGray };
    private readonly DataGridView _grid = new();
    private UserRow? _selected;

    public FrmUsers()
    {
        Text = "المستخدمون — SEC-001";
        Name = nameof(FrmUsers);
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(1120, 720);
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        Font = new Font("Tahoma", 9F);
        BackColor = Color.FromArgb(247, 249, 252);

        _source.DataSource = _users;
        _users.Add(new UserRow("USR-001", "مدير النظام", "System Administrator", "admin", "مدير النظام", "نشط", "لم يسجل"));
        BuildLayout();
        ApplyFilter();
    }

    private void BuildLayout()
    {
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 6, Padding = new Padding(14), BackColor = BackColor };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 42));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 58));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.Controls.Add(new Label { Text = "المستخدمون  —  SEC-001", AutoSize = true, Font = new Font("Tahoma", 13F, FontStyle.Bold), ForeColor = Color.FromArgb(30, 64, 116), Padding = new Padding(6, 2, 6, 10) }, 0, 0);
        root.Controls.Add(CreateToolbar(), 0, 1);
        root.Controls.Add(CreateTabs(), 0, 2);
        root.Controls.Add(CreateSearch(), 0, 3);
        root.Controls.Add(CreateGrid(), 0, 4);
        root.Controls.Add(CreateAuditBar(), 0, 5);
        Controls.Add(root);
    }

    private Control CreateToolbar()
    {
        var bar = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.RightToLeft, WrapContents = false, Padding = new Padding(0, 0, 0, 9) };
        bar.Controls.AddRange([
            ActionButton("جديد", (_, _) => ClearEditor(), Color.FromArgb(33, 150, 243)),
            ActionButton("حفظ", (_, _) => SaveUser(), Color.FromArgb(46, 125, 50)),
            ActionButton("تعديل", (_, _) => LoadSelected(), Color.FromArgb(245, 124, 0)),
            ActionButton("إيقاف", (_, _) => SetStatus("موقوف"), Color.FromArgb(117, 117, 117)),
            ActionButton("حذف", (_, _) => DeleteSelected(), Color.FromArgb(198, 40, 40)),
            ActionButton("طباعة", (_, _) => PrintPreview(), Color.FromArgb(97, 97, 97))
        ]);
        return bar;
    }

    private Control CreateTabs()
    {
        var tabs = new TabControl { Dock = DockStyle.Fill, RightToLeft = RightToLeft.Yes, RightToLeftLayout = true };
        tabs.TabPages.Add(CreateAccountTab());
        tabs.TabPages.Add(CreateRolesTab());
        tabs.TabPages.Add(CreateScopeTab());
        tabs.TabPages.Add(CreateSecurityTab());
        tabs.TabPages.Add(CreateAuditTab());
        return tabs;
    }

    private TabPage CreateAccountTab()
    {
        var page = new TabPage("الحساب") { RightToLeft = RightToLeft.Yes, AutoScroll = true, BackColor = Color.White };
        var fields = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 4, Padding = new Padding(14), RightToLeft = RightToLeft.Yes };
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16)); fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16)); fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34));
        AddField(fields, 0, "كود المستخدم *", _code); AddField(fields, 0, "الاسم العربي *", _nameAr, 2);
        AddField(fields, 1, "الاسم الإنجليزي", _nameEn); AddField(fields, 1, "اسم الدخول *", _username, 2);
        AddField(fields, 2, "البريد الإلكتروني", _email); AddField(fields, 2, "الجوال", _mobile, 2);
        AddField(fields, 3, "الحالة *", _status);
        page.Controls.Add(fields);
        return page;
    }

    private TabPage CreateRolesTab() => DetailGridTab("الأدوار", "الدور", "شركة/فرع", "الحالة");
    private TabPage CreateScopeTab() => DetailGridTab("نطاق الوصول", "الشركة", "الفرع", "الوحدة التنظيمية");
    private TabPage CreateSecurityTab()
    {
        var page = new TabPage("الأمان") { RightToLeft = RightToLeft.Yes, BackColor = Color.White, Padding = new Padding(14) };
        page.Controls.Add(new Label { Dock = DockStyle.Top, Height = 42, Text = "إدارة حالة الحساب وسياسات الدخول من دون عرض كلمات المرور أو أسرار الاسترداد.", TextAlign = ContentAlignment.MiddleRight, ForeColor = Color.FromArgb(35, 55, 80) });
        return page;
    }
    private TabPage CreateAuditTab() => DetailGridTab("التدقيق", "التاريخ", "الإجراء", "المستخدم");

    private static TabPage DetailGridTab(string title, params string[] columns)
    {
        var page = new TabPage(title) { RightToLeft = RightToLeft.Yes, BackColor = Color.White, Padding = new Padding(12) };
        var grid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, AutoGenerateColumns = false, BackgroundColor = Color.White, RightToLeft = RightToLeft.Yes };
        foreach (var column in columns) grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = column, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
        page.Controls.Add(grid);
        return page;
    }

    private Control CreateSearch()
    {
        var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(8), BackColor = Color.White };
        AddFilter(panel, "اسم المستخدم", _searchUser);
        AddFilter(panel, "الحالة", _searchStatus);
        AddFilter(panel, "الدور", _searchRole);
        AddFilter(panel, "الشركة", _searchCompany);
        AddFilter(panel, "الفرع", _searchBranch);
        foreach (var control in new Control[] { _searchUser, _searchRole, _searchCompany, _searchBranch }) control.TextChanged += (_, _) => ApplyFilter();
        _searchStatus.SelectedIndexChanged += (_, _) => ApplyFilter();
        return panel;
    }

    private Control CreateGrid()
    {
        var panel = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, Padding = new Padding(12), BackColor = Color.White };
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _grid.Dock = DockStyle.Fill; _grid.AutoGenerateColumns = false; _grid.DataSource = _source; _grid.ReadOnly = true; _grid.AllowUserToAddRows = false; _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect; _grid.MultiSelect = false; _grid.RightToLeft = RightToLeft.Yes; _grid.BackgroundColor = Color.White;
        AddGridColumn(nameof(UserRow.Username), "اسم الدخول"); AddGridColumn(nameof(UserRow.NameAr), "الاسم العربي"); AddGridColumn(nameof(UserRow.Roles), "الأدوار"); AddGridColumn(nameof(UserRow.Status), "الحالة"); AddGridColumn(nameof(UserRow.LastLogin), "آخر دخول");
        _grid.SelectionChanged += (_, _) => LoadSelected();
        var paging = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.RightToLeft };
        paging.Controls.AddRange([new Button { Text = "الأول" }, new Button { Text = "السابق" }, new Label { Text = "1", Padding = new Padding(8) }, new Button { Text = "التالي" }, new Button { Text = "الأخير" }, _counter]);
        panel.Controls.Add(_grid, 0, 0); panel.Controls.Add(paging, 0, 1);
        return panel;
    }

    private Control CreateAuditBar()
    {
        var panel = new Panel { Dock = DockStyle.Fill, Height = 34, BackColor = Color.FromArgb(235, 240, 247), Padding = new Padding(8) };
        _audit.Text = "بيانات الإنشاء والتعديل — الشركة والفرع والسنة من سياق النظام الرئيسي فقط.";
        panel.Controls.Add(_audit);
        return panel;
    }

    private void ApplyFilter()
    {
        var query = _searchUser.Text.Trim(); var status = _searchStatus.Text;
        var view = _users.Where(x => (string.IsNullOrEmpty(query) || x.Username.Contains(query, StringComparison.OrdinalIgnoreCase) || x.NameAr.Contains(query, StringComparison.OrdinalIgnoreCase)) && (status == "الكل" || x.Status == status) && (string.IsNullOrWhiteSpace(_searchRole.Text) || x.Roles.Contains(_searchRole.Text, StringComparison.OrdinalIgnoreCase))).ToList();
        _source.DataSource = new BindingList<UserRow>(view); _counter.Text = "عدد السجلات: " + view.Count;
    }

    private void SaveUser()
    {
        if (string.IsNullOrWhiteSpace(_code.Text) || string.IsNullOrWhiteSpace(_nameAr.Text) || string.IsNullOrWhiteSpace(_username.Text)) { MessageBox.Show("أكمل الحقول الإلزامية.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        if (_selected is null) _users.Add(new UserRow(_code.Text.Trim(), _nameAr.Text.Trim(), _nameEn.Text.Trim(), _username.Text.Trim(), "—", _status.Text, "لم يسجل"));
        else { _selected.Code = _code.Text.Trim(); _selected.NameAr = _nameAr.Text.Trim(); _selected.NameEn = _nameEn.Text.Trim(); _selected.Username = _username.Text.Trim(); _selected.Status = _status.Text; }
        _audit.Text = "آخر تعديل بواسطة المستخدم الحالي — " + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        ClearEditor(); ApplyFilter();
    }

    private void LoadSelected()
    {
        if (_grid.CurrentRow?.DataBoundItem is not UserRow row) return;
        _selected = row; _code.Text = row.Code; _nameAr.Text = row.NameAr; _nameEn.Text = row.NameEn; _username.Text = row.Username; _status.SelectedItem = row.Status;
    }
    private void ClearEditor() { _selected = null; _code.Clear(); _nameAr.Clear(); _nameEn.Clear(); _username.Clear(); _email.Clear(); _mobile.Clear(); _status.SelectedIndex = 0; _code.Focus(); }
    private void SetStatus(string value) { if (_selected is null) return; _selected.Status = value; _status.SelectedItem = value; ApplyFilter(); }
    private void DeleteSelected() { if (_selected is null) return; if (MessageBox.Show("هل تريد حذف المستخدم المحدد؟", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) { _users.Remove(_selected); ClearEditor(); ApplyFilter(); } }
    private void PrintPreview() => MessageBox.Show("الطباعة تتطلب صلاحية وتُسجل في التدقيق عند ربط الخدمة.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

    private static TextBox RequiredTextBox() => new() { Dock = DockStyle.Fill, BackColor = Color.FromArgb(255, 253, 231), BorderStyle = BorderStyle.FixedSingle, RightToLeft = RightToLeft.Yes };
    private static TextBox PlainTextBox() => new() { Width = 150, Dock = DockStyle.Fill, BorderStyle = BorderStyle.FixedSingle, RightToLeft = RightToLeft.Yes };
    private static ComboBox RequiredCombo(params string[] values) { var box = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 130, Dock = DockStyle.Fill, BackColor = Color.FromArgb(255, 253, 231), RightToLeft = RightToLeft.Yes }; box.Items.AddRange(values); box.SelectedIndex = 0; return box; }
    private static Button ActionButton(string text, EventHandler action, Color color) { var button = new Button { Text = text, AutoSize = true, BackColor = color, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Margin = new Padding(4), Padding = new Padding(12, 5, 12, 5) }; button.FlatAppearance.BorderSize = 0; button.Click += action; return button; }
    private static void AddField(TableLayoutPanel panel, int row, string caption, Control control, int column = 0) { while (panel.RowCount <= row) panel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); panel.Controls.Add(new Label { Text = caption, AutoSize = true, Padding = new Padding(3, 7, 3, 3) }, column, row); panel.Controls.Add(control, column + 1, row); }
    private static void AddFilter(FlowLayoutPanel panel, string caption, Control control) { panel.Controls.Add(new Label { Text = caption + ":", AutoSize = true, Padding = new Padding(5, 7, 0, 0) }); control.Width = 135; panel.Controls.Add(control); }
    private void AddGridColumn(string property, string title) => _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = property, HeaderText = title, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
    private sealed class UserRow
    {
        public UserRow(string code, string nameAr, string nameEn, string username, string roles, string status, string lastLogin) { Code = code; NameAr = nameAr; NameEn = nameEn; Username = username; Roles = roles; Status = status; LastLogin = lastLogin; }
        public string Code { get; set; } public string NameAr { get; set; } public string NameEn { get; set; } public string Username { get; set; } public string Roles { get; set; } public string Status { get; set; } public string LastLogin { get; set; }
    }
}
