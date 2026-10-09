using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.Setup.Security;

// Test host only. The existing Designer file is copied byte-for-byte.
// No original business handlers, runtime layout additions, or service calls are loaded.
public partial class FrmUsersPermissions : FrmBase
{
    public FrmUsersPermissions() => InitializeComponent();

    public void SetAuditFixture(AuditCounterDisplayValues values) => auditFooter.SetDisplayValues(values);
}
