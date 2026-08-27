using Microsoft.Maui.Storage;
using TransportERP.Application.Sync;
using TransportERP.Offline;
using TransportERP.Offline.Transport;

namespace TransportERP.Mobile.Driver.Offline;

public sealed record DriverOfflineActionGrant(
    string ActionCode,
    string OperationType,
    string EntityType);

/// <summary>
/// Deliberately not a record: generated record formatting would include the session bearer if a
/// caller accidentally logged the request object.
/// </summary>
public sealed class DriverOfflineActivationRequest
{
    public DriverOfflineActivationRequest(
        Guid companyId,
        Guid branchId,
        Guid userId,
        Guid registeredDeviceId,
        Guid sessionId,
        string deviceId,
        string sessionBearer,
        Uri batchEndpoint,
        IReadOnlyCollection<DriverOfflineActionGrant> grantedActions,
        DriverOfflineOperationPermissions operationPermissions,
        SyncClientEffectivePolicy effectivePolicy,
        BuildIdentityV1 buildIdentity,
        bool offlineRuntimeAuthorized = false)
    {
        CompanyId = companyId;
        BranchId = branchId;
        UserId = userId;
        RegisteredDeviceId = registeredDeviceId;
        SessionId = sessionId;
        DeviceId = deviceId;
        SessionBearer = sessionBearer;
        BatchEndpoint = batchEndpoint;
        GrantedActions = grantedActions;
        OperationPermissions = operationPermissions;
        EffectivePolicy = effectivePolicy;
        BuildIdentity = buildIdentity;
        OfflineRuntimeAuthorized = offlineRuntimeAuthorized;
    }

    public Guid CompanyId { get; }
    public Guid BranchId { get; }
    public Guid UserId { get; }
    public Guid RegisteredDeviceId { get; }
    public Guid SessionId { get; }
    public string DeviceId { get; }
    internal string SessionBearer { get; }
    public Uri BatchEndpoint { get; }
    public IReadOnlyCollection<DriverOfflineActionGrant> GrantedActions { get; }
    public DriverOfflineOperationPermissions OperationPermissions { get; }
    public SyncClientEffectivePolicy EffectivePolicy { get; }
    public BuildIdentityV1 BuildIdentity { get; }
    public bool OfflineRuntimeAuthorized { get; }
}

public sealed record DriverOfflineActivationResult(
    Guid SessionId,
    DriverOfflineRuntime Runtime);

public interface IDriverOfflineFeatureGate
{
    bool Allows(DriverDeviceKeyBindingContext context);
}

/// <summary>The shipping host is closed until the owner deliberately replaces this registration.</summary>
public sealed class DriverClosedOfflineFeatureGate : IDriverOfflineFeatureGate
{
    public bool Allows(DriverDeviceKeyBindingContext context) => false;
}

/// <summary>
/// Ephemeral, exact-session authorization populated only from GET /api/v1/sync/activation.
/// Restart, logout and access-token expiry all return the gate to closed.
/// </summary>
public sealed class DriverServerOfflineFeatureGate : IDriverOfflineFeatureGate
{
    private readonly object _gate = new();
    private DriverOfflineFeatureAuthorization? _authorization;

    public bool Allows(DriverDeviceKeyBindingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        lock (_gate)
        {
            var authorization = _authorization;
            return authorization is not null && authorization.ExpiresAt > DateTimeOffset.UtcNow &&
                authorization.CompanyId == context.CompanyId && authorization.BranchId == context.BranchId &&
                authorization.UserId == context.UserId &&
                authorization.RegisteredDeviceId == context.RegisteredDeviceId &&
                authorization.SessionId == context.SessionId &&
                string.Equals(authorization.DeviceId, context.DeviceId, StringComparison.Ordinal);
        }
    }

    internal void Authorize(DriverServerActivationDecision decision, DateTimeOffset expiresAt)
    {
        ArgumentNullException.ThrowIfNull(decision);
        lock (_gate)
        {
            _authorization = decision.Enabled && expiresAt > DateTimeOffset.UtcNow
                ? new(decision.CompanyId, decision.BranchId, decision.UserId,
                    decision.RegisteredDeviceId, decision.SessionId, decision.DeviceId, expiresAt)
                : null;
        }
    }

    internal void Clear()
    {
        lock (_gate) _authorization = null;
    }

