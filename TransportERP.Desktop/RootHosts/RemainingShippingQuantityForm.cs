using System.Drawing.Printing;
using TransportERP.Contracts.Waybills;
using TransportERP.Desktop.CoreUI.Architecture;

namespace TransportERP.Desktop.Waybills;

/// <summary>SHP-019 — typed read-only operational balance projection.</summary>
 
/// <summary>Compatibility window hosting the reusable business surface.</summary>
public sealed partial class RemainingShippingQuantityForm : ShippingRtlForm
{
    public UcRemainingShippingQuantity Content { get; } = new()
    {
        Dock = DockStyle.Fill
    };

    public RemainingShippingQuantityForm() : base("المتبقي غير المرحل — SHP-019", 1050, 560)
    {
        Controls.Add(Content);
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }

    public void Bind(IReadOnlyList<RemainingShippingRow> rows) => Content.Bind(rows);
}
