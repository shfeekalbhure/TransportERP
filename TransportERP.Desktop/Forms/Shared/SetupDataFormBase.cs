using System.ComponentModel;
using System.Drawing;

namespace TransportERP.Desktop.Forms.Shared;

/// <summary>قالب تشغيلي موحد لشاشات التهيئة: أدوات، حقول، بحث، جدول، ترقيم وتدقيق محلي.</summary>
public abstract class SetupDataFormBase : Form
{
    private readonly BindingList<SetupRecord> _records = new();
    private readonly DataGridView _grid = new();
    private readonly FlowLayoutPanel _fields = new();
    private readonly TextBox _searchBox = new();
    private readonly Label _counter = new();
    private readonly Label _audit = new();
    private readonly Dictionary<string, TextBox> _inputs = new();
    private int _nextId = 1;
    private int _currentIndex = -1;

    protected SetupDataFormBase(string code, string title, params string[] fields)
    {
        ScreenCode = code;
        ScreenTitle = title;
        FieldNames = fields;
        Text = $"{code} — {title}";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        Font = new Font("Segoe UI", 10F);
        BackColor = Color.FromArgb(247, 249, 252);
        Dock = DockStyle.Fill;
        TopLevel = false;
        FormBorderStyle = FormBorderStyle.None;
        BuildLayout();
        SeedRecords();
        RefreshGrid();
        NewRecord();
    }

    protected string ScreenCode { get; }
    protected string ScreenTitle { get; }
    protected IReadOnlyList<string> FieldNames { get; }

