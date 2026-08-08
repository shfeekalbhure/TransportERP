using System.ComponentModel;
using TransportERP.Desktop.CoreUI.Controls;

namespace TransportERP.Desktop.CoreUI.Profiles;

public enum TransportScreenProfile
{
    None,
    MasterData,
    TabbedMaster,
    ReadOnlyLog,
    Settings,
    Tree,
    Transaction
}

public enum TransportLayoutRole
{
    None,
    Toolbar,
    MainData,
    Search,
    Filters,
    Grid,
    TabsHost,
    Audit,
    Pagination,
    Alerts,
    TreeHost,
    SettingsHost,
    DetailsHost,
    ActionPanel,
    Totals,
    Summary
}

public enum TransportGridProfile
{
    None,
    Display,
    Editable,
    Log,
    TransactionLines,
    Lookup,
    Summary
}

public enum TransportFieldProfile
{
    None,
    Input,
    Display,
    Calculated,
    Lookup,
    Status,
    Reference,
    SystemMetadata,
    Derived
}

/// <summary>
/// قاعدة صغيرة للشاشات التي تدخل V1. إعلان ScreenProfile صريح وTyped،
/// لذلك لا يعتمد CoreUI على اسم الشاشة أو نوع الحاوية أو موضعها.
/// </summary>
public abstract class TransportScreenBase : UserControl
{
    [Category("TransportERP")]
    [DefaultValue(TransportScreenProfile.None)]
    public TransportScreenProfile ScreenProfile { get; set; } = TransportScreenProfile.None;
}

/// <summary>
/// Metadata Typed تصف وظيفة المنطقة وسلوك الجدول ودلالة الحقل.
/// الـMetadata تصف فقط ولا تنفذ Layout داخل الـDesigner.
/// </summary>
[ProvideProperty("LayoutRole", typeof(Control))]
[ProvideProperty("GridProfile", typeof(Control))]
[ProvideProperty("FieldProfile", typeof(Control))]
public sealed class TransportLayoutRoleProvider : Component, IExtenderProvider
{
    private readonly Dictionary<Control, TransportLayoutRole> _layoutRoles = new();
    private readonly Dictionary<Control, TransportGridProfile> _gridProfiles = new();
    private readonly Dictionary<Control, TransportFieldProfile> _fieldProfiles = new();

    public bool CanExtend(object extendee) => extendee is Control;

    [Category("TransportERP")]
    [DefaultValue(TransportLayoutRole.None)]
    public TransportLayoutRole GetLayoutRole(Control control) =>
        _layoutRoles.TryGetValue(control, out var value) ? value : TransportLayoutRole.None;

    public void SetLayoutRole(Control control, TransportLayoutRole value) =>
        SetMetadata(_layoutRoles, control, value, TransportLayoutRole.None);

    [Category("TransportERP")]
    [DefaultValue(TransportGridProfile.None)]
    public TransportGridProfile GetGridProfile(Control control) =>
        _gridProfiles.TryGetValue(control, out var value) ? value : TransportGridProfile.None;

    public void SetGridProfile(Control control, TransportGridProfile value) =>
        SetMetadata(_gridProfiles, control, value, TransportGridProfile.None);

    [Category("TransportERP")]
    [DefaultValue(TransportFieldProfile.None)]
    public TransportFieldProfile GetFieldProfile(Control control) =>
        _fieldProfiles.TryGetValue(control, out var value) ? value : TransportFieldProfile.None;

    public void SetFieldProfile(Control control, TransportFieldProfile value) =>
        SetMetadata(_fieldProfiles, control, value, TransportFieldProfile.None);

    internal IEnumerable<KeyValuePair<Control, TransportLayoutRole>> LayoutRoles => _layoutRoles;
    internal IEnumerable<KeyValuePair<Control, TransportGridProfile>> GridProfiles => _gridProfiles;

    private static void SetMetadata<T>(Dictionary<Control, T> values, Control control, T value, T none)
        where T : struct, Enum
    {
        ArgumentNullException.ThrowIfNull(control);
        if (EqualityComparer<T>.Default.Equals(value, none))
        {
            values.Remove(control);
            return;
        }

        values[control] = value;
    }
}

