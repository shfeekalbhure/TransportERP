using System.Globalization;
using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
public partial class UcInventoryReceiptAuthorization
{
    bool loadingInputChoices;
    public bool HasUnsavedChanges {get;private set;}
    public event Action<BatchFiveLookupRequest>? ReferenceLookupRequested;
    public string? SelectedWarehouseId=>(fieldWarehouse.SelectedItem as BatchFiveChoice)?.Id;
    public int CurrentItemRow=>gridItems.CurrentCell?.RowIndex??-1;
    private void ConfigureLocalInputs()
    {
        fieldWarehouse.Enabled=true;fieldWarehouse.DisplayMember=nameof(BatchFiveChoice.Label);fieldWarehouse.ValueMember=nameof(BatchFiveChoice.Id);
        fieldWarehouse.SelectedIndexChanged+=(_,_)=>{if(!loadingInputChoices)HasUnsavedChanges=true;};
        fieldWarehouse.KeyDown+=(_,e)=>
        {
            if(e.KeyCode!=Keys.F9)return;
            ReferenceLookupRequested?.Invoke(new("warehouse",Keys.F9));
            if(fieldWarehouse.Items.Count>0)fieldWarehouse.DroppedDown=true;
            e.Handled=true;e.SuppressKeyPress=true;
        };
        foreach(var editor in new[]{fieldDate,fieldReference,fieldDescription,fieldVehicleNumber,fieldDriverName})
        {editor.ReadOnly=false;editor.TextChanged+=(_,_)=>{if(!loadingInputChoices)HasUnsavedChanges=true;};}
        gridItems.ReadOnly=false;gridItems.AllowUserToAddRows=true;gridItems.AllowUserToDeleteRows=true;
        colItemNumber.ReadOnly=false;colExpiryDate.ReadOnly=false;colQuantity.ReadOnly=false;colDescription.ReadOnly=false;
        gridItems.CellValueChanged+=(_,e)=>{if(e.RowIndex>=0&&!loadingInputChoices)HasUnsavedChanges=true;};
        gridItems.UserDeletedRow+=(_,_)=>{if(!loadingInputChoices)HasUnsavedChanges=true;};
        gridItems.KeyDown+=(_,e)=>
        {
            if(e.KeyCode!=Keys.F9||gridItems.CurrentCell?.OwningColumn is not { } column)return;
            var key=column==colLookup||column==colItemName?colItemNumber.Name:column.Name;
            ReferenceLookupRequested?.Invoke(new(key,Keys.F9));e.Handled=true;e.SuppressKeyPress=true;
        };
        gridItems.CellContentClick+=(_,e)=>
        {
            if(e.RowIndex<0||e.ColumnIndex!=colLookup.Index)return;
            gridItems.CurrentCell=gridItems.Rows[e.RowIndex].Cells[colItemNumber.Index];
            ReferenceLookupRequested?.Invoke(new(colItemNumber.Name,Keys.None));
        };
        gridItems.CellValidating+=(_,e)=>
        {
            if(e.RowIndex<0||e.ColumnIndex!=colQuantity.Index)return;
            var text=Convert.ToString(e.FormattedValue);bool valid=string.IsNullOrWhiteSpace(text)||decimal.TryParse(text,NumberStyles.Number,CultureInfo.CurrentCulture,out var parsedQuantity);
            e.Cancel=!valid;gridItems.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText=valid?"":"أدخل كمية رقمية صحيحة";
        };
        commandBar.Commands[StandardCommand.Add].Enabled=true;
        commandBar.Commands[StandardCommand.Add].Click+=(_,_)=>
        {
            if(HasUnsavedChanges&&MessageBox.Show(FindForm(),"توجد تعديلات محلية غير محفوظة. هل تريد بدء مسودة جديدة؟","إذن التوريد",MessageBoxButtons.YesNo,MessageBoxIcon.Question)!=DialogResult.Yes)return;
            loadingInputChoices=true;
            try{gridItems.Rows.Clear();fieldWarehouse.SelectedIndex=-1;foreach(var editor in new[]{fieldDate,fieldReference,fieldDescription,fieldVehicleNumber,fieldDriverName})editor.Clear();}
            finally{loadingInputChoices=false;HasUnsavedChanges=false;}
        };
    }
    public void SetWarehouseChoices(IEnumerable<BatchFiveChoice> choices)
    {
        var supplied=choices.ToArray();
        if(supplied.Select(c=>c.Id).Distinct(StringComparer.Ordinal).Count()!=supplied.Length)throw new ArgumentException("Duplicate choice identity.",nameof(choices));
        var selectedId=SelectedWarehouseId;
        loadingInputChoices=true;
        try{fieldWarehouse.Items.Clear();fieldWarehouse.Items.AddRange(supplied);fieldWarehouse.SelectedIndex=-1;if(selectedId!=null)fieldWarehouse.SelectedItem=supplied.FirstOrDefault(c=>c.Id==selectedId);}
        finally{loadingInputChoices=false;}
        if(selectedId!=null&&SelectedWarehouseId!=selectedId)HasUnsavedChanges=true;
    }
    public void SetItemSelection(int rowIndex,string itemNumber,string itemName,string unit)
    {
        if(rowIndex<0||rowIndex>=gridItems.Rows.Count)throw new ArgumentOutOfRangeException(nameof(rowIndex));
        if(gridItems.Rows[rowIndex].IsNewRow)rowIndex=gridItems.Rows.Add();
        var row=gridItems.Rows[rowIndex];row.Cells[colItemNumber.Index].Value=itemNumber;row.Cells[colItemName.Index].Value=itemName;row.Cells[colUnit.Index].Value=unit;
    }
    public void AcceptInputBaseline()=>HasUnsavedChanges=false;
}
