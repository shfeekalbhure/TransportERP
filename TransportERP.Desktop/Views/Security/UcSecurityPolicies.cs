using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.CoreUI.Profiles;
using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

/// <summary>SEC-021 — سياسات الأمان؛ Settings workspace صريحة مع Tabs ممتدة وحاويات داخلية تملأ المساحة دون تمديد الحقول رأسيًا.</summary>
public partial class UcSecurityPolicies : TransportScreenBase
{
    public UcSecurityPolicies()
    {
        InitializeComponent();
        ConfigureSettingsTabFillPropagation();
        ConfigureProfileMetadata();
        TransportScreenProfilePolicy.Apply(this, profileMetadata);
        SecurityViewRuntime.Initialize(this, screenShell, "سياسات الأمان", "ابحث في سياسات الأمان...", SecurityWorkspaceMode.Edit);
    }

    /// <summary>
    /// SEC-021 فقط: تمرير Fill عبر الحاويات الوسيطة داخل كل Tab حتى حاوية الحقول.
    /// الحقول وTransportDataEntryPanel لا تتمدد رأسيًا؛ DataEntry يبقى Content/Top.
    /// </summary>
    private void ConfigureSettingsTabFillPropagation()
    {
        foreach (TabPage page in tabDetails.TabPages)
        {
            page.AutoScroll = false;

            foreach (Control child in page.Controls)
            {
                PromoteIntermediateContainerToFill(child);
            }
        }
    }

    private static void PromoteIntermediateContainerToFill(Control control)
    {
        // نقطة التوقف: حاوية الحقول هي مصدر PreferredHeight ولا تصبح Fill رأسيًا.
        if (control is TransportDataEntryPanel dataEntry)
        {
            dataEntry.Dock = DockStyle.Top;
            dataEntry.AutoScroll = false;
            dataEntry.EnableProfileContentSizing();
            return;
        }

        // Workspaces المرنة تملأ المساحة المتاحة بطبيعتها.
        if (control is TransportDataGrid or TreeView or CheckedListBox or SplitContainer)
        {
            control.Dock = DockStyle.Fill;
            return;
        }

        // الحاويات الوسيطة من TabPage وحتى DataEntry/Grid/Tree تمرر المساحة كاملة.
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
                PromoteContentRowsToFill(layout);
                break;

            case Panel panel:
                panel.AutoSize = false;
                panel.Dock = DockStyle.Fill;
                break;
        }

        foreach (Control child in control.Controls)
        {
            // Labels والأدوات الفعلية تحتفظ بقياسها؛ نعالج فقط الحاويات/Workspaces.
            if (child is Label)
            {
                continue;
            }

            PromoteIntermediateContainerToFill(child);
        }
    }

    private static void PromoteContentRowsToFill(TableLayoutPanel layout)
    {
        // أي Row يحمل DataEntry (مباشرة أو عبر حاوية وسيطة) يصبح المساحة المرنة.
        // صفوف الوصف/الأوامر ذات Absolute تبقى ثابتة.
        foreach (Control child in layout.Controls)
        {
            if (!ContainsDescendant<TransportDataEntryPanel>(child) && child is not TransportDataEntryPanel)
            {
                continue;
            }

            var row = layout.GetRow(child);
            if (row < 0 || row >= layout.RowStyles.Count)
            {
                continue;
            }

            var style = layout.RowStyles[row];
            if (style.SizeType != SizeType.Absolute)
            {
                style.SizeType = SizeType.Percent;
                style.Height = 100F;
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
        // الإعلان دلالي ومحصور في SEC-021؛ CoreUI Policy تقرأ الأدوار ولا تخمنها من أسماء الأدوات.
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
