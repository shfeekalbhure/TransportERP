using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;

/// <summary>Customer beginner manual pp72–75. Calculation does not itself post a journal.</summary>
public partial class UcRepresentativeCommissions : UserControl, IExplicitScreenLayout
{
    public UcRepresentativeCommissions()
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
public class FrmRepresentativeCommissions : Form
{
    public FrmRepresentativeCommissions()
    {
        var screen = new UcRepresentativeCommissions { Dock = DockStyle.Fill };
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

