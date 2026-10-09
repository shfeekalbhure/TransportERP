

namespace TransportERP.EmptyForms;

/// <summary>Compatibility window hosting UcOnyxSCREEN0104.</summary>
public partial class FrmOnyxSCREEN0104 : System.Windows.Forms.Form
{
    public FrmOnyxSCREEN0104()
    {
        Controls.Add(new UcOnyxSCREEN0104 { Dock = System.Windows.Forms.DockStyle.Fill });
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }
}
