using TransportERP.Desktop.Forms.Shared;

namespace TransportERP.Desktop.Forms.Setup.General;

/// <summary>GEN-012 — الفروع. شاشة تشغيلية محلية وفق القالب الموحد.</summary>
public sealed class FrmBranches : SetupDataFormBase
{
    public FrmBranches() : base("GEN-012", "الفروع", "كود الفرع", "اسم الفرع عربي", "اسم الفرع إنجليزي", "الشركة", "المدينة", "الحالة") { }
}
