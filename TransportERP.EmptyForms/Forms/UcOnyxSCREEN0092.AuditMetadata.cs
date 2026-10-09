// Audit rollout: shared Designer control with a bounded legacy-source adapter.
namespace TransportERP.EmptyForms;
partial class UcOnyxSCREEN0092
{
    protected override void OnLoad(System.EventArgs e)
    {
        base.OnLoad(e);
        TransportERP.Desktop.CoreUI.AuditMetadataInstaller.Apply(this,
            TransportERP.Desktop.CoreUI.AuditMetadataProfile.Standard);
    }
}
