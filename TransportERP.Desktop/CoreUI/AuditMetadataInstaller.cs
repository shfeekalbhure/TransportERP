using System.Runtime.CompilerServices;
using System.Reflection;
using System.Text.RegularExpressions;

namespace TransportERP.Desktop.CoreUI;

public enum AuditMetadataProfile { Standard, ModificationOnly, CyanModification, ReceiptReference }

/// <summary>
/// Narrow adapter for legacy audit sources and code-built screens. The visual controls live in
/// AuditMetadataControl.Designer.cs. Source objects, bindings, names and values are retained.
/// </summary>
public static class AuditMetadataInstaller
{
    private static readonly ConditionalWeakTable<Control, object> Applied = new();
    private static readonly ConditionalWeakTable<Control, object> AwaitingDesigner = new();
    private static readonly HashSet<string> ExcludedApplicationShells = new(StringComparer.Ordinal)
    {
        "TransportERP.Desktop.FrmLogin", "TransportERP.Desktop.FrmMain",
        "TransportERP.Desktop.FrmDashboard", "TransportERP.Desktop.FrmWorkScope"
    };
    private static readonly HashSet<string> ContainerNames = new(StringComparer.Ordinal)
    { "tlpAuditInfo", "auditInfoContainer", "goldenMetadata", "auditFooter", "audit", "referenceAudit" };
    private static readonly Dictionary<string, string> Names = new(StringComparer.OrdinalIgnoreCase)
    {
        ["txtCreatedBy"]="CreatedBy", ["lblCreatedBy"]="CreatedBy", ["auditCreatedBy"]="CreatedBy",
        ["txtCreatedByCode"]="CreatedByCode", ["auditCreatedById"]="CreatedByCode",
        ["txtModifiedBy"]="UpdatedBy", ["txtUpdatedBy"]="UpdatedBy", ["lblModifiedBy"]="UpdatedBy", ["auditUpdatedBy"]="UpdatedBy",
        ["txtUpdatedByCode"]="UpdatedByCode", ["auditUpdatedById"]="UpdatedByCode",
        ["txtCreatedAt"]="CreatedAt", ["lblCreatedAt"]="CreatedAt", ["auditCreatedDateTime"]="CreatedAt",
        ["txtUpdatedAt"]="UpdatedAt", ["txtModifiedAt"]="UpdatedAt", ["lblModifiedAt"]="UpdatedAt", ["auditUpdatedDateTime"]="UpdatedAt",
        ["txtCreatedDevice"]="CreatedDevice", ["lblCreatedDevice"]="CreatedDevice", ["auditCreatedDevice"]="CreatedDevice",
        ["txtUpdatedDevice"]="UpdatedDevice", ["lblModifiedDevice"]="UpdatedDevice", ["auditUpdatedDevice"]="UpdatedDevice",
        ["txtPrintCount"]="PrintCount", ["lblPrintCount"]="PrintCount",
        ["txtUpdateCount"]="UpdateCount", ["lblEditCount"]="UpdateCount", ["auditUpdatedCount"]="UpdateCount",
        ["lblAccountName"]="AccountName", ["lblLastPrintedAt"]="LastPrintedAt"
    };
    private static readonly Dictionary<string, string> Captions = new(StringComparer.Ordinal)
    {
        ["مدخل السجل"]="CreatedBy", ["أنشأ بواسطة"]="CreatedBy", ["المستخدم المدخل"]="CreatedBy",
        ["معدل السجل"]="UpdatedBy", ["عدل بواسطة"]="UpdatedBy", ["عدّل بواسطة"]="UpdatedBy", ["المستخدم المعدل"]="UpdatedBy",
        ["تاريخ الإدخال"]="CreatedAt", ["تاريخ الانشاء"]="CreatedAt", ["تاريخ الإنشاء"]="CreatedAt",
        ["تاريخ آخر تعديل"]="UpdatedAt", ["تاريخ التعديل"]="UpdatedAt",
        ["الجهاز المدخل"]="CreatedDevice", ["جهاز الإدخال"]="CreatedDevice",
        ["الجهاز المعدل"]="UpdatedDevice", ["جهاز التعديل"]="UpdatedDevice",
        ["مرات الطباعة"]="PrintCount", ["عدد مرات الطباعة"]="PrintCount",
        ["مرات التعديل"]="UpdateCount", ["عدد التعديلات"]="UpdateCount", ["عدد مرات التعديل"]="UpdateCount",
        ["اسم الحساب"]="AccountName", ["تاريخ اخر طباعة"]="LastPrintedAt", ["تاريخ آخر طباعة"]="LastPrintedAt"
    };

