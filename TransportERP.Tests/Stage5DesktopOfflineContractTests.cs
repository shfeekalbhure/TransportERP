namespace TransportERP.Tests;

public sealed class Stage5DesktopOfflineContractTests
{
    [Fact]
    public void Desktop_is_an_executable_with_closed_default_startup_and_encrypted_offline_core()
    {
        var project = Read("TransportERP.Desktop", "TransportERP.Desktop.csproj");
        var program = Read("TransportERP.Desktop", "Program.cs");
        var context = Read("TransportERP.Desktop", "Application", "DesktopApplicationContext.cs");
        var shell = Read("TransportERP.Desktop", "Application", "DesktopShellForm.cs");

        Assert.Contains("<OutputType>WinExe</OutputType>", project, StringComparison.Ordinal);
        Assert.Contains("TransportERP.Offline\\TransportERP.Offline.csproj", project, StringComparison.Ordinal);
        Assert.Contains("[STAThread]", program, StringComparison.Ordinal);
        Assert.Contains("--startup-smoke", program, StringComparison.Ordinal);
        Assert.Contains("OfflineRuntimeAuthorizedByDefault ? 1 : 0", program, StringComparison.Ordinal);
        Assert.Contains("OfflineRuntimeAuthorizedByDefault = false", context, StringComparison.Ordinal);
        Assert.Contains("ActivateAuthenticatedOfflineRuntime", context, StringComparison.Ordinal);
        Assert.Contains("RunSyncSupervisorAsync", context, StringComparison.Ordinal);
        Assert.Contains("Enabled = false", shell, StringComparison.Ordinal);
        Assert.Contains("AttachAuthenticatedRuntime", shell, StringComparison.Ordinal);
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

        foreach (var forbidden in new[] { "PayloadJson", "PayloadHash", "Proof", "Token", "Nonce", "Jti", "Credential" })
            Assert.DoesNotContain($"Column(nameof(SyncOperationDisplayRow.{forbidden})", form, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Retry_and_conflict_actions_enforce_state_and_injected_permission_before_side_effect()
    {
        var controller = Read("TransportERP.Desktop", "Offline", "SyncOperationsController.cs");

        Assert.Contains("operation.Status != OfflineOperationStatus.Failed", controller, StringComparison.Ordinal);
        Assert.Contains("_permissions.CanRetry(operation)", controller, StringComparison.Ordinal);
        Assert.Contains("operation.Status != OfflineOperationStatus.Conflict", controller, StringComparison.Ordinal);
        Assert.Contains("_permissions.CanResolveConflict(operation, decision)", controller, StringComparison.Ordinal);
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
        Assert.Contains("string.IsNullOrWhiteSpace(reason)", controller, StringComparison.Ordinal);
        Assert.Contains("CONFLICT_REASON_REQUIRED", controller, StringComparison.Ordinal);
        Assert.Contains("reason, baseVersion, cancellationToken", composition, StringComparison.Ordinal);
    }

    private static string Read(params string[] path) =>
        File.ReadAllText(Path.Combine([RepositoryRoot(), .. path]));

    private static string RepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "TransportERP.slnx")))
            directory = directory.Parent;

        return directory?.FullName
            ?? throw new DirectoryNotFoundException("TransportERP repository root was not found.");
    }
}
