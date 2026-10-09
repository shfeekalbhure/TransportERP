using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
public partial class UcScreen_04_05_01 : UserControl, IWorkspaceChangeState, IBatchFiveScreen
{
    private bool? descriptionRequired;
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public bool? DescriptionRequired => descriptionRequired;
    private Func<Task<TransportERP.Contracts.Accounting.LedgerSettingsDocument>>? loadDescriptionSettings;
    private bool loadingDescriptionSettings;
    // Always load the persisted company variable, never infer it from approval settings.
    public void ConnectDescriptionSettings(Func<Task<TransportERP.Contracts.Accounting.LedgerSettingsDocument>> loader)
    {
        ArgumentNullException.ThrowIfNull(loader);
        if (loadDescriptionSettings != null) throw new InvalidOperationException("إعداد البيان مربوط بالفعل");
        loadDescriptionSettings = loader;
        Load += async (_, _) => await ReloadDescriptionSettingsAsync();
        if (IsHandleCreated) _ = ReloadDescriptionSettingsAsync();
    }
    public async Task ReloadDescriptionSettingsAsync()
    {
        if (loadingDescriptionSettings) return;
        SetDescriptionRequirement(null);
        if (loadDescriptionSettings == null) { lblStatus.Text = "إعداد إلزام البيان غير موصول"; return; }
        loadingDescriptionSettings = true;
        try
        {
            var settings = await loadDescriptionSettings();
            if (IsDisposed) return;
            SetDescriptionRequirement(settings.Policy?.RequireJournalDescription);
            lblStatus.Text = descriptionRequired == null ? "لم تُحدد قيمة إعداد إلزام البيان؛ حدّدها في إعدادات الأستاذ العام" : "تم تحميل إعداد إلزام البيان من إعدادات الشركة";
        }
        catch (Exception error) when (error is System.Net.Http.HttpRequestException or InvalidOperationException or TaskCanceledException or System.Text.Json.JsonException or UnauthorizedAccessException)
        { if (!IsDisposed) lblStatus.Text = "تعذر تحميل إعداد إلزام البيان: " + error.Message; }
        finally { loadingDescriptionSettings = false; }
    }
    public void SetDescriptionRequirement(bool? required)
    {
        descriptionRequired = required;
        global::TransportERP.RequiredFieldAppearance.Apply(field_description, required);
        field_description.AccessibleDescription = required switch
        {
            true => "البيان مطلوب حسب إعداد الأستاذ العام",
            false => "البيان اختياري حسب إعداد الأستاذ العام",
            null => "إعداد إلزام البيان غير موصول؛ لا يمكن اعتماد بيان فارغ حتى تحديد الإعداد"
        };
        validationErrors.SetError(field_description, "");
    }
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public BatchFiveUiSession Binding { get; } = null!;
    public bool HasUnsavedChanges => Binding?.HasPendingChanges == true;
    public event EventHandler? CloseRequested;
    public UcScreen_04_05_01()
    {
        InitializeComponent();
        // A nested screen may be instantiated by the designer before Site is assigned.
        if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
            return;
        ConfigureColumnContracts();
        // Static appearance belongs to InitializeComponent and the Properties window.
        ConfigureImportLayout();
        // Both read-only displays share the document number supplied by the binding.
        field_documentNumber.TextChanged += (_, _) => textBox2.Text = field_documentNumber.Text;
        textBox2.Text = field_documentNumber.Text;
        // Keep the journal-specific layout serialized in the designer.
        Binding = new BatchFiveUiSession(this,lblStatus,validationErrors,
        new Dictionary<string,BatchFiveField>
        {
            ["documentNumber"] = new(field_documentNumber, false, false, true),
            ["accountingDate"] = new(field_accountingDate, true, false, false),
            ["externalReference"] = new(field_externalReference, false, false, false),
            ["description"] = new(field_description, false, false, false) { RequiredWhen = () => descriptionRequired },
            ["currencyRef"] = new(field_currencyRef, true, false, false),
            ["exchangeRate"] = new(field_exchangeRate, true, true, false),
            ["state"] = new(field_state, false, false, true),
        }, dgvLines,new Dictionary<string,Button>
        {
            ["View"] = btnView,
            ["Create"] = btnCreate,
            ["Edit"] = btnEdit,
            ["Cancel"] = btnCancel,
            ["Post"] = btnPost,
            ["Reverse"] = btnReverse,
        }, mainLayout,btnClear,btnAddRow,btnRemoveRow,
            new[] { "analyticalAccount", "accountName", "foreignDebit", "foreignCredit", "approvalNumber", "salesperson", "collector", "referenceNumber" });
        SetDescriptionRequirement(null);
        documentImportChoose.Click += (_,_) => Binding.ChooseJournalImport(documentImportPath,documentImportPreview);
        documentImportApply.Click += (_,_) => Binding.ImportJournalPreview(documentImportPreview);
        btnClose.Click += (_,_) => { CloseRequested?.Invoke(this,EventArgs.Empty); };

    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
    // Binding keys and validation rules are runtime contracts, not designer component names.
    private void ConfigureColumnContracts()
    {
        col_rowNo.Name = "col_rowNo";
        col_rowNo.Name = "rowNo";
        col_rowNo.Tag = "text";
        col_accountRef.Name = "col_accountRef";
        col_accountRef.Name = "accountRef";
        col_accountRef.HeaderCell.Tag = "required";
        col_accountRef.Tag = "lookup";
        col_costCenterRef.Name = "col_costCenterRef";
        col_costCenterRef.Name = "costCenterRef";
        col_costCenterRef.Tag = "lookup";
        col_lineDescription.Name = "col_lineDescription";
        col_lineDescription.Name = "lineDescription";
        col_lineDescription.Tag = "text";
        col_debit.Name = "col_debit";
        col_debit.Name = "debit";
        col_debit.Tag = "decimal";
        col_credit.Name = "col_credit";
        col_credit.Name = "credit";
        col_credit.Tag = "decimal";
        col_currencyRef.Name = "col_currencyRef";
        col_currencyRef.Name = "currencyRef";
        col_currencyRef.Tag = "lookup";
        col_exchangeRate.Name = "col_exchangeRate";
        col_exchangeRate.Name = "exchangeRate";
        col_exchangeRate.Tag = "decimal";
        col_accountingAmount.Name = "col_accountingAmount";
        col_accountingAmount.Name = "accountingAmount";
        col_accountingAmount.Tag = "decimal";
        col_analyticalAccount.Name = "analyticalAccount";
        col_accountName.Name = "accountName";
        col_foreignDebit.Name = "foreignDebit";
        col_foreignCredit.Name = "foreignCredit";
        col_approvalNumber.Name = "approvalNumber";
        col_salesperson.Name = "salesperson";
        col_collector.Name = "collector";
        col_referenceNumber.Name = "referenceNumber";
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

}
