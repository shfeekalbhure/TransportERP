using TransportERP.Desktop.CoreUI;

namespace TransportERP.EmptyForms;

/// <summary>Inventory curriculum pages 26, 45, 52, 65 and 67. Read-only pictured journal auxiliary view.</summary>
public partial class UcInventoryJournalView : UserControl, IExplicitScreenLayout
{
    public UcInventoryJournalView()
    {
        InitializeComponent();
        global::TransportERP.ScreenRootProperties.Attach(this);
        foreach (var entry in commandBar.Commands)
        {
            entry.Value.Visible = entry.Key == StandardCommand.Print || entry.Key == StandardCommand.Close;
            entry.Value.Enabled = entry.Key == StandardCommand.Close;
        }
        commandBar.Commands[StandardCommand.Close].Click += (_, _) => CloseRequested?.Invoke(this, EventArgs.Empty);
        Load += (_, _) => AuditMetadataInstaller.Apply(this);
    }

    public event EventHandler? CloseRequested;
}

public class FrmInventoryJournalView : Form
{
    public FrmInventoryJournalView()
    {
        var screen = new UcInventoryJournalView { Dock = DockStyle.Fill };
        Text = screen.Text;
        ClientSize = screen.Size;
        MinimumSize = new Size(1060, 660);
        StartPosition = FormStartPosition.CenterParent;
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        screen.CloseRequested += (_, _) => Close();
        Controls.Add(screen);
        global::TransportERP.ScreenRootProperties.Attach(this);
    }
}

