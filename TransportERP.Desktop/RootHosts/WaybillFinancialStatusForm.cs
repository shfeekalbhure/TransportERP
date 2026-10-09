using TransportERP.Contracts.Waybills;
using TransportERP.Desktop.CoreUI.Architecture;

namespace TransportERP.Desktop.Waybills;

/// <summary>SHP-012 — read-only projection; FinancialStatus is derived by server rules and never edited here.</summary>
 
/// <summary>Compatibility window hosting the reusable business surface.</summary>
public sealed partial class WaybillFinancialStatusForm : Form
{
    public UcWaybillFinancialStatus Content { get; } = new()
    {
        Dock = DockStyle.Fill
    };

    public WaybillFinancialStatusForm()
    {
        Text = "حالة السداد والمتبقي — SHP-012";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        MinimumSize = new Size(720, 400);
        Controls.Add(Content);
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }

    public void Bind(WaybillFinancialStatusResponse value) => Content.Bind(value);
}
