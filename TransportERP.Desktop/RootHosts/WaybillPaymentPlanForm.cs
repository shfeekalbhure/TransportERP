using TransportERP.Contracts.Waybills;
using TransportERP.Desktop.CoreUI.Architecture;

namespace TransportERP.Desktop.Waybills;

/// <summary>SHP-010 — payment plan is distinct from actual collections.</summary>
 
/// <summary>Compatibility window hosting the reusable business surface.</summary>
public sealed partial class WaybillPaymentPlanForm : Form
{
    public UcWaybillPaymentPlan Content { get; } = new()
    {
        Dock = DockStyle.Fill
    };

    public WaybillPaymentPlanForm()
    {
        Text = "خطة الدفع للبوليصة — SHP-010";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        MinimumSize = new Size(900, 520);
        Controls.Add(Content);
        Content.SaveRequested += (_, e) => SaveRequested?.Invoke(this, e);
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }

    public event EventHandler? SaveRequested;
    public void Bind(PaymentPlanResponse response) => Content.Bind(response);
}
