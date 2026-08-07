namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// المقاسات الموحدة لجميع المكونات المشتركة في واجهات TransportERP.
/// جميع الشاشات تعتمد هذه القيم حتى يبقى التصميم متناسقًا وقابلًا للتعديل من مكان واحد.
/// القيم تقريبية على أساس 96 DPI.
/// </summary>
internal static class TransportUiMetrics
{
    // المقاسات الأساسية بالمليمتر تقريبًا.
    internal const int Container12Mm = 45;
    internal const int Container10Mm = 38;
    internal const int Control9Mm = 34;
    internal const int Control8Mm = 30;

    // هوامش الشاشة والحاويات.
    internal const int ScreenOuterPadding = 6;
    internal const int GroupHorizontalPadding = 6;
    internal const int GroupTopPadding = 4;
    internal const int GroupBottomPadding = 4;
    internal const int SectionGap = 0;

    // شريط الأوامر مصغر حتى يحافظ على مساحة الجدول.
    internal const int ToolbarHeight = 36;
    internal const int ToolbarButtonHeight = 28;
    internal const int ToolbarButtonWidth = 78;
    internal const int ToolbarButtonGap = 4;

    // البيانات الرئيسية: الحقل 8 مم، والمسافة بين الصفوف 1.5 مم تقريبًا.
    internal const int MainDataGroupHeight = 230;
    internal const int MainDataControlHeight = Control8Mm;
    internal const int MainDataRowGap = 6;
    internal const int MainDataVerticalMargin = MainDataRowGap / 2;
    internal const int MainDataHorizontalMargin = 4;
    internal const int MainDataRowHeight = MainDataControlHeight + MainDataRowGap;
    internal const int MainDataMultilineMinHeight = 58;

    // البحث والتصفية: الحاوية 10 مم والحقول 8 مم.
    internal const int SearchPanelHeight = Container10Mm;
    internal const int SearchControlHeight = Control8Mm;

    // التنقل: الحاوية 10 مم والأزرار 8 مم.
    internal const int PaginationHeight = Container10Mm;
    internal const int PaginationButtonHeight = Control8Mm;

    // معلومات الإنشاء والتعديل: الحاوية 10 مم والمحتوى 8 مم.
    internal const int AuditPanelHeight = Container10Mm;
    internal const int AuditContentHeight = Control8Mm;

    // الإشعارات: الحاوية 12 مم والمحتوى 9 مم.
    internal const int AlertBarHeight = Container12Mm;
    internal const int AlertContentHeight = Control9Mm;

    // الجدول: ارتفاعات مؤسسية مضغوطة وواضحة.
    internal const int GridHeaderHeight = 34;
    internal const int GridRowHeight = 32;
    internal const int GridCellHorizontalPadding = 6;

    // عنوان وحدود GroupBox بدون هدر رأسي.
    internal const int GroupBoxHeaderSpace = 20;
    internal const int SearchGroupHeight = SearchPanelHeight + GroupBoxHeaderSpace;
    internal const int AuditGroupHeight = AuditPanelHeight + GroupBoxHeaderSpace;
    internal const int AlertGroupHeight = AlertBarHeight + GroupBoxHeaderSpace;

    // الصف العلوي يضم الإشعارات والتنقل معًا.
    internal const int TopUtilityRowHeight = AlertGroupHeight;

    internal const int CompactPadding = 4;
    internal const int CompactGap = 6;
}
