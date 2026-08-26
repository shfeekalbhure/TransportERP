namespace TransportERP.Contracts.Waybills;

public sealed record ReleaseItemRequest(
    decimal Quantity,
    DateTimeOffset ReleasedAt,
    string ClientOperationId);

public sealed record ItemQuantityStateResponse(
    Guid WaybillId,
    Guid ItemId,
    decimal OriginalQuantity,
    decimal ReleasedNet,
    decimal RemainingToRelease,
    Guid CorrelationId);

public sealed record TripStopInput(
    int StopNo,
    Guid LocationId,
    string StopType,
    DateTimeOffset? PlannedAt);

public sealed record CreateTripRequest(
    string TripNo,
    Guid VehicleId,
    Guid DriverId,
    Guid OriginId,
    Guid DestinationId,
    DateTimeOffset PlannedDepartAt,
    IReadOnlyList<TripStopInput>? Stops,
    string ClientOperationId);

public sealed record TripStopResponse(
    Guid Id,
    int StopNo,
    Guid LocationId,
    string StopType,
    DateTimeOffset? PlannedAt,
    string Status);

public sealed record TripResponse(
    Guid Id,
    Guid CompanyId,
    Guid BranchId,
    string TripNo,
    Guid VehicleId,
    Guid DriverId,
    Guid OriginId,
    Guid DestinationId,
    DateTimeOffset PlannedDepartAt,
    DateTimeOffset? ActualDepartAt,
    string Status,
    long Version,
    IReadOnlyList<TripStopResponse> Stops,
    Guid CorrelationId);

public sealed record AllocateItemRequest(
    Guid WaybillItemId,
    Guid ReleaseId,
    decimal Quantity,
    string ClientOperationId);

public sealed record AllocationResponse(
    Guid Id,
    Guid WaybillItemId,
    Guid ReleaseId,
    Guid TripId,
    decimal Quantity,
    string Status,
    Guid? ReversalOfId,
    Guid CorrelationId);

public sealed record UnallocateRequest(
    string Reason,
    string ClientOperationId);

public sealed record GenerateManifestRequest(
    string? ManifestNo,
    string ClientOperationId);

public sealed record ManifestLineResponse(
    Guid Id,
    Guid AllocationId,
    Guid WaybillId,
    Guid WaybillItemId,
    decimal Quantity,
    decimal LoadedQuantity,
    decimal Weight,
    decimal Volume,
    string LoadStatus,
    Guid? MovementEventId = null);

public sealed record ManifestResponse(
    Guid Id,
    Guid TripId,
    string ManifestNo,
    DateTimeOffset CreatedAt,
    DateTimeOffset? HandoverAt,
    DateTimeOffset? DriverAcceptedAt,
    string Status,
    long Version,
    long TripVersion,
    IReadOnlyList<ManifestLineResponse> Lines,
    Guid CorrelationId);

public sealed record LoadManifestLineRequest(
    decimal Quantity,
    DateTimeOffset OccurredAt,
    bool ResourceConstraintConfirmed,
    string ClientOperationId);

public sealed record FinalizeManifestRequest(
    long ExpectedVersion,
    string ClientOperationId);

public sealed record HandoverManifestRequest(
    Guid DriverId,
    DateTimeOffset AcceptedAt,
    long ExpectedVersion,
    string ClientOperationId);

public sealed record StartTripRequest(
    DateTimeOffset ActualDepartAt,
    long ExpectedVersion,
    string ClientOperationId);

public static class ShippingExecutionPermissionCodes
{
    public const string Release = "waybill.release";
    public const string TripCreate = "trip.create";
    public const string Allocate = "waybill.allocate";
    public const string Unallocate = "waybill.unallocate";
    public const string ManifestCreate = "manifest.create";
    public const string ManifestLoad = "manifest.load";
    public const string ManifestFinalize = "manifest.finalize";
    public const string ManifestHandover = "manifest.handover";
    public const string TripStart = "trip.start";
}
