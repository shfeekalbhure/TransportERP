using System.ComponentModel;
using TransportERP.Desktop.Controls;
using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop.Forms.Setup.General;

/// <summary>
/// منشئ مركزي لمحتوى شاشات المجموعة الثانية. يعتمد على CoreUI للمقاسات والحاويات والحقول العامة،
/// ويترك لكل شاشة تعريف التبويبات والبيانات المتخصصة فقط.
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
            HotTrack = true,
            Margin = Padding.Empty,
            Padding = new Point(TransportUiMetrics.TabHorizontalPadding, TransportUiMetrics.TabVerticalPadding)
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
            BackColor = UiTheme.SurfaceBackground,
            Padding = new Padding(TransportUiMetrics.TabContentPadding),
            AutoScroll = false
        };

        if (spec.IsLog)
        {
            // TabPage -> Container -> Grid، حتى يبقى مبدأ الحاويات ثابتًا.
            var logHost = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = false,
                BackColor = UiTheme.SurfaceBackground,
                Margin = Padding.Empty,
                Padding = Padding.Empty,
                RightToLeft = RightToLeft.Yes
            };
            logHost.Controls.Add(CreateLogGrid());
            page.Controls.Add(logHost);
            return page;
        }

        var fieldColumns = TransportUiMetrics.ResolveMainDataFieldColumns(spec.Fields.Length);
        var fieldRows = Math.Max(1, (int)Math.Ceiling(spec.Fields.Length / (double)fieldColumns));
        if (fieldRows > TransportUiMetrics.MainDataMaxRows)
        {
            throw new InvalidOperationException($"التبويب '{spec.Title}' يتجاوز الحد الحاكم البالغ {TransportUiMetrics.MainDataMaxRows} صفوف.");
        }

        var root = new TableLayoutPanel
        {
            ColumnCount = 1,
            RowCount = spec.ActionButtons is { Length: > 0 } ? 2 : 1,
            Dock = DockStyle.Top,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            AutoScroll = false,
            BackColor = UiTheme.SurfaceBackground,
            RightToLeft = RightToLeft.Yes,
            Padding = Padding.Empty,
            Margin = Padding.Empty
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var dataEntry = new TransportDataEntryPanel
        {
            FieldColumnCount = fieldColumns,
            Dock = DockStyle.Top,
            AutoScroll = false,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };

        for (var index = 0; index < spec.Fields.Length; index++)
        {
            var field = spec.Fields[index];
            var label = field.Caption + (field.Kind == FieldKind.RequiredText ? " *" : string.Empty);
            dataEntry.AddField(label, CreateEditor(field), index);
        }

        root.Controls.Add(dataEntry, 0, 0);

        if (spec.ActionButtons is { Length: > 0 } buttons)
        {
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, TransportUiMetrics.ActionPanelHeight));
            var actions = new TransportActionPanel { Dock = DockStyle.Fill, Margin = Padding.Empty };
            foreach (var caption in buttons)
            {
                actions.AddAction(caption);
            }
            root.Controls.Add(actions, 0, 1);
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
            FieldKind.Date => new TransportDatePicker(),
            FieldKind.Number => new NumericUpDown
            {
                DecimalPlaces = 2,
                Maximum = 999999999M,
                ThousandsSeparator = true,
                TextAlign = HorizontalAlignment.Right,
                RightToLeft = RightToLeft.Yes
            },
            FieldKind.Check => new CheckBox { Text = "نعم", AutoSize = true, TextAlign = ContentAlignment.MiddleRight, RightToLeft = RightToLeft.Yes },
            FieldKind.Multiline => new TransportMultilineTextBox(),
            FieldKind.Picture => new PictureBox
            {
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                MinimumSize = new Size(160, TransportUiMetrics.MainDataMultilineMinHeight)
            },
            _ => new TransportTextBox()
        };

        control.Dock = field.Kind == FieldKind.Check ? DockStyle.Right : DockStyle.Fill;
        control.RightToLeft = RightToLeft.Yes;
        control.Name = "fld" + NormalizeName(field.Caption);
        return control;
    }

    private static TransportComboBox CreateCombo(string[]? items)
    {
        var combo = new TransportComboBox();
        if (items is { Length: > 0 }) combo.Items.AddRange(items.Cast<object>().ToArray());
        return combo;
    }

    private static TransportDataGrid CreateLogGrid()
    {
        var grid = new TransportDataGrid { Dock = DockStyle.Fill, AutoGenerateColumns = false, Margin = Padding.Empty };
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
