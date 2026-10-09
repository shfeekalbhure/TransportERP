using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
/// <summary>Ultimate Academy inventory beginner course, PDF pp81-82. UI only until item custody service binding exists.</summary>
public partial class UcInventoryCustodyIssue : UserControl, IExplicitScreenLayout, IWorkspaceChangeState
{
    public UcInventoryCustodyIssue()
    {
        InitializeComponent();
        global::TransportERP.ScreenRootProperties.Attach(this);
        foreach(var entry in commandBar.Commands)
        {
            entry.Value.Visible = entry.Key <= StandardCommand.Close;
            entry.Value.Enabled = entry.Key == StandardCommand.Close;
        }
        commandBar.Commands[StandardCommand.Close].Click += (_, _) => CloseRequested?.Invoke(this,EventArgs.Empty);
        ConfigureLocalInputs();
        Load += (_, _) => AuditMetadataInstaller.Apply(this);
    }
    public event EventHandler? CloseRequested;
}
public class FrmInventoryCustodyIssue : Form
{
    public FrmInventoryCustodyIssue()
    {
        var screen = new UcInventoryCustodyIssue { Dock=DockStyle.Fill };
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
