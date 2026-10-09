using TransportERP.Contracts.Waybills;
using TransportERP.Desktop.CoreUI.Architecture;

namespace TransportERP.Desktop.Waybills;

/// <summary>
/// SHP-009 — B only exposes the pricing totals inherited from the Waybill and the action that opens the governed payment plan.
/// No accounting account or posting rule is embedded in Desktop.
/// </summary>
 
/// <summary>Compatibility window hosting the reusable business surface.</summary>
public sealed partial class WaybillPricingForm : Form
{
    public UcWaybillPricing Content { get; } = new()
    {
        Dock = DockStyle.Fill
    };

    public WaybillPricingForm()
    {
        Text = "الخدمات والرسوم — SHP-009";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(720, 360);
        Controls.Add(Content);
        Content.PaymentPlanRequested += (_, e) => PaymentPlanRequested?.Invoke(this, e);
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }

    public event EventHandler? PaymentPlanRequested;
    public void Bind(WaybillResponse waybill) => Content.Bind(waybill);
}
