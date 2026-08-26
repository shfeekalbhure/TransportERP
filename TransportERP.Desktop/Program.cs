using TransportERP.Desktop.Application;

namespace TransportERP.Desktop;

internal static class Program
{
    [STAThread]
    private static int Main(string[] args)
    {
        if (args.Contains("--startup-smoke", StringComparer.Ordinal))
            return DesktopStartupPolicy.OfflineRuntimeAuthorizedByDefault ? 1 : 0;

        ApplicationConfiguration.Initialize();
        using var context = new DesktopApplicationContext();
        System.Windows.Forms.Application.Run(context);
        return 0;
    }
}
