namespace TransportERP.Desktop;

public sealed partial class FrmDashboard : CoreUI.FrmBase
{
    private bool sidebarCollapsed;
    private bool alertsCollapsed;

    public FrmDashboard()
    {
        InitializeComponent();
    }

    private void ToggleRightSidebar(params Control[] controls)
    {
        sidebarCollapsed = !sidebarCollapsed;
        shell.ColumnStyles[0].Width = sidebarCollapsed ? CollapsedWidth : SidebarWidth;
        rightSidebar.Padding = sidebarCollapsed ? new Padding(8, 16, 8, 22) : new Padding(16, 16, 16, 22);
        foreach (var control in controls.Take(controls.Length - 1))
        {
            control.Visible = !sidebarCollapsed;
        }
        if (controls.LastOrDefault() is Button button)
        {
            button.Text = sidebarCollapsed ? "فتح" : "طي";
        }
    }

    private void ToggleAlerts(Label title, Control[] rows, Button button)
    {
        alertsCollapsed = !alertsCollapsed;
        shell.ColumnStyles[2].Width = alertsCollapsed ? CollapsedWidth : AlertsWidth;
        leftAlerts.Padding = alertsCollapsed ? new Padding(8, 16, 8, 22) : new Padding(14, 16, 14, 22);
        title.Visible = !alertsCollapsed;
        foreach (var row in rows)
        {
            row.Visible = !alertsCollapsed;
        }
        button.Text = alertsCollapsed ? "فتح" : "طي";
    }

    private void FrmDashboard_Load(object sender, EventArgs e)
    {

    }
}
