using System.Diagnostics;

namespace TransportERP.Desktop.E2ETests;

public sealed class DesktopStartupFailureClassifierTests
{
    private const string SyncRuntimePolicyException =
        "TransportERP.Api.Startup.SyncRuntimePolicyStartupOptionsValidationException";

    [Theory]
    [InlineData("TransportERP.Api.Startup.SyncRuntimePolicyStartupOptionsValidationException", "DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_VALIDATION")]
    [InlineData("TransportERP.Api.Startup.EffectivePolicyStartupOptionsValidationException", "DESKTOP_E2E_API_STARTUP_EFFECTIVE_POLICY_VALIDATION")]
    [InlineData("TransportERP.Api.Startup.AuthStartupOptionsValidationException", "DESKTOP_E2E_API_STARTUP_AUTH_VALIDATION")]
    [InlineData("Microsoft.Extensions.Options.OptionsValidationException", "DESKTOP_E2E_API_STARTUP_OPTIONS_VALIDATION")]
    [InlineData("System.InvalidOperationException", "DESKTOP_E2E_API_STARTUP_INVALID_OPERATION")]
    [InlineData("System.TypeInitializationException", "DESKTOP_E2E_API_STARTUP_TYPE_INITIALIZATION")]
    [InlineData("System.Security.Cryptography.CryptographicException", "DESKTOP_E2E_API_STARTUP_CRYPTOGRAPHIC")]
    [InlineData("System.IO.IOException", "DESKTOP_E2E_API_STARTUP_IO")]
    [InlineData("System.IO.FileNotFoundException", "DESKTOP_E2E_API_STARTUP_FILE_NOT_FOUND")]
    [InlineData("System.UnauthorizedAccessException", "DESKTOP_E2E_API_STARTUP_UNAUTHORIZED_ACCESS")]
    [InlineData("System.Net.Sockets.SocketException", "DESKTOP_E2E_API_STARTUP_SOCKET")]
    [InlineData("Npgsql.NpgsqlException", "DESKTOP_E2E_API_STARTUP_POSTGRESQL")]
    [InlineData("Npgsql.PostgresException", "DESKTOP_E2E_API_STARTUP_POSTGRESQL")]
    [InlineData("System.ArgumentException", "DESKTOP_E2E_API_STARTUP_ARGUMENT")]
    [InlineData("System.FormatException", "DESKTOP_E2E_API_STARTUP_FORMAT")]
    public void Exact_top_level_allowlist_maps_to_fixed_code(string type, string expected)
    {
        var classifier = new DesktopReleaseKestrelApiHost.StartupFailureClassifier();

        classifier.Observe($"Unhandled exception. {type}: synthetic message");

        Assert.Equal(expected, classifier.Code);
    }

    [Theory]
    [InlineData(
        "Sync:Offline:ActivationImplementationSha must match the exact running API build.",
        "DESKTOP_E2E_API_STARTUP_SYNC_IMPLEMENTATION_MISMATCH")]
    [InlineData(
        "Sync:Offline:AuthorizedBuilds must contain one valid exact identity per approved platform.",
        "DESKTOP_E2E_API_STARTUP_SYNC_AUTHORIZED_BUILD_INVALID")]
    public void Exact_single_sync_deployment_failure_maps_to_fixed_code(
        string failure,
        string expected)
    {
        var classifier = new DesktopReleaseKestrelApiHost.StartupFailureClassifier();

        classifier.Observe($"Unhandled exception. {SyncRuntimePolicyException}: {failure}");

        Assert.Equal(expected, classifier.Code);
    }

