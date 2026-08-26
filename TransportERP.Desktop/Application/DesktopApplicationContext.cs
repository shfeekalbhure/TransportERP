using TransportERP.Desktop.Offline;

namespace TransportERP.Desktop.Application;

internal static class DesktopStartupPolicy
{
    // Owner authority is required before a host can supply an authorized runtime.
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
    private CancellationTokenSource? _supervisorCancellation;
    private Task? _supervisor;
    private DesktopOfflineRuntime? _runtime;

    internal DesktopApplicationContext()
    {
        _shell = new DesktopShellForm();
        _shell.FormClosed += (_, _) => ExitThread();
        MainForm = _shell;
        _shell.Show();
    }

    internal void ActivateAuthenticatedOfflineRuntime(DesktopOfflineRuntime runtime)
    {
        ArgumentNullException.ThrowIfNull(runtime);
        if (_runtime is not null)
            throw new InvalidOperationException("DESKTOP_OFFLINE_RUNTIME_ALREADY_ACTIVE");
        if (runtime.Status.Mode != DesktopOfflineRuntimeMode.Ready)
            throw new InvalidOperationException(runtime.Status.ReasonCode);

        _runtime = runtime;
        _shell.AttachAuthenticatedRuntime(runtime);
        _supervisorCancellation = new CancellationTokenSource();
        _supervisor = RunSupervisorAsync(runtime, _supervisorCancellation.Token);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _supervisorCancellation?.Cancel();
            try
            {
                _supervisor?.GetAwaiter().GetResult();
            }
            catch (OperationCanceledException)
            {
                // Expected during application shutdown.
            }
            _supervisorCancellation?.Dispose();
            _runtime?.Dispose();
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
}
