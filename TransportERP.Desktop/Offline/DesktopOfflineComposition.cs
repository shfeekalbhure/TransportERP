using System.Security.Cryptography;
using System.Text;
using TransportERP.Application.Sync;
using TransportERP.Offline;
using TransportERP.Offline.Transport;

namespace TransportERP.Desktop.Offline;

public enum DesktopOfflineRuntimeMode { Closed, SecurityUnavailable, Ready }

public sealed record DesktopOfflineRuntimeStatus(
    DesktopOfflineRuntimeMode Mode,
    string ReasonCode,
    bool WriteQueueAvailable,
    bool ReadCacheAvailable,
    bool SyncTransportAvailable);

public sealed record DesktopOfflineCompositionOptions(
    Guid CompanyId,
    Guid BranchId,
    Guid UserId,
    Guid RegisteredDeviceId,
    string OutboxDatabasePath,
    string ReadCacheDatabasePath,
    string ProtectedKeyDirectory,
    string DeviceSigningCertificateThumbprint,
    DesktopDeviceProofBinding ProofBinding,
    OfflineSyncTransportOptions TransportOptions,
    SyncClientEffectivePolicy EffectivePolicy,
    OfflineRetryPolicy? RetryPolicy = null,
    bool OfflineRuntimeAuthorized = false);

public sealed record DesktopDeviceProofBinding(
    int Version,
    string JwkThumbprint,
    string X,
    string Y);

public interface IDesktopOfflineWritePolicy
{
    bool Allows(string actionCode, string operationType, string entityType);
}

public interface IDesktopSyncNetworkProvider
{
    bool IsTransportAvailable { get; }
    bool IsNetworkAvailable { get; }
    HttpClient SyncHttpClient { get; }
}

public sealed record DesktopOfflineDependencies(
    IInMemoryBearerTokenProvider VolatileSession,
    IDesktopSyncNetworkProvider Network,
    IDesktopOfflineWritePolicy WritePolicy,
    ISyncOperationsPermissionPolicy UiPermissions);

/// <summary>
/// Desktop composition remains inert until the host passes exact server-issued session, policy and
/// device-key authority.
/// It creates no plaintext fallback and owns the opaque device signing handle for its lifetime.
/// </summary>
public sealed class DesktopOfflineRuntime : IDisposable
{
    private readonly DesktopOfflineCompositionOptions _options;
    private readonly DesktopOfflineDependencies _dependencies;
    private readonly OfflineOperationStore? _outbox;
    private readonly OfflineReadCacheStore? _readCache;
    private readonly OfflineSyncTransportClient? _transport;
    private readonly OfflineSyncConflictClient? _conflictClient;
    private readonly OfflineSyncSupervisor? _supervisor;
    private readonly IDeviceProofSigningKey? _signingKey;

    internal DesktopOfflineRuntime(
        DesktopOfflineCompositionOptions options,
        DesktopOfflineDependencies dependencies,
        DesktopOfflineRuntimeStatus status,
        OfflineOperationStore? outbox = null,
        OfflineReadCacheStore? readCache = null,
        OfflineSyncTransportClient? transport = null,
        OfflineSyncConflictClient? conflictClient = null,
        OfflineSyncSupervisor? supervisor = null,
        IDeviceProofSigningKey? signingKey = null)
    {
        _options = options;
        _dependencies = dependencies;
        Status = status;
        _outbox = outbox;
        _readCache = readCache;
        _transport = transport;
        _conflictClient = conflictClient;
        _supervisor = supervisor;
        _signingKey = signingKey;
    }

    public DesktopOfflineRuntimeStatus Status { get; }
    public SyncClientEffectivePolicy EffectivePolicy => _options.EffectivePolicy;
    public bool CanQueueOperationalParties => _outbox is not null &&
        _dependencies.WritePolicy.Allows("CreateOperationalParty", "CREATE", "OperationalParty");

    public DesktopOfflineBusinessProducer CreateBusinessProducer()
    {
        if (_outbox is null) throw Unavailable();
        if (!CanQueueOperationalParties)
            throw new OfflineStoreException(
                "OFFLINE_ACTION_NOT_AUTHORIZED", "The action is not authorized for offline execution.");
        return new DesktopOfflineBusinessProducer(this, Scope(_options));
    }

    [Obsolete("Use the identity-aware template overload so the business payload is bound to ClientOperationId atomically.")]
    public Task<OfflineEnqueueResult> QueueAsync(
        OfflineOperationEnqueueRequest request,
        CancellationToken cancellationToken = default) =>
        Task.FromException<OfflineEnqueueResult>(new OfflineStoreException(
            "OFFLINE_IDENTITY_FACTORY_REQUIRED",
            "The desktop host must build the payload from the durable operation identity."));

