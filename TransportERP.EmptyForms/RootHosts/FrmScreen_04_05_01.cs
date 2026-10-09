using TransportERP.Desktop.CoreUI;

namespace TransportERP.EmptyForms;

public partial class FrmScreen_04_05_01 : Form
{
    public FrmScreen_04_05_01()
    {
        var screen = new UcScreen_04_05_01();
        screen.Dock = DockStyle.Fill;
        Text = screen.Text;
        ClientSize = new Size(1440, 900);
        MinimumSize = new Size(1000, 720);
        StartPosition = FormStartPosition.CenterParent;
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
