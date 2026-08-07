namespace TransportERP.Desktop.Themes;

/// <summary>
/// الهوية البصرية المركزية لنظام TransportERP.
/// جميع الألوان والخطوط المشتركة تعرف هنا حتى لا تتكرر قيم الألوان داخل الشاشات.
/// </summary>
internal static class UiTheme
{
    // الأزرق: الإجراءات العامة مثل جديد وطباعة.
    internal static Color PrimaryBlue => Color.FromArgb(35, 111, 229);
    internal static Color PrimaryBlueHover => Color.FromArgb(24, 88, 197);

    // الأخضر: الحفظ؛ يدل على إتمام العملية بأمان.
    internal static Color ActionSave => Color.FromArgb(22, 145, 90);
    internal static Color ActionSaveHover => Color.FromArgb(17, 120, 74);

    // الكهرماني: التعديل؛ يلفت الانتباه إلى تغيير بيانات موجودة.
    internal static Color ActionEdit => Color.FromArgb(202, 138, 4);
    internal static Color ActionEditHover => Color.FromArgb(169, 111, 2);

    // البرتقالي الداكن: الإيقاف؛ عملية حساسة لكنها ليست حذفًا نهائيًا.
    internal static Color ActionDisable => Color.FromArgb(217, 100, 18);
    internal static Color ActionDisableHover => Color.FromArgb(180, 78, 14);

    // الأحمر: الحذف فقط، لأنه الإجراء الأعلى خطورة في الشريط.
    internal static Color ActionDelete => Color.FromArgb(200, 55, 55);
    internal static Color ActionDeleteHover => Color.FromArgb(168, 42, 42);

    // الرمادي الداكن: الإغلاق، لأنه إجراء تنقلي وليس تعديل بيانات.
    internal static Color ActionClose => Color.FromArgb(71, 85, 105);
    internal static Color ActionCloseHover => Color.FromArgb(51, 65, 85);

    // حالات الأزرار المشتركة. Disabled لا يعتمد على SystemColors حتى لا يختلف حسب الجهاز/الثيم.
    internal static Color ActionText => Color.White;
    internal static Color ActionDisabledBackground => Color.FromArgb(203, 213, 225);
    internal static Color ActionDisabledText => Color.FromArgb(51, 65, 85);

    // الأسطح والحاويات: معرفة مركزيًا حتى لا تمتلك كل وحدة Theme فرعيًا خاصًا بها.
    internal static Color WindowBackground => Color.FromArgb(239, 245, 252);
    internal static Color WorkspaceBackground => Color.FromArgb(247, 249, 252);
    internal static Color SurfaceBackground => Color.White;
    internal static Color GroupBorder => Color.FromArgb(214, 222, 233); // #D6DEE9
    internal static Color GroupText => Color.FromArgb(45, 55, 72);
    internal static Color ControlBorder => Color.FromArgb(203, 213, 225);

    internal static Color HeadingText => Color.FromArgb(17, 43, 78);
    internal static Color SecondaryText => Color.FromArgb(91, 111, 139);
    internal static Color BrandGradientStart => Color.FromArgb(17, 58, 140);
    internal static Color BrandGradientEnd => Color.FromArgb(38, 132, 232);
    internal static Color FocusedInputBackground => Color.FromArgb(245, 249, 255);

    internal static Font CreateRegularFont(float size) => new("Segoe UI", size, FontStyle.Regular);
    internal static Font CreateBoldFont(float size) => new("Segoe UI", size, FontStyle.Bold);
}
