namespace TransportERP.Desktop.Forms.Security;

/// <summary>شاشة المستخدمون الفعلية وفق القالب الموحد؛ تخزن العمليات محلياً لحين ربط API.</summary>
public sealed class FrmUsers : SecurityWorkspaceForm
{
    public FrmUsers() : base(new SecurityScreenDefinition(
        "FrmUsers", "SEC-001", "المستخدمون", "اسم الدخول *", "البريد الإلكتروني", "الدور الافتراضي"))
    {
    }
}
