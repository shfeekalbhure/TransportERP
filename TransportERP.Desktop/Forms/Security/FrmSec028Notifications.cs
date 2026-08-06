using TransportERP.Desktop.Forms.Security;

namespace TransportERP.Desktop.Forms.Security;

public sealed class FrmSec028Notifications : SecurityScreenForm
{
    public FrmSec028Notifications()
        : base(new SecurityScreenDefinition(
            "SEC-028",
            "الإشعارات",
            new[] { "البيانات الرئيسية", "المستلمون", "قنوات الإرسال", "المعاينة والجدولة", "سجل الإرسال والقراءة", "سجل العمليات" },
            new[] { "رقم الإشعار", "العنوان", "النص", "القالب", "نوع الإشعار", "الأولوية", "المستلمون", "الشركة", "الفرع", "القنوات", "تاريخ الجدولة والانتهاء", "الرابط المرجعي", "يتطلب إقراراً", "الحالة", "المرفقات المسموحة" },
            new[] { "جديد", "حفظ كمسودة", "معاينة", "جدولة", "إرسال", "إلغاء المجدول", "إعادة إرسال للفاشل", "طباعة", "تصدير" },
            new[] { "القالب", "الحالة", "المستلم", "الفترة" },
            SecurityScreenLayout.Standard))
    {
    }
}
