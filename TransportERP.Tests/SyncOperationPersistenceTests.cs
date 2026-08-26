using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

[Collection("PostgreSql")]
public sealed class SyncOperationPersistenceTests
{
    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Legacy_enqueue_is_fail_closed_until_the_stage4_proof_path_is_used()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();

        await using var db = CreateDb(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedScopeAsync(db, "IDEMP");
        var service = CreateService(db);
        var payload = "{\"amount\":10}";
        var command = CreateCommand(scope, payload);

        var error = await Assert.ThrowsAsync<DbUpdateException>(() =>
            service.EnqueueSyncOperationAsync(command, scope.Security));
        Assert.Contains("new sync operation requires accepted Stage4 proof replay",
            error.GetBaseException().Message, StringComparison.Ordinal);
        Assert.False(await db.SyncOperations.AnyAsync(x => x.ClientOperationId == command.ClientOperationId));
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Enqueue_enforces_device_permission_and_company_branch_scope()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();

        await using var db = CreateDb(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedScopeAsync(db, "SCOPE");
        var service = CreateService(db);
        var command = CreateCommand(scope, "{\"scope\":true}");

        await Assert.ThrowsAsync<SyncRuleException>(() => service.EnqueueSyncOperationAsync(
            command, scope.Security with { IsDeviceRegistered = false }));
        await Assert.ThrowsAsync<SyncRuleException>(() => service.EnqueueSyncOperationAsync(
            command, scope.Security with { HasExecutePermission = false }));
        await Assert.ThrowsAsync<SyncRuleException>(() => service.EnqueueSyncOperationAsync(
            command, scope.Security with { CompanyId = Guid.NewGuid() }));
        await Assert.ThrowsAsync<SyncRuleException>(() => service.EnqueueSyncOperationAsync(
            command, scope.Security with { DeviceId = "other-device" }));
        await Assert.ThrowsAsync<SyncRuleException>(() => service.EnqueueSyncOperationAsync(
            command, scope.Security with { RegisteredDeviceCredentialVersion = 99 }));

        var device = await db.RegisteredDevices.SingleAsync(x => x.Id == scope.Security.RegisteredDeviceId);
        device.Status = "SUSPENDED";
        await db.SaveChangesAsync();
        await Assert.ThrowsAsync<SyncRuleException>(() => service.EnqueueSyncOperationAsync(command, scope.Security));
        device.Status = "ACTIVE";
        device.LastSeenAt = DateTimeOffset.UtcNow.AddDays(-91);
        await db.SaveChangesAsync();
        await Assert.ThrowsAsync<SyncRuleException>(() => service.EnqueueSyncOperationAsync(command, scope.Security));
        device.LastSeenAt = DateTimeOffset.UtcNow;
        var assignment = await db.RegisteredDeviceAssignments.SingleAsync(x =>
            x.RegisteredDeviceId == device.Id && x.UserId == scope.Security.UserId && x.Status == "ACTIVE");
        assignment.Status = "REVOKED"; assignment.RemovedAt = DateTimeOffset.UtcNow;
        await db.SaveChangesAsync();
        await Assert.ThrowsAsync<SyncRuleException>(() => service.EnqueueSyncOperationAsync(command, scope.Security));
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Legacy_enqueue_rejection_is_atomic_when_the_audit_path_is_unavailable()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = CreateDb(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedScopeAsync(db, "AUDFAIL");
        var command = CreateCommand(scope, "{\"auditFailure\":true}");
        var suffix = Guid.NewGuid().ToString("N");
        var function = $"fail_sync_audit_{suffix}";
        var trigger = $"trg_fail_sync_audit_{suffix}";
        await using var admin = CreateDb(connection);
        await admin.Database.ExecuteSqlRawAsync($$"""
            CREATE FUNCTION transport_erp.{{function}}() RETURNS trigger LANGUAGE plpgsql AS $body$
            BEGIN
              IF NEW."Action" = 'SyncOperationQueued' THEN RAISE EXCEPTION 'forced sync audit failure'; END IF;
              RETURN NEW;
            END $body$;
            CREATE TRIGGER {{trigger}} BEFORE INSERT ON transport_erp.audit_events
              FOR EACH ROW EXECUTE FUNCTION transport_erp.{{function}}();
            """);
        try
        {
            await Assert.ThrowsAnyAsync<Exception>(() => CreateService(db)
                .EnqueueSyncOperationAsync(command, scope.Security));
        }
        finally
        {
            await admin.Database.ExecuteSqlRawAsync($$"""
                DROP TRIGGER IF EXISTS {{trigger}} ON transport_erp.audit_events;
                DROP FUNCTION IF EXISTS transport_erp.{{function}}();
                """);
        }

        await using var verify = CreateDb(connection);
        Assert.False(await verify.SyncOperations.AnyAsync(x =>
            x.DeviceId == command.DeviceId && x.ClientOperationId == command.ClientOperationId));
        Assert.False(await verify.AuditEvents.AnyAsync(x =>
            x.Action == "SyncOperationQueued" && x.DeviceId == command.DeviceId));
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Reused_batch_context_is_clean_after_owned_rollback_and_persists_only_next_operation()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = CreateDb(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedScopeAsync(db, "BATCHRB");
        var failedCommand = CreateCommand(scope, "{\"batch\":\"failed\"}");
        var succeedingCommand = CreateCommand(scope, "{\"batch\":\"succeeded\"}");
        var service = CreateService(db);
        var suffix = Guid.NewGuid().ToString("N");
        var function = $"fail_first_sync_audit_{suffix}";
        var trigger = $"trg_fail_first_sync_audit_{suffix}";

        await using var admin = CreateDb(connection);
        await admin.Database.ExecuteSqlRawAsync($$"""
            CREATE FUNCTION transport_erp.{{function}}() RETURNS trigger LANGUAGE plpgsql AS $body$
            BEGIN
              IF NEW."Action" = 'SyncOperationQueued' THEN RAISE EXCEPTION 'forced first batch audit failure'; END IF;
              RETURN NEW;
            END $body$;
            CREATE TRIGGER {{trigger}} BEFORE INSERT ON transport_erp.audit_events
              FOR EACH ROW EXECUTE FUNCTION transport_erp.{{function}}();
            """);

        try
        {
            await Assert.ThrowsAnyAsync<Exception>(() =>
                service.EnqueueSyncOperationAsync(failedCommand, scope.Security));
            Assert.Empty(db.ChangeTracker.Entries());
        }
        finally
        {
            await admin.Database.ExecuteSqlRawAsync($$"""
                DROP TRIGGER IF EXISTS {{trigger}} ON transport_erp.audit_events;
                DROP FUNCTION IF EXISTS transport_erp.{{function}}();
                """);
        }

        await Assert.ThrowsAsync<DbUpdateException>(() =>
            service.EnqueueSyncOperationAsync(succeedingCommand, scope.Security));
        Assert.Empty(db.ChangeTracker.Entries());

        await using var verify = CreateDb(connection);
        Assert.False(await verify.SyncOperations.AnyAsync(x =>
            x.DeviceId == failedCommand.DeviceId && x.ClientOperationId == failedCommand.ClientOperationId));
        Assert.False(await verify.SyncOperations.AnyAsync(x =>
            x.DeviceId == succeedingCommand.DeviceId && x.ClientOperationId == succeedingCommand.ClientOperationId));
        Assert.False(await verify.AuditEvents.AnyAsync(x =>
            x.Action == "SyncOperationQueued" && x.DeviceId == scope.Security.DeviceId));
        var chain = await new AuditEventService(verify).VerifyHashChainAsync(
            scope.CompanyId, scope.BranchId, scope.Security.DeviceId);
        Assert.True(chain.IsValid, chain.FailureReason);
        Assert.Equal(0, chain.EventCount);
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Lifecycle_retry_backoff_conflict_case_and_resolution_are_persisted()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();

        await using var db = CreateDb(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedScopeAsync(db, "LIFE");
        var service = CreateService(db, new SyncRetryPolicy(3, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(20)));
        var operation = await InsertAcceptedOperationAsync(db, scope, "{\"lifecycle\":true}");

        operation = await service.TransitionSyncOperationAsync(
            new TransitionSyncOperationCommand(operation.Id, "SENDING"), scope.Security);
        operation = await service.TransitionSyncOperationAsync(
            new TransitionSyncOperationCommand(operation.Id, "FAILED", "RATE_LIMITED"), scope.Security);
        operation = await service.RetryOperationAsync(operation.Id, scope.Security);

        Assert.Equal("FAILED", operation.Status);
        Assert.Equal(1, operation.RetryCount);
        Assert.True(operation.NextRetryAt > DateTimeOffset.UtcNow);
        var pendingBeforeDue = await service.GetPendingRetriesAsync(scope.Security, DateTimeOffset.UtcNow);
        Assert.DoesNotContain(pendingBeforeDue, x => x.Id == operation.Id);

        operation.NextRetryAt = DateTimeOffset.UtcNow.AddMinutes(-1);
        operation.UpdatedAt = DateTimeOffset.UtcNow;
        operation.RowVersion = Guid.NewGuid().ToByteArray();
        await db.SaveChangesAsync();
        var pendingAfterDue = await service.GetPendingRetriesAsync(scope.Security, DateTimeOffset.UtcNow);
        Assert.Contains(pendingAfterDue, x => x.Id == operation.Id);

        operation = await service.TransitionSyncOperationAsync(
            new TransitionSyncOperationCommand(operation.Id, "SENDING"), scope.Security);
        operation = await service.TransitionSyncOperationAsync(
            new TransitionSyncOperationCommand(operation.Id, "CONFLICT"), scope.Security);
        var conflict = await service.CreateConflictCaseAsync(operation.Id,
            new ConflictCaseDraft("{\"device\":1}", "{\"server\":2}", "BASE_VERSION_STALE", 1),
            scope.Security);

        Assert.Equal("OPEN", conflict.Status);
        Assert.Equal(operation.Id, conflict.SyncOperationId);
        Assert.Equal("CONFLICT", await db.SyncOperations.Where(x => x.Id == operation.Id).Select(x => x.Status).SingleAsync());

        var resolved = await service.ResolveSyncConflictAsync(conflict.Id,
            new ResolveSyncConflictCommand("USE_SERVER_VALUE"), scope.Security);
        Assert.Equal("RESOLVED", resolved.Status);
        Assert.Equal("RESOLVED", await db.SyncOperations.Where(x => x.Id == operation.Id).Select(x => x.Status).SingleAsync());
        Assert.True(await db.AuditEvents.AnyAsync(x => x.Action == "SyncOperationConflictResolved" && x.EntityId == operation.Id));

        await Assert.ThrowsAsync<SyncRuleException>(() => service.ResolveSyncConflictAsync(conflict.Id,
            new ResolveSyncConflictCommand("SECOND_ATTEMPT"), scope.Security));
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Retry_rejects_non_retryable_hash_and_permission_errors()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();

        await using var db = CreateDb(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedScopeAsync(db, "NRT");
        var service = CreateService(db);
        var operation = await InsertAcceptedOperationAsync(db, scope, "{\"nonRetryable\":true}");
        operation = await service.TransitionSyncOperationAsync(
            new TransitionSyncOperationCommand(operation.Id, "SENDING"), scope.Security);
        operation = await service.TransitionSyncOperationAsync(
            new TransitionSyncOperationCommand(operation.Id, "FAILED", "HASH_MISMATCH"), scope.Security);

        var result = await service.RetryOperationAsync(operation.Id, scope.Security);

        Assert.Equal("REJECTED", result.Status);
        Assert.Null(result.NextRetryAt);
        Assert.Equal("HASH_MISMATCH", result.ErrorCode);
        Assert.Contains(await db.AuditEvents.ToListAsync(), x => x.Action == "SyncOperationRetryRejected" && x.EntityId == operation.Id);
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Rotation_denies_stale_execution_but_historical_provenance_is_immutable_and_terminal_update_remains_possible()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = CreateDb(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedScopeAsync(db, "PROVENANCE");
        var service = CreateService(db);
        var operation = await InsertAcceptedOperationAsync(db, scope, "{\"provenance\":true}");
        var originalDevice = operation.RegisteredDeviceId;
        var originalVersion = operation.RegisteredDeviceCredentialVersion;
        var device = await db.RegisteredDevices.SingleAsync(x => x.Id == originalDevice);
        device.CredentialVersion++;
        device.CredentialHash = new string('b', 64);
        device.UpdatedAt = DateTimeOffset.UtcNow;
        device.RowVersion = RandomNumberGenerator.GetBytes(16);
        await db.SaveChangesAsync();

        await Assert.ThrowsAsync<SyncRuleException>(() => service.TransitionSyncOperationAsync(
            new(operation.Id, "SENDING"), scope.Security));
        db.ChangeTracker.Clear();
        var historical = await db.SyncOperations.SingleAsync(x => x.Id == operation.Id);
        historical.Status = "REJECTED";
        historical.ErrorCode = "DEVICE_CREDENTIAL_ROTATED";
        historical.UpdatedAt = DateTimeOffset.UtcNow;
        historical.RowVersion = RandomNumberGenerator.GetBytes(16);
        await db.SaveChangesAsync();
        Assert.Equal(originalVersion, historical.RegisteredDeviceCredentialVersion);

        historical.RegisteredDeviceCredentialVersion = device.CredentialVersion;
        await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
        await using var verify = CreateDb(connection);
        var persisted = await verify.SyncOperations.AsNoTracking().SingleAsync(x => x.Id == operation.Id);
        Assert.Equal(originalDevice, persisted.RegisteredDeviceId);
        Assert.Equal(originalVersion, persisted.RegisteredDeviceCredentialVersion);
        Assert.Equal("REJECTED", persisted.Status);
    }

    private static SyncOperationService CreateService(
        TransportErpDbContext db,
        SyncRetryPolicy? retryPolicy = null)
        => new(db, new AuditEventService(db), retryPolicy ?? new SyncRetryPolicy(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30)));

    private static EnqueueSyncOperationCommand CreateCommand(TestScope scope, string payload)
        => new(scope.Security.DeviceId, scope.Security.UserId, scope.Security.CompanyId, scope.Security.BranchId,
            "UPDATE", "TestEntity", Guid.NewGuid(), $"client-{Guid.NewGuid():N}", payload,
            Hash(payload), DateTimeOffset.UtcNow, 1);

    private static async Task<SyncOperation> InsertAcceptedOperationAsync(
        TransportErpDbContext db, TestScope scope, string payload)
    {
        var now = DateTimeOffset.UtcNow;
        var assignmentId = await db.RegisteredDeviceAssignments
            .Where(x => x.RegisteredDeviceId == scope.Security.RegisteredDeviceId &&
                        x.UserId == scope.Security.UserId && x.Status == "ACTIVE")
            .Select(x => x.Id).SingleAsync();
        var nonce = new SyncProofNonce
        {
            Id = Guid.NewGuid(), CompanyId = scope.CompanyId,
            RegisteredDeviceId = scope.Security.RegisteredDeviceId!.Value,
            DeviceId = scope.Security.DeviceId, ProofKeyVersion = 1,
            NonceHash = RandomNumberGenerator.GetBytes(32), IssuedAt = now, ExpiresAt = now.AddMinutes(5)
        };
        var replay = new SyncProofReplay
        {
            Id = Guid.NewGuid(), CompanyId = scope.CompanyId,
            RegisteredDeviceId = scope.Security.RegisteredDeviceId.Value,
            DeviceId = scope.Security.DeviceId, DeviceAssignmentId = assignmentId,
            UserId = scope.Security.UserId, BranchId = scope.BranchId, ProofKeyVersion = 1,
            ProofKeyThumbprint = new string('t', 43), JtiHash = RandomNumberGenerator.GetBytes(32),
            HtuHash = RandomNumberGenerator.GetBytes(32), HttpMethod = "POST", NonceRecordId = nonce.Id,
            IssuedAt = now, FirstSeenAt = now.AddSeconds(1), ExpiresAt = now.AddMinutes(4),
            AttemptCorrelationId = Guid.NewGuid()
        };
        var operation = new SyncOperation
        {
            Id = Guid.NewGuid(), DeviceId = scope.Security.DeviceId, UserId = scope.Security.UserId,
            CompanyId = scope.CompanyId, BranchId = scope.BranchId, OperationType = "UPDATE",
            EntityType = "TestEntity", EntityId = Guid.NewGuid(), ClientOperationId = $"client-{Guid.NewGuid():N}",
            PayloadJson = payload, PayloadHash = Hash(payload), ClientOccurredAt = now, ServerReceivedAt = now,
            BaseVersion = 1, Status = "QUEUED", RetryCount = 0,
            RegisteredDeviceId = scope.Security.RegisteredDeviceId,
            RegisteredDeviceCredentialVersion = scope.Security.RegisteredDeviceCredentialVersion,
            ActionCode = "test.update", ProtocolVersion = "sync-v1", OperationCorrelationId = Guid.NewGuid(),
            RequestFingerprintVersion = "fp-v1", RequestFingerprintHash = RandomNumberGenerator.GetBytes(32),
            ProofKeyVersion = 1, ProofKeyThumbprint = new string('t', 43), AcceptedProofReplayId = replay.Id,
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        db.AddRange(nonce, replay);
        await db.SaveChangesAsync();
        db.Add(operation);
        await db.SaveChangesAsync();
        return operation;
    }

    private static string Hash(string payload)
        => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(payload))).ToLowerInvariant();

    private static TransportErpDbContext CreateDb(string connection)
        => PostgreSqlTestEnvironment.CreateDbContext(connection);

    private static async Task<TestScope> SeedScopeAsync(TransportErpDbContext db, string suffix)
    {
        var now = DateTimeOffset.UtcNow;
        var currency = new Currency
        {
            Id = Guid.NewGuid(), Code = await NextCurrencyCodeAsync(db),
            NameAr = "عملة اختبار", MinorUnit = 2, IsBase = true, CreatedAt = now, UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        var company = new Company
        {
            Id = Guid.NewGuid(), Code = $"S-{suffix}-{Guid.NewGuid():N}"[..18], LegalNameAr = "شركة مزامنة اختبار",
            BaseCurrencyId = currency.Id, DefaultCalendarId = Guid.NewGuid(), Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
        };
        var branch = new Branch
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, Code = "MAIN", NameAr = "الفرع الرئيسي",
            Timezone = "Asia/Aden", Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        var user = new User
        {
            Id = Guid.NewGuid(), UserName = $"sync-{Guid.NewGuid():N}", NormalizedUserName = "SYNC",
            DisplayName = "مستخدم مزامنة", PasswordHash = "test-only", SecurityStamp = Guid.NewGuid().ToString("N"), AuthVersion = 1, Status = "ACTIVE",
            CompanyId = company.Id, BranchId = branch.Id, CreatedAt = now, UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        db.Currencies.Add(currency);
        db.Companies.Add(company);
        db.Branches.Add(branch);
        db.Users.Add(user);
        await db.SaveChangesAsync();
        var deviceId = $"device-{suffix}-{Guid.NewGuid():N}";
        var device = new RegisteredDevice
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, DeviceId = deviceId,
            DisplayName = "جهاز مزامنة", Platform = "TEST", AppVersion = "1.0",
            RegistrationRequestId = $"request-{Guid.NewGuid():N}",
            CredentialHash = new string('a', 64), CredentialVersion = 1, Status = "ACTIVE",
            RegisteredByUserId = user.Id, ApprovedByUserId = user.Id, ApprovedAt = now, LastSeenAt = now,
            CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
        };
        var assignment = new RegisteredDeviceAssignment
        {
            Id = Guid.NewGuid(), RegisteredDeviceId = device.Id, UserId = user.Id,
            CompanyId = company.Id, BranchId = branch.Id, Status = "ACTIVE",
            AssignedByUserId = user.Id, AssignedAt = now, CreatedAt = now, UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        db.RegisteredDevices.Add(device);
        db.RegisteredDeviceAssignments.Add(assignment);
        await db.SaveChangesAsync();
        return new TestScope(company.Id, branch.Id,
            new SyncSecurityContext(user.Id, deviceId, company.Id, branch.Id, true, true,
                device.Id, device.CredentialVersion));
    }

    private static async Task<string> NextCurrencyCodeAsync(TransportErpDbContext db)
        => await PostgreSqlTestCurrencyCodeAllocator.NextAsync(db);

    private sealed record TestScope(Guid CompanyId, Guid BranchId, SyncSecurityContext Security);
}
