using TransportERP.Desktop.CoreUI.Architecture;
using TransportERP.Desktop.Themes;

namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// إطار CoreUI الموحد. يملك الصفوف الرأسية المشتركة فقط، بينما يبني كل Profile
/// بنيته المعلنة صراحةً بدلاً من تحويل كل شاشة إلى نموذج حقول وجدول عام.
/// </summary>
public sealed class TransportScreenFrame : UserControl
{
    private readonly TableLayoutPanel _root = new();

    public TransportToolbar Toolbar { get; } = new();
    public TransportSearchPanel Search { get; } = new();
    public Panel MainData { get; } = new();
    public TransportDataGrid Grid { get; } = new();
    public TransportPagination Pagination { get; } = new();
    public TransportAuditPanel Audit { get; } = new();

    public TransportScreenFrame(ReferenceScreenDefinition definition)
    {
        ArgumentNullException.ThrowIfNull(definition);
        AutoScaleMode = AutoScaleMode.Dpi;
        Dock = DockStyle.Fill;
        BackColor = UiTheme.WorkspaceBackground;
        Padding = new Padding(8);
        RightToLeft = RightToLeft.Yes;

        _root.Dock = DockStyle.Fill;
        _root.ColumnCount = 1;
        _root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _root.RowCount = 6;
        _root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Toolbar: Fixed
        _root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Search: Fixed
        _root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Profile content: Content or Fill
        _root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // Results/lines/history: Fill
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, 0F)); // Pagination where required
        _root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Audit: Fixed

        Toolbar.Dock = DockStyle.Top;
        Search.Dock = DockStyle.Top;
        MainData.Dock = DockStyle.Top;
        MainData.AutoSize = true;
        MainData.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        MainData.AutoScroll = false;
        MainData.Padding = new Padding(8);
        MainData.BackColor = UiTheme.SurfaceBackground;
        Grid.Dock = DockStyle.Fill;
        Grid.AutoGenerateColumns = false;
        Pagination.Dock = DockStyle.Fill;
        Audit.Dock = DockStyle.Top;

        _root.Controls.Add(Toolbar, 0, 0);
        _root.Controls.Add(Search, 0, 1);
        _root.Controls.Add(MainData, 0, 2);
        _root.Controls.Add(Grid, 0, 3);
        _root.Controls.Add(Pagination, 0, 4);
        _root.Controls.Add(Audit, 0, 5);
        Controls.Add(_root);

        BuildProfile(definition);
    }

    private void BuildProfile(ReferenceScreenDefinition definition)
    {
        Search.SearchPlaceholder = $"ابحث في {definition.Title}";
        Search.SetStatusItems("نشط", "موقوف");

        switch (definition.Profile)
        {
            case TransportScreenProfile.MasterData:
                BuildMasterData(definition);
                break;
            case TransportScreenProfile.TreeMaster:
                BuildTreeMaster(definition);
                break;
            case TransportScreenProfile.ControlApproval:
                BuildPeriodLifecycle(definition);
                break;
            case TransportScreenProfile.Transaction:
                BuildTransaction(definition);
                break;
            case TransportScreenProfile.ReportInquiry:
                BuildReportInquiry(definition);
                break;
            case TransportScreenProfile.Settings:
                BuildScopedSettings(definition);
                break;
            default:
                throw new InvalidOperationException($"Unsupported frozen profile: {definition.Profile}");
        }
    }

    private void BuildMasterData(ReferenceScreenDefinition definition)
    {
        MainData.Controls.Add(CreateFields(definition.Fields, definition.IsReadOnly));
        ConfigureGrid(definition.GridColumns, definition.IsReadOnly, "قائمة السجلات");
    }

    private void BuildTreeMaster(ReferenceScreenDefinition definition)
    {
        SetMainDataToFill();
        Grid.Visible = false;
        _root.RowStyles[3].SizeType = SizeType.Absolute;
        _root.RowStyles[3].Height = 0F;

        var splitHost = new SplitContainer
        {
            AccessibleName = "SplitHost",
            Dock = DockStyle.Fill,
            FixedPanel = FixedPanel.Panel1,
            IsSplitterFixed = false,
            RightToLeft = RightToLeft.Yes,
            SplitterDistance = 310
        };
        var treeHost = new Panel { AccessibleName = "TreeHost", Dock = DockStyle.Fill, Padding = new Padding(6), BackColor = UiTheme.SurfaceBackground };
        var treeSearch = new TextBox { Dock = DockStyle.Top, PlaceholderText = "بحث في دليل الحسابات...", RightToLeft = RightToLeft.Yes };
        var tree = new TreeView { Dock = DockStyle.Fill, RightToLeft = RightToLeft.Yes, RightToLeftLayout = true, HideSelection = false };
        var assets = tree.Nodes.Add("الأصول");
        assets.Nodes.Add("الأصول المتداولة");
        var liabilities = tree.Nodes.Add("الالتزامات");
        liabilities.Nodes.Add("الالتزامات قصيرة الأجل");
        tree.Nodes.Add("الإيرادات");
        tree.Nodes.Add("المصروفات");
        tree.ExpandAll();
        treeHost.Controls.Add(tree);
        treeHost.Controls.Add(treeSearch);

        var detailsHost = new Panel { AccessibleName = "DetailsHost", Dock = DockStyle.Fill, Padding = new Padding(10), BackColor = UiTheme.SurfaceBackground };
        detailsHost.Controls.Add(CreateSectionTitle("تفاصيل الحساب المحدد", DockStyle.Top));
        var fields = CreateFields(definition.Fields, false);
        fields.Dock = DockStyle.Top;
        detailsHost.Controls.Add(fields);
        splitHost.Panel1.Controls.Add(treeHost);
        splitHost.Panel2.Controls.Add(detailsHost);
        MainData.Controls.Add(splitHost);
    }

    private void BuildPeriodLifecycle(ReferenceScreenDefinition definition)
    {
        var header = CreateFields(definition.Fields, false);
        var lifecycle = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Top, FlowDirection = FlowDirection.RightToLeft, RightToLeft = RightToLeft.Yes, Padding = new Padding(6) };
        lifecycle.Controls.Add(CreateBadge("الحالة الحالية: مفتوحة", Color.FromArgb(220, 252, 231), Color.FromArgb(22, 101, 52)));
        lifecycle.Controls.Add(CreateActionButton("إقفال الفترة"));
        lifecycle.Controls.Add(CreateActionButton("إعادة فتح"));
        lifecycle.Controls.Add(CreateActionButton("عرض السجل"));
        MainData.Controls.Add(lifecycle);
        MainData.Controls.Add(header);
        ConfigureGrid(["التاريخ", "الإجراء", "الحالة السابقة", "الحالة الجديدة", "المنفذ", "السبب"], true, "سجل دورة الفترة");
        Grid.Rows.Add("2026/08/10", "فتح الفترة", "—", "مفتوحة", "مدير النظام", "بداية السنة المالية");
    }

    private void BuildTransaction(ReferenceScreenDefinition definition)
    {
        var header = CreateFields(definition.Fields, true);
        var totals = new TableLayoutPanel { AutoSize = true, Dock = DockStyle.Top, ColumnCount = 4, RightToLeft = RightToLeft.Yes, Padding = new Padding(6) };
        for (var index = 0; index < 4; index++) totals.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        totals.Controls.Add(CreateSummaryLabel("الحالة: مرحّل", true), 0, 0);
        totals.Controls.Add(CreateSummaryLabel("إجمالي المدين: 0.00", false), 1, 0);
        totals.Controls.Add(CreateSummaryLabel("إجمالي الدائن: 0.00", false), 2, 0);
        totals.Controls.Add(CreateSummaryLabel("الفرق: 0.00", false), 3, 0);
        MainData.Controls.Add(totals);
        MainData.Controls.Add(header);
        Toolbar.SetActionEnabled(ToolbarAction.Save, false);
        Toolbar.SetActionEnabled(ToolbarAction.Edit, false);
        ConfigureGrid(["الحساب", "البيان", "مركز التكلفة", "مدين", "دائن"], true, "سطور القيد — قراءة فقط بعد الترحيل");
        Grid.Rows.Add("", "", "", "0.00", "0.00");
    }

    private void BuildReportInquiry(ReferenceScreenDefinition definition)
    {
        var filters = CreateFields(definition.Fields, false);
        var totals = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.RightToLeft, RightToLeft = RightToLeft.Yes, Dock = DockStyle.Bottom, Padding = new Padding(6) };
        totals.Controls.Add(CreateSummaryLabel("إجمالي مدين: 0.00 (من الخادم)", false));
        totals.Controls.Add(CreateSummaryLabel("إجمالي دائن: 0.00 (من الخادم)", false));
        totals.Controls.Add(CreateSummaryLabel("صافي الرصيد: 0.00 (من الخادم)", false));
        MainData.Controls.Add(totals);
        MainData.Controls.Add(filters);
        ConfigureGrid(definition.GridColumns, true, "نتائج ميزان المراجعة — للقراءة فقط");
        Grid.Rows.Add("", "0.00", "0.00", "0.00");
        Pagination.SetPageInfo(1, 1, 0, 0, 0);
        _root.RowStyles[4].SizeType = SizeType.AutoSize;
        _root.RowStyles[4].Height = TransportUiMetrics.PaginationHeight;
        Toolbar.SetActionVisible(ToolbarAction.New, false);
        Toolbar.SetActionVisible(ToolbarAction.Save, false);
        Toolbar.SetActionVisible(ToolbarAction.Edit, false);
        Toolbar.SetActionVisible(ToolbarAction.Disable, false);
        Toolbar.SetActionVisible(ToolbarAction.Delete, false);
    }

    private void BuildScopedSettings(ReferenceScreenDefinition definition)
    {
        SetMainDataToFill();
        Grid.Visible = false;
        _root.RowStyles[3].SizeType = SizeType.Absolute;
        _root.RowStyles[3].Height = 0F;
        Search.Visible = false;
        _root.RowStyles[1].SizeType = SizeType.Absolute;
        _root.RowStyles[1].Height = 0F;

        var settingsHost = new SplitContainer { AccessibleName = "SettingsHost", Dock = DockStyle.Fill, FixedPanel = FixedPanel.Panel1, RightToLeft = RightToLeft.Yes, SplitterDistance = 360 };
        var context = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10), BackColor = UiTheme.SurfaceBackground };
        context.Controls.Add(CreateSectionTitle("سياق الإعداد", DockStyle.Top));
        context.Controls.Add(new Label { Dock = DockStyle.Top, Height = 28, Text = "الترتيب الفعّال: المستخدم ← الفرع ← الشركة ← النظام ← الافتراضي", TextAlign = ContentAlignment.MiddleRight, ForeColor = UiTheme.SecondaryText });
        var scope = new ComboBox { Dock = DockStyle.Top, DropDownStyle = ComboBoxStyle.DropDownList, RightToLeft = RightToLeft.Yes };
        scope.Items.AddRange(["المستخدم الحالي", "الفرع الحالي", "الشركة الحالية", "إعداد النظام"]);
        scope.SelectedIndex = 0;
        context.Controls.Add(scope);
        context.Controls.Add(new Label { Dock = DockStyle.Top, Height = 26, Text = "نطاق التحرير", TextAlign = ContentAlignment.MiddleRight });

        var overridesHost = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10), BackColor = UiTheme.SurfaceBackground };
        overridesHost.Controls.Add(CreateSectionTitle("إعدادات التشغيل والوراثة", DockStyle.Top));
        var overrideGrid = new TransportDataGrid { AccessibleName = "NearestOverride", Dock = DockStyle.Fill, ReadOnly = true, EmptyStateText = "لا توجد قيم محلية تتجاوز الإعداد الأعلى" };
        foreach (var column in new[] { "المفتاح", "القيمة الفعّالة", "المصدر", "الأولوية" }) overrideGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = column, Name = column, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true });
        overrideGrid.Rows.Add("عملة العرض", "ريال يمني", "إعداد النظام", "4");
        overrideGrid.Rows.Add("حد نتائج البحث", "50", "الفرع الحالي", "2");
        overridesHost.Controls.Add(overrideGrid);
        settingsHost.Panel1.Controls.Add(context);
        settingsHost.Panel2.Controls.Add(overridesHost);
        MainData.Controls.Add(settingsHost);
    }

    private void SetMainDataToFill()
    {
        MainData.AutoSize = false;
        MainData.Dock = DockStyle.Fill;
        _root.RowStyles[2].SizeType = SizeType.Percent;
        _root.RowStyles[2].Height = 100F;
    }

    private void ConfigureGrid(IEnumerable<string> columns, bool readOnly, string emptyState)
    {
        Grid.Columns.Clear();
        Grid.Rows.Clear();
        Grid.ReadOnly = readOnly;
        Grid.EmptyStateText = emptyState;
        foreach (var column in columns)
            Grid.Columns.Add(new DataGridViewTextBoxColumn { Name = column, HeaderText = column, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = readOnly });
    }

    private static TableLayoutPanel CreateFields(IEnumerable<string> captions, bool readOnly)
    {
        var fields = new TableLayoutPanel { AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Dock = DockStyle.Top, ColumnCount = 4, RightToLeft = RightToLeft.Yes, Padding = new Padding(2) };
        for (var column = 0; column < 4; column++) fields.ColumnStyles.Add(new ColumnStyle(column % 2 == 0 ? SizeType.AutoSize : SizeType.Percent, column % 2 == 0 ? 0F : 50F));
        foreach (var (caption, index) in captions.Select((caption, index) => (caption, index)))
        {
            var row = index / 2;
            while (fields.RowCount <= row) { fields.RowCount++; fields.RowStyles.Add(new RowStyle(SizeType.AutoSize)); }
            var column = (index % 2) * 2;
            fields.Controls.Add(new Label { Text = caption, AutoSize = true, Anchor = AnchorStyles.Right, Margin = new Padding(6, 8, 6, 8), TextAlign = ContentAlignment.MiddleRight }, column, row);
            fields.Controls.Add(new TextBox { Dock = DockStyle.Fill, MinimumSize = new Size(150, 0), RightToLeft = RightToLeft.Yes, ReadOnly = readOnly, Margin = new Padding(6) }, column + 1, row);
        }
        return fields;
    }

    private static Label CreateSectionTitle(string text, DockStyle dock) => new() { Text = text, Dock = dock, Height = 32, Font = UiTheme.CreateBoldFont(10F), TextAlign = ContentAlignment.MiddleRight };
    private static Label CreateBadge(string text, Color back, Color fore) => new() { Text = text, AutoSize = true, BackColor = back, ForeColor = fore, Padding = new Padding(10, 7, 10, 7), Margin = new Padding(4) };
    private static Button CreateActionButton(string text) => new() { Text = text, AutoSize = true, Margin = new Padding(4), RightToLeft = RightToLeft.Yes };
    private static Label CreateSummaryLabel(string text, bool attention) => new() { Text = text, AutoSize = true, Margin = new Padding(5), Padding = new Padding(8, 5, 8, 5), BackColor = attention ? Color.FromArgb(254, 242, 242) : Color.FromArgb(241, 245, 249), ForeColor = attention ? Color.FromArgb(153, 27, 27) : UiTheme.HeadingText };
}
