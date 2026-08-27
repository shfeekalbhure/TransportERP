using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TransportERP.Application.Sync;

namespace TransportERP.Infrastructure.Persistence;

public sealed record SyncSecurityContext(
    Guid UserId,
    string DeviceId,
    Guid CompanyId,
    Guid? BranchId,
    bool IsDeviceRegistered,
    bool HasExecutePermission,
    Guid? RegisteredDeviceId = null,
    int? RegisteredDeviceCredentialVersion = null);

public sealed record SyncRetryPolicy(
    int MaxRetryCount,
    TimeSpan BaseDelay,
    TimeSpan MaxDelay)
{
    public SyncRetryPolicy Validate()
    {
        if (MaxRetryCount < 0) throw new ArgumentOutOfRangeException(nameof(MaxRetryCount));
        if (BaseDelay <= TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(BaseDelay));
        if (MaxDelay < BaseDelay) throw new ArgumentOutOfRangeException(nameof(MaxDelay));
        return this;
    }
}

/// <summary>
/// Resolves the immutable effective server retry policy for an operation scope.
/// A null result is fail-closed: the worker must not execute or schedule the row.
/// </summary>
public interface ISyncRetryPolicyResolver
{
    ValueTask<SyncRetryPolicy?> ResolveAsync(
        Guid companyId,
        Guid? branchId,
        Guid? registeredDeviceId,
        string? deviceId,
        CancellationToken cancellationToken = default);

    ValueTask<SyncExecutionPolicyDecision> AuthorizeExecutionAsync(
        Guid companyId,
        Guid? branchId,
        Guid? registeredDeviceId,
        string? deviceId,
        string actionCode,
        CancellationToken cancellationToken = default)
        => ValueTask.FromResult(SyncExecutionPolicyDecision.Allowed);
}

public sealed record SyncExecutionPolicyDecision(
    bool IsAllowed,
    string? ErrorCode,
    string? PolicySourceVersion = null,
    string? PolicySourceFingerprint = null)
{
    public static SyncExecutionPolicyDecision Allowed { get; } = new(true, null);
    public static SyncExecutionPolicyDecision Denied(string errorCode)
        => new(false, string.IsNullOrWhiteSpace(errorCode) ? "SYNC_RUNTIME_POLICY_UNAVAILABLE" : errorCode);

    public SyncExecutionPolicyDecision WithPolicySource(string version, string fingerprint)
        => this with { PolicySourceVersion = version, PolicySourceFingerprint = fingerprint };
}

public sealed class FixedSyncRetryPolicyResolver(SyncRetryPolicy policy) : ISyncRetryPolicyResolver
{
    private readonly SyncRetryPolicy validated = policy.Validate();

    public ValueTask<SyncRetryPolicy?> ResolveAsync(
        Guid companyId,
        Guid? branchId,
        Guid? registeredDeviceId,
        string? deviceId,
        CancellationToken cancellationToken = default)
        => ValueTask.FromResult<SyncRetryPolicy?>(validated);
}

public sealed record EnqueueSyncOperationCommand(
    string DeviceId,
    Guid UserId,
    Guid CompanyId,
    Guid? BranchId,
    string OperationType,
    string EntityType,
    Guid EntityId,
    string ClientOperationId,
    string PayloadJson,
    string PayloadHash,
    DateTimeOffset ClientOccurredAt,
    long? BaseVersion = null);

public sealed record EnqueueAcceptedSyncOperationCommand(
    string ProtocolVersion,
    string ActionCode,
    string OperationType,
    string EntityType,
    Guid? EntityId,
    string ClientOperationId,
    string PayloadJson,
    string PayloadHash,
    DateTimeOffset ClientOccurredAt,
    Guid OperationCorrelationId,
    long? BaseVersion = null,
    string? EffectivePolicySourceVersion = null,
    string? EffectivePolicySourceFingerprint = null);

public sealed record TransitionSyncOperationCommand(Guid OperationId, string NewStatus, string? ErrorCode = null);

public sealed record SyncOperationExecutionClaim(
    Guid OperationId,
    Guid ClaimToken,
    DateTimeOffset AttemptStartedAt,
    DateTimeOffset LeaseExpiresAt,
    bool RecoveredStaleClaim,
    Guid CompanyId,
    Guid BranchId,
    Guid UserId,
    Guid RegisteredDeviceId,
    int RegisteredDeviceCredentialVersion,
    int ProofKeyVersion,
    string DeviceId,
    string ProtocolVersion,
    string ActionCode,
    string OperationType,
    string EntityType,
    Guid? EntityId,
    long? BaseVersion,
    string PayloadJson,
    string PayloadHash,
    string ClientOperationId,
    Guid OperationCorrelationId,
    int ServerRetryCount);

public sealed record SyncExecutionSuccess(Guid ResultEntityId, long? ResultVersion);

public sealed record ConflictCaseDraft(
    string DeviceSnapshot,
    string ServerSnapshot,
    string ConflictReason,
    long? BaseVersion = null);

public sealed class SyncRuleException(string code, string detail) : InvalidOperationException($"{code}: {detail}")
{
    public string Code { get; } = code;
}

public sealed class SyncOperationService
{
    private readonly TransportErpDbContext db;
    private readonly AuditEventService audit;
    private readonly SyncRetryPolicy _globalRetryPolicy;
    private readonly ISyncRetryPolicyResolver _retryPolicies;

    public SyncOperationService(
        TransportErpDbContext db,
        AuditEventService audit,
        SyncRetryPolicy globalRetryPolicy)
        : this(db, audit, globalRetryPolicy, new FixedSyncRetryPolicyResolver(globalRetryPolicy))
    {
    }

    public SyncOperationService(
        TransportErpDbContext db,
        AuditEventService audit,
        SyncRetryPolicy globalRetryPolicy,
        ISyncRetryPolicyResolver retryPolicies)
    {
        this.db = db;
        this.audit = audit;
        _globalRetryPolicy = globalRetryPolicy.Validate();
        _retryPolicies = retryPolicies ?? throw new ArgumentNullException(nameof(retryPolicies));
    }

