namespace TransportERP.EmptyForms;

public partial class UcScreen_04_04_01
{
    private readonly HashSet<string> waybillReceiptTypes = new(StringComparer.Ordinal);

    /// <summary>Type IDs and eligible waybill IDs must come from the scoped provider, never from label matching.</summary>
    public void SetReceiptWaybillLookups(IEnumerable<string> typeIds, IEnumerable<BatchFiveChoice> waybills)
    {
        var ids = typeIds.ToArray();
        if (ids.Any(string.IsNullOrWhiteSpace)) throw new ArgumentException("Receipt type identity is required.");
        Binding.SetChoices("waybillRef", waybills);
        waybillReceiptTypes.Clear();
        waybillReceiptTypes.UnionWith(ids);
        UpdateReceiptWaybillVisibility();
    }

    private void ConfigureReceiptWaybillColumn()
    {
        receiptWaybillColumn.DisplayMember = nameof(BatchFiveChoice.Label);
        receiptWaybillColumn.ValueMember = nameof(BatchFiveChoice.Id);
    }

    private void UpdateReceiptWaybillVisibility()
    {
        if (receiptWaybillColumn == null) return;
        bool required = field_voucherType.SelectedItem is BatchFiveChoice choice && (waybillReceiptTypes.Contains(choice.Id) ||
            (receiptDocument is { State: not "DRAFT" } document && document.Draft.TypeId.ToString() == choice.Id &&
                document.Draft.Additional.GetValueOrDefault("requiresWaybill") == "true"));
        receiptWaybillColumn.Visible = required;
        receiptWaybillColumn.HeaderCell.Tag = required ? "required" : null;
        // Keep the selected IDs when the type changes; they must round-trip with the draft.
    }
}