    public async Task<OfflineEnqueueResult> QueueAsync(
        OfflineOperationEnqueueTemplate template,
        Func<OfflineGeneratedOperationIdentity, string> payloadFactory,
        CancellationToken cancellationToken = default)
    {
        var outbox = _outbox ?? throw Unavailable();
        if (template.CompanyId != _options.CompanyId || template.BranchId != _options.BranchId ||
            template.UserId != _options.UserId || template.RegisteredDeviceId != _options.RegisteredDeviceId)
            throw new OfflineStoreException("LOCAL_SCOPE_DENIED", "The operation does not match the authenticated desktop scope.");
        if (!_dependencies.WritePolicy.Allows(template.ActionCode, template.OperationType, template.EntityType))
            throw new OfflineStoreException("OFFLINE_ACTION_NOT_AUTHORIZED", "The action is not authorized for offline execution.");
        var result = await outbox.EnqueueAsync(template, payloadFactory, cancellationToken);
        if (result.Created)
            _supervisor?.NotifyWorkAvailable();
        return result;
    }

    public Task RunSyncSupervisorAsync(CancellationToken cancellationToken = default) =>
        (_supervisor ?? throw Unavailable()).RunAsync(cancellationToken);

    public Task<OfflineSyncTransportRunResult> SynchronizeAsync(
        int? maximumOperations = null,
        CancellationToken cancellationToken = default)
    {
        var transport = _transport ?? throw Unavailable();
        if (!_dependencies.Network.IsNetworkAvailable)
            throw new OfflineStoreException("NETWORK_UNAVAILABLE", "The sync network is unavailable.");
        return transport.ProcessNextBatchAsync(maximumOperations, cancellationToken);
    }

    public Task PutReadCacheAsync(string kind, string key, string payloadJson, TimeSpan lifetime,
        CancellationToken cancellationToken = default)
    {
        if (lifetime <= TimeSpan.Zero || lifetime > _options.EffectivePolicy.CacheMaxAge)
            throw new OfflineStoreException(
                "READ_CACHE_POLICY_DENIED", "The requested cache lifetime exceeds the effective server policy.");
        return (_readCache ?? throw Unavailable()).PutAsync(kind, key, payloadJson, lifetime, cancellationToken);
    }

    public Task<string?> GetReadCacheAsync(string kind, string key,
        CancellationToken cancellationToken = default) =>
        (_readCache ?? throw Unavailable()).GetAsync(kind, key, cancellationToken);

    public Task<OfflineOperation?> GetOperationAsync(
        Guid localOperationId,
        CancellationToken cancellationToken = default) =>
        (_outbox ?? throw Unavailable()).GetAsync(localOperationId, Scope(_options), cancellationToken);

    public SyncOperationsForm CreateOperationsForm()
    {
        var outbox = _outbox ?? throw Unavailable();
        var scope = Scope(_options);
        return new SyncOperationsForm(new SyncOperationsController(
            new StoreOperationsQuery(outbox, scope),
            new StoreManualRetryService(outbox, scope),
            new StoreConflictActionService(
                _conflictClient ?? throw Unavailable(),
                new StoreConflictBaseVersionProvider(outbox, scope)),
            _dependencies.UiPermissions));
    }

    public Task<int> RedactExpiredPayloadsAsync(CancellationToken cancellationToken = default) =>
        (_outbox ?? throw Unavailable()).RedactExpiredPayloadsAsync(
            new OfflineRetentionPolicy(
                _options.EffectivePolicy.LocalSuccessRetention,
                _options.EffectivePolicy.LocalRejectedRetention),
            cancellationToken);

    public void Dispose() => _signingKey?.Dispose();

    private OfflineStoreException Unavailable() =>
        new(Status.ReasonCode, "The desktop offline runtime is unavailable.");

    private static OfflineOperationScope Scope(DesktopOfflineCompositionOptions options) =>
        new(options.CompanyId, options.BranchId, options.UserId, options.RegisteredDeviceId);

    private sealed class StoreOperationsQuery(
        OfflineOperationStore store,
        OfflineOperationScope scope) : ISyncOperationsQuery
    {
        public Task<IReadOnlyList<OfflineOperation>> ListAsync(CancellationToken cancellationToken = default) =>
            store.ListAsync(scope, cancellationToken);
    }

    private sealed class StoreManualRetryService(
        OfflineOperationStore store,
        OfflineOperationScope scope) : ISyncManualRetryService
    {
        public Task RetryAsync(Guid localOperationId, CancellationToken cancellationToken = default) =>
            store.RequeueFailedAsync(localOperationId, scope, cancellationToken);
    }

