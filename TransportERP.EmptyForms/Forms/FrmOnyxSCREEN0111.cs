using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;

/// <summary>Currency exchange main view, general-ledger curriculum pages 55 and 57.</summary>
public partial class UcOnyxSCREEN0111 : UserControl, IExplicitScreenLayout
{
    public event EventHandler? CloseRequested;
    public UcOnyxSCREEN0111()
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
            else if (FindForm() is FrmOnyxSCREEN0111 host) host.Close();
        };
    }
}
