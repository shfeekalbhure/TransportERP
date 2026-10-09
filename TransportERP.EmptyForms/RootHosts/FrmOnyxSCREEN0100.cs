using TransportERP.Desktop.CoreUI;

namespace TransportERP.EmptyForms;

public partial class FrmOnyxSCREEN0100 : Form
{
    public FrmOnyxSCREEN0100()
    {
        var screen = new UcOnyxSCREEN0100();
        Text = screen.Text;
        ClientSize = new Size(1440, 940);
        MinimumSize = new Size(1000, 720);
        screen.CloseRequested += (_, _) => Close();
        FormClosing += (_, e) =>
        {
            if (!screen.Binding.ConfirmLeave())
                e.Cancel = true;
        };
        Controls.Add(screen);
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }
}
