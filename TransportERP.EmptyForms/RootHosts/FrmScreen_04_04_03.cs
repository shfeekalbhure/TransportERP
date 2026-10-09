using TransportERP.Desktop.CoreUI;

namespace TransportERP.EmptyForms;

public partial class FrmScreen_04_04_03 : Form
{
    public FrmScreen_04_04_03()
    {
        var screen = new UcScreen_04_04_03();
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
