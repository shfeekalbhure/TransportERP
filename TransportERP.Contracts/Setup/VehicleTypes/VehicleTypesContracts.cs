namespace TransportERP.Contracts.Setup.VehicleTypes;

public enum VehicleTypeStatus
{
    Active = 1,
    Suspended = 2
}

public sealed record VehicleTypeDto(
    Guid Id,
    string Code,
    string ArabicName,
    string? EnglishName,
    string Category,
    int? PassengerCapacity,
    decimal? CargoCapacity,
    VehicleTypeStatus Status,
    string? Notes,
    string CreatedBy,
    DateTimeOffset CreatedAt,
    string? ModifiedBy,
    DateTimeOffset? ModifiedAt,
    int EditCount,
    int PrintCount);

public sealed record VehicleTypeSearchRequest(
    string? Query,
    VehicleTypeStatus? Status,
    int Page = 1,
    int PageSize = 25);

public sealed record CreateVehicleTypeRequest(
    string Code,
    string ArabicName,
    string? EnglishName,
    string Category,
    int? PassengerCapacity,
    decimal? CargoCapacity,
    VehicleTypeStatus Status,
    string? Notes);

public sealed record UpdateVehicleTypeRequest(
    string ArabicName,
    string? EnglishName,
    string Category,
    int? PassengerCapacity,
    decimal? CargoCapacity,
    VehicleTypeStatus Status,
    string? Notes);

public sealed record VehicleTypeSearchResponse(
    IReadOnlyList<VehicleTypeDto> Items,
    int TotalCount,
    int Page,
    int PageSize,
    bool StorageAvailable,
    string? BlockerCode,
    string? Message);

public sealed record VehicleTypeCommandResponse(
    bool Succeeded,
    bool StorageAvailable,
    string? ErrorCode,
    string? Message,
    VehicleTypeDto? Item);

public static class VehicleTypeBlockers
{
    public const string ApprovedStorageUnavailable = "APPROVED_STORAGE_UNAVAILABLE";
}
