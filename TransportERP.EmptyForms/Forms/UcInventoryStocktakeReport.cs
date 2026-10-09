using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;

/// <summary>Inventory curriculum page 62: pictured report parameters. Printed stocktake output is not an input grid.</summary>
public partial class UcInventoryStocktakeReport : UserControl, IExplicitScreenLayout, IWorkspaceChangeState
{
    public bool HasUnsavedChanges => false; // Report filters are transient, not document edits.
    public event EventHandler? CloseRequested;
    public UcInventoryStocktakeReport()
    {
        InitializeComponent();
        global::TransportERP.ScreenRootProperties.Attach(this);
        foreach (var entry in commandBar.Commands)
        {
            entry.Value.Visible = entry.Key <= StandardCommand.Close;
            entry.Value.Enabled = entry.Key == StandardCommand.Close;
        }
        commandBar.Commands[StandardCommand.Close].Click += (_, _) => CloseRequested?.Invoke(this, EventArgs.Empty);
        Load += (_, _) => AuditMetadataInstaller.Apply(this);
    }
}

public class FrmInventoryStocktakeReport : Form
{
    public FrmInventoryStocktakeReport()
    {
        var screen = new UcInventoryStocktakeReport { Dock = DockStyle.Fill };
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
