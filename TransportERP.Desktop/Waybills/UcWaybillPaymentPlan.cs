using TransportERP.Contracts.Waybills;

namespace TransportERP.Desktop.Waybills;

public sealed partial class UcWaybillPaymentPlan : UserControl
{
    public UcWaybillPaymentPlan()
    {
        InitializeComponent();
        TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    public event EventHandler? SaveRequested;

    public void Bind(PaymentPlanResponse response)
    {
        _total.Text = response.NetAmount.ToString("N2");
        _version.Text = response.WaybillVersion.ToString();
        _lines.DataSource = response.Lines.ToList();
    }

    private void save_Click(object? sender, EventArgs e) => SaveRequested?.Invoke(this, EventArgs.Empty);
}
