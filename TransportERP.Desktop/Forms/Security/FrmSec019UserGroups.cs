using TransportERP.Desktop.Forms.Security;

namespace TransportERP.Desktop.Forms.Security;

public sealed class FrmSec019UserGroups : SecurityScreenForm
{
    public FrmSec019UserGroups()
        : base(new SecurityScreenDefinition(
            "SEC-019",
            "مجموعات المستخدمين",
            new[] { "البيانات الرئيسية", "الأعضاء", "الإشعارات والتوزيع", "سجل العمليات" },
            new[] { "رمز المجموعة", "الاسم العربي", "الاسم الإنجليزي", "الوصف", "الشركة", "نطاق الفروع", "نوع المجموعة", "مالك المجموعة", "الحالة", "تاريخ البداية والنهاية", "الملاحظات" },
            new[] { "جديد", "حفظ", "تعديل", "إضافة/إزالة أعضاء", "إيقاف", "حذف غير المرتبط", "طباعة", "تصدير" },
            new[] { "الشركة", "الحالة", "النوع" },
            SecurityScreenLayout.Standard))
    {
    }
}
