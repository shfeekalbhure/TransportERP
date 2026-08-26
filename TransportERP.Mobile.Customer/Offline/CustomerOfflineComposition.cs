using TransportERP.Mobile.Internal;
using TransportERP.Offline;
using TransportERP.Offline.Transport;

namespace TransportERP.Mobile.Customer.Offline;

public enum CustomerOfflineRuntimeMode { Closed, SecurityUnavailable, ReadCacheOnly, Ready }

public sealed record CustomerOfflineRuntimeStatus(
    CustomerOfflineRuntimeMode Mode,
    string ReasonCode,
    bool ReadCacheAvailable,
    bool WriteQueueAvailable,
    bool SyncTransportAvailable);

/// <summary>Payload-free metadata suitable for a future native status screen.</summary>
public sealed record CustomerOfflineOperationStatusView(
    Guid LocalOperationId,
    string ActionCode,
    OfflineOperationStatus Status,
    int RetryCount,
    DateTimeOffset? NextRetryAt,
    string? ResultCode,
    DateTimeOffset UpdatedAt)
{
    internal static CustomerOfflineOperationStatusView From(OfflineOperation operation) => new(
        operation.LocalOperationId,
        operation.ActionCode,
        operation.Status,
        operation.ClientTransportRetryCount,
        operation.NextRetryAt,
        operation.ResultCode,
        operation.UpdatedAt);
}

public sealed record CustomerOfflineCompositionOptions(
    Guid CompanyId,
    Guid BranchId,
    Guid UserId,
    Guid RegisteredDeviceId,
    string OutboxDatabasePath,
    string ReadCacheDatabasePath,
    OfflineSyncTransportOptions TransportOptions,
    OfflineRetryPolicy? RetryPolicy = null,
    bool OfflineRuntimeAuthorized = false);

public interface ICustomerNativeEncryptionKeyProvider : ILocalEncryptionKeyProvider
{
    ValueTask<bool> IsNativeSecureStorageAvailableAsync(CancellationToken cancellationToken = default);
}

public interface ICustomerNativeDeviceSigningKey : IDeviceProofSigningKey
{
    ValueTask<bool> IsNativeSigningKeyAvailableAsync(CancellationToken cancellationToken = default);
}

public interface ICustomerSyncNetworkProvider
{
    bool IsPlatformTransportAvailable { get; }

    /// <summary>The handler must not persist or trace Authorization, DPoP, nonce, or request bodies.</summary>
    HttpClient SyncHttpClient { get; }
    ValueTask<bool> IsNetworkAvailableAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// An explicit application contract is mandatory for customer offline writes. Its absence is a
/// normal read-cache-only state and never creates a durable write queue.
/// </summary>
public interface ICustomerOfflineWriteContract
{
    string ContractId { get; }
    bool Allows(string actionCode, string operationType, string entityType);
}

public sealed record CustomerOfflineDependencies(
    ICustomerNativeEncryptionKeyProvider EncryptionKeys,
    ICustomerNativeDeviceSigningKey DeviceSigningKey,
    IInMemoryBearerTokenProvider VolatileSession,
    ICustomerSyncNetworkProvider Network,
    ICustomerOfflineWriteContract? WriteContract = null);

/// <summary>No plaintext or software-key fallback; native hosts replace this adapter.</summary>
public sealed class CustomerUnavailableNativeSecurity : ICustomerNativeEncryptionKeyProvider, ICustomerNativeDeviceSigningKey
{
    public ValueTask<bool> IsNativeSecureStorageAvailableAsync(CancellationToken cancellationToken = default) =>
        ValueTask.FromResult(false);

    public ValueTask<bool> IsNativeSigningKeyAvailableAsync(CancellationToken cancellationToken = default) =>
        ValueTask.FromResult(false);

    public ValueTask<byte[]> GetKeyAsync(LocalStorePurpose purpose, CancellationToken cancellationToken = default) =>
        ValueTask.FromException<byte[]>(new CustomerOfflineUnavailableException("NATIVE_SECURE_STORAGE_UNAVAILABLE"));

