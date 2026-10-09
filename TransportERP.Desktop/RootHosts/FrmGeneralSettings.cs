using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.SystemSettings.General;

/// <summary>Compatibility window for the reusable settings control.</summary>
public sealed partial class FrmGeneralSettings : FrmBase
{
    public FrmGeneralSettings() : this(new UcGeneralSettings())
    {
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    }

    private FrmGeneralSettings(UcGeneralSettings screen)
    {
        SuspendLayout();
        AutoScaleMode = AutoScaleMode.None;
        Name = nameof(FrmGeneralSettings);
        Text = screen.Text;
        ClientSize = screen.Size;
        MinimumSize = SizeFromClientSize(screen.MinimumSize);
        screen.Dock = DockStyle.Fill;
        screen.CloseRequested += (_, _) => Close();
        Controls.Add(screen);
        ResumeLayout(true);
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }
}
