using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
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
/// outbox, DPoP transport, sync worker and PostgreSQL. The release-process case starts the normal
/// API entry point on Kestrel; the narrower policy case retains TestServer for deterministic
/// policy-boundary assertions. Production configuration remains closed in both cases.
/// </summary>
public sealed class DesktopProductionEndToEndPostgreSqlTests
{
    private const string Password = "Desktop-E2E-Password-42!";
    private const string Issuer = "TransportERP.Desktop.E2E";
    private const string Audience = "TransportERP.Desktop.E2E.Api";
    private const string SigningKey = "transport-erp-desktop-e2e-signing-key-32-characters-minimum";

    [Fact]
    [Trait("Acceptance", "DESKTOP-E2E")]
    public void Measured_deployment_tree_changes_when_any_binary_changes()
    {
        var root = Path.Combine(Path.GetTempPath(), "transporterp-build-identity", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            File.WriteAllBytes(Path.Combine(root, "TransportERP.Desktop.exe"), [1, 2, 3]);
            File.WriteAllBytes(Path.Combine(root, "TransportERP.Application.dll"), [4, 5, 6]);
            var original = DesktopBuildIdentityProbe.Measure(root);
            File.WriteAllBytes(Path.Combine(root, "TransportERP.Application.dll"), [4, 5, 7]);
            var substituted = DesktopBuildIdentityProbe.Measure(root);

            Assert.True(original.IsValid);
            Assert.True(substituted.IsValid);
            Assert.False(original.FixedTimeEquals(substituted));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    [Trait("Acceptance", "DESKTOP-E2E")]
    public async Task WinExe_writes_its_measured_build_identity_to_an_explicit_new_file()
    {
        Assert.True(OperatingSystem.IsWindows(), "DESKTOP-E2E must execute on Windows; SKIPPED is not PASS.");
        var executable = Path.Combine(ReleaseDeploymentDirectory(), "TransportERP.Desktop.exe");
        Assert.True(File.Exists(executable), $"Desktop release executable is missing: {executable}");
        var outputDirectory = Path.Combine(Path.GetTempPath(), "transporterp-desktop-identity", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(outputDirectory);
        var output = Path.Combine(outputDirectory, "build-identity.json");
        try
        {
            using var process = Process.Start(new ProcessStartInfo
            {
                FileName = executable,
                UseShellExecute = false,
                ArgumentList = { "--print-build-identity", output }
            });
            Assert.NotNull(process);
            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            await process!.WaitForExitAsync(timeout.Token);
            Assert.Equal(0, process.ExitCode);
            Assert.True(File.Exists(output));
            var actual = JsonSerializer.Deserialize<BuildIdentityV1>(
                await File.ReadAllTextAsync(output, timeout.Token),
                new JsonSerializerOptions(JsonSerializerDefaults.Web));
            var expected = DesktopBuildIdentityProbe.Measure(Path.GetDirectoryName(executable));
            Assert.NotNull(actual);
            Assert.True(actual!.FixedTimeEquals(expected));
        }
        finally
        {
            Directory.Delete(outputDirectory, recursive: true);
        }
    }

    [Fact]
    [Trait("Acceptance", "DESKTOP-E2E")]
    public async Task Release_WinExe_starts_closed_and_is_driven_through_stable_UI_Automation_ids()
    {
        Assert.True(OperatingSystem.IsWindows(), "DESKTOP-E2E must execute on Windows; SKIPPED is not PASS.");
        var executable = Path.Combine(ReleaseDeploymentDirectory(), "TransportERP.Desktop.exe");
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        await using var desktop = await DesktopReleaseUiAutomation.LaunchAsync(executable, timeout.Token);
        Assert.True(string.Equals(
            Path.GetFullPath(executable), Path.GetFullPath(desktop.ExecutablePath),
            StringComparison.OrdinalIgnoreCase));
        desktop.AssertClosedDefault();
    }

    [Fact]
    [Trait("Acceptance", "DESKTOP-E2E")]
    [Trait("Category", "PostgreSQL")]
    [Trait("Category", "HTTP")]
    public async Task Same_release_WinExe_UI_Kestrel_SQLCipher_worker_and_PostgreSql_succeed()
    {
        Assert.True(OperatingSystem.IsWindows(), "DESKTOP-E2E must execute on Windows; SKIPPED is not PASS.");
        var implementationSha = SyncClientDeploymentAuthority.ImplementationSha;
        Assert.Matches("^[0-9a-f]{40}$", implementationSha ?? string.Empty);
        var origin = SyncClientDeploymentAuthority.Origin;
        var executable = Path.Combine(ReleaseDeploymentDirectory(), "TransportERP.Desktop.exe");
        Assert.True(File.Exists(executable));
        var measuredBuildIdentity = DesktopBuildIdentityProbe.Measure(Path.GetDirectoryName(executable));
        Assert.True(measuredBuildIdentity.IsValid);
        var connection = RequireConnection();
        var credential = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        var keyName = "TransportERP.Desktop.Release.E2E." + Guid.NewGuid().ToString("N");
        string? certificateThumbprint = null;
        string? localRoot = null;

        using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(3));
        try
        {
            certificateThumbprint = CreateCertificate(keyName);
            var proof = await ReadProofAsync(certificateThumbprint);
            await using (var migrationDb = CreateDbContext(connection))
                await migrationDb.Database.MigrateAsync(timeout.Token);
            var seeded = await SeedAsync(connection, credential, proof);
            localRoot = LocalRoot(seeded);
            DeleteDirectory(localRoot);

            var settings = ReleaseHostConfiguration(
                connection, origin, implementationSha!, measuredBuildIdentity, seeded);
            await using var api = await DesktopReleaseKestrelApiHost.StartAsync(
                origin, settings, timeout.Token);
            await using (var desktop = await DesktopReleaseUiAutomation.LaunchAsync(
                             executable, timeout.Token))
            {
                Assert.True(string.Equals(
                    Path.GetFullPath(executable), Path.GetFullPath(desktop.ExecutablePath),
                    StringComparison.OrdinalIgnoreCase));
                desktop.AssertClosedDefault();
                await desktop.SignInAsync(
                    seeded.UserName, Password, seeded.CompanyId, seeded.BranchId,
                    seeded.DeviceId, credential, certificateThumbprint, timeout.Token);
                await desktop.QueueOperationalPartyAsync(
                    "Desktop Release UI E2E", "700000001", "UI Automation Kestrel PostgreSQL",
                    timeout.Token);
                Assert.True(desktop.ReadStatus().StartsWith(
                    "تمت إضافة العملية المشفرة", StringComparison.Ordinal));
                await WaitForReleaseOperationAsync(connection, seeded, timeout.Token);
                await desktop.CloseNormallyAsync(timeout.Token);
            }
            await using (var restartedDesktop = await DesktopReleaseUiAutomation.LaunchAsync(
                             executable, timeout.Token))
            {
                Assert.True(string.Equals(
                    Path.GetFullPath(executable), Path.GetFullPath(restartedDesktop.ExecutablePath),
                    StringComparison.OrdinalIgnoreCase));
                restartedDesktop.AssertClosedDefault();
                await restartedDesktop.SignInAsync(
                    seeded.UserName, Password, seeded.CompanyId, seeded.BranchId,
                    seeded.DeviceId, credential, certificateThumbprint, timeout.Token);
                await restartedDesktop.WaitForPersistedSucceededOperationAsync(timeout.Token);
                await restartedDesktop.CloseNormallyAsync(timeout.Token);
            }

            var encryptedOutbox = Path.Combine(localRoot, "write-outbox.db");
            Assert.True(File.Exists(encryptedOutbox));
            var rawDatabase = await File.ReadAllBytesAsync(encryptedOutbox, timeout.Token);
            Assert.Equal(-1, rawDatabase.AsSpan().IndexOf(
                Encoding.UTF8.GetBytes("Desktop Release UI E2E")));

            await using var verify = CreateDbContext(connection);
            var operation = await verify.SyncOperations.AsNoTracking().SingleAsync(x =>
                x.CompanyId == seeded.CompanyId &&
                x.RegisteredDeviceId == seeded.RegisteredDeviceId,
                timeout.Token);
            Assert.Equal("SUCCEEDED", operation.Status);
            Assert.NotNull(operation.OperationCorrelationId);
            Assert.NotEqual(Guid.Empty, operation.OperationCorrelationId!.Value);
            var businessKey = SyncBusinessIdempotencyKey.Create(
                seeded.CompanyId, seeded.BranchId, seeded.RegisteredDeviceId,
                operation.ClientOperationId);
            Assert.Single(await verify.Set<OperationalPartyEntity>().AsNoTracking().Where(x =>
                x.CompanyId == seeded.CompanyId && x.ClientOperationId == businessKey)
                .ToListAsync(timeout.Token));
            var audits = await verify.AuditEvents.AsNoTracking().Where(x =>
                    x.CompanyId == seeded.CompanyId &&
                    x.OperationCorrelationId == operation.OperationCorrelationId)
                .Select(x => x.Action).ToListAsync(timeout.Token);
            Assert.Contains("SyncOperationQueued", audits);
            Assert.Contains("SyncOperationExecutionSucceeded", audits);
        }
        finally
        {
            if (certificateThumbprint is not null) RemoveCertificate(certificateThumbprint);
            DeleteCngKey(keyName);
            if (localRoot is not null) DeleteDirectory(localRoot);
        }
    }

    [Fact]
    [Trait("Acceptance", "DESKTOP-E2E")]
    [Trait("Category", "PostgreSQL")]
    [Trait("Category", "HTTP")]
    public async Task Authenticated_producer_CNG_DPAPI_SQLCipher_HTTP_worker_PostgreSql_and_restart_succeed()
    {
        Assert.True(OperatingSystem.IsWindows(), "DESKTOP-E2E must execute on Windows; SKIPPED is not PASS.");
        var implementationSha = SyncClientDeploymentAuthority.ImplementationSha;
        Assert.Matches("^[0-9a-f]{40}$", implementationSha ?? string.Empty);
        var measuredBuildIdentity = DesktopBuildIdentityProbe.Measure();
        Assert.True(measuredBuildIdentity.IsValid);
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

            using var factory = CreateFactory(
                connection, implementationSha!, measuredBuildIdentity, seeded);
            var request = new DesktopOnlineSignInRequest(
                seeded.UserName, Password, seeded.CompanyId, seeded.BranchId,
                seeded.DeviceId, credential, certificateThumbprint);

            Guid localOperationId;
            string clientOperationId;
            Guid correlationId;
            SyncClientEffectivePolicy effectivePolicy;
            using (var authenticator = new DesktopOnlineSessionAuthenticator(factory.Server.CreateHandler))
            {
                var authentication = await authenticator.AuthenticateAsync(request, CancellationToken.None);
                Assert.True(authentication.Succeeded,
                    $"Desktop online authentication failed closed with {authentication.Code}.");
                Assert.NotNull(authentication.Activation);
                effectivePolicy = authentication.Activation!.EffectivePolicy;
                Assert.True(authentication.Activation.MeasuredBuildIdentity.FixedTimeEquals(measuredBuildIdentity));
                var allowedAction = Assert.Single(authentication.Activation.AllowedActions);
                Assert.Equal("CreateOperationalParty", allowedAction.Action);
                Assert.Equal("CREATE", allowedAction.Operation);
                Assert.Equal("OperationalParty", allowedAction.Entity);
                using var runtime = await authentication.Activation!.CreateRuntimeAsync(CancellationToken.None);
                Assert.Equal(DesktopOfflineRuntimeMode.Ready, runtime.Status.Mode);
                Assert.True(runtime.CanQueueOperationalParties);
                Assert.Equal(effectivePolicy, runtime.EffectivePolicy);
                Assert.Equal(1, effectivePolicy.MaxBatchOperations);
                Assert.Equal(2, effectivePolicy.ClientTransportMaxRetryCount);
                Assert.Equal(9, effectivePolicy.ClientTransportBaseSeconds);
                Assert.Equal(30, effectivePolicy.ClientTransportMaxDelayMinutes);
                Assert.Equal(12, effectivePolicy.LocalSuccessHours);
                Assert.Equal(3, effectivePolicy.LocalRejectedDays);
                Assert.Equal(30, effectivePolicy.ServerPayloadDays);
                Assert.Equal(1, effectivePolicy.CacheMaxAgeHours);
                Assert.Equal(4096, effectivePolicy.MaximumRequestBodyBytes);
                Assert.Equal(1024, effectivePolicy.MaximumPayloadBytes);
                Assert.Equal("desktop-e2e-v1", effectivePolicy.SourceVersion);
                Assert.Matches("^[0-9a-f]{64}$", effectivePolicy.SourceFingerprint);
                Assert.Equal(implementationSha, effectivePolicy.ActivationImplementationSha);

                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => runtime.SynchronizeAsync(2));
                var cacheDenied = await Assert.ThrowsAsync<OfflineStoreException>(() =>
                    runtime.PutReadCacheAsync(
                        "policy", "too-long", "{}", TimeSpan.FromHours(1).Add(TimeSpan.FromSeconds(1))));
                Assert.Equal("READ_CACHE_POLICY_DENIED", cacheDenied.Code);
                var actionDenied = await Assert.ThrowsAsync<OfflineStoreException>(() => runtime.QueueAsync(
                    new OfflineOperationEnqueueTemplate(
                        Guid.NewGuid(), seeded.CompanyId, seeded.BranchId, seeded.UserId,
                        seeded.RegisteredDeviceId, "CreateWaybillDraft", "CREATE",
                        "Waybill", null, null, DateTimeOffset.UtcNow),
                    identity => JsonSerializer.Serialize(new { clientOperationId = identity.ClientOperationId })));
                Assert.Equal("OFFLINE_ACTION_NOT_AUTHORIZED", actionDenied.Code);
                var oversized = await runtime.QueueAsync(
                    new OfflineOperationEnqueueTemplate(
                        Guid.NewGuid(), seeded.CompanyId, seeded.BranchId, seeded.UserId,
                        seeded.RegisteredDeviceId, "CreateOperationalParty", "CREATE",
                        "OperationalParty", null, null, DateTimeOffset.UtcNow),
                    identity => JsonSerializer.Serialize(new
                    {
                        clientOperationId = identity.ClientOperationId,
                        oversized = new string('x', 1100)
                    }));
                var localRejection = await runtime.SynchronizeAsync();
                Assert.Equal(1, localRejection.Rejected);
                var rejected = await runtime.GetOperationAsync(oversized.Operation.LocalOperationId);
                Assert.Equal(OfflineOperationStatus.Rejected, rejected?.Status);
                Assert.Equal("PAYLOAD_TOO_LARGE", rejected?.ResultCode);

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

            await AssertPostgreSqlEvidenceAsync(
                connection, seeded, clientOperationId, correlationId, effectivePolicy);
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
        BuildIdentityV1 measuredBuildIdentity,
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
            builder.UseSetting("Sync:Offline:AuthorizedBuilds:0:Platform", measuredBuildIdentity.Platform);
            builder.UseSetting("Sync:Offline:AuthorizedBuilds:0:ArtifactSha256", measuredBuildIdentity.ArtifactSha256);
            if (measuredBuildIdentity.SignerCertificateSha256 is { } signer)
                builder.UseSetting("Sync:Offline:AuthorizedBuilds:0:SignerCertificateSha256", signer);
            builder.UseSetting("Sync:ServerExecution:Enabled", "true");
            builder.UseSetting("Sync:EffectivePolicy:SourceVersion", "desktop-e2e-v1");
            var companyPolicy = $"Sync:EffectivePolicy:Companies:{seeded.CompanyId:D}";
            builder.UseSetting($"{companyPolicy}:MaxBatchOperations", "20");
            builder.UseSetting($"{companyPolicy}:MaximumRequestBodyBytes", "1048576");
            builder.UseSetting($"{companyPolicy}:MaximumPayloadBytes", "8192");
            builder.UseSetting($"{companyPolicy}:ClientTransportMaxRetryCount", "4");
            builder.UseSetting($"{companyPolicy}:ClientTransportBaseSeconds", "6");
            builder.UseSetting($"{companyPolicy}:LocalSuccessHours", "20");
            builder.UseSetting($"{companyPolicy}:LocalRejectedDays", "6");
            builder.UseSetting($"{companyPolicy}:ServerPayloadDays", "80");
            builder.UseSetting($"{companyPolicy}:CacheMaxAgeHours", "12");
            var branchPolicy = $"Sync:EffectivePolicy:Branches:{seeded.CompanyId:D}:{seeded.BranchId:D}";
            builder.UseSetting($"{branchPolicy}:MaxBatchOperations", "5");
            builder.UseSetting($"{branchPolicy}:MaximumRequestBodyBytes", "65536");
            builder.UseSetting($"{branchPolicy}:MaximumPayloadBytes", "4096");
            builder.UseSetting($"{branchPolicy}:ClientTransportMaxRetryCount", "3");
            builder.UseSetting($"{branchPolicy}:ClientTransportBaseSeconds", "8");
            builder.UseSetting($"{branchPolicy}:LocalSuccessHours", "18");
            builder.UseSetting($"{branchPolicy}:LocalRejectedDays", "5");
            builder.UseSetting($"{branchPolicy}:ServerPayloadDays", "60");
            builder.UseSetting($"{branchPolicy}:CacheMaxAgeHours", "4");
            var devicePolicy = $"Sync:EffectivePolicy:Devices:{seeded.RegisteredDeviceId:D}";
            builder.UseSetting($"{devicePolicy}:CompanyId", seeded.CompanyId.ToString("D"));
            builder.UseSetting($"{devicePolicy}:BranchId", seeded.BranchId.ToString("D"));
            builder.UseSetting($"{devicePolicy}:DeviceId", seeded.DeviceId);
            builder.UseSetting($"{devicePolicy}:AllowedActions:0", "CreateOperationalParty");
            builder.UseSetting($"{devicePolicy}:MaxBatchOperations", "1");
            builder.UseSetting($"{devicePolicy}:MaximumRequestBodyBytes", "4096");
            builder.UseSetting($"{devicePolicy}:MaximumPayloadBytes", "1024");
            builder.UseSetting($"{devicePolicy}:ClientTransportMaxRetryCount", "2");
            builder.UseSetting($"{devicePolicy}:ClientTransportBaseSeconds", "9");
            builder.UseSetting($"{devicePolicy}:LocalSuccessHours", "12");
            builder.UseSetting($"{devicePolicy}:LocalRejectedDays", "3");
            builder.UseSetting($"{devicePolicy}:ServerPayloadDays", "30");
            builder.UseSetting($"{devicePolicy}:CacheMaxAgeHours", "1");
        });

    private static IReadOnlyDictionary<string, string> ReleaseHostConfiguration(
        string connection,
        Uri origin,
        string implementationSha,
        BuildIdentityV1 measuredBuildIdentity,
        SeededScope seeded)
    {
        var values = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["ConnectionStrings__TransportErp"] = connection,
            ["AllowedHosts"] = origin.IdnHost,
            ["Auth__Mode"] = "LocalSessions",
            ["Auth__Issuer"] = Issuer,
            ["Auth__Audience"] = Audience,
            ["Auth__SigningKey"] = SigningKey,
            ["Auth__SigningKeyId"] = "desktop-release-e2e-current",
            ["Auth__AccessTokenMinutes"] = "15",
            ["Auth__RefreshTokenDays"] = "30",
            ["Auth__MaxFailures"] = "5",
            ["Auth__LockoutMinutes"] = "15",
            ["Auth__LoginRateLimitPermitCount"] = "10",
            ["Auth__RefreshRateLimitPermitCount"] = "20",
            ["Auth__RateLimitWindowSeconds"] = "60",
            ["Auth__RateLimiterMode"] = "SingleNode",
            ["Auth__ApplicationInstanceCount"] = "1",
            ["Sync__Offline__Enabled"] = "true",
            ["Sync__Offline__ActivationDecisionId"] = "DEC-G5-DESKTOP-RELEASE-E2E",
            ["Sync__Offline__ActivationImplementationSha"] = implementationSha,
            ["Sync__Offline__AuthorizedBuilds__0__Platform"] = measuredBuildIdentity.Platform,
            ["Sync__Offline__AuthorizedBuilds__0__ArtifactSha256"] = measuredBuildIdentity.ArtifactSha256,
            ["Sync__Offline__AllowedActions__0"] = "CreateOperationalParty",
            ["Sync__ServerExecution__Enabled"] = "true",
            ["Sync__Protocol__AllowedVersions__0"] = "sync-v1",
            ["Sync__Retry__ClientTransport__MaxCount"] = "5",
            ["Sync__Retry__ClientTransport__BaseSeconds"] = "5",
            ["Sync__Retry__ClientTransport__MaxDelayMinutes"] = "30",
            ["Sync__Retry__ServerExecution__MaxCount"] = "5",
            ["Sync__Retry__ServerExecution__BaseSeconds"] = "5",
            ["Sync__Retry__ServerExecution__MaxDelayMinutes"] = "30",
            ["Sync__Batch__MaxOperations"] = "100",
            ["Sync__Conflict__AutoMerge"] = "false",
            ["Sync__Retention__LocalSuccessHours"] = "24",
            ["Sync__Retention__LocalRejectedDays"] = "7",
            ["Sync__Retention__ServerPayloadDays"] = "90",
            ["Sync__Cache__MaxAgeHours"] = "24",
            ["Sync__Proof__PublicOrigin"] = origin.ToString(),
            ["Sync__Proof__MaximumPastSeconds"] = "120",
            ["Sync__Proof__MaximumFutureSeconds"] = "30",
            ["Sync__Proof__NonceLifetimeSeconds"] = "300",
            ["Sync__Proof__ReplayRetentionSeconds"] = "600",
            ["Sync__Proof__MaximumRequestBodyBytes"] = "2097152",
            ["Sync__Proof__MaximumPayloadBytes"] = "16384",
            ["Sync__Proof__ForwardedHeadersEnabled"] = "false",
            ["Sync__EffectivePolicy__SourceVersion"] = "desktop-release-e2e-v1"
        };
        if (measuredBuildIdentity.SignerCertificateSha256 is { } signer)
            values["Sync__Offline__AuthorizedBuilds__0__SignerCertificateSha256"] = signer;

        var device = $"Sync__EffectivePolicy__Devices__{seeded.RegisteredDeviceId:D}__";
        values[device + "CompanyId"] = seeded.CompanyId.ToString("D");
        values[device + "BranchId"] = seeded.BranchId.ToString("D");
        values[device + "DeviceId"] = seeded.DeviceId;
        values[device + "AllowedActions__0"] = "CreateOperationalParty";
        return values;
    }

