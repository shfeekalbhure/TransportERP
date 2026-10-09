using TransportERP.Contracts.Waybills;

namespace TransportERP.Desktop.Waybills;

public sealed partial class UcWaybillFinancialStatus : UserControl
{
    public UcWaybillFinancialStatus()
    {
        InitializeComponent();
        TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    public void Bind(WaybillFinancialStatusResponse value)
    {
        _net.Text = $"{value.NetAmount.Amount:N2} / {value.NetAmount.CurrencyId}";
        _paid.Text = $"{value.PaidEquivalent.Amount:N2} / {value.PaidEquivalent.CurrencyId}";
        _remaining.Text = $"{value.RemainingEquivalent.Amount:N2} / {value.RemainingEquivalent.CurrencyId}";
        _status.Text = value.FinancialStatus;
        _version.Text = value.WaybillVersion.ToString();
    }


}
