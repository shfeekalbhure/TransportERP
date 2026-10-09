using System.Globalization;
using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;

public partial class UcInventoryExternalRepairOrder
{
    private bool loadingInputChoices;
    public bool HasUnsavedChanges { get; private set; }
    public event Action<BatchFiveLookupRequest>? ReferenceLookupRequested;
    public string? SelectedWarehouseId => (fieldWarehouse.SelectedItem as BatchFiveChoice)?.Id;
    public int CurrentItemRow => gridItems.CurrentCell?.RowIndex ?? -1;

    private void ConfigureLocalInputs()
    {
        fieldWarehouse.Enabled = true;
        fieldWarehouse.TabStop = true;
        fieldWarehouse.DisplayMember = nameof(BatchFiveChoice.Label);
        fieldWarehouse.ValueMember = nameof(BatchFiveChoice.Id);
        fieldWarehouse.SelectedIndexChanged += (_, _) => MarkInputChanged();
        fieldWarehouse.KeyDown += (_, e) =>
        {
            if (e.KeyCode != Keys.F9) return;
            ReferenceLookupRequested?.Invoke(new("warehouse", Keys.F9));
            if (fieldWarehouse.Items.Count > 0) fieldWarehouse.DroppedDown = true;
            e.Handled = true; e.SuppressKeyPress = true;
        };
        fieldOrderDate.Enabled = true;
        fieldOrderDate.TabStop = true;
        fieldOrderDate.ValueChanged += (_, _) => MarkInputChanged();
        foreach (var editor in new[] { fieldReference, fieldDescription })
        {
            editor.ReadOnly = false;
            editor.TabStop = true;
            editor.TextChanged += (_, _) => MarkInputChanged();
        }
        gridItems.ReadOnly = false;
        gridItems.AllowUserToAddRows = true;
        gridItems.AllowUserToDeleteRows = true;
        foreach (var column in new[] { colItemNumber, colQuantity, colReason, colNotes, colDepartureDate }) column.ReadOnly = false;
        // Manual p74: return date is entered when editing the original order after return.
        // This unbound screen represents a new local draft, not a loaded existing order.
        colReturnDate.ReadOnly = true;
        colCost.ReadOnly = true;
        colQuantity.ValueType = typeof(decimal);
        gridItems.DataError += (_, e) =>
        {
            if (e.RowIndex < 0 || e.ColumnIndex != colQuantity.Index) return;
            e.ThrowException = false; e.Cancel = true;
            gridItems.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "أدخل كمية رقمية";
        };
        gridItems.CellEndEdit += (_, e) =>
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == colQuantity.Index && gridItems.Rows[e.RowIndex].Cells[e.ColumnIndex].Value is decimal)
                gridItems.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "";
        };
        gridItems.CellValueChanged += (_, e) => { if (e.RowIndex >= 0) MarkInputChanged(); };
        gridItems.UserDeletedRow += (_, _) => MarkInputChanged();
        gridItems.KeyDown += (_, e) =>
        {
            if (e.KeyCode != Keys.F9 || gridItems.CurrentCell?.OwningColumn is not { } column) return;
            if (column != colItemNumber && column != colItemName && column != colItemLookup) return;
            ReferenceLookupRequested?.Invoke(new("itemNumber", Keys.F9));
            e.Handled = true; e.SuppressKeyPress = true;
        };
        gridItems.CellContentClick += (_, e) =>
        {
            if (e.RowIndex < 0 || e.ColumnIndex != colItemLookup.Index) return;
            gridItems.CurrentCell = gridItems.Rows[e.RowIndex].Cells[colItemNumber.Index];
            ReferenceLookupRequested?.Invoke(new("itemNumber", Keys.None));
        };
        gridItems.CellValidating += (_, e) =>
        {
            if (e.RowIndex < 0 || gridItems.Rows[e.RowIndex].IsNewRow) return;
            var text = Convert.ToString(e.FormattedValue);
            bool valid = true;
            string message = "";
            if (e.ColumnIndex == colQuantity.Index)
            {
                valid = decimal.TryParse(text, NumberStyles.Number, CultureInfo.CurrentCulture, out var quantity);
                message = "أدخل كمية رقمية";
            }
            else if (e.ColumnIndex == colDepartureDate.Index)
            {
                valid = string.IsNullOrWhiteSpace(text) || DateTime.TryParse(text, CultureInfo.CurrentCulture, DateTimeStyles.None, out var departureDate);
                message = "أدخل تاريخ خروج صحيحًا";
            }
            e.Cancel = !valid;
            gridItems.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = valid ? "" : message;
        };
        commandBar.Commands[StandardCommand.Add].Enabled = true;
        commandBar.Commands[StandardCommand.Add].Click += (_, _) =>
        {
            if (HasUnsavedChanges && MessageBox.Show(FindForm(), "توجد تعديلات محلية غير محفوظة. هل تريد بدء مسودة جديدة؟", "أمر إصلاح خارجي", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            loadingInputChoices = true;
            try
            {
                gridItems.CancelEdit(); gridItems.Rows.Clear();
                fieldWarehouse.SelectedIndex = -1;
                fieldReference.Clear(); fieldDescription.Clear(); fieldOrderDate.Value = DateTime.Today;
            }
            finally { loadingInputChoices = false; HasUnsavedChanges = false; }
        };
    }

    private void MarkInputChanged() { if (!loadingInputChoices) HasUnsavedChanges = true; }
    public void SetWarehouseChoices(IEnumerable<BatchFiveChoice> choices)
    {
        var supplied = choices.ToArray();
        if (supplied.Select(c => c.Id).Distinct(StringComparer.Ordinal).Count() != supplied.Length) throw new ArgumentException("Duplicate choice identity.", nameof(choices));
        var selectedId = SelectedWarehouseId;
        loadingInputChoices = true;
        try
        {
            fieldWarehouse.Items.Clear(); fieldWarehouse.Items.AddRange(supplied); fieldWarehouse.SelectedIndex = -1;
            if (selectedId != null) fieldWarehouse.SelectedItem = supplied.FirstOrDefault(c => c.Id == selectedId);
        }
        finally { loadingInputChoices = false; }
        if (selectedId != null && selectedId != SelectedWarehouseId) HasUnsavedChanges = true;
    }
    public void SetItemSelection(int rowIndex, string itemNumber, string itemName, string unit)
    {
        if (rowIndex < 0 || rowIndex >= gridItems.Rows.Count) throw new ArgumentOutOfRangeException(nameof(rowIndex));
        if (gridItems.Rows[rowIndex].IsNewRow) rowIndex = gridItems.Rows.Add();
        var row = gridItems.Rows[rowIndex];
        row.Cells[colItemNumber.Index].Value = itemNumber;
        row.Cells[colItemName.Index].Value = itemName;
        row.Cells[colUnit.Index].Value = unit;
    }
    public void AcceptInputBaseline() => HasUnsavedChanges = false;
}
