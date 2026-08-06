namespace TransportERP.Desktop.Forms.Security;

/// <summary>شاشة الجلسات النشطة الفعلية وفق القالب الموحد؛ تخزن العمليات محلياً لحين ربط API.</summary>
public sealed class FrmActiveSessions : SecurityWorkspaceForm
{
    public FrmActiveSessions() : base(new SecurityScreenDefinition(
        "FrmActiveSessions", "SEC-006", "الجلسات النشطة", "المستخدم *", "عنوان IP", "وقت بدء الجلسة"))
    {
    }
}
