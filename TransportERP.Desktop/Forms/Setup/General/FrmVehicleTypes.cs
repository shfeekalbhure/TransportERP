using TransportERP.Desktop.Forms.Shared;

namespace TransportERP.Desktop.Forms.Setup.General;

/// <summary>GEN-008 — أنواع المركبات. شاشة تشغيلية محلية وفق القالب الموحد.</summary>
public sealed class FrmVehicleTypes : SetupDataFormBase
{
    public FrmVehicleTypes() : base("GEN-008", "أنواع المركبات", "كود النوع", "اسم النوع عربي", "اسم النوع إنجليزي", "الحمولة الافتراضية", "الحالة", "ملاحظات") { }
}
