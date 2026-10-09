using TransportERP.Desktop.CoreUI;

namespace TransportERP.EmptyForms;

public partial class FrmOnyxSCREEN0085 : Form
{
    public FrmOnyxSCREEN0085()
    {
        var screen = new UcOnyxSCREEN0085();
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
