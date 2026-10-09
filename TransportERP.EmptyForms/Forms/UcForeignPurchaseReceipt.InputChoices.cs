using System.Globalization;
namespace TransportERP.EmptyForms;
public partial class UcForeignPurchaseReceipt
{
    bool loadingInputChoices;
    public bool HasUnsavedChanges {get;private set;}
    public event Action<BatchFiveLookupRequest>? ReferenceLookupRequested;
    public string? SelectedInvoiceId=>(fieldInvoiceNumber.SelectedItem as BatchFiveChoice)?.Id;
    public string? SelectedCreditId {get;private set;}
    private void ConfigureLocalInputs()
    {
        fieldInvoiceNumber.Enabled=true;fieldInvoiceNumber.DisplayMember=nameof(BatchFiveChoice.Label);fieldInvoiceNumber.ValueMember=nameof(BatchFiveChoice.Id);
        fieldInvoiceNumber.SelectedIndexChanged+=(_,_)=>{if(!loadingInputChoices)HasUnsavedChanges=true;};
        fieldInvoiceNumber.KeyDown+=(_,e)=>
        {
            if(e.KeyCode!=Keys.F9)return;
            ReferenceLookupRequested?.Invoke(new("invoiceNumber",Keys.F9));
            if(fieldInvoiceNumber.Items.Count>0)fieldInvoiceNumber.DroppedDown=true;
            e.Handled=true;e.SuppressKeyPress=true;
        };
        fieldCreditNumber.KeyDown+=(_,e)=>
        {
            if(e.KeyCode!=Keys.F9)return;
            ReferenceLookupRequested?.Invoke(new("creditNumber",Keys.F9));
            e.Handled=true;e.SuppressKeyPress=true;
        };
        gridItems.ReadOnly=false;
        colReceivedQuantity.ReadOnly=false;colReceivedQuantityDetail.ReadOnly=true;colReceivedFreeQuantity.ReadOnly=false;
        gridItems.CellValueChanged+=(_,e)=>{if(e.RowIndex>=0&&!loadingInputChoices)HasUnsavedChanges=true;};
        gridItems.CellValidating+=(_,e)=>
        {
            if(e.RowIndex<0||gridItems.Columns[e.ColumnIndex].ReadOnly)return;
            var text=Convert.ToString(e.FormattedValue);bool valid=string.IsNullOrWhiteSpace(text)||decimal.TryParse(text,NumberStyles.Number,CultureInfo.CurrentCulture,out var parsedQuantity);
            e.Cancel=!valid;gridItems.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText=valid?"":"أدخل كمية رقمية صحيحة";
        };
    }
    public void SetInvoiceChoices(IEnumerable<BatchFiveChoice> choices)
    {
        var supplied=choices.ToArray();
        if(supplied.Select(c=>c.Id).Distinct(StringComparer.Ordinal).Count()!=supplied.Length)throw new ArgumentException("Duplicate choice identity.",nameof(choices));
        var selectedId=SelectedInvoiceId;
        loadingInputChoices=true;
        try{fieldInvoiceNumber.Items.Clear();fieldInvoiceNumber.Items.AddRange(supplied);fieldInvoiceNumber.SelectedIndex=-1;if(selectedId!=null)fieldInvoiceNumber.SelectedItem=supplied.FirstOrDefault(c=>c.Id==selectedId);}
        finally{loadingInputChoices=false;}
        if(selectedId!=null&&SelectedInvoiceId!=selectedId)HasUnsavedChanges=true;
    }
    // Called with the actual selection from the application's credit lookup adapter.
    public void SetCreditSelection(BatchFiveChoice? choice)
    {
        HasUnsavedChanges|=SelectedCreditId!=choice?.Id;SelectedCreditId=choice?.Id;fieldCreditNumber.Text=choice?.Id??string.Empty;
    }
    public void AcceptInputBaseline()=>HasUnsavedChanges=false;
}
