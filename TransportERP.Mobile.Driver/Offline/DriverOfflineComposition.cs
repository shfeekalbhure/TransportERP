using TransportERP.Mobile.Internal;
using TransportERP.Application.Sync;
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
    bool ServerAccepted,
    DateTimeOffset UpdatedAt,
    long? ConflictBaseVersion,
    string? ConflictReason,
    long? ConflictServerVersion,
    string? RedactedLocalSnapshot,
    string? RedactedServerSnapshot,
    bool ConflictDecisionReady)
{
    public string SafeSummary =>
        $"{LocalOperationId:D} | {ActionCode} | {Status} | retries={RetryCount} | " +
        $"next={NextRetryAt?.ToString("O") ?? "NONE"} | result={ResultCode ?? "NONE"} | updated={UpdatedAt:O}";

    public string SafeConflictReview => !ConflictDecisionReady
        ? "Conflict review: NOT_AVAILABLE"
        : $"Conflict review: reason={ConflictReason}; base={ConflictBaseVersion}; " +
          $"serverVersion={ConflictServerVersion?.ToString() ?? "NONE"}; " +
          $"local={RedactedLocalSnapshot}; server={RedactedServerSnapshot}";

    internal static DriverOfflineOperationStatusView From(OfflineOperation operation) => new(
        operation.LocalOperationId,
        SanitizeActionCode(operation.ActionCode),
        operation.Status,
        operation.ClientTransportRetryCount,
        operation.NextRetryAt,
        SanitizeResultCode(operation.ResultCode),
        operation.ServerOperationId is { } serverOperationId && serverOperationId != Guid.Empty,
        operation.UpdatedAt,
        operation.ConflictReview?.BaseVersion,
        SanitizeConflictReason(operation.ConflictReview?.ConflictReason),
        operation.ConflictReview?.ServerSnapshot?.CurrentVersion,
        BuildRedactedLocalSnapshot(operation.ConflictReview),
        BuildRedactedServerSnapshot(operation.ConflictReview),
        operation.ConflictReview is { IsDecisionReady: true, BaseVersion: > 0 });

    private static string SanitizeActionCode(string? code) => code switch
    {
        { Length: > 0 and <= 96 } when code.All(character =>
            char.IsAsciiLetterOrDigit(character) || character is '_' or '-' or '.') => code,
        _ => "INVALID_ACTION_CODE"
    };

    private static string? SanitizeResultCode(string? code) => code switch
    {
        null => null,
        "invalid_dpop_proof" => "INVALID_DPOP_PROOF",
        "use_dpop_nonce" => "USE_DPOP_NONCE",
        { Length: > 0 and <= 64 } when code.All(character =>
            character is >= 'A' and <= 'Z' or >= '0' and <= '9' or '_') => code,
        _ => "INVALID_RESULT_CODE"
    };

    private static string? SanitizeConflictReason(string? code) => code switch
    {
        null => null,
        { Length: > 0 and <= 120 } when code.All(character =>
            char.IsAsciiLetterOrDigit(character) || character is '_' or '-' or '.') => code,
        _ => "INVALID_CONFLICT_REASON"
    };

    private static string? BuildRedactedLocalSnapshot(OfflineConflictReview? review) => review?.LocalSnapshot is { } local
        ? $"action={SanitizeActionCode(local.ActionCode)},entityType={SanitizeActionCode(local.EntityType)}," +
          $"entityId={local.EntityId?.ToString("D") ?? "NONE"},base={local.BaseVersion}"
        : null;

    private static string? BuildRedactedServerSnapshot(OfflineConflictReview? review) => review?.ServerSnapshot is { } server
        ? $"entityType={SanitizeActionCode(server.EntityType)},entityId={server.EntityId?.ToString("D") ?? "NONE"}," +
          $"exists={server.Exists},version={server.CurrentVersion?.ToString() ?? "NONE"}"
        : null;
}

