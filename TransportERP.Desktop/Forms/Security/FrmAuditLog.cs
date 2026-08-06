namespace TransportERP.Desktop.Forms.Security;

/// <summary>شاشة سجل التدقيق العام الفعلية وفق القالب الموحد؛ تخزن العمليات محلياً لحين ربط API.</summary>
public sealed class FrmAuditLog : SecurityWorkspaceForm
{
    public FrmAuditLog() : base(new SecurityScreenDefinition(
        "FrmAuditLog", "SEC-007", "سجل التدقيق العام", "العملية *", "المستخدم", "مرجع السجل"))
    {
    }
}
