using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.SystemSettings.General;

/// <summary>Compatibility host for the existing Form navigation factory.</summary>
public sealed partial class FrmTextSetup : FrmBase
{
    public FrmTextSetup(UserControl screen)
    {
        Text = screen.Text;
        ClientSize = screen.Size;
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        screen.Dock = DockStyle.Fill;
        Controls.Add(screen);
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }

    public FrmTextSetup() : this(new UserControl())
    {
    }
}
