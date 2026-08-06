using TransportERP.Desktop.Forms.Security;

namespace TransportERP.Desktop.Forms.Security;

public sealed class FrmSec034FailedLoginAttempts : SecurityScreenForm
{
    public FrmSec034FailedLoginAttempts()
        : base(new SecurityScreenDefinition(
            "SEC-034",
            "محاولات الدخول الفاشلة",
            new[] { "المحاولات والحالة", "إدارة الحظر والمعالجة", "الأنماط والمصادر", "سجل العمليات" },
            new[] { "المستخدم/المعرف المدخل", "الشركة", "الفرع", "وقت المحاولة", "سبب الفشل", "IP", "الجهاز", "الموقع", "عدد المحاولات ضمن النافذة", "حالة الحساب", "نوع الحظر", "بداية ونهاية الحظر", "المسؤول", "سبب المعالجة", "المرجع" },
            new[] { "تحديث", "حظر حساب أو IP حسب السياسة", "تمديد الحظر", "رفع الحظر بسبب", "فتح المستخدم", "إنشاء تنبيه", "طباعة", "تصدير" },
            new[] { "الفترة", "المستخدم", "IP", "الجهاز", "السبب", "الحالة" },
            SecurityScreenLayout.ReadOnly))
    {
    }
}
