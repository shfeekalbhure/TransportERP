using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

[Collection("PostgreSql")]
public sealed class Stage4SyncRuntimePostgreSqlTests
{
    private const string Htu = "https://sync.example.test/api/v1/sync/operations:batch";

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Nonce_claim_and_business_replay_are_atomic_and_idempotent()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "FLOW");
        var proofRuntime = new SyncProofRuntimeService(db, new AuditEventService(db));

        var firstNonce = await proofRuntime.IssueNonceAsync(scope.Security);
        var secondNonce = await proofRuntime.IssueNonceAsync(scope.Security);
        var jti = Guid.NewGuid().ToString("D");
        // Issuing a newer nonce does not invalidate the still-live previous nonce.
        var firstClaim = await proofRuntime.ClaimAsync(scope.Security,
            Proof(firstNonce.Value, jti, scope.Thumbprint));
        var operationService = CreateOperationService(db);
        var command = Command("client-" + Guid.NewGuid().ToString("N"), "{\"amount\":10}");
        var first = await operationService.EnqueueAcceptedSyncOperationAsync(command, firstClaim);

        var secondClaim = await proofRuntime.ClaimAsync(scope.Security,
            Proof(secondNonce.Value, Guid.NewGuid().ToString("D"), scope.Thumbprint));
        var replay = await operationService.EnqueueAcceptedSyncOperationAsync(command, secondClaim);

        Assert.Equal(first.Id, replay.Id);
        Assert.Equal(first.OperationCorrelationId, replay.OperationCorrelationId);
        Assert.Single(await db.SyncOperations.Where(x => x.ClientOperationId == command.ClientOperationId).ToListAsync());
        Assert.Single(await db.AuditEvents.Where(x => x.Action == "SyncOperationQueued" &&
            x.OperationCorrelationId == command.OperationCorrelationId).ToListAsync());
        Assert.Equal(2, await db.SyncProofReplays.CountAsync(x => x.RegisteredDeviceId == scope.DeviceId));
        var nonceBytes = DecodeBase64Url(firstNonce.Value);
        var persistedHashes = await db.SyncProofNonces.Where(x => x.RegisteredDeviceId == scope.DeviceId)
            .Select(x => x.NonceHash).ToListAsync();
        var expectedHash = SHA256.HashData(nonceBytes);
        Assert.Contains(persistedHashes, value => value.SequenceEqual(expectedHash));

        var changed = command with
        {
            PayloadJson = "{\"amount\":11}",
            PayloadHash = Hash("{\"amount\":11}")
        };
        var mismatch = await Assert.ThrowsAsync<SyncRuleException>(() =>
            operationService.EnqueueAcceptedSyncOperationAsync(changed, secondClaim));
        Assert.Equal("IDEMPOTENCY_MISMATCH", mismatch.Code);

        var duplicateProof = await Assert.ThrowsAsync<SyncProofRuntimeException>(() =>
            proofRuntime.ClaimAsync(scope.Security, Proof(firstNonce.Value, jti, scope.Thumbprint)));
        Assert.Equal("invalid_dpop_proof", duplicateProof.Code);
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Concurrent_claim_across_contexts_accepts_exactly_one_jti()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using (var setup = PostgreSqlTestEnvironment.CreateDbContext(connection))
        {
            await setup.Database.MigrateAsync();
            var seeded = await SeedAsync(setup, "RACE");
            var issuer = new SyncProofRuntimeService(setup, new AuditEventService(setup));
            var nonce = await issuer.IssueNonceAsync(seeded.Security);
            var proof = Proof(nonce.Value, Guid.NewGuid().ToString("D"), seeded.Thumbprint);

            await using var firstDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
            await using var secondDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
            var firstRuntime = new SyncProofRuntimeService(firstDb, new AuditEventService(firstDb));
            var secondRuntime = new SyncProofRuntimeService(secondDb, new AuditEventService(secondDb));
            var first = Record.ExceptionAsync(() => firstRuntime.ClaimAsync(seeded.Security, proof));
            var second = Record.ExceptionAsync(() => secondRuntime.ClaimAsync(seeded.Security, proof));
            var outcomes = await Task.WhenAll(first, second);

            Assert.Single(outcomes, x => x is null);
            var rejected = Assert.Single(outcomes, x => x is not null);
            Assert.Equal("invalid_dpop_proof", Assert.IsType<SyncProofRuntimeException>(rejected).Code);
        }
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Concurrent_business_replay_converges_and_same_client_key_is_isolated_by_tenant()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        TestScope firstScope;
        AcceptedSyncProofContext firstProof;
        AcceptedSyncProofContext secondProof;
        await using (var setup = PostgreSqlTestEnvironment.CreateDbContext(connection))
        {
            await setup.Database.MigrateAsync();
            firstScope = await SeedAsync(setup, "BUSINESS-RACE");
            var runtime = new SyncProofRuntimeService(setup, new AuditEventService(setup));
            var firstNonce = await runtime.IssueNonceAsync(firstScope.Security);
            firstProof = await runtime.ClaimAsync(firstScope.Security,
                Proof(firstNonce.Value, Guid.NewGuid().ToString("D"), firstScope.Thumbprint));
            var secondNonce = await runtime.IssueNonceAsync(firstScope.Security);
            secondProof = await runtime.ClaimAsync(firstScope.Security,
                Proof(secondNonce.Value, Guid.NewGuid().ToString("D"), firstScope.Thumbprint));
        }

        var clientOperationId = "shared-client-" + Guid.NewGuid().ToString("N");
        var command = Command(clientOperationId, "{\"shared\":true}");
        await using (var firstDb = PostgreSqlTestEnvironment.CreateDbContext(connection))
        await using (var secondDb = PostgreSqlTestEnvironment.CreateDbContext(connection))
        {
            var firstTask = CreateOperationService(firstDb).EnqueueAcceptedSyncOperationAsync(command, firstProof);
            var secondTask = CreateOperationService(secondDb).EnqueueAcceptedSyncOperationAsync(command, secondProof);
            var results = await Task.WhenAll(firstTask, secondTask);
            Assert.Equal(results[0].Id, results[1].Id);
        }

        await using (var verify = PostgreSqlTestEnvironment.CreateDbContext(connection))
        {
            Assert.Single(await verify.SyncOperations.Where(x => x.CompanyId == firstScope.Security.CompanyId &&
                x.ClientOperationId == clientOperationId).ToListAsync());
            Assert.Single(await verify.AuditEvents.Where(x => x.CompanyId == firstScope.Security.CompanyId &&
                x.Action == "SyncOperationQueued" && x.OperationCorrelationId == command.OperationCorrelationId)
                .ToListAsync());
        }

        await using (var otherTenantDb = PostgreSqlTestEnvironment.CreateDbContext(connection))
        {
            var otherScope = await SeedAsync(otherTenantDb, "BUSINESS-OTHER-TENANT");
            var runtime = new SyncProofRuntimeService(otherTenantDb, new AuditEventService(otherTenantDb));
            var nonce = await runtime.IssueNonceAsync(otherScope.Security);
            var proof = await runtime.ClaimAsync(otherScope.Security,
                Proof(nonce.Value, Guid.NewGuid().ToString("D"), otherScope.Thumbprint));
            var other = await CreateOperationService(otherTenantDb)
                .EnqueueAcceptedSyncOperationAsync(command, proof);
            Assert.Equal(otherScope.Security.CompanyId, other.CompanyId);
            Assert.NotEqual(firstScope.Security.CompanyId, other.CompanyId);
        }
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Nonce_and_claim_are_tenant_scoped_and_expiry_is_rechecked_under_lock()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "SCOPE");
        var runtime = new SyncProofRuntimeService(db, new AuditEventService(db));
        var before = await db.SyncProofNonces.CountAsync();

        var tenantError = await Assert.ThrowsAsync<SyncProofRuntimeException>(() =>
            runtime.IssueNonceAsync(scope.Security with { CompanyId = Guid.NewGuid() }));
        Assert.Equal("DEVICE_NOT_REGISTERED", tenantError.Code);
        Assert.Equal(before, await db.SyncProofNonces.CountAsync());

        var nonce = await runtime.IssueNonceAsync(scope.Security);
        var wrongThumbprint = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
            .TrimEnd('=').Replace('+', '-').Replace('/', '_');
        var thumbprintError = await Assert.ThrowsAsync<SyncProofRuntimeException>(() => runtime.ClaimAsync(
            scope.Security, Proof(nonce.Value, Guid.NewGuid().ToString("D"), wrongThumbprint)));
        Assert.Equal("invalid_dpop_proof", thumbprintError.Code);
        Assert.False(await db.SyncProofReplays.AnyAsync(x => x.RegisteredDeviceId == scope.DeviceId));

        var row = await db.SyncProofNonces.SingleAsync(x => x.RegisteredDeviceId == scope.DeviceId);
        row.IssuedAt = DateTimeOffset.UtcNow.AddMinutes(-6);
        row.ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(-1);
        await db.SaveChangesAsync();
        var expired = await Assert.ThrowsAsync<SyncProofRuntimeException>(() => runtime.ClaimAsync(
            scope.Security, Proof(nonce.Value, Guid.NewGuid().ToString("D"), scope.Thumbprint)));
        Assert.Equal("use_dpop_nonce", expired.Code);
        Assert.False(await db.SyncProofReplays.AnyAsync(x => x.RegisteredDeviceId == scope.DeviceId));
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Claim_rechecks_device_assignment_expiry_and_rotated_key_state()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();

        async Task AssertRejectedAsync(
            string suffix,
            Func<TestScope, Task> mutate,
            string expectedCode)
        {
            var scope = await SeedAsync(db, suffix);
            var runtime = new SyncProofRuntimeService(db, new AuditEventService(db));
            var nonce = await runtime.IssueNonceAsync(scope.Security);
            await mutate(scope);
            db.ChangeTracker.Clear();

            var error = await Assert.ThrowsAsync<SyncProofRuntimeException>(() => runtime.ClaimAsync(
                scope.Security, Proof(nonce.Value, Guid.NewGuid().ToString("D"), scope.Thumbprint)));

            Assert.Equal(expectedCode, error.Code);
            Assert.False(await db.SyncProofReplays.AnyAsync(x => x.RegisteredDeviceId == scope.DeviceId));
        }

        await AssertRejectedAsync("CLAIM-DEVICE-SUSPENDED", async scope =>
        {
            await db.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE transport_erp.registered_devices SET "Status"='SUSPENDED'
                WHERE "Id"={scope.DeviceId}
                """);
        }, "DEVICE_NOT_REGISTERED");

        await AssertRejectedAsync("CLAIM-ASSIGNMENT-REVOKED", async scope =>
        {
            await db.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE transport_erp.registered_device_assignments SET "Status"='REVOKED'
                WHERE "RegisteredDeviceId"={scope.DeviceId}
                """);
        }, "DEVICE_NOT_REGISTERED");

        await AssertRejectedAsync("CLAIM-DEVICE-EXPIRED", async scope =>
        {
            await db.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE transport_erp.registered_devices SET "ExpiresAt"={DateTimeOffset.UtcNow.AddMinutes(-1)}
                WHERE "Id"={scope.DeviceId}
                """);
        }, "DEVICE_NOT_REGISTERED");

        await AssertRejectedAsync("CLAIM-KEY-ROTATED", async scope =>
        {
            var rotatedThumbprint = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
                .TrimEnd('=').Replace('+', '-').Replace('/', '_');
            await db.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE transport_erp.registered_devices
                SET "ProofKeyVersion"=2, "ProofKeyThumbprint"={rotatedThumbprint}
                WHERE "Id"={scope.DeviceId}
                """);
        }, "invalid_dpop_proof");
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Third_nonce_hash_collision_is_mapped_without_leaking_db_exception()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "NONCE-COLLISION");
        var runtime = new SyncProofRuntimeService(db, new AuditEventService(db));
        _ = await runtime.IssueNonceAsync(scope.Security);
        var suffix = Guid.NewGuid().ToString("N");
        var function = $"force_nonce_collision_{suffix}";
        var trigger = $"trg_force_nonce_collision_{suffix}";
        await db.Database.ExecuteSqlRawAsync($$"""
            CREATE FUNCTION transport_erp.{{function}}() RETURNS trigger LANGUAGE plpgsql AS $body$
            BEGIN
              SELECT "NonceHash" INTO NEW."NonceHash" FROM transport_erp.sync_proof_nonces ORDER BY "Id" LIMIT 1;
              RETURN NEW;
            END $body$;
            CREATE TRIGGER {{trigger}} BEFORE INSERT ON transport_erp.sync_proof_nonces
              FOR EACH ROW EXECUTE FUNCTION transport_erp.{{function}}();
            """);
        try
        {
            var error = await Assert.ThrowsAsync<SyncProofRuntimeException>(() =>
                runtime.IssueNonceAsync(scope.Security));
            Assert.Equal("NONCE_GENERATION_FAILED", error.Code);
        }
        finally
        {
            await using var cleanup = PostgreSqlTestEnvironment.CreateDbContext(connection);
            await cleanup.Database.ExecuteSqlRawAsync($$"""
                DROP TRIGGER IF EXISTS {{trigger}} ON transport_erp.sync_proof_nonces;
                DROP FUNCTION IF EXISTS transport_erp.{{function}}();
                """);
        }
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Exact_legacy_unique_constraint_is_mapped_to_legacy_idempotency_conflict()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "LEGACY-MAP");
        var proofRuntime = new SyncProofRuntimeService(db, new AuditEventService(db));
        var nonce = await proofRuntime.IssueNonceAsync(scope.Security);
        var accepted = await proofRuntime.ClaimAsync(scope.Security,
            Proof(nonce.Value, Guid.NewGuid().ToString("D"), scope.Thumbprint));
        var suffix = Guid.NewGuid().ToString("N");
        var function = $"force_legacy_unique_{suffix}";
        var trigger = $"trg_force_legacy_unique_{suffix}";
        await db.Database.ExecuteSqlRawAsync($$"""
            CREATE FUNCTION transport_erp.{{function}}() RETURNS trigger LANGUAGE plpgsql AS $body$
            BEGIN
              RAISE unique_violation USING CONSTRAINT='ux_sync_op_legacy_company_device_client';
            END $body$;
            CREATE TRIGGER {{trigger}} BEFORE INSERT ON transport_erp.sync_operations
              FOR EACH ROW EXECUTE FUNCTION transport_erp.{{function}}();
            """);
        try
        {
            var error = await Assert.ThrowsAsync<SyncRuleException>(() => CreateOperationService(db)
                .EnqueueAcceptedSyncOperationAsync(Command("client-" + Guid.NewGuid().ToString("N"), "{}"), accepted));
            Assert.Equal("LEGACY_IDEMPOTENCY_CONFLICT", error.Code);
        }
        finally
        {
            await using var cleanup = PostgreSqlTestEnvironment.CreateDbContext(connection);
            await cleanup.Database.ExecuteSqlRawAsync($$"""
                DROP TRIGGER IF EXISTS {{trigger}} ON transport_erp.sync_operations;
                DROP FUNCTION IF EXISTS transport_erp.{{function}}();
                """);
        }

        var unknownFunction = $"force_unknown_unique_{suffix}";
        var unknownTrigger = $"trg_force_unknown_unique_{suffix}";
        await using var unknownDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await unknownDb.Database.ExecuteSqlRawAsync($$"""
            CREATE FUNCTION transport_erp.{{unknownFunction}}() RETURNS trigger LANGUAGE plpgsql AS $body$
            BEGIN
              RAISE unique_violation USING CONSTRAINT='ux_unknown_sync_test';
            END $body$;
            CREATE TRIGGER {{unknownTrigger}} BEFORE INSERT ON transport_erp.sync_operations
              FOR EACH ROW EXECUTE FUNCTION transport_erp.{{unknownFunction}}();
            """);
        try
        {
            var unknown = await Assert.ThrowsAsync<DbUpdateException>(() => CreateOperationService(unknownDb)
                .EnqueueAcceptedSyncOperationAsync(Command("client-" + Guid.NewGuid().ToString("N"), "{}"), accepted));
            Assert.DoesNotContain("LEGACY_IDEMPOTENCY_CONFLICT", unknown.ToString(), StringComparison.Ordinal);
        }
        finally
        {
            await using var cleanup = PostgreSqlTestEnvironment.CreateDbContext(connection);
            await cleanup.Database.ExecuteSqlRawAsync($$"""
                DROP TRIGGER IF EXISTS {{unknownTrigger}} ON transport_erp.sync_operations;
                DROP FUNCTION IF EXISTS transport_erp.{{unknownFunction}}();
                """);
        }
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Cleanup_deletes_expired_replays_before_unreferenced_nonces_and_never_deletes_early()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "CLEANUP");
        var now = Normalize(DateTimeOffset.UtcNow);
        var assignmentId = await db.RegisteredDeviceAssignments
            .Where(x => x.RegisteredDeviceId == scope.DeviceId && x.Status == "ACTIVE")
            .Select(x => x.Id).SingleAsync();
        var expiredReferenced = Nonce(scope, now.AddMinutes(-20), now.AddMinutes(-15));
        var expiredStillReferenced = Nonce(scope, now.AddMinutes(-6), now.AddMinutes(-1));
        var futureUnreferenced = Nonce(scope, now.AddMinutes(-4), now.AddMinutes(1));
        var expiredUnreferenced = Nonce(scope, now.AddMinutes(-6), now.AddMinutes(-1));
        var shortExpiryReferenced = Nonce(scope, now.AddMinutes(-6), now.AddMinutes(-1));
        db.SyncProofNonces.AddRange(expiredReferenced, expiredStillReferenced,
            futureUnreferenced, expiredUnreferenced, shortExpiryReferenced);
        await db.SaveChangesAsync();
        var boundaryReplay = Replay(scope, assignmentId, expiredReferenced.Id,
            now.AddMinutes(-10), now);
        var futureReplay = Replay(scope, assignmentId, expiredStillReferenced.Id,
            now.AddMinutes(-9), now.AddMinutes(1));
        var malformedShortReplay = Replay(scope, assignmentId, shortExpiryReferenced.Id,
            now.AddMinutes(-5), now.AddMinutes(-1));
        db.SyncProofReplays.AddRange(boundaryReplay, futureReplay, malformedShortReplay);
        await db.SaveChangesAsync();

        var cleanup = new SyncProofCleanupService(db);
        var first = await cleanup.CleanupExpiredAsync(now);

        Assert.Equal(1, first.DeletedReplays);
        Assert.Equal(2, first.DeletedNonces);
        Assert.False(await db.SyncProofReplays.AnyAsync(x => x.Id == boundaryReplay.Id));
        Assert.False(await db.SyncProofNonces.AnyAsync(x => x.Id == expiredReferenced.Id));
        Assert.False(await db.SyncProofNonces.AnyAsync(x => x.Id == expiredUnreferenced.Id));
        Assert.True(await db.SyncProofReplays.AnyAsync(x => x.Id == futureReplay.Id));
        Assert.True(await db.SyncProofReplays.AnyAsync(x => x.Id == malformedShortReplay.Id));
        Assert.True(await db.SyncProofNonces.AnyAsync(x => x.Id == expiredStillReferenced.Id));
        Assert.True(await db.SyncProofNonces.AnyAsync(x => x.Id == shortExpiryReferenced.Id));
        Assert.True(await db.SyncProofNonces.AnyAsync(x => x.Id == futureUnreferenced.Id));

        var second = await cleanup.CleanupExpiredAsync(now.AddMinutes(2));
        Assert.Equal(1, second.DeletedReplays);
        Assert.Equal(2, second.DeletedNonces);
        Assert.False(await db.SyncProofReplays.AnyAsync(x => x.Id == futureReplay.Id));
        Assert.True(await db.SyncProofReplays.AnyAsync(x => x.Id == malformedShortReplay.Id));
        Assert.True(await db.SyncProofNonces.AnyAsync(x => x.Id == shortExpiryReferenced.Id));

        var third = await cleanup.CleanupExpiredAsync(now.AddMinutes(6));
        Assert.Equal(1, third.DeletedReplays);
        Assert.Equal(1, third.DeletedNonces);
        Assert.False(await db.SyncProofReplays.AnyAsync(x => x.Id == malformedShortReplay.Id));
        Assert.False(await db.SyncProofNonces.AnyAsync(x => x.RegisteredDeviceId == scope.DeviceId));
    }

    private static SyncOperationService CreateOperationService(TransportErpDbContext db)
        => new(db, new AuditEventService(db), new SyncRetryPolicy(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30)));

    private static EnqueueAcceptedSyncOperationCommand Command(string clientOperationId, string payload)
        => new("sync-v1", "CreateJournalEntry", "CREATE", "JournalEntry", null,
            clientOperationId, payload, Hash(payload), DateTimeOffset.UtcNow,
            Guid.NewGuid(), null);

    private static VerifiedSyncProofMaterial Proof(string nonce, string jti, string thumbprint)
        => new(jti, nonce, thumbprint, DateTimeOffset.UtcNow, Htu, Guid.NewGuid());

    private static string Hash(string payload)
        => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(payload))).ToLowerInvariant();

    private static byte[] DecodeBase64Url(string value)
    {
        var padded = value.Replace('-', '+').Replace('_', '/');
        padded += new string('=', (4 - padded.Length % 4) % 4);
        return Convert.FromBase64String(padded);
    }

    private static SyncProofNonce Nonce(TestScope scope, DateTimeOffset issuedAt, DateTimeOffset expiresAt)
        => new()
        {
            Id = Guid.NewGuid(), CompanyId = scope.Security.CompanyId, RegisteredDeviceId = scope.DeviceId,
            DeviceId = scope.Security.DeviceId, ProofKeyVersion = 1,
            NonceHash = RandomNumberGenerator.GetBytes(32), IssuedAt = issuedAt, ExpiresAt = expiresAt
        };

    private static SyncProofReplay Replay(TestScope scope, Guid assignmentId, Guid nonceId,
        DateTimeOffset firstSeenAt, DateTimeOffset expiresAt)
        => new()
        {
            Id = Guid.NewGuid(), CompanyId = scope.Security.CompanyId, RegisteredDeviceId = scope.DeviceId,
            DeviceId = scope.Security.DeviceId, DeviceAssignmentId = assignmentId,
            UserId = scope.Security.UserId, BranchId = scope.Security.BranchId,
            ProofKeyVersion = 1, ProofKeyThumbprint = scope.Thumbprint,
            JtiHash = RandomNumberGenerator.GetBytes(32), HtuHash = RandomNumberGenerator.GetBytes(32),
            HttpMethod = "POST", NonceRecordId = nonceId, IssuedAt = firstSeenAt,
            FirstSeenAt = firstSeenAt, ExpiresAt = expiresAt, AttemptCorrelationId = Guid.NewGuid()
        };

    private static DateTimeOffset Normalize(DateTimeOffset value)
    {
        var ticks = value.UtcDateTime.Ticks;
        return new DateTimeOffset(new DateTime(
            ticks - ticks % TimeSpan.TicksPerMicrosecond, DateTimeKind.Utc));
    }

    private static async Task<TestScope> SeedAsync(TransportErpDbContext db, string suffix)
    {
        var now = DateTimeOffset.UtcNow;
        var currency = new Currency
        {
            Id = Guid.NewGuid(), Code = await PostgreSqlTestCurrencyCodeAllocator.NextAsync(db),
            NameAr = "عملة Stage4", MinorUnit = 2, IsBase = true, CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var company = new Company
        {
            Id = Guid.NewGuid(), Code = $"R-{suffix}-{Guid.NewGuid():N}"[..18], LegalNameAr = "شركة Stage4",
            BaseCurrencyId = currency.Id, DefaultCalendarId = Guid.NewGuid(), Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var branch = new Branch
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, Code = "MAIN", NameAr = "الفرع الرئيسي",
            Timezone = "Asia/Aden", Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var user = new User
        {
            Id = Guid.NewGuid(), UserName = $"runtime-{Guid.NewGuid():N}",
            NormalizedUserName = $"RUNTIME-{Guid.NewGuid():N}", DisplayName = "مستخدم Runtime",
            PasswordHash = "test-only", SecurityStamp = Guid.NewGuid().ToString("N"), AuthVersion = 1,
            Status = "ACTIVE", CompanyId = company.Id, BranchId = branch.Id,
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var deviceIdText = $"runtime-{suffix}-{Guid.NewGuid():N}";
        var thumbprint = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
            .TrimEnd('=').Replace('+', '-').Replace('/', '_');
        var device = new RegisteredDevice
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, DeviceId = deviceIdText, DisplayName = "جهاز Runtime",
            Platform = "TEST", AppVersion = "1.0", RegistrationRequestId = "req-" + Guid.NewGuid().ToString("N"),
            CredentialHash = new string('a', 64), CredentialVersion = 1, Status = "ACTIVE",
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
        return new TestScope(device.Id, thumbprint,
            new SyncProofSecurityContext(user.Id, company.Id, branch.Id, device.Id, deviceIdText));
    }

    private sealed record TestScope(Guid DeviceId, string Thumbprint, SyncProofSecurityContext Security);
}
