using TransportERP.Desktop.Application;

namespace TransportERP.Desktop;

internal static class Program
{
    [STAThread]
    private static int Main(string[] args)
    {
        if (args.Contains("--startup-smoke", StringComparer.Ordinal))
            return DesktopStartupContractProbe.VerifyClosedDefault() ? 0 : 1;

        ApplicationConfiguration.Initialize();
        // This bridge is the secret-free, typed handoff used by the online sign-in host. It starts
        // closed: no runtime factory is invoked until that host publishes an authenticated grant.
        using var authenticatedSessions = new DesktopOnlineSignInSessionBridge();
        using var context = new DesktopApplicationContext(authenticatedSessions);
        System.Windows.Forms.Application.Run(context);
        return 0;
    }
}
