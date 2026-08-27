using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

[Collection("PostgreSql")]
public sealed class Stage4G4MigrationClosurePostgreSqlTests
{
    private const string BeforeExecution = "20260826070000_P0OperationalPartyScopeHardening";
    private const string Execution = "20260826080000_P1Stage4SyncExecutionClaimFoundation";
    private const string Retention = "20260826090000_P1Stage4SyncRetentionRedaction";
    private const string BeforeStage5Hardening = "20260826095000_P1SyncConflictResolvePermission";
    private const string Stage5Hardening = "20260827100000_P1Stage5TenantIntegrityHardening";
    private const string Htu = "https://sync.example.test/api/v1/sync/operations:batch";

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Execution_claim_migration_roundtrips_fresh_up_down_up()
    {
        await WithFreshDatabaseAsync(async connection =>
        {
            await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
            var migrator = db.GetService<IMigrator>();
            await migrator.MigrateAsync(BeforeExecution);
            Assert.Equal(0, await ExecutionSchemaObjectCountAsync(db));

            await migrator.MigrateAsync(Execution);
            Assert.Equal(6, await ExecutionSchemaObjectCountAsync(db));
            Assert.Equal(1, await MigrationHistoryCountAsync(db, Execution));

            await migrator.MigrateAsync(BeforeExecution);
            Assert.Equal(0, await ExecutionSchemaObjectCountAsync(db));
            Assert.Equal(0, await MigrationHistoryCountAsync(db, Execution));

            await migrator.MigrateAsync(Execution);
            Assert.Equal(6, await ExecutionSchemaObjectCountAsync(db));
            Assert.Equal(1, await MigrationHistoryCountAsync(db, Execution));
        });
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Execution_claim_down_fails_closed_then_succeeds_after_claim_is_cleared()
    {
        await WithFreshDatabaseAsync(async connection =>
        {
            await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
            var migrator = db.GetService<IMigrator>();
            await migrator.MigrateAsync(Execution);
            var accepted = await SeedAcceptedProofAsync(db, "EXEC-DOWN");
            var operationId = await InsertStage4OperationAsync(db, accepted, "SENDING", DateTimeOffset.UtcNow);

            var failure = await Assert.ThrowsAnyAsync<Exception>(() =>
                migrator.MigrateAsync(BeforeExecution));
            Assert.Contains("STAGE4_EXECUTION_CLAIM_DOWN_BLOCKED_ACTIVE_CLAIM",
                failure.GetBaseException().Message, StringComparison.Ordinal);
            Assert.Equal(6, await ExecutionSchemaObjectCountAsync(db));
            Assert.Equal(1, await MigrationHistoryCountAsync(db, Execution));
            Assert.Equal(1, await db.Database.SqlQuery<int>($"""
                SELECT count(*)::int AS "Value"
                FROM transport_erp.sync_operations
                WHERE "Id"={operationId} AND "Status"='SENDING'
                  AND "ExecutionClaimToken" IS NOT NULL
                  AND "ExecutionAttemptStartedAt" IS NOT NULL
                  AND "ExecutionLeaseExpiresAt" IS NOT NULL
                """).SingleAsync());

            await db.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE transport_erp.sync_operations
                SET "Status"='REJECTED',
                    "ErrorCode"='TEST_CLAIM_RELEASED',
                    "ExecutionClaimToken"=NULL,
                    "ExecutionAttemptStartedAt"=NULL,
                    "ExecutionLeaseExpiresAt"=NULL,
                    "UpdatedAt"={Normalize(DateTimeOffset.UtcNow)},
                    "RowVersion"={RandomNumberGenerator.GetBytes(16)}
                WHERE "Id"={operationId}
                """);

            await migrator.MigrateAsync(BeforeExecution);
            Assert.Equal(0, await ExecutionSchemaObjectCountAsync(db));
            Assert.Equal(0, await MigrationHistoryCountAsync(db, Execution));
            await migrator.MigrateAsync(Execution);
            Assert.Equal(6, await ExecutionSchemaObjectCountAsync(db));
        });
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Retention_redaction_migration_roundtrips_fresh_up_down_up()
    {
        await WithFreshDatabaseAsync(async connection =>
        {
            await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
            var migrator = db.GetService<IMigrator>();
            await migrator.MigrateAsync(Execution);
            Assert.Equal(0, await RetentionSchemaObjectCountAsync(db));

            await migrator.MigrateAsync(Retention);
            Assert.Equal(7, await RetentionSchemaObjectCountAsync(db));
            Assert.Equal(1, await MigrationHistoryCountAsync(db, Retention));

            await migrator.MigrateAsync(Execution);
            Assert.Equal(0, await RetentionSchemaObjectCountAsync(db));
            Assert.Equal(0, await MigrationHistoryCountAsync(db, Retention));

            await migrator.MigrateAsync(Retention);
            Assert.Equal(7, await RetentionSchemaObjectCountAsync(db));
            Assert.Equal(1, await MigrationHistoryCountAsync(db, Retention));
        });
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Retention_down_fails_closed_for_redacted_data_then_succeeds_after_row_removal()
    {
        await WithFreshDatabaseAsync(async connection =>
        {
            await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
            var migrator = db.GetService<IMigrator>();
            await migrator.MigrateAsync(Retention);
            var accepted = await SeedAcceptedProofAsync(db, "RETENTION-DOWN");
            var terminalAt = Normalize(DateTimeOffset.UtcNow.AddDays(-91));
            var operationId = await InsertStage4OperationAsync(db, accepted, "SUCCEEDED", terminalAt);
            var redactedAt = Normalize(DateTimeOffset.UtcNow);
            const string emptyJson = "{}";
            await db.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE transport_erp.sync_operations
                SET "PayloadJson"={emptyJson}, "RedactedAt"={redactedAt}
                WHERE "Id"={operationId}
                """);

            var failure = await Assert.ThrowsAnyAsync<Exception>(() =>
                migrator.MigrateAsync(Execution));
            Assert.Contains("STAGE4_RETENTION_DOWN_BLOCKED_REDACTED_DATA",
                failure.GetBaseException().Message, StringComparison.Ordinal);
            Assert.Equal(7, await RetentionSchemaObjectCountAsync(db));
            Assert.Equal(1, await MigrationHistoryCountAsync(db, Retention));
            Assert.Equal(1, await db.Database.SqlQuery<int>($"""
                SELECT count(*)::int AS "Value"
                FROM transport_erp.sync_operations
                WHERE "Id"={operationId} AND "PayloadJson"={emptyJson} AND "RedactedAt"={redactedAt}
                """).SingleAsync());

            await db.Database.ExecuteSqlInterpolatedAsync(
                $"DELETE FROM transport_erp.sync_operations WHERE \"Id\"={operationId}");
            await migrator.MigrateAsync(Execution);
            Assert.Equal(0, await RetentionSchemaObjectCountAsync(db));
            Assert.Equal(0, await MigrationHistoryCountAsync(db, Retention));
            await migrator.MigrateAsync(Retention);
            Assert.Equal(7, await RetentionSchemaObjectCountAsync(db));
        });
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Stage5_hardening_upgrades_preexisting_redacted_rows_with_deterministic_90_day_stamp()
    {
        await WithFreshDatabaseAsync(async connection =>
        {
            await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
            var migrator = db.GetService<IMigrator>();
            await migrator.MigrateAsync(BeforeStage5Hardening);
            var accepted = await SeedAcceptedProofAsync(db, "STAGE5-UPGRADE");
            var operationId = await InsertStage4OperationAsync(
                db, accepted, "SUCCEEDED", Normalize(DateTimeOffset.UtcNow.AddDays(-91)));
            var conflictId = Guid.NewGuid();
            var resolvedAt = Normalize(DateTimeOffset.UtcNow.AddDays(-91));
            const string redactedJson = "{}";
            await db.Database.ExecuteSqlInterpolatedAsync($"""
                INSERT INTO transport_erp.conflict_cases
                  ("Id","SyncOperationId","CompanyId","BranchId","BaseVersion","DeviceSnapshot",
                   "ServerSnapshot","ConflictReason","Resolution","ResolvedBy","ResolvedAt",
                   "ReplacedByOperationId","Status","CreatedAt","UpdatedAt","RowVersion")
                VALUES ({conflictId},{operationId},{accepted.Security.CompanyId},{accepted.Security.BranchId},NULL,
                        {"{\"old\":true}"},{"{\"server\":true}"},'UPGRADE_TEST','KEEP_SERVER','test',
                        {resolvedAt},NULL,'RESOLVED',{resolvedAt},{resolvedAt},{RandomNumberGenerator.GetBytes(16)})
                """);
            await db.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE transport_erp.sync_operations
                SET "PayloadJson"={redactedJson},"RedactedAt"={Normalize(DateTimeOffset.UtcNow)}
                WHERE "Id"={operationId}
                """);
            await db.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE transport_erp.conflict_cases
                SET "DeviceSnapshot"={redactedJson},"ServerSnapshot"={redactedJson},
                    "RedactedAt"={Normalize(DateTimeOffset.UtcNow)}
                WHERE "Id"={conflictId}
                """);

            await migrator.MigrateAsync(Stage5Hardening);
            Assert.Equal(90, await db.Database.SqlQuery<int>($"""
                SELECT "RetentionDaysApplied" AS "Value"
                FROM transport_erp.sync_operations WHERE "Id"={operationId}
                """).SingleAsync());
            Assert.False(await db.Database.SqlQuery<bool>($"""
                SELECT "LegalHold" AS "Value"
                FROM transport_erp.sync_operations WHERE "Id"={operationId}
                """).SingleAsync());
            Assert.Equal(90, await db.Database.SqlQuery<int>($"""
                SELECT "RetentionDaysApplied" AS "Value"
                FROM transport_erp.conflict_cases WHERE "Id"={conflictId}
                """).SingleAsync());
            Assert.Equal(4, await Stage5RetentionColumnCountAsync(db));
            Assert.Equal(1, await MigrationHistoryCountAsync(db, Stage5Hardening));
        });
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Stage5_hardening_roundtrips_up_down_up_when_retention_data_is_absent()
    {
        await WithFreshDatabaseAsync(async connection =>
        {
            await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
            var migrator = db.GetService<IMigrator>();
            await migrator.MigrateAsync(BeforeStage5Hardening);
            Assert.Equal(0, await Stage5RetentionColumnCountAsync(db));
            await migrator.MigrateAsync(Stage5Hardening);
            Assert.Equal(4, await Stage5RetentionColumnCountAsync(db));
            await migrator.MigrateAsync(BeforeStage5Hardening);
            Assert.Equal(0, await Stage5RetentionColumnCountAsync(db));
            await migrator.MigrateAsync(Stage5Hardening);
            Assert.Equal(4, await Stage5RetentionColumnCountAsync(db));
        });
    }

    private static async Task<int> ExecutionSchemaObjectCountAsync(TransportErpDbContext db)
    {
        var columns = await db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value"
            FROM information_schema.columns
            WHERE table_schema='transport_erp' AND table_name='sync_operations'
              AND column_name IN ('ExecutionClaimToken','ExecutionAttemptStartedAt','ExecutionLeaseExpiresAt')
            """).SingleAsync();
        var indexes = await db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value"
            FROM pg_indexes
            WHERE schemaname='transport_erp'
              AND indexname IN ('ux_sync_operation_execution_claim','ix_sync_operation_execution_queue')
            """).SingleAsync();
        var constraints = await db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value"
            FROM pg_constraint c JOIN pg_namespace n ON n.oid=c.connamespace
            WHERE n.nspname='transport_erp' AND c.conname='ck_sync_execution_claim_bundle'
            """).SingleAsync();
        return columns + indexes + constraints;
    }

    private static async Task<int> RetentionSchemaObjectCountAsync(TransportErpDbContext db)
    {
        var columns = await db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value"
            FROM information_schema.columns
            WHERE table_schema='transport_erp'
              AND ((table_name='sync_operations' AND column_name='RedactedAt')
                OR (table_name='conflict_cases' AND column_name='RedactedAt'))
            """).SingleAsync();
        var indexes = await db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value"
            FROM pg_indexes
            WHERE schemaname='transport_erp'
              AND indexname IN ('ix_sync_operation_retention_cleanup','ix_sync_conflict_retention_cleanup')
            """).SingleAsync();
        var constraints = await db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value"
            FROM pg_constraint c JOIN pg_namespace n ON n.oid=c.connamespace
            WHERE n.nspname='transport_erp'
              AND c.conname IN ('ck_sync_payload_redaction_shape','ck_conflict_snapshot_redaction_shape')
            """).SingleAsync();
        var triggers = await db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value"
            FROM pg_trigger t JOIN pg_class c ON c.oid=t.tgrelid
            JOIN pg_namespace n ON n.oid=c.relnamespace
            WHERE n.nspname='transport_erp' AND NOT t.tgisinternal
              AND t.tgname='trg_sync_conflict_redaction'
            """).SingleAsync();
        return columns + indexes + constraints + triggers;
    }

    private static Task<int> Stage5RetentionColumnCountAsync(TransportErpDbContext db) =>
        db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value"
            FROM information_schema.columns
            WHERE table_schema='transport_erp'
              AND table_name IN ('sync_operations','conflict_cases')
              AND column_name IN ('LegalHold','RetentionDaysApplied')
            """).SingleAsync();

    private static Task<int> MigrationHistoryCountAsync(
        TransportErpDbContext db,
        string migrationId)
        => db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value"
            FROM transport_erp."__EFMigrationsHistory"
            WHERE "MigrationId"={migrationId}
            """).SingleAsync();

    private static async Task<AcceptedScope> SeedAcceptedProofAsync(
        TransportErpDbContext db,
        string suffix)
    {
        var now = Normalize(DateTimeOffset.UtcNow);
        var currency = new Currency
        {
            Id = Guid.NewGuid(),
            Code = await PostgreSqlTestCurrencyCodeAllocator.NextAsync(db),
            NameAr = "عملة G4 migration",
            MinorUnit = 2,
            IsBase = true,
            CreatedAt = now,
            UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var company = new Company
        {
            Id = Guid.NewGuid(),
            Code = $"GM-{suffix}-{Guid.NewGuid():N}"[..18],
            LegalNameAr = "شركة G4 migration",
            BaseCurrencyId = currency.Id,
            DefaultCalendarId = Guid.NewGuid(),
            Status = "ACTIVE",
            CreatedAt = now,
            UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var branch = new Branch
        {
            Id = Guid.NewGuid(),
            CompanyId = company.Id,
            Code = "MAIN",
            NameAr = "الفرع الرئيسي",
            Timezone = "Asia/Riyadh",
            Status = "ACTIVE",
            CreatedAt = now,
            UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var userName = "g4-migration-" + Guid.NewGuid().ToString("N");
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = userName,
            NormalizedUserName = userName.ToUpperInvariant(),
            DisplayName = "G4 migration user",
            PasswordHash = "test-only",
            SecurityStamp = Guid.NewGuid().ToString("N"),
            AuthVersion = 1,
            Status = "ACTIVE",
            CompanyId = company.Id,
            BranchId = branch.Id,
            CreatedAt = now,
            UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var deviceId = "g4-migration-device-" + Guid.NewGuid().ToString("N");
        var thumbprint = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
            .TrimEnd('=').Replace('+', '-').Replace('/', '_');
        var device = new RegisteredDevice
        {
            Id = Guid.NewGuid(),
            CompanyId = company.Id,
            DeviceId = deviceId,
            DisplayName = "G4 migration device",
            Platform = "TEST",
            AppVersion = "1",
            RegistrationRequestId = "request-" + Guid.NewGuid().ToString("N"),
            CredentialHash = new string('a', 64),
            CredentialVersion = 1,
            Status = "ACTIVE",
            RegisteredByUserId = user.Id,
            ApprovedByUserId = user.Id,
            ApprovedAt = now,
            LastSeenAt = now,
            ProofPublicJwkCanonicalJson = "{\"crv\":\"P-256\",\"kty\":\"EC\",\"x\":\"x\",\"y\":\"y\"}",
            ProofKeyThumbprint = thumbprint,
            ProofKeyVersion = 1,
            ProofKeyChangedAt = now,
            ProofKeyChangedByUserId = user.Id,
            CreatedAt = now,
            UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var assignment = new RegisteredDeviceAssignment
        {
            Id = Guid.NewGuid(),
            RegisteredDeviceId = device.Id,
            UserId = user.Id,
            CompanyId = company.Id,
            BranchId = branch.Id,
            Status = "ACTIVE",
            AssignedByUserId = user.Id,
            AssignedAt = now,
            CreatedAt = now,
            UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        db.AddRange(currency, company, branch, user, device, assignment);
        await db.SaveChangesAsync();
        var security = new SyncProofSecurityContext(
            user.Id, company.Id, branch.Id, device.Id, deviceId);
        var runtime = new SyncProofRuntimeService(db, new AuditEventService(db));
        var nonce = await runtime.IssueNonceAsync(security);
        var proof = await runtime.ClaimAsync(security, new VerifiedSyncProofMaterial(
            Guid.NewGuid().ToString("D"), nonce.Value, thumbprint,
            DateTimeOffset.UtcNow, Htu, Guid.NewGuid()));
        return new AcceptedScope(security, proof);
    }

    private static async Task<Guid> InsertStage4OperationAsync(
        TransportErpDbContext db,
        AcceptedScope accepted,
        string status,
        DateTimeOffset updatedAt)
    {
        var operationId = Guid.NewGuid();
        var resultEntityId = status == "SUCCEEDED" ? Guid.NewGuid() : (Guid?)null;
        var resultVersion = status == "SUCCEEDED" ? 1L : (long?)null;
        var claimToken = status == "SENDING" ? Guid.NewGuid() : (Guid?)null;
        var attemptStartedAt = status == "SENDING" ? updatedAt : (DateTimeOffset?)null;
        var leaseExpiresAt = status == "SENDING" ? updatedAt.AddMinutes(2) : (DateTimeOffset?)null;
        var clientOperationId = "g4-migration-operation-" + Guid.NewGuid().ToString("N");
        const string payload = "{\"sensitive\":\"redact-me\"}";
        var payloadHash = Convert.ToHexString(
            SHA256.HashData(Encoding.UTF8.GetBytes(payload))).ToLowerInvariant();
        var operationCorrelationId = Guid.NewGuid();
        await db.Database.ExecuteSqlInterpolatedAsync($"""
            INSERT INTO transport_erp.sync_operations
              ("Id","DeviceId","UserId","CompanyId","BranchId","OperationType","EntityType","EntityId",
               "ClientOperationId","PayloadJson","PayloadHash","ClientOccurredAt","ServerReceivedAt","BaseVersion",
               "ResultVersion","Status","RetryCount","NextRetryAt","ErrorCode","RegisteredDeviceId",
               "RegisteredDeviceCredentialVersion","CreatedAt","UpdatedAt","RowVersion","ResultEntityId",
               "ActionCode","ProtocolVersion","OperationCorrelationId","RequestFingerprintVersion",
               "RequestFingerprintHash","ProofKeyVersion","ProofKeyThumbprint","AcceptedProofReplayId",
               "ExecutionClaimToken","ExecutionAttemptStartedAt","ExecutionLeaseExpiresAt")
            VALUES
              ({operationId},{accepted.Security.DeviceId},{accepted.Security.UserId},{accepted.Security.CompanyId},
               {accepted.Security.BranchId},'CREATE','JournalEntry',NULL,{clientOperationId},{payload},{payloadHash},
               {updatedAt},{updatedAt},NULL,{resultVersion},{status},0,NULL,NULL,
               {accepted.Security.RegisteredDeviceId},{accepted.Proof.DeviceCredentialVersion},{updatedAt},{updatedAt},
               {RandomNumberGenerator.GetBytes(16)},{resultEntityId},'CreateJournalEntry','sync-v1',
               {operationCorrelationId},'fp-v1',{RandomNumberGenerator.GetBytes(32)},
               {accepted.Proof.ProofKeyVersion},{accepted.Proof.ProofKeyThumbprint},{accepted.Proof.ReplayId},
               {claimToken},{attemptStartedAt},{leaseExpiresAt})
            """);
        return operationId;
    }

    private static DateTimeOffset Normalize(DateTimeOffset value)
    {
        var ticks = value.UtcDateTime.Ticks;
        return new DateTimeOffset(new DateTime(
            ticks - ticks % TimeSpan.TicksPerMicrosecond, DateTimeKind.Utc));
    }

    private static async Task WithFreshDatabaseAsync(Func<string, Task> test)
    {
        var baseConnection = PostgreSqlTestEnvironment.RequireConnection();
        var database = "terp_g4mig_" + Guid.NewGuid().ToString("N");
        var adminBuilder = new NpgsqlConnectionStringBuilder(baseConnection)
        {
            Database = "postgres",
            Pooling = false
        };
        await using (var admin = new NpgsqlConnection(adminBuilder.ConnectionString))
        {
            await admin.OpenAsync();
            await using var create = new NpgsqlCommand($"CREATE DATABASE \"{database}\"", admin);
            await create.ExecuteNonQueryAsync();
        }
        var testBuilder = new NpgsqlConnectionStringBuilder(baseConnection)
        {
            Database = database,
            Pooling = false
        };
        try
        {
            await test(testBuilder.ConnectionString);
        }
        finally
        {
            NpgsqlConnection.ClearAllPools();
            await using var admin = new NpgsqlConnection(adminBuilder.ConnectionString);
            await admin.OpenAsync();
            await using var drop = new NpgsqlCommand(
                $"DROP DATABASE IF EXISTS \"{database}\" WITH (FORCE)", admin);
            await drop.ExecuteNonQueryAsync();
        }
    }

    private sealed record AcceptedScope(
        SyncProofSecurityContext Security,
        AcceptedSyncProofContext Proof);
}
