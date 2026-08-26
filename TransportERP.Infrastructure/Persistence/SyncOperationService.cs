using System.Security.Cryptography;
using System.Text;
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
        if (MaxRetryCount < 1) throw new ArgumentOutOfRangeException(nameof(MaxRetryCount));
        if (BaseDelay <= TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(BaseDelay));
        if (MaxDelay < BaseDelay) throw new ArgumentOutOfRangeException(nameof(MaxDelay));
        return this;
    }
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
    long? BaseVersion = null);

public sealed record TransitionSyncOperationCommand(Guid OperationId, string NewStatus, string? ErrorCode = null);

public sealed record ConflictCaseDraft(
    string DeviceSnapshot,
    string ServerSnapshot,
    string ConflictReason,
    long? BaseVersion = null);

public sealed record ResolveSyncConflictCommand(
    string Resolution,
    Guid? ReplacedByOperationId = null);

public sealed class SyncRuleException(string code, string detail) : InvalidOperationException($"{code}: {detail}")
{
    public string Code { get; } = code;
}

public sealed class SyncOperationService(
    TransportErpDbContext db,
    AuditEventService audit,
    SyncRetryPolicy retryPolicy)
{
    private readonly SyncRetryPolicy _retryPolicy = retryPolicy.Validate();

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

        await using var transaction = await db.Database.BeginTransactionAsync(
            IsolationLevel.ReadCommitted, cancellationToken);
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
                await transaction.CommitAsync(cancellationToken);
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
                OperationCorrelationId: operation.OperationCorrelationId), cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return operation;
        }
        catch (DbUpdateException exception) when (
            exception.GetBaseException() is PostgresException
            {
                SqlState: PostgresErrorCodes.UniqueViolation,
                ConstraintName: "ux_sync_op_registered_device_client"
            })
        {
            await transaction.RollbackAsync(cancellationToken);
            db.ChangeTracker.Clear();
            var existing = await db.SyncOperations.Include(x => x.ConflictCase).SingleAsync(x =>
                x.CompanyId == acceptedProof.CompanyId &&
                x.RegisteredDeviceId == acceptedProof.RegisteredDeviceId &&
                x.ClientOperationId == command.ClientOperationId &&
                x.RequestFingerprintVersion == "fp-v1", cancellationToken);
            EnsureAcceptedReplayMatches(existing, fingerprint, acceptedProof);
            return existing;
        }
        catch (DbUpdateException exception) when (
            exception.GetBaseException() is PostgresException
            {
                SqlState: PostgresErrorCodes.UniqueViolation,
                ConstraintName: "ux_sync_op_legacy_company_device_client"
            })
        {
            await transaction.RollbackAsync(cancellationToken);
            db.ChangeTracker.Clear();
            throw new SyncRuleException("LEGACY_IDEMPOTENCY_CONFLICT", command.ClientOperationId);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            db.ChangeTracker.Clear();
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

        operation.Status = newStatus;
        if (newStatus == "FAILED")
        {
            if (string.IsNullOrWhiteSpace(command.ErrorCode))
                throw new SyncRuleException("ERROR_CODE_REQUIRED", operation.ClientOperationId);
            operation.ErrorCode = command.ErrorCode.Trim().ToUpperInvariant();
            if (!IsRetryableErrorCode(operation.ErrorCode))
                operation.NextRetryAt = null;
        }
        operation.UpdatedAt = NormalizePostgreSqlTimestamp(DateTimeOffset.UtcNow);
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
                CorrelationId: Guid.NewGuid(), DeviceId: security.DeviceId, Reason: newStatus), cancellationToken);
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

        if (operation.RetryCount >= _retryPolicy.MaxRetryCount)
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

        var retryNumber = operation.RetryCount + 1;
        var delay = CalculateBackoff(retryNumber);
        operation.RetryCount = retryNumber;
        operation.NextRetryAt = NormalizePostgreSqlTimestamp(DateTimeOffset.UtcNow.Add(delay));
        operation.ErrorCode = null;
        operation.UpdatedAt = NormalizePostgreSqlTimestamp(DateTimeOffset.UtcNow);
        operation.RowVersion = Guid.NewGuid().ToByteArray();
        return await ExecuteMutationAsync(async () =>
        {
            await db.SaveChangesAsync(cancellationToken);
            await audit.AppendAuditEventAsync(new AuditEventDraft(
                "SyncOperationRetry", "SUCCESS", nameof(SyncOperation), operation.Id,
                security.UserId, operation.CompanyId, operation.BranchId,
                CorrelationId: Guid.NewGuid(), DeviceId: security.DeviceId, Reason: $"RetryCount={retryNumber}"), cancellationToken);
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
        var dueAt = now ?? DateTimeOffset.UtcNow;
        var query = db.SyncOperations.AsNoTracking()
            .Where(x => x.CompanyId == security.CompanyId && x.Status == "FAILED" &&
                        x.NextRetryAt != null && x.NextRetryAt <= dueAt &&
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

    public async Task<ConflictCase> ResolveSyncConflictAsync(
        Guid conflictCaseId,
        ResolveSyncConflictCommand command,
        SyncSecurityContext security,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Resolution))
            throw new SyncRuleException("RESOLUTION_REQUIRED", conflictCaseId.ToString());
        var conflict = await db.ConflictCases.Include(x => x.SyncOperation)
            .SingleOrDefaultAsync(x => x.Id == conflictCaseId, cancellationToken)
            ?? throw new SyncRuleException("CONFLICT_NOT_FOUND", conflictCaseId.ToString());
        var operation = conflict.SyncOperation ?? throw new SyncRuleException("OPERATION_NOT_FOUND", conflict.SyncOperationId.ToString());
        EnsureTenantScope(operation, security);
        await EnsureSecurityAsync(security, operation.CompanyId, operation.BranchId, cancellationToken);
        if (conflict.Status != "OPEN" || operation.Status != "CONFLICT")
            throw new SyncRuleException("CONFLICT_ALREADY_RESOLVED", conflictCaseId.ToString());

        if (command.ReplacedByOperationId is not null)
        {
            var replacement = await db.SyncOperations.AsNoTracking().SingleOrDefaultAsync(
                x => x.Id == command.ReplacedByOperationId.Value, cancellationToken)
                ?? throw new SyncRuleException("REPLACEMENT_NOT_FOUND", command.ReplacedByOperationId.Value.ToString());
            EnsureTenantScope(replacement, security);
        }

        var now = NormalizePostgreSqlTimestamp(DateTimeOffset.UtcNow);
        conflict.Status = "RESOLVED";
        conflict.Resolution = command.Resolution.Trim();
        conflict.ResolvedBy = security.UserId.ToString();
        conflict.ResolvedAt = now;
        conflict.ReplacedByOperationId = command.ReplacedByOperationId;
        conflict.UpdatedAt = now;
        conflict.RowVersion = Guid.NewGuid().ToByteArray();
        operation.Status = "RESOLVED";
        operation.ErrorCode = null;
        operation.UpdatedAt = now;
        operation.RowVersion = Guid.NewGuid().ToByteArray();
        return await ExecuteMutationAsync(async () =>
        {
            await db.SaveChangesAsync(cancellationToken);
            await audit.AppendAuditEventAsync(new AuditEventDraft(
                "SyncOperationConflictResolved", "SUCCESS", nameof(SyncOperation), operation.Id,
                security.UserId, operation.CompanyId, operation.BranchId,
                CorrelationId: Guid.NewGuid(), DeviceId: security.DeviceId, Reason: conflict.Resolution), cancellationToken);
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
    }

    private static void EnsureAcceptedReplayMatches(
        SyncOperation operation,
        byte[] fingerprint,
        AcceptedSyncProofContext acceptedProof)
    {
        if (operation.CompanyId != acceptedProof.CompanyId ||
            operation.RegisteredDeviceId != acceptedProof.RegisteredDeviceId ||
            operation.RequestFingerprintVersion != "fp-v1" || operation.RequestFingerprintHash is null ||
            !CryptographicOperations.FixedTimeEquals(operation.RequestFingerprintHash, fingerprint))
            throw new SyncRuleException("IDEMPOTENCY_MISMATCH", operation.ClientOperationId);
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

    private TimeSpan CalculateBackoff(int retryNumber)
    {
        var multiplier = Math.Pow(2, retryNumber - 1);
        var milliseconds = Math.Min(_retryPolicy.MaxDelay.TotalMilliseconds,
            _retryPolicy.BaseDelay.TotalMilliseconds * multiplier);
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
