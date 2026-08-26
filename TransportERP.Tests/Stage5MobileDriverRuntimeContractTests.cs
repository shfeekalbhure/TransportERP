namespace TransportERP.Tests;

public sealed class Stage5MobileDriverRuntimeContractTests
{
    [Fact]
    public void Driver_is_a_real_android_maui_executable_with_no_startup_enable_path()
    {
        var project = Read("TransportERP.Mobile.Driver", "TransportERP.Mobile.Driver.csproj");
        var program = Read("TransportERP.Mobile.Driver", "MauiProgram.cs");
        var activation = Read("TransportERP.Mobile.Driver", "Offline", "DriverOfflineActivationService.cs");

        Assert.Contains("<TargetFramework>net10.0-android</TargetFramework>", project, StringComparison.Ordinal);
        Assert.Contains("<OutputType>Exe</OutputType>", project, StringComparison.Ordinal);
        Assert.Contains("<UseMaui>true</UseMaui>", project, StringComparison.Ordinal);
        Assert.Contains("IDriverOfflineFeatureGate, DriverClosedOfflineFeatureGate", program, StringComparison.Ordinal);
        Assert.DoesNotContain("ActivateAsync(", program, StringComparison.Ordinal);
        Assert.Contains("!request.OfflineRuntimeAuthorized || !featureGate.IsOfflineRuntimeAuthorized",
            activation, StringComparison.Ordinal);
        Assert.Contains("RunSyncSupervisorAsync", activation, StringComparison.Ordinal);
        Assert.Contains("_supervisorCancellation?.Cancel()", activation, StringComparison.Ordinal);
        Assert.Contains("volatileSession.Clear()", activation, StringComparison.Ordinal);
        Assert.Contains("public sealed class DriverOfflineActivationRequest", activation, StringComparison.Ordinal);
        Assert.DoesNotContain("record DriverOfflineActivationRequest", activation, StringComparison.Ordinal);
    }

    [Fact]
    public void Android_native_security_is_purpose_separated_non_exportable_and_fail_closed()
    {
        var storage = Read("TransportERP.Mobile.Driver", "Platforms", "Android",
            "AndroidSecureStorageEncryptionKeyProvider.cs");
        var signer = Read("TransportERP.Mobile.Driver", "Platforms", "Android",
            "AndroidKeystoreDeviceSigningKey.cs");
        var manifest = Read("TransportERP.Mobile.Driver", "Platforms", "Android", "AndroidManifest.xml");

        Assert.Contains("SecureStorage.Default", storage, StringComparison.Ordinal);
        Assert.Contains("sqlcipher.outbox.v1", storage, StringComparison.Ordinal);
        Assert.Contains("sqlcipher.readcache.v1", storage, StringComparison.Ordinal);
        Assert.Contains("!CryptographicOperations.FixedTimeEquals(outbox, readCache)", storage,
            StringComparison.Ordinal);
        Assert.DoesNotContain("Preferences.", storage, StringComparison.Ordinal);
        Assert.DoesNotContain("File.Write", storage, StringComparison.Ordinal);

        Assert.Contains("AndroidKeyStore", signer, StringComparison.Ordinal);
        Assert.Contains("KeyStorePurpose.Sign | KeyStorePurpose.Verify", signer, StringComparison.Ordinal);
        Assert.Contains("ECGenParameterSpec(\"secp256r1\")", signer, StringComparison.Ordinal);
        Assert.Contains("DerToP1363", signer, StringComparison.Ordinal);
        Assert.Contains("new byte[64]", signer, StringComparison.Ordinal);
        Assert.DoesNotContain("ExportParameters(includePrivateParameters: true)", signer,
            StringComparison.Ordinal);
        Assert.DoesNotContain("ExportPkcs8PrivateKey", signer, StringComparison.Ordinal);

        Assert.Contains("android:allowBackup=\"false\"", manifest, StringComparison.Ordinal);
        Assert.Contains("android:usesCleartextTraffic=\"false\"", manifest, StringComparison.Ordinal);
    }

