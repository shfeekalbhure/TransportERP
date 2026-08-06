using System.ComponentModel;
using System.Drawing;
using TransportERP.Contracts.Accounting;
using TransportERP.Desktop.Services;

namespace TransportERP.Desktop.Forms.Accounting;

/// <summary>
/// ACC-021 دفتر اليومية: تقرير قراءة فقط، بفلاتر الكراسة ونتائج محلية منظمة إلى حين ربط خدمة التقارير.
/// </summary>
public abstract class AccountingJournalReportForm : Form
{
    private readonly BindingList<JournalRow> _allRows = new();
    private readonly IJournalReportApiClient _client = new JournalReportApiClient(new HttpClient());
    private readonly BindingSource _source = new();
    private readonly DateTimePicker _fromDate = new() { Format = DateTimePickerFormat.Short, Value = DateTime.Today.AddMonths(-1) };
    private readonly DateTimePicker _toDate = new() { Format = DateTimePickerFormat.Short, Value = DateTime.Today };
    private readonly ComboBox _account = RequiredCombo("كل الحسابات", "النقدية", "الإيرادات", "المصروفات");
    private readonly ComboBox _branch = RequiredCombo("الفرع الرئيسي");
    private readonly ComboBox _costCenter = RequiredCombo("كل مراكز التكلفة", "الإدارة", "التشغيل");
    private readonly ComboBox _status = RequiredCombo("الكل", "مرحّل", "معلق", "ملغي");
    private readonly ComboBox _currency = RequiredCombo("كل العملات", "YER", "USD");
    private readonly ComboBox _journalType = RequiredCombo("كل أنواع القيود", "قيد يومي", "تسوية", "عكس");
    private readonly TextBox _search = new() { Width = 230, PlaceholderText = "رقم القيد أو الحساب أو البيان..." };
    private readonly Label _resultCount = new() { AutoSize = true, Padding = new Padding(12, 7, 0, 0), ForeColor = Color.FromArgb(35, 65, 100) };
    private readonly Label _audit = new() { Dock = DockStyle.Right, AutoSize = true, ForeColor = Color.DimGray };

    protected AccountingJournalReportForm()
    {
        Text = "TransportERP — دفتر اليومية";
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(1120, 700);
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        Font = new Font("Segoe UI", 10F);
        BackColor = Color.FromArgb(245, 247, 250);
        _source.DataSource = _allRows;
        BuildLayout();
        ApplyFilters();
    }

    private static ComboBox RequiredCombo(params string[] values)
    {
        var value = new ComboBox
        {
            Width = 150,
            DropDownStyle = ComboBoxStyle.DropDownList,
            RightToLeft = RightToLeft.Yes,
            BackColor = Color.FromArgb(255, 252, 219)
        };

        value.Items.AddRange(values);
        value.SelectedIndex = 0;
        return value;
    }

