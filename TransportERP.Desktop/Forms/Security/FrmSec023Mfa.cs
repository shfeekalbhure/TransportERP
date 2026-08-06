using TransportERP.Desktop.Forms.Security;

namespace TransportERP.Desktop.Forms.Security;

public sealed class FrmSec023Mfa : SecurityScreenForm
{
    public FrmSec023Mfa()
        : base(new SecurityScreenDefinition(
            "SEC-023",
            "المصادقة متعددة العوامل",
            new[] { "الطرق والسياسات", "المستخدمون المسجلون", "الاسترداد والرموز الاحتياطية", "سجل العمليات" },
            new[] { "طريقة المصادقة", "النطاق", "إلزامية/اختيارية", "المستخدم", "حالة التسجيل", "تاريخ التسجيل", "آخر تحقق", "الجهاز", "عدد الرموز الاحتياطية المتبقية", "حالة الاسترداد", "الملاحظات" },
            new[] { "حفظ السياسة", "إلزام التسجيل", "إعادة ضبط MFA", "إلغاء جهاز", "توليد رموز استرداد بآلية آمنة", "طباعة إشعار غير سري", "تصدير محدود" },
            new[] { "التسجيل", "الإلزام", "الفشل" },
            SecurityScreenLayout.Standard))
    {
    }
}
