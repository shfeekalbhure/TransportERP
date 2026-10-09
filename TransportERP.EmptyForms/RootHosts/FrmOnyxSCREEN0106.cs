

namespace TransportERP.EmptyForms;

/// <summary>Compatibility window hosting UcOnyxSCREEN0106.</summary>
public partial class FrmOnyxSCREEN0106 : System.Windows.Forms.Form
{
    public FrmOnyxSCREEN0106()
    {
        Controls.Add(new UcOnyxSCREEN0106 { Dock = System.Windows.Forms.DockStyle.Fill });
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }
}
