using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TransportERP.Desktop.Forms.Setup.Geographic;

namespace TransportERP.Desktop;

/// <summary>
/// ربط شاشات البيانات الجغرافية الحالية بقائمة التهيئة العامة وتبويبات Dashboard.
/// </summary>
public partial class FrmDashboard
{
    private const string GovernoratesTabKey = "GEN-004";
    private const string DirectoratesTabKey = "GEN-005";
    private const string AreasTabKey = "GEN-007";

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        EnsureGeographicMenuItems();
    }

    private void EnsureGeographicMenuItems()
    {
        if (_generalSetupMenu is null)
        {
            return;
        }

        var geographicDataItem = _generalSetupMenu.Items
            .OfType<ToolStripMenuItem>()
            .FirstOrDefault(item =>
                string.Equals(item.Text, "البيانات الجغرافية", StringComparison.Ordinal));

        if (geographicDataItem is null)
        {
            return;
        }

        AddMenuItemIfMissing(
            geographicDataItem,
            "mnuGovernorates",
            "المحافظات",
            "GEN-004 — المحافظات",
            OpenGovernoratesTab);

        AddMenuItemIfMissing(
            geographicDataItem,
            "mnuDirectorates",
            "المديريات",
            "GEN-005 — المديريات",
            OpenDirectoratesTab);

        AddMenuItemIfMissing(
            geographicDataItem,
            "mnuAreas",
            "المناطق",
            "GEN-007 — المناطق",
            OpenAreasTab);
    }

    private static void AddMenuItemIfMissing(
        ToolStripMenuItem parent,
        string name,
        string text,
        string toolTipText,
        Action clickAction)
    {
        if (parent.DropDownItems.ContainsKey(name))
        {
            return;
        }

        var menuItem = new ToolStripMenuItem(text)
        {
            Name = name,
            ToolTipText = toolTipText,
            RightToLeft = RightToLeft.Yes
        };

        menuItem.Click += (_, _) => clickAction();
        parent.DropDownItems.Add(menuItem);
    }

    private void OpenGovernoratesTab() =>
        OpenGeographicTab(GovernoratesTabKey, "المحافظات", new FrmGovernorates());

    private void OpenDirectoratesTab() =>
        OpenGeographicTab(DirectoratesTabKey, "المديريات", new FrmDirectorates());

    private void OpenAreasTab() =>
        OpenGeographicTab(AreasTabKey, "المناطق", new FrmAreas());

    private void OpenGeographicTab(string tabKey, string title, Form form)
    {
        if (_workspaceTabs is null)
        {
            form.Dispose();
            return;
        }

        var existingPage = _workspaceTabs.TabPages
            .Cast<TabPage>()
            .FirstOrDefault(page =>
                string.Equals(page.Name, tabKey, StringComparison.Ordinal));

        if (existingPage is not null)
        {
            form.Dispose();
            _workspaceTabs.SelectedTab = existingPage;
            existingPage.Focus();
            return;
        }

        ConfigureFormForTabHosting(form);

        var page = new TabPage
        {
            Name = tabKey,
            Text = $"{title}  ×",
            BackColor = Color.FromArgb(239, 245, 252),
            RightToLeft = RightToLeft.Yes,
            Padding = Padding.Empty,
            Tag = form
        };

        form.FormClosed += (_, _) =>
        {
            if (_workspaceTabs.TabPages.Contains(page))
            {
                _workspaceTabs.TabPages.Remove(page);
                page.Dispose();
            }
        };

        page.Controls.Add(form);
        _workspaceTabs.TabPages.Add(page);
        _workspaceTabs.SelectedTab = page;
        form.Show();
    }

    private static void ConfigureFormForTabHosting(Form form)
    {
        ArgumentNullException.ThrowIfNull(form);

        form.TopLevel = false;
        form.FormBorderStyle = FormBorderStyle.None;
        form.Dock = DockStyle.Fill;
        form.WindowState = FormWindowState.Normal;
        form.ShowInTaskbar = false;
        form.ControlBox = false;
        form.RightToLeft = RightToLeft.Yes;
        form.RightToLeftLayout = true;
    }
}
