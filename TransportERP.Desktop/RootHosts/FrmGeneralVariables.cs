using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.SystemSettings.General;

public sealed partial class FrmGeneralVariables : FrmBase
{
    public FrmGeneralVariables()
    {
        var screen = new UcGeneralVariables();
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
}
