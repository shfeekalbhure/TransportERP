

namespace TransportERP.EmptyForms;

/// <summary>Compatibility window hosting the same Designer screen.</summary>
public partial class FrmScreen_04_07_16 : Form
{
    public FrmScreen_04_07_16()
    {
        var screen = new UcScreen_04_07_16();
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
