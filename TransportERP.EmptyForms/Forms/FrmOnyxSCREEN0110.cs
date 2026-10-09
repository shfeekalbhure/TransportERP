using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;

/// <summary>Currency exchange request main view, general-ledger curriculum page 92.</summary>
public partial class UcOnyxSCREEN0110 : UserControl, IExplicitScreenLayout
{
    public event EventHandler? CloseRequested;
    public UcOnyxSCREEN0110()
    {
        InitializeComponent();
        global::TransportERP.ScreenRootProperties.Attach(this);
        foreach (var entry in commandBar.Commands)
        {
            entry.Value.Visible = entry.Key <= StandardCommand.Close;
            entry.Value.Enabled = entry.Key == StandardCommand.Close;
        }
        commandBar.Commands[StandardCommand.Close].Click += (_, _) =>
        {
            if (CloseRequested is { } close) close(this, EventArgs.Empty);
            else if (FindForm() is FrmOnyxSCREEN0110 host) host.Close();
        };
    }
}
