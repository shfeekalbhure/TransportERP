namespace TransportERP.Desktop.Forms.Security;

/// <summary>شاشة المصادقة الثنائية الفعلية وفق القالب الموحد؛ تخزن العمليات محلياً لحين ربط API.</summary>
public sealed class FrmTwoFactorAuthentication : SecurityWorkspaceForm
{
    public FrmTwoFactorAuthentication() : base(new SecurityScreenDefinition(
        "FrmTwoFactorAuthentication", "SEC-013", "المصادقة الثنائية", "المستخدم *", "طريقة التحقق", "حالة الاعتماد"))
    {
    }
}
