using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace TransportERP.Desktop.CoreUI;

/// <summary>A screen with real persistence owns its dirty-state decision.</summary>
public interface IWorkspaceChangeState
{
    bool HasUnsavedChanges { get; }
}

public interface IWorkspaceCloseGuard : IWorkspaceChangeState
{
    bool IsBusy { get; }
    bool ConfirmLeave();
}

/// <summary>
/// In-memory fallback for screens without an explicit persistence contract.
/// Compares editable values with opening values; never treats a Save caption as
/// proof of persistence. Reading a snapshot never commits or cancels an edit.
/// </summary>
public sealed class WorkspaceChangeSnapshot
{
    private readonly Control root;
    private readonly Dictionary<string, string> openingValues;

    public WorkspaceChangeSnapshot(Control root)
    {
        this.root = root ?? throw new ArgumentNullException(nameof(root));
        openingValues = Capture();
    }

    public bool HasChanges
    {
        get
        {
            if (root.IsDisposed || root.Disposing) return false;
            if (root is IWorkspaceChangeState owner) return owner.HasUnsavedChanges;
            Dictionary<string, string> current = Capture();
            if (current.Count != openingValues.Count) return true;
            foreach (KeyValuePair<string, string> item in openingValues)
                if (!current.TryGetValue(item.Key, out string? value)
                    || !StringComparer.Ordinal.Equals(item.Value, value)) return true;
            return false;
        }
    }

    private Dictionary<string, string> Capture()
    {
        var values = new Dictionary<string, string>(StringComparer.Ordinal);
        if (!root.IsDisposed && !root.Disposing) CaptureControl(root, "root", values);
        return values;
    }

    private static string ValueText(object? value)
        => value is null || value == DBNull.Value ? string.Empty
            : Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;

    private static void CaptureControl(Control control, string path, Dictionary<string, string> values)
    {
        if (control.IsDisposed || control.Disposing) return;
        switch (control)
        {
            case DataGridView grid:
                CaptureGrid(grid, path, values);
                return; // Framework editing controls belong to their cells.
            case TextBoxBase text:
                if (!text.ReadOnly) values[path + "/text"] = text.Text;
                return;
            case CheckedListBox list:
                for (int item = 0; item < list.Items.Count; item++)
                    values[path + "/item/" + item.ToString(CultureInfo.InvariantCulture)]
                        = ValueText((int)list.GetItemCheckState(item));
                return;
            case TreeView tree:
                if (tree.CheckBoxes) CaptureNodes(tree.Nodes, path, values);
                return;
            case ComboBox combo:
                values[path + "/text"] = combo.Text;
                values[path + "/index"] = ValueText(combo.SelectedIndex);
                return;
            case NumericUpDown number:
                // Reading NumericUpDown.Value validates pending input. Text is a
                // non-committing snapshot of the displayed value and pending edit.
                // Spinner changes are captured even when typing is ReadOnly.
                values[path + "/numberText"] = number.Text;
                return;
            case DateTimePicker date:
                values[path + "/date"] = date.Value.ToString("O", CultureInfo.InvariantCulture);
                values[path + "/checked"] = ValueText(date.Checked);
                return;
            case CheckBox check:
                values[path + "/check"] = ValueText((int)check.CheckState);
                return;
            case RadioButton radio:
                values[path + "/checked"] = ValueText(radio.Checked);
                return;
        }

        for (int index = 0; index < control.Controls.Count; index++)
        {
            Control child = control.Controls[index];
            // Index path is independent of missing/duplicate designer Names.
            CaptureControl(child, path + "/" + index.ToString(CultureInfo.InvariantCulture)
                + ":" + child.GetType().FullName, values);
        }
    }

    private static void CaptureNodes(TreeNodeCollection nodes, string path, Dictionary<string, string> values)
    {
        for (int index = 0; index < nodes.Count; index++)
        {
            TreeNode node = nodes[index];
            string nodePath = path + "/node/" + index.ToString(CultureInfo.InvariantCulture);
            values[nodePath] = ValueText(node.Checked);
            CaptureNodes(node.Nodes, nodePath, values);
        }
    }

    private static void CaptureGrid(DataGridView grid, string path, Dictionary<string, string> values)
    {
        if (grid.ReadOnly) return;
        int rows = 0;
        foreach (DataGridViewRow row in grid.Rows)
        {
            bool pendingNewRow = row.IsNewRow && grid.CurrentCell?.RowIndex == row.Index
                && grid.IsCurrentCellDirty;
            if (row.IsNewRow && !pendingNewRow) continue;
            string rowPath = path + "/row/" + rows.ToString(CultureInfo.InvariantCulture);
            rows++;
            foreach (DataGridViewCell cell in row.Cells)
            {
                if (cell.ReadOnly) continue;
                string cellPath = rowPath + "/cell/" + cell.ColumnIndex.ToString(CultureInfo.InvariantCulture);
                values[cellPath + "/value"] = ValueText(cell.Value);
                // Both baseline and subsequent snapshots use formatted text, so
                // merely entering edit mode is not itself a change.
                object? effective = cell.FormattedValue;
                if (ReferenceEquals(cell, grid.CurrentCell)
                    && (cell.IsInEditMode || grid.IsCurrentCellDirty))
                {
                    effective = grid.EditingControl is IDataGridViewEditingControl editor
                        ? editor.EditingControlFormattedValue : cell.EditedFormattedValue;
                }
                values[cellPath + "/editing"] = ValueText(effective);
            }
        }
        values[path + "/rows"] = ValueText(rows);
    }
}
