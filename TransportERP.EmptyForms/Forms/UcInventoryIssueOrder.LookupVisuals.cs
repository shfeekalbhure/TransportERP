namespace TransportERP.EmptyForms;
public partial class UcInventoryIssueOrder
{
    private void LookupVisual_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.F9 && e.KeyCode != Keys.F3) return;
        e.Handled = true; e.SuppressKeyPress = true;
        using Form dialog = e.KeyCode == Keys.F3 ? new FrmInventoryQuantityLookup() : new FrmInventoryItemLookup();
        dialog.ShowDialog(FindForm());
    }
}
