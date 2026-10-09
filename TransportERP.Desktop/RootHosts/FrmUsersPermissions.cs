extern alias SharedCommands;
using TransportERP.Desktop.CoreUI;
using SharedCommands::TransportERP.Desktop.SharedUI.Commands;

namespace TransportERP.Desktop.Forms.Setup.Security;

/// <summary>Compatibility window for the reusable settings control.</summary>
public sealed partial class FrmUsersPermissions : FrmBase
{
    public FrmUsersPermissions() : this(new UcUsersPermissions())
    {
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    }

    public FrmUsersPermissions(string initialTab) : this(new UcUsersPermissions(initialTab))
    {
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    }

    private FrmUsersPermissions(UcUsersPermissions screen)
    {
        SuspendLayout();
        AutoScaleMode = AutoScaleMode.None;
        Name = nameof(FrmUsersPermissions);
        Text = screen.Text;
        ClientSize = screen.Size;
        MinimumSize = SizeFromClientSize(screen.MinimumSize);
        screen.Dock = DockStyle.Fill;
        screen.CloseRequested += (_, _) => Close();
        FormClosing += (_, e) =>
        {
            if (!screen.ConfirmLeave())
                e.Cancel = true;
        };
        Controls.Add(screen);
        ResumeLayout(true);
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }
}
