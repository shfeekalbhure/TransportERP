using System.Data;
using Microsoft.Data.Sqlite;

namespace TransportERP.Offline;

public sealed class OfflineOperationStore
{
    private readonly EncryptedSqliteConnectionFactory _connections;
    private readonly TimeProvider _timeProvider;
    private readonly OfflineRetryPolicy _retryPolicy;

    public OfflineOperationStore(
        string databasePath,
        ILocalEncryptionKeyProvider keyProvider,
        TimeProvider? timeProvider = null,
        OfflineRetryPolicy? retryPolicy = null)
    {
        _connections = new EncryptedSqliteConnectionFactory(databasePath, keyProvider, LocalStorePurpose.WriteOutbox);
        _timeProvider = timeProvider ?? TimeProvider.System;
        _retryPolicy = retryPolicy ?? new OfflineRetryPolicy();
        _retryPolicy.Validate();
    }

    public string DatabasePath => _connections.DatabasePath;

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await using var connection = await _connections.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = """
            PRAGMA journal_mode = WAL;
            PRAGMA synchronous = FULL;
            CREATE TABLE IF NOT EXISTS offline_operations (
                LocalOperationId TEXT PRIMARY KEY NOT NULL,
                LocalIntentId TEXT NOT NULL,
                CompanyId TEXT NOT NULL,
                BranchId TEXT NOT NULL,
                UserId TEXT NOT NULL,
                RegisteredDeviceId TEXT NOT NULL,
                ClientOperationId TEXT NOT NULL,
                OperationCorrelationId TEXT NOT NULL,
                AttemptCorrelationId TEXT NULL,
                ProtocolVersion TEXT NOT NULL CHECK (ProtocolVersion = 'sync-v1'),
                ActionCode TEXT NOT NULL,
                OperationType TEXT NOT NULL,
                EntityType TEXT NOT NULL,
                EntityId TEXT NULL,
                BaseVersion INTEGER NULL,
                ClientOccurredAt TEXT NOT NULL,
                PayloadJson TEXT NULL,
                PayloadHash TEXT NOT NULL,
                RequestFingerprint TEXT NOT NULL,
                Status TEXT NOT NULL CHECK (Status IN ('QUEUED','SENDING','SUCCEEDED','FAILED','CONFLICT','REJECTED','RESOLVED')),
                ClientTransportRetryCount INTEGER NOT NULL DEFAULT 0 CHECK (ClientTransportRetryCount >= 0),
                NextRetryAt TEXT NULL,
                LeaseOwner TEXT NULL,
                LeaseExpiresAt TEXT NULL,
                ResultCode TEXT NULL,
                ConflictCaseId TEXT NULL,
                ServerOperationId TEXT NULL,
                ResultEntityId TEXT NULL,
                ResultVersion INTEGER NULL,
                CreatedAt TEXT NOT NULL,
                UpdatedAt TEXT NOT NULL,
                AcknowledgedAt TEXT NULL,
                RedactedAt TEXT NULL,
                UNIQUE (CompanyId, RegisteredDeviceId, LocalIntentId),
                UNIQUE (CompanyId, RegisteredDeviceId, ClientOperationId),
                UNIQUE (OperationCorrelationId)
            );
            CREATE INDEX IF NOT EXISTS ix_offline_operations_claim
                ON offline_operations (Status, NextRetryAt, LeaseExpiresAt, CreatedAt);
            """;
        await command.ExecuteNonQueryAsync(cancellationToken);

