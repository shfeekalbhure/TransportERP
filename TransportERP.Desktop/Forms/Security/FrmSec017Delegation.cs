using TransportERP.Desktop.Forms.Security;

namespace TransportERP.Desktop.Forms.Security;

public sealed class FrmSec017Delegation : SecurityScreenForm
{
    public FrmSec017Delegation()
        : base(new SecurityScreenDefinition(
            "SEC-017",
            "تفويض الصلاحيات",
            new[] { "البيانات الرئيسية", "نطاق الصلاحيات", "مدة التفويض والموافقة", "سجل التدقيق" },
            new[] { "رقم التفويض", "المفوِّض", "المفوَّض إليه", "الشركة", "الفرع", "الوحدة التنظيمية", "تاريخ البداية", "تاريخ النهاية", "حالة التفويض", "سبب التفويض", "ملاحظات" },
            new[] { "جديد", "حفظ", "تعديل", "إيقاف أو إلغاء بسبب", "طباعة" },
            new[] { "حالة التفويض" },
            SecurityScreenLayout.Standard))
    {
    }
}
