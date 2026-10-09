namespace TransportERP.EmptyForms;
public partial class UcInventorySettlement
{
    private void JournalView_Click(object? sender, EventArgs e)
    {
        using var journal = new FrmInventoryJournalView();
        journal.ShowDialog(FindForm());
    }
}
