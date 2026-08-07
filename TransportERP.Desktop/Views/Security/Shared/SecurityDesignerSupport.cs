using TransportERP.Desktop.CoreUI.Controls;

namespace TransportERP.Desktop.Views.Security.Shared;

/// <summary>
/// أدوات تصميم مشتركة لشاشات الأمن والإدارة.
/// تستدعى من ملفات Designer لتجنب تكرار بناء التبويبات والحقول المتخصصة،
/// مع إبقاء شريط الأوامر والبحث والجدول والتنقل والتدقيق داخل CoreUI.
/// </summary>
internal static class SecurityDesignerSupport
{
    internal static void ConfigureScreen(
        TransportReferenceScreenShell shell,
        TabControl tabs,
        IReadOnlyList<SecurityTabDefinition> tabDefinitions,
        IReadOnlyList<SecurityFieldDefinition> fields,
        IReadOnlyList<string> gridColumns,
        IReadOnlyList<string> specialActions,
        SecurityWorkspaceMode mode)
    {
        shell.Dock = DockStyle.Fill;
        shell.RightToLeft = RightToLeft.Yes;
        shell.DataGroupTitle = "تفاصيل الشاشة";

        tabs.Dock = DockStyle.Fill;
        tabs.RightToLeft = RightToLeft.Yes;
        tabs.RightToLeftLayout = true;
        tabs.Multiline = false;
        tabs.Padding = new Point(16, 5);

        for (var i = 0; i < tabDefinitions.Count; i++)
        {
            var definition = tabDefinitions[i];
            var page = CreateTabPage(definition.Title);

            if (i == 0)
            {
                page.Controls.Add(CreateFieldWorkspace(fields, specialActions));
            }
            else
            {
                page.Controls.Add(CreateSpecializedWorkspace(definition));
            }

            tabs.TabPages.Add(page);
        }

        shell.DataHost.Controls.Add(tabs);

        shell.Grid.AutoGenerateColumns = false;
        shell.Grid.Columns.Clear();
        foreach (var column in gridColumns)
        {
            shell.Grid.Columns.Add($"col{shell.Grid.Columns.Count + 1}", column);
        }

        shell.ConfigureWorkspaceMode(
            showSearch: mode != SecurityWorkspaceMode.Settings,
            showGrid: mode is not SecurityWorkspaceMode.Settings and not SecurityWorkspaceMode.Tree,
            expandDataWorkspace: mode is SecurityWorkspaceMode.Settings or SecurityWorkspaceMode.Tree);
    }

    private static TabPage CreateTabPage(string title) => new()
    {
        Text = title,
        BackColor = Color.White,
        RightToLeft = RightToLeft.Yes,
        Padding = new Padding(8)
    };

