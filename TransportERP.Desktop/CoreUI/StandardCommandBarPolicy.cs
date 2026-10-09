using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TransportERP.Desktop.CoreUI;

public sealed record CommandBarCoverage(string Screen, string Mode, string? SourceContainer, int StandardCommands, string[] MissingIcons);

/// <summary>Toolbar-only migration adapter. Does not change root, field, grid or audit properties.</summary>
public static class StandardCommandBarPolicy
{
    private static readonly ConditionalWeakTable<Control, CommandBarCoverage> Applied = new();
    private static readonly ConditionalWeakTable<Control, object> EmbeddedViews = new();
    public static void ExcludeEmbeddedView(Control root) => EmbeddedViews.GetValue(root, _ => new object());
    private static readonly HashSet<string> ShellTypes = new(StringComparer.Ordinal)
    {
        "TransportERP.Desktop.FrmLogin", "TransportERP.Desktop.FrmMain",
        "TransportERP.Desktop.FrmDashboard", "TransportERP.Desktop.FrmWorkScope",
        "TransportERP.Desktop.Forms.Templates.FrmDashboardTemplate"
    };
    private static bool IsShell(Type? type)
    {
        for(; type != null; type = type.BaseType)
            if(type.FullName is string name && ShellTypes.Contains(name)) return true;
        return false;
    }
    private static readonly Dictionary<string, StandardCommand> Aliases = new(StringComparer.OrdinalIgnoreCase)
    {
        ["btnAdd"] = StandardCommand.Add, ["btnNew"] = StandardCommand.Add,
        ["btnEdit"] = StandardCommand.Edit, ["btnSave"] = StandardCommand.Save, ["save"] = StandardCommand.Save,
        ["btnDelete"] = StandardCommand.Delete, ["btnCancel"] = StandardCommand.Cancel, ["btnUndo"] = StandardCommand.Cancel,
        ["btnView"] = StandardCommand.View, ["btnSearch"] = StandardCommand.View,
        ["btnPrint"] = StandardCommand.Print, ["btnFirst"] = StandardCommand.First,
        ["btnPrevious"] = StandardCommand.Previous, ["btnPrev"] = StandardCommand.Previous,
        ["btnNext"] = StandardCommand.Next, ["btnLast"] = StandardCommand.Last,
        ["btnClose"] = StandardCommand.Close, ["btnExit"] = StandardCommand.Close,
        ["btnRefresh"] = StandardCommand.Refresh, ["btnReload"] = StandardCommand.Refresh,
        ["btnImport"] = StandardCommand.Import, ["btnImportExcel"] = StandardCommand.Import,
        ["btnExport"] = StandardCommand.Export, ["btnExportExcel"] = StandardCommand.Export,
        ["btnHelp"] = StandardCommand.Help
    };
    private static readonly Dictionary<string, StandardCommand> Captions = new()
    {
        ["إضافة"] = StandardCommand.Add, ["جديد"] = StandardCommand.Add,
        ["تعديل"] = StandardCommand.Edit, ["حفظ"] = StandardCommand.Save,
        ["حذف"] = StandardCommand.Delete, ["تراجع"] = StandardCommand.Cancel, ["إلغاء"] = StandardCommand.Cancel,
        ["عرض"] = StandardCommand.View, ["بحث"] = StandardCommand.View, ["طباعة"] = StandardCommand.Print,
        ["الأول"] = StandardCommand.First, ["السابق"] = StandardCommand.Previous,
        ["التالي"] = StandardCommand.Next, ["الأخير"] = StandardCommand.Last,
        ["إغلاق"] = StandardCommand.Close, ["خروج"] = StandardCommand.Close,
        ["تحديث"] = StandardCommand.Refresh, ["استيراد"] = StandardCommand.Import,
        ["تصدير"] = StandardCommand.Export, ["مساعدة"] = StandardCommand.Help
    };
    private static readonly HashSet<string> PreserveReference = new()
    {
        "UcCurrencySetup", "UcScreen_04_05_01", "UcScreen_04_04_01", "UcScreen_04_04_02"
    };
    private static readonly HashSet<string> ReferenceMaster = new()
    {
        "UcSystemPeriods", "UcInternationalRegions", "UcCountryData", "UcGovernorateData", "UcAreaData",
        "UcCompanyData", "UcBranchData", "UcChartSetup", "UcAccountingChartData", "UcChartOfAccounts",
        "UcCostCenterSetup", "UcCostCenterData", "UcAccountGroupsAndTypes"
    };
    private static readonly HashSet<string> ReferenceSettings = new()
    { "UcGeneralVariables", "UcGeneralVariablesSetup", "UcScreenBackgrounds" };

