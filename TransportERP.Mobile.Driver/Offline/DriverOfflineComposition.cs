using TransportERP.Mobile.Internal;
using TransportERP.Offline;
using TransportERP.Offline.Transport;

namespace TransportERP.Mobile.Driver.Offline;

public enum DriverOfflineRuntimeMode
{
    Closed,
    SecurityUnavailable,
    ReadCacheOnly,
    Ready
}

public sealed record DriverOfflineRuntimeStatus(
    DriverOfflineRuntimeMode Mode,
    string ReasonCode,
    bool ReadCacheAvailable,
    bool WriteQueueAvailable,
    bool SyncTransportAvailable);

/// <summary>Payload-free metadata suitable for a future native status screen.</summary>
public sealed record DriverOfflineOperationStatusView(
    Guid LocalOperationId,
    string ActionCode,
    OfflineOperationStatus Status,
    int RetryCount,
    DateTimeOffset? NextRetryAt,
    string? ResultCode,
    DateTimeOffset UpdatedAt)
{
    internal static DriverOfflineOperationStatusView From(OfflineOperation operation) => new(
        operation.LocalOperationId,
        operation.ActionCode,
        operation.Status,
        operation.ClientTransportRetryCount,
        operation.NextRetryAt,
        operation.ResultCode,
        operation.UpdatedAt);
}

public sealed record DriverOfflineCompositionOptions(
    Guid CompanyId,
    Guid BranchId,
    Guid UserId,
    Guid RegisteredDeviceId,
    string OutboxDatabasePath,
    string ReadCacheDatabasePath,
    OfflineSyncTransportOptions TransportOptions,
    OfflineRetryPolicy? RetryPolicy = null,
    bool OfflineRuntimeAuthorized = false);

/// <summary>
/// Must be backed by Android/iOS/Windows native protected storage. Returning false prevents every
/// local database from being created; plaintext fallback is not permitted.
/// </summary>
public interface IDriverNativeEncryptionKeyProvider : ILocalEncryptionKeyProvider
{
    ValueTask<bool> IsNativeSecureStorageAvailableAsync(CancellationToken cancellationToken = default);
}

/// <summary>Signs through an opaque OS key handle. Private key export is outside this contract.</summary>
public interface IDriverNativeDeviceSigningKey : IDeviceProofSigningKey
{
    ValueTask<bool> IsNativeSigningKeyAvailableAsync(CancellationToken cancellationToken = default);
}

public interface IDriverSyncNetworkProvider
{
    bool IsPlatformTransportAvailable { get; }

    /// <summary>The handler must not persist or trace Authorization, DPoP, nonce, or request bodies.</summary>
    HttpClient SyncHttpClient { get; }
    ValueTask<bool> IsNetworkAvailableAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Supplied by the authenticated native host from the driver's current permissions. This gate is
/// applied before the Offline core's own supported-action validation.
/// </summary>
public interface IDriverOfflineActionAllowlist
{
    bool Allows(string actionCode, string operationType, string entityType);
}

public sealed record DriverOfflineDependencies(
    IDriverNativeEncryptionKeyProvider EncryptionKeys,
    IDriverNativeDeviceSigningKey DeviceSigningKey,
    IInMemoryBearerTokenProvider VolatileSession,
    IDriverSyncNetworkProvider Network,
    IDriverOfflineActionAllowlist? ActionAllowlist);

/// <summary>A concrete fail-closed placeholder until the real native security adapter is supplied.</summary>
public sealed class DriverUnavailableNativeSecurity : IDriverNativeEncryptionKeyProvider, IDriverNativeDeviceSigningKey
{
    public ValueTask<bool> IsNativeSecureStorageAvailableAsync(CancellationToken cancellationToken = default) =>
        ValueTask.FromResult(false);

    public ValueTask<bool> IsNativeSigningKeyAvailableAsync(CancellationToken cancellationToken = default) =>
        ValueTask.FromResult(false);

    public ValueTask<byte[]> GetKeyAsync(LocalStorePurpose purpose, CancellationToken cancellationToken = default) =>
        ValueTask.FromException<byte[]>(new DriverOfflineUnavailableException("NATIVE_SECURE_STORAGE_UNAVAILABLE"));

    public ValueTask<DevicePublicP256Jwk> GetPublicJwkAsync(CancellationToken cancellationToken = default) =>
        ValueTask.FromException<DevicePublicP256Jwk>(new DriverOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE"));

    public ValueTask<byte[]> SignEs256Async(ReadOnlyMemory<byte> signingInput, CancellationToken cancellationToken = default) =>
        ValueTask.FromException<byte[]>(new DriverOfflineUnavailableException("NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE"));
}

public sealed class DriverOfflineUnavailableException(string code) : InvalidOperationException(code)
{
    public string Code { get; } = code;
}

public sealed class DriverOfflineRuntime
{
    private readonly Guid _companyId;
    private readonly Guid _branchId;
    private readonly Guid _userId;
    private readonly Guid _registeredDeviceId;
    private readonly IDriverOfflineActionAllowlist? _allowlist;
    private readonly IDriverSyncNetworkProvider _network;
    private readonly OfflineReadCacheStore? _readCache;
    private readonly OfflineOperationStore? _outbox;
    private readonly OfflineSyncTransportClient? _transport;

