namespace TransportERP.EmptyForms;
public partial class UcInventoryReceiptOrder
{
    private void ReceiptItemLookup_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F7)
        {
            e.Handled = true; e.SuppressKeyPress = true;
            using var search = new FrmInventoryReceiptItemSearch();
            search.ShowDialog(FindForm());
            return;
        }
        if (e.KeyCode != Keys.F9 && e.KeyCode != Keys.F8) return;
        e.Handled = true; e.SuppressKeyPress = true;
        using var dialog = new FrmInventoryItemLookup();
        dialog.Controls.OfType<UcInventoryItemLookup>().Single().ShowReceiptColumns(e.KeyCode == Keys.F8);
        dialog.ShowDialog(FindForm());
    }

    private void AuthorizationLookup_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.F9) return;
        e.Handled = true; e.SuppressKeyPress = true;
        using var dialog = new FrmInventoryReceiptAuthorizationLookup();
        dialog.ShowDialog(FindForm());
    }
}
