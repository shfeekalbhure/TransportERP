using TransportERP.Desktop.Forms.Security;

namespace TransportERP.Desktop.Forms.Security;

public sealed class FrmSec026OrganizationalUnits : SecurityScreenForm
{
    public FrmSec026OrganizationalUnits()
        : base(new SecurityScreenDefinition(
            "SEC-026",
            "الوحدات التنظيمية",
            new[] { "البيانات الرئيسية", "الهيكل التنظيمي", "المسؤولون والاتصال", "الصلاحيات والنطاق", "سجل العمليات" },
            new[] { "رمز الوحدة", "الاسم العربي", "الاسم الإنجليزي", "الوحدة الأب", "النوع" },
            new[] { "جديد", "حفظ", "تعديل", "إيقاف", "حذف غير المرتبط", "طباعة", "تصدير" },
            new[] { "الوحدة الأب", "النوع" },
            SecurityScreenLayout.Tree))
    {
    }
}