    /// <summary>
    /// Atomically claims one due Stage 4 operation. The database row lock is held only until the
    /// claim token and lease have been persisted; business execution happens after this method returns.
    /// </summary>
    public async Task<SyncOperationExecutionClaim?> ClaimNextExecutionAsync(
        TimeSpan leaseDuration,
        DateTimeOffset? now = null,
        CancellationToken cancellationToken = default)
    {
        if (leaseDuration < TimeSpan.FromSeconds(5) || leaseDuration > TimeSpan.FromMinutes(30))
            throw new ArgumentOutOfRangeException(nameof(leaseDuration));
        if (!db.Database.IsNpgsql())
            throw new SyncRuleException("EXECUTION_STORE_UNSUPPORTED", "PostgreSQL is required");

        var claimedAt = NormalizePostgreSqlTimestamp(now ?? DateTimeOffset.UtcNow);
        var leaseExpiresAt = NormalizePostgreSqlTimestamp(claimedAt.Add(leaseDuration));
        await using var transaction = await db.Database.BeginTransactionAsync(
            IsolationLevel.ReadCommitted, cancellationToken);
        try
        {
            var candidates = await db.SyncOperations.FromSqlInterpolated($$"""
                SELECT o.*
                FROM transport_erp.sync_operations AS o
                WHERE o."ActionCode" IS NOT NULL
                  AND o."ProtocolVersion" = 'sync-v1'
                  AND o."RegisteredDeviceId" IS NOT NULL
                  AND o."BranchId" IS NOT NULL
                  AND (
                    o."Status" = 'QUEUED'
                    OR (o."Status" = 'FAILED'
                        AND o."NextRetryAt" IS NOT NULL
                        AND o."NextRetryAt" <= {{claimedAt}}
                        AND o."RetryCount" <= {{_globalRetryPolicy.MaxRetryCount}})
                    OR (o."Status" = 'SENDING'
                        AND o."ExecutionLeaseExpiresAt" IS NOT NULL
                        AND o."ExecutionLeaseExpiresAt" <= {{claimedAt}})
                  )
                ORDER BY
                  CASE WHEN o."Status" = 'SENDING' THEN 0
                       WHEN o."Status" = 'FAILED' THEN 1 ELSE 2 END,
                  o."ExecutionLeaseExpiresAt" NULLS LAST,
                  o."NextRetryAt" NULLS LAST,
                  o."CreatedAt",
                  o."Id"
                FOR UPDATE OF o SKIP LOCKED
                LIMIT 1
                """).AsTracking().ToListAsync(cancellationToken);

            var operation = candidates.SingleOrDefault();
            if (operation is null)
            {
                await transaction.CommitAsync(cancellationToken);
                return null;
            }

            var effectiveRetryPolicy = await ResolveRetryPolicyAsync(operation, cancellationToken);
            if (effectiveRetryPolicy is null || operation.RetryCount > effectiveRetryPolicy.MaxRetryCount)
            {
                var missing = effectiveRetryPolicy is null;
                operation.Status = missing ? "FAILED" : "REJECTED";
                operation.ErrorCode = missing ? "SYNC_RUNTIME_POLICY_UNAVAILABLE" : "RETRY_EXHAUSTED";
                operation.NextRetryAt = null;
                ClearExecutionClaim(operation);
                operation.UpdatedAt = claimedAt;
                operation.RowVersion = Guid.NewGuid().ToByteArray();
                await db.SaveChangesAsync(cancellationToken);
                await audit.AppendAuditEventAsync(new AuditEventDraft(
                    missing ? "SyncOperationExecutionPolicyUnavailable" : "SyncOperationExecutionRetryExhausted",
                    missing ? "FAILED" : "REJECTED", nameof(SyncOperation), operation.Id,
                    operation.UserId, operation.CompanyId, operation.BranchId,
                    Guid.NewGuid(), operation.DeviceId,
                    Reason: $"{operation.ErrorCode};RetryCount={operation.RetryCount}",
                    OperationCorrelationId: operation.OperationCorrelationId), cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return null;
            }

            var executionPolicy = await _retryPolicies.AuthorizeExecutionAsync(
                operation.CompanyId, operation.BranchId, operation.RegisteredDeviceId,
                operation.DeviceId, operation.ActionCode!, cancellationToken);
            if (!executionPolicy.IsAllowed)
            {
                operation.Status = "REJECTED";
                operation.ErrorCode = executionPolicy.ErrorCode ?? "SYNC_RUNTIME_POLICY_UNAVAILABLE";
                operation.NextRetryAt = null;
                ClearExecutionClaim(operation);
                operation.UpdatedAt = claimedAt;
                operation.RowVersion = Guid.NewGuid().ToByteArray();
                await db.SaveChangesAsync(cancellationToken);
                await audit.AppendAuditEventAsync(new AuditEventDraft(
                    "SyncOperationExecutionPolicyDenied", "REJECTED", nameof(SyncOperation), operation.Id,
                    operation.UserId, operation.CompanyId, operation.BranchId,
                    Guid.NewGuid(), operation.DeviceId,
                    Reason: $"{operation.ErrorCode};{PolicySourceReason(executionPolicy)}",
                    OperationCorrelationId: operation.OperationCorrelationId), cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return null;
            }

            if (operation.Status == "FAILED")
            {
                var retryNumber = Math.Max(1, operation.RetryCount);
                var requiredNextRetryAt = NormalizePostgreSqlTimestamp(
                    operation.UpdatedAt.Add(CalculateBackoff(effectiveRetryPolicy, retryNumber)));
                if (operation.NextRetryAt < requiredNextRetryAt)
                {
                    operation.NextRetryAt = requiredNextRetryAt;
                    operation.RowVersion = Guid.NewGuid().ToByteArray();
                    await db.SaveChangesAsync(cancellationToken);
                    await audit.AppendAuditEventAsync(new AuditEventDraft(
                        "SyncOperationExecutionBackoffTightened", "SUCCESS", nameof(SyncOperation), operation.Id,
                        operation.UserId, operation.CompanyId, operation.BranchId,
                        Guid.NewGuid(), operation.DeviceId,
                        Reason: $"RetryCount={operation.RetryCount};NextRetryAt={requiredNextRetryAt:O}",
                        OperationCorrelationId: operation.OperationCorrelationId), cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    return null;
                }
            }

            var recoveredStaleClaim = operation.Status == "SENDING";
            var claimToken = Guid.NewGuid();
            operation.Status = "SENDING";
            operation.ExecutionClaimToken = claimToken;
            operation.ExecutionAttemptStartedAt = claimedAt;
            operation.ExecutionLeaseExpiresAt = leaseExpiresAt;
            operation.NextRetryAt = null;
            operation.UpdatedAt = claimedAt;
            operation.RowVersion = Guid.NewGuid().ToByteArray();
            await db.SaveChangesAsync(cancellationToken);
            await audit.AppendAuditEventAsync(new AuditEventDraft(
                recoveredStaleClaim ? "SyncOperationExecutionReclaimed" : "SyncOperationExecutionClaimed",
                "SUCCESS", nameof(SyncOperation), operation.Id,
                operation.UserId, operation.CompanyId, operation.BranchId,
                claimToken, operation.DeviceId,
                Reason: $"LeaseExpiresAt={leaseExpiresAt:O};{PolicySourceReason(executionPolicy)}",
                OperationCorrelationId: operation.OperationCorrelationId), cancellationToken);
            var claim = ToExecutionClaim(operation, claimToken, claimedAt, leaseExpiresAt, recoveredStaleClaim);
            await transaction.CommitAsync(cancellationToken);
            return claim;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            db.ChangeTracker.Clear();
            throw;
        }
    }

    public async Task<SyncOperation> CompleteExecutionSuccessAsync(
        Guid operationId,
        Guid claimToken,
        SyncExecutionSuccess result,
        DateTimeOffset? now = null,
        CancellationToken cancellationToken = default)
    {
        if (result.ResultEntityId == Guid.Empty)
            throw new SyncRuleException("RESULT_ENTITY_INVALID", operationId.ToString());
        if (result.ResultVersion is <= 0)
            throw new SyncRuleException("RESULT_VERSION_INVALID", operationId.ToString());

        var completedAt = NormalizePostgreSqlTimestamp(now ?? DateTimeOffset.UtcNow);
        return await CompleteClaimAsync(operationId, claimToken, completedAt, async operation =>
        {
            var definition = SyncActionCatalog.Definitions.SingleOrDefault(x =>
                string.Equals(x.ActionCodeValue, operation.ActionCode, StringComparison.Ordinal));
            if (definition is null ||
                (definition.ResultVersionRequired && !result.ResultVersion.HasValue))
                throw new SyncRuleException("RESULT_VERSION_REQUIRED", operationId.ToString());
            operation.ResultEntityId = result.ResultEntityId;
            operation.ResultVersion = result.ResultVersion;
            operation.Status = "SUCCEEDED";
            operation.ErrorCode = null;
            operation.NextRetryAt = null;
            ClearExecutionClaim(operation);
            await audit.AppendAuditEventAsync(new AuditEventDraft(
                "SyncOperationExecutionSucceeded", "SUCCESS", nameof(SyncOperation), operation.Id,
                operation.UserId, operation.CompanyId, operation.BranchId,
                claimToken, operation.DeviceId,
                OperationCorrelationId: operation.OperationCorrelationId), cancellationToken);
        }, cancellationToken);
    }

    /// <summary>
    /// Records an actual executor failure. Claiming, replaying enqueue, or recovering an expired
    /// lease never calls this method and therefore never consumes the server retry counter.
    /// </summary>
    public async Task<SyncOperation> CompleteExecutionFailureAsync(
        Guid operationId,
        Guid claimToken,
        string errorCode,
        DateTimeOffset? now = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedError = errorCode.Trim().ToUpperInvariant();
        if (normalizedError.Length is < 1 or > 80)
            throw new SyncRuleException("ERROR_CODE_INVALID", operationId.ToString());

        var failedAt = NormalizePostgreSqlTimestamp(now ?? DateTimeOffset.UtcNow);
        return await CompleteClaimAsync(operationId, claimToken, failedAt, async operation =>
        {
            var retryPolicy = await ResolveRetryPolicyAsync(operation, cancellationToken);
            string auditAction;
            string auditOutcome;
            if (retryPolicy is null)
            {
                operation.Status = "FAILED";
                operation.ErrorCode = "SYNC_RUNTIME_POLICY_UNAVAILABLE";
                operation.NextRetryAt = null;
                auditAction = "SyncOperationExecutionPolicyUnavailable";
                auditOutcome = "FAILED";
            }
            else if (string.Equals(normalizedError, "RATE_LIMITED", StringComparison.Ordinal))
            {
                if (operation.RetryCount >= retryPolicy.MaxRetryCount)
                {
                    operation.Status = "REJECTED";
                    operation.ErrorCode = "RETRY_EXHAUSTED";
                    operation.NextRetryAt = null;
                    auditAction = "SyncOperationExecutionRetryExhausted";
                    auditOutcome = "REJECTED";
                }
                else
                {
                    operation.RetryCount++;
                    operation.Status = "FAILED";
                    operation.ErrorCode = normalizedError;
                    operation.NextRetryAt = NormalizePostgreSqlTimestamp(
                        failedAt.Add(CalculateBackoff(retryPolicy, operation.RetryCount)));
                    auditAction = "SyncOperationExecutionFailed";
                    auditOutcome = "FAILED";
                }
            }
            else
            {
                operation.Status = "REJECTED";
                operation.ErrorCode = normalizedError;
                operation.NextRetryAt = null;
                auditAction = "SyncOperationExecutionRejected";
                auditOutcome = "REJECTED";
            }

            ClearExecutionClaim(operation);
            await audit.AppendAuditEventAsync(new AuditEventDraft(
                auditAction, auditOutcome, nameof(SyncOperation), operation.Id,
                operation.UserId, operation.CompanyId, operation.BranchId,
                claimToken, operation.DeviceId,
                Reason: $"{operation.ErrorCode};RetryCount={operation.RetryCount}",
                OperationCorrelationId: operation.OperationCorrelationId), cancellationToken);
        }, cancellationToken);
    }

    /// <summary>
    /// Schedules bounded recovery when the business commit outcome is ambiguous. The replay uses
    /// the same tenant-scoped business idempotency key, so it can recover a committed result without
    /// repeating the effect. The shared server retry budget preserves the original + five retries
    /// contract. Exhaustion remains FAILED (not REJECTED), because the business effect may exist.
    /// </summary>
    public async Task<SyncOperation> CompleteExecutionPendingAsync(
        Guid operationId,
        Guid claimToken,
        DateTimeOffset? now = null,
        CancellationToken cancellationToken = default)
    {
        var pendingAt = NormalizePostgreSqlTimestamp(now ?? DateTimeOffset.UtcNow);
        return await CompleteClaimAsync(operationId, claimToken, pendingAt, async operation =>
        {
            var retryPolicy = await ResolveRetryPolicyAsync(operation, cancellationToken);
            string auditAction;
            if (retryPolicy is null)
            {
                operation.Status = "FAILED";
                operation.ErrorCode = "SYNC_RUNTIME_POLICY_UNAVAILABLE";
                operation.NextRetryAt = null;
                auditAction = "SyncOperationExecutionPolicyUnavailable";
            }
            else if (operation.RetryCount >= retryPolicy.MaxRetryCount)
            {
                operation.Status = "FAILED";
                operation.ErrorCode = "COMPLETION_RECOVERY_EXHAUSTED";
                operation.NextRetryAt = null;
                auditAction = "SyncOperationCompletionRecoveryExhausted";
            }
            else
            {
                operation.RetryCount++;
                operation.Status = "FAILED";
                operation.ErrorCode = "COMPLETION_PENDING";
                operation.NextRetryAt = NormalizePostgreSqlTimestamp(
                    pendingAt.Add(CalculateBackoff(retryPolicy, operation.RetryCount)));
                auditAction = "SyncOperationCompletionRecoveryScheduled";
            }

            ClearExecutionClaim(operation);
            await audit.AppendAuditEventAsync(new AuditEventDraft(
                auditAction, "FAILED", nameof(SyncOperation), operation.Id,
                operation.UserId, operation.CompanyId, operation.BranchId,
                claimToken, operation.DeviceId,
                Reason: $"{operation.ErrorCode};RetryCount={operation.RetryCount}",
                OperationCorrelationId: operation.OperationCorrelationId), cancellationToken);
        }, cancellationToken);
    }

    public async Task<SyncOperation> CompleteExecutionConflictAsync(
        Guid operationId,
        Guid claimToken,
        string errorCode,
        DateTimeOffset? now = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedError = errorCode.Trim().ToUpperInvariant();
        if (normalizedError is not ("CONCURRENCY_CONFLICT" or "BASE_VERSION_CONFLICT"))
            throw new SyncRuleException("CONFLICT_CODE_INVALID", operationId.ToString());

        var completedAt = NormalizePostgreSqlTimestamp(now ?? DateTimeOffset.UtcNow);
        return await CompleteClaimAsync(operationId, claimToken, completedAt, async operation =>
        {
            if (await db.ConflictCases.AnyAsync(x => x.SyncOperationId == operation.Id, cancellationToken))
                throw new SyncRuleException("CONFLICT_ALREADY_EXISTS", operationId.ToString());

            long? currentServerVersion = null;
            var serverEntityExists = false;
            if (string.Equals(operation.EntityType, "Waybill", StringComparison.Ordinal) &&
                operation.EntityId.HasValue)
            {
                var serverState = await db.Set<WaybillEntity>().AsNoTracking()
                    .Where(x => x.Id == operation.EntityId.Value && x.CompanyId == operation.CompanyId &&
                                x.BranchId == operation.BranchId)
                    .Select(x => new { x.Version })
                    .SingleOrDefaultAsync(cancellationToken);
                serverEntityExists = serverState is not null;
                currentServerVersion = serverState?.Version;
            }

            var conflict = new ConflictCase
            {
                Id = Guid.NewGuid(),
                SyncOperationId = operation.Id,
                CompanyId = operation.CompanyId,
                BranchId = operation.BranchId,
                BaseVersion = operation.BaseVersion,
                // Typed decision metadata only: enough to compare the requested and current
                // versions without copying payload, party, address, identity or contact fields.
                DeviceSnapshot = JsonSerializer.Serialize(new
                {
                    operation.ActionCode,
                    operation.EntityType,
                    operation.EntityId,
                    RequestedBaseVersion = operation.BaseVersion
                }),
                ServerSnapshot = JsonSerializer.Serialize(new
                {
                    operation.EntityType,
                    operation.EntityId,
                    Exists = serverEntityExists,
                    CurrentVersion = currentServerVersion
                }),
                ConflictReason = normalizedError,
                Status = "OPEN",
                CreatedAt = completedAt,
                UpdatedAt = completedAt,
                RowVersion = Guid.NewGuid().ToByteArray()
            };
            db.ConflictCases.Add(conflict);
            operation.ConflictCase = conflict;
            operation.Status = "CONFLICT";
            operation.ErrorCode = normalizedError;
            operation.NextRetryAt = null;
            ClearExecutionClaim(operation);
            await audit.AppendAuditEventAsync(new AuditEventDraft(
                "SyncOperationExecutionConflict", "CONFLICT", nameof(SyncOperation), operation.Id,
                operation.UserId, operation.CompanyId, operation.BranchId,
                claimToken, operation.DeviceId,
                Reason: $"{normalizedError};BaseVersion={operation.BaseVersion?.ToString() ?? "none"}",
                OperationCorrelationId: operation.OperationCorrelationId), cancellationToken);
        }, cancellationToken);
    }

    public async Task<SyncOperation> EnqueueAcceptedSyncOperationAsync(
        EnqueueAcceptedSyncOperationCommand command,
        AcceptedSyncProofContext acceptedProof,
        CancellationToken cancellationToken = default)
    {
        ValidateAcceptedCommand(command, acceptedProof);
        if (!PayloadHashMatches(command.PayloadJson, command.PayloadHash))
            throw new SyncRuleException("HASH_MISMATCH", command.ClientOperationId);

        var clientOccurredAt = CanonicalTimestamp(command.ClientOccurredAt);
        byte[] fingerprint;
        try
        {
            fingerprint = SyncOperationFingerprintV1.ComputeHash(new SyncOperationFingerprintV1Input(
                acceptedProof.CompanyId, acceptedProof.RegisteredDeviceId, acceptedProof.UserId,
                acceptedProof.BranchId, command.ProtocolVersion, command.ActionCode,
                command.OperationType, command.EntityType, command.EntityId,
                command.ClientOperationId, command.PayloadHash, clientOccurredAt,
                command.BaseVersion, command.OperationCorrelationId));
        }
        catch (ArgumentException exception)
        {
            throw new SyncRuleException("PAYLOAD_INVALID", exception.ParamName ?? command.ClientOperationId);
        }

        var ownsTransaction = db.Database.CurrentTransaction is null;
        await using var transaction = ownsTransaction
            ? await db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken)
            : null;
        try
        {
            var userLockKey = "user-scope|" + acceptedProof.UserId;
            await db.Database.ExecuteSqlInterpolatedAsync(
                $"SELECT pg_advisory_xact_lock(hashtextextended({userLockKey}, 0))", cancellationToken);
            var idempotencyLockKey = $"sync-stage4|{acceptedProof.CompanyId}|{acceptedProof.RegisteredDeviceId}|{command.ClientOperationId}";
            await db.Database.ExecuteSqlInterpolatedAsync(
                $"SELECT pg_advisory_xact_lock(hashtextextended({idempotencyLockKey}, 0))", cancellationToken);

            var existing = await db.SyncOperations.Include(x => x.ConflictCase).SingleOrDefaultAsync(x =>
                x.CompanyId == acceptedProof.CompanyId &&
                x.RegisteredDeviceId == acceptedProof.RegisteredDeviceId &&
                x.ClientOperationId == command.ClientOperationId &&
                x.RequestFingerprintVersion == "fp-v1", cancellationToken);
            if (existing is not null)
            {
                EnsureAcceptedReplayMatches(existing, fingerprint, acceptedProof);
                if (transaction is not null) await transaction.CommitAsync(cancellationToken);
                return existing;
            }

            var now = NormalizePostgreSqlTimestamp(DateTimeOffset.UtcNow);
            var operation = new SyncOperation
            {
                Id = Guid.NewGuid(),
                DeviceId = acceptedProof.DeviceId,
                UserId = acceptedProof.UserId,
                CompanyId = acceptedProof.CompanyId,
                BranchId = acceptedProof.BranchId,
                OperationType = command.OperationType,
                EntityType = command.EntityType,
                EntityId = command.EntityId,
                ClientOperationId = command.ClientOperationId,
                PayloadJson = command.PayloadJson,
                PayloadHash = command.PayloadHash.ToLowerInvariant(),
                ClientOccurredAt = NormalizePostgreSqlTimestamp(command.ClientOccurredAt),
                ServerReceivedAt = now,
                BaseVersion = command.BaseVersion,
                Status = "QUEUED",
                RetryCount = 0,
                RegisteredDeviceId = acceptedProof.RegisteredDeviceId,
                // Snapshot claim-time provenance; enqueue does not re-lock the live device.
                RegisteredDeviceCredentialVersion = acceptedProof.DeviceCredentialVersion,
                ActionCode = command.ActionCode,
                ProtocolVersion = command.ProtocolVersion,
                OperationCorrelationId = command.OperationCorrelationId,
                RequestFingerprintVersion = "fp-v1",
                RequestFingerprintHash = fingerprint,
                ProofKeyVersion = acceptedProof.ProofKeyVersion,
                ProofKeyThumbprint = acceptedProof.ProofKeyThumbprint,
                AcceptedProofReplayId = acceptedProof.ReplayId,
                CreatedAt = now,
                UpdatedAt = now,
                RowVersion = Guid.NewGuid().ToByteArray()
            };
            db.SyncOperations.Add(operation);
            await db.SaveChangesAsync(cancellationToken);
            await audit.AppendAuditEventAsync(new AuditEventDraft(
                "SyncOperationQueued", "SUCCESS", nameof(SyncOperation), operation.Id,
                operation.UserId, operation.CompanyId, operation.BranchId,
                acceptedProof.AttemptCorrelationId, operation.DeviceId,
                Reason: PolicySourceReason(command),
                OperationCorrelationId: operation.OperationCorrelationId), cancellationToken);
            if (transaction is not null) await transaction.CommitAsync(cancellationToken);
            return operation;
        }
        catch (DbUpdateException exception) when (ownsTransaction &&
            exception.GetBaseException() is PostgresException
            {
                SqlState: PostgresErrorCodes.UniqueViolation,
                ConstraintName: "ux_sync_op_registered_device_client"
            })
        {
            await transaction!.RollbackAsync(cancellationToken);
            db.ChangeTracker.Clear();
            var existing = await db.SyncOperations.Include(x => x.ConflictCase).SingleAsync(x =>
                x.CompanyId == acceptedProof.CompanyId &&
                x.RegisteredDeviceId == acceptedProof.RegisteredDeviceId &&
                x.ClientOperationId == command.ClientOperationId &&
                x.RequestFingerprintVersion == "fp-v1", cancellationToken);
            EnsureAcceptedReplayMatches(existing, fingerprint, acceptedProof);
            return existing;
        }
        catch (DbUpdateException exception) when (ownsTransaction &&
            exception.GetBaseException() is PostgresException
            {
                SqlState: PostgresErrorCodes.UniqueViolation,
                ConstraintName: "ux_sync_op_legacy_company_device_client"
            })
        {
            await transaction!.RollbackAsync(cancellationToken);
            db.ChangeTracker.Clear();
            throw new SyncRuleException("LEGACY_IDEMPOTENCY_CONFLICT", command.ClientOperationId);
        }
        catch
        {
            if (transaction is not null)
            {
                await transaction.RollbackAsync(cancellationToken);
                db.ChangeTracker.Clear();
            }
            throw;
        }
    }

    public async Task<SyncOperation> EnqueueSyncOperationAsync(
        EnqueueSyncOperationCommand command,
        SyncSecurityContext security,
        CancellationToken cancellationToken = default)
    {
        ValidateCommand(command);
        await EnsureSecurityAsync(security, command.CompanyId, command.BranchId, cancellationToken, command.UserId, command.DeviceId);
        if (!PayloadHashMatches(command.PayloadJson, command.PayloadHash))
            throw new SyncRuleException("HASH_MISMATCH", command.ClientOperationId);

        var existing = await db.SyncOperations
            .Include(x => x.ConflictCase)
            .SingleOrDefaultAsync(
                x => x.DeviceId == command.DeviceId && x.ClientOperationId == command.ClientOperationId,
                cancellationToken);
        if (existing is not null)
        {
            EnsureSameOwnerScope(existing, security);
            if (!string.Equals(existing.PayloadHash, command.PayloadHash, StringComparison.OrdinalIgnoreCase))
                throw new SyncRuleException("IDEMPOTENCY_HASH_MISMATCH", command.ClientOperationId);
            return existing;
        }

        var now = NormalizePostgreSqlTimestamp(DateTimeOffset.UtcNow);
        var operation = new SyncOperation
        {
            Id = Guid.NewGuid(),
            DeviceId = command.DeviceId.Trim(),
            UserId = command.UserId,
            CompanyId = command.CompanyId,
            BranchId = command.BranchId,
            OperationType = command.OperationType.Trim().ToUpperInvariant(),
            EntityType = command.EntityType.Trim(),
            EntityId = command.EntityId,
            ClientOperationId = command.ClientOperationId.Trim(),
            PayloadJson = command.PayloadJson,
            PayloadHash = command.PayloadHash.Trim().ToLowerInvariant(),
            ClientOccurredAt = NormalizePostgreSqlTimestamp(command.ClientOccurredAt),
            ServerReceivedAt = now,
            Status = "QUEUED",
            RetryCount = 0,
            RegisteredDeviceId = security.RegisteredDeviceId,
            RegisteredDeviceCredentialVersion = security.RegisteredDeviceCredentialVersion,
            CreatedAt = now,
            UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        };

        var canRecoverIdempotentRace = db.Database.CurrentTransaction is null;
        try
        {
            await ExecuteMutationAsync(async () =>
            {
                db.SyncOperations.Add(operation);
                await db.SaveChangesAsync(cancellationToken);
                await audit.AppendAuditEventAsync(new AuditEventDraft(
                    "SyncOperationQueued", "SUCCESS", nameof(SyncOperation), operation.Id,
                    security.UserId, operation.CompanyId, operation.BranchId,
                    CorrelationId: Guid.NewGuid(), DeviceId: security.DeviceId), cancellationToken);
                return operation;
            }, cancellationToken);
        }
        catch (DbUpdateException ex) when (canRecoverIdempotentRace &&
                                           ex.GetBaseException() is PostgresException { SqlState: "23505" })
        {
            db.ChangeTracker.Clear();
            var concurrent = await db.SyncOperations
                .Include(x => x.ConflictCase)
                .SingleAsync(
                    x => x.DeviceId == command.DeviceId && x.ClientOperationId == command.ClientOperationId,
                    cancellationToken);
            EnsureSameOwnerScope(concurrent, security);
            if (!string.Equals(concurrent.PayloadHash, command.PayloadHash, StringComparison.OrdinalIgnoreCase))
                throw new SyncRuleException("IDEMPOTENCY_HASH_MISMATCH", command.ClientOperationId);
            return concurrent;
        }
        return operation;
    }

    public async Task<SyncOperation> TransitionSyncOperationAsync(
        TransitionSyncOperationCommand command,
        SyncSecurityContext security,
        CancellationToken cancellationToken = default)
    {
        var operation = await db.SyncOperations.SingleOrDefaultAsync(x => x.Id == command.OperationId, cancellationToken)
            ?? throw new SyncRuleException("OPERATION_NOT_FOUND", command.OperationId.ToString());
        EnsureTenantScope(operation, security);
        await EnsureSecurityAsync(security, operation.CompanyId, operation.BranchId, cancellationToken);

        var newStatus = command.NewStatus.Trim().ToUpperInvariant();
        if (operation.Status == newStatus) return operation;
        if (!IsAllowedTransition(operation.Status, newStatus))
            throw new SyncRuleException("INVALID_STATE_TRANSITION", $"{operation.Status}->{newStatus}");
        if (newStatus == "SENDING" && operation.Status == "FAILED" &&
            operation.NextRetryAt is not null && operation.NextRetryAt > DateTimeOffset.UtcNow)
            throw new SyncRuleException("RETRY_BACKOFF_ACTIVE", operation.ClientOperationId);

        var transitionAt = NormalizePostgreSqlTimestamp(DateTimeOffset.UtcNow);
        operation.Status = newStatus;
        if (newStatus == "SENDING")
        {
            // Compatibility for the pre-worker transition API. The production worker uses
            // ClaimNextExecutionAsync, whose SKIP LOCKED transaction is the authoritative claim path.
            operation.ExecutionClaimToken = Guid.NewGuid();
            operation.ExecutionAttemptStartedAt = transitionAt;
            operation.ExecutionLeaseExpiresAt = transitionAt.AddMinutes(5);
        }
        else if (operation.ExecutionClaimToken is not null)
        {
            ClearExecutionClaim(operation);
        }
        if (newStatus == "FAILED")
        {
            if (string.IsNullOrWhiteSpace(command.ErrorCode))
                throw new SyncRuleException("ERROR_CODE_REQUIRED", operation.ClientOperationId);
            operation.ErrorCode = command.ErrorCode.Trim().ToUpperInvariant();
            if (IsRetryableErrorCode(operation.ErrorCode))
            {
                var retryPolicy = await ResolveRetryPolicyAsync(operation, cancellationToken)
                    ?? throw new SyncRuleException("SYNC_RUNTIME_POLICY_UNAVAILABLE", operation.ClientOperationId);
                // The legacy transition represents a completed SENDING attempt. It therefore
                // follows the same actual-failure accounting as the claim-token completion path.
                if (operation.RetryCount >= retryPolicy.MaxRetryCount)
                {
                    operation.Status = "REJECTED";
                    operation.ErrorCode = "RETRY_EXHAUSTED";
                    operation.NextRetryAt = null;
                }
                else
                {
                    operation.RetryCount++;
                    operation.NextRetryAt = NormalizePostgreSqlTimestamp(
                        transitionAt.Add(CalculateBackoff(retryPolicy, operation.RetryCount)));
                }
            }
            else
            {
                operation.NextRetryAt = null;
            }
        }
        operation.UpdatedAt = transitionAt;
        operation.RowVersion = Guid.NewGuid().ToByteArray();
        if (newStatus == "SUCCEEDED")
        {
            operation.ErrorCode = null;
            operation.NextRetryAt = null;
        }
        return await ExecuteMutationAsync(async () =>
        {
            await db.SaveChangesAsync(cancellationToken);
            await audit.AppendAuditEventAsync(new AuditEventDraft(
                "SyncOperationTransition", "SUCCESS", nameof(SyncOperation), operation.Id,
                security.UserId, operation.CompanyId, operation.BranchId,
                CorrelationId: Guid.NewGuid(), DeviceId: security.DeviceId, Reason: operation.Status), cancellationToken);
            return operation;
        }, cancellationToken);
    }

    public async Task<SyncOperation> RetryOperationAsync(
        Guid operationId,
        SyncSecurityContext security,
        CancellationToken cancellationToken = default)
    {
        var operation = await db.SyncOperations.SingleOrDefaultAsync(x => x.Id == operationId, cancellationToken)
            ?? throw new SyncRuleException("OPERATION_NOT_FOUND", operationId.ToString());
        EnsureTenantScope(operation, security);
        await EnsureSecurityAsync(security, operation.CompanyId, operation.BranchId, cancellationToken);
        if (operation.Status != "FAILED")
            throw new SyncRuleException("RETRY_NOT_ALLOWED", operation.ClientOperationId);

        if (!IsRetryableErrorCode(operation.ErrorCode))
        {
            operation.Status = "REJECTED";
            operation.NextRetryAt = null;
            operation.UpdatedAt = NormalizePostgreSqlTimestamp(DateTimeOffset.UtcNow);
            operation.RowVersion = Guid.NewGuid().ToByteArray();
            return await ExecuteMutationAsync(async () =>
            {
                await db.SaveChangesAsync(cancellationToken);
                await audit.AppendAuditEventAsync(new AuditEventDraft(
                    "SyncOperationRetryRejected", "REJECTED", nameof(SyncOperation), operation.Id,
                    security.UserId, operation.CompanyId, operation.BranchId,
                    CorrelationId: Guid.NewGuid(), DeviceId: security.DeviceId,
                    Reason: operation.ErrorCode ?? "ERROR_NOT_RETRYABLE"), cancellationToken);
                return operation;
            }, cancellationToken);
        }

        var retryPolicy = await ResolveRetryPolicyAsync(operation, cancellationToken);
        if (retryPolicy is null)
        {
            operation.Status = "FAILED";
            operation.ErrorCode = "SYNC_RUNTIME_POLICY_UNAVAILABLE";
            operation.NextRetryAt = null;
            operation.UpdatedAt = NormalizePostgreSqlTimestamp(DateTimeOffset.UtcNow);
            operation.RowVersion = Guid.NewGuid().ToByteArray();
            return await ExecuteMutationAsync(async () =>
            {
                await db.SaveChangesAsync(cancellationToken);
                await audit.AppendAuditEventAsync(new AuditEventDraft(
                    "SyncOperationRetryPolicyUnavailable", "FAILED", nameof(SyncOperation), operation.Id,
                    security.UserId, operation.CompanyId, operation.BranchId,
                    CorrelationId: Guid.NewGuid(), DeviceId: security.DeviceId,
                    Reason: operation.ErrorCode), cancellationToken);
                return operation;
            }, cancellationToken);
        }

        if (operation.RetryCount > retryPolicy.MaxRetryCount)
        {
            operation.Status = "REJECTED";
            operation.ErrorCode = "RETRY_EXHAUSTED";
            operation.NextRetryAt = null;
            operation.UpdatedAt = NormalizePostgreSqlTimestamp(DateTimeOffset.UtcNow);
            operation.RowVersion = Guid.NewGuid().ToByteArray();
            return await ExecuteMutationAsync(async () =>
            {
                await db.SaveChangesAsync(cancellationToken);
                await audit.AppendAuditEventAsync(new AuditEventDraft(
                    "SyncOperationRetryRejected", "REJECTED", nameof(SyncOperation), operation.Id,
                    security.UserId, operation.CompanyId, operation.BranchId,
                    CorrelationId: Guid.NewGuid(), DeviceId: security.DeviceId, Reason: "RETRY_EXHAUSTED"), cancellationToken);
                return operation;
            }, cancellationToken);
        }

        // A manual retry request only schedules the next attempt. The counter is execution
        // evidence and is incremented exclusively by CompleteExecutionFailureAsync.
        var retryNumber = Math.Max(1, operation.RetryCount);
        var delay = CalculateBackoff(retryPolicy, retryNumber);
        operation.NextRetryAt = NormalizePostgreSqlTimestamp(DateTimeOffset.UtcNow.Add(delay));
        operation.UpdatedAt = NormalizePostgreSqlTimestamp(DateTimeOffset.UtcNow);
        operation.RowVersion = Guid.NewGuid().ToByteArray();
        return await ExecuteMutationAsync(async () =>
        {
            await db.SaveChangesAsync(cancellationToken);
            await audit.AppendAuditEventAsync(new AuditEventDraft(
                "SyncOperationRetry", "SUCCESS", nameof(SyncOperation), operation.Id,
                security.UserId, operation.CompanyId, operation.BranchId,
                CorrelationId: Guid.NewGuid(), DeviceId: security.DeviceId,
                Reason: $"ScheduledRetryNumber={retryNumber};RetryCount={operation.RetryCount}"), cancellationToken);
            return operation;
        }, cancellationToken);
    }

    public async Task<IReadOnlyList<SyncOperation>> GetPendingRetriesAsync(
        SyncSecurityContext security,
        DateTimeOffset? now = null,
        int take = 100,
        CancellationToken cancellationToken = default)
    {
        if (take is < 1 or > 1000) throw new ArgumentOutOfRangeException(nameof(take));
        await EnsureSecurityAsync(security, security.CompanyId, security.BranchId, cancellationToken);
        var retryPolicy = await _retryPolicies.ResolveAsync(
            security.CompanyId, security.BranchId, security.RegisteredDeviceId,
            security.DeviceId, cancellationToken);
        if (retryPolicy is null) return [];
        retryPolicy.Validate();
        var dueAt = now ?? DateTimeOffset.UtcNow;
        var query = db.SyncOperations.AsNoTracking()
            .Where(x => x.CompanyId == security.CompanyId && x.Status == "FAILED" &&
                        x.NextRetryAt != null && x.NextRetryAt <= dueAt &&
                        x.RetryCount <= retryPolicy.MaxRetryCount &&
                        (x.ErrorCode == null || x.ErrorCode == "RATE_LIMITED"));
        if (security.BranchId is not null)
            query = query.Where(x => x.BranchId == security.BranchId);
        return await query.OrderBy(x => x.NextRetryAt).ThenBy(x => x.CreatedAt).Take(take).ToListAsync(cancellationToken);
    }

    public async Task<ConflictCase> CreateConflictCaseAsync(
        Guid operationId,
        ConflictCaseDraft draft,
        SyncSecurityContext security,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(draft.DeviceSnapshot) || string.IsNullOrWhiteSpace(draft.ServerSnapshot) ||
            string.IsNullOrWhiteSpace(draft.ConflictReason))
            throw new SyncRuleException("CONFLICT_DATA_REQUIRED", operationId.ToString());

        var operation = await db.SyncOperations.SingleOrDefaultAsync(x => x.Id == operationId, cancellationToken)
            ?? throw new SyncRuleException("OPERATION_NOT_FOUND", operationId.ToString());
        EnsureTenantScope(operation, security);
        await EnsureSecurityAsync(security, operation.CompanyId, operation.BranchId, cancellationToken);
        if (operation.Status != "CONFLICT")
            throw new SyncRuleException("CONFLICT_NOT_FOUND", operation.ClientOperationId);
        if (await db.ConflictCases.AnyAsync(x => x.SyncOperationId == operationId, cancellationToken))
            throw new SyncRuleException("CONFLICT_ALREADY_EXISTS", operation.ClientOperationId);

        var now = NormalizePostgreSqlTimestamp(DateTimeOffset.UtcNow);
        var conflict = new ConflictCase
        {
            Id = Guid.NewGuid(),
            SyncOperationId = operation.Id,
            CompanyId = operation.CompanyId,
            BranchId = operation.BranchId,
            BaseVersion = draft.BaseVersion ?? operation.BaseVersion,
            DeviceSnapshot = draft.DeviceSnapshot,
            ServerSnapshot = draft.ServerSnapshot,
            ConflictReason = draft.ConflictReason.Trim(),
            Status = "OPEN",
            CreatedAt = now,
            UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        return await ExecuteMutationAsync(async () =>
        {
            db.ConflictCases.Add(conflict);
            operation.ConflictCase = conflict;
            operation.UpdatedAt = now;
            operation.RowVersion = Guid.NewGuid().ToByteArray();
            await db.SaveChangesAsync(cancellationToken);
            await audit.AppendAuditEventAsync(new AuditEventDraft(
                "SyncOperationConflict", "CONFLICT", nameof(SyncOperation), operation.Id,
                security.UserId, operation.CompanyId, operation.BranchId,
                CorrelationId: Guid.NewGuid(), DeviceId: security.DeviceId, Reason: conflict.ConflictReason), cancellationToken);
            return conflict;
        }, cancellationToken);
    }

    private async Task<T> ExecuteMutationAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken)
    {
        if (db.Database.CurrentTransaction is not null)
            return await action();
        await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);
        try
        {
            var result = await action();
            await transaction.CommitAsync(cancellationToken);
            return result;
        }
        catch
        {
            try
            {
                await transaction.RollbackAsync(cancellationToken);
            }
            finally
            {
                // SaveChanges accepts entity states before the database transaction commits. If a
                // later audit write fails, rollback alone therefore leaves non-persisted operations,
                // conflicts and audit rows tracked as Unchanged. This service owns this transaction,
                // so it must reset its unit of work before the scoped context handles another batch
                // item. The ambient-transaction path above deliberately remains caller-owned.
                db.ChangeTracker.Clear();
            }
            throw;
        }
    }

