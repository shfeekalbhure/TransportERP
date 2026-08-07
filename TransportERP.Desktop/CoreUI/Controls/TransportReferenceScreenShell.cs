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
    private readonly TableLayoutPanel _topUtilityRow = new();

    public GroupBox NotificationGroup { get; } = CreateGroupBox("الإشعارات");
    public GroupBox DataGroup { get; } = CreateGroupBox("البيانات الرئيسية");
    public GroupBox SearchGroup { get; } = CreateGroupBox("البحث والتصفية");
    public GroupBox GridGroup { get; } = CreateGroupBox("قائمة السجلات");
    public GroupBox AuditGroup { get; } = CreateGroupBox("معلومات الإنشاء والتعديل");

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
        _root.RowCount = 6;
        _root.RightToLeft = RightToLeft.Yes;

        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, TransportUiMetrics.TopUtilityRowHeight));
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, TransportUiMetrics.ToolbarHeight));
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, 230F));
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, TransportUiMetrics.SearchGroupHeight));
        _root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, TransportUiMetrics.AuditGroupHeight));

        _topUtilityRow.ColumnCount = 2;
        _topUtilityRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 72F));
        _topUtilityRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));
        _topUtilityRow.Dock = DockStyle.Fill;
        _topUtilityRow.RightToLeft = RightToLeft.Yes;
        _topUtilityRow.RowCount = 1;
        _topUtilityRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        DataHost.BackColor = Color.White;
        DataHost.Dock = DockStyle.Fill;
        DataHost.Padding = new Padding(4);
        DataHost.RightToLeft = RightToLeft.Yes;

        // أي جدول حقول يضاف إلى البيانات الرئيسية يطبق عليه القالب تلقائيًا.
        // الهدف هو توحيد المسافة بين الصفوف إلى 1.5 مم من مكان واحد فقط.
        DataHost.ControlAdded += (_, e) => ApplyMainDataSpacing(e.Control);
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

        _topUtilityRow.Controls.Add(NotificationGroup, 0, 0);
        _topUtilityRow.Controls.Add(Pagination, 1, 0);

        _root.Controls.Add(_topUtilityRow, 0, 0);
        _root.Controls.Add(Toolbar, 0, 1);
        _root.Controls.Add(DataGroup, 0, 2);
        _root.Controls.Add(SearchGroup, 0, 3);
        _root.Controls.Add(GridGroup, 0, 4);
        _root.Controls.Add(AuditGroup, 0, 5);

        Controls.Add(_root);
    }

    /// <summary>
    /// يطبق المسافة الرأسية المعتمدة 1.5 مم تقريبًا بين صفوف البيانات الرئيسية.
    /// يتم استدعاؤه تلقائيًا عند إضافة جدول حقول لأي شاشة إلى DataHost.
    /// </summary>
    private static void ApplyMainDataSpacing(Control control)
    {
        if (control is not TableLayoutPanel table)
        {
            return;
        }

        // الصفوف الثابتة تستخدم ارتفاع الحقل 8 مم + مسافة 1.5 مم.
        // الصف المرن الأخير مثل الملاحظات لا نغيره حتى يملأ المساحة المتبقية.
        for (var row = 0; row < table.RowStyles.Count; row++)
        {
            var style = table.RowStyles[row];
            if (style.SizeType == SizeType.Absolute)
            {
                style.Height = TransportUiMetrics.MainDataRowHeight;
            }
        }

        // توزيع 6 بكسل بالتساوي أعلى وأسفل كل أداة يعطي فراغًا بصريًا ثابتًا بين الصفوف.
        foreach (Control child in table.Controls)
        {
            var current = child.Margin;
            child.Margin = new Padding(
                current.Left,
                TransportUiMetrics.MainDataVerticalMargin,
                current.Right,
                TransportUiMetrics.MainDataVerticalMargin);
        }
    }

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