    private static Control CreateFieldWorkspace(
        IReadOnlyList<SecurityFieldDefinition> fields,
        IReadOnlyList<string> specialActions)
    {
        var host = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            RightToLeft = RightToLeft.Yes,
            ColumnCount = 1,
            RowCount = 2
        };
        host.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        host.RowStyles.Add(new RowStyle(SizeType.Absolute, specialActions.Count == 0 ? 0F : 38F));
        host.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var actionStrip = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            RightToLeft = RightToLeft.Yes,
            WrapContents = false,
            AutoScroll = true,
            Padding = new Padding(2)
        };

        foreach (var action in specialActions)
        {
            actionStrip.Controls.Add(new Button
            {
                AutoSize = true,
                Height = 30,
                Text = action,
                RightToLeft = RightToLeft.Yes,
                FlatStyle = FlatStyle.System,
                Tag = action
            });
        }

        var scroll = new Panel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            BackColor = Color.White,
            RightToLeft = RightToLeft.Yes
        };

        var table = new TableLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 4,
            Dock = DockStyle.Top,
            RightToLeft = RightToLeft.Yes,
            Padding = new Padding(4)
        };

        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145F));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145F));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

        var rows = (fields.Count + 1) / 2;
        table.RowCount = rows;
        for (var row = 0; row < rows; row++)
        {
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, TransportUiMetrics.MainDataRowHeight));
        }

        for (var index = 0; index < fields.Count; index++)
        {
            var row = index / 2;
            var pair = index % 2;
            var labelColumn = pair == 0 ? 0 : 2;
            var fieldColumn = labelColumn + 1;
            var definition = fields[index];

            var label = new Label
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(4),
                Text = definition.Label,
                TextAlign = ContentAlignment.MiddleRight,
                RightToLeft = RightToLeft.Yes
            };

            var editor = CreateEditor(definition);
            table.Controls.Add(label, labelColumn, row);
            table.Controls.Add(editor, fieldColumn, row);
        }

        scroll.Controls.Add(table);
        host.Controls.Add(actionStrip, 0, 0);
        host.Controls.Add(scroll, 0, 1);
        return host;
    }

    private static Control CreateEditor(SecurityFieldDefinition definition)
    {
        Control editor = definition.Kind switch
        {
            SecurityFieldKind.RequiredText => new RequiredTextBox(),
            SecurityFieldKind.Choice => CreateChoice(definition.Items),
            SecurityFieldKind.Date => new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "yyyy/MM/dd",
                ShowCheckBox = true
            },
            SecurityFieldKind.Boolean => CreateChoice(new[] { "نعم", "لا" }),
            SecurityFieldKind.Multiline => new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Height = 58
            },
            SecurityFieldKind.Masked => new TextBox
            {
                UseSystemPasswordChar = true,
                ReadOnly = true
            },
            _ => new TextBox()
        };

        editor.Name = $"fld{definition.Label.GetHashCode():X8}";
        editor.Tag = definition.Label;
        editor.Dock = DockStyle.Fill;
        editor.Margin = new Padding(4, TransportUiMetrics.MainDataVerticalMargin, 8, TransportUiMetrics.MainDataVerticalMargin);
        editor.RightToLeft = RightToLeft.Yes;

        if (editor is TextBox textBox)
        {
            textBox.TextAlign = HorizontalAlignment.Right;
        }

        return editor;
    }

    private static ComboBox CreateChoice(IReadOnlyList<string>? items)
    {
        var combo = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            RightToLeft = RightToLeft.Yes
        };

        if (items is not null)
        {
            combo.Items.AddRange(items.Cast<object>().ToArray());
        }

        return combo;
    }

    private static Control CreateSpecializedWorkspace(SecurityTabDefinition definition)
    {
        var host = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 2,
            RightToLeft = RightToLeft.Yes,
            BackColor = Color.White
        };
        host.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        host.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        host.Controls.Add(new Label
        {
            Dock = DockStyle.Fill,
            Text = definition.Description,
            TextAlign = ContentAlignment.MiddleRight,
            ForeColor = Color.FromArgb(80, 88, 102),
            RightToLeft = RightToLeft.Yes
        }, 0, 0);

        Control content = definition.Kind switch
        {
            SecurityTabKind.Tree => CreateTree(),
            SecurityTabKind.CheckList => CreateCheckList(),
            SecurityTabKind.Comparison => CreateComparison(),
            SecurityTabKind.Settings => CreateSettingsPanel(),
            SecurityTabKind.Audit => CreateAuditList(),
            _ => CreateDetailsList()
        };

        host.Controls.Add(content, 0, 1);
        return host;
    }

    private static TreeView CreateTree() => new()
    {
        Dock = DockStyle.Fill,
        CheckBoxes = true,
        FullRowSelect = true,
        HideSelection = false,
        RightToLeft = RightToLeft.Yes,
        BorderStyle = BorderStyle.FixedSingle
    };

    private static CheckedListBox CreateCheckList() => new()
    {
        Dock = DockStyle.Fill,
        CheckOnClick = true,
        RightToLeft = RightToLeft.Yes,
        BorderStyle = BorderStyle.FixedSingle
    };

    private static ListView CreateDetailsList()
    {
        var list = new ListView
        {
            Dock = DockStyle.Fill,
            FullRowSelect = true,
            GridLines = true,
            HideSelection = false,
            RightToLeft = RightToLeft.Yes,
            View = View.Details
        };
        list.Columns.Add("العنصر", 240, HorizontalAlignment.Right);
        list.Columns.Add("القيمة / الحالة", 420, HorizontalAlignment.Right);
        list.Columns.Add("ملاحظات", 320, HorizontalAlignment.Right);
        return list;
    }

    private static ListView CreateAuditList()
    {
        var list = new ListView
        {
            Dock = DockStyle.Fill,
            FullRowSelect = true,
            GridLines = true,
            HideSelection = false,
            RightToLeft = RightToLeft.Yes,
            View = View.Details
        };
        list.Columns.Add("التاريخ والوقت", 160, HorizontalAlignment.Right);
        list.Columns.Add("المستخدم", 180, HorizontalAlignment.Right);
        list.Columns.Add("العملية", 220, HorizontalAlignment.Right);
        list.Columns.Add("السبب / المرجع", 380, HorizontalAlignment.Right);
        return list;
    }

    private static Control CreateSettingsPanel()
    {
        var panel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true,
            RightToLeft = RightToLeft.Yes,
            Padding = new Padding(12)
        };

        foreach (var text in new[] { "تفعيل الإعداد ضمن النطاق", "الوراثة من المستوى الأعلى", "يتطلب اعتمادًا قبل التطبيق" })
        {
            panel.Controls.Add(new CheckBox
            {
                AutoSize = true,
                Text = text,
                RightToLeft = RightToLeft.Yes,
                Margin = new Padding(6)
            });
        }

        return panel;
    }

    private static Control CreateComparison()
    {
        var split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Vertical,
            RightToLeft = RightToLeft.Yes,
            SplitterDistance = 480
        };

        split.Panel1.Controls.Add(CreateReadOnlyBox("القيم قبل العملية"));
        split.Panel2.Controls.Add(CreateReadOnlyBox("القيم بعد العملية"));
        return split;
    }

    private static TextBox CreateReadOnlyBox(string placeholder) => new()
    {
        Dock = DockStyle.Fill,
        Multiline = true,
        ReadOnly = true,
        ScrollBars = ScrollBars.Both,
        RightToLeft = RightToLeft.Yes,
        TextAlign = HorizontalAlignment.Right,
        Text = placeholder
    };
}

internal enum SecurityFieldKind
{
    Text,
    RequiredText,
    Choice,
    Date,
    Boolean,
    Multiline,
    Masked
}

internal enum SecurityTabKind
{
    Details,
    Tree,
    CheckList,
    Settings,
    Audit,
    Comparison
}

internal enum SecurityWorkspaceMode
{
    Edit,
    ReadOnly,
    ReadOnlyWithActions,
    Settings,
    Tree
}

internal sealed record SecurityFieldDefinition(
    string Label,
    SecurityFieldKind Kind = SecurityFieldKind.Text,
    IReadOnlyList<string>? Items = null);

internal sealed record SecurityTabDefinition(
    string Title,
    SecurityTabKind Kind,
    string Description);