using System.Drawing.Printing;
using TransportERP.Contracts.Waybills;

namespace TransportERP.Desktop.Waybills;

public sealed partial class UcRemainingShippingQuantity : ShippingRtlControl
{

    public UcRemainingShippingQuantity()
    {
        InitializeComponent();
        TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    }

    public void Bind(IReadOnlyList<RemainingShippingRow> rows) => _rows.DataSource = rows.ToList();
}
