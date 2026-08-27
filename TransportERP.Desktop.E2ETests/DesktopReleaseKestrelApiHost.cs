using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;

namespace TransportERP.Desktop.E2ETests;

/// <summary>Runs the normal API entry point on the exact HTTPS origin embedded in the client.</summary>
internal sealed class DesktopReleaseKestrelApiHost : IAsyncDisposable
{
    private readonly Process _process;
    private readonly Task _stdoutDrain;
    private readonly Task _stderrDrain;
    private readonly CancellationTokenSource _drainCancellation;
    private readonly StartupFailureClassifier _startupFailureClassifier;
    private readonly X509Certificate2 _certificate;
    private readonly string _temporaryRoot;

    private DesktopReleaseKestrelApiHost(
        Process process,
        Task stdoutDrain,
        Task stderrDrain,
        CancellationTokenSource drainCancellation,
        StartupFailureClassifier startupFailureClassifier,
        X509Certificate2 certificate,
        string temporaryRoot)
    {
        _process = process;
        _stdoutDrain = stdoutDrain;
        _stderrDrain = stderrDrain;
        _drainCancellation = drainCancellation;
        _startupFailureClassifier = startupFailureClassifier;
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
        EnsureEphemeralHostedMachineTrustAuthority();
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
        var publicCertificatePath = Path.Combine(temporaryRoot, "kestrel.cer");
        var pfxPassword = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        Checkpoint("HOST_CERTIFICATE_CREATE_STARTED");
        var certificate = CreateServerCertificate(origin.Host);
        var trustMutationMayHaveOccurred = false;
        Checkpoint("HOST_CERTIFICATE_CREATED");
        try
        {
            await File.WriteAllBytesAsync(
                pfxPath, certificate.Export(X509ContentType.Pfx, pfxPassword), cancellationToken);
            await File.WriteAllBytesAsync(
                publicCertificatePath, certificate.Export(X509ContentType.Cert), cancellationToken);
            Checkpoint("HOST_CERTIFICATE_WRITTEN");
            Checkpoint("HOST_CERTIFICATE_TRUST_STARTED");
            VerifyCertificateAbsent(certificate);
            Checkpoint("HOST_CERTIFICATE_ABSENCE_VERIFIED");
            Checkpoint("HOST_CERTIFICATE_IMPORT_STARTED");
            trustMutationMayHaveOccurred = true;
            await RunCertificateStoreCommandAsync(
                ["-addstore", "Root", publicCertificatePath],
                cancellationToken);
            Checkpoint("HOST_CERTIFICATE_IMPORT_RETURNED");
            VerifyTrustedCertificate(certificate);
            Checkpoint("HOST_CERTIFICATE_IMPORT_VERIFIED");
        }
        catch
        {
            try
            {
                if (trustMutationMayHaveOccurred)
                    await RemoveTrustedCertificateAsync(certificate);
            }
            finally
            {
                certificate.Dispose();
                DeleteTemporaryRoot(temporaryRoot);
            }
            throw;
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
            try { await RemoveTrustedCertificateAsync(certificate); }
            finally
            {
                certificate.Dispose();
                DeleteTemporaryRoot(temporaryRoot);
            }
            throw;
        }
        // Drain both streams continuously. The test never forwards process logs because they are
        // not needed as acceptance evidence and may contain environment-specific paths.
        var drainCancellation = new CancellationTokenSource();
        var startupFailureClassifier = new StartupFailureClassifier();
        var stdoutDrain = DrainAsync(process.StandardOutput, drainCancellation.Token);
        var stderrDrain = DrainAsync(
            process.StandardError, startupFailureClassifier.Observe, drainCancellation.Token);
        Checkpoint("HOST_DRAINS_STARTED");
        var host = new DesktopReleaseKestrelApiHost(
            process, stdoutDrain, stderrDrain, drainCancellation, startupFailureClassifier,
            certificate, temporaryRoot);
        try
        {
            Checkpoint("HOST_WAIT_READY");
            await host.WaitForReadyAsync(origin, cancellationToken);
            Checkpoint("HOST_READY");
            Checkpoint("HOST_HOSTNAME_NEGATIVE_STARTED");
            await VerifyHostnameMismatchRejectedAsync(origin, cancellationToken);
            Checkpoint("HOST_HOSTNAME_NEGATIVE_REJECTED");
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
        cancellationToken.ThrowIfCancellationRequested();
        var startupFailureCode = await GetStartupFailureCodeAsync(cancellationToken);
        throw new InvalidOperationException(
            $"DESKTOP_E2E_API_EXITED_{_process.ExitCode}_{startupFailureCode}");
    }

    private async Task<string> GetStartupFailureCodeAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _stderrDrain.WaitAsync(TimeSpan.FromSeconds(2), cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch { return "DESKTOP_E2E_API_STARTUP_UNCLASSIFIED"; }
        return _startupFailureClassifier.Code;
    }

    private static async Task VerifyHostnameMismatchRejectedAsync(
        Uri origin,
        CancellationToken cancellationToken)
    {
        if (!string.Equals(origin.Host, "127.0.0.1", StringComparison.Ordinal))
            throw new InvalidOperationException("DESKTOP_E2E_TLS_HOSTNAME_NEGATIVE_UNAVAILABLE");

        using var tcp = new TcpClient();
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeout.CancelAfter(TimeSpan.FromSeconds(5));
        await tcp.ConnectAsync(origin.Host, origin.Port, timeout.Token);
        await using var tls = new SslStream(tcp.GetStream(), leaveInnerStreamOpen: false);
        try
        {
            await tls.AuthenticateAsClientAsync(
                new SslClientAuthenticationOptions { TargetHost = "localhost" },
                timeout.Token);
        }
        catch (AuthenticationException)
        {
            return;
        }

        throw new InvalidOperationException("DESKTOP_E2E_TLS_HOSTNAME_VALIDATION_BYPASSED");
    }

    private static void EnsureEphemeralHostedMachineTrustAuthority()
    {
        using var identity = WindowsIdentity.GetCurrent();
        var principal = new WindowsPrincipal(identity);
        if (!string.Equals(Environment.GetEnvironmentVariable("GITHUB_ACTIONS"), "true",
                StringComparison.Ordinal) ||
            !string.Equals(Environment.GetEnvironmentVariable("RUNNER_ENVIRONMENT"), "github-hosted",
                StringComparison.Ordinal) ||
            !principal.IsInRole(WindowsBuiltInRole.Administrator))
            throw new InvalidOperationException("DESKTOP_E2E_MACHINE_TRUST_AUTHORITY_DENIED");
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

    private static async Task DrainAsync(
        StreamReader reader,
        Action<string> observe,
        CancellationToken cancellationToken)
    {
        while (await reader.ReadLineAsync(cancellationToken) is { } line)
            observe(line);
    }

    internal sealed class StartupFailureClassifier
    {
        private int _kind;

        internal string Code => (StartupFailureKind)Volatile.Read(ref _kind) switch
        {
            StartupFailureKind.SyncImplementationMismatch =>
                "DESKTOP_E2E_API_STARTUP_SYNC_IMPLEMENTATION_MISMATCH",
            StartupFailureKind.SyncAuthorizedBuildInvalid =>
                "DESKTOP_E2E_API_STARTUP_SYNC_AUTHORIZED_BUILD_INVALID",
            StartupFailureKind.SyncRuntimePolicyValidation =>
                "DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_VALIDATION",
            StartupFailureKind.EffectivePolicyValidation =>
                "DESKTOP_E2E_API_STARTUP_EFFECTIVE_POLICY_VALIDATION",
            StartupFailureKind.AuthValidation =>
                "DESKTOP_E2E_API_STARTUP_AUTH_VALIDATION",
            StartupFailureKind.OptionsValidation => "DESKTOP_E2E_API_STARTUP_OPTIONS_VALIDATION",
            StartupFailureKind.InvalidOperation => "DESKTOP_E2E_API_STARTUP_INVALID_OPERATION",
            StartupFailureKind.TypeInitialization => "DESKTOP_E2E_API_STARTUP_TYPE_INITIALIZATION",
            StartupFailureKind.Cryptographic => "DESKTOP_E2E_API_STARTUP_CRYPTOGRAPHIC",
            StartupFailureKind.Io => "DESKTOP_E2E_API_STARTUP_IO",
            StartupFailureKind.UnauthorizedAccess => "DESKTOP_E2E_API_STARTUP_UNAUTHORIZED_ACCESS",
            StartupFailureKind.Socket => "DESKTOP_E2E_API_STARTUP_SOCKET",
            StartupFailureKind.PostgreSql => "DESKTOP_E2E_API_STARTUP_POSTGRESQL",
            StartupFailureKind.Argument => "DESKTOP_E2E_API_STARTUP_ARGUMENT",
            StartupFailureKind.Format => "DESKTOP_E2E_API_STARTUP_FORMAT",
            StartupFailureKind.FileNotFound => "DESKTOP_E2E_API_STARTUP_FILE_NOT_FOUND",
            _ => "DESKTOP_E2E_API_STARTUP_UNCLASSIFIED"
        };

        internal void Observe(string line)
        {
            var kind = ClassifyTopLevel(line);
            if (kind != StartupFailureKind.Unobserved)
                Interlocked.CompareExchange(ref _kind, (int)kind, 0);
        }

        private static StartupFailureKind ClassifyTopLevel(string line)
        {
            const string prefix = "Unhandled exception. ";
            if (!line.StartsWith(prefix, StringComparison.Ordinal))
                return StartupFailureKind.Unobserved;
            if (string.Equals(
                    line,
                    "Unhandled exception. TransportERP.Api.Startup.SyncRuntimePolicyStartupOptionsValidationException: Sync:Offline:ActivationImplementationSha must match the exact running API build.",
                    StringComparison.Ordinal))
                return StartupFailureKind.SyncImplementationMismatch;
            if (string.Equals(
                    line,
                    "Unhandled exception. TransportERP.Api.Startup.SyncRuntimePolicyStartupOptionsValidationException: Sync:Offline:AuthorizedBuilds must contain one valid exact identity per approved platform.",
                    StringComparison.Ordinal))
                return StartupFailureKind.SyncAuthorizedBuildInvalid;
            var exception = line.AsSpan(prefix.Length);
            if (exception.StartsWith(
                    "TransportERP.Api.Startup.SyncRuntimePolicyStartupOptionsValidationException:"
                        .AsSpan(),
                    StringComparison.Ordinal)) return StartupFailureKind.SyncRuntimePolicyValidation;
            if (exception.StartsWith(
                    "TransportERP.Api.Startup.EffectivePolicyStartupOptionsValidationException:"
                        .AsSpan(),
                    StringComparison.Ordinal)) return StartupFailureKind.EffectivePolicyValidation;
            if (exception.StartsWith(
                    "TransportERP.Api.Startup.AuthStartupOptionsValidationException:".AsSpan(),
                    StringComparison.Ordinal)) return StartupFailureKind.AuthValidation;
            if (exception.StartsWith(
                    "Microsoft.Extensions.Options.OptionsValidationException:".AsSpan(),
                    StringComparison.Ordinal)) return StartupFailureKind.OptionsValidation;
            if (exception.StartsWith("System.InvalidOperationException:".AsSpan(),
                    StringComparison.Ordinal)) return StartupFailureKind.InvalidOperation;
            if (exception.StartsWith("System.TypeInitializationException:".AsSpan(),
                    StringComparison.Ordinal)) return StartupFailureKind.TypeInitialization;
            if (exception.StartsWith(
                    "System.Security.Cryptography.CryptographicException:".AsSpan(),
                    StringComparison.Ordinal)) return StartupFailureKind.Cryptographic;
            if (exception.StartsWith("System.IO.FileNotFoundException:".AsSpan(),
                    StringComparison.Ordinal)) return StartupFailureKind.FileNotFound;
            if (exception.StartsWith("System.IO.IOException:".AsSpan(),
                    StringComparison.Ordinal)) return StartupFailureKind.Io;
            if (exception.StartsWith("System.UnauthorizedAccessException:".AsSpan(),
                    StringComparison.Ordinal)) return StartupFailureKind.UnauthorizedAccess;
            if (exception.StartsWith("System.Net.Sockets.SocketException:".AsSpan(),
                    StringComparison.Ordinal)) return StartupFailureKind.Socket;
            if (exception.StartsWith("Npgsql.NpgsqlException:".AsSpan(),
                    StringComparison.Ordinal) ||
                exception.StartsWith("Npgsql.PostgresException:".AsSpan(),
                    StringComparison.Ordinal)) return StartupFailureKind.PostgreSql;
            if (exception.StartsWith("System.ArgumentException:".AsSpan(),
                    StringComparison.Ordinal)) return StartupFailureKind.Argument;
            if (exception.StartsWith("System.FormatException:".AsSpan(),
                    StringComparison.Ordinal)) return StartupFailureKind.Format;
            return StartupFailureKind.ObservedUnclassified;
        }

        private enum StartupFailureKind
        {
            Unobserved,
            ObservedUnclassified,
            SyncImplementationMismatch,
            SyncAuthorizedBuildInvalid,
            SyncRuntimePolicyValidation,
            EffectivePolicyValidation,
            AuthValidation,
            OptionsValidation,
            InvalidOperation,
            TypeInitialization,
            Cryptographic,
            Io,
            UnauthorizedAccess,
            Socket,
            PostgreSql,
            Argument,
            Format,
            FileNotFound
        }
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
        string? cleanupFailureCode = null;
        Checkpoint("HOST_DISPOSE_STARTED");
        if (!_process.HasExited)
        {
            Checkpoint("HOST_KILL_STARTED");
            if (await TerminateProcessBoundedAsync(_process))
            {
                Checkpoint("HOST_KILL_RETURNED");
                Checkpoint("HOST_PROCESS_EXITED_AFTER_KILL");
            }
            else
            {
                cleanupFailureCode = "DESKTOP_E2E_API_TERMINATION_FAILED";
            }
        }
        Checkpoint("HOST_DRAIN_WAIT_STARTED");
        if (await CompleteDrainsBoundedAsync(
                _stdoutDrain, _stderrDrain, _drainCancellation))
            Checkpoint("HOST_DRAIN_WAIT_COMPLETED");
        else
        {
            Checkpoint("HOST_DRAINS_CANCELLED");
            cleanupFailureCode ??= "DESKTOP_E2E_API_DRAIN_FAILED";
        }
        _drainCancellation.Dispose();
        _process.Dispose();
        Checkpoint("HOST_PROCESS_DISPOSED");
        Checkpoint("HOST_CERTIFICATE_REMOVE_STARTED");
        try
        {
            try
            {
                await RemoveTrustedCertificateAsync(_certificate);
                Checkpoint("HOST_CERTIFICATE_REMOVED");
            }
            catch
            {
                cleanupFailureCode ??= "DESKTOP_E2E_CERTIFICATE_REMOVAL_FAILED";
            }
        }
        finally
        {
            _certificate.Dispose();
            DeleteTemporaryRoot(_temporaryRoot);
            Checkpoint("HOST_TEMP_ROOT_DELETE_RETURNED");
        }
        Checkpoint("HOST_DISPOSE_COMPLETED");
        if (cleanupFailureCode is not null)
            throw new InvalidOperationException(cleanupFailureCode);
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


    private static async Task RunCertificateStoreCommandAsync(
        IReadOnlyList<string> arguments,
        CancellationToken cancellationToken)
    {
        var windowsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
        var certutilPath = Path.Combine(windowsDirectory, "System32", "certutil.exe");
        if (!Path.IsPathFullyQualified(certutilPath) || !File.Exists(certutilPath))
            throw new InvalidOperationException("DESKTOP_E2E_CERTIFICATE_STORE_TOOL_UNAVAILABLE");

        var start = new ProcessStartInfo
        {
            FileName = certutilPath,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        foreach (var argument in arguments) start.ArgumentList.Add(argument);

        using var process = Process.Start(start)
            ?? throw new InvalidOperationException("DESKTOP_E2E_CERTIFICATE_STORE_TOOL_START_FAILED");
        process.StandardInput.Close();
        using var drainCancellation = new CancellationTokenSource();
        var stdoutDrain = DrainAsync(process.StandardOutput, drainCancellation.Token);
        var stderrDrain = DrainAsync(process.StandardError, drainCancellation.Token);
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeout.CancelAfter(TimeSpan.FromSeconds(20));
        var callerCancelled = false;
        string? failureCode = null;
        try
        {
            await process.WaitForExitAsync(timeout.Token);
        }
        catch (OperationCanceledException)
        {
            callerCancelled = cancellationToken.IsCancellationRequested;
            if (!await TerminateProcessBoundedAsync(process))
                failureCode = "DESKTOP_E2E_CERTIFICATE_STORE_TOOL_TERMINATION_FAILED";
            else if (!callerCancelled)
                failureCode = "DESKTOP_E2E_CERTIFICATE_STORE_TOOL_TIMEOUT";
        }
        catch
        {
            failureCode = "DESKTOP_E2E_CERTIFICATE_STORE_TOOL_WAIT_FAILED";
            if (!await TerminateProcessBoundedAsync(process))
                failureCode = "DESKTOP_E2E_CERTIFICATE_STORE_TOOL_TERMINATION_FAILED";
        }
        if (!await CompleteDrainsBoundedAsync(
                stdoutDrain, stderrDrain, drainCancellation))
            failureCode ??= "DESKTOP_E2E_CERTIFICATE_STORE_TOOL_DRAIN_FAILED";
        if (failureCode is not null)
            throw new InvalidOperationException(failureCode);
        if (callerCancelled)
            cancellationToken.ThrowIfCancellationRequested();
        if (process.ExitCode != 0)
            throw new InvalidOperationException("DESKTOP_E2E_CERTIFICATE_STORE_TOOL_FAILED");
    }

    private static async Task<bool> TerminateProcessBoundedAsync(Process process)
    {
        try
        {
            if (!process.HasExited) process.Kill(entireProcessTree: true);
        }
        catch (InvalidOperationException) when (process.HasExited) { }
        catch { return false; }

        using var termination = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        try
        {
            await process.WaitForExitAsync(termination.Token);
            return true;
        }
        catch { return false; }
    }

    private static async Task<bool> CompleteDrainsBoundedAsync(
        Task stdoutDrain,
        Task stderrDrain,
        CancellationTokenSource drainCancellation)
    {
        var drains = Task.WhenAll(stdoutDrain, stderrDrain);
        try
        {
            await drains.WaitAsync(TimeSpan.FromSeconds(5));
            return true;
        }
        catch
        {
            drainCancellation.Cancel();
            try { await drains.WaitAsync(TimeSpan.FromSeconds(2)); }
            catch { }
            return false;
        }
    }

    private static void VerifyTrustedCertificate(X509Certificate2 certificate)
    {
        using var root = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
        root.Open(OpenFlags.ReadOnly);
        var matches = root.Certificates.Find(
            X509FindType.FindByThumbprint, certificate.Thumbprint, validOnly: false);
        try
        {
            if (matches.Count != 1 || matches[0].HasPrivateKey ||
                !matches[0].RawData.AsSpan().SequenceEqual(certificate.RawData))
                throw new InvalidOperationException("DESKTOP_E2E_CERTIFICATE_IMPORT_VERIFICATION_FAILED");
        }
        finally
        {
            foreach (var match in matches) match.Dispose();
        }
    }

    private static void VerifyCertificateAbsent(X509Certificate2 certificate)
    {
        using var root = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
        root.Open(OpenFlags.ReadOnly);
        var matches = root.Certificates.Find(
            X509FindType.FindByThumbprint, certificate.Thumbprint, validOnly: false);
        try
        {
            if (matches.Count != 0)
                throw new InvalidOperationException("DESKTOP_E2E_CERTIFICATE_ALREADY_PRESENT");
        }
        finally
        {
            foreach (var match in matches) match.Dispose();
        }
    }

    private static async Task RemoveTrustedCertificateAsync(X509Certificate2 certificate)
    {
        using (var root = new X509Store(StoreName.Root, StoreLocation.LocalMachine))
        {
            root.Open(OpenFlags.ReadOnly);
            var matches = root.Certificates.Find(
                X509FindType.FindByThumbprint, certificate.Thumbprint, validOnly: false);
            try
            {
                if (matches.Count == 0) return;
                if (matches.Count != 1 || matches[0].HasPrivateKey ||
                    !matches[0].RawData.AsSpan().SequenceEqual(certificate.RawData))
                    throw new InvalidOperationException(
                        "DESKTOP_E2E_CERTIFICATE_REMOVAL_TARGET_INVALID");
            }
            finally
            {
                foreach (var match in matches) match.Dispose();
            }
        }

        await RunCertificateStoreCommandAsync(
            ["-delstore", "Root", certificate.Thumbprint], CancellationToken.None);
        using var verify = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
        verify.Open(OpenFlags.ReadOnly);
        var remaining = verify.Certificates.Find(
            X509FindType.FindByThumbprint, certificate.Thumbprint, validOnly: false);
        try
        {
            if (remaining.Count != 0)
                throw new InvalidOperationException("DESKTOP_E2E_CERTIFICATE_REMOVAL_VERIFICATION_FAILED");
        }
        finally
        {
            foreach (var match in remaining) match.Dispose();
        }
    }

    private static void DeleteTemporaryRoot(string path)
    {
        try { Directory.Delete(path, recursive: true); }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException) { }
    }
}
