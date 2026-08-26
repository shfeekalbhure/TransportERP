using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TransportERP.Api.Sync;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

[Collection("PostgreSql")]
public sealed class Stage4SyncRetentionPostgreSqlTests
{
    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Server_90_day_cleanup_redacts_only_terminal_content_and_never_applies_client_24h_or_7d_deletion()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedScopeAsync(db);
        var now = DateTimeOffset.UtcNow;
        var eligiblePayload = "{\"secret\":\"eligible-operation\"}";
        var eligible = await AddOperationAsync(db, scope, "SUCCEEDED", now.AddDays(-100),
            now.AddDays(-91), eligiblePayload);
        var at89Days = await AddOperationAsync(db, scope, "SUCCEEDED", now.AddDays(-100),
            now.AddDays(-89), "{\"secret\":\"89-day-operation\"}");
        var oldCreatedRecentTerminal = await AddOperationAsync(db, scope, "SUCCEEDED", now.AddDays(-100),
            now.AddMinutes(-5), "{\"secret\":\"recent-terminal\"}");
        var clientSuccess = await AddOperationAsync(db, scope, "SUCCEEDED", now.AddDays(-3),
            now.AddDays(-2), "{\"secret\":\"client-24-hour-policy\"}");
        var nonTerminal = await AddOperationAsync(db, scope, "QUEUED", now.AddDays(-100),
            now.AddDays(-91), "{\"secret\":\"non-terminal\"}");
        var clientRejected = await AddOperationAsync(db, scope, "REJECTED", now.AddDays(-10),
            now.AddDays(-8), "{\"secret\":\"client-seven-day-policy\"}");
        var recentResolutionOperation = await AddOperationAsync(db, scope, "RESOLVED", now.AddDays(-100),
            now.AddDays(-91), "{\"secret\":\"old-operation-recent-conflict\"}");
        var eligibleConflict = await AddConflictAsync(db, scope, eligible, now.AddDays(-100),
            now.AddDays(-91), "{\"deviceSecret\":\"eligible\"}", "{\"serverSecret\":\"eligible\"}");
        var recentResolutionConflict = await AddConflictAsync(db, scope, recentResolutionOperation,
            now.AddDays(-100), now.AddMinutes(-5),
            "{\"deviceSecret\":\"recent-resolution\"}", "{\"serverSecret\":\"recent-resolution\"}");
        var originalPayloadHash = eligible.PayloadHash;

