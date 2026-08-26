using TransportERP.Mobile.Driver.Offline;

namespace TransportERP.Mobile.Driver;

public sealed class App : Application
{
    private readonly DriverOfflineActivationService _activation;

    public App(DriverOfflineActivationService activation)
    {
        _activation = activation ?? throw new ArgumentNullException(nameof(activation));
    }

    protected override Window CreateWindow(IActivationState? activationState) => new(new MainPage(_activation));
}
