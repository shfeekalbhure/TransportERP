using System.ComponentModel;

namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// القالب الموحد لشاشات البيانات المرجعية مثل الدول والمحافظات والمديريات والمدن والمناطق.
/// هذا العنصر يجمع كل الأجزاء الثابتة في مكان واحد بدل تكرارها داخل كل شاشة.
/// الشاشة المستضيفة تضيف فقط حقولها الخاصة داخل DataHost وتحدد أعمدة الجدول.
/// </summary>
[ToolboxItem(true)]
public sealed class TransportReferenceScreenShell : UserControl
{
    // الجدول الرئيسي يقسم الشاشة رأسيًا إلى المناطق الثابتة المعتمدة.
    private readonly TableLayoutPanel _root = new();

    // الأدوات المشتركة التي تظهر بنفس الاسم ونفس الوظيفة في جميع الشاشات.
    public TransportAlertBar AlertBar { get; } = new();
    public TransportToolbar Toolbar { get; } = new();
    public GroupBox DataGroup { get; } = new();
    public Panel DataHost { get; } = new();
    public TransportSearchPanel SearchPanel { get; } = new();
    public TransportDataGrid Grid { get; } = new();
    public TransportPagination Pagination { get; } = new();
    public TransportAuditPanel AuditPanel { get; } = new();

    /// <summary>
    /// إنشاء القالب الموحد مرة واحدة.
    /// بعد وضعه في أي UserControl تصبح جميع الحاويات العامة جاهزة دون تكرار كودها.
    /// </summary>
    public TransportReferenceScreenShell()
    {
        InitializeLayout();
    }

    /// <summary>
    /// عنوان حاوية البيانات الرئيسية، ويمكن تغييره عند الحاجة دون تغيير التصميم.
    /// </summary>
    [Category("TransportERP")]
    [Description("عنوان حاوية البيانات الرئيسية.")]
    [DefaultValue("البيانات الرئيسية")]
    public string DataGroupTitle
    {
        get => DataGroup.Text;
        set => DataGroup.Text = string.IsNullOrWhiteSpace(value) ? "البيانات الرئيسية" : value.Trim();
    }

    /// <summary>
    /// تجهيز ترتيب الشاشة وفق القرار المعتمد:
    /// التنبيهات والأوامر والبيانات والبحث تثبت في الأعلى،
    /// الجدول يأخذ كل المساحة المتبقية Fill،
    /// والتنقل وبيانات الإنشاء والتعديل تثبت في الأسفل.
    /// </summary>
    private void InitializeLayout()
    {
        BackColor = Color.FromArgb(247, 249, 252);
        Dock = DockStyle.Fill;
        Padding = new Padding(16);
        RightToLeft = RightToLeft.Yes;

        _root.ColumnCount = 1;
        _root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _root.Dock = DockStyle.Fill;
        _root.RowCount = 7;
        _root.RightToLeft = RightToLeft.Yes;

        // التنبيه في أعلى الشاشة، ويختفي بالكامل عند عدم وجود رسالة.
        _root.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        // حاوية الأزرار الرئيسية: 12 مم تقريبًا.
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, TransportUiMetrics.Container12Mm));

        // البيانات الرئيسية تبقى أعلى الجدول. ارتفاعها مستقل لأن عدد حقول الشاشة يختلف.
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, 230F));

        // البحث والتصفية: 10 مم تقريبًا.
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, TransportUiMetrics.Container10Mm));

        // الجدول هو الجزء المرن الوحيد ويملأ كل المساحة الوسطية المتبقية.
        _root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        // التنقل أسفل الجدول مباشرة: 10 مم تقريبًا.
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, TransportUiMetrics.Container10Mm));

        // معلومات الإنشاء والتعديل والعدادات في آخر الشاشة: 12 مم تقريبًا.
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, TransportUiMetrics.Container12Mm));

        DataGroup.Dock = DockStyle.Fill;
        DataGroup.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        DataGroup.Padding = new Padding(12);
        DataGroup.RightToLeft = RightToLeft.Yes;
        DataGroup.Text = "البيانات الرئيسية";

        // DataHost هو المكان الوحيد الذي تضيف إليه كل شاشة حقولها الخاصة.
        DataHost.BackColor = Color.White;
        DataHost.Dock = DockStyle.Fill;
        DataHost.Padding = new Padding(4);
        DataHost.RightToLeft = RightToLeft.Yes;
        DataGroup.Controls.Add(DataHost);

        // الجدول Fill حتى يتمدد تلقائيًا مع تكبير وتصغير النافذة.
        Grid.Dock = DockStyle.Fill;

        // الترتيب من أعلى الشاشة إلى أسفلها.
        _root.Controls.Add(AlertBar, 0, 0);
        _root.Controls.Add(Toolbar, 0, 1);
        _root.Controls.Add(DataGroup, 0, 2);
        _root.Controls.Add(SearchPanel, 0, 3);
        _root.Controls.Add(Grid, 0, 4);
        _root.Controls.Add(Pagination, 0, 5);
        _root.Controls.Add(AuditPanel, 0, 6);

        Controls.Add(_root);
    }
}
