using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using TransportERP.Application.Sync;
using TransportERP.Offline;
using TransportERP.Offline.Transport;

namespace TransportERP.Desktop.Offline;

/// <summary>
/// Windows-only CI hook for the physical client boundary: CurrentUser CNG non-exportable signing,
/// DPAPI-protected SQLCipher keys, encrypted enqueue, nonce/DPoP transport and process-like runtime
/// recreation. It deliberately uses an in-process protocol peer and does not claim server/Postgres
/// or interactive sign-in coverage.
/// </summary>
internal static class DesktopRuntimePlatformProbe
{
    private static readonly Uri Endpoint = new(
        "https://sync.example.test/api/v1/sync/operations:batch");

    // Exit codes identify only a fixed probe checkpoint. They deliberately exclude exception text,
    // paths, key references, payloads and tokens so CI can locate a platform failure safely.
    internal static async Task<int> RunAsync()
    {
        if (!OperatingSystem.IsWindows()) return 2;
        if (SyncClientDeploymentAuthority.ImplementationSha is not { } sha) return 3;

        var root = Path.Combine(Path.GetTempPath(), "transporterp-desktop-runtime-probe-" + Guid.NewGuid().ToString("N"));
        var keyName = "TransportERP.Desktop.Probe." + Guid.NewGuid().ToString("N");
        string? thumbprint = null;
        var checkpoint = 10;
        try
        {
            Directory.CreateDirectory(root);
            checkpoint = 20;
            thumbprint = CreateProbeCertificate(keyName);
            checkpoint = 21;
            var publicKey = await new WindowsCertificateDeviceProofSigningKeyStore().OpenAsync(thumbprint);
            DesktopDeviceProofBinding binding;
            try
            {
                binding = new DesktopDeviceProofBinding(
                    1, Thumbprint(publicKey.PublicKey), publicKey.PublicKey.X, publicKey.PublicKey.Y);
            }
            finally { publicKey.Dispose(); }

            var scope = new OfflineOperationScope(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            using var peer = new ProbeProtocolPeer();
            var options = Options(root, scope, thumbprint, binding, sha);
            var dependencies = Dependencies(peer.Client);
            Guid localOperationId;
            checkpoint = 30;
            using (var first = await DesktopOfflineComposition.CreateAsync(options, dependencies))
            {
                if (first.Status.Mode != DesktopOfflineRuntimeMode.Ready) return 31;
                checkpoint = 40;
                var queued = await first.CreateBusinessProducer().QueueOperationalPartyAsync(
                    "Windows platform probe", "700000000", "DPAPI CNG SQLCipher probe");
                localOperationId = queued.Operation.LocalOperationId;
                checkpoint = 50;
                var sent = await first.SynchronizeAsync();
                if (sent.Succeeded != 1 || peer.SignedRequests != 1) return 51;
            }

            // A separately composed runtime reopens DPAPI material, the CNG handle and SQLCipher.
            checkpoint = 60;
            using var reopened = await DesktopOfflineComposition.CreateAsync(options, Dependencies(peer.Client));
            var persisted = await reopened.GetOperationAsync(localOperationId);
            return reopened.Status.Mode == DesktopOfflineRuntimeMode.Ready &&
                persisted is { Status: OfflineOperationStatus.Succeeded, PayloadJson: not null }
                ? 0
                : 61;
        }
        catch
        {
            return checkpoint;
        }
        finally
        {
            if (thumbprint is not null) RemoveProbeCertificate(thumbprint);
            DeleteCngKey(keyName);
            try
            {
                if (Directory.Exists(root)) Directory.Delete(root, recursive: true);
            }
            catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
            {
                // Cleanup failure must not turn a successful security probe into a false negative.
                // The directory contains only randomly named encrypted probe material.
            }
        }
    }

    private static DesktopOfflineCompositionOptions Options(
        string root,
        OfflineOperationScope scope,
        string thumbprint,
        DesktopDeviceProofBinding binding,
        string sha)
    {
        var policy = new SyncClientEffectivePolicy(
            25, 2, 5, 20, 12, 4, 45, 8, 2_097_152, 16_384,
            "desktop-platform-probe-v1", new string('a', 64), sha);
        return new DesktopOfflineCompositionOptions(
            scope.CompanyId, scope.BranchId, scope.UserId, scope.RegisteredDeviceId,
            Path.Combine(root, "outbox.db"), Path.Combine(root, "read-cache.db"),
            Path.Combine(root, "keys"), thumbprint, binding,
            new OfflineSyncTransportOptions(
                Endpoint, "desktop-platform-probe", scope.RegisteredDeviceId,
                scope.CompanyId, scope.BranchId, scope.UserId, "desktop-probe-worker",
                MaximumBatchOperations: policy.MaxBatchOperations,
                MaximumRequestBodyBytes: policy.MaximumRequestBodyBytes,
                MaximumPayloadBytes: policy.MaximumPayloadBytes),
            policy,
            new OfflineRetryPolicy(
                policy.ClientTransportMaxRetryCount,
                policy.ClientRetryBaseDelay,
                policy.ClientRetryMaxDelay),
            OfflineRuntimeAuthorized: true);
    }

    private static DesktopOfflineDependencies Dependencies(HttpClient http) => new(
        new ProbeBearer(),
        new ProbeNetwork(http),
        new ProbeWritePolicy(),
        new ProbeOperationPermissions());

    private static string CreateProbeCertificate(string keyName)
    {
        var creation = new CngKeyCreationParameters
        {
            Provider = CngProvider.MicrosoftSoftwareKeyStorageProvider,
            ExportPolicy = CngExportPolicies.None,
            KeyUsage = CngKeyUsages.Signing
        };
        using var key = CngKey.Create(CngAlgorithm.ECDsaP256, keyName, creation);
        using var signer = new ECDsaCng(key);
        var request = new CertificateRequest(
            new X500DistinguishedName("CN=TransportERP Desktop Platform Probe"),
            signer, HashAlgorithmName.SHA256);
        using var certificate = request.CreateSelfSigned(
            DateTimeOffset.UtcNow.AddMinutes(-1), DateTimeOffset.UtcNow.AddHours(1));
        using var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
        store.Open(OpenFlags.ReadWrite);
        store.Add(certificate);
        return certificate.Thumbprint;
    }

    private static void RemoveProbeCertificate(string thumbprint)
    {
        try
        {
            using var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite);
            foreach (var certificate in store.Certificates.Find(
                         X509FindType.FindByThumbprint, thumbprint, validOnly: false))
                store.Remove(certificate);
        }
        catch { }
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

    private static string Thumbprint(DeviceProofPublicKey key)
    {
        var bytes = Encoding.ASCII.GetBytes(
            $"{{\"crv\":\"P-256\",\"kty\":\"EC\",\"x\":\"{key.X}\",\"y\":\"{key.Y}\"}}");
        return Convert.ToBase64String(SHA256.HashData(bytes)).TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }

    private sealed class ProbeBearer : IInMemoryBearerTokenProvider
    {
        public ValueTask<string> GetBearerTokenAsync(CancellationToken cancellationToken = default) =>
            ValueTask.FromResult("desktop-platform-probe-bearer");
    }

    private sealed class ProbeNetwork(HttpClient http) : IDesktopSyncNetworkProvider
    {
        public bool IsTransportAvailable => true;
        public bool IsNetworkAvailable => true;
        public HttpClient SyncHttpClient { get; } = http;
    }

    private sealed class ProbeWritePolicy : IDesktopOfflineWritePolicy
    {
        public bool Allows(string actionCode, string operationType, string entityType) =>
            actionCode == "CreateOperationalParty" && operationType == "CREATE" && entityType == "OperationalParty";
    }

    private sealed class ProbeOperationPermissions : ISyncOperationsPermissionPolicy
    {
        public bool CanRetry(OfflineOperation operation) => false;
        public bool CanResolveConflict(OfflineOperation operation, SyncConflictDecision decision) => false;
    }

    private sealed class ProbeProtocolPeer : IDisposable
    {
        private readonly HttpClient _client;
        private int _signedRequests;

        internal ProbeProtocolPeer() => _client = new HttpClient(new Handler(this));
        internal HttpClient Client => _client;
        internal int SignedRequests => Volatile.Read(ref _signedRequests);

        public void Dispose() => _client.Dispose();

        private sealed class Handler(ProbeProtocolPeer owner) : HttpMessageHandler
        {
            protected override async Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                var attempt = Guid.Parse(request.Headers.GetValues("X-Correlation-Id").Single());
                if (!request.Headers.Contains("DPoP"))
                {
                    var challenge = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    challenge.Headers.TryAddWithoutValidation(
                        "DPoP-Nonce", Base64Url(RandomNumberGenerator.GetBytes(32)));
                    return challenge;
                }

                Interlocked.Increment(ref owner._signedRequests);
                var body = await request.Content!.ReadAsByteArrayAsync(cancellationToken);
                var batch = JsonSerializer.Deserialize<SyncV1BatchRequest>(
                    body, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;
                var operation = batch.Operations.Single();
                var response = new SyncV1BatchResponse(
                    "sync-v1",
                    [new SyncV1OperationResult(
                        operation.ClientOperationId, operation.OperationCorrelationId, Guid.NewGuid(),
                        operation.ActionCode, Guid.NewGuid(), "SUCCEEDED", 1, null, null,
                        DateTimeOffset.UtcNow)],
                    DateTimeOffset.UtcNow,
                    attempt);
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        JsonSerializer.Serialize(response, new JsonSerializerOptions(JsonSerializerDefaults.Web)),
                        Encoding.UTF8,
                        "application/json")
                };
            }
        }

        private static string Base64Url(ReadOnlySpan<byte> value) =>
            Convert.ToBase64String(value).TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }
}
