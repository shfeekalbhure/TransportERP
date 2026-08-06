using TransportERP.Desktop.Forms.Security;

namespace TransportERP.Desktop.Forms.Security;

public sealed class FrmSec022LoginLog : SecurityScreenForm
{
    public FrmSec022LoginLog()
        : base(new SecurityScreenDefinition(
            "SEC-022",
            "سجل الدخول",
            new[] { "معايير البحث", "النتائج والتفاصيل", "سجل التصدير" },
            new[] { "من/إلى", "المستخدم أو البريد", "الشركة", "الفرع", "النتيجة", "سبب الفشل", "عنوان IP", "الجهاز", "نظام التشغيل", "المتصفح/العميل", "الموقع التقريبي إن توفر", "معرف الجلسة", "MFA", "وقت الدخول والخروج" },
            new[] { "عرض", "مسح المرشحات", "فتح المستخدم أو الجلسة بصلاحية", "طباعة", "Excel", "PDF" },
            new[] { "النتيجة", "المستخدم", "IP", "الجهاز", "الفترة", "MFA" },
            SecurityScreenLayout.ReadOnly))
    {
    }
}
