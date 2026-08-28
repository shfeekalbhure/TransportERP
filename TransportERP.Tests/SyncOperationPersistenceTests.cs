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
    public async Task Enqueue_is_idempotent_and_rejects_payload_hash_reuse()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();

        await using var db = CreateDb(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedScopeAsync(db, "IDEMP");
        var service = CreateService(db);
        var security = scope.Security;
        var payload = "{\"amount\":10}";
        var command = CreateCommand(scope, payload);

        var first = await service.EnqueueSyncOperationAsync(command, security);
        var replay = await service.EnqueueSyncOperationAsync(command, security);

        Assert.Equal(first.Id, replay.Id);
        Assert.Equal(1, await db.SyncOperations.CountAsync(x => x.Id == first.Id));

        var mismatch = command with { PayloadJson = "{\"amount\":11}" };
        await Assert.ThrowsAsync<SyncRuleException>(() => service.EnqueueSyncOperationAsync(mismatch, security));
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
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Enqueue_rejects_claim_only_permission_and_stored_membership_mismatch()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();

        await using var db = CreateDb(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedScopeAsync(db, "AUTHZ");
        var service = CreateService(db);
        var command = CreateCommand(scope, "{\"authority\":true}");

        var grant = await db.RolePermissions.SingleAsync(x =>
            x.PermissionId == scope.PermissionId && x.RoleId == scope.RoleId);
        db.RolePermissions.Remove(grant);
        await db.SaveChangesAsync();

        var claimOnly = await Assert.ThrowsAsync<SyncRuleException>(() =>
            service.EnqueueSyncOperationAsync(command, scope.Security));
        Assert.Equal("PERMISSION_DENIED", claimOnly.Code);

        db.RolePermissions.Add(grant);
        db.UserPermissionOverrides.Add(new UserPermissionOverride
        {
            UserId = scope.Security.UserId, PermissionId = scope.PermissionId, IsAllowed = false,
            CompanyId = scope.CompanyId, BranchId = scope.BranchId, Reason = "revoked during test",
            CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow,
            RowVersion = Guid.NewGuid().ToByteArray()
        });
        await db.SaveChangesAsync();
        var revoked = await Assert.ThrowsAsync<SyncRuleException>(() =>
            service.EnqueueSyncOperationAsync(command with { ClientOperationId = $"revoked-{Guid.NewGuid():N}" }, scope.Security));
        Assert.Equal("PERMISSION_DENIED", revoked.Code);
        db.UserPermissionOverrides.Remove(await db.UserPermissionOverrides.SingleAsync(x =>
            x.UserId == scope.Security.UserId && x.PermissionId == scope.PermissionId));

        var otherBranch = new Branch
        {
            Id = Guid.NewGuid(), CompanyId = scope.CompanyId, Code = $"B{Guid.NewGuid():N}"[..12],
            NameAr = "فرع آخر", Timezone = "Asia/Aden", Status = "ACTIVE",
            CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        db.Branches.Add(otherBranch);
        await db.SaveChangesAsync();

        var wrongBranchSecurity = scope.Security with { BranchId = otherBranch.Id };
        var wrongBranchCommand = command with { BranchId = otherBranch.Id };
        var membership = await Assert.ThrowsAsync<SyncRuleException>(() =>
            service.EnqueueSyncOperationAsync(wrongBranchCommand, wrongBranchSecurity));
        Assert.Equal("SCOPE_DENIED", membership.Code);
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Lifecycle_mutations_deny_same_tenant_different_user_or_device()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();

        await using var db = CreateDb(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedScopeAsync(db, "OWNER");
        var otherUser = await AddUserWithRoleAsync(db, scope, "other-owner");
        var otherUserSecurity = scope.Security with { UserId = otherUser.Id };
        var otherDeviceSecurity = scope.Security with { DeviceId = "other-device" };
        var service = CreateService(db);

        var transitionOperation = await service.EnqueueSyncOperationAsync(
            CreateCommand(scope, "{\"transition\":true}"), scope.Security);
        await AssertScopeDeniedAsync(() => service.TransitionSyncOperationAsync(
            new TransitionSyncOperationCommand(transitionOperation.Id, "SENDING"), otherUserSecurity));
        await AssertScopeDeniedAsync(() => service.TransitionSyncOperationAsync(
            new TransitionSyncOperationCommand(transitionOperation.Id, "SENDING"), otherDeviceSecurity));

        await service.TransitionSyncOperationAsync(
            new TransitionSyncOperationCommand(transitionOperation.Id, "SENDING"), scope.Security);
        await service.TransitionSyncOperationAsync(
            new TransitionSyncOperationCommand(transitionOperation.Id, "FAILED", "RATE_LIMITED"), scope.Security);
        await AssertScopeDeniedAsync(() => service.RetryOperationAsync(transitionOperation.Id, otherUserSecurity));
        await AssertScopeDeniedAsync(() => service.RetryOperationAsync(transitionOperation.Id, otherDeviceSecurity));

        var conflictOperation = await service.EnqueueSyncOperationAsync(
            CreateCommand(scope, "{\"conflict\":true}"), scope.Security);
        await service.TransitionSyncOperationAsync(
            new TransitionSyncOperationCommand(conflictOperation.Id, "SENDING"), scope.Security);
        await service.TransitionSyncOperationAsync(
            new TransitionSyncOperationCommand(conflictOperation.Id, "CONFLICT"), scope.Security);
        var draft = new ConflictCaseDraft("{\"device\":1}", "{\"server\":2}", "OWNER_TEST");
        await AssertScopeDeniedAsync(() => service.CreateConflictCaseAsync(
            conflictOperation.Id, draft, otherUserSecurity));
        await AssertScopeDeniedAsync(() => service.CreateConflictCaseAsync(
            conflictOperation.Id, draft, otherDeviceSecurity));

        var conflict = await service.CreateConflictCaseAsync(conflictOperation.Id, draft, scope.Security);
        await AssertScopeDeniedAsync(() => service.ResolveSyncConflictAsync(
            conflict.Id, new ResolveSyncConflictCommand("DENIED"), otherUserSecurity));
        await AssertScopeDeniedAsync(() => service.ResolveSyncConflictAsync(
            conflict.Id, new ResolveSyncConflictCommand("DENIED"), otherDeviceSecurity));

        var replacementPayload = "{\"replacement\":true}";
        var replacementCommand = new EnqueueSyncOperationCommand(
            otherUserSecurity.DeviceId, otherUser.Id, scope.CompanyId, scope.BranchId,
            "UPDATE", "TestEntity", Guid.NewGuid(), $"client-{Guid.NewGuid():N}", replacementPayload,
            Hash(replacementPayload), DateTimeOffset.UtcNow, 1);
        var replacement = await service.EnqueueSyncOperationAsync(replacementCommand, otherUserSecurity);
        await AssertScopeDeniedAsync(() => service.ResolveSyncConflictAsync(
            conflict.Id, new ResolveSyncConflictCommand("FOREIGN_REPLACEMENT", replacement.Id), scope.Security));
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
        var operation = await service.EnqueueSyncOperationAsync(CreateCommand(scope, "{\"lifecycle\":true}"), scope.Security);

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
        var operation = await service.EnqueueSyncOperationAsync(CreateCommand(scope, "{\"nonRetryable\":true}"), scope.Security);
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

    private static SyncOperationService CreateService(
        TransportErpDbContext db,
        SyncRetryPolicy? retryPolicy = null)
        => new(db, new AuditEventService(db),
            retryPolicy ?? new SyncRetryPolicy(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30)),
            new PersistentPermissionResolver(db));

    private static EnqueueSyncOperationCommand CreateCommand(TestScope scope, string payload)
        => new(scope.Security.DeviceId, scope.Security.UserId, scope.Security.CompanyId, scope.Security.BranchId,
            "UPDATE", "TestEntity", Guid.NewGuid(), $"client-{Guid.NewGuid():N}", payload,
            Hash(payload), DateTimeOffset.UtcNow, 1);

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
            DisplayName = "مستخدم مزامنة", PasswordHash = "test-only", Status = "ACTIVE",
            CompanyId = company.Id, BranchId = branch.Id, CreatedAt = now, UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        var permission = await db.Permissions.SingleOrDefaultAsync(x => x.Code == "sync.operations.execute");
        if (permission is null)
        {
            permission = new Permission
            {
                Id = Guid.NewGuid(), Code = "sync.operations.execute", NameAr = "تنفيذ عمليات المزامنة",
                Resource = "sync.operations", Action = "execute", ScopeType = "BRANCH", IsSystem = true,
                Status = "ACTIVE", CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
            };
            db.Permissions.Add(permission);
        }
        var role = new Role
        {
            Id = Guid.NewGuid(), Code = $"SYNC-{Guid.NewGuid():N}"[..24], NameAr = "دور اختبار مزامنة",
            CompanyId = company.Id, Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        db.AddRange(currency, company, branch, user, role);
        db.UserRoles.Add(new UserRole
        {
            UserId = user.Id, RoleId = role.Id, CompanyId = company.Id, BranchId = branch.Id,
            CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
        });
        db.RolePermissions.Add(new RolePermission
        {
            RoleId = role.Id, PermissionId = permission.Id, ScopeType = "BRANCH",
            CompanyId = company.Id, BranchId = branch.Id,
            CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
        });
        await db.SaveChangesAsync();
        return new TestScope(company.Id, branch.Id, permission.Id, role.Id,
            new SyncSecurityContext(user.Id, $"device-{suffix}-{Guid.NewGuid():N}", company.Id, branch.Id, true, true));
    }

    private static async Task<User> AddUserWithRoleAsync(TransportErpDbContext db, TestScope scope, string suffix)
    {
        var now = DateTimeOffset.UtcNow;
        var user = new User
        {
            Id = Guid.NewGuid(), UserName = $"{suffix}-{Guid.NewGuid():N}",
            NormalizedUserName = $"{suffix}-{Guid.NewGuid():N}".ToUpperInvariant(),
            DisplayName = "مستخدم مالك آخر", PasswordHash = "test-only", Status = "ACTIVE",
            CompanyId = scope.CompanyId, BranchId = scope.BranchId,
            CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
        };
        db.Users.Add(user);
        db.UserRoles.Add(new UserRole
        {
            UserId = user.Id, RoleId = scope.RoleId, CompanyId = scope.CompanyId, BranchId = scope.BranchId,
            CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
        });
        await db.SaveChangesAsync();
        return user;
    }

    private static async Task AssertScopeDeniedAsync(Func<Task> action)
    {
        var exception = await Assert.ThrowsAsync<SyncRuleException>(action);
        Assert.Equal("SCOPE_DENIED", exception.Code);
    }

    private static async Task<string> NextCurrencyCodeAsync(TransportErpDbContext db)
    {
        for (var attempt = 0; attempt < 16; attempt++)
        {
            var code = Guid.NewGuid().ToString("N")[..3].ToUpperInvariant();
            if (!await db.Currencies.AnyAsync(x => x.Code == code))
                return code;
        }

        throw new InvalidOperationException("Unable to allocate a unique three-character currency code for the PostgreSQL sync test.");
    }

    private sealed record TestScope(
        Guid CompanyId,
        Guid BranchId,
        Guid PermissionId,
        Guid RoleId,
        SyncSecurityContext Security);
}
