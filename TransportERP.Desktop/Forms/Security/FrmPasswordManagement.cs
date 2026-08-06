namespace TransportERP.Desktop.Forms.Security;

/// <summary>شاشة إدارة كلمات المرور الفعلية وفق القالب الموحد؛ تخزن العمليات محلياً لحين ربط API.</summary>
public sealed class FrmPasswordManagement : SecurityWorkspaceForm
{
    public FrmPasswordManagement() : base(new SecurityScreenDefinition(
        "FrmPasswordManagement", "SEC-010", "إدارة كلمات المرور", "المستخدم *", "إجراء كلمة المرور", "سبب الإجراء"))
    {
    }
}
