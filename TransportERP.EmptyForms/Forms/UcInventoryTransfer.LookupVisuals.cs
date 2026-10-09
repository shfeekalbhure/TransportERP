namespace TransportERP.EmptyForms;
public partial class UcInventoryTransfer
{
    private void LookupVisual_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.F9) return;
        e.Handled = true; e.SuppressKeyPress = true;
        if (gridItems.CurrentCell?.OwningColumn == colExpiryDate)
        {
            using var quantities = new FrmInventoryQuantityLookup();
            quantities.Controls.OfType<UcInventoryQuantityLookup>().Single().ShowTransferColumns();
            quantities.ShowDialog(FindForm());
        }
        else
        {
            using var items = new FrmInventoryItemLookup();
            items.ShowDialog(FindForm());
        }
    }
}
