using TransportERP.Desktop.Forms.Shared;

namespace TransportERP.Desktop.Forms.Setup.General;

/// <summary>GEN-011 — الشركات. شاشة تشغيلية محلية وفق القالب الموحد.</summary>
public sealed class FrmCompanies : SetupDataFormBase
{
    public FrmCompanies() : base("GEN-011", "الشركات", "كود الشركة", "اسم الشركة عربي", "اسم الشركة إنجليزي", "الرقم الضريبي", "العملة الأساسية", "الحالة") { }
}
