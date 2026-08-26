using TransportERP.Mobile.Driver.Offline;

namespace TransportERP.Mobile.Driver;

public sealed class App : Microsoft.Maui.Controls.Application
{
    private readonly DriverOfflineActivationService _activation;
    private readonly DriverAuthenticatedActivationCoordinator _authenticatedActivation;

    public App(
        DriverOfflineActivationService activation,
        DriverAuthenticatedActivationCoordinator authenticatedActivation)
    {
        _activation = activation ?? throw new ArgumentNullException(nameof(activation));
        _authenticatedActivation = authenticatedActivation ?? throw new ArgumentNullException(nameof(authenticatedActivation));
    }

    protected override Window CreateWindow(IActivationState? activationState) =>
        new(new MainPage(_activation, _authenticatedActivation));
}
