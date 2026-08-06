using TransportERP.Desktop.Forms.Security;

namespace TransportERP.Desktop.Forms.Security;

public sealed class FrmSec031GeneralSecuritySettings : SecurityScreenForm
{
    public FrmSec031GeneralSecuritySettings()
        : base(new SecurityScreenDefinition(
            "SEC-031",
            "إعدادات الأمان العامة",
            new[] { "الإعدادات العامة", "الجلسات والأجهزة", "التنبيهات والحماية", "الشبكة والتكامل", "سجل العمليات" },
            new[] { "نطاق الإعداد", "مفاتيح الأمان", "السماح بالتذكر", "مهلة الخمول", "عدد الجلسات", "سياسة الجهاز الموثوق", "قوائم IP", "قيود الموقع", "مستوى السجل", "قنوات التنبيه", "حالة الصيانة الأمنية", "تاريخ السريان" },
            new[] { "حفظ", "اختبار الإعدادات", "استعادة القيم المعتمدة", "تفعيل", "طباعة", "تصدير الإعدادات غير الحساسة" },
            new[] { "نطاق الإعداد" },
            SecurityScreenLayout.Standard))
    {
    }
}
