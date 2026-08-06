using System.Net.Http.Json;
using TransportERP.Contracts.Setup.Currencies;

namespace TransportERP.Desktop.Services;

public interface ICurrenciesApiClient
{
    Task<CurrencySearchResponse> SearchAsync(CurrencySearchRequest request, CancellationToken cancellationToken);
    Task<CurrencyCommandResponse> CreateAsync(CreateCurrencyRequest request, CancellationToken cancellationToken);
    Task<CurrencyCommandResponse> UpdateAsync(Guid id, UpdateCurrencyRequest request, CancellationToken cancellationToken);
    Task<CurrencyCommandResponse> SuspendAsync(Guid id, CancellationToken cancellationToken);
    Task<CurrencyCommandResponse> DeleteAsync(Guid id, CancellationToken cancellationToken);
}

public sealed class CurrenciesApiClient(HttpClient httpClient) : ICurrenciesApiClient
{
    public async Task<CurrencySearchResponse> SearchAsync(CurrencySearchRequest request, CancellationToken cancellationToken)
    {
        var path = $"api/setup/currencies?query={Uri.EscapeDataString(request.Query ?? string.Empty)}&status={request.Status}&page={request.Page}&pageSize={request.PageSize}";
        var response = await httpClient.GetAsync(path, cancellationToken);
        return await response.Content.ReadFromJsonAsync<CurrencySearchResponse>(cancellationToken: cancellationToken)
            ?? new CurrencySearchResponse(Array.Empty<CurrencyDto>(), 0, request.Page, request.PageSize, false, CurrencyBlockers.ApprovedStorageUnavailable, "تعذر الاتصال بخدمة العملات.");
    }

    public async Task<CurrencyCommandResponse> CreateAsync(CreateCurrencyRequest request, CancellationToken cancellationToken)
        => await ReadAsync(await httpClient.PostAsJsonAsync("api/setup/currencies", request, cancellationToken), cancellationToken);

    public async Task<CurrencyCommandResponse> UpdateAsync(Guid id, UpdateCurrencyRequest request, CancellationToken cancellationToken)
        => await ReadAsync(await httpClient.PutAsJsonAsync($"api/setup/currencies/{id}", request, cancellationToken), cancellationToken);

    public async Task<CurrencyCommandResponse> SuspendAsync(Guid id, CancellationToken cancellationToken)
        => await ReadAsync(await httpClient.PostAsync($"api/setup/currencies/{id}/suspend", null, cancellationToken), cancellationToken);

    public async Task<CurrencyCommandResponse> DeleteAsync(Guid id, CancellationToken cancellationToken)
        => await ReadAsync(await httpClient.DeleteAsync($"api/setup/currencies/{id}", cancellationToken), cancellationToken);

    private static async Task<CurrencyCommandResponse> ReadAsync(HttpResponseMessage response, CancellationToken cancellationToken)
        => await response.Content.ReadFromJsonAsync<CurrencyCommandResponse>(cancellationToken: cancellationToken)
            ?? new CurrencyCommandResponse(false, false, CurrencyBlockers.ApprovedStorageUnavailable, "تعذر الاتصال بخدمة العملات.", null);

    public static CurrenciesApiClient CreateDefault()
    {
        var baseUrl = Environment.GetEnvironmentVariable("TRANSPORTERP_API_BASE_URL") ?? "https://localhost:5001/";
        return new CurrenciesApiClient(new HttpClient { BaseAddress = new Uri(baseUrl, UriKind.Absolute) });
    }
}
