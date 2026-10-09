using System.Drawing.Printing;
using TransportERP.Contracts.Waybills;
using TransportERP.Desktop.CoreUI.Architecture;

namespace TransportERP.Desktop.Waybills;

/// <summary>SHP-025 — create-trip input surface using the existing C CreateTripRequest contract.</summary>
 
/// <summary>Compatibility window hosting the reusable business surface.</summary>
public sealed partial class TripForm : ShippingRtlForm
{
    public UcTrip Content { get; } = new()
    {
        Dock = DockStyle.Fill
    };

    public TripForm() : base("إنشاء الرحلة — SHP-025", 920, 680)
    {
        Controls.Add(Content);
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }

    public event Action<CreateTripRequest>? CreateTripRequested { add => Content.CreateTripRequested += value; remove => Content.CreateTripRequested -= value; }

    public void SetPlannedStops(IReadOnlyList<TripStopInput> stops) => Content.SetPlannedStops(stops);
    public void SetReferences(Guid vehicleId, Guid driverId, Guid originId, Guid destinationId) => Content.SetReferences(vehicleId, driverId, originId, destinationId);
    public void Bind(TripResponse trip) => Content.Bind(trip);
}
