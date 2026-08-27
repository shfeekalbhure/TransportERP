using TransportERP.Application.Sync;
using TransportERP.Desktop.Offline;

namespace TransportERP.Desktop.Application;

internal sealed record DesktopAuthenticatedSessionScope(
    Guid CompanyId,
    Guid BranchId,
    Guid UserId,
    Guid RegisteredDeviceId)
{
    internal bool IsComplete =>
        CompanyId != Guid.Empty && BranchId != Guid.Empty &&
        UserId != Guid.Empty && RegisteredDeviceId != Guid.Empty;
}

/// <summary>
/// A capability emitted by the online sign-in host after it has authenticated the user, resolved
/// the effective company/branch/device scope and authorized Offline for this session. It carries
/// identity scope and a one-use runtime factory only; session secrets remain in volatile services.
/// </summary>
internal sealed record DesktopAuthenticatedOfflineActivation(
    Guid SessionId,
    DateTimeOffset SessionExpiresAt,
    DesktopAuthenticatedSessionScope AuthenticatedScope,
    DesktopAuthenticatedSessionScope AuthorizedOfflineScope,
    bool OfflineRuntimeAuthorized,
    SyncClientEffectivePolicy EffectivePolicy,
    BuildIdentityV1 MeasuredBuildIdentity,
    IReadOnlySet<(string Action, string Operation, string Entity)> AllowedActions,
    Func<CancellationToken, Task<DesktopOfflineRuntime>> CreateRuntimeAsync);

internal interface IDesktopAuthenticatedSessionSource : IDisposable
{
    event EventHandler<DesktopAuthenticatedOfflineActivation>? SessionAuthenticated;
    event EventHandler<Guid>? SessionEnded;

    void Start(IDesktopOnlineSignInSurface signInSurface);
}

internal interface IDesktopOnlineSignInSurface
{
    event EventHandler<DesktopOnlineSignInRequest>? SignInRequested;
    event EventHandler? LogoutRequested;

    void ReportSignInFailed(string reasonCode);
    void ReportSignInSucceeded();
}

/// <summary>
/// Typed boundary called by the online sign-in host. The executable subscribes to this boundary
/// before the shell is displayed. The bridge begins closed and permits one authenticated session
/// publication only; it never transports or retains authentication secrets.
/// </summary>
internal sealed class DesktopOnlineSignInSessionBridge : IDesktopAuthenticatedSessionSource
{
    private readonly object _gate = new();
    private readonly IDesktopOnlineSessionAuthenticator _authenticator;
    private readonly CancellationTokenSource _lifetime = new();
    private IDesktopOnlineSignInSurface? _surface;
    private CancellationTokenSource? _sessionExpiry;
    private bool _started;
    private bool _published;
    private bool _ended;
    private bool _disposed;
    private Guid _sessionId;

    internal DesktopOnlineSignInSessionBridge(IDesktopOnlineSessionAuthenticator authenticator) =>
        _authenticator = authenticator ?? throw new ArgumentNullException(nameof(authenticator));

    public event EventHandler<DesktopAuthenticatedOfflineActivation>? SessionAuthenticated;
    public event EventHandler<Guid>? SessionEnded;

