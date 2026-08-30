namespace TransportERP.Infrastructure.Persistence;

public sealed class ArrivalReceiptEntity : P2Entity
{
    public Guid CompanyId { get; set; }
    public Guid ReceivingBranchId { get; set; }
    public Guid TripId { get; set; }
    public Guid ManifestId { get; set; }
    public Guid LocationId { get; set; }
    public DateTimeOffset ReceivedAt { get; set; }
    public Guid ReceivedBy { get; set; }
    public string Status { get; set; } = "DRAFT";
    public string CreateClientOperationId { get; set; } = string.Empty;
    public string LastClientOperationId { get; set; } = string.Empty;
    public TripEntity? Trip { get; set; }
    public ManifestEntity? Manifest { get; set; }
    public ICollection<ArrivalReceiptLineEntity> Lines { get; set; } = new List<ArrivalReceiptLineEntity>();
}

public sealed class ArrivalReceiptLineEntity
{
    public Guid Id { get; set; }
    public Guid ArrivalReceiptId { get; set; }
    public Guid ManifestLineId { get; set; }
    public Guid WaybillItemId { get; set; }
    public decimal ExpectedQty { get; set; }
    public decimal ActualQty { get; set; }
    public string DifferenceType { get; set; } = "UNVALIDATED";
    public decimal DamageQty { get; set; }
    public Guid? EvidenceAttachmentId { get; set; }
    public string? Notes { get; set; }
    public ArrivalReceiptEntity? ArrivalReceipt { get; set; }
    public ManifestLineEntity? ManifestLine { get; set; }
    public WaybillItemEntity? WaybillItem { get; set; }
}

public sealed class WarehouseHoldingEntity : P2Entity
{
    public Guid CompanyId { get; set; }
    public Guid BranchId { get; set; }
    public Guid WaybillItemId { get; set; }
    public Guid LocationId { get; set; }
    public decimal Quantity { get; set; }
    public string HoldingType { get; set; } = "TRANSIT";
    public string Status { get; set; } = "AVAILABLE";
    public string SourceClientOperationId { get; set; } = string.Empty;
    public WaybillItemEntity? WaybillItem { get; set; }
}

public sealed class ShipmentExceptionEntity : P2Entity
{
    public Guid CompanyId { get; set; }
    public Guid BranchId { get; set; }
    public Guid TripId { get; set; }
    public Guid? WaybillId { get; set; }
    public Guid? WaybillItemId { get; set; }
    public string ExceptionType { get; set; } = string.Empty;
    public string Severity { get; set; } = "BLOCKING";
    public string Status { get; set; } = "OPEN";
    public string? ResolutionNotes { get; set; }
}
