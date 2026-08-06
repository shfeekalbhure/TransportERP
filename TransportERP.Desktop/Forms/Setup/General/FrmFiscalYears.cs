using TransportERP.Desktop.Forms.Shared;

namespace TransportERP.Desktop.Forms.Setup.General;

/// <summary>GEN-013 — السنوات المالية. شاشة تشغيلية محلية وفق القالب الموحد.</summary>
public sealed class FrmFiscalYears : SetupDataFormBase
{
    public FrmFiscalYears() : base("GEN-013", "السنوات المالية", "كود السنة", "اسم السنة المالية", "تاريخ البداية", "تاريخ النهاية", "حالة السنة", "ملاحظات") { }
}
