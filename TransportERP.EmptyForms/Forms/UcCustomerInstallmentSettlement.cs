using TransportERP.Desktop.CoreUI;

namespace TransportERP.EmptyForms;

/// <summary>Customer beginner manual, pages 68–70. Allocates an existing credit document to installments.</summary>
public partial class UcCustomerInstallmentSettlement : UserControl, IExplicitScreenLayout
{
    public UcCustomerInstallmentSettlement()
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

    public event EventHandler? CloseRequested;
}

public class FrmCustomerInstallmentSettlement : Form
{
    public FrmCustomerInstallmentSettlement()
    {
        var screen = new UcCustomerInstallmentSettlement { Dock = DockStyle.Fill };
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

