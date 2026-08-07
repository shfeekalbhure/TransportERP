using System.ComponentModel;
using TransportERP.Desktop.Controls;
using TransportERP.Desktop.CoreUI.Controls;

namespace TransportERP.Desktop.Forms.Setup.General;

/// <summary>
/// منشئ مركزي لمحتوى شاشات المجموعة الثانية. يعتمد حصراً على CoreUI للعناصر المشتركة،
/// ويترك لكل شاشة تعريف تبويباتها وحقولها المتخصصة فقط.
/// </summary>
internal static class GeneralSetupScreenBuilder
{
    internal enum FieldKind
    {
        Text,
        RequiredText,
        Combo,
        Date,
        Number,
        Check,
        Multiline,
        Picture
    }

    internal sealed record FieldSpec(string Caption, FieldKind Kind = FieldKind.Text, string[]? Items = null);
    internal sealed record TabSpec(string Title, FieldSpec[] Fields, bool IsLog = false, string[]? ActionButtons = null);

    internal static TransportReferenceScreenShell Build(
        string screenCode,
        string screenTitle,
        IReadOnlyList<TabSpec> tabs,
        string searchPlaceholder,
        params string[] gridHeaders)
    {
        var shell = new TransportReferenceScreenShell
        {
            Dock = DockStyle.Fill,
            RightToLeft = RightToLeft.Yes,
            DataGroupTitle = screenTitle
        };

        shell.AlertBar.Text = $"{screenCode} — {screenTitle}";
        shell.SearchPanel.SearchPlaceholder = searchPlaceholder;
        shell.SearchPanel.SetStatusItems("نشط", "موقوف", "مسودة", "مغلق");

        var tabControl = new TabControl
        {
            Dock = DockStyle.Fill,
            RightToLeft = RightToLeft.Yes,
            RightToLeftLayout = true,
            Multiline = false,
            HotTrack = true
        };

        foreach (var tab in tabs)
        {
            tabControl.TabPages.Add(CreateTab(tab));
        }

        shell.DataHost.Controls.Add(tabControl);
        shell.ConfigureWorkspaceMode(showSearch: true, showGrid: true, expandDataWorkspace: false);
        ConfigureMainGrid(shell.Grid, gridHeaders);
        shell.Grid.BindData(new BindingList<PreviewRow>());

        // الأحداث الأساسية هنا لا تنفذ منطق أعمال ولا تتصل بقاعدة البيانات؛
        // دورها إبقاء الواجهة جاهزة للربط بطبقة HTTP/API لاحقاً.
        shell.Toolbar.NewRequested += (_, _) => ClearEditableFields(tabControl);
        shell.Toolbar.SaveRequested += (_, _) => shell.AlertBar.Text = $"{screenCode}: الواجهة جاهزة لحفظ البيانات عبر خدمة API.";
        shell.Toolbar.EditRequested += (_, _) => shell.AlertBar.Text = $"{screenCode}: الواجهة في وضع التعديل.";
        shell.Toolbar.DisableRequested += (_, _) => shell.AlertBar.Text = $"{screenCode}: طلب الإيقاف يحتاج خدمة وصلاحية من API.";
        shell.Toolbar.DeleteRequested += (_, _) => shell.AlertBar.Text = $"{screenCode}: الحذف لا ينفذ محلياً؛ يمر عبر API والصلاحيات.";
        shell.Toolbar.PrintRequested += (_, _) => shell.AlertBar.Text = $"{screenCode}: تم تجهيز أمر الطباعة للربط بخدمة التقارير.";
        shell.Toolbar.CloseRequested += (_, _) => shell.AlertBar.Text = "أغلق الشاشة من تبويب مساحة العمل لمنع إغلاق الإطار الرئيسي.";
        shell.SearchPanel.SearchTextChanged += (_, _) => shell.AlertBar.Text = $"{screenCode}: تصفية السجلات حسب نص البحث.";
        shell.SearchPanel.StatusChanged += (_, _) => shell.AlertBar.Text = $"{screenCode}: تصفية السجلات حسب الحالة.";

        return shell;
    }

    internal static FieldSpec Required(string caption) => new(caption, FieldKind.RequiredText);
    internal static FieldSpec Text(string caption) => new(caption, FieldKind.Text);
    internal static FieldSpec Combo(string caption, params string[] items) => new(caption, FieldKind.Combo, items);
    internal static FieldSpec Date(string caption) => new(caption, FieldKind.Date);
    internal static FieldSpec Number(string caption) => new(caption, FieldKind.Number);
    internal static FieldSpec Check(string caption) => new(caption, FieldKind.Check);
    internal static FieldSpec Multiline(string caption) => new(caption, FieldKind.Multiline);
    internal static FieldSpec Picture(string caption) => new(caption, FieldKind.Picture);

