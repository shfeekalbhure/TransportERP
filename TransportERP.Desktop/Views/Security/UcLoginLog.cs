using TransportERP.Desktop.CoreUI.Profiles;

namespace TransportERP.Desktop.Views.Security;

/// <summary>SEC-022 — سجل الدخول؛ ReadOnlyLog صريح بلا CRUD تقليدي.</summary>
public partial class UcLoginLog : TransportScreenBase
{
    public UcLoginLog()
    {
        InitializeComponent();
        TransportScreenProfilePolicy.Apply(this, profileMetadata);
        screenShell.Pagination.SetPageInfo(1, 1, 0, 0, 0);
        screenShell.AlertBar.HideMessage();
        screenShell.Toolbar.PrintRequested += (_, _) =>
            screenShell.AlertBar.Text = "طباعة سجل الدخول تمر عبر خدمة التقارير عند توفرها.";
        screenShell.Toolbar.CloseRequested += (_, _) => CloseHostTab();
    }

    private void CloseHostTab()
    {
        if (Parent is not TabPage page || page.Parent is not TabControl tabs) return;
        tabs.TabPages.Remove(page);
        Dispose();
        page.Dispose();
    }
}
