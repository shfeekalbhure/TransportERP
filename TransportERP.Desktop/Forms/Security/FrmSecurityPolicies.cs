namespace TransportERP.Desktop.Forms.Security;

/// <summary>شاشة سياسات الأمان الفعلية وفق القالب الموحد؛ تخزن العمليات محلياً لحين ربط API.</summary>
public sealed class FrmSecurityPolicies : SecurityWorkspaceForm
{
    public FrmSecurityPolicies() : base(new SecurityScreenDefinition(
        "FrmSecurityPolicies", "SEC-004", "سياسات الأمان", "اسم السياسة *", "مدة الجلسة", "قواعد كلمة المرور"))
    {
    }
}
