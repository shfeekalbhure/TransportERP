using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

[Collection("PostgreSql")]
public sealed class Stage4ProofKeyLifecycleMigrationPostgreSqlTests
{
    private const string Stage4Foundation = "20260826030000_P1Stage4SyncIdempotencyFoundation";
    private const string ProofKeyLifecycle = "20260826040000_P1Stage4ProofKeyLifecycle";
    private const string BeforeStage5Hardening = "20260826095000_P1SyncConflictResolvePermission";
    private const string Thumbprint = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
    private const string RotatedThumbprint = "BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB";
    private const string NeverUsedThumbprint = "CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC";

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Catalog_matches_the_governing_proof_key_lifecycle_contract()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();

        Assert.Equal(5, await db.Database.SqlQuery<int>($"""
            WITH expected(column_name,data_type,is_nullable,max_length) AS (VALUES
              ('ProofPublicJwkCanonicalJson','character varying','YES',512),
              ('ProofKeyThumbprint','character varying','YES',43),
              ('ProofKeyVersion','integer','YES',NULL::integer),
              ('ProofKeyChangedAt','timestamp with time zone','YES',NULL::integer),
              ('ProofKeyChangedByUserId','uuid','YES',NULL::integer)
            )
            SELECT count(*)::int AS "Value" FROM expected e
            JOIN information_schema.columns c
              ON c.table_schema='transport_erp' AND c.table_name='registered_devices'
             AND c.column_name=e.column_name AND c.data_type=e.data_type
             AND c.is_nullable=e.is_nullable
             AND c.character_maximum_length IS NOT DISTINCT FROM e.max_length
            """).SingleAsync());

        Assert.Equal(27, await db.Database.SqlQuery<int>($"""
            WITH expected(table_name,column_name,data_type,is_nullable,max_length) AS (VALUES
              ('registered_device_proof_key_challenges','Id','uuid','NO',NULL::integer),
              ('registered_device_proof_key_challenges','CompanyId','uuid','NO',NULL::integer),
              ('registered_device_proof_key_challenges','RegisteredDeviceId','uuid','NO',NULL::integer),
              ('registered_device_proof_key_challenges','DeviceId','character varying','NO',120),
              ('registered_device_proof_key_challenges','ChangeRequestId','uuid','NO',NULL::integer),
              ('registered_device_proof_key_challenges','ChangeType','character varying','NO',8),
              ('registered_device_proof_key_challenges','ExpectedProofKeyVersion','integer','YES',NULL::integer),
              ('registered_device_proof_key_challenges','NewProofKeyThumbprint','character varying','NO',43),
              ('registered_device_proof_key_challenges','ChallengeHash','bytea','NO',NULL::integer),
              ('registered_device_proof_key_challenges','IssuedAt','timestamp with time zone','NO',NULL::integer),
              ('registered_device_proof_key_challenges','ExpiresAt','timestamp with time zone','NO',NULL::integer),
              ('registered_device_proof_key_challenges','ConsumedAt','timestamp with time zone','YES',NULL::integer),
              ('registered_device_proof_key_challenges','CreatedByUserId','uuid','NO',NULL::integer),
              ('registered_device_proof_key_changes','Id','uuid','NO',NULL::integer),
              ('registered_device_proof_key_changes','CompanyId','uuid','NO',NULL::integer),
              ('registered_device_proof_key_changes','RegisteredDeviceId','uuid','NO',NULL::integer),
              ('registered_device_proof_key_changes','DeviceId','character varying','NO',120),
              ('registered_device_proof_key_changes','ChangeRequestId','uuid','NO',NULL::integer),
              ('registered_device_proof_key_changes','ChallengeId','uuid','NO',NULL::integer),
              ('registered_device_proof_key_changes','ChangeType','character varying','NO',8),
              ('registered_device_proof_key_changes','ExpectedProofKeyVersion','integer','YES',NULL::integer),
              ('registered_device_proof_key_changes','PreviousProofKeyThumbprint','character varying','YES',43),
              ('registered_device_proof_key_changes','NewProofKeyThumbprint','character varying','NO',43),
              ('registered_device_proof_key_changes','ResultProofKeyVersion','integer','NO',NULL::integer),
              ('registered_device_proof_key_changes','ChangedByUserId','uuid','NO',NULL::integer),
              ('registered_device_proof_key_changes','Reason','character varying','YES',500),
              ('registered_device_proof_key_changes','ChangedAt','timestamp with time zone','NO',NULL::integer)
            )
            SELECT count(*)::int AS "Value" FROM expected e
            JOIN information_schema.columns c
              ON c.table_schema='transport_erp' AND c.table_name=e.table_name
             AND c.column_name=e.column_name AND c.data_type=e.data_type
             AND c.is_nullable=e.is_nullable
             AND c.character_maximum_length IS NOT DISTINCT FROM e.max_length
            """).SingleAsync());

