

namespace TransportERP.EmptyForms;

/// <summary>Compatibility window hosting UcOnyxSCREEN0110.</summary>
public partial class FrmOnyxSCREEN0110 : System.Windows.Forms.Form
{
    public FrmOnyxSCREEN0110()
    {
        Controls.Add(new UcOnyxSCREEN0110 { Dock = System.Windows.Forms.DockStyle.Fill });
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }
}