    private sealed class StoreConflictActionService(
        OfflineSyncConflictClient conflicts,
        StoreConflictBaseVersionProvider versions) : ISyncConflictActionService
    {
        public async Task ResolveAsync(Guid localOperationId, SyncConflictDecision decision, string reason,
            CancellationToken cancellationToken = default)
        {
            long? baseVersion = null;
            if (decision == SyncConflictDecision.Reapply)
                baseVersion = await versions.GetCurrentServerVersionAsync(localOperationId, cancellationToken);
            await conflicts.ResolveAsync(localOperationId,
                decision == SyncConflictDecision.KeepServer
                    ? OfflineConflictDecision.KeepServer
                    : OfflineConflictDecision.Reapply,
                reason, baseVersion, cancellationToken);
        }
    }

    private sealed class StoreConflictBaseVersionProvider(
        OfflineOperationStore store,
        OfflineOperationScope scope)
    {
        public async Task<long> GetCurrentServerVersionAsync(
            Guid localOperationId,
            CancellationToken cancellationToken = default)
        {
            var operation = (await store.ListAsync(scope, cancellationToken))
                .SingleOrDefault(item => item.LocalOperationId == localOperationId);
            if (operation?.Status != OfflineOperationStatus.Conflict ||
                operation.ConflictReview?.ServerSnapshot?.CurrentVersion is not > 0)
                throw new OfflineStoreException(
                    "CONFLICT_REVIEW_REQUIRED",
                    "A redacted server version must be reviewed before reapply.");
            return operation.ConflictReview.ServerSnapshot.CurrentVersion.Value;
        }
    }
}

public static class DesktopOfflineComposition
{
    public static async Task<DesktopOfflineRuntime> CreateAsync(
        DesktopOfflineCompositionOptions options,
        DesktopOfflineDependencies dependencies,
        TimeProvider? timeProvider = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(dependencies);
        Validate(options);
        if (!options.OfflineRuntimeAuthorized)
            return Closed(options, dependencies, "OFFLINE_CLOSED");
        if (!OperatingSystem.IsWindows() || !dependencies.Network.IsTransportAvailable)
            return Closed(options, dependencies, "DESKTOP_SECURE_RUNTIME_UNAVAILABLE", security: true);

        IDeviceProofSigningKey? signingKey = null;
        try
        {
            // The OS handle is opened and bound to the exact server-authorized JWK before DPAPI,
            // SQLCipher files or any local offline state can be created.
            signingKey = await new WindowsCertificateDeviceProofSigningKeyStore()
                .OpenAsync(options.DeviceSigningCertificateThumbprint, cancellationToken);
            VerifyProofBinding(options.ProofBinding, signingKey.PublicKey);
            var keys = new WindowsDpapiLocalEncryptionKeyProvider(options.ProtectedKeyDirectory);
            var outbox = new OfflineOperationStore(options.OutboxDatabasePath, keys, timeProvider, options.RetryPolicy);
            var scope = new OfflineOperationScope(
                options.CompanyId, options.BranchId, options.UserId, options.RegisteredDeviceId);
            var readCache = new OfflineReadCacheStore(options.ReadCacheDatabasePath, keys, scope, timeProvider);
            await outbox.InitializeAsync(cancellationToken);
            await readCache.InitializeAsync(cancellationToken);
            var transport = new OfflineSyncTransportClient(
                dependencies.Network.SyncHttpClient, outbox, dependencies.VolatileSession,
                signingKey, options.TransportOptions, timeProvider);
            var conflicts = new OfflineSyncConflictClient(
                dependencies.Network.SyncHttpClient, outbox, dependencies.VolatileSession,
                signingKey, options.TransportOptions, timeProvider);
            var supervisor = new OfflineSyncSupervisor(
                outbox, transport, new DesktopSyncConnectivity(dependencies.Network),
                new OfflineSyncSupervisorOptions(
                    options.EffectivePolicy.MaxBatchOperations,
                    RetentionPolicy: new OfflineRetentionPolicy(
                        options.EffectivePolicy.LocalSuccessRetention,
                        options.EffectivePolicy.LocalRejectedRetention)));
            return new DesktopOfflineRuntime(options, dependencies,
                new(DesktopOfflineRuntimeMode.Ready, "READY", true, true, true),
                outbox, readCache, transport, conflicts, supervisor, signingKey);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            signingKey?.Dispose();
            throw;
        }
        catch
        {
            signingKey?.Dispose();
            return Closed(options, dependencies, "DESKTOP_SECURE_RUNTIME_UNAVAILABLE", security: true);
        }
    }

    private sealed class DesktopSyncConnectivity(IDesktopSyncNetworkProvider network) : IOfflineSyncConnectivity
    {
        public ValueTask<bool> IsOnlineAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return ValueTask.FromResult(network.IsTransportAvailable && network.IsNetworkAvailable);
        }

