using TransportERP.Desktop.Controls;
using TransportERP.Desktop.CoreUI.Controls;

namespace TransportERP.Desktop.Views.Security.Shared;

/// <summary>
/// أدوات تصميم مشتركة لشاشات الأمن والإدارة.
/// تعتمد حصريًا على مكونات CoreUI للأجزاء العامة، وتسمح فقط بالأدوات المتخصصة
/// مثل TreeView وCheckedListBox داخل حاويات منظمة وليست مباشرة على الشاشة أو التبويب.
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

        for (var i = 0; i < tabDefinitions.Count; i++)
        {
            var definition = tabDefinitions[i];
            var page = CreateTabPage(definition.Title);
            var workspace = i == 0
                ? CreateFieldWorkspace(fields, specialActions)
                : CreateSpecializedWorkspace(definition);

            // لا توضع أداة وظيفية مباشرة على TabPage؛ الحاوية هي الابن المباشر الوحيد.
            page.Controls.Add(workspace);
            tabs.TabPages.Add(page);
        }

        // شاشة الـUserControl تحتوي القالب العام فقط؛ والتبويبات تستضاف داخل DataHost.
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
        tabs.Padding = new Point(
            TransportUiMetrics.TabHorizontalPadding,
            TransportUiMetrics.TabVerticalPadding);
        tabs.Margin = Padding.Empty;
    }

    private static TabPage CreateTabPage(string title) => new()
    {
        Text = title,
        BackColor = Color.White,
        RightToLeft = RightToLeft.Yes,
        Padding = new Padding(TransportUiMetrics.TabContentPadding)
    };

    /// <summary>
    /// يبني التبويب الرئيسي من TransportActionPanel وTransportDataEntryPanel.
    /// بهذه الطريقة تبقى الأزرار والحقول والمقاسات مرتبطة مباشرة بالـCoreUI.
    /// </summary>
    private static Control CreateFieldWorkspace(
        IReadOnlyList<SecurityFieldDefinition> fields,
        IReadOnlyList<string> specialActions)
    {
        var section = CreateSection("بيانات الشاشة");
        var layout = CreateSingleColumnLayout(specialActions.Count == 0 ? 0 : TransportUiMetrics.ActionPanelHeight);

        var actions = new TransportActionPanel
        {
            Dock = DockStyle.Fill,
            Visible = specialActions.Count > 0
        };

        foreach (var action in specialActions)
        {
            actions.AddAction(action);
        }

        var scrollHost = new Panel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            BackColor = Color.White,
            RightToLeft = RightToLeft.Yes,
            Padding = Padding.Empty,
            Margin = Padding.Empty
        };

        var dataEntry = new TransportDataEntryPanel
        {
            Dock = DockStyle.Top,
            Margin = Padding.Empty
        };

        for (var index = 0; index < fields.Count; index++)
        {
            var definition = fields[index];
            dataEntry.AddField(definition.Label, CreateEditor(definition), index);
        }

        scrollHost.Controls.Add(dataEntry);
        layout.Controls.Add(actions, 0, 0);
        layout.Controls.Add(scrollHost, 0, 1);
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
            SecurityFieldKind.Masked => new TransportTextBox
            {
                ReadOnly = true,
                UseSystemPasswordChar = true
            },
            _ => new TransportTextBox()
        };

        editor.Name = $"fld{definition.Label.GetHashCode():X8}";
        editor.Tag = definition.Label;
        editor.RightToLeft = RightToLeft.Yes;

        if (editor is TextBox textBox)
        {
            textBox.TextAlign = HorizontalAlignment.Right;
        }

        // حالات الحقول موحدة من CoreUI؛ لا توجد ألوان Required/ReadOnly محلية داخل شاشات الأمن.
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

    /// <summary>
    /// كل تبويب متخصص يبدأ بحاوية عامة، ثم وصف بارتفاع مركزي، ثم محتوى متخصص داخل حاوية.
    /// </summary>
    private static Control CreateSpecializedWorkspace(SecurityTabDefinition definition)
    {
        var section = CreateSection(definition.Title);
        var host = CreateSingleColumnLayout(TransportUiMetrics.TabDescriptionHeight);

        var description = new Label
        {
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            Padding = new Padding(TransportUiMetrics.CompactPadding),
            RightToLeft = RightToLeft.Yes,
            Text = definition.Description,
            TextAlign = ContentAlignment.MiddleRight,
            ForeColor = SystemColors.GrayText
        };

        var content = definition.Kind switch
        {
            SecurityTabKind.Tree => CreateTreeWorkspace(),
            SecurityTabKind.CheckList => CreateCheckListWorkspace(),
            SecurityTabKind.Comparison => CreateComparisonWorkspace(),
            SecurityTabKind.Settings => CreateSettingsWorkspace(),
            SecurityTabKind.Audit => CreateGridWorkspace(
                "سجل العمليات",
                new[] { "التاريخ والوقت", "المستخدم", "العملية", "السبب / المرجع" }),
            _ => CreateGridWorkspace(
                "التفاصيل",
                new[] { "العنصر", "القيمة / الحالة", "ملاحظات" })
        };

        host.Controls.Add(description, 0, 0);
        host.Controls.Add(content, 0, 1);
        section.Controls.Add(host);
        return section;
    }

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
        var grid = new TransportDataGrid
        {
            Dock = DockStyle.Fill,
            AutoGenerateColumns = false,
            Margin = Padding.Empty
        };

        foreach (var column in columns)
        {
            grid.Columns.Add($"col{grid.Columns.Count + 1}", column);
        }

        section.Controls.Add(grid);
        return section;
    }

    private static Control CreateSettingsWorkspace()
    {
        var section = CreateSection("الإعدادات");
        var scrollHost = new Panel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            BackColor = Color.White,
            RightToLeft = RightToLeft.Yes,
            Padding = Padding.Empty,
            Margin = Padding.Empty
        };

        var dataEntry = new TransportDataEntryPanel
        {
            Dock = DockStyle.Top,
            Margin = Padding.Empty
        };

        var settings = new[]
        {
            "تفعيل الإعداد ضمن النطاق",
            "الوراثة من المستوى الأعلى",
            "يتطلب اعتمادًا قبل التطبيق"
        };

        for (var index = 0; index < settings.Length; index++)
        {
            dataEntry.AddField(settings[index], CreateChoice(new[] { "نعم", "لا" }), index);
        }

        scrollHost.Controls.Add(dataEntry);
        section.Controls.Add(scrollHost);
        return section;
    }

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

    private static TransportGroupBox CreateSection(string title) => new()
    {
        Dock = DockStyle.Fill,
        Margin = Padding.Empty,
        Text = title,
        RightToLeft = RightToLeft.Yes
    };

    private static Panel CreateContentHost() => new()
    {
        Dock = DockStyle.Fill,
        BackColor = Color.White,
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
            BackColor = Color.White,
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
    string Description);
