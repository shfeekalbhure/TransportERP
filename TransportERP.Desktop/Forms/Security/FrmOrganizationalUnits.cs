namespace TransportERP.Desktop.Forms.Security;

/// <summary>شاشة الوحدات التنظيمية الفعلية وفق القالب الموحد؛ تخزن العمليات محلياً لحين ربط API.</summary>
public sealed class FrmOrganizationalUnits : SecurityWorkspaceForm
{
    public FrmOrganizationalUnits() : base(new SecurityScreenDefinition(
        "FrmOrganizationalUnits", "SEC-016", "الوحدات التنظيمية", "اسم الوحدة *", "الوحدة الأم", "مسؤول الوحدة"))
    {
    }
}