    private async Task<SyncOperation> CompleteClaimAsync(
        Guid operationId,
        Guid claimToken,
        DateTimeOffset completedAt,
        Func<SyncOperation, Task> mutateAndAudit,
        CancellationToken cancellationToken)
    {
        if (claimToken == Guid.Empty)
            throw new SyncRuleException("EXECUTION_CLAIM_INVALID", operationId.ToString());
        if (!db.Database.IsNpgsql())
            throw new SyncRuleException("EXECUTION_STORE_UNSUPPORTED", "PostgreSQL is required");

        await using var transaction = await db.Database.BeginTransactionAsync(
            IsolationLevel.ReadCommitted, cancellationToken);
        try
        {
            var rows = await db.SyncOperations.FromSqlInterpolated($$"""
                SELECT o.*
                FROM transport_erp.sync_operations AS o
                WHERE o."Id" = {{operationId}}
                FOR UPDATE OF o
                """).AsTracking().ToListAsync(cancellationToken);
            var operation = rows.SingleOrDefault()
                ?? throw new SyncRuleException("OPERATION_NOT_FOUND", operationId.ToString());
            if (operation.Status != "SENDING" ||
                operation.ExecutionClaimToken != claimToken ||
                operation.ExecutionLeaseExpiresAt is null ||
                operation.ExecutionLeaseExpiresAt <= completedAt)
                throw new SyncRuleException("EXECUTION_CLAIM_LOST", operationId.ToString());

            operation.UpdatedAt = completedAt;
            operation.RowVersion = Guid.NewGuid().ToByteArray();
            await mutateAndAudit(operation);
            await db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return operation;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            db.ChangeTracker.Clear();
            throw;
        }
    }

