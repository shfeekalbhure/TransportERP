using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.CoreUI.Profiles;

namespace TransportERP.Desktop.Forms.Setup.General;

/// <summary>GEN-009 — تعريف العملة فقط؛ أسعار الصرف وسياساتها وAssignments خارج ملكية الشاشة.</summary>
public partial class UcGen009Currencies : TransportScreenBase
{
    public UcGen009Currencies()
    {
        InitializeComponent();
        TransportScreenProfilePolicy.Apply(this, profileMetadata);
    }

    internal TransportReferenceScreenShell ScreenShell => screenShell;
}
