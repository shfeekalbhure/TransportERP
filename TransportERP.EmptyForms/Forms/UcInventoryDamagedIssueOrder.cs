using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
/// <summary>Ultimate Academy inventory beginner course, PDF pp27-30. UI only until damaged issue service binding exists.</summary>
public partial class UcInventoryDamagedIssueOrder : UserControl, IExplicitScreenLayout
{
    public UcInventoryDamagedIssueOrder()
    {
        InitializeComponent();
        global::TransportERP.ScreenRootProperties.Attach(this);
        foreach(var entry in commandBar.Commands)
        {
            entry.Value.Visible = entry.Key <= StandardCommand.Close;
            entry.Value.Enabled = entry.Key == StandardCommand.Close;
        }
        commandBar.Commands[StandardCommand.Close].Click += (_, _) => CloseRequested?.Invoke(this,EventArgs.Empty);
        Load += (_, _) => AuditMetadataInstaller.Apply(this);
    }
    public event EventHandler? CloseRequested;
}
public class FrmInventoryDamagedIssueOrder : Form
{
    public FrmInventoryDamagedIssueOrder()
    {
        var screen = new UcInventoryDamagedIssueOrder { Dock=DockStyle.Fill };
        Text=screen.Text;
        ClientSize=screen.Size;
        MinimumSize=new Size(1060,660);
        StartPosition=FormStartPosition.CenterParent;
        RightToLeft=RightToLeft.Yes;
        RightToLeftLayout=true;
        screen.CloseRequested += (_,_) => Close();
        Controls.Add(screen);
        global::TransportERP.ScreenRootProperties.Attach(this);
    }
}
