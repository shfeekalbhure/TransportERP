using System.ComponentModel;

namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// القالب الموحد لشاشات البيانات المرجعية.
/// كل الحاويات العامة تعرف هنا مرة واحدة وتستخدمها جميع الشاشات.
/// </summary>
[ToolboxItem(true)]
public sealed class TransportReferenceScreenShell : UserControl
{
    private readonly TableLayoutPanel _root = new();

    // الحاويات الخارجية الثابتة التي تظهر بعنوان وحدود واضحة.
    public GroupBox NotificationGroup { get; } = CreateGroupBox("الإشعارات");
    public GroupBox DataGroup { get; } = CreateGroupBox("البيانات الرئيسية");
    public GroupBox SearchGroup { get; } = CreateGroupBox("البحث والتصفية");
    public GroupBox GridGroup { get; } = CreateGroupBox("قائمة السجلات");
    public GroupBox AuditGroup { get; } = CreateGroupBox("معلومات الإنشاء والتعديل");

    // الأدوات الداخلية المشتركة.
    public TransportAlertBar AlertBar { get; } = new();
    public TransportToolbar Toolbar { get; } = new();
    public Panel DataHost { get; } = new();
    public TransportSearchPanel SearchPanel { get; } = new();
    public TransportDataGrid Grid { get; } = new();
    public TransportPagination Pagination { get; } = new();
    public TransportAuditPanel AuditPanel { get; } = new();

    public TransportReferenceScreenShell()
    {
        InitializeLayout();
    }

    [Category("TransportERP")]
    [Description("عنوان حاوية البيانات الرئيسية.")]
    [DefaultValue("البيانات الرئيسية")]
    public string DataGroupTitle
    {
        get => DataGroup.Text;
        set => DataGroup.Text = string.IsNullOrWhiteSpace(value) ? "البيانات الرئيسية" : value.Trim();
    }

    private void InitializeLayout()
    {
        BackColor = Color.FromArgb(247, 249, 252);
        Dock = DockStyle.Fill;
        Padding = new Padding(12);
        RightToLeft = RightToLeft.Yes;

        _root.ColumnCount = 1;
        _root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _root.Dock = DockStyle.Fill;
        _root.RowCount = 7;
        _root.RightToLeft = RightToLeft.Yes;

        // الإشعارات داخل GroupBox مستقل.
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, TransportUiMetrics.AlertGroupHeight));

        // شريط الأوامر بدون GroupBox حتى يبقى خفيفًا، لكنه مثبت أعلى الشاشة.
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, TransportUiMetrics.ToolbarHeight));

        // البيانات الرئيسية.
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, 230F));

        // البحث والتصفية داخل GroupBox مستقل.
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, TransportUiMetrics.SearchGroupHeight));

        // الجدول داخل GroupBox ويأخذ كل المساحة المتبقية.
        _root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        // التنقل يبقى في المنتصف أسفل الجدول.
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, TransportUiMetrics.PaginationHeight));

        // معلومات الإنشاء والتعديل داخل GroupBox مستقل.
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, TransportUiMetrics.AuditGroupHeight));

        DataHost.BackColor = Color.White;
        DataHost.Dock = DockStyle.Fill;
        DataHost.Padding = new Padding(4);
        DataHost.RightToLeft = RightToLeft.Yes;
        DataGroup.Controls.Add(DataHost);

        AlertBar.Dock = DockStyle.Fill;
        NotificationGroup.Controls.Add(AlertBar);

        SearchPanel.Dock = DockStyle.Fill;
        SearchGroup.Controls.Add(SearchPanel);

        Grid.Dock = DockStyle.Fill;
        GridGroup.Controls.Add(Grid);

        AuditPanel.Dock = DockStyle.Fill;
        AuditGroup.Controls.Add(AuditPanel);

        Toolbar.Dock = DockStyle.Fill;
        Pagination.Dock = DockStyle.Fill;

        _root.Controls.Add(NotificationGroup, 0, 0);
        _root.Controls.Add(Toolbar, 0, 1);
        _root.Controls.Add(DataGroup, 0, 2);
        _root.Controls.Add(SearchGroup, 0, 3);
        _root.Controls.Add(GridGroup, 0, 4);
        _root.Controls.Add(Pagination, 0, 5);
        _root.Controls.Add(AuditGroup, 0, 6);

        Controls.Add(_root);
    }

    /// <summary>
    /// إنشاء GroupBox موحد حتى تكون جميع حدود وعناوين الحاويات بنفس الشكل.
    /// </summary>
    private static GroupBox CreateGroupBox(string title) => new()
    {
        BackColor = Color.White,
        Dock = DockStyle.Fill,
        Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
        Padding = new Padding(8, 6, 8, 6),
        RightToLeft = RightToLeft.Yes,
        Text = title
    };
}
