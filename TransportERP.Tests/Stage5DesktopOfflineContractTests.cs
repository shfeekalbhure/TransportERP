namespace TransportERP.Tests;

public sealed class Stage5DesktopOfflineContractTests
{
    [Fact]
    public void Client_build_identity_requires_two_exact_equal_lowercase_commit_shas()
    {
        const string sha = "0123456789abcdef0123456789abcdef01234567";
        Assert.True(TransportERP.Application.Sync.SyncClientDeploymentAuthority.FixedShaEquals(sha, sha));
        Assert.False(TransportERP.Application.Sync.SyncClientDeploymentAuthority.FixedShaEquals(
            sha, "1123456789abcdef0123456789abcdef01234567"));
        Assert.False(TransportERP.Application.Sync.SyncClientDeploymentAuthority.FixedShaEquals(
            sha.ToUpperInvariant(), sha));
        Assert.False(TransportERP.Application.Sync.SyncClientDeploymentAuthority.FixedShaEquals("UNBOUND", sha));
    }

    [Fact]
    public void Desktop_is_an_executable_with_closed_default_startup_and_encrypted_offline_core()
    {
        var project = Read("TransportERP.Desktop", "TransportERP.Desktop.csproj");
        var program = Read("TransportERP.Desktop", "Program.cs");
        var context = Read("TransportERP.Desktop", "Application", "DesktopApplicationContext.cs");
        var sessions = Read("TransportERP.Desktop", "Application", "DesktopAuthenticatedSessionBridge.cs");
        var onlineAuth = Read("TransportERP.Desktop", "Application", "DesktopOnlineAuthentication.cs");
        var shell = Read("TransportERP.Desktop", "Application", "DesktopShellForm.cs");
        var operations = Read("TransportERP.Desktop", "Offline", "SyncOperationsForm.cs");

        Assert.Contains("<OutputType>WinExe</OutputType>", project, StringComparison.Ordinal);
        Assert.Contains("TransportERP.Offline\\TransportERP.Offline.csproj", project, StringComparison.Ordinal);
        Assert.Contains("[STAThread]", program, StringComparison.Ordinal);
        Assert.Contains("--startup-smoke", program, StringComparison.Ordinal);
        Assert.Contains("DesktopStartupContractProbe.VerifyClosedDefault() ? 0 : 1", program, StringComparison.Ordinal);
        Assert.Contains("OfflineRuntimeAuthorizedByDefault = false", context, StringComparison.Ordinal);
        Assert.Contains("ActivateAuthenticatedOfflineRuntimeAsync", context, StringComparison.Ordinal);
        Assert.Contains("new DesktopOnlineSignInSessionBridge(", program, StringComparison.Ordinal);
        Assert.Contains("new DesktopOnlineSessionAuthenticator()", program, StringComparison.Ordinal);
        Assert.Contains("new DesktopApplicationContext(authenticatedSessions)", program, StringComparison.Ordinal);
        Assert.Contains("_authenticatedSessions.SessionAuthenticated += OnSessionAuthenticated", context, StringComparison.Ordinal);
        Assert.Contains("_authenticatedSessions.Start(_shell)", context, StringComparison.Ordinal);
        Assert.Contains("_activationAttempted = true", context, StringComparison.Ordinal);
        Assert.Contains("activation.CreateRuntimeAsync", context, StringComparison.Ordinal);
        Assert.Contains("RunSyncSupervisorAsync", context, StringComparison.Ordinal);
        Assert.Contains("Enabled = false", shell, StringComparison.Ordinal);
        Assert.Contains("AttachAuthenticatedRuntime", shell, StringComparison.Ordinal);
        Assert.Contains("DesktopAutomationIds.OfflineStatus", shell, StringComparison.Ordinal);
        Assert.Contains("DesktopAutomationIds.SignIn", shell, StringComparison.Ordinal);
        Assert.Contains("DesktopAutomationIds.QueueParty", shell, StringComparison.Ordinal);
        Assert.Contains("SyncOperationsAutomationIds.Summary", operations, StringComparison.Ordinal);
        Assert.Contains("آخر حالة: {rows[0].StatusText}", operations, StringComparison.Ordinal);
        Assert.Contains("The bridge begins closed", sessions, StringComparison.Ordinal);
        Assert.Contains("/api/v1/auth/sessions", onlineAuth, StringComparison.Ordinal);
        Assert.Contains("/api/v1/sync/activation", onlineAuth, StringComparison.Ordinal);
    }

