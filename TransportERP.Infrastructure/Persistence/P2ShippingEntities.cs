namespace TransportERP.Infrastructure.Persistence;

public sealed class ItemReleaseEntity
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid BranchId { get; set; }
    public Guid WaybillItemId { get; set; }
    public decimal Quantity { get; set; }
    public DateTimeOffset ReleasedAt { get; set; }
    public Guid ReleasedBy { get; set; }
    public string ClientOperationId { get; set; } = string.Empty;
    public string Status { get; set; } = "ACTIVE";
    public Guid? ReversalOfId { get; set; }
    public string? Reason { get; set; }
    public WaybillItemEntity? WaybillItem { get; set; }
    public ItemReleaseEntity? ReversalOf { get; set; }
}

public sealed class TripEntity : P2Entity
{
    public Guid CompanyId { get; set; }
    public Guid BranchId { get; set; }
    public string TripNo { get; set; } = string.Empty;
    public Guid VehicleId { get; set; }
    public Guid DriverId { get; set; }
    public Guid OriginId { get; set; }
    public Guid DestinationId { get; set; }
    public DateTimeOffset PlannedDepartAt { get; set; }
    public DateTimeOffset? ActualDepartAt { get; set; }
    public DateTimeOffset? ActualArriveAt { get; set; }
    public string Status { get; set; } = "DRAFT";
    public string CreateClientOperationId { get; set; } = string.Empty;
    public string LastClientOperationId { get; set; } = string.Empty;
    public ICollection<TripStopEntity> Stops { get; set; } = new List<TripStopEntity>();
    public ICollection<TripAllocationEntity> Allocations { get; set; } = new List<TripAllocationEntity>();
    public ICollection<ManifestEntity> Manifests { get; set; } = new List<ManifestEntity>();
}

public sealed class TripStopEntity
{
    public Guid Id { get; set; }
    public Guid TripId { get; set; }
    public int StopNo { get; set; }
    public Guid LocationId { get; set; }
    public string StopType { get; set; } = string.Empty;
    public DateTimeOffset? PlannedAt { get; set; }
    public DateTimeOffset? ArrivedAt { get; set; }
    public DateTimeOffset? DepartedAt { get; set; }
    public string Status { get; set; } = "PLANNED";
    public TripEntity? Trip { get; set; }
}

public sealed class TripAllocationEntity
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid BranchId { get; set; }
    public Guid WaybillItemId { get; set; }
    public Guid ReleaseId { get; set; }
    public Guid TripId { get; set; }
    public decimal Quantity { get; set; }
    public DateTimeOffset AllocatedAt { get; set; }
    public string ClientOperationId { get; set; } = string.Empty;
    public string Status { get; set; } = "ALLOCATED";
    public Guid? ReversalOfId { get; set; }
    public string? Reason { get; set; }
    public WaybillItemEntity? WaybillItem { get; set; }
    public ItemReleaseEntity? Release { get; set; }
    public TripEntity? Trip { get; set; }
    public TripAllocationEntity? ReversalOf { get; set; }
}

public sealed class ManifestEntity : P2Entity
{
    public Guid CompanyId { get; set; }
    public Guid BranchId { get; set; }
    public Guid TripId { get; set; }
    public string ManifestNo { get; set; } = string.Empty;
    public DateTimeOffset? HandoverAt { get; set; }
    public DateTimeOffset? DriverAcceptedAt { get; set; }
    public string Status { get; set; } = "DRAFT";
    public string CreateClientOperationId { get; set; } = string.Empty;
    public string LastClientOperationId { get; set; } = string.Empty;
    public TripEntity? Trip { get; set; }
    public ICollection<ManifestLineEntity> Lines { get; set; } = new List<ManifestLineEntity>();
}

public sealed class ManifestLineEntity
{
    public Guid Id { get; set; }
    public Guid ManifestId { get; set; }
    public Guid AllocationId { get; set; }
    public Guid WaybillId { get; set; }
    public Guid WaybillItemId { get; set; }
    public decimal Quantity { get; set; }
    public decimal LoadedQuantity { get; set; }
    public decimal Weight { get; set; }
    public decimal Volume { get; set; }
    public string LoadStatus { get; set; } = "PLANNED";
    public ManifestEntity? Manifest { get; set; }
    public TripAllocationEntity? Allocation { get; set; }
}

public sealed class MovementEventEntity
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid BranchId { get; set; }
    public Guid WaybillId { get; set; }
    public Guid? WaybillItemId { get; set; }
    public Guid? AllocationId { get; set; }
    public Guid? ManifestLineId { get; set; }
    public string EventType { get; set; } = string.Empty;
    public decimal? Quantity { get; set; }
    public Guid? TripId { get; set; }
    public Guid? ManifestId { get; set; }
    public Guid? FromLocationId { get; set; }
    public Guid? ToLocationId { get; set; }
    public DateTimeOffset OccurredAt { get; set; }
    public DateTimeOffset RecordedAt { get; set; }
    public Guid RecordedBy { get; set; }
    public string? ReasonCode { get; set; }
    public Guid? ReversesEventId { get; set; }
    public string? ClientOperationId { get; set; }
}

public sealed class WaybillHoldEntity : P2Entity
{
    public Guid CompanyId { get; set; }
    public Guid BranchId { get; set; }
    public Guid WaybillId { get; set; }
    public string HoldType { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTimeOffset PlacedAt { get; set; }
    public Guid PlacedBy { get; set; }
    public DateTimeOffset? ReleasedAt { get; set; }
    public Guid? ReleasedBy { get; set; }
    public string Status { get; set; } = "ACTIVE";
}