/// <summary>
/// Policy V1 تقرأ الإعلان Typed وتطبق السلوك؛ لا تستخدم اسم Control أو Tag أو موقعه للتخمين.
/// LayoutRole يصف المنطقة، بينما VerticalSizingBehavior مشتق داخليًا من ScreenProfile + LayoutRole.
/// </summary>
public static class TransportScreenProfilePolicy
{
    private enum VerticalSizingBehavior
    {
        Fixed,
        Content,
        Fill
    }

    public static void Apply(TransportScreenBase screen, TransportLayoutRoleProvider metadata)
    {
        ArgumentNullException.ThrowIfNull(screen);
        ArgumentNullException.ThrowIfNull(metadata);

        if (screen.ScreenProfile == TransportScreenProfile.None)
        {
            return;
        }

        foreach (var pair in metadata.LayoutRoles)
        {
            ApplyLayoutRole(screen.ScreenProfile, pair.Value, pair.Key);
        }

        foreach (var pair in metadata.GridProfiles)
        {
            if (pair.Key is DataGridView grid)
            {
                ApplyGridProfile(pair.Value, grid);
            }
        }

        ConfigureContentPropagation(screen, metadata);
    }

    private static void ApplyLayoutRole(TransportScreenProfile screenProfile, TransportLayoutRole role, Control control)
    {
        var verticalBehavior = ResolveVerticalSizing(screenProfile, role);

        switch (verticalBehavior)
        {
            case VerticalSizingBehavior.Fixed:
                break;
            case VerticalSizingBehavior.Content:
                if (control is ScrollableControl scrollable)
                {
                    scrollable.AutoScroll = false;
                }
                break;
            case VerticalSizingBehavior.Fill:
                if (role == TransportLayoutRole.Grid)
                {
                    control.Dock = DockStyle.Fill;
                }
                break;
        }

        if (screenProfile == TransportScreenProfile.ReadOnlyLog &&
            role == TransportLayoutRole.Toolbar &&
            control is TransportToolbar toolbar)
        {
            toolbar.SetActionVisible(ToolbarAction.New, false);
            toolbar.SetActionVisible(ToolbarAction.Save, false);
            toolbar.SetActionVisible(ToolbarAction.Edit, false);
            toolbar.SetActionVisible(ToolbarAction.Disable, false);
            toolbar.SetActionVisible(ToolbarAction.Delete, false);
        }
    }

    private static VerticalSizingBehavior ResolveVerticalSizing(
        TransportScreenProfile screenProfile,
        TransportLayoutRole role)
    {
        if (role == TransportLayoutRole.Grid)
        {
            return VerticalSizingBehavior.Fill;
        }

        if (screenProfile is TransportScreenProfile.MasterData or TransportScreenProfile.TabbedMaster &&
            role == TransportLayoutRole.MainData)
        {
            return VerticalSizingBehavior.Content;
        }

        if (screenProfile == TransportScreenProfile.ReadOnlyLog && role == TransportLayoutRole.Filters)
        {
            return VerticalSizingBehavior.Content;
        }

        return role switch
        {
            TransportLayoutRole.Toolbar or
            TransportLayoutRole.Search or
            TransportLayoutRole.Pagination => VerticalSizingBehavior.Fixed,
            TransportLayoutRole.Audit or
            TransportLayoutRole.Alerts or
            TransportLayoutRole.ActionPanel => VerticalSizingBehavior.Content,
            TransportLayoutRole.TreeHost or
            TransportLayoutRole.SettingsHost => VerticalSizingBehavior.Fill,
            _ => VerticalSizingBehavior.Fixed
        };
    }

