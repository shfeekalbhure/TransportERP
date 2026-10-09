using System.Globalization;
namespace TransportERP.EmptyForms;

public sealed record OpeningReferenceFilter(string? BalanceTypeId, string? DisplayMethodId, string? CurrencyId, string AccountFrom, string AccountTo);
public sealed record OpeningForeignBalance(decimal? Debit, decimal? Credit);
public sealed record OpeningReferenceTotals(decimal? Debit, decimal? Credit, decimal? Difference);

public partial class UcScreen_04_07_16
{
    private readonly Dictionary<DataGridViewRow, OpeningForeignBalance> referenceAmounts = new();
    private Action<OpeningReferenceFilter>? referenceLoader;
    public event Action<BatchFiveLookupRequest>? ReferenceFilterLookupRequested;

    private void ConfigureReferenceDisplay()
    {
        foreach (var combo in new[] { referenceBalanceType, referenceDisplayMethod, referenceCurrency })
        {
            combo.DisplayMember = nameof(BatchFiveChoice.Label);
            combo.ValueMember = nameof(BatchFiveChoice.Id);
            combo.KeyDown += (_, e) =>
            {
                if (e.KeyCode != Keys.F9) return;
                ReferenceFilterLookupRequested?.Invoke(new(combo.Name, Keys.F9));
                if (combo.Items.Count > 0) combo.DroppedDown = true;
                e.Handled = true; e.SuppressKeyPress = true;
            };
            global::TransportERP.RequiredFieldAppearance.Apply(combo, null);
        }
        foreach (var editor in new[] { referenceAccountFrom, referenceAccountTo })
        {
            editor.KeyDown += (_, e) =>
            {
                if (e.KeyCode != Keys.F9) return;
                ReferenceFilterLookupRequested?.Invoke(new(editor.Name, Keys.F9));
                e.Handled = true; e.SuppressKeyPress = true;
            };
            global::TransportERP.RequiredFieldAppearance.Apply(editor, null);
        }
        btnReferenceLoad.Click += (_, _) => referenceLoader?.Invoke(new(
            (referenceBalanceType.SelectedItem as BatchFiveChoice)?.Id,
            (referenceDisplayMethod.SelectedItem as BatchFiveChoice)?.Id,
            (referenceCurrency.SelectedItem as BatchFiveChoice)?.Id,
            referenceAccountFrom.Text, referenceAccountTo.Text));
        dgvOpeningLines.CellFormatting += (_, e) =>
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || !referenceAmounts.TryGetValue(dgvOpeningLines.Rows[e.RowIndex], out var values)) return;
            if (e.ColumnIndex == colReferenceForeignDebit.Index) e.Value = values.Debit;
            else if (e.ColumnIndex == colReferenceForeignCredit.Index) e.Value = values.Credit;
        };
        dgvOpeningLines.CellValueChanged += (_, e) => { if (e.RowIndex >= 0) ClearReferenceDisplay(); };
        dgvOpeningLines.RowsAdded += (_, _) => ClearReferenceDisplay();
        dgvOpeningLines.RowsRemoved += (_, _) => ClearReferenceDisplay();
        txtOpeningBatch.TextChanged += (_, _) => ClearReferenceDisplay();
        cmbFiscalYear.SelectedIndexChanged += (_, _) => ClearReferenceDisplay();
        Binding.DocumentLoaded += ClearReferenceDisplay;
        Binding.DraftReset += ClearReferenceDisplay;
    }

    public void SetReferenceFilterChoices(string key, IEnumerable<BatchFiveChoice> choices)
    {
        var combo = key switch
        {
            "referenceBalanceType" => referenceBalanceType,
            "referenceDisplayMethod" => referenceDisplayMethod,
            "referenceCurrency" => referenceCurrency,
            _ => throw new ArgumentException("Unknown reference filter.", nameof(key))
        };
        var supplied = choices.ToArray();
        if (supplied.Select(c => c.Id).Distinct(StringComparer.Ordinal).Count() != supplied.Length) throw new ArgumentException("Duplicate choice identity.", nameof(choices));
        var selectedId = (combo.SelectedItem as BatchFiveChoice)?.Id;
        combo.Items.Clear(); combo.Items.AddRange(supplied); combo.SelectedIndex = -1;
        if (selectedId != null) combo.SelectedItem = supplied.FirstOrDefault(c => c.Id == selectedId);
    }

    public void SetReferenceLoader(Action<OpeningReferenceFilter>? loader)
    {
        referenceLoader = loader;
        btnReferenceLoad.Enabled = loader != null;
    }

    // Display values must be supplied for this exact loaded version and current row order.
    // They are not inferred from the existing nine-column editing contract.
    public bool SetReferenceDisplay(string expectedVersion, IReadOnlyList<OpeningForeignBalance> rows, OpeningReferenceTotals totals)
    {
        if (string.IsNullOrEmpty(expectedVersion) || expectedVersion != Binding.ExpectedVersion || Binding.HasPendingChanges) return false;
        var currentRows = dgvOpeningLines.Rows.Cast<DataGridViewRow>().Where(r => !r.IsNewRow).ToArray();
        if (rows.Count != currentRows.Length) return false;
        referenceAmounts.Clear();
        for (var i = 0; i < currentRows.Length; i++) referenceAmounts.Add(currentRows[i], rows[i]);
        referenceTotalDebit.Text = totals.Debit?.ToString(CultureInfo.CurrentCulture) ?? "";
        referenceTotalCredit.Text = totals.Credit?.ToString(CultureInfo.CurrentCulture) ?? "";
        referenceDifference.Text = totals.Difference?.ToString(CultureInfo.CurrentCulture) ?? "";
        dgvOpeningLines.Invalidate();
        return true;
    }

    private void ClearReferenceDisplay()
    {
        referenceAmounts.Clear();
        referenceTotalDebit.Clear(); referenceTotalCredit.Clear(); referenceDifference.Clear();
        dgvOpeningLines.Invalidate();
    }
}