    [Fact]
    public void Desktop_authenticated_activation_is_single_use_scope_bound_and_session_lifetime_bound()
    {
        var program = Read("TransportERP.Desktop", "Program.cs");
        var bridge = Read("TransportERP.Desktop", "Application", "DesktopAuthenticatedSessionBridge.cs");
        var context = Read("TransportERP.Desktop", "Application", "DesktopApplicationContext.cs");
        var onlineAuth = Read("TransportERP.Desktop", "Application", "DesktopOnlineAuthentication.cs");
        var shell = Read("TransportERP.Desktop", "Application", "DesktopShellForm.cs");
        var releaseHost = Read(
            "TransportERP.Desktop.E2ETests", "DesktopReleaseKestrelApiHost.cs");

        Assert.Contains("DesktopAuthenticatedSessionScope AuthenticatedScope", bridge, StringComparison.Ordinal);
        Assert.Contains("DesktopAuthenticatedSessionScope AuthorizedOfflineScope", bridge, StringComparison.Ordinal);
        Assert.Contains("DateTimeOffset SessionExpiresAt", bridge, StringComparison.Ordinal);
        Assert.Contains("bool OfflineRuntimeAuthorized", bridge, StringComparison.Ordinal);
        Assert.Contains("Func<CancellationToken, Task<DesktopOfflineRuntime>> CreateRuntimeAsync", bridge, StringComparison.Ordinal);
        Assert.Contains("if (_published || _ended)", bridge, StringComparison.Ordinal);
        Assert.Contains("DESKTOP_SESSION_REPLAY_DENIED", bridge, StringComparison.Ordinal);
        Assert.Contains("sessionId != _sessionId", bridge, StringComparison.Ordinal);
        Assert.Contains("DESKTOP_SESSION_END_DENIED", bridge, StringComparison.Ordinal);

        var governedGate = context.IndexOf("!IsGovernedActivation(activation)", StringComparison.Ordinal);
        var factoryCall = context.IndexOf("await activation.CreateRuntimeAsync", StringComparison.Ordinal);
        Assert.True(governedGate >= 0 && governedGate < factoryCall);
        Assert.Contains("activation.AuthenticatedScope == activation.AuthorizedOfflineScope", context, StringComparison.Ordinal);
        Assert.Contains("activation.OfflineRuntimeAuthorized", context, StringComparison.Ordinal);
        Assert.Contains("activation.SessionExpiresAt > DateTimeOffset.UtcNow", context, StringComparison.Ordinal);
        Assert.Contains("if (_activationAttempted || !IsGovernedActivation(activation))", context, StringComparison.Ordinal);
        Assert.Contains("_activeSessionId != sessionId", context, StringComparison.Ordinal);
        Assert.Contains("_activationCancellation?.Cancel()", context, StringComparison.Ordinal);
        Assert.Contains("_supervisorCancellation?.Cancel()", context, StringComparison.Ordinal);
        Assert.Contains("_runtime?.Dispose()", context, StringComparison.Ordinal);
        Assert.Contains("_shell.CloseForSessionEnd(reasonCode)", context, StringComparison.Ordinal);
        Assert.Contains("Close();", shell, StringComparison.Ordinal);
        Assert.Contains("LogoutRequested += OnLogoutRequested", bridge, StringComparison.Ordinal);
        Assert.Contains("MonitorExpiryAsync", bridge, StringComparison.Ordinal);
        Assert.Contains("EndSessionAsync", bridge, StringComparison.Ordinal);
        Assert.Contains("/api/v1/auth/sessions/{sessionId:D}:revoke", onlineAuth, StringComparison.Ordinal);

        Assert.DoesNotContain("Environment.GetEnvironmentVariable", program, StringComparison.Ordinal);
        Assert.DoesNotContain("Environment.GetEnvironmentVariable", onlineAuth, StringComparison.Ordinal);
        Assert.DoesNotContain("ServerCertificateCustomValidationCallback", onlineAuth, StringComparison.Ordinal);
        Assert.DoesNotContain("DangerousAcceptAnyServerCertificateValidator", onlineAuth, StringComparison.Ordinal);
        Assert.Contains("[\"-addstore\", \"Root\", publicCertificatePath]", releaseHost,
            StringComparison.Ordinal);
        Assert.Contains("[\"-delstore\", \"Root\", certificate.Thumbprint]", releaseHost,
            StringComparison.Ordinal);
        Assert.DoesNotContain("[\"-user\",", releaseHost, StringComparison.Ordinal);
        Assert.Contains("StoreLocation.LocalMachine", releaseHost, StringComparison.Ordinal);
        Assert.DoesNotContain("StoreLocation.CurrentUser", releaseHost, StringComparison.Ordinal);
        Assert.Contains("VerifyCertificateAbsent(certificate)", releaseHost, StringComparison.Ordinal);
        Assert.Contains("if (trustMutationMayHaveOccurred)", releaseHost, StringComparison.Ordinal);
        Assert.DoesNotContain("[\"-f\",", releaseHost, StringComparison.Ordinal);
        Assert.Contains("matches.Count != 1 || matches[0].HasPrivateKey", releaseHost,
            StringComparison.Ordinal);
        Assert.Contains("Environment.GetEnvironmentVariable(\"RUNNER_ENVIRONMENT\"), \"github-hosted\"",
            releaseHost,
            StringComparison.Ordinal);
        Assert.Contains("WindowsBuiltInRole.Administrator", releaseHost, StringComparison.Ordinal);
        Assert.Contains("new SslStream(tcp.GetStream(), leaveInnerStreamOpen: false)", releaseHost,
            StringComparison.Ordinal);
        Assert.Contains("TargetHost = \"localhost\"", releaseHost, StringComparison.Ordinal);
        Assert.Contains("timeout.CancelAfter(TimeSpan.FromSeconds(5))", releaseHost,
            StringComparison.Ordinal);
        Assert.Contains("catch (AuthenticationException)", releaseHost, StringComparison.Ordinal);
        Assert.DoesNotContain("RemoteCertificateValidationCallback", releaseHost, StringComparison.Ordinal);
        Assert.DoesNotContain("DangerousAcceptAnyServerCertificateValidator", releaseHost,
            StringComparison.Ordinal);
        Assert.Contains("args.Length != 2", program, StringComparison.Ordinal);
        Assert.Contains("Path.IsPathFullyQualified(args[1])", program, StringComparison.Ordinal);
        Assert.Contains("FileMode.CreateNew", program, StringComparison.Ordinal);
        Assert.DoesNotContain("Password", program, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Credential", program, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("BearerToken", bridge, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("DeviceCredential", bridge, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ProofJson", bridge, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ExportParameters", bridge, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("DesktopStartupContractProbe.VerifyClosedDefault", program, StringComparison.Ordinal);
        Assert.Contains("return activationCount == 0", bridge, StringComparison.Ordinal);
    }

    [Fact]
    public void Desktop_release_host_classifies_startup_stderr_without_retaining_raw_output()
    {
        var releaseHost = Read(
            "TransportERP.Desktop.E2ETests", "DesktopReleaseKestrelApiHost.cs");
        var apiProgram = Read("TransportERP.Api", "Program.cs");
        var startupExceptions = Read(
            "TransportERP.Api", "Startup", "StartupOptionsValidationExceptions.cs");
        var productionE2E = Read(
            "TransportERP.Desktop.E2ETests", "DesktopProductionEndToEndPostgreSqlTests.cs");
        var workflow = Read(".github", "workflows", "ci.yml");

        Assert.Contains("StartupFailureClassifier", releaseHost, StringComparison.Ordinal);
        Assert.Contains("process.StandardError, startupFailureClassifier.Observe", releaseHost,
            StringComparison.Ordinal);
        Assert.Contains("var stdoutDrain = DrainAsync(process.StandardOutput, drainCancellation.Token)",
            releaseHost, StringComparison.Ordinal);
        Assert.Contains("private string? _code", releaseHost, StringComparison.Ordinal);
        Assert.Contains("Volatile.Read(ref _code)", releaseHost, StringComparison.Ordinal);
        Assert.Contains("Interlocked.CompareExchange(", releaseHost, StringComparison.Ordinal);
        Assert.Contains("const string prefix = \"Unhandled exception. \"", releaseHost,
            StringComparison.Ordinal);
        Assert.Contains("StringComparison.Ordinal", releaseHost, StringComparison.Ordinal);
        Assert.Contains("DESKTOP_E2E_API_STARTUP_OPTIONS_VALIDATION", releaseHost,
            StringComparison.Ordinal);
        Assert.Contains("DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_VALIDATION", releaseHost,
            StringComparison.Ordinal);
        Assert.Contains("DESKTOP_E2E_API_STARTUP_SYNC_IMPLEMENTATION_MISMATCH", releaseHost,
            StringComparison.Ordinal);
        Assert.Contains("DESKTOP_E2E_API_STARTUP_SYNC_AUTHORIZED_BUILD_INVALID", releaseHost,
            StringComparison.Ordinal);
        Assert.Contains("F01_OFFLINE_ENABLED_REQUIRED", releaseHost,
            StringComparison.Ordinal);
        Assert.Contains("F25_PAYLOAD_EXCEEDS_REQUEST", releaseHost,
            StringComparison.Ordinal);
        Assert.Contains("MaximumGovernedFailureLineLength", releaseHost,
            StringComparison.Ordinal);
        Assert.Contains("DESKTOP_E2E_API_STARTUP_EFFECTIVE_POLICY_VALIDATION", releaseHost,
            StringComparison.Ordinal);
        Assert.Contains("DESKTOP_E2E_API_STARTUP_AUTH_VALIDATION", releaseHost,
            StringComparison.Ordinal);
        Assert.Contains("throw new SyncRuntimePolicyStartupOptionsValidationException(", apiProgram,
            StringComparison.Ordinal);
        Assert.Contains("throw new EffectivePolicyStartupOptionsValidationException(", apiProgram,
            StringComparison.Ordinal);
        Assert.Contains("throw new AuthStartupOptionsValidationException(", apiProgram,
            StringComparison.Ordinal);
        Assert.Contains(": OptionsValidationException(\"Sync\", typeof(SyncRuntimePolicyOptions), failures)",
            startupExceptions, StringComparison.Ordinal);
        Assert.Contains("\"Sync:EffectivePolicy\", typeof(EffectivePolicyConfiguration), failures)",
            startupExceptions, StringComparison.Ordinal);
        Assert.Contains(": OptionsValidationException(\"Auth\", typeof(TransportSecurityOptions), failures)",
            startupExceptions, StringComparison.Ordinal);
        Assert.Contains("Run and verify Desktop startup classifier tests", workflow,
            StringComparison.Ordinal);
        Assert.Contains("FullyQualifiedName~DesktopStartupFailureClassifierTests", workflow,
            StringComparison.Ordinal);
        Assert.Contains("$total -ne 61", workflow, StringComparison.Ordinal);
        Assert.DoesNotContain("override string Message", startupExceptions,
            StringComparison.Ordinal);
        Assert.Contains("DESKTOP_E2E_API_STARTUP_CRYPTOGRAPHIC", releaseHost,
            StringComparison.Ordinal);
        Assert.Contains("DESKTOP_E2E_API_STARTUP_POSTGRESQL", releaseHost,
            StringComparison.Ordinal);
        Assert.Contains("DESKTOP_E2E_API_STARTUP_UNCLASSIFIED", releaseHost,
            StringComparison.Ordinal);
        Assert.Contains("await _stderrDrain.WaitAsync(TimeSpan.FromSeconds(2), cancellationToken)", releaseHost,
            StringComparison.Ordinal);
        Assert.Contains("catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)",
            releaseHost, StringComparison.Ordinal);
        Assert.Contains("return \"DESKTOP_E2E_API_STARTUP_UNCLASSIFIED\"", releaseHost,
            StringComparison.Ordinal);
        Assert.DoesNotContain("exception.Message", releaseHost, StringComparison.Ordinal);
        Assert.DoesNotContain("line.ToString", releaseHost, StringComparison.Ordinal);
        Assert.DoesNotContain(".StackTrace", releaseHost, StringComparison.Ordinal);
        Assert.DoesNotContain("ReadToEnd", releaseHost, StringComparison.Ordinal);
        Assert.Contains("new StringBuilder(\"DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY\")",
            releaseHost, StringComparison.Ordinal);
        Assert.Contains("Append(GovernedSyncPolicyFailures[currentIndex].Code)", releaseHost,
            StringComparison.Ordinal);
        Assert.DoesNotContain("Append(token", releaseHost, StringComparison.Ordinal);
        Assert.Contains("SyncActionCatalog.Definitions", productionE2E, StringComparison.Ordinal);
        Assert.Contains("Sync__Offline__AllowedActions__{index}", productionE2E,
            StringComparison.Ordinal);
        Assert.Contains("governedActions.Distinct(StringComparer.Ordinal).Count()", productionE2E,
            StringComparison.Ordinal);
        Assert.DoesNotContain("Console.Write", releaseHost, StringComparison.Ordinal);
        Assert.DoesNotContain("Trace.Write", releaseHost, StringComparison.Ordinal);
        Assert.DoesNotContain("Debug.Write", releaseHost, StringComparison.Ordinal);
        Assert.DoesNotContain("WriteLine(line)", releaseHost, StringComparison.Ordinal);
    }

    [Fact]
    public void Desktop_online_producer_authorizes_before_local_storage_or_signing_key_and_keeps_secrets_volatile()
    {
        var onlineAuth = Read("TransportERP.Desktop", "Application", "DesktopOnlineAuthentication.cs");
        var bridge = Read("TransportERP.Desktop", "Application", "DesktopAuthenticatedSessionBridge.cs");
        var shell = Read("TransportERP.Desktop", "Application", "DesktopShellForm.cs");
        var authority = Read("TransportERP.Application", "Sync", "SyncClientDeploymentAuthority.cs");
        var applicationProject = Read("TransportERP.Application", "TransportERP.Application.csproj");

        var login = onlineAuth.IndexOf("await CreateSessionAsync", StringComparison.Ordinal);
        var policy = onlineAuth.IndexOf("await GetSyncActivationAsync", StringComparison.Ordinal);
        var composition = onlineAuth.IndexOf("DesktopOfflineComposition.CreateAsync", StringComparison.Ordinal);
        Assert.True(login >= 0 && login < policy && policy < composition);

        var requestStart = onlineAuth.IndexOf("internal sealed record DesktopOnlineSignInRequest(", StringComparison.Ordinal);
        var requestEnd = onlineAuth.IndexOf(");", requestStart, StringComparison.Ordinal);
        Assert.True(requestStart >= 0 && requestEnd > requestStart);
        Assert.DoesNotContain("Origin", onlineAuth[requestStart..requestEnd], StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("_publicOrigin", shell, StringComparison.Ordinal);
        Assert.DoesNotContain("عنوان خادم", shell, StringComparison.Ordinal);
        Assert.Contains("var origin = SyncClientDeploymentAuthority.Origin", onlineAuth, StringComparison.Ordinal);
        Assert.Contains("public static class SyncClientDeploymentAuthority", authority, StringComparison.Ordinal);
        Assert.Contains("GetCustomAttributes<AssemblyMetadataAttribute>()", authority, StringComparison.Ordinal);
        Assert.Contains("Uri.UriSchemeHttps", authority, StringComparison.Ordinal);
        Assert.Contains("TransportERPClientPublicOrigin", applicationProject, StringComparison.Ordinal);
        Assert.Contains("https://sync.example.test/", applicationProject, StringComparison.Ordinal);
        Assert.DoesNotContain("Environment", authority, StringComparison.Ordinal);
        Assert.DoesNotContain("IConfiguration", authority, StringComparison.Ordinal);
        Assert.DoesNotContain("TryCanonicalOrigin", onlineAuth, StringComparison.Ordinal);

        Assert.Contains("Uri.UriSchemeHttps", onlineAuth, StringComparison.Ordinal);
        Assert.Contains("AllowAutoRedirect = false", onlineAuth, StringComparison.Ordinal);
        Assert.Contains("UseCookies = false", onlineAuth, StringComparison.Ordinal);
        Assert.Contains("activation.Enabled", onlineAuth, StringComparison.Ordinal);
        Assert.Contains("activation.ClosedReason is not null", onlineAuth, StringComparison.Ordinal);
        Assert.Contains("activation.CompanyId != session.CompanyId", onlineAuth, StringComparison.Ordinal);
        Assert.Contains("activation.SessionId != session.SessionId", onlineAuth, StringComparison.Ordinal);
        Assert.Contains("activation.RegisteredDeviceId == Guid.Empty", onlineAuth, StringComparison.Ordinal);
        Assert.Contains("activation.AllowedActions.Count == 0", onlineAuth, StringComparison.Ordinal);
        Assert.Contains("IsLowerHex64(activation.PolicySourceFingerprint)", onlineAuth, StringComparison.Ordinal);
        Assert.Contains("value is { Length: 64 }", onlineAuth, StringComparison.Ordinal);
        Assert.Contains("character is >= '0' and <= '9' or >= 'a' and <= 'f'", onlineAuth, StringComparison.Ordinal);
        Assert.Contains("new ServerValidatedOfflineWritePolicy(allowedActions!)", onlineAuth, StringComparison.Ordinal);
        Assert.Contains("new VolatileBearerTokenProvider(session.AccessToken)", onlineAuth, StringComparison.Ordinal);
        Assert.Contains("private string? _token", onlineAuth, StringComparison.Ordinal);
        Assert.Contains("public void Dispose() => _token = null", onlineAuth, StringComparison.Ordinal);
        Assert.Contains("Environment.SpecialFolder.LocalApplicationData", onlineAuth, StringComparison.Ordinal);
        Assert.Contains("WindowsDpapiLocalEncryptionKeyProvider", Read(
            "TransportERP.Desktop", "Offline", "DesktopOfflineComposition.cs"), StringComparison.Ordinal);
        Assert.Contains("SignInRequested += OnSignInRequested", bridge, StringComparison.Ordinal);
        Assert.Contains("PublishAuthenticatedSession(result.Activation)", bridge, StringComparison.Ordinal);
        Assert.Contains("UseSystemPasswordChar = password", shell, StringComparison.Ordinal);
        Assert.Contains("_password.Clear()", shell, StringComparison.Ordinal);
        Assert.Contains("_deviceCredential.Clear()", shell, StringComparison.Ordinal);

        Assert.DoesNotContain("File.WriteAllText", onlineAuth, StringComparison.Ordinal);
        Assert.DoesNotContain("ProtectedData.Protect", onlineAuth, StringComparison.Ordinal);
        Assert.DoesNotContain("WindowsCertificateDeviceProofSigningKeyStore", onlineAuth, StringComparison.Ordinal);
        Assert.DoesNotContain("ProbeDpopNonceAsync", onlineAuth, StringComparison.Ordinal);
        Assert.DoesNotContain("DeviceCredential =", onlineAuth, StringComparison.Ordinal);
        Assert.DoesNotContain("Environment.GetEnvironmentVariable", onlineAuth, StringComparison.Ordinal);
        Assert.DoesNotContain("args[", onlineAuth, StringComparison.Ordinal);
    }

    [Fact]
    public void Desktop_consumes_exact_effective_policy_build_identity_and_has_a_real_business_producer()
    {
        var onlineAuth = Read("TransportERP.Desktop", "Application", "DesktopOnlineAuthentication.cs");
        var composition = Read("TransportERP.Desktop", "Offline", "DesktopOfflineComposition.cs");
        var producer = Read("TransportERP.Desktop", "Offline", "DesktopOfflineBusinessProducer.cs");
        var sharedProducer = Read("TransportERP.Offline", "OperationalPartyOfflineProducer.cs");
        var shell = Read("TransportERP.Desktop", "Application", "DesktopShellForm.cs");
        var program = Read("TransportERP.Desktop", "Program.cs");
        var platformProbe = Read("TransportERP.Desktop", "Offline", "DesktopRuntimePlatformProbe.cs");
        var authority = Read("TransportERP.Application", "Sync", "SyncClientDeploymentAuthority.cs");
        var policy = Read("TransportERP.Application", "Sync", "SyncClientEffectivePolicy.cs");
        var project = Read("TransportERP.Application", "TransportERP.Application.csproj");

        Assert.Contains("TryEffectivePolicy(activation", onlineAuth, StringComparison.Ordinal);
        Assert.Contains("activation.MaxBatchOperations", onlineAuth, StringComparison.Ordinal);
        Assert.Contains("activation.ClientTransportMaxRetryCount", onlineAuth, StringComparison.Ordinal);
        Assert.Contains("activation.LocalSuccessHours", onlineAuth, StringComparison.Ordinal);
        Assert.Contains("activation.CacheMaxAgeHours", onlineAuth, StringComparison.Ordinal);
        Assert.Contains("activation.MaximumRequestBodyBytes", onlineAuth, StringComparison.Ordinal);
        Assert.Contains("activation.MaximumPayloadBytes", onlineAuth, StringComparison.Ordinal);
        Assert.Contains("activation.ActivationImplementationSha", onlineAuth, StringComparison.Ordinal);
        Assert.Contains("options.EffectivePolicy.IsValid", composition, StringComparison.Ordinal);
        Assert.Contains("options.EffectivePolicy.MaxBatchOperations", composition, StringComparison.Ordinal);
        Assert.Contains("options.EffectivePolicy.LocalSuccessRetention", composition, StringComparison.Ordinal);
        Assert.Contains("READ_CACHE_POLICY_DENIED", composition, StringComparison.Ordinal);

        Assert.Contains("TransportERPImplementationSha", project, StringComparison.Ordinal);
        Assert.Contains("IsAuthorizedImplementation", authority, StringComparison.Ordinal);
        Assert.Contains("CryptographicOperations.FixedTimeEquals", authority, StringComparison.Ordinal);
        Assert.Contains("SyncClientDeploymentAuthority.IsAuthorizedImplementation", policy, StringComparison.Ordinal);
        Assert.DoesNotContain("Environment.GetEnvironmentVariable", authority, StringComparison.Ordinal);

        Assert.Contains("QueueOperationalPartyAsync", producer, StringComparison.Ordinal);
        Assert.Contains("new OperationalPartyOfflineProducer", producer, StringComparison.Ordinal);
        Assert.Contains("new OperationalPartyCreateRequest", sharedProducer, StringComparison.Ordinal);
        Assert.Contains("identity.ClientOperationId", sharedProducer, StringComparison.Ordinal);
        Assert.Contains("runtime.QueueAsync", producer, StringComparison.Ordinal);
        Assert.Contains("QueueOperationalPartyAsync", shell, StringComparison.Ordinal);
        Assert.Contains("CreateBusinessProducer", shell, StringComparison.Ordinal);
        Assert.Contains("_runtimeAuthorized = false", shell, StringComparison.Ordinal);
        Assert.Contains("_runtime = null", shell, StringComparison.Ordinal);
        Assert.Contains("_businessProducer = null", shell, StringComparison.Ordinal);
        Assert.Contains("_runtimeAuthorized && _businessProducer is not null", shell,
            StringComparison.Ordinal);
        Assert.True(
            shell.IndexOf("_runtimeAuthorized = false", shell.IndexOf("ReportSupervisorStopped", StringComparison.Ordinal),
                StringComparison.Ordinal) <
            shell.IndexOf("_queueParty.Enabled = false", shell.IndexOf("ReportSupervisorStopped", StringComparison.Ordinal),
                StringComparison.Ordinal));
        Assert.Contains("--runtime-platform-smoke", program, StringComparison.Ordinal);
        Assert.Contains("--print-build-identity", program, StringComparison.Ordinal);
        Assert.Contains("DesktopBuildIdentityProbe.Measure()", program, StringComparison.Ordinal);
        Assert.Contains("BUILD_IDENTITY_OUTPUT_INVALID", program, StringComparison.Ordinal);
        Assert.Contains("FileMode.CreateNew", program, StringComparison.Ordinal);
        Assert.DoesNotContain("Console.Out", program, StringComparison.Ordinal);
        Assert.Contains("CngExportPolicies.None", platformProbe, StringComparison.Ordinal);
        Assert.Contains("WindowsDpapiLocalEncryptionKeyProvider", composition, StringComparison.Ordinal);
        Assert.Contains("QueueOperationalPartyAsync", platformProbe, StringComparison.Ordinal);
        Assert.Contains("SynchronizeAsync", platformProbe, StringComparison.Ordinal);
        Assert.Contains("GetOperationAsync", platformProbe, StringComparison.Ordinal);
        Assert.Contains("Status: OfflineOperationStatus.Succeeded", platformProbe, StringComparison.Ordinal);
        Assert.Contains("in-process protocol peer", platformProbe, StringComparison.Ordinal);
    }

    [Fact]
    public void Sqlcipher_key_provider_is_DPAPI_current_user_purpose_separated_and_fail_closed()
    {
        var source = Read("TransportERP.Desktop", "Offline", "WindowsDpapiLocalEncryptionKeyProvider.cs");

        Assert.Contains("ProtectedData.Protect", source, StringComparison.Ordinal);
        Assert.Contains("ProtectedData.Unprotect", source, StringComparison.Ordinal);
        Assert.Contains("DataProtectionScope.CurrentUser", source, StringComparison.Ordinal);
        Assert.DoesNotContain("DataProtectionScope.LocalMachine", source, StringComparison.Ordinal);
        Assert.Contains("WriteOutbox => \"write-outbox.v1.dpapi\"", source, StringComparison.Ordinal);
        Assert.Contains("ReadCache => \"read-cache.v1.dpapi\"", source, StringComparison.Ordinal);
        Assert.Contains("offline-sqlcipher|v1|{purpose}", source, StringComparison.Ordinal);
        Assert.Contains("LOCAL_SECURE_STORAGE_UNAVAILABLE", source, StringComparison.Ordinal);
        Assert.Contains("DataProtectionScope.CurrentUser", source, StringComparison.Ordinal);
        Assert.Contains("platformProbeCheckpoint: null", source, StringComparison.Ordinal);
        var usingDeclaration = source.IndexOf("using (var stream = new FileStream(", StringComparison.Ordinal);
        Assert.True(usingDeclaration >= 0, "The protected-blob write must own a disposing stream scope.");
        var usingOpenBrace = source.IndexOf('{', usingDeclaration);
        Assert.True(usingOpenBrace > usingDeclaration, "The disposing stream scope must have a block.");
        var usingCloseBrace = FindMatchingBrace(source, usingOpenBrace);
        Assert.True(usingCloseBrace > usingOpenBrace, "The disposing stream scope must close before publication.");
        var flush = source.IndexOf("stream.Flush(flushToDisk: true);", usingOpenBrace,
            StringComparison.Ordinal);
        var atomicMove = source.IndexOf("File.Move(temporaryPath, path, overwrite: false);", usingCloseBrace,
            StringComparison.Ordinal);
        Assert.True(usingDeclaration >= 0 && usingOpenBrace > usingDeclaration &&
            flush > usingOpenBrace && flush < usingCloseBrace && atomicMove > usingCloseBrace,
            "The write must flush inside a disposing using block and publish only after the handle closes.");
        Assert.DoesNotContain("exception.Message", source, StringComparison.Ordinal);
        Assert.DoesNotContain("return new byte[", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Device_proof_key_is_a_non_exportable_certificate_store_signing_handle()
    {
        var source = Read("TransportERP.Desktop", "Offline", "DeviceProofSigningKeyStore.cs");

        Assert.Contains("StoreLocation.CurrentUser", source, StringComparison.Ordinal);
        Assert.Contains("GetECDsaPrivateKey", source, StringComparison.Ordinal);
        Assert.Contains("exportPolicy != CngExportPolicies.None", source, StringComparison.Ordinal);
        Assert.Contains("DEVICE_KEY_EXPORTABLE", source, StringComparison.Ordinal);
        Assert.Contains("DSASignatureFormat.IeeeP1363FixedFieldConcatenation", source, StringComparison.Ordinal);
        Assert.DoesNotContain("ExportParameters(includePrivateParameters: true)", source, StringComparison.Ordinal);
        Assert.DoesNotContain("GetPrivateKey", source, StringComparison.Ordinal);
        Assert.Contains("TransportERP.Offline.Transport.IDeviceProofSigningKey", source, StringComparison.Ordinal);
        Assert.Contains("SignEs256Async", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Operations_screen_is_RTL_explicitly_bound_and_never_adds_secret_columns()
    {
        var form = Read("TransportERP.Desktop", "Offline", "SyncOperationsForm.cs");
        var contracts = Read("TransportERP.Desktop", "Offline", "SyncOperationsContracts.cs");

        Assert.Contains("RightToLeft = RightToLeft.Yes", form, StringComparison.Ordinal);
        Assert.Contains("RightToLeftLayout = true", form, StringComparison.Ordinal);
        Assert.Contains("AutoGenerateColumns = false", form, StringComparison.Ordinal);
        Assert.Contains("Column(nameof(SyncOperationDisplayRow.StatusText)", form, StringComparison.Ordinal);
        Assert.Contains("OfflineOperationStatus.Queued =>", contracts, StringComparison.Ordinal);
        Assert.Contains("OfflineOperationStatus.Sending =>", contracts, StringComparison.Ordinal);
        Assert.Contains("OfflineOperationStatus.Succeeded =>", contracts, StringComparison.Ordinal);
        Assert.Contains("OfflineOperationStatus.Failed =>", contracts, StringComparison.Ordinal);
        Assert.Contains("OfflineOperationStatus.Conflict =>", contracts, StringComparison.Ordinal);
        Assert.Contains("OfflineOperationStatus.Rejected =>", contracts, StringComparison.Ordinal);
        Assert.Contains("OfflineOperationStatus.Resolved =>", contracts, StringComparison.Ordinal);
        Assert.Contains("مراجعة التعارض (بيانات منقّحة فقط)", form, StringComparison.Ordinal);
        Assert.Contains("الإصدار الأساسي", form, StringComparison.Ordinal);
        Assert.Contains("لقطة العميل المنقّحة", form, StringComparison.Ordinal);
        Assert.Contains("لقطة الخادم المنقّحة", form, StringComparison.Ordinal);
        Assert.Contains("المقرّر", form, StringComparison.Ordinal);
        Assert.Contains("نتيجة الحل", form, StringComparison.Ordinal);
        Assert.Contains("SyncConflictReviewDisplay", contracts, StringComparison.Ordinal);

        foreach (var forbidden in new[] { "PayloadJson", "PayloadHash", "Proof", "Token", "Nonce", "Jti", "Credential" })
            Assert.DoesNotContain($"Column(nameof(SyncOperationDisplayRow.{forbidden})", form, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Retry_and_conflict_actions_enforce_state_and_injected_permission_before_side_effect()
    {
        var controller = Read("TransportERP.Desktop", "Offline", "SyncOperationsController.cs");
        var contracts = Read("TransportERP.Desktop", "Offline", "SyncOperationsContracts.cs");

        Assert.Contains("operation.Status != OfflineOperationStatus.Failed", controller, StringComparison.Ordinal);
        Assert.Contains("_permissions.CanRetry(operation)", controller, StringComparison.Ordinal);
        Assert.Contains("operation.Status != OfflineOperationStatus.Conflict", controller, StringComparison.Ordinal);
        Assert.Contains("_permissions.CanResolveConflict(operation, decision)", controller, StringComparison.Ordinal);
        Assert.Contains("operation.ConflictReview?.IsDecisionReady != true", controller, StringComparison.Ordinal);
        Assert.Contains("SyncUiActionResult.ReviewRequired()", controller, StringComparison.Ordinal);
        Assert.Contains("CONFLICT_REVIEW_REQUIRED", contracts, StringComparison.Ordinal);
        Assert.True(
            controller.IndexOf("_permissions.CanRetry(operation)", StringComparison.Ordinal) <
            controller.IndexOf("_retry.RetryAsync", StringComparison.Ordinal));
        Assert.True(
            controller.IndexOf("_permissions.CanResolveConflict(operation, decision)", StringComparison.Ordinal) <
            controller.IndexOf("_conflicts.ResolveAsync", StringComparison.Ordinal));
        Assert.Contains("SyncConflictDecision.KeepServer", controller, StringComparison.Ordinal);
        Assert.Contains("SyncConflictDecision.Reapply", controller, StringComparison.Ordinal);
    }

    [Fact]
    public void Desktop_composition_is_explicitly_closed_and_connects_encrypted_store_transport_and_ui()
    {
        var source = Read("TransportERP.Desktop", "Offline", "DesktopOfflineComposition.cs");

        Assert.Contains("bool OfflineRuntimeAuthorized = false", source, StringComparison.Ordinal);
        Assert.Contains("if (!options.OfflineRuntimeAuthorized)", source, StringComparison.Ordinal);
        Assert.Contains("WindowsDpapiLocalEncryptionKeyProvider", source, StringComparison.Ordinal);
        Assert.Contains("WindowsCertificateDeviceProofSigningKeyStore", source, StringComparison.Ordinal);
        Assert.Contains("VerifyProofBinding(options.ProofBinding, signingKey.PublicKey)", source, StringComparison.Ordinal);
        Assert.True(
            source.IndexOf("VerifyProofBinding(options.ProofBinding", StringComparison.Ordinal) <
            source.IndexOf("new WindowsDpapiLocalEncryptionKeyProvider", StringComparison.Ordinal));
        Assert.Contains("DEVICE_KEY_BINDING_MISMATCH", source, StringComparison.Ordinal);
        Assert.Contains("CryptographicOperations.FixedTimeEquals", source, StringComparison.Ordinal);
        Assert.Contains("new OfflineOperationStore", source, StringComparison.Ordinal);
        Assert.Contains("new OfflineReadCacheStore", source, StringComparison.Ordinal);
        Assert.Contains("new OfflineSyncTransportClient", source, StringComparison.Ordinal);
        Assert.Contains("new OfflineSyncConflictClient", source, StringComparison.Ordinal);
        Assert.Contains("new OfflineSyncSupervisor", source, StringComparison.Ordinal);
        Assert.Contains("RunSyncSupervisorAsync", source, StringComparison.Ordinal);
        Assert.Contains("OfflineOperationEnqueueTemplate", source, StringComparison.Ordinal);
        Assert.Contains("payloadFactory", source, StringComparison.Ordinal);
        Assert.Contains("store.ListAsync(scope", source, StringComparison.Ordinal);
        Assert.Contains("keys, scope, timeProvider", source, StringComparison.Ordinal);
        Assert.Contains("new SyncOperationsController", source, StringComparison.Ordinal);
        Assert.Contains("StoreConflictBaseVersionProvider(outbox, scope)", source, StringComparison.Ordinal);
        Assert.Contains("ConflictReview?.ServerSnapshot?.CurrentVersion is not > 0", source, StringComparison.Ordinal);
        Assert.Contains("template.CompanyId != _options.CompanyId", source, StringComparison.Ordinal);
        Assert.DoesNotContain("OfflineRuntimeAuthorized = true", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Conflict_decisions_require_a_reviewed_reason_before_transport()
    {
        var form = Read("TransportERP.Desktop", "Offline", "SyncOperationsForm.cs");
        var controller = Read("TransportERP.Desktop", "Offline", "SyncOperationsController.cs");
        var composition = Read("TransportERP.Desktop", "Offline", "DesktopOfflineComposition.cs");

        Assert.Contains("سبب قرار التعارض (مطلوب)", form, StringComparison.Ordinal);
        Assert.Contains("راجعت بيانات التعارض المعروضة", form, StringComparison.Ordinal);
        Assert.Contains("_reviewConfirmed.Checked", form, StringComparison.Ordinal);
        Assert.Contains("HasCompleteConflictReview", form, StringComparison.Ordinal);
        Assert.Contains("string.IsNullOrWhiteSpace(reason)", controller, StringComparison.Ordinal);
        Assert.Contains("CONFLICT_REASON_REQUIRED", controller, StringComparison.Ordinal);
        Assert.Contains("reason, baseVersion, cancellationToken", composition, StringComparison.Ordinal);
        Assert.Contains("new StoreManualRetryService(outbox, scope)", composition, StringComparison.Ordinal);
        Assert.Contains("store.RequeueFailedAsync(localOperationId, scope", composition, StringComparison.Ordinal);
    }

    private static string Read(params string[] path) =>
        File.ReadAllText(Path.Combine([RepositoryRoot(), .. path]));

    private static int FindMatchingBrace(string source, int openBrace)
    {
        if (openBrace < 0 || openBrace >= source.Length || source[openBrace] != '{') return -1;
        var depth = 0;
        for (var index = openBrace; index < source.Length; index++)
        {
            if (source[index] == '{') depth++;
            if (source[index] != '}') continue;
            depth--;
            if (depth == 0) return index;
        }
        return -1;
    }

    private static string RepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "TransportERP.slnx")))
            directory = directory.Parent;

        return directory?.FullName
            ?? throw new DirectoryNotFoundException("TransportERP repository root was not found.");
    }
}