        try
        {
            var first = await new SyncRetentionCleanupService(db, new AuditEventService(db))
                .CleanupBatchAsync();
            db.ChangeTracker.Clear();

            var redacted = await db.SyncOperations.AsNoTracking().SingleAsync(x => x.Id == eligible.Id);
            Assert.Equal("{}", redacted.PayloadJson);
            Assert.NotNull(redacted.RedactedAt);
            Assert.Equal(originalPayloadHash, redacted.PayloadHash);
            Assert.Equal(Convert.ToHexString(eligible.RequestFingerprintHash!),
                Convert.ToHexString(redacted.RequestFingerprintHash!));
            Assert.Equal("SUCCEEDED", redacted.Status);

            await AssertOperationNotRedactedAsync(db, at89Days.Id, "89-day-operation");
            await AssertOperationNotRedactedAsync(db, oldCreatedRecentTerminal.Id, "recent-terminal");
            await AssertOperationNotRedactedAsync(db, clientSuccess.Id, "client-24-hour-policy");
            await AssertOperationNotRedactedAsync(db, nonTerminal.Id, "non-terminal");
            await AssertOperationNotRedactedAsync(db, clientRejected.Id, "client-seven-day-policy");

            var conflict = await db.ConflictCases.AsNoTracking().SingleAsync(x => x.Id == eligibleConflict.Id);
            Assert.Equal("{}", conflict.DeviceSnapshot);
            Assert.Equal("{}", conflict.ServerSnapshot);
            Assert.NotNull(conflict.RedactedAt);
            var recentConflict = await db.ConflictCases.AsNoTracking()
                .SingleAsync(x => x.Id == recentResolutionConflict.Id);
            Assert.Null(recentConflict.RedactedAt);
            Assert.Contains("recent-resolution", recentConflict.DeviceSnapshot, StringComparison.Ordinal);
            Assert.Contains("recent-resolution", recentConflict.ServerSnapshot, StringComparison.Ordinal);

            Assert.Equal(2, first.RedactedOperations);
            Assert.Equal(1, first.RedactedConflictCases);
            Assert.NotNull(first.AuditCorrelationId);
            var audit = await db.AuditEvents.AsNoTracking()
                .SingleAsync(x => x.CorrelationId == first.AuditCorrelationId);
            Assert.Null(audit.BeforeJson);
            Assert.Null(audit.AfterJson);
            Assert.Contains("RetentionDays=90", audit.Reason, StringComparison.Ordinal);
            Assert.DoesNotContain("eligible-operation", audit.Reason, StringComparison.Ordinal);
            Assert.DoesNotContain("eligible", JsonSerializer.Serialize(audit), StringComparison.OrdinalIgnoreCase);

            var firstRedactedAt = redacted.RedactedAt;
            var rerun = await new SyncRetentionCleanupService(db, new AuditEventService(db))
                .CleanupBatchAsync();
            db.ChangeTracker.Clear();
            var rerunRow = await db.SyncOperations.AsNoTracking().SingleAsync(x => x.Id == eligible.Id);
            Assert.Equal(firstRedactedAt, rerunRow.RedactedAt);
            Assert.Equal("{}", rerunRow.PayloadJson);
            Assert.Equal(0, rerun.RedactedOperations);
            Assert.Equal(0, rerun.RedactedConflictCases);
        }
        finally
        {
            await DeleteScopeEvidenceAsync(db, scope.CompanyId);
        }
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Database_rejects_early_nonterminal_reversal_snapshot_reversal_and_hash_changes()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedScopeAsync(db);
        var now = DateTimeOffset.UtcNow;
        var early = await AddOperationAsync(db, scope, "SUCCEEDED", now.AddDays(-100),
            now.AddDays(-89), "{\"secret\":\"early\"}");
        var nonTerminal = await AddOperationAsync(db, scope, "QUEUED", now.AddDays(-100),
            now.AddDays(-91), "{\"secret\":\"queued\"}");
        var oldPayload = "{\"secret\":\"one-way\"}";
        var eligible = await AddOperationAsync(db, scope, "RESOLVED", now.AddDays(-100),
            now.AddDays(-91), oldPayload);
        var conflict = await AddConflictAsync(db, scope, eligible, now.AddDays(-100),
            now.AddDays(-91), "{\"device\":\"one-way\"}", "{\"server\":\"one-way\"}");
        var recentConflict = await AddConflictAsync(db, scope, early, now.AddDays(-100),
            now.AddMinutes(-5), "{\"device\":\"early\"}", "{\"server\":\"early\"}");
        var originalHash = eligible.PayloadHash;

        try
        {
            await AssertDeniedAsync(() => db.Database.ExecuteSqlInterpolatedAsync($$"""
                UPDATE transport_erp.sync_operations
                SET "PayloadJson"='{}',"RedactedAt"=clock_timestamp()
                WHERE "Id"={{early.Id}}
                """), "redaction transition denied");
            await AssertDeniedAsync(() => db.Database.ExecuteSqlInterpolatedAsync($$"""
                UPDATE transport_erp.sync_operations
                SET "UpdatedAt"=clock_timestamp()-INTERVAL '91 days'
                WHERE "Id"={{early.Id}}
                """), "terminal retention timestamp is immutable");
            await AssertDeniedAsync(() => db.Database.ExecuteSqlInterpolatedAsync($$"""
                UPDATE transport_erp.sync_operations
                SET "PayloadJson"='{}',"RedactedAt"=clock_timestamp()
                WHERE "Id"={{nonTerminal.Id}}
                """), "redaction transition denied");
            await AssertDeniedAsync(() => db.Database.ExecuteSqlInterpolatedAsync($$"""
                UPDATE transport_erp.conflict_cases
                SET "DeviceSnapshot"='{}',"ServerSnapshot"='{}',"RedactedAt"=clock_timestamp()
                WHERE "Id"={{recentConflict.Id}}
                """), "conflict redaction transition denied");
            await AssertDeniedAsync(() => db.Database.ExecuteSqlInterpolatedAsync($$"""
                UPDATE transport_erp.conflict_cases
                SET "ResolvedAt"=clock_timestamp()-INTERVAL '91 days'
                WHERE "Id"={{recentConflict.Id}}
                """), "resolution retention timestamp is immutable");

            _ = await new SyncRetentionCleanupService(db, new AuditEventService(db)).CleanupBatchAsync();
            db.ChangeTracker.Clear();
            Assert.NotNull((await db.SyncOperations.AsNoTracking().SingleAsync(x => x.Id == eligible.Id)).RedactedAt);
            Assert.NotNull((await db.ConflictCases.AsNoTracking().SingleAsync(x => x.Id == conflict.Id)).RedactedAt);

            await AssertDeniedAsync(() => db.Database.ExecuteSqlInterpolatedAsync($$"""
                UPDATE transport_erp.sync_operations
                SET "PayloadJson"={{oldPayload}},"RedactedAt"=NULL
                WHERE "Id"={{eligible.Id}}
                """), "redaction transition denied");
            await AssertDeniedAsync(() => db.Database.ExecuteSqlInterpolatedAsync($$"""
                UPDATE transport_erp.sync_operations
                SET "PayloadHash"={{new string('f', 64)}}
                WHERE "Id"={{eligible.Id}}
                """), "provenance is immutable");
            await AssertDeniedAsync(() => db.Database.ExecuteSqlInterpolatedAsync($$"""
                UPDATE transport_erp.conflict_cases
                SET "DeviceSnapshot"='{\"restored\":true}',"ServerSnapshot"='{\"restored\":true}',
                    "RedactedAt"=NULL
                WHERE "Id"={{conflict.Id}}
                """), "conflict redaction transition denied");

            var persisted = await db.SyncOperations.AsNoTracking().SingleAsync(x => x.Id == eligible.Id);
            Assert.Equal("{}", persisted.PayloadJson);
            Assert.Equal(originalHash, persisted.PayloadHash);
        }
        finally
        {
            await DeleteScopeEvidenceAsync(db, scope.CompanyId);
        }
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Two_cleanup_workers_claim_distinct_rows_and_redact_each_once()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        Guid companyId;
        Guid firstId;
        Guid secondId;
        await using (var seed = PostgreSqlTestEnvironment.CreateDbContext(connection))
        {
            await seed.Database.MigrateAsync();
            var scope = await SeedScopeAsync(seed);
            companyId = scope.CompanyId;
            var old = DateTimeOffset.UtcNow.AddDays(-91);
            firstId = (await AddOperationAsync(seed, scope, "SUCCEEDED", old, old,
                "{\"worker\":1}")).Id;
            secondId = (await AddOperationAsync(seed, scope, "REJECTED", old, old,
                "{\"worker\":2}")).Id;
        }

