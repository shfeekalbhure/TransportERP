using System.ComponentModel;
using System.Drawing;

namespace TransportERP.Desktop.Forms.Accounting;

/// <summary>
/// القالب التنفيذي الموحد لشاشات المحاسبة. يعمل ببيانات محلية منظمة إلى حين اعتماد API.
/// </summary>
public abstract class AccountingScreenForm : Form
{
    private readonly string _screenCode;
    private readonly string _screenName;
    private readonly bool _usesEntryGrid;
    private readonly BindingList<AccountingRow> _records = new();
    private readonly DataGridView _grid = new();
    private readonly TextBox _search = new();
    private readonly TextBox _code = RequiredTextBox();
    private readonly TextBox _nameAr = RequiredTextBox();
    private readonly TextBox _nameEn = RequiredTextBox();
    private readonly ComboBox _company = RequiredCombo("الشركة الرئيسية");
    private readonly ComboBox _branch = RequiredCombo("الفرع الرئيسي");
    private readonly ComboBox _currency = RequiredCombo("ريال يمني (YER)");
    private readonly NumericUpDown _exchangeRate = new() { DecimalPlaces = 4, Minimum = 0.0001M, Maximum = 999999M, Value = 1M, Dock = DockStyle.Fill };
    private readonly ComboBox _status = RequiredCombo("نشط", "معلق", "معتمد", "موقوف", "ملغي");
    private readonly Label _recordCount = new() { AutoSize = true, Text = "عدد السجلات: 0", ForeColor = Color.FromArgb(52, 73, 94) };
    private readonly Label _audit = new() { AutoSize = true, Text = "الإنشاء والتعديل: لم يتم الحفظ بعد", ForeColor = Color.FromArgb(95, 105, 120) };
    private AccountingRow? _selected;

    protected AccountingScreenForm(string screenCode, string screenName, bool usesEntryGrid = false)
    {
        _screenCode = screenCode;
        _screenName = screenName;
        _usesEntryGrid = usesEntryGrid;

        Text = $"TransportERP — {screenName}";
        Name = $"Frm{screenCode.Replace("-", string.Empty, StringComparison.Ordinal)}";
        StartPosition = FormStartPosition.CenterScreen;
        WindowState = FormWindowState.Maximized;
        MinimumSize = new Size(1120, 720);
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        BackColor = Color.FromArgb(245, 247, 250);
        Font = new Font("Segoe UI", 10F);

        BuildLayout();
        SeedLocalData();
        BindRecords(_records);
    }

    private static TextBox RequiredTextBox() => new()
    {
        Dock = DockStyle.Fill,
        BackColor = Color.FromArgb(255, 252, 219),
        BorderStyle = BorderStyle.FixedSingle,
        RightToLeft = RightToLeft.Yes
    };

    private static ComboBox RequiredCombo(params string[] values)
    {
        var combo = new ComboBox
        {
            Dock = DockStyle.Fill,
            DropDownStyle = ComboBoxStyle.DropDownList,
            BackColor = Color.FromArgb(255, 252, 219),
            RightToLeft = RightToLeft.Yes
        };
        combo.Items.AddRange(values);
        combo.SelectedIndex = 0;
        return combo;
    }

