namespace TransportERP.EmptyForms;
public partial class UcInventoryIssueOrder
{
    private void JournalView_Click(object? sender, EventArgs e)
    {
        using var journal = new FrmInventoryJournalView();
        journal.ShowDialog(FindForm());
    }
}
