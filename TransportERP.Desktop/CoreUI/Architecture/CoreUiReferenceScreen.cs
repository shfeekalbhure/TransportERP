using TransportERP.Desktop.CoreUI.Controls;

namespace TransportERP.Desktop.CoreUI.Architecture;

/// <summary>
/// Executable base for a reference screen. Shared shell, RTL, layout and common control
/// behavior remain in CoreUI; a reference screen declares only its profile identity.
/// </summary>
public abstract class CoreUiReferenceScreen : UserControl
{
    protected CoreUiReferenceScreen(TransportScreenProfile profile, string title)
    {
        Profile = profile;
        Title = title;

        Dock = DockStyle.Fill;
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;

        Shell = new TransportReferenceScreenShell
        {
            Dock = DockStyle.Fill,
            DataGroupTitle = title
        };

        Controls.Add(Shell);
    }

    public TransportScreenProfile Profile { get; }

    public string Title { get; }

    public TransportReferenceScreenShell Shell { get; }
}
