

namespace TransportERP.EmptyForms;

/// <summary>Compatibility window hosting UcOnyxSCREEN0111.</summary>
public partial class FrmOnyxSCREEN0111 : System.Windows.Forms.Form
{
    public FrmOnyxSCREEN0111()
    {
        Controls.Add(new UcOnyxSCREEN0111 { Dock = System.Windows.Forms.DockStyle.Fill });
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }
}
