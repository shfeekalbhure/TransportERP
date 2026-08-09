using TransportERP.Api.Clients;

namespace TransportERP.Api.Services;

/// <summary>
/// Operational API-layer consumer of the typed client. Its target is server configuration, never
/// a caller-supplied URL, so the resilience contract is exercised by a real request path.
/// </summary>
public interface IDownstreamStatusService
{
    Task<HttpResponseMessage> ProbeAsync(CancellationToken cancellationToken = default);
}

public sealed class DownstreamStatusService(IApiClient apiClient, IConfiguration configuration) : IDownstreamStatusService
{
    public Task<HttpResponseMessage> ProbeAsync(CancellationToken cancellationToken = default)
    {
        var configuredUrl = configuration["Downstream:StatusUrl"];
        if (!Uri.TryCreate(configuredUrl, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            throw new InvalidOperationException("Downstream:StatusUrl must be an absolute HTTP(S) server configuration value.");
        }

        return apiClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri), cancellationToken);
    }
}
