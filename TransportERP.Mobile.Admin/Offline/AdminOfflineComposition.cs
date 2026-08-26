using TransportERP.Mobile.Internal;
using TransportERP.Offline;
using TransportERP.Offline.Transport;

namespace TransportERP.Mobile.Admin.Offline;

public enum AdminOfflineRuntimeMode { Closed, SecurityUnavailable, ReadCacheOnly, Ready }

public sealed record AdminOfflineRuntimeStatus(
    AdminOfflineRuntimeMode Mode,
    string ReasonCode,
    bool ReadCacheAvailable,
    bool WriteQueueAvailable,
    bool SyncTransportAvailable);

/// <summary>Payload-free metadata suitable for a future native status screen.</summary>
public sealed record AdminOfflineOperationStatusView(
    Guid LocalOperationId,
    string ActionCode,
    OfflineOperationStatus Status,
    int RetryCount,
    DateTimeOffset? NextRetryAt,
    string? ResultCode,
    DateTimeOffset UpdatedAt)
{
    internal static AdminOfflineOperationStatusView From(OfflineOperation operation) => new(
        operation.LocalOperationId,
        operation.ActionCode,
        operation.Status,
        operation.ClientTransportRetryCount,
        operation.NextRetryAt,
        operation.ResultCode,
        operation.UpdatedAt);
}

public sealed record AdminOfflineCompositionOptions(
    Guid CompanyId,
    Guid BranchId,
    Guid UserId,
    Guid RegisteredDeviceId,
    string OutboxDatabasePath,
    string ReadCacheDatabasePath,
    OfflineSyncTransportOptions TransportOptions,
    OfflineRetryPolicy? RetryPolicy = null,
    bool OfflineRuntimeAuthorized = false);

public interface IAdminNativeEncryptionKeyProvider : ILocalEncryptionKeyProvider
{
    ValueTask<bool> IsNativeSecureStorageAvailableAsync(CancellationToken cancellationToken = default);
}

public interface IAdminNativeDeviceSigningKey : IDeviceProofSigningKey
{
    ValueTask<bool> IsNativeSigningKeyAvailableAsync(CancellationToken cancellationToken = default);
}

public interface IAdminSyncNetworkProvider
{
    bool IsPlatformTransportAvailable { get; }

    /// <summary>The handler must not persist or trace Authorization, DPoP, nonce, or request bodies.</summary>
    HttpClient SyncHttpClient { get; }
    ValueTask<bool> IsNetworkAvailableAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// An explicit application contract is mandatory for admin offline writes. Without one, the
/// composition exposes only the encrypted approved read cache.
/// </summary>
public interface IAdminOfflineWriteContract
{
    string ContractId { get; }
    bool Allows(string actionCode, string operationType, string entityType);
}

public sealed record AdminOfflineDependencies(
    IAdminNativeEncryptionKeyProvider EncryptionKeys,
    IAdminNativeDeviceSigningKey DeviceSigningKey,
    IInMemoryBearerTokenProvider VolatileSession,
    IAdminSyncNetworkProvider Network,
    IAdminOfflineWriteContract? WriteContract = null);

/// <summary>No plaintext or software-key fallback; native hosts replace this adapter.</summary>
public sealed class AdminUnavailableNativeSecurity : IAdminNativeEncryptionKeyProvider, IAdminNativeDeviceSigningKey
{
    public ValueTask<bool> IsNativeSecureStorageAvailableAsync(CancellationToken cancellationToken = default) =>
        ValueTask.FromResult(false);

    public ValueTask<bool> IsNativeSigningKeyAvailableAsync(CancellationToken cancellationToken = default) =>
        ValueTask.FromResult(false);

    public ValueTask<byte[]> GetKeyAsync(LocalStorePurpose purpose, CancellationToken cancellationToken = default) =>
        ValueTask.FromException<byte[]>(new AdminOfflineUnavailableException("NATIVE_SECURE_STORAGE_UNAVAILABLE"));

