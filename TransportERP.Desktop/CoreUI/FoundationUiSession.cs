using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TransportERP.Desktop.CoreUI;

public interface IFoundationScreen : IWorkspaceCloseGuard { FoundationUiSession Foundation { get; } }
public sealed record FoundationChoice(string Id, string Label) { public override string ToString() => Label; }
public sealed record FoundationRequest(string Action, IReadOnlyDictionary<string, object?> Fields,
    IReadOnlyDictionary<string, IReadOnlyList<IReadOnlyDictionary<string, object?>>> Tables, string? Version, IReadOnlyDictionary<string, IReadOnlyList<string>>? CheckedTreePaths = null);
public sealed record FoundationResult(bool Success, bool Persisted, string Message, string? Version = null, Action? ApplyValidatedView = null);

/// <summary>UI-only binding contract. Control names are UI keys, never inferred database fields.</summary>
public sealed class FoundationUiSession
{
    private readonly Control root;
    private readonly Dictionary<string, Control> editors;
    private readonly Dictionary<string, DataGridView> grids;
    private readonly Dictionary<string, Button> actions;
    private readonly Dictionary<string, object?> initial;
    public IReadOnlyDictionary<string, Control> Controls { get; }
    private readonly Dictionary<string, string[]> requiredFields = new();
    private string baseline;
    private string? version;
    private Form? nativeHost;
    private Func<FoundationRequest, Task<FoundationResult>>? handler;
    private HashSet<string> allowed = new();
    private HashSet<string> replacesView = new();
    public bool IsBusy { get; private set; }
    private bool forcedDirty;
    public bool HasCommandBinding => handler != null;
    public bool HasUnsavedChanges => forcedDirty || Snapshot() != baseline || grids.Values.Any(g => g.IsCurrentCellDirty);
    public Func<string, string?>? ValidateAction { get; set; }
    public event EventHandler? DocumentLoaded;
    public FoundationUiSession(Control root, bool bindDisabledActions = true)
    {
        this.root = root;
        // Audit mirrors are presentation fields. Retained legacy source controls
        // remain part of the original screen contract and must still be collected.
        var controls = Walk(root).Where(c => !AuditMetadataControl.IsPresentationControl(c)).ToArray();
        Controls = KeyControls(controls.Where(c => c.Name.Length > 0));
        editors = KeyControls(controls.Where(c => !string.IsNullOrEmpty(c.Name) && c is TextBoxBase or ComboBox or CheckBox or DateTimePicker or NumericUpDown or CheckedListBox));
        grids = KeyControls(controls.OfType<DataGridView>().Where(c => c.Name.Length > 0));
        actions = KeyControls(controls.OfType<Button>().Where(b => bindDisabledActions && !b.Enabled && b.Name.Length > 0));
        root.HandleCreated += (_, _) => AttachNativeHost();
        root.ParentChanged += (_, _) => AttachNativeHost();
        root.Disposed += (_, _) => { if (nativeHost != null) nativeHost.FormClosing -= NativeClosing; };
        initial = CaptureFields();
        baseline = Snapshot();
        foreach (var pair in actions)
        {
            string key = pair.Key;
            pair.Value.Click += async (_, _) => await RunAsync(key);
        }
    }
    private void AttachNativeHost()
    {
        var form = root.FindForm();
        if (form == null || form.GetType().Name == "FrmMain" || form == nativeHost) return;
        if (nativeHost != null) nativeHost.FormClosing -= NativeClosing;
        nativeHost = form; nativeHost.FormClosing += NativeClosing;
    }
    private void NativeClosing(object? sender, FormClosingEventArgs e) { if (!e.Cancel) e.Cancel = !ConfirmLeave(); }
    private static Dictionary<string, T> KeyControls<T>(IEnumerable<T> controls) where T : Control
    {
        var result = new Dictionary<string, T>();
        foreach (var control in controls)
        {
            string key = control.Name;
            if (result.ContainsKey(key))
            {
                for (Control? parent = control.Parent; parent != null; parent = parent.Parent) key = parent.Name + "/" + key;
                if (result.ContainsKey(key)) throw new InvalidOperationException("Duplicate control path: " + key);
            }
            result.Add(key, control);
        }
        return result;
    }
    private static IEnumerable<Control> Walk(Control parent)
    {
        foreach (Control c in parent.Controls) { yield return c; foreach (var child in Walk(c)) yield return child; }
    }
    private static object? Read(Control c) => c switch
    {
        ComboBox x => x.SelectedItem is FoundationChoice choice ? choice.Id : x.SelectedItem?.ToString() ?? x.Text,
        CheckBox x => x.ThreeState ? (object)x.CheckState : x.Checked, NumericUpDown x => x.Value,
        DateTimePicker x => x.ShowCheckBox && !x.Checked ? null : x.Value,
        CheckedListBox x => x.CheckedItems.Cast<object>().Select(i => i is FoundationChoice ch ? ch.Id : i.ToString()).ToArray(),
        _ => c.Text
    };
    public IReadOnlyCollection<string> FieldKeys => editors.Keys;
    public IReadOnlyCollection<string> GridKeys => grids.Keys;
    public IReadOnlyCollection<string> ActionKeys => actions.Keys;
    public Dictionary<string, object?> CaptureFields() => editors.ToDictionary(p => p.Key, p => Read(p.Value));
    public Dictionary<string, IReadOnlyList<IReadOnlyDictionary<string, object?>>> CaptureTables() => grids.ToDictionary(p => p.Key,
        p => (IReadOnlyList<IReadOnlyDictionary<string, object?>>)p.Value.Rows.Cast<DataGridViewRow>().Where(r => !r.IsNewRow)
        .Select(r => (IReadOnlyDictionary<string, object?>)p.Value.Columns.Cast<DataGridViewColumn>().ToDictionary(c => c.Name, c => r.Cells[c.Index].Value)).ToArray());
    private string Snapshot() => System.Text.Json.JsonSerializer.Serialize(new { Fields = CaptureFields(), Tables = CaptureTables(),
        Trees = Controls.Where(p => p.Value is TreeView).ToDictionary(p => p.Key, p => TreeValues(((TreeView)p.Value).Nodes).ToArray()) });
    private static IEnumerable<string> TreeValues(TreeNodeCollection nodes)
    {
        foreach (TreeNode n in nodes) { yield return n.FullPath + "=" + n.Checked; foreach (var child in TreeValues(n.Nodes)) yield return child; }
    }
    private static IEnumerable<string> CheckedPaths(TreeNodeCollection nodes)
    {
        foreach (TreeNode n in nodes) { if (n.Checked) yield return n.FullPath; foreach (var child in CheckedPaths(n.Nodes)) yield return child; }
    }
    public void RequireFields(IEnumerable<string> actionKeys, params string[] fieldKeys)
    {
        if (fieldKeys.Any(k => !editors.ContainsKey(k))) throw new ArgumentException("Unknown requiredFields editor.");
        foreach (string action in actionKeys) requiredFields[action] = fieldKeys;
    }

