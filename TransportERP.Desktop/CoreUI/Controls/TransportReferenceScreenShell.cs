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

    // هذه هي الأدوات المشتركة التي ستظهر بنفس الاسم ونفس الوظيفة في جميع الشاشات.
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
    /// تجهيز ترتيب الحاويات الثابت من أعلى الشاشة إلى أسفلها.
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

        // شريط التنبيهات مخفي تلقائيًا، لذلك يأخذ ارتفاعه فقط عند ظهور رسالة.
        _root.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        // شريط الأوامر ثابت الارتفاع حتى تبقى الأزرار بنفس المقاس في كل شاشة.
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, 62F));

        // حاوية البيانات الرئيسية لها ارتفاع قياسي ويمكن زيادته للشاشات الأكبر عند الحاجة.
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, 230F));

        // البحث والتصفية له ارتفاع موحد.
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));

        // الجدول يأخذ كل المساحة المرنة المتبقية.
        _root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        // التصفح ومعلومات التدقيق لهما ارتفاعان ثابتان.
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));

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

        Grid.Dock = DockStyle.Fill;

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
