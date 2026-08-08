using System.ComponentModel;

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
/// </summary>
public static class TransportScreenProfilePolicy
{
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
    }

    private static void ApplyLayoutRole(TransportScreenProfile screenProfile, TransportLayoutRole role, Control control)
    {
        if (screenProfile == TransportScreenProfile.MasterData && role == TransportLayoutRole.MainData)
        {
            control.Dock = DockStyle.Top;
            if (control is ScrollableControl scrollable) scrollable.AutoScroll = false;
            if (control is TableLayoutPanel table)
            {
                table.AutoSize = true;
                table.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            }
        }

        if (screenProfile == TransportScreenProfile.ReadOnlyLog && role == TransportLayoutRole.Filters)
        {
            control.Dock = DockStyle.Top;
            if (control is ScrollableControl scrollable) scrollable.AutoScroll = false;
            if (control is TableLayoutPanel table)
            {
                table.AutoSize = true;
                table.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            }
        }

        if (role == TransportLayoutRole.Grid) control.Dock = DockStyle.Fill;

        if (screenProfile == TransportScreenProfile.ReadOnlyLog &&
            role == TransportLayoutRole.Toolbar &&
            control is TransportERP.Desktop.CoreUI.Controls.TransportToolbar toolbar)
        {
            toolbar.SetActionVisible(TransportERP.Desktop.CoreUI.Controls.ToolbarAction.New, false);
            toolbar.SetActionVisible(TransportERP.Desktop.CoreUI.Controls.ToolbarAction.Save, false);
            toolbar.SetActionVisible(TransportERP.Desktop.CoreUI.Controls.ToolbarAction.Edit, false);
            toolbar.SetActionVisible(TransportERP.Desktop.CoreUI.Controls.ToolbarAction.Disable, false);
            toolbar.SetActionVisible(TransportERP.Desktop.CoreUI.Controls.ToolbarAction.Delete, false);
        }
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
            foreach (DataGridViewColumn column in grid.Columns) column.SortMode = DataGridViewColumnSortMode.Automatic;
        }
    }
}
