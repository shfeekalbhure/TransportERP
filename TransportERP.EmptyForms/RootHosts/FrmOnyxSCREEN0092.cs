

namespace TransportERP.EmptyForms;

/// <summary>Compatibility window hosting UcOnyxSCREEN0092.</summary>
public partial class FrmOnyxSCREEN0092 : System.Windows.Forms.Form
{
    public FrmOnyxSCREEN0092()
    {
        Controls.Add(new UcOnyxSCREEN0092 { Dock = System.Windows.Forms.DockStyle.Fill });
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }
}