    private static async Task WaitForReleaseOperationAsync(
        string connection,
        SeededScope seeded,
        CancellationToken cancellationToken)
    {
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await using var db = CreateDbContext(connection);
            var statuses = await db.SyncOperations.AsNoTracking().Where(x =>
                    x.CompanyId == seeded.CompanyId &&
                    x.RegisteredDeviceId == seeded.RegisteredDeviceId)
                .Select(x => x.Status).ToListAsync(cancellationToken);
            if (statuses.Count == 1 && statuses[0] == "SUCCEEDED") return;
            Assert.DoesNotContain("FAILED", statuses);
            Assert.DoesNotContain("REJECTED", statuses);
            await Task.Delay(200, cancellationToken);
        }
    }

    private static string ReleaseDeploymentDirectory()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (directory is not null)
        {
            var project = Path.Combine(directory.FullName,
                "TransportERP.Desktop", "TransportERP.Desktop.csproj");
            if (File.Exists(project))
                return Path.Combine(directory.FullName,
                    "TransportERP.Desktop", "bin", "Release", "net10.0-windows");
            directory = directory.Parent;
        }
        throw new InvalidOperationException("DESKTOP_E2E_REPOSITORY_ROOT_UNAVAILABLE");
    }

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
        Guid correlationId,
        SyncClientEffectivePolicy effectivePolicy)
    {
        await using var db = CreateDbContext(connection);
        var operation = await db.SyncOperations.AsNoTracking().SingleAsync(x =>
            x.CompanyId == seeded.CompanyId && x.RegisteredDeviceId == seeded.RegisteredDeviceId &&
            x.ClientOperationId == clientOperationId);
        Assert.Equal("SUCCEEDED", operation.Status);
        Assert.Equal(correlationId, operation.OperationCorrelationId);
        var queuedAudit = await db.AuditEvents.AsNoTracking().SingleAsync(x =>
            x.Action == "SyncOperationQueued" && x.OperationCorrelationId == correlationId);
        Assert.Equal(
            $"PolicySourceVersion={effectivePolicy.SourceVersion};" +
            $"PolicySourceFingerprint={effectivePolicy.SourceFingerprint}",
            queuedAudit.Reason);
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
