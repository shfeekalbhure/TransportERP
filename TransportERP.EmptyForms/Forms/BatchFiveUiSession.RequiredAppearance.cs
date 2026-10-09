namespace TransportERP.EmptyForms;

public sealed partial class BatchFiveUiSession
{
    // Explicitly reviewed batches; reserved Onyx, bank reconciliation, inventory, purchases
    // and all designer-only shipping controls are deliberately not opted in here.
    private static readonly HashSet<string> AppearanceBatch = new(StringComparer.Ordinal)
    {
        "UcScreen_04_05_01", "UcScreen_04_07_16", "UcScreen_04_08_05",
        "UcScreen_04_11_02", "UcScreen_04_04_02", "UcScreen_04_08_01", "UcScreen_04_08_02",
        "UcScreen_04_03_02", "UcScreen_04_11_03", "UcScreen_04_10_03", "UcScreen_04_10_04",
        "UcScreen_04_10_05", "UcScreen_04_04_03", "UcScreen_04_11_04", "UcScreen_04_11_05",
        "UcScreen_04_04_04", "UcScreen_04_04_05", "UcScreen_04_04_06"
    };
    private bool requiredAppearanceEnabled;
    private void ConfigureKnownRequiredAppearance()
    {
        if (!AppearanceBatch.Contains(owner.GetType().Name)) return;
        requiredAppearanceEnabled = true;
        StateChanged += RefreshRequiredFieldAppearance;
        DocumentLoaded += RefreshRequiredFieldAppearance;
        owner.Load += (_, _) => RefreshRequiredFieldAppearance();
        RefreshRequiredFieldAppearance();
    }
    public void RefreshRequiredFieldAppearance()
    {
        if (!requiredAppearanceEnabled) return;
        foreach (var field in fields.Values)
            global::TransportERP.RequiredFieldAppearance.Apply(field.Editor,
                field.ReadOnly ? false : field.RequiredWhen == null ? field.Required : field.RequiredWhen());
        if (grid == null) return;
        foreach (DataGridViewColumn column in grid.Columns)
            global::TransportERP.RequiredFieldAppearance.Apply(column,
                editableColumns.Contains(column.Name) && Equals(column.HeaderCell.Tag, "required"));
    }
}
