using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.CoreUI.Profiles;
using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

/// <summary>SEC-021 — سياسات الأمان؛ Settings workspace ممتدة مع إبقاء أقسام الحقول Content-sized.</summary>
public partial class UcSecurityPolicies : TransportScreenBase
{
    public UcSecurityPolicies()
    {
        InitializeComponent();
        ConfigureSettingsTabLayout();
        ConfigureProfileMetadata();
        TransportScreenProfilePolicy.Apply(this, profileMetadata);
        SecurityViewRuntime.Initialize(this, screenShell, "سياسات الأمان", "ابحث في سياسات الأمان...", SecurityWorkspaceMode.Edit);
    }

    /// <summary>
    /// SEC-021 فقط:
    /// - الـTab والـWorkspace المرن يملآن المساحة.
    /// - أي فرع يحتوي حقول إدخال يبقى Content/Top ولا يتمدد رأسيًا.
    /// - Grid/Tree/Audit workspaces تبقى Fill.
    /// </summary>
    private void ConfigureSettingsTabLayout()
    {
        foreach (TabPage page in tabDetails.TabPages)
        {
            page.AutoScroll = false;

            foreach (Control child in page.Controls)
            {
                ConfigureTabBranch(child);
            }
        }
    }

    private static void ConfigureTabBranch(Control control)
    {
        // فرع الحقول كله Content-sized من أول Section يحتوي DataEntry حتى الحقول نفسها.
        if (control is TransportDataEntryPanel dataEntry)
        {
            dataEntry.Dock = DockStyle.Top;
            dataEntry.AutoScroll = false;
            dataEntry.EnableProfileContentSizing();
            return;
        }

        var containsFields = ContainsDescendant<TransportDataEntryPanel>(control);
        if (containsFields)
        {
            ConfigureFieldContentContainer(control);

            foreach (Control child in control.Controls)
            {
                if (child is Label)
                {
                    continue;
                }

                ConfigureTabBranch(child);
            }

            return;
        }

        // Workspaces المرنة فقط هي التي تستهلك بقية مساحة التبويب.
        if (control is TransportDataGrid or TreeView or CheckedListBox or SplitContainer)
        {
            control.Dock = DockStyle.Fill;
            return;
        }

        switch (control)
        {
            case TransportGroupBox group:
                group.AutoSize = false;
                group.AutoSizeMode = AutoSizeMode.GrowOnly;
                group.Dock = DockStyle.Fill;
                break;

            case TableLayoutPanel layout:
                layout.AutoSize = false;
                layout.Dock = DockStyle.Fill;
                break;

            case Panel panel:
                panel.AutoSize = false;
                panel.Dock = DockStyle.Fill;
                break;
        }

        foreach (Control child in control.Controls)
        {
            if (child is Label)
            {
                continue;
            }

            ConfigureTabBranch(child);
        }
    }

    private static void ConfigureFieldContentContainer(Control control)
    {
        switch (control)
        {
            case TransportGroupBox group:
                group.Dock = DockStyle.Top;
                group.AutoSize = true;
                group.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                break;

            case TableLayoutPanel layout:
                layout.Dock = DockStyle.Top;
                layout.AutoSize = true;
                layout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                RestoreFieldRowsToContent(layout);
                break;

            case Panel panel:
                // وسيط داخل فرع حقول: لا يملأ الارتفاع المتبقي.
                panel.Dock = DockStyle.Top;
                panel.AutoSize = true;
                panel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                break;
        }
    }

    private static void RestoreFieldRowsToContent(TableLayoutPanel layout)
    {
        foreach (Control child in layout.Controls)
        {
            if (child is not TransportDataEntryPanel && !ContainsDescendant<TransportDataEntryPanel>(child))
            {
                continue;
            }

            var row = layout.GetRow(child);
            if (row < 0 || row >= layout.RowStyles.Count)
            {
                continue;
            }

            // صف الحقول يقاس من PreferredHeight الحقيقي؛ صفوف الأوامر Absolute تبقى كما هي.
            var style = layout.RowStyles[row];
            if (style.SizeType != SizeType.Absolute)
            {
                style.SizeType = SizeType.AutoSize;
                style.Height = 0F;
            }
        }
    }

    private static bool ContainsDescendant<TControl>(Control root)
        where TControl : Control
    {
        foreach (Control child in root.Controls)
        {
            if (child is TControl || ContainsDescendant<TControl>(child))
            {
                return true;
            }
        }

        return false;
    }

    private void ConfigureProfileMetadata()
    {
        profileMetadata.SetLayoutRole(screenShell.DataHost, TransportLayoutRole.SettingsHost);
        profileMetadata.SetLayoutRole(tabDetails, TransportLayoutRole.TabsHost);
        profileMetadata.SetLayoutRole(screenShell.Toolbar, TransportLayoutRole.Toolbar);
        profileMetadata.SetLayoutRole(screenShell.AlertBar, TransportLayoutRole.Alerts);

        foreach (var grid in EnumerateDescendants<TransportDataGrid>(tabDetails))
        {
            profileMetadata.SetLayoutRole(grid, TransportLayoutRole.Grid);
        }

        foreach (var tree in EnumerateDescendants<TreeView>(tabDetails))
        {
            profileMetadata.SetLayoutRole(tree, TransportLayoutRole.TreeHost);
        }

        foreach (var dataEntry in EnumerateDescendants<TransportDataEntryPanel>(tabDetails))
        {
            dataEntry.EnableProfileContentSizing();
        }
    }

    private static IEnumerable<TControl> EnumerateDescendants<TControl>(Control root)
        where TControl : Control
    {
        foreach (Control child in root.Controls)
        {
            if (child is TControl match)
            {
                yield return match;
            }

            foreach (var nested in EnumerateDescendants<TControl>(child))
            {
                yield return nested;
            }
        }
    }
}
