using Microsoft.EntityFrameworkCore;
using TransportERP.Application.Sync;

namespace TransportERP.Infrastructure.Persistence;

/// <summary>
/// Revalidates mutable security state immediately before invoking the closed typed dispatcher.
/// No claim-time device, assignment, key, or permission decision is trusted for execution.
/// </summary>
public sealed class SyncBusinessActionExecutor(
    TransportErpDbContext db,
    IEffectivePermissionResolver permissions,
    SyncBusinessDispatcher dispatcher,
    ISyncBusinessDispatchAuditSink audit) : ISyncActionExecutor
{
    private static readonly TimeSpan InactivityWindow = TimeSpan.FromDays(90);

    public async Task<SyncActionExecutionOutcome> ExecuteAsync(
        SyncOperationExecutionClaim claim,
        CancellationToken cancellationToken = default)
    {
        var definition = SyncActionCatalog.Definitions.SingleOrDefault(x =>
            string.Equals(x.ActionCodeValue, claim.ActionCode, StringComparison.Ordinal));
        if (definition is null ||
            definition.DispatcherSupport != SyncActionDispatcherSupport.Supported ||
            definition.RuntimeAvailability != SyncActionRuntimeAvailability.Available)
            return await RejectAsync(claim, "ACTION_RUNTIME_UNAVAILABLE", cancellationToken);

        var now = DateTimeOffset.UtcNow;
        var deviceBindingIsCurrent = await (
            from device in db.RegisteredDevices.AsNoTracking()
            join assignment in db.RegisteredDeviceAssignments.AsNoTracking()
                on device.Id equals assignment.RegisteredDeviceId
            where device.Id == claim.RegisteredDeviceId &&
                  device.CompanyId == claim.CompanyId &&
                  device.DeviceId == claim.DeviceId &&
                  device.Status == "ACTIVE" &&
                  device.CredentialVersion == claim.RegisteredDeviceCredentialVersion &&
                  device.ProofKeyVersion == claim.ProofKeyVersion &&
                  device.ApprovedAt != null &&
                  device.RevokedAt == null &&
                  device.SuspendedAt == null &&
                  (device.ExpiresAt == null || device.ExpiresAt > now) &&
                  (device.LastSeenAt ?? device.ApprovedAt ?? device.CreatedAt) > now - InactivityWindow &&
                  assignment.UserId == claim.UserId &&
                  assignment.CompanyId == claim.CompanyId &&
                  assignment.BranchId == claim.BranchId &&
                  assignment.Status == "ACTIVE" &&
                  assignment.RemovedAt == null
            select assignment.Id).AnyAsync(cancellationToken);
        if (!deviceBindingIsCurrent)
            return await RejectAsync(claim, "DEVICE_NOT_REGISTERED", cancellationToken);

        if (!await permissions.HasPermissionAsync(
                claim.UserId, claim.CompanyId, claim.BranchId,
                definition.RequiredPermission, cancellationToken))
            return await RejectAsync(claim, "SCOPE_DENIED", cancellationToken);

        var actor = new SyncBusinessActorContext(
            claim.CompanyId,
            claim.BranchId,
            claim.UserId,
            claim.RegisteredDeviceId,
            new HashSet<string>([definition.RequiredPermission], StringComparer.Ordinal),
            claim.OperationCorrelationId);
        var command = new SyncBusinessDispatchCommand(
            claim.CompanyId,
            claim.BranchId,
            claim.UserId,
            claim.RegisteredDeviceId,
            claim.ProtocolVersion,
            claim.ActionCode,
            claim.OperationType,
            claim.EntityType,
            claim.EntityId,
            claim.BaseVersion,
            claim.ClientOperationId,
            claim.OperationCorrelationId,
            claim.PayloadJson);

        SyncBusinessDispatchResult result;
        try
        {
            result = await dispatcher.DispatchAsync(actor, command, cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch
        {
            await RejectAuditOnlyAsync(claim, "ACTION_EXECUTION_FAILED", cancellationToken);
            throw;
        }

        if (!result.IsSuccess)
            return new SyncActionExecutionOutcome.Failed(result.ErrorCode ?? "ACTION_EXECUTION_FAILED");
        if (!result.ResultEntityId.HasValue || result.ResultEntityId == Guid.Empty ||
            (definition.ResultVersionRequired && !result.ResultVersion.HasValue) ||
            result.ResultVersion is <= 0)
            return new SyncActionExecutionOutcome.Failed("BUSINESS_RESULT_INVALID");
        return new SyncActionExecutionOutcome.Succeeded(
            result.ResultEntityId.Value, result.ResultVersion);
    }

    private async Task<SyncActionExecutionOutcome> RejectAsync(
        SyncOperationExecutionClaim claim,
        string errorCode,
        CancellationToken cancellationToken)
    {
        await RejectAuditOnlyAsync(claim, errorCode, cancellationToken);
        return new SyncActionExecutionOutcome.Failed(errorCode);
    }

    private Task RejectAuditOnlyAsync(
        SyncOperationExecutionClaim claim,
        string errorCode,
        CancellationToken cancellationToken)
        => audit.WriteAsync(new SyncBusinessDispatchAuditRecord(
            claim.CompanyId,
            claim.BranchId,
            claim.UserId,
            claim.RegisteredDeviceId,
            claim.OperationCorrelationId,
            claim.ClientOperationId,
            claim.ActionCode,
            claim.EntityId,
            claim.BaseVersion,
            "REJECTED",
            null,
            null,
            errorCode), cancellationToken);
}

/// <summary>
/// Metadata-only dispatcher audit. Payload, proof, credential, nonce, JTI, and key material are
/// deliberately absent from both the audit draft and its reason text.
/// </summary>
public sealed class SyncBusinessDispatchAuditSink(AuditEventService audit) : ISyncBusinessDispatchAuditSink
{
    public async Task WriteAsync(
        SyncBusinessDispatchAuditRecord record,
        CancellationToken cancellationToken)
    {
        var entityId = record.ResultEntityId ?? record.EntityId;
        await audit.AppendAuditEventAsync(new AuditEventDraft(
            "SyncBusinessDispatchAttempt",
            record.Status,
            "SyncOperation",
            entityId,
            record.UserId,
            record.CompanyId,
            record.BranchId,
            record.OperationCorrelationId,
            Reason: $"Action={record.ActionCode};Result={record.ErrorCode ?? record.Status}",
            OperationCorrelationId: record.OperationCorrelationId), cancellationToken);
    }
}
