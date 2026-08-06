using TransportERP.Desktop.Forms.Security;

namespace TransportERP.Desktop.Forms.Security;

public sealed class FrmSec025SecurityAlertLog : SecurityScreenForm
{
    public FrmSec025SecurityAlertLog()
        : base(new SecurityScreenDefinition(
            "SEC-025",
            "سجل التنبيهات الأمنية",
            new[] { "معايير البحث", "التنبيهات والتفاصيل", "المعالجة والتعليقات", "سجل التصدير" },
            new[] { "رقم التنبيه", "الوقت", "القاعدة", "نوع الحدث", "الخطورة", "الشركة", "الفرع", "المستخدم", "IP", "الجهاز", "الحالة", "المسؤول", "وقت الإقرار", "نتيجة المعالجة", "المرجع", "الملاحظات" },
            new[] { "عرض", "إقرار الاستلام", "إسناد", "إغلاق/إعادة فتح بسبب", "فتح المرجع", "طباعة", "Excel", "PDF" },
            new[] { "الفترة", "الخطورة", "الحالة", "القاعدة", "المستخدم", "الجهاز", "المسؤول" },
            SecurityScreenLayout.ReadOnly))
    {
    }
}
