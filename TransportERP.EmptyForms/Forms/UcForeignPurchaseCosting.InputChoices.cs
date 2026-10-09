namespace TransportERP.EmptyForms;
public partial class UcForeignPurchaseCosting
{
    private bool loadingInputChoices;
    public bool HasUnsavedChanges { get; private set; }
    public event Action<BatchFiveLookupRequest>? ReferenceLookupRequested;
    public string? SelectedReceiptId => (fieldReceiptNumber.SelectedItem as BatchFiveChoice)?.Id;
    public string? SelectedCreditId => (fieldCreditNumber.SelectedItem as BatchFiveChoice)?.Id;
    private void ConfigureInputChoices()
    {
        foreach(var combo in new[]{fieldReceiptNumber,fieldCreditNumber})
        {
            combo.DisplayMember=nameof(BatchFiveChoice.Label);combo.ValueMember=nameof(BatchFiveChoice.Id);combo.SelectedIndex=-1;
            combo.SelectedIndexChanged+=(_,_)=> { if(!loadingInputChoices)HasUnsavedChanges=true; };
            combo.KeyDown+=(_,e)=>
            {
                if(e.KeyCode!=Keys.F9)return;
                ReferenceLookupRequested?.Invoke(new(combo==fieldReceiptNumber?"receiptNumber":"creditNumber",Keys.F9));
                if(combo.Items.Count>0)combo.DroppedDown=true;
                e.Handled=true;e.SuppressKeyPress=true;
            };
        }
        gridAccounts.CellValueChanged+=(_,e)=> { if(e.RowIndex>=0&&!loadingInputChoices)HasUnsavedChanges=true; };
    }
    // Caller-supplied choices only; no query, costing or persistence service.
    public void SetReferenceChoices(string key,IEnumerable<BatchFiveChoice> choices)
    {
        var combo=key switch {"receiptNumber"=>fieldReceiptNumber,"creditNumber"=>fieldCreditNumber,_=>throw new ArgumentOutOfRangeException(nameof(key))};
        var supplied=choices.ToArray();
        if(supplied.Select(c=>c.Id).Distinct(StringComparer.Ordinal).Count()!=supplied.Length)throw new ArgumentException("Duplicate choice identity.",nameof(choices));
        var selectedId=(combo.SelectedItem as BatchFiveChoice)?.Id;
        loadingInputChoices=true;
        try {combo.Items.Clear();combo.Items.AddRange(supplied);combo.SelectedIndex=-1;if(selectedId!=null)combo.SelectedItem=supplied.FirstOrDefault(c=>c.Id==selectedId);}
        finally {loadingInputChoices=false;}
        if(selectedId!=null&&(combo.SelectedItem as BatchFiveChoice)?.Id!=selectedId)HasUnsavedChanges=true;
    }
    public void AcceptInputBaseline()=>HasUnsavedChanges=false;
}
