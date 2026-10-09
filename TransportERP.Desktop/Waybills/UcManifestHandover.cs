using System.Drawing.Printing;
using TransportERP.Contracts.Waybills;

namespace TransportERP.Desktop.Waybills;

public sealed partial class UcManifestHandover : ShippingRtlControl
{
    private ManifestScreenState? _state;

    public UcManifestHandover()
    {
        InitializeComponent();
        TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    public event Action<Guid, HandoverManifestRequest>? AcceptRequested;

    public void Bind(ManifestScreenState state)
    {
        _state = state;
        var accepted = state.Manifest.DriverAcceptedAt?.ToLocalTime().ToString("yyyy-MM-dd HH:mm") ?? "لم يتم القبول بعد";
        _summary.Text =
            $"الكشف: {state.Manifest.ManifestNo}\r\n" +
            $"السائق: {state.Trip.DriverId}\r\n" +
            $"المركبة: {state.Trip.VehicleId}\r\n" +
            $"إجمالي الكمية: {state.TotalQuantity:N3}\r\n" +
            $"إجمالي الوزن: {state.TotalWeight:N3}\r\n" +
            $"إجمالي الحجم: {state.TotalVolume:N3}\r\n" +
            $"الحالة: {state.Manifest.Status}\r\n" +
            $"وقت القبول: {accepted}";
    }

    private void RequestAccept()
    {
        if (_state is null) return;
        AcceptRequested?.Invoke(
            _state.Manifest.Id,
            new HandoverManifestRequest(
                _state.Trip.DriverId,
                DateTimeOffset.UtcNow,
                _state.Manifest.Version,
                OperationId("desktop-handover")));
    }

    private void btnAccept_Click(object? sender, EventArgs e) => RequestAccept();
}
