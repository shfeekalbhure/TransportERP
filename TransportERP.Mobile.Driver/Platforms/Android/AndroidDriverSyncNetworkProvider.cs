using System.Net;
#if TRANSPORTERP_DEVICE_TESTS
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
#endif
using Microsoft.Maui.Networking;
using TransportERP.Application.Sync;
using TransportERP.Mobile.Driver.Offline;
using Xamarin.Android.Net;

namespace TransportERP.Mobile.Driver.Platforms.Android;

public sealed class AndroidDriverSyncNetworkProvider : IDriverSyncNetworkProvider, IDisposable
{
    private readonly HttpClient _client = new(CreateHandler())
    {
        Timeout = TimeSpan.FromSeconds(45)
    };

    private static HttpMessageHandler CreateHandler()
    {
#if TRANSPORTERP_DEVICE_TESTS
        var handler = new HttpClientHandler
        {
            AllowAutoRedirect = false,
            AutomaticDecompression = DecompressionMethods.None,
            UseCookies = false
        };
        handler.ServerCertificateCustomValidationCallback = ValidatePinnedDeviceTestCertificate;
        return handler;
#else
        var handler = new AndroidMessageHandler
        {
            AllowAutoRedirect = false,
            AutomaticDecompression = DecompressionMethods.None,
            UseCookies = false
        };
        return handler;
#endif
    }

#if TRANSPORTERP_DEVICE_TESTS
    private const string TestCertificateMetadataKey = "TransportERPDeviceTestServerCertificateSha256";

    private static bool ValidatePinnedDeviceTestCertificate(
        HttpRequestMessage request,
        X509Certificate2? certificate,
        X509Chain? chain,
        SslPolicyErrors errors)
    {
        _ = request;
        _ = chain;
        _ = errors;
        var configured = typeof(AndroidDriverSyncNetworkProvider).Assembly
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .Where(attribute => string.Equals(
                attribute.Key, TestCertificateMetadataKey, StringComparison.Ordinal))
            .Select(attribute => attribute.Value)
            .ToArray();
        var origin = SyncClientDeploymentAuthority.Origin;
        if (request.RequestUri is null || request.RequestUri.Scheme != Uri.UriSchemeHttps ||
            !string.Equals(request.RequestUri.IdnHost, origin.IdnHost, StringComparison.OrdinalIgnoreCase) ||
            request.RequestUri.Port != origin.Port || certificate is null || configured.Length != 1 ||
            configured[0] is not { Length: 64 } expected ||
            expected.Any(character => character is not (>= '0' and <= '9' or >= 'a' and <= 'f')))
            return false;

        var actual = Convert.ToHexString(SHA256.HashData(certificate.RawData)).ToLowerInvariant();
        var actualBytes = Encoding.ASCII.GetBytes(actual);
        var expectedBytes = Encoding.ASCII.GetBytes(expected);
        try { return CryptographicOperations.FixedTimeEquals(actualBytes, expectedBytes); }
        finally
        {
            CryptographicOperations.ZeroMemory(actualBytes);
            CryptographicOperations.ZeroMemory(expectedBytes);
        }
    }
#endif

    public bool IsPlatformTransportAvailable => true;
    public HttpClient SyncHttpClient => _client;

    public ValueTask<bool> IsNetworkAvailableAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return ValueTask.FromResult(Connectivity.Current.NetworkAccess == NetworkAccess.Internet);
    }

    public void Dispose() => _client.Dispose();
}
