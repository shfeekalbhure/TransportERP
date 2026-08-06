using TransportERP.Desktop.Forms.Shared;

namespace TransportERP.Desktop.Forms.Setup.General;

/// <summary>GEN-016 — المتغيرات العامة. شاشة تشغيلية محلية وفق القالب الموحد.</summary>
public sealed class FrmGeneralSettings : SetupDataFormBase
{
    public FrmGeneralSettings() : base("GEN-016", "المتغيرات العامة", "رمز المتغير", "اسم المتغير", "القيمة", "نوع القيمة", "الوصف", "الحالة") { }
}