        try
        {
            await using var firstDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
            await using var secondDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
            var results = await Task.WhenAll(
                new SyncRetentionCleanupService(firstDb, new AuditEventService(firstDb)).CleanupBatchAsync(1),
                new SyncRetentionCleanupService(secondDb, new AuditEventService(secondDb)).CleanupBatchAsync(1));

            Assert.Equal(2, results.Sum(x => x.RedactedOperations));
            await using var verify = PostgreSqlTestEnvironment.CreateDbContext(connection);
            var rows = await verify.SyncOperations.AsNoTracking()
                .Where(x => x.Id == firstId || x.Id == secondId).ToListAsync();
            Assert.Equal(2, rows.Count);
            Assert.All(rows, x =>
            {
                Assert.Equal("{}", x.PayloadJson);
                Assert.NotNull(x.RedactedAt);
            });
        }
        finally
        {
            await using var cleanup = PostgreSqlTestEnvironment.CreateDbContext(connection);
            await DeleteScopeEvidenceAsync(cleanup, companyId);
        }
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Migration_catalog_contains_one_way_retention_guards()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();

        Assert.Equal(2, await db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value"
            FROM information_schema.columns
            WHERE table_schema='transport_erp' AND column_name='RedactedAt'
              AND table_name IN ('sync_operations','conflict_cases')
              AND data_type='timestamp with time zone' AND is_nullable='YES'
            """).SingleAsync());
        Assert.Equal(2, await db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value" FROM pg_constraint
            WHERE conname IN ('ck_sync_payload_redaction_shape','ck_conflict_snapshot_redaction_shape')
              AND contype='c'
            """).SingleAsync());
        Assert.Equal(2, await db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value" FROM pg_indexes
            WHERE schemaname='transport_erp'
              AND indexname IN ('ix_sync_operation_retention_cleanup','ix_sync_conflict_retention_cleanup')
            """).SingleAsync());
        var operationGuard = await db.Database.SqlQuery<string>($"""
            SELECT pg_get_functiondef('transport_erp.enforce_sync_operation_device_binding()'::regprocedure) AS "Value"
            """).SingleAsync();
        Assert.Contains("redaction transition denied", operationGuard, StringComparison.Ordinal);
        Assert.Contains("UpdatedAt", operationGuard, StringComparison.Ordinal);
        Assert.Contains("PayloadHash", operationGuard, StringComparison.Ordinal);
        var conflictGuard = await db.Database.SqlQuery<string>($"""
            SELECT pg_get_functiondef('transport_erp.enforce_sync_conflict_redaction()'::regprocedure) AS "Value"
            """).SingleAsync();
        Assert.Contains("ResolvedAt", conflictGuard, StringComparison.Ordinal);
        Assert.Contains("90 days", conflictGuard, StringComparison.Ordinal);
    }

    [Fact]
    public void Sync_batch_API_contract_never_exposes_payload_or_payload_hash()
    {
        var operation = new SyncOperation
        {
            Id = Guid.NewGuid(), ClientOperationId = "client-1", OperationCorrelationId = Guid.NewGuid(),
            ActionCode = "CreateWaybillDraft", Status = "SUCCEEDED", PayloadJson = "{}",
            PayloadHash = new string('a', 64), RedactedAt = DateTimeOffset.UtcNow
        };

        var json = JsonSerializer.Serialize(SyncBatchOperationResult.From(operation, DateTimeOffset.UtcNow));

        Assert.DoesNotContain("PayloadJson", json, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("PayloadHash", json, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain(new string('a', 64), json, StringComparison.Ordinal);
    }

    private static async Task AssertOperationNotRedactedAsync(
        TransportErpDbContext db,
        Guid operationId,
        string retainedMarker)
    {
        var operation = await db.SyncOperations.AsNoTracking().SingleAsync(x => x.Id == operationId);
        Assert.Null(operation.RedactedAt);
        Assert.Contains(retainedMarker, operation.PayloadJson, StringComparison.Ordinal);
    }

    private static async Task AssertDeniedAsync(Func<Task<int>> action, string message)
    {
        var exception = await Assert.ThrowsAnyAsync<Exception>(action);
        var postgres = Assert.IsType<PostgresException>(exception.GetBaseException());
        Assert.Equal("P0001", postgres.SqlState);
        Assert.Contains(message, postgres.Message, StringComparison.Ordinal);
    }

    private static async Task<SyncOperation> AddOperationAsync(
        TransportErpDbContext db,
        RetentionScope scope,
        string status,
        DateTimeOffset createdAt,
        DateTimeOffset terminalUpdatedAt,
        string payload)
    {
        var operation = new SyncOperation
        {
            Id = Guid.NewGuid(), DeviceId = scope.DeviceId, UserId = scope.UserId,
            CompanyId = scope.CompanyId, BranchId = scope.BranchId,
            OperationType = "CREATE", EntityType = "Waybill", ClientOperationId = $"ret-{Guid.NewGuid():N}",
            PayloadJson = payload, PayloadHash = Hash(payload), ClientOccurredAt = createdAt,
            ServerReceivedAt = createdAt, Status = status, RetryCount = 0,
            RegisteredDeviceId = scope.RegisteredDeviceId, RegisteredDeviceCredentialVersion = 1,
            ActionCode = "CreateWaybillDraft", ProtocolVersion = "sync-v1",
            OperationCorrelationId = Guid.NewGuid(), RequestFingerprintVersion = "fp-v1",
            RequestFingerprintHash = RandomNumberGenerator.GetBytes(32), ProofKeyVersion = 1,
            ProofKeyThumbprint = scope.Thumbprint, AcceptedProofReplayId = scope.ReplayId,
            CreatedAt = createdAt, UpdatedAt = terminalUpdatedAt,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        db.SyncOperations.Add(operation);
        await db.SaveChangesAsync();
        return operation;
    }

    private static async Task<ConflictCase> AddConflictAsync(
        TransportErpDbContext db,
        RetentionScope scope,
        SyncOperation operation,
        DateTimeOffset createdAt,
        DateTimeOffset resolvedAt,
        string deviceSnapshot,
        string serverSnapshot)
    {
        var conflict = new ConflictCase
        {
            Id = Guid.NewGuid(), SyncOperationId = operation.Id, CompanyId = scope.CompanyId,
            BranchId = scope.BranchId, DeviceSnapshot = deviceSnapshot, ServerSnapshot = serverSnapshot,
            ConflictReason = "VERSION_MISMATCH", Resolution = "KEEP_SERVER", ResolvedBy = "retention-test",
            ResolvedAt = resolvedAt, Status = "RESOLVED", CreatedAt = createdAt, UpdatedAt = resolvedAt,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        db.ConflictCases.Add(conflict);
        await db.SaveChangesAsync();
        return conflict;
    }

    private static async Task<RetentionScope> SeedScopeAsync(TransportErpDbContext db)
    {
        var now = DateTimeOffset.UtcNow;
        var currency = new Currency
        {
            Id = Guid.NewGuid(), Code = await PostgreSqlTestCurrencyCodeAllocator.NextAsync(db),
            NameAr = "عملة retention", MinorUnit = 2, IsBase = true, Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var company = new Company
        {
            Id = Guid.NewGuid(), Code = $"RET-{Guid.NewGuid():N}"[..18], LegalNameAr = "شركة retention",
            BaseCurrencyId = currency.Id, DefaultCalendarId = Guid.NewGuid(), Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var branch = new Branch
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, Code = "MAIN", NameAr = "فرع retention",
            Timezone = "Asia/Riyadh", Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var userName = $"ret-{Guid.NewGuid():N}";
        var user = new User
        {
            Id = Guid.NewGuid(), UserName = userName, NormalizedUserName = userName.ToUpperInvariant(),
            DisplayName = "Retention worker test", PasswordHash = "test-only", Status = "ACTIVE",
            SecurityStamp = Guid.NewGuid().ToString("N"), AuthVersion = 1,
            CompanyId = company.Id, BranchId = branch.Id, CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var deviceId = $"retention-{Guid.NewGuid():N}";
        var thumbprint = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
            .TrimEnd('=').Replace('+', '-').Replace('/', '_');
        var device = new RegisteredDevice
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, DeviceId = deviceId, DisplayName = "Retention device",
            Platform = "TEST", AppVersion = "1", RegistrationRequestId = $"req-{Guid.NewGuid():N}",
            CredentialHash = new string('c', 64), CredentialVersion = 1, Status = "ACTIVE",
            RegisteredByUserId = user.Id, ApprovedByUserId = user.Id, ApprovedAt = now, LastSeenAt = now,
            ProofPublicJwkCanonicalJson = "{\"crv\":\"P-256\",\"kty\":\"EC\",\"x\":\"x\",\"y\":\"y\"}",
            ProofKeyThumbprint = thumbprint, ProofKeyVersion = 1, ProofKeyChangedAt = now,
            ProofKeyChangedByUserId = user.Id, CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var assignment = new RegisteredDeviceAssignment
        {
            Id = Guid.NewGuid(), RegisteredDeviceId = device.Id, UserId = user.Id,
            CompanyId = company.Id, BranchId = branch.Id, Status = "ACTIVE", AssignedByUserId = user.Id,
            AssignedAt = now, CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        db.AddRange(currency, company, branch, user, device, assignment);
        await db.SaveChangesAsync();

        var nonce = new SyncProofNonce
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, RegisteredDeviceId = device.Id,
            DeviceId = deviceId, ProofKeyVersion = 1, NonceHash = RandomNumberGenerator.GetBytes(32),
            IssuedAt = now, ExpiresAt = now.AddMinutes(10)
        };
        db.SyncProofNonces.Add(nonce);
        await db.SaveChangesAsync();
        var replay = new SyncProofReplay
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, RegisteredDeviceId = device.Id,
            DeviceId = deviceId, DeviceAssignmentId = assignment.Id, UserId = user.Id, BranchId = branch.Id,
            ProofKeyVersion = 1, ProofKeyThumbprint = thumbprint,
            JtiHash = RandomNumberGenerator.GetBytes(32), HtuHash = RandomNumberGenerator.GetBytes(32),
            HttpMethod = "POST", NonceRecordId = nonce.Id, IssuedAt = now, FirstSeenAt = now,
            ExpiresAt = now.AddMinutes(10), AttemptCorrelationId = Guid.NewGuid()
        };
        db.SyncProofReplays.Add(replay);
        await db.SaveChangesAsync();
        return new(company.Id, branch.Id, user.Id, device.Id, deviceId, thumbprint, replay.Id);
    }

    private static async Task DeleteScopeEvidenceAsync(TransportErpDbContext db, Guid companyId)
    {
        db.ChangeTracker.Clear();
        await db.Database.ExecuteSqlInterpolatedAsync(
            $"DELETE FROM transport_erp.conflict_cases WHERE \"CompanyId\"={companyId}");
        await db.Database.ExecuteSqlInterpolatedAsync(
            $"DELETE FROM transport_erp.sync_operations WHERE \"CompanyId\"={companyId}");
        await db.Database.ExecuteSqlInterpolatedAsync(
            $"DELETE FROM transport_erp.sync_proof_replays WHERE \"CompanyId\"={companyId}");
        await db.Database.ExecuteSqlInterpolatedAsync(
            $"DELETE FROM transport_erp.sync_proof_nonces WHERE \"CompanyId\"={companyId}");
    }

    private static string Hash(string payload)
        => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(payload))).ToLowerInvariant();

    private sealed record RetentionScope(
        Guid CompanyId,
        Guid BranchId,
        Guid UserId,
        Guid RegisteredDeviceId,
        string DeviceId,
        string Thumbprint,
        Guid ReplayId);
}
