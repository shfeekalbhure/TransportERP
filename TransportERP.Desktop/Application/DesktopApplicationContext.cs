using TransportERP.Desktop.Offline;

namespace TransportERP.Desktop.Application;

internal static class DesktopStartupPolicy
{
    // Startup is closed; only an exact server-issued session/policy/device grant may activate it.
    internal const bool OfflineRuntimeAuthorizedByDefault = false;
}

/// <summary>
/// Owns the executable shell and the lifetime of an authenticated offline runtime. Startup itself
/// never creates local databases, opens a certificate, reads a bearer token, or enables Offline.
/// The online authentication host must explicitly activate a scoped runtime after sign-in.
/// </summary>
internal sealed class DesktopApplicationContext : System.Windows.Forms.ApplicationContext
{
    private readonly DesktopShellForm _shell;
    private readonly IDesktopAuthenticatedSessionSource _authenticatedSessions;
    private readonly CancellationTokenSource _applicationCancellation = new();
    private CancellationTokenSource? _activationCancellation;
    private CancellationTokenSource? _supervisorCancellation;
    private Task? _supervisor;
    private DesktopOfflineRuntime? _runtime;
    private Guid? _activeSessionId;
    private bool _activationAttempted;
    private bool _shutdownStarted;

    internal DesktopApplicationContext(IDesktopAuthenticatedSessionSource authenticatedSessions)
    {
        _authenticatedSessions = authenticatedSessions ?? throw new ArgumentNullException(nameof(authenticatedSessions));
        _shell = new DesktopShellForm();
        _shell.FormClosed += (_, _) => ExitThread();
        _authenticatedSessions.SessionAuthenticated += OnSessionAuthenticated;
        _authenticatedSessions.SessionEnded += OnSessionEnded;
        MainForm = _shell;
        _shell.Show();
        _authenticatedSessions.Start(_shell);
    }

    private void OnSessionAuthenticated(object? sender, DesktopAuthenticatedOfflineActivation activation) =>
        RunOnUiThread(() => ActivateAuthenticatedOfflineRuntimeAsync(activation));

    private void OnSessionEnded(object? sender, Guid sessionId) =>
        RunOnUiThread(() => EndAuthenticatedSessionAsync(sessionId));

    private async Task ActivateAuthenticatedOfflineRuntimeAsync(DesktopAuthenticatedOfflineActivation activation)
    {
        if (_shutdownStarted)
            return;
        if (_activationAttempted || !IsGovernedActivation(activation))
        {
            await TerminateFailClosedAsync("DESKTOP_AUTHENTICATED_ACTIVATION_DENIED");
            return;
        }

        // Mark and bind the session before invoking untrusted composition. A duplicate/replayed
        // event can never create a second local database or open another signing handle.
        _activationAttempted = true;
        _activeSessionId = activation.SessionId;
        _activationCancellation = CancellationTokenSource.CreateLinkedTokenSource(_applicationCancellation.Token);

        DesktopOfflineRuntime runtime;
        try
        {
            runtime = await activation.CreateRuntimeAsync(_activationCancellation.Token);
        }
        catch (OperationCanceledException) when (_activationCancellation.IsCancellationRequested)
        {
            return;
        }
        catch
        {
            await TerminateFailClosedAsync("DESKTOP_SECURE_RUNTIME_UNAVAILABLE");
            return;
        }

        if (_shutdownStarted || _activeSessionId != activation.SessionId)
        {
            runtime.Dispose();
            return;
        }
        if (runtime.Status.Mode != DesktopOfflineRuntimeMode.Ready)
        {
            var reason = runtime.Status.ReasonCode;
            runtime.Dispose();
            await TerminateFailClosedAsync(reason);
            return;
        }

        _runtime = runtime;
        _shell.AttachAuthenticatedRuntime(runtime);
        var supervisorCancellation = new CancellationTokenSource();
        _supervisorCancellation = supervisorCancellation;
        var supervisorToken = supervisorCancellation.Token;
        // Activation is delivered on the WinForms thread. Keep the long-running supervisor off
        // that SynchronizationContext so normal WM_CLOSE can synchronously cancel and join it
        // after Application.Run returns, without waiting for a continuation queued to the stopped
        // UI message loop. Do not use the supervisor token as Task.Run's scheduling token: once
        // published, the delegate must run and settle the owned lifetime even if close races it.
        _supervisor = Task.Run(() => RunSupervisorAsync(runtime, supervisorToken));
    }

