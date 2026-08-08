using System.ComponentModel;
using TransportERP.Desktop.Controls;
using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.CoreUI.Profiles;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop.Forms.Setup.General;

/// <summary>
/// منشئ مركزي لمحتوى شاشات المجموعة الثانية. دعم الـProfiles اختياري حتى لا تتأثر الشاشات غير المهاجرة.
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

    internal sealed record FieldSpec(
        string Caption,
        FieldKind Kind = FieldKind.Text,
        string[]? Items = null,
        TransportFieldProfile Profile = TransportFieldProfile.None);

    internal sealed record TabSpec(string Title, FieldSpec[] Fields, bool IsLog = false, string[]? ActionButtons = null);

    internal static TransportReferenceScreenShell Build(
        string screenCode,
        string screenTitle,
        IReadOnlyList<TabSpec> tabs,
        string searchPlaceholder,
        params string[] gridHeaders)
        => BuildCore(null, TransportGridProfile.None, screenCode, screenTitle, tabs, searchPlaceholder, gridHeaders);

    /// <summary>
    /// مسار Migration اختياري للشاشات التي تعلن V1 صراحة. الـMetadata توصف هنا، والـPolicy تطبق لاحقًا من الشاشة.
    /// </summary>
    internal static TransportReferenceScreenShell BuildProfiled(
        TransportLayoutRoleProvider metadata,
        TransportGridProfile gridProfile,
        string screenCode,
        string screenTitle,
        IReadOnlyList<TabSpec> tabs,
        string searchPlaceholder,
        params string[] gridHeaders)
        => BuildCore(metadata, gridProfile, screenCode, screenTitle, tabs, searchPlaceholder, gridHeaders);

    private static TransportReferenceScreenShell BuildCore(
        TransportLayoutRoleProvider? metadata,
        TransportGridProfile gridProfile,
        string screenCode,
        string screenTitle,
        IReadOnlyList<TabSpec> tabs,
        string searchPlaceholder,
        IReadOnlyList<string> gridHeaders)
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

        for (var index = 0; index < tabs.Count; index++)
        {
            tabControl.TabPages.Add(CreateTab(tabs[index], metadata, index == 0));
        }

        shell.DataHost.Controls.Add(tabControl);
        shell.ConfigureWorkspaceMode(showSearch: true, showGrid: true, expandDataWorkspace: false);
        ConfigureMainGrid(shell.Grid, gridHeaders);
        shell.Grid.BindData(new BindingList<PreviewRow>());

        if (metadata is not null)
        {
            metadata.SetLayoutRole(shell.Toolbar, TransportLayoutRole.Toolbar);
            metadata.SetLayoutRole(shell.SearchPanel, TransportLayoutRole.Search);
            metadata.SetLayoutRole(shell.Grid, TransportLayoutRole.Grid);
            metadata.SetLayoutRole(shell.Pagination, TransportLayoutRole.Pagination);
            metadata.SetLayoutRole(shell.AuditPanel, TransportLayoutRole.Audit);
            metadata.SetLayoutRole(shell.AlertBar, TransportLayoutRole.Alerts);
            metadata.SetGridProfile(shell.Grid, gridProfile);
        }

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

    internal static FieldSpec Required(string caption, TransportFieldProfile profile = TransportFieldProfile.None) =>
        new(caption, FieldKind.RequiredText, null, profile);

    internal static FieldSpec Text(string caption, TransportFieldProfile profile = TransportFieldProfile.None) =>
        new(caption, FieldKind.Text, null, profile);

    internal static FieldSpec Combo(string caption, params string[] items) =>
        new(caption, FieldKind.Combo, items);

    internal static FieldSpec ProfiledCombo(string caption, TransportFieldProfile profile, params string[] items) =>
        new(caption, FieldKind.Combo, items, profile);

    internal static FieldSpec Date(string caption, TransportFieldProfile profile = TransportFieldProfile.None) =>
        new(caption, FieldKind.Date, null, profile);

    internal static FieldSpec Number(string caption, TransportFieldProfile profile = TransportFieldProfile.None) =>
        new(caption, FieldKind.Number, null, profile);

    internal static FieldSpec Check(string caption, TransportFieldProfile profile = TransportFieldProfile.None) =>
        new(caption, FieldKind.Check, null, profile);

    internal static FieldSpec Multiline(string caption, TransportFieldProfile profile = TransportFieldProfile.None) =>
        new(caption, FieldKind.Multiline, null, profile);

    internal static FieldSpec Picture(string caption, TransportFieldProfile profile = TransportFieldProfile.None) =>
        new(caption, FieldKind.Picture, null, profile);

    private static TabPage CreateTab(TabSpec spec, TransportLayoutRoleProvider? metadata, bool isPrimary)
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

        if (metadata is not null && isPrimary)
        {
            metadata.SetLayoutRole(dataEntry, TransportLayoutRole.MainData);
        }

        for (var index = 0; index < spec.Fields.Length; index++)
        {
            var field = spec.Fields[index];
            var editor = CreateEditor(field);
            if (metadata is not null && field.Profile != TransportFieldProfile.None)
            {
                metadata.SetFieldProfile(editor, field.Profile);
            }

            var label = field.Caption + (field.Kind == FieldKind.RequiredText ? " *" : string.Empty);
            dataEntry.AddField(label, editor, index);
        }

        root.Controls.Add(dataEntry, 0, 0);

        if (spec.ActionButtons is { Length: > 0 } buttons)
        {
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, TransportUiMetrics.ActionPanelHeight));
            var actions = new TransportActionPanel { Dock = DockStyle.Fill, Margin = Padding.Empty };
            foreach (var caption in buttons) actions.AddAction(caption);
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
        foreach (var header in headers) AddColumn(grid, header, header is "الرمز" or "الحالة" ? 120 : null);
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
