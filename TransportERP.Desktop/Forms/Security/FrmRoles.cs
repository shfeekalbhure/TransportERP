namespace TransportERP.Desktop.Forms.Security;

/// <summary>شاشة الأدوار الفعلية وفق القالب الموحد؛ تخزن العمليات محلياً لحين ربط API.</summary>
public sealed class FrmRoles : SecurityWorkspaceForm
{
    public FrmRoles() : base(new SecurityScreenDefinition(
        "FrmRoles", "SEC-002", "الأدوار", "اسم الدور *", "نطاق الشركة/الفرع", "وصف الدور"))
    {
    }
}
