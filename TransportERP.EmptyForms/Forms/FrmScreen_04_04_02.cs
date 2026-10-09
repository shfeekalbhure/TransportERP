using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;

public partial class UcScreen_04_04_02 : UserControl, IWorkspaceChangeState, IBatchFiveScreen
{
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public BatchFiveUiSession Binding { get; } = null!;
    public bool HasUnsavedChanges => Binding?.HasPendingChanges == true;
    public event EventHandler? CloseRequested;
    public UcScreen_04_04_02()
    {
        InitializeComponent();
        // A nested screen may be instantiated by the designer before Site is assigned.
        if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
            return;
        ConfigureColumnContracts();
        // Preserve this screen's designer font and background after shared setup.
        var designerFont = Font;
        var designerBackColor = BackColor;
        ScreenProperties.Apply(this);
        Font = designerFont;
        BackColor = designerBackColor;
        ConfigureImportLayout();
        Binding = new BatchFiveUiSession(this, lblStatus, validationErrors,
        new Dictionary<string, BatchFiveField>
        {
            ["voucherNumber"] = new(field_voucherNumber, false, false, true),
            ["voucherDate"] = new(field_voucherDate, true, false, false),
            ["partyRef"] = new(field_partyRef, false, false, false),
            ["sourceCashBankRef"] = new(field_sourceCashBankRef, false, false, false),
            ["destinationCashBankRef"] = new(field_destinationCashBankRef, false, false, false),
            ["currencyRef"] = new(field_currencyRef, true, false, false),
            ["amount"] = new(field_amount, true, true, false),
            ["exchangeRate"] = new(field_exchangeRate, true, true, false),
            ["counterAccountRef"] = new(field_counterAccountRef, true, false, false),
            ["description"] = new(field_description, true, false, false),
            ["state"] = new(field_state, false, false, true),
        }, dgvLines, new Dictionary<string, Button>
        {
            ["View"] = btnView,
            ["Create"] = btnCreate,
            ["Edit"] = btnEdit,
            ["Cancel"] = btnCancel,
            ["Post"] = btnPost,
            ["Reverse"] = btnReverse,
        }, mainLayout, btnClear, btnAddRow, btnRemoveRow, new[] { "referenceOnly0", "referenceOnly1", "referenceOnly3" });
        documentImportChoose.Click += (_, _) => Binding.ChooseCsv(documentImportPath, documentImportPreview);
        documentImportApply.Click += (_, _) => Binding.ImportPreview(documentImportPreview);
        btnClose.Click += (_, _) => { CloseRequested?.Invoke(this, EventArgs.Empty); };

    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
    // Binding keys and validation rules are runtime contracts, not designer component names.
    private void ConfigureColumnContracts()
    {
        col_rowNo.Name = "rowNo";
        col_rowNo.Tag = "text";
        col_partyRef.Name = "partyRef";
        col_partyRef.Tag = "lookup";
        col_counterAccountRef.Name = "counterAccountRef";
        col_counterAccountRef.Tag = "lookup";
        col_lineDescription.Name = "lineDescription";
        col_lineDescription.Tag = "text";
        col_currencyRef.Name = "currencyRef";
        col_currencyRef.Tag = "lookup";
        col_amount.Name = "amount";
        col_amount.Tag = "decimal";
        col_exchangeRate.Name = "exchangeRate";
        col_exchangeRate.Tag = "decimal";
        col_accountingAmount.Name = "accountingAmount";
        col_accountingAmount.Tag = "decimal";
        referenceCol0.Name = "referenceOnly0";
        referenceCol1.Name = "referenceOnly1";
        referenceCol3.Name = "referenceOnly3";
        documentAccountsGridColumn0.Name = "documentAccountsGridColumn0";
        documentAccountsGridColumn1.Name = "documentAccountsGridColumn1";
        documentAccountsGridColumn2.Name = "documentAccountsGridColumn2";
        documentAccountsGridColumn3.Name = "documentAccountsGridColumn3";
        documentAccountsGridColumn4.Name = "documentAccountsGridColumn4";
        documentAccountsGridColumn5.Name = "documentAccountsGridColumn5";
        documentAccountsGridColumn6.Name = "documentAccountsGridColumn6";
        documentAccountsGridColumn7.Name = "documentAccountsGridColumn7";
    }

    private void ConfigureImportLayout()
    {
        var headerRow = mainLayout.RowStyles[2];
        var detailsRow = mainLayout.RowStyles[3];
        var headerSizeType = headerRow.SizeType;
        var headerHeight = headerRow.Height;
        var detailsSizeType = detailsRow.SizeType;
        var detailsHeight = detailsRow.Height;

        void UpdateLayout()
        {
            var importing = tabs.SelectedTab == documentImport;
            mainLayout.SuspendLayout();
            try
            {
                // Keep rows and bindings intact; only give the preview their space.
                dgvLines.Visible = !importing;
                detailsRow.SizeType = importing ? SizeType.Absolute : detailsSizeType;
                detailsRow.Height = importing ? 0F : detailsHeight;
                headerRow.SizeType = importing ? SizeType.Percent : headerSizeType;
                headerRow.Height = importing ? 100F : headerHeight;
            }
            finally
            {
                mainLayout.ResumeLayout(true);
            }
        }

        tabs.SelectedIndexChanged += (_, _) => UpdateLayout();
        UpdateLayout();
    }

    private void dgvLines_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
    }

    private void flpActions_Paint(object sender, PaintEventArgs e)
    {

    }
}
