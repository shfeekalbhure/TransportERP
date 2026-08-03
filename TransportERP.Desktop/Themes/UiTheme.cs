namespace TransportERP.Desktop.Themes;

/// <summary>
/// يحتوي الألوان والخطوط الأساسية المعتمدة لهوية واجهات TransportERP.
/// يُستخدم هذا الملف لتوحيد المظهر ومنع تكرار قيم الألوان داخل كل شاشة.
/// </summary>
internal static class UiTheme
{
    /// <summary>
    /// اللون الأزرق الرئيسي المستخدم في الأزرار والعناصر النشطة.
    /// </summary>
    internal static Color PrimaryBlue => Color.FromArgb(35, 111, 229);

    /// <summary>
    /// اللون الأزرق الداكن المستخدم عند مرور مؤشر الفأرة.
    /// </summary>
    internal static Color PrimaryBlueHover => Color.FromArgb(24, 88, 197);

    /// <summary>
    /// لون خلفية النوافذ الرئيسية.
    /// </summary>
    internal static Color WindowBackground => Color.FromArgb(239, 245, 252);

    /// <summary>
    /// لون العناوين الرئيسية والنصوص المهمة.
    /// </summary>
    internal static Color HeadingText => Color.FromArgb(17, 43, 78);

    /// <summary>
    /// لون النصوص الثانوية والتوضيحية.
    /// </summary>
    internal static Color SecondaryText => Color.FromArgb(91, 111, 139);

    /// <summary>
    /// لون بداية التدرج الخاص بلوحة هوية النظام.
    /// </summary>
    internal static Color BrandGradientStart => Color.FromArgb(17, 58, 140);

    /// <summary>
    /// لون نهاية التدرج الخاص بلوحة هوية النظام.
    /// </summary>
    internal static Color BrandGradientEnd => Color.FromArgb(38, 132, 232);

    /// <summary>
    /// لون تمييز حقل الإدخال النشط.
    /// </summary>
    internal static Color FocusedInputBackground => Color.FromArgb(245, 249, 255);

    /// <summary>
    /// إنشاء الخط الافتراضي للنصوص العادية.
    /// </summary>
    /// <param name="size">حجم الخط المطلوب.</param>
    internal static Font CreateRegularFont(float size) => new("Segoe UI", size, FontStyle.Regular);

    /// <summary>
    /// إنشاء الخط الافتراضي للنصوص البارزة والعناوين.
    /// </summary>
    /// <param name="size">حجم الخط المطلوب.</param>
    internal static Font CreateBoldFont(float size) => new("Segoe UI", size, FontStyle.Bold);
}
