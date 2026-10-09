using System.Drawing.Printing;
using TransportERP.Contracts.Waybills;

namespace TransportERP.Desktop.Waybills;

public sealed partial class UcItemRelease : ShippingRtlControl
{
    private ItemReleaseScreenState? _state;

    public UcItemRelease()
    {
        InitializeComponent();
        TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    public event Action<Guid, Guid, ReleaseItemRequest>? ReleaseRequested;

    public void Bind(ItemReleaseScreenState state)
    {
        _state = state;
        _waybill.Text = state.Waybill;
        _item.Text = state.Item;
        _original.Text = state.Quantity.OriginalQuantity.ToString("N3");
        _released.Text = state.Quantity.ReleasedNet.ToString("N3");
        _remaining.Text = state.Quantity.RemainingToRelease.ToString("N3");
        _holdStatus.Text = string.IsNullOrWhiteSpace(state.HoldStatus) ? "لا يوجد حجز نشط" : state.HoldStatus;
        _releaseQty.Maximum = Math.Max(0m, state.Quantity.RemainingToRelease);
        _releaseQty.Value = 0m;
        _message.Text = "";
    }

    public void Bind(ItemQuantityStateResponse value)
        => Bind(new ItemReleaseScreenState(
            value.WaybillId.ToString(),
            value.ItemId.ToString(),
            value,
            "حالة الحجز غير محملة"));

    private void RequestRelease()
    {
        if (_state is null)
        {
            _message.Text = "حمّل بيانات البوليصة والصنف أولاً.";
            return;
        }

        var qty = _releaseQty.Value;
        if (qty <= 0m || qty > _state.Quantity.RemainingToRelease)
        {
            _message.Text = "كمية الإطلاق يجب أن تكون موجبة ولا تتجاوز المتبقي.";
            return;
        }

        _message.Text = "سيتم التحقق من الحجز والمتبقي نهائياً في الخادم.";
        ReleaseRequested?.Invoke(
            _state.Quantity.WaybillId,
            _state.Quantity.ItemId,
            new ReleaseItemRequest(qty, DateTimeOffset.UtcNow, OperationId("desktop-release")));
    }

    private void btnRelease_Click(object? sender, EventArgs e) => RequestRelease();
}
