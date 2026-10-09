// Audit rollout: shared Designer control with a bounded legacy-source adapter.
namespace TransportERP.Desktop.Forms.Setup.Security;
partial class UcUsersPermissions
{
    protected override void OnLoad(System.EventArgs e)
    {
        base.OnLoad(e);
        TransportERP.Desktop.CoreUI.AuditMetadataInstaller.Apply(this,
            TransportERP.Desktop.CoreUI.AuditMetadataProfile.Standard);
    }
}
