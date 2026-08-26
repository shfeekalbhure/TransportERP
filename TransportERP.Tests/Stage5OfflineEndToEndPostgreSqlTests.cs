using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using TransportERP.Api.Security;
using TransportERP.Api.Sync;
using TransportERP.Infrastructure.Persistence;
using TransportERP.Offline;
using TransportERP.Offline.Transport;

namespace TransportERP.Tests;

/// <summary>
/// Cross-process-boundary acceptance evidence: an encrypted client outbox talks to the real HTTP
/// sync-v1 endpoint, which persists in PostgreSQL and is completed by the real typed processor.
/// The production Offline and server-worker switches remain false; only this isolated TestServer
/// replaces the gate and retry-policy source, and the processor is driven explicitly by the test.
/// </summary>
[Collection("PostgreSql")]
public sealed class Stage5OfflineEndToEndPostgreSqlTests
{
    private const string Issuer = "TransportERP.Stage5.E2E";
    private const string Audience = "TransportERP.Stage5.E2E.Api";
    private const string SigningKey = "transport-erp-stage5-e2e-signing-key-32-characters-minimum";
    private static readonly Uri PublicOrigin = new("https://sync.example.test");
    private static readonly Uri BatchEndpoint = new(PublicOrigin, "/api/v1/sync/operations:batch");

