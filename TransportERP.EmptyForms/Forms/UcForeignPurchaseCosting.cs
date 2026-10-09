using TransportERP.Desktop.CoreUI;

namespace TransportERP.EmptyForms;

/// <summary>Supplier curriculum page 54. Visual costing view; no calculation or posting behavior.</summary>
public partial class UcForeignPurchaseCosting : UserControl, IExplicitScreenLayout, IWorkspaceChangeState
{
    public UcForeignPurchaseCosting()
    {
        InitializeComponent();
        ConfigureInputChoices();
        global::TransportERP.ScreenRootProperties.Attach(this);
        foreach (var entry in commandBar.Commands)
        {
            entry.Value.Visible = entry.Key <= StandardCommand.Close;
            entry.Value.Enabled = entry.Key == StandardCommand.Close;
        }
        commandBar.Commands[StandardCommand.Close].Click += (_, _) => CloseRequested?.Invoke(this, EventArgs.Empty);
        Load += (_, _) => AuditMetadataInstaller.Apply(this);
    }

    public event EventHandler? CloseRequested;
}

public class FrmForeignPurchaseCosting : Form
{
    public FrmForeignPurchaseCosting()
    {
        var screen = new UcForeignPurchaseCosting { Dock = DockStyle.Fill };
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

