namespace TransportERP.Desktop.Forms.Security;

/// <summary>شاشة مفاتيح API والتكامل الفعلية وفق القالب الموحد؛ تخزن العمليات محلياً لحين ربط API.</summary>
public sealed class FrmApiKeysIntegrations : SecurityWorkspaceForm
{
    public FrmApiKeysIntegrations() : base(new SecurityScreenDefinition(
        "FrmApiKeysIntegrations", "SEC-011", "مفاتيح API والتكامل", "اسم التكامل *", "مفتاح الوصول", "نطاق الصلاحية"))
    {
    }
}
