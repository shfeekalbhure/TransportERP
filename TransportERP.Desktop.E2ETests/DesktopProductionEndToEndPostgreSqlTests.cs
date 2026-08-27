using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TransportERP.Api.Identity;
using TransportERP.Api.Sync;
using TransportERP.Application.Sync;
using TransportERP.Desktop.Application;
using TransportERP.Desktop.Offline;
using TransportERP.Infrastructure.Persistence;
using TransportERP.Offline;
using ApiProgram = global::Program;

namespace TransportERP.Desktop.E2ETests;

/// <summary>
/// Windows acceptance evidence for the complete Desktop production path. The test uses the real
/// HTTPS sign-in and activation endpoints, CNG certificate store, CurrentUser DPAPI, SQLCipher
/// outbox, DPoP transport, sync worker and PostgreSQL. Production configuration remains closed;
/// only this isolated TestServer receives the explicit test activation decision.
/// </summary>
public sealed class DesktopProductionEndToEndPostgreSqlTests
{
    private const string Password = "Desktop-E2E-Password-42!";
    private const string Issuer = "TransportERP.Desktop.E2E";
    private const string Audience = "TransportERP.Desktop.E2E.Api";
    private const string SigningKey = "transport-erp-desktop-e2e-signing-key-32-characters-minimum";

    [Fact]
    [Trait("Acceptance", "DESKTOP-E2E")]
    [Trait("Category", "PostgreSQL")]
    [Trait("Category", "HTTP")]
    public async Task Authenticated_producer_CNG_DPAPI_SQLCipher_HTTP_worker_PostgreSql_and_restart_succeed()
    {
        Assert.True(OperatingSystem.IsWindows(), "DESKTOP-E2E must execute on Windows; SKIPPED is not PASS.");
        var implementationSha = SyncClientDeploymentAuthority.ImplementationSha;
        Assert.Matches("^[0-9a-f]{40}$", implementationSha ?? string.Empty);
        var connection = RequireConnection();
        var credential = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        var keyName = "TransportERP.Desktop.E2E." + Guid.NewGuid().ToString("N");
        string? certificateThumbprint = null;
        string? localRoot = null;

        try
        {
            certificateThumbprint = CreateCertificate(keyName);
            var proof = await ReadProofAsync(certificateThumbprint);
            await using (var migrationDb = CreateDbContext(connection))
                await migrationDb.Database.MigrateAsync();
            var seeded = await SeedAsync(connection, credential, proof);
            localRoot = LocalRoot(seeded);
            DeleteDirectory(localRoot);

            using var factory = CreateFactory(connection, implementationSha!, seeded);
            var request = new DesktopOnlineSignInRequest(
                seeded.UserName, Password, seeded.CompanyId, seeded.BranchId,
                seeded.DeviceId, credential, certificateThumbprint);

            Guid localOperationId;
            string clientOperationId;
            Guid correlationId;
            using (var authenticator = new DesktopOnlineSessionAuthenticator(factory.Server.CreateHandler))
            {
                var authentication = await authenticator.AuthenticateAsync(request, CancellationToken.None);
                Assert.True(authentication.Succeeded,
                    $"Desktop online authentication failed closed with {authentication.Code}.");
                Assert.NotNull(authentication.Activation);
                using var runtime = await authentication.Activation!.CreateRuntimeAsync(CancellationToken.None);
                Assert.Equal(DesktopOfflineRuntimeMode.Ready, runtime.Status.Mode);
                Assert.True(runtime.CanQueueOperationalParties);

                var enqueued = await runtime.CreateBusinessProducer().QueueOperationalPartyAsync(
                    "Desktop PostgreSQL E2E", "700000000", "CNG DPAPI SQLCipher HTTP worker");
                localOperationId = enqueued.Operation.LocalOperationId;
                clientOperationId = enqueued.Operation.ClientOperationId;
                correlationId = enqueued.Operation.OperationCorrelationId;

                var accepted = await runtime.SynchronizeAsync();
                Assert.Equal(1, accepted.AcceptedPending);
                await ExecuteUntilCompleteAsync(factory, connection, seeded, clientOperationId);
                var completed = await PollUntilSucceededAsync(runtime, localOperationId);
                Assert.Equal(clientOperationId, completed.ClientOperationId);
                Assert.Equal(correlationId, completed.OperationCorrelationId);
            }

            var encryptedOutbox = Path.Combine(localRoot, "write-outbox.db");
            Assert.True(File.Exists(encryptedOutbox), "The production SQLCipher outbox was not created.");
            var rawDatabase = await File.ReadAllBytesAsync(encryptedOutbox);
            Assert.Equal(-1, rawDatabase.AsSpan().IndexOf(Encoding.UTF8.GetBytes("Desktop PostgreSQL E2E")));

            // A fresh authenticator and runtime recreate every production boundary: a new online
            // session, CNG handle, DPAPI key access and SQLCipher connection reopen the same outbox.
            using (var restartedAuthenticator = new DesktopOnlineSessionAuthenticator(factory.Server.CreateHandler))
            {
                var restartedAuthentication = await restartedAuthenticator.AuthenticateAsync(request, CancellationToken.None);
                Assert.True(restartedAuthentication.Succeeded,
                    $"Desktop restart authentication failed closed with {restartedAuthentication.Code}.");
                using var restarted = await restartedAuthentication.Activation!.CreateRuntimeAsync(CancellationToken.None);
                Assert.Equal(DesktopOfflineRuntimeMode.Ready, restarted.Status.Mode);
                var persisted = await restarted.GetOperationAsync(localOperationId);
                Assert.NotNull(persisted);
                Assert.Equal(OfflineOperationStatus.Succeeded, persisted!.Status);
                Assert.Equal(clientOperationId, persisted.ClientOperationId);
                Assert.Equal(correlationId, persisted.OperationCorrelationId);
            }

            await AssertPostgreSqlEvidenceAsync(connection, seeded, clientOperationId, correlationId);
        }
        finally
        {
            if (certificateThumbprint is not null) RemoveCertificate(certificateThumbprint);
            DeleteCngKey(keyName);
            if (localRoot is not null) DeleteDirectory(localRoot);
        }
    }

