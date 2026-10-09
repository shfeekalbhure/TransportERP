using TransportERP.Desktop.CoreUI;

namespace TransportERP.EmptyForms;

/// <summary>Inventory curriculum pages 22, 40 and 50. Pictured lookup presentation without query or selection services.</summary>
public partial class UcInventoryQuantityLookup : UserControl, IExplicitScreenLayout
{
    public UcInventoryQuantityLookup()
    {
        InitializeComponent();
        global::TransportERP.ScreenRootProperties.Attach(this);
        foreach (var entry in commandBar.Commands)
        {
            entry.Value.Visible = entry.Key == StandardCommand.Close;
            entry.Value.Enabled = entry.Key == StandardCommand.Close;
        }
        commandBar.Commands[StandardCommand.Close].Click += (_, _) => CloseRequested?.Invoke(this, EventArgs.Empty);
        btnCancel.Click += (_, _) => CloseRequested?.Invoke(this, EventArgs.Empty);
        Load += (_, _) => AuditMetadataInstaller.Apply(this);
    }

    public void ShowTransferColumns()
    {
        colBatchNumber.Visible = false;
        Text = lblTitle.Text = "الكميات المتوفرة / Available Qty";
    }

    public event EventHandler? CloseRequested;
}

public class FrmInventoryQuantityLookup : Form
{
    public FrmInventoryQuantityLookup()
    {
        var screen = new UcInventoryQuantityLookup { Dock = DockStyle.Fill };
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

