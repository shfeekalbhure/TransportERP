namespace TransportERP.Desktop.Forms.Security;

/// <summary>شاشة الإشعارات الفعلية وفق القالب الموحد؛ تخزن العمليات محلياً لحين ربط API.</summary>
public sealed class FrmNotifications : SecurityWorkspaceForm
{
    public FrmNotifications() : base(new SecurityScreenDefinition(
        "FrmNotifications", "SEC-008", "الإشعارات", "نوع الإشعار *", "المستلم", "قناة الإرسال"))
    {
    }
}
