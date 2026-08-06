namespace TransportERP.Desktop.Forms.Security;

/// <summary>شاشة قوالب الإشعارات الفعلية وفق القالب الموحد؛ تخزن العمليات محلياً لحين ربط API.</summary>
public sealed class FrmNotificationTemplates : SecurityWorkspaceForm
{
    public FrmNotificationTemplates() : base(new SecurityScreenDefinition(
        "FrmNotificationTemplates", "SEC-009", "قوالب الإشعارات", "اسم القالب *", "قناة الإرسال", "موضوع الرسالة"))
    {
    }
}
