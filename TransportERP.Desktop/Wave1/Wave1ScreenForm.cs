using System.Collections;
using System.Drawing;
using TransportERP.Contracts.Wave1;

namespace TransportERP.Desktop.Wave1;

/// <summary>
/// TransportERP-native W3 implementation for the ten governing WAVE-1 screens.
/// Layout is generated from the governing visual catalog and deliberately omits
/// a screen-level Close command; closure remains owned by the host/tab container.
/// </summary>
public sealed class Wave1ScreenForm : Form
{
    private readonly Dictionary<string, Control> _fields = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, ToolStripStatusLabel> _summaries = new(StringComparer.OrdinalIgnoreCase);
    private readonly DataGridView _grid = new();

    public Wave1ScreenDefinition Definition { get; }
    public Wave1VisualDefinition VisualDefinition { get; }

    public event EventHandler<Wave1ActionEventArgs>? ActionInvoked;

    public Wave1ScreenForm(string screenId)
        : this(Wave1ScreenCatalog.GetRequired(screenId), Wave1VisualCatalog.GetRequired(screenId))
    {
    }

    public Wave1ScreenForm(Wave1ScreenDefinition definition, Wave1VisualDefinition visualDefinition)
    {
        Definition = definition ?? throw new ArgumentNullException(nameof(definition));
        VisualDefinition = visualDefinition ?? throw new ArgumentNullException(nameof(visualDefinition));
        if (!string.Equals(definition.ScreenId, visualDefinition.ScreenId, StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Screen and visual definitions must refer to the same screen.");
        InitializeScreen();
    }

    public IReadOnlyDictionary<string, object?> GetValues()
        => _fields.ToDictionary(x => x.Key, x => ReadValue(x.Value), StringComparer.OrdinalIgnoreCase);

    public void SetValue(string key, object? value)
    {
        if (!_fields.TryGetValue(key, out var control))
            throw new KeyNotFoundException($"Unknown field '{key}'.");
        WriteValue(control, value);
    }

    public void SetRows(IEnumerable<IReadOnlyDictionary<string, object?>> rows)
    {
        ArgumentNullException.ThrowIfNull(rows);
        _grid.Rows.Clear();
        foreach (var row in rows)
        {
            var index = _grid.Rows.Add();
            foreach (var column in VisualDefinition.Columns)
            {
                if (row.TryGetValue(column.Key, out var value))
                    _grid.Rows[index].Cells[column.Key].Value = value;
            }
        }
    }

    public void SetSummary(string key, object? value)
    {
        if (!_summaries.TryGetValue(key, out var label))
            throw new KeyNotFoundException($"Unknown summary '{key}'.");
        label.Text = $"{SummaryCaption(key)}: {value}";
    }

    private void InitializeScreen()
    {
        SuspendLayout();

        Text = $"{Definition.ArabicName} — {Definition.ScreenId}";
        Name = $"Frm{Definition.ScreenId.Replace("-", string.Empty, StringComparison.Ordinal)}";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(1080, 680);
        AutoScaleMode = AutoScaleMode.Font;

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 5,
            RightToLeft = RightToLeft.Yes,
            Padding = new Padding(12)
        };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        root.Controls.Add(BuildHeader(), 0, 0);
        root.Controls.Add(BuildToolbar(), 0, 1);
        root.Controls.Add(BuildInputPanel(), 0, 2);
        root.Controls.Add(BuildGrid(), 0, 3);
        root.Controls.Add(BuildStatus(), 0, 4);

        Controls.Add(root);
        ResumeLayout(performLayout: true);
    }

    private Control BuildHeader()
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 2,
            RightToLeft = RightToLeft.Yes,
            Padding = new Padding(0, 0, 0, 6)
        };
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

