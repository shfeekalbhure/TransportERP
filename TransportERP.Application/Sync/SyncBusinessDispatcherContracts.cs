using TransportERP.Contracts.Core;
using TransportERP.Contracts.Waybills;
using System.Security.Cryptography;
using System.Text;

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
    string BusinessIdempotencyKey,
    Guid OperationCorrelationId);

/// <summary>
/// Produces the internal idempotency key used by domain persistence for an
/// operation that originated from a registered offline device. The external
/// ClientOperationId remains unchanged in the sync protocol and audit trail;
/// only the key passed into legacy business persistence is scoped. Online
/// callers therefore continue to use their existing idempotency keys.
/// </summary>
public static class SyncBusinessIdempotencyKey
{
    private const string Prefix = "sync-device-v1:";

    public static string Create(
        Guid companyId,
        Guid branchId,
        Guid registeredDeviceId,
        string clientOperationId)
    {
        if (companyId == Guid.Empty || branchId == Guid.Empty || registeredDeviceId == Guid.Empty)
            throw new ArgumentException("A complete device scope is required.");
        ArgumentException.ThrowIfNullOrWhiteSpace(clientOperationId);
        var normalized = clientOperationId.Trim();
        if (normalized.Length > 160)
            throw new ArgumentOutOfRangeException(nameof(clientOperationId));

        var canonical = string.Create(
            36 + 1 + 36 + 1 + 36 + 1 + normalized.Length,
            (companyId, branchId, registeredDeviceId, normalized),
            static (span, value) =>
            {
                value.companyId.TryFormat(span[..36], out _, "D");
                span[36] = '\n';
                value.branchId.TryFormat(span.Slice(37, 36), out _, "D");
                span[73] = '\n';
                value.registeredDeviceId.TryFormat(span.Slice(74, 36), out _, "D");
                span[110] = '\n';
                value.normalized.AsSpan().CopyTo(span[111..]);
            });
        return Prefix + Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(canonical)))
            .ToLowerInvariant();
    }
}

public sealed record EffectiveSyncRetentionPolicy(
    int ServerPayloadDays,
    string SourceVersion,
    string SourceFingerprint);

/// <summary>
/// Supplies the current immutable Global -> Company -> Branch -> Device
/// retention decision to persistence cleanup. A null decision is fail-closed:
/// content must remain untouched until a governed policy can be resolved.
/// </summary>
public interface IEffectiveSyncRetentionPolicyProvider
{
    ValueTask<EffectiveSyncRetentionPolicy?> ResolveAsync(
        Guid companyId,
        Guid? branchId,
        Guid? registeredDeviceId,
        string? deviceId,
        CancellationToken cancellationToken = default);
}

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