    [Theory]
    [InlineData(
        "Sync:Offline:Enabled must be explicitly configured.",
        "DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_F01_OFFLINE_ENABLED_REQUIRED")]
    [InlineData(
        "Sync:ServerExecution:Enabled must be explicitly configured.",
        "DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_F02_SERVER_EXECUTION_REQUIRED")]
    [InlineData(
        "Offline activation evidence must be absent while Sync:Offline:Enabled is false.",
        "DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_F03_ACTIVATION_EVIDENCE_WHILE_CLOSED")]
    [InlineData(
        "Sync:ServerExecution:Enabled must be true before Offline can be activated.",
        "DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_F04_SERVER_EXECUTION_DISABLED")]
    [InlineData(
        "Sync:Offline:ActivationDecisionId must be an explicit safe G5 decision identifier.",
        "DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_F05_ACTIVATION_DECISION_INVALID")]
    [InlineData(
        "Sync:Offline:ActivationImplementationSha must bind G5 activation to an exact 40-character commit SHA.",
        "DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_F06_ACTIVATION_SHA_INVALID")]
    [InlineData(
        "Sync:Offline:ActivationImplementationSha must match the exact running API build.",
        "DESKTOP_E2E_API_STARTUP_SYNC_IMPLEMENTATION_MISMATCH")]
    [InlineData(
        "Sync:Offline:AuthorizedBuilds must contain one valid exact identity per approved platform.",
        "DESKTOP_E2E_API_STARTUP_SYNC_AUTHORIZED_BUILD_INVALID")]
    [InlineData(
        "Sync:Protocol:AllowedVersions must contain only sync-v1.",
        "DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_F09_PROTOCOL_INVALID")]
    [InlineData(
        "Sync:Offline:AllowedActions must be a non-empty, unique subset of the typed sync action catalog.",
        "DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_F10_ACTIONS_INVALID")]
    [InlineData(
        "Sync:Retry:ClientTransport:MaxCount must be between 0 and 5.",
        "DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_F11_CLIENT_RETRY_COUNT_INVALID")]
    [InlineData(
        "Sync:Retry:ClientTransport:BaseSeconds must be explicitly set to 5.",
        "DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_F12_CLIENT_RETRY_BASE_INVALID")]
    [InlineData(
        "Sync:Retry:ClientTransport:MaxDelayMinutes must be explicitly set to 30.",
        "DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_F13_CLIENT_RETRY_DELAY_INVALID")]
    [InlineData(
        "Sync:Retry:ServerExecution:MaxCount must be between 0 and 5.",
        "DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_F14_SERVER_RETRY_COUNT_INVALID")]
    [InlineData(
        "Sync:Retry:ServerExecution:BaseSeconds must be explicitly set to 5.",
        "DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_F15_SERVER_RETRY_BASE_INVALID")]
    [InlineData(
        "Sync:Retry:ServerExecution:MaxDelayMinutes must be explicitly set to 30.",
        "DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_F16_SERVER_RETRY_DELAY_INVALID")]
    [InlineData(
        "Sync:Batch:MaxOperations must be explicitly set to 100.",
        "DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_F17_BATCH_SIZE_INVALID")]
    [InlineData(
        "Sync:Conflict:AutoMerge must be explicitly false.",
        "DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_F18_AUTO_MERGE_INVALID")]
    [InlineData(
        "Sync:Retention:LocalSuccessHours must be explicitly set to 24.",
        "DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_F19_LOCAL_SUCCESS_RETENTION_INVALID")]
    [InlineData(
        "Sync:Retention:LocalRejectedDays must be explicitly set to 7.",
        "DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_F20_LOCAL_REJECTED_RETENTION_INVALID")]
    [InlineData(
        "Sync:Retention:ServerPayloadDays must be explicitly set to 90.",
        "DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_F21_SERVER_PAYLOAD_RETENTION_INVALID")]
    [InlineData(
        "Sync:Cache:MaxAgeHours must be explicitly set to 24.",
        "DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_F22_CACHE_AGE_INVALID")]
    [InlineData(
        "Sync:Proof:MaximumRequestBodyBytes must be explicitly set to 2097152.",
        "DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_F23_REQUEST_BODY_LIMIT_INVALID")]
    [InlineData(
        "Sync:Proof:MaximumPayloadBytes must be explicitly set to 16384.",
        "DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_F24_PAYLOAD_LIMIT_INVALID")]
    [InlineData(
        "Sync payload limit cannot exceed the request body limit.",
        "DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_F25_PAYLOAD_EXCEEDS_REQUEST")]
    public void Exact_governed_sync_failure_maps_to_fixed_allowlisted_code(
        string failure,
        string expected)
    {
        var classifier = new DesktopReleaseKestrelApiHost.StartupFailureClassifier();

        classifier.Observe($"Unhandled exception. {SyncRuntimePolicyException}: {failure}");

        Assert.Equal(expected, classifier.Code);
    }

