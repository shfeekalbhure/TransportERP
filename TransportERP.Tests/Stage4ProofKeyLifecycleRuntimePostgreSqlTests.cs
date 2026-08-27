using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TransportERP.Api.Identity;
using TransportERP.Api.Security;
using TransportERP.Contracts.Identity;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

[Collection("PostgreSql")]
public sealed class Stage4ProofKeyLifecycleRuntimePostgreSqlTests
{
    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Bind_rotate_recover_enforce_possession_idempotency_session_revoke_and_no_raw_leak()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db);
        var validator = new ProofKeyChangeProofValidator();
        var service = new ProofKeyLifecycleService(db, new AuditEventService(db), validator);
        var current = new CurrentSecurityContext(scope.UserId, scope.CompanyId, scope.BranchId,
            scope.SessionId, scope.DeviceName, true, scope.DeviceId, 1);
        const string token = "test-access-token.without-secret-material";

        using var firstKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        using var secondKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        using var recoveryKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);

        var bind = await ExecuteAsync(service, validator, scope, current, "BIND", null,
            firstKey, null, token);
        Assert.Equal(1, bind.Result.ProofKeyVersion);
        var bindChallengeReplay = await service.CreateChallengeAsync(scope.DeviceId, current,
            bind.ChallengeRequest, Guid.NewGuid(), default);
        Assert.Null(bindChallengeReplay.Challenge);
        var bindReplay = await service.ChangeAsync(scope.DeviceId, "BIND", current, bind.ChangeRequest,
            null, bind.NewProof, token, bind.RawBody, bind.Htu, Guid.NewGuid(), default);
        Assert.Equal(0, bind.Result.ChangedAt.UtcDateTime.Ticks % TimeSpan.TicksPerMicrosecond);
        Assert.Equal(bind.Result, bindReplay);

        var rotate = await ExecuteAsync(service, validator, scope, current, "ROTATE", 1,
            secondKey, firstKey, token);
        Assert.Equal(2, rotate.Result.ProofKeyVersion);

        var recovery = await ExecuteAsync(service, validator, scope, current, "RECOVER", 2,
            recoveryKey, null, token, "operator verified recovery");
        Assert.Equal(3, recovery.Result.ProofKeyVersion);
        Assert.Equal(2, await db.AuthSessions.CountAsync(x =>
            x.RegisteredDeviceId == scope.DeviceId && x.RevokedAt != null &&
            x.RevokeReason == "DEVICE_PROOF_KEY_RECOVERED"));

        db.ChangeTracker.Clear();
        var persisted = await db.RegisteredDevices.AsNoTracking().SingleAsync(x => x.Id == scope.DeviceId);
        Assert.Equal(3, persisted.ProofKeyVersion);
        Assert.Equal(validator.ReadPublicKey(PublicJwk(recoveryKey)).Thumbprint, persisted.ProofKeyThumbprint);

        var deviceAudits = await db.AuditEvents.AsNoTracking()
            .Where(x => x.EntityId == scope.DeviceId).ToListAsync();
        var auditText = string.Join('\n', deviceAudits.Select(x =>
            (x.BeforeJson ?? "") + (x.AfterJson ?? "") + (x.Reason ?? "")));
        Assert.DoesNotContain(bind.RawChallenge, auditText, StringComparison.Ordinal);
        Assert.DoesNotContain(rotate.RawChallenge, auditText, StringComparison.Ordinal);
        Assert.DoesNotContain(recovery.RawChallenge, auditText, StringComparison.Ordinal);
        Assert.DoesNotContain(token, auditText, StringComparison.Ordinal);
        Assert.DoesNotContain(bind.NewProof, auditText, StringComparison.Ordinal);
        Assert.DoesNotContain("\"crv\"", auditText, StringComparison.Ordinal);
        Assert.All(deviceAudits, item => Assert.Equal("SUCCESS", item.Outcome));
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Rotate_rejects_missing_or_wrong_old_key_possession_without_consuming_challenge()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db);
        var validator = new ProofKeyChangeProofValidator();
        var service = new ProofKeyLifecycleService(db, new AuditEventService(db), validator);
        var current = new CurrentSecurityContext(scope.UserId, scope.CompanyId, scope.BranchId,
            scope.SessionId, scope.DeviceName, true, scope.DeviceId, 1);
        const string token = "test-token";
        using var currentKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        using var nextKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        using var wrongKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        _ = await ExecuteAsync(service, validator, scope, current, "BIND", null, currentKey, null, token);

        var rotate = await PrepareAsync(service, validator, scope, current, "ROTATE", 1,
            nextKey, wrongKey, token, null);
        var failure = await Assert.ThrowsAsync<ProofKeyLifecycleException>(() => service.ChangeAsync(
            scope.DeviceId, "ROTATE", current, rotate.ChangeRequest, rotate.CurrentProof,
            rotate.NewProof, token, rotate.RawBody, rotate.Htu, Guid.NewGuid(), default));

        Assert.Equal("PROOF_KEY_PROOF_INVALID", failure.Code);
        db.ChangeTracker.Clear();
        Assert.Null(await db.RegisteredDeviceProofKeyChallenges.Where(x => x.Id == rotate.ChallengeId)
            .Select(x => x.ConsumedAt).SingleAsync());
        Assert.Equal(1, await db.RegisteredDevices.Where(x => x.Id == scope.DeviceId)
            .Select(x => x.ProofKeyVersion).SingleAsync());
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Rotate_and_recover_reject_current_and_previous_keys_while_new_keys_are_allowed()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db);
        var validator = new ProofKeyChangeProofValidator();
        var service = new ProofKeyLifecycleService(db, new AuditEventService(db), validator);
        var current = new CurrentSecurityContext(scope.UserId, scope.CompanyId, scope.BranchId,
            scope.SessionId, scope.DeviceName, true, scope.DeviceId, 1);
        const string token = "same-key-test-token";
        using var currentKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        using var nextKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        using var recoveryKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        _ = await ExecuteAsync(service, validator, scope, current, "BIND", null,
            currentKey, null, token);
        var challengeCount = await db.RegisteredDeviceProofKeyChallenges.CountAsync(x =>
            x.RegisteredDeviceId == scope.DeviceId);

        foreach (var changeType in new[] { "ROTATE", "RECOVER" })
        {
            var failure = await Assert.ThrowsAsync<ProofKeyLifecycleException>(() =>
                service.CreateChallengeAsync(scope.DeviceId, current,
                    new CreateProofKeyChallengeRequest(Guid.NewGuid(), changeType, 1, PublicJwk(currentKey)),
                    Guid.NewGuid(), default));

            Assert.Equal("PROOF_KEY_REUSE_NOT_ALLOWED", failure.Code);
        }

        Assert.Equal(challengeCount, await db.RegisteredDeviceProofKeyChallenges.CountAsync(x =>
            x.RegisteredDeviceId == scope.DeviceId));
        Assert.Equal(1, await db.RegisteredDevices.Where(x => x.Id == scope.DeviceId)
            .Select(x => x.ProofKeyVersion).SingleAsync());

        var rotate = await ExecuteAsync(service, validator, scope, current, "ROTATE", 1,
            nextKey, currentKey, token);
        Assert.Equal(2, rotate.Result.ProofKeyVersion);
        Assert.NotEqual(validator.ReadPublicKey(PublicJwk(currentKey)).Thumbprint,
            rotate.Result.ProofKeyThumbprint);

        challengeCount = await db.RegisteredDeviceProofKeyChallenges.CountAsync(x =>
            x.RegisteredDeviceId == scope.DeviceId);
        foreach (var changeType in new[] { "ROTATE", "RECOVER" })
        {
            var failure = await Assert.ThrowsAsync<ProofKeyLifecycleException>(() =>
                service.CreateChallengeAsync(scope.DeviceId, current,
                    new CreateProofKeyChallengeRequest(Guid.NewGuid(), changeType, 2, PublicJwk(currentKey)),
                    Guid.NewGuid(), default));

            Assert.Equal("PROOF_KEY_REUSE_NOT_ALLOWED", failure.Code);
        }
        Assert.Equal(challengeCount, await db.RegisteredDeviceProofKeyChallenges.CountAsync(x =>
            x.RegisteredDeviceId == scope.DeviceId));

        var recovery = await ExecuteAsync(service, validator, scope, current, "RECOVER", 2,
            recoveryKey, null, token, "verified recovery to a never-used key");
        Assert.Equal(3, recovery.Result.ProofKeyVersion);
        Assert.NotEqual(rotate.Result.ProofKeyThumbprint, recovery.Result.ProofKeyThumbprint);
    }

    [Theory]
    [InlineData("ROTATE")]
    [InlineData("RECOVER")]
    [Trait("Category", "PostgreSQL")]
    public async Task A_second_live_challenge_for_the_same_new_key_is_rejected_by_the_service(string changeType)
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db);
        var validator = new ProofKeyChangeProofValidator();
        var service = new ProofKeyLifecycleService(db, new AuditEventService(db), validator);
        var current = new CurrentSecurityContext(scope.UserId, scope.CompanyId, scope.BranchId,
            scope.SessionId, scope.DeviceName, true, scope.DeviceId, 1);
        using var currentKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        using var nextKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        _ = await ExecuteAsync(service, validator, scope, current, "BIND", null,
            currentKey, null, "pending-key-test-token");

        var first = await service.CreateChallengeAsync(scope.DeviceId, current,
            new CreateProofKeyChallengeRequest(Guid.NewGuid(), changeType, 1, PublicJwk(nextKey)),
            Guid.NewGuid(), default);
        Assert.NotNull(first.Challenge);
        var failure = await Assert.ThrowsAsync<ProofKeyLifecycleException>(() =>
            service.CreateChallengeAsync(scope.DeviceId, current,
                new CreateProofKeyChallengeRequest(Guid.NewGuid(), changeType, 1, PublicJwk(nextKey)),
                Guid.NewGuid(), default));

        Assert.Equal("PROOF_KEY_REUSE_NOT_ALLOWED", failure.Code);
        var thumbprint = validator.ReadPublicKey(PublicJwk(nextKey)).Thumbprint;
        Assert.Equal(1, await db.RegisteredDeviceProofKeyChallenges.AsNoTracking().CountAsync(x =>
            x.RegisteredDeviceId == scope.DeviceId && x.NewProofKeyThumbprint == thumbprint &&
            x.ConsumedAt == null));
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Rotation_wins_device_lock_and_old_key_claim_leaves_no_partial_replay()
    {
        await AssertRotationClaimLinearizationAsync(rotationFirst: true);
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Claim_wins_device_lock_and_commits_version_one_replay_before_rotation()
    {
        await AssertRotationClaimLinearizationAsync(rotationFirst: false);
    }

    private static async Task AssertRotationClaimLinearizationAsync(bool rotationFirst)
    {
        var baseConnection = PostgreSqlTestEnvironment.RequireConnection();
        Scope scope;
        Prepared rotate;
        SyncProofSecurityContext syncSecurity;
        VerifiedSyncProofMaterial oldKeyProof;
        CurrentSecurityContext current;
        const string token = "linearization-access-token";
        await using (var setup = PostgreSqlTestEnvironment.CreateDbContext(baseConnection))
        {
            await setup.Database.MigrateAsync();
            scope = await SeedAsync(setup);
            var validator = new ProofKeyChangeProofValidator();
            var lifecycle = new ProofKeyLifecycleService(setup, new AuditEventService(setup), validator);
            current = new CurrentSecurityContext(scope.UserId, scope.CompanyId, scope.BranchId,
                scope.SessionId, scope.DeviceName, true, scope.DeviceId, 1);
            using var oldKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            using var newKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            _ = await ExecuteAsync(lifecycle, validator, scope, current, "BIND", null,
                oldKey, null, token);
            syncSecurity = new SyncProofSecurityContext(scope.UserId, scope.CompanyId, scope.BranchId,
                scope.DeviceId, scope.DeviceName);
            var sync = new SyncProofRuntimeService(setup, new AuditEventService(setup));
            var nonce = await sync.IssueNonceAsync(syncSecurity);
            oldKeyProof = new VerifiedSyncProofMaterial(
                Guid.NewGuid().ToString("N"), nonce.Value,
                validator.ReadPublicKey(PublicJwk(oldKey)).Thumbprint,
                DateTimeOffset.UtcNow, "https://sync.example.test/api/v1/sync/operations:batch", Guid.NewGuid());
            rotate = await PrepareAsync(lifecycle, validator, scope, current, "ROTATE", 1,
                newKey, oldKey, token, null);
        }

        var suffix = Guid.NewGuid().ToString("N");
        var rotationApp = "pk-rotation-" + suffix;
        var claimApp = "pk-claim-" + suffix;
        await using var blocker = PostgreSqlTestEnvironment.CreateDbContext(baseConnection);
        await using var blockerTransaction = await blocker.Database.BeginTransactionAsync();
        var actorLockKey = "user-scope|" + scope.UserId;
        await blocker.Database.ExecuteSqlInterpolatedAsync(
            $"SELECT pg_advisory_xact_lock(hashtextextended({actorLockKey}, 0))");

        await using var rotationDb = PostgreSqlTestEnvironment.CreateDbContext(
            WithApplicationName(baseConnection, rotationApp));
        await using var claimDb = PostgreSqlTestEnvironment.CreateDbContext(
            WithApplicationName(baseConnection, claimApp));
        var rotationService = new ProofKeyLifecycleService(rotationDb, new AuditEventService(rotationDb),
            new ProofKeyChangeProofValidator());
        var claimService = new SyncProofRuntimeService(claimDb, new AuditEventService(claimDb));

        Task<ProofKeyChangeResponse> StartRotation() => rotationService.ChangeAsync(
            scope.DeviceId, "ROTATE", current, rotate.ChangeRequest, rotate.CurrentProof,
            rotate.NewProof, token, rotate.RawBody, rotate.Htu, Guid.NewGuid(), default);
        Task<AcceptedSyncProofContext> StartClaim() => claimService.ClaimAsync(syncSecurity, oldKeyProof);

        Task<ProofKeyChangeResponse> rotationTask;
        Task<AcceptedSyncProofContext> claimTask;
        if (rotationFirst)
        {
            rotationTask = StartRotation();
            await WaitForPostgreSqlLockWaitAsync(baseConnection, rotationApp);
            claimTask = StartClaim();
            await WaitForPostgreSqlLockWaitAsync(baseConnection, claimApp);
        }
        else
        {
            claimTask = StartClaim();
            await WaitForPostgreSqlLockWaitAsync(baseConnection, claimApp);
            rotationTask = StartRotation();
            await WaitForPostgreSqlLockWaitAsync(baseConnection, rotationApp);
        }

        await blockerTransaction.CommitAsync();
        var rotationErrorTask = Record.ExceptionAsync(async () => await rotationTask);
        var claimErrorTask = Record.ExceptionAsync(async () => await claimTask);
        await Task.WhenAll(rotationErrorTask, claimErrorTask);
        var rotationError = rotationErrorTask.Result;
        var claimError = claimErrorTask.Result;

        Assert.Null(rotationError);
        if (rotationFirst)
            Assert.Equal("invalid_dpop_proof", Assert.IsType<SyncProofRuntimeException>(claimError).Code);
        else
            Assert.Null(claimError);

        await using var verify = PostgreSqlTestEnvironment.CreateDbContext(baseConnection);
        Assert.Equal(2, await verify.RegisteredDevices.Where(x => x.Id == scope.DeviceId)
            .Select(x => x.ProofKeyVersion).SingleAsync());
        Assert.Single(await verify.RegisteredDeviceProofKeyChanges.Where(x =>
            x.RegisteredDeviceId == scope.DeviceId && x.ChangeRequestId == rotate.ChangeRequest.ChangeRequestId)
            .ToListAsync());
        Assert.NotNull(await verify.RegisteredDeviceProofKeyChallenges.Where(x => x.Id == rotate.ChallengeId)
            .Select(x => x.ConsumedAt).SingleAsync());
        var replayCount = await verify.SyncProofReplays.CountAsync(x =>
            x.RegisteredDeviceId == scope.DeviceId && x.AttemptCorrelationId == oldKeyProof.AttemptCorrelationId);
        var replayAuditCount = await verify.AuditEvents.CountAsync(x => x.Action == "SyncProofAccepted" &&
            x.CorrelationId == oldKeyProof.AttemptCorrelationId);
        Assert.Equal(rotationFirst ? 0 : 1, replayCount);
        Assert.Equal(replayCount, replayAuditCount);
        if (!rotationFirst)
            Assert.Equal(1, await verify.SyncProofReplays.Where(x =>
                x.RegisteredDeviceId == scope.DeviceId && x.AttemptCorrelationId == oldKeyProof.AttemptCorrelationId)
                .Select(x => x.ProofKeyVersion).SingleAsync());
    }

    private static string WithApplicationName(string connectionString, string applicationName)
    {
        var builder = new Npgsql.NpgsqlConnectionStringBuilder(connectionString)
        {
            ApplicationName = applicationName,
            Pooling = false
        };
        return builder.ConnectionString;
    }

    private static async Task WaitForPostgreSqlLockWaitAsync(string connectionString, string applicationName)
    {
        await using var observer = new Npgsql.NpgsqlConnection(connectionString);
        await observer.OpenAsync();
        for (var attempt = 0; attempt < 200; attempt++)
        {
            await using var command = new Npgsql.NpgsqlCommand("""
                SELECT EXISTS (
                  SELECT 1 FROM pg_stat_activity
                   WHERE application_name=@application_name AND wait_event_type='Lock')
                """, observer);
            command.Parameters.AddWithValue("application_name", applicationName);
            if (Assert.IsType<bool>(await command.ExecuteScalarAsync())) return;
            await Task.Delay(25);
        }
        throw new TimeoutException($"PostgreSQL session {applicationName} did not reach the device lock.");
    }

    private static async Task<Execution> ExecuteAsync(
        ProofKeyLifecycleService service,
        ProofKeyChangeProofValidator validator,
        Scope scope,
        CurrentSecurityContext current,
        string changeType,
        int? expectedVersion,
        ECDsa newKey,
        ECDsa? currentKey,
        string token,
        string? reason = null)
    {
        var prepared = await PrepareAsync(service, validator, scope, current, changeType, expectedVersion,
            newKey, currentKey, token, reason);
        var result = await service.ChangeAsync(scope.DeviceId, changeType, current, prepared.ChangeRequest,
            prepared.CurrentProof, prepared.NewProof, token, prepared.RawBody, prepared.Htu,
            Guid.NewGuid(), default);
        return new(prepared.ChallengeRequest, prepared.ChangeRequest, result, prepared.RawBody,
            prepared.Htu, prepared.NewProof, prepared.RawChallenge);
    }

    private static async Task<Prepared> PrepareAsync(
        ProofKeyLifecycleService service,
        ProofKeyChangeProofValidator validator,
        Scope scope,
        CurrentSecurityContext current,
        string changeType,
        int? expectedVersion,
        ECDsa newKey,
        ECDsa? currentKey,
        string token,
        string? reason)
    {
        var newJwk = PublicJwk(newKey);
        var newMaterial = validator.ReadPublicKey(newJwk);
        var requestId = Guid.NewGuid();
        var challengeRequest = new CreateProofKeyChallengeRequest(requestId, changeType, expectedVersion, newJwk);
        var challenge = await service.CreateChallengeAsync(scope.DeviceId, current, challengeRequest,
            Guid.NewGuid(), default);
        Assert.NotNull(challenge.Challenge);
        var changeRequest = new ChangeProofKeyRequest(challenge.ChallengeId, requestId, changeType,
            expectedVersion, newJwk, reason);
        var body = JsonSerializer.SerializeToUtf8Bytes(changeRequest, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        var htu = $"https://erp.example/api/v1/devices/{scope.DeviceId:D}:{changeType.ToLowerInvariant()}-proof-key";
        var baseClaims = Claims(scope.DeviceId, challenge.ChallengeId, requestId, changeType,
            challenge.Challenge!, newMaterial.Thumbprint, token, body, htu);
        var newClaims = new Dictionary<string, object?>(baseClaims) { ["jti"] = Guid.NewGuid().ToString("D") };
        var newProof = Sign(newKey, newJwk, newClaims);
        string? oldProof = null;
        if (currentKey is not null)
        {
            var currentClaims = new Dictionary<string, object?>(baseClaims) { ["jti"] = Guid.NewGuid().ToString("D") };
            oldProof = Sign(currentKey, PublicJwk(currentKey), currentClaims);
        }
        return new(challengeRequest, changeRequest, challenge.ChallengeId, challenge.Challenge!, body,
            htu, newProof, oldProof);
    }

    private static Dictionary<string, object?> Claims(
        Guid deviceId, Guid challengeId, Guid requestId, string changeType, string challenge,
        string newThumbprint, string token, byte[] body, string htu) => new()
    {
        ["cid"] = challengeId.ToString("D"), ["rid"] = requestId.ToString("D"),
        ["did"] = deviceId.ToString("D"), ["ct"] = changeType,
        ["iat"] = DateTimeOffset.UtcNow.ToUnixTimeSeconds(), ["jti"] = Guid.NewGuid().ToString("D"),
        ["chl"] = challenge, ["nkt"] = newThumbprint, ["htm"] = "POST", ["htu"] = htu,
        ["ath"] = Base64Url(SHA256.HashData(Encoding.ASCII.GetBytes(token))),
        ["tbh"] = Base64Url(SHA256.HashData(body))
    };

    private static JsonElement PublicJwk(ECDsa key)
    {
        var parameters = key.ExportParameters(false);
        return JsonSerializer.SerializeToElement(new Dictionary<string, object?>
        {
            ["kty"] = "EC", ["crv"] = "P-256",
            ["x"] = Base64Url(parameters.Q.X!), ["y"] = Base64Url(parameters.Q.Y!)
        });
    }

    private static string Sign(ECDsa key, JsonElement jwk, IReadOnlyDictionary<string, object?> claims)
    {
        var header = Base64Url(JsonSerializer.SerializeToUtf8Bytes(new Dictionary<string, object?>
        {
            ["typ"] = "transporterp-key-change+jwt", ["alg"] = "ES256", ["jwk"] = jwk
        }));
        var payload = Base64Url(JsonSerializer.SerializeToUtf8Bytes(claims));
        var signature = key.SignData(Encoding.ASCII.GetBytes(header + "." + payload),
            HashAlgorithmName.SHA256, DSASignatureFormat.IeeeP1363FixedFieldConcatenation);
        return header + "." + payload + "." + Base64Url(signature);
    }

    private static async Task<Scope> SeedAsync(TransportErpDbContext db)
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
            Id = Guid.NewGuid(), Code = $"RPK-{Guid.NewGuid():N}"[..18], LegalNameAr = "شركة runtime",
            BaseCurrencyId = currency.Id, DefaultCalendarId = Guid.NewGuid(), Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var branch = new Branch
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, Code = $"B-{Guid.NewGuid():N}"[..12], NameAr = "فرع",
            Timezone = "Asia/Riyadh", Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var userName = $"runtime-pk-{Guid.NewGuid():N}";
        var user = new User
        {
            Id = Guid.NewGuid(), UserName = userName, NormalizedUserName = userName.ToUpperInvariant(),
            DisplayName = "Proof runtime operator", PasswordHash = "test-only", Status = "ACTIVE",
            SecurityStamp = Guid.NewGuid().ToString("N"), AuthVersion = 1,
            CompanyId = company.Id, BranchId = branch.Id, CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        db.AddRange(currency, company, branch, user);
        await db.SaveChangesAsync();
        var device = new RegisteredDevice
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, DeviceId = $"runtime-{Guid.NewGuid():N}",
            DisplayName = "Runtime proof device", Platform = "TEST", AppVersion = "1",
            RegistrationRequestId = $"runtime-request-{Guid.NewGuid():N}", CredentialHash = new string('e', 64),
            CredentialVersion = 1, Status = "ACTIVE", RegisteredByUserId = user.Id,
            ApprovedByUserId = user.Id, ApprovedAt = now, LastSeenAt = now,
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        db.RegisteredDevices.Add(device);
        await db.SaveChangesAsync();
        db.RegisteredDeviceAssignments.Add(new RegisteredDeviceAssignment
        {
            Id = Guid.NewGuid(), RegisteredDeviceId = device.Id, UserId = user.Id,
            CompanyId = company.Id, BranchId = branch.Id, Status = "ACTIVE",
            AssignedByUserId = user.Id, AssignedAt = now,
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        });
        await db.SaveChangesAsync();
        var firstSession = Session(user, company, branch, device, now);
        var secondSession = Session(user, company, branch, device, now);
        db.AuthSessions.AddRange(firstSession, secondSession);
        await db.SaveChangesAsync();
        return new(company.Id, branch.Id, user.Id, device.Id, device.DeviceId, firstSession.Id);
    }

    private static AuthSession Session(User user, Company company, Branch branch, RegisteredDevice device,
        DateTimeOffset now) => new()
    {
        Id = Guid.NewGuid(), UserId = user.Id, CompanyId = company.Id, BranchId = branch.Id,
        DeviceId = device.DeviceId, Mode = "LOCAL", SecurityStampAtIssue = user.SecurityStamp,
        AuthVersionAtIssue = 1, RefreshTokenHash = Convert.ToHexString(RandomNumberGenerator.GetBytes(32)).ToLowerInvariant(),
        RefreshTokenFamilyId = Guid.NewGuid(), IssuedAt = now, AccessTokenExpiresAt = now.AddMinutes(15),
        RefreshTokenExpiresAt = now.AddDays(1), RegisteredDeviceId = device.Id, DeviceCredentialVersion = 1,
        CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
    };

    private static string Base64Url(byte[] value)
        => Convert.ToBase64String(value).TrimEnd('=').Replace('+', '-').Replace('/', '_');

    private sealed record Scope(Guid CompanyId, Guid BranchId, Guid UserId, Guid DeviceId,
        string DeviceName, Guid SessionId);
    private sealed record Prepared(CreateProofKeyChallengeRequest ChallengeRequest, ChangeProofKeyRequest ChangeRequest,
        Guid ChallengeId, string RawChallenge, byte[] RawBody, string Htu, string NewProof, string? CurrentProof);
    private sealed record Execution(CreateProofKeyChallengeRequest ChallengeRequest, ChangeProofKeyRequest ChangeRequest,
        ProofKeyChangeResponse Result, byte[] RawBody, string Htu, string NewProof, string RawChallenge);
}