    private void BuildLayout()
    {
        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(16),
            ColumnCount = 1,
            RowCount = 6,
            RightToLeft = RightToLeft.Yes
        };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 72));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 56));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 56));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 238));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));

        root.Controls.Add(BuildTitle(), 0, 0);
        root.Controls.Add(BuildToolbar(), 0, 1);
        root.Controls.Add(BuildSearch(), 0, 2);
        root.Controls.Add(BuildFieldsCard(), 0, 3);
        root.Controls.Add(BuildGridCard(), 0, 4);
        root.Controls.Add(BuildAuditBar(), 0, 5);
        Controls.Add(root);
    }

    private Control BuildTitle() => new Panel
    {
        Dock = DockStyle.Fill,
        BackColor = Color.White,
        Padding = new Padding(16),
        Controls =
        {
            new Label
            {
                Dock = DockStyle.Fill,
                Text = $"{_screenName}\n{_screenCode} — المحاسبة",
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 65, 100)
            }
        }
    };

    private Control BuildToolbar()
    {
        var panel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            Padding = new Padding(8),
            BackColor = Color.FromArgb(35, 65, 100)
        };
        AddButton(panel, "جديد", (_, _) => StartNew());
        AddButton(panel, "حفظ", (_, _) => SaveRecord());
        AddButton(panel, "تعديل", (_, _) => SetEditing(true));
        AddButton(panel, "إيقاف", (_, _) => StopRecord());
        AddButton(panel, "حذف", (_, _) => DeleteRecord());
        AddButton(panel, "طباعة", (_, _) => PrintRecord());
        return panel;
    }

    private static void AddButton(FlowLayoutPanel panel, string text, EventHandler action)
    {
        var button = new Button
        {
            Text = text,
            Width = 88,
            Height = 34,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.White,
            ForeColor = Color.FromArgb(35, 65, 100),
            Margin = new Padding(4, 2, 4, 2)
        };
        button.FlatAppearance.BorderColor = Color.FromArgb(220, 228, 238);
        button.Click += action;
        panel.Controls.Add(button);
    }

    private Control BuildSearch()
    {
        var panel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(12, 10, 12, 10) };
        _search.Dock = DockStyle.Right;
        _search.Width = 420;
        _search.PlaceholderText = "بحث بالكود أو الاسم أو الحالة...";
        _search.TextChanged += (_, _) => Filter(_search.Text);
        _recordCount.Dock = DockStyle.Left;
        _recordCount.TextAlign = ContentAlignment.MiddleLeft;
        panel.Controls.Add(_recordCount);
        panel.Controls.Add(_search);
        return panel;
    }

    private Control BuildFieldsCard()
    {
        var card = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(18, 12, 18, 12) };
        var heading = new Label
        {
            Dock = DockStyle.Top,
            Height = 30,
            Text = _usesEntryGrid ? "بيانات المستند ورؤوس القيود" : "البيانات الرئيسية",
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = Color.FromArgb(35, 65, 100),
            TextAlign = ContentAlignment.MiddleRight
        };
        var fields = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 4, RightToLeft = RightToLeft.Yes, Padding = new Padding(0, 5, 0, 0) };
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 122));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 122));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        for (var row = 0; row < 4; row++) fields.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
        AddField(fields, 0, 0, "الكود / الرقم *", _code);
        AddField(fields, 2, 0, "الاسم العربي *", _nameAr);
        AddField(fields, 0, 1, "الاسم الإنجليزي *", _nameEn);
        AddField(fields, 2, 1, "الحالة *", _status);
        AddField(fields, 0, 2, "الشركة *", _company);
        AddField(fields, 2, 2, "الفرع *", _branch);
        AddField(fields, 0, 3, "العملة *", _currency);
        AddField(fields, 2, 3, "سعر الصرف *", _exchangeRate);
        card.Controls.Add(fields);
        card.Controls.Add(heading);
        return card;
    }

    private static void AddField(TableLayoutPanel panel, int column, int row, string caption, Control field)
    {
        var label = new Label { Text = caption, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight, ForeColor = Color.FromArgb(52, 73, 94) };
        panel.Controls.Add(label, column, row);
        panel.Controls.Add(field, column + 1, row);
    }

    private Control BuildGridCard()
    {
        var card = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(12) };
        _grid.Dock = DockStyle.Fill;
        _grid.AutoGenerateColumns = false;
        _grid.AllowUserToAddRows = _usesEntryGrid;
        _grid.AllowUserToDeleteRows = _usesEntryGrid;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.MultiSelect = false;
        _grid.ReadOnly = false;
        _grid.RightToLeft = RightToLeft.Yes;
        _grid.BackgroundColor = Color.White;
        _grid.BorderStyle = BorderStyle.None;
        _grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        _grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(235, 241, 248);
        _grid.EnableHeadersVisualStyles = false;
        AddColumn("Code", "الكود / الرقم", 125);
        AddColumn("NameAr", "الاسم العربي", 190);
        AddColumn("NameEn", "الاسم الإنجليزي", 190);
        AddColumn("Currency", "العملة", 105);
        AddColumn("ExchangeRate", "سعر الصرف", 105);
        AddColumn("Status", "الحالة", 95);
        _grid.SelectionChanged += (_, _) => LoadSelected();
        card.Controls.Add(_grid);
        return card;
    }

    private void AddColumn(string property, string title, int width) =>
        _grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = property, HeaderText = title, Width = width, SortMode = DataGridViewColumnSortMode.Automatic });

    private Control BuildAuditBar()
    {
        var bar = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(235, 241, 248), Padding = new Padding(10, 7, 10, 4) };
        _audit.Dock = DockStyle.Right;
        bar.Controls.Add(_audit);
        return bar;
    }

    private void SeedLocalData()
    {
        _records.Add(new AccountingRow($"{_screenCode}-001", $"سجل تجريبي — {_screenName}", $"Sample — {_screenName}", "YER", 1M, "نشط"));
        _records.Add(new AccountingRow($"{_screenCode}-002", $"سجل مراجعة — {_screenName}", $"Review — {_screenName}", "USD", 535.25M, "معلق"));
    }

    private void BindRecords(IEnumerable<AccountingRow> source)
    {
        _grid.DataSource = new BindingList<AccountingRow>(source.ToList());
        _recordCount.Text = $"عدد السجلات: {_grid.Rows.Count}";
    }

    private void Filter(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText)) { BindRecords(_records); return; }
        var value = searchText.Trim();
        BindRecords(_records.Where(x => x.Code.Contains(value, StringComparison.OrdinalIgnoreCase) || x.NameAr.Contains(value, StringComparison.OrdinalIgnoreCase) || x.NameEn.Contains(value, StringComparison.OrdinalIgnoreCase) || x.Status.Contains(value, StringComparison.OrdinalIgnoreCase)));
    }

    private void StartNew()
    {
        _selected = null;
        _code.Clear(); _nameAr.Clear(); _nameEn.Clear();
        _status.SelectedIndex = 0; _currency.SelectedIndex = 0; _exchangeRate.Value = 1M;
        SetEditing(true);
        _code.Focus();
    }

    private bool Valid() => !string.IsNullOrWhiteSpace(_code.Text) && !string.IsNullOrWhiteSpace(_nameAr.Text) && !string.IsNullOrWhiteSpace(_nameEn.Text);

    private void SaveRecord()
    {
        if (!Valid())
        {
            MessageBox.Show("أكمل الحقول الإلزامية ذات الخلفية الصفراء.", _screenName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (_selected is null)
        {
            _selected = ReadForm();
            _records.Add(_selected);
        }
        else
        {
            _selected.Code = _code.Text.Trim(); _selected.NameAr = _nameAr.Text.Trim(); _selected.NameEn = _nameEn.Text.Trim();
            _selected.Currency = _currency.Text; _selected.ExchangeRate = _exchangeRate.Value; _selected.Status = _status.Text;
        }
        _audit.Text = $"الإنشاء/التعديل: المستخدم الحالي — {DateTime.Now:yyyy/MM/dd HH:mm} | الشركة: {_company.Text} | الفرع: {_branch.Text}";
        BindRecords(_records);
        SetEditing(false);
    }

    private AccountingRow ReadForm() => new(_code.Text.Trim(), _nameAr.Text.Trim(), _nameEn.Text.Trim(), _currency.Text, _exchangeRate.Value, _status.Text);

    private void LoadSelected()
    {
        if (_grid.CurrentRow?.DataBoundItem is not AccountingRow row) return;
        _selected = row;
        _code.Text = row.Code; _nameAr.Text = row.NameAr; _nameEn.Text = row.NameEn;
        _currency.SelectedItem = row.Currency; _exchangeRate.Value = Math.Clamp(row.ExchangeRate, _exchangeRate.Minimum, _exchangeRate.Maximum); _status.SelectedItem = row.Status;
        SetEditing(false);
    }

    private void StopRecord()
    {
        if (_selected is null) return;
        _selected.Status = "موقوف";
        _status.SelectedItem = "موقوف";
        _audit.Text = $"تم إيقاف السجل بواسطة المستخدم الحالي — {DateTime.Now:yyyy/MM/dd HH:mm}";
        BindRecords(_records);
    }

    private void DeleteRecord()
    {
        if (_selected is null) return;
        if (MessageBox.Show("هل تريد حذف السجل المحدد؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) != DialogResult.Yes) return;
        _records.Remove(_selected);
        StartNew();
        BindRecords(_records);
    }

    private void PrintRecord() => MessageBox.Show($"تم تجهيز {_screenName} للطباعة.\nعدد مرات الطباعة مسجل محليًا إلى حين ربط خدمة التدقيق.", "طباعة", MessageBoxButtons.OK, MessageBoxIcon.Information);

    private void SetEditing(bool enabled)
    {
        foreach (var control in new Control[] { _code, _nameAr, _nameEn, _company, _branch, _currency, _exchangeRate, _status }) control.Enabled = enabled;
    }

    private sealed class AccountingRow
    {
        public AccountingRow() : this("", "", "", "YER", 1M, "معلق") { }
        public AccountingRow(string code, string nameAr, string nameEn, string currency, decimal exchangeRate, string status) { Code = code; NameAr = nameAr; NameEn = nameEn; Currency = currency; ExchangeRate = exchangeRate; Status = status; }
        public string Code { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Currency { get; set; }
        public decimal ExchangeRate { get; set; }
        public string Status { get; set; }
    }
}
