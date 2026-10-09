namespace TransportERP.EmptyForms;
public partial class UcInventoryTransferReceipt
{
    private void LookupVisual_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.F9) return;
        e.Handled = true; e.SuppressKeyPress = true;
        using var dialog = new FrmInventoryTransferLookup();
        dialog.ShowDialog(FindForm());
    }
}