    private async Task EndAuthenticatedSessionAsync(Guid sessionId)
    {
        if (_shutdownStarted)
            return;
        if (!_activationAttempted || sessionId == Guid.Empty || _activeSessionId != sessionId)
        {
            await TerminateFailClosedAsync("DESKTOP_SESSION_END_DENIED");
            return;
        }
        await TerminateFailClosedAsync("DESKTOP_SESSION_ENDED");
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _shutdownStarted = true;
            _applicationCancellation.Cancel();
            _activationCancellation?.Cancel();
            _supervisorCancellation?.Cancel();
            WaitForSupervisor();
            _authenticatedSessions.SessionAuthenticated -= OnSessionAuthenticated;
            _authenticatedSessions.SessionEnded -= OnSessionEnded;
            _authenticatedSessions.Dispose();
            _activationCancellation?.Dispose();
            _supervisorCancellation?.Dispose();
            _runtime?.Dispose();
            _applicationCancellation.Dispose();
            _shell.Dispose();
        }
        base.Dispose(disposing);
    }

    private async Task RunSupervisorAsync(DesktopOfflineRuntime runtime, CancellationToken cancellationToken)
    {
        try
        {
            await runtime.RunSyncSupervisorAsync(cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            // Expected during application shutdown.
        }
        catch
        {
            _shell.ReportSupervisorStopped();
        }
    }

    private bool IsGovernedActivation(DesktopAuthenticatedOfflineActivation activation) =>
        activation.SessionId != Guid.Empty &&
        activation.SessionExpiresAt > DateTimeOffset.UtcNow &&
        activation.OfflineRuntimeAuthorized &&
        activation.MeasuredBuildIdentity is { IsValid: true } &&
        activation.AuthenticatedScope.IsComplete &&
        activation.AuthorizedOfflineScope.IsComplete &&
        activation.AuthenticatedScope == activation.AuthorizedOfflineScope &&
        activation.CreateRuntimeAsync is not null;

    private async Task TerminateFailClosedAsync(string reasonCode)
    {
        if (_shutdownStarted)
            return;
        _shutdownStarted = true;
        _applicationCancellation.Cancel();
        _activationCancellation?.Cancel();
        _supervisorCancellation?.Cancel();
        try
        {
            if (_supervisor is not null)
                await _supervisor;
        }
        catch (OperationCanceledException)
        {
            // Expected for logout, revocation, expiry and fail-closed shutdown.
        }

        _runtime?.Dispose();
        _runtime = null;
        _shell.CloseForSessionEnd(reasonCode);
    }

    private void RunOnUiThread(Func<Task> operation)
    {
        if (_shell.IsDisposed || _shutdownStarted)
            return;
        if (_shell.InvokeRequired)
        {
            _shell.BeginInvoke((Action)(() => _ = RunObservedAsync(operation)));
            return;
        }
        _ = RunObservedAsync(operation);
    }

    private async Task RunObservedAsync(Func<Task> operation)
    {
        try
        {
            await operation();
        }
        catch
        {
            await TerminateFailClosedAsync("DESKTOP_SESSION_RUNTIME_FAILURE");
        }
    }

    private void WaitForSupervisor()
    {
        try
        {
            _supervisor?.GetAwaiter().GetResult();
        }
        catch (OperationCanceledException)
        {
            // Expected during application shutdown.
        }
    }
}
