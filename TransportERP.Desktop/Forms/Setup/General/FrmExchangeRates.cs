using TransportERP.Desktop.Forms.Shared;

namespace TransportERP.Desktop.Forms.Setup.General;

/// <summary>GEN-010 — أسعار الصرف. شاشة تشغيلية محلية وفق القالب الموحد.</summary>
public sealed class FrmExchangeRates : SetupDataFormBase
{
    public FrmExchangeRates() : base("GEN-010", "أسعار الصرف", "رقم السعر", "العملة", "تاريخ السعر", "سعر الشراء", "سعر البيع", "الحالة") { }
}
