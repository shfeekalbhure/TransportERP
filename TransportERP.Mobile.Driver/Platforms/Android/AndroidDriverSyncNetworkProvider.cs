using System.Net;
using Microsoft.Maui.Networking;
using TransportERP.Mobile.Driver.Offline;
using Xamarin.Android.Net;

namespace TransportERP.Mobile.Driver.Platforms.Android;

public sealed class AndroidDriverSyncNetworkProvider : IDriverSyncNetworkProvider, IDisposable
{
    private readonly HttpClient _client = new(new AndroidMessageHandler
    {
        AllowAutoRedirect = false,
        AutomaticDecompression = DecompressionMethods.None,
        UseCookies = false
    })
    {
        Timeout = TimeSpan.FromSeconds(45)
    };

    public bool IsPlatformTransportAvailable => true;
    public HttpClient SyncHttpClient => _client;

    public ValueTask<bool> IsNetworkAvailableAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return ValueTask.FromResult(Connectivity.Current.NetworkAccess == NetworkAccess.Internet);
    }

    public void Dispose() => _client.Dispose();
}
