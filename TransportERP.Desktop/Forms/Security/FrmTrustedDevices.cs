namespace TransportERP.Desktop.Forms.Security;

/// <summary>شاشة الأجهزة الموثوقة الفعلية وفق القالب الموحد؛ تخزن العمليات محلياً لحين ربط API.</summary>
public sealed class FrmTrustedDevices : SecurityWorkspaceForm
{
    public FrmTrustedDevices() : base(new SecurityScreenDefinition(
        "FrmTrustedDevices", "SEC-005", "الأجهزة الموثوقة", "معرف الجهاز *", "المستخدم المرتبط", "تاريخ الاعتماد"))
    {
    }
}