    [Fact]
    public void Ordered_known_sync_failures_map_to_a_bounded_allowlisted_combination()
    {
        var classifier = new DesktopReleaseKestrelApiHost.StartupFailureClassifier();

        classifier.Observe(
            $"Unhandled exception. {SyncRuntimePolicyException}: " +
            "Sync:Offline:Enabled must be explicitly configured.; " +
            "Sync:ServerExecution:Enabled must be explicitly configured.; " +
            "Sync:Conflict:AutoMerge must be explicitly false.");

        Assert.Equal(
            "DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_" +
            "F01_OFFLINE_ENABLED_REQUIRED_F02_SERVER_EXECUTION_REQUIRED_F18_AUTO_MERGE_INVALID",
            classifier.Code);
    }

    [Fact]
    public void Real_options_validation_exception_first_line_uses_the_governed_delimiter()
    {
        var exceptionType = typeof(TransportERP.Api.Sync.SyncRuntimePolicyOptions).Assembly.GetType(
            SyncRuntimePolicyException, throwOnError: true)!;
        var failures = new[]
        {
            "Sync:Offline:Enabled must be explicitly configured.",
            "Sync:ServerExecution:Enabled must be explicitly configured."
        };
        var exception = (Exception)Activator.CreateInstance(
            exceptionType,
            System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.NonPublic,
            binder: null,
            args: [failures],
            culture: null)!;
        var firstLine = exception.ToString().Split(Environment.NewLine, 2)[0];
        var classifier = new DesktopReleaseKestrelApiHost.StartupFailureClassifier();

        classifier.Observe("Unhandled exception. " + firstLine);

        Assert.Equal(
            "DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_" +
            "F01_OFFLINE_ENABLED_REQUIRED_F02_SERVER_EXECUTION_REQUIRED",
            classifier.Code);
    }

    [Theory]
    [InlineData(
        "Sync:Offline:Enabled must be explicitly configured.; Sync:Offline:Enabled must be explicitly configured.")]
    [InlineData(
        "Sync:Conflict:AutoMerge must be explicitly false.; Sync:Offline:Enabled must be explicitly configured.")]
    [InlineData("Sync:Offline:Enabled must be explicitly configured.; ")]
    [InlineData("Sync:Offline:Enabled must be explicitly configured.; UNKNOWN")]
    public void Duplicate_reversed_empty_or_unknown_sync_failure_stays_general(string failures)
    {
        var classifier = new DesktopReleaseKestrelApiHost.StartupFailureClassifier();

        classifier.Observe($"Unhandled exception. {SyncRuntimePolicyException}: {failures}");

        Assert.Equal("DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_VALIDATION", classifier.Code);
        Assert.DoesNotContain(failures, classifier.Code, StringComparison.Ordinal);
    }