    private static async Task ExecuteUntilCompleteAsync(
        WebApplicationFactory<ApiProgram> factory,
        string connection,
        SeededScope seeded,
        string clientOperationId)
    {
        for (var attempt = 0; attempt < 20; attempt++)
        {
            await using var verify = CreateDbContext(connection);
            var status = await verify.SyncOperations.AsNoTracking()
                .Where(x => x.CompanyId == seeded.CompanyId &&
                            x.RegisteredDeviceId == seeded.RegisteredDeviceId &&
                            x.ClientOperationId == clientOperationId)
                .Select(x => x.Status)
                .SingleAsync();
            if (status == "SUCCEEDED") return;
            Assert.NotEqual("REJECTED", status);
            Assert.NotEqual("FAILED", status);
            await using var serviceScope = factory.Services.CreateAsyncScope();
            await serviceScope.ServiceProvider.GetRequiredService<SyncExecutionProcessor>()
                .ExecuteNextAsync(TimeSpan.FromSeconds(30));
            await Task.Delay(100);
        }
        throw new TimeoutException("The production sync worker did not persist SUCCEEDED.");
    }

    private static async Task<OfflineOperation> PollUntilSucceededAsync(
        DesktopOfflineRuntime runtime,
        Guid localOperationId)
    {
        for (var attempt = 0; attempt < 20; attempt++)
        {
            // The production client intentionally polls accepted work no faster than five seconds.
            await Task.Delay(TimeSpan.FromMilliseconds(500));
            await runtime.SynchronizeAsync();
            var operation = await runtime.GetOperationAsync(localOperationId);
            if (operation?.Status == OfflineOperationStatus.Succeeded) return operation;
        }
        throw new TimeoutException("The production Desktop outbox did not observe SUCCEEDED.");
    }

    private static WebApplicationFactory<ApiProgram> CreateFactory(
        string connection,
        string implementationSha,
        SeededScope seeded) =>
        new WebApplicationFactory<ApiProgram>().WithWebHostBuilder(builder =>
        {
            builder.UseSetting("ConnectionStrings:TransportErp", connection);
            builder.UseSetting("Auth:Mode", "LocalSessions");
            builder.UseSetting("Auth:Issuer", Issuer);
            builder.UseSetting("Auth:Audience", Audience);
            builder.UseSetting("Auth:SigningKey", SigningKey);
            builder.UseSetting("Auth:SigningKeyId", "desktop-e2e-current");
            builder.UseSetting("AllowedHosts", SyncClientDeploymentAuthority.Origin.Host);
            builder.UseSetting("Sync:Proof:PublicOrigin",
                SyncClientDeploymentAuthority.Origin.ToString().TrimEnd('/'));
            builder.UseSetting("Sync:Offline:Enabled", "true");
            builder.UseSetting("Sync:Offline:ActivationDecisionId", "DEC-G5-DESKTOP-E2E");
            builder.UseSetting("Sync:Offline:ActivationImplementationSha", implementationSha);
            builder.UseSetting("Sync:ServerExecution:Enabled", "true");
            builder.UseSetting("Sync:EffectivePolicy:SourceVersion", "desktop-e2e-v1");
            var devicePolicy = $"Sync:EffectivePolicy:Devices:{seeded.RegisteredDeviceId:D}";
            builder.UseSetting($"{devicePolicy}:CompanyId", seeded.CompanyId.ToString("D"));
            builder.UseSetting($"{devicePolicy}:BranchId", seeded.BranchId.ToString("D"));
            builder.UseSetting($"{devicePolicy}:DeviceId", seeded.DeviceId);
            builder.UseSetting($"{devicePolicy}:AllowedActions:0", "CreateOperationalParty");
        });

