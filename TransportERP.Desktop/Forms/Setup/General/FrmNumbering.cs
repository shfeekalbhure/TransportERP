using TransportERP.Desktop.Forms.Shared;

namespace TransportERP.Desktop.Forms.Setup.General;

/// <summary>GEN-014 — الترقيم العام. شاشة تشغيلية محلية وفق القالب الموحد.</summary>
public sealed class FrmNumbering : SetupDataFormBase
{
    public FrmNumbering() : base("GEN-014", "الترقيم العام", "رمز الترقيم", "نوع المستند", "بادئة الرقم", "آخر رقم", "نطاق الترقيم", "الحالة") { }
}