        panel.Controls.Add(new Label
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            Font = new Font(Font.FontFamily, 14, FontStyle.Bold),
            Text = Definition.ArabicName,
            TextAlign = ContentAlignment.MiddleRight,
            Padding = new Padding(4, 8, 4, 8)
        }, 0, 0);

        panel.Controls.Add(new Label
        {
            AutoSize = true,
            Text = $"{Definition.ScreenId}  •  {Definition.Profile}/{Definition.Variant}",
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(8, 12, 8, 8)
        }, 1, 0);
        return panel;
    }

    private Control BuildToolbar()
    {
        var toolbar = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = true,
            Padding = new Padding(4, 2, 4, 8)
        };

        foreach (var binding in Definition.Actions
                     .Where(x => !string.Equals(x.Action, "Close", StringComparison.OrdinalIgnoreCase))
                     .GroupBy(x => x.Action, StringComparer.OrdinalIgnoreCase)
                     .Select(x => x.First()))
        {
            var button = new Button
            {
                AutoSize = true,
                Text = ActionCaption(binding.Action),
                Tag = binding,
                MinimumSize = new Size(96, 34),
                UseVisualStyleBackColor = true
            };
            button.Click += (_, _) => ActionInvoked?.Invoke(this, new Wave1ActionEventArgs(binding, GetValues()));
            toolbar.Controls.Add(button);
        }
        return toolbar;
    }

    private Control BuildInputPanel()
    {
        var group = new GroupBox
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            Text = VisualDefinition.Mode == "ReportInquiry" ? "معايير التقرير" : "بيانات السجل",
            RightToLeft = RightToLeft.Yes,
            Padding = new Padding(10)
        };

        var table = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 4,
            RightToLeft = RightToLeft.Yes,
            Padding = new Padding(4)
        };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32));

        var row = 0;
        for (var i = 0; i < VisualDefinition.Fields.Count; i += 2)
        {
            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            AddField(table, VisualDefinition.Fields[i], row, 0);
            if (i + 1 < VisualDefinition.Fields.Count)
                AddField(table, VisualDefinition.Fields[i + 1], row, 2);
            row++;
        }

        group.Controls.Add(table);
        return group;
    }

    private void AddField(TableLayoutPanel table, Wave1VisualField field, int row, int labelColumn)
    {
        var label = new Label
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            Text = field.ArabicLabel + (field.Required ? " *" : string.Empty),
            TextAlign = ContentAlignment.MiddleRight,
            Padding = new Padding(4, 8, 4, 4)
        };
        var control = CreateFieldControl(field);
        control.Name = $"fld{field.Key}";
        control.Tag = field;
        control.Dock = DockStyle.Top;
        control.Enabled = !field.ReadOnly;
        _fields[field.Key] = control;
        table.Controls.Add(label, labelColumn, row);
        table.Controls.Add(control, labelColumn + 1, row);
    }

    private static Control CreateFieldControl(Wave1VisualField field)
    {
        switch (field.Kind)
        {
            case Wave1VisualFieldKind.Date:
                return new DateTimePicker { Format = DateTimePickerFormat.Short, Width = 180 };
            case Wave1VisualFieldKind.Boolean:
                return new CheckBox { AutoSize = true, Text = "نعم", Padding = new Padding(4, 6, 4, 4) };
            case Wave1VisualFieldKind.Lookup:
            {
                var combo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 260 };
                if (!string.IsNullOrWhiteSpace(field.LookupSource) && field.LookupSource.Contains('|'))
                    combo.Items.AddRange(field.LookupSource.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
                return combo;
            }
            case Wave1VisualFieldKind.Number:
                return new NumericUpDown { DecimalPlaces = 0, Maximum = decimal.MaxValue, Width = 180, ThousandsSeparator = true };
            case Wave1VisualFieldKind.Multiline:
                return new TextBox { Multiline = true, Height = 54, ScrollBars = ScrollBars.Vertical, Width = 320 };
            default:
                return new TextBox { Width = 280 };
        }
    }

    private Control BuildGrid()
    {
        _grid.Name = "dgvResults";
        _grid.Dock = DockStyle.Fill;
        _grid.RightToLeft = RightToLeft.Yes;
        _grid.AutoGenerateColumns = false;
        _grid.AllowUserToAddRows = false;
        _grid.AllowUserToDeleteRows = false;
        _grid.ReadOnly = VisualDefinition.Mode == "ReportInquiry";
        _grid.MultiSelect = false;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        _grid.RowHeadersVisible = false;
        _grid.BackgroundColor = SystemColors.Window;

        foreach (var column in VisualDefinition.Columns)
        {
            var gridColumn = new DataGridViewTextBoxColumn
            {
                Name = column.Key,
                DataPropertyName = column.Key,
                HeaderText = column.ArabicHeader,
                Width = column.Width,
                SortMode = DataGridViewColumnSortMode.Programmatic
            };
            if (!string.IsNullOrWhiteSpace(column.Format))
                gridColumn.DefaultCellStyle.Format = column.Format;
            _grid.Columns.Add(gridColumn);
        }

        return _grid;
    }

    private Control BuildStatus()
    {
        var status = new StatusStrip { SizingGrip = false, RightToLeft = RightToLeft.Yes };
        status.Items.Add(new ToolStripStatusLabel($"Profile: {Definition.Profile}"));
        status.Items.Add(new ToolStripStatusLabel($"Variant: {Definition.Variant}"));
        status.Items.Add(new ToolStripStatusLabel("RTL"));
        foreach (var key in VisualDefinition.SummaryKeys)
        {
            var label = new ToolStripStatusLabel($"{SummaryCaption(key)}: —");
            _summaries[key] = label;
            status.Items.Add(label);
        }
        return status;
    }

    private static object? ReadValue(Control control) => control switch
    {
        TextBox x => x.Text,
        ComboBox x => x.SelectedValue ?? x.SelectedItem ?? x.Text,
        DateTimePicker x => x.Value.Date,
        CheckBox x => x.Checked,
        NumericUpDown x => x.Value,
        _ => control.Text
    };

    private static void WriteValue(Control control, object? value)
    {
        switch (control)
        {
            case TextBox x:
                x.Text = value?.ToString() ?? string.Empty;
                break;
            case ComboBox x:
                x.SelectedValue = value;
                if (x.SelectedIndex < 0 && value is not null) x.Text = value.ToString();
                break;
            case DateTimePicker x when value is DateTime date:
                x.Value = date;
                break;
            case DateTimePicker x when value is DateTimeOffset dto:
                x.Value = dto.LocalDateTime;
                break;
            case CheckBox x:
                x.Checked = value is bool b && b;
                break;
            case NumericUpDown x:
                if (value is not null && decimal.TryParse(value.ToString(), out var number))
                    x.Value = Math.Min(x.Maximum, Math.Max(x.Minimum, number));
                break;
            default:
                control.Text = value?.ToString() ?? string.Empty;
                break;
        }
    }

    private static string ActionCaption(string action) => action switch
    {
        "View" => "عرض",
        "New" => "جديد",
        "Save" => "حفظ",
        "Edit" => "تعديل",
        "Activate/Disable" => "تفعيل/إيقاف",
        "Search" => "بحث",
        "Refresh" => "تحديث",
        "Reserve" => "حجز رقم",
        "Commit" => "اعتماد الرقم",
        "Cancel" => "إلغاء الحجز",
        "Override" => "إجراء محمي",
        "ApplyFilters" => "تطبيق",
        "DrillDown" => "تفاصيل",
        "Export" => "تصدير",
        "Print" => "طباعة",
        _ => action
    };

    private static string SummaryCaption(string key) => key switch
    {
        "GrandTotal" => "الإجمالي",
        "AssetsTotal" => "الأصول",
        "LiabilitiesTotal" => "الالتزامات",
        "EquityTotal" => "حقوق الملكية",
        "CurrentEarnings" => "نتيجة الفترة",
        "EquationDifference" => "فرق المعادلة",
        "OperatingNet" => "تشغيلي",
        "InvestingNet" => "استثماري",
        "FinancingNet" => "تمويلي",
        "UnclassifiedNet" => "غير مصنف",
        "NetCashMovement" => "صافي الحركة",
        "ActiveSequences" => "التسلسلات النشطة",
        "Reserved" => "محجوز",
        "Committed" => "معتمد",
        "Cancelled" => "ملغي",
        "Languages" => "اللغات",
        "Translations" => "الترجمات",
        _ => key
    };
}

public sealed class Wave1ActionEventArgs(
    Wave1ActionBinding binding,
    IReadOnlyDictionary<string, object?> values) : EventArgs
{
    public Wave1ActionBinding Binding { get; } = binding;
    public IReadOnlyDictionary<string, object?> Values { get; } = values;
}

public static class Wave1ScreenFormFactory
{
    public static Form Create(string screenId) => new Wave1ScreenForm(screenId);
}
