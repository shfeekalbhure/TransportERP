using TransportERP.Desktop.Forms.Security;

namespace TransportERP.Desktop.Forms.Security;

public sealed class FrmSec027AuditLog : SecurityScreenForm
{
    public FrmSec027AuditLog()
        : base(new SecurityScreenDefinition(
            "SEC-027",
            "سجل التدقيق العام",
            new[] { "معايير البحث", "النتائج", "تفاصيل العملية والقيم قبل/بعد", "سجل التصدير" },
            new[] { "معرف العملية", "التاريخ والوقت", "المستخدم", "الدور", "الشركة", "الفرع", "الوحدة", "الشاشة", "الكيان", "معرف السجل", "نوع العملية", "النتيجة", "السبب", "IP", "الجهاز", "معرف الطلب", "القيم قبل/بعد" },
            new[] { "عرض", "مقارنة قبل/بعد", "فتح المرجع بصلاحية", "طباعة", "Excel", "PDF", "حفظ مرشح" },
            new[] { "الفترة", "المستخدم", "الوحدة", "الشاشة", "العملية", "النتيجة", "الكيان" },
            SecurityScreenLayout.ReadOnly))
    {
    }
}