    /// <summary>
    /// يطبق Propagation على الشاشات المعلنة فقط: المحتوى يعيد PreferredSize أولًا،
    /// ثم الـPolicy تطلب من الـShell المركزي إعادة حساب صف البيانات مرة واحدة.
    /// لا تستخدم Layout/SizeChanged الداخلية كحل ولا تدخل الشاشات غير المهاجرة في هذا المسار.
    /// </summary>
    private static void ConfigureContentPropagation(
        TransportScreenBase screen,
        TransportLayoutRoleProvider metadata)
    {
        var contentRole = screen.ScreenProfile switch
        {
            TransportScreenProfile.MasterData or TransportScreenProfile.TabbedMaster => TransportLayoutRole.MainData,
            TransportScreenProfile.ReadOnlyLog => TransportLayoutRole.Filters,
            _ => TransportLayoutRole.None
        };

        if (contentRole == TransportLayoutRole.None)
        {
            return;
        }

        var declaredContent = metadata.LayoutRoles
            .FirstOrDefault(pair => pair.Value == contentRole)
            .Key;

        if (declaredContent is null)
        {
            return;
        }

        var shell = FindAncestor<TransportReferenceScreenShell>(declaredContent);
        if (shell is null)
        {
            return;
        }

        var tabsHost = metadata.LayoutRoles
            .Where(pair => pair.Value == TransportLayoutRole.TabsHost)
            .Select(pair => pair.Key)
            .OfType<TabControl>()
            .FirstOrDefault(control => IsDescendantOf(control, declaredContent));

        var isRecalculating = false;

        void Recalculate()
        {
            if (isRecalculating || screen.IsDisposed)
            {
                return;
            }

            try
            {
                isRecalculating = true;
                screen.SuspendLayout();
                shell.SuspendLayout();

                var dataEntry = ResolveActiveDataEntry(declaredContent, tabsHost);
                if (dataEntry is null)
                {
                    return;
                }

                dataEntry.EnableProfileContentSizing();
                var availableWidth = Math.Max(
                    1,
                    dataEntry.Parent?.ClientSize.Width ?? declaredContent.ClientSize.Width);
                var preferred = dataEntry.GetProfilePreferredSize(new Size(availableWidth, 0));
                dataEntry.Height = preferred.Height;

                // الـShell يملك صف MainData فعليًا. نجهز semantics أولًا ثم نطلب منه الحساب المركزي مرة واحدة.
                shell.AutoFitDataGroup = true;
                shell.PerformLayout();
                shell.AutoFitDataGroup = false;
            }
            finally
            {
                shell.ResumeLayout(true);
                screen.ResumeLayout(true);
                isRecalculating = false;
            }
        }

        Recalculate();

        // Events محددة فقط: تغير المحتوى، تغير التبويب النشط، أو Resize فعلي للشاشة.
        var initialEntry = ResolveActiveDataEntry(declaredContent, tabsHost);
        if (initialEntry is not null)
        {
            initialEntry.ControlAdded += (_, _) => Recalculate();
            initialEntry.ControlRemoved += (_, _) => Recalculate();
        }

        if (tabsHost is not null)
        {
            tabsHost.SelectedIndexChanged += (_, _) => Recalculate();
        }

        screen.SizeChanged += (_, _) => Recalculate();
    }

    private static TransportDataEntryPanel? ResolveActiveDataEntry(Control declaredContent, TabControl? tabsHost)
    {
        if (declaredContent is TransportDataEntryPanel direct)
        {
            return direct;
        }

        if (tabsHost?.SelectedTab is TabPage selectedTab)
        {
            return FindDescendant<TransportDataEntryPanel>(selectedTab);
        }

        return FindDescendant<TransportDataEntryPanel>(declaredContent);
    }

    private static T? FindAncestor<T>(Control control)
        where T : Control
    {
        Control? current = control;
        while (current is not null)
        {
            if (current is T match)
            {
                return match;
            }
            current = current.Parent;
        }

        return null;
    }

    private static T? FindDescendant<T>(Control root)
        where T : Control
    {
        foreach (Control child in root.Controls)
        {
            if (child is T match)
            {
                return match;
            }

            var nested = FindDescendant<T>(child);
            if (nested is not null)
            {
                return nested;
            }
        }

        return null;
    }

    private static bool IsDescendantOf(Control control, Control ancestor)
    {
        Control? current = control;
        while (current is not null)
        {
            if (ReferenceEquals(current, ancestor))
            {
                return true;
            }
            current = current.Parent;
        }

        return false;
    }

    private static void ApplyGridProfile(TransportGridProfile profile, DataGridView grid)
    {
        if (profile is TransportGridProfile.Display or TransportGridProfile.Log or TransportGridProfile.Lookup or TransportGridProfile.Summary)
        {
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
        }

        if (profile is TransportGridProfile.Display or TransportGridProfile.Log)
        {
            grid.Dock = DockStyle.Fill;
            foreach (DataGridViewColumn column in grid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic;
            }
        }
    }
}