        public async Task WaitUntilOnlineAsync(CancellationToken cancellationToken = default)
        {
            while (!network.IsTransportAvailable || !network.IsNetworkAvailable)
                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        }
    }

    private static DesktopOfflineRuntime Closed(
        DesktopOfflineCompositionOptions options,
        DesktopOfflineDependencies dependencies,
        string reason,
        bool security = false) => new(options, dependencies,
            new(security ? DesktopOfflineRuntimeMode.SecurityUnavailable : DesktopOfflineRuntimeMode.Closed,
                reason, false, false, false));

    private static void Validate(DesktopOfflineCompositionOptions options)
    {
        if (options.CompanyId == Guid.Empty || options.BranchId == Guid.Empty ||
            options.UserId == Guid.Empty || options.RegisteredDeviceId == Guid.Empty ||
            options.TransportOptions.CompanyId != options.CompanyId ||
            options.TransportOptions.BranchId != options.BranchId ||
            options.TransportOptions.UserId != options.UserId ||
            options.TransportOptions.RegisteredDeviceId != options.RegisteredDeviceId ||
            options.EffectivePolicy is null || !options.EffectivePolicy.IsValid ||
            options.TransportOptions.MaximumBatchOperations != options.EffectivePolicy.MaxBatchOperations ||
            options.TransportOptions.MaximumRequestBodyBytes != options.EffectivePolicy.MaximumRequestBodyBytes ||
            options.TransportOptions.MaximumPayloadBytes != options.EffectivePolicy.MaximumPayloadBytes ||
            options.RetryPolicy is null ||
            options.RetryPolicy.MaxRetryCount != options.EffectivePolicy.ClientTransportMaxRetryCount ||
            options.RetryPolicy.EffectiveBaseDelay != options.EffectivePolicy.ClientRetryBaseDelay ||
            options.RetryPolicy.EffectiveMaxDelay != options.EffectivePolicy.ClientRetryMaxDelay ||
            options.ProofBinding is null || options.ProofBinding.Version < 1 ||
            string.IsNullOrEmpty(options.ProofBinding.JwkThumbprint) ||
            options.ProofBinding.JwkThumbprint.Length != 43 ||
            string.IsNullOrEmpty(options.ProofBinding.X) || string.IsNullOrEmpty(options.ProofBinding.Y) ||
            string.IsNullOrWhiteSpace(options.OutboxDatabasePath) ||
            string.IsNullOrWhiteSpace(options.ReadCacheDatabasePath) ||
            string.Equals(Path.GetFullPath(options.OutboxDatabasePath), Path.GetFullPath(options.ReadCacheDatabasePath),
                StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("The desktop offline scope and storage configuration are invalid.", nameof(options));
    }

    private static void VerifyProofBinding(DesktopDeviceProofBinding expected, DeviceProofPublicKey actual)
    {
        if (!string.Equals(actual.Curve, "P-256", StringComparison.Ordinal) ||
            !FixedAscii(actual.X, expected.X) || !FixedAscii(actual.Y, expected.Y))
            throw new DeviceProofKeyStoreException(
                "DEVICE_KEY_BINDING_MISMATCH",
                "The selected Windows signing key is not the server-bound device proof key.");

        var canonicalJwk = Encoding.UTF8.GetBytes(
            $"{{\"crv\":\"P-256\",\"kty\":\"EC\",\"x\":\"{actual.X}\",\"y\":\"{actual.Y}\"}}");
        var hash = SHA256.HashData(canonicalJwk);
        try
        {
            var thumbprint = Convert.ToBase64String(hash).TrimEnd('=').Replace('+', '-').Replace('/', '_');
            if (!FixedAscii(thumbprint, expected.JwkThumbprint))
                throw new DeviceProofKeyStoreException(
                    "DEVICE_KEY_BINDING_MISMATCH",
                    "The selected Windows signing key thumbprint does not match the server binding.");
        }
        finally
        {
            CryptographicOperations.ZeroMemory(canonicalJwk);
            CryptographicOperations.ZeroMemory(hash);
        }
    }

    private static bool FixedAscii(string left, string right)
    {
        if (left.Length != right.Length || left.Any(character => character > 0x7f) ||
            right.Any(character => character > 0x7f))
            return false;
        var leftBytes = Encoding.ASCII.GetBytes(left);
        var rightBytes = Encoding.ASCII.GetBytes(right);
        try { return CryptographicOperations.FixedTimeEquals(leftBytes, rightBytes); }
        finally
        {
            CryptographicOperations.ZeroMemory(leftBytes);
            CryptographicOperations.ZeroMemory(rightBytes);
        }
    }
}
