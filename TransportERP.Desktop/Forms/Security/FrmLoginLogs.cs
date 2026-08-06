namespace TransportERP.Desktop.Forms.Security;

/// <summary>شاشة سجلات تسجيل الدخول الفعلية وفق القالب الموحد؛ تخزن العمليات محلياً لحين ربط API.</summary>
public sealed class FrmLoginLogs : SecurityWorkspaceForm
{
    public FrmLoginLogs() : base(new SecurityScreenDefinition(
        "FrmLoginLogs", "SEC-012", "سجلات تسجيل الدخول", "المستخدم *", "عنوان IP", "نتيجة المحاولة"))
    {
    }
}