    private static IEnumerable<Control> Walk(Control root)
    {
        foreach (Control child in root.Controls)
        {
            yield return child;
            if (child is DataGridView || child is AuditMetadataControl) continue;
            foreach (var descendant in Walk(child)) yield return descendant;
        }
    }

    private static AuditMetadataControl? DesignedFooter(Control screen)
    {
        // Prefer the owning screen's actual Designer field; a nested tab can
        // contain another screen with a field having the same conventional name.
        for (Type? type = screen.GetType(); type is not null && type != typeof(Control); type = type.BaseType)
            if (type.GetField("standardAuditMetadata", BindingFlags.Instance | BindingFlags.Public
                | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)?.GetValue(screen) is AuditMetadataControl field)
                return field;
        return Walk(screen).OfType<AuditMetadataControl>().FirstOrDefault(footer =>
        {
            for (Control? parent = footer.Parent; parent is not null && parent != screen; parent = parent.Parent)
                if (parent is UserControl && parent.GetType().Assembly != typeof(Control).Assembly) return false;
            return true;
        });
    }

    public static void Apply(Control screen, AuditMetadataProfile profile = AuditMetadataProfile.Standard)
    {
        // Explicit user boundary: authentication, navigation and the home shell are
        // not record-entry screens. Their hosted business screens retain their own footer.
        if (ExcludedApplicationShells.Contains(screen.GetType().FullName ?? "")) return;
        // WinForms can raise Load while a base constructor creates handles, before
        // the derived InitializeComponent assigns its explicitly declared footer.
        // Wait for that object instead of installing a competing runtime instance.
        bool pendingDesigner = false;
        for (Type? type = screen.GetType(); type is not null && type != typeof(Control); type = type.BaseType)
            pendingDesigner |= type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
                .Any(field => field.FieldType == typeof(AuditMetadataControl) && field.GetValue(screen) is null);
        if (pendingDesigner)
        {
            if (!AwaitingDesigner.TryGetValue(screen, out _))
            {
                AwaitingDesigner.Add(screen, new object());
                void Queue() => screen.BeginInvoke((Action)(() => { if (!screen.IsDisposed) Apply(screen, profile); }));
                if (screen.IsHandleCreated) Queue();
                else
                {
                    EventHandler? ready = null;
                    ready = (_, _) => { screen.HandleCreated -= ready; Queue(); };
                    screen.HandleCreated += ready;
                }
            }
            return;
        }
        if (Applied.TryGetValue(screen, out _)) return;
        // A containing record screen has already retained the embedded tab's
        // legacy sources in its single shared footer. Do not add a second display
        // when that embedded control receives its first Load on tab activation.
        for (Control? parent = screen.Parent; parent is not null; parent = parent.Parent)
            if (Applied.TryGetValue(parent, out _))
            {
                if (DesignedFooter(screen) is { } embedded) embedded.Visible = false;
                return;
            }
        // Some older Designer fields never set Control.Name (SCREEN0098 is one).
        // Recognize the actual field object without renaming it or breaking its bindings.
        var declaredRoots = new HashSet<Control>();
        for (Type? type = screen.GetType(); type is not null && type != typeof(Control); type = type.BaseType)
            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                if (ContainerNames.Contains(field.Name) && field.GetValue(screen) is Control control)
                    declaredRoots.Add(control);
        var candidates = Walk(screen).Where(c => declaredRoots.Contains(c) || ContainerNames.Contains(c.Name)
            || c.GetType().Name == "AuditCountersControl").ToArray();
        var roots = candidates.Where(c => !candidates.Any(p => p != c && p.Contains(c))).ToArray();
        // Legacy audit-tab source objects are retained, but only the shared footer
        // is displayed. Activating an old tab must not reveal a second audit display.
        bool IsInTab(Control control)
        {
            for (var parent = control.Parent; parent is not null && parent != screen; parent = parent.Parent)
                if (parent is TabPage) return true;
            return false;
        }
        var tabSources = roots.Where(IsInTab).ToHashSet();
        screen.SuspendLayout();
        try
        {
            var designed = DesignedFooter(screen);
            if (designed is not null && profile == AuditMetadataProfile.Standard) profile = designed.Profile;
            var footer = designed ?? new AuditMetadataControl { Name = "standardAuditMetadata", Dock = DockStyle.Fill, Margin = Padding.Empty };
            if (profile is AuditMetadataProfile.ModificationOnly or AuditMetadataProfile.CyanModification)
                footer.ModificationOnly(profile == AuditMetadataProfile.CyanModification);
            if (profile == AuditMetadataProfile.ReceiptReference) footer.ReceiptReference();
            var leaves = roots.SelectMany(r => r is Label or TextBoxBase ? new[] { r } : Walk(r))
                .Where(c => c is Label or TextBoxBase).Distinct().ToArray();
            // Reparenting must not detach the existing CurrencyManager while sources
            // pass through the retained panel before it is attached to the screen.
            foreach (var source in leaves.Where(c => c.DataBindings.Count > 0))
                source.BindingContext = source.BindingContext;
            var mapped = new HashSet<Control>();
            var bound = new HashSet<string>();
            // Prefer actual value editors over labels having the same semantic name.
            foreach (var source in leaves.OrderBy(c => c is TextBoxBase ? 0
                : c is Label && roots.Contains(c) && c.Text.Count(x => x == ':') > 1 ? 2 : 1))
            {
                if (source is Label && roots.Contains(source) && source.Text.Count(c => c == ':') > 1)
                {
                    var aggregateCaptions = Captions.Append(new KeyValuePair<string,string>("الجهاز","CreatedDevice"));
                    bool recognized = false;
                    foreach (var pair in aggregateCaptions)
                    {
                        if ((pair.Value is "AccountName" or "LastPrintedAt") && profile != AuditMetadataProfile.ReceiptReference) continue;
                        if (!Regex.IsMatch(source.Text, Regex.Escape(pair.Key) + @"\s*:")) continue;
                        recognized = true;
                        if (bound.Add(pair.Value)) footer.BindAggregateValue(pair.Value, source, pair.Key,
                            aggregateCaptions.Select(p => p.Key));
                    }
                    if (recognized) { mapped.Add(source); continue; }
                }
                string caption = source.Text.Split(':')[0].Trim();
                bool captionOnly = source is Label && Captions.ContainsKey(source.Text.Trim());
                if (captionOnly) { mapped.Add(source); continue; }
                string? key = Names.GetValueOrDefault(source.Name);
                if (key is null && source.AccessibleName is string accessible)
                    key = Captions.GetValueOrDefault(accessible.Trim());
                if (key is null && source is Label && source.Text.Contains(':')) key = Captions.GetValueOrDefault(caption);
                if (key is null && source is TextBoxBase && source.Parent is TableLayoutPanel table)
                {
                    var cell = table.GetPositionFromControl(source);
                    var label = table.GetControlFromPosition(Math.Max(0, cell.Column - 1), cell.Row) as Label;
                    if (label is not null) key = Captions.GetValueOrDefault(label.Text.Trim());
                }
                if ((key is "AccountName" or "LastPrintedAt") && profile != AuditMetadataProfile.ReceiptReference)
                    key = null; // Preserved by the additional metadata row in the standard layout.
                if (key is null) continue;
                mapped.Add(source);
                bool captioned = source is Label && Captions.TryGetValue(caption, out var captionKey) && captionKey == key;
                if (bound.Add(key)) footer.BindValue(key, source, captioned);
            }
            // Static placeholders are not record values; dynamic/unknown legacy metadata stays visible.
            var extras = leaves.Where(c => !mapped.Contains(c) && !c.Name.StartsWith("lblAuditCaption", StringComparison.Ordinal)
                && !(c.Name == "referenceAudit" && c.Text.Contains('—'))).ToArray();
            footer.AddLegacyDetails(extras);
            footer.MaximumSize = new Size(0, footer.Height);

            var primary = roots.FirstOrDefault(c => !tabSources.Contains(c) && (c is Panel || c is UserControl));
            var retained = new Panel { Name = "auditLegacySources", Visible = false, Size = Size.Empty, TabStop = false };
            if (designed is not null && (primary is null || !primary.Contains(designed)))
            {
                // The Designer owns the visible placement. Keep that same object
                // and retain the old source blocks without installing another one.
                foreach (var oldRoot in roots)
                {
                    if (oldRoot.Parent is TableLayoutPanel oldTable)
                    {
                        int row = oldTable.GetRow(oldRoot);
                        if (row >= 0 && row < oldTable.RowStyles.Count && oldTable.Controls.Cast<Control>().All(c => c == oldRoot || oldTable.GetRow(c) != row))
                        { oldTable.RowStyles[row].SizeType = SizeType.Absolute; oldTable.RowStyles[row].Height = 0; }
                    }
                    retained.Controls.Add(oldRoot);
                }
                primary = null;
                SizeHost(footer, footer.Height);
            }
            else if (primary is TableLayoutPanel tableHost)
            {
                footer.Margin = Padding.Empty;
                foreach (Control child in tableHost.Controls.Cast<Control>().Where(c => c != footer).ToArray()) retained.Controls.Add(child);
                tableHost.ColumnStyles.Clear(); tableHost.RowStyles.Clear();
                tableHost.ColumnCount = 1; tableHost.RowCount = 1;
                tableHost.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                tableHost.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                tableHost.Padding = Padding.Empty; tableHost.Margin = new Padding(1);
                tableHost.AutoSize = false; tableHost.BorderStyle = BorderStyle.None;
                tableHost.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
                tableHost.Controls.Add(footer, 0, 0);
                SizeHost(tableHost, footer.Height);
            }
            else if (primary is not null)
            {
                foreach (Control child in primary.Controls.Cast<Control>().Where(c => c != footer).ToArray()) retained.Controls.Add(child);
                primary.Padding = Padding.Empty;
                if(primary is Panel oldPanel) oldPanel.BorderStyle=BorderStyle.None;
                if(primary is UserControl oldControl) oldControl.BorderStyle=BorderStyle.None;
                primary.Controls.Add(footer);
                SizeHost(primary, footer.Height);
            }
            else if (roots.FirstOrDefault(r => !tabSources.Contains(r)) is Control old && old.Parent is Control parent)
            {
                footer.Dock = old.Dock;
                if (parent is TableLayoutPanel table)
                {
                    var cell = table.GetPositionFromControl(old);
                    int span = table.GetColumnSpan(old);
                    retained.Controls.Add(old);
                    table.Controls.Add(footer, cell.Column, cell.Row);
                    table.SetColumnSpan(footer, span);
                }
                else
                {
                    int index = parent.Controls.GetChildIndex(old);
                    footer.Bounds = old.Bounds;
                    retained.Controls.Add(old);
                    parent.Controls.Add(footer); parent.Controls.SetChildIndex(footer, index);
                }
                SizeHost(footer, footer.Height);
            }
            else AddToScreen(screen, footer);
            // Reserve the legacy bottom slot before its sibling Fill workspace.
            if (primary?.Dock == DockStyle.Bottom) primary.SendToBack();
            // A legacy slot inside a narrow work column must not crop the shared
            // identity fields when the screen itself has room for the full footer.
            if (primary is not null && footer.Parent == primary && primary.Width < 1000
                && screen.ClientSize.Width >= 1000)
            {
                primary.Controls.Remove(footer);
                primary.MinimumSize = Size.Empty; primary.Height = 0; primary.Visible = false;
                if (primary.Parent is TableLayoutPanel legacyLayout)
                {
                    int auditRow = legacyLayout.GetRow(primary);
                    if (auditRow >= 0 && auditRow < legacyLayout.RowStyles.Count
                        && legacyLayout.Controls.Cast<Control>().All(c => c == primary || legacyLayout.GetRow(c) != auditRow))
                    { legacyLayout.RowStyles[auditRow].SizeType = SizeType.Absolute; legacyLayout.RowStyles[auditRow].Height = 0; }
                }
                AddToScreen(screen, footer);
            }
            // Other aggregate audit blocks are retained, not duplicated visually.
            foreach (var extraRoot in roots.Where(r => r != primary && r.Parent != retained))
            {
                if (extraRoot.Parent is TableLayoutPanel table)
                {
                    int row = table.GetRow(extraRoot);
                    if (row >= 0 && row < table.RowStyles.Count && table.Controls.Cast<Control>().Count(c => table.GetRow(c) == row) == 1)
                    { table.RowStyles[row].SizeType = SizeType.Absolute; table.RowStyles[row].Height = 0; }
                }
                retained.Controls.Add(extraRoot);
            }
            footer.Controls.Add(retained);
            Applied.Add(screen, new object());
            KeepAuditSlotVisible(screen, footer, primary);
        }
        finally { screen.ResumeLayout(true); }
    }

