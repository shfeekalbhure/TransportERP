using System.Drawing.Printing;
using TransportERP.Contracts.Waybills;

namespace TransportERP.Desktop.Waybills;

public sealed partial class UcTripDeparture : ShippingRtlControl
{
    private DepartureScreenState? _state;

    public UcTripDeparture()
    {
        InitializeComponent();
        TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    public event Action<Guid, StartTripRequest>? StartRequested;

    public void Bind(DepartureScreenState state)
    {
        _state = state;
        var actual = state.Trip.ActualDepartAt?.ToLocalTime().ToString("yyyy-MM-dd HH:mm") ?? "لم تنطلق بعد";
        _summary.Text =
            $"الرحلة: {state.Trip.TripNo}\r\n" +
            $"الكشف: {state.Manifest.ManifestNo}\r\n" +
            $"السائق: {state.Trip.DriverId}\r\n" +
            $"المركبة: {state.Trip.VehicleId}\r\n" +
            $"الانطلاق المخطط: {state.Trip.PlannedDepartAt.ToLocalTime():yyyy-MM-dd HH:mm}\r\n" +
            $"الانطلاق الفعلي: {actual}\r\n" +
            $"الحالة: {state.Trip.Status}";
    }

    public void Bind(TripResponse trip)
    {
        _state = null;
        var actual = trip.ActualDepartAt?.ToLocalTime().ToString("yyyy-MM-dd HH:mm") ?? "لم تنطلق بعد";
        _summary.Text =
            $"الرحلة: {trip.TripNo}\r\n" +
            "الكشف: بيانات الكشف غير محملة\r\n" +
            $"السائق: {trip.DriverId}\r\n" +
            $"المركبة: {trip.VehicleId}\r\n" +
            $"الانطلاق المخطط: {trip.PlannedDepartAt.ToLocalTime():yyyy-MM-dd HH:mm}\r\n" +
            $"الانطلاق الفعلي: {actual}\r\n" +
            $"الحالة: {trip.Status}";
    }

    private void RequestStart()
    {
        if (_state is null) return;
        StartRequested?.Invoke(
            _state.Trip.Id,
            new StartTripRequest(
                DateTimeOffset.UtcNow,
                _state.Trip.Version,
                OperationId("desktop-trip-start")));
    }

    private void btnStart_Click(object? sender, EventArgs e) => RequestStart();
}
