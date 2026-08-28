using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TransportERP.Application.Sync;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

[Collection("PostgreSql")]
public sealed class Stage4G4ServerClosurePostgreSqlTests
{
    private const string Htu = "https://sync.example.test/api/v1/sync/operations:batch";

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Same_device_and_client_operation_text_are_isolated_between_companies()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var sharedDeviceId = "g4-shared-device-" + Guid.NewGuid().ToString("N");
        var sharedClientOperationId = "g4-shared-operation-" + Guid.NewGuid().ToString("N");
        var firstScope = await SeedAsync(db, "TENANT-A", sharedDeviceId);
        var secondScope = await SeedAsync(db, "TENANT-B", sharedDeviceId);
        var runtime = new SyncProofRuntimeService(db, new AuditEventService(db));
        var firstProof = await AcceptProofAsync(runtime, firstScope);
        var secondProof = await AcceptProofAsync(runtime, secondScope);
        const string payload = "{\"amount\":10}";
        var occurredAt = DateTimeOffset.UtcNow;
        var operationCorrelationId = Guid.NewGuid();
        var command = Command(sharedClientOperationId, payload, occurredAt, operationCorrelationId);
        var service = CreateOperationService(db);

        var first = await service.EnqueueAcceptedSyncOperationAsync(command, firstProof);
        var second = await service.EnqueueAcceptedSyncOperationAsync(command, secondProof);

