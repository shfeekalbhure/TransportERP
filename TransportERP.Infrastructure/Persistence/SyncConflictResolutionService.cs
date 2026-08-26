using System.Data;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TransportERP.Application.Sync;

namespace TransportERP.Infrastructure.Persistence;

public static class SyncConflictPermissionCodes
{
    public const string Resolve = "sync.conflicts.resolve";
}

public static class SyncConflictResolutionDecisions
{
    public const string KeepServerAndRejectLocal = "KEEP_SERVER_AND_REJECT_LOCAL";
    public const string ReapplyAsNew = "REAPPLY_AS_NEW";
}

public sealed record SyncConflictResolutionContext(
    Guid UserId,
    Guid CompanyId,
    Guid BranchId,
    Guid RegisteredDeviceId,
    int RegisteredDeviceCredentialVersion,
    string DeviceId,
    Guid CorrelationId);

public sealed record SyncReapplyAsNewRequest(
    string ClientOperationId,
    Guid OperationCorrelationId,
    string ActionCode,
    string OperationType,
    string EntityType,
    Guid? EntityId,
    long? BaseVersion,
    DateTimeOffset ClientOccurredAt,
    string PayloadJson);

public sealed record ResolveSyncConflictRequest(
    string Decision,
    string Reason,
    SyncReapplyAsNewRequest? Reapply = null);

public sealed record SyncConflictResolutionResult(
    Guid ConflictCaseId,
    Guid OriginalOperationId,
    string Decision,
    string ConflictStatus,
    string OriginalOperationStatus,
    string? OriginalOperationErrorCode,
    Guid? ReplacedByOperationId,
    DateTimeOffset ResolvedAt,
    Guid CorrelationId);

public interface ISyncConflictResolutionService
{
    Task<SyncConflictResolutionResult> ResolveAsync(
        Guid conflictCaseId,
        ResolveSyncConflictRequest request,
        SyncConflictResolutionContext context,
        CancellationToken cancellationToken = default);
}

