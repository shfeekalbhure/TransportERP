namespace TransportERP.EmptyForms;

/// <summary>ACC-053 Transaction/Opening design. Provider and server lifecycle bindings remain gated.</summary>
public partial class UcScreen_04_07_16 : UserControl, TransportERP.Desktop.CoreUI.IWorkspaceChangeState, IBatchFiveScreen
{
    public BatchFiveUiSession Binding { get; }
    public bool HasUnsavedChanges => Binding.HasPendingChanges;
    public void SetValidationResults(IReadOnlyDictionary<string,string> results)
    {
        var targets=new Dictionary<string,Label>{["fiscalYear"]=lblResultFiscalYear,["postableAccounts"]=lblResultPostableAccounts,["dimensions"]=lblResultDimensions,["balance"]=lblResultBalance,["currencyRate"]=lblResultCurrencyRate,["postingState"]=lblResultPostingState};
        foreach(var item in targets)item.Value.Text=results.TryGetValue(item.Key,out var value)?value:"لم يتم التحقق";
    }
    private void ShowSelectedLine()
    {
        var row=dgvOpeningLines.CurrentRow;
        cmbAccount.DataSource=null;cmbCurrency.DataSource=null;cmbCostCenterDimensions.DataSource=null;
        foreach(var pair in new[]{(cmbAccount,"accountRef"),(cmbCurrency,"currencyRef"),(cmbCostCenterDimensions,"costCenterDimensions")})
        {
            var text=row==null?"":Convert.ToString(row.Cells[pair.Item2].FormattedValue)??"";
            pair.Item1.Items.Clear();if(text.Length>0){pair.Item1.Items.Add(text);pair.Item1.SelectedIndex=0;}
        }
        txtDebitOpening.Text=Convert.ToString(row?.Cells["debitOpening"].Value)??"";
        txtCreditOpening.Text=Convert.ToString(row?.Cells["creditOpening"].Value)??"";
        txtExchangeRate.Text=Convert.ToString(row?.Cells["exchangeRate"].Value)??"";
        txtAccountingAmount.Text=Convert.ToString(row?.Cells["accountingAmount"].Value)??"";
    }
    public UcScreen_04_07_16()
    {
        InitializeComponent();
        TransportERP.Desktop.CoreUI.ScreenProperties.Apply(this);
        TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
        Binding=new BatchFiveUiSession(this,lblPreview,validationErrors,new Dictionary<string,BatchFiveField>
        {
            ["openingBatch"]=new(txtOpeningBatch,true,false,false),
            ["fiscalYearRef"]=new(cmbFiscalYear,true,false,false),
            ["batchState"]=new(txtBatchState,false,false,true)
        },dgvOpeningLines,new Dictionary<string,Button>{["View"]=btnView,["Create"]=btnCreate,["Edit"]=btnEdit,["Cancel"]=btnCancel,["Post"]=btnPost},workspaceViewport,btnClear,btnAddRow,btnRemoveRow,
            displayOnlyColumnNames: new[] { "referenceForeignDebit", "referenceForeignCredit" });
        ConfigureReferenceDisplay();
        dgvOpeningLines.SelectionChanged+=(_,_)=>ShowSelectedLine();
        dgvOpeningLines.RowsAdded+=(_,_)=>SetValidationResults(new Dictionary<string,string>());
        dgvOpeningLines.RowsRemoved+=(_,_)=>SetValidationResults(new Dictionary<string,string>());
        dgvOpeningLines.CellValueChanged+=(_,_)=>{ShowSelectedLine();SetValidationResults(new Dictionary<string,string>());};
        txtOpeningBatch.TextChanged+=(_,_)=>SetValidationResults(new Dictionary<string,string>());
        cmbFiscalYear.SelectedIndexChanged+=(_,_)=>SetValidationResults(new Dictionary<string,string>());
        Binding.DocumentLoaded+=()=>{SetValidationResults(new Dictionary<string,string>());ShowSelectedLine();};
        cmbAccount.Enabled=false;cmbCurrency.Enabled=false;cmbCostCenterDimensions.Enabled=false;
        txtDebitOpening.ReadOnly=true;txtCreditOpening.ReadOnly=true;txtExchangeRate.ReadOnly=true;
        UpdateWorkspaceLayout();
        DpiChangedAfterParent += (_, _) => UpdateWorkspaceLayout();
        workspaceViewport.SizeChanged += (_, _) => UpdateWorkspaceLayout();
        headerLayout.SizeChanged += (_, _) => UpdateWorkspaceLayout();
        tabValidation.SizeChanged += (_, _) => UpdateWorkspaceLayout();
        tabPosting.SizeChanged += (_, _) => UpdateWorkspaceLayout();
        tabLog.SizeChanged += (_, _) => UpdateWorkspaceLayout();
        // Grid owns draft line editing; the details tab is a selected-line projection.
        // No accounting formula or database property mapping is inferred.
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    private bool updatingWorkspaceLayout;
    private void UpdateWorkspaceLayout()
    {
        if (updatingWorkspaceLayout || IsDisposed) return;
        updatingWorkspaceLayout = true;
        try
        {
            // Header and tabs share the same interior extent, so narrowing the
            // workspace cannot collapse the fixed-label header field columns.
            int width = Math.Max(tabs.MinimumSize.Width, workspaceLayout.ClientSize.Width);
            int height = headerLayout.GetPreferredSize(new System.Drawing.Size(width, 0)).Height
                + headerLayout.Margin.Vertical + tabs.MinimumSize.Height + tabs.Margin.Vertical;
            var extent = new System.Drawing.Size(tabs.MinimumSize.Width, height);
            workspaceLayout.MinimumSize = extent;
            workspaceViewport.AutoScrollMinSize = extent;
            lblPreview.MaximumSize = new System.Drawing.Size(Math.Max(1, mainLayout.ClientSize.Width - lblPreview.Margin.Horizontal), 0);
            lblValidation.MaximumSize = new System.Drawing.Size(Math.Max(1, tabValidation.ClientSize.Width - tabValidation.Padding.Horizontal), 0);
            lblPosting.MaximumSize = new System.Drawing.Size(Math.Max(1, tabPosting.ClientSize.Width - tabPosting.Padding.Horizontal), 0);
            lblLog.MaximumSize = new System.Drawing.Size(Math.Max(1, tabLog.ClientSize.Width - tabLog.Padding.Horizontal), 0);
        }
        finally { updatingWorkspaceLayout = false; }
    }

    public event EventHandler? CloseRequested;
    private void BtnClose_Click(object? sender, EventArgs e) => CloseRequested?.Invoke(this, EventArgs.Empty);
}