    [Fact]
    public void Oversized_sync_failure_line_stays_general_without_retaining_input()
    {
        var classifier = new DesktopReleaseKestrelApiHost.StartupFailureClassifier();
        var oversized = new string('S', 4097);

        classifier.Observe($"Unhandled exception. {SyncRuntimePolicyException}: {oversized}");

        Assert.Equal("DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_VALIDATION", classifier.Code);
        Assert.DoesNotContain(oversized, classifier.Code, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("; OTHER_FAILURE")]
    [InlineData(" fake-bearer|fake-pfx-password|D:\\private\\api")]
    public void Sync_deployment_failure_with_any_suffix_stays_at_the_general_category(string suffix)
    {
        var classifier = new DesktopReleaseKestrelApiHost.StartupFailureClassifier();
        var failure =
            "Sync:Offline:ActivationImplementationSha must match the exact running API build.";

        classifier.Observe(
            $"Unhandled exception. {SyncRuntimePolicyException}: {failure}{suffix}");

        Assert.Equal("DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_VALIDATION", classifier.Code);
        Assert.DoesNotContain(suffix, classifier.Code, StringComparison.Ordinal);
    }

    [Fact]
    public void Fixed_sync_failure_text_under_an_unknown_type_remains_unclassified()
    {
        var classifier = new DesktopReleaseKestrelApiHost.StartupFailureClassifier();

        classifier.Observe(
            "Unhandled exception. Example.UnknownException: Sync:Offline:ActivationImplementationSha must match the exact running API build.");

        Assert.Equal("DESKTOP_E2E_API_STARTUP_UNCLASSIFIED", classifier.Code);
    }

    [Theory]
    [InlineData("Unhandled exception. Example.UnknownException: unknown")]
    [InlineData("System.InvalidOperationException: inner line")]
    [InlineData("message mentions Microsoft.Extensions.Options.OptionsValidationException: only")]
    [InlineData("Unhandled exception. System.InvalidOperationExceptionExtra: prefix spoof")]
    [InlineData("Unhandled exception. TransportERP.Api.Startup.SyncRuntimePolicyStartupOptionsValidationExceptionExtra: prefix spoof")]
    public void Unknown_inner_or_message_tokens_remain_unclassified(string line)
    {
        var classifier = new DesktopReleaseKestrelApiHost.StartupFailureClassifier();

        classifier.Observe(line);

        Assert.Equal("DESKTOP_E2E_API_STARTUP_UNCLASSIFIED", classifier.Code);
    }

    [Fact]
    public void Classification_never_returns_synthetic_secret_path_or_message()
    {
        const string secret = "bearer-secret|pfx-password|D:\\sensitive\\kestrel.pfx";
        var classifier = new DesktopReleaseKestrelApiHost.StartupFailureClassifier();

        classifier.Observe(
            $"Unhandled exception. TransportERP.Api.Startup.SyncRuntimePolicyStartupOptionsValidationException: {secret}");

        Assert.Equal("DESKTOP_E2E_API_STARTUP_SYNC_RUNTIME_POLICY_VALIDATION", classifier.Code);
        Assert.DoesNotContain(secret, classifier.Code, StringComparison.Ordinal);
    }

    [Fact]
    public void First_exact_top_level_allowlisted_type_wins()
    {
        var classifier = new DesktopReleaseKestrelApiHost.StartupFailureClassifier();

        classifier.Observe(
            "Unhandled exception. System.InvalidOperationException: outer");
        classifier.Observe(
            "Unhandled exception. System.Security.Cryptography.CryptographicException: later");

        Assert.Equal("DESKTOP_E2E_API_STARTUP_INVALID_OPERATION", classifier.Code);
    }

    [Fact]
    public void First_unknown_top_level_type_latches_unclassified()
    {
        var classifier = new DesktopReleaseKestrelApiHost.StartupFailureClassifier();

        classifier.Observe("Unhandled exception. Example.UnknownException: first");
        classifier.Observe(
            "Unhandled exception. System.InvalidOperationException: later");

        Assert.Equal("DESKTOP_E2E_API_STARTUP_UNCLASSIFIED", classifier.Code);
    }

    [Fact]
    public void Non_top_level_lines_do_not_preempt_the_first_top_level_type()
    {
        var classifier = new DesktopReleaseKestrelApiHost.StartupFailureClassifier();

        classifier.Observe("System.InvalidOperationException: inner-before-top-level");
        classifier.Observe(
            "Unhandled exception. System.FormatException: top-level");

        Assert.Equal("DESKTOP_E2E_API_STARTUP_FORMAT", classifier.Code);
    }

    [Fact]
    public async Task Bounded_normal_exit_accepts_only_exit_code_zero()
    {
        using var process = StartCommand("exit 0");

        await DesktopReleaseUiAutomation.WaitForNormalExitAsync(
            process, TimeSpan.FromSeconds(5), CancellationToken.None);

        Assert.True(process.HasExited);
        Assert.Equal(0, process.ExitCode);
    }

    [Fact]
    public async Task Bounded_normal_exit_rejects_a_nonzero_exit()
    {
        using var process = StartCommand("exit 7");

        var failure = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            DesktopReleaseUiAutomation.WaitForNormalExitAsync(
                process, TimeSpan.FromSeconds(5), CancellationToken.None));

        Assert.Equal("DESKTOP_E2E_NORMAL_CLOSE_NONZERO", failure.Message);
    }