    [Fact]
    public void Android_runtime_self_test_is_compile_gated_sanitized_and_exercises_process_restart()
    {
        var project = Read("TransportERP.Mobile.Driver", "TransportERP.Mobile.Driver.csproj");
        var activity = Read("TransportERP.Mobile.Driver", "Platforms", "Android",
            "DriverDeviceTestActivity.cs");
        var selfTest = Read("TransportERP.Mobile.Driver", "DeviceTesting",
            "AndroidDriverRuntimeSelfTest.cs");
        var workflow = Read(".github", "workflows", "ci.yml");

        Assert.Contains("Condition=\"'$(TransportERPDeviceTests)' == 'true'\"", project,
            StringComparison.Ordinal);
        Assert.Contains("TRANSPORTERP_DEVICE_TESTS", project, StringComparison.Ordinal);
        Assert.True(activity.StartsWith("#if TRANSPORTERP_DEVICE_TESTS", StringComparison.Ordinal));
        Assert.True(selfTest.StartsWith("#if TRANSPORTERP_DEVICE_TESTS", StringComparison.Ordinal));
        Assert.Contains("IDriverOfflineFeatureGate", selfTest, StringComparison.Ordinal);
        Assert.Contains("activation.Active is null", selfTest, StringComparison.Ordinal);
        Assert.Contains("PrivateSigningKeyIsNonExportable", selfTest, StringComparison.Ordinal);
        Assert.Contains("VerifyP1363ForDeviceTestAsync", selfTest, StringComparison.Ordinal);
        Assert.Contains("VerifyP1363ForDeviceTestAsync", signer, StringComparison.Ordinal);
        Assert.Contains("P1363ToDer", signer, StringComparison.Ordinal);
        Assert.Contains("verifier.InitVerify(publicKey)", signer, StringComparison.Ordinal);
        Assert.Contains("VerifySealedProbe", selfTest, StringComparison.Ordinal);
        Assert.DoesNotContain("ActivateAsync", selfTest, StringComparison.Ordinal);
        Assert.DoesNotContain("SessionBearer", selfTest, StringComparison.Ordinal);

        Assert.Contains("runs-on: ubuntu-24.04", workflow, StringComparison.Ordinal);
        Assert.Contains("system-images;android-35;google_apis;x86_64", workflow,
            StringComparison.Ordinal);
        Assert.Contains("adb shell am force-stop", workflow, StringComparison.Ordinal);
        Assert.Contains("run_phase startup", workflow, StringComparison.Ordinal);
        Assert.Contains("run_phase seed", workflow, StringComparison.Ordinal);
        Assert.Contains("run_phase verify", workflow, StringComparison.Ordinal);
        Assert.Contains("run_phase loss", workflow, StringComparison.Ordinal);
        Assert.Contains("offline_runtime_enabled=false", workflow, StringComparison.Ordinal);
        Assert.Contains("The test-only Android activity leaked into the Release APK", workflow,
            StringComparison.Ordinal);
    }

