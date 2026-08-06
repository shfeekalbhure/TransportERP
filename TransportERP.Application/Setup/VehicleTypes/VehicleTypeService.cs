using TransportERP.Contracts.Setup.VehicleTypes;

namespace TransportERP.Application.Setup.VehicleTypes;

public interface IVehicleTypeService
{
    Task<VehicleTypeSearchResponse> SearchAsync(VehicleTypeSearchRequest request, CancellationToken cancellationToken);
    Task<VehicleTypeCommandResponse> CreateAsync(CreateVehicleTypeRequest request, CancellationToken cancellationToken);
    Task<VehicleTypeCommandResponse> UpdateAsync(Guid id, UpdateVehicleTypeRequest request, CancellationToken cancellationToken);
    Task<VehicleTypeCommandResponse> SuspendAsync(Guid id, CancellationToken cancellationToken);
    Task<VehicleTypeCommandResponse> DeleteAsync(Guid id, CancellationToken cancellationToken);
}

/// <summary>
/// طبقة التطبيق لا تنشئ بيانات بديلة عند غياب التخزين المعتمد.
/// </summary>
public sealed class VehicleTypeService : IVehicleTypeService
{
    private const string BlockerMessage = "لا يتوفر تخزين معتمد لأنواع المركبات بعد. لم تُنشأ بيانات بديلة.";

    public Task<VehicleTypeSearchResponse> SearchAsync(VehicleTypeSearchRequest request, CancellationToken cancellationToken)
    {
        var page = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);

        return Task.FromResult(new VehicleTypeSearchResponse(
            Items: Array.Empty<VehicleTypeDto>(),
            TotalCount: 0,
            Page: page,
            PageSize: pageSize,
            StorageAvailable: false,
            BlockerCode: VehicleTypeBlockers.ApprovedStorageUnavailable,
            Message: BlockerMessage));
    }

    public Task<VehicleTypeCommandResponse> CreateAsync(CreateVehicleTypeRequest request, CancellationToken cancellationToken)
        => Task.FromResult(ValidateCreate(request) ?? StorageUnavailable());

    public Task<VehicleTypeCommandResponse> UpdateAsync(Guid id, UpdateVehicleTypeRequest request, CancellationToken cancellationToken)
        => Task.FromResult(ValidateUpdate(request) ?? StorageUnavailable());

    public Task<VehicleTypeCommandResponse> SuspendAsync(Guid id, CancellationToken cancellationToken)
        => Task.FromResult(StorageUnavailable());

    public Task<VehicleTypeCommandResponse> DeleteAsync(Guid id, CancellationToken cancellationToken)
        => Task.FromResult(StorageUnavailable());

    private static VehicleTypeCommandResponse? ValidateCreate(CreateVehicleTypeRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Code) || string.IsNullOrWhiteSpace(request.ArabicName) || string.IsNullOrWhiteSpace(request.Category))
        {
            return ValidationFailed("كود النوع والاسم العربي وفئة المركبة حقول إلزامية.");
        }

        return null;
    }

    private static VehicleTypeCommandResponse? ValidateUpdate(UpdateVehicleTypeRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.ArabicName) || string.IsNullOrWhiteSpace(request.Category))
        {
            return ValidationFailed("الاسم العربي وفئة المركبة حقول إلزامية.");
        }

        return null;
    }

    private static VehicleTypeCommandResponse ValidationFailed(string message)
        => new(false, true, "VALIDATION_FAILED", message, null);

    private static VehicleTypeCommandResponse StorageUnavailable()
        => new(false, false, VehicleTypeBlockers.ApprovedStorageUnavailable, BlockerMessage, null);
}
