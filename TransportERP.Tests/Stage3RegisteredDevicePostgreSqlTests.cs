using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Options;
using TransportERP.Api.Identity;
using TransportERP.Api.Security;
using TransportERP.Contracts.Identity;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

[Collection("PostgreSql")]
public sealed class Stage3RegisteredDevicePostgreSqlTests
{
    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Registration_is_full_payload_idempotent_and_never_persists_secret()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "REG");
        var service = new RegisteredDeviceService(db, new AuditEventService(db));
        var secret = Secret();
        var request = new RegisterDeviceRequest("terminal-reg", "Shared terminal", "ANDROID", "1.2.3",
            "Model-T", "14", "request-reg", secret);

        var first = await service.RegisterAsync(scope.Actor, request, Guid.NewGuid(), default);
        var replay = await service.RegisterAsync(scope.Actor, request, Guid.NewGuid(), default);

        Assert.Equal(first.Id, replay.Id);
        var stored = await db.RegisteredDevices.SingleAsync(x => x.Id == first.Id);
        Assert.NotEqual(secret, stored.CredentialHash);
        Assert.Equal(64, stored.CredentialHash.Length);
        Assert.DoesNotContain(secret, string.Join('|', await db.AuditEvents
            .Where(x => x.EntityId == first.Id).Select(x => (x.BeforeJson ?? "") + (x.AfterJson ?? "") + (x.Reason ?? ""))
            .ToListAsync()), StringComparison.Ordinal);
        var conflicts = new[]
        {
            request with { DeviceId = "terminal-reg-other" }, request with { DisplayName = "Changed" },
            request with { Platform = "IOS" }, request with { AppVersion = "9" },
            request with { DeviceModel = "Other" }, request with { OsVersion = "15" },
            request with { Credential = Secret() }
        };
        foreach (var conflict in conflicts)
        {
            var error = await Assert.ThrowsAsync<RegisteredDeviceException>(() =>
                service.RegisterAsync(scope.Actor, conflict, Guid.NewGuid(), default));
            Assert.Equal("DEVICE_REGISTRATION_CONFLICT", error.Code);
        }
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Approval_assignment_rotation_and_shared_terminal_are_scope_and_version_bound()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "FLOW", secondUser: true);
        var service = new RegisteredDeviceService(db, new AuditEventService(db));
        var oldSecret = Secret();
        var registered = await service.RegisterAsync(scope.Actor,
            new("terminal-flow", "Counter terminal", "WINDOWS", "5.0", null, "11", "request-flow", oldSecret),
            Guid.NewGuid(), default);
        await service.ApproveAsync(registered.Id, scope.Actor, Guid.NewGuid(), default);
        await service.AddAssignmentAsync(registered.Id, new(scope.UserId, scope.BranchId),
            scope.Actor, Guid.NewGuid(), default);
        var secondAssignment = await service.AddAssignmentAsync(registered.Id, new(scope.SecondUserId!.Value, scope.BranchId),
            scope.Actor, Guid.NewGuid(), default);

        Assert.NotNull(await service.ValidateBindingAsync(scope.UserId, scope.CompanyId, scope.BranchId,
            "terminal-flow", oldSecret, true, Guid.NewGuid(), default));
        Assert.NotNull(await service.ValidateBindingAsync(scope.SecondUserId.Value, scope.CompanyId, scope.BranchId,
            "terminal-flow", oldSecret, true, Guid.NewGuid(), default));
        await service.RemoveAssignmentAsync(registered.Id, secondAssignment.Id, scope.Actor, Guid.NewGuid(), default);
        Assert.Null(await service.ValidateBindingAsync(scope.SecondUserId.Value, scope.CompanyId, scope.BranchId,
            "terminal-flow", oldSecret, true, Guid.NewGuid(), default));
        var readded = await service.AddAssignmentAsync(registered.Id,
            new(scope.SecondUserId.Value, scope.BranchId), scope.Actor, Guid.NewGuid(), default);
        Assert.NotEqual(secondAssignment.Id, readded.Id);

        var session = NewSession(scope, registered.Id, 1, "terminal-flow");
        db.AuthSessions.Add(session);
        await db.SaveChangesAsync();
        var newSecret = Secret();
        var rotated = await service.RotateCredentialAsync(registered.Id, new(newSecret, 1),
            scope.Actor, Guid.NewGuid(), default);
        Assert.Equal(2, rotated.CredentialVersion);
        Assert.Null(await service.ValidateBindingAsync(scope.UserId, scope.CompanyId, scope.BranchId,
            "terminal-flow", oldSecret, true, Guid.NewGuid(), default));
        Assert.NotNull(await service.ValidateBindingAsync(scope.UserId, scope.CompanyId, scope.BranchId,
            "terminal-flow", newSecret, true, Guid.NewGuid(), default));
        Assert.NotNull((await db.AuthSessions.AsNoTracking().SingleAsync(x => x.Id == session.Id)).RevokedAt);

        await service.SuspendAsync(registered.Id, scope.Actor, Guid.NewGuid(), default);
        Assert.Null(await service.ValidateBindingAsync(scope.UserId, scope.CompanyId, scope.BranchId,
            "terminal-flow", newSecret, true, Guid.NewGuid(), default));
        var activeAgain = await service.ReactivateAsync(registered.Id, scope.Actor, Guid.NewGuid(), default);
        Assert.Equal("ACTIVE", activeAgain.Status);
        Assert.Null(activeAgain.ExpiresAt);
        Assert.NotNull(await service.ValidateBindingAsync(scope.UserId, scope.CompanyId, scope.BranchId,
            "terminal-flow", newSecret, true, Guid.NewGuid(), default));
        await service.RevokeAsync(registered.Id, scope.Actor, Guid.NewGuid(), default);
        Assert.Null(await service.ValidateBindingAsync(scope.UserId, scope.CompanyId, scope.BranchId,
            "terminal-flow", newSecret, true, Guid.NewGuid(), default));
        var terminal = await Assert.ThrowsAsync<RegisteredDeviceException>(() =>
            service.ReactivateAsync(registered.Id, scope.Actor, Guid.NewGuid(), default));
        Assert.Equal("DEVICE_REVOKED", terminal.Code);
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Last_seen_is_throttled_and_current_requires_exact_bound_session_context()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "SEEN");
        var service = new RegisteredDeviceService(db, new AuditEventService(db));
        var secret = Secret();
        var registered = await service.RegisterAsync(scope.Actor,
            new("terminal-seen", "Terminal", "IOS", "1", null, null, "request-seen", secret),
            Guid.NewGuid(), default);
        await service.ApproveAsync(registered.Id, scope.Actor, Guid.NewGuid(), default);
        await service.AddAssignmentAsync(registered.Id, new(scope.UserId, scope.BranchId),
            scope.Actor, Guid.NewGuid(), default);
        var before = await db.RegisteredDevices.AsNoTracking().Where(x => x.Id == registered.Id)
            .Select(x => x.LastSeenAt).SingleAsync();
        Assert.Null(await service.ValidateBindingAsync(scope.UserId, scope.CompanyId, scope.BranchId,
            "terminal-seen", Secret(), true, Guid.NewGuid(), default));
        Assert.Equal(before, await db.RegisteredDevices.AsNoTracking().Where(x => x.Id == registered.Id)
            .Select(x => x.LastSeenAt).SingleAsync());
        await service.ValidateBindingAsync(scope.UserId, scope.CompanyId, scope.BranchId,
            "terminal-seen", secret, true, Guid.NewGuid(), default);
        Assert.Equal(before, await db.RegisteredDevices.AsNoTracking().Where(x => x.Id == registered.Id)
            .Select(x => x.LastSeenAt).SingleAsync());

        await db.Database.ExecuteSqlInterpolatedAsync(
            $"UPDATE transport_erp.registered_devices SET \"LastSeenAt\"={DateTimeOffset.UtcNow.AddMinutes(-16)} WHERE \"Id\"={registered.Id}");
        db.ChangeTracker.Clear();
        await service.ValidateBindingAsync(scope.UserId, scope.CompanyId, scope.BranchId,
            "terminal-seen", secret, true, Guid.NewGuid(), default);
        var after = await db.RegisteredDevices.AsNoTracking().Where(x => x.Id == registered.Id)
            .Select(x => x.LastSeenAt).SingleAsync();
        Assert.True(after > DateTimeOffset.UtcNow.AddMinutes(-2));

        Assert.Null(await service.CurrentAsync(scope.Actor, default));
        var bound = scope.Actor with { SessionId = Guid.NewGuid(), RegisteredDeviceId = registered.Id,
            DeviceCredentialVersion = 1, DeviceId = "terminal-seen" };
        Assert.Equal(registered.Id, (await service.CurrentAsync(bound, default))!.Id);
        Assert.Null(await service.CurrentAsync(bound with { DeviceId = "another-device" }, default));

        var session = NewSession(scope, registered.Id, 1, "terminal-seen");
        db.AuthSessions.Add(session);
        await db.SaveChangesAsync();
        await db.Database.ExecuteSqlInterpolatedAsync(
            $"UPDATE transport_erp.registered_devices SET \"LastSeenAt\"={DateTimeOffset.UtcNow.AddDays(-91)} WHERE \"Id\"={registered.Id}");
        db.ChangeTracker.Clear();
        Assert.Null(await service.ValidateBindingAsync(scope.UserId, scope.CompanyId, scope.BranchId,
            "terminal-seen", secret, true, Guid.NewGuid(), default));
        Assert.Equal("EXPIRED", await db.RegisteredDevices.AsNoTracking().Where(x => x.Id == registered.Id)
            .Select(x => x.Status).SingleAsync());
        Assert.NotNull((await db.AuthSessions.AsNoTracking().SingleAsync(x => x.Id == session.Id)).RevokedAt);
        Assert.True(await db.AuditEvents.AnyAsync(x => x.Action == "RegisteredDeviceExpired" && x.EntityId == registered.Id));
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Concurrent_device_registration_has_one_winner_and_cross_tenant_assignment_is_rejected()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var seedDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await seedDb.Database.MigrateAsync();
        var scope = await SeedAsync(seedDb, "RACE");
        var secret = Secret();
        var requestA = new RegisterDeviceRequest("terminal-race", "Terminal", "LINUX", "1", null, null,
            "request-a", secret);
        var requestB = requestA with { RegistrationRequestId = "request-b" };
        async Task<(bool Success, string? Code, Guid? Id)> Run(RegisterDeviceRequest request)
        {
            await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
            try
            {
                var result = await new RegisteredDeviceService(db, new AuditEventService(db))
                    .RegisterAsync(scope.Actor, request, Guid.NewGuid(), default);
                return (true, null, result.Id);
            }
            catch (RegisteredDeviceException ex) { return (false, ex.Code, null); }
        }
        var identical = await Task.WhenAll(Run(requestA), Run(requestA));
        Assert.All(identical, x => Assert.True(x.Success));
        Assert.Single(identical.Select(x => x.Id).Distinct());
        Assert.Equal(1, await seedDb.AuditEvents.CountAsync(x => x.Action == "RegisteredDeviceCreated" &&
            x.EntityId == identical[0].Id));
        var conflictResult = await Run(requestB);
        Assert.False(conflictResult.Success);
        Assert.Equal("DEVICE_REGISTRATION_CONFLICT", conflictResult.Code);

        await using var otherDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
        var other = await SeedAsync(otherDb, "OTHER");
        var deviceId = await seedDb.RegisteredDevices.Where(x => x.CompanyId == scope.CompanyId && x.DeviceId == "terminal-race")
            .Select(x => x.Id).SingleAsync();
        var service = new RegisteredDeviceService(seedDb, new AuditEventService(seedDb));
        var exception = await Assert.ThrowsAsync<RegisteredDeviceException>(() => service.AddAssignmentAsync(deviceId,
            new(other.UserId, scope.BranchId), scope.Actor, Guid.NewGuid(), default));
        Assert.Equal("ASSIGNMENT_SCOPE_INVALID", exception.Code);
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Login_binding_failure_does_not_record_success_and_bound_refresh_requires_device_secret()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "AUTH");
        var hasher = new PasswordHasher<User>();
        var user = await db.Users.SingleAsync(x => x.Id == scope.UserId);
        user.PasswordHash = hasher.HashPassword(user, "Correct-Horse-42!");
        user.AccessFailedCount = 3;
        await db.SaveChangesAsync();
        var devices = new RegisteredDeviceService(db, new AuditEventService(db));
        var secret = Secret();
        var registered = await devices.RegisterAsync(scope.Actor,
            new("terminal-auth", "Auth terminal", "ANDROID", "2", null, null, "request-auth", secret),
            Guid.NewGuid(), default);
        await devices.ApproveAsync(registered.Id, scope.Actor, Guid.NewGuid(), default);
        await devices.AddAssignmentAsync(registered.Id, new(scope.UserId, scope.BranchId),
            scope.Actor, Guid.NewGuid(), default);
        var identity = new IdentitySessionService(db, hasher, new IdentityPasswordSentinel(hasher),
            new TenantScopeResolver(db, new EffectivePermissionResolver(db)), new AuditEventService(db),
            Options.Create(SecurityOptions()), devices);

        var denied = new CreateIdentitySessionRequest(user.UserName, "Correct-Horse-42!", scope.CompanyId,
            scope.BranchId, "terminal-auth", Secret());
        var failure = await Assert.ThrowsAsync<IdentitySessionException>(() =>
            identity.CreateAsync(denied, Guid.NewGuid(), "127.0.0.1", default));
        Assert.Equal("INVALID_CREDENTIALS", failure.Code);
        db.ChangeTracker.Clear();
        var afterFailure = await db.Users.AsNoTracking().SingleAsync(x => x.Id == scope.UserId);
        Assert.Null(afterFailure.LastLoginAt);
        Assert.Equal(3, afterFailure.AccessFailedCount);

        var login = await identity.CreateAsync(denied with { DeviceCredential = secret },
            Guid.NewGuid(), "127.0.0.1", default);
        var persistedSession = await db.AuthSessions.AsNoTracking().SingleAsync(x => x.Id == login.SessionId);
        Assert.Equal(registered.Id, persistedSession.RegisteredDeviceId);
        Assert.Equal(1, persistedSession.DeviceCredentialVersion);
        await Assert.ThrowsAsync<IdentitySessionException>(() => identity.RefreshAsync(
            new(login.RefreshToken, "terminal-auth"), Guid.NewGuid(), "127.0.0.1", default));
        await Assert.ThrowsAsync<IdentitySessionException>(() => identity.RefreshAsync(
            new(login.RefreshToken, "terminal-auth", Secret()), Guid.NewGuid(), "127.0.0.1", default));
        var refreshed = await identity.RefreshAsync(new(login.RefreshToken, "terminal-auth", secret),
            Guid.NewGuid(), "127.0.0.1", default);
        Assert.NotEqual(login.SessionId, refreshed.SessionId);
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Database_guards_reject_cross_company_assignment_and_sync_actor()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var owner = await SeedAsync(db, "GUARD-A");
        var other = await SeedAsync(db, "GUARD-B");
        var service = new RegisteredDeviceService(db, new AuditEventService(db));
        var device = await service.RegisterAsync(owner.Actor,
            new("terminal-guard", "Guard terminal", "TEST", "1", null, null, "request-guard", Secret()),
            Guid.NewGuid(), default);

        var ownerUser = await db.Users.SingleAsync(x => x.Id == owner.UserId);
        ownerUser.CompanyId = other.CompanyId;
        ownerUser.BranchId = other.BranchId;
        await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
        db.ChangeTracker.Clear();

        db.RegisteredDeviceAssignments.Add(new RegisteredDeviceAssignment
        {
            Id = Guid.NewGuid(), RegisteredDeviceId = device.Id, UserId = other.UserId,
            CompanyId = owner.CompanyId, BranchId = owner.BranchId, Status = "ACTIVE",
            AssignedByUserId = owner.UserId, AssignedAt = DateTimeOffset.UtcNow,
            CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        });
        await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
        db.ChangeTracker.Clear();

        var payload = "{}";
        db.SyncOperations.Add(new SyncOperation
        {
            Id = Guid.NewGuid(), DeviceId = "guard-direct", UserId = other.UserId,
            CompanyId = owner.CompanyId, BranchId = owner.BranchId, OperationType = "UPDATE",
            EntityType = "Guard", EntityId = Guid.NewGuid(), ClientOperationId = $"guard-{Guid.NewGuid():N}",
            PayloadJson = payload, PayloadHash = Convert.ToHexString(SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(payload))).ToLowerInvariant(),
            ClientOccurredAt = DateTimeOffset.UtcNow, ServerReceivedAt = DateTimeOffset.UtcNow,
            Status = "QUEUED", CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        });
        await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Refresh_racing_credential_rotation_has_no_deadlock_or_surviving_stale_session()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var seedDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await seedDb.Database.MigrateAsync();
        var scope = await SeedAsync(seedDb, "LOCKRACE");
        var hasher = new PasswordHasher<User>();
        var user = await seedDb.Users.SingleAsync(x => x.Id == scope.UserId);
        user.PasswordHash = hasher.HashPassword(user, "Race-Password-42!");
        await seedDb.SaveChangesAsync();
        var oldSecret = Secret();
        var deviceService = new RegisteredDeviceService(seedDb, new AuditEventService(seedDb));
        var device = await deviceService.RegisterAsync(scope.Actor,
            new("terminal-lock-race", "Race", "TEST", "1", null, null, "request-lock-race", oldSecret),
            Guid.NewGuid(), default);
        await deviceService.ApproveAsync(device.Id, scope.Actor, Guid.NewGuid(), default);
        await deviceService.AddAssignmentAsync(device.Id, new(scope.UserId, scope.BranchId),
            scope.Actor, Guid.NewGuid(), default);
        var identity = CreateIdentity(seedDb, hasher, deviceService);
        var login = await identity.CreateAsync(new(user.UserName, "Race-Password-42!", scope.CompanyId,
            scope.BranchId, "terminal-lock-race", oldSecret), Guid.NewGuid(), "127.0.0.1", default);
        var newSecret = Secret();

        async Task<Exception?> Refresh()
        {
            await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
            var localHasher = new PasswordHasher<User>();
            var devices = new RegisteredDeviceService(db, new AuditEventService(db));
            try
            {
                await CreateIdentity(db, localHasher, devices).RefreshAsync(
                    new(login.RefreshToken, "terminal-lock-race", oldSecret), Guid.NewGuid(), "127.0.0.1", default);
                return null;
            }
            catch (Exception ex) { return ex; }
        }
        async Task<Exception?> Rotate()
        {
            await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
            try
            {
                await new RegisteredDeviceService(db, new AuditEventService(db)).RotateCredentialAsync(device.Id,
                    new(newSecret, 1), scope.Actor, Guid.NewGuid(), default);
                return null;
            }
            catch (Exception ex) { return ex; }
        }
        var outcomes = await Task.WhenAll(Refresh(), Rotate());
        Assert.Null(outcomes[1]);
        Assert.True(outcomes[0] is null or IdentitySessionException);
        Assert.DoesNotContain(outcomes, x => x is Npgsql.PostgresException { SqlState: "40P01" } ||
            x?.GetBaseException() is Npgsql.PostgresException { SqlState: "40P01" });
        await using var verify = PostgreSqlTestEnvironment.CreateDbContext(connection);
        Assert.False(await verify.AuthSessions.AnyAsync(x => x.RegisteredDeviceId == device.Id && x.RevokedAt == null));
        Assert.Equal(2, await verify.RegisteredDevices.Where(x => x.Id == device.Id).Select(x => x.CredentialVersion).SingleAsync());
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task External_authority_device_claims_never_create_a_trusted_registered_device_binding()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "EXTERNAL");
        var secret = Secret();
        var devices = new RegisteredDeviceService(db, new AuditEventService(db));
        var device = await devices.RegisterAsync(scope.Actor,
            new("external-spoof", "External spoof", "TEST", "1", null, null, "external-request", secret),
            Guid.NewGuid(), default);
        await devices.ApproveAsync(device.Id, scope.Actor, Guid.NewGuid(), default);
        await devices.AddAssignmentAsync(device.Id, new(scope.UserId, scope.BranchId), scope.Actor,
            Guid.NewGuid(), default);
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, scope.UserId.ToString()),
            new Claim("company_id", scope.CompanyId.ToString()),
            new Claim("branch_id", scope.BranchId.ToString()),
            new Claim("device_id", device.DeviceId),
            new Claim("registered_device_id", device.Id.ToString()),
            new Claim("device_credential_version", device.CredentialVersion.ToString())
        }, "ExternalAuthority");
        var resolver = new CurrentSecurityContextService(db, new EffectivePermissionResolver(db),
            Options.Create(new TransportSecurityOptions { Mode = TransportAuthMode.ExternalAuthority }));

        var current = await resolver.ResolveAsync(new ClaimsPrincipal(identity));

        Assert.NotNull(current);
        Assert.False(current!.IsLocalSession);
        Assert.Null(current.SessionId);
        Assert.Null(current.RegisteredDeviceId);
        Assert.Null(current.DeviceCredentialVersion);
        Assert.Null(await devices.CurrentAsync(current, default));
        Assert.Null(await new RegisteredDeviceTrustResolver(devices).ResolveForSyncAsync(current,
            device.DeviceId, secret, Guid.NewGuid(), default));
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Device_assignment_current_and_sync_are_isolated_between_branches_of_one_company()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "BRANCH");
        var now = DateTimeOffset.UtcNow;
        var otherBranch = new Branch
        {
            Id = Guid.NewGuid(), CompanyId = scope.CompanyId, Code = $"B-{Guid.NewGuid():N}"[..12],
            NameAr = "فرع آخر", Timezone = "Asia/Riyadh", Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        db.Branches.Add(otherBranch);
        await db.SaveChangesAsync();
        var secret = Secret();
        var devices = new RegisteredDeviceService(db, new AuditEventService(db));
        var device = await devices.RegisterAsync(scope.Actor,
            new("branch-terminal", "Branch terminal", "TEST", "1", null, null, "branch-request", secret),
            Guid.NewGuid(), default);
        await devices.ApproveAsync(device.Id, scope.Actor, Guid.NewGuid(), default);
        await devices.AddAssignmentAsync(device.Id, new(scope.UserId, scope.BranchId), scope.Actor,
            Guid.NewGuid(), default);

        var assignmentError = await Assert.ThrowsAsync<RegisteredDeviceException>(() =>
            devices.AddAssignmentAsync(device.Id, new(scope.UserId, otherBranch.Id), scope.Actor,
                Guid.NewGuid(), default));
        Assert.Equal("ASSIGNMENT_SCOPE_INVALID", assignmentError.Code);
        var wrongBranchContext = scope.Actor with
        {
            BranchId = otherBranch.Id, SessionId = Guid.NewGuid(), DeviceId = device.DeviceId,
            IsLocalSession = true, RegisteredDeviceId = device.Id,
            DeviceCredentialVersion = device.CredentialVersion
        };
        Assert.Null(await devices.CurrentAsync(wrongBranchContext, default));
        Assert.Null(await devices.ValidateBindingAsync(scope.UserId, scope.CompanyId, otherBranch.Id,
            device.DeviceId, secret, false, Guid.NewGuid(), default));

        var payload = "{\"branch\":\"wrong\"}";
        var command = new EnqueueSyncOperationCommand(device.DeviceId, scope.UserId, scope.CompanyId,
            otherBranch.Id, "UPDATE", "BranchIsolation", Guid.NewGuid(), $"branch-{Guid.NewGuid():N}",
            payload, Convert.ToHexString(SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(payload))).ToLowerInvariant(),
            DateTimeOffset.UtcNow, 1);
        var sync = new SyncOperationService(db, new AuditEventService(db),
            new SyncRetryPolicy(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30)));
        var syncError = await Assert.ThrowsAsync<SyncRuleException>(() => sync.EnqueueSyncOperationAsync(command,
            new SyncSecurityContext(scope.UserId, device.DeviceId, scope.CompanyId, otherBranch.Id,
                true, true, device.Id, device.CredentialVersion)));
        Assert.Contains(syncError.Code, new[] { "USER_NOT_FOUND", "DEVICE_NOT_REGISTERED", "SCOPE_DENIED" });
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Registered_device_migration_round_trips_and_enforces_new_sync_provenance_without_blocking_legacy_rows()
    {
        const string Previous = "20260825220000_P1SecurityIdentity";
        const string Current = "20260826010000_P1RegisteredDevices";
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var migrator = db.GetService<IMigrator>();
        await migrator.MigrateAsync(Previous);

        var now = DateTimeOffset.UtcNow;
        var currency = new Currency
        {
            Id = Guid.NewGuid(), Code = await PostgreSqlTestCurrencyCodeAllocator.NextAsync(db), NameAr = "عملة",
            MinorUnit = 2, IsBase = true, Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var company = new Company
        {
            Id = Guid.NewGuid(), Code = $"MG-{Guid.NewGuid():N}"[..18], LegalNameAr = "شركة ترحيل",
            BaseCurrencyId = currency.Id, DefaultCalendarId = Guid.NewGuid(), Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var adminRole = new Role
        {
            Id = Guid.NewGuid(), Code = "SYSTEM_ADMIN", NameAr = "مدير النظام", IsSystem = true,
            CompanyId = company.Id, Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var branch = new Branch
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, Code = $"MB-{Guid.NewGuid():N}"[..12],
            NameAr = "فرع الترحيل", Timezone = "Asia/Riyadh", Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var userName = $"migration-{Guid.NewGuid():N}";
        var user = new User
        {
            Id = Guid.NewGuid(), UserName = userName, NormalizedUserName = userName.ToUpperInvariant(),
            DisplayName = "مستخدم الترحيل", PasswordHash = "test-only",
            SecurityStamp = Guid.NewGuid().ToString("N"), AuthVersion = 1, Status = "ACTIVE",
            CompanyId = company.Id, BranchId = branch.Id, CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        db.AddRange(currency, company, branch, user, adminRole);
        await db.SaveChangesAsync();

        var historicalId = Guid.NewGuid();
        var historicalPayload = "{\"legacy\":true}";
        var historicalHash = Convert.ToHexString(SHA256.HashData(
            System.Text.Encoding.UTF8.GetBytes(historicalPayload))).ToLowerInvariant();
        await db.Database.ExecuteSqlInterpolatedAsync($"""
            INSERT INTO transport_erp.sync_operations
              ("Id","DeviceId","UserId","CompanyId","BranchId","OperationType","EntityType","EntityId",
               "ClientOperationId","PayloadJson","PayloadHash","ClientOccurredAt","ServerReceivedAt","Status",
               "RetryCount","CreatedAt","UpdatedAt","RowVersion")
            VALUES ({historicalId},{"legacy-device"},{user.Id},{company.Id},{branch.Id},{"UPDATE"},{"Legacy"},
                    {Guid.NewGuid()},{"legacy-" + Guid.NewGuid().ToString("N")},{historicalPayload},{historicalHash},
                    {now},{now},{"QUEUED"},{0},{now},{now},{RandomNumberGenerator.GetBytes(16)})
            """);

        await migrator.MigrateAsync(Current);
        await AssertDeviceMigrationStateAsync(db, adminRole.Id, company.Id, present: true);

        db.ChangeTracker.Clear();
        var historical = await db.SyncOperations.SingleAsync(x => x.Id == historicalId);
        Assert.Null(historical.RegisteredDeviceId);
        Assert.Null(historical.RegisteredDeviceCredentialVersion);
        historical.Status = "REJECTED";
        historical.ErrorCode = "LEGACY_TERMINAL";
        historical.UpdatedAt = DateTimeOffset.UtcNow;
        historical.RowVersion = RandomNumberGenerator.GetBytes(16);
        await db.SaveChangesAsync();
        db.ChangeTracker.Clear();
        Assert.Equal("REJECTED", await db.SyncOperations.Where(x => x.Id == historicalId)
            .Select(x => x.Status).SingleAsync());

        var unbound = NewDirectSync(user.Id, company.Id, branch.Id, "new-unbound", now);
        db.SyncOperations.Add(unbound);
        await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
        db.ChangeTracker.Clear();
        Assert.False(await db.SyncOperations.AsNoTracking().AnyAsync(x => x.Id == unbound.Id));

        var registered = new RegisteredDevice
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, DeviceId = "migration-bound-device",
            DisplayName = "جهاز مربوط", Platform = "TEST", AppVersion = "1",
            RegistrationRequestId = $"migration-{Guid.NewGuid():N}", CredentialHash = new string('a', 64),
            CredentialVersion = 1, Status = "ACTIVE", RegisteredByUserId = user.Id,
            ApprovedByUserId = user.Id, ApprovedAt = now, LastSeenAt = now,
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var assignment = new RegisteredDeviceAssignment
        {
            Id = Guid.NewGuid(), RegisteredDeviceId = registered.Id, UserId = user.Id,
            CompanyId = company.Id, BranchId = branch.Id, Status = "ACTIVE", AssignedByUserId = user.Id,
            AssignedAt = now, CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        db.AddRange(registered, assignment);
        await db.SaveChangesAsync();
        var bound = NewDirectSync(user.Id, company.Id, branch.Id, registered.DeviceId, now);
        bound.RegisteredDeviceId = registered.Id;
        bound.RegisteredDeviceCredentialVersion = registered.CredentialVersion;
        db.SyncOperations.Add(bound);
        await db.SaveChangesAsync();
        Assert.True(await db.SyncOperations.AsNoTracking().AnyAsync(x => x.Id == bound.Id));

        await migrator.MigrateAsync(Previous);
        await AssertDeviceMigrationStateAsync(db, adminRole.Id, company.Id, present: false);
        await migrator.MigrateAsync(Current);
        await AssertDeviceMigrationStateAsync(db, adminRole.Id, company.Id, present: true);
    }

    private static SyncOperation NewDirectSync(Guid userId, Guid companyId, Guid branchId,
        string deviceId, DateTimeOffset now)
    {
        var payload = "{\"direct\":true}";
        return new SyncOperation
        {
            Id = Guid.NewGuid(), DeviceId = deviceId, UserId = userId, CompanyId = companyId,
            BranchId = branchId, OperationType = "UPDATE", EntityType = "Direct",
            EntityId = Guid.NewGuid(), ClientOperationId = $"direct-{Guid.NewGuid():N}",
            PayloadJson = payload, PayloadHash = Convert.ToHexString(SHA256.HashData(
                System.Text.Encoding.UTF8.GetBytes(payload))).ToLowerInvariant(),
            ClientOccurredAt = now, ServerReceivedAt = now, Status = "QUEUED", RetryCount = 0,
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
    }

    private static async Task AssertDeviceMigrationStateAsync(TransportErpDbContext db, Guid adminRoleId,
        Guid companyId, bool present)
    {
        var codes = new[] { "devices.register", "devices.read", "devices.manage" };
        var permissionIds = new[]
        {
            Guid.Parse("d1000000-0000-4000-8000-000000000001"),
            Guid.Parse("d1000000-0000-4000-8000-000000000002"),
            Guid.Parse("d1000000-0000-4000-8000-000000000003")
        };
        db.ChangeTracker.Clear();
        var permissions = await db.Permissions.IgnoreQueryFilters().Where(x => codes.Contains(x.Code))
            .OrderBy(x => x.Code).ToListAsync();
        if (!present)
        {
            Assert.Empty(permissions);
            Assert.False(await db.RolePermissions.AnyAsync(x => permissionIds.Contains(x.PermissionId)));
            Assert.Equal(0, await db.Database.SqlQueryRaw<int>(
                "SELECT count(*)::int AS \"Value\" FROM pg_class c JOIN pg_namespace n ON n.oid=c.relnamespace WHERE n.nspname='transport_erp' AND c.relname IN ('registered_devices','registered_device_assignments')")
                .SingleAsync());
            Assert.Equal(0, await db.Database.SqlQueryRaw<int>(
                "SELECT count(*)::int AS \"Value\" FROM information_schema.columns WHERE table_schema='transport_erp' AND ((table_name='auth_sessions' AND column_name IN ('RegisteredDeviceId','DeviceCredentialVersion')) OR (table_name='sync_operations' AND column_name IN ('RegisteredDeviceId','RegisteredDeviceCredentialVersion'))) ")
                .SingleAsync());
            Assert.Equal(0, await db.Database.SqlQueryRaw<int>(
                "SELECT count(*)::int AS \"Value\" FROM pg_trigger WHERE NOT tgisinternal AND tgname IN ('trg_registered_devices_user_scope','trg_registered_device_assignments_user_scope','trg_auth_sessions_user_scope','trg_sync_operations_user_scope','trg_sync_operations_device_binding','trg_users_prevent_scope_reference_drift')")
                .SingleAsync());
            return;
        }

        Assert.Equal(codes.OrderBy(x => x), permissions.Select(x => x.Code));
        Assert.Equal(permissionIds.OrderBy(x => x), permissions.Select(x => x.Id).OrderBy(x => x));
        Assert.All(permissions, permission =>
        {
            Assert.Equal("COMPANY", permission.ScopeType);
            Assert.Equal("devices", permission.Resource);
            Assert.True(permission.IsSystem);
            Assert.Equal("ACTIVE", permission.Status);
            Assert.Equal(permission.Code.Split('.')[1], permission.Action);
            Assert.Null(permission.DeletedAt);
            Assert.Equal(permission.Code switch
            {
                "devices.register" => "تسجيل جهاز",
                "devices.read" => "عرض الأجهزة",
                "devices.manage" => "إدارة الأجهزة",
                _ => throw new InvalidOperationException(permission.Code)
            }, permission.NameAr);
        });
        var grants = await db.RolePermissions.Where(x => x.RoleId == adminRoleId && permissionIds.Contains(x.PermissionId))
            .ToListAsync();
        Assert.Equal(3, grants.Count);
        Assert.All(grants, grant =>
        {
            Assert.Equal("COMPANY", grant.ScopeType);
            Assert.Equal((Guid?)companyId, grant.CompanyId);
            Assert.Null(grant.BranchId);
        });
        Assert.Equal(6, await db.Database.SqlQueryRaw<int>(
            "SELECT count(*)::int AS \"Value\" FROM pg_trigger WHERE NOT tgisinternal AND tgname IN ('trg_registered_devices_user_scope','trg_registered_device_assignments_user_scope','trg_auth_sessions_user_scope','trg_sync_operations_user_scope','trg_sync_operations_device_binding','trg_users_prevent_scope_reference_drift')")
            .SingleAsync());
    }

    [Theory]
    [InlineData("approve", false)]
    [InlineData("add", false)]
    [InlineData("suspend", false)]
    [InlineData("suspend", true)]
    [InlineData("rotate", false)]
    [InlineData("rotate", true)]
    [InlineData("remove", false)]
    [InlineData("remove", true)]
    [InlineData("revoke", false)]
    [InlineData("revoke", true)]
    [Trait("Category", "PostgreSQL")]
    public async Task Identity_and_device_mutations_use_one_lock_order_without_deadlock_or_stale_session(
        string mutation, bool useRefresh)
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var seedDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await seedDb.Database.MigrateAsync();
        var scope = await SeedAsync(seedDb, $"DL{mutation[..2].ToUpperInvariant()}{(useRefresh ? "R" : "L")}");
        var hasher = new PasswordHasher<User>();
        var user = await seedDb.Users.SingleAsync(x => x.Id == scope.UserId);
        const string password = "Deadlock-Proof-42!";
        user.PasswordHash = hasher.HashPassword(user, password);
        await seedDb.SaveChangesAsync();
        var secret = Secret();
        var seedDevices = new RegisteredDeviceService(seedDb, new AuditEventService(seedDb));
        var device = await seedDevices.RegisterAsync(scope.Actor,
            new($"terminal-{Guid.NewGuid():N}", "Lock order", "TEST", "1", null, null,
                $"request-{Guid.NewGuid():N}", secret), Guid.NewGuid(), default);
        RegisteredDeviceAssignmentResponse? assignment = null;
        if (mutation != "approve")
            await seedDevices.ApproveAsync(device.Id, scope.Actor, Guid.NewGuid(), default);
        if (mutation != "add")
            assignment = await seedDevices.AddAssignmentAsync(device.Id, new(scope.UserId, scope.BranchId),
                scope.Actor, Guid.NewGuid(), default);

        IdentitySessionResponse? existing = null;
        if (useRefresh)
        {
            existing = await CreateIdentity(seedDb, hasher, seedDevices).CreateAsync(
                new(user.UserName, password, scope.CompanyId, scope.BranchId, device.DeviceId, secret),
                Guid.NewGuid(), "127.0.0.1", default);
        }

        var start = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        async Task<Exception?> IdentityAction()
        {
            await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
            var localHasher = new PasswordHasher<User>();
            var devices = new RegisteredDeviceService(db, new AuditEventService(db));
            await start.Task;
            try
            {
                var identity = CreateIdentity(db, localHasher, devices);
                if (useRefresh)
                    await identity.RefreshAsync(new(existing!.RefreshToken, device.DeviceId, secret),
                        Guid.NewGuid(), "127.0.0.1", default);
                else
                    await identity.CreateAsync(new(user.UserName, password, scope.CompanyId, scope.BranchId,
                        device.DeviceId, secret), Guid.NewGuid(), "127.0.0.1", default);
                return null;
            }
            catch (Exception ex) { return ex; }
        }
        async Task<Exception?> DeviceAction()
        {
            await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
            var devices = new RegisteredDeviceService(db, new AuditEventService(db));
            await start.Task;
            try
            {
                switch (mutation)
                {
                    case "approve":
                        await devices.ApproveAsync(device.Id, scope.Actor, Guid.NewGuid(), default);
                        break;
                    case "add":
                        await devices.AddAssignmentAsync(device.Id, new(scope.UserId, scope.BranchId),
                            scope.Actor, Guid.NewGuid(), default);
                        break;
                    case "suspend":
                        await devices.SuspendAsync(device.Id, scope.Actor, Guid.NewGuid(), default);
                        break;
                    case "rotate":
                        await devices.RotateCredentialAsync(device.Id, new(Secret(), 1), scope.Actor,
                            Guid.NewGuid(), default);
                        break;
                    case "remove":
                        await devices.RemoveAssignmentAsync(device.Id, assignment!.Id, scope.Actor,
                            Guid.NewGuid(), default);
                        break;
                    case "revoke":
                        await devices.RevokeAsync(device.Id, scope.Actor, Guid.NewGuid(), default);
                        break;
                    default:
                        throw new InvalidOperationException(mutation);
                }
                return null;
            }
            catch (Exception ex) { return ex; }
        }

        var identityTask = IdentityAction();
        var deviceTask = DeviceAction();
        start.SetResult();
        var outcomes = await Task.WhenAll(identityTask, deviceTask);
        Assert.Null(outcomes[1]);
        Assert.True(outcomes[0] is null or IdentitySessionException);
        Assert.DoesNotContain(outcomes, IsDeadlock);

        await using var verify = PostgreSqlTestEnvironment.CreateDbContext(connection);
        if (mutation is "suspend" or "rotate" or "remove" or "revoke")
            Assert.False(await verify.AuthSessions.AnyAsync(x => x.RegisteredDeviceId == device.Id && x.RevokedAt == null));
    }

    private static bool IsDeadlock(Exception? exception)
        => exception is Npgsql.PostgresException { SqlState: "40P01" } ||
           exception?.GetBaseException() is Npgsql.PostgresException { SqlState: "40P01" };

    [Fact]
    public async Task Stage3_offline_policy_is_hard_disabled_even_when_configuration_might_be_true()
    {
        var policy = new OfflineSyncPolicyService();
        Assert.False(await policy.IsEnabledAsync(Guid.NewGuid(), default));
    }

    private static AuthSession NewSession(Scope scope, Guid deviceId, int version, string textualDeviceId)
    {
        var now = DateTimeOffset.UtcNow;
        return new AuthSession
        {
            Id = Guid.NewGuid(), UserId = scope.UserId, CompanyId = scope.CompanyId, BranchId = scope.BranchId,
            DeviceId = textualDeviceId, Mode = "LOCAL", SecurityStampAtIssue = scope.SecurityStamp,
            AuthVersionAtIssue = 1, RefreshTokenHash = Convert.ToHexString(RandomNumberGenerator.GetBytes(32)).ToLowerInvariant(),
            RefreshTokenFamilyId = Guid.NewGuid(), IssuedAt = now, AccessTokenExpiresAt = now.AddMinutes(15),
            RefreshTokenExpiresAt = now.AddDays(30), RegisteredDeviceId = deviceId,
            DeviceCredentialVersion = version, CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
    }

    private static string Secret() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

    private static TransportSecurityOptions SecurityOptions() => new()
    {
        Mode = TransportAuthMode.LocalSessions, Issuer = "stage3-tests", Audience = "stage3-tests",
        SigningKeyId = "current", SigningKey = "01234567890123456789012345678901",
        AccessTokenMinutes = 15, RefreshTokenDays = 30, MaxFailures = 5, LockoutMinutes = 15
    };

    private static IdentitySessionService CreateIdentity(TransportErpDbContext db, IPasswordHasher<User> hasher,
        RegisteredDeviceService devices) => new(db, hasher, new IdentityPasswordSentinel(hasher),
        new TenantScopeResolver(db, new EffectivePermissionResolver(db)), new AuditEventService(db),
        Options.Create(SecurityOptions()), devices);

    private static async Task<Scope> SeedAsync(TransportErpDbContext db, string suffix, bool secondUser = false)
    {
        var now = DateTimeOffset.UtcNow;
        var currency = new Currency { Id = Guid.NewGuid(), Code = await PostgreSqlTestCurrencyCodeAllocator.NextAsync(db),
            NameAr = "عملة", MinorUnit = 2, IsBase = true, Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16) };
        var company = new Company { Id = Guid.NewGuid(), Code = $"D-{suffix}-{Guid.NewGuid():N}"[..18],
            LegalNameAr = "شركة أجهزة", BaseCurrencyId = currency.Id, DefaultCalendarId = Guid.NewGuid(), Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16) };
        var branch = new Branch { Id = Guid.NewGuid(), CompanyId = company.Id, Code = $"B-{Guid.NewGuid():N}"[..12],
            NameAr = "فرع", Timezone = "Asia/Riyadh", Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16) };
        var stamp = Guid.NewGuid().ToString("N");
        var user = NewUser(company.Id, branch.Id, stamp, now);
        var second = secondUser ? NewUser(company.Id, branch.Id, Guid.NewGuid().ToString("N"), now) : null;
        db.AddRange(currency, company, branch, user);
        if (second is not null) db.Users.Add(second);
        await db.SaveChangesAsync();
        return new Scope(company.Id, branch.Id, user.Id, second?.Id, stamp,
            new CurrentSecurityContext(user.Id, company.Id, branch.Id, Guid.NewGuid(), "online-only", true));
    }

    private static User NewUser(Guid companyId, Guid branchId, string stamp, DateTimeOffset now)
    {
        var userName = $"device-{Guid.NewGuid():N}";
        return new() { Id = Guid.NewGuid(), UserName = userName,
            NormalizedUserName = userName.ToUpperInvariant(), DisplayName = "Device operator",
            PasswordHash = "test-only", SecurityStamp = stamp, AuthVersion = 1, Status = "ACTIVE",
            CompanyId = companyId, BranchId = branchId, CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16) };
    }

    private sealed record Scope(Guid CompanyId, Guid BranchId, Guid UserId, Guid? SecondUserId,
        string SecurityStamp, CurrentSecurityContext Actor);
}
