using Microsoft.Extensions.DependencyInjection;
using TransportERP.Mobile.Driver.Offline;
using TransportERP.Mobile.Driver.Platforms.Android;

namespace TransportERP.Mobile.Driver;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        builder.Services.AddSingleton<AndroidSecureStorageEncryptionKeyProvider>();
        builder.Services.AddSingleton<IDriverNativeEncryptionKeyProvider>(services =>
            services.GetRequiredService<AndroidSecureStorageEncryptionKeyProvider>());
        builder.Services.AddSingleton<AndroidKeystoreDeviceSigningKey>();
        builder.Services.AddSingleton<IDriverNativeDeviceSigningKey>(services =>
            services.GetRequiredService<AndroidKeystoreDeviceSigningKey>());
        builder.Services.AddSingleton<AndroidDriverSyncNetworkProvider>();
        builder.Services.AddSingleton<IDriverSyncNetworkProvider>(services =>
            services.GetRequiredService<AndroidDriverSyncNetworkProvider>());
        builder.Services.AddSingleton<DriverVolatileSessionProvider>();
        builder.Services.AddSingleton<IDriverOfflineFeatureGate, DriverClosedOfflineFeatureGate>();
        builder.Services.AddSingleton<IDriverDeviceKeyBindingVerifier, DriverClosedDeviceKeyBindingVerifier>();
        builder.Services.AddSingleton<DriverOfflineActivationService>();

        // No activation at startup: an authenticated flow must explicitly supply the scope,
        // volatile session and granted action contract to DriverOfflineActivationService.
        return builder.Build();
    }
}
