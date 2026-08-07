namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// المقاسات الموحدة لجميع المكونات المشتركة في واجهات TransportERP.
/// القيم التالية محسوبة تقريبًا على أساس 96 DPI حتى تبقى المقاسات متناسقة بين الشاشات.
/// إذا تغيّر قرار التصميم مستقبلًا نعدّل هذا الملف فقط بدل تعديل كل شاشة على حدة.
/// </summary>
internal static class TransportUiMetrics
{
    internal const int Container12Mm = 45;
    internal const int Container10Mm = 38;
    internal const int Control9Mm = 34;
    internal const int Control8Mm = 30;

    internal const int ToolbarHeight = Container12Mm;
    internal const int ToolbarButtonHeight = Control9Mm;
    internal const int SearchPanelHeight = Container10Mm;
    internal const int SearchControlHeight = Control8Mm;
    internal const int PaginationHeight = Container10Mm;
    internal const int PaginationButtonHeight = Control8Mm;
    internal const int AuditPanelHeight = Container12Mm;
    internal const int AuditContentHeight = Control9Mm;
    internal const int AlertBarHeight = Container12Mm;
    internal const int AlertContentHeight = Control9Mm;

    // مساحة عنوان وحدود GroupBox فوق الحاوية الداخلية.
    internal const int GroupBoxHeaderSpace = 24;

    // الارتفاع الخارجي للحاويات التي أصبحت داخل GroupBox.
    internal const int SearchGroupHeight = SearchPanelHeight + GroupBoxHeaderSpace;
    internal const int AuditGroupHeight = AuditPanelHeight + GroupBoxHeaderSpace;
    internal const int AlertGroupHeight = AlertBarHeight + GroupBoxHeaderSpace;

    internal const int CompactPadding = 4;
    internal const int CompactGap = 6;
}
