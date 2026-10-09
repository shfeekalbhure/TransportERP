namespace TransportERP.EmptyForms;

/// <summary>السنوات المالية — 02.04.06. Empty scaffold only.</summary>
public partial class UcScreen_02_04_06 : System.Windows.Forms.UserControl, TransportERP.Desktop.CoreUI.IFoundationScreen
{
    public TransportERP.Desktop.CoreUI.FoundationUiSession Foundation { get; }
    public bool HasUnsavedChanges => Foundation.HasUnsavedChanges;
    public bool IsBusy => Foundation.IsBusy;
    public bool ConfirmLeave() => Foundation.ConfirmLeave();

    public UcScreen_02_04_06()
    {
        InitializeComponent();
        TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
        TransportERP.Desktop.CoreUI.SharedScreenProperties.ApplyContent(this);
        Foundation = new TransportERP.Desktop.CoreUI.FoundationUiSession(this);
        Foundation.RequireFields(new[] { "btnReopen" }, txtReason.Name);
        Foundation.RequireFields(new[] { "btnExecute" }, cboAction.Name);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
}