    private sealed record DriverOfflineFeatureAuthorization(
        Guid CompanyId,
        Guid BranchId,
        Guid UserId,
        Guid RegisteredDeviceId,
        Guid SessionId,
        string DeviceId,
        DateTimeOffset ExpiresAt);
}

/// <summary>
/// Explicit authenticated activation boundary. Registration of this service has no side effects;
/// callers must provide the complete scope, current volatile session and server-derived grants.
/// </summary>
public sealed class DriverOfflineActivationService(
    IDriverNativeEncryptionKeyProvider encryptionKeys,
    IDriverNativeDeviceSigningKey signingKey,
    IDriverDeviceKeyBindingVerifier deviceKeyBindingVerifier,
    DriverVolatileSessionProvider volatileSession,
    IDriverSyncNetworkProvider network,
    IDriverOfflineFeatureGate featureGate)
{
    private readonly SemaphoreSlim _gate = new(1, 1);
    private DriverOfflineActivationResult? _active;
    private CancellationTokenSource? _supervisorCancellation;
    private Task? _supervisorTask;

    public DriverOfflineActivationResult? Active => Volatile.Read(ref _active);
    public event EventHandler? StateChanged;

    public async Task<DriverOfflineActivationResult> ActivateAsync(
        DriverOfflineActivationRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        Validate(request);
        var bindingContext = new DriverDeviceKeyBindingContext(
            request.CompanyId,
            request.BranchId,
            request.UserId,
            request.RegisteredDeviceId,
            request.SessionId,
            request.DeviceId);
        if (!request.OfflineRuntimeAuthorized || !featureGate.Allows(bindingContext))
            throw new DriverOfflineUnavailableException("OFFLINE_CLOSED");

        await _gate.WaitAsync(cancellationToken);
        try
        {
            if (_active is not null)
                throw new DriverOfflineUnavailableException("DRIVER_OFFLINE_ALREADY_ACTIVE");

            var verifiedDeviceKeyBinding = await DriverDeviceKeyBindingGuard.RequireMatchAsync(
                bindingContext,
                signingKey,
                deviceKeyBindingVerifier,
                cancellationToken);

            volatileSession.Set(request.SessionBearer);
            try
            {
                var scopeDirectory = Path.Combine(
                    FileSystem.AppDataDirectory,
                    "offline-v1",
                    request.CompanyId.ToString("N"),
                    request.BranchId.ToString("N"),
                    request.UserId.ToString("N"),
                    request.RegisteredDeviceId.ToString("N"));
                var transport = new OfflineSyncTransportOptions(
                    request.BatchEndpoint,
                    request.DeviceId,
                    request.RegisteredDeviceId,
                    request.CompanyId,
                    request.BranchId,
                    request.UserId,
                    $"driver-android-{Guid.NewGuid():N}",
                    MaximumBatchOperations: request.EffectivePolicy.MaxBatchOperations,
                    MaximumRequestBodyBytes: request.EffectivePolicy.MaximumRequestBodyBytes,
                    MaximumPayloadBytes: request.EffectivePolicy.MaximumPayloadBytes,
                    BuildIdentity: request.BuildIdentity);
                var options = new DriverOfflineCompositionOptions(
                    request.CompanyId,
                    request.BranchId,
                    request.UserId,
                    request.RegisteredDeviceId,
                    Path.Combine(scopeDirectory, "outbox.db"),
                    Path.Combine(scopeDirectory, "read-cache.db"),
                    transport,
                    request.EffectivePolicy,
                    new OfflineRetryPolicy(
                        request.EffectivePolicy.ClientTransportMaxRetryCount,
                        request.EffectivePolicy.ClientRetryBaseDelay,
                        request.EffectivePolicy.ClientRetryMaxDelay),
                    OfflineRuntimeAuthorized: true);
                var dependencies = new DriverOfflineDependencies(
                    encryptionKeys,
                    signingKey,
                    volatileSession,
                    network,
                    new ExactDriverOfflineActionAllowlist(request.GrantedActions),
                    request.OperationPermissions,
                    verifiedDeviceKeyBinding);
                var runtime = await DriverOfflineComposition.CreateAsync(
                    options,
                    dependencies,
                    cancellationToken: cancellationToken);
                if (runtime.Status.Mode is not DriverOfflineRuntimeMode.Ready)
                    throw new DriverOfflineUnavailableException(runtime.Status.ReasonCode);

                var activated = new DriverOfflineActivationResult(request.SessionId, runtime);
                var supervisorCancellation = new CancellationTokenSource();
                var supervisorTask = RunSupervisorAsync(runtime, supervisorCancellation.Token);
                _supervisorCancellation = supervisorCancellation;
                _supervisorTask = supervisorTask;
                Volatile.Write(ref _active, activated);
                NotifyStateChanged();
                return activated;
            }
            catch
            {
                volatileSession.Clear();
                throw;
            }
        }
        finally
        {
            _gate.Release();
        }
    }

    public async Task DeactivateAsync(CancellationToken cancellationToken = default)
    {
        await _gate.WaitAsync(cancellationToken);
        try
        {
            Volatile.Write(ref _active, null);
            NotifyStateChanged();
            _supervisorCancellation?.Cancel();
            if (_supervisorTask is not null)
            {
                try
                {
                    await _supervisorTask.WaitAsync(cancellationToken);
                }
                catch (OperationCanceledException) when (_supervisorCancellation?.IsCancellationRequested == true)
                {
                    // Expected when the authenticated session is deactivated.
                }
                catch
                {
                    // Session teardown must still clear the bearer; supervisor details can contain
                    // transport data and are deliberately not retained or surfaced here.
                }
            }
            _supervisorTask = null;
            _supervisorCancellation?.Dispose();
            _supervisorCancellation = null;
            volatileSession.Clear();
        }
        finally
        {
            _gate.Release();
        }
    }

    private static async Task RunSupervisorAsync(
        DriverOfflineRuntime runtime,
        CancellationToken cancellationToken)
    {
        try
        {
            await runtime.RunSyncSupervisorAsync(cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            // Expected when the authenticated session is deactivated.
        }
    }

    private static void Validate(DriverOfflineActivationRequest request)
    {
        if (request.CompanyId == Guid.Empty || request.BranchId == Guid.Empty ||
            request.UserId == Guid.Empty || request.RegisteredDeviceId == Guid.Empty ||
            request.SessionId == Guid.Empty || string.IsNullOrWhiteSpace(request.DeviceId) ||
            request.DeviceId.Any(char.IsWhiteSpace) ||
            string.IsNullOrEmpty(request.SessionBearer) ||
            request.SessionBearer.Any(character => character > 0x7f || char.IsWhiteSpace(character)) ||
            !request.BatchEndpoint.IsAbsoluteUri || request.BatchEndpoint.Scheme != Uri.UriSchemeHttps ||
            request.GrantedActions is null || request.GrantedActions.Count == 0 ||
            request.OperationPermissions is null || request.EffectivePolicy is null ||
            !request.EffectivePolicy.IsValid || request.BuildIdentity is not { IsValid: true })
        {
            throw new ArgumentException("A complete HTTPS scope, session and grant set is required.", nameof(request));
        }
    }

    private void NotifyStateChanged()
    {
        var handlers = StateChanged?.GetInvocationList();
        if (handlers is null) return;
        foreach (var subscriber in handlers)
        {
            if (subscriber is not EventHandler handler) continue;
            try
            {
                handler(this, EventArgs.Empty);
            }
            catch
            {
                // UI notification cannot alter activation or expose runtime details.
            }
        }
    }

    private sealed class ExactDriverOfflineActionAllowlist : IDriverOfflineActionAllowlist
    {
        private readonly HashSet<(string ActionCode, string OperationType, string EntityType)> _grants;

        public ExactDriverOfflineActionAllowlist(IEnumerable<DriverOfflineActionGrant> grants)
        {
            _grants = new(grants.Select(grant =>
                (ValidatePart(grant.ActionCode), ValidatePart(grant.OperationType), ValidatePart(grant.EntityType))));
        }

        public bool Allows(string actionCode, string operationType, string entityType) =>
            _grants.Contains((actionCode, operationType, entityType));

        private static string ValidatePart(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Offline action grants cannot contain empty fields.");
            return value;
        }
    }
}

/// <summary>Session bearer lives in managed memory only and is cleared on deactivation/failure.</summary>
public sealed class DriverVolatileSessionProvider : IInMemoryBearerTokenProvider
{
    private string? _bearer;

    public ValueTask<string> GetBearerTokenAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return ValueTask.FromResult(
            Volatile.Read(ref _bearer) ??
            throw new DriverOfflineUnavailableException("SESSION_TOKEN_UNAVAILABLE"));
    }

    internal void Set(string bearer) => Volatile.Write(ref _bearer, bearer);
    internal void Clear() => Volatile.Write(ref _bearer, null);
}
