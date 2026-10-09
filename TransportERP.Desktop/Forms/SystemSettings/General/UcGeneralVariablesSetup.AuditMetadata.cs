// Audit rollout: shared Designer control with a bounded legacy-source adapter.
namespace TransportERP.Desktop.Forms.SystemSettings.General;
partial class UcGeneralVariablesSetup
{
    protected override void OnLoad(System.EventArgs e)
    {
        base.OnLoad(e);
        TransportERP.Desktop.CoreUI.AuditMetadataInstaller.Apply(this,
            TransportERP.Desktop.CoreUI.AuditMetadataProfile.ModificationOnly);
    }
}
