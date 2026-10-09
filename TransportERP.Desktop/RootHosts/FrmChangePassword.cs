using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.Setup.Security;

/// <summary>Dialog host retained for existing password navigation.</summary>
public sealed partial class FrmChangePassword : FrmBase
{
    public FrmChangePassword()
    {
        var screen = new UcChangePassword();
        AutoScaleMode = AutoScaleMode.None;
        Name = nameof(FrmChangePassword);
        Text = screen.Text;
        Tag = screen.Tag;
        ClientSize = screen.Size;
        MinimumSize = SizeFromClientSize(screen.MinimumSize);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = MinimizeBox = false;
        screen.Dock = DockStyle.Fill;
        screen.CloseRequested += (_, _) => Close();
        FormClosing += (_, e) =>
        {
            if (!e.Cancel)
                e.Cancel = !screen.ConfirmLeave();
        };
        Controls.Add(screen);
        CancelButton = screen.CloseButton;
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }
}