    [Fact]
    [Trait("Category", "PostgreSQL")]
    [Trait("Category", "HTTP")]
    [Trait("Category", "OfflineE2E")]
    public async Task Encrypted_outbox_reopens_then_nonce_batch_worker_and_status_replay_reach_succeeded()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var seedDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await seedDb.Database.MigrateAsync();
        using var proofKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var scope = await SeedAsync(seedDb, proofKey, "REOPEN");
        using var factory = CreateFactory(connection);
        using var inspection = new ProofInspectionHandler(factory.Server.CreateHandler());
        using var http = new HttpClient(inspection) { BaseAddress = PublicOrigin };
        var time = new AdjustableTimeProvider(DateTimeOffset.UtcNow);
        var directory = CreateTemporaryDirectory();
        try
        {
            var path = Path.Combine(directory, "outbox.db");
            var keys = new FixedLocalKeyProvider(RandomNumberGenerator.GetBytes(32));
            var firstStore = CreateStore(path, keys, time);
            await firstStore.InitializeAsync();
            var enqueued = await EnqueuePartyAsync(firstStore, scope, time.GetUtcNow(), "reopen");

            // A new store instance proves that identity and payload survived an actual encrypted
            // database close/reopen boundary, not merely an in-memory service recreation.
            var reopenedStore = CreateStore(path, keys, time);
            await reopenedStore.InitializeAsync();
            var beforeSend = await reopenedStore.GetAsync(
                enqueued.Operation.LocalOperationId, LocalScope(scope));
            Assert.NotNull(beforeSend);
            Assert.Equal(enqueued.Operation.ClientOperationId, beforeSend!.ClientOperationId);
            Assert.Equal(enqueued.Operation.OperationCorrelationId, beforeSend.OperationCorrelationId);

            var transport = CreateTransport(http, reopenedStore, proofKey, scope, time, "reopen-worker");
            var accepted = await transport.ProcessNextBatchAsync();
            var pendingLocal = await reopenedStore.GetAsync(
                enqueued.Operation.LocalOperationId, LocalScope(scope));
            Assert.True(accepted.Claimed == 1 && accepted.AcceptedPending == 1,
                $"Expected one accepted pending operation; result={accepted}; " +
                $"local={pendingLocal?.Status}/{pendingLocal?.ResultCode}; proofCheck={inspection.Result}.");
            Assert.Equal(OfflineOperationStatus.Queued, pendingLocal!.Status);
            Assert.Equal("QUEUED", pendingLocal.ResultCode);
            Assert.NotNull(pendingLocal.ServerOperationId);

            await AssertServerStateAsync(connection, scope, enqueued.Operation, "QUEUED", partyCount: 0);
            Assert.True(await ExecuteOneServerOperationAsync(factory));
            await AssertServerStateAsync(connection, scope, enqueued.Operation, "SUCCEEDED", partyCount: 1);

            time.Advance(TimeSpan.FromSeconds(1));
            var polled = await transport.ProcessNextBatchAsync();
            Assert.Equal(1, polled.Succeeded);
            var succeeded = await reopenedStore.GetAsync(
                enqueued.Operation.LocalOperationId, LocalScope(scope));
            Assert.Equal(OfflineOperationStatus.Succeeded, succeeded!.Status);
            Assert.Equal(enqueued.Operation.ClientOperationId, succeeded.ClientOperationId);
            Assert.Equal(enqueued.Operation.OperationCorrelationId, succeeded.OperationCorrelationId);
            Assert.Equal(pendingLocal.ServerOperationId, succeeded.ServerOperationId);
            await AssertEvidenceAsync(connection, scope, enqueued.Operation);
        }
        finally
        {
            DeleteTemporaryDirectory(directory);
        }
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    [Trait("Category", "HTTP")]
    [Trait("Category", "OfflineE2E")]
    public async Task Lost_response_after_acceptance_replays_stable_business_identity_without_duplicate_effect()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var seedDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await seedDb.Database.MigrateAsync();
        using var proofKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var scope = await SeedAsync(seedDb, proofKey, "LOST");
        using var factory = CreateFactory(connection);
        using var inspection = new ProofInspectionHandler(factory.Server.CreateHandler());
        using var dropping = new DropFirstSignedSuccessHandler(inspection);
        using var http = new HttpClient(dropping) { BaseAddress = PublicOrigin };
        var time = new AdjustableTimeProvider(DateTimeOffset.UtcNow);
        var directory = CreateTemporaryDirectory();
        try
        {
            var store = CreateStore(Path.Combine(directory, "outbox.db"),
                new FixedLocalKeyProvider(RandomNumberGenerator.GetBytes(32)), time);
            await store.InitializeAsync();
            var enqueued = await EnqueuePartyAsync(store, scope, time.GetUtcNow(), "lost-response");

            // The inner TestServer completed and committed its 200/QUEUED response; the wrapper
            // drops it before the client can observe it, exactly modelling timeout-after-accept.
            var lost = await CreateTransport(http, store, proofKey, scope, time, "lost-worker")
                .ProcessNextBatchAsync();
            var failedLocal = await store.GetAsync(
                enqueued.Operation.LocalOperationId, LocalScope(scope));
            Assert.True(lost.RetryScheduled == 1 && dropping.DroppedResponses == 1,
                $"Expected one retry after a dropped signed response; result={lost}; " +
                $"dropped={dropping.DroppedResponses}; local={failedLocal?.Status}/{failedLocal?.ResultCode}; " +
                $"proofCheck={inspection.Result}.");
            Assert.Equal(OfflineOperationStatus.Failed, failedLocal!.Status);
            Assert.Equal(1, failedLocal.ClientTransportRetryCount);
            await AssertServerStateAsync(connection, scope, enqueued.Operation, "QUEUED", partyCount: 0);

            Assert.True(await ExecuteOneServerOperationAsync(factory));
            await AssertServerStateAsync(connection, scope, enqueued.Operation, "SUCCEEDED", partyCount: 1);

            time.Advance(TimeSpan.FromSeconds(2));
            var replayed = await CreateTransport(http, store, proofKey, scope, time, "lost-worker")
                .ProcessNextBatchAsync();
            Assert.Equal(1, replayed.Succeeded);
            var local = await store.GetAsync(
                enqueued.Operation.LocalOperationId, LocalScope(scope));
            Assert.Equal(OfflineOperationStatus.Succeeded, local!.Status);
            Assert.Equal(enqueued.Operation.ClientOperationId, local.ClientOperationId);
            Assert.Equal(enqueued.Operation.OperationCorrelationId, local.OperationCorrelationId);

            await using var verify = PostgreSqlTestEnvironment.CreateDbContext(connection);
            Assert.Single(await verify.SyncOperations.AsNoTracking().Where(x =>
                x.CompanyId == scope.CompanyId &&
                x.RegisteredDeviceId == scope.RegisteredDeviceId &&
                x.ClientOperationId == enqueued.Operation.ClientOperationId).ToListAsync());
            Assert.Single(await verify.Set<OperationalPartyEntity>().AsNoTracking().Where(x =>
                x.CompanyId == scope.CompanyId &&
                x.ClientOperationId == enqueued.Operation.ClientOperationId).ToListAsync());
            await AssertEvidenceAsync(connection, scope, enqueued.Operation);
        }
        finally
        {
            DeleteTemporaryDirectory(directory);
        }
    }

    private static OfflineOperationStore CreateStore(
        string path,
        ILocalEncryptionKeyProvider keys,
        TimeProvider time)
        => new(path, keys, time, new OfflineRetryPolicy(
            MaxRetryCount: 5, BaseDelay: TimeSpan.FromSeconds(1), MaxDelay: TimeSpan.FromMinutes(1)));

    private static OfflineOperationScope LocalScope(E2eScope scope)
        => new(scope.CompanyId, scope.BranchId, scope.UserId, scope.RegisteredDeviceId);

    private static OfflineSyncTransportClient CreateTransport(
        HttpClient http,
        OfflineOperationStore store,
        ECDsa proofKey,
        E2eScope scope,
        TimeProvider time,
        string workerId)
        => new(
            http,
            store,
            new FixedBearerProvider(scope.Bearer),
            new EcdsaDeviceSigningKey(proofKey),
            new OfflineSyncTransportOptions(
                BatchEndpoint,
                scope.DeviceId,
                scope.RegisteredDeviceId,
                scope.CompanyId,
                scope.BranchId,
                scope.UserId,
                workerId,
                LeaseDuration: TimeSpan.FromSeconds(30),
                AcceptedPollInterval: TimeSpan.FromMilliseconds(100)),
            time);

    private static Task<OfflineEnqueueResult> EnqueuePartyAsync(
        OfflineOperationStore store,
        E2eScope scope,
        DateTimeOffset occurredAt,
        string suffix)
    {
        var template = new OfflineOperationEnqueueTemplate(
            Guid.NewGuid(), scope.CompanyId, scope.BranchId, scope.UserId, scope.RegisteredDeviceId,
            "CreateOperationalParty", "CREATE", "OperationalParty", null, null, occurredAt);
        return store.EnqueueAsync(template, identity => JsonSerializer.Serialize(new
        {
            name = $"Offline E2E {suffix}",
            mobile = "700000000",
            identityType = (string?)null,
            identityNo = (string?)null,
            address = new
            {
                countryId = (Guid?)null,
                governorateId = (Guid?)null,
                cityId = (Guid?)null,
                areaId = (Guid?)null,
                addressLine = "Encrypted offline E2E"
            },
            clientOperationId = identity.ClientOperationId
        }));
    }

    private static async Task<bool> ExecuteOneServerOperationAsync(WebApplicationFactory<Program> factory)
    {
        await using var scope = factory.Services.CreateAsyncScope();
        return await scope.ServiceProvider.GetRequiredService<SyncExecutionProcessor>()
            .ExecuteNextAsync(TimeSpan.FromSeconds(30));
    }

    private static async Task AssertServerStateAsync(
        string connection,
        E2eScope scope,
        OfflineOperation local,
        string status,
        int partyCount)
    {
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        var operation = await db.SyncOperations.AsNoTracking().SingleAsync(x =>
            x.CompanyId == scope.CompanyId &&
            x.RegisteredDeviceId == scope.RegisteredDeviceId &&
            x.ClientOperationId == local.ClientOperationId);
        Assert.Equal(status, operation.Status);
        Assert.Equal(local.OperationCorrelationId, operation.OperationCorrelationId);
        Assert.Equal(partyCount, await db.Set<OperationalPartyEntity>().AsNoTracking().CountAsync(x =>
            x.CompanyId == scope.CompanyId && x.ClientOperationId == local.ClientOperationId));
    }

    private static async Task AssertEvidenceAsync(string connection, E2eScope scope, OfflineOperation local)
    {
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        var operation = await db.SyncOperations.AsNoTracking().SingleAsync(x =>
            x.CompanyId == scope.CompanyId &&
            x.RegisteredDeviceId == scope.RegisteredDeviceId &&
            x.ClientOperationId == local.ClientOperationId);
        Assert.Equal("SUCCEEDED", operation.Status);
        Assert.Equal(local.OperationCorrelationId, operation.OperationCorrelationId);
        Assert.NotNull(operation.ResultEntityId);
        Assert.Equal(1, operation.ResultVersion);
        Assert.True(await db.SyncProofReplays.AsNoTracking().CountAsync(x =>
            x.CompanyId == scope.CompanyId && x.RegisteredDeviceId == scope.RegisteredDeviceId) >= 2);

        var actions = await db.AuditEvents.AsNoTracking()
            .Where(x => x.CompanyId == scope.CompanyId &&
                        x.OperationCorrelationId == local.OperationCorrelationId)
            .Select(x => x.Action).ToListAsync();
        Assert.Contains("SyncOperationQueued", actions);
        Assert.Contains("SyncOperationExecutionClaimed", actions);
        Assert.Contains("SyncBusinessDispatchAttempt", actions);
        Assert.Contains("SyncOperationExecutionSucceeded", actions);
    }

    private static WebApplicationFactory<Program> CreateFactory(string connection)
        => new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseSetting("ConnectionStrings:TransportErp", connection);
            builder.UseSetting("Auth:Mode", "LocalSessions");
            builder.UseSetting("Auth:Issuer", Issuer);
            builder.UseSetting("Auth:Audience", Audience);
            builder.UseSetting("Auth:SigningKey", SigningKey);
            builder.UseSetting("Auth:SigningKeyId", "stage5-e2e-current");
            builder.UseSetting("Sync:Proof:PublicOrigin", PublicOrigin.ToString().TrimEnd('/'));
            builder.UseSetting("AllowedHosts", PublicOrigin.Host);
            // Do not start the hosted worker and do not change Offline. The test drives one real
            // processor iteration deterministically after proving the persisted QUEUED state.
            builder.UseSetting("Sync:ServerExecution:Enabled", "false");
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<ISyncRuntimeGate>();
                services.AddScoped<ISyncRuntimeGate, IsolatedOpenSyncRuntimeGate>();
                services.RemoveAll<ISyncRetryPolicyResolver>();
                services.AddSingleton<ISyncRetryPolicyResolver>(new FixedSyncRetryPolicyResolver(
                    new SyncRetryPolicy(5, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(30))));
            });
        });

    private static async Task<E2eScope> SeedAsync(
        TransportErpDbContext db,
        ECDsa proofKey,
        string suffix)
    {
        var now = DateTimeOffset.UtcNow;
        var companyId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        var registeredDeviceId = Guid.NewGuid();
        var deviceId = $"stage5-e2e-{suffix.ToLowerInvariant()}-{Guid.NewGuid():N}";
        var currency = new Currency
        {
            Id = Guid.NewGuid(), Code = await PostgreSqlTestCurrencyCodeAllocator.NextAsync(db),
            NameAr = "عملة اختبار E2E", MinorUnit = 2, IsBase = true, Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var company = new Company
        {
            Id = companyId, Code = $"E5-{suffix}-{Guid.NewGuid():N}"[..18],
            LegalNameAr = "شركة اختبار Offline E2E", BaseCurrencyId = currency.Id,
            DefaultCalendarId = Guid.NewGuid(), Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var branch = new Branch
        {
            Id = branchId, CompanyId = companyId, Code = "MAIN", NameAr = "الفرع الرئيسي",
            Timezone = "Asia/Riyadh", Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var securityStamp = Guid.NewGuid().ToString("N");
        var user = new User
        {
            Id = userId, UserName = $"stage5-{Guid.NewGuid():N}",
            NormalizedUserName = $"STAGE5{Guid.NewGuid():N}", DisplayName = "مستخدم E2E",
            PasswordHash = "test-only", SecurityStamp = securityStamp, AuthVersion = 1, Status = "ACTIVE",
            CompanyId = companyId, BranchId = branchId, CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var role = new Role
        {
            Id = Guid.NewGuid(), Code = $"E5-{suffix}-{Guid.NewGuid():N}", NameAr = "دور E2E",
            CompanyId = companyId, Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        db.AddRange(currency, company, branch, user, role);
        db.UserRoles.Add(new UserRole
        {
            UserId = userId, RoleId = role.Id, CompanyId = companyId, BranchId = branchId,
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        });
        foreach (var code in new[] { "sync.operations.execute", "party.create" })
        {
            var permission = await db.Permissions.SingleOrDefaultAsync(x => x.Code == code);
            if (permission is null)
            {
                permission = new Permission
                {
                    Id = Guid.NewGuid(), Code = code, NameAr = code, Resource = code.Split('.')[0],
                    Action = code.Split('.')[^1], ScopeType = "BRANCH", Status = "ACTIVE",
                    CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
                };
                db.Permissions.Add(permission);
            }
            var grantCompany = permission.ScopeType == "PLATFORM" ? (Guid?)null : companyId;
            var grantBranch = permission.ScopeType == "BRANCH" ? branchId : (Guid?)null;
            db.RolePermissions.Add(new RolePermission
            {
                RoleId = role.Id, PermissionId = permission.Id, ScopeType = permission.ScopeType,
                CompanyId = grantCompany, BranchId = grantBranch, CreatedAt = now, UpdatedAt = now,
                RowVersion = RandomNumberGenerator.GetBytes(16)
            });
        }

        var publicParameters = proofKey.ExportParameters(false);
        var x = Base64Url(publicParameters.Q.X!);
        var y = Base64Url(publicParameters.Q.Y!);
        var canonicalJwk = $"{{\"crv\":\"P-256\",\"kty\":\"EC\",\"x\":\"{x}\",\"y\":\"{y}\"}}";
        var thumbprint = Base64Url(SHA256.HashData(Encoding.ASCII.GetBytes(canonicalJwk)));
        db.RegisteredDevices.Add(new RegisteredDevice
        {
            Id = registeredDeviceId, CompanyId = companyId, DeviceId = deviceId,
            DisplayName = "Stage5 E2E device", Platform = "TEST", AppVersion = "1.0",
            RegistrationRequestId = "stage5-e2e-" + Guid.NewGuid().ToString("N"),
            CredentialHash = new string('a', 64), CredentialVersion = 1, Status = "ACTIVE",
            RegisteredByUserId = userId, ApprovedByUserId = userId, ApprovedAt = now, LastSeenAt = now,
            ProofPublicJwkCanonicalJson = canonicalJwk, ProofKeyThumbprint = thumbprint, ProofKeyVersion = 1,
            ProofKeyChangedAt = now, ProofKeyChangedByUserId = userId,
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        });
        db.RegisteredDeviceAssignments.Add(new RegisteredDeviceAssignment
        {
            Id = Guid.NewGuid(), RegisteredDeviceId = registeredDeviceId, UserId = userId,
            CompanyId = companyId, BranchId = branchId, Status = "ACTIVE", AssignedByUserId = userId,
            AssignedAt = now, CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        });
        db.AuthSessions.Add(new AuthSession
        {
            Id = sessionId, UserId = userId, CompanyId = companyId, BranchId = branchId,
            DeviceId = deviceId, RegisteredDeviceId = registeredDeviceId, DeviceCredentialVersion = 1,
            Mode = "LOCAL", SecurityStampAtIssue = securityStamp, AuthVersionAtIssue = 1,
            RefreshTokenHash = Convert.ToHexString(SHA256.HashData(sessionId.ToByteArray())).ToLowerInvariant(),
            RefreshTokenFamilyId = Guid.NewGuid(), IssuedAt = now, AccessTokenExpiresAt = now.AddMinutes(15),
            RefreshTokenExpiresAt = now.AddDays(1), CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        });
        await db.SaveChangesAsync();
        var bearer = CreateToken(userId, companyId, branchId, sessionId, registeredDeviceId, deviceId, securityStamp);
        return new E2eScope(companyId, branchId, userId, sessionId, registeredDeviceId, deviceId, bearer);
    }

    private static string CreateToken(
        Guid userId,
        Guid companyId,
        Guid branchId,
        Guid sessionId,
        Guid registeredDeviceId,
        string deviceId,
        string securityStamp)
    {
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString("D")),
                new Claim("company_id", companyId.ToString("D")),
                new Claim("branch_id", branchId.ToString("D")),
                new Claim("sid", sessionId.ToString("D")),
                new Claim("device_id", deviceId),
                new Claim("registered_device_id", registeredDeviceId.ToString("D")),
                new Claim("device_credential_version", "1"),
                new Claim("security_stamp", securityStamp),
                new Claim("auth_version", "1")
            ]),
            Issuer = Issuer,
            Audience = Audience,
            Expires = DateTime.UtcNow.AddMinutes(10),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey)) { KeyId = "stage5-e2e-current" },
                SecurityAlgorithms.HmacSha256)
        };
        return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityTokenHandler().CreateToken(descriptor));
    }

    private static string Base64Url(ReadOnlySpan<byte> value)
        => Convert.ToBase64String(value).TrimEnd('=').Replace('+', '-').Replace('/', '_');

    private static string CreateTemporaryDirectory()
    {
        var path = Path.Combine(Path.GetTempPath(), "transport-erp-stage5-e2e-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(path);
        return path;
    }

    private static void DeleteTemporaryDirectory(string path)
    {
        if (Directory.Exists(path)) Directory.Delete(path, recursive: true);
    }

    private sealed record E2eScope(
        Guid CompanyId,
        Guid BranchId,
        Guid UserId,
        Guid SessionId,
        Guid RegisteredDeviceId,
        string DeviceId,
        string Bearer);

    private sealed class IsolatedOpenSyncRuntimeGate : ISyncRuntimeGate
    {
        public Task<EffectiveSyncPolicy> ResolveAsync(
            CurrentSecurityContext current,
            CancellationToken cancellationToken)
            => Task.FromResult(new EffectiveSyncPolicy(
                true,
                new HashSet<string>(["CreateOperationalParty"], StringComparer.Ordinal),
                new HashSet<string>(["sync-v1"], StringComparer.Ordinal),
                100, 2_097_152, 16_384, 5, 5, 5, 5, 30, 30, 24, 7, 90, 24, null));
    }

    private sealed class FixedLocalKeyProvider(byte[] key) : ILocalEncryptionKeyProvider
    {
        private readonly byte[] _key = key.ToArray();

        public ValueTask<byte[]> GetKeyAsync(
            LocalStorePurpose purpose,
            CancellationToken cancellationToken = default)
            => ValueTask.FromResult(_key.ToArray());
    }

    private sealed class FixedBearerProvider(string bearer) : IInMemoryBearerTokenProvider
    {
        public ValueTask<string> GetBearerTokenAsync(CancellationToken cancellationToken = default)
            => ValueTask.FromResult(bearer);
    }

    private sealed class EcdsaDeviceSigningKey(ECDsa key) : IDeviceProofSigningKey
    {
        public ValueTask<DevicePublicP256Jwk> GetPublicJwkAsync(CancellationToken cancellationToken = default)
        {
            var parameters = key.ExportParameters(false);
            return ValueTask.FromResult(new DevicePublicP256Jwk(
                Base64Url(parameters.Q.X!), Base64Url(parameters.Q.Y!)));
        }

        public ValueTask<byte[]> SignEs256Async(
            ReadOnlyMemory<byte> signingInput,
            CancellationToken cancellationToken = default)
            => ValueTask.FromResult(key.SignData(
                signingInput.Span,
                HashAlgorithmName.SHA256,
                DSASignatureFormat.IeeeP1363FixedFieldConcatenation));
    }

    private sealed class AdjustableTimeProvider(DateTimeOffset now) : TimeProvider
    {
        private DateTimeOffset _now = now;
        public override DateTimeOffset GetUtcNow() => _now;
        public void Advance(TimeSpan duration) => _now = _now.Add(duration);
    }

    private sealed class DropFirstSignedSuccessHandler(HttpMessageHandler inner) : DelegatingHandler(inner)
    {
        private int _dropped;
        public int DroppedResponses => Volatile.Read(ref _dropped);

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            if (request.Headers.Contains("DPoP") && response.IsSuccessStatusCode &&
                Interlocked.CompareExchange(ref _dropped, 1, 0) == 0)
            {
                response.Dispose();
                throw new HttpRequestException("Injected response loss after the server committed acceptance.");
            }
            return response;
        }
    }

    private sealed class ProofInspectionHandler(HttpMessageHandler inner) : DelegatingHandler(inner)
    {
        public string Result { get; private set; } = "NOT_SEEN";

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (request.Headers.TryGetValues("DPoP", out var proofs))
            {
                try
                {
                    var proof = Assert.Single(proofs);
                    var body = await request.Content!.ReadAsByteArrayAsync(cancellationToken);
                    var bearer = request.Headers.Authorization?.Parameter ?? string.Empty;
                    var correlation = Guid.ParseExact(
                        Assert.Single(request.Headers.GetValues("X-Correlation-Id")), "D");
                    var material = new SyncPopProofValidator().Validate(new SyncPopProofValidationInput(
                        proof, bearer, body, BatchEndpoint.AbsoluteUri, correlation, DateTimeOffset.UtcNow));
                    Result = $"PASS:{material.ProofKeyThumbprint}";
                }
                catch (Exception exception)
                {
                    Result = "FAIL:" + exception.GetType().Name;
                }
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
