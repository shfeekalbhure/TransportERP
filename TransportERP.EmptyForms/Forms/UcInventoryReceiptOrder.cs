using TransportERP.Desktop.CoreUI;

namespace TransportERP.EmptyForms;

/// <summary>Inventory curriculum pages 6–20. Receipt order, distinct from authorization and shipping receipt.</summary>
public partial class UcInventoryReceiptOrder : UserControl, IExplicitScreenLayout
{
    public UcInventoryReceiptOrder()
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

    private void JournalView_Click(object? sender, EventArgs e)
    {
        using var journal = new FrmInventoryJournalView();
        journal.ShowDialog(FindForm());
    }

    public event EventHandler? CloseRequested;
}

public class FrmInventoryReceiptOrder : Form
{
    public FrmInventoryReceiptOrder()
    {
        var screen = new UcInventoryReceiptOrder { Dock = DockStyle.Fill };
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

