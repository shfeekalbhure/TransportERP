namespace TransportERP.Desktop.Forms.Security;

/// <summary>شاشة الصلاحيات الفعلية وفق القالب الموحد؛ تخزن العمليات محلياً لحين ربط API.</summary>
public sealed class FrmPermissions : SecurityWorkspaceForm
{
    public FrmPermissions() : base(new SecurityScreenDefinition(
        "FrmPermissions", "SEC-003", "الصلاحيات", "الوحدة/الشاشة *", "الإجراء المسموح", "مستوى الوصول"))
    {
    }
}