    public static void OnScreenLoad(object? sender, EventArgs e)
    {
        if (sender is not Control root || root.IsDisposed || LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
        if (IsShell(root.GetType())) return;
        if (EmbeddedViews.TryGetValue(root, out _)) return;
        // Run after all screen-specific Load handlers and visibility/permission bindings.
        if (root.IsHandleCreated) root.BeginInvoke((MethodInvoker)(() => { if(!root.IsDisposed) Apply(root); }));
    }
    public static CommandBarCoverage? Coverage(Control root) => Applied.TryGetValue(root, out var result) ? result : StaticCoverage(root);
    private static CommandBarCoverage? StaticCoverage(Control root)
    {
        if(IsShell(root.GetType())) return null;
        static IEnumerable<DesignerCommandBar> Owned(Control parent)
        {
            foreach(Control child in parent.Controls)
            {
                if(child is DesignerCommandBar bar) yield return bar;
                else if(child is not UserControl)
                    foreach(var nested in Owned(child)) yield return nested;
            }
        }
        var bar = Owned(root).SingleOrDefault();
        return bar == null ? null : new CommandBarCoverage(root.GetType().FullName!, "static-designer", bar.Name,
            bar.Commands.Count, bar.Commands.Where(p => p.Key <= StandardCommand.Close && p.Value.Image == null).Select(p => p.Key.ToString()).ToArray());
    }
    private static IEnumerable<Control> Walk(Control parent)
    { foreach(Control child in parent.Controls) { yield return child; foreach(var nested in Walk(child)) yield return nested; } }
    private static StandardCommand? Identify(Button b)
    {
        if(Aliases.TryGetValue(b.Name, out var id)) return id;
        return Captions.TryGetValue(b.Text.Replace("&", "").Trim(), out id) ? id : null;
    }
    public static CommandBarCoverage Apply(Control root)
    {
        if(IsShell(root.GetType()))
            return Applied.GetValue(root, c => new CommandBarCoverage(c.GetType().FullName!, "excluded-shell", null, 0, Array.Empty<string>()));
        if(Applied.TryGetValue(root, out var done)) return done;
        if(StaticCoverage(root) is { } staticCoverage) return staticCoverage;
        var controls = Walk(root).ToArray();
        var candidates = controls.Where(c => c is Panel || c is TableLayoutPanel).Where(c =>
            c.Name is "pnlToolbar" or "pnlActions" or "flpActions" or "actions" or "toolbar" or "commandBarContainer" or "passwordActions"
            || c.Name.Equals("commandBar", StringComparison.OrdinalIgnoreCase)
            || root.GetType().Name == "UcShipmentDispatch" && c.Name == "flowLayoutPanel1")
            .Where(c => Walk(c).OfType<Button>().Any()).ToArray();
        var primary = candidates.OrderByDescending(c => c.Name is "pnlToolbar" or "commandBarContainer")
            .ThenByDescending(c => Walk(c).OfType<Button>().Count(b => Identify(b) != null)).FirstOrDefault();
        // Toolbar candidates must contain commands, not an arbitrary form/grid container.
        primary ??= controls.OfType<FlowLayoutPanel>().Where(c => c.Controls.OfType<Button>().Count(b => Identify(b) != null) >= 2)
            .OrderByDescending(c => c.Controls.OfType<Button>().Count(b => Identify(b) != null)).FirstOrDefault();

        var layoutBatch = new Control?[] { root, primary?.Parent, primary }.Where(c => c != null).Cast<Control>().Distinct().ToArray();
        foreach(var control in layoutBatch) control.SuspendLayout();
        try
        {
        var catalog = new StandardCommandBar();
        bool sourceVisible = primary?.Visible ?? true;
        var defaultCommands = Enum.GetValues<StandardCommand>().Where(c => c <= StandardCommand.Close).ToHashSet();
        var sourceButtons = primary == null ? Array.Empty<Button>() : Walk(primary).OfType<Button>().ToArray();
        if(primary == null || !sourceButtons.Any(b => Identify(b) is StandardCommand.Add or StandardCommand.Edit or StandardCommand.Save or StandardCommand.Delete)
            && !ReferenceMaster.Contains(root.GetType().Name))
            defaultCommands = new[] { StandardCommand.View, StandardCommand.Print, StandardCommand.Refresh, StandardCommand.Export, StandardCommand.Close }.ToHashSet();
        if(root.GetType().Name is "UcPrintSettings" or "UcGeneralSettings" or "UcGeneralLedgerSettings")
            defaultCommands = new[] { StandardCommand.Edit, StandardCommand.Save, StandardCommand.Cancel, StandardCommand.Print, StandardCommand.Close }.ToHashSet();
        if(root.GetType().Name == "UcChangePassword") defaultCommands = new[] { StandardCommand.Close }.ToHashSet();
        if(ReferenceSettings.Contains(root.GetType().Name))
            defaultCommands = new[] { StandardCommand.Edit, StandardCommand.Save, StandardCommand.Cancel, StandardCommand.Print, StandardCommand.Close }.ToHashSet();
        if(root.GetType().Name == "UcSystemPeriods")
            defaultCommands.ExceptWith(new[] { StandardCommand.Cancel, StandardCommand.View, StandardCommand.Print });
        catalog.SetProfile(defaultCommands);
        if(ReferenceMaster.Contains(root.GetType().Name) || ReferenceSettings.Contains(root.GetType().Name)) catalog.UseCompactReferenceHeight();

        bool preserve = PreserveReference.Contains(root.GetType().Name)
            || primary != null && (Walk(primary).Any(c => c is TextBoxBase or ComboBox or DataGridView or NumericUpDown) || !primary.Enabled);
        string mode;
        if(preserve && primary != null)
        {
            // Replace the host while retaining the reference layout and the actual command objects.
            catalog.SetProfile(Array.Empty<StandardCommand>());
            var parent = primary.Parent!;
            var index = parent.Controls.GetChildIndex(primary);
            var bounds = primary.Bounds;
            var dock = primary.Dock; var anchor = primary.Anchor;
            if(parent is TableLayoutPanel table)
            {
                int row = table.GetRow(primary), col = table.GetColumn(primary);
                int rows = table.GetRowSpan(primary), cols = table.GetColumnSpan(primary);
                parent.Controls.Remove(primary);
                table.Controls.Add(catalog, col, row);
                table.SetRowSpan(catalog, rows); table.SetColumnSpan(catalog, cols);
            }
            else { parent.Controls.Remove(primary); parent.Controls.Add(catalog); }
            catalog.Bounds = bounds; catalog.Dock = dock; catalog.Anchor = anchor;
            parent.Controls.SetChildIndex(catalog, Math.Min(index, parent.Controls.Count - 1));
            foreach(var b in sourceButtons)
                if(Identify(b) is StandardCommand id) catalog.BindReference(id, b);
            catalog.UseReferenceLayout(primary);
            mode = "reference-layout-container-replaced";
        }
        else
        {
            var buttons = sourceButtons;
            var auxiliary = primary == null ? Array.Empty<Control>() : Walk(primary).Where(c => c is Label or PictureBox).ToArray();
            var selected = new HashSet<StandardCommand>();
            foreach(var button in buttons)
            {
                var command = Identify(button);
                bool visible = button.Visible;
                bool enabled = button.Enabled;
                if(command is StandardCommand id && selected.Add(id))
                {
                    if(ReferenceSettings.Contains(root.GetType().Name) || root.GetType().Name == "UcSystemPeriods") visible &= defaultCommands.Contains(id);
                    catalog.Adopt(id, button, visible);
                }
                else catalog.AddSpecial(button, visible);
                button.Enabled = enabled;
            }
            foreach(var control in auxiliary) catalog.AddAuxiliary(control);
            if(primary != null)
            {
                var parent = primary.Parent!;
                var index = parent.Controls.GetChildIndex(primary);
                var bounds = primary.Bounds;
                if(parent is TableLayoutPanel table)
                {
                    int row = table.GetRow(primary), col = table.GetColumn(primary);
                    int rowSpan = table.GetRowSpan(primary), colSpan = table.GetColumnSpan(primary);
                    parent.Controls.Remove(primary);
                    table.Controls.Add(catalog, col, row);
                    table.SetRowSpan(catalog, rowSpan); table.SetColumnSpan(catalog, colSpan);
                    if(row >= 0 && row < table.RowStyles.Count && rowSpan == 1)
                    { table.RowStyles[row].SizeType = SizeType.Absolute; table.RowStyles[row].Height = catalog.Height; }
                    catalog.Dock = DockStyle.Fill;
                }
                else
                {
                    var dock = primary.Dock; var anchor = primary.Anchor;
                    parent.Controls.Remove(primary); parent.Controls.Add(catalog);
                    catalog.Bounds = new Rectangle(bounds.Location, new Size(bounds.Width, catalog.Height));
                    catalog.Dock = dock; catalog.Anchor = anchor;
                    parent.Controls.SetChildIndex(catalog, Math.Min(index, parent.Controls.Count - 1));
                }
                // Retain the original named container for existing screen references and disposal.
                root.Controls.Add(primary); primary.Visible = false;
                primary.EnabledChanged += (_, _) => { if(!catalog.IsDisposed) catalog.Enabled = primary.Enabled; };
                mode = ReferenceMaster.Contains(root.GetType().Name) || ReferenceSettings.Contains(root.GetType().Name) ? "reference-command-profile" : "standard-command-profile";
            }
            else
            {
                catalog.Dock = DockStyle.Top; root.Controls.Add(catalog); catalog.SendToBack();
                mode = "standard-command-profile-new-toolbar";
            }
        }
        catalog.Visible = sourceVisible;
        var missing = Walk(root).OfType<Button>().Where(b => b.Image == null && (b.AccessibleDescription ?? "").Contains("Missing icon"))
            .Select(b => b.Name).Distinct().ToArray();
        var result = new CommandBarCoverage(root.GetType().FullName!, mode, primary?.Name, catalog.Commands.Count, missing);
        Applied.Add(root, result);
        return result;
        }
        finally
        {
            foreach(var control in layoutBatch.Reverse()) if(!control.IsDisposed) control.ResumeLayout(true);
        }
    }
}
