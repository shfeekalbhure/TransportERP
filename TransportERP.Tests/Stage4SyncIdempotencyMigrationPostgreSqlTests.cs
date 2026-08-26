using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

[Collection("PostgreSql")]
public sealed class Stage4SyncIdempotencyMigrationPostgreSqlTests
{
    private const string Stage3Migration = "20260826010000_P1RegisteredDevices";
    private const string Stage4Migration = "20260826030000_P1Stage4SyncIdempotencyFoundation";

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Accepted_replay_is_required_and_idempotency_is_company_scoped()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var deviceId = $"stage4-shared-{Guid.NewGuid():N}";
        var first = await SeedScopeAsync(db, "A", deviceId);
        var second = await SeedScopeAsync(db, "B", deviceId);
        var clientOperationId = $"client-{Guid.NewGuid():N}";

        try
        {
            var firstOperation = await InsertAcceptedOperationAsync(db, first, clientOperationId);
            var secondOperation = await InsertAcceptedOperationAsync(db, second, clientOperationId);
            Assert.NotEqual(firstOperation.Id, secondOperation.Id);

            var duplicate = await Assert.ThrowsAsync<DbUpdateException>(() =>
                InsertAcceptedOperationAsync(db, first, clientOperationId));
            var duplicateDetail = Assert.IsType<Npgsql.PostgresException>(duplicate.GetBaseException());
            Assert.Equal("23505", duplicateDetail.SqlState);
            Assert.Equal("ux_sync_op_registered_device_client", duplicateDetail.ConstraintName);
            db.ChangeTracker.Clear();

            var partialBundle = NewAcceptedOperation(first, firstOperation.AcceptedProofReplayId!.Value,
                $"partial-{Guid.NewGuid():N}");
            partialBundle.ProtocolVersion = null;
            db.SyncOperations.Add(partialBundle);
            var partialBundleError = await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
            var partialBundleDetail = Assert.IsType<Npgsql.PostgresException>(
                partialBundleError.GetBaseException());
            Assert.Equal("23514", partialBundleDetail.SqlState);
            Assert.Equal("ck_sync_stage4_contract_bundle", partialBundleDetail.ConstraintName);
            db.ChangeTracker.Clear();

            var missingFingerprintVersion = NewAcceptedOperation(first,
                firstOperation.AcceptedProofReplayId!.Value, $"partial-fp-{Guid.NewGuid():N}");
            missingFingerprintVersion.RequestFingerprintVersion = null;
            db.SyncOperations.Add(missingFingerprintVersion);
            var missingFingerprintError = await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
            var missingFingerprintDetail = Assert.IsType<Npgsql.PostgresException>(
                missingFingerprintError.GetBaseException());
            Assert.Equal("P0001", missingFingerprintDetail.SqlState);
            Assert.Contains("new sync operation requires accepted Stage4 proof replay",
                missingFingerprintDetail.Message, StringComparison.Ordinal);
            db.ChangeTracker.Clear();

            db.SyncOperations.Add(NewAcceptedOperation(first, Guid.NewGuid(), $"fake-{Guid.NewGuid():N}"));
            var fakeReplay = await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
            Assert.Contains("accepted proof replay scope mismatch",
                fakeReplay.GetBaseException().Message, StringComparison.Ordinal);
            db.ChangeTracker.Clear();

            db.SyncOperations.Add(NewAcceptedOperation(first, secondOperation.AcceptedProofReplayId!.Value,
                $"mismatch-{Guid.NewGuid():N}"));
            var mismatchedReplay = await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
            Assert.Contains("accepted proof replay scope mismatch",
                mismatchedReplay.GetBaseException().Message, StringComparison.Ordinal);
            db.ChangeTracker.Clear();

            await AssertPostgresAsync("P0001", () => db.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE transport_erp.sync_proof_replays SET "FirstSeenAt"="FirstSeenAt"
                WHERE "Id"={firstOperation.AcceptedProofReplayId!.Value}
                """));

            db.SyncOperations.Add(NewLegacyOperation(first, $"legacy-{Guid.NewGuid():N}"));
            var legacyError = await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
            Assert.Contains("new sync operation requires accepted Stage4 proof replay",
                legacyError.GetBaseException().Message, StringComparison.Ordinal);
            db.ChangeTracker.Clear();

            var persisted = await db.SyncOperations.SingleAsync(x => x.Id == firstOperation.Id);
            var executionStartedAt = DateTimeOffset.UtcNow;
            persisted.Status = "SENDING";
            persisted.ExecutionClaimToken = Guid.NewGuid();
            persisted.ExecutionAttemptStartedAt = executionStartedAt;
            persisted.ExecutionLeaseExpiresAt = executionStartedAt.AddMinutes(2);
            persisted.UpdatedAt = executionStartedAt;
            persisted.RowVersion = RandomNumberGenerator.GetBytes(16);
            await db.SaveChangesAsync();
            persisted.ProofKeyVersion++;
            var immutableError = await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
            Assert.Contains("sync operation provenance is immutable",
                immutableError.GetBaseException().Message, StringComparison.Ordinal);
            db.ChangeTracker.Clear();
            var changedPayload = "{\"changed\":true}";
            await AssertPostgresAsync("P0001", () => db.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE transport_erp.sync_operations SET "PayloadJson"={changedPayload}
                WHERE "Id"={firstOperation.Id}
                """));
            await AssertPostgresAsync("P0001", () => db.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE transport_erp.sync_operations SET "RegisteredDeviceCredentialVersion"=2
                WHERE "Id"={firstOperation.Id}
                """));
        }
        finally
        {
            db.ChangeTracker.Clear();
            await DeleteStage4EvidenceAsync(db, first.CompanyId, second.CompanyId);
        }
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Migration_preserves_legacy_nullable_update_blocks_cross_generation_collision_and_roundtrips()
    {
        await WithFreshDatabaseAsync(async connection =>
        {
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedScopeAsync(db, "LEGACY", $"legacy-device-{Guid.NewGuid():N}");
        var migrator = db.GetService<IMigrator>();
        await migrator.MigrateAsync(Stage3Migration);

        var legacyId = Guid.NewGuid();
        var entityId = Guid.NewGuid();
        var clientOperationId = $"legacy-client-{Guid.NewGuid():N}";
        var legacyPayload = "{\"legacy\":true}";
        var now = DateTimeOffset.UtcNow;
        await db.Database.ExecuteSqlInterpolatedAsync($"""
            INSERT INTO transport_erp.sync_operations
              ("Id","DeviceId","UserId","CompanyId","BranchId","OperationType","EntityType","EntityId",
               "ClientOperationId","PayloadJson","PayloadHash","ClientOccurredAt","ServerReceivedAt","BaseVersion",
               "ResultVersion","Status","RetryCount","NextRetryAt","ErrorCode","RegisteredDeviceId",
               "RegisteredDeviceCredentialVersion","CreatedAt","UpdatedAt","RowVersion")
            VALUES
              ({legacyId},{scope.DeviceId},{scope.UserId},{scope.CompanyId},{scope.BranchId},'UPDATE','LegacyEntity',{entityId},
               {clientOperationId},{legacyPayload},{new string('a', 64)},{now},{now},1,
               NULL,'QUEUED',0,NULL,NULL,{scope.RegisteredDeviceId},1,{now},{now},{RandomNumberGenerator.GetBytes(16)})
            """);

        // The DbContext model includes later Stage 4 execution columns. Bring the
        // database to the current exact-head schema before exercising an EF insert;
        // otherwise EF correctly emits those columns against the older 030000 schema
        // and masks the cross-generation idempotency assertion with 42703.
        await db.Database.MigrateAsync();
        var legacyShape = await db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value" FROM transport_erp.sync_operations
            WHERE "Id"={legacyId} AND "RequestFingerprintVersion" IS NULL
              AND "AcceptedProofReplayId" IS NULL AND "ResultEntityId" IS NULL
            """).SingleAsync();
        Assert.Equal(1, legacyShape);
        await db.Database.ExecuteSqlInterpolatedAsync(
            $"UPDATE transport_erp.sync_operations SET \"Status\"='REJECTED' WHERE \"Id\"={legacyId}");

