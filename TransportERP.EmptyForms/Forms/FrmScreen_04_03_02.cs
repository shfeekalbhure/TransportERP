using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
public partial class UcScreen_04_03_02 : UserControl, IWorkspaceChangeState, IBatchFiveScreen
{
    public BatchFiveUiSession Binding { get; }
    public bool HasUnsavedChanges => Binding.HasPendingChanges;
    public event EventHandler? CloseRequested;
    public UcScreen_04_03_02()
    {
        InitializeComponent();ScreenProperties.Apply(this);SharedScreenProperties.Apply(this);
        Binding=new BatchFiveUiSession(this,lblSourceStatus,validationErrors,new Dictionary<string,BatchFiveField>
        {
            ["BankNumber"]=new(txtBankNumber,false,false,false),
            ["BankName"]=new(txtBankName,false,false,false),
            ["ForeignName"]=new(txtForeignName,false,false,false),
            ["ReceiptNumberingType"]=new(cmbReceiptNumberingType,false,false,false),
            ["GroupNumber"]=new(cmbGroupNumber,false,false,false),
            ["BranchNumber"]=new(cmbBranchNumber,false,false,false),
            ["PrintTemplate"]=new(cmbPrintTemplate,false,false,false),
            ["Email"]=new(txtEmail,false,false,false),
            ["Website"]=new(txtWebsite,false,false,false),
            ["Phone"]=new(txtPhone,false,false,false),
            ["POBox"]=new(txtPOBox,false,false,false),
            ["Address"]=new(txtAddress,false,false,false),
            ["BankAccountNumber"]=new(txtBankAccountNumber,false,false,false),
            ["Intermediary"]=new(cmbIntermediary,false,false,false),
            ["CheckPaymentIntermediary"]=new(cmbCheckPaymentIntermediary,false,false,false),
            ["PayableNotes"]=new(cmbPayableNotes,false,false,false),
            ["ReceivableNotes"]=new(cmbReceivableNotes,false,false,false),
            ["PostingMethod"]=new(cmbPostingMethod,false,false,false),
            ["CommissionTaxDecimalPlaces"]=new(txtCommissionTaxDecimalPlaces,false,true,false),
            ["CardType"]=new(cmbCardType,false,false,false),
            ["CardName"]=new(txtCardName,false,false,false),
            ["CommissionPercent"]=new(txtCommissionPercent,false,true,false),
            ["CommissionAmount"]=new(txtCommissionAmount,false,true,false),
            ["MaturityDays"]=new(txtMaturityDays,false,true,false),
        },dgvCurrencies,new Dictionary<string,Button>{["Save"]=btnSave,["Permissions"]=btnPermissions},tabMain,btnClear,btnAddRow,btnRemoveRow);
        Binding.ValidateCommand = action =>
        {
            var ids=dgvCurrencies.Rows.Cast<DataGridViewRow>().Select(row=>Convert.ToString(row.Cells["currencyRef"].Value)).Where(id=>!string.IsNullOrWhiteSpace(id)).ToArray();
            return ids.Distinct().Count()!=ids.Length?"لا تكرر العملة داخل البنك":null;
        };
        dgvCurrencies.CellValueChanged += (_,e) =>
        {
            if(e.RowIndex>=0 && e.ColumnIndex==bank_currencyRef.Index)
            {
                var cell=dgvCurrencies.Rows[e.RowIndex].Cells["currencyRef"];
                dgvCurrencies.Rows[e.RowIndex].Cells["currencyName"].Value=(bank_currencyRef.DataSource as BatchFiveChoice[])?.FirstOrDefault(c=>c.Id==Convert.ToString(cell.Value))?.Label;
            }
        };
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
    private void BtnClose_Click(object? sender,EventArgs e)=>CloseRequested?.Invoke(this,EventArgs.Empty);
}
