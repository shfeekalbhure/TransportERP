using TransportERP.Desktop.CoreUI.Controls;

namespace TransportERP.Desktop.CoreUI.Architecture;

/// <summary>
/// Executable base for a reference screen. Shared shell, RTL, layout and common control
/// behavior remain in CoreUI; a reference screen declares only its profile identity.
/// </summary>
public abstract class CoreUiReferenceScreen : UserControl
{
    protected CoreUiReferenceScreen(ReferenceScreenDefinition definition)
    {
        Definition = definition;
        Profile = definition.Profile;
        Title = definition.Title;

        Dock = DockStyle.Fill;
        RightToLeft = RightToLeft.Yes;
        AutoScaleMode = AutoScaleMode.Dpi;
        Frame = new TransportScreenFrame(definition) { Dock = DockStyle.Fill };
        Controls.Add(Frame);
    }

    public TransportScreenProfile Profile { get; }

    public string Title { get; }

    public ReferenceScreenDefinition Definition { get; }

    public TransportScreenFrame Frame { get; }
}
