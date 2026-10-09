using TransportERP.Contracts.Waybills;

namespace TransportERP.Desktop.Waybills;

public sealed partial class UcWaybillPricing : UserControl
{
    public UcWaybillPricing()
    {
        InitializeComponent();
        TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    public event EventHandler? PaymentPlanRequested;

    public void Bind(WaybillResponse waybill)
    {
        _freight.Text = waybill.FreightTotal.ToString("N2");
        _discount.Text = waybill.DiscountTotal.ToString("N2");
        _net.Text = waybill.NetAmount.ToString("N2");
        _currency.Text = waybill.CurrencyId.ToString();
    }

    private void openPlan_Click(object? sender, EventArgs e) => PaymentPlanRequested?.Invoke(this, EventArgs.Empty);
}
