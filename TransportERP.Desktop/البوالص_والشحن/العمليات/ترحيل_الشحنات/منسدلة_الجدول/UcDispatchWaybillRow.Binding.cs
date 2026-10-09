using System.ComponentModel;
using TransportERP.Contracts.Waybills;

namespace TransportERP.Desktop.البوالص_والشحن.العمليات.ترحيل_الشحنات.منسدلة_الجدول;

public partial class UcDispatchWaybillRow
{
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Guid? WaybillId { get; private set; }

    /// <summary>Displays only this waybill's items in the grid owned by this row.</summary>
    public void Bind(WaybillResponse waybill)
    {
        ArgumentNullException.ThrowIfNull(waybill);
        WaybillId = waybill.Id;
        foreach (var field in tlpWaybillRow.Controls.OfType<TextBox>()) field.Clear();
        txtWaybillNumber.Text = waybill.WaybillNo ?? waybill.DraftNo;
        txtSender.Text = waybill.Parties.FirstOrDefault(p =>
            string.Equals(p.Role, "SENDER", StringComparison.OrdinalIgnoreCase))?.Name ?? string.Empty;
        txtReceiver.Text = waybill.Parties.FirstOrDefault(p =>
            string.Equals(p.Role, "RECEIVER", StringComparison.OrdinalIgnoreCase))?.Name ?? string.Empty;
        // These existing controls occupy the waybill-status and priority columns.
        txtPackageCount1.Text = waybill.Status;
        txtCollectionOfficer1.Text = waybill.Priority;

        dgvMainItems.Rows.Clear();
        colPackageType.Items.Clear();
        foreach (string itemType in waybill.Items.Select(item => item.ItemType).Distinct())
            colPackageType.Items.Add(itemType);

        foreach (var item in waybill.Items)
        {
            int index = dgvMainItems.Rows.Add(item.LineNo, false, item.ItemType,
                item.Quantity, item.Contents, null, item.Notes, item.Weight);
            dgvMainItems.Rows[index].Tag = item.Id;
        }

        tabPage1.Text = waybill.Items.Count == 0 ? "الأصناف — لا توجد أصناف" : $"الأصناف ({waybill.Items.Count})";
        isExpanded = false;
        pnlPackages.Visible = false;
        btnExpandWaybill.Text = "+";
        AdjustExpandedHeight();
    }
}
