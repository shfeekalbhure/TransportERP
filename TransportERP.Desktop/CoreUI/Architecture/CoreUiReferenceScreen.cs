using TransportERP.Desktop.CoreUI.Controls;

namespace TransportERP.Desktop.CoreUI.Architecture;

/// <summary>
/// Executable base for a reference screen. Shared shell, RTL, layout and common control
/// behavior remain in CoreUI; a reference screen declares only its profile identity.
/// </summary>
public abstract class CoreUiReferenceScreen : UserControl
{
    protected CoreUiReferenceScreen(ScreenDefinition definition)
    {
        Definition = definition;
        Profile = definition.Profile;
        Title = definition.Title;

        Dock = DockStyle.Fill;
        RightToLeft = RightToLeft.Yes;
        AutoScaleMode = AutoScaleMode.Dpi;

        Shell = new TransportReferenceScreenShell
        {
            Dock = DockStyle.Fill,
            DataGroupTitle = title
        };

        Controls.Add(Shell);
    }

    public TransportScreenProfile Profile { get; }
    public ScreenDefinition Definition { get; }

    public string Title { get; }

    public TransportReferenceScreenShell Shell { get; }
}
