namespace TransportERP.Desktop.Forms.Security;

/// <summary>شاشة تفويض الصلاحيات المحدود المدة وفق القالب الموحد.</summary>
public sealed class FrmAccessReviews : SecurityWorkspaceForm
{
    public FrmAccessReviews() : base(new SecurityScreenDefinition(
        "FrmAccessDelegations", "SEC-017", "تفويض الصلاحيات", "المفوِّض *", "المفوَّض إليه *", "الدور أو الصلاحية *"))
    {
    }
}
