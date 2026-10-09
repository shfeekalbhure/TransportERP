using TransportERP.Desktop.CoreUI;

namespace TransportERP.EmptyForms;

/// <summary>Inventory curriculum pages 23–25 and 29–30. Pictured cost/selling-price main views and download view.</summary>
public partial class UcInventoryIssueOrder : UserControl, IExplicitScreenLayout
{
    public UcInventoryIssueOrder()
    {
        InitializeComponent();
        fieldCostMethod.SelectedIndexChanged += (_, _) => ApplyVisualMode();
        global::TransportERP.ScreenRootProperties.Attach(this);
        foreach (var entry in commandBar.Commands)
        {
            entry.Value.Visible = entry.Key <= StandardCommand.Close;
            entry.Value.Enabled = entry.Key == StandardCommand.Close;
        }
        commandBar.Commands[StandardCommand.Close].Click += (_, _) => CloseRequested?.Invoke(this, EventArgs.Empty);
        Load += (_, _) => AuditMetadataInstaller.Apply(this);
    }

    // The selector changes presentation only; no price or inventory calculation is performed.
    private void ApplyVisualMode()
    {
        var selling = fieldCostMethod.SelectedIndex == 1;
        colPrice.Visible = selling;
        colPriceTotal.Visible = selling;
        lblTotalPrice.Visible = selling;
        txtTotalPrice.Visible = selling;
        lblBurdenAccount.Text = selling ? "حساب الفارق" : "حساب الأعباء";
        fieldBurdenAccount.AccessibleName = lblBurdenAccount.Text;
    }

    public event EventHandler? CloseRequested;
}

public class FrmInventoryIssueOrder : Form
{
    public FrmInventoryIssueOrder()
    {
        var screen = new UcInventoryIssueOrder { Dock = DockStyle.Fill };
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