        Assert.NotEqual(first.Id, second.Id);
        Assert.Equal(firstScope.Security.CompanyId, first.CompanyId);
        Assert.Equal(secondScope.Security.CompanyId, second.CompanyId);
        Assert.Equal(sharedDeviceId, first.DeviceId);
        Assert.Equal(sharedDeviceId, second.DeviceId);
        Assert.Equal(sharedClientOperationId, first.ClientOperationId);
        Assert.Equal(sharedClientOperationId, second.ClientOperationId);
        Assert.Equal(2, await db.SyncOperations.AsNoTracking().CountAsync(x =>
            x.DeviceId == sharedDeviceId && x.ClientOperationId == sharedClientOperationId));
        Assert.Equal(2, await db.AuditEvents.AsNoTracking().CountAsync(x =>
            x.Action == "SyncOperationQueued" &&
            x.OperationCorrelationId == operationCorrelationId &&
            (x.CompanyId == firstScope.Security.CompanyId || x.CompanyId == secondScope.Security.CompanyId)));
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Replay_mutation_of_each_variable_fingerprint_field_is_rejected_without_extra_effect()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "FINGERPRINT");
        var runtime = new SyncProofRuntimeService(db, new AuditEventService(db));
        var proof = await AcceptProofAsync(runtime, scope);
        var clientOperationId = "g4-fingerprint-" + Guid.NewGuid().ToString("N");
        const string payload = "{\"amount\":10}";
        var occurredAt = DateTimeOffset.UtcNow;
        var operationCorrelationId = Guid.NewGuid();
        var original = Command(clientOperationId, payload, occurredAt, operationCorrelationId);
        var service = CreateOperationService(db);
        await service.EnqueueAcceptedSyncOperationAsync(original, proof);
        var operationCount = await db.SyncOperations.AsNoTracking().CountAsync(x =>
            x.CompanyId == scope.Security.CompanyId &&
            x.RegisteredDeviceId == scope.DeviceId &&
            x.ClientOperationId == clientOperationId);
        var queueAuditCount = await db.AuditEvents.AsNoTracking().CountAsync(x =>
            x.Action == "SyncOperationQueued" && x.CompanyId == scope.Security.CompanyId);
        const string changedPayload = "{\"amount\":11}";
        var mutations = new (string Field, EnqueueAcceptedSyncOperationCommand Command, AcceptedSyncProofContext Proof)[]
        {
            ("UserId", original, proof with { UserId = Guid.NewGuid() }),
            ("BranchId", original, proof with { BranchId = Guid.NewGuid() }),
            ("ActionCode", original with { ActionCode = "CreateOperationalParty" }, proof),
            ("OperationType", original with { OperationType = "COMMAND" }, proof),
            ("EntityType", original with { EntityType = "OperationalParty" }, proof),
            ("EntityId", original with { EntityId = Guid.NewGuid() }, proof),
            ("PayloadHash", original with
            {
                PayloadJson = changedPayload,
                PayloadHash = Hash(changedPayload)
            }, proof),
            ("ClientOccurredAt", original with { ClientOccurredAt = occurredAt.AddSeconds(1) }, proof),
            ("BaseVersion", original with { BaseVersion = 1 }, proof),
            ("OperationCorrelationId", original with { OperationCorrelationId = Guid.NewGuid() }, proof)
        };

        foreach (var mutation in mutations)
        {
            var mismatch = await Assert.ThrowsAsync<SyncRuleException>(() =>
                service.EnqueueAcceptedSyncOperationAsync(mutation.Command, mutation.Proof));
            Assert.True(mismatch.Code == "IDEMPOTENCY_CONFLICT",
                $"Expected IDEMPOTENCY_CONFLICT for {mutation.Field}, received {mismatch.Code}.");
            Assert.Equal(operationCount,
                await db.SyncOperations.AsNoTracking().CountAsync(x =>
                    x.CompanyId == scope.Security.CompanyId &&
                    x.RegisteredDeviceId == scope.DeviceId &&
                    x.ClientOperationId == clientOperationId));
            Assert.Equal(queueAuditCount,
                await db.AuditEvents.AsNoTracking().CountAsync(x =>
                    x.Action == "SyncOperationQueued" && x.CompanyId == scope.Security.CompanyId));
        }

        Assert.Single(await db.SyncOperations.AsNoTracking().Where(x =>
            x.CompanyId == scope.Security.CompanyId &&
            x.RegisteredDeviceId == scope.DeviceId &&
            x.ClientOperationId == clientOperationId).ToListAsync());
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Distinct_operations_for_same_entity_and_base_version_converge_to_success_and_conflict()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "ENTITY-RACE");
        var runtime = new SyncProofRuntimeService(db, new AuditEventService(db));
        var proof = await AcceptProofAsync(runtime, scope);
        var entityId = Guid.NewGuid();
        const long baseVersion = 7;
        const string payload = "{}";
        var service = CreateOperationService(db);
        var first = await service.EnqueueAcceptedSyncOperationAsync(
            UpdateCommand("g4-entity-first-" + Guid.NewGuid().ToString("N"), entityId, baseVersion, payload), proof);
        var second = await service.EnqueueAcceptedSyncOperationAsync(
            UpdateCommand("g4-entity-second-" + Guid.NewGuid().ToString("N"), entityId, baseVersion, payload), proof);
        await RejectOtherExecutionCandidatesAsync(db, first.Id, second.Id);
        var claimAt = Normalize(DateTimeOffset.UtcNow.AddMinutes(1));

        db.ChangeTracker.Clear();
        var executor = new AtomicVersionExecutor(entityId, baseVersion);
        await using (var firstWorkerDb = PostgreSqlTestEnvironment.CreateDbContext(connection))
        await using (var secondWorkerDb = PostgreSqlTestEnvironment.CreateDbContext(connection))
        {
            var firstWorker = new SyncExecutionProcessor(CreateOperationService(firstWorkerDb), executor);
            var secondWorker = new SyncExecutionProcessor(CreateOperationService(secondWorkerDb), executor);
            var processed = await Task.WhenAll(
                firstWorker.ExecuteNextAsync(TimeSpan.FromMinutes(2), claimAt),
                secondWorker.ExecuteNextAsync(TimeSpan.FromMinutes(2), claimAt));
            Assert.All(processed, value => Assert.True(value));
        }

        db.ChangeTracker.Clear();
        var outcomes = await db.SyncOperations.AsNoTracking()
            .Where(x => x.Id == first.Id || x.Id == second.Id)
            .OrderBy(x => x.Status)
            .ToListAsync();
        Assert.Equal(2, outcomes.Count);
        Assert.Single(outcomes, x => x.Status == "SUCCEEDED" &&
            x.ResultEntityId == entityId && x.ResultVersion == baseVersion + 1);
        var conflicted = Assert.Single(outcomes, x => x.Status == "CONFLICT" &&
            x.ErrorCode == "BASE_VERSION_CONFLICT");
        var conflict = await db.ConflictCases.AsNoTracking().SingleAsync(x =>
            x.SyncOperationId == conflicted.Id);
        Assert.Equal(baseVersion, conflict.BaseVersion);
        Assert.Equal("OPEN", conflict.Status);
        Assert.Equal(1, executor.EffectCount);
        Assert.Equal(2, await db.AuditEvents.AsNoTracking().CountAsync(x =>
            (x.EntityId == first.Id || x.EntityId == second.Id) &&
            (x.Action == "SyncOperationExecutionSucceeded" ||
             x.Action == "SyncOperationExecutionConflict")));
    }

    private static SyncOperationService CreateOperationService(TransportErpDbContext db)
        => new(db, new AuditEventService(db),
            new SyncRetryPolicy(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30)));

    private static EnqueueAcceptedSyncOperationCommand Command(
        string clientOperationId,
        string payload,
        DateTimeOffset occurredAt,
        Guid operationCorrelationId)
        => new("sync-v1", "CreateJournalEntry", "CREATE", "JournalEntry", null,
            clientOperationId, payload, Hash(payload), occurredAt, operationCorrelationId, null);

    private static EnqueueAcceptedSyncOperationCommand UpdateCommand(
        string clientOperationId,
        Guid entityId,
        long baseVersion,
        string payload)
        => new("sync-v1", "UpdateWaybillDraft", "UPDATE", "Waybill", entityId,
            clientOperationId, payload, Hash(payload), DateTimeOffset.UtcNow, Guid.NewGuid(), baseVersion);

    private static async Task<AcceptedSyncProofContext> AcceptProofAsync(
        SyncProofRuntimeService runtime,
        TestScope scope)
    {
        var nonce = await runtime.IssueNonceAsync(scope.Security);
        return await runtime.ClaimAsync(scope.Security,
            new VerifiedSyncProofMaterial(
                Guid.NewGuid().ToString("D"), nonce.Value, scope.Thumbprint,
                DateTimeOffset.UtcNow, Htu, Guid.NewGuid()));
    }

    private static Task<int> RejectOtherExecutionCandidatesAsync(
        TransportErpDbContext db,
        Guid firstOperationId,
        Guid secondOperationId)
        => db.Database.ExecuteSqlInterpolatedAsync($$"""
            UPDATE transport_erp.sync_operations
            SET "Status"='REJECTED',
                "ErrorCode"='TEST_EXECUTION_ISOLATION',
                "NextRetryAt"=NULL,
                "ExecutionClaimToken"=NULL,
                "ExecutionAttemptStartedAt"=NULL,
                "ExecutionLeaseExpiresAt"=NULL,
                "UpdatedAt"={{Normalize(DateTimeOffset.UtcNow)}}
            WHERE "Id"<>{{firstOperationId}}
              AND "Id"<>{{secondOperationId}}
              AND "ActionCode" IS NOT NULL
              AND "Status" IN ('QUEUED','FAILED','SENDING')
            """);

    private static string Hash(string payload)
        => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(payload))).ToLowerInvariant();

    private static DateTimeOffset Normalize(DateTimeOffset value)
    {
        var ticks = value.UtcDateTime.Ticks;
        return new DateTimeOffset(new DateTime(
            ticks - ticks % TimeSpan.TicksPerMicrosecond, DateTimeKind.Utc));
    }

    private static async Task<TestScope> SeedAsync(
        TransportErpDbContext db,
        string suffix,
        string? deviceIdOverride = null)
    {
        var now = DateTimeOffset.UtcNow;
        var currency = new Currency
        {
            Id = Guid.NewGuid(),
            Code = await PostgreSqlTestCurrencyCodeAllocator.NextAsync(db),
            NameAr = "عملة G4",
            MinorUnit = 2,
            IsBase = true,
            CreatedAt = now,
            UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var company = new Company
        {
            Id = Guid.NewGuid(),
            Code = $"G4-{suffix}-{Guid.NewGuid():N}"[..18],
            LegalNameAr = "شركة G4",
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
            Timezone = "Asia/Aden",
            Status = "ACTIVE",
            CreatedAt = now,
            UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = $"g4-{Guid.NewGuid():N}",
            NormalizedUserName = $"G4-{Guid.NewGuid():N}",
            DisplayName = "مستخدم G4",
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
        var deviceId = deviceIdOverride ?? $"g4-{suffix}-{Guid.NewGuid():N}";
        var thumbprint = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
            .TrimEnd('=').Replace('+', '-').Replace('/', '_');
        var device = new RegisteredDevice
        {
            Id = Guid.NewGuid(),
            CompanyId = company.Id,
            DeviceId = deviceId,
            DisplayName = "جهاز G4",
            Platform = "TEST",
            AppVersion = "1.0",
            RegistrationRequestId = "req-" + Guid.NewGuid().ToString("N"),
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
        return new TestScope(device.Id, thumbprint,
            new SyncProofSecurityContext(user.Id, company.Id, branch.Id, device.Id, deviceId));
    }

    private sealed record TestScope(
        Guid DeviceId,
        string Thumbprint,
        SyncProofSecurityContext Security);

    private sealed class AtomicVersionExecutor(Guid entityId, long initialVersion) : ISyncActionExecutor
    {
        private long _version = initialVersion;
        private int _effectCount;

        public int EffectCount => Volatile.Read(ref _effectCount);

        public Task<SyncActionExecutionOutcome> ExecuteAsync(
            SyncOperationExecutionClaim claim,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (claim.EntityId != entityId || claim.BaseVersion != initialVersion)
                return Task.FromResult<SyncActionExecutionOutcome>(
                    new SyncActionExecutionOutcome.Failed("ACTION_CONTRACT_MISMATCH"));

            var observed = Interlocked.CompareExchange(
                ref _version, initialVersion + 1, initialVersion);
            if (observed == initialVersion)
            {
                Interlocked.Increment(ref _effectCount);
                return Task.FromResult<SyncActionExecutionOutcome>(
                    new SyncActionExecutionOutcome.Succeeded(entityId, initialVersion + 1));
            }

            return Task.FromResult<SyncActionExecutionOutcome>(
                new SyncActionExecutionOutcome.Conflict("BASE_VERSION_CONFLICT"));
        }
    }
}
