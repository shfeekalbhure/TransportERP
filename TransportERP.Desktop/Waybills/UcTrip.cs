using System.Drawing.Printing;
using TransportERP.Contracts.Waybills;

namespace TransportERP.Desktop.Waybills;

public sealed partial class UcTrip : ShippingRtlControl
{
    private IReadOnlyList<TripStopInput> _plannedStops = [];

    public UcTrip()
    {
        InitializeComponent();
        TransportERP.Desktop.CoreUI.SharedScreenProperties.Apply(this);
    
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
    
        if (standardAuditMetadata.Parent == null) Controls.Add(standardAuditMetadata);
        standardAuditMetadata.SendToBack();
}

    public event Action<CreateTripRequest>? CreateTripRequested;

    public void SetPlannedStops(IReadOnlyList<TripStopInput> stops)
    {
        _plannedStops = stops;
        _stops.DataSource = stops.ToList();
    }

    public void SetReferences(Guid vehicleId, Guid driverId, Guid originId, Guid destinationId)
    {
        _vehicle.Text = vehicleId.ToString();
        _driver.Text = driverId.ToString();
        _origin.Text = originId.ToString();
        _destination.Text = destinationId.ToString();
    }

    public void Bind(TripResponse trip)
    {
        _tripNo.Text = trip.TripNo;
        _driver.Text = trip.DriverId.ToString();
        _vehicle.Text = trip.VehicleId.ToString();
        _origin.Text = trip.OriginId.ToString();
        _destination.Text = trip.DestinationId.ToString();
        _plannedDepartAt.Value = trip.PlannedDepartAt.LocalDateTime;
        _status.Text = trip.Status;
        _stops.DataSource = trip.Stops.ToList();
    }

    private void RequestCreate()
    {
        if (string.IsNullOrWhiteSpace(_tripNo.Text) ||
            !Guid.TryParse(_vehicle.Text, out var vehicleId) ||
            !Guid.TryParse(_driver.Text, out var driverId) ||
            !Guid.TryParse(_origin.Text, out var originId) ||
            !Guid.TryParse(_destination.Text, out var destinationId))
        {
            _message.Text = "رقم الرحلة ومراجع السائق والمركبة والمنشأ والوجهة مطلوبة.";
            return;
        }

        _message.Text = "سيتم التحقق من المسار والمراجع في الخادم.";
        CreateTripRequested?.Invoke(new CreateTripRequest(
            _tripNo.Text.Trim(),
            vehicleId,
            driverId,
            originId,
            destinationId,
            new DateTimeOffset(_plannedDepartAt.Value),
            _plannedStops,
            OperationId("desktop-trip-create")));
    }

    private void btnCreate_Click(object? sender, EventArgs e) => RequestCreate();
}