    [Fact]
    public async Task Bounded_normal_exit_reports_its_phase_timeout()
    {
        using var process = StartLongRunningProcess();
        try
        {
            var failure = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                DesktopReleaseUiAutomation.WaitForNormalExitAsync(
                    process, TimeSpan.FromMilliseconds(100), CancellationToken.None));

            Assert.Equal("DESKTOP_E2E_NORMAL_CLOSE_TIMEOUT", failure.Message);
        }
        finally
        {
            await KillAsync(process);
        }
    }

    [Fact]
    public async Task Parent_cancellation_precedes_the_normal_close_timeout()
    {
        using var process = StartLongRunningProcess();
        using var parent = new CancellationTokenSource();
        parent.Cancel();
        try
        {
            var failure = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                DesktopReleaseUiAutomation.WaitForNormalExitAsync(
                    process, TimeSpan.FromSeconds(5), parent.Token));

            Assert.Equal("DESKTOP_E2E_PARENT_BUDGET_EXHAUSTED", failure.Message);
        }
        finally
        {
            await KillAsync(process);
        }
    }

    [Fact]
    public async Task Process_exit_before_a_close_request_is_not_acceptance()
    {
        using var process = StartCommand("exit 0");
        await process.WaitForExitAsync();

        var failure = Assert.Throws<InvalidOperationException>(() =>
            DesktopReleaseUiAutomation.RequireRunningBeforeClose(process));

        Assert.Equal("DESKTOP_E2E_PROCESS_EXITED_BEFORE_CLOSE", failure.Message);
    }

    private static Process StartCommand(string command)
    {
        var start = new ProcessStartInfo
        {
            FileName = Environment.GetEnvironmentVariable("ComSpec") ?? "cmd.exe",
            UseShellExecute = false,
            CreateNoWindow = true
        };
        start.ArgumentList.Add("/d");
        start.ArgumentList.Add("/c");
        start.ArgumentList.Add(command);
        return Process.Start(start)
            ?? throw new InvalidOperationException("DESKTOP_E2E_TEST_PROCESS_START_FAILED");
    }

    private static Process StartLongRunningProcess()
    {
        var start = new ProcessStartInfo
        {
            FileName = Path.Combine(Environment.SystemDirectory, "PING.EXE"),
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        start.ArgumentList.Add("-n");
        start.ArgumentList.Add("30");
        start.ArgumentList.Add("127.0.0.1");
        return Process.Start(start)
            ?? throw new InvalidOperationException("DESKTOP_E2E_TEST_PROCESS_START_FAILED");
    }

    private static async Task KillAsync(Process process)
    {
        if (process.HasExited) return;
        process.Kill(entireProcessTree: true);
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        await process.WaitForExitAsync(timeout.Token);
    }
}