    public void Start(IDesktopOnlineSignInSurface signInSurface)
    {
        ArgumentNullException.ThrowIfNull(signInSurface);
        lock (_gate)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            if (_started)
                throw new InvalidOperationException("DESKTOP_SESSION_SOURCE_ALREADY_STARTED");
            _started = true;
            _surface = signInSurface;
            signInSurface.SignInRequested += OnSignInRequested;
            signInSurface.LogoutRequested += OnLogoutRequested;
        }
    }

    /// <summary>Called exactly once by the authenticated online sign-in host.</summary>
    internal void PublishAuthenticatedSession(DesktopAuthenticatedOfflineActivation activation)
    {
        ArgumentNullException.ThrowIfNull(activation);
        EventHandler<DesktopAuthenticatedOfflineActivation>? handler;
        lock (_gate)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            if (!_started)
                throw new InvalidOperationException("DESKTOP_SESSION_SOURCE_NOT_STARTED");
            if (_published || _ended)
                throw new InvalidOperationException("DESKTOP_SESSION_REPLAY_DENIED");

            _published = true;
            _sessionId = activation.SessionId;
            handler = SessionAuthenticated;
        }
        handler?.Invoke(this, activation);
        StartExpiryMonitor(activation.SessionId, activation.SessionExpiresAt);
    }

    /// <summary>Called by the online host when logout, revocation or authenticated session expiry occurs.</summary>
    internal void PublishSessionEnded(Guid sessionId)
    {
        EventHandler<Guid>? handler;
        lock (_gate)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            if (!_started || !_published || _ended || sessionId == Guid.Empty || sessionId != _sessionId)
                throw new InvalidOperationException("DESKTOP_SESSION_END_DENIED");

            _ended = true;
            _sessionExpiry?.Cancel();
            handler = SessionEnded;
        }
        handler?.Invoke(this, sessionId);
    }

    public void Dispose()
    {
        lock (_gate)
        {
            if (_disposed)
                return;
            _disposed = true;
            _lifetime.Cancel();
            _sessionExpiry?.Cancel();
            if (_surface is not null)
            {
                _surface.SignInRequested -= OnSignInRequested;
                _surface.LogoutRequested -= OnLogoutRequested;
            }
            SessionAuthenticated = null;
            SessionEnded = null;
        }
        _sessionExpiry?.Dispose();
        _lifetime.Dispose();
        _authenticator.Dispose();
    }

    private async void OnSignInRequested(object? sender, DesktopOnlineSignInRequest request)
    {
        try
        {
            var result = await _authenticator.AuthenticateAsync(request, _lifetime.Token);
            if (!result.Succeeded || result.Activation is null)
            {
                _surface?.ReportSignInFailed(result.Code);
                return;
            }
            _surface?.ReportSignInSucceeded();
            PublishAuthenticatedSession(result.Activation);
        }
        catch (OperationCanceledException) when (_lifetime.IsCancellationRequested)
        {
            // Application teardown.
        }
        catch
        {
            _surface?.ReportSignInFailed("DESKTOP_AUTHENTICATION_FAILED");
        }
    }

    private async void OnLogoutRequested(object? sender, EventArgs args)
    {
        Guid sessionId;
        lock (_gate)
            sessionId = _sessionId;
        if (sessionId == Guid.Empty)
            return;
        try
        {
            await _authenticator.EndSessionAsync(sessionId, _lifetime.Token);
        }
        catch
        {
            // Remote revocation failure never preserves the local authenticated capability.
        }
        finally
        {
            TryPublishSessionEnded(sessionId);
        }
    }

    private void StartExpiryMonitor(Guid sessionId, DateTimeOffset expiresAt)
    {
        _sessionExpiry = CancellationTokenSource.CreateLinkedTokenSource(_lifetime.Token);
        _ = MonitorExpiryAsync(sessionId, expiresAt, _sessionExpiry.Token);
    }

    private async Task MonitorExpiryAsync(Guid sessionId, DateTimeOffset expiresAt, CancellationToken cancellationToken)
    {
        try
        {
            var delay = expiresAt - DateTimeOffset.UtcNow;
            if (delay > TimeSpan.Zero)
                await Task.Delay(delay, cancellationToken);
            await _authenticator.EndSessionAsync(sessionId, cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            // Logout or application teardown.
        }
        catch
        {
            // Session expiry remains fail-closed even when remote revocation is unavailable.
        }
        finally
        {
            if (!cancellationToken.IsCancellationRequested)
                TryPublishSessionEnded(sessionId);
        }
    }

    private void TryPublishSessionEnded(Guid sessionId)
    {
        try { PublishSessionEnded(sessionId); }
        catch (InvalidOperationException) { /* A concurrent expiry/logout already ended it. */ }
    }
}

internal static class DesktopStartupContractProbe
{
    internal static bool VerifyClosedDefault()
    {
        if (DesktopStartupPolicy.OfflineRuntimeAuthorizedByDefault)
            return false;

        var activationCount = 0;
        using var source = new DesktopOnlineSignInSessionBridge(new ClosedSessionAuthenticator());
        var surface = new ClosedSignInSurface();
        source.SessionAuthenticated += (_, _) => activationCount++;
        source.Start(surface);

        // Starting the executable boundary alone publishes nothing and therefore cannot invoke a
        // runtime factory, initialize a local database, or open an OS signing handle.
        return activationCount == 0;
    }

    private sealed class ClosedSessionAuthenticator : IDesktopOnlineSessionAuthenticator
    {
        public Task<DesktopOnlineAuthenticationResult> AuthenticateAsync(
            DesktopOnlineSignInRequest request,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("STARTUP_SMOKE_MUST_NOT_AUTHENTICATE");

        public Task EndSessionAsync(Guid sessionId, CancellationToken cancellationToken) =>
            throw new InvalidOperationException("STARTUP_SMOKE_HAS_NO_SESSION");

        public void Dispose() { }
    }

    private sealed class ClosedSignInSurface : IDesktopOnlineSignInSurface
    {
        public event EventHandler<DesktopOnlineSignInRequest>? SignInRequested;
        public event EventHandler? LogoutRequested;

        public void ReportSignInFailed(string reasonCode) { }
        public void ReportSignInSucceeded() { }
    }
}
