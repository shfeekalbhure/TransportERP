using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TransportERP.Api.Identity;
using TransportERP.Api.Security;
using TransportERP.Contracts.Identity;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

[Collection("PostgreSql")]
public sealed class P1SecurityHttpPostgreSqlTests
{
    private const string Password = "P1-security-test-password!";

    [Fact]
    [Trait("Category", "PostgreSQL")]
    [Trait("Category", "HTTP")]
    public async Task Real_identity_pipeline_enforces_unified_login_refresh_stamp_and_self_revoke()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "PIPE");
        using var factory = CreateFactory(connection);
        using var client = factory.CreateClient();

        var login = await LoginAsync(client, scope, Password, scope.CompanyId, scope.BranchId, "device-pipe");
        Assert.Equal(HttpStatusCode.OK, login.Response.StatusCode);
        Assert.NotNull(login.Session);

        await AssertInvalidCredentialsAsync(await client.PostAsJsonAsync("/api/v1/auth/sessions",
            new CreateIdentitySessionRequest("unknown-user", Password, scope.CompanyId, scope.BranchId, "unknown-device")));
        await AssertInvalidCredentialsAsync((await LoginAsync(client, scope, "wrong-password", scope.CompanyId,
            scope.BranchId, "wrong-device")).Response);
        await AssertInvalidCredentialsAsync((await LoginAsync(client, scope, Password, Guid.NewGuid(),
            scope.BranchId, "scope-device")).Response);

        await db.Entry(scope.User).ReloadAsync();
        scope.User.LockoutEnd = DateTimeOffset.UtcNow.AddMinutes(15);
        scope.User.UpdatedAt = DateTimeOffset.UtcNow;
        scope.User.RowVersion = Guid.NewGuid().ToByteArray();
        await db.SaveChangesAsync();
        await AssertInvalidCredentialsAsync((await LoginAsync(client, scope, Password, scope.CompanyId,
            scope.BranchId, "locked-device")).Response);
        scope.User.LockoutEnd = null;
        scope.User.Status = "ACTIVE";
        scope.User.UpdatedAt = DateTimeOffset.UtcNow;
        scope.User.RowVersion = Guid.NewGuid().ToByteArray();
        await db.SaveChangesAsync();

        var rotatedResponse = await client.PostAsJsonAsync("/api/v1/auth/sessions:refresh",
            new RefreshIdentitySessionRequest(login.Session!.RefreshToken, "device-pipe"));
        Assert.Equal(HttpStatusCode.OK, rotatedResponse.StatusCode);
        var rotated = await rotatedResponse.Content.ReadFromJsonAsync<IdentitySessionResponse>();
        Assert.NotNull(rotated);

        Assert.Equal(HttpStatusCode.Unauthorized, (await client.PostAsJsonAsync("/api/v1/auth/sessions:refresh",
            new RefreshIdentitySessionRequest(login.Session.RefreshToken, "device-pipe"))).StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.PostAsJsonAsync("/api/v1/auth/sessions:refresh",
            new RefreshIdentitySessionRequest(rotated!.RefreshToken, "device-pipe"))).StatusCode);

        var stampLogin = await LoginAsync(client, scope, Password, scope.CompanyId, scope.BranchId, "device-stamp");
        Assert.NotNull(stampLogin.Session);
        await db.Entry(scope.User).ReloadAsync();
        scope.User.SecurityStamp = Guid.NewGuid().ToString("N");
        scope.User.AuthVersion++;
        scope.User.UpdatedAt = DateTimeOffset.UtcNow;
        scope.User.RowVersion = Guid.NewGuid().ToByteArray();
        await db.SaveChangesAsync();
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.PostAsJsonAsync("/api/v1/auth/sessions:refresh",
            new RefreshIdentitySessionRequest(stampLogin.Session!.RefreshToken, "device-stamp"))).StatusCode);

        var revokeLogin = await LoginAsync(client, scope, Password, scope.CompanyId, scope.BranchId, "device-revoke");
        Assert.NotNull(revokeLogin.Session);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", revokeLogin.Session!.AccessToken);
        var revoke = await client.PostAsJsonAsync($"/api/v1/auth/sessions/{revokeLogin.Session.SessionId}:revoke",
            new RevokeIdentitySessionRequest("test self revoke"));
        Assert.Equal(HttpStatusCode.NoContent, revoke.StatusCode);
        var afterRevoke = await client.GetAsync("/api/v1/audit/events?take=10");
        Assert.Contains(afterRevoke.StatusCode, new[] { HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden });
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    [Trait("Category", "HTTP")]
    public async Task Registered_device_login_requires_its_credential_while_unknown_device_remains_online_only()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "DEVICE-LOGIN");
        var actor = DeviceAdministrator(scope);
        var devices = new RegisteredDeviceService(db, new AuditEventService(db));
        var credential = NewDeviceCredential();
        var registered = await devices.RegisterAsync(actor,
            new("registered-http-device", "HTTP device", "TEST", "1", null, null,
                $"request-{Guid.NewGuid():N}", credential), Guid.NewGuid(), default);
        await devices.ApproveAsync(registered.Id, actor, Guid.NewGuid(), default);
        await devices.AddAssignmentAsync(registered.Id, new(scope.User.Id, scope.BranchId),
            actor, Guid.NewGuid(), default);

        using var factory = CreateFactory(connection);
        using var client = factory.CreateClient();
        await AssertInvalidCredentialsAsync((await LoginAsync(client, scope, Password, scope.CompanyId,
            scope.BranchId, registered.DeviceId)).Response);
        await AssertInvalidCredentialsAsync((await LoginAsync(client, scope, Password, scope.CompanyId,
            scope.BranchId, registered.DeviceId, NewDeviceCredential())).Response);
        Assert.Equal(2, await db.AuditEvents.CountAsync(x => x.Action == "IdentityLogin" &&
            x.Outcome == "FAILURE" && x.DeviceId == registered.DeviceId &&
            x.Reason == "DEVICE_BINDING_DENIED"));
        Assert.False(await db.AuthSessions.AnyAsync(x =>
            x.UserId == scope.User.Id && x.DeviceId == registered.DeviceId));

        var trusted = await LoginAsync(client, scope, Password, scope.CompanyId, scope.BranchId,
            registered.DeviceId, credential);
        Assert.Equal(HttpStatusCode.OK, trusted.Response.StatusCode);
        Assert.NotNull(trusted.Session);
        var stored = await db.AuthSessions.AsNoTracking().SingleAsync(x => x.Id == trusted.Session!.SessionId);
        Assert.Equal(registered.Id, stored.RegisteredDeviceId);
        Assert.Equal(registered.CredentialVersion, stored.DeviceCredentialVersion);

        var onlineOnly = await LoginAsync(client, scope, Password, scope.CompanyId, scope.BranchId,
            $"unknown-{Guid.NewGuid():N}");
        Assert.Equal(HttpStatusCode.OK, onlineOnly.Response.StatusCode);
        var onlineOnlyStored = await db.AuthSessions.AsNoTracking()
            .SingleAsync(x => x.Id == onlineOnly.Session!.SessionId);
        Assert.Null(onlineOnlyStored.RegisteredDeviceId);
        Assert.Null(onlineOnlyStored.DeviceCredentialVersion);
    }

    [Theory]
    [InlineData("ROTATE")]
    [InlineData("RECOVER")]
    [Trait("Category", "PostgreSQL")]
    [Trait("Category", "HTTP")]
    public async Task Duplicate_live_proof_key_challenge_returns_governed_error_not_http_500(string changeType)
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, $"PENDING-{changeType}");
        var actor = DeviceAdministrator(scope);
        var devices = new RegisteredDeviceService(db, new AuditEventService(db));
        var credential = NewDeviceCredential();
        var registered = await devices.RegisterAsync(actor,
            new($"pending-key-{Guid.NewGuid():N}", "Pending key HTTP device", "TEST", "1", null, null,
                $"request-{Guid.NewGuid():N}", credential), Guid.NewGuid(), default);
        await devices.ApproveAsync(registered.Id, actor, Guid.NewGuid(), default);
        await devices.AddAssignmentAsync(registered.Id,
            new(scope.User.Id, scope.BranchId), actor, Guid.NewGuid(), default);

        using var currentKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        using var nextKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var validator = new ProofKeyChangeProofValidator();
        var currentJwk = PublicJwk(currentKey);
        var currentMaterial = validator.ReadPublicKey(currentJwk);
        var storedDevice = await db.RegisteredDevices.SingleAsync(item => item.Id == registered.Id);
        storedDevice.ProofPublicJwkCanonicalJson = currentMaterial.CanonicalJson;
        storedDevice.ProofKeyThumbprint = currentMaterial.Thumbprint;
        storedDevice.ProofKeyVersion = 1;
        storedDevice.ProofKeyChangedAt = DateTimeOffset.UtcNow;
        storedDevice.ProofKeyChangedByUserId = scope.User.Id;
        storedDevice.UpdatedAt = DateTimeOffset.UtcNow;
        storedDevice.RowVersion = RandomNumberGenerator.GetBytes(16);
        var managePermission = await db.Permissions.SingleAsync(permission =>
            permission.Code == RegisteredDevicePermissionCodes.Manage);
        db.UserPermissionOverrides.Add(new UserPermissionOverride
        {
            UserId = scope.User.Id,
            PermissionId = managePermission.Id,
            IsAllowed = true,
            Reason = "proof-key HTTP regression",
            CompanyId = scope.CompanyId,
            BranchId = managePermission.ScopeType == "BRANCH" ? scope.BranchId : null,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        });
        await db.SaveChangesAsync();

        using var factory = CreateFactory(connection);
        using var client = factory.CreateClient();
        var login = await LoginAsync(client, scope, Password, scope.CompanyId, scope.BranchId,
            registered.DeviceId, credential);
        Assert.Equal(HttpStatusCode.OK, login.Response.StatusCode);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", login.Session!.AccessToken);
        var nextJwk = PublicJwk(nextKey);
        var first = await client.PostAsJsonAsync(
            $"/api/v1/devices/{registered.Id:D}/proof-key-challenges",
            new CreateProofKeyChallengeRequest(Guid.NewGuid(), changeType, 1, nextJwk));
        Assert.Equal(HttpStatusCode.OK, first.StatusCode);

        var duplicate = await client.PostAsJsonAsync(
            $"/api/v1/devices/{registered.Id:D}/proof-key-challenges",
            new CreateProofKeyChallengeRequest(Guid.NewGuid(), changeType, 1, nextJwk));
        Assert.Equal(HttpStatusCode.BadRequest, duplicate.StatusCode);
        using var error = JsonDocument.Parse(await duplicate.Content.ReadAsStringAsync());
        Assert.Equal("PROOF_KEY_REUSE_NOT_ALLOWED",
            error.RootElement.GetProperty("errorCode").GetString());
        var nextThumbprint = validator.ReadPublicKey(nextJwk).Thumbprint;
        Assert.Equal(1, await db.RegisteredDeviceProofKeyChallenges.AsNoTracking().CountAsync(item =>
            item.RegisteredDeviceId == registered.Id &&
            item.NewProofKeyThumbprint == nextThumbprint && item.ConsumedAt == null));
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    [Trait("Category", "HTTP")]
    public async Task Registering_an_unknown_device_revokes_and_defensively_denies_its_old_unbound_session()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "DEVICE-DOWNGRADE");
        var deviceId = $"downgrade-{Guid.NewGuid():N}";

        using var factory = CreateFactory(connection);
        using var client = factory.CreateClient();
        var onlineOnly = await LoginAsync(
            client, scope, Password, scope.CompanyId, scope.BranchId, deviceId);
        Assert.Equal(HttpStatusCode.OK, onlineOnly.Response.StatusCode);
        Assert.NotNull(onlineOnly.Session);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", onlineOnly.Session!.AccessToken);
        Assert.Equal(HttpStatusCode.OK,
            (await client.GetAsync("/api/v1/audit/events?take=1")).StatusCode);

        var devices = new RegisteredDeviceService(db, new AuditEventService(db));
        var actor = DeviceAdministrator(scope);
        var credential = NewDeviceCredential();
        var registered = await devices.RegisterAsync(actor,
            new(deviceId, "Downgrade regression device", "TEST", "1", null, null,
                $"request-{Guid.NewGuid():N}", credential), Guid.NewGuid(), default);
        await devices.ApproveAsync(registered.Id, actor, Guid.NewGuid(), default);
        await devices.AddAssignmentAsync(registered.Id,
            new(scope.User.Id, scope.BranchId), actor, Guid.NewGuid(), default);

        var revoked = await db.AuthSessions.SingleAsync(
            session => session.Id == onlineOnly.Session.SessionId);
        Assert.NotNull(revoked.RevokedAt);
        Assert.Equal("DEVICE_REGISTERED_REAUTHENTICATION_REQUIRED", revoked.RevokeReason);
        var revocationAudit = Assert.Single(await db.AuditEvents.AsNoTracking().Where(audit =>
            audit.Action == "RegisteredDeviceUnboundSessionsRevoked" &&
            audit.EntityId == registered.Id).ToListAsync());
        Assert.Equal(
            "Count=1;Reason=DEVICE_REGISTERED_REAUTHENTICATION_REQUIRED",
            revocationAudit.Reason);
        Assert.Null(revocationAudit.BeforeJson);
        Assert.Null(revocationAudit.AfterJson);
        Assert.DoesNotContain(onlineOnly.Session.RefreshToken,
            System.Text.Json.JsonSerializer.Serialize(revocationAudit), StringComparison.Ordinal);

        // Re-open the legacy row deliberately to prove both request-time defenses rather than
        // relying only on the registration mutation. Neither access nor refresh may upgrade it.
        revoked.RevokedAt = null;
        revoked.RevokeReason = null;
        revoked.UpdatedAt = DateTimeOffset.UtcNow;
        revoked.RowVersion = Guid.NewGuid().ToByteArray();
        await db.SaveChangesAsync();

        Assert.Equal(HttpStatusCode.Unauthorized,
            (await client.GetAsync("/api/v1/audit/events?take=1")).StatusCode);
        client.DefaultRequestHeaders.Authorization = null;
        var refresh = await client.PostAsJsonAsync("/api/v1/auth/sessions:refresh",
            new RefreshIdentitySessionRequest(onlineOnly.Session.RefreshToken, deviceId));
        Assert.Equal(HttpStatusCode.Unauthorized, refresh.StatusCode);

        await using var verify = PostgreSqlTestEnvironment.CreateDbContext(connection);
        var terminal = await verify.AuthSessions.AsNoTracking().SingleAsync(
            session => session.Id == onlineOnly.Session.SessionId);
        Assert.NotNull(terminal.RevokedAt);
        Assert.Equal("DEVICE_REGISTERED_REAUTHENTICATION_REQUIRED", terminal.RevokeReason);
        Assert.False(await verify.AuthSessions.AnyAsync(session =>
            session.CompanyId == scope.CompanyId && session.DeviceId == deviceId &&
            session.RegisteredDeviceId == null && session.RevokedAt == null));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    [Trait("Category", "PostgreSQL")]
    [Trait("Category", "HTTP")]
    public async Task Login_and_registration_follow_both_deterministic_device_lock_orders(bool loginFirst)
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var seedDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await seedDb.Database.MigrateAsync();
        var scope = await SeedAsync(seedDb, loginFirst ? "LOGIN-FIRST" : "REGISTER-FIRST");
        var deviceId = $"ordered-login-register-{Guid.NewGuid():N}";
        using var factory = CreateFactory(connection);
        using var loginClient = factory.CreateClient();
        await using var registrationDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
        var devices = new RegisteredDeviceService(registrationDb, new AuditEventService(registrationDb));
        var credential = NewDeviceCredential();

        (HttpResponseMessage Response, IdentitySessionResponse? Session) login;
        if (loginFirst)
        {
            login = await LoginAsync(loginClient, scope, Password, scope.CompanyId, scope.BranchId, deviceId);
            _ = await devices.RegisterAsync(DeviceAdministrator(scope),
                new(deviceId, "Ordered registration device", "TEST", "1", null, null,
                    $"request-{Guid.NewGuid():N}", credential), Guid.NewGuid(), default);
        }
        else
        {
            _ = await devices.RegisterAsync(DeviceAdministrator(scope),
                new(deviceId, "Ordered registration device", "TEST", "1", null, null,
                    $"request-{Guid.NewGuid():N}", credential), Guid.NewGuid(), default);
            login = await LoginAsync(loginClient, scope, Password, scope.CompanyId, scope.BranchId, deviceId);
        }

        Assert.Equal(loginFirst ? HttpStatusCode.OK : HttpStatusCode.Unauthorized,
            login.Response.StatusCode);
        if (login.Session is { } session)
        {
            loginClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", session.AccessToken);
            Assert.Equal(HttpStatusCode.Unauthorized,
                (await loginClient.GetAsync("/api/v1/audit/events?take=1")).StatusCode);
        }

        await using var verify = PostgreSqlTestEnvironment.CreateDbContext(connection);
        Assert.False(await verify.AuthSessions.AsNoTracking().AnyAsync(session =>
            session.CompanyId == scope.CompanyId && session.DeviceId == deviceId &&
            session.RegisteredDeviceId == null && session.RevokedAt == null));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    [Trait("Category", "PostgreSQL")]
    [Trait("Category", "HTTP")]
    public async Task Refresh_and_registration_follow_both_deterministic_device_lock_orders(bool refreshFirst)
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var seedDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await seedDb.Database.MigrateAsync();
        var scope = await SeedAsync(seedDb, refreshFirst ? "REFRESH-FIRST" : "REGISTER-REFRESH-FIRST");
        var deviceId = $"ordered-refresh-register-{Guid.NewGuid():N}";
        using var setupFactory = CreateFactory(connection);
        using var setupClient = setupFactory.CreateClient();
        var original = await LoginAsync(
            setupClient, scope, Password, scope.CompanyId, scope.BranchId, deviceId);
        Assert.NotNull(original.Session);

        using var refreshFactory = CreateFactory(connection);
        using var refreshClient = refreshFactory.CreateClient();
        await using var registrationDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
        var devices = new RegisteredDeviceService(registrationDb, new AuditEventService(registrationDb));
        var credential = NewDeviceCredential();

        HttpResponseMessage refresh;
        if (refreshFirst)
        {
            refresh = await refreshClient.PostAsJsonAsync("/api/v1/auth/sessions:refresh",
                new RefreshIdentitySessionRequest(original.Session!.RefreshToken, deviceId));
            _ = await devices.RegisterAsync(DeviceAdministrator(scope),
                new(deviceId, "Ordered refresh device", "TEST", "1", null, null,
                    $"request-{Guid.NewGuid():N}", credential), Guid.NewGuid(), default);
        }
        else
        {
            _ = await devices.RegisterAsync(DeviceAdministrator(scope),
                new(deviceId, "Ordered refresh device", "TEST", "1", null, null,
                    $"request-{Guid.NewGuid():N}", credential), Guid.NewGuid(), default);
            refresh = await refreshClient.PostAsJsonAsync("/api/v1/auth/sessions:refresh",
                new RefreshIdentitySessionRequest(original.Session!.RefreshToken, deviceId));
        }

        Assert.Equal(refreshFirst ? HttpStatusCode.OK : HttpStatusCode.Unauthorized,
            refresh.StatusCode);
        if (refresh.StatusCode == HttpStatusCode.OK)
        {
            var rotated = await refresh.Content.ReadFromJsonAsync<IdentitySessionResponse>();
            Assert.NotNull(rotated);
            refreshClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", rotated!.AccessToken);
            Assert.Equal(HttpStatusCode.Unauthorized,
                (await refreshClient.GetAsync("/api/v1/audit/events?take=1")).StatusCode);
        }

        await using var verify = PostgreSqlTestEnvironment.CreateDbContext(connection);
        Assert.False(await verify.AuthSessions.AsNoTracking().AnyAsync(session =>
            session.CompanyId == scope.CompanyId && session.DeviceId == deviceId &&
            session.RegisteredDeviceId == null && session.RevokedAt == null));
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    [Trait("Category", "HTTP")]
    public async Task Concurrent_unknown_device_refresh_and_registration_leave_no_active_unbound_family()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var registrationDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await registrationDb.Database.MigrateAsync();
        var scope = await SeedAsync(registrationDb, "DEVICE-DOWNGRADE-RACE");
        var deviceId = $"downgrade-race-{Guid.NewGuid():N}";
        using var factory = CreateFactory(connection);
        using var loginClient = factory.CreateClient();
        using var refreshClient = factory.CreateClient();
        var onlineOnly = await LoginAsync(
            loginClient, scope, Password, scope.CompanyId, scope.BranchId, deviceId);
        Assert.NotNull(onlineOnly.Session);

        var devices = new RegisteredDeviceService(
            registrationDb, new AuditEventService(registrationDb));
        var credential = NewDeviceCredential();
        var registerTask = devices.RegisterAsync(DeviceAdministrator(scope),
            new(deviceId, "Downgrade race device", "TEST", "1", null, null,
                $"request-{Guid.NewGuid():N}", credential), Guid.NewGuid(), default);
        var refreshTask = refreshClient.PostAsJsonAsync("/api/v1/auth/sessions:refresh",
            new RefreshIdentitySessionRequest(onlineOnly.Session!.RefreshToken, deviceId));
        await Task.WhenAll(registerTask, refreshTask);

        Assert.Contains(refreshTask.Result.StatusCode,
            new[] { HttpStatusCode.OK, HttpStatusCode.Unauthorized });
        if (refreshTask.Result.StatusCode == HttpStatusCode.OK)
        {
            var raced = await refreshTask.Result.Content.ReadFromJsonAsync<IdentitySessionResponse>();
            Assert.NotNull(raced);
            refreshClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", raced!.AccessToken);
            Assert.Equal(HttpStatusCode.Unauthorized,
                (await refreshClient.GetAsync("/api/v1/audit/events?take=1")).StatusCode);
        }

        await using var verify = PostgreSqlTestEnvironment.CreateDbContext(connection);
        Assert.False(await verify.AuthSessions.AsNoTracking().AnyAsync(session =>
            session.CompanyId == scope.CompanyId && session.DeviceId == deviceId &&
            session.RegisteredDeviceId == null && session.RevokedAt == null));
        var familyId = await verify.AuthSessions.AsNoTracking()
            .Where(session => session.Id == onlineOnly.Session.SessionId)
            .Select(session => session.RefreshTokenFamilyId)
            .SingleAsync();
        Assert.All(await verify.AuthSessions.AsNoTracking()
            .Where(session => session.RefreshTokenFamilyId == familyId).ToListAsync(),
            session =>
            {
                Assert.NotNull(session.RevokedAt);
                Assert.Contains(session.RevokeReason,
                    new[] { "ROTATED", "DEVICE_REGISTERED_REAUTHENTICATION_REQUIRED" });
            });
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    [Trait("Category", "HTTP")]
    public async Task Concurrent_unknown_device_login_and_registration_are_linearized_by_the_device_lock()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var registrationDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await registrationDb.Database.MigrateAsync();
        var scope = await SeedAsync(registrationDb, "DEVICE-LOGIN-REGISTER-RACE");
        var deviceId = $"login-register-race-{Guid.NewGuid():N}";
        using var factory = CreateFactory(connection);
        using var loginClient = factory.CreateClient();
        var devices = new RegisteredDeviceService(
            registrationDb, new AuditEventService(registrationDb));
        var credential = NewDeviceCredential();

        var loginTask = LoginAsync(
            loginClient, scope, Password, scope.CompanyId, scope.BranchId, deviceId);
        var registerTask = devices.RegisterAsync(DeviceAdministrator(scope),
            new(deviceId, "Login registration race device", "TEST", "1", null, null,
                $"request-{Guid.NewGuid():N}", credential), Guid.NewGuid(), default);
        await Task.WhenAll(loginTask, registerTask);

        Assert.Contains(loginTask.Result.Response.StatusCode,
            new[] { HttpStatusCode.OK, HttpStatusCode.Unauthorized });
        if (loginTask.Result.Session is not null)
        {
            loginClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", loginTask.Result.Session.AccessToken);
            Assert.Equal(HttpStatusCode.Unauthorized,
                (await loginClient.GetAsync("/api/v1/audit/events?take=1")).StatusCode);
        }

        await using var verify = PostgreSqlTestEnvironment.CreateDbContext(connection);
        Assert.True(await verify.RegisteredDevices.AsNoTracking().AnyAsync(device =>
            device.CompanyId == scope.CompanyId && device.DeviceId == deviceId));
        Assert.False(await verify.AuthSessions.AsNoTracking().AnyAsync(session =>
            session.CompanyId == scope.CompanyId && session.DeviceId == deviceId &&
            session.RegisteredDeviceId == null && session.RevokedAt == null));
    }

    [Theory]
    [InlineData("suspend")]
    [InlineData("revoke")]
    [InlineData("rotate")]
    [InlineData("assignment-remove")]
    [Trait("Category", "PostgreSQL")]
    [Trait("Category", "HTTP")]
    public async Task Bound_session_is_denied_after_device_trust_is_withdrawn(string mutation)
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, $"BOUND-{mutation.ToUpperInvariant()}");
        var actor = DeviceAdministrator(scope);
        var devices = new RegisteredDeviceService(db, new AuditEventService(db));
        var credential = NewDeviceCredential();
        var registered = await devices.RegisterAsync(actor,
            new($"bound-{mutation}-{Guid.NewGuid():N}", "Bound device", "TEST", "1", null, null,
                $"request-{Guid.NewGuid():N}", credential), Guid.NewGuid(), default);
        await devices.ApproveAsync(registered.Id, actor, Guid.NewGuid(), default);
        var assignment = await devices.AddAssignmentAsync(registered.Id,
            new(scope.User.Id, scope.BranchId), actor, Guid.NewGuid(), default);

        using var factory = CreateFactory(connection);
        using var client = factory.CreateClient();
        var login = await LoginAsync(client, scope, Password, scope.CompanyId, scope.BranchId,
            registered.DeviceId, credential);
        Assert.Equal(HttpStatusCode.OK, login.Response.StatusCode);
        Assert.NotNull(login.Session);

        switch (mutation)
        {
            case "suspend":
                await devices.SuspendAsync(registered.Id, actor, Guid.NewGuid(), default);
                break;
            case "revoke":
                await devices.RevokeAsync(registered.Id, actor, Guid.NewGuid(), default);
                break;
            case "rotate":
                await devices.RotateCredentialAsync(registered.Id,
                    new(NewDeviceCredential(), registered.CredentialVersion), actor, Guid.NewGuid(), default);
                break;
            case "assignment-remove":
                await devices.RemoveAssignmentAsync(registered.Id, assignment.Id, actor, Guid.NewGuid(), default);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(mutation), mutation, null);
        }

        var refresh = await client.PostAsJsonAsync("/api/v1/auth/sessions:refresh",
            new RefreshIdentitySessionRequest(login.Session!.RefreshToken, registered.DeviceId, credential));
        Assert.Equal(HttpStatusCode.Unauthorized, refresh.StatusCode);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", login.Session.AccessToken);
        Assert.Equal(HttpStatusCode.Unauthorized,
            (await client.GetAsync("/api/v1/devices/current")).StatusCode);
        await using var verify = PostgreSqlTestEnvironment.CreateDbContext(connection);
        Assert.False(await verify.AuthSessions.AnyAsync(x =>
            x.RegisteredDeviceId == registered.Id && x.RevokedAt == null));
    }

    [Theory]
    [InlineData("expires-at")]
    [InlineData("inactive-90-days")]
    [Trait("Category", "PostgreSQL")]
    [Trait("Category", "HTTP")]
    public async Task Bound_access_and_refresh_are_denied_after_device_expiry_or_inactivity(string expiryMode)
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, $"BOUND-EXPIRY-{expiryMode.ToUpperInvariant()}");
        var actor = DeviceAdministrator(scope);
        var devices = new RegisteredDeviceService(db, new AuditEventService(db));
        var credential = NewDeviceCredential();
        var registered = await devices.RegisterAsync(actor,
            new($"bound-expiry-{Guid.NewGuid():N}", "Bound expiring device", "TEST", "1", null, null,
                $"request-{Guid.NewGuid():N}", credential), Guid.NewGuid(), default);
        await devices.ApproveAsync(registered.Id, actor, Guid.NewGuid(), default);
        await devices.AddAssignmentAsync(registered.Id,
            new(scope.User.Id, scope.BranchId), actor, Guid.NewGuid(), default);

        using var factory = CreateFactory(connection);
        using var client = factory.CreateClient();
        var login = await LoginAsync(client, scope, Password, scope.CompanyId, scope.BranchId,
            registered.DeviceId, credential);
        Assert.Equal(HttpStatusCode.OK, login.Response.StatusCode);
        Assert.NotNull(login.Session);

        if (expiryMode == "expires-at")
        {
            await db.Database.ExecuteSqlInterpolatedAsync($$"""
                UPDATE transport_erp.registered_devices
                SET "ExpiresAt"={{DateTimeOffset.UtcNow.AddMinutes(-1)}}
                WHERE "Id"={{registered.Id}}
                """);
        }
        else
        {
            await db.Database.ExecuteSqlInterpolatedAsync($$"""
                UPDATE transport_erp.registered_devices
                SET "LastSeenAt"={{DateTimeOffset.UtcNow.Subtract(RegisteredDeviceService.InactivityLimit).AddMinutes(-1)}}
                WHERE "Id"={{registered.Id}}
                """);
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", login.Session!.AccessToken);
        Assert.Equal(HttpStatusCode.Unauthorized,
            (await client.GetAsync("/api/v1/devices/current")).StatusCode);
        client.DefaultRequestHeaders.Authorization = null;
        Assert.Equal(HttpStatusCode.Unauthorized,
            (await client.PostAsJsonAsync("/api/v1/auth/sessions:refresh",
                new RefreshIdentitySessionRequest(
                    login.Session.RefreshToken, registered.DeviceId, credential))).StatusCode);

        await using var verify = PostgreSqlTestEnvironment.CreateDbContext(connection);
        Assert.False(await verify.AuthSessions.AsNoTracking().AnyAsync(session =>
            session.RegisteredDeviceId == registered.Id && session.RevokedAt == null));
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    [Trait("Category", "HTTP")]
    public async Task Oversized_credentials_and_cross_field_login_ambiguity_fail_closed_with_generic_responses()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "BOUNDS");
        var now = DateTimeOffset.UtcNow;
        var ambiguous = new User
        {
            Id = Guid.NewGuid(), UserName = $"ambiguous-{Guid.NewGuid():N}",
            NormalizedUserName = $"AMBIGUOUS-{Guid.NewGuid():N}",
            Email = scope.User.UserName, NormalizedEmail = scope.User.NormalizedUserName,
            DisplayName = "مستخدم التباس مقصود", CompanyId = scope.CompanyId, BranchId = scope.BranchId,
            Status = "ACTIVE", SecurityStamp = Guid.NewGuid().ToString("N"), AuthVersion = 1,
            CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
        };
        ambiguous.PasswordHash = new PasswordHasher<User>().HashPassword(ambiguous, Password);
        db.Users.Add(ambiguous);
        await db.SaveChangesAsync();

        using var factory = CreateFactory(connection);
        using var client = factory.CreateClient();
        await AssertInvalidCredentialsAsync((await LoginAsync(client, scope, Password, scope.CompanyId,
            scope.BranchId, "ambiguous-device")).Response);
        await AssertInvalidCredentialsAsync((await LoginAsync(client, scope,
            new string('p', IdentitySessionService.MaxPasswordLength + 1), scope.CompanyId,
            scope.BranchId, "oversized-password-device")).Response);

        var invalidRefresh = await client.PostAsJsonAsync("/api/v1/auth/sessions:refresh",
            new RefreshIdentitySessionRequest(
                new string('a', IdentitySessionService.MaxRefreshTokenLength + 1), "oversized-refresh-device"));
        Assert.Equal(HttpStatusCode.Unauthorized, invalidRefresh.StatusCode);
        using var body = JsonDocument.Parse(await invalidRefresh.Content.ReadAsStringAsync());
        Assert.Equal("REFRESH_TOKEN_INVALID", body.RootElement.GetProperty("errorCode").GetString());
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    [Trait("Category", "HTTP")]
    public async Task Concurrent_refresh_has_one_winner_and_reuse_revokes_the_family()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "RACE");
        using var factory = CreateFactory(connection);
        using var clientA = factory.CreateClient();
        using var clientB = factory.CreateClient();
        var login = await LoginAsync(clientA, scope, Password, scope.CompanyId, scope.BranchId, "device-race");
        Assert.NotNull(login.Session);

        var request = new RefreshIdentitySessionRequest(login.Session!.RefreshToken, "device-race");
        var responses = await Task.WhenAll(
            clientA.PostAsJsonAsync("/api/v1/auth/sessions:refresh", request),
            clientB.PostAsJsonAsync("/api/v1/auth/sessions:refresh", request));
        Assert.Single(responses.Where(x => x.StatusCode == HttpStatusCode.OK));
        Assert.Single(responses.Where(x => x.StatusCode == HttpStatusCode.Unauthorized));
        var winner = await responses.Single(x => x.StatusCode == HttpStatusCode.OK)
            .Content.ReadFromJsonAsync<IdentitySessionResponse>();
        Assert.NotNull(winner);
        Assert.Equal(HttpStatusCode.Unauthorized, (await clientA.PostAsJsonAsync("/api/v1/auth/sessions:refresh",
            new RefreshIdentitySessionRequest(winner!.RefreshToken, "device-race"))).StatusCode);
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    [Trait("Category", "HTTP")]
    public async Task Database_resolved_deny_and_tenant_query_mismatch_are_forbidden()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "RBAC");
        using var factory = CreateFactory(connection);
        using var client = factory.CreateClient();
        var login = await LoginAsync(client, scope, Password, scope.CompanyId, scope.BranchId, "device-rbac");
        Assert.NotNull(login.Session);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login.Session!.AccessToken);

        db.UserPermissionOverrides.Add(new UserPermissionOverride
        {
            UserId = scope.User.Id, PermissionId = scope.AuditPermissionId, IsAllowed = false,
            CompanyId = scope.CompanyId, BranchId = scope.BranchId, Reason = "explicit deny test",
            CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow,
            RowVersion = Guid.NewGuid().ToByteArray()
        });
        await db.SaveChangesAsync();
        Assert.Equal(HttpStatusCode.Forbidden, (await client.GetAsync("/api/v1/audit/events?take=10")).StatusCode);

        db.UserPermissionOverrides.Remove(await db.UserPermissionOverrides.SingleAsync(x =>
            x.UserId == scope.User.Id && x.PermissionId == scope.AuditPermissionId));
        await db.SaveChangesAsync();
        Assert.Equal(HttpStatusCode.Forbidden,
            (await client.GetAsync($"/api/v1/audit/events?companyId={Guid.NewGuid()}&take=10")).StatusCode);
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Concurrent_identity_stream_audits_preserve_the_hash_chain()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var seedDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await seedDb.Database.MigrateAsync();
        var scope = await SeedAsync(seedDb, "AUDIT");
        const string device = "p1-concurrent-audit";

        await Task.WhenAll(Enumerable.Range(0, 6).Select(async index =>
        {
            await using var eventDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
            await new AuditEventService(eventDb).AppendAuditEventAsync(new AuditEventDraft(
                "IdentityConcurrencyTest", "SUCCESS", nameof(AuthSession), ActorUserId: scope.User.Id,
                CompanyId: scope.CompanyId, BranchId: scope.BranchId, DeviceId: device,
                CorrelationId: Guid.NewGuid(), Reason: index.ToString()));
        }));

        await using var verifyDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
        var result = await new AuditEventService(verifyDb).VerifyHashChainAsync(scope.CompanyId, scope.BranchId, device);
        Assert.True(result.IsValid, result.FailureReason);
        Assert.Equal(6, result.EventCount);
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Session_creation_rolls_back_when_audit_append_fails()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "ROLLBACK");
        var permissions = new EffectivePermissionResolver(db);
        var hasher = new PasswordHasher<User>();
        var service = new IdentitySessionService(db, hasher, new IdentityPasswordSentinel(hasher),
            new TenantScopeResolver(db, permissions), new AuditEventService(db),
            Options.Create(new TransportSecurityOptions
            {
                Mode = TransportAuthMode.LocalSessions, Issuer = "rollback-test", Audience = "rollback-test",
                SigningKeyId = "rollback-current", SigningKey = "transport-erp-rollback-test-key-minimum-32-characters"
            }));

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(
            new CreateIdentitySessionRequest(scope.User.UserName, Password, scope.CompanyId, scope.BranchId, "rollback-device"),
            Guid.NewGuid(), new string('x', 65), default));

        await using var verifyDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
        Assert.False(await verifyDb.AuthSessions.AnyAsync(x => x.UserId == scope.User.Id && x.DeviceId == "rollback-device"));
        Assert.False(await verifyDb.AuditEvents.AnyAsync(x => x.ActorUserId == scope.User.Id && x.DeviceId == "rollback-device"));
    }

    private static async Task AssertInvalidCredentialsAsync(HttpResponseMessage response)
    {
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal("INVALID_CREDENTIALS", body.RootElement.GetProperty("errorCode").GetString());
        Assert.False(body.RootElement.TryGetProperty("reason", out _));
    }

    private static async Task<(HttpResponseMessage Response, IdentitySessionResponse? Session)> LoginAsync(
        HttpClient client, SecurityScope scope, string password, Guid? companyId, Guid? branchId, string deviceId,
        string? deviceCredential = null)
    {
        var response = await client.PostAsJsonAsync("/api/v1/auth/sessions",
            new CreateIdentitySessionRequest(scope.User.UserName, password, companyId, branchId, deviceId,
                deviceCredential));
        return (response, response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<IdentitySessionResponse>()
            : null);
    }

    private static CurrentSecurityContext DeviceAdministrator(SecurityScope scope) => new(
        scope.User.Id, scope.CompanyId, scope.BranchId, Guid.NewGuid(), "device-administrator", true);

    private static string NewDeviceCredential()
        => Convert.ToBase64String(System.Security.Cryptography.RandomNumberGenerator.GetBytes(32));

    private static JsonElement PublicJwk(ECDsa key)
    {
        var parameters = key.ExportParameters(false);
        return JsonSerializer.SerializeToElement(new Dictionary<string, object?>
        {
            ["kty"] = "EC",
            ["crv"] = "P-256",
            ["x"] = Base64Url(parameters.Q.X!),
            ["y"] = Base64Url(parameters.Q.Y!)
        });
    }

    private static string Base64Url(byte[] value)
        => Convert.ToBase64String(value).TrimEnd('=').Replace('+', '-').Replace('/', '_');

    private static WebApplicationFactory<Program> CreateFactory(string connection)
        => new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseSetting("ConnectionStrings:TransportErp", connection);
            builder.UseSetting("Auth:Mode", "LocalSessions");
            builder.UseSetting("Auth:Issuer", "TransportERP.P1.Security.Tests");
            builder.UseSetting("Auth:Audience", "TransportERP.P1.Security.Tests.Api");
            builder.UseSetting("Auth:SigningKeyId", "p1-current");
            builder.UseSetting("Auth:SigningKey", "transport-erp-p1-security-test-key-32-chars-minimum");
        });

    private static async Task<SecurityScope> SeedAsync(TransportErpDbContext db, string suffix)
    {
        var now = DateTimeOffset.UtcNow;
        var currency = new Currency
        {
            Id = Guid.NewGuid(), Code = await PostgreSqlTestCurrencyCodeAllocator.NextAsync(db), NameAr = "عملة أمن",
            MinorUnit = 2, IsBase = true, Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        // Company.Code is capped at 18 characters. Putting the scenario suffix first used to
        // truncate the random tail for long names, so parallel cases could insert the same code.
        var companyCode = $"SEC-{Guid.NewGuid():N}"[..18];
        var company = new Company
        {
            Id = Guid.NewGuid(), Code = companyCode, LegalNameAr = "شركة أمن",
            BaseCurrencyId = currency.Id, DefaultCalendarId = Guid.NewGuid(), Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
        };
        var branch = new Branch
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, Code = "MAIN", NameAr = "الفرع الرئيسي",
            Timezone = "Asia/Riyadh", Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        var userName = $"p1-{suffix}-{Guid.NewGuid():N}";
        var user = new User
        {
            Id = Guid.NewGuid(), UserName = userName, NormalizedUserName = userName.ToUpperInvariant(),
            Email = $"{Guid.NewGuid():N}@example.invalid", DisplayName = "مستخدم أمن", CompanyId = company.Id,
            BranchId = branch.Id, Status = "ACTIVE", SecurityStamp = Guid.NewGuid().ToString("N"), AuthVersion = 1,
            CreatedAt = now, UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        user.NormalizedEmail = user.Email.ToUpperInvariant();
        user.PasswordHash = new PasswordHasher<User>().HashPassword(user, Password);

        var permission = await db.Permissions.SingleOrDefaultAsync(x => x.Code == "audit.events.read");
        if (permission is null)
        {
            permission = new Permission
            {
                Id = Guid.NewGuid(), Code = "audit.events.read", NameAr = "قراءة التدقيق", Resource = "audit.events",
                Action = "read", ScopeType = "BRANCH", Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
                RowVersion = Guid.NewGuid().ToByteArray()
            };
            db.Permissions.Add(permission);
        }
        var role = new Role
        {
            Id = Guid.NewGuid(), Code = $"P1-{suffix}-{Guid.NewGuid():N}", NameAr = "دور اختبار الأمن",
            CompanyId = company.Id, Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        db.AddRange(currency, company, branch, user, role);
        db.UserRoles.Add(new UserRole
        {
            UserId = user.Id, RoleId = role.Id, CompanyId = company.Id, BranchId = branch.Id,
            CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
        });
        var (grantCompany, grantBranch) = permission.ScopeType switch
        {
            "PLATFORM" => ((Guid?)null, (Guid?)null),
            "COMPANY" => (company.Id, (Guid?)null),
            _ => (company.Id, (Guid?)branch.Id)
        };
        db.RolePermissions.Add(new RolePermission
        {
            RoleId = role.Id, PermissionId = permission.Id, ScopeType = permission.ScopeType,
            CompanyId = grantCompany, BranchId = grantBranch, CreatedAt = now, UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        });
        await db.SaveChangesAsync();
        return new SecurityScope(company.Id, branch.Id, user, permission.Id);
    }

    private sealed record SecurityScope(Guid CompanyId, Guid BranchId, User User, Guid AuditPermissionId);
}
