using TransportERP.Desktop.CoreUI;

namespace TransportERP.EmptyForms;

public partial class FrmScreen_02_05_21 : Form
{
    public FrmScreen_02_05_21()
    {
        var screen = new UcScreen_02_05_21();
        AutoScaleMode = AutoScaleMode.None;
        Text = screen.Text;
        ClientSize = screen.Size;
        screen.Dock = DockStyle.Fill;
        screen.CloseRequested += (_, _) => Close();
        Controls.Add(screen);
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }
}
