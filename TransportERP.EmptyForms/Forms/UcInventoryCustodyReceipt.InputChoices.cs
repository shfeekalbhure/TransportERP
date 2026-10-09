using System.Globalization;
using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;

public sealed record CustodyReceiptInvoiceLine(string ItemNumber, string ItemName, string Unit, decimal Quantity, decimal? FreeQuantity = null, decimal? Price = null, decimal? Value = null);
public sealed record CustodyReceiptInvoiceView(BatchFiveChoice Invoice, string CustomerId, string CustomerNumber, string CustomerName, string AccountNumber, BatchFiveChoice? Warehouse, BatchFiveChoice? Currency, string ExchangeRate, IReadOnlyList<CustodyReceiptInvoiceLine> Lines, decimal? Total = null);

public partial class UcInventoryCustodyReceipt
{
    private bool loadingInputChoices;
    private bool? descriptionRequired;
    private string? selectedInvoiceId, selectedCustomerId;
    private readonly Dictionary<DataGridViewRow, decimal> invoiceQuantities = new();
    private readonly Dictionary<string, BatchFiveChoice[]> manualChoices = new();
    private bool localInputChanged;
    public bool HasUnsavedChanges { get => localInputChanged || gridItems.IsCurrentCellDirty; private set => localInputChanged = value; }
    public bool IsInvoiceMode => selectedInvoiceId != null;
    public string? SelectedInvoiceId => selectedInvoiceId;
    public string? SelectedCustomerId => selectedCustomerId;
    public int CurrentItemRow => gridItems.CurrentCell?.RowIndex ?? -1;
    public bool DescriptionRequirementKnown => descriptionRequired.HasValue;
    public event Action<BatchFiveLookupRequest>? ReferenceLookupRequested;

