using TransportERP.Desktop.Forms.Security;

namespace TransportERP.Desktop.Forms.Security;

public sealed class FrmSec024SecurityAlerts : SecurityScreenForm
{
    public FrmSec024SecurityAlerts()
        : base(new SecurityScreenDefinition(
            "SEC-024",
            "تنبيهات الأمان",
            new[] { "البيانات الرئيسية", "شروط التفعيل", "المستلمون والقنوات", "التصعيد", "سجل العمليات" },
            new[] { "رمز القاعدة", "الاسم", "نوع الحدث", "مستوى الخطورة", "النطاق", "الشرط والعتبة", "نافذة الزمن", "منع التكرار", "المستلمون", "القنوات", "زمن التصعيد", "الإجراء التلقائي المسموح", "الحالة" },
            new[] { "جديد", "حفظ", "تعديل", "اختبار القاعدة", "تفعيل", "إيقاف", "نسخ", "طباعة", "تصدير" },
            new[] { "نوع الحدث", "الخطورة", "الحالة" },
            SecurityScreenLayout.Standard))
    {
    }
}
