using TransportERP.Desktop.Forms.Security;

namespace TransportERP.Desktop.Forms.Security;

public sealed class FrmSec029NotificationTemplates : SecurityScreenForm
{
    public FrmSec029NotificationTemplates()
        : base(new SecurityScreenDefinition(
            "SEC-029",
            "قوالب الإشعارات",
            new[] { "البيانات الرئيسية", "محتوى القالب", "المتغيرات", "القنوات واللغات", "الإصدارات والمعاينة", "سجل العمليات" },
            new[] { "رمز القالب", "الاسم", "النوع", "اللغة", "القناة", "عنوان الرسالة", "المحتوى", "المتغيرات المسموحة والإلزامية", "النص البديل", "الحد الأقصى للطول", "الإصدار", "الحالة", "تاريخ السريان" },
            new[] { "جديد", "حفظ", "تعديل", "نسخ إصدار", "معاينة ببيانات اختبار", "اعتماد", "إيقاف", "طباعة", "تصدير" },
            new[] { "النوع", "القناة", "اللغة", "الحالة" },
            SecurityScreenLayout.Standard))
    {
    }
}
