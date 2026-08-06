using TransportERP.Desktop.Forms.Shared;

namespace TransportERP.Desktop.Forms.Setup.General;

/// <summary>GEN-009 — العملات. شاشة تشغيلية محلية وفق القالب الموحد.</summary>
public sealed class FrmCurrencies : SetupDataFormBase
{
    public FrmCurrencies() : base("GEN-009", "العملات", "رمز العملة", "اسم العملة عربي", "اسم العملة إنجليزي", "رمز ISO", "عدد المنازل العشرية", "الحالة") { }
}
