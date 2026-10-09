using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.SystemSettings.General;

/// <summary>Compatibility window for the reusable settings control.</summary>
public sealed partial class FrmPrintSettings : FrmBase
{
    public FrmPrintSettings() : this(new UcPrintSettings())
    {
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    }

    private FrmPrintSettings(UcPrintSettings screen)
    {
        SuspendLayout();
        AutoScaleMode = AutoScaleMode.None;
        Name = nameof(FrmPrintSettings);
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
