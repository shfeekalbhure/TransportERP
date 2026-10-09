using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
public partial class UcScreen_04_08_05 : UserControl, IWorkspaceChangeState, IBatchFiveScreen
{
    public BatchFiveUiSession Binding { get; }
    public bool HasUnsavedChanges => Binding.HasPendingChanges;
    public event EventHandler? CloseRequested;
    public UcScreen_04_08_05()
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
            ["T04_EV_0267"] = new(field_T04_EV_0267, false, false, false),
            ["T04_EV_0268"] = new(field_T04_EV_0268, false, false, false),
            ["T04_EV_0269"] = new(field_T04_EV_0269, false, false, true),
            ["T04_EV_0270"] = new(field_T04_EV_0270, false, false, false),
            ["T04_EV_0271"] = new(field_T04_EV_0271, false, false, false),
            ["T04_EV_0272"] = new(field_T04_EV_0272, false, false, true),
            ["T04_EV_0273"] = new(field_T04_EV_0273, false, false, true),
            ["T04_EV_0274"] = new(field_T04_EV_0274, false, false, false),
            ["T04_EV_0277"] = new(field_T04_EV_0277, false, false, false),
            ["T04_EV_0279"] = new(field_T04_EV_0279, false, false, false),
            ["R05_AT_0292"] = new(field_R05_AT_0292, false, false, false),
            ["R05_AT_0293"] = new(field_R05_AT_0293, false, false, false),
            ["T04_EV_0278"] = new(field_T04_EV_0278, false, false, false),
            ["T04_EV_0275"] = new(field_T04_EV_0275, false, false, true),
            ["T04_EV_0280"] = new(field_T04_EV_0280, false, false, true),
            ["T04_EV_0281"] = new(field_T04_EV_0281, false, false, true),
            ["R05_AT_0296"] = new(field_R05_AT_0296, false, false, true),
            ["R05_AT_0297"] = new(field_R05_AT_0297, false, false, true),
            ["R05_AT_0298"] = new(field_R05_AT_0298, false, false, true),
            ["localNotes"] = new(field_localNotes, false, false, false),
        }, dgvLines,new Dictionary<string,Button>
        {
            ["Save"] = btnSave,
            ["Approve"] = btnApprove,
        }, mainLayout,btnClear,btnAddRow,btnRemoveRow, new[] { "referenceOnly0", "referenceOnly2" });
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
            var importing = tabs.SelectedTab == tp1;
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
