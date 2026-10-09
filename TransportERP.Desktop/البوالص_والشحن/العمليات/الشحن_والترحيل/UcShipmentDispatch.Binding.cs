using TransportERP.Contracts.Waybills;
using TransportERP.Desktop.البوالص_والشحن.العمليات.ترحيل_الشحنات.منسدلة_الجدول;

namespace TransportERP.Desktop.البوالص_والشحن.العمليات.الشحن_والترحيل;

public partial class UcShipmentDispatch
{
    /// <summary>Loads supplied results without querying or changing operational data.</summary>
    public void Bind(IReadOnlyList<WaybillResponse> waybills)
    {
        ArgumentNullException.ThrowIfNull(waybills);
        // Construct first so a failed bind does not destroy the currently displayed rows.
        var rows = new List<UcDispatchWaybillRow>();
        try
        {
            foreach (var waybill in waybills)
            {
                var row = new UcDispatchWaybillRow();
                rows.Add(row);
                row.Bind(waybill);
            }
        }
        catch
        {
            foreach (var row in rows) row.Dispose();
            throw;
        }

        flpWaybills.SuspendLayout();
        try
        {
            foreach (var row in flpWaybills.Controls.OfType<UcDispatchWaybillRow>().ToArray())
            {
                flpWaybills.Controls.Remove(row);
                row.Dispose();
            }
            flpWaybills.Controls.AddRange(rows.ToArray());
            AdjustWaybillRowsWidth();
        }
        finally
        {
            flpWaybills.ResumeLayout(true);
        }
    }
}