    private void BuildLayout()
    {
        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 5,
            Padding = new Padding(16),
            BackColor = BackColor
        };

        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 62));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 128));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));

        root.Controls.Add(new Label
        {
            Text = "دفتر اليومية — ACC-021",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleRight,
            Font = new Font(Font, FontStyle.Bold),
            ForeColor = Color.FromArgb(35, 65, 100),
            BackColor = Color.White,
            Padding = new Padding(14)
        }, 0, 0);

        root.Controls.Add(BuildToolbar(), 0, 1);
        root.Controls.Add(BuildFilters(), 0, 2);
        root.Controls.Add(BuildGrid(), 0, 3);
        root.Controls.Add(BuildAudit(), 0, 4);
        Controls.Add(root);
    }

    private Control BuildToolbar()
    {
        var bar = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(7),
            BackColor = Color.FromArgb(35, 65, 100)
        };

        AddButton(bar, "تحديث", async (_, _) => await LoadFromApiAsync());
        AddButton(bar, "طباعة", (_, _) => ReportMessage("تم تجهيز دفتر اليومية للطباعة وفق الصلاحية."));
        AddButton(bar, "تصدير", (_, _) => ReportMessage("سيتم تنفيذ التصدير من خدمة التقارير بعد الربط."));
        return bar;
    }

    private Control BuildFilters()
    {
        var panel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = true,
            Padding = new Padding(10),
            BackColor = Color.White
        };

        AddFilter(panel, "من تاريخ", _fromDate);
        AddFilter(panel, "إلى تاريخ", _toDate);
        AddFilter(panel, "الحساب", _account);
        AddFilter(panel, "الفرع", _branch);
        AddFilter(panel, "مركز التكلفة", _costCenter);
        AddFilter(panel, "الحالة", _status);
        AddFilter(panel, "العملة", _currency);
        AddFilter(panel, "نوع القيد", _journalType);

        _search.TextChanged += (_, _) => ApplyFilters();
        _fromDate.ValueChanged += (_, _) => ApplyFilters();
        _toDate.ValueChanged += (_, _) => ApplyFilters();
        _account.SelectedIndexChanged += (_, _) => ApplyFilters();
        _branch.SelectedIndexChanged += (_, _) => ApplyFilters();
        _costCenter.SelectedIndexChanged += (_, _) => ApplyFilters();
        _status.SelectedIndexChanged += (_, _) => ApplyFilters();
        _journalType.SelectedIndexChanged += (_, _) => ApplyFilters();
        _currency.SelectedIndexChanged += (_, _) => ApplyFilters();

        panel.Controls.Add(_search);
        panel.Controls.Add(_resultCount);
        return panel;
    }

    private static void AddFilter(FlowLayoutPanel panel, string caption, Control control)
    {
        panel.Controls.Add(new Label
        {
            Text = caption + ":",
            AutoSize = true,
            Padding = new Padding(5, 7, 2, 0)
        });
        panel.Controls.Add(control);
    }

    private Control BuildGrid()
    {
        var grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            DataSource = _source,
            AutoGenerateColumns = false,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            RightToLeft = RightToLeft.Yes,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };

        grid.Columns.AddRange(
            Column(nameof(JournalRow.Date), "التاريخ"),
            Column(nameof(JournalRow.Number), "رقم القيد"),
            Column(nameof(JournalRow.Account), "الحساب"),
            Column(nameof(JournalRow.Description), "البيان"),
            Column(nameof(JournalRow.Debit), "مدين"),
            Column(nameof(JournalRow.Credit), "دائن"),
            Column(nameof(JournalRow.Status), "الحالة"));

        return grid;
    }

    private static DataGridViewTextBoxColumn Column(string name, string title) => new()
    {
        DataPropertyName = name,
        HeaderText = title,
        SortMode = DataGridViewColumnSortMode.Automatic
    };

    private Control BuildAudit()
    {
        var bar = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(235, 241, 248),
            Padding = new Padding(10, 7, 10, 2)
        };

        _audit.Text = "تقرير قراءة فقط | آخر تحديث محلي: — | الطباعة والتصدير تخضعان للصلاحيات.";
        bar.Controls.Add(_audit);
        return bar;
    }

    private static void AddButton(FlowLayoutPanel bar, string text, EventHandler click)
    {
        var button = new Button
        {
            Text = text,
            Width = 88,
            Height = 32,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.White,
            ForeColor = Color.FromArgb(35, 65, 100),
            Margin = new Padding(4, 2, 4, 2)
        };

        button.FlatAppearance.BorderSize = 0;
        button.Click += click;
        bar.Controls.Add(button);
    }

    private async Task LoadFromApiAsync()
    {
        var response = await _client.QueryAsync(new JournalReportQuery(
            DateOnly.FromDateTime(_fromDate.Value),
            DateOnly.FromDateTime(_toDate.Value),
            null,
            null,
            null,
            _status.Text,
            _currency.Text,
            _journalType.Text,
            _search.Text));

        _allRows.Clear();
        _audit.Text = response.StorageAvailable
            ? "تم استلام التقرير من الخدمة."
            : $"مانع التخزين: {response.BlockerCode} — {response.BlockerMessage}";
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        var text = _search.Text.Trim();
        var rows = _allRows.Where(x =>
            x.Date.Date >= _fromDate.Value.Date
            && x.Date.Date <= _toDate.Value.Date
            && (_account.Text == "كل الحسابات" || x.Account == _account.Text)
            && (_branch.Text == "كل الفروع" || x.Branch == _branch.Text)
            && (_costCenter.Text == "كل مراكز التكلفة" || x.CostCenter == _costCenter.Text)
            && (_status.Text == "الكل" || x.Status == _status.Text)
            && (_currency.Text == "كل العملات" || x.Currency == _currency.Text)
            && (_journalType.Text == "كل أنواع القيود" || x.JournalType == _journalType.Text)
            && (string.IsNullOrWhiteSpace(text)
                || x.Number.Contains(text, StringComparison.OrdinalIgnoreCase)
                || x.Account.Contains(text, StringComparison.OrdinalIgnoreCase)
                || x.Description.Contains(text, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        _source.DataSource = new BindingList<JournalRow>(rows);
        _resultCount.Text = $"عدد النتائج: {rows.Count}";
        _audit.Text = $"تقرير قراءة فقط | آخر تحديث محلي: {DateTime.Now:yyyy/MM/dd HH:mm} | الطباعة والتصدير تخضعان للصلاحيات.";
    }

    private void ReportMessage(string text) => MessageBox.Show(
        text,
        "دفتر اليومية",
        MessageBoxButtons.OK,
        MessageBoxIcon.Information);

    private sealed record JournalRow(
        DateTime Date,
        string Number,
        string Account,
        string Description,
        decimal Debit,
        decimal Credit,
        string Status,
        string Currency,
        string JournalType,
        string Branch,
        string CostCenter);
}
