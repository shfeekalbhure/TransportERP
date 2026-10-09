

namespace TransportERP.EmptyForms;

/// <summary>Compatibility window hosting UcScreen_03_03_01.</summary>
public partial class FrmScreen_03_03_01 : System.Windows.Forms.Form
{
    public FrmScreen_03_03_01()
    {
        Controls.Add(new UcScreen_03_03_01 { Dock = System.Windows.Forms.DockStyle.Fill });
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }
}
