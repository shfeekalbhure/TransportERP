using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;

/// <summary>Customer curriculum page 71: report parameters only; printed example is not an input grid.</summary>
public partial class UcCustomerDebtReport : UserControl, IExplicitScreenLayout, IWorkspaceChangeState
{
    public bool HasUnsavedChanges => false; // Report filters are transient, not document edits.
    public event EventHandler? CloseRequested;
    public UcCustomerDebtReport()
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

public class FrmCustomerDebtReport : Form
{
    public FrmCustomerDebtReport()
    {
        var screen = new UcCustomerDebtReport { Dock = DockStyle.Fill };
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
