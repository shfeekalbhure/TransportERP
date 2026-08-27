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
    string PayloadJson,
    string PayloadHash);

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
        AcceptedSyncProofContext acceptedProof,
        CancellationToken cancellationToken = default);
}

public sealed class SyncConflictResolutionService(
    TransportErpDbContext db,
    AuditEventService audit,
    IEffectivePermissionResolver permissions,
    SyncOperationService syncOperations,
    TimeProvider? timeProvider = null) : ISyncConflictResolutionService
{
    private readonly TimeProvider clock = timeProvider ?? TimeProvider.System;

    public async Task<SyncConflictResolutionResult> ResolveAsync(
        Guid conflictCaseId,
        ResolveSyncConflictRequest request,
        SyncConflictResolutionContext context,
        AcceptedSyncProofContext acceptedProof,
        CancellationToken cancellationToken = default)
    {
        ValidateRequest(conflictCaseId, request, context, acceptedProof);
        if (!db.Database.IsNpgsql())
            throw new SyncRuleException("CONFLICT_STORE_UNSUPPORTED", "PostgreSQL is required");

        await using var transaction = await db.Database.BeginTransactionAsync(
            IsolationLevel.ReadCommitted, cancellationToken);
        try
        {
            var userLockKey = "user-scope|" + context.UserId;
            await db.Database.ExecuteSqlInterpolatedAsync(
                $"SELECT pg_advisory_xact_lock(hashtextextended({userLockKey}, 0))", cancellationToken);
            if (request.Decision == SyncConflictResolutionDecisions.ReapplyAsNew &&
                !string.IsNullOrWhiteSpace(request.Reapply!.ClientOperationId) &&
                request.Reapply.ClientOperationId.Trim().Length <= 120)
            {
                var replacementIdempotencyLockKey =
                    $"sync-stage4|{context.CompanyId}|{context.RegisteredDeviceId}|{request.Reapply.ClientOperationId.Trim()}";
                await db.Database.ExecuteSqlInterpolatedAsync(
                    $"SELECT pg_advisory_xact_lock(hashtextextended({replacementIdempotencyLockKey}, 0))",
                    cancellationToken);
            }

            // Legal-hold propagation owns the parent operation before its conflict. Resolve in
            // the same global order to prevent operation-hold versus resolution deadlocks.
            var operations = await db.SyncOperations.FromSqlInterpolated($$"""
                SELECT o.* FROM transport_erp.sync_operations AS o
                JOIN transport_erp.conflict_cases AS c
                  ON c."SyncOperationId"=o."Id" AND c."CompanyId"=o."CompanyId"
                WHERE c."Id"={{conflictCaseId}}
                FOR UPDATE OF o
                """).AsTracking().ToListAsync(cancellationToken);
            var operation = operations.SingleOrDefault()
                ?? throw new SyncRuleException("CONFLICT_NOT_FOUND", conflictCaseId.ToString());

            var conflicts = await db.ConflictCases.FromSqlInterpolated($$"""
                SELECT c.* FROM transport_erp.conflict_cases AS c
                WHERE c."Id"={{conflictCaseId}}
                FOR UPDATE OF c
                """).AsTracking().ToListAsync(cancellationToken);
            var conflict = conflicts.SingleOrDefault()
                ?? throw new SyncRuleException("CONFLICT_NOT_FOUND", conflictCaseId.ToString());
            if (conflict.SyncOperationId != operation.Id || conflict.CompanyId != operation.CompanyId)
                throw new SyncRuleException("CONFLICT_SCOPE_MISMATCH", conflictCaseId.ToString());

            await EnsureAuthorityAsync(conflict, operation, context, cancellationToken);
            await EnsureFreshAcceptedProofAsync(acceptedProof, cancellationToken);
            if (operation.AcceptedProofReplayId == acceptedProof.ReplayId)
                throw new SyncRuleException("invalid_dpop_proof", conflictCaseId.ToString());
            if (conflict.Status != "OPEN" || operation.Status != "CONFLICT" ||
                conflict.Resolution is not null || conflict.ResolvedAt.HasValue || conflict.ReplacedByOperationId.HasValue)
            {
                if (!await IsExactResolutionReplayAsync(conflict, operation, request, cancellationToken))
                    throw new SyncRuleException("CONFLICT_ALREADY_RESOLVED", conflictCaseId.ToString());

                await audit.AppendAuditEventAsync(new AuditEventDraft(
                    "SyncConflictResolutionReplayed", "SUCCESS", nameof(ConflictCase), conflict.Id,
                    context.UserId, context.CompanyId, context.BranchId, context.CorrelationId,
                    context.DeviceId,
                    AfterJson: JsonSerializer.Serialize(new
                    {
                        Decision = conflict.Resolution,
                        conflict.SyncOperationId,
                        conflict.ReplacedByOperationId
                    }),
                    Reason: "IDEMPOTENT_RESOLUTION_REPLAY",
                    OperationCorrelationId: operation.OperationCorrelationId), cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return Result(conflict, operation, context.CorrelationId, conflict.ResolvedAt!.Value);
            }

            if (request.Decision == SyncConflictResolutionDecisions.ReapplyAsNew)
            {
                var reapply = request.Reapply!;
                var clientOperationId = reapply.ClientOperationId.Trim();
                await ValidateReapplyAsync(operation, reapply, context, cancellationToken);

                // Enqueue is deliberately the first state mutation. It shares this transaction,
                // so the replacement and both resolution links either commit together or not at all.
                var replacement = await syncOperations.EnqueueAcceptedSyncOperationAsync(
                    new EnqueueAcceptedSyncOperationCommand(
                        "sync-v1", reapply.ActionCode, reapply.OperationType, reapply.EntityType,
                        reapply.EntityId, clientOperationId, reapply.PayloadJson,
                        reapply.PayloadHash.ToLowerInvariant(), reapply.ClientOccurredAt,
                        reapply.OperationCorrelationId, reapply.BaseVersion),
                    acceptedProof, cancellationToken);
                if (replacement.Id == operation.Id || replacement.CompanyId != operation.CompanyId ||
                    replacement.BranchId != operation.BranchId || replacement.UserId != context.UserId ||
                    replacement.RegisteredDeviceId != context.RegisteredDeviceId ||
                    !string.Equals(replacement.DeviceId, context.DeviceId, StringComparison.Ordinal) ||
                    !string.Equals(replacement.ActionCode, operation.ActionCode, StringComparison.Ordinal) ||
                    !string.Equals(replacement.OperationType, operation.OperationType, StringComparison.Ordinal) ||
                    !string.Equals(replacement.EntityType, operation.EntityType, StringComparison.Ordinal) ||
                    replacement.EntityId != operation.EntityId || replacement.Status != "QUEUED")
                    throw new SyncRuleException("REAPPLY_SCOPE_MISMATCH", conflictCaseId.ToString());

                var reapplyNow = NormalizeTimestamp(clock.GetUtcNow());
                conflict.Status = "RESOLVED";
                conflict.Resolution = SyncConflictResolutionDecisions.ReapplyAsNew;
                conflict.ResolvedBy = context.UserId.ToString();
                conflict.ResolvedAt = reapplyNow;
                conflict.ReplacedByOperationId = replacement.Id;
                conflict.UpdatedAt = reapplyNow;
                conflict.RowVersion = Guid.NewGuid().ToByteArray();

                operation.Status = "RESOLVED";
                operation.ErrorCode = "SUPERSEDED";
                operation.ResultEntityId = null;
                operation.ResultVersion = null;
                operation.NextRetryAt = null;
                operation.ExecutionClaimToken = null;
                operation.ExecutionAttemptStartedAt = null;
                operation.ExecutionLeaseExpiresAt = null;
                operation.UpdatedAt = reapplyNow;
                operation.RowVersion = Guid.NewGuid().ToByteArray();

                await db.SaveChangesAsync(cancellationToken);
                await AppendResolutionAuditAsync(conflict, operation, context, request.Reason, cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return Result(conflict, operation, context.CorrelationId, reapplyNow);
            }

            var now = NormalizeTimestamp(clock.GetUtcNow());
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
            await AppendResolutionAuditAsync(conflict, operation, context, request.Reason, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return Result(conflict, operation, context.CorrelationId, now);
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
            conflict.CompanyId != context.CompanyId || conflict.BranchId != context.BranchId)
            throw new SyncRuleException("SCOPE_DENIED", conflict.Id.ToString());

        var now = clock.GetUtcNow();
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

    private async Task EnsureFreshAcceptedProofAsync(
        AcceptedSyncProofContext proof,
        CancellationToken cancellationToken)
    {
        var now = clock.GetUtcNow();
        var exists = await db.SyncProofReplays.AsNoTracking().AnyAsync(x =>
            x.Id == proof.ReplayId && x.UserId == proof.UserId && x.CompanyId == proof.CompanyId &&
            x.BranchId == proof.BranchId && x.RegisteredDeviceId == proof.RegisteredDeviceId &&
            x.DeviceId == proof.DeviceId && x.ProofKeyVersion == proof.ProofKeyVersion &&
            x.ProofKeyThumbprint == proof.ProofKeyThumbprint &&
            x.AttemptCorrelationId == proof.AttemptCorrelationId && x.ExpiresAt > now,
            cancellationToken);
        var alreadyConsumed = await db.SyncOperations.AsNoTracking().AnyAsync(
            x => x.AcceptedProofReplayId == proof.ReplayId, cancellationToken);
        if (!exists || alreadyConsumed)
            throw new SyncRuleException("invalid_dpop_proof", proof.ReplayId.ToString());
    }

    private async Task ValidateReapplyAsync(
        SyncOperation original,
        SyncReapplyAsNewRequest reapply,
        SyncConflictResolutionContext context,
        CancellationToken cancellationToken)
    {
        if (reapply is null || string.IsNullOrWhiteSpace(reapply.ClientOperationId) ||
            reapply.ClientOperationId.Trim().Length > 120 || reapply.OperationCorrelationId == Guid.Empty ||
            string.IsNullOrWhiteSpace(reapply.PayloadJson) || reapply.ClientOccurredAt == default ||
            string.IsNullOrWhiteSpace(reapply.PayloadHash) || reapply.PayloadHash.Length != 64)
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
        byte[] suppliedHash;
        try { suppliedHash = Convert.FromHexString(reapply.PayloadHash); }
        catch (FormatException) { throw new SyncRuleException("REAPPLY_REQUEST_INVALID", original.Id.ToString()); }
        if (!CryptographicOperations.FixedTimeEquals(Convert.FromHexString(payloadHash), suppliedHash))
            throw new SyncRuleException("HASH_MISMATCH", original.Id.ToString());
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

    private async Task<bool> IsExactResolutionReplayAsync(
        ConflictCase conflict,
        SyncOperation original,
        ResolveSyncConflictRequest request,
        CancellationToken cancellationToken)
    {
        if (conflict.Status != "RESOLVED" || !conflict.ResolvedAt.HasValue ||
            !string.Equals(conflict.Resolution, request.Decision, StringComparison.Ordinal))
            return false;

        var originalReason = await db.AuditEvents.AsNoTracking()
            .Where(x => x.Action == "SyncConflictResolved" && x.EntityId == conflict.Id)
            .OrderBy(x => x.SequenceNo)
            .Select(x => x.Reason)
            .FirstOrDefaultAsync(cancellationToken);
        if (!string.Equals(originalReason, request.Reason.Trim(), StringComparison.Ordinal))
            return false;

        if (request.Decision == SyncConflictResolutionDecisions.KeepServerAndRejectLocal)
            return request.Reapply is null && original.Status == "REJECTED" &&
                   original.ErrorCode == "KEEP_SERVER" && conflict.ReplacedByOperationId is null;

        if (request.Reapply is null || !conflict.ReplacedByOperationId.HasValue ||
            original.Status != "RESOLVED" || original.ErrorCode != "SUPERSEDED")
            return false;

        var replacement = await db.SyncOperations.AsNoTracking().SingleOrDefaultAsync(
            x => x.Id == conflict.ReplacedByOperationId.Value, cancellationToken);
        if (replacement is null) return false;
        var reapply = request.Reapply;
        return replacement.CompanyId == original.CompanyId && replacement.BranchId == original.BranchId &&
               replacement.RegisteredDeviceId.HasValue &&
               string.Equals(replacement.ProtocolVersion, "sync-v1", StringComparison.Ordinal) &&
               string.Equals(replacement.ClientOperationId, reapply.ClientOperationId.Trim(), StringComparison.Ordinal) &&
               replacement.OperationCorrelationId == reapply.OperationCorrelationId &&
               string.Equals(replacement.ActionCode, reapply.ActionCode, StringComparison.Ordinal) &&
               string.Equals(replacement.OperationType, reapply.OperationType, StringComparison.Ordinal) &&
               string.Equals(replacement.EntityType, reapply.EntityType, StringComparison.Ordinal) &&
               replacement.EntityId == reapply.EntityId && replacement.BaseVersion == reapply.BaseVersion &&
               replacement.ClientOccurredAt == NormalizeTimestamp(reapply.ClientOccurredAt) &&
               string.Equals(replacement.PayloadHash, reapply.PayloadHash.ToLowerInvariant(), StringComparison.Ordinal) &&
               (replacement.RedactedAt.HasValue ||
                string.Equals(replacement.PayloadJson, reapply.PayloadJson, StringComparison.Ordinal));
    }

    private static void ValidateRequest(
        Guid conflictCaseId,
        ResolveSyncConflictRequest request,
        SyncConflictResolutionContext context,
        AcceptedSyncProofContext acceptedProof)
    {
        if (conflictCaseId == Guid.Empty || context.UserId == Guid.Empty || context.CompanyId == Guid.Empty ||
            context.BranchId == Guid.Empty || context.RegisteredDeviceId == Guid.Empty ||
            context.RegisteredDeviceCredentialVersion < 1 || string.IsNullOrWhiteSpace(context.DeviceId) ||
            context.CorrelationId == Guid.Empty)
            throw new SyncRuleException("SCOPE_DENIED", conflictCaseId.ToString());
        if (acceptedProof.ReplayId == Guid.Empty || acceptedProof.AttemptCorrelationId != context.CorrelationId ||
            acceptedProof.UserId != context.UserId || acceptedProof.CompanyId != context.CompanyId ||
            acceptedProof.BranchId != context.BranchId ||
            acceptedProof.RegisteredDeviceId != context.RegisteredDeviceId ||
            acceptedProof.DeviceCredentialVersion != context.RegisteredDeviceCredentialVersion ||
            !string.Equals(acceptedProof.DeviceId, context.DeviceId, StringComparison.Ordinal))
            throw new SyncRuleException("SCOPE_DENIED", conflictCaseId.ToString());
        if (request is null || string.IsNullOrWhiteSpace(request.Decision) ||
            request.Decision is not (SyncConflictResolutionDecisions.KeepServerAndRejectLocal or
                SyncConflictResolutionDecisions.ReapplyAsNew))
            throw new SyncRuleException("RESOLUTION_INVALID", conflictCaseId.ToString());
        if (string.IsNullOrWhiteSpace(request.Reason) || request.Reason.Trim().Length > 500)
            throw new SyncRuleException("REASON_REQUIRED", conflictCaseId.ToString());
        if (IsUnsafeAuditReason(request.Reason))
            throw new SyncRuleException("REASON_INVALID", conflictCaseId.ToString());
        if (request.Decision == SyncConflictResolutionDecisions.KeepServerAndRejectLocal && request.Reapply is not null)
            throw new SyncRuleException("RESOLUTION_INVALID", conflictCaseId.ToString());
        if (request.Decision == SyncConflictResolutionDecisions.ReapplyAsNew && request.Reapply is null)
            throw new SyncRuleException("REAPPLY_REQUEST_INVALID", conflictCaseId.ToString());
    }

    private static bool IsUnsafeAuditReason(string reason)
    {
        var value = reason.Trim();
        if (value.Any(char.IsControl))
            return true;

        string[] prohibitedTerms =
        [
            "authorization", "bearer", "token", "credential", "password", "secret",
            "proof", "nonce", "jti", "private key", "private_key"
        ];
        if (prohibitedTerms.Any(term => value.Contains(term, StringComparison.OrdinalIgnoreCase)))
            return true;

        // Reject pasted JWTs, credentials and other long ASCII base64/base64url-shaped material
        // even when the operator omits a label. Human Arabic text and ordinary ticket references
        // do not match this shape.
        return value.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)
            .Any(part => part.Length >= 32 && part.All(character =>
                character is >= 'A' and <= 'Z' or >= 'a' and <= 'z' or >= '0' and <= '9' or
                    '-' or '_' or '+' or '/' or '=' or '.'));
    }

    private static DateTimeOffset NormalizeTimestamp(DateTimeOffset value)
    {
        var ticks = value.UtcDateTime.Ticks;
        return new DateTimeOffset(new DateTime(ticks - ticks % TimeSpan.TicksPerMicrosecond, DateTimeKind.Utc));
    }

    private static string CanonicalTimestamp(DateTimeOffset value)
        => NormalizeTimestamp(value).ToString("yyyy-MM-dd'T'HH:mm:ss.FFFFFF'Z'", CultureInfo.InvariantCulture);

    private async Task AppendResolutionAuditAsync(
        ConflictCase conflict,
        SyncOperation operation,
        SyncConflictResolutionContext context,
        string reason,
        CancellationToken cancellationToken)
        => await audit.AppendAuditEventAsync(new AuditEventDraft(
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
            Reason: reason.Trim(),
            OperationCorrelationId: operation.OperationCorrelationId), cancellationToken);

    private static SyncConflictResolutionResult Result(
        ConflictCase conflict,
        SyncOperation operation,
        Guid correlationId,
        DateTimeOffset resolvedAt)
        => new(conflict.Id, operation.Id, conflict.Resolution!, conflict.Status, operation.Status,
            operation.ErrorCode, conflict.ReplacedByOperationId, resolvedAt, correlationId);
}
