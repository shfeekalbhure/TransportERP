using TransportERP.Desktop.Forms.Security;

namespace TransportERP.Desktop.Forms.Security;

public sealed class FrmSec032SessionsAndTrustedDevices : SecurityScreenForm
{
    public FrmSec032SessionsAndTrustedDevices()
        : base(new SecurityScreenDefinition(
            "SEC-032",
            "إدارة الجلسات النشطة والأجهزة الموثوقة",
            new[] { "الجلسات النشطة", "الأجهزة الموثوقة", "تفاصيل الجلسة والجهاز", "سجل العمليات" },
            new[] { "المستخدم", "الشركة", "الفرع", "معرف الجلسة", "وقت البدء وآخر نشاط", "الانتهاء", "IP", "الجهاز", "نظام التشغيل", "العميل", "الموقع", "MFA", "حالة الجلسة", "حالة الثقة", "تاريخ الثقة", "آخر استخدام", "سبب الإنهاء" },
            new[] { "تحديث", "إنهاء جلسة", "إنهاء جميع جلسات المستخدم", "إلغاء الثقة", "فتح المستخدم", "طباعة", "تصدير" },
            new[] { "الجلسات المنتهية", "MFA", "الثقة", "الفترة" },
            SecurityScreenLayout.ReadOnly))
    {
    }
}
