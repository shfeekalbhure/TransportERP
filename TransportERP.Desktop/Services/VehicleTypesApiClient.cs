using System.Net;
using System.Net.Http.Json;
using TransportERP.Contracts.Setup.VehicleTypes;

namespace TransportERP.Desktop.Services;

public interface IVehicleTypesApiClient
{
    Task<VehicleTypeSearchResponse> SearchAsync(VehicleTypeSearchRequest request, CancellationToken cancellationToken);
    Task<VehicleTypeCommandResponse> CreateAsync(CreateVehicleTypeRequest request, CancellationToken cancellationToken);
    Task<VehicleTypeCommandResponse> UpdateAsync(Guid id, UpdateVehicleTypeRequest request, CancellationToken cancellationToken);
    Task<VehicleTypeCommandResponse> SuspendAsync(Guid id, CancellationToken cancellationToken);
    Task<VehicleTypeCommandResponse> DeleteAsync(Guid id, CancellationToken cancellationToken);
}

public sealed class VehicleTypesApiClient : IVehicleTypesApiClient
{
    private readonly HttpClient _httpClient;

    public VehicleTypesApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<VehicleTypeSearchResponse> SearchAsync(VehicleTypeSearchRequest request, CancellationToken cancellationToken)
    {
        var query = $"api/setup/vehicle-types?query={Uri.EscapeDataString(request.Query ?? string.Empty)}&status={request.Status}&page={request.Page}&pageSize={request.PageSize}";
        var response = await _httpClient.GetAsync(query, cancellationToken);
        return await ReadSearchAsync(response, request, cancellationToken);
    }

    public async Task<VehicleTypeCommandResponse> CreateAsync(CreateVehicleTypeRequest request, CancellationToken cancellationToken)
        => await SendAsync(() => _httpClient.PostAsJsonAsync("api/setup/vehicle-types", request, cancellationToken), cancellationToken);

    public async Task<VehicleTypeCommandResponse> UpdateAsync(Guid id, UpdateVehicleTypeRequest request, CancellationToken cancellationToken)
        => await SendAsync(() => _httpClient.PutAsJsonAsync($"api/setup/vehicle-types/{id}", request, cancellationToken), cancellationToken);

    public async Task<VehicleTypeCommandResponse> SuspendAsync(Guid id, CancellationToken cancellationToken)
        => await SendAsync(() => _httpClient.PostAsync($"api/setup/vehicle-types/{id}/suspend", null, cancellationToken), cancellationToken);

    public async Task<VehicleTypeCommandResponse> DeleteAsync(Guid id, CancellationToken cancellationToken)
        => await SendAsync(() => _httpClient.DeleteAsync($"api/setup/vehicle-types/{id}", cancellationToken), cancellationToken);

    private static async Task<VehicleTypeSearchResponse> ReadSearchAsync(HttpResponseMessage response, VehicleTypeSearchRequest request, CancellationToken cancellationToken)
    {
        var payload = await response.Content.ReadFromJsonAsync<VehicleTypeSearchResponse>(cancellationToken: cancellationToken);
        return payload ?? new VehicleTypeSearchResponse(Array.Empty<VehicleTypeDto>(), 0, request.Page, request.PageSize, false, VehicleTypeBlockers.ApprovedStorageUnavailable, "تعذر الاتصال بخدمة أنواع المركبات.");
    }

    private static async Task<VehicleTypeCommandResponse> SendAsync(Func<Task<HttpResponseMessage>> send, CancellationToken cancellationToken)
    {
        var response = await send();
        var payload = await response.Content.ReadFromJsonAsync<VehicleTypeCommandResponse>(cancellationToken: cancellationToken);
        return payload ?? new VehicleTypeCommandResponse(false, false, VehicleTypeBlockers.ApprovedStorageUnavailable, "تعذر الاتصال بخدمة أنواع المركبات.", null);
    }

    public static VehicleTypesApiClient CreateDefault()
    {
        var baseUrl = Environment.GetEnvironmentVariable("TRANSPORTERP_API_BASE_URL") ?? "https://localhost:5001/";
        return new VehicleTypesApiClient(new HttpClient { BaseAddress = new Uri(baseUrl, UriKind.Absolute) });
    }
}
