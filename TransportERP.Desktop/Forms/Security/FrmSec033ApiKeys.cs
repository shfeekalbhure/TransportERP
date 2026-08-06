using TransportERP.Desktop.Forms.Security;

namespace TransportERP.Desktop.Forms.Security;

public sealed class FrmSec033ApiKeys : SecurityScreenForm
{
    public FrmSec033ApiKeys()
        : base(new SecurityScreenDefinition(
            "SEC-033",
            "مفاتيح API والتكامل",
            new[] { "البيانات الرئيسية", "النطاقات والصلاحيات", "القيود والدوران", "سجل الاستخدام", "سجل العمليات" },
            new[] { "معرف المفتاح", "اسم العميل", "المالك", "الشركة", "الفروع", "الصلاحيات", "عناوين IP المسموحة", "معدل الطلبات", "تاريخ الإصدار والانتهاء", "آخر استخدام", "حالة المفتاح", "تاريخ الدوران", "بصمة المفتاح", "الملاحظات" },
            new[] { "إنشاء مفتاح", "عرض السر مرة واحدة", "نسخ آمن", "تدوير", "إيقاف", "إلغاء", "اختبار وصول", "طباعة بيانات غير سرية", "تصدير" },
            new[] { "العميل", "المالك", "الحالة", "الصلاحية", "آخر استخدام" },
            SecurityScreenLayout.Standard))
    {
    }
}
