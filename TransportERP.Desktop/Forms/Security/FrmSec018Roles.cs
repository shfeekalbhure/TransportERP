using TransportERP.Desktop.Forms.Security;

namespace TransportERP.Desktop.Forms.Security;

public sealed class FrmSec018Roles : SecurityScreenForm
{
    public FrmSec018Roles()
        : base(new SecurityScreenDefinition(
            "SEC-018",
            "الأدوار",
            new[] { "البيانات الرئيسية", "الصلاحيات", "نطاق البيانات الافتراضي", "المستخدمون المرتبطون", "سجل العمليات" },
            new[] { "رمز الدور", "الاسم العربي", "الاسم الإنجليزي", "الوصف", "نوع الدور", "الشركة", "الفروع المسموحة", "مستوى الحساسية", "الدور الأب اختيارياً", "الحالة", "تاريخ السريان والانتهاء", "الملاحظات" },
            new[] { "جديد", "حفظ", "تعديل", "نسخ دور", "إيقاف", "حذف غير المرتبط", "إدارة الصلاحيات", "طباعة", "تصدير" },
            new[] { "الحالة", "الشركة", "نوع الدور" },
            SecurityScreenLayout.Standard))
    {
    }
}
