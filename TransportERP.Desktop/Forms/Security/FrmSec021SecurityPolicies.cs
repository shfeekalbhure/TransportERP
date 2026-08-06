using TransportERP.Desktop.Forms.Security;

namespace TransportERP.Desktop.Forms.Security;

public sealed class FrmSec021SecurityPolicies : SecurityScreenForm
{
    public FrmSec021SecurityPolicies()
        : base(new SecurityScreenDefinition(
            "SEC-021",
            "سياسات الأمان",
            new[] { "البيانات الرئيسية", "كلمة المرور والمصادقة", "الدخول والجلسات", "التدقيق والتنبيهات", "سجل العمليات" },
            new[] { "رمز السياسة", "الاسم", "النطاق", "الحد الأدنى للطول والتعقيد", "مدة الصلاحية", "منع إعادة الاستخدام", "عدد المحاولات", "مدة الحظر", "مهلة الجلسة", "الأجهزة المتزامنة", "MFA", "قنوات التنبيه", "الحالة" },
            new[] { "جديد", "حفظ", "تعديل", "محاكاة السياسة", "تفعيل", "إيقاف", "طباعة", "تصدير" },
            new[] { "النطاق", "الحالة" },
            SecurityScreenLayout.Standard))
    {
    }
}
