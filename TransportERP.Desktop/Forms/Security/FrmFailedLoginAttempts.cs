namespace TransportERP.Desktop.Forms.Security;

/// <summary>شاشة محاولات الدخول الفاشلة الفعلية وفق القالب الموحد؛ تخزن العمليات محلياً لحين ربط API.</summary>
public sealed class FrmFailedLoginAttempts : SecurityWorkspaceForm
{
    public FrmFailedLoginAttempts() : base(new SecurityScreenDefinition(
        "FrmFailedLoginAttempts", "SEC-015", "محاولات الدخول الفاشلة", "اسم الدخول *", "عنوان IP", "وقت المحاولة"))
    {
    }
}
