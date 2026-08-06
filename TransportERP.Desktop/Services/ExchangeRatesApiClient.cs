using System.Net.Http.Json;
using TransportERP.Contracts.Setup.ExchangeRates;

namespace TransportERP.Desktop.Services;

public interface IExchangeRatesApiClient
{
    Task<ExchangeRateSearchResponse> SearchAsync(ExchangeRateSearchRequest request, CancellationToken cancellationToken);
    Task<ExchangeRateCommandResponse> CreateAsync(CreateExchangeRateRequest request, CancellationToken cancellationToken);
    Task<ExchangeRateCommandResponse> UpdateAsync(Guid id, UpdateExchangeRateRequest request, CancellationToken cancellationToken);
    Task<ExchangeRateCommandResponse> SuspendAsync(Guid id, CancellationToken cancellationToken);
    Task<ExchangeRateCommandResponse> DeleteAsync(Guid id, CancellationToken cancellationToken);
}

public sealed class ExchangeRatesApiClient(HttpClient httpClient) : IExchangeRatesApiClient
{
    public async Task<ExchangeRateSearchResponse> SearchAsync(ExchangeRateSearchRequest r,CancellationToken c){var x=await httpClient.GetAsync($"api/setup/exchange-rates?query={Uri.EscapeDataString(r.Query??string.Empty)}&status={r.Status}&page={r.Page}&pageSize={r.PageSize}",c);return await x.Content.ReadFromJsonAsync<ExchangeRateSearchResponse>(cancellationToken:c)??new(Array.Empty<ExchangeRateDto>(),0,r.Page,r.PageSize,false,ExchangeRateBlockers.ApprovedStorageUnavailable,"تعذر الاتصال بخدمة أسعار الصرف.");}
    public async Task<ExchangeRateCommandResponse> CreateAsync(CreateExchangeRateRequest r,CancellationToken c)=>await Read(await httpClient.PostAsJsonAsync("api/setup/exchange-rates",r,c),c);
    public async Task<ExchangeRateCommandResponse> UpdateAsync(Guid id,UpdateExchangeRateRequest r,CancellationToken c)=>await Read(await httpClient.PutAsJsonAsync($"api/setup/exchange-rates/{id}",r,c),c);
    public async Task<ExchangeRateCommandResponse> SuspendAsync(Guid id,CancellationToken c)=>await Read(await httpClient.PostAsync($"api/setup/exchange-rates/{id}/suspend",null,c),c);
    public async Task<ExchangeRateCommandResponse> DeleteAsync(Guid id,CancellationToken c)=>await Read(await httpClient.DeleteAsync($"api/setup/exchange-rates/{id}",c),c);
    private static async Task<ExchangeRateCommandResponse> Read(HttpResponseMessage x,CancellationToken c)=>await x.Content.ReadFromJsonAsync<ExchangeRateCommandResponse>(cancellationToken:c)??new(false,false,ExchangeRateBlockers.ApprovedStorageUnavailable,"تعذر الاتصال بخدمة أسعار الصرف.",null);
    public static ExchangeRatesApiClient CreateDefault(){var u=Environment.GetEnvironmentVariable("TRANSPORTERP_API_BASE_URL")??"https://localhost:5001/";return new(new HttpClient{BaseAddress=new Uri(u,UriKind.Absolute)});}
}