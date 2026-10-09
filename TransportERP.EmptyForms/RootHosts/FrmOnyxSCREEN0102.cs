using TransportERP.Desktop.CoreUI;

namespace TransportERP.EmptyForms;

public partial class FrmOnyxSCREEN0102 : Form
{
    public FrmOnyxSCREEN0102()
    {
        var screen = new UcOnyxSCREEN0102
        {
            Dock = DockStyle.Fill
        };
        Text = screen.Text;
        ClientSize = screen.Size;
        MinimumSize = new Size(1060, 660);
        StartPosition = FormStartPosition.CenterParent;
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        screen.CloseRequested += (_, _) => Close();
        Controls.Add(screen);
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }
}
