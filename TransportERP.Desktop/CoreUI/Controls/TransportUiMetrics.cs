namespace TransportERP.Desktop.CoreUI.Controls;

/// <summary>
/// المقاسات الموحدة لجميع المكونات المشتركة في واجهات TransportERP.
/// القيم التالية محسوبة تقريبًا على أساس 96 DPI حتى تبقى المقاسات متناسقة بين الشاشات.
/// إذا تغيّر قرار التصميم مستقبلًا نعدّل هذا الملف فقط بدل تعديل كل شاشة على حدة.
/// </summary>
internal static class TransportUiMetrics
{
    // 12 مم تقريبًا = 45 بكسل: ارتفاع حاوية الأزرار والتنبيهات وبيانات التدقيق.
    internal const int Container12Mm = 45;

    // 10 مم تقريبًا = 38 بكسل: ارتفاع حاوية البحث والتصفية وحاوية التنقل.
    internal const int Container10Mm = 38;

    // 9 مم تقريبًا = 34 بكسل: ارتفاع الأزرار والمحتوى داخل الحاويات ذات ارتفاع 12 مم.
    internal const int Control9Mm = 34;

    // 8 مم تقريبًا = 30 بكسل: ارتفاع الحقول وأزرار التنقل داخل الحاويات ذات ارتفاع 10 مم.
    internal const int Control8Mm = 30;

    // أسماء دلالية تستخدمها المكونات حتى يكون معنى الرقم واضحًا داخل الكود.
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

    // المسافة الداخلية القياسية الصغيرة بين حدود الحاوية والعناصر.
    internal const int CompactPadding = 4;

    // المسافة الأفقية القياسية بين العناصر المتجاورة.
    internal const int CompactGap = 6;
}