        var replay = await InsertProofAsync(db, scope);
        db.SyncOperations.Add(NewAcceptedOperation(scope, replay.Id, clientOperationId));
        var collision = await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
        Assert.Contains("Stage4 idempotency key collides with legacy row",
            collision.GetBaseException().Message, StringComparison.Ordinal);
        var collisionDetail = Assert.IsType<Npgsql.PostgresException>(collision.GetBaseException());
        Assert.Equal("23505", collisionDetail.SqlState);
        Assert.Equal("ux_sync_op_legacy_company_device_client", collisionDetail.ConstraintName);
        db.ChangeTracker.Clear();

        await db.Database.ExecuteSqlInterpolatedAsync(
            $"DELETE FROM transport_erp.sync_operations WHERE \"Id\"={legacyId}");
        await DeleteStage4EvidenceAsync(db, scope.CompanyId);
        await migrator.MigrateAsync(Stage3Migration);
        Assert.Equal(0, await db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value" FROM information_schema.tables
            WHERE table_schema='transport_erp' AND table_name IN ('sync_proof_nonces','sync_proof_replays')
            """).SingleAsync());
        Assert.Equal(1, await db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value" FROM pg_indexes
            WHERE schemaname='transport_erp' AND indexname='IX_sync_operations_DeviceId_ClientOperationId'
            """).SingleAsync());
        await db.Database.MigrateAsync();
        Assert.Equal(2, await db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value" FROM information_schema.tables
            WHERE table_schema='transport_erp' AND table_name IN ('sync_proof_nonces','sync_proof_replays')
            """).SingleAsync());
        });
    }

    [Theory]
    [InlineData("missing")]
    [InlineData("drift")]
    [Trait("Category", "PostgreSQL")]
    public async Task Up_preflight_rejects_missing_or_drifted_legacy_index_without_partial_ddl(string shape)
    {
        await WithFreshDatabaseAsync(async connection =>
        {
            await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
            var migrator = db.GetService<IMigrator>();
            await migrator.MigrateAsync(Stage3Migration);
            var guardFunctionsBefore = await db.Database.SqlQuery<string>($"""
                SELECT string_agg(pg_get_functiondef(p.oid), E'\n---\n' ORDER BY p.proname) AS "Value"
                FROM pg_proc p JOIN pg_namespace n ON n.oid=p.pronamespace
                WHERE n.nspname='transport_erp' AND p.proname IN
                  ('enforce_sync_operation_device_binding','enforce_sync_operation_user_scope')
                """).SingleAsync();
            var guardTriggersBefore = await db.Database.SqlQuery<string>($"""
                SELECT string_agg(pg_get_triggerdef(t.oid), E'\n---\n' ORDER BY t.tgname) AS "Value"
                FROM pg_trigger t JOIN pg_class c ON c.oid=t.tgrelid
                JOIN pg_namespace n ON n.oid=c.relnamespace
                WHERE n.nspname='transport_erp' AND NOT t.tgisinternal AND t.tgname IN
                  ('trg_sync_operations_device_binding','trg_sync_operations_user_scope')
                """).SingleAsync();

            await db.Database.ExecuteSqlRawAsync(
                "DROP INDEX transport_erp.\"IX_sync_operations_DeviceId_ClientOperationId\"");
            if (shape == "drift")
            {
                await db.Database.ExecuteSqlRawAsync("""
                    CREATE UNIQUE INDEX "IX_sync_operations_DeviceId_ClientOperationId"
                      ON transport_erp.sync_operations ("ClientOperationId","DeviceId")
                    """);
            }

            var error = await Assert.ThrowsAnyAsync<Exception>(() => migrator.MigrateAsync(Stage4Migration));
            Assert.Contains("STAGE4_UP_LEGACY_INDEX_MISSING_OR_DRIFT",
                error.GetBaseException().Message, StringComparison.Ordinal);

            Assert.Equal(0, await db.Database.SqlQuery<int>($"""
                SELECT count(*)::int AS "Value" FROM information_schema.tables
                WHERE table_schema='transport_erp'
                  AND table_name IN ('sync_proof_nonces','sync_proof_replays')
                """).SingleAsync());
            Assert.Equal(0, await db.Database.SqlQuery<int>($"""
                SELECT count(*)::int AS "Value" FROM information_schema.columns
                WHERE table_schema='transport_erp' AND (
                  (table_name='sync_operations' AND column_name IN
                    ('ResultEntityId','ActionCode','ProtocolVersion','OperationCorrelationId',
                     'RequestFingerprintVersion','RequestFingerprintHash','ProofKeyVersion',
                     'ProofKeyThumbprint','AcceptedProofReplayId')) OR
                  (table_name='audit_events' AND column_name='OperationCorrelationId'))
                """).SingleAsync());
            Assert.Equal(0, await db.Database.SqlQuery<int>($"""
                SELECT count(*)::int AS "Value" FROM pg_constraint c
                JOIN pg_namespace n ON n.oid=c.connamespace
                WHERE n.nspname='transport_erp' AND c.conname IN
                  ('ux_device_assignment_proof_scope','ck_sync_stage4_contract_bundle')
                """).SingleAsync());
            Assert.Equal(0, await db.Database.SqlQuery<int>($"""
                SELECT count(*)::int AS "Value" FROM pg_indexes
                WHERE schemaname='transport_erp' AND indexname IN
                  ('ix_audit_event_operation_correlation','ux_sync_nonce_hash',
                   'ix_sync_nonce_device_key_expiry','ix_sync_nonce_expiry',
                   'ux_sync_replay_device_key_jti','ix_sync_replay_expiry','ix_sync_replay_nonce',
                   'ux_sync_op_registered_device_client','ux_sync_op_legacy_company_device_client',
                   'ix_sync_op_accepted_proof')
                """).SingleAsync());
            Assert.Equal(0, await db.Database.SqlQuery<int>($"""
                SELECT count(*)::int AS "Value" FROM pg_proc p
                JOIN pg_namespace n ON n.oid=p.pronamespace
                WHERE n.nspname='transport_erp' AND p.proname='fn_sync_replay_append_only'
                """).SingleAsync());
            Assert.Equal(0, await db.Database.SqlQuery<int>($"""
                SELECT count(*)::int AS "Value" FROM pg_trigger t
                JOIN pg_class c ON c.oid=t.tgrelid JOIN pg_namespace n ON n.oid=c.relnamespace
                WHERE n.nspname='transport_erp' AND NOT t.tgisinternal
                  AND t.tgname='trg_sync_replay_append_only'
                """).SingleAsync());
            Assert.Equal(guardFunctionsBefore, await db.Database.SqlQuery<string>($"""
                SELECT string_agg(pg_get_functiondef(p.oid), E'\n---\n' ORDER BY p.proname) AS "Value"
                FROM pg_proc p JOIN pg_namespace n ON n.oid=p.pronamespace
                WHERE n.nspname='transport_erp' AND p.proname IN
                  ('enforce_sync_operation_device_binding','enforce_sync_operation_user_scope')
                """).SingleAsync());
            Assert.Equal(guardTriggersBefore, await db.Database.SqlQuery<string>($"""
                SELECT string_agg(pg_get_triggerdef(t.oid), E'\n---\n' ORDER BY t.tgname) AS "Value"
                FROM pg_trigger t JOIN pg_class c ON c.oid=t.tgrelid
                JOIN pg_namespace n ON n.oid=c.relnamespace
                WHERE n.nspname='transport_erp' AND NOT t.tgisinternal AND t.tgname IN
                  ('trg_sync_operations_device_binding','trg_sync_operations_user_scope')
                """).SingleAsync());
            Assert.Equal(0, await db.Database.SqlQuery<int>($"""
                SELECT count(*)::int AS "Value" FROM transport_erp."__EFMigrationsHistory"
                WHERE "MigrationId"={Stage4Migration}
                """).SingleAsync());
        });
    }

    [Theory]
    [InlineData("stage4", "STAGE4_DOWN_BLOCKED_DATA_PRESENT")]
    [InlineData("audit", "STAGE4_DOWN_LEGACY_SHAPE_CONFLICT")]
    [InlineData("null-entity", "STAGE4_DOWN_LEGACY_SHAPE_CONFLICT")]
    [InlineData("result-entity", "STAGE4_DOWN_LEGACY_SHAPE_CONFLICT")]
    [InlineData("global-duplicate", "STAGE4_DOWN_LEGACY_SHAPE_CONFLICT")]
    [Trait("Category", "PostgreSQL")]
    public async Task Down_is_fail_closed_without_partial_ddl(string shape, string expectedCode)
    {
        await WithFreshDatabaseAsync(async connection =>
        {
            await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
            await db.Database.MigrateAsync();
            var scope = await SeedScopeAsync(db, $"DOWN-{shape}", $"down-{shape}-{Guid.NewGuid():N}");

            if (shape == "stage4")
            {
                await InsertAcceptedOperationAsync(db, scope, $"down-{Guid.NewGuid():N}");
            }
            else if (shape == "audit")
            {
                db.AuditEvents.Add(new AuditEvent
                {
                    Id = Guid.NewGuid(), OccurredAt = DateTimeOffset.UtcNow, ActorUserId = scope.UserId,
                    CompanyId = scope.CompanyId, BranchId = scope.BranchId, Action = "Stage4DownGuard",
                    Outcome = "SUCCESS", EntityType = "Stage4Guard", CorrelationId = Guid.NewGuid(),
                    OperationCorrelationId = Guid.NewGuid(), Hash = Convert.ToHexString(RandomNumberGenerator.GetBytes(32))
                });
                await db.SaveChangesAsync();
            }
            else if (shape is "null-entity" or "result-entity")
            {
                var operation = await InsertAcceptedOperationAsync(db, scope, $"down-null-{Guid.NewGuid():N}");
                await db.Database.ExecuteSqlRawAsync("""
                    ALTER TABLE transport_erp.sync_operations
                      DISABLE TRIGGER trg_sync_operations_device_binding
                    """);
                try
                {
                    if (shape == "null-entity")
                    {
                        await db.Database.ExecuteSqlInterpolatedAsync($"""
                            UPDATE transport_erp.sync_operations SET
                              "EntityId"=NULL, "ActionCode"=NULL, "ProtocolVersion"=NULL,
                              "OperationCorrelationId"=NULL, "RequestFingerprintVersion"=NULL,
                              "RequestFingerprintHash"=NULL, "ProofKeyVersion"=NULL,
                              "ProofKeyThumbprint"=NULL, "AcceptedProofReplayId"=NULL
                            WHERE "Id"={operation.Id}
                            """);
                    }
                    else
                    {
                        await db.Database.ExecuteSqlInterpolatedAsync($"""
                            UPDATE transport_erp.sync_operations SET
                              "ResultEntityId"={Guid.NewGuid()}, "ActionCode"=NULL, "ProtocolVersion"=NULL,
                              "OperationCorrelationId"=NULL, "RequestFingerprintVersion"=NULL,
                              "RequestFingerprintHash"=NULL, "ProofKeyVersion"=NULL,
                              "ProofKeyThumbprint"=NULL, "AcceptedProofReplayId"=NULL
                            WHERE "Id"={operation.Id}
                            """);
                    }
                }
                finally
                {
                    await db.Database.ExecuteSqlRawAsync("""
                        ALTER TABLE transport_erp.sync_operations
                          ENABLE TRIGGER trg_sync_operations_device_binding
                        """);
                }
                await db.Database.ExecuteSqlInterpolatedAsync(
                    $"DELETE FROM transport_erp.sync_proof_replays WHERE \"CompanyId\"={scope.CompanyId}");
                await db.Database.ExecuteSqlInterpolatedAsync(
                    $"DELETE FROM transport_erp.sync_proof_nonces WHERE \"CompanyId\"={scope.CompanyId}");
            }
            else if (shape == "global-duplicate")
            {
                var second = await SeedScopeAsync(db, "DOWN-DUP-B", scope.DeviceId);
                var clientOperationId = $"down-duplicate-{Guid.NewGuid():N}";
                await db.Database.ExecuteSqlRawAsync("""
                    ALTER TABLE transport_erp.sync_operations
                      DISABLE TRIGGER trg_sync_operations_device_binding
                    """);
                try
                {
                    db.SyncOperations.AddRange(
                        NewLegacyOperation(scope, clientOperationId),
                        NewLegacyOperation(second, clientOperationId));
                    await db.SaveChangesAsync();
                }
                finally
                {
                    await db.Database.ExecuteSqlRawAsync("""
                        ALTER TABLE transport_erp.sync_operations
                          ENABLE TRIGGER trg_sync_operations_device_binding
                    """);
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(shape), shape, "Unknown Stage4 Down guard shape.");
            }

            var migrator = db.GetService<IMigrator>();
            var error = await Assert.ThrowsAnyAsync<Exception>(() => migrator.MigrateAsync(Stage3Migration));
            Assert.Contains(expectedCode, error.GetBaseException().Message, StringComparison.Ordinal);
            Assert.Equal(2, await db.Database.SqlQuery<int>($"""
                SELECT count(*)::int AS "Value" FROM information_schema.tables
                WHERE table_schema='transport_erp' AND table_name IN ('sync_proof_nonces','sync_proof_replays')
                """).SingleAsync());
            Assert.Equal(1, await db.Database.SqlQuery<int>($"""
                SELECT count(*)::int AS "Value" FROM pg_indexes
                WHERE schemaname='transport_erp' AND indexname='ux_sync_op_registered_device_client'
                """).SingleAsync());
        });
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Catalog_has_exact_stage4_constraints_indexes_and_triggers()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();

        var constraintCount = await db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value" FROM pg_constraint c
            JOIN pg_namespace n ON n.oid=c.connamespace
            WHERE n.nspname='transport_erp' AND c.conname IN
              ('ux_device_assignment_proof_scope','ck_sync_stage4_contract_bundle',
               'pk_sync_proof_nonces','ux_sync_nonce_scope','fk_sync_nonce_registered_device',
               'ck_sync_nonce_key_version','ck_sync_nonce_hash_len','ck_sync_nonce_window',
               'pk_sync_proof_replays','fk_sync_replay_registered_device','fk_sync_replay_assignment_scope',
               'fk_sync_replay_nonce_scope','ck_sync_replay_key_version','ck_sync_replay_hash_len',
               'ck_sync_replay_method','ck_sync_replay_window')
            """).SingleAsync();
        Assert.Equal(16, constraintCount);

        var bundleDefinition = await GetConstraintDefinitionAsync(db, "ck_sync_stage4_contract_bundle");
        Assert.Contains("\"RequestFingerprintVersion\"", bundleDefinition, StringComparison.Ordinal);
        Assert.Contains("'fp-v1'", bundleDefinition, StringComparison.Ordinal);
        Assert.Contains("\"ProtocolVersion\"", bundleDefinition, StringComparison.Ordinal);
        Assert.Contains("'sync-v1'", bundleDefinition, StringComparison.Ordinal);
        Assert.Contains("\"RequestFingerprintVersion\" IS NOT NULL", bundleDefinition,
            StringComparison.Ordinal);
        Assert.Contains("\"ProtocolVersion\" IS NOT NULL", bundleDefinition,
            StringComparison.Ordinal);
        Assert.Contains("octet_length(\"RequestFingerprintHash\") = 32", bundleDefinition,
            StringComparison.Ordinal);
        Assert.Contains("length((\"ProofKeyThumbprint\")::text) = 43", bundleDefinition,
            StringComparison.Ordinal);
        Assert.Contains("\"AcceptedProofReplayId\" IS NOT NULL", bundleDefinition,
            StringComparison.Ordinal);

        var nonceKeyDefinition = await GetConstraintDefinitionAsync(db, "ck_sync_nonce_key_version");
        Assert.Contains("\"ProofKeyVersion\" >= 1", nonceKeyDefinition, StringComparison.Ordinal);
        var nonceHashDefinition = await GetConstraintDefinitionAsync(db, "ck_sync_nonce_hash_len");
        Assert.Contains("octet_length(\"NonceHash\") = 32", nonceHashDefinition,
            StringComparison.Ordinal);
        var nonceWindowDefinition = await GetConstraintDefinitionAsync(db, "ck_sync_nonce_window");
        Assert.Contains("\"ExpiresAt\" > \"IssuedAt\"", nonceWindowDefinition,
            StringComparison.Ordinal);

        var replayKeyDefinition = await GetConstraintDefinitionAsync(db, "ck_sync_replay_key_version");
        Assert.Contains("\"ProofKeyVersion\" >= 1", replayKeyDefinition, StringComparison.Ordinal);
        var replayHashDefinition = await GetConstraintDefinitionAsync(db, "ck_sync_replay_hash_len");
        Assert.Contains("octet_length(\"JtiHash\") = 32", replayHashDefinition,
            StringComparison.Ordinal);
        Assert.Contains("octet_length(\"HtuHash\") = 32", replayHashDefinition,
            StringComparison.Ordinal);
        Assert.Contains("char_length((\"ProofKeyThumbprint\")::text) = 43", replayHashDefinition,
            StringComparison.Ordinal);
        var replayMethodDefinition = await GetConstraintDefinitionAsync(db, "ck_sync_replay_method");
        Assert.Contains("\"HttpMethod\"", replayMethodDefinition, StringComparison.Ordinal);
        Assert.Contains("'POST'", replayMethodDefinition, StringComparison.Ordinal);
        var replayWindowDefinition = await GetConstraintDefinitionAsync(db, "ck_sync_replay_window");
        Assert.Contains("\"ExpiresAt\" > \"FirstSeenAt\"", replayWindowDefinition,
            StringComparison.Ordinal);
        Assert.Contains("00:00:30", replayWindowDefinition, StringComparison.Ordinal);
        Assert.Contains("00:02:00", replayWindowDefinition, StringComparison.Ordinal);

        var indexCount = await db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value" FROM pg_indexes
            WHERE schemaname='transport_erp' AND indexname IN
              ('ix_audit_event_operation_correlation','ux_sync_nonce_hash','ix_sync_nonce_device_key_expiry',
               'ix_sync_nonce_expiry','ux_sync_replay_device_key_jti','ix_sync_replay_expiry',
               'ix_sync_replay_nonce','ux_sync_op_registered_device_client',
               'ux_sync_op_legacy_company_device_client','ix_sync_op_accepted_proof')
            """).SingleAsync();
        Assert.Equal(10, indexCount);

        Assert.Equal(9, await db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value" FROM information_schema.columns
            WHERE table_schema='transport_erp' AND table_name='sync_operations' AND column_name IN
              ('ResultEntityId','ActionCode','ProtocolVersion','OperationCorrelationId','RequestFingerprintVersion',
               'RequestFingerprintHash','ProofKeyVersion','ProofKeyThumbprint','AcceptedProofReplayId')
            """).SingleAsync());
        Assert.Equal(8, await db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value" FROM information_schema.columns
            WHERE table_schema='transport_erp' AND table_name='sync_proof_nonces'
            """).SingleAsync());
        Assert.Equal(17, await db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value" FROM information_schema.columns
            WHERE table_schema='transport_erp' AND table_name='sync_proof_replays'
            """).SingleAsync());

        Assert.Equal(10, await db.Database.SqlQuery<int>($"""
            WITH expected(table_name,column_name,data_type,is_nullable,max_length) AS (VALUES
              ('audit_events','OperationCorrelationId','uuid','YES',NULL::integer),
              ('sync_operations','ResultEntityId','uuid','YES',NULL::integer),
              ('sync_operations','ActionCode','character varying','YES',120),
              ('sync_operations','ProtocolVersion','character varying','YES',20),
              ('sync_operations','OperationCorrelationId','uuid','YES',NULL::integer),
              ('sync_operations','RequestFingerprintVersion','character varying','YES',16),
              ('sync_operations','RequestFingerprintHash','bytea','YES',NULL::integer),
              ('sync_operations','ProofKeyVersion','integer','YES',NULL::integer),
              ('sync_operations','ProofKeyThumbprint','character varying','YES',43),
              ('sync_operations','AcceptedProofReplayId','uuid','YES',NULL::integer)
            )
            SELECT count(*)::int AS "Value"
            FROM expected e
            JOIN information_schema.columns c
              ON c.table_schema='transport_erp' AND c.table_name=e.table_name
             AND c.column_name=e.column_name AND c.data_type=e.data_type
             AND c.is_nullable=e.is_nullable
             AND c.character_maximum_length IS NOT DISTINCT FROM e.max_length
            """).SingleAsync());
        Assert.Equal(25, await db.Database.SqlQuery<int>($"""
            WITH expected(table_name,column_name,data_type,max_length) AS (VALUES
              ('sync_proof_nonces','Id','uuid',NULL::integer),
              ('sync_proof_nonces','CompanyId','uuid',NULL::integer),
              ('sync_proof_nonces','RegisteredDeviceId','uuid',NULL::integer),
              ('sync_proof_nonces','DeviceId','character varying',120),
              ('sync_proof_nonces','ProofKeyVersion','integer',NULL::integer),
              ('sync_proof_nonces','NonceHash','bytea',NULL::integer),
              ('sync_proof_nonces','IssuedAt','timestamp with time zone',NULL::integer),
              ('sync_proof_nonces','ExpiresAt','timestamp with time zone',NULL::integer),
              ('sync_proof_replays','Id','uuid',NULL::integer),
              ('sync_proof_replays','CompanyId','uuid',NULL::integer),
              ('sync_proof_replays','RegisteredDeviceId','uuid',NULL::integer),
              ('sync_proof_replays','DeviceId','character varying',120),
              ('sync_proof_replays','DeviceAssignmentId','uuid',NULL::integer),
              ('sync_proof_replays','UserId','uuid',NULL::integer),
              ('sync_proof_replays','BranchId','uuid',NULL::integer),
              ('sync_proof_replays','ProofKeyVersion','integer',NULL::integer),
              ('sync_proof_replays','ProofKeyThumbprint','character varying',43),
              ('sync_proof_replays','JtiHash','bytea',NULL::integer),
              ('sync_proof_replays','HtuHash','bytea',NULL::integer),
              ('sync_proof_replays','HttpMethod','character varying',8),
              ('sync_proof_replays','NonceRecordId','uuid',NULL::integer),
              ('sync_proof_replays','IssuedAt','timestamp with time zone',NULL::integer),
              ('sync_proof_replays','FirstSeenAt','timestamp with time zone',NULL::integer),
              ('sync_proof_replays','ExpiresAt','timestamp with time zone',NULL::integer),
              ('sync_proof_replays','AttemptCorrelationId','uuid',NULL::integer)
            )
            SELECT count(*)::int AS "Value"
            FROM expected e
            JOIN information_schema.columns c
              ON c.table_schema='transport_erp' AND c.table_name=e.table_name
             AND c.column_name=e.column_name AND c.data_type=e.data_type
             AND c.is_nullable='NO'
             AND c.character_maximum_length IS NOT DISTINCT FROM e.max_length
            """).SingleAsync());

        var stage4Index = await db.Database.SqlQuery<string>($"""
            SELECT indexdef AS "Value" FROM pg_indexes
            WHERE schemaname='transport_erp' AND indexname='ux_sync_op_registered_device_client'
            """).SingleAsync();
        Assert.Contains("(\"CompanyId\", \"RegisteredDeviceId\", \"ClientOperationId\")", stage4Index,
            StringComparison.Ordinal);
        var stage4Predicate = await db.Database.SqlQuery<string>($"""
            SELECT pg_get_expr(x.indpred,x.indrelid) AS "Value" FROM pg_index x
            JOIN pg_class i ON i.oid=x.indexrelid JOIN pg_namespace n ON n.oid=i.relnamespace
            WHERE n.nspname='transport_erp' AND i.relname='ux_sync_op_registered_device_client'
            """).SingleAsync();
        Assert.Contains("\"RegisteredDeviceId\" IS NOT NULL", stage4Predicate, StringComparison.Ordinal);
        Assert.Contains("\"RequestFingerprintVersion\"", stage4Predicate, StringComparison.Ordinal);
        Assert.Contains("fp-v1", stage4Predicate, StringComparison.Ordinal);
        var legacyIndex = await db.Database.SqlQuery<string>($"""
            SELECT indexdef AS "Value" FROM pg_indexes
            WHERE schemaname='transport_erp' AND indexname='ux_sync_op_legacy_company_device_client'
            """).SingleAsync();
        Assert.Contains("(\"CompanyId\", \"DeviceId\", \"ClientOperationId\")", legacyIndex,
            StringComparison.Ordinal);
        var legacyPredicate = await db.Database.SqlQuery<string>($"""
            SELECT pg_get_expr(x.indpred,x.indrelid) AS "Value" FROM pg_index x
            JOIN pg_class i ON i.oid=x.indexrelid JOIN pg_namespace n ON n.oid=i.relnamespace
            WHERE n.nspname='transport_erp' AND i.relname='ux_sync_op_legacy_company_device_client'
            """).SingleAsync();
        Assert.Contains("\"RequestFingerprintVersion\" IS NULL", legacyPredicate,
            StringComparison.Ordinal);

        Assert.Equal(4, await db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value" FROM pg_constraint
            WHERE conname IN ('fk_sync_nonce_registered_device','fk_sync_replay_registered_device',
                              'fk_sync_replay_assignment_scope','fk_sync_replay_nonce_scope')
              AND contype='f' AND confdeltype='r'
            """).SingleAsync());
        Assert.Equal(4, await db.Database.SqlQuery<int>($"""
            WITH expected(name,child_table,parent_table,child_columns,parent_columns) AS (VALUES
              ('fk_sync_nonce_registered_device','sync_proof_nonces','registered_devices',
               ARRAY['RegisteredDeviceId','CompanyId','DeviceId']::text[],
               ARRAY['Id','CompanyId','DeviceId']::text[]),
              ('fk_sync_replay_registered_device','sync_proof_replays','registered_devices',
               ARRAY['RegisteredDeviceId','CompanyId','DeviceId']::text[],
               ARRAY['Id','CompanyId','DeviceId']::text[]),
              ('fk_sync_replay_assignment_scope','sync_proof_replays','registered_device_assignments',
               ARRAY['DeviceAssignmentId','RegisteredDeviceId','CompanyId','UserId','BranchId']::text[],
               ARRAY['Id','RegisteredDeviceId','CompanyId','UserId','BranchId']::text[]),
              ('fk_sync_replay_nonce_scope','sync_proof_replays','sync_proof_nonces',
               ARRAY['NonceRecordId','CompanyId','RegisteredDeviceId','DeviceId','ProofKeyVersion']::text[],
               ARRAY['Id','CompanyId','RegisteredDeviceId','DeviceId','ProofKeyVersion']::text[])
            )
            SELECT count(*)::int AS "Value"
            FROM expected e
            JOIN pg_constraint c ON c.conname=e.name AND c.contype='f' AND c.confdeltype='r'
            JOIN pg_class child ON child.oid=c.conrelid AND child.relname=e.child_table
            JOIN pg_namespace child_ns ON child_ns.oid=child.relnamespace
              AND child_ns.nspname='transport_erp'
            JOIN pg_class parent ON parent.oid=c.confrelid AND parent.relname=e.parent_table
            JOIN pg_namespace parent_ns ON parent_ns.oid=parent.relnamespace
              AND parent_ns.nspname='transport_erp'
            WHERE (SELECT array_agg(a.attname::text ORDER BY u.ordinality)
                   FROM unnest(c.conkey) WITH ORDINALITY u(attnum,ordinality)
                   JOIN pg_attribute a ON a.attrelid=c.conrelid AND a.attnum=u.attnum)=e.child_columns
              AND (SELECT array_agg(a.attname::text ORDER BY u.ordinality)
                   FROM unnest(c.confkey) WITH ORDINALITY u(attnum,ordinality)
                   JOIN pg_attribute a ON a.attrelid=c.confrelid AND a.attnum=u.attnum)=e.parent_columns
            """).SingleAsync());
        Assert.Equal(0, await db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value" FROM pg_constraint c
            JOIN pg_class t ON t.oid=c.conrelid JOIN pg_namespace n ON n.oid=t.relnamespace
            WHERE n.nspname='transport_erp' AND t.relname='sync_operations' AND c.contype='f'
              AND pg_get_constraintdef(c.oid) LIKE '%AcceptedProofReplayId%'
            """).SingleAsync());

        var triggerCount = await db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value" FROM pg_trigger t
            JOIN pg_class c ON c.oid=t.tgrelid
            JOIN pg_namespace n ON n.oid=c.relnamespace
            WHERE n.nspname='transport_erp' AND NOT t.tgisinternal AND t.tgenabled='O'
              AND t.tgname IN ('trg_sync_operations_user_scope','trg_sync_operations_device_binding',
                               'trg_sync_replay_append_only')
            """).SingleAsync();
        Assert.Equal(3, triggerCount);

        Assert.Equal(3, await db.Database.SqlQuery<int>($"""
            WITH expected(name,table_name,function_name,trigger_type) AS (VALUES
              ('trg_sync_operations_device_binding','sync_operations',
               'enforce_sync_operation_device_binding',23::smallint),
              ('trg_sync_operations_user_scope','sync_operations',
               'enforce_sync_operation_user_scope',23::smallint),
              ('trg_sync_replay_append_only','sync_proof_replays',
               'fn_sync_replay_append_only',19::smallint)
            )
            SELECT count(*)::int AS "Value"
            FROM expected e
            JOIN pg_trigger t ON t.tgname=e.name AND NOT t.tgisinternal
              AND t.tgenabled='O' AND t.tgtype=e.trigger_type
            JOIN pg_class c ON c.oid=t.tgrelid AND c.relname=e.table_name
            JOIN pg_namespace n ON n.oid=c.relnamespace AND n.nspname='transport_erp'
            JOIN pg_proc p ON p.oid=t.tgfoid AND p.proname=e.function_name
            """).SingleAsync());
        var userScopeTriggerColumns = await db.Database.SqlQuery<string>($"""
            SELECT array_to_string(ARRAY(
              SELECT a.attname FROM unnest(t.tgattr::smallint[]) WITH ORDINALITY u(attnum,ordinality)
              JOIN pg_attribute a ON a.attrelid=t.tgrelid AND a.attnum=u.attnum
              ORDER BY u.ordinality
            ),',') AS "Value"
            FROM pg_trigger t JOIN pg_class c ON c.oid=t.tgrelid
            JOIN pg_namespace n ON n.oid=c.relnamespace
            WHERE n.nspname='transport_erp' AND t.tgname='trg_sync_operations_user_scope'
            """).SingleAsync();
        Assert.Equal("UserId,CompanyId,BranchId", userScopeTriggerColumns);

        var replayTrigger = await db.Database.SqlQuery<string>($"""
            SELECT pg_get_triggerdef(t.oid) AS "Value" FROM pg_trigger t
            JOIN pg_class c ON c.oid=t.tgrelid JOIN pg_namespace n ON n.oid=c.relnamespace
            WHERE n.nspname='transport_erp' AND t.tgname='trg_sync_replay_append_only'
            """).SingleAsync();
        Assert.Contains("BEFORE UPDATE", replayTrigger, StringComparison.Ordinal);
        Assert.DoesNotContain("DELETE", replayTrigger, StringComparison.Ordinal);
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Replay_scope_shape_and_append_only_guards_are_exact()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var first = await SeedScopeAsync(db, "REPLAY-A", $"replay-a-{Guid.NewGuid():N}");
        var second = await SeedScopeAsync(db, "REPLAY-B", $"replay-b-{Guid.NewGuid():N}");

        try
        {
            var firstNonce = await InsertNonceAsync(db, first);
            var secondNonce = await InsertNonceAsync(db, second);
            var valid = NewReplay(first, firstNonce.Id);
            db.SyncProofReplays.Add(valid);
            await db.SaveChangesAsync();

            await AssertPostgresAsync("P0001", () => db.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE transport_erp.sync_proof_replays SET "FirstSeenAt"="FirstSeenAt" WHERE "Id"={valid.Id}
                """));

            var shortThumbprint = NewReplay(first, firstNonce.Id);
            shortThumbprint.ProofKeyThumbprint = new string('t', 42);
            await AssertConstraintAsync(db, shortThumbprint, "ck_sync_replay_hash_len");

            var shortJti = NewReplay(first, firstNonce.Id);
            shortJti.JtiHash = RandomNumberGenerator.GetBytes(31);
            await AssertConstraintAsync(db, shortJti, "ck_sync_replay_hash_len");

            var shortHtu = NewReplay(first, firstNonce.Id);
            shortHtu.HtuHash = RandomNumberGenerator.GetBytes(31);
            await AssertConstraintAsync(db, shortHtu, "ck_sync_replay_hash_len");

            var wrongMethod = NewReplay(first, firstNonce.Id);
            wrongMethod.HttpMethod = "GET";
            await AssertConstraintAsync(db, wrongMethod, "ck_sync_replay_method");

            var zeroRemainingWindow = NewReplay(first, firstNonce.Id);
            zeroRemainingWindow.ExpiresAt = zeroRemainingWindow.FirstSeenAt;
            await AssertConstraintAsync(db, zeroRemainingWindow, "ck_sync_replay_window");

            var wrongAssignment = NewReplay(first, firstNonce.Id);
            wrongAssignment.DeviceAssignmentId = second.AssignmentId;
            await AssertConstraintAsync(db, wrongAssignment, "fk_sync_replay_assignment_scope");

            var wrongNonce = NewReplay(first, secondNonce.Id);
            await AssertConstraintAsync(db, wrongNonce, "fk_sync_replay_nonce_scope");

            var invalidDeviceNonce = NewNonce(first);
            invalidDeviceNonce.DeviceId = $"missing-{Guid.NewGuid():N}";
            db.SyncProofNonces.Add(invalidDeviceNonce);
            var deviceError = await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
            var deviceDetail = Assert.IsType<Npgsql.PostgresException>(deviceError.GetBaseException());
            Assert.Equal("23503", deviceDetail.SqlState);
            Assert.Equal("fk_sync_nonce_registered_device", deviceDetail.ConstraintName);
            db.ChangeTracker.Clear();

            var invalidNonceKey = NewNonce(first);
            invalidNonceKey.ProofKeyVersion = 0;
            await AssertNonceConstraintAsync(db, invalidNonceKey, "ck_sync_nonce_key_version");

            var shortNonceHash = NewNonce(first);
            shortNonceHash.NonceHash = RandomNumberGenerator.GetBytes(31);
            await AssertNonceConstraintAsync(db, shortNonceHash, "ck_sync_nonce_hash_len");

            var zeroNonceWindow = NewNonce(first);
            zeroNonceWindow.ExpiresAt = zeroNonceWindow.IssuedAt;
            await AssertNonceConstraintAsync(db, zeroNonceWindow, "ck_sync_nonce_window");
        }
        finally
        {
            db.ChangeTracker.Clear();
            await DeleteStage4EvidenceAsync(db, first.CompanyId, second.CompanyId);
        }
    }

    [Theory]
    [InlineData(30, true)]
    [InlineData(31, false)]
    [InlineData(-120, true)]
    [InlineData(-121, false)]
    [Trait("Category", "PostgreSQL")]
    public async Task Replay_iat_window_accepts_exact_skew_boundaries_and_rejects_one_second_beyond(
        int issuedOffsetSeconds, bool shouldBeAccepted)
    {
        await WithFreshDatabaseAsync(async connection =>
        {
            await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
            await db.Database.MigrateAsync();
            var scope = await SeedScopeAsync(db, $"IAT-{issuedOffsetSeconds}",
                $"iat-{issuedOffsetSeconds}-{Guid.NewGuid():N}");
            var nonce = await InsertNonceAsync(db, scope);
            var replay = NewReplay(scope, nonce.Id);
            replay.FirstSeenAt = DateTimeOffset.UtcNow;
            replay.IssuedAt = replay.FirstSeenAt.AddSeconds(issuedOffsetSeconds);
            replay.ExpiresAt = replay.FirstSeenAt.AddMinutes(10);

            if (shouldBeAccepted)
            {
                db.SyncProofReplays.Add(replay);
                await db.SaveChangesAsync();
                Assert.True(await db.SyncProofReplays.AnyAsync(x => x.Id == replay.Id));
            }
            else
            {
                await AssertConstraintAsync(db, replay, "ck_sync_replay_window");
            }
        });
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Replay_registered_device_fk_rejects_exact_scope_mismatch()
    {
        await WithFreshDatabaseAsync(async connection =>
        {
            await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
            await db.Database.MigrateAsync();
            var scope = await SeedScopeAsync(db, "REPLAY-DEVICE", $"replay-device-{Guid.NewGuid():N}");

            await db.Database.ExecuteSqlRawAsync("""
                ALTER TABLE transport_erp.sync_proof_nonces
                  DROP CONSTRAINT fk_sync_nonce_registered_device
                """);
            var invalidDeviceId = $"missing-{Guid.NewGuid():N}";
            var nonce = NewNonce(scope);
            nonce.DeviceId = invalidDeviceId;
            db.SyncProofNonces.Add(nonce);
            await db.SaveChangesAsync();

            var replay = NewReplay(scope, nonce.Id);
            replay.DeviceId = invalidDeviceId;
            await AssertConstraintAsync(db, replay, "fk_sync_replay_registered_device");
        });
    }

    private static async Task<SyncOperation> InsertAcceptedOperationAsync(
        TransportErpDbContext db, Stage4Scope scope, string clientOperationId)
    {
        var replay = await InsertProofAsync(db, scope);
        var operation = NewAcceptedOperation(scope, replay.Id, clientOperationId);
        db.SyncOperations.Add(operation);
        await db.SaveChangesAsync();
        return operation;
    }

    private static async Task<SyncProofReplay> InsertProofAsync(TransportErpDbContext db, Stage4Scope scope)
    {
        var nonce = await InsertNonceAsync(db, scope);
        var replay = NewReplay(scope, nonce.Id);
        db.SyncProofReplays.Add(replay);
        await db.SaveChangesAsync();
        return replay;
    }

    private static async Task<SyncProofNonce> InsertNonceAsync(TransportErpDbContext db, Stage4Scope scope)
    {
        var nonce = NewNonce(scope);
        db.SyncProofNonces.Add(nonce);
        await db.SaveChangesAsync();
        return nonce;
    }

    private static SyncProofNonce NewNonce(Stage4Scope scope)
    {
        var now = DateTimeOffset.UtcNow;
        return new SyncProofNonce
        {
            Id = Guid.NewGuid(), CompanyId = scope.CompanyId, RegisteredDeviceId = scope.RegisteredDeviceId,
            DeviceId = scope.DeviceId, ProofKeyVersion = 1, NonceHash = RandomNumberGenerator.GetBytes(32),
            IssuedAt = now, ExpiresAt = now.AddMinutes(5)
        };
    }

    private static SyncProofReplay NewReplay(Stage4Scope scope, Guid nonceId)
    {
        var now = DateTimeOffset.UtcNow;
        return new SyncProofReplay
        {
            Id = Guid.NewGuid(), CompanyId = scope.CompanyId, RegisteredDeviceId = scope.RegisteredDeviceId,
            DeviceId = scope.DeviceId, DeviceAssignmentId = scope.AssignmentId, UserId = scope.UserId,
            BranchId = scope.BranchId, ProofKeyVersion = 1, ProofKeyThumbprint = new string('t', 43),
            JtiHash = RandomNumberGenerator.GetBytes(32), HtuHash = RandomNumberGenerator.GetBytes(32),
            HttpMethod = "POST", NonceRecordId = nonceId, IssuedAt = now,
            FirstSeenAt = now.AddSeconds(1), ExpiresAt = now.AddMinutes(4), AttemptCorrelationId = Guid.NewGuid()
        };
    }

    private static async Task AssertConstraintAsync(
        TransportErpDbContext db, SyncProofReplay replay, string constraintName)
    {
        db.SyncProofReplays.Add(replay);
        var error = await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
        var detail = Assert.IsType<Npgsql.PostgresException>(error.GetBaseException());
        Assert.Contains(detail.SqlState, new[] { "23503", "23514" });
        Assert.Equal(constraintName, detail.ConstraintName);
        db.ChangeTracker.Clear();
    }

    private static async Task AssertNonceConstraintAsync(
        TransportErpDbContext db, SyncProofNonce nonce, string constraintName)
    {
        db.SyncProofNonces.Add(nonce);
        var error = await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
        var detail = Assert.IsType<Npgsql.PostgresException>(error.GetBaseException());
        Assert.Equal("23514", detail.SqlState);
        Assert.Equal(constraintName, detail.ConstraintName);
        db.ChangeTracker.Clear();
    }

    private static Task<string> GetConstraintDefinitionAsync(
        TransportErpDbContext db, string constraintName)
        => db.Database.SqlQuery<string>($"""
            SELECT pg_get_constraintdef(c.oid) AS "Value"
            FROM pg_constraint c JOIN pg_namespace n ON n.oid=c.connamespace
            WHERE n.nspname='transport_erp' AND c.conname={constraintName}
            """).SingleAsync();

    private static SyncOperation NewAcceptedOperation(Stage4Scope scope, Guid replayId, string clientOperationId)
    {
        var now = DateTimeOffset.UtcNow;
        return new SyncOperation
        {
            Id = Guid.NewGuid(), DeviceId = scope.DeviceId, UserId = scope.UserId, CompanyId = scope.CompanyId,
            BranchId = scope.BranchId, OperationType = "UPDATE", EntityType = "Stage4Entity",
            EntityId = Guid.NewGuid(), ClientOperationId = clientOperationId, PayloadJson = "{\"stage4\":true}",
            PayloadHash = new string('b', 64), ClientOccurredAt = now, ServerReceivedAt = now, BaseVersion = 1,
            Status = "QUEUED", RetryCount = 0, RegisteredDeviceId = scope.RegisteredDeviceId,
            RegisteredDeviceCredentialVersion = 1, ActionCode = "stage4.update", ProtocolVersion = "sync-v1",
            OperationCorrelationId = Guid.NewGuid(), RequestFingerprintVersion = "fp-v1",
            RequestFingerprintHash = RandomNumberGenerator.GetBytes(32), ProofKeyVersion = 1,
            ProofKeyThumbprint = new string('t', 43), AcceptedProofReplayId = replayId,
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
    }

    private static SyncOperation NewLegacyOperation(Stage4Scope scope, string clientOperationId)
    {
        var now = DateTimeOffset.UtcNow;
        return new SyncOperation
        {
            Id = Guid.NewGuid(), DeviceId = scope.DeviceId, UserId = scope.UserId, CompanyId = scope.CompanyId,
            BranchId = scope.BranchId, OperationType = "UPDATE", EntityType = "LegacyEntity",
            EntityId = Guid.NewGuid(), ClientOperationId = clientOperationId, PayloadJson = "{\"legacy\":true}",
            PayloadHash = new string('c', 64), ClientOccurredAt = now, ServerReceivedAt = now,
            Status = "QUEUED", RetryCount = 0, RegisteredDeviceId = scope.RegisteredDeviceId,
            RegisteredDeviceCredentialVersion = 1, CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
    }

    private static async Task<Stage4Scope> SeedScopeAsync(
        TransportErpDbContext db, string suffix, string deviceId)
    {
        var now = DateTimeOffset.UtcNow;
        var currency = new Currency
        {
            Id = Guid.NewGuid(), Code = await PostgreSqlTestCurrencyCodeAllocator.NextAsync(db),
            NameAr = "عملة", MinorUnit = 2, IsBase = true, Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var company = new Company
        {
            Id = Guid.NewGuid(), Code = $"S4-{suffix}-{Guid.NewGuid():N}"[..18], LegalNameAr = "شركة Stage4",
            BaseCurrencyId = currency.Id, DefaultCalendarId = Guid.NewGuid(), Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var branch = new Branch
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, Code = $"B-{Guid.NewGuid():N}"[..12], NameAr = "فرع",
            Timezone = "Asia/Riyadh", Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var userName = $"s4-{Guid.NewGuid():N}";
        var user = new User
        {
            Id = Guid.NewGuid(), UserName = userName, NormalizedUserName = userName.ToUpperInvariant(),
            DisplayName = "Stage4 operator", PasswordHash = "test-only", SecurityStamp = Guid.NewGuid().ToString("N"),
            AuthVersion = 1, Status = "ACTIVE", CompanyId = company.Id, BranchId = branch.Id,
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        db.AddRange(currency, company, branch, user);
        await db.SaveChangesAsync();
        var device = new RegisteredDevice
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, DeviceId = deviceId, DisplayName = "Stage4 device",
            Platform = "TEST", AppVersion = "1", RegistrationRequestId = $"request-{Guid.NewGuid():N}",
            CredentialHash = new string('d', 64), CredentialVersion = 1, Status = "ACTIVE",
            RegisteredByUserId = user.Id, ApprovedByUserId = user.Id, ApprovedAt = now, LastSeenAt = now,
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var assignment = new RegisteredDeviceAssignment
        {
            Id = Guid.NewGuid(), RegisteredDeviceId = device.Id, UserId = user.Id, CompanyId = company.Id,
            BranchId = branch.Id, Status = "ACTIVE", AssignedByUserId = user.Id, AssignedAt = now,
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        db.AddRange(device, assignment);
        await db.SaveChangesAsync();
        return new(company.Id, branch.Id, user.Id, device.Id, assignment.Id, deviceId);
    }

    private static async Task AssertPostgresAsync(string sqlState, Func<Task> action)
    {
        var exception = await Assert.ThrowsAnyAsync<Exception>(action);
        Assert.Equal(sqlState, (exception.GetBaseException() as Npgsql.PostgresException)?.SqlState);
    }

    private static async Task DeleteStage4EvidenceAsync(TransportErpDbContext db, params Guid[] companyIds)
    {
        foreach (var companyId in companyIds)
        {
            await db.Database.ExecuteSqlInterpolatedAsync(
                $"DELETE FROM transport_erp.sync_operations WHERE \"CompanyId\"={companyId}");
            await db.Database.ExecuteSqlInterpolatedAsync(
                $"DELETE FROM transport_erp.sync_proof_replays WHERE \"CompanyId\"={companyId}");
            await db.Database.ExecuteSqlInterpolatedAsync(
                $"DELETE FROM transport_erp.sync_proof_nonces WHERE \"CompanyId\"={companyId}");
        }
    }

    private static async Task WithFreshDatabaseAsync(Func<string, Task> test)
    {
        var baseConnection = PostgreSqlTestEnvironment.RequireConnection();
        var database = $"transporterp_stage4_migration_{Guid.NewGuid():N}";
        var adminBuilder = new Npgsql.NpgsqlConnectionStringBuilder(baseConnection)
        {
            Database = "postgres", Pooling = false
        };
        await using (var admin = new Npgsql.NpgsqlConnection(adminBuilder.ConnectionString))
        {
            await admin.OpenAsync();
            await using var create = new Npgsql.NpgsqlCommand($"CREATE DATABASE \"{database}\"", admin);
            await create.ExecuteNonQueryAsync();
        }
        var testBuilder = new Npgsql.NpgsqlConnectionStringBuilder(baseConnection)
        {
            Database = database, Pooling = false
        };
        try
        {
            await test(testBuilder.ConnectionString);
        }
        finally
        {
            Npgsql.NpgsqlConnection.ClearAllPools();
            await using var admin = new Npgsql.NpgsqlConnection(adminBuilder.ConnectionString);
            await admin.OpenAsync();
            await using var drop = new Npgsql.NpgsqlCommand(
                $"DROP DATABASE IF EXISTS \"{database}\" WITH (FORCE)", admin);
            await drop.ExecuteNonQueryAsync();
        }
    }

    private sealed record Stage4Scope(
        Guid CompanyId, Guid BranchId, Guid UserId, Guid RegisteredDeviceId, Guid AssignmentId, string DeviceId);
}
