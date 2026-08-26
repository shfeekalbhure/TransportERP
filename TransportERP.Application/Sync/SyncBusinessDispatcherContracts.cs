using TransportERP.Contracts.Core;
using TransportERP.Contracts.Waybills;

namespace TransportERP.Application.Sync;

public sealed record SyncBusinessActorContext(
    Guid CompanyId,
    Guid BranchId,
    Guid UserId,
    Guid RegisteredDeviceId,
    IReadOnlySet<string> Permissions,
    Guid CorrelationId);

public sealed record SyncBusinessDispatchCommand(
    Guid CompanyId,
    Guid BranchId,
    Guid UserId,
    Guid RegisteredDeviceId,
    string ProtocolVersion,
    string ActionCode,
    string OperationType,
    string EntityType,
    Guid? EntityId,
    long? BaseVersion,
    string ClientOperationId,
    Guid OperationCorrelationId,
    string PayloadJson);

public sealed record SyncBusinessDispatchResult(
    string Status,
    Guid? ResultEntityId,
    long? ResultVersion,
    string? ErrorCode)
{
    public bool IsSuccess => Status == "SUCCEEDED" && ErrorCode is null;
    public bool IsConflict => Status == "CONFLICT" && ErrorCode is not null;

    public static SyncBusinessDispatchResult Succeeded(Guid entityId, long? version)
        => new("SUCCEEDED", entityId, version, null);

    public static SyncBusinessDispatchResult Rejected(string errorCode)
        => new("REJECTED", null, null, errorCode);

    public static SyncBusinessDispatchResult Conflict(string errorCode)
        => new("CONFLICT", null, null, errorCode);
}

public sealed class SyncBusinessDispatchAuditException(Exception innerException)
    : InvalidOperationException("SYNC_BUSINESS_AUDIT_PENDING", innerException)
{
}

public sealed record SyncBusinessExecutionContext(
    OperationContext Operation,
    Guid RegisteredDeviceId,
    string ClientOperationId,
    Guid OperationCorrelationId);

public sealed record SyncBusinessActionResult(Guid EntityId, long? Version);

public sealed record SyncBusinessDispatchAuditRecord(
    Guid CompanyId,
    Guid BranchId,
    Guid UserId,
    Guid RegisteredDeviceId,
    Guid OperationCorrelationId,
    string ClientOperationId,
    string ActionCode,
    Guid? EntityId,
    long? BaseVersion,
    string Status,
    Guid? ResultEntityId,
    long? ResultVersion,
    string? ErrorCode);

public sealed record SyncLoadAllocatedQuantityPayload(
    Guid ManifestId,
    LoadManifestLineRequest Request);

public interface ISyncWaybillBusinessAdapter
{
    Task<SyncBusinessActionResult> CreateDraftAsync(
        SyncBusinessExecutionContext context, CreateWaybillDraftRequest request, CancellationToken cancellationToken);
    Task<SyncBusinessActionResult> UpdateDraftAsync(
        SyncBusinessExecutionContext context, Guid waybillId, UpdateWaybillDraftRequest request, CancellationToken cancellationToken);
    Task<SyncBusinessActionResult> CreateOperationalPartyAsync(
        SyncBusinessExecutionContext context, OperationalPartyCreateRequest request, CancellationToken cancellationToken);
}

public interface ISyncFinanceBusinessAdapter
{
    Task<SyncBusinessActionResult> RecordCollectionAsync(
        SyncBusinessExecutionContext context, Guid waybillId, RecordCollectionRequest request, CancellationToken cancellationToken);
}

public interface ISyncShippingBusinessAdapter
{
    Task<SyncBusinessActionResult> LoadAllocatedQuantityAsync(
        SyncBusinessExecutionContext context,
        Guid manifestLineId,
        SyncLoadAllocatedQuantityPayload payload,
        CancellationToken cancellationToken);
}

/// <summary>
/// Server composition persists metadata-only dispatch evidence. A sink failure is surfaced as
/// completion-pending: the worker retains its lease state and later replays the business idempotency
/// key to recover the same result before completing the sync-operation transition.
/// </summary>
public interface ISyncBusinessDispatchAuditSink
{
    Task WriteAsync(
        SyncBusinessDispatchAuditRecord record,
        CancellationToken cancellationToken);
}
