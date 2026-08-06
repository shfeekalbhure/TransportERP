namespace TransportERP.Desktop.Forms.Security;

/// <summary>شاشة إعدادات الأمان العامة الفعلية وفق القالب الموحد؛ تخزن العمليات محلياً لحين ربط API.</summary>
public sealed class FrmGeneralSecuritySettings : SecurityWorkspaceForm
{
    public FrmGeneralSecuritySettings() : base(new SecurityScreenDefinition(
        "FrmGeneralSecuritySettings", "SEC-014", "إعدادات الأمان العامة", "اسم الإعداد *", "القيمة", "تصنيف الإعداد"))
    {
    }
}