        Assert.Equal(17, await db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value" FROM pg_constraint c
            JOIN pg_namespace n ON n.oid=c.connamespace
            WHERE n.nspname='transport_erp' AND c.conname IN
              ('ck_reg_device_proof_key_bundle','fk_reg_device_proof_changed_by',
               'pk_device_key_challenges','ux_key_challenge_change_scope',
               'fk_key_challenge_registered_device','fk_key_challenge_created_by',
               'ck_key_challenge_type','ck_key_challenge_expected_version',
               'ck_key_challenge_hash_len','ck_key_challenge_window',
               'pk_device_key_changes','fk_key_change_registered_device',
               'fk_key_change_challenge_scope','fk_key_change_changed_by',
               'ck_key_change_type','ck_key_change_version_shape','ck_key_change_recovery_reason')
            """).SingleAsync());

        Assert.Equal(6, await db.Database.SqlQuery<int>($"""
            WITH expected(name,child_table,parent_table,child_columns,parent_columns) AS (VALUES
              ('fk_reg_device_proof_changed_by','registered_devices','users',
               ARRAY['ProofKeyChangedByUserId']::text[],ARRAY['Id']::text[]),
              ('fk_key_challenge_registered_device','registered_device_proof_key_challenges','registered_devices',
               ARRAY['RegisteredDeviceId','CompanyId','DeviceId']::text[],
               ARRAY['Id','CompanyId','DeviceId']::text[]),
              ('fk_key_challenge_created_by','registered_device_proof_key_challenges','users',
               ARRAY['CreatedByUserId']::text[],ARRAY['Id']::text[]),
              ('fk_key_change_registered_device','registered_device_proof_key_changes','registered_devices',
               ARRAY['RegisteredDeviceId','CompanyId','DeviceId']::text[],
               ARRAY['Id','CompanyId','DeviceId']::text[]),
              ('fk_key_change_challenge_scope','registered_device_proof_key_changes',
               'registered_device_proof_key_challenges',
               ARRAY['ChallengeId','CompanyId','RegisteredDeviceId','DeviceId','ChangeRequestId','ChangeType','NewProofKeyThumbprint']::text[],
               ARRAY['Id','CompanyId','RegisteredDeviceId','DeviceId','ChangeRequestId','ChangeType','NewProofKeyThumbprint']::text[]),
              ('fk_key_change_changed_by','registered_device_proof_key_changes','users',
               ARRAY['ChangedByUserId']::text[],ARRAY['Id']::text[])
            )
            SELECT count(*)::int AS "Value" FROM expected e
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

        Assert.Equal(4, await db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value" FROM pg_indexes
            WHERE schemaname='transport_erp' AND indexname IN
              ('ux_registered_device_proof_thumbprint','ux_device_key_challenge_request',
               'ix_device_key_challenge_expiry','ux_device_key_change_request')
            """).SingleAsync());

        Assert.Equal(5, await db.Database.SqlQuery<int>($"""
            WITH expected(name,table_name,function_name,trigger_type) AS (VALUES
              ('trg_reg_device_proof_key_transition','registered_devices',
               'fn_reg_device_proof_key_transition',19::smallint),
              ('trg_key_challenge_user_scope','registered_device_proof_key_challenges',
               'fn_key_challenge_user_scope',23::smallint),
              ('trg_key_challenge_update_guard','registered_device_proof_key_challenges',
               'fn_key_challenge_update_guard',19::smallint),
              ('trg_key_change_insert_guard','registered_device_proof_key_changes',
               'fn_key_change_insert_guard',7::smallint),
              ('trg_device_key_change_append_only','registered_device_proof_key_changes',
               'fn_device_key_change_append_only',27::smallint)
            )
            SELECT count(*)::int AS "Value" FROM expected e
            JOIN pg_trigger t ON t.tgname=e.name AND NOT t.tgisinternal
              AND t.tgenabled='O' AND t.tgtype=e.trigger_type
            JOIN pg_class c ON c.oid=t.tgrelid AND c.relname=e.table_name
            JOIN pg_namespace n ON n.oid=c.relnamespace AND n.nspname='transport_erp'
            JOIN pg_proc p ON p.oid=t.tgfoid AND p.proname=e.function_name
            """).SingleAsync());

        Assert.Equal(5, await db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value" FROM pg_proc p
            JOIN pg_namespace n ON n.oid=p.pronamespace
            WHERE n.nspname='transport_erp' AND p.proname IN
              ('fn_reg_device_proof_key_transition','fn_key_challenge_user_scope',
               'fn_key_challenge_update_guard','fn_key_change_insert_guard','fn_device_key_change_append_only')
            """).SingleAsync());
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Bind_guards_enforce_order_scope_version_and_append_only_ledger()
    {
        await WithFreshDatabaseAsync(async connection =>
        {
            await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
            await db.Database.MigrateAsync();
            var scope = await SeedScopeAsync(db, "BIND");
            var other = await SeedScopeAsync(db, "OTHER");
            var crossTenantChallenge = NewBindChallenge(scope);
            crossTenantChallenge.CreatedByUserId = other.UserId;
            db.RegisteredDeviceProofKeyChallenges.Add(crossTenantChallenge);
            await AssertDbGuardAsync(db, "proof key challenge actor scope mismatch");

            var challenge = NewBindChallenge(scope);
            db.RegisteredDeviceProofKeyChallenges.Add(challenge);
            await db.SaveChangesAsync();

            await AssertPostgresAsync("P0001", () => db.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE transport_erp.registered_device_proof_key_challenges
                   SET "ChallengeHash"=decode(repeat('00',32),'hex') WHERE "Id"={challenge.Id}
                """));
            await AssertPostgresAsync("P0001", () => db.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE transport_erp.registered_device_proof_key_challenges
                   SET "ExpiresAt"="ExpiresAt" + interval '1 minute' WHERE "Id"={challenge.Id}
                """));
            await AssertPostgresAsync("P0001", () => db.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE transport_erp.registered_device_proof_key_challenges
                   SET "ConsumedAt"=clock_timestamp() WHERE "Id"={challenge.Id}
                """));

            var change = NewBindChange(scope, challenge);
            await using (var transaction = await db.Database.BeginTransactionAsync())
            {
                db.RegisteredDeviceProofKeyChanges.Add(change);
                await db.SaveChangesAsync();
                challenge.ConsumedAt = DateTimeOffset.UtcNow;
                var boundDevice = await db.RegisteredDevices.SingleAsync(x => x.Id == scope.RegisteredDeviceId);
                boundDevice.ProofPublicJwkCanonicalJson =
                    "{\"crv\":\"P-256\",\"kty\":\"EC\",\"x\":\"x\",\"y\":\"y\"}";
                boundDevice.ProofKeyThumbprint = Thumbprint;
                boundDevice.ProofKeyVersion = 1;
                boundDevice.ProofKeyChangedAt = DateTimeOffset.UtcNow;
                boundDevice.ProofKeyChangedByUserId = scope.UserId;
                await db.SaveChangesAsync();
                await transaction.CommitAsync();
            }

            await AssertPostgresAsync("P0001", () => db.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE transport_erp.registered_device_proof_key_challenges
                   SET "ConsumedAt"=NULL WHERE "Id"={challenge.Id}
                """));

            await AssertPostgresAsync("P0001", () => db.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE transport_erp.registered_device_proof_key_changes
                   SET "ChangedAt"="ChangedAt" WHERE "Id"={change.Id}
                """));
            await AssertPostgresAsync("P0001", () => db.Database.ExecuteSqlInterpolatedAsync($"""
                DELETE FROM transport_erp.registered_device_proof_key_changes WHERE "Id"={change.Id}
                """));

            db.ChangeTracker.Clear();
            var device = await db.RegisteredDevices.SingleAsync(x => x.Id == scope.RegisteredDeviceId);
            device.ProofKeyVersion = 3;
            await AssertDbGuardAsync(db, "registered device proof key version must increment exactly once");

            db.ChangeTracker.Clear();
            device = await db.RegisteredDevices.SingleAsync(x => x.Id == scope.RegisteredDeviceId);
            device.ProofPublicJwkCanonicalJson = null;
            device.ProofKeyThumbprint = null;
            device.ProofKeyVersion = null;
            device.ProofKeyChangedAt = null;
            device.ProofKeyChangedByUserId = null;
            await AssertDbGuardAsync(db, "registered device proof key cannot be cleared");

        });
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Failed_lifecycle_transaction_leaves_no_partial_challenge_ledger_or_device_key()
    {
        await WithFreshDatabaseAsync(async connection =>
        {
            await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
            await db.Database.MigrateAsync();
            var scope = await SeedScopeAsync(db, "ROLLBACK");
            var challenge = NewBindChallenge(scope);
            var change = NewBindChange(scope, challenge);
            db.RegisteredDeviceProofKeyChallenges.Add(challenge);
            await db.SaveChangesAsync();

            await using (var transaction = await db.Database.BeginTransactionAsync())
            {
                db.RegisteredDeviceProofKeyChanges.Add(change);
                await db.SaveChangesAsync();
                challenge.ConsumedAt = DateTimeOffset.UtcNow;
                var device = await db.RegisteredDevices.SingleAsync(x => x.Id == scope.RegisteredDeviceId);
                device.ProofPublicJwkCanonicalJson = "{}";
                device.ProofKeyThumbprint = Thumbprint;
                device.ProofKeyVersion = 2;
                device.ProofKeyChangedAt = DateTimeOffset.UtcNow;
                device.ProofKeyChangedByUserId = scope.UserId;
                var failure = await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
                Assert.Contains("binding must start at version 1", failure.GetBaseException().Message,
                    StringComparison.Ordinal);
                await transaction.RollbackAsync();
            }

            db.ChangeTracker.Clear();
            var persistedChallenge = await db.RegisteredDeviceProofKeyChallenges
                .SingleAsync(x => x.Id == challenge.Id);
            Assert.Null(persistedChallenge.ConsumedAt);
            Assert.False(await db.RegisteredDeviceProofKeyChanges.AnyAsync(x => x.Id == change.Id));
            Assert.Null(await db.RegisteredDevices.Where(x => x.Id == scope.RegisteredDeviceId)
                .Select(x => x.ProofKeyVersion).SingleAsync());
        });
    }

    [Theory]
    [InlineData("ROTATE", "ACTIVE")]
    [InlineData("RECOVER", "SUSPENDED")]
    [Trait("Category", "PostgreSQL")]
    public async Task Rotate_and_recover_guards_require_current_version_thumbprint_and_allowed_state(
        string changeType, string deviceStatus)
    {
        await WithFreshDatabaseAsync(async connection =>
        {
            await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
            await db.Database.MigrateAsync();
            var scope = await SeedScopeAsync(db, changeType);
            var device = await db.RegisteredDevices.SingleAsync(x => x.Id == scope.RegisteredDeviceId);
            device.Status = deviceStatus;
            device.ProofPublicJwkCanonicalJson = "{\"key\":\"one\"}";
            device.ProofKeyThumbprint = Thumbprint;
            device.ProofKeyVersion = 1;
            device.ProofKeyChangedAt = DateTimeOffset.UtcNow;
            device.ProofKeyChangedByUserId = scope.UserId;
            await db.SaveChangesAsync();

            var challenge = new RegisteredDeviceProofKeyChallenge
            {
                Id = Guid.NewGuid(), CompanyId = scope.CompanyId,
                RegisteredDeviceId = scope.RegisteredDeviceId, DeviceId = scope.DeviceId,
                ChangeRequestId = Guid.NewGuid(), ChangeType = changeType,
                ExpectedProofKeyVersion = 1, NewProofKeyThumbprint = RotatedThumbprint,
                ChallengeHash = RandomNumberGenerator.GetBytes(32), IssuedAt = DateTimeOffset.UtcNow,
                ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(5), CreatedByUserId = scope.UserId
            };
            db.RegisteredDeviceProofKeyChallenges.Add(challenge);
            await db.SaveChangesAsync();

            var change = new RegisteredDeviceProofKeyChange
            {
                Id = Guid.NewGuid(), CompanyId = scope.CompanyId,
                RegisteredDeviceId = scope.RegisteredDeviceId, DeviceId = scope.DeviceId,
                ChangeRequestId = challenge.ChangeRequestId, ChallengeId = challenge.Id,
                ChangeType = changeType, ExpectedProofKeyVersion = 1,
                PreviousProofKeyThumbprint = Thumbprint, NewProofKeyThumbprint = RotatedThumbprint,
                ResultProofKeyVersion = 2, ChangedByUserId = scope.UserId,
                Reason = changeType == "RECOVER" ? "verified recovery" : null,
                ChangedAt = DateTimeOffset.UtcNow
            };
            await using (var transaction = await db.Database.BeginTransactionAsync())
            {
                db.RegisteredDeviceProofKeyChanges.Add(change);
                await db.SaveChangesAsync();
                challenge.ConsumedAt = DateTimeOffset.UtcNow;
                device.ProofPublicJwkCanonicalJson = "{\"key\":\"two\"}";
                device.ProofKeyThumbprint = RotatedThumbprint;
                device.ProofKeyVersion = 2;
                device.ProofKeyChangedAt = DateTimeOffset.UtcNow;
                await db.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            Assert.Equal(2, await db.RegisteredDevices.Where(x => x.Id == scope.RegisteredDeviceId)
                .Select(x => x.ProofKeyVersion).SingleAsync());

            device.Status = "REVOKED";
            await db.SaveChangesAsync();
            var rejected = new RegisteredDeviceProofKeyChallenge
            {
                Id = Guid.NewGuid(), CompanyId = scope.CompanyId,
                RegisteredDeviceId = scope.RegisteredDeviceId, DeviceId = scope.DeviceId,
                ChangeRequestId = Guid.NewGuid(), ChangeType = changeType,
                ExpectedProofKeyVersion = 2,
                NewProofKeyThumbprint = "CCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCCC",
                ChallengeHash = RandomNumberGenerator.GetBytes(32), IssuedAt = DateTimeOffset.UtcNow,
                ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(5), CreatedByUserId = scope.UserId
            };
            db.RegisteredDeviceProofKeyChallenges.Add(rejected);
            await AssertDbGuardAsync(db, "proof key challenge device state mismatch");
        });
    }

    [Theory]
    [InlineData("ROTATE")]
    [InlineData("RECOVER")]
    [Trait("Category", "PostgreSQL")]
    public async Task Rotate_and_recover_reject_current_or_historical_key_material_and_allow_never_used_key(
        string changeType)
    {
        await WithFreshDatabaseAsync(async connection =>
        {
            await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
            await db.Database.MigrateAsync();
            var scope = await SeedScopeAsync(db, $"REUSE-{changeType}");

            var bindChallenge = NewBindChallenge(scope);
            db.RegisteredDeviceProofKeyChallenges.Add(bindChallenge);
            await db.SaveChangesAsync();
            var bindChange = NewBindChange(scope, bindChallenge);
            await using (var bind = await db.Database.BeginTransactionAsync())
            {
                db.RegisteredDeviceProofKeyChanges.Add(bindChange);
                await db.SaveChangesAsync();
                bindChallenge.ConsumedAt = DateTimeOffset.UtcNow;
                var device = await db.RegisteredDevices.SingleAsync(x => x.Id == scope.RegisteredDeviceId);
                device.ProofPublicJwkCanonicalJson = "{\"key\":\"one\"}";
                device.ProofKeyThumbprint = Thumbprint;
                device.ProofKeyVersion = 1;
                device.ProofKeyChangedAt = DateTimeOffset.UtcNow;
                device.ProofKeyChangedByUserId = scope.UserId;
                await db.SaveChangesAsync();
                await bind.CommitAsync();
            }

            var sameCurrent = NewChangeChallenge(scope, changeType, 1, Thumbprint);
            db.RegisteredDeviceProofKeyChallenges.Add(sameCurrent);
            await AssertDbGuardAsync(db, "proof key material reuse denied");

            var valid = NewChangeChallenge(scope, changeType, 1, RotatedThumbprint);
            db.RegisteredDeviceProofKeyChallenges.Add(valid);
            await db.SaveChangesAsync();
            var validChange = NewChange(scope, valid, Thumbprint, 2);
            await using (var rotate = await db.Database.BeginTransactionAsync())
            {
                db.RegisteredDeviceProofKeyChanges.Add(validChange);
                await db.SaveChangesAsync();
                valid.ConsumedAt = DateTimeOffset.UtcNow;
                var device = await db.RegisteredDevices.SingleAsync(x => x.Id == scope.RegisteredDeviceId);
                device.ProofPublicJwkCanonicalJson = "{\"key\":\"two\"}";
                device.ProofKeyThumbprint = RotatedThumbprint;
                device.ProofKeyVersion = 2;
                device.ProofKeyChangedAt = DateTimeOffset.UtcNow;
                await db.SaveChangesAsync();
                await rotate.CommitAsync();
            }

            var historical = NewChangeChallenge(scope, changeType, 2, Thumbprint);
            db.RegisteredDeviceProofKeyChallenges.Add(historical);
            await AssertDbGuardAsync(db, "proof key material reuse denied");

            var neverUsed = NewChangeChallenge(scope, changeType, 2, NeverUsedThumbprint);
            db.RegisteredDeviceProofKeyChallenges.Add(neverUsed);
            await db.SaveChangesAsync();
            Assert.True(await db.RegisteredDeviceProofKeyChallenges.AnyAsync(x => x.Id == neverUsed.Id));
        });
    }

    [Theory]
    [InlineData("ROTATE")]
    [InlineData("RECOVER")]
    [Trait("Category", "PostgreSQL")]
    public async Task Stage5_change_guard_blocks_same_key_challenge_created_before_hardening(string changeType)
    {
        await WithFreshDatabaseAsync(async connection =>
        {
            await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
            var migrator = db.GetService<IMigrator>();
            await migrator.MigrateAsync(BeforeStage5Hardening);
            var scope = await SeedScopeAsync(db, $"LEGACY-REUSE-{changeType}");
            var device = await db.RegisteredDevices.SingleAsync(x => x.Id == scope.RegisteredDeviceId);
            device.ProofPublicJwkCanonicalJson = "{\"key\":\"one\"}";
            device.ProofKeyThumbprint = Thumbprint;
            device.ProofKeyVersion = 1;
            device.ProofKeyChangedAt = DateTimeOffset.UtcNow;
            device.ProofKeyChangedByUserId = scope.UserId;
            await db.SaveChangesAsync();

            var legacyChallenge = NewChangeChallenge(scope, changeType, 1, Thumbprint);
            db.RegisteredDeviceProofKeyChallenges.Add(legacyChallenge);
            await db.SaveChangesAsync();

            await db.Database.MigrateAsync();
            db.RegisteredDeviceProofKeyChanges.Add(NewChange(scope, legacyChallenge, Thumbprint, 2));
            await AssertDbGuardAsync(db, "proof key material reuse denied");
            Assert.Null(await db.RegisteredDeviceProofKeyChallenges.AsNoTracking()
                .Where(x => x.Id == legacyChallenge.Id)
                .Select(x => x.ConsumedAt)
                .SingleAsync());
        });
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Challenge_and_change_requests_are_single_use_and_expired_challenges_fail_closed()
    {
        await WithFreshDatabaseAsync(async connection =>
        {
            await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
            await db.Database.MigrateAsync();
            var scope = await SeedScopeAsync(db, "SINGLE-USE");

            var challenge = NewBindChallenge(scope);
            db.RegisteredDeviceProofKeyChallenges.Add(challenge);
            await db.SaveChangesAsync();

            var duplicateChallenge = NewBindChallenge(scope);
            duplicateChallenge.ChangeRequestId = challenge.ChangeRequestId;
            duplicateChallenge.NewProofKeyThumbprint = RotatedThumbprint;
            db.RegisteredDeviceProofKeyChallenges.Add(duplicateChallenge);
            await AssertUniqueConstraintAsync(db, "ux_device_key_challenge_request");

            var firstChange = NewBindChange(scope, challenge);
            db.RegisteredDeviceProofKeyChanges.Add(firstChange);
            await db.SaveChangesAsync();

            var duplicateChange = NewBindChange(scope, challenge);
            db.RegisteredDeviceProofKeyChanges.Add(duplicateChange);
            await AssertUniqueConstraintAsync(db, "ux_device_key_change_request");

            var expiredChallenge = NewBindChallenge(scope);
            expiredChallenge.IssuedAt = DateTimeOffset.UtcNow.AddMinutes(-10);
            expiredChallenge.ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(-5);
            db.RegisteredDeviceProofKeyChallenges.Add(expiredChallenge);
            await db.SaveChangesAsync();
            db.RegisteredDeviceProofKeyChanges.Add(NewBindChange(scope, expiredChallenge));
            await AssertDbGuardAsync(db, "proof key change challenge mismatch");
        });
    }

    [Theory]
    [InlineData("challenge")]
    [InlineData("change")]
    [Trait("Category", "PostgreSQL")]
    public async Task User_scope_drift_is_blocked_for_each_proof_lifecycle_actor_reference(string shape)
    {
        await WithFreshDatabaseAsync(async connection =>
        {
            await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
            await db.Database.MigrateAsync();
            var scope = await SeedScopeAsync(db, $"DRIFT-{shape}");
            var destination = await SeedScopeAsync(db, $"DEST-{shape}");
            var challenge = NewBindChallenge(scope);
            var actorUserName = $"proof-actor-{Guid.NewGuid():N}";
            var actor = new User
            {
                Id = Guid.NewGuid(), UserName = actorUserName,
                NormalizedUserName = actorUserName.ToUpperInvariant(),
                DisplayName = "Proof key lifecycle actor", PasswordHash = "test-only",
                SecurityStamp = Guid.NewGuid().ToString("N"), AuthVersion = 1, Status = "ACTIVE",
                CompanyId = scope.CompanyId, BranchId = scope.BranchId,
                CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow,
                RowVersion = RandomNumberGenerator.GetBytes(16)
            };
            db.Users.Add(actor);
            await db.SaveChangesAsync();
            if (shape == "challenge")
            {
                challenge.CreatedByUserId = actor.Id;
                db.RegisteredDeviceProofKeyChallenges.Add(challenge);
                await db.SaveChangesAsync();
            }
            else
            {
                db.RegisteredDeviceProofKeyChallenges.Add(challenge);
                await db.SaveChangesAsync();
                var change = NewBindChange(scope, challenge);
                change.ChangedByUserId = actor.Id;
                db.RegisteredDeviceProofKeyChanges.Add(change);
                await db.SaveChangesAsync();
            }

            actor.CompanyId = destination.CompanyId;
            actor.BranchId = destination.BranchId;
            await AssertDbGuardAsync(db, "user scope change would strand tenant-scoped references");
        });
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Migration_roundtrips_up_down_up_when_lifecycle_data_is_absent()
    {
        await WithFreshDatabaseAsync(async connection =>
        {
            await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
            var migrator = db.GetService<IMigrator>();
            await migrator.MigrateAsync(Stage4Foundation);
            Assert.Equal(0, await LifecycleTableCountAsync(db));
            await migrator.MigrateAsync(ProofKeyLifecycle);
            Assert.Equal(2, await LifecycleTableCountAsync(db));
            await migrator.MigrateAsync(Stage4Foundation);
            Assert.Equal(0, await LifecycleTableCountAsync(db));
            Assert.Equal(0, await ProofColumnCountAsync(db));
            Assert.Equal(0, await ProofLifecycleFunctionCountAsync(db));
            var restoredUserScopeFunctions = await UserScopeFunctionDefinitionsAsync(db);
            Assert.DoesNotContain("ProofKeyChangedByUserId", restoredUserScopeFunctions,
                StringComparison.Ordinal);
            Assert.DoesNotContain("registered_device_proof_key_challenges", restoredUserScopeFunctions,
                StringComparison.Ordinal);
            Assert.DoesNotContain("registered_device_proof_key_changes", restoredUserScopeFunctions,
                StringComparison.Ordinal);
            await migrator.MigrateAsync(ProofKeyLifecycle);
            Assert.Equal(2, await LifecycleTableCountAsync(db));
            Assert.Equal(5, await ProofColumnCountAsync(db));
            Assert.Equal(5, await ProofLifecycleFunctionCountAsync(db));
        });
    }

    [Theory]
    [InlineData("bound-key")]
    [InlineData("challenge")]
    [InlineData("change")]
    [Trait("Category", "PostgreSQL")]
    public async Task Down_is_fail_closed_without_partial_ddl_for_each_lifecycle_shape(string shape)
    {
        await WithFreshDatabaseAsync(async connection =>
        {
            await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
            await db.Database.MigrateAsync();
            var scope = await SeedScopeAsync(db, $"DOWN-{shape}");
            if (shape == "bound-key")
            {
                var device = await db.RegisteredDevices.SingleAsync(x => x.Id == scope.RegisteredDeviceId);
                device.ProofPublicJwkCanonicalJson = "{}";
                device.ProofKeyThumbprint = Thumbprint;
                device.ProofKeyVersion = 1;
                device.ProofKeyChangedAt = DateTimeOffset.UtcNow;
                device.ProofKeyChangedByUserId = scope.UserId;
                await db.SaveChangesAsync();
            }
            else
            {
                var challenge = NewBindChallenge(scope);
                db.RegisteredDeviceProofKeyChallenges.Add(challenge);
                await db.SaveChangesAsync();
                if (shape == "change")
                {
                    db.RegisteredDeviceProofKeyChanges.Add(NewBindChange(scope, challenge));
                    await db.SaveChangesAsync();
                }
            }

            var migrator = db.GetService<IMigrator>();
            var failure = await Assert.ThrowsAnyAsync<Exception>(() => migrator.MigrateAsync(Stage4Foundation));
            Assert.Contains("STAGE4_DOWN_BLOCKED_DATA_PRESENT", failure.GetBaseException().Message,
                StringComparison.Ordinal);
            Assert.Equal(2, await LifecycleTableCountAsync(db));
            Assert.Equal(5, await ProofColumnCountAsync(db));
            Assert.Equal(1, await db.Database.SqlQuery<int>($"""
                SELECT count(*)::int AS "Value" FROM transport_erp."__EFMigrationsHistory"
                WHERE "MigrationId"={ProofKeyLifecycle}
                """).SingleAsync());
        });
    }

    private static async Task<ProofScope> SeedScopeAsync(TransportErpDbContext db, string suffix)
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
            Id = Guid.NewGuid(), Code = $"PK-{suffix}-{Guid.NewGuid():N}"[..18], LegalNameAr = "شركة proof key",
            BaseCurrencyId = currency.Id, DefaultCalendarId = Guid.NewGuid(), Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var branch = new Branch
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, Code = $"B-{Guid.NewGuid():N}"[..12], NameAr = "فرع",
            Timezone = "Asia/Riyadh", Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var userName = $"pk-{Guid.NewGuid():N}";
        var user = new User
        {
            Id = Guid.NewGuid(), UserName = userName, NormalizedUserName = userName.ToUpperInvariant(),
            DisplayName = "Proof key operator", PasswordHash = "test-only",
            SecurityStamp = Guid.NewGuid().ToString("N"), AuthVersion = 1, Status = "ACTIVE",
            CompanyId = company.Id, BranchId = branch.Id, CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        db.AddRange(currency, company, branch, user);
        await db.SaveChangesAsync();
        var device = new RegisteredDevice
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, DeviceId = $"proof-{Guid.NewGuid():N}",
            DisplayName = "Proof device", Platform = "TEST", AppVersion = "1",
            RegistrationRequestId = $"request-{Guid.NewGuid():N}", CredentialHash = new string('d', 64),
            CredentialVersion = 1, Status = "ACTIVE", RegisteredByUserId = user.Id,
            ApprovedByUserId = user.Id, ApprovedAt = now, LastSeenAt = now,
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        db.RegisteredDevices.Add(device);
        await db.SaveChangesAsync();
        return new(company.Id, branch.Id, user.Id, device.Id, device.DeviceId);
    }

    private static RegisteredDeviceProofKeyChallenge NewBindChallenge(ProofScope scope) => new()
    {
        Id = Guid.NewGuid(), CompanyId = scope.CompanyId, RegisteredDeviceId = scope.RegisteredDeviceId,
        DeviceId = scope.DeviceId, ChangeRequestId = Guid.NewGuid(), ChangeType = "BIND",
        ExpectedProofKeyVersion = null, NewProofKeyThumbprint = Thumbprint,
        ChallengeHash = RandomNumberGenerator.GetBytes(32), IssuedAt = DateTimeOffset.UtcNow,
        ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(5), CreatedByUserId = scope.UserId
    };

    private static RegisteredDeviceProofKeyChange NewBindChange(
        ProofScope scope, RegisteredDeviceProofKeyChallenge challenge) => new()
    {
        Id = Guid.NewGuid(), CompanyId = scope.CompanyId, RegisteredDeviceId = scope.RegisteredDeviceId,
        DeviceId = scope.DeviceId, ChangeRequestId = challenge.ChangeRequestId, ChallengeId = challenge.Id,
        ChangeType = "BIND", ExpectedProofKeyVersion = null, PreviousProofKeyThumbprint = null,
        NewProofKeyThumbprint = challenge.NewProofKeyThumbprint, ResultProofKeyVersion = 1,
        ChangedByUserId = scope.UserId, ChangedAt = DateTimeOffset.UtcNow
    };

    private static RegisteredDeviceProofKeyChallenge NewChangeChallenge(
        ProofScope scope, string changeType, int expectedVersion, string newThumbprint) => new()
    {
        Id = Guid.NewGuid(), CompanyId = scope.CompanyId, RegisteredDeviceId = scope.RegisteredDeviceId,
        DeviceId = scope.DeviceId, ChangeRequestId = Guid.NewGuid(), ChangeType = changeType,
        ExpectedProofKeyVersion = expectedVersion, NewProofKeyThumbprint = newThumbprint,
        ChallengeHash = RandomNumberGenerator.GetBytes(32), IssuedAt = DateTimeOffset.UtcNow,
        ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(5), CreatedByUserId = scope.UserId
    };

    private static RegisteredDeviceProofKeyChange NewChange(
        ProofScope scope, RegisteredDeviceProofKeyChallenge challenge,
        string previousThumbprint, int resultVersion) => new()
    {
        Id = Guid.NewGuid(), CompanyId = scope.CompanyId, RegisteredDeviceId = scope.RegisteredDeviceId,
        DeviceId = scope.DeviceId, ChangeRequestId = challenge.ChangeRequestId, ChallengeId = challenge.Id,
        ChangeType = challenge.ChangeType, ExpectedProofKeyVersion = challenge.ExpectedProofKeyVersion,
        PreviousProofKeyThumbprint = previousThumbprint,
        NewProofKeyThumbprint = challenge.NewProofKeyThumbprint,
        ResultProofKeyVersion = resultVersion, ChangedByUserId = scope.UserId,
        Reason = challenge.ChangeType == "RECOVER" ? "verified recovery" : null,
        ChangedAt = DateTimeOffset.UtcNow
    };

    private static async Task AssertDbGuardAsync(TransportErpDbContext db, string message)
    {
        var failure = await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
        Assert.Contains(message, failure.GetBaseException().Message, StringComparison.Ordinal);
        db.ChangeTracker.Clear();
    }

    private static async Task AssertPostgresAsync(string sqlState, Func<Task> action)
    {
        var failure = await Assert.ThrowsAnyAsync<Exception>(action);
        Assert.Equal(sqlState, (failure.GetBaseException() as Npgsql.PostgresException)?.SqlState);
    }

    private static async Task AssertUniqueConstraintAsync(TransportErpDbContext db, string constraintName)
    {
        var failure = await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
        var detail = Assert.IsType<Npgsql.PostgresException>(failure.GetBaseException());
        Assert.Equal("23505", detail.SqlState);
        Assert.Equal(constraintName, detail.ConstraintName);
        db.ChangeTracker.Clear();
    }

    private static Task<int> LifecycleTableCountAsync(TransportErpDbContext db) =>
        db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value" FROM information_schema.tables
            WHERE table_schema='transport_erp' AND table_name IN
              ('registered_device_proof_key_challenges','registered_device_proof_key_changes')
            """).SingleAsync();

    private static Task<int> ProofColumnCountAsync(TransportErpDbContext db) =>
        db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value" FROM information_schema.columns
            WHERE table_schema='transport_erp' AND table_name='registered_devices' AND column_name IN
              ('ProofPublicJwkCanonicalJson','ProofKeyThumbprint','ProofKeyVersion',
               'ProofKeyChangedAt','ProofKeyChangedByUserId')
            """).SingleAsync();

    private static Task<int> ProofLifecycleFunctionCountAsync(TransportErpDbContext db) =>
        db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value" FROM pg_proc p
            JOIN pg_namespace n ON n.oid=p.pronamespace
            WHERE n.nspname='transport_erp' AND p.proname IN
              ('fn_reg_device_proof_key_transition','fn_key_challenge_user_scope',
               'fn_key_challenge_update_guard','fn_key_change_insert_guard','fn_device_key_change_append_only')
            """).SingleAsync();

    private static Task<string> UserScopeFunctionDefinitionsAsync(TransportErpDbContext db) =>
        db.Database.SqlQuery<string>($"""
            SELECT string_agg(pg_get_functiondef(p.oid), E'\n---\n' ORDER BY p.proname) AS "Value"
            FROM pg_proc p JOIN pg_namespace n ON n.oid=p.pronamespace
            WHERE n.nspname='transport_erp' AND p.proname IN
              ('enforce_registered_device_user_scope','prevent_user_scope_reference_drift')
            """).SingleAsync();

    private static async Task WithFreshDatabaseAsync(Func<string, Task> test)
    {
        var baseConnection = PostgreSqlTestEnvironment.RequireConnection();
        var database = $"transporterp_stage4_key_{Guid.NewGuid():N}";
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

    private sealed record ProofScope(
        Guid CompanyId, Guid BranchId, Guid UserId, Guid RegisteredDeviceId, string DeviceId);
}
