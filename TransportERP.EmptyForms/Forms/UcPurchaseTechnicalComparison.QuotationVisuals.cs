namespace TransportERP.EmptyForms;
public partial class UcPurchaseTechnicalComparison
{
    private void QuotationLookup_Click(object? sender, EventArgs e)
    {
        using var dialog = new FrmPurchaseQuotationSelector();
        dialog.ShowDialog(FindForm());
    }
}
