using System.Globalization;
using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;

public sealed record CustodyIssueSourceLine(string ItemNumber, string ItemName, string Unit, decimal Quantity, decimal? FreeQuantity = null, decimal? Price = null, decimal? Value = null);
public sealed record CustodyIssueReceiptView(BatchFiveChoice Receipt, string CustomerNumber, string CustomerName, string AccountNumber, BatchFiveChoice? Warehouse, BatchFiveChoice? Currency, string ExchangeRate, IReadOnlyList<CustodyIssueSourceLine> Lines);

public partial class UcInventoryCustodyIssue
{
    private bool loadingInputChoices;
    private string? selectedReceiptId;
    private readonly Dictionary<DataGridViewRow, decimal> sourceQuantities = new();
    public bool HasUnsavedChanges { get; private set; }
    public string? SelectedReceiptId => selectedReceiptId;
    public event Action<BatchFiveLookupRequest>? ReferenceLookupRequested;

    private void ConfigureLocalInputs()
    {
        fieldSourceNumber.TabStop = true;
        fieldSourceNumber.KeyDown += (_, e) =>
        {
            if (e.KeyCode != Keys.F9) return;
            if (ReferenceLookupRequested == null) lblOperationStatus.Text = "قائمة أذون التوريد غير متاحة حاليًا.";
            ReferenceLookupRequested?.Invoke(new("custodyReceipt", Keys.F9));
            e.Handled = true; e.SuppressKeyPress = true;
        };
        fieldDescription.ReadOnly = false; fieldDescription.TabStop = true;
        fieldDescription.TextChanged += (_, _) => { if (!loadingInputChoices) HasUnsavedChanges = true; };
        gridItems.ReadOnly = false; colQuantity.ReadOnly = false; colQuantity.ValueType = typeof(decimal);
        // Lines originate from the selected receipt; no unrelated manual rows are introduced.
        gridItems.AllowUserToAddRows = false; gridItems.AllowUserToDeleteRows = false;
        gridItems.CellValueChanged += (_, e) =>
        {
            if (loadingInputChoices || e.RowIndex < 0 || e.ColumnIndex != colQuantity.Index) return;
            HasUnsavedChanges = true;
            gridItems.Rows[e.RowIndex].Cells[colValue.Index].Value = null;
            txtTotal.Clear();
        };
        gridItems.DataError += (_, e) =>
        {
            if (e.RowIndex < 0 || e.ColumnIndex != colQuantity.Index) return;
            e.ThrowException = false; e.Cancel = true;
            gridItems.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "أدخل كمية رقمية";
        };
        gridItems.CellValidating += (_, e) =>
        {
            if (loadingInputChoices || e.RowIndex < 0 || e.ColumnIndex != colQuantity.Index) return;
            var row = gridItems.Rows[e.RowIndex];
            var valid = QuantityInReceipt(row, e.FormattedValue);
            e.Cancel = !valid;
            row.Cells[e.ColumnIndex].ErrorText = valid ? "" : "أدخل كمية ضمن كمية إذن التوريد المختار";
        };
        gridItems.CellEndEdit += (_, e) =>
        {
            if (e.RowIndex < 0 || e.ColumnIndex != colQuantity.Index) return;
            var row = gridItems.Rows[e.RowIndex];
            if (QuantityInReceipt(row, row.Cells[e.ColumnIndex].Value)) row.Cells[e.ColumnIndex].ErrorText = "";
        };
        commandBar.Commands[StandardCommand.Add].Enabled = true;
        commandBar.Commands[StandardCommand.Add].Click += (_, _) =>
        {
            if (HasUnsavedChanges && MessageBox.Show(FindForm(), "توجد تعديلات محلية غير محفوظة. هل تريد بدء مسودة جديدة؟", "إذن صرف أمانات", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            loadingInputChoices = true;
            try
            {
                gridItems.CancelEdit(); gridItems.Rows.Clear(); sourceQuantities.Clear(); selectedReceiptId = null;
                foreach (var editor in new[] { fieldSourceNumber, fieldCustomerNumber, fieldCustomerName, fieldAccountNumber, fieldExchangeRate, fieldDescription, txtTotal }) editor.Clear();
                fieldWarehouse.Items.Clear(); fieldCurrency.Items.Clear();
            }
            finally { loadingInputChoices = false; HasUnsavedChanges = false; }
        };
    }

    private bool QuantityInReceipt(DataGridViewRow row, object? value) => sourceQuantities.TryGetValue(row, out var maximum)
        && decimal.TryParse(Convert.ToString(value, CultureInfo.CurrentCulture), NumberStyles.Number, CultureInfo.CurrentCulture, out var quantity)
        && quantity >= 0 && quantity <= maximum;

    public bool ValidateLocalQuantities()
    {
        if (!gridItems.EndEdit()) return false;
        bool valid = selectedReceiptId != null && gridItems.Rows.Count > 0;
        foreach (DataGridViewRow row in gridItems.Rows)
        {
            bool rowValid = QuantityInReceipt(row, row.Cells[colQuantity.Index].Value);
            row.Cells[colQuantity.Index].ErrorText = rowValid ? "" : "أدخل كمية ضمن كمية إذن التوريد المختار";
            valid &= rowValid;
        }
        return valid;
    }

    public bool ApplyReceiptSelection(CustodyIssueReceiptView receipt, Func<bool>? confirmReplace = null)
    {
        if (receipt.Lines.Any(line => line.Quantity < 0)) throw new ArgumentException("Negative source receipt quantity.", nameof(receipt));
        if (HasUnsavedChanges && confirmReplace?.Invoke() != true) return false;
        loadingInputChoices = true;
        try
        {
            gridItems.CancelEdit(); gridItems.Rows.Clear(); sourceQuantities.Clear();
            selectedReceiptId = receipt.Receipt.Id; fieldSourceNumber.Text = receipt.Receipt.Label;
            fieldCustomerNumber.Text = receipt.CustomerNumber; fieldCustomerName.Text = receipt.CustomerName; fieldAccountNumber.Text = receipt.AccountNumber;
            fieldExchangeRate.Text = receipt.ExchangeRate;
            foreach (var pair in new[] { (fieldWarehouse, receipt.Warehouse), (fieldCurrency, receipt.Currency) })
            {
                pair.Item1.DisplayMember = nameof(BatchFiveChoice.Label); pair.Item1.ValueMember = nameof(BatchFiveChoice.Id);
                pair.Item1.Items.Clear(); if (pair.Item2 != null) { pair.Item1.Items.Add(pair.Item2); pair.Item1.SelectedIndex = 0; }
            }
            foreach (var line in receipt.Lines)
            {
                var row = gridItems.Rows[gridItems.Rows.Add()];
                row.Cells[colSequence.Index].Value = row.Index + 1;
                row.Cells[colItemNumber.Index].Value = line.ItemNumber; row.Cells[colItemName.Index].Value = line.ItemName; row.Cells[colUnit.Index].Value = line.Unit;
                row.Cells[colQuantity.Index].Value = line.Quantity; row.Cells[colFreeQuantity.Index].Value = line.FreeQuantity;
                row.Cells[colPrice.Index].Value = line.Price; row.Cells[colValue.Index].Value = line.Value;
                sourceQuantities.Add(row, line.Quantity);
            }
            txtTotal.Clear();
        }
        finally { loadingInputChoices = false; }
        HasUnsavedChanges = true;
        lblOperationStatus.Text = "مسودة محلية غير محفوظة";
        return true;
    }
    public void AcceptInputBaseline() => HasUnsavedChanges = false;
}
