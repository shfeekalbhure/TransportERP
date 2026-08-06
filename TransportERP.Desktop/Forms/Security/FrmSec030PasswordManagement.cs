using TransportERP.Desktop.Forms.Security;

namespace TransportERP.Desktop.Forms.Security;

public sealed class FrmSec030PasswordManagement : SecurityScreenForm
{
    public FrmSec030PasswordManagement()
        : base(new SecurityScreenDefinition(
            "SEC-030",
            "إدارة كلمات المرور",
            new[] { "طلبات إعادة التعيين", "المستخدم والحالة", "الموافقة والتنفيذ", "سجل العمليات" },
            new[] { "رقم الطلب", "المستخدم", "الشركة", "الفرع", "سبب الطلب", "المصدر", "الحالة", "تاريخ الطلب", "تاريخ الانتهاء", "المعتمد", "قناة التسليم", "عدد المحاولات", "حالة القفل", "يتطلب تغييراً عند الدخول", "الملاحظات" },
            new[] { "إنشاء طلب", "إرسال للموافقة", "موافقة", "رفض بسبب", "إصدار رابط/رمز مؤقت", "إلغاء", "فتح المستخدم", "طباعة إشعار", "تصدير محدود" },
            new[] { "الحالة", "الفترة", "الشركة", "القفل", "الانتهاء", "المصدر" },
            SecurityScreenLayout.Standard))
    {
    }
}
