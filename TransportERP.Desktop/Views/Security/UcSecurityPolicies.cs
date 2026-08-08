using TransportERP.Desktop.CoreUI.Controls;
using TransportERP.Desktop.CoreUI.Profiles;
using TransportERP.Desktop.Views.Security.Shared;

namespace TransportERP.Desktop.Views.Security;

/// <summary>SEC-021 — سياسات الأمان؛ Settings workspace صريحة مع Tabs ممتدة وأقسام حقول Content-sized.</summary>
public partial class UcSecurityPolicies : TransportScreenBase
{
    public UcSecurityPolicies()
    {
        InitializeComponent();
        ConfigureProfileMetadata();
        TransportScreenProfilePolicy.Apply(this, profileMetadata);
        SecurityViewRuntime.Initialize(this, screenShell, "سياسات الأمان", "ابحث في سياسات الأمان...", SecurityWorkspaceMode.Edit);
    }

    private void ConfigureProfileMetadata()
    {
        // الإعلان دلالي ومحصور في SEC-021؛ CoreUI Policy تقرأ الأدوار ولا تخمنها من أسماء الأدوات.
        profileMetadata.SetLayoutRole(screenShell.DataHost, TransportLayoutRole.SettingsHost);
        profileMetadata.SetLayoutRole(tabDetails, TransportLayoutRole.TabsHost);
        profileMetadata.SetLayoutRole(screenShell.Toolbar, TransportLayoutRole.Toolbar);
        profileMetadata.SetLayoutRole(screenShell.AlertBar, TransportLayoutRole.Alerts);

        // SecurityDesignerSupport أنشأ Workspaces التبويبات. في هذا الـPilot نعلن فقط العناصر الممتدة فعليًا.
        foreach (var grid in EnumerateDescendants<TransportDataGrid>(tabDetails))
        {
            profileMetadata.SetLayoutRole(grid, TransportLayoutRole.Grid);
        }

        foreach (var tree in EnumerateDescendants<TreeView>(tabDetails))
        {
            profileMetadata.SetLayoutRole(tree, TransportLayoutRole.TreeHost);
        }

        // أقسام الحقول تبقى Content: نفعّل PreferredSize المحسوب دون تحويلها إلى Fill.
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
