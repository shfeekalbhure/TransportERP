using TransportERP.Desktop.CoreUI;
namespace TransportERP.EmptyForms;
/// <summary>مجموعات الصناديق: partial source-backed preview, without persistence binding.</summary>
public partial class UcOnyxSCREEN0079 : UserControl, IFoundationScreen
{
    public UcOnyxSCREEN0079()
    {
        InitializeComponent();
        ScreenProperties.Apply(this);
        SharedScreenProperties.Apply(this);
        Foundation = new FoundationUiSession(this);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}
    public FoundationUiSession Foundation { get; }
    public bool HasUnsavedChanges => Foundation.HasUnsavedChanges;
    public bool IsBusy => Foundation.IsBusy;
    public bool ConfirmLeave() => Foundation.ConfirmLeave();

    public event EventHandler? CloseRequested;
    private void BtnClose_Click(object? sender, EventArgs e) => CloseRequested?.Invoke(this, EventArgs.Empty);
}