public sealed class SyncConflictResolutionService(
    TransportErpDbContext db,
    AuditEventService audit,
    IEffectivePermissionResolver permissions) : ISyncConflictResolutionService
{
    public async Task<SyncConflictResolutionResult> ResolveAsync(
        Guid conflictCaseId,
        ResolveSyncConflictRequest request,
        SyncConflictResolutionContext context,
        CancellationToken cancellationToken = default)
    {
        ValidateRequest(conflictCaseId, request, context);
        if (!db.Database.IsNpgsql())
            throw new SyncRuleException("CONFLICT_STORE_UNSUPPORTED", "PostgreSQL is required");

        await using var transaction = await db.Database.BeginTransactionAsync(
            IsolationLevel.ReadCommitted, cancellationToken);
        try
        {
            var conflicts = await db.ConflictCases.FromSqlInterpolated($$"""
                SELECT c.* FROM transport_erp.conflict_cases AS c
                WHERE c."Id"={{conflictCaseId}}
                FOR UPDATE OF c
                """).AsTracking().ToListAsync(cancellationToken);
            var conflict = conflicts.SingleOrDefault()
                ?? throw new SyncRuleException("CONFLICT_NOT_FOUND", conflictCaseId.ToString());

            var operations = await db.SyncOperations.FromSqlInterpolated($$"""
                SELECT o.* FROM transport_erp.sync_operations AS o
                WHERE o."Id"={{conflict.SyncOperationId}}
                FOR UPDATE OF o
                """).AsTracking().ToListAsync(cancellationToken);
            var operation = operations.SingleOrDefault()
                ?? throw new SyncRuleException("OPERATION_NOT_FOUND", conflict.SyncOperationId.ToString());

            await EnsureAuthorityAsync(conflict, operation, context, cancellationToken);
            if (conflict.Status != "OPEN" || operation.Status != "CONFLICT" ||
                conflict.Resolution is not null || conflict.ResolvedAt.HasValue || conflict.ReplacedByOperationId.HasValue)
                throw new SyncRuleException("CONFLICT_ALREADY_RESOLVED", conflictCaseId.ToString());

            if (request.Decision == SyncConflictResolutionDecisions.ReapplyAsNew)
            {
                await ValidateReapplyAsync(operation, request.Reapply!, context, cancellationToken);
                // A new Stage 4 row must point at a fresh, accepted PoP replay record. This endpoint
                // has no such proof and must not reuse the original operation's proof provenance.
                throw new SyncRuleException("REAPPLY_PROOF_REQUIRED", conflictCaseId.ToString());
            }

            var now = NormalizeTimestamp(DateTimeOffset.UtcNow);
            conflict.Status = "RESOLVED";
            conflict.Resolution = SyncConflictResolutionDecisions.KeepServerAndRejectLocal;
            conflict.ResolvedBy = context.UserId.ToString();
            conflict.ResolvedAt = now;
            conflict.ReplacedByOperationId = null;
            conflict.UpdatedAt = now;
            conflict.RowVersion = Guid.NewGuid().ToByteArray();

            operation.Status = "REJECTED";
            operation.ErrorCode = "KEEP_SERVER";
            operation.ResultEntityId = null;
            operation.ResultVersion = null;
            operation.NextRetryAt = null;
            operation.ExecutionClaimToken = null;
            operation.ExecutionAttemptStartedAt = null;
            operation.ExecutionLeaseExpiresAt = null;
            operation.UpdatedAt = now;
            operation.RowVersion = Guid.NewGuid().ToByteArray();

            await db.SaveChangesAsync(cancellationToken);
            await audit.AppendAuditEventAsync(new AuditEventDraft(
                "SyncConflictResolved", "SUCCESS", nameof(ConflictCase), conflict.Id,
                context.UserId, context.CompanyId, context.BranchId, context.CorrelationId,
                context.DeviceId,
                AfterJson: JsonSerializer.Serialize(new
                {
                    Decision = conflict.Resolution,
                    conflict.SyncOperationId,
                    OriginalStatus = operation.Status,
                    OriginalErrorCode = operation.ErrorCode,
                    conflict.ReplacedByOperationId
                }),
                Reason: request.Reason.Trim(),
                OperationCorrelationId: operation.OperationCorrelationId), cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return new SyncConflictResolutionResult(
                conflict.Id, operation.Id, conflict.Resolution, conflict.Status, operation.Status,
                operation.ErrorCode, conflict.ReplacedByOperationId, now, context.CorrelationId);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            db.ChangeTracker.Clear();
            throw;
        }
    }

    private async Task EnsureAuthorityAsync(
        ConflictCase conflict,
        SyncOperation operation,
        SyncConflictResolutionContext context,
        CancellationToken cancellationToken)
    {
        if (operation.CompanyId != context.CompanyId || operation.BranchId != context.BranchId ||
            operation.UserId != context.UserId || operation.RegisteredDeviceId != context.RegisteredDeviceId ||
            !string.Equals(operation.DeviceId, context.DeviceId, StringComparison.Ordinal) ||
            conflict.CompanyId != context.CompanyId || conflict.BranchId != context.BranchId)
            throw new SyncRuleException("SCOPE_DENIED", conflict.Id.ToString());

        var now = DateTimeOffset.UtcNow;
        var inactivityCutoff = now - TimeSpan.FromDays(90);
        var activeBinding = await (
            from device in db.RegisteredDevices.AsNoTracking()
            join assignment in db.RegisteredDeviceAssignments.AsNoTracking()
                on device.Id equals assignment.RegisteredDeviceId
            where device.Id == context.RegisteredDeviceId && device.CompanyId == context.CompanyId &&
                  device.DeviceId == context.DeviceId && device.Status == "ACTIVE" &&
                  device.CredentialVersion == context.RegisteredDeviceCredentialVersion &&
                  device.ApprovedAt != null && device.SuspendedAt == null && device.RevokedAt == null &&
                  (device.ExpiresAt == null || device.ExpiresAt > now) &&
                  (device.LastSeenAt ?? device.ApprovedAt ?? device.CreatedAt) > inactivityCutoff &&
                  assignment.RegisteredDeviceId == context.RegisteredDeviceId &&
                  assignment.CompanyId == context.CompanyId && assignment.BranchId == context.BranchId &&
                  assignment.UserId == context.UserId && assignment.Status == "ACTIVE" &&
                  assignment.RemovedAt == null && assignment.RemovedByUserId == null
            select assignment.Id).AnyAsync(cancellationToken);
        if (!activeBinding)
            throw new SyncRuleException("DEVICE_NOT_REGISTERED", context.RegisteredDeviceId.ToString());

        if (!await permissions.HasPermissionAsync(context.UserId, context.CompanyId, context.BranchId,
                SyncConflictPermissionCodes.Resolve, cancellationToken))
            throw new SyncRuleException("PERMISSION_DENIED", SyncConflictPermissionCodes.Resolve);

        var definition = SyncActionCatalog.Definitions.SingleOrDefault(x =>
            string.Equals(x.ActionCodeValue, operation.ActionCode, StringComparison.Ordinal));
        if (definition is null)
            throw new SyncRuleException("ORIGINAL_ACTION_UNAVAILABLE", operation.ActionCode ?? operation.Id.ToString());
        if (!await permissions.HasPermissionAsync(context.UserId, context.CompanyId, context.BranchId,
                definition.RequiredPermission, cancellationToken))
            throw new SyncRuleException("PERMISSION_DENIED", definition.RequiredPermission);
    }

    private async Task ValidateReapplyAsync(
        SyncOperation original,
        SyncReapplyAsNewRequest reapply,
        SyncConflictResolutionContext context,
        CancellationToken cancellationToken)
    {
        if (reapply is null || string.IsNullOrWhiteSpace(reapply.ClientOperationId) ||
            reapply.ClientOperationId.Trim().Length > 120 || reapply.OperationCorrelationId == Guid.Empty ||
            string.IsNullOrWhiteSpace(reapply.PayloadJson) || reapply.ClientOccurredAt == default)
            throw new SyncRuleException("REAPPLY_REQUEST_INVALID", original.Id.ToString());
        if (string.Equals(reapply.ClientOperationId.Trim(), original.ClientOperationId, StringComparison.Ordinal) ||
            reapply.OperationCorrelationId == original.OperationCorrelationId)
            throw new SyncRuleException("REAPPLY_ID_REUSE", original.Id.ToString());
        if (!string.Equals(reapply.ActionCode, original.ActionCode, StringComparison.Ordinal) ||
            !string.Equals(reapply.OperationType, original.OperationType, StringComparison.Ordinal) ||
            !string.Equals(reapply.EntityType, original.EntityType, StringComparison.Ordinal) ||
            reapply.EntityId != original.EntityId)
            throw new SyncRuleException("REAPPLY_SCOPE_MISMATCH", original.Id.ToString());

        var shape = SyncActionCatalog.ValidateShape(reapply.ActionCode, reapply.OperationType,
            reapply.EntityType, reapply.EntityId, reapply.BaseVersion);
        if (shape.ErrorCode is not null)
            throw new SyncRuleException(shape.ErrorCode, original.Id.ToString());
        if (await db.SyncOperations.AsNoTracking().AnyAsync(x =>
                x.CompanyId == context.CompanyId && x.RegisteredDeviceId == context.RegisteredDeviceId &&
                (x.ClientOperationId == reapply.ClientOperationId.Trim() ||
                 x.OperationCorrelationId == reapply.OperationCorrelationId), cancellationToken))
            throw new SyncRuleException("REAPPLY_ID_REUSE", original.Id.ToString());

        var payloadHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(reapply.PayloadJson)))
            .ToLowerInvariant();
        try
        {
            _ = SyncOperationFingerprintV1.ComputeHash(new SyncOperationFingerprintV1Input(
                context.CompanyId, context.RegisteredDeviceId, context.UserId, context.BranchId,
                "sync-v1", original.ActionCode!, original.OperationType, original.EntityType,
                original.EntityId, reapply.ClientOperationId.Trim(), payloadHash,
                CanonicalTimestamp(reapply.ClientOccurredAt), reapply.BaseVersion,
                reapply.OperationCorrelationId));
        }
        catch (ArgumentException exception)
        {
            throw new SyncRuleException("REAPPLY_REQUEST_INVALID", exception.ParamName ?? original.Id.ToString());
        }
    }

    private static void ValidateRequest(
        Guid conflictCaseId,
        ResolveSyncConflictRequest request,
        SyncConflictResolutionContext context)
    {
        if (conflictCaseId == Guid.Empty || context.UserId == Guid.Empty || context.CompanyId == Guid.Empty ||
            context.BranchId == Guid.Empty || context.RegisteredDeviceId == Guid.Empty ||
            context.RegisteredDeviceCredentialVersion < 1 || string.IsNullOrWhiteSpace(context.DeviceId) ||
            context.CorrelationId == Guid.Empty)
            throw new SyncRuleException("SCOPE_DENIED", conflictCaseId.ToString());
        if (request is null || string.IsNullOrWhiteSpace(request.Decision) ||
            request.Decision is not (SyncConflictResolutionDecisions.KeepServerAndRejectLocal or
                SyncConflictResolutionDecisions.ReapplyAsNew))
            throw new SyncRuleException("RESOLUTION_INVALID", conflictCaseId.ToString());
        if (string.IsNullOrWhiteSpace(request.Reason) || request.Reason.Trim().Length > 500)
            throw new SyncRuleException("REASON_REQUIRED", conflictCaseId.ToString());
        if (request.Decision == SyncConflictResolutionDecisions.KeepServerAndRejectLocal && request.Reapply is not null)
            throw new SyncRuleException("RESOLUTION_INVALID", conflictCaseId.ToString());
        if (request.Decision == SyncConflictResolutionDecisions.ReapplyAsNew && request.Reapply is null)
            throw new SyncRuleException("REAPPLY_REQUEST_INVALID", conflictCaseId.ToString());
    }

    private static DateTimeOffset NormalizeTimestamp(DateTimeOffset value)
    {
        var ticks = value.UtcDateTime.Ticks;
        return new DateTimeOffset(new DateTime(ticks - ticks % TimeSpan.TicksPerMicrosecond, DateTimeKind.Utc));
    }

    private static string CanonicalTimestamp(DateTimeOffset value)
        => NormalizeTimestamp(value).ToString("yyyy-MM-dd'T'HH:mm:ss.FFFFFF'Z'", CultureInfo.InvariantCulture);
}
