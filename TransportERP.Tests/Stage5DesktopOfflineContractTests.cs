namespace TransportERP.Tests;

public sealed class Stage5DesktopOfflineContractTests
{
    [Fact]
    public void Desktop_is_an_executable_with_closed_default_startup_and_encrypted_offline_core()
    {
        var project = Read("TransportERP.Desktop", "TransportERP.Desktop.csproj");
        var program = Read("TransportERP.Desktop", "Program.cs");
        var context = Read("TransportERP.Desktop", "Application", "DesktopApplicationContext.cs");
        var sessions = Read("TransportERP.Desktop", "Application", "DesktopAuthenticatedSessionBridge.cs");
        var shell = Read("TransportERP.Desktop", "Application", "DesktopShellForm.cs");

        Assert.Contains("<OutputType>WinExe</OutputType>", project, StringComparison.Ordinal);
        Assert.Contains("TransportERP.Offline\\TransportERP.Offline.csproj", project, StringComparison.Ordinal);
        Assert.Contains("[STAThread]", program, StringComparison.Ordinal);
        Assert.Contains("--startup-smoke", program, StringComparison.Ordinal);
        Assert.Contains("DesktopStartupContractProbe.VerifyClosedDefault() ? 0 : 1", program, StringComparison.Ordinal);
        Assert.Contains("OfflineRuntimeAuthorizedByDefault = false", context, StringComparison.Ordinal);
        Assert.Contains("ActivateAuthenticatedOfflineRuntimeAsync", context, StringComparison.Ordinal);
        Assert.Contains("new DesktopOnlineSignInSessionBridge()", program, StringComparison.Ordinal);
        Assert.Contains("new DesktopApplicationContext(authenticatedSessions)", program, StringComparison.Ordinal);
        Assert.Contains("_authenticatedSessions.SessionAuthenticated += OnSessionAuthenticated", context, StringComparison.Ordinal);
        Assert.Contains("_authenticatedSessions.Start()", context, StringComparison.Ordinal);
        Assert.Contains("_activationAttempted = true", context, StringComparison.Ordinal);
        Assert.Contains("activation.CreateRuntimeAsync", context, StringComparison.Ordinal);
        Assert.Contains("RunSyncSupervisorAsync", context, StringComparison.Ordinal);
        Assert.Contains("Enabled = false", shell, StringComparison.Ordinal);
        Assert.Contains("AttachAuthenticatedRuntime", shell, StringComparison.Ordinal);
        Assert.Contains("The bridge begins closed", sessions, StringComparison.Ordinal);
    }

    [Fact]
    public void Desktop_authenticated_activation_is_single_use_scope_bound_and_session_lifetime_bound()
    {
        var program = Read("TransportERP.Desktop", "Program.cs");
        var bridge = Read("TransportERP.Desktop", "Application", "DesktopAuthenticatedSessionBridge.cs");
        var context = Read("TransportERP.Desktop", "Application", "DesktopApplicationContext.cs");
        var shell = Read("TransportERP.Desktop", "Application", "DesktopShellForm.cs");

        Assert.Contains("DesktopAuthenticatedSessionScope AuthenticatedScope", bridge, StringComparison.Ordinal);
        Assert.Contains("DesktopAuthenticatedSessionScope AuthorizedOfflineScope", bridge, StringComparison.Ordinal);
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
        Assert.Contains("if (_activationAttempted || !IsGovernedActivation(activation))", context, StringComparison.Ordinal);
        Assert.Contains("_activeSessionId != sessionId", context, StringComparison.Ordinal);
        Assert.Contains("_activationCancellation?.Cancel()", context, StringComparison.Ordinal);
        Assert.Contains("_supervisorCancellation?.Cancel()", context, StringComparison.Ordinal);
        Assert.Contains("_runtime?.Dispose()", context, StringComparison.Ordinal);
        Assert.Contains("_shell.CloseForSessionEnd(reasonCode)", context, StringComparison.Ordinal);
        Assert.Contains("Close();", shell, StringComparison.Ordinal);

        Assert.DoesNotContain("Environment.GetEnvironmentVariable", program, StringComparison.Ordinal);
        Assert.DoesNotContain("BearerToken", bridge, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("DeviceCredential", bridge, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ProofJson", bridge, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ExportParameters", bridge, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("DesktopStartupContractProbe.VerifyClosedDefault", program, StringComparison.Ordinal);
        Assert.Contains("return activationCount == 0", bridge, StringComparison.Ordinal);
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

    private static string RepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "TransportERP.slnx")))
            directory = directory.Parent;

        return directory?.FullName
            ?? throw new DirectoryNotFoundException("TransportERP repository root was not found.");
    }
}
