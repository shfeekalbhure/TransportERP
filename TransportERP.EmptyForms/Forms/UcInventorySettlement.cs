using TransportERP.Desktop.CoreUI;

namespace TransportERP.EmptyForms;

/// <summary>Inventory curriculum pages 56, 60, 63, 64 and 66. Quantity/cost presentations and pictured download view.</summary>
public partial class UcInventorySettlement : UserControl, IExplicitScreenLayout
{
    public UcInventorySettlement()
    {
        InitializeComponent();
        radioQuantity.CheckedChanged += (_, _) => ApplyVisualMode();
        radioCost.CheckedChanged += (_, _) => ApplyVisualMode();
        global::TransportERP.ScreenRootProperties.Attach(this);
        foreach (var entry in commandBar.Commands)
        {
            entry.Value.Visible = entry.Key <= StandardCommand.Close;
            entry.Value.Enabled = entry.Key == StandardCommand.Close;
        }
        commandBar.Commands[StandardCommand.Close].Click += (_, _) => CloseRequested?.Invoke(this, EventArgs.Empty);
        Load += (_, _) => AuditMetadataInstaller.Apply(this);
    }

    // Presentation only: switching these modes never calculates or changes stock.
    private void ApplyVisualMode()
    {
        colQuantity.Visible = !radioCost.Checked;
        colNewCost.Visible = radioCost.Checked;
        colDifference.Visible = radioCost.Checked;
        lblItemCost.Visible = radioCost.Checked;
        fieldItemCost.Visible = radioCost.Checked;
    }

    public event EventHandler? CloseRequested;
}

public class FrmInventorySettlement : Form
{
    public FrmInventorySettlement()
    {
        var screen = new UcInventorySettlement { Dock = DockStyle.Fill };
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

