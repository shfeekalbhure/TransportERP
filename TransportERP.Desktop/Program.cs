using TransportERP.Desktop.Application;
using TransportERP.Desktop.Offline;

namespace TransportERP.Desktop;

internal static class Program
{
    [STAThread]
    private static int Main(string[] args)
    {
        if (args.Contains("--startup-smoke", StringComparer.Ordinal))
            return DesktopStartupContractProbe.VerifyClosedDefault() ? 0 : 1;
        if (args.Contains("--runtime-platform-smoke", StringComparer.Ordinal))
            return DesktopRuntimePlatformProbe.RunAsync().GetAwaiter().GetResult() ? 0 : 1;

        ApplicationConfiguration.Initialize();
        // The producer calls the governed HTTPS sign-in/device/sync authorization APIs. It starts
        // closed: no network, local database, DPAPI material or signing key is opened by startup.
        using var authenticatedSessions = new DesktopOnlineSignInSessionBridge(
            new DesktopOnlineSessionAuthenticator());
        using var context = new DesktopApplicationContext(authenticatedSessions);
        System.Windows.Forms.Application.Run(context);
        return 0;
    }
}