    private static TabPage CreateTab(TabSpec spec)
    {
        var page = new TabPage
        {
            Text = spec.Title,
            RightToLeft = RightToLeft.Yes,
            BackColor = Color.White,
            Padding = new Padding(8)
        };

        if (spec.IsLog)
        {
            page.Controls.Add(CreateLogGrid());
            return page;
        }

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RightToLeft = RightToLeft.Yes,
            ColumnCount = 4,
            AutoScroll = true,
            Padding = Padding.Empty,
            Margin = Padding.Empty
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 155F));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 155F));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

        var rowCount = Math.Max(1, (int)Math.Ceiling(spec.Fields.Length / 2d));
        root.RowCount = rowCount + (spec.ActionButtons is { Length: > 0 } ? 1 : 0);
        for (var row = 0; row < rowCount; row++)
        {
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, TransportUiMetrics.MainDataRowHeight));
        }

        for (var index = 0; index < spec.Fields.Length; index++)
        {
            var field = spec.Fields[index];
            var pair = index % 2;
            var row = index / 2;
            var labelColumn = pair == 0 ? 0 : 2;
            var controlColumn = pair == 0 ? 1 : 3;

            var label = new Label
            {
                Text = field.Caption + (field.Kind == FieldKind.RequiredText ? " *" : string.Empty),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight,
                RightToLeft = RightToLeft.Yes
            };
            root.Controls.Add(label, labelColumn, row);
            root.Controls.Add(CreateEditor(field), controlColumn, row);
        }

        if (spec.ActionButtons is { Length: > 0 } buttons)
        {
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, TransportUiMetrics.ToolbarHeight));
            var actions = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                RightToLeft = RightToLeft.Yes,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                AutoSize = true
            };
            foreach (var caption in buttons)
            {
                actions.Controls.Add(new PrimaryButton { Text = caption, AutoSize = true });
            }
            root.Controls.Add(actions, 0, rowCount);
            root.SetColumnSpan(actions, 4);
        }

        page.Controls.Add(root);
        return page;
    }

    private static Control CreateEditor(FieldSpec field)
    {
        Control control = field.Kind switch
        {
            FieldKind.RequiredText => new RequiredTextBox(),
            FieldKind.Combo => CreateCombo(field.Items),
            FieldKind.Date => new DateTimePicker { Format = DateTimePickerFormat.Short, RightToLeftLayout = true },
            FieldKind.Number => new NumericUpDown { DecimalPlaces = 2, Maximum = 999999999M, ThousandsSeparator = true, TextAlign = HorizontalAlignment.Right },
            FieldKind.Check => new CheckBox { Text = "نعم", AutoSize = true, TextAlign = ContentAlignment.MiddleRight },
            FieldKind.Multiline => new TextBox { Multiline = true, ScrollBars = ScrollBars.Vertical, MinimumSize = new Size(0, TransportUiMetrics.MainDataMultilineMinHeight) },
            FieldKind.Picture => new PictureBox { BorderStyle = BorderStyle.FixedSingle, SizeMode = PictureBoxSizeMode.Zoom, MinimumSize = new Size(160, 72) },
            _ => new TextBox { TextAlign = HorizontalAlignment.Right }
        };

        control.Dock = field.Kind == FieldKind.Check ? DockStyle.Right : DockStyle.Fill;
        control.RightToLeft = RightToLeft.Yes;
        control.Name = "fld" + NormalizeName(field.Caption);
        return control;
    }

    private static ComboBox CreateCombo(string[]? items)
    {
        var combo = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            RightToLeft = RightToLeft.Yes
        };
        if (items is { Length: > 0 }) combo.Items.AddRange(items.Cast<object>().ToArray());
        return combo;
    }

    private static TransportDataGrid CreateLogGrid()
    {
        var grid = new TransportDataGrid { Dock = DockStyle.Fill, AutoGenerateColumns = false };
        AddColumn(grid, "التاريخ", 145);
        AddColumn(grid, "المستخدم", 150);
        AddColumn(grid, "العملية", 130);
        AddColumn(grid, "القيمة السابقة", null);
        AddColumn(grid, "القيمة الجديدة", null);
        AddColumn(grid, "السبب/الملاحظة", null);
        return grid;
    }

    private static void ConfigureMainGrid(TransportDataGrid grid, IReadOnlyList<string> headers)
    {
        grid.AutoGenerateColumns = false;
        grid.Columns.Clear();
        foreach (var header in headers)
        {
            AddColumn(grid, header, header is "الرمز" or "الحالة" ? 120 : null);
        }
    }

    private static void AddColumn(DataGridView grid, string header, int? width)
    {
        var column = new DataGridViewTextBoxColumn
        {
            HeaderText = header,
            Name = "col" + NormalizeName(header),
            ReadOnly = true,
            SortMode = DataGridViewColumnSortMode.Automatic
        };
        if (width.HasValue)
        {
            column.Width = width.Value;
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        }
        else
        {
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        grid.Columns.Add(column);
    }

    private static void ClearEditableFields(Control root)
    {
        foreach (Control child in root.Controls)
        {
            switch (child)
            {
                case TextBox textBox:
                    textBox.Clear();
                    break;
                case ComboBox combo:
                    combo.SelectedIndex = -1;
                    break;
                case NumericUpDown number:
                    number.Value = number.Minimum;
                    break;
                case CheckBox check:
                    check.Checked = false;
                    break;
            }
            if (child.HasChildren) ClearEditableFields(child);
        }
    }

    private static string NormalizeName(string value)
    {
        var filtered = new string(value.Where(char.IsLetterOrDigit).ToArray());
        return string.IsNullOrWhiteSpace(filtered) ? Guid.NewGuid().ToString("N") : filtered;
    }

    private sealed class PreviewRow { }
}