        // Existing encrypted stores predate conflict resolution. Upgrade them in place without
        // recreating or decrypting into a plaintext staging database.
        await EnsureColumnAsync(connection, "offline_operations", "ConflictCaseId", "TEXT NULL", cancellationToken);
        await EnsureColumnAsync(connection, "offline_operations", "ServerOperationId", "TEXT NULL", cancellationToken);
    }

    public async Task<OfflineEnqueueResult> EnqueueAsync(
        OfflineOperationEnqueueRequest request,
        CancellationToken cancellationToken = default)
    {
        var (payloadHash, fingerprint) = OfflineOperationIntegrity.ValidateAndHash(request);
        var now = _timeProvider.GetUtcNow();
        await using var connection = await _connections.OpenAsync(cancellationToken);
        await using var transaction = connection.BeginTransaction(deferred: false);

        var existing = await FindByIntentAsync(connection, transaction, request.CompanyId, request.RegisteredDeviceId, request.LocalIntentId, cancellationToken);
        if (existing is not null)
        {
            if (!string.Equals(existing.RequestFingerprint, fingerprint, StringComparison.Ordinal))
            {
                throw new OfflineStoreException("LOCAL_IDEMPOTENCY_MISMATCH", "The local intent identity is already bound to different operation content.");
            }

            await transaction.CommitAsync(cancellationToken);
            return new OfflineEnqueueResult(existing, false);
        }

        await using (var stale = connection.CreateCommand())
        {
            stale.Transaction = transaction;
            stale.CommandText = """
                SELECT EXISTS (
                    SELECT 1 FROM offline_operations
                    WHERE CompanyId = $companyId AND BranchId = $branchId AND UserId = $userId
                      AND RegisteredDeviceId = $registeredDeviceId
                      AND Status IN ('QUEUED','SENDING','FAILED','CONFLICT')
                      AND CreatedAt <= $staleBoundary
                );
                """;
            Add(stale, "$companyId", request.CompanyId);
            Add(stale, "$branchId", request.BranchId);
            Add(stale, "$userId", request.UserId);
            Add(stale, "$registeredDeviceId", request.RegisteredDeviceId);
            stale.Parameters.AddWithValue("$staleBoundary", Format(now - TimeSpan.FromDays(7)));
            if (Convert.ToInt64(await stale.ExecuteScalarAsync(cancellationToken),
                    System.Globalization.CultureInfo.InvariantCulture) != 0)
                throw new OfflineStoreException(
                    "OFFLINE_QUEUE_ESCALATION_REQUIRED",
                    "A non-terminal operation has reached seven days; synchronize or escalate before creating new offline writes.");
        }

        var localOperationId = Guid.NewGuid();
        var clientOperationId = Guid.NewGuid().ToString("D");
        var operationCorrelationId = Guid.NewGuid();
        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = """
            INSERT INTO offline_operations (
                LocalOperationId, LocalIntentId, CompanyId, BranchId, UserId, RegisteredDeviceId,
                ClientOperationId, OperationCorrelationId, ProtocolVersion, ActionCode, OperationType,
                EntityType, EntityId, BaseVersion, ClientOccurredAt, PayloadJson, PayloadHash,
                RequestFingerprint, Status, CreatedAt, UpdatedAt)
            VALUES (
                $localOperationId, $localIntentId, $companyId, $branchId, $userId, $registeredDeviceId,
                $clientOperationId, $operationCorrelationId, 'sync-v1', $actionCode, $operationType,
                $entityType, $entityId, $baseVersion, $clientOccurredAt, $payloadJson, $payloadHash,
                $fingerprint, 'QUEUED', $now, $now);
            """;
        Add(command, "$localOperationId", localOperationId);
        Add(command, "$localIntentId", request.LocalIntentId);
        Add(command, "$companyId", request.CompanyId);
        Add(command, "$branchId", request.BranchId);
        Add(command, "$userId", request.UserId);
        Add(command, "$registeredDeviceId", request.RegisteredDeviceId);
        command.Parameters.AddWithValue("$clientOperationId", clientOperationId);
        Add(command, "$operationCorrelationId", operationCorrelationId);
        command.Parameters.AddWithValue("$actionCode", request.ActionCode);
        command.Parameters.AddWithValue("$operationType", request.OperationType);
        command.Parameters.AddWithValue("$entityType", request.EntityType);
        AddNullable(command, "$entityId", request.EntityId);
        AddNullable(command, "$baseVersion", request.BaseVersion);
        command.Parameters.AddWithValue("$clientOccurredAt", Format(request.ClientOccurredAt));
        command.Parameters.AddWithValue("$payloadJson", request.PayloadJson);
        command.Parameters.AddWithValue("$payloadHash", payloadHash);
        command.Parameters.AddWithValue("$fingerprint", fingerprint);
        command.Parameters.AddWithValue("$now", Format(now));
        await command.ExecuteNonQueryAsync(cancellationToken);

        var operation = await GetRequiredAsync(connection, transaction, localOperationId, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return new OfflineEnqueueResult(operation, true);
    }

    public async Task<OfflineEnqueueResult> EnqueueAsync(
        OfflineOperationEnqueueTemplate template,
        Func<OfflineGeneratedOperationIdentity, string> payloadFactory,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(template);
        ArgumentNullException.ThrowIfNull(payloadFactory);
        var now = _timeProvider.GetUtcNow();
        await using var connection = await _connections.OpenAsync(cancellationToken);
        await using var transaction = connection.BeginTransaction(deferred: false);
        var existing = await FindByIntentAsync(connection, transaction, template.CompanyId,
            template.RegisteredDeviceId, template.LocalIntentId, cancellationToken);
        if (existing is not null)
        {
            EnsureTemplateMatches(existing, template);
            await transaction.CommitAsync(cancellationToken);
            return new OfflineEnqueueResult(existing, false);
        }

        await EnsureNoStaleQueueAsync(connection, transaction, template.CompanyId, template.BranchId,
            template.UserId, template.RegisteredDeviceId, now, cancellationToken);
        var identity = new OfflineGeneratedOperationIdentity(Guid.NewGuid().ToString("D"), Guid.NewGuid());
        var payloadJson = payloadFactory(identity);
        OfflineOperationIntegrity.ValidatePayloadIdentity(payloadJson, identity.ClientOperationId);
        var request = new OfflineOperationEnqueueRequest(
            template.LocalIntentId, template.CompanyId, template.BranchId, template.UserId,
            template.RegisteredDeviceId, template.ActionCode, template.OperationType, template.EntityType,
            template.EntityId, template.BaseVersion, template.ClientOccurredAt, payloadJson);
        var (payloadHash, fingerprint) = OfflineOperationIntegrity.ValidateAndHash(request);

        var localOperationId = Guid.NewGuid();
        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = """
            INSERT INTO offline_operations (
                LocalOperationId, LocalIntentId, CompanyId, BranchId, UserId, RegisteredDeviceId,
                ClientOperationId, OperationCorrelationId, ProtocolVersion, ActionCode, OperationType,
                EntityType, EntityId, BaseVersion, ClientOccurredAt, PayloadJson, PayloadHash,
                RequestFingerprint, Status, CreatedAt, UpdatedAt)
            VALUES (
                $localOperationId, $localIntentId, $companyId, $branchId, $userId, $registeredDeviceId,
                $clientOperationId, $operationCorrelationId, 'sync-v1', $actionCode, $operationType,
                $entityType, $entityId, $baseVersion, $clientOccurredAt, $payloadJson, $payloadHash,
                $fingerprint, 'QUEUED', $now, $now);
            """;
        Add(command, "$localOperationId", localOperationId);
        Add(command, "$localIntentId", template.LocalIntentId);
        Add(command, "$companyId", template.CompanyId);
        Add(command, "$branchId", template.BranchId);
        Add(command, "$userId", template.UserId);
        Add(command, "$registeredDeviceId", template.RegisteredDeviceId);
        command.Parameters.AddWithValue("$clientOperationId", identity.ClientOperationId);
        Add(command, "$operationCorrelationId", identity.OperationCorrelationId);
        command.Parameters.AddWithValue("$actionCode", template.ActionCode);
        command.Parameters.AddWithValue("$operationType", template.OperationType);
        command.Parameters.AddWithValue("$entityType", template.EntityType);
        AddNullable(command, "$entityId", template.EntityId);
        AddNullable(command, "$baseVersion", template.BaseVersion);
        command.Parameters.AddWithValue("$clientOccurredAt", Format(template.ClientOccurredAt));
        command.Parameters.AddWithValue("$payloadJson", payloadJson);
        command.Parameters.AddWithValue("$payloadHash", payloadHash);
        command.Parameters.AddWithValue("$fingerprint", fingerprint);
        command.Parameters.AddWithValue("$now", Format(now));
        await command.ExecuteNonQueryAsync(cancellationToken);
        var operation = await GetRequiredAsync(connection, transaction, localOperationId, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return new OfflineEnqueueResult(operation, true);
    }

    public async Task<OfflineOperation?> ClaimNextAsync(
        string workerId,
        TimeSpan leaseDuration,
        OfflineOperationScope scope,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(workerId) || leaseDuration <= TimeSpan.Zero)
        {
            throw new ArgumentException("A worker identity and positive lease duration are required.");
        }
        ArgumentNullException.ThrowIfNull(scope);
        scope.Validate();

        var now = _timeProvider.GetUtcNow();
        var attemptId = Guid.NewGuid();
        await using var connection = await _connections.OpenAsync(cancellationToken);
        await using var transaction = connection.BeginTransaction(deferred: false);
        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = """
            UPDATE offline_operations
            SET Status = 'SENDING',
                AttemptCorrelationId = $attemptId,
                LeaseOwner = $workerId,
                LeaseExpiresAt = $leaseExpiresAt,
                UpdatedAt = $now
            WHERE LocalOperationId = (
                SELECT LocalOperationId
                FROM offline_operations
                WHERE CompanyId = $companyId AND BranchId = $branchId AND UserId = $userId
                  AND RegisteredDeviceId = $registeredDeviceId
                  AND ((Status = 'QUEUED' AND (NextRetryAt IS NULL OR NextRetryAt <= $now))
                    OR (Status = 'FAILED' AND NextRetryAt IS NOT NULL AND NextRetryAt <= $now)
                    OR (Status = 'SENDING' AND LeaseExpiresAt IS NOT NULL AND LeaseExpiresAt <= $now))
                ORDER BY CreatedAt, LocalOperationId
                LIMIT 1
            )
            RETURNING *;
            """;
        Add(command, "$attemptId", attemptId);
        command.Parameters.AddWithValue("$workerId", workerId);
        command.Parameters.AddWithValue("$leaseExpiresAt", Format(now + leaseDuration));
        command.Parameters.AddWithValue("$now", Format(now));
        Add(command, "$companyId", scope.CompanyId);
        Add(command, "$branchId", scope.BranchId);
        Add(command, "$userId", scope.UserId);
        Add(command, "$registeredDeviceId", scope.RegisteredDeviceId);
        var operation = await ReadSingleAsync(command, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return operation;
    }

    public Task MarkSucceededAsync(Guid localOperationId, Guid attemptCorrelationId, Guid? resultEntityId, long? resultVersion, CancellationToken cancellationToken = default) =>
        MarkSucceededAsync(localOperationId, attemptCorrelationId, resultEntityId, resultVersion, null, cancellationToken);

    public Task MarkSucceededAsync(Guid localOperationId, Guid attemptCorrelationId, Guid? resultEntityId,
        long? resultVersion, Guid? serverOperationId, CancellationToken cancellationToken = default) =>
        CompleteAttemptAsync(localOperationId, attemptCorrelationId, OfflineOperationStatus.Succeeded,
            "SUCCEEDED", null, serverOperationId, resultEntityId, resultVersion, cancellationToken);

    public Task MarkConflictAsync(Guid localOperationId, Guid attemptCorrelationId, Guid conflictCaseId,
        string resultCode, CancellationToken cancellationToken = default)
        => MarkConflictAsync(localOperationId, attemptCorrelationId, conflictCaseId, resultCode, null, cancellationToken);

    public Task MarkConflictAsync(Guid localOperationId, Guid attemptCorrelationId, Guid conflictCaseId,
        string resultCode, Guid? serverOperationId, CancellationToken cancellationToken = default)
    {
        if (conflictCaseId == Guid.Empty)
            throw new ArgumentException("A server conflict identity is required.", nameof(conflictCaseId));
        return CompleteAttemptAsync(localOperationId, attemptCorrelationId, OfflineOperationStatus.Conflict,
            resultCode, conflictCaseId, serverOperationId, null, null, cancellationToken);
    }

    public Task MarkRejectedAsync(Guid localOperationId, Guid attemptCorrelationId, string resultCode, CancellationToken cancellationToken = default) =>
        CompleteAttemptAsync(localOperationId, attemptCorrelationId, OfflineOperationStatus.Rejected,
            resultCode, null, null, null, null, cancellationToken);

    /// <summary>
    /// Records durable server acceptance without claiming business success. The operation remains
    /// queued for an idempotent status replay with fresh attempt/PoP identities, and this polling
    /// transition does not consume the client transport retry budget.
    /// </summary>
    public async Task MarkAcceptedPendingAsync(
        Guid localOperationId,
        Guid attemptCorrelationId,
        Guid serverOperationId,
        string serverStatus,
        TimeSpan pollInterval,
        CancellationToken cancellationToken = default)
    {
        if (serverOperationId == Guid.Empty)
            throw new ArgumentException("A server operation identity is required.", nameof(serverOperationId));
        if (serverStatus is not ("QUEUED" or "SENDING"))
            throw new ArgumentException("Only a pending server status can be recorded.", nameof(serverStatus));
        if (pollInterval <= TimeSpan.Zero || pollInterval > TimeSpan.FromHours(1))
            throw new ArgumentOutOfRangeException(nameof(pollInterval));

        var now = _timeProvider.GetUtcNow();
        await using var connection = await _connections.OpenAsync(cancellationToken);
        await using var transaction = connection.BeginTransaction(deferred: false);
        var current = await GetRequiredAsync(connection, transaction, localOperationId, cancellationToken);
        EnsureAttempt(current, attemptCorrelationId);
        if (current.ServerOperationId is { } existing && existing != serverOperationId)
            throw new OfflineStoreException("SERVER_OPERATION_MISMATCH", "The server operation identity changed across an idempotent replay.");

        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = """
            UPDATE offline_operations
            SET Status = 'QUEUED', ServerOperationId = $serverOperationId, ResultCode = $serverStatus,
                NextRetryAt = $nextPollAt, LeaseOwner = NULL, LeaseExpiresAt = NULL, UpdatedAt = $now
            WHERE LocalOperationId = $id AND Status = 'SENDING' AND AttemptCorrelationId = $attemptId;
            """;
        Add(command, "$serverOperationId", serverOperationId);
        command.Parameters.AddWithValue("$serverStatus", serverStatus);
        command.Parameters.AddWithValue("$nextPollAt", Format(now + pollInterval));
        command.Parameters.AddWithValue("$now", Format(now));
        Add(command, "$id", localOperationId);
        Add(command, "$attemptId", attemptCorrelationId);
        if (await command.ExecuteNonQueryAsync(cancellationToken) != 1)
            throw new OfflineStoreException("LOCAL_ATTEMPT_STALE", "The claimed attempt is no longer current.");
        await transaction.CommitAsync(cancellationToken);
    }

    public async Task<DateTimeOffset?> GetNextWorkAtAsync(
        OfflineOperationScope scope,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(scope);
        scope.Validate();
        var now = _timeProvider.GetUtcNow();
        await using var connection = await _connections.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT DueAt FROM (
                SELECT CASE
                    WHEN Status = 'QUEUED' THEN COALESCE(NextRetryAt, $now)
                    WHEN Status = 'FAILED' THEN NextRetryAt
                    WHEN Status = 'SENDING' THEN LeaseExpiresAt
                END AS DueAt
                FROM offline_operations
                WHERE Status IN ('QUEUED','FAILED','SENDING')
                  AND CompanyId = $companyId AND BranchId = $branchId AND UserId = $userId
                  AND RegisteredDeviceId = $registeredDeviceId
            ) AS pending
            WHERE DueAt IS NOT NULL
            ORDER BY DueAt
            LIMIT 1;
            """;
        command.Parameters.AddWithValue("$now", Format(now));
        Add(command, "$companyId", scope.CompanyId);
        Add(command, "$branchId", scope.BranchId);
        Add(command, "$userId", scope.UserId);
        Add(command, "$registeredDeviceId", scope.RegisteredDeviceId);
        var value = await command.ExecuteScalarAsync(cancellationToken) as string;
        return value is null ? null : DateTimeOffset.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
    }

    public async Task MarkResolvedAsync(Guid localOperationId, string resultCode, CancellationToken cancellationToken = default)
    {
        var now = _timeProvider.GetUtcNow();
        await using var connection = await _connections.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = """
            UPDATE offline_operations
            SET Status = 'RESOLVED', ResultCode = $resultCode, AcknowledgedAt = $now, UpdatedAt = $now,
                LeaseOwner = NULL, LeaseExpiresAt = NULL, NextRetryAt = NULL
            WHERE LocalOperationId = $id AND Status = 'CONFLICT';
            """;
        Add(command, "$id", localOperationId);
        command.Parameters.AddWithValue("$resultCode", RequireResultCode(resultCode));
        command.Parameters.AddWithValue("$now", Format(now));
        if (await command.ExecuteNonQueryAsync(cancellationToken) != 1)
        {
            throw new OfflineStoreException("LOCAL_STATE_CONFLICT", "Only a conflict can be resolved.");
        }
    }

    public async Task<OfflineTransportFailureDisposition> MarkTransportFailureAsync(
        Guid localOperationId,
        Guid attemptCorrelationId,
        bool retryable,
        string resultCode,
        CancellationToken cancellationToken = default)
    {
        if (!retryable)
        {
            await CompleteAttemptAsync(localOperationId, attemptCorrelationId, OfflineOperationStatus.Rejected,
                resultCode, null, null, null, null, cancellationToken);
            return OfflineTransportFailureDisposition.Rejected;
        }

        var now = _timeProvider.GetUtcNow();
        await using var connection = await _connections.OpenAsync(cancellationToken);
        await using var transaction = connection.BeginTransaction(deferred: false);
        var current = await GetRequiredAsync(connection, transaction, localOperationId, cancellationToken);
        EnsureAttempt(current, attemptCorrelationId);

        var exhausted = current.ClientTransportRetryCount >= _retryPolicy.MaxRetryCount;
        var retryCount = exhausted ? current.ClientTransportRetryCount : current.ClientTransportRetryCount + 1;
        DateTimeOffset? nextRetryAt = exhausted ? null : now + _retryPolicy.DelayForRetry(retryCount);
        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = """
            UPDATE offline_operations
            SET Status = $status, ClientTransportRetryCount = $retryCount, NextRetryAt = $nextRetryAt,
                ResultCode = $resultCode, LeaseOwner = NULL, LeaseExpiresAt = NULL, UpdatedAt = $now,
                AcknowledgedAt = $acknowledgedAt
            WHERE LocalOperationId = $id AND Status = 'SENDING' AND AttemptCorrelationId = $attemptId;
            """;
        command.Parameters.AddWithValue("$status", exhausted ? "REJECTED" : "FAILED");
        command.Parameters.AddWithValue("$retryCount", retryCount);
        AddNullable(command, "$nextRetryAt", nextRetryAt is null ? null : Format(nextRetryAt.Value));
        command.Parameters.AddWithValue("$resultCode", exhausted ? "RETRY_EXHAUSTED" : RequireResultCode(resultCode));
        command.Parameters.AddWithValue("$now", Format(now));
        AddNullable(command, "$acknowledgedAt", exhausted ? Format(now) : null);
        Add(command, "$id", localOperationId);
        Add(command, "$attemptId", attemptCorrelationId);
        await command.ExecuteNonQueryAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return exhausted
            ? OfflineTransportFailureDisposition.Rejected
            : OfflineTransportFailureDisposition.RetryScheduled;
    }

    public async Task<int> RedactExpiredPayloadsAsync(
        OfflineRetentionPolicy? policy = null,
        CancellationToken cancellationToken = default)
    {
        policy ??= new OfflineRetentionPolicy();
        var now = _timeProvider.GetUtcNow();
        var succeededBoundary = now - policy.EffectiveSucceededOrResolved;
        var rejectedBoundary = now - policy.EffectiveRejected;
        await using var connection = await _connections.OpenAsync(cancellationToken);
        await using var transaction = connection.BeginTransaction(deferred: false);
        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = """
            UPDATE offline_operations
            SET PayloadJson = NULL, RedactedAt = $now, UpdatedAt = $now
            WHERE PayloadJson IS NOT NULL
              AND AcknowledgedAt IS NOT NULL
              AND ((Status IN ('SUCCEEDED','RESOLVED') AND AcknowledgedAt <= $succeededBoundary)
                OR (Status = 'REJECTED' AND AcknowledgedAt <= $rejectedBoundary));
            """;
        command.Parameters.AddWithValue("$now", Format(now));
        command.Parameters.AddWithValue("$succeededBoundary", Format(succeededBoundary));
        command.Parameters.AddWithValue("$rejectedBoundary", Format(rejectedBoundary));
        var count = await command.ExecuteNonQueryAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        await using var checkpoint = connection.CreateCommand();
        checkpoint.CommandText = "PRAGMA wal_checkpoint(TRUNCATE);";
        await checkpoint.ExecuteNonQueryAsync(cancellationToken);
        return count;
    }

    [Obsolete("An authenticated local scope is required.")]
    public Task<OfflineOperation?> GetAsync(Guid localOperationId, CancellationToken cancellationToken = default) =>
        Task.FromException<OfflineOperation?>(new OfflineStoreException(
            "LOCAL_SCOPE_REQUIRED", "An authenticated local scope is required to read an offline operation."));

    public async Task<OfflineOperation?> GetAsync(
        Guid localOperationId,
        OfflineOperationScope scope,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(scope);
        scope.Validate();
        await using var connection = await _connections.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT * FROM offline_operations
            WHERE LocalOperationId = $id AND CompanyId = $companyId AND BranchId = $branchId
              AND UserId = $userId AND RegisteredDeviceId = $registeredDeviceId;
            """;
        Add(command, "$id", localOperationId);
        Add(command, "$companyId", scope.CompanyId);
        Add(command, "$branchId", scope.BranchId);
        Add(command, "$userId", scope.UserId);
        Add(command, "$registeredDeviceId", scope.RegisteredDeviceId);
        return await ReadSingleAsync(command, cancellationToken);
    }

    [Obsolete("An authenticated local scope is required.")]
    public Task<IReadOnlyList<OfflineOperation>> ListAsync(CancellationToken cancellationToken = default) =>
        Task.FromException<IReadOnlyList<OfflineOperation>>(new OfflineStoreException(
            "LOCAL_SCOPE_REQUIRED", "An authenticated local scope is required to list offline operations."));

    public async Task<IReadOnlyList<OfflineOperation>> ListAsync(
        OfflineOperationScope scope,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(scope);
        scope.Validate();
        await using var connection = await _connections.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT * FROM offline_operations
            WHERE CompanyId = $companyId AND BranchId = $branchId AND UserId = $userId
              AND RegisteredDeviceId = $registeredDeviceId
            ORDER BY UpdatedAt DESC, LocalOperationId;
            """;
        Add(command, "$companyId", scope.CompanyId);
        Add(command, "$branchId", scope.BranchId);
        Add(command, "$userId", scope.UserId);
        Add(command, "$registeredDeviceId", scope.RegisteredDeviceId);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var operations = new List<OfflineOperation>();
        while (await reader.ReadAsync(cancellationToken)) operations.Add(Read(reader));
        return operations;
    }

    public async Task RequeueFailedAsync(Guid localOperationId, CancellationToken cancellationToken = default)
    {
        var now = _timeProvider.GetUtcNow();
        await using var connection = await _connections.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = """
            UPDATE offline_operations
            SET Status = 'QUEUED', NextRetryAt = NULL, LeaseOwner = NULL, LeaseExpiresAt = NULL,
                AttemptCorrelationId = NULL, ResultCode = NULL, UpdatedAt = $now
            WHERE LocalOperationId = $id AND Status = 'FAILED';
            """;
        Add(command, "$id", localOperationId);
        command.Parameters.AddWithValue("$now", Format(now));
        if (await command.ExecuteNonQueryAsync(cancellationToken) != 1)
            throw new OfflineStoreException("LOCAL_STATE_CONFLICT", "Only a failed operation can be retried manually.");
    }

    private async Task CompleteAttemptAsync(
        Guid localOperationId,
        Guid attemptCorrelationId,
        OfflineOperationStatus status,
        string resultCode,
        Guid? conflictCaseId,
        Guid? serverOperationId,
        Guid? resultEntityId,
        long? resultVersion,
        CancellationToken cancellationToken)
    {
        if (status is not (OfflineOperationStatus.Succeeded or OfflineOperationStatus.Conflict or OfflineOperationStatus.Rejected))
        {
            throw new ArgumentOutOfRangeException(nameof(status));
        }

        var now = _timeProvider.GetUtcNow();
        await using var connection = await _connections.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = """
            UPDATE offline_operations
            SET Status = $status, ResultCode = $resultCode, ConflictCaseId = $conflictCaseId,
                ServerOperationId = COALESCE(ServerOperationId, $serverOperationId),
                ResultEntityId = $resultEntityId,
                ResultVersion = $resultVersion, AcknowledgedAt = $now, UpdatedAt = $now,
                LeaseOwner = NULL, LeaseExpiresAt = NULL, NextRetryAt = NULL
            WHERE LocalOperationId = $id AND Status = 'SENDING' AND AttemptCorrelationId = $attemptId
              AND (ServerOperationId IS NULL OR $serverOperationId IS NULL OR ServerOperationId = $serverOperationId);
            """;
        command.Parameters.AddWithValue("$status", ToDatabase(status));
        command.Parameters.AddWithValue("$resultCode", RequireResultCode(resultCode));
        AddNullable(command, "$conflictCaseId", conflictCaseId?.ToString("D"));
        AddNullable(command, "$serverOperationId", serverOperationId?.ToString("D"));
        AddNullable(command, "$resultEntityId", resultEntityId);
        AddNullable(command, "$resultVersion", resultVersion);
        command.Parameters.AddWithValue("$now", Format(now));
        Add(command, "$id", localOperationId);
        Add(command, "$attemptId", attemptCorrelationId);
        if (await command.ExecuteNonQueryAsync(cancellationToken) != 1)
        {
            throw new OfflineStoreException("LOCAL_ATTEMPT_STALE", "The claimed attempt is no longer current.");
        }
    }

    private static async Task<OfflineOperation?> FindByIntentAsync(
        SqliteConnection connection,
        SqliteTransaction transaction,
        Guid companyId,
        Guid deviceId,
        Guid localIntentId,
        CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = """
            SELECT * FROM offline_operations
            WHERE CompanyId = $companyId AND RegisteredDeviceId = $deviceId AND LocalIntentId = $localIntentId;
            """;
        Add(command, "$companyId", companyId);
        Add(command, "$deviceId", deviceId);
        Add(command, "$localIntentId", localIntentId);
        return await ReadSingleAsync(command, cancellationToken);
    }

    private static async Task EnsureNoStaleQueueAsync(
        SqliteConnection connection,
        SqliteTransaction transaction,
        Guid companyId,
        Guid branchId,
        Guid userId,
        Guid registeredDeviceId,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = """
            SELECT EXISTS (
                SELECT 1 FROM offline_operations
                WHERE CompanyId = $companyId AND BranchId = $branchId AND UserId = $userId
                  AND RegisteredDeviceId = $registeredDeviceId
                  AND Status IN ('QUEUED','SENDING','FAILED','CONFLICT')
                  AND CreatedAt <= $staleBoundary
            );
            """;
        Add(command, "$companyId", companyId);
        Add(command, "$branchId", branchId);
        Add(command, "$userId", userId);
        Add(command, "$registeredDeviceId", registeredDeviceId);
        command.Parameters.AddWithValue("$staleBoundary", Format(now - TimeSpan.FromDays(7)));
        if (Convert.ToInt64(await command.ExecuteScalarAsync(cancellationToken),
                System.Globalization.CultureInfo.InvariantCulture) != 0)
            throw new OfflineStoreException(
                "OFFLINE_QUEUE_ESCALATION_REQUIRED",
                "A non-terminal operation has reached seven days; synchronize or escalate before creating new offline writes.");
    }

    private static void EnsureTemplateMatches(
        OfflineOperation existing,
        OfflineOperationEnqueueTemplate template)
    {
        if (existing.CompanyId != template.CompanyId || existing.BranchId != template.BranchId ||
            existing.UserId != template.UserId || existing.RegisteredDeviceId != template.RegisteredDeviceId ||
            !string.Equals(existing.ActionCode, template.ActionCode, StringComparison.Ordinal) ||
            !string.Equals(existing.OperationType, template.OperationType, StringComparison.Ordinal) ||
            !string.Equals(existing.EntityType, template.EntityType, StringComparison.Ordinal) ||
            existing.EntityId != template.EntityId || existing.BaseVersion != template.BaseVersion ||
            existing.ClientOccurredAt.ToUniversalTime() != template.ClientOccurredAt.ToUniversalTime())
            throw new OfflineStoreException(
                "LOCAL_IDEMPOTENCY_MISMATCH",
                "The local intent identity is already bound to different operation metadata.");
    }

    private static async Task<OfflineOperation> GetRequiredAsync(
        SqliteConnection connection,
        SqliteTransaction transaction,
        Guid id,
        CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "SELECT * FROM offline_operations WHERE LocalOperationId = $id;";
        Add(command, "$id", id);
        return await ReadSingleAsync(command, cancellationToken)
            ?? throw new OfflineStoreException("LOCAL_OPERATION_NOT_FOUND", "The local operation does not exist.");
    }

    private static async Task<OfflineOperation?> ReadSingleAsync(SqliteCommand command, CancellationToken cancellationToken)
    {
        await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow, cancellationToken);
        return await reader.ReadAsync(cancellationToken) ? Read(reader) : null;
    }

    private static OfflineOperation Read(SqliteDataReader reader) => new(
        Guid.Parse(reader.GetString(reader.GetOrdinal("LocalOperationId"))),
        Guid.Parse(reader.GetString(reader.GetOrdinal("LocalIntentId"))),
        Guid.Parse(reader.GetString(reader.GetOrdinal("CompanyId"))),
        Guid.Parse(reader.GetString(reader.GetOrdinal("BranchId"))),
        Guid.Parse(reader.GetString(reader.GetOrdinal("UserId"))),
        Guid.Parse(reader.GetString(reader.GetOrdinal("RegisteredDeviceId"))),
        reader.GetString(reader.GetOrdinal("ClientOperationId")),
        Guid.Parse(reader.GetString(reader.GetOrdinal("OperationCorrelationId"))),
        NullableGuid(reader, "AttemptCorrelationId"),
        reader.GetString(reader.GetOrdinal("ProtocolVersion")),
        reader.GetString(reader.GetOrdinal("ActionCode")),
        reader.GetString(reader.GetOrdinal("OperationType")),
        reader.GetString(reader.GetOrdinal("EntityType")),
        NullableGuid(reader, "EntityId"),
        NullableInt64(reader, "BaseVersion"),
        DateTimeOffset.Parse(reader.GetString(reader.GetOrdinal("ClientOccurredAt")), System.Globalization.CultureInfo.InvariantCulture),
        NullableString(reader, "PayloadJson"),
        reader.GetString(reader.GetOrdinal("PayloadHash")),
        reader.GetString(reader.GetOrdinal("RequestFingerprint")),
        ParseStatus(reader.GetString(reader.GetOrdinal("Status"))),
        reader.GetInt32(reader.GetOrdinal("ClientTransportRetryCount")),
        NullableDateTimeOffset(reader, "NextRetryAt"),
        NullableString(reader, "LeaseOwner"),
        NullableDateTimeOffset(reader, "LeaseExpiresAt"),
        NullableString(reader, "ResultCode"),
        NullableGuid(reader, "ConflictCaseId"),
        NullableGuid(reader, "ServerOperationId"),
        NullableGuid(reader, "ResultEntityId"),
        NullableInt64(reader, "ResultVersion"),
        DateTimeOffset.Parse(reader.GetString(reader.GetOrdinal("CreatedAt")), System.Globalization.CultureInfo.InvariantCulture),
        DateTimeOffset.Parse(reader.GetString(reader.GetOrdinal("UpdatedAt")), System.Globalization.CultureInfo.InvariantCulture),
        NullableDateTimeOffset(reader, "AcknowledgedAt"),
        NullableDateTimeOffset(reader, "RedactedAt"));

    private static void EnsureAttempt(OfflineOperation operation, Guid attemptCorrelationId)
    {
        if (operation.Status != OfflineOperationStatus.Sending || operation.AttemptCorrelationId != attemptCorrelationId)
        {
            throw new OfflineStoreException("LOCAL_ATTEMPT_STALE", "The claimed attempt is no longer current.");
        }
    }

    private static string RequireResultCode(string resultCode) =>
        !string.IsNullOrWhiteSpace(resultCode)
            ? resultCode
            : throw new ArgumentException("A result code is required.", nameof(resultCode));

    private static string ToDatabase(OfflineOperationStatus status) => status.ToString().ToUpperInvariant();

    private static OfflineOperationStatus ParseStatus(string value) => value switch
    {
        "QUEUED" => OfflineOperationStatus.Queued,
        "SENDING" => OfflineOperationStatus.Sending,
        "SUCCEEDED" => OfflineOperationStatus.Succeeded,
        "FAILED" => OfflineOperationStatus.Failed,
        "CONFLICT" => OfflineOperationStatus.Conflict,
        "REJECTED" => OfflineOperationStatus.Rejected,
        "RESOLVED" => OfflineOperationStatus.Resolved,
        _ => throw new OfflineStoreException("LOCAL_STORE_CORRUPT", "The local operation contains an unknown state.")
    };

    private static string Format(DateTimeOffset value) => value.ToUniversalTime().ToString("O");
    private static void Add(SqliteCommand command, string name, Guid value) => command.Parameters.AddWithValue(name, value.ToString("D"));
    private static void AddNullable(SqliteCommand command, string name, object? value) => command.Parameters.AddWithValue(name, value ?? DBNull.Value);
    private static string? NullableString(SqliteDataReader reader, string name) { var i = reader.GetOrdinal(name); return reader.IsDBNull(i) ? null : reader.GetString(i); }
    private static Guid? NullableGuid(SqliteDataReader reader, string name) { var value = NullableString(reader, name); return value is null ? null : Guid.Parse(value); }
    private static long? NullableInt64(SqliteDataReader reader, string name) { var i = reader.GetOrdinal(name); return reader.IsDBNull(i) ? null : reader.GetInt64(i); }
    private static DateTimeOffset? NullableDateTimeOffset(SqliteDataReader reader, string name) { var value = NullableString(reader, name); return value is null ? null : DateTimeOffset.Parse(value, System.Globalization.CultureInfo.InvariantCulture); }

    private static async Task EnsureColumnAsync(SqliteConnection connection, string table, string column,
        string definition, CancellationToken cancellationToken)
    {
        await using var inspect = connection.CreateCommand();
        inspect.CommandText = $"PRAGMA table_info({table});";
        await using var reader = await inspect.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
            if (string.Equals(reader.GetString(1), column, StringComparison.OrdinalIgnoreCase)) return;
        await reader.DisposeAsync();
        await using var alter = connection.CreateCommand();
        alter.CommandText = $"ALTER TABLE {table} ADD COLUMN {column} {definition};";
        try { await alter.ExecuteNonQueryAsync(cancellationToken); }
        catch (SqliteException exception) when (exception.SqliteErrorCode == 1 &&
            exception.Message.Contains("duplicate column name", StringComparison.OrdinalIgnoreCase))
        {
            // A second process completed the same monotonic encrypted-store upgrade first.
        }
    }
}