    private static void KeepAuditSlotVisible(Control screen, AuditMetadataControl footer, Control? oldHost)
    {
        bool queued = false;
        bool moved = footer.Parent == screen;
        void Check()
        {
            queued = false;
            if (moved || screen.IsDisposed || footer.IsDisposed || !footer.Visible) return;
            var bounds = footer.RectangleToScreen(footer.ClientRectangle);
            var visible = bounds;
            for (Control? parent = footer.Parent; parent is not null; parent = parent.Parent)
            {
                visible = Rectangle.Intersect(visible, parent.RectangleToScreen(parent.ClientRectangle));
                if (parent == screen) break;
            }
            if (visible == bounds) return;
            // Root minimum/scroll extents belong to the screen layout. A footer
            // cannot reserve a fixed band there without changing that contract.
            if (screen is ScrollableControl scroll && scroll.AutoScroll
                && (scroll.DisplayRectangle.Height > screen.ClientSize.Height
                    || scroll.DisplayRectangle.Width > screen.ClientSize.Width)) return;
            if (screen.Controls.Cast<Control>().Any(c => c.Visible && c.Dock == DockStyle.Fill
                && (c.MinimumSize.Width > screen.ClientSize.Width
                    || c.MinimumSize.Height > screen.ClientSize.Height - footer.Height))) return;
            // Only a proven clipped audit slot is moved. Leave the scrolling work
            // area and its minimum size intact, and retain all source objects.
            moved = true;
            var slot = oldHost ?? footer;
            if (slot.Parent is TableLayoutPanel table)
            {
                int row = table.GetRow(slot);
                if (row >= 0 && row < table.RowStyles.Count && table.Controls.Cast<Control>().All(c => c == slot || table.GetRow(c) != row))
                { table.RowStyles[row].SizeType = SizeType.Absolute; table.RowStyles[row].Height = 0; }
            }
            if (oldHost is not null)
            { oldHost.MinimumSize = Size.Empty; oldHost.Height = 0; oldHost.Visible = false; }
            AddToScreen(screen, footer);
        }
        void Schedule(object? sender, EventArgs e)
        {
            if (queued || moved || screen.IsDisposed || !screen.IsHandleCreated) return;
            queued = true;
            screen.BeginInvoke((Action)Check);
        }
        screen.Layout += Schedule;
        footer.LocationChanged += Schedule;
        footer.SizeChanged += Schedule;
        Schedule(null, EventArgs.Empty);
    }

