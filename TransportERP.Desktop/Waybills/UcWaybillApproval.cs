using TransportERP.Contracts.Waybills;

namespace TransportERP.Desktop.Waybills;

public sealed partial class UcWaybillApproval : UserControl
{
    public UcWaybillApproval()
    {
        InitializeComponent();
        TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    public event EventHandler? ApproveRequested;
    public event EventHandler? ReturnRequested;

    public void Bind(WaybillResponse waybill, WaybillValidationResponse validation)
    {
        _draft.Text = waybill.DraftNo;
        _status.Text = waybill.Status;
        _blocking.DataSource = validation.BlockingErrors.ToList();
    }
    private void BtnApprove_Click(object? sender, EventArgs e)
        => ApproveRequested?.Invoke(this, EventArgs.Empty);

    private void BtnReturn_Click(object? sender, EventArgs e)
        => ReturnRequested?.Invoke(this, EventArgs.Empty);

}
