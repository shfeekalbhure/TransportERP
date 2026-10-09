using TransportERP.Desktop.CoreUI;

namespace TransportERP.EmptyForms;

public partial class FrmOnyxSCREEN0103 : Form
{
    public FrmOnyxSCREEN0103()
    {
        var screen = new UcOnyxSCREEN0103();
        Text = screen.Text;
        ClientSize = new Size(1100, 720);
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