    private static SyncOperationExecutionClaim ToExecutionClaim(
        SyncOperation operation,
        Guid claimToken,
        DateTimeOffset claimedAt,
        DateTimeOffset leaseExpiresAt,
        bool recoveredStaleClaim)
        => new(
            operation.Id,
            claimToken,
            claimedAt,
            leaseExpiresAt,
            recoveredStaleClaim,
            operation.CompanyId,
            operation.BranchId!.Value,
            operation.UserId,
            operation.RegisteredDeviceId!.Value,
            operation.RegisteredDeviceCredentialVersion!.Value,
            operation.ProofKeyVersion!.Value,
            operation.DeviceId,
            operation.ProtocolVersion!,
            operation.ActionCode!,
            operation.OperationType,
            operation.EntityType,
            operation.EntityId,
            operation.BaseVersion,
            operation.PayloadJson,
            operation.PayloadHash,
            operation.ClientOperationId,
            operation.OperationCorrelationId!.Value,
            operation.RetryCount);

    private static void ClearExecutionClaim(SyncOperation operation)
    {
        operation.ExecutionClaimToken = null;
        operation.ExecutionAttemptStartedAt = null;
        operation.ExecutionLeaseExpiresAt = null;
    }

    private async Task EnsureSecurityAsync(
        SyncSecurityContext security,
        Guid companyId,
        Guid? branchId,
        CancellationToken cancellationToken,
        Guid? requiredUserId = null,
        string? requiredDeviceId = null)
    {
        if (!security.IsDeviceRegistered) throw new SyncRuleException("DEVICE_NOT_REGISTERED", security.DeviceId);
        if (!security.RegisteredDeviceId.HasValue || !security.RegisteredDeviceCredentialVersion.HasValue ||
            !security.BranchId.HasValue)
            throw new SyncRuleException("DEVICE_NOT_REGISTERED", security.DeviceId);
        if (!security.HasExecutePermission) throw new SyncRuleException("PERMISSION_DENIED", "sync.operations.execute");
        if (requiredDeviceId is not null && security.DeviceId != requiredDeviceId)
            throw new SyncRuleException("SCOPE_DENIED", companyId.ToString());
        if (requiredUserId is not null && security.UserId != requiredUserId)
            throw new SyncRuleException("SCOPE_DENIED", companyId.ToString());
        if (security.CompanyId != companyId || security.BranchId != branchId)
            throw new SyncRuleException("SCOPE_DENIED", companyId.ToString());
        var companyExists = await db.Companies.AnyAsync(x => x.Id == companyId && x.Status == "ACTIVE", cancellationToken);
        if (!companyExists) throw new SyncRuleException("COMPANY_NOT_FOUND", companyId.ToString());
        if (branchId is not null && !await db.Branches.AnyAsync(x => x.Id == branchId && x.CompanyId == companyId && x.Status == "ACTIVE", cancellationToken))
            throw new SyncRuleException("BRANCH_NOT_FOUND", branchId.ToString()!);
        if (!await db.Users.AnyAsync(x => x.Id == security.UserId && x.Status == "ACTIVE" &&
                x.CompanyId == security.CompanyId &&
                (x.BranchId == null || x.BranchId == security.BranchId), cancellationToken))
            throw new SyncRuleException("USER_NOT_FOUND", security.UserId.ToString());
        var now = DateTimeOffset.UtcNow;
        var activeDeviceBinding = await (
            from device in db.RegisteredDevices.AsNoTracking()
            join assignment in db.RegisteredDeviceAssignments.AsNoTracking()
                on device.Id equals assignment.RegisteredDeviceId
            where device.Id == security.RegisteredDeviceId && device.CompanyId == security.CompanyId &&
                  device.DeviceId == security.DeviceId && device.Status == "ACTIVE" &&
                  device.CredentialVersion == security.RegisteredDeviceCredentialVersion &&
                  (device.ExpiresAt == null || device.ExpiresAt > now) &&
                  (device.LastSeenAt ?? device.ApprovedAt ?? device.CreatedAt) >
                      now - TimeSpan.FromDays(90) &&
                  assignment.UserId == security.UserId && assignment.CompanyId == security.CompanyId &&
                  assignment.BranchId == security.BranchId && assignment.Status == "ACTIVE"
            select device.Id).AnyAsync(cancellationToken);
        if (!activeDeviceBinding)
            throw new SyncRuleException("DEVICE_NOT_REGISTERED", security.DeviceId);
    }