    private static void SizeHost(Control host, int height)
    {
        host.MinimumSize = new Size(0, height);
        host.Height = height;
        if (host.Parent is TableLayoutPanel table)
        {
            int row = table.GetRow(host);
            // This changes only the audit control's span, never the root columns or
            // other fields. Some legacy footers occupied half of an otherwise empty row.
            if (row >= 0 && table.ColumnCount > 1 && table.Controls.Cast<Control>()
                .All(c => c == host || table.GetRow(c) != row))
            {
                table.SetColumn(host, 0);
                table.SetColumnSpan(host, table.ColumnCount);
            }
            if (row >= 0 && row < table.RowStyles.Count)
            {
                table.RowStyles[row].SizeType = SizeType.Absolute;
                // The legacy row's grid border consumes a physical pixel at 125% DPI.
                // Reserve that border outside the shared footer's own client area.
                table.RowStyles[row].Height = height + host.Margin.Vertical + (int)Math.Ceiling(host.DeviceDpi / 96d);
            }
        }
    }

    private static void AddToScreen(Control screen, AuditMetadataControl footer)
    {
        // Screens without an audit slot get only this new docked component. Do not
        // rewrite their root grid, other row styles, form properties or command bar.
        footer.Dock = DockStyle.Bottom;
        screen.Controls.Add(footer);
        footer.SendToBack(); // Reserve footer bounds before the workspace's Fill child.
    }

}
