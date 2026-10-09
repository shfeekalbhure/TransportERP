

namespace TransportERP.EmptyForms;

/// <summary>Compatibility window hosting UcOnyxSCREEN0105.</summary>
public partial class FrmOnyxSCREEN0105 : System.Windows.Forms.Form
{
    public FrmOnyxSCREEN0105()
    {
        Controls.Add(new UcOnyxSCREEN0105 { Dock = System.Windows.Forms.DockStyle.Fill });
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }
}