    private static async Task<SeededScope> SeedAsync(
        string connection,
        string credential,
        ProofBinding proof)
    {
        await using var db = CreateDbContext(connection);
        var now = DateTimeOffset.UtcNow;
        var companyId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var registeredDeviceId = Guid.NewGuid();
        var suffix = Guid.NewGuid().ToString("N");
        var userName = "desktop-e2e-" + suffix;
        var deviceId = "desktop-e2e-" + suffix;
        var currency = new Currency
        {
            Id = Guid.NewGuid(), Code = await UniqueCurrencyCodeAsync(db), NameAr = "عملة Desktop E2E",
            MinorUnit = 2, IsBase = true, Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var company = new Company
        {
            Id = companyId, Code = "DE-" + suffix[..15], LegalNameAr = "شركة Desktop E2E",
            BaseCurrencyId = currency.Id, DefaultCalendarId = Guid.NewGuid(), Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var branch = new Branch
        {
            Id = branchId, CompanyId = companyId, Code = "MAIN", NameAr = "الفرع الرئيسي",
            Timezone = "Asia/Riyadh", Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var user = new User
        {
            Id = userId, UserName = userName, NormalizedUserName = userName.ToUpperInvariant(),
            DisplayName = "Desktop E2E User", SecurityStamp = Guid.NewGuid().ToString("N"),
            AuthVersion = 1, Status = "ACTIVE", CompanyId = companyId, BranchId = branchId,
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        user.PasswordHash = new PasswordHasher<User>().HashPassword(user, Password);
        var role = new Role
        {
            Id = Guid.NewGuid(), Code = "DE-ROLE-" + suffix, NameAr = "دور Desktop E2E",
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
            var permission = await db.Permissions.SingleAsync(x => x.Code == code);
            db.RolePermissions.Add(new RolePermission
            {
                RoleId = role.Id, PermissionId = permission.Id, ScopeType = permission.ScopeType,
                CompanyId = companyId, BranchId = permission.ScopeType == "BRANCH" ? branchId : null,
                CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
            });
        }
        db.RegisteredDevices.Add(new RegisteredDevice
        {
            Id = registeredDeviceId, CompanyId = companyId, DeviceId = deviceId,
            DisplayName = "Desktop E2E device", Platform = "WINDOWS", AppVersion = "1.0",
            RegistrationRequestId = "desktop-e2e-" + suffix,
            CredentialHash = RegisteredDeviceService.HashCredential(credential), CredentialVersion = 1,
            Status = "ACTIVE", RegisteredByUserId = userId, ApprovedByUserId = userId,
            ApprovedAt = now, LastSeenAt = now, ProofPublicJwkCanonicalJson = proof.CanonicalJwk,
            ProofKeyThumbprint = proof.Thumbprint, ProofKeyVersion = 1, ProofKeyChangedAt = now,
            ProofKeyChangedByUserId = userId, CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        });
        db.RegisteredDeviceAssignments.Add(new RegisteredDeviceAssignment
        {
            Id = Guid.NewGuid(), RegisteredDeviceId = registeredDeviceId, UserId = userId,
            CompanyId = companyId, BranchId = branchId, Status = "ACTIVE", AssignedByUserId = userId,
            AssignedAt = now, CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        });
        await db.SaveChangesAsync();
        return new SeededScope(companyId, branchId, userId, registeredDeviceId, userName, deviceId);
    }

    private static async Task AssertPostgreSqlEvidenceAsync(
        string connection,
        SeededScope seeded,
        string clientOperationId,
        Guid correlationId)
    {
        await using var db = CreateDbContext(connection);
        var operation = await db.SyncOperations.AsNoTracking().SingleAsync(x =>
            x.CompanyId == seeded.CompanyId && x.RegisteredDeviceId == seeded.RegisteredDeviceId &&
            x.ClientOperationId == clientOperationId);
        Assert.Equal("SUCCEEDED", operation.Status);
        Assert.Equal(correlationId, operation.OperationCorrelationId);
        var businessKey = SyncBusinessIdempotencyKey.Create(
            seeded.CompanyId, seeded.BranchId, seeded.RegisteredDeviceId, clientOperationId);
        Assert.Single(await db.Set<OperationalPartyEntity>().AsNoTracking().Where(x =>
            x.CompanyId == seeded.CompanyId && x.ClientOperationId == businessKey).ToListAsync());
        var actions = await db.AuditEvents.AsNoTracking()
            .Where(x => x.CompanyId == seeded.CompanyId && x.OperationCorrelationId == correlationId)
            .Select(x => x.Action).ToListAsync();
        Assert.Contains("SyncOperationQueued", actions);
        Assert.Contains("SyncOperationExecutionSucceeded", actions);
    }

    private static async Task<string> UniqueCurrencyCodeAsync(TransportErpDbContext db)
    {
        for (var attempt = 0; attempt < 100; attempt++)
        {
            var code = "D" + Convert.ToHexString(RandomNumberGenerator.GetBytes(1));
            if (!await db.Currencies.AsNoTracking().AnyAsync(x => x.Code == code)) return code;
        }
        throw new InvalidOperationException("Unable to allocate a Desktop E2E currency code.");
    }

    private static TransportErpDbContext CreateDbContext(string connection)
    {
        var options = new DbContextOptionsBuilder<TransportErpDbContext>();
        options.ConfigureTransportErpPostgreSql(connection);
        return new TransportErpDbContext(options.Options);
    }

    private static string RequireConnection() =>
        Environment.GetEnvironmentVariable("TRANSPORTERP_TEST_CONNSTR") is { Length: > 0 } value
            ? value
            : throw new InvalidOperationException(
                "TRANSPORTERP_TEST_CONNSTR is required; DESKTOP-E2E never skips PostgreSQL.");

    private static string LocalRoot(SeededScope seeded) => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "TransportERP", "offline", seeded.CompanyId.ToString("N"), seeded.BranchId.ToString("N"),
        seeded.UserId.ToString("N"), seeded.RegisteredDeviceId.ToString("N"));

    private static string CreateCertificate(string keyName)
    {
        var parameters = new CngKeyCreationParameters
        {
            Provider = CngProvider.MicrosoftSoftwareKeyStorageProvider,
            ExportPolicy = CngExportPolicies.None,
            KeyUsage = CngKeyUsages.Signing
        };
        using var key = CngKey.Create(CngAlgorithm.ECDsaP256, keyName, parameters);
        using var signer = new ECDsaCng(key);
        var request = new CertificateRequest(
            new X500DistinguishedName("CN=TransportERP Desktop PostgreSQL E2E"),
            signer, HashAlgorithmName.SHA256);
        using var certificate = request.CreateSelfSigned(
            DateTimeOffset.UtcNow.AddMinutes(-1), DateTimeOffset.UtcNow.AddHours(1));
        using var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
        store.Open(OpenFlags.ReadWrite);
        store.Add(certificate);
        return certificate.Thumbprint;
    }

    private static async Task<ProofBinding> ReadProofAsync(string thumbprint)
    {
        using var signingKey = await new WindowsCertificateDeviceProofSigningKeyStore().OpenAsync(thumbprint);
        var key = signingKey.PublicKey;
        var canonical = $"{{\"crv\":\"P-256\",\"kty\":\"EC\",\"x\":\"{key.X}\",\"y\":\"{key.Y}\"}}";
        var hash = SHA256.HashData(Encoding.ASCII.GetBytes(canonical));
        var proofThumbprint = Convert.ToBase64String(hash).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        return new ProofBinding(canonical, proofThumbprint);
    }

    private static void RemoveCertificate(string thumbprint)
    {
        try
        {
            using var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite);
            foreach (var certificate in store.Certificates.Find(
                         X509FindType.FindByThumbprint, thumbprint, validOnly: false))
                store.Remove(certificate);
        }
        catch (CryptographicException) { }
    }

    private static void DeleteCngKey(string keyName)
    {
        try
        {
            using var key = CngKey.Open(keyName, CngProvider.MicrosoftSoftwareKeyStorageProvider);
            key.Delete();
        }
        catch (CryptographicException) { }
    }

    private static void DeleteDirectory(string path)
    {
        try
        {
            if (Directory.Exists(path)) Directory.Delete(path, recursive: true);
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException) { }
    }

    private sealed record ProofBinding(string CanonicalJwk, string Thumbprint);
    private sealed record SeededScope(
        Guid CompanyId,
        Guid BranchId,
        Guid UserId,
        Guid RegisteredDeviceId,
        string UserName,
        string DeviceId);
}
