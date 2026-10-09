using System.ComponentModel;
using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
/// <summary>مجموعات البنوك: partial source-backed preview, without persistence binding.</summary>
public partial class UcOnyxSCREEN0080 : UserControl, IFoundationScreen, IExplicitScreenLayout
{
    private FoundationUiSession? foundation;
    public UcOnyxSCREEN0080()
    {
        InitializeComponent();
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public FoundationUiSession Foundation => foundation ??= new FoundationUiSession(this);
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool HasUnsavedChanges => foundation?.HasUnsavedChanges ?? false;
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsBusy => foundation?.IsBusy ?? false;
    public bool ConfirmLeave() => foundation?.ConfirmLeave() ?? true;

    public event EventHandler? CloseRequested;
    private void BtnClose_Click(object? sender, EventArgs e) => CloseRequested?.Invoke(this, EventArgs.Empty);
}