    [Fact]
    public void Android_registered_device_key_binding_fails_closed_for_loss_mismatch_and_unverified_state()
    {
        var program = Read("TransportERP.Mobile.Driver", "MauiProgram.cs");
        var activation = Read("TransportERP.Mobile.Driver", "Offline",
            "DriverOfflineActivationService.cs");
        var binding = Read("TransportERP.Mobile.Driver", "Offline", "DriverDeviceKeyBinding.cs");
        var signer = Read("TransportERP.Mobile.Driver", "Platforms", "Android",
            "AndroidKeystoreDeviceSigningKey.cs");
        var selfTest = Read("TransportERP.Mobile.Driver", "DeviceTesting",
            "AndroidDriverRuntimeSelfTest.cs");

        Assert.Contains(
            "IDriverDeviceKeyBindingVerifier, DriverClosedDeviceKeyBindingVerifier",
            program,
            StringComparison.Ordinal);
        Assert.Contains("DriverDeviceKeyBindingGuard.RequireMatchAsync", activation,
            StringComparison.Ordinal);
        Assert.True(
            activation.IndexOf("DriverDeviceKeyBindingGuard.RequireMatchAsync", StringComparison.Ordinal) <
            activation.IndexOf("volatileSession.Set(request.SessionBearer)", StringComparison.Ordinal));

        Assert.Contains("DriverDeviceKeyBindingDecision.Match => null", binding,
            StringComparison.Ordinal);
        Assert.Contains("RegisteredBindingMissing => \"DEVICE_KEY_REBIND_REQUIRED\"", binding,
            StringComparison.Ordinal);
        Assert.Contains("Mismatch => \"DEVICE_KEY_ROTATION_REQUIRED\"", binding,
            StringComparison.Ordinal);
        Assert.Contains("VerificationUnavailable => \"DEVICE_KEY_BINDING_VERIFICATION_REQUIRED\"",
            binding, StringComparison.Ordinal);
        Assert.DoesNotContain("HttpClient", binding, StringComparison.Ordinal);
        Assert.DoesNotContain("http://", binding, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("https://", binding, StringComparison.OrdinalIgnoreCase);

        Assert.Contains("RequireExistingKeyAsync", signer, StringComparison.Ordinal);
        Assert.Contains("DEVICE_KEY_REBIND_REQUIRED", signer, StringComparison.Ordinal);
        Assert.DoesNotContain("EnsureKeyAsync", signer, StringComparison.Ordinal);
        Assert.Contains("#if TRANSPORTERP_DEVICE_TESTS", signer, StringComparison.Ordinal);
        Assert.Contains("ProvisionFreshKeyForDeviceTestAsync", signer, StringComparison.Ordinal);
        var testGateStart = signer.IndexOf("#if TRANSPORTERP_DEVICE_TESTS", StringComparison.Ordinal);
        var generation = signer.IndexOf("GenerateKeyPair", StringComparison.Ordinal);
        var testGateEnd = signer.IndexOf("#endif", testGateStart, StringComparison.Ordinal);
        Assert.True(testGateStart >= 0 && generation > testGateStart && generation < testGateEnd);
        Assert.DoesNotContain("GenerateKeyPair", signer[..testGateStart], StringComparison.Ordinal);

        Assert.Contains("matching_registered_binding_accepted", selfTest, StringComparison.Ordinal);
        Assert.Contains("mismatched_registered_binding_requires_rotation", selfTest,
            StringComparison.Ordinal);
        Assert.Contains("missing_registered_binding_requires_rebind", selfTest,
            StringComparison.Ordinal);
        Assert.Contains("missing_alias_was_not_reprovisioned", selfTest, StringComparison.Ordinal);
        Assert.Contains("unavailable_binding_verification_fails_closed", selfTest,
            StringComparison.Ordinal);
        Assert.Contains("default_device_key_binding_verifier_is_closed", selfTest,
            StringComparison.Ordinal);
        Assert.Contains("IsNativeSecureStorageAvailableAsync", selfTest, StringComparison.Ordinal);
        Assert.Contains("new OfflineOperationStore(DeviceTestOutboxPath(), encryptionKeys)", selfTest,
            StringComparison.Ordinal);
        Assert.Contains("sqlcipher_outbox_operation_survived_restart", selfTest,
            StringComparison.Ordinal);
        Assert.Contains("sqlcipher_outbox_identity_preserved", selfTest, StringComparison.Ordinal);
        Assert.Contains("sqlcipher_outbox_status_preserved", selfTest, StringComparison.Ordinal);
        Assert.Contains("sqlcipher_outbox_payload_hash_preserved", selfTest,
            StringComparison.Ordinal);
        Assert.Contains("sqlcipher_outbox_replay_did_not_duplicate", selfTest,
            StringComparison.Ordinal);
        Assert.Contains("SQLCIPHER_OUTBOX_INITIALIZATION_FAILED", selfTest,
            StringComparison.Ordinal);
        Assert.Contains("NATIVE_SIGNING_KEY_TEST_PROVISION_FAILED", selfTest,
            StringComparison.Ordinal);
        Assert.DoesNotContain("ActivateAsync", selfTest, StringComparison.Ordinal);
    }

    [Fact]
    public void Driver_operation_surface_is_scope_permission_and_activation_bound_with_conflict_runtime()
    {
        var app = Read("TransportERP.Mobile.Driver", "App.cs");
        var page = Read("TransportERP.Mobile.Driver", "MainPage.cs");
        var activation = Read("TransportERP.Mobile.Driver", "Offline",
            "DriverOfflineActivationService.cs");
        var composition = Read("TransportERP.Mobile.Driver", "Offline",
            "DriverOfflineComposition.cs");

        Assert.Contains("new MainPage(_activation)", app, StringComparison.Ordinal);
        Assert.DoesNotContain("ActivateAsync", app, StringComparison.Ordinal);
        Assert.Contains("Offline runtime: CLOSED", page, StringComparison.Ordinal);
        Assert.Contains("Reason: OFFLINE_CLOSED", page, StringComparison.Ordinal);
        Assert.Contains("CollectionView", page, StringComparison.Ordinal);
        Assert.Contains("Refresh operation status", page, StringComparison.Ordinal);
        Assert.Contains("Manual retry", page, StringComparison.Ordinal);
        Assert.Contains("KEEP_SERVER", page, StringComparison.Ordinal);
        Assert.Contains("REAPPLY", page, StringComparison.Ordinal);
        Assert.Contains("Resolution reason (required)", page, StringComparison.Ordinal);
        Assert.Contains("CanRetryFailedOperations", page, StringComparison.Ordinal);
        Assert.Contains("CanResolveConflicts", page, StringComparison.Ordinal);
        Assert.Contains("SafeCode(exception)", page, StringComparison.Ordinal);
        Assert.Contains("OPERATION_FAILED", page, StringComparison.Ordinal);
        Assert.DoesNotContain("exception.Message", page, StringComparison.Ordinal);
        Assert.DoesNotContain("PayloadJson", page, StringComparison.Ordinal);
        Assert.DoesNotContain("PayloadHash", page, StringComparison.Ordinal);
        Assert.DoesNotContain("ActivateAsync", page, StringComparison.Ordinal);

        Assert.Contains("DriverOfflineOperationPermissions operationPermissions", activation,
            StringComparison.Ordinal);
        Assert.Contains("request.OperationPermissions", activation, StringComparison.Ordinal);
        Assert.Contains("StateChanged", activation, StringComparison.Ordinal);
        Assert.Contains("OFFLINE_CLOSED", activation, StringComparison.Ordinal);
        Assert.True(
            activation.IndexOf("!request.OfflineRuntimeAuthorized || !featureGate.IsOfflineRuntimeAuthorized",
                StringComparison.Ordinal) <
            activation.IndexOf("DriverDeviceKeyBindingGuard.RequireMatchAsync", StringComparison.Ordinal));

        Assert.Contains("new OfflineSyncConflictClient", composition, StringComparison.Ordinal);
        Assert.Contains("ListAsync(_scope", composition, StringComparison.Ordinal);
        Assert.Contains("RequeueFailedAsync(localOperationId, _scope", composition,
            StringComparison.Ordinal);
        Assert.Contains("ResolveConflictAsync", composition, StringComparison.Ordinal);
        Assert.Contains("SYNC_OPERATION_RETRY_NOT_AUTHORIZED", composition, StringComparison.Ordinal);
        Assert.Contains("SYNC_CONFLICT_RESOLVE_NOT_AUTHORIZED", composition, StringComparison.Ordinal);
        Assert.Contains("OperationPermissions.CanRetryFailedOperations", composition,
            StringComparison.Ordinal);
        Assert.Contains("OperationPermissions.CanResolveConflicts", composition,
            StringComparison.Ordinal);
        Assert.Contains("SanitizeResultCode(operation.ResultCode)", composition,
            StringComparison.Ordinal);
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
