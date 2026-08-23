namespace TransportERP.Desktop.CoreUI;

/// <summary>DEC-014 shared contextual alert host. Alert policy and screen-specific messages are not implemented in W0.</summary>
public sealed class ContextAlertHost : FlowLayoutPanel
{
    public ContextAlertHost()
    {
        Name = "ContextAlertHost";
        Dock = DockStyle.None;
        AutoSize = true;
        FlowDirection = FlowDirection.RightToLeft;
        RightToLeft = RightToLeft.Yes;
        WrapContents = false;
        Visible = false;
        Anchor = AnchorStyles.Left | AnchorStyles.Top;
    }
}