    private void RequestReference(string key, Keys shortcut)
    {
        if (ReferenceLookupRequested == null) lblOperationStatus.Text = "قائمة الاختيار غير متاحة حاليًا.";
        ReferenceLookupRequested?.Invoke(new(key, shortcut));
    }
    private void ConfigureLocalInputs()
    {
        fieldSourceNumber.TabStop = true;
        fieldSourceNumber.KeyDown += (_, e) => { if (e.KeyCode == Keys.F9) { RequestReference("invoice", Keys.F9); e.Handled = true; e.SuppressKeyPress = true; } };
        fieldCustomerNumber.KeyDown += (_, e) => { if (e.KeyCode == Keys.F9 && !IsInvoiceMode) { RequestReference("customer", Keys.F9); e.Handled = true; e.SuppressKeyPress = true; } };
        fieldDescription.ReadOnly = false; fieldDescription.TabStop = true;
        fieldDescription.TextChanged += (_, _) => { if (!loadingInputChoices) HasUnsavedChanges = true; };
        foreach (var pair in new[] { (fieldWarehouse, "warehouse"), (fieldCurrency, "currency") })
        {
            pair.Item1.DisplayMember = nameof(BatchFiveChoice.Label); pair.Item1.ValueMember = nameof(BatchFiveChoice.Id);
            pair.Item1.SelectedIndexChanged += (_, _) =>
            {
                if (loadingInputChoices) return;
                HasUnsavedChanges = true;
                if (pair.Item1 == fieldCurrency) { fieldExchangeRate.Clear(); ClearCalculatedValues(); }
            };
            pair.Item1.KeyDown += (_, e) =>
            {
                if (e.KeyCode != Keys.F9 || IsInvoiceMode) return;
                RequestReference(pair.Item2, Keys.F9);
                if (pair.Item1.Items.Count > 0) pair.Item1.DroppedDown = true;
                e.Handled = true; e.SuppressKeyPress = true;
            };
        }
        gridItems.ReadOnly = false;
        foreach (var column in new[] { colQuantity, colFreeQuantity, colPrice }) column.ValueType = typeof(decimal);
        gridItems.CellValueChanged += (_, e) =>
        {
            if (loadingInputChoices || e.RowIndex < 0) return;
            if (e.ColumnIndex != colQuantity.Index && e.ColumnIndex != colFreeQuantity.Index && e.ColumnIndex != colPrice.Index && e.ColumnIndex != colItemNumber.Index) return;
            HasUnsavedChanges = true;
            gridItems.Rows[e.RowIndex].Cells[colValue.Index].Value = null; txtTotal.Clear();
            if (e.ColumnIndex == colItemNumber.Index)
            {
                gridItems.Rows[e.RowIndex].Cells[colItemName.Index].Value = null;
                gridItems.Rows[e.RowIndex].Cells[colUnit.Index].Value = null;
            }
        };
        gridItems.UserDeletedRow += (_, e) => { invoiceQuantities.Remove(e.Row); if (!loadingInputChoices) { HasUnsavedChanges = true; txtTotal.Clear(); } };
        gridItems.RowsRemoved += (_, _) => { if (!loadingInputChoices) { HasUnsavedChanges = true; txtTotal.Clear(); } };
        gridItems.KeyDown += (_, e) =>
        {
            if (e.KeyCode != Keys.F9 || IsInvoiceMode || gridItems.CurrentCell?.OwningColumn is not { } column) return;
            if (column != colItemNumber && column != colItemName && column != colItemLookup) return;
            RequestReference("itemNumber", Keys.F9); e.Handled = true; e.SuppressKeyPress = true;
        };
        gridItems.CellContentClick += (_, e) =>
        {
            if (IsInvoiceMode || e.RowIndex < 0 || e.ColumnIndex != colItemLookup.Index) return;
            gridItems.CurrentCell = gridItems.Rows[e.RowIndex].Cells[colItemNumber.Index]; RequestReference("itemNumber", Keys.None);
        };
        gridItems.DataError += (_, e) =>
        {
            if (e.RowIndex < 0 || (e.ColumnIndex != colQuantity.Index && e.ColumnIndex != colFreeQuantity.Index && e.ColumnIndex != colPrice.Index)) return;
            e.ThrowException = false; e.Cancel = true; gridItems.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "أدخل قيمة رقمية";
        };
        gridItems.CellValidating += (_, e) =>
        {
            if (loadingInputChoices || e.RowIndex < 0 || e.ColumnIndex != colQuantity.Index || gridItems.Rows[e.RowIndex].IsNewRow) return;
            bool valid = ValidQuantity(gridItems.Rows[e.RowIndex], e.FormattedValue);
            e.Cancel = !valid; gridItems.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = valid ? "" : QuantityError;
        };
        gridItems.CellEndEdit += (_, e) =>
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            var cell = gridItems.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (cell.Value is decimal && (e.ColumnIndex != colQuantity.Index || ValidQuantity(gridItems.Rows[e.RowIndex], cell.Value))) cell.ErrorText = "";
        };
        commandBar.Commands[StandardCommand.Add].Enabled = true;
        commandBar.Commands[StandardCommand.Add].Click += (_, _) =>
        {
            if (HasUnsavedChanges && MessageBox.Show(FindForm(), "توجد تعديلات محلية غير محفوظة. هل تريد بدء مسودة جديدة؟", "إذن توريد أمانات", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            loadingInputChoices = true;
            try
            {
                gridItems.CancelEdit(); gridItems.Rows.Clear(); invoiceQuantities.Clear(); selectedInvoiceId = null; selectedCustomerId = null;
                foreach (var editor in new[] { fieldSourceNumber, fieldCustomerNumber, fieldCustomerName, fieldAccountNumber, fieldExchangeRate, fieldDescription, txtTotal }) editor.Clear();
                RestoreManualChoices(fieldWarehouse, "warehouse"); RestoreManualChoices(fieldCurrency, "currency"); RefreshInputMode();
            }
            finally { loadingInputChoices = false; HasUnsavedChanges = false; }
        };
        RefreshInputMode(); SetDescriptionRequirement(null);
    }
    private void RefreshInputMode()
    {
        fieldCustomerNumber.TabStop = !IsInvoiceMode;
        fieldWarehouse.Enabled = !IsInvoiceMode; fieldWarehouse.TabStop = !IsInvoiceMode;
        fieldCurrency.Enabled = !IsInvoiceMode; fieldCurrency.TabStop = !IsInvoiceMode;
        gridItems.AllowUserToAddRows = !IsInvoiceMode; gridItems.AllowUserToDeleteRows = true;
        colItemNumber.ReadOnly = IsInvoiceMode; colPrice.ReadOnly = IsInvoiceMode; colFreeQuantity.ReadOnly = IsInvoiceMode;
        colQuantity.ReadOnly = false; colValue.ReadOnly = true; txtTotal.ReadOnly = true;
    }
    private void ClearCalculatedValues()
    {
        foreach (DataGridViewRow row in gridItems.Rows) if (!row.IsNewRow) row.Cells[colValue.Index].Value = null;
        txtTotal.Clear();
    }
    private void RestoreManualChoices(ComboBox editor, string key)
    {
        editor.Items.Clear(); if (manualChoices.TryGetValue(key, out var choices)) editor.Items.AddRange(choices); editor.SelectedIndex = -1;
    }
    public void SetManualChoices(string key, IEnumerable<BatchFiveChoice> choices)
    {
        var editor = key switch { "warehouse" => fieldWarehouse, "currency" => fieldCurrency, _ => throw new ArgumentException("Unknown manual lookup.", nameof(key)) };
        var supplied = choices.ToArray();
        if (supplied.Select(c => c.Id).Distinct(StringComparer.Ordinal).Count() != supplied.Length) throw new ArgumentException("Duplicate choice identity.", nameof(choices));
        manualChoices[key] = supplied; if (IsInvoiceMode) return;
        var selected = (editor.SelectedItem as BatchFiveChoice)?.Id;
        loadingInputChoices = true;
        try { RestoreManualChoices(editor, key); if (selected != null) editor.SelectedItem = supplied.FirstOrDefault(c => c.Id == selected); }
        finally { loadingInputChoices = false; }
        if (selected != null && (editor.SelectedItem as BatchFiveChoice)?.Id != selected)
        { HasUnsavedChanges = true; if (key == "currency") { fieldExchangeRate.Clear(); ClearCalculatedValues(); } }
    }
    public bool SetManualCustomer(string id, string number, string name, string accountNumber)
    {
        if (IsInvoiceMode) return false;
        if (selectedCustomerId == id && fieldCustomerNumber.Text == number && fieldCustomerName.Text == name && fieldAccountNumber.Text == accountNumber) return true;
        selectedCustomerId = id; fieldCustomerNumber.Text = number; fieldCustomerName.Text = name; fieldAccountNumber.Text = accountNumber; HasUnsavedChanges = true; return true;
    }
    public bool SetManualExchangeRate(string currencyId, decimal? rate)
    {
        if (IsInvoiceMode || (fieldCurrency.SelectedItem as BatchFiveChoice)?.Id != currencyId) return false;
        var text = rate?.ToString(CultureInfo.CurrentCulture) ?? "";
        if (fieldExchangeRate.Text != text) { fieldExchangeRate.Text = text; HasUnsavedChanges = true; ClearCalculatedValues(); }
        return true;
    }
    public bool SetManualItemSelection(int rowIndex, string number, string name, string unit)
    {
        if (IsInvoiceMode) return false;
        if (rowIndex < 0 || rowIndex >= gridItems.Rows.Count) throw new ArgumentOutOfRangeException(nameof(rowIndex));
        loadingInputChoices = true;
        try
        {
            if (gridItems.Rows[rowIndex].IsNewRow) rowIndex = gridItems.Rows.Add();
            var row = gridItems.Rows[rowIndex];
            bool same = Convert.ToString(row.Cells[colItemNumber.Index].Value) == number && Convert.ToString(row.Cells[colItemName.Index].Value) == name && Convert.ToString(row.Cells[colUnit.Index].Value) == unit;
            if (same) return true;
            row.Cells[colSequence.Index].Value = rowIndex + 1;
            row.Cells[colItemNumber.Index].Value = number; row.Cells[colItemName.Index].Value = name; row.Cells[colUnit.Index].Value = unit;
            foreach (var column in new[] { colQuantity, colFreeQuantity, colPrice, colValue }) row.Cells[column.Index].Value = null;
            txtTotal.Clear(); HasUnsavedChanges = true; return true;
        }
        finally { loadingInputChoices = false; }
    }
    public bool ApplyInvoiceSelection(CustodyReceiptInvoiceView invoice, Func<bool>? confirmReplace = null)
    {
        if (invoice.Lines.Any(line => line.Quantity < 0)) throw new ArgumentException("Negative invoice quantity.", nameof(invoice));
        if (HasUnsavedChanges && confirmReplace?.Invoke() != true) return false;
        loadingInputChoices = true;
        try
        {
            gridItems.CancelEdit(); gridItems.Rows.Clear(); invoiceQuantities.Clear(); selectedInvoiceId = invoice.Invoice.Id; selectedCustomerId = invoice.CustomerId;
            fieldSourceNumber.Text = invoice.Invoice.Label; fieldCustomerNumber.Text = invoice.CustomerNumber; fieldCustomerName.Text = invoice.CustomerName; fieldAccountNumber.Text = invoice.AccountNumber; fieldExchangeRate.Text = invoice.ExchangeRate;
            foreach (var pair in new[] { (fieldWarehouse, invoice.Warehouse), (fieldCurrency, invoice.Currency) })
            { pair.Item1.Items.Clear(); if (pair.Item2 != null) { pair.Item1.Items.Add(pair.Item2); pair.Item1.SelectedIndex = 0; } }
            RefreshInputMode();
            foreach (var line in invoice.Lines)
            {
                var row = gridItems.Rows[gridItems.Rows.Add()]; row.Cells[colSequence.Index].Value = row.Index + 1;
                row.Cells[colItemNumber.Index].Value = line.ItemNumber; row.Cells[colItemName.Index].Value = line.ItemName; row.Cells[colUnit.Index].Value = line.Unit;
                row.Cells[colQuantity.Index].Value = line.Quantity; row.Cells[colFreeQuantity.Index].Value = line.FreeQuantity; row.Cells[colPrice.Index].Value = line.Price; row.Cells[colValue.Index].Value = line.Value;
                invoiceQuantities.Add(row, line.Quantity);
            }
            txtTotal.Text = invoice.Total?.ToString(CultureInfo.CurrentCulture) ?? "";
        }
        finally { loadingInputChoices = false; }
        HasUnsavedChanges = true; return true;
    }
    private bool ValidQuantity(DataGridViewRow row, object? value) => decimal.TryParse(Convert.ToString(value, CultureInfo.CurrentCulture), NumberStyles.Number, CultureInfo.CurrentCulture, out var quantity)
        && quantity >= 0 && (!IsInvoiceMode || invoiceQuantities.TryGetValue(row, out var maximum) && quantity <= maximum);
    private string QuantityError => IsInvoiceMode ? "أدخل كمية صحيحة ضمن الكمية المتاحة في الفاتورة" : "أدخل كمية رقمية غير سالبة";
    public bool ValidateLocalInputs()
    {
        if (!gridItems.EndEdit()) return false;
        var rows = gridItems.Rows.Cast<DataGridViewRow>().Where(r => !r.IsNewRow).ToArray();
        bool valid = rows.Length > 0 && (descriptionRequired != true || !string.IsNullOrWhiteSpace(fieldDescription.Text));
        foreach (var row in rows)
        {
            bool rowValid = ValidQuantity(row, row.Cells[colQuantity.Index].Value);
            row.Cells[colQuantity.Index].ErrorText = rowValid ? "" : QuantityError;
            valid &= rowValid && !string.IsNullOrWhiteSpace(Convert.ToString(row.Cells[colItemNumber.Index].Value));
        }
        return valid;
    }
    public void SetDescriptionRequirement(bool? required)
    {
        descriptionRequired = required;
        global::TransportERP.RequiredFieldAppearance.Apply(fieldDescription, required);
        lblDescription.Text = required == true ? "البيان *" : "البيان";
    }
    public void AcceptInputBaseline() => HasUnsavedChanges = false;
}
