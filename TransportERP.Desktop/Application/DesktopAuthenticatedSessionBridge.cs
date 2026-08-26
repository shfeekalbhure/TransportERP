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
    DesktopAuthenticatedSessionScope AuthenticatedScope,
    DesktopAuthenticatedSessionScope AuthorizedOfflineScope,
    bool OfflineRuntimeAuthorized,
    Func<CancellationToken, Task<DesktopOfflineRuntime>> CreateRuntimeAsync);

internal interface IDesktopAuthenticatedSessionSource : IDisposable
{
    event EventHandler<DesktopAuthenticatedOfflineActivation>? SessionAuthenticated;
    event EventHandler<Guid>? SessionEnded;

    void Start();
}

/// <summary>
/// Typed boundary called by the online sign-in host. The executable subscribes to this boundary
/// before the shell is displayed. The bridge begins closed and permits one authenticated session
/// publication only; it never transports or retains authentication secrets.
/// </summary>
internal sealed class DesktopOnlineSignInSessionBridge : IDesktopAuthenticatedSessionSource
{
    private readonly object _gate = new();
    private bool _started;
    private bool _published;
    private bool _ended;
    private bool _disposed;
    private Guid _sessionId;

    public event EventHandler<DesktopAuthenticatedOfflineActivation>? SessionAuthenticated;
    public event EventHandler<Guid>? SessionEnded;

    public void Start()
    {
        lock (_gate)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            if (_started)
                throw new InvalidOperationException("DESKTOP_SESSION_SOURCE_ALREADY_STARTED");
            _started = true;
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
            SessionAuthenticated = null;
            SessionEnded = null;
        }
    }
}

internal static class DesktopStartupContractProbe
{
    internal static bool VerifyClosedDefault()
    {
        if (DesktopStartupPolicy.OfflineRuntimeAuthorizedByDefault)
            return false;

        var activationCount = 0;
        using var source = new DesktopOnlineSignInSessionBridge();
        source.SessionAuthenticated += (_, _) => activationCount++;
        source.Start();

        // Starting the executable boundary alone publishes nothing and therefore cannot invoke a
        // runtime factory, initialize a local database, or open an OS signing handle.
        return activationCount == 0;
    }
}
