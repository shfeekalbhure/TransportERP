using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
public partial class UcOnyxSCREEN0098 : UserControl, IWorkspaceChangeState, IBatchFiveScreen
{
    public BatchFiveUiSession Binding { get; }
    public bool HasUnsavedChanges => Binding.HasPendingChanges;
    public event EventHandler? CloseRequested;
    public UcOnyxSCREEN0098()
    {
        InitializeComponent();
        // Preserve this screen's designer font and background after shared setup.
        var designerFont = Font;
        var designerBackColor = BackColor;
        ScreenProperties.Apply(this);
        Font = designerFont;
        BackColor = designerBackColor;
        ConfigureImportLayout();
        Binding = new BatchFiveUiSession(this,lblStatus,validationErrors,
        new Dictionary<string,BatchFiveField>
        {
            ["ReceiptRequest_T04_EV_0111"] = new(field_ReceiptRequest_T04_EV_0111, false, false, false),
            ["ReceiptRequest_T04_EV_0112"] = new(field_ReceiptRequest_T04_EV_0112, false, false, false),
            ["ReceiptRequest_T04_EV_0115"] = new(field_ReceiptRequest_T04_EV_0115, false, false, false),
            ["ReceiptRequest_T04_EV_0116"] = new(field_ReceiptRequest_T04_EV_0116, false, true, false),
            ["ReceiptRequest_T04_EV_0117"] = new(field_ReceiptRequest_T04_EV_0117, false, false, false),
            ["ReceiptRequest_T04_EV_0118"] = new(field_ReceiptRequest_T04_EV_0118, false, false, true),
            ["ReceiptRequest_T04_EV_0119"] = new(field_ReceiptRequest_T04_EV_0119, false, false, false),
            ["ReceiptRequest_T04_EV_0122"] = new(field_ReceiptRequest_T04_EV_0122, false, false, false),
            ["ReceiptRequest_T04_EV_0123"] = new(field_ReceiptRequest_T04_EV_0123, false, false, false),
            ["ReceiptRequest_T04_EV_0439"] = new(field_ReceiptRequest_T04_EV_0439, false, true, false),
            ["ReceiptRequest_T04_EV_0440"] = new(field_ReceiptRequest_T04_EV_0440, false, true, false),
            ["ReceiptRequest_T04_EV_0441"] = new(field_ReceiptRequest_T04_EV_0441, false, false, false),
            ["ReceiptRequest_T04_EV_0442"] = new(field_ReceiptRequest_T04_EV_0442, false, false, false),
            ["ReceiptRequest_T04_EV_0443"] = new(field_ReceiptRequest_T04_EV_0443, false, false, false),
            ["ReceiptRequest_R05_AT_0272"] = new(field_ReceiptRequest_R05_AT_0272, false, false, false),
            ["ReceiptRequest_R05_AT_0273"] = new(field_ReceiptRequest_R05_AT_0273, false, false, true),
            ["ReceiptRequest_R05_AT_0274"] = new(field_ReceiptRequest_R05_AT_0274, false, false, false),
            ["ReceiptRequest_R05_AT_0275"] = new(field_ReceiptRequest_R05_AT_0275, false, false, true),
            ["ReceiptRequest_T04_EV_0124"] = new(field_ReceiptRequest_T04_EV_0124, false, false, true),
            ["ReceiptRequest_T04_EV_0125"] = new(field_ReceiptRequest_T04_EV_0125, false, false, true),
            ["localNotes"] = new(field_localNotes, false, false, false),
        }, dgvLines,new Dictionary<string,Button>
        {
            ["Save"] = btnSave,
            ["Approve"] = btnApprove,
        }, mainLayout,btnClear,btnAddRow,btnRemoveRow, new[] { "referenceOnly2" });
        btnClose.Click += (_,_) => { CloseRequested?.Invoke(this,EventArgs.Empty); };
        btnChooseCsv.Click += (_,_) => Binding.ChooseCsv(txtCsvPath,dgvImport);
        btnImportCsv.Click += (_,_) => Binding.ImportPreview(dgvImport);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
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
            var importing = tabs.SelectedTab == tp5;
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
