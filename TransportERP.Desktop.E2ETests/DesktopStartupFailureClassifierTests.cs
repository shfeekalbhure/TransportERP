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
}