    public ValueTask<DevicePublicP256Jwk> GetPublicJwkAsync(CancellationToken cancellationToken = default) =>
        ValueTask.FromException<DevicePublicP256Jwk>(new AdminOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE"));

    public ValueTask<byte[]> SignEs256Async(ReadOnlyMemory<byte> signingInput, CancellationToken cancellationToken = default) =>
        ValueTask.FromException<byte[]>(new AdminOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE"));
}

public sealed class AdminOfflineUnavailableException(string code) : InvalidOperationException(code)
{
    public string Code { get; } = code;
}

public sealed class AdminOfflineRuntime
{
    private readonly AdminOfflineCompositionOptions _options;
    private readonly AdminOfflineDependencies _dependencies;
    private readonly OfflineReadCacheStore? _readCache;
    private readonly OfflineOperationStore? _outbox;
    private readonly OfflineSyncTransportClient? _transport;
    private readonly OfflineOperationScope _scope;
    private readonly OfflineSyncSupervisor? _supervisor;

    internal AdminOfflineRuntime(
        AdminOfflineCompositionOptions options,
        AdminOfflineDependencies dependencies,
        MobileOfflineKernelResult result)
    {
        _options = options;
        _dependencies = dependencies;
        _readCache = result.ReadCache;
        _outbox = result.Outbox;
        _transport = result.Transport;
        _scope = new OfflineOperationScope(
            options.CompanyId, options.BranchId, options.UserId, options.RegisteredDeviceId);
        _supervisor = result is { Outbox: not null, Transport: not null }
            ? new OfflineSyncSupervisor(
                result.Outbox, result.Transport, new AdminSyncConnectivity(dependencies.Network))
            : null;
        Status = new(
            Map(result.Mode), result.ReasonCode,
            result.ReadCache is not null, result.Outbox is not null, result.Transport is not null);
    }

    public AdminOfflineRuntimeStatus Status { get; }

    [Obsolete("Use the identity-aware template overload so the business payload is bound to ClientOperationId atomically.")]
    public Task<OfflineEnqueueResult> QueueAsync(
        OfflineOperationEnqueueRequest request,
        CancellationToken cancellationToken = default) =>
        Task.FromException<OfflineEnqueueResult>(
            new AdminOfflineUnavailableException("OFFLINE_IDENTITY_FACTORY_REQUIRED"));

    public async Task<OfflineEnqueueResult> QueueAsync(
        OfflineOperationEnqueueTemplate request,
        Func<OfflineGeneratedOperationIdentity, string> payloadFactory,
        CancellationToken cancellationToken = default)
    {
        var outbox = _outbox ?? throw Unavailable();
        if (!MatchesScope(request)) throw new AdminOfflineUnavailableException("LOCAL_SCOPE_DENIED");

        var contract = _dependencies.WriteContract;
        if (contract is null || string.IsNullOrWhiteSpace(contract.ContractId) ||
            !contract.Allows(request.ActionCode, request.OperationType, request.EntityType))
        {
            throw new AdminOfflineUnavailableException("OFFLINE_WRITE_CONTRACT_REQUIRED");
        }

        var result = await outbox.EnqueueAsync(request, payloadFactory, cancellationToken);
        if (result.Created)
            _supervisor?.NotifyWorkAvailable();
        return result;
    }

    public Task RunSyncSupervisorAsync(CancellationToken cancellationToken = default) =>
        (_supervisor ?? throw Unavailable()).RunAsync(cancellationToken);

    public async Task<OfflineSyncTransportRunResult> SynchronizeAsync(
        int? maximumOperations = null,
        CancellationToken cancellationToken = default)
    {
        var transport = _transport ?? throw Unavailable();
        if (!await _dependencies.Network.IsNetworkAvailableAsync(cancellationToken))
            throw new AdminOfflineUnavailableException("NETWORK_UNAVAILABLE");
        return await transport.ProcessNextBatchAsync(maximumOperations, cancellationToken);
    }

    public Task PutReadCacheAsync(string cacheKind, string cacheKey, string payloadJson, TimeSpan lifetime,
        CancellationToken cancellationToken = default) =>
        (_readCache ?? throw Unavailable()).PutAsync(cacheKind, cacheKey, payloadJson, lifetime, cancellationToken);

    public Task<string?> GetReadCacheAsync(string cacheKind, string cacheKey,
        CancellationToken cancellationToken = default) =>
        (_readCache ?? throw Unavailable()).GetAsync(cacheKind, cacheKey, cancellationToken);

    public async Task<AdminOfflineOperationStatusView?> GetOperationStatusAsync(
        Guid localOperationId, CancellationToken cancellationToken = default)
    {
        var operation = await (_outbox ?? throw Unavailable()).GetAsync(localOperationId, _scope, cancellationToken);
        return operation is null ? null : AdminOfflineOperationStatusView.From(operation);
    }

    public Task<int> RedactExpiredPayloadsAsync(CancellationToken cancellationToken = default) =>
        (_outbox ?? throw Unavailable()).RedactExpiredPayloadsAsync(cancellationToken: cancellationToken);

    private bool MatchesScope(OfflineOperationEnqueueTemplate request) =>
        request.CompanyId == _options.CompanyId && request.BranchId == _options.BranchId &&
        request.UserId == _options.UserId && request.RegisteredDeviceId == _options.RegisteredDeviceId;

    private AdminOfflineUnavailableException Unavailable() => new(Status.ReasonCode);

    private sealed class AdminSyncConnectivity(IAdminSyncNetworkProvider network) : IOfflineSyncConnectivity
    {
        public async ValueTask<bool> IsOnlineAsync(CancellationToken cancellationToken = default) =>
            network.IsPlatformTransportAvailable && await network.IsNetworkAvailableAsync(cancellationToken);

        public async Task WaitUntilOnlineAsync(CancellationToken cancellationToken = default)
        {
            while (!await IsOnlineAsync(cancellationToken))
                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        }
    }

    private static AdminOfflineRuntimeMode Map(MobileOfflineKernelMode mode) => mode switch
    {
        MobileOfflineKernelMode.Closed => AdminOfflineRuntimeMode.Closed,
        MobileOfflineKernelMode.SecurityUnavailable => AdminOfflineRuntimeMode.SecurityUnavailable,
        MobileOfflineKernelMode.ReadCacheOnly => AdminOfflineRuntimeMode.ReadCacheOnly,
        MobileOfflineKernelMode.Ready => AdminOfflineRuntimeMode.Ready,
        _ => AdminOfflineRuntimeMode.Closed
    };
}

public static class AdminOfflineComposition
{
    public static async Task<AdminOfflineRuntime> CreateAsync(
        AdminOfflineCompositionOptions options,
        AdminOfflineDependencies dependencies,
        TimeProvider? timeProvider = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(dependencies);
        ValidateScope(options);
        var writeContractAvailable = dependencies.WriteContract is not null &&
                                     !string.IsNullOrWhiteSpace(dependencies.WriteContract.ContractId);
        var result = await MobileOfflineCompositionKernel.ComposeAsync(
            new(options.OfflineRuntimeAuthorized, writeContractAvailable, options.OutboxDatabasePath,
                options.ReadCacheDatabasePath, options.TransportOptions, options.RetryPolicy),
            dependencies.EncryptionKeys,
            dependencies.DeviceSigningKey,
            dependencies.VolatileSession,
            () => dependencies.Network.SyncHttpClient,
            dependencies.EncryptionKeys.IsNativeSecureStorageAvailableAsync,
            dependencies.DeviceSigningKey.IsNativeSigningKeyAvailableAsync,
            () => dependencies.Network.IsPlatformTransportAvailable,
            timeProvider,
            cancellationToken);
        return new AdminOfflineRuntime(options, dependencies, result);
    }

    private static void ValidateScope(AdminOfflineCompositionOptions options)
    {
        if (options.CompanyId == Guid.Empty || options.BranchId == Guid.Empty || options.UserId == Guid.Empty ||
            options.RegisteredDeviceId == Guid.Empty || options.TransportOptions.RegisteredDeviceId != options.RegisteredDeviceId ||
            options.TransportOptions.CompanyId != options.CompanyId || options.TransportOptions.BranchId != options.BranchId ||
            options.TransportOptions.UserId != options.UserId)
            throw new ArgumentException("The configured mobile scope and transport device must be identical.", nameof(options));
    }
}
