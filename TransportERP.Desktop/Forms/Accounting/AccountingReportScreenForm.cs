using System.ComponentModel;
using System.Drawing;

namespace TransportERP.Desktop.Forms.Accounting;

/// <summary>قالب القراءة للتقارير والاستعلامات المحاسبية؛ لا يسمح بتحرير القيود من شاشة التقرير.</summary>
public abstract class AccountingReportScreenForm : Form
{
    private readonly string _code;
    private readonly string _title;
    private readonly string[] _filterNames;
    private readonly BindingList<ReportRow> _rows = new();
    private readonly BindingSource _source = new();
    private readonly TextBox _search = new() { Width = 260, PlaceholderText = "بحث داخل النتائج..." };
    private readonly Label _audit = new() { AutoSize = true, ForeColor = Color.DimGray };
    private readonly Label _count = new() { AutoSize = true, ForeColor = Color.FromArgb(35, 65, 100) };

    protected AccountingReportScreenForm(string code, string title, params string[] filterNames)
    {
        _code = code;
        _title = title;
        _filterNames = filterNames;
        Text = $"TransportERP — {title}";
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(1040, 670);
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        BackColor = Color.FromArgb(245, 247, 250);
        Font = new Font("Segoe UI", 10F);
        _source.DataSource = _rows;
        Build();
        Seed();
        ApplyFilter();
    }

    private void Build()
    {
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 5, Padding = new Padding(16), BackColor = BackColor };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 64));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 106));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));

        root.Controls.Add(new Label { Text = $"{_title} — {_code}", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight, Font = new Font(Font, FontStyle.Bold), ForeColor = Color.FromArgb(35, 65, 100), BackColor = Color.White, Padding = new Padding(14) }, 0, 0);
        root.Controls.Add(Toolbar(), 0, 1);
        root.Controls.Add(Filters(), 0, 2);
        root.Controls.Add(Grid(), 0, 3);
        root.Controls.Add(Audit(), 0, 4);
        Controls.Add(root);
    }

    private Control Toolbar()
    {
        var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(7), BackColor = Color.FromArgb(35, 65, 100) };
        AddButton(panel, "تحديث", (_, _) => ApplyFilter());
        AddButton(panel, "طباعة", (_, _) => MessageBox.Show("تم تجهيز التقرير للطباعة.", _title, MessageBoxButtons.OK, MessageBoxIcon.Information));
        AddButton(panel, "تصدير", (_, _) => MessageBox.Show("سيتم ربط التصدير الرسمي عند توفر خدمة التقارير.", _title, MessageBoxButtons.OK, MessageBoxIcon.Information));
        return panel;
    }

    private Control Filters()
    {
        var card = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft, WrapContents = true, Padding = new Padding(10), BackColor = Color.White };
        card.Controls.Add(new Label { Text = "التصفية:", AutoSize = true, Padding = new Padding(4, 7, 4, 0), Font = new Font(Font, FontStyle.Bold) });
        foreach (var name in _filterNames)
        {
            var box = new TextBox { Width = 160, PlaceholderText = name, BackColor = Color.FromArgb(255, 252, 219), RightToLeft = RightToLeft.Yes };
            box.TextChanged += (_, _) => ApplyFilter();
            card.Controls.Add(box);
        }

        _search.TextChanged += (_, _) => ApplyFilter();
        card.Controls.Add(_search);
        card.Controls.Add(_count);
        return card;
    }

    private Control Grid()
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
            new DataGridViewTextBoxColumn { DataPropertyName = nameof(ReportRow.Reference), HeaderText = "المرجع" },
            new DataGridViewTextBoxColumn { DataPropertyName = nameof(ReportRow.Description), HeaderText = "البيان" },
            new DataGridViewTextBoxColumn { DataPropertyName = nameof(ReportRow.Debit), HeaderText = "مدين" },
            new DataGridViewTextBoxColumn { DataPropertyName = nameof(ReportRow.Credit), HeaderText = "دائن" },
            new DataGridViewTextBoxColumn { DataPropertyName = nameof(ReportRow.Balance), HeaderText = "الرصيد" });

        return grid;
    }

    private Control Audit()
    {
        var bar = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(235, 241, 248), Padding = new Padding(10, 6, 10, 2) };
        _audit.Text = "آخر تحديث محلي: — | صلاحيات العرض والطباعة والتصدير تُتحقق من الخادم عند الربط.";
        bar.Controls.Add(_audit);
        return bar;
    }

    private static void AddButton(FlowLayoutPanel panel, string text, EventHandler action)
    {
        var button = new Button { Text = text, Width = 90, Height = 32, FlatStyle = FlatStyle.Flat, BackColor = Color.White, ForeColor = Color.FromArgb(35, 65, 100), Margin = new Padding(4, 2, 4, 2) };
        button.FlatAppearance.BorderSize = 0;
        button.Click += action;
        panel.Controls.Add(button);
    }

    private void Seed()
    {
        _rows.Add(new ReportRow("ACC-LOCAL-001", "رصيد افتتاحي", 250000M, 0M, 250000M));
        _rows.Add(new ReportRow("ACC-LOCAL-002", "حركة مراجعة", 0M, 45000M, 205000M));
    }

    private void ApplyFilter()
    {
        var value = _search.Text.Trim();

        // توحيد نوع طرفي التعبير الشرطي إلى List<ReportRow> لمنع خطأ استنتاج النوع.
        List<ReportRow> view = string.IsNullOrEmpty(value)
            ? _rows.ToList()
            : _rows
                .Where(x =>
                    x.Reference.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                    x.Description.Contains(value, StringComparison.OrdinalIgnoreCase))
                .ToList();

        _source.DataSource = new BindingList<ReportRow>(view);
        _count.Text = $"عدد النتائج: {view.Count}";
        _audit.Text = $"آخر تحديث محلي: {DateTime.Now:yyyy/MM/dd HH:mm} | صلاحيات العرض والطباعة والتصدير تُتحقق من الخادم عند الربط.";
    }

    private sealed record ReportRow(string Reference, string Description, decimal Debit, decimal Credit, decimal Balance);
}
