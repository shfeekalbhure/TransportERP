using TransportERP.Desktop.CoreUI;

namespace TransportERP.EmptyForms;

/// <summary>Inventory curriculum page 18. Pictured warehouse receipt authorization main view; separate from shipping receipt order.</summary>
public partial class UcInventoryReceiptAuthorization : UserControl, IExplicitScreenLayout, IWorkspaceChangeState
{
    public UcInventoryReceiptAuthorization()
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
        ConfigureLocalInputs();
    }

    public event EventHandler? CloseRequested;
}

public class FrmInventoryReceiptAuthorization : Form
{
    public FrmInventoryReceiptAuthorization()
    {
        var screen = new UcInventoryReceiptAuthorization { Dock = DockStyle.Fill };
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

