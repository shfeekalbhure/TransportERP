using TransportERP.Desktop.Forms.Security;

namespace TransportERP.Desktop.Forms.Security;

public sealed class FrmSec020PermissionCatalog : SecurityScreenForm
{
    public FrmSec020PermissionCatalog()
        : base(new SecurityScreenDefinition(
            "SEC-020",
            "كتالوج الصلاحيات",
            new[] { "البيانات الرئيسية", "الارتباط بالوحدات والشاشات", "الاعتماد والاستخدام", "سجل العمليات" },
            new[] { "مفتاح الصلاحية", "الاسم العربي", "الاسم الإنجليزي", "الوحدة", "الشاشة", "نوع الصلاحية", "مستوى الحساسية", "الوصف", "قابلة للتفويض", "تتطلب موافقة", "الحالة", "الإصدار" },
            new[] { "جديد", "حفظ", "تعديل", "إيقاف", "مزامنة الكتالوج", "فحص الاستخدام", "طباعة", "تصدير" },
            new[] { "النوع", "الحساسية", "الحالة" },
            SecurityScreenLayout.Standard))
    {
    }
}
