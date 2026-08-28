using System.Text.Json;
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
            return DesktopRuntimePlatformProbe.RunAsync().GetAwaiter().GetResult();
        var buildIdentityOutput = ExplicitBuildIdentityOutput(args);
        if (buildIdentityOutput is not null)
            return WriteBuildIdentity(buildIdentityOutput);

        ApplicationConfiguration.Initialize();
        // The producer calls the governed HTTPS sign-in/device/sync authorization APIs. It starts
        // closed: no network, local database, DPAPI material or signing key is opened by startup.
        using var authenticatedSessions = new DesktopOnlineSignInSessionBridge(
            new DesktopOnlineSessionAuthenticator());
        using var context = new DesktopApplicationContext(authenticatedSessions);
        System.Windows.Forms.Application.Run(context);
        return 0;
    }

    private static string? ExplicitBuildIdentityOutput(string[] args)
    {
        var index = Array.FindIndex(args,
            value => string.Equals(value, "--print-build-identity", StringComparison.Ordinal));
        if (index < 0) return null;
        if (args.Length != 2 || index != 0 || string.IsNullOrWhiteSpace(args[1]) ||
            !Path.IsPathFullyQualified(args[1]))
            throw new InvalidOperationException("BUILD_IDENTITY_OUTPUT_INVALID");
        return Path.GetFullPath(args[1]);
    }

    private static int WriteBuildIdentity(string outputPath)
    {
        var identity = DesktopBuildIdentityProbe.Measure();
        if (!identity.IsValid) return 2;
        var directory = Path.GetDirectoryName(outputPath);
        if (string.IsNullOrEmpty(directory) || !Directory.Exists(directory)) return 3;
        var json = JsonSerializer.Serialize(identity, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        using var stream = new FileStream(outputPath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
        using var writer = new StreamWriter(stream, new System.Text.UTF8Encoding(false));
        writer.Write(json);
        return 0;
    }
}
