using TransportERP.Desktop.CoreUI;

namespace TransportERP.EmptyForms;

public partial class FrmOnyxSCREEN0081 : Form
{
    public FrmOnyxSCREEN0081()
    {
        var screen = new UcOnyxSCREEN0081();
        AutoScaleMode = AutoScaleMode.None;
        Text = screen.Text;
        ClientSize = screen.Size;
        screen.Dock = DockStyle.Fill;
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
