using TransportERP.Desktop.Controls;
using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop.Views.Security.Shared;

/// <summary>
/// أدوات تصميم مشتركة لشاشات الأمن والإدارة.
/// تعتمد على CoreUI للأجزاء العامة وتبقي الحقول ضمن حد 3 أعمدة/5 صفوف بلا Scroll في البيانات الرئيسية.
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

        ConfigureTabs(tabs);
        var hasMembersTab = tabDefinitions.Any(definition => IsMembersTab(definition.Title));

        for (var i = 0; i < tabDefinitions.Count; i++)
        {
            var definition = tabDefinitions[i];
            var page = CreateTabPage(definition.Title);
            var workspace = i == 0
                ? CreateFieldWorkspace(fields, hasMembersTab ? Array.Empty<string>() : specialActions)
                : CreateSpecializedWorkspace(definition, specialActions);

            page.Controls.Add(workspace);
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

    private static void ConfigureTabs(TabControl tabs)
    {
        tabs.Dock = DockStyle.Fill;
        tabs.RightToLeft = RightToLeft.Yes;
        tabs.RightToLeftLayout = true;
        tabs.Multiline = false;
        tabs.Padding = new Point(TransportUiMetrics.TabHorizontalPadding, TransportUiMetrics.TabVerticalPadding);
        tabs.Margin = Padding.Empty;
    }

    private static TabPage CreateTabPage(string title) => new()
    {
        Text = title,
        BackColor = UiTheme.SurfaceBackground,
        RightToLeft = RightToLeft.Yes,
        Padding = new Padding(TransportUiMetrics.TabContentPadding),
        AutoScroll = false
    };

    private static Control CreateFieldWorkspace(
        IReadOnlyList<SecurityFieldDefinition> fields,
        IReadOnlyList<string> specialActions)
        => CreateFormWorkspace("بيانات الشاشة", fields, specialActions);

    /// <summary>
    /// نموذج الحقول الأمني يقاس من محتواه الحقيقي. لا توجد حاوية Scroll بين التبويب وTransportDataEntryPanel.
    /// </summary>
    private static Control CreateFormWorkspace(
        string title,
        IReadOnlyList<SecurityFieldDefinition> fields,
        IReadOnlyList<string>? actions = null)
    {
        actions ??= Array.Empty<string>();
        var fieldColumns = TransportUiMetrics.ResolveMainDataFieldColumns(fields.Count);
        var rows = Math.Max(1, (int)Math.Ceiling(fields.Count / (double)fieldColumns));
        if (rows > TransportUiMetrics.MainDataMaxRows)
        {
            throw new InvalidOperationException($"قسم الأمن '{title}' يتجاوز الحد الحاكم البالغ {TransportUiMetrics.MainDataMaxRows} صفوف.");
        }

        var section = CreateSection(title, autoSize: true);
        var layout = new TableLayoutPanel
        {
            ColumnCount = 1,
            RowCount = actions.Count > 0 ? 2 : 1,
            Dock = DockStyle.Top,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            BackColor = UiTheme.SurfaceBackground,
            RightToLeft = RightToLeft.Yes,
            Padding = Padding.Empty,
            Margin = Padding.Empty
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

        var dataEntry = new TransportDataEntryPanel
        {
            FieldColumnCount = fieldColumns,
            Dock = DockStyle.Top,
            AutoScroll = false,
            Margin = Padding.Empty,
            Padding = Padding.Empty
        };

        for (var index = 0; index < fields.Count; index++)
        {
            var definition = fields[index];
            dataEntry.AddField(definition.Label, CreateEditor(definition), index);
        }

        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.Controls.Add(dataEntry, 0, 0);

        if (actions.Count > 0)
        {
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, TransportUiMetrics.ActionPanelHeight));
            var actionPanel = new TransportActionPanel { Dock = DockStyle.Fill, Margin = Padding.Empty };
            foreach (var action in actions)
            {
                actionPanel.AddAction(action);
            }
            layout.Controls.Add(actionPanel, 0, 1);
        }

        section.Controls.Add(layout);
        return section;
    }

    private static Control CreateEditor(SecurityFieldDefinition definition)
    {
        Control editor = definition.Kind switch
        {
            SecurityFieldKind.RequiredText => new RequiredTextBox(),
            SecurityFieldKind.Choice => CreateChoice(definition.Items),
            SecurityFieldKind.Date => new TransportDatePicker { ShowCheckBox = true },
            SecurityFieldKind.Boolean => CreateChoice(new[] { "نعم", "لا" }),
            SecurityFieldKind.Multiline => new TransportMultilineTextBox(),
            SecurityFieldKind.Masked => new TransportTextBox { ReadOnly = true, UseSystemPasswordChar = true },
            _ => new TransportTextBox()
        };

        editor.Name = $"fld{definition.Label.GetHashCode():X8}";
        editor.Tag = definition.Label;
        editor.RightToLeft = RightToLeft.Yes;

        if (editor is TextBox textBox)
        {
            textBox.TextAlign = HorizontalAlignment.Right;
        }

        var visualState = definition.Kind switch
        {
            SecurityFieldKind.RequiredText => TransportFieldVisualState.Required,
            SecurityFieldKind.Masked => TransportFieldVisualState.ReadOnly,
            _ => TransportFieldVisualState.Normal
        };
        TransportFieldState.Apply(editor, visualState);
        return editor;
    }

    private static TransportComboBox CreateChoice(IReadOnlyList<string>? items)
    {
        var combo = new TransportComboBox();
        if (items is not null)
        {
            combo.Items.AddRange(items.Cast<object>().ToArray());
        }
        return combo;
    }

    private static Control CreateSpecializedWorkspace(
        SecurityTabDefinition definition,
        IReadOnlyList<string> screenActions)
    {
        if (definition.Fields is { Count: > 0 })
        {
            var outer = CreateSection(definition.Title, autoSize: true);
            var host = CreateAutoSizeDescriptionLayout();
            host.Controls.Add(CreateDescription(definition.Description), 0, 0);
            host.Controls.Add(CreateFormWorkspace(
                definition.Title,
                definition.Fields,
                definition.Actions ?? Array.Empty<string>()), 0, 1);
            outer.Controls.Add(host);
            return outer;
        }

        var section = CreateSection(definition.Title);
        var layout = CreateSingleColumnLayout(TransportUiMetrics.TabDescriptionHeight);
        layout.Controls.Add(CreateDescription(definition.Description), 0, 0);

        Control content;
        if (IsMembersTab(definition.Title))
        {
            content = CreateMembersWorkspace(definition.Actions ?? screenActions);
        }
        else if (definition.Columns is { Count: > 0 })
        {
            content = CreateGridWorkspace(definition.Title, definition.Columns);
        }
        else
        {
            content = definition.Kind switch
            {
                SecurityTabKind.Tree => CreateTreeWorkspace(),
                SecurityTabKind.CheckList => CreateCheckListWorkspace(),
                SecurityTabKind.Comparison => CreateComparisonWorkspace(),
                SecurityTabKind.Settings => CreateSettingsWorkspace(),
                SecurityTabKind.Audit => CreateGridWorkspace("سجل العمليات", new[] { "التاريخ والوقت", "المستخدم", "العملية", "السبب / المرجع" }),
                _ => CreateGridWorkspace("التفاصيل", new[] { "العنصر", "القيمة / الحالة", "ملاحظات" })
            };
        }

        layout.Controls.Add(content, 0, 1);
        section.Controls.Add(layout);
        return section;
    }

    private static Label CreateDescription(string text) => new()
    {
        Dock = DockStyle.Fill,
        Margin = Padding.Empty,
        Padding = new Padding(TransportUiMetrics.CompactPadding),
        RightToLeft = RightToLeft.Yes,
        Text = text,
        TextAlign = ContentAlignment.MiddleRight,
        ForeColor = UiTheme.SecondaryText
    };

    private static TableLayoutPanel CreateAutoSizeDescriptionLayout()
    {
        var layout = new TableLayoutPanel
        {
            ColumnCount = 1,
            RowCount = 2,
            Dock = DockStyle.Top,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            BackColor = UiTheme.SurfaceBackground,
            RightToLeft = RightToLeft.Yes,
            Padding = Padding.Empty,
            Margin = Padding.Empty
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, TransportUiMetrics.TabDescriptionHeight));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        return layout;
    }

    private static Control CreateMembersWorkspace(IReadOnlyList<string> specialActions)
    {
        var section = CreateSection("أعضاء المجموعة");
        var layout = CreateSingleColumnLayout(TransportUiMetrics.ActionPanelHeight);
        var actions = new TransportActionPanel { Dock = DockStyle.Fill, Visible = specialActions.Count > 0 };
        foreach (var action in specialActions) actions.AddAction(action);

        var grid = new TransportDataGrid
        {
            Dock = DockStyle.Fill,
            AutoGenerateColumns = false,
            Margin = Padding.Empty,
            EmptyStateText = "لا يوجد أعضاء ضمن المجموعة"
        };
        foreach (var column in new[] { "العضو", "نوع العضو", "الشركة", "الفرع", "الحالة", "تاريخ الإضافة", "أضيف بواسطة" })
        {
            grid.Columns.Add($"col{grid.Columns.Count + 1}", column);
        }
        layout.Controls.Add(actions, 0, 0);
        layout.Controls.Add(grid, 0, 1);
        section.Controls.Add(layout);
        return section;
    }

    private static bool IsMembersTab(string title) => string.Equals(title?.Trim(), "الأعضاء", StringComparison.Ordinal);

    private static Control CreateTreeWorkspace()
    {
        var section = CreateSection("الشجرة");
        var host = CreateContentHost();
        var tree = new TreeView
        {
            Dock = DockStyle.Fill,
            CheckBoxes = true,
            FullRowSelect = true,
            HideSelection = false,
            RightToLeft = RightToLeft.Yes,
            BorderStyle = BorderStyle.FixedSingle
        };
        host.Controls.Add(tree);
        section.Controls.Add(host);
        return section;
    }

    private static Control CreateCheckListWorkspace()
    {
        var section = CreateSection("العناصر المتاحة");
        var host = CreateContentHost();
        var list = new CheckedListBox
        {
            Dock = DockStyle.Fill,
            CheckOnClick = true,
            RightToLeft = RightToLeft.Yes,
            BorderStyle = BorderStyle.FixedSingle
        };
        host.Controls.Add(list);
        section.Controls.Add(host);
        return section;
    }

    private static Control CreateGridWorkspace(string title, IReadOnlyList<string> columns)
    {
        var section = CreateSection(title);
        var grid = new TransportDataGrid { Dock = DockStyle.Fill, AutoGenerateColumns = false, Margin = Padding.Empty };
        foreach (var column in columns) grid.Columns.Add($"col{grid.Columns.Count + 1}", column);
        section.Controls.Add(grid);
        return section;
    }

    private static Control CreateSettingsWorkspace()
        => CreateFormWorkspace(
            "الإعدادات",
            new SecurityFieldDefinition[]
            {
                new("تفعيل الإعداد ضمن النطاق", SecurityFieldKind.Boolean),
                new("الوراثة من المستوى الأعلى", SecurityFieldKind.Boolean),
                new("يتطلب اعتمادًا قبل التطبيق", SecurityFieldKind.Boolean)
            });

    private static Control CreateComparisonWorkspace()
    {
        var section = CreateSection("المقارنة");
        var split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Vertical,
            RightToLeft = RightToLeft.Yes,
            Margin = Padding.Empty
        };
        split.Panel1.Controls.Add(CreateReadOnlySection("القيم قبل العملية"));
        split.Panel2.Controls.Add(CreateReadOnlySection("القيم بعد العملية"));
        section.Controls.Add(split);
        return section;
    }

    private static Control CreateReadOnlySection(string title)
    {
        var section = CreateSection(title);
        var text = new TransportMultilineTextBox
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            Text = title,
            Margin = Padding.Empty
        };
        TransportFieldState.Apply(text, TransportFieldVisualState.ReadOnly);
        section.Controls.Add(text);
        return section;
    }

    private static TransportGroupBox CreateSection(string title, bool autoSize = false) => new()
    {
        Dock = autoSize ? DockStyle.Top : DockStyle.Fill,
        AutoSize = autoSize,
        AutoSizeMode = autoSize ? AutoSizeMode.GrowAndShrink : AutoSizeMode.GrowOnly,
        Margin = Padding.Empty,
        Text = title,
        RightToLeft = RightToLeft.Yes
    };

    private static Panel CreateContentHost() => new()
    {
        Dock = DockStyle.Fill,
        AutoScroll = false,
        BackColor = UiTheme.SurfaceBackground,
        RightToLeft = RightToLeft.Yes,
        Padding = new Padding(TransportUiMetrics.CompactPadding),
        Margin = Padding.Empty
    };

    private static TableLayoutPanel CreateSingleColumnLayout(int firstRowHeight)
    {
        var layout = new TableLayoutPanel
        {
            ColumnCount = 1,
            RowCount = 2,
            Dock = DockStyle.Fill,
            BackColor = UiTheme.SurfaceBackground,
            RightToLeft = RightToLeft.Yes,
            Padding = Padding.Empty,
            Margin = Padding.Empty
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, firstRowHeight));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        return layout;
    }
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
    string Description,
    IReadOnlyList<SecurityFieldDefinition>? Fields = null,
    IReadOnlyList<string>? Columns = null,
    IReadOnlyList<string>? Actions = null);
