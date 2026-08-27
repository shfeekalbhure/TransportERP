using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace TransportERP.Desktop.E2ETests;

/// <summary>Runs the normal API entry point on the exact HTTPS origin embedded in the client.</summary>
internal sealed class DesktopReleaseKestrelApiHost : IAsyncDisposable
{
    private readonly Process _process;
    private readonly Task _stdoutDrain;
    private readonly Task _stderrDrain;
    private readonly X509Certificate2 _certificate;
    private readonly string _temporaryRoot;

    private DesktopReleaseKestrelApiHost(
        Process process,
        Task stdoutDrain,
        Task stderrDrain,
        X509Certificate2 certificate,
        string temporaryRoot)
    {
        _process = process;
        _stdoutDrain = stdoutDrain;
        _stderrDrain = stderrDrain;
        _certificate = certificate;
        _temporaryRoot = temporaryRoot;
    }

    internal static async Task<DesktopReleaseKestrelApiHost> StartAsync(
        Uri origin,
        IReadOnlyDictionary<string, string> configuration,
        CancellationToken cancellationToken)
    {
        Checkpoint("HOST_START");
        if (!OperatingSystem.IsWindows() || origin.Scheme != Uri.UriSchemeHttps ||
            origin.AbsolutePath != "/" || origin.IsDefaultPort)
            throw new InvalidOperationException("DESKTOP_E2E_EMBEDDED_ORIGIN_MUST_BE_EXACT_HTTPS_LOOPBACK");
        Checkpoint("HOST_LOOPBACK_CHECK_STARTED");
        if (!await ResolvesOnlyToLoopbackAsync(origin))
            throw new InvalidOperationException("DESKTOP_E2E_EMBEDDED_ORIGIN_MUST_BE_EXACT_HTTPS_LOOPBACK");
        Checkpoint("HOST_LOOPBACK_CHECK_COMPLETED");
        Checkpoint("HOST_ORIGIN_VALIDATED");

        var apiAssembly = Path.Combine(AppContext.BaseDirectory, "TransportERP.Api.dll");
        var runtimeConfig = Path.Combine(AppContext.BaseDirectory,
            "TransportERP.Desktop.E2ETests.runtimeconfig.json");
        var dependencyManifest = Path.Combine(AppContext.BaseDirectory,
            "TransportERP.Desktop.E2ETests.deps.json");
        if (!File.Exists(apiAssembly) || !File.Exists(runtimeConfig) || !File.Exists(dependencyManifest))
            throw new InvalidOperationException("DESKTOP_E2E_API_ENTRY_POINT_UNAVAILABLE");
        Checkpoint("HOST_ENTRYPOINT_VALIDATED");

        var temporaryRoot = Path.Combine(
            Path.GetTempPath(), "transporterp-desktop-kestrel", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(temporaryRoot);
        var pfxPath = Path.Combine(temporaryRoot, "kestrel.pfx");
        var pfxPassword = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        Checkpoint("HOST_CERTIFICATE_CREATE_STARTED");
        var certificate = CreateServerCertificate(origin.Host);
        Checkpoint("HOST_CERTIFICATE_CREATED");
        await File.WriteAllBytesAsync(
            pfxPath, certificate.Export(X509ContentType.Pfx, pfxPassword), cancellationToken);
        Checkpoint("HOST_CERTIFICATE_WRITTEN");
        Checkpoint("HOST_CERTIFICATE_TRUST_STARTED");
        using (var root = new X509Store(StoreName.Root, StoreLocation.CurrentUser))
        {
            root.Open(OpenFlags.ReadWrite);
            using var publicCertificate = new X509Certificate2(
                certificate.Export(X509ContentType.Cert));
            root.Add(publicCertificate);
        }
        Checkpoint("HOST_CERTIFICATE_TRUSTED");

        var start = new ProcessStartInfo
        {
            FileName = "dotnet",
            WorkingDirectory = AppContext.BaseDirectory,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        start.ArgumentList.Add("exec");
        start.ArgumentList.Add("--runtimeconfig");
        start.ArgumentList.Add(runtimeConfig);
        start.ArgumentList.Add("--depsfile");
        start.ArgumentList.Add(dependencyManifest);
        start.ArgumentList.Add(apiAssembly);
        start.Environment["ASPNETCORE_URLS"] = origin.GetLeftPart(UriPartial.Authority);
        start.Environment["ASPNETCORE_ENVIRONMENT"] = "Production";
        start.Environment["ASPNETCORE_Kestrel__Certificates__Default__Path"] = pfxPath;
        start.Environment["ASPNETCORE_Kestrel__Certificates__Default__Password"] = pfxPassword;
        foreach (var setting in configuration)
            start.Environment[setting.Key] = setting.Value;

        Process process;
        try
        {
            Checkpoint("HOST_PROCESS_START_STARTED");
            process = Process.Start(start)
                ?? throw new InvalidOperationException("DESKTOP_E2E_API_START_FAILED");
            Checkpoint("HOST_PROCESS_STARTED");
        }
        catch
        {
            RemoveTrustedCertificate(certificate.Thumbprint);
            certificate.Dispose();
            DeleteTemporaryRoot(temporaryRoot);
            throw;
        }
        // Drain both streams continuously. The test never forwards process logs because they are
        // not needed as acceptance evidence and may contain environment-specific paths.
        var stdoutDrain = DrainAsync(process.StandardOutput, cancellationToken);
        var stderrDrain = DrainAsync(process.StandardError, cancellationToken);
        Checkpoint("HOST_DRAINS_STARTED");
        var host = new DesktopReleaseKestrelApiHost(
            process, stdoutDrain, stderrDrain, certificate, temporaryRoot);
        try
        {
            Checkpoint("HOST_WAIT_READY");
            await host.WaitForReadyAsync(origin, cancellationToken);
            Checkpoint("HOST_READY");
            return host;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            Checkpoint("HOST_START_CANCELLED");
            await host.DisposeAsync();
            throw;
        }
        catch
        {
            Checkpoint("HOST_START_FAILED");
            await host.DisposeAsync();
            throw;
        }
    }

    private async Task WaitForReadyAsync(Uri origin, CancellationToken cancellationToken)
    {
        using var client = new HttpClient { BaseAddress = origin, Timeout = TimeSpan.FromSeconds(2) };
        while (!_process.HasExited)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/sync/activation");
                using var response = await client.SendAsync(request, cancellationToken);
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Checkpoint("HOST_ACTIVATION_REACHABLE");
                    return;
                }
            }
            catch (HttpRequestException) { }
            catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested) { }
            await Task.Delay(100, cancellationToken);
        }
        throw new InvalidOperationException($"DESKTOP_E2E_API_EXITED_{_process.ExitCode}");
    }

    private static async Task<bool> ResolvesOnlyToLoopbackAsync(Uri origin)
    {
        try
        {
            var addresses = await Dns.GetHostAddressesAsync(origin.IdnHost);
            return addresses.Length > 0 && addresses.All(IPAddress.IsLoopback);
        }
        catch (Exception exception) when (exception is System.Net.Sockets.SocketException or ArgumentException)
        {
            return false;
        }
    }

    private static async Task DrainAsync(StreamReader reader, CancellationToken cancellationToken)
    {
        while (await reader.ReadLineAsync(cancellationToken) is not null) { }
    }

    private static X509Certificate2 CreateServerCertificate(string host)
    {
        using var key = RSA.Create(2048);
        var request = new CertificateRequest(
            $"CN={host}", key, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        var alternativeNames = new SubjectAlternativeNameBuilder();
        if (IPAddress.TryParse(host, out var address)) alternativeNames.AddIpAddress(address);
        else alternativeNames.AddDnsName(host);
        request.CertificateExtensions.Add(alternativeNames.Build());
        request.CertificateExtensions.Add(new X509BasicConstraintsExtension(true, false, 0, true));
        request.CertificateExtensions.Add(new X509KeyUsageExtension(
            X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.KeyCertSign, true));
        request.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(
            new OidCollection { new Oid("1.3.6.1.5.5.7.3.1") }, false));
        return request.CreateSelfSigned(
            DateTimeOffset.UtcNow.AddMinutes(-5), DateTimeOffset.UtcNow.AddHours(2));
    }

    public async ValueTask DisposeAsync()
    {
        Checkpoint("HOST_DISPOSE_STARTED");
        if (!_process.HasExited)
        {
            Checkpoint("HOST_KILL_STARTED");
            _process.Kill(entireProcessTree: true);
            Checkpoint("HOST_KILL_RETURNED");
            await _process.WaitForExitAsync();
            Checkpoint("HOST_PROCESS_EXITED_AFTER_KILL");
        }
        Checkpoint("HOST_DRAIN_WAIT_STARTED");
        try { await Task.WhenAll(_stdoutDrain, _stderrDrain); }
        catch (OperationCanceledException) { Checkpoint("HOST_DRAINS_CANCELLED"); }
        Checkpoint("HOST_DRAIN_WAIT_COMPLETED");
        _process.Dispose();
        Checkpoint("HOST_PROCESS_DISPOSED");
        Checkpoint("HOST_CERTIFICATE_REMOVE_STARTED");
        try
        {
            RemoveTrustedCertificate(_certificate.Thumbprint);
            Checkpoint("HOST_CERTIFICATE_REMOVED");
        }
        finally
        {
            _certificate.Dispose();
            DeleteTemporaryRoot(_temporaryRoot);
            Checkpoint("HOST_TEMP_ROOT_DELETE_RETURNED");
        }
        Checkpoint("HOST_DISPOSE_COMPLETED");
    }

    private static void Checkpoint(string name)
    {
        var checkpointFile = Environment.GetEnvironmentVariable(
            "TRANSPORTERP_DESKTOP_E2E_CHECKPOINT_FILE");
        if (string.IsNullOrWhiteSpace(checkpointFile) ||
            !Path.IsPathFullyQualified(checkpointFile))
            return;
        try
        {
            using var stream = new FileStream(
                checkpointFile, FileMode.Append, FileAccess.Write, FileShare.Read,
                bufferSize: 4096, options: FileOptions.WriteThrough);
            using var writer = new StreamWriter(stream, new UTF8Encoding(false));
            writer.WriteLine(name);
            writer.Flush();
            stream.Flush(flushToDisk: true);
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            // Checkpoints must never prevent release-host cleanup.
        }
    }


    private static void RemoveTrustedCertificate(string thumbprint)
    {
        using var root = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
        root.Open(OpenFlags.ReadWrite);
        foreach (var match in root.Certificates.Find(
                     X509FindType.FindByThumbprint, thumbprint, validOnly: false))
            root.Remove(match);
    }

    private static void DeleteTemporaryRoot(string path)
    {
        try { Directory.Delete(path, recursive: true); }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException) { }
    }
}