    public void BindCommands(Func<FoundationRequest, Task<FoundationResult>> execute, IEnumerable<string> actionKeys, IEnumerable<string>? replacesViewKeys = null)
    {
        if (IsBusy) throw new InvalidOperationException("An operation is active.");
        var keys = actionKeys.ToHashSet();
        if (keys.Any(k => !actions.ContainsKey(k))) throw new ArgumentException("Unknown UI action.");
        handler = execute ?? throw new ArgumentNullException(nameof(execute)); allowed = keys;
        replacesView = (replacesViewKeys ?? keys.Where(k => new[] { "View", "Search", "Refresh", "New", "First", "Previous", "Next", "Last", "Undo", "Reset", "Revert" }.Any(part => k.Contains(part, StringComparison.OrdinalIgnoreCase)))).ToHashSet();
        foreach (var p in actions) p.Value.Enabled = allowed.Contains(p.Key);
    }
    public bool ConfirmLeave()
    {
        if (IsBusy) { MessageBox.Show(root, "انتظر اكتمال العملية الحالية."); return false; }
        foreach (var grid in grids.Values) if (!grid.EndEdit()) return false;
        return !HasUnsavedChanges || MessageBox.Show(root, "توجد تعديلات غير محفوظة. تجاهلها؟", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
    }
    public void SetChoices(string key, IEnumerable<FoundationChoice> choices)
    {
        if (IsBusy) throw new InvalidOperationException("An operation is active.");
        if (!editors.TryGetValue(key, out var control) || control is not ComboBox combo) throw new ArgumentException("Unknown choice editor.");
        var list = choices.ToArray();
        if (list.Any(i => string.IsNullOrWhiteSpace(i.Id)) || list.Select(i => i.Id).Distinct().Count() != list.Length) throw new ArgumentException("Choice IDs must be unique.");
        var previous = combo.SelectedItem as FoundationChoice;
        if (previous != null && !list.Any(i => i.Id == previous.Id)) throw new ArgumentException("Current selected ID is absent.");
        bool clean = !HasUnsavedChanges;
        combo.DataSource = list;
        combo.SelectedItem = previous == null ? null : list.Single(i => i.Id == previous.Id);
        if (clean) baseline = Snapshot();
    }
    public void SetReadOnly(string key, bool readOnly)
    {
        if (IsBusy) throw new InvalidOperationException("An operation is active.");
        if (!editors.TryGetValue(key, out var editor)) throw new ArgumentException("Unknown editor.");
        if (editor is TextBoxBase text) text.ReadOnly = readOnly; else editor.Enabled = !readOnly;
    }
    public bool LoadFields(IReadOnlyDictionary<string, object?> values, string? documentVersion = null)
    {
        foreach (var p in values)
        {
            if (!editors.TryGetValue(p.Key, out var c)) throw new ArgumentException("Unknown editor: " + p.Key);
            if (c is NumericUpDown n && (Convert.ToDecimal(p.Value) < n.Minimum || Convert.ToDecimal(p.Value) > n.Maximum)) throw new ArgumentOutOfRangeException(p.Key);
            if (c is DateTimePicker d && p.Value != null && (Convert.ToDateTime(p.Value) < d.MinDate || Convert.ToDateTime(p.Value) > d.MaxDate)) throw new ArgumentOutOfRangeException(p.Key);
            if (c is CheckBox && p.Value is not bool && p.Value is not CheckState) throw new ArgumentException("Boolean requiredFields: " + p.Key);
            if (c is CheckedListBox) throw new ArgumentException("Use the checklist binding contract for: " + p.Key);
            if (c is ComboBox b && p.Value != null && p.Value.ToString()!.Length > 0 && !b.Items.Cast<object>().Any(i => (i is FoundationChoice ch ? ch.Id : i.ToString()) == p.Value.ToString())) throw new ArgumentException("Unknown choice: " + p.Key);
        }
        if (IsBusy) return false;
        bool wasClean = !HasUnsavedChanges;
        if (!wasClean && documentVersion != null && documentVersion != version)
            throw new InvalidOperationException("Use LoadView to replace a document with a different version.");
        if (!wasClean && !ConfirmLeave()) return false;
        foreach (var p in values)
        {
            var c = editors[p.Key];
            switch(c)
            {
                case ComboBox b: b.SelectedItem = b.Items.Cast<object>().FirstOrDefault(i => (i is FoundationChoice ch ? ch.Id : i.ToString()) == p.Value?.ToString()); break;
                case CheckBox b: if (p.Value is CheckState state) b.CheckState = state; else b.Checked = (bool)p.Value!; break;
                case NumericUpDown n: n.Value = Convert.ToDecimal(p.Value); break;
                case DateTimePicker d: if (p.Value == null) d.Checked = false; else { d.Value = Convert.ToDateTime(p.Value); d.Checked = true; } break;
                default: c.Text = p.Value?.ToString() ?? ""; break;
            }
        }
        if (wasClean) { version = documentVersion; baseline = Snapshot(); } return true;
    }
    /// <summary>Adapter supplies a prevalidated complete view (grids, tree, checklist and field data).
    /// Returning false preserves the current view. The adapter must validate its payload before calling.</summary>
    public bool LoadView(Action populateValidatedView, string? documentVersion = null)
    {
        if (!ConfirmLeave()) return false;
        populateValidatedView();
        version = documentVersion;
        forcedDirty = false;
        DocumentLoaded?.Invoke(this, EventArgs.Empty);
        baseline = Snapshot();
        return true;
    }
    public async Task RunAsync(string action)
    {
        if (IsBusy || handler == null || !allowed.Contains(action)) return;
        foreach (var grid in grids.Values) if (!grid.EndEdit()) return;
        if (replacesView.Contains(action) && !ConfirmLeave()) return;
        if (requiredFields.TryGetValue(action, out var requiredKeys))
        {
            foreach (string key in requiredKeys)
                if (string.IsNullOrWhiteSpace(Read(editors[key])?.ToString()))
                {
                    Control c = editors[key];
                    for (Control? parent = c.Parent; parent != null; parent = parent.Parent)
                        if (parent is TabPage page && page.Parent is TabControl tabs) tabs.SelectedTab = page;
                    c.Focus(); MessageBox.Show(root, "أكمل الحقل المطلوب: " + (c.AccessibleName ?? key)); return;
                }
        }
        string? error = ValidateAction?.Invoke(action);
        if (error != null) { MessageBox.Show(root, error); return; }
        if ((action.Contains("Delete", StringComparison.OrdinalIgnoreCase) || action.Contains("Approve", StringComparison.OrdinalIgnoreCase) || action.Contains("Publish", StringComparison.OrdinalIgnoreCase) || action.Contains("Reopen", StringComparison.OrdinalIgnoreCase) || action.Contains("Execute", StringComparison.OrdinalIgnoreCase) || action.Contains("Reject", StringComparison.OrdinalIgnoreCase) || action.Contains("Return", StringComparison.OrdinalIgnoreCase))
            && MessageBox.Show(root, "تأكيد تنفيذ الإجراء؟", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        var request = new FoundationRequest(action, CaptureFields(), CaptureTables(), version, Controls.Where(p => p.Value is TreeView).ToDictionary(p => p.Key, p => (IReadOnlyList<string>)CheckedPaths(((TreeView)p.Value).Nodes).ToArray()));
        IsBusy = true; root.Enabled = false;
        try
        {
            var result = await handler(request);
            if (result.Success && result.ApplyValidatedView != null)
            {
                // Payload is validated by the adapter before returning. Dirty discard was
                // confirmed before replacement requests, and controls stayed frozen meanwhile.
                IsBusy = false;
                if (!replacesView.Contains(action) && !result.Persisted && !ConfirmLeave()) return;
                result.ApplyValidatedView();
                DocumentLoaded?.Invoke(this, EventArgs.Empty);
                version = result.Version; baseline = Snapshot();
                forcedDirty = !result.Persisted && !replacesView.Contains(action);
            }
            if (result.Success && result.Persisted) { version = result.Version; baseline = Snapshot(); forcedDirty = false; }
            MessageBox.Show(root, result.Message);
        }
        catch (Exception ex) { MessageBox.Show(root, "تعذر إكمال العملية: " + ex.Message); }
        finally { IsBusy = false; if (!root.IsDisposed) root.Enabled = true; }
    }
}