    private void BuildLayout()
    {
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 5, Padding = new Padding(16), BackColor = BackColor };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 38));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 62));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var toolbar = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = false, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(8), BackColor = Color.FromArgb(28, 80, 130) };
        AddButton(toolbar, "جديد", (_, _) => NewRecord(), Color.FromArgb(47, 128, 237));
        AddButton(toolbar, "حفظ", (_, _) => SaveRecord(), Color.FromArgb(39, 174, 96));
        AddButton(toolbar, "تعديل", (_, _) => EditRecord(), Color.FromArgb(242, 153, 74));
        AddButton(toolbar, "إيقاف", (_, _) => ToggleStatus(), Color.FromArgb(130, 130, 130));
        AddButton(toolbar, "حذف", (_, _) => DeleteRecord(), Color.FromArgb(235, 87, 87));
        AddButton(toolbar, "طباعة", (_, _) => PrintPreview(), Color.FromArgb(155, 81, 224));
        root.Controls.Add(toolbar, 0, 0);

        var search = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(0, 10, 0, 8) };
        search.Controls.Add(new Label { Text = "البحث والتصفية:", AutoSize = true, Padding = new Padding(4, 8, 5, 0), Font = new Font(Font, FontStyle.Bold) });
        _searchBox.Width = 280; _searchBox.PlaceholderText = "ابحث بالرمز أو الاسم أو الحالة"; _searchBox.TextChanged += (_, _) => RefreshGrid();
        search.Controls.Add(_searchBox);
        var clear = new Button { Text = "مسح التصفية", AutoSize = true }; clear.Click += (_, _) => _searchBox.Clear(); search.Controls.Add(clear);
        _counter.AutoSize = true; _counter.Padding = new Padding(18, 8, 0, 0); search.Controls.Add(_counter);
        root.Controls.Add(search, 0, 1);

        var formBox = new GroupBox { Text = $"بيانات {ScreenTitle}", Dock = DockStyle.Fill, Padding = new Padding(16), RightToLeft = RightToLeft.Yes };
        _fields.Dock = DockStyle.Fill; _fields.AutoScroll = true; _fields.FlowDirection = FlowDirection.RightToLeft; _fields.WrapContents = true;
        foreach (var field in FieldNames)
        {
            var holder = new Panel { Width = 300, Height = 66, Margin = new Padding(10, 4, 10, 4) };
            var label = new Label { Text = field + " *", Dock = DockStyle.Top, Height = 22, TextAlign = ContentAlignment.MiddleRight };
            var input = new TextBox { Dock = DockStyle.Bottom, Height = 30, BackColor = Color.FromArgb(255, 252, 220), Name = "txt" + field.Replace(" ", string.Empty) };
            holder.Controls.Add(input); holder.Controls.Add(label); _fields.Controls.Add(holder); _inputs[field] = input;
        }
        formBox.Controls.Add(_fields); root.Controls.Add(formBox, 0, 2);

        _grid.Dock = DockStyle.Fill; _grid.ReadOnly = true; _grid.AllowUserToAddRows = false; _grid.AllowUserToDeleteRows = false; _grid.AutoGenerateColumns = false;
        _grid.BackgroundColor = Color.White; _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect; _grid.MultiSelect = false; _grid.RightToLeft = RightToLeft.Yes;
        _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(SetupRecord.Code), HeaderText = "الرمز", Width = 110 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(SetupRecord.Name), HeaderText = "الاسم", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(SetupRecord.Status), HeaderText = "الحالة", Width = 110 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(SetupRecord.ModifiedAt), HeaderText = "آخر تعديل", Width = 155 });
        _grid.CellClick += (_, _) => LoadSelected();
        root.Controls.Add(_grid, 0, 3);

        var footer = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.RightToLeft, BackColor = Color.White, Padding = new Padding(10, 6, 10, 6) };
        _audit.AutoSize = true; footer.Controls.Add(_audit);
        footer.Controls.Add(new Label { Text = " | الشركة: شركة النقل الرئيسية | الفرع: الرئيسي | السنة المالية: 2026 | المستخدم: مدير النظام", AutoSize = true });
        root.Controls.Add(footer, 0, 4);
        Controls.Add(root);
    }

    private static void AddButton(FlowLayoutPanel host, string text, EventHandler click, Color color)
    {
        var button = new Button { Text = text, AutoSize = true, Height = 36, Margin = new Padding(4), BackColor = color, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
        button.FlatAppearance.BorderSize = 0; button.Click += click; host.Controls.Add(button);
    }

    private void SeedRecords()
    {
        _records.Add(new SetupRecord { Code = $"{ScreenCode}-001", Name = $"{ScreenTitle} الرئيسي", Status = "نشط", ModifiedAt = DateTime.Now.AddDays(-3) });
        _records.Add(new SetupRecord { Code = $"{ScreenCode}-002", Name = $"{ScreenTitle} تجريبي", Status = "نشط", ModifiedAt = DateTime.Now.AddDays(-1) });
        _nextId = 3;
    }

    private void RefreshGrid()
    {
        var query = _searchBox.Text.Trim();
        var shown = string.IsNullOrWhiteSpace(query) ? _records : _records.Where(x => x.Code.Contains(query, StringComparison.OrdinalIgnoreCase) || x.Name.Contains(query, StringComparison.OrdinalIgnoreCase) || x.Status.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
        _grid.DataSource = new BindingList<SetupRecord>(shown.ToList());
        _counter.Text = $"عدد السجلات: {shown.Count}";
        _audit.Text = _currentIndex >= 0 ? $"السجل الحالي: {_currentIndex + 1} | مرات التعديل: {_records[_currentIndex].EditCount} | آخر تعديل: {_records[_currentIndex].ModifiedAt:yyyy/MM/dd HH:mm}" : "سجل جديد";
    }

    private void NewRecord()
    {
        _currentIndex = -1; foreach (var input in _inputs.Values) input.Clear();
        if (FieldNames.Count > 0) _inputs[FieldNames[0]].Text = $"{ScreenCode}-{_nextId:000}";
        _audit.Text = "سجل جديد — املأ الحقول ثم اضغط حفظ";
    }

    private bool ValidateInputs()
    {
        foreach (var field in FieldNames)
            if (string.IsNullOrWhiteSpace(_inputs[field].Text)) { _inputs[field].Focus(); MessageBox.Show($"حقل «{field}» إلزامي.", ScreenTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
        return true;
    }

    private void SaveRecord()
    {
        if (!ValidateInputs()) return;
        if (_currentIndex >= 0) { EditRecord(); return; }
        var record = new SetupRecord { Code = _inputs[FieldNames[0]].Text.Trim(), Name = _inputs[FieldNames.Count > 1 ? FieldNames[1] : FieldNames[0]].Text.Trim(), Status = "نشط", ModifiedAt = DateTime.Now };
        _records.Add(record); _nextId++; _currentIndex = _records.Count - 1; RefreshGrid();
    }

    private void EditRecord()
    {
        if (_currentIndex < 0) { MessageBox.Show("اختر سجلًا من الجدول أولًا.", ScreenTitle, MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
        if (!ValidateInputs()) return;
        var record = _records[_currentIndex]; record.Code = _inputs[FieldNames[0]].Text.Trim(); record.Name = _inputs[FieldNames.Count > 1 ? FieldNames[1] : FieldNames[0]].Text.Trim(); record.ModifiedAt = DateTime.Now; record.EditCount++; RefreshGrid();
    }

    private void ToggleStatus()
    {
        if (_currentIndex < 0) return;
        var record = _records[_currentIndex]; record.Status = record.Status == "نشط" ? "موقوف" : "نشط"; record.ModifiedAt = DateTime.Now; record.EditCount++; RefreshGrid();
    }

    private void DeleteRecord()
    {
        if (_currentIndex < 0) return;
        if (MessageBox.Show("هل تريد حذف السجل المحدد؟", ScreenTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
        _records.RemoveAt(_currentIndex); NewRecord(); RefreshGrid();
    }

    private void PrintPreview() => MessageBox.Show($"تم تجهيز بيانات {ScreenTitle} للطباعة.\nعدد السجلات الظاهرة: {_grid.Rows.Count}", "طباعة", MessageBoxButtons.OK, MessageBoxIcon.Information);

    private void LoadSelected()
    {
        if (_grid.CurrentRow?.DataBoundItem is not SetupRecord selected) return;
        _currentIndex = _records.IndexOf(selected);
        if (_currentIndex < 0) return;
        _inputs[FieldNames[0]].Text = selected.Code;
        _inputs[FieldNames.Count > 1 ? FieldNames[1] : FieldNames[0]].Text = selected.Name;
        foreach (var field in FieldNames.Skip(2)) if (string.IsNullOrWhiteSpace(_inputs[field].Text)) _inputs[field].Text = "—";
        RefreshGrid();
    }

    private sealed class SetupRecord { public string Code { get; set; } = ""; public string Name { get; set; } = ""; public string Status { get; set; } = ""; public DateTime ModifiedAt { get; set; } public int EditCount { get; set; } }
}
