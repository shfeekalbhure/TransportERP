using TransportERP.Desktop.CoreUI;

namespace TransportERP.Desktop.Forms.SystemSettings.General;

// Text-driven subset of printed page 3, not a verified screenshot reconstruction.
// Sole content source: https://www.scribd.com/document/973153595/ONYX-ERP-v8-%D8%AA%D9%87%D9%8A%D8%A6%D8%A9-%D8%A7%D9%84%D9%86%D8%B8%D8%A7%D9%85
public partial class UcGeneralVariables : UserControl, IFoundationScreen, TransportERP.Desktop.CoreUI.IExplicitScreenLayout
{
    public FoundationUiSession Foundation { get; }
    public bool HasUnsavedChanges => Foundation.HasUnsavedChanges;
    public bool IsBusy => Foundation.IsBusy;
    public bool ConfirmLeave() => Foundation.ConfirmLeave();

    public UcGeneralVariables()
    {
        InitializeComponent();
        OnyxPhaseOneProperties.Apply(this);
        // Paths retain LTR editing within the RTL form.
        txtMobileFilesPath.RightToLeft = RightToLeft.No;
        txtMobileFilesPath.TextAlign = HorizontalAlignment.Left;
        Foundation = new FoundationUiSession(this);
        btnEdit.Click += (_, _) =>
        {
            grpMainData.Enabled = true;
            btnEdit.Enabled = false;
            txtMobileFilesPath.Focus();
        };
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
}