    private static void EnsureTenantScope(SyncOperation operation, SyncSecurityContext security)
    {
        if (operation.CompanyId != security.CompanyId || operation.BranchId != security.BranchId)
            throw new SyncRuleException("SCOPE_DENIED", operation.ClientOperationId);
    }

    private static void EnsureSameOwnerScope(SyncOperation operation, SyncSecurityContext security)
    {
        EnsureTenantScope(operation, security);
        if (operation.DeviceId != security.DeviceId || operation.UserId != security.UserId)
            throw new SyncRuleException("SCOPE_DENIED", operation.ClientOperationId);
    }

    private static void ValidateCommand(EnqueueSyncOperationCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.DeviceId) || string.IsNullOrWhiteSpace(command.ClientOperationId) ||
            string.IsNullOrWhiteSpace(command.PayloadJson) || string.IsNullOrWhiteSpace(command.PayloadHash) ||
            string.IsNullOrWhiteSpace(command.OperationType) || string.IsNullOrWhiteSpace(command.EntityType))
            throw new SyncRuleException("PAYLOAD_INVALID", command.ClientOperationId);
        if (command.OperationType.Trim().ToUpperInvariant() is not ("CREATE" or "UPDATE" or "DELETE" or "COMMAND"))
            throw new SyncRuleException("OPERATION_TYPE_INVALID", command.OperationType);
    }

    private static void ValidateAcceptedCommand(
        EnqueueAcceptedSyncOperationCommand command,
        AcceptedSyncProofContext acceptedProof)
    {
        if (acceptedProof.ReplayId == Guid.Empty || acceptedProof.UserId == Guid.Empty ||
            acceptedProof.CompanyId == Guid.Empty || acceptedProof.BranchId == Guid.Empty ||
            acceptedProof.RegisteredDeviceId == Guid.Empty || acceptedProof.AttemptCorrelationId == Guid.Empty ||
            acceptedProof.DeviceCredentialVersion < 1 || acceptedProof.ProofKeyVersion < 1 ||
            acceptedProof.ProofKeyThumbprint.Length != 43 ||
            string.IsNullOrEmpty(acceptedProof.DeviceId))
            throw new SyncRuleException("PROOF_CONTEXT_INVALID", command.ClientOperationId);
        if (!string.Equals(command.ProtocolVersion, "sync-v1", StringComparison.Ordinal) ||
            string.IsNullOrEmpty(command.ActionCode) || string.IsNullOrEmpty(command.EntityType) ||
            string.IsNullOrEmpty(command.ClientOperationId) || string.IsNullOrEmpty(command.PayloadJson) ||
            command.OperationCorrelationId == Guid.Empty || string.IsNullOrEmpty(command.PayloadHash) ||
            command.PayloadHash.Length != 64 ||
            command.OperationType is not ("CREATE" or "UPDATE" or "DELETE" or "COMMAND"))
            throw new SyncRuleException("PAYLOAD_INVALID", command.ClientOperationId);

        var hasPolicyVersion = command.EffectivePolicySourceVersion is not null;
        var hasPolicyFingerprint = command.EffectivePolicySourceFingerprint is not null;
        if (hasPolicyVersion != hasPolicyFingerprint ||
            (hasPolicyVersion && (!IsSafePolicyVersion(command.EffectivePolicySourceVersion!) ||
                !IsSha256Hex(command.EffectivePolicySourceFingerprint!))))
            throw new SyncRuleException("SYNC_RUNTIME_POLICY_UNAVAILABLE", command.ClientOperationId);
    }

    private static string PolicySourceReason(EnqueueAcceptedSyncOperationCommand command) =>
        command.EffectivePolicySourceVersion is { } version &&
        command.EffectivePolicySourceFingerprint is { } fingerprint
            ? $"PolicySourceVersion={version};PolicySourceFingerprint={fingerprint}"
            : "PolicySourceVersion=INTERNAL_REAPPLY;PolicySourceFingerprint=NOT_CAPTURED";

    private static string PolicySourceReason(SyncExecutionPolicyDecision decision) =>
        decision.PolicySourceVersion is { } version && decision.PolicySourceFingerprint is { } fingerprint
            ? $"PolicySourceVersion={version};PolicySourceFingerprint={fingerprint}"
            : "PolicySourceVersion=UNAVAILABLE;PolicySourceFingerprint=UNAVAILABLE";

    private static bool IsSafePolicyVersion(string value) =>
        value.Length is >= 1 and <= 80 &&
        value.All(character => char.IsAsciiLetterOrDigit(character) || character is '-' or '_' or '.');

    private static bool IsSha256Hex(string value) =>
        value.Length == 64 && value.All(character => character is >= '0' and <= '9' or >= 'a' and <= 'f');

    private static void EnsureAcceptedReplayMatches(
        SyncOperation operation,
        byte[] fingerprint,
        AcceptedSyncProofContext acceptedProof)
    {
        if (operation.CompanyId != acceptedProof.CompanyId ||
            operation.RegisteredDeviceId != acceptedProof.RegisteredDeviceId ||
            operation.RequestFingerprintVersion != "fp-v1" || operation.RequestFingerprintHash is null ||
            !CryptographicOperations.FixedTimeEquals(operation.RequestFingerprintHash, fingerprint))
            throw new SyncRuleException("IDEMPOTENCY_CONFLICT", operation.ClientOperationId);
    }

    private static string CanonicalTimestamp(DateTimeOffset value)
    {
        var utc = NormalizePostgreSqlTimestamp(value);
        return utc.ToString("yyyy-MM-dd'T'HH:mm:ss.FFFFFF'Z'", System.Globalization.CultureInfo.InvariantCulture);
    }

    private static bool PayloadHashMatches(string payload, string expectedHash)
        => string.Equals(Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(payload))).ToLowerInvariant(),
            expectedHash.Trim().ToLowerInvariant(), StringComparison.Ordinal);

    private static bool IsRetryableErrorCode(string? errorCode)
        => string.IsNullOrWhiteSpace(errorCode) ||
           string.Equals(errorCode.Trim(), "RATE_LIMITED", StringComparison.OrdinalIgnoreCase);

    private async ValueTask<SyncRetryPolicy?> ResolveRetryPolicyAsync(
        SyncOperation operation,
        CancellationToken cancellationToken)
    {
        var policy = await _retryPolicies.ResolveAsync(
            operation.CompanyId, operation.BranchId, operation.RegisteredDeviceId,
            operation.DeviceId, cancellationToken);
        return policy?.Validate();
    }

    private static TimeSpan CalculateBackoff(SyncRetryPolicy retryPolicy, int retryNumber)
    {
        var multiplier = Math.Pow(2, retryNumber - 1);
        var milliseconds = Math.Min(retryPolicy.MaxDelay.TotalMilliseconds,
            retryPolicy.BaseDelay.TotalMilliseconds * multiplier);
        return TimeSpan.FromMilliseconds(milliseconds);
    }

    private static bool IsAllowedTransition(string from, string to) => (from, to) switch
    {
        ("QUEUED", "SENDING") => true,
        ("QUEUED", "REJECTED") => true,
        ("SENDING", "SUCCEEDED") => true,
        ("SENDING", "FAILED") => true,
        ("SENDING", "CONFLICT") => true,
        ("SENDING", "REJECTED") => true,
        ("FAILED", "SENDING") => true,
        ("CONFLICT", "RESOLVED") => true,
        ("CONFLICT", "REJECTED") => true,
        _ => false
    };

    private static DateTimeOffset NormalizePostgreSqlTimestamp(DateTimeOffset value)
    {
        var ticks = value.UtcDateTime.Ticks;
        return new DateTimeOffset(new DateTime(ticks - ticks % TimeSpan.TicksPerMicrosecond, DateTimeKind.Utc));
    }
}