    internal DriverOfflineRuntime(
        DriverOfflineCompositionOptions options,
        DriverOfflineDependencies dependencies,
        MobileOfflineKernelResult result)
    {
        _companyId = options.CompanyId;
        _branchId = options.BranchId;
        _userId = options.UserId;
        _registeredDeviceId = options.RegisteredDeviceId;
        _allowlist = dependencies.ActionAllowlist;
        _network = dependencies.Network;
        _readCache = result.ReadCache;
        _outbox = result.Outbox;
        _transport = result.Transport;
        Status = new(
            Map(result.Mode),
            result.ReasonCode,
            result.ReadCache is not null,
            result.Outbox is not null,
            result.Transport is not null);
    }

    public DriverOfflineRuntimeStatus Status { get; }

    public async Task<OfflineEnqueueResult> QueueAsync(
        OfflineOperationEnqueueRequest request,
        CancellationToken cancellationToken = default)
    {
        var outbox = _outbox ?? throw Unavailable();
        if (!MatchesScope(request))
        {
            throw new DriverOfflineUnavailableException("LOCAL_SCOPE_DENIED");
        }

        if (_allowlist is null || !_allowlist.Allows(request.ActionCode, request.OperationType, request.EntityType))
        {
            throw new DriverOfflineUnavailableException("OFFLINE_ACTION_NOT_AUTHORIZED");
        }

        return await outbox.EnqueueAsync(request, cancellationToken);
    }

    public async Task<OfflineSyncTransportRunResult> SynchronizeAsync(
        int? maximumOperations = null,
        CancellationToken cancellationToken = default)
    {
        var transport = _transport ?? throw Unavailable();
        if (!await _network.IsNetworkAvailableAsync(cancellationToken))
        {
            throw new DriverOfflineUnavailableException("NETWORK_UNAVAILABLE");
        }

        return await transport.ProcessNextBatchAsync(maximumOperations, cancellationToken);
    }

    public Task PutReadCacheAsync(
        string cacheKind,
        string cacheKey,
        string payloadJson,
        TimeSpan lifetime,
        CancellationToken cancellationToken = default) =>
        (_readCache ?? throw Unavailable()).PutAsync(cacheKind, cacheKey, payloadJson, lifetime, cancellationToken);

    public Task<string?> GetReadCacheAsync(
        string cacheKind,
        string cacheKey,
        CancellationToken cancellationToken = default) =>
        (_readCache ?? throw Unavailable()).GetAsync(cacheKind, cacheKey, cancellationToken);

    public async Task<DriverOfflineOperationStatusView?> GetOperationStatusAsync(
        Guid localOperationId,
        CancellationToken cancellationToken = default)
    {
        var operation = await (_outbox ?? throw Unavailable()).GetAsync(localOperationId, cancellationToken);
        return operation is null ? null : DriverOfflineOperationStatusView.From(operation);
    }

    public Task<int> RedactExpiredPayloadsAsync(CancellationToken cancellationToken = default) =>
        (_outbox ?? throw Unavailable()).RedactExpiredPayloadsAsync(cancellationToken: cancellationToken);

    private bool MatchesScope(OfflineOperationEnqueueRequest request) =>
        request.CompanyId == _companyId && request.BranchId == _branchId &&
        request.UserId == _userId && request.RegisteredDeviceId == _registeredDeviceId;

    private DriverOfflineUnavailableException Unavailable() => new(Status.ReasonCode);

    private static DriverOfflineRuntimeMode Map(MobileOfflineKernelMode mode) => mode switch
    {
        MobileOfflineKernelMode.Closed => DriverOfflineRuntimeMode.Closed,
        MobileOfflineKernelMode.SecurityUnavailable => DriverOfflineRuntimeMode.SecurityUnavailable,
        MobileOfflineKernelMode.ReadCacheOnly => DriverOfflineRuntimeMode.ReadCacheOnly,
        MobileOfflineKernelMode.Ready => DriverOfflineRuntimeMode.Ready,
        _ => DriverOfflineRuntimeMode.Closed
    };
}

public static class DriverOfflineComposition
{
    public static async Task<DriverOfflineRuntime> CreateAsync(
        DriverOfflineCompositionOptions options,
        DriverOfflineDependencies dependencies,
        TimeProvider? timeProvider = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(dependencies);
        ValidateScope(options);

        var result = await MobileOfflineCompositionKernel.ComposeAsync(
            new(
                options.OfflineRuntimeAuthorized,
                dependencies.ActionAllowlist is not null,
                options.OutboxDatabasePath,
                options.ReadCacheDatabasePath,
                options.TransportOptions,
                options.RetryPolicy),
            dependencies.EncryptionKeys,
            dependencies.DeviceSigningKey,
            dependencies.VolatileSession,
            () => dependencies.Network.SyncHttpClient,
            dependencies.EncryptionKeys.IsNativeSecureStorageAvailableAsync,
            dependencies.DeviceSigningKey.IsNativeSigningKeyAvailableAsync,
            () => dependencies.Network.IsPlatformTransportAvailable,
            timeProvider,
            cancellationToken);
        return new DriverOfflineRuntime(options, dependencies, result);
    }

    private static void ValidateScope(DriverOfflineCompositionOptions options)
    {
        if (options.CompanyId == Guid.Empty || options.BranchId == Guid.Empty || options.UserId == Guid.Empty ||
            options.RegisteredDeviceId == Guid.Empty ||
            options.TransportOptions.RegisteredDeviceId != options.RegisteredDeviceId ||
            options.TransportOptions.CompanyId != options.CompanyId ||
            options.TransportOptions.BranchId != options.BranchId ||
            options.TransportOptions.UserId != options.UserId)
        {
            throw new ArgumentException("The configured mobile scope and transport device must be identical.", nameof(options));
        }
    }
}
