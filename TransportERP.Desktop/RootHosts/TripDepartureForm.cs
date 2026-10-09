using System.Drawing.Printing;
using TransportERP.Contracts.Waybills;
using TransportERP.Desktop.CoreUI.Architecture;

namespace TransportERP.Desktop.Waybills;

/// <summary>SHP-030 — departure confirmation with trip/manifest/driver/vehicle/planned/actual/status.</summary>
 
/// <summary>Compatibility window hosting the reusable business surface.</summary>
public sealed partial class TripDepartureForm : ShippingRtlForm
{
    public UcTripDeparture Content { get; } = new()
    {
        Dock = DockStyle.Fill
    };

    public TripDepartureForm() : base("انطلاق الرحلة — SHP-030", 800, 460)
    {
        Controls.Add(Content);
        // Screen-root properties only; child containers belong to their own designers.
        global::TransportERP.ScreenRootProperties.Attach(this);
        InitializeComponent();
    }

    public event Action<Guid, StartTripRequest>? StartRequested { add => Content.StartRequested += value; remove => Content.StartRequested -= value; }

    public void Bind(DepartureScreenState state) => Content.Bind(state);
    public void Bind(TripResponse trip) => Content.Bind(trip);
}
