namespace TransportERP.Contracts.Waybills;

public sealed record RecordArrivalRequest(
    Guid ManifestId,
    Guid LocationId,
    DateTimeOffset ReceivedAt,
    string ClientOperationId);

public sealed record ArrivalUnloadLineInput(
    Guid ManifestLineId,
    decimal ActualQuantity,
    decimal DamageQuantity,
    string? DifferenceType,
    Guid? EvidenceAttachmentId,
    string? Notes);

public sealed record RecordUnloadRequest(
    IReadOnlyList<ArrivalUnloadLineInput> Lines,
    DateTimeOffset OccurredAt,
    string ClientOperationId);

public sealed record ReallocateTransitRequest(
    Guid NextTripId,
    decimal Quantity,
    string ClientOperationId);

public sealed record FinalizeArrivalRequest(
    long ExpectedVersion,
    string ClientOperationId);

public sealed record CloseTripRequest(
    DateTimeOffset ClosedAt,
    long ExpectedVersion,
    string ClientOperationId);

public sealed record MovementQueryRequest(
    DateTimeOffset? From,
    DateTimeOffset? To);

public sealed record ArrivalReceiptLineResponse(
    Guid Id,
    Guid ManifestLineId,
    Guid WaybillItemId,
    decimal ExpectedQuantity,
    decimal ActualQuantity,
    string DifferenceType,
    decimal DamageQuantity,
    Guid? EvidenceAttachmentId,
    string? Notes);

public sealed record ArrivalReceiptResponse(
    Guid Id,
    Guid TripId,
    Guid ManifestId,
    Guid LocationId,
    Guid ReceivingBranchId,
    DateTimeOffset ReceivedAt,
    Guid ReceivedBy,
    string Status,
    long Version,
    IReadOnlyList<ArrivalReceiptLineResponse> Lines,
    Guid CorrelationId);

public sealed record WarehouseHoldingResponse(
    Guid Id,
    Guid WaybillItemId,
    Guid LocationId,
    Guid BranchId,
    decimal Quantity,
    string HoldingType,
    string Status,
    long Version);

public sealed record MovementEventResponse(
    Guid Id,
    Guid WaybillId,
    Guid? WaybillItemId,
    string EventType,
    decimal? Quantity,
    Guid? TripId,
    Guid? ManifestId,
    Guid? FromLocationId,
    Guid? ToLocationId,
    DateTimeOffset OccurredAt,
    DateTimeOffset RecordedAt,
    Guid RecordedBy,
    string? ReasonCode);

public sealed record WaybillMovementResponse(
    Guid WaybillId,
    IReadOnlyList<MovementEventResponse> Timeline,
    Guid CorrelationId);

public sealed record ItemMovementResponse(
    Guid WaybillId,
    Guid ItemId,
    decimal OriginalQuantity,
    decimal ReleasedQuantity,
    decimal AllocatedQuantity,
    decimal LoadedQuantity,
    decimal ArrivedQuantity,
    decimal DeliveredQuantity,
    decimal RemainingQuantity,
    IReadOnlyList<MovementEventResponse> Timeline,
    Guid CorrelationId);

public static class ArrivalExecutionPermissionCodes
{
    public const string RecordArrival = "arrival.record";
    public const string RecordUnload = "arrival.unload";
    public const string Reallocate = "waybill.reallocate";
    public const string FinalizeArrival = "arrival.finalize";
    public const string TripClose = "trip.close";
    public const string WaybillMovementView = "waybill.movement.view";
    public const string ItemMovementView = "waybill.item.movement.view";
}
