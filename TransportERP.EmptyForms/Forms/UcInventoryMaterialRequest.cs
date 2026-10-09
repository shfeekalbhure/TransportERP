using TransportERP.Desktop.CoreUI;

namespace TransportERP.EmptyForms;

/// <summary>Inventory curriculum pages 46–48. Pictured material request main and download views only.</summary>
public partial class UcInventoryMaterialRequest : UserControl, IExplicitScreenLayout
{
    public UcInventoryMaterialRequest()
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

public class FrmInventoryMaterialRequest : Form
{
    public FrmInventoryMaterialRequest()
    {
        var screen = new UcInventoryMaterialRequest { Dock = DockStyle.Fill };
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

