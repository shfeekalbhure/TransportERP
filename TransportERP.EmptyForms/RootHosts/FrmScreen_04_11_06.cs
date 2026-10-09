

namespace TransportERP.EmptyForms;

/// <summary>Compatibility window hosting UcScreen_04_11_06.</summary>
public partial class FrmScreen_04_11_06 : System.Windows.Forms.Form
{
    public FrmScreen_04_11_06()
    {
        Controls.Add(new UcScreen_04_11_06 { Dock = System.Windows.Forms.DockStyle.Fill });
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }
}
