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

    // شريط الأوامر أصبح أصغر قليلًا: الحاوية 10 مم والأزرار 8 مم.
    internal const int ToolbarHeight = Container10Mm;
    internal const int ToolbarButtonHeight = Control8Mm;

    // البحث والتصفية يبقى حسب القرار: الحاوية 10 مم والحقول 8 مم.
    internal const int SearchPanelHeight = Container10Mm;
    internal const int SearchControlHeight = Control8Mm;

    // التنقل يستخدم نفس مقاس 10/8 مم، لكنه يعرض في أعلى الشاشة بجانب الإشعارات.
    internal const int PaginationHeight = Container10Mm;
    internal const int PaginationButtonHeight = Control8Mm;

    // معلومات الإنشاء والتعديل أصبحت أصغر: الحاوية 10 مم والمحتوى 8 مم.
    internal const int AuditPanelHeight = Container10Mm;
    internal const int AuditContentHeight = Control8Mm;

    // الإشعارات تبقى بحاوية 12 مم ومحتوى 9 مم.
    internal const int AlertBarHeight = Container12Mm;
    internal const int AlertContentHeight = Control9Mm;

    // مساحة عنوان وحدود GroupBox فوق الحاوية الداخلية.
    internal const int GroupBoxHeaderSpace = 24;

    internal const int SearchGroupHeight = SearchPanelHeight + GroupBoxHeaderSpace;
    internal const int AuditGroupHeight = AuditPanelHeight + GroupBoxHeaderSpace;
    internal const int AlertGroupHeight = AlertBarHeight + GroupBoxHeaderSpace;

    // الصف العلوي يضم الإشعارات والتنقل معًا، ولذلك يأخذ ارتفاع حاوية الإشعارات الخارجية.
    internal const int TopUtilityRowHeight = AlertGroupHeight;

    internal const int CompactPadding = 4;
    internal const int CompactGap = 6;
}
