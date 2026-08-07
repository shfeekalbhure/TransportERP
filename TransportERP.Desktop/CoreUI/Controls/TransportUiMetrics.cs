namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// المقاسات الموحدة لجميع المكونات المشتركة في واجهات TransportERP.
/// القيم محسوبة تقريبًا على أساس 96 DPI، ويتم تعديلها هنا فقط حتى تنعكس على كل الشاشات.
/// </summary>
internal static class TransportUiMetrics
{
    internal const int Container12Mm = 45;
    internal const int Container10Mm = 38;
    internal const int Control9Mm = 34;
    internal const int Control8Mm = 30;

    // شريط الأوامر مصغر حتى يحافظ على مساحة الجدول.
    internal const int ToolbarHeight = 36;
    internal const int ToolbarButtonHeight = 28;

    // البحث والتصفية حسب القرار: الحاوية 10 مم والحقول 8 مم.
    internal const int SearchPanelHeight = Container10Mm;
    internal const int SearchControlHeight = Control8Mm;

    // التنقل يستخدم نفس مقاس 10/8 مم ويظهر أعلى الشاشة بجانب الإشعارات.
    internal const int PaginationHeight = Container10Mm;
    internal const int PaginationButtonHeight = Control8Mm;

    // معلومات الإنشاء والتعديل أصغر: الحاوية 10 مم والمحتوى 8 مم.
    internal const int AuditPanelHeight = Container10Mm;
    internal const int AuditContentHeight = Control8Mm;

    // الإشعارات تبقى بحاوية 12 مم ومحتوى 9 مم.
    internal const int AlertBarHeight = Container12Mm;
    internal const int AlertContentHeight = Control9Mm;

    // المسافة الرأسية المعتمدة بين صفوف البيانات الرئيسية: 1.5 مم تقريبًا = 6 بكسل.
    internal const int MainDataRowGap = 6;

    // نوزع المسافة بالتساوي أعلى وأسفل الأداة داخل الصف.
    internal const int MainDataVerticalMargin = MainDataRowGap / 2;

    // ارتفاع صف البيانات القياسي = ارتفاع الحقل 8 مم + المسافة المعتمدة.
    internal const int MainDataRowHeight = Control8Mm + MainDataRowGap;

    // مساحة عنوان وحدود الحاوية، خُفّضت حتى لا تستهلك ارتفاعًا غير ضروري.
    internal const int GroupBoxHeaderSpace = 20;

    internal const int SearchGroupHeight = SearchPanelHeight + GroupBoxHeaderSpace;
    internal const int AuditGroupHeight = AuditPanelHeight + GroupBoxHeaderSpace;
    internal const int AlertGroupHeight = AlertBarHeight + GroupBoxHeaderSpace;

    // الصف العلوي يضم الإشعارات والتنقل معًا.
    internal const int TopUtilityRowHeight = AlertGroupHeight;

    // لا توجد مسافات بين الحاويات العامة؛ الفراغات الداخلية فقط تكون صغيرة ومدروسة.
    internal const int SectionGap = 0;
    internal const int CompactPadding = 4;
    internal const int CompactGap = 6;
}
