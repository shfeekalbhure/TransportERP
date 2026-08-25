using System.Security.Cryptography;
using System.Text;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace TransportERP.Infrastructure.Persistence;

public sealed record SyncSecurityContext(
    Guid UserId,
    string DeviceId,
    Guid CompanyId,
    Guid? BranchId,
    bool IsDeviceRegistered,
    bool HasExecutePermission);

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
        if (!await db.Users.AnyAsync(x => x.Id == security.UserId && x.Status == "ACTIVE", cancellationToken))
            throw new SyncRuleException("USER_NOT_FOUND", security.UserId.ToString());
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