    public ValueTask<DevicePublicP256Jwk> GetPublicJwkAsync(CancellationToken cancellationToken = default) =>
        ValueTask.FromException<DevicePublicP256Jwk>(new CustomerOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE"));

    public ValueTask<byte[]> SignEs256Async(ReadOnlyMemory<byte> signingInput, CancellationToken cancellationToken = default) =>
        ValueTask.FromException<byte[]>(new CustomerOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE"));
}

public sealed class CustomerOfflineUnavailableException(string code) : InvalidOperationException(code)
{
    public string Code { get; } = code;
}

public sealed class CustomerOfflineRuntime
{
    private readonly CustomerOfflineCompositionOptions _options;
    private readonly CustomerOfflineDependencies _dependencies;
    private readonly OfflineReadCacheStore? _readCache;
    private readonly OfflineOperationStore? _outbox;
    private readonly OfflineSyncTransportClient? _transport;
    private readonly OfflineOperationScope _scope;
    private readonly OfflineSyncSupervisor? _supervisor;

    internal CustomerOfflineRuntime(
        CustomerOfflineCompositionOptions options,
        CustomerOfflineDependencies dependencies,
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
                result.Outbox, result.Transport, new CustomerSyncConnectivity(dependencies.Network))
            : null;
        Status = new(
            Map(result.Mode), result.ReasonCode,
            result.ReadCache is not null, result.Outbox is not null, result.Transport is not null);
    }

    public CustomerOfflineRuntimeStatus Status { get; }

    [Obsolete("Use the identity-aware template overload so the business payload is bound to ClientOperationId atomically.")]
    public Task<OfflineEnqueueResult> QueueAsync(
        OfflineOperationEnqueueRequest request,
        CancellationToken cancellationToken = default) =>
        Task.FromException<OfflineEnqueueResult>(
            new CustomerOfflineUnavailableException("OFFLINE_IDENTITY_FACTORY_REQUIRED"));

    public async Task<OfflineEnqueueResult> QueueAsync(
        OfflineOperationEnqueueTemplate request,
        Func<OfflineGeneratedOperationIdentity, string> payloadFactory,
        CancellationToken cancellationToken = default)
    {
        var outbox = _outbox ?? throw Unavailable();
        if (!MatchesScope(request)) throw new CustomerOfflineUnavailableException("LOCAL_SCOPE_DENIED");

        var contract = _dependencies.WriteContract;
        if (contract is null || string.IsNullOrWhiteSpace(contract.ContractId) ||
            !contract.Allows(request.ActionCode, request.OperationType, request.EntityType))
        {
            throw new CustomerOfflineUnavailableException("OFFLINE_WRITE_CONTRACT_REQUIRED");
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
            throw new CustomerOfflineUnavailableException("NETWORK_UNAVAILABLE");
        return await transport.ProcessNextBatchAsync(maximumOperations, cancellationToken);
    }

    public Task PutReadCacheAsync(string cacheKind, string cacheKey, string payloadJson, TimeSpan lifetime,
        CancellationToken cancellationToken = default) =>
        (_readCache ?? throw Unavailable()).PutAsync(cacheKind, cacheKey, payloadJson, lifetime, cancellationToken);

    public Task<string?> GetReadCacheAsync(string cacheKind, string cacheKey,
        CancellationToken cancellationToken = default) =>
        (_readCache ?? throw Unavailable()).GetAsync(cacheKind, cacheKey, cancellationToken);

    public async Task<CustomerOfflineOperationStatusView?> GetOperationStatusAsync(
        Guid localOperationId, CancellationToken cancellationToken = default)
    {
        var operation = await (_outbox ?? throw Unavailable()).GetAsync(localOperationId, _scope, cancellationToken);
        return operation is null ? null : CustomerOfflineOperationStatusView.From(operation);
    }

    public Task<int> RedactExpiredPayloadsAsync(CancellationToken cancellationToken = default) =>
        (_outbox ?? throw Unavailable()).RedactExpiredPayloadsAsync(cancellationToken: cancellationToken);

    private bool MatchesScope(OfflineOperationEnqueueTemplate request) =>
        request.CompanyId == _options.CompanyId && request.BranchId == _options.BranchId &&
        request.UserId == _options.UserId && request.RegisteredDeviceId == _options.RegisteredDeviceId;

    private CustomerOfflineUnavailableException Unavailable() => new(Status.ReasonCode);

    private sealed class CustomerSyncConnectivity(ICustomerSyncNetworkProvider network) : IOfflineSyncConnectivity
    {
        public async ValueTask<bool> IsOnlineAsync(CancellationToken cancellationToken = default) =>
            network.IsPlatformTransportAvailable && await network.IsNetworkAvailableAsync(cancellationToken);

        public async Task WaitUntilOnlineAsync(CancellationToken cancellationToken = default)
        {
            while (!await IsOnlineAsync(cancellationToken))
                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        }
    }

    private static CustomerOfflineRuntimeMode Map(MobileOfflineKernelMode mode) => mode switch
    {
        MobileOfflineKernelMode.Closed => CustomerOfflineRuntimeMode.Closed,
        MobileOfflineKernelMode.SecurityUnavailable => CustomerOfflineRuntimeMode.SecurityUnavailable,
        MobileOfflineKernelMode.ReadCacheOnly => CustomerOfflineRuntimeMode.ReadCacheOnly,
        MobileOfflineKernelMode.Ready => CustomerOfflineRuntimeMode.Ready,
        _ => CustomerOfflineRuntimeMode.Closed
    };
}

public static class CustomerOfflineComposition
{
    public static async Task<CustomerOfflineRuntime> CreateAsync(
        CustomerOfflineCompositionOptions options,
        CustomerOfflineDependencies dependencies,
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
        return new CustomerOfflineRuntime(options, dependencies, result);
    }

    private static void ValidateScope(CustomerOfflineCompositionOptions options)
    {
        if (options.CompanyId == Guid.Empty || options.BranchId == Guid.Empty || options.UserId == Guid.Empty ||
            options.RegisteredDeviceId == Guid.Empty || options.TransportOptions.RegisteredDeviceId != options.RegisteredDeviceId ||
            options.TransportOptions.CompanyId != options.CompanyId || options.TransportOptions.BranchId != options.BranchId ||
            options.TransportOptions.UserId != options.UserId)
            throw new ArgumentException("The configured mobile scope and transport device must be identical.", nameof(options));
    }
}
