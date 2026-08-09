namespace TransportERP.Api.Clients;

/// <summary>Only supported consumer for outbound HTTP calls from the API layer.</summary>
public interface IApiClient
{
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default);
}

public sealed class ApiClient(HttpClient httpClient) : IApiClient
{
    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default) =>
        httpClient.SendAsync(request, cancellationToken);
}
