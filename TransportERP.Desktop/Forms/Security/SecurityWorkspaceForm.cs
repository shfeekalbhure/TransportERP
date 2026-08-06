using System.ComponentModel;
using System.Drawing;

namespace TransportERP.Desktop.Forms.Security;

/// <summary>
/// القالب الموحد لشاشات الأمن والإدارة. يعمل محلياً ببيانات منظمة إلى أن تتوفر API.
/// </summary>
public abstract class SecurityWorkspaceForm : Form
{
    private readonly BindingList<SecurityRecord> _records = new();
    private readonly BindingSource _source = new();
    private readonly TextBox _code = CreateTextBox();
    private readonly TextBox _name = CreateTextBox();
    private readonly ComboBox _status = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly TextBox _search = CreateTextBox();
    private readonly ComboBox _filter = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly Label _audit = new() { AutoSize = true, ForeColor = Color.DimGray, Padding = new Padding(8) };
    private readonly SecurityScreenDefinition _definition;
    private readonly SecurityScreenProfile _profile;
    private readonly Dictionary<string, TextBox> _details = new();

    protected SecurityWorkspaceForm(SecurityScreenDefinition definition)
    {
        _definition = definition;
        _profile = SecurityScreenProfiles.For(definition.Code);
        Text = definition.Title;
        Name = definition.FormName;
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(980, 640);
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        Font = new Font("Tahoma", 9F);
        BackColor = Color.FromArgb(247, 249, 252);

        _status.Items.AddRange(["نشط", "موقوف"]);
        _status.SelectedIndex = 0;
        _filter.Items.AddRange(["الكل", "نشط", "موقوف"]);
        _filter.SelectedIndex = 0;
        _source.DataSource = _records;

        var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 5, Padding = new Padding(14), BackColor = BackColor };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 42));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 58));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.Controls.Add(CreateHeader(), 0, 0);
        root.Controls.Add(CreateToolbar(), 0, 1);
        root.Controls.Add(CreateDataCard(), 0, 2);
        root.Controls.Add(CreateListCard(), 0, 3);
        root.Controls.Add(CreateAuditBar(), 0, 4);
        Controls.Add(root);

        SeedLocalData();
        ApplyFilter();
    }

    private Control CreateHeader() => new Label
    {
        Text = _definition.Title + "  —  " + _definition.Code,
        Dock = DockStyle.Fill, AutoSize = true, Font = new Font("Tahoma", 13F, FontStyle.Bold),
        ForeColor = Color.FromArgb(30, 64, 116), Padding = new Padding(6, 2, 6, 10)
    };

    private Control CreateToolbar()
    {
        var bar = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.RightToLeft, WrapContents = false, Padding = new Padding(0, 0, 0, 9) };
        bar.Controls.AddRange([
            Button("جديد", (_, _) => ClearEditor(), Color.FromArgb(33, 150, 243)),
            Button("حفظ", (_, _) => SaveLocal(), Color.FromArgb(46, 125, 50)),
            Button("تعديل", (_, _) => LoadSelected(), Color.FromArgb(245, 124, 0)),
            Button("إيقاف", (_, _) => SetStatus("موقوف"), Color.FromArgb(117, 117, 117)),
            Button("حذف", (_, _) => DeleteSelected(), Color.FromArgb(198, 40, 40)),
            Button("طباعة", (_, _) => PrintPreview(), Color.FromArgb(97, 97, 97))
        ]);
        return bar;
    }

    private Control CreateDataCard()
    {
        var group = Card("بيانات " + _definition.Title);
        var tabs = new TabControl { Dock = DockStyle.Fill, RightToLeftLayout = true, RightToLeft = RightToLeft.Yes };
        var tabNames = _profile.Tabs.Length == 0 ? new[] { "البيانات الرئيسية" } : _profile.Tabs;

        for (var index = 0; index < tabNames.Length; index++)
        {
            var page = new TabPage(tabNames[index]) { RightToLeft = RightToLeft.Yes, AutoScroll = true, BackColor = Color.White };
            if (index == 0)
            {
                page.Controls.Add(CreateProfileFields());
            }
            else
            {
                page.Controls.Add(CreateDetailTab(tabNames[index]));
            }
            tabs.TabPages.Add(page);
        }

        group.Controls.Add(tabs);
        return group;
    }

    private Control CreateProfileFields()
    {
        var captions = _profile.Fields.Length == 0
            ? new[] { "الكود *", "الاسم *", "الحالة *", _definition.Field1, _definition.Field2, _definition.Field3 }
            : _profile.Fields;
        var fields = new TableLayoutPanel
        {
            Dock = DockStyle.Top, ColumnCount = 4, AutoSize = true, Padding = new Padding(12), RightToLeft = RightToLeft.Yes
        };
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32));

        for (var index = 0; index < captions.Length; index++)
        {
            Control control = index switch
            {
                0 => _code,
                1 => _name,
                _ when captions[index].Contains("الحالة") => _status,
                _ => AddDetail(captions[index])
            };
            if (control is TextBox textBox) textBox.ReadOnly = _profile.ReadOnly;
            AddField(fields, index / 2, captions[index], control, captions[index].Contains("*"), (index % 2) * 2);
        }
        return fields;
    }

    private Control CreateDetailTab(string tabName)
    {
        var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(12), BackColor = Color.White };
        var note = new Label
        {
            Dock = DockStyle.Top, Height = 36, Text = "تبويب " + tabName + " — تُعرض تفاصيل السجل المحدد وفق الصلاحية.",
            TextAlign = ContentAlignment.MiddleRight, ForeColor = Color.FromArgb(35, 55, 80)
        };
        var list = new DataGridView
        {
            Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
            AutoGenerateColumns = false, BackgroundColor = Color.White, BorderStyle = BorderStyle.FixedSingle,
            RightToLeft = RightToLeft.Yes, SelectionMode = DataGridViewSelectionMode.FullRowSelect
        };
        list.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = tabName, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
        list.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "الحالة", Width = 140 });
        panel.Controls.Add(list);
        panel.Controls.Add(note);
        return panel;
    }

    private Control CreateListCard()
    {
        var group = Card("البحث والتصفية وقائمة البيانات");
        var outer = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, Padding = new Padding(12) };
        outer.RowStyles.Add(new RowStyle(SizeType.AutoSize)); outer.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        var filters = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.RightToLeft };
        filters.Controls.AddRange([new Label { Text = "بحث: ", AutoSize = true, Padding = new Padding(4, 7, 0, 0) }, _search, new Label { Text = "الحالة: ", AutoSize = true, Padding = new Padding(10, 7, 0, 0) }, _filter]);
        _search.Width = 230; _filter.Width = 130; _search.TextChanged += (_, _) => ApplyFilter(); _filter.SelectedIndexChanged += (_, _) => ApplyFilter();
        var grid = new DataGridView { Dock = DockStyle.Fill, DataSource = _source, AutoGenerateColumns = false, AllowUserToAddRows = false, ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false, BackgroundColor = Color.White, BorderStyle = BorderStyle.None, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, RightToLeft = RightToLeft.Yes };
        grid.Columns.AddRange(new DataGridViewTextBoxColumn { DataPropertyName = nameof(SecurityRecord.Code), HeaderText = "الكود" }, new DataGridViewTextBoxColumn { DataPropertyName = nameof(SecurityRecord.Name), HeaderText = "الاسم" }, new DataGridViewTextBoxColumn { DataPropertyName = nameof(SecurityRecord.Status), HeaderText = "الحالة" }, new DataGridViewTextBoxColumn { DataPropertyName = nameof(SecurityRecord.UpdatedAt), HeaderText = "آخر تعديل" });
        grid.SelectionChanged += (_, _) => { if (grid.CurrentRow?.DataBoundItem is SecurityRecord r) _audit.Text = "أُنشئ محلياً: " + r.CreatedAt.ToString("yyyy-MM-dd HH:mm") + "   |   آخر تعديل: " + r.UpdatedAt.ToString("yyyy-MM-dd HH:mm") + "   |   المستخدم: مدير النظام"; };
        outer.Controls.Add(filters, 0, 0); outer.Controls.Add(grid, 0, 1); group.Controls.Add(outer); return group;
    }

    private Control CreateAuditBar()
    {
        var panel = new Panel { Dock = DockStyle.Fill, Height = 35, BackColor = Color.FromArgb(235, 240, 247) };
        panel.Controls.Add(_audit);
        _audit.Text = "بيانات الإنشاء والتعديل — المستخدم: مدير النظام | الشركة/الفرع/السنة تؤخذ من سياق النظام الرئيسي. الإغلاق من تبويب النافذة فقط.";
        return panel;
    }
    private GroupBox Card(string text) => new() { Text = text, Dock = DockStyle.Fill, Padding = new Padding(10), BackColor = Color.White, ForeColor = Color.FromArgb(35, 55, 80), Margin = new Padding(0, 0, 0, 10) };
    private TextBox AddDetail(string field)
    {
        var box = CreateTextBox();
        // لا تُعرض مفاتيح التكامل أو كلمات المرور أو الرموز السرية كنص واضح في الواجهة.
        box.UseSystemPasswordChar = field.Contains("مفتاح") || field.Contains("سر") || field.Contains("كلمة المرور") || field.Contains("رمز");
        _details[field] = box;
        return box;
    }
    private static TextBox CreateTextBox() => new() { Dock = DockStyle.Fill, BackColor = Color.FromArgb(255, 253, 231), BorderStyle = BorderStyle.FixedSingle };
    private static Button Button(string text, EventHandler onClick, Color color) { var b = new Button { Text = text, AutoSize = true, FlatStyle = FlatStyle.Flat, BackColor = color, ForeColor = Color.White, Margin = new Padding(4), Padding = new Padding(12, 5, 12, 5) }; b.FlatAppearance.BorderSize = 0; b.Click += onClick; return b; }
    private static void AddField(TableLayoutPanel panel, int row, string label, Control control, bool required = false, int col = 0) { while (panel.RowCount <= row) panel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); panel.Controls.Add(new Label { Text = label, AutoSize = true, Padding = new Padding(3, 7, 3, 3) }, col, row); panel.Controls.Add(control, col + 1, row); }
    private void SeedLocalData() { _records.Add(new SecurityRecord { Code = _definition.Code + "-001", Name = "سجل محلي تجريبي", Status = "نشط", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }); }
    private void ClearEditor() { _code.Clear(); _name.Clear(); foreach (var item in _details.Values) item.Clear(); _status.SelectedIndex = 0; _code.Focus(); }
    private void SaveLocal() { if (string.IsNullOrWhiteSpace(_code.Text) || string.IsNullOrWhiteSpace(_name.Text)) { MessageBox.Show("أدخل الكود والاسم، فهما حقول إلزامية.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning); return; } var existing = _records.FirstOrDefault(x => x.Code.Equals(_code.Text.Trim(), StringComparison.OrdinalIgnoreCase)); if (existing is null) _records.Add(new SecurityRecord { Code = _code.Text.Trim(), Name = _name.Text.Trim(), Status = _status.Text, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }); else { existing.Name = _name.Text.Trim(); existing.Status = _status.Text; existing.UpdatedAt = DateTime.Now; _source.ResetBindings(false); } ClearEditor(); ApplyFilter(); }
    private SecurityRecord? Selected() => _source.Current as SecurityRecord;
    private void LoadSelected() { var r = Selected(); if (r is null) return; _code.Text = r.Code; _name.Text = r.Name; _status.SelectedItem = r.Status; }
    private void SetStatus(string status) { var r = Selected(); if (r is null) return; r.Status = status; r.UpdatedAt = DateTime.Now; _source.ResetBindings(false); }
    private void DeleteSelected() { var r = Selected(); if (r is null) return; if (MessageBox.Show("هل تريد حذف السجل المحدد؟", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) { _records.Remove(r); ApplyFilter(); } }
    private void PrintPreview() => MessageBox.Show("سيتم ربط الطباعة بتقرير رسمي عند توفر خدمة التقارير.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
    private void ApplyFilter() { var query = _search.Text.Trim(); var status = _filter.Text; var view = _records.Where(r => (status == "الكل" || r.Status == status) && (string.IsNullOrEmpty(query) || r.Code.Contains(query, StringComparison.OrdinalIgnoreCase) || r.Name.Contains(query, StringComparison.OrdinalIgnoreCase))).ToList(); _source.DataSource = new BindingList<SecurityRecord>(view); }
}
public sealed class SecurityRecord { public string Code { get; set; } = ""; public string Name { get; set; } = ""; public string Status { get; set; } = "نشط"; public DateTime CreatedAt { get; set; } = DateTime.Now; public DateTime UpdatedAt { get; set; } = DateTime.Now; }
public sealed record SecurityScreenDefinition(string FormName, string Code, string Title, string Field1, string Field2, string Field3);