public sealed record DriverOfflineCompositionOptions(
    Guid CompanyId,
    Guid BranchId,
    Guid UserId,
    Guid RegisteredDeviceId,
    string OutboxDatabasePath,
    string ReadCacheDatabasePath,
    OfflineSyncTransportOptions TransportOptions,
    SyncClientEffectivePolicy EffectivePolicy,
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

/// <summary>Server-derived user capabilities for interactive local operation management.</summary>
public sealed record DriverOfflineOperationPermissions(
    bool CanRetryFailedOperations,
    bool CanResolveConflicts);

public sealed record DriverOfflineDependencies(
    IDriverNativeEncryptionKeyProvider EncryptionKeys,
    IDriverNativeDeviceSigningKey DeviceSigningKey,
    IInMemoryBearerTokenProvider VolatileSession,
    IDriverSyncNetworkProvider Network,
    IDriverOfflineActionAllowlist? ActionAllowlist,
    DriverOfflineOperationPermissions OperationPermissions,
    DriverVerifiedDeviceKeyBinding? VerifiedDeviceKeyBinding);

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

public sealed class DriverOfflineUnavailableException : InvalidOperationException
{
    public DriverOfflineUnavailableException(string code, Exception? innerException = null)
        : base(code, innerException)
    {
        Code = code;
    }

    public string Code { get; }
}

public sealed class DriverOfflineRuntime
{
    private readonly Guid _companyId;
    private readonly Guid _branchId;
    private readonly Guid _userId;
    private readonly Guid _registeredDeviceId;
    private readonly SyncClientEffectivePolicy _effectivePolicy;
    private readonly OfflineOperationScope _scope;
    private readonly IDriverOfflineActionAllowlist? _allowlist;
    private readonly IDriverSyncNetworkProvider _network;
    private readonly OfflineReadCacheStore? _readCache;
    private readonly OfflineOperationStore? _outbox;
    private readonly OfflineSyncConflictClient? _conflicts;
    private readonly OfflineSyncSupervisor? _supervisor;

    internal DriverOfflineRuntime(
        DriverOfflineCompositionOptions options,
        DriverOfflineDependencies dependencies,
        MobileOfflineKernelResult result,
        TimeProvider? timeProvider)
    {
        _companyId = options.CompanyId;
        _branchId = options.BranchId;
        _userId = options.UserId;
        _registeredDeviceId = options.RegisteredDeviceId;
        _effectivePolicy = options.EffectivePolicy;
        _scope = new OfflineOperationScope(
            options.CompanyId, options.BranchId, options.UserId, options.RegisteredDeviceId);
        _allowlist = dependencies.ActionAllowlist;
        _network = dependencies.Network;
        _readCache = result.ReadCache;
        _outbox = result.Outbox;
        OperationPermissions = dependencies.OperationPermissions;
        _conflicts = result is { Outbox: not null, Transport: not null }
            ? new OfflineSyncConflictClient(
                dependencies.Network.SyncHttpClient,
                result.Outbox,
                dependencies.VolatileSession,
                dependencies.DeviceSigningKey,
                options.TransportOptions,
                timeProvider)
            : null;
        _supervisor = result is { Outbox: not null, Transport: not null }
            ? new OfflineSyncSupervisor(
                result.Outbox, result.Transport, new DriverSyncConnectivity(dependencies.Network),
                new OfflineSyncSupervisorOptions(
                    options.EffectivePolicy.MaxBatchOperations,
                    RetentionPolicy: new OfflineRetentionPolicy(
                        options.EffectivePolicy.LocalSuccessRetention,
                        options.EffectivePolicy.LocalRejectedRetention)))
            : null;
        Status = new(
            Map(result.Mode),
            result.ReasonCode,
            result.ReadCache is not null,
            result.Outbox is not null,
            result.Transport is not null);
    }

    public DriverOfflineRuntimeStatus Status { get; }
    public DriverOfflineOperationPermissions OperationPermissions { get; }
    public SyncClientEffectivePolicy EffectivePolicy => _effectivePolicy;
    public OfflineSyncSupervisorFailure? LastSyncSupervisorFailure =>
        _supervisor?.LastObservedFailure;
    public bool CanQueueOperationalParties => _outbox is not null && _allowlist?.Allows(
        "CreateOperationalParty", "CREATE", "OperationalParty") == true;

    public DriverOfflineBusinessProducer CreateBusinessProducer()
    {
        if (_outbox is null) throw Unavailable();
        if (!CanQueueOperationalParties)
            throw new DriverOfflineUnavailableException("OFFLINE_ACTION_NOT_AUTHORIZED");
        return new DriverOfflineBusinessProducer(this, _scope);
    }

    [Obsolete("Use the identity-aware template overload so the business payload is bound to ClientOperationId atomically.")]
    public Task<OfflineEnqueueResult> QueueAsync(
        OfflineOperationEnqueueRequest request,
        CancellationToken cancellationToken = default) =>
        Task.FromException<OfflineEnqueueResult>(
            new DriverOfflineUnavailableException("OFFLINE_IDENTITY_FACTORY_REQUIRED"));

    public async Task<OfflineEnqueueResult> QueueAsync(
        OfflineOperationEnqueueTemplate template,
        Func<OfflineGeneratedOperationIdentity, string> payloadFactory,
        CancellationToken cancellationToken = default)
    {
        var outbox = _outbox ?? throw Unavailable();
        if (!MatchesScope(template))
        {
            throw new DriverOfflineUnavailableException("LOCAL_SCOPE_DENIED");
        }

        if (_allowlist is null || !_allowlist.Allows(template.ActionCode, template.OperationType, template.EntityType))
        {
            throw new DriverOfflineUnavailableException("OFFLINE_ACTION_NOT_AUTHORIZED");
        }

        var result = await outbox.EnqueueAsync(template, payloadFactory, cancellationToken);
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
        var supervisor = _supervisor ?? throw Unavailable();
        if (!await _network.IsNetworkAvailableAsync(cancellationToken))
        {
            throw new DriverOfflineUnavailableException("NETWORK_UNAVAILABLE");
        }

        return await supervisor.SynchronizeNowAsync(maximumOperations, cancellationToken);
    }

    public Task PutReadCacheAsync(
        string cacheKind,
        string cacheKey,
        string payloadJson,
        TimeSpan lifetime,
        CancellationToken cancellationToken = default)
    {
        if (lifetime <= TimeSpan.Zero || lifetime > _effectivePolicy.CacheMaxAge)
            throw new DriverOfflineUnavailableException("READ_CACHE_POLICY_DENIED");
        return (_readCache ?? throw Unavailable()).PutAsync(
            cacheKind, cacheKey, payloadJson, lifetime, cancellationToken);
    }

    public Task<string?> GetReadCacheAsync(
        string cacheKind,
        string cacheKey,
        CancellationToken cancellationToken = default) =>
        (_readCache ?? throw Unavailable()).GetAsync(cacheKind, cacheKey, cancellationToken);

    public async Task<DriverOfflineOperationStatusView?> GetOperationStatusAsync(
        Guid localOperationId,
        CancellationToken cancellationToken = default)
    {
        var operation = await (_outbox ?? throw Unavailable()).GetAsync(localOperationId, _scope, cancellationToken);
        return operation is null ? null : DriverOfflineOperationStatusView.From(operation);
    }

    public async Task<IReadOnlyList<DriverOfflineOperationStatusView>> ListOperationStatusesAsync(
        CancellationToken cancellationToken = default)
    {
        var operations = await (_outbox ?? throw Unavailable()).ListAsync(_scope, cancellationToken);
        return operations.Select(DriverOfflineOperationStatusView.From).ToArray();
    }

    public async Task RetryFailedOperationAsync(
        Guid localOperationId,
        CancellationToken cancellationToken = default)
    {
        if (!OperationPermissions.CanRetryFailedOperations)
            throw new DriverOfflineUnavailableException("SYNC_OPERATION_RETRY_NOT_AUTHORIZED");

        await (_outbox ?? throw Unavailable()).RequeueFailedAsync(localOperationId, _scope, cancellationToken);
        _supervisor?.NotifyWorkAvailable();
    }

    public Task ResolveConflictAsync(
        Guid localOperationId,
        OfflineConflictDecision decision,
        string reason,
        long? reapplyBaseVersion = null,
        CancellationToken cancellationToken = default)
    {
        if (!OperationPermissions.CanResolveConflicts)
            throw new DriverOfflineUnavailableException("SYNC_CONFLICT_RESOLVE_NOT_AUTHORIZED");

        return (_conflicts ?? throw Unavailable()).ResolveAsync(
            localOperationId, decision, reason, reapplyBaseVersion, cancellationToken);
    }

    public Task<int> RedactExpiredPayloadsAsync(CancellationToken cancellationToken = default) =>
        (_outbox ?? throw Unavailable()).RedactExpiredPayloadsAsync(
            new OfflineRetentionPolicy(
                _effectivePolicy.LocalSuccessRetention,
                _effectivePolicy.LocalRejectedRetention),
            cancellationToken);

    private bool MatchesScope(OfflineOperationEnqueueTemplate request) =>
        request.CompanyId == _companyId && request.BranchId == _branchId &&
        request.UserId == _userId && request.RegisteredDeviceId == _registeredDeviceId;

    private DriverOfflineUnavailableException Unavailable() => new(Status.ReasonCode);

    private sealed class DriverSyncConnectivity(IDriverSyncNetworkProvider network) : IOfflineSyncConnectivity
    {
        public async ValueTask<bool> IsOnlineAsync(CancellationToken cancellationToken = default) =>
            network.IsPlatformTransportAvailable && await network.IsNetworkAvailableAsync(cancellationToken);

        public async Task WaitUntilOnlineAsync(CancellationToken cancellationToken = default)
        {
            while (!await IsOnlineAsync(cancellationToken))
                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        }
    }

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
        ArgumentNullException.ThrowIfNull(dependencies.OperationPermissions);
        ValidateScope(options);
        if (dependencies.VerifiedDeviceKeyBinding is null)
            throw new DriverOfflineUnavailableException("DEVICE_KEY_BINDING_VERIFICATION_REQUIRED");
        await DriverDeviceKeyBindingGuard.RequireStillCurrentAsync(
            dependencies.VerifiedDeviceKeyBinding,
            options,
            dependencies.DeviceSigningKey,
            cancellationToken);

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
        return new DriverOfflineRuntime(options, dependencies, result, timeProvider);
    }

    private static void ValidateScope(DriverOfflineCompositionOptions options)
    {
        if (options.CompanyId == Guid.Empty || options.BranchId == Guid.Empty || options.UserId == Guid.Empty ||
            options.RegisteredDeviceId == Guid.Empty ||
            options.TransportOptions.RegisteredDeviceId != options.RegisteredDeviceId ||
            options.TransportOptions.CompanyId != options.CompanyId ||
            options.TransportOptions.BranchId != options.BranchId ||
            options.TransportOptions.UserId != options.UserId ||
            options.EffectivePolicy is null || !options.EffectivePolicy.IsValid ||
            options.TransportOptions.MaximumBatchOperations != options.EffectivePolicy.MaxBatchOperations ||
            options.TransportOptions.MaximumRequestBodyBytes != options.EffectivePolicy.MaximumRequestBodyBytes ||
            options.TransportOptions.MaximumPayloadBytes != options.EffectivePolicy.MaximumPayloadBytes ||
            options.RetryPolicy is null ||
            options.RetryPolicy.MaxRetryCount != options.EffectivePolicy.ClientTransportMaxRetryCount ||
            options.RetryPolicy.EffectiveBaseDelay != options.EffectivePolicy.ClientRetryBaseDelay ||
            options.RetryPolicy.EffectiveMaxDelay != options.EffectivePolicy.ClientRetryMaxDelay)
        {
            throw new ArgumentException("The configured mobile scope and transport device must be identical.", nameof(options));
        }
    }
}
