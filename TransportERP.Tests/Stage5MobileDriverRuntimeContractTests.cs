namespace TransportERP.Tests;

public sealed class Stage5MobileDriverRuntimeContractTests
{
    [Fact]
    public void Driver_is_a_real_android_maui_executable_with_no_startup_enable_path()
    {
        var project = Read("TransportERP.Mobile.Driver", "TransportERP.Mobile.Driver.csproj");
        var program = Read("TransportERP.Mobile.Driver", "MauiProgram.cs");
        var activation = Read("TransportERP.Mobile.Driver", "Offline", "DriverOfflineActivationService.cs");
        var authenticated = Read("TransportERP.Mobile.Driver", "Offline",
            "DriverAuthenticatedActivationCoordinator.cs");
        var authority = Read("TransportERP.Application", "Sync", "SyncClientDeploymentAuthority.cs");
        var applicationProject = Read("TransportERP.Application", "TransportERP.Application.csproj");

        Assert.Contains("<TargetFramework>net10.0-android</TargetFramework>", project, StringComparison.Ordinal);
        Assert.Contains("<OutputType>Exe</OutputType>", project, StringComparison.Ordinal);
        Assert.Contains("<UseMaui>true</UseMaui>", project, StringComparison.Ordinal);
        Assert.Contains("TransportERP.Application.csproj", project, StringComparison.Ordinal);
        Assert.Contains("DriverServerOfflineFeatureGate", program, StringComparison.Ordinal);
        Assert.Contains("DriverAuthenticatedActivationCoordinator", program, StringComparison.Ordinal);
        Assert.DoesNotContain("ActivateAsync(", program, StringComparison.Ordinal);
        Assert.Contains("!request.OfflineRuntimeAuthorized || !featureGate.Allows(bindingContext)",
            activation, StringComparison.Ordinal);
        Assert.Contains("/api/v1/auth/sessions", authenticated, StringComparison.Ordinal);
        Assert.Contains("/api/v1/sync/activation", authenticated, StringComparison.Ordinal);
        Assert.Contains("ValidateDecision(request, session, decision, measuredBuildIdentity)", authenticated,
            StringComparison.Ordinal);
        Assert.Contains("session.CompanyId != request.CompanyId", authenticated, StringComparison.Ordinal);
        Assert.Contains("session.BranchId != request.BranchId", authenticated, StringComparison.Ordinal);
        Assert.Contains("request.CompanyId is null", authenticated, StringComparison.Ordinal);
        Assert.Contains("request.BranchId is null", authenticated, StringComparison.Ordinal);
        Assert.Contains("SyncClientDeploymentAuthority.Origin", authenticated, StringComparison.Ordinal);
        Assert.Contains("SameOrigin(SyncClientDeploymentAuthority.Origin, decision.BatchEndpoint)", authenticated,
            StringComparison.Ordinal);
        Assert.Contains("public static class SyncClientDeploymentAuthority", authority, StringComparison.Ordinal);
        Assert.Contains("GetCustomAttributes<AssemblyMetadataAttribute>()", authority, StringComparison.Ordinal);
        Assert.Contains("TransportERPClientPublicOrigin", applicationProject, StringComparison.Ordinal);
        Assert.Contains("https://sync.example.test/", applicationProject, StringComparison.Ordinal);
        Assert.DoesNotContain("Environment", authority, StringComparison.Ordinal);
        Assert.DoesNotContain("IConfiguration", authority, StringComparison.Ordinal);
        Assert.DoesNotContain("ServerOrigin", authenticated, StringComparison.Ordinal);
        Assert.DoesNotContain("Environment.GetEnvironmentVariable", authenticated, StringComparison.Ordinal);
        Assert.Contains("MaximumJsonResponseBytes = 65_536", authenticated, StringComparison.Ordinal);
        Assert.Contains("ReadAsStreamAsync", authenticated, StringComparison.Ordinal);
        Assert.Contains("SERVER_RESPONSE_TOO_LARGE", authenticated, StringComparison.Ordinal);
        Assert.Contains("ArmExpiry(session.AccessTokenExpiresAt)", authenticated, StringComparison.Ordinal);
        Assert.Contains("PolicySourceFingerprint", authenticated, StringComparison.Ordinal);
        Assert.Contains("IsSha256Hex(decision.PolicySourceFingerprint)", authenticated,
            StringComparison.Ordinal);
        Assert.Contains("decision.ProofKeyVersion is <= 0", authenticated, StringComparison.Ordinal);
        Assert.Contains("HasOnlySupportedUniqueActions(decision.AllowedActions)", authenticated,
            StringComparison.Ordinal);
        Assert.Contains("SyncActionCatalog.Definitions", authenticated, StringComparison.Ordinal);
        Assert.Contains("SyncActionRuntimeAvailability.Available", authenticated, StringComparison.Ordinal);
        Assert.Contains("!actual.Add((action.ActionCode, action.OperationType, action.EntityType))", authenticated,
            StringComparison.Ordinal);
        Assert.Contains("!supported.Contains((action.ActionCode, action.OperationType, action.EntityType))", authenticated,
            StringComparison.Ordinal);
        Assert.Contains("decision.ClosedReason is not null", authenticated, StringComparison.Ordinal);
        Assert.Contains("decision.ClosedReason != \"PROOF_KEY_BINDING_REQUIRED\"", authenticated,
            StringComparison.Ordinal);
        Assert.Contains("/api/v1/auth/sessions/{session.SessionId:D}:revoke", authenticated,
            StringComparison.Ordinal);
        Assert.Contains("RunSyncSupervisorAsync", activation, StringComparison.Ordinal);
        Assert.Contains("_supervisorCancellation?.Cancel()", activation, StringComparison.Ordinal);
        Assert.Contains("volatileSession.Clear()", activation, StringComparison.Ordinal);
        Assert.Contains("public sealed class DriverOfflineActivationRequest", activation, StringComparison.Ordinal);
        Assert.DoesNotContain("record DriverOfflineActivationRequest", activation, StringComparison.Ordinal);
    }

    [Fact]
    public void Android_release_UI_E2E_uses_the_ordinary_non_debuggable_launcher_and_only_stable_automation_ids()
    {
        var page = Read("TransportERP.Mobile.Driver", "MainPage.cs");
        var script = Read("eng", "ci", "android_release_ui_e2e.py");

        Assert.Contains("Content = new ScrollView", page, StringComparison.Ordinal);
        Assert.Contains("AutomationId = \"driver_main_scroll\"", page, StringComparison.Ordinal);
        foreach (var automationId in new[]
                 {
                     "driver_mode", "driver_reason", "driver_user_name", "driver_password",
                     "driver_company_id", "driver_branch_id", "driver_device_id",
                     "driver_device_credential", "driver_sign_in", "driver_sign_out",
                     "driver_party_name", "driver_party_mobile", "driver_party_address",
                     "driver_queue_party", "driver_operation_list", "driver_operation_summary"
                 })
            Assert.Contains($"\"{automationId}\"", page, StringComparison.Ordinal);

        Assert.Contains("android.intent.category.LAUNCHER", script, StringComparison.Ordinal);
        Assert.Contains("\"--components\"", script, StringComparison.Ordinal);
        Assert.DoesNotContain("\"--brief\"", script, StringComparison.Ordinal);
        Assert.Contains("INSTALLED_PACKAGE_IS_DEBUGGABLE", script, StringComparison.Ordinal);
        Assert.Contains("uiautomator", script, StringComparison.Ordinal);
        Assert.Contains("ordinary-launcher", script, StringComparison.Ordinal);
        Assert.Contains("businessOperationSucceeded", script, StringComparison.Ordinal);
        Assert.Contains("persistedAfterReleaseRestart", script, StringComparison.Ordinal);
        foreach (var phase in new[]
                 {
                     "INITIAL_LAUNCH", "INITIAL_CLOSED_MODE", "INITIAL_CLOSED_REASON",
                     "INITIAL_SIGN_IN", "INITIAL_OPERATION_LIST", "QUEUE_PARTY_NAME",
                     "QUEUE_PARTY_MOBILE", "QUEUE_PARTY_ADDRESS", "QUEUE_PARTY_ACTION",
                     "QUEUE_PARTY_RESULT", "NEW_OPERATION_VISIBLE", "INITIAL_OPERATION_SUCCESS",
                     "RESTART_FORCE_STOP", "RESTART_LAUNCH", "RESTART_CLOSED_MODE",
                     "RESTART_SIGN_IN", "PERSISTED_OPERATION_SUCCESS", "SIGN_OUT_ACTION",
                     "SIGN_OUT_CLOSED_MODE"
                 })
            Assert.Contains($"phase = \"{phase}\"", script, StringComparison.Ordinal);
        Assert.Contains("raise UiE2EFailure(f\"{phase}:{error}\") from error", script,
            StringComparison.Ordinal);
        foreach (var signInPhase in new[]
                 {
                     "USER_NAME", "PASSWORD", "COMPANY_ID", "BRANCH_ID", "DEVICE_ID",
                     "DEVICE_CREDENTIAL", "HIDE_KEYBOARD", "SUBMIT", "ACTION_RESULT",
                     "MODE_READY"
                 })
            Assert.Contains($"phase = \"{signInPhase}\"", script, StringComparison.Ordinal);
        Assert.Contains("re.fullmatch(r\"Result: ([A-Z0-9_]{1,64})\"", script,
            StringComparison.Ordinal);
        Assert.Contains("re.fullmatch(r\"Offline runtime: (CLOSED|READY)\"", script,
            StringComparison.Ordinal);
        Assert.Contains("OBSERVATION_UNAVAILABLE", script, StringComparison.Ordinal);
        Assert.DoesNotContain("{phase}:{value}", script, StringComparison.Ordinal);
        Assert.Contains("UI_TEXT_VERIFY_ELEMENT_COUNT_INVALID", script, StringComparison.Ordinal);
        Assert.Contains("UI_TEXT_VERIFY_{text_state}_{focus_state}", script,
            StringComparison.Ordinal);
        Assert.Contains("self.focus_input(automation_id)", script, StringComparison.Ordinal);
        Assert.Contains("def focus_input(self, automation_id: str)", script,
            StringComparison.Ordinal);
        Assert.Contains("node.attrib.get(\"focused\") == \"true\"", script,
            StringComparison.Ordinal);
        Assert.Contains("UI_INPUT_FOCUS_FAILED", script, StringComparison.Ordinal);
        Assert.Contains("for attempt in range(5)", script, StringComparison.Ordinal);
        Assert.Contains("def safe_focus_observation(self, automation_id: str)", script,
            StringComparison.Ordinal);
        foreach (var focusDiagnostic in new[]
                 {
                     "COUNT_UNKNOWN", "VISIBLE_UNKNOWN", "FOCUSABLE_UNKNOWN", "CLICKABLE_UNKNOWN",
                     "OWNER_UNKNOWN", "ZONE_UNKNOWN", "IME_UNKNOWN", "FOCUS_OWNER_ALLOWLIST"
                 })
            Assert.Contains(focusDiagnostic, script, StringComparison.Ordinal);
        var focusObservationStart = script.IndexOf(
            "    def safe_focus_observation(self, automation_id: str)", StringComparison.Ordinal);
        var focusObservationEnd = script.IndexOf(
            "    def hide_keyboard(self)", focusObservationStart, StringComparison.Ordinal);
        Assert.True(focusObservationStart >= 0 && focusObservationEnd > focusObservationStart);
        var focusObservation = script[focusObservationStart..focusObservationEnd];
        Assert.DoesNotContain("attrib.get(\"text\")", focusObservation, StringComparison.Ordinal);
        Assert.DoesNotContain("completed.stdout", focusObservation, StringComparison.Ordinal);
        Assert.Contains("observed == value", script, StringComparison.Ordinal);
        Assert.Contains("else \"EMPTY\" if not observed else \"MISMATCH\"", script,
            StringComparison.Ordinal);
        Assert.DoesNotContain("{observed}", script, StringComparison.Ordinal);
        Assert.Contains("driver.run(\"shell\", \"am\", \"force-stop\"", script,
            StringComparison.Ordinal);
        Assert.Contains("driver.wait_for_operation_success(operation_id)", script,
            StringComparison.Ordinal);
        Assert.Contains("signedOutClosed", script, StringComparison.Ordinal);
        Assert.DoesNotContain("DriverDeviceTestActivity", script, StringComparison.Ordinal);
        Assert.DoesNotContain("DangerousAcceptAnyServerCertificateValidator", script,
            StringComparison.Ordinal);
        Assert.DoesNotContain("http://", script, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("https://", script, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("run-as", script, StringComparison.Ordinal);
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
        Assert.Contains("GetInstance(\"SHA256withECDSA\")", signer, StringComparison.Ordinal);
        Assert.DoesNotContain("SHA256withECDSAinP1363Format", signer, StringComparison.Ordinal);
        Assert.Contains("X509EncodedKeySpec", signer, StringComparison.Ordinal);
        Assert.DoesNotContain("ExportParameters(includePrivateParameters: true)", signer,
            StringComparison.Ordinal);
        Assert.DoesNotContain("ExportPkcs8PrivateKey", signer, StringComparison.Ordinal);
        Assert.Contains("ProvisionForAuthorizedEnrollmentAsync", signer, StringComparison.Ordinal);
        Assert.Contains("ReplaceForAuthorizedRecoveryAsync", signer, StringComparison.Ordinal);
        Assert.Contains("DriverDeviceKeyEnrollmentAuthorization", signer, StringComparison.Ordinal);

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
        var composition = Read("TransportERP.Mobile.Driver", "Offline", "DriverOfflineComposition.cs");
        var signer = Read("TransportERP.Mobile.Driver", "Platforms", "Android",
            "AndroidKeystoreDeviceSigningKey.cs");
        var network = Read("TransportERP.Mobile.Driver", "Platforms", "Android",
            "AndroidDriverSyncNetworkProvider.cs");
        var workflow = Read(".github", "workflows", "ci.yml");

        Assert.Contains("Condition=\"'$(TransportERPDeviceTests)' == 'true'\"", project,
            StringComparison.Ordinal);
        Assert.Contains("TRANSPORTERP_DEVICE_TESTS", project, StringComparison.Ordinal);
        Assert.True(activity.StartsWith("#if TRANSPORTERP_DEVICE_TESTS", StringComparison.Ordinal));
        Assert.True(selfTest.StartsWith("#if TRANSPORTERP_DEVICE_TESTS", StringComparison.Ordinal));
        Assert.Contains("IDriverOfflineFeatureGate", selfTest, StringComparison.Ordinal);
        Assert.Contains("activation.Active is null", selfTest, StringComparison.Ordinal);
        Assert.Contains("PrivateSigningKeyIsNonExportable", selfTest, StringComparison.Ordinal);
        Assert.Contains("VerifyP1363ForDeviceTestAsync", signer, StringComparison.Ordinal);
        Assert.Contains("P1363ToDer", signer, StringComparison.Ordinal);
        Assert.Contains("java_der_signature_verified", selfTest, StringComparison.Ordinal);
        Assert.Contains("der_p1363_round_trip_verified", selfTest, StringComparison.Ordinal);
        Assert.Contains("public_jwk_matches_keystore_certificate", selfTest, StringComparison.Ordinal);
        Assert.Contains("rawSigner.InitSign(privateKey)", signer, StringComparison.Ordinal);
        Assert.Contains("verifier.InitVerify(publicKey)", signer, StringComparison.Ordinal);
        Assert.Contains("VerifySealedProbe", selfTest, StringComparison.Ordinal);
        Assert.Contains("SubmitBusinessOperationEndToEndAsync", selfTest, StringComparison.Ordinal);
        Assert.Contains("SignInAndActivateAsync", selfTest, StringComparison.Ordinal);
        Assert.Contains("QueueOperationalPartyAsync", selfTest, StringComparison.Ordinal);
        Assert.DoesNotContain("ProcessNextBatchAsync", selfTest, StringComparison.Ordinal);
        Assert.Contains("supervisor.SynchronizeNowAsync", composition, StringComparison.Ordinal);
        Assert.Contains("terminal?.ServerAccepted == true", selfTest, StringComparison.Ordinal);
        Assert.Contains("http_batch_server_acceptance_persisted", selfTest, StringComparison.Ordinal);
        Assert.Contains("terminal, requestedCycle, activated.Runtime.LastSyncSupervisorFailure", selfTest,
            StringComparison.Ordinal);
        Assert.Contains("public OfflineSyncSupervisorFailure? LastSyncSupervisorFailure", composition,
            StringComparison.Ordinal);
        Assert.Contains("local_succeeded_survived_restart", selfTest, StringComparison.Ordinal);
        Assert.DoesNotContain("SessionBearer", selfTest, StringComparison.Ordinal);

        Assert.Contains("runs-on: ubuntu-24.04", workflow, StringComparison.Ordinal);
        Assert.Contains("system-images;android-35;google_apis;x86_64", workflow,
            StringComparison.Ordinal);
        Assert.Contains("adb_root_bounded()", workflow, StringComparison.Ordinal);
        Assert.Contains("for attempt in $(seq 1 6)", workflow, StringComparison.Ordinal);
        Assert.Contains("timeout 10s adb root >/dev/null 2>&1", workflow,
            StringComparison.Ordinal);
        Assert.Contains("timeout 10s adb shell id -u", workflow, StringComparison.Ordinal);
        Assert.Contains("ANDROID_EMULATOR_ADB_ROOT_UNAVAILABLE", workflow,
            StringComparison.Ordinal);
        Assert.DoesNotContain("          adb root\n", workflow, StringComparison.Ordinal);
        Assert.Contains("adb_bounded() { timeout 30s adb", workflow, StringComparison.Ordinal);
        Assert.Contains("app_pid()", workflow, StringComparison.Ordinal);
        Assert.Contains("| tr -d '\\r' || true", workflow, StringComparison.Ordinal);
        Assert.Contains("cmd package resolve-activity --components", workflow, StringComparison.Ordinal);
        Assert.DoesNotContain("cmd package resolve-activity --brief", workflow, StringComparison.Ordinal);
        Assert.Contains("Android launcher resolution returned an unexpected line count.", workflow,
            StringComparison.Ordinal);
        Assert.Contains("Android launcher resolution returned an invalid component.", workflow,
            StringComparison.Ordinal);
        Assert.Contains("grep -Fx 'Status: ok'", workflow, StringComparison.Ordinal);
        Assert.DoesNotContain("adb_bounded shell monkey", workflow, StringComparison.Ordinal);
        Assert.Contains("timeout-minutes: 12", workflow, StringComparison.Ordinal);
        Assert.Contains("adb_bounded shell am force-stop", workflow, StringComparison.Ordinal);
        Assert.Contains("run_phase startup", workflow, StringComparison.Ordinal);
        Assert.Contains("run_phase seed", workflow, StringComparison.Ordinal);
        Assert.Contains("run_phase verify", workflow, StringComparison.Ordinal);
        Assert.Contains("run_phase e2e-submit", workflow, StringComparison.Ordinal);
        Assert.Contains("run_phase e2e-verify", workflow, StringComparison.Ordinal);
        Assert.Contains("run_phase loss", workflow, StringComparison.Ordinal);
        Assert.Contains("image: postgres:18.6-bookworm", workflow, StringComparison.Ordinal);
        Assert.Contains("Verify Android business result in PostgreSQL 18.6", workflow,
            StringComparison.Ordinal);
        Assert.Contains("TransportERPDeviceTestServerCertificateSha256", workflow,
            StringComparison.Ordinal);
        Assert.Contains("ValidatePinnedDeviceTestCertificate", network, StringComparison.Ordinal);
        Assert.Contains("#if TRANSPORTERP_DEVICE_TESTS", network, StringComparison.Ordinal);
        Assert.DoesNotContain("DangerousAcceptAnyServerCertificateValidator", network,
            StringComparison.Ordinal);
        Assert.Contains("adb_bounded exec-out run-as", workflow, StringComparison.Ordinal);
        Assert.Contains("adb_bounded shell run-as \"$ANDROID_TEST_PACKAGE\" dd", workflow,
            StringComparison.Ordinal);
        Assert.DoesNotContain("--es password", workflow, StringComparison.Ordinal);
        Assert.DoesNotContain("--es deviceCredential", workflow, StringComparison.Ordinal);
        Assert.DoesNotContain("GetStringExtra(\"password\")", activity, StringComparison.Ordinal);
        Assert.Contains("File.Delete(e2eInputPath)", activity, StringComparison.Ordinal);
        Assert.Contains("production_offline_default=false", workflow, StringComparison.Ordinal);
        Assert.Contains("ci_test_runtime_activated=true", workflow, StringComparison.Ordinal);
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

        Assert.Contains("DriverServerDeviceKeyBindingVerifier", program, StringComparison.Ordinal);
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
        Assert.Contains("authorization.Consume()", signer, StringComparison.Ordinal);
        Assert.Contains("GenerateFreshKeyAsync", signer, StringComparison.Ordinal);
        Assert.DoesNotContain("GenerateFreshKeyAsync", activation, StringComparison.Ordinal);

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
        Assert.Contains("SignInAndActivateAsync", selfTest, StringComparison.Ordinal);
        Assert.Contains("process_restart_reauthenticated", selfTest, StringComparison.Ordinal);
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
        var authenticated = Read("TransportERP.Mobile.Driver", "Offline",
            "DriverAuthenticatedActivationCoordinator.cs");

        Assert.Contains("new MainPage(_activation, _authenticatedActivation)", app, StringComparison.Ordinal);
        Assert.DoesNotContain("ActivateAsync", app, StringComparison.Ordinal);
        Assert.Contains("Offline runtime: CLOSED", page, StringComparison.Ordinal);
        Assert.Contains("Reason: OFFLINE_CLOSED", page, StringComparison.Ordinal);
        Assert.Contains("CollectionView", page, StringComparison.Ordinal);
        Assert.Contains("Refresh operation status", page, StringComparison.Ordinal);
        Assert.Contains("Manual retry", page, StringComparison.Ordinal);
        Assert.Contains("KEEP_SERVER", page, StringComparison.Ordinal);
        Assert.Contains("REAPPLY", page, StringComparison.Ordinal);
        Assert.Contains("Resolution reason (required)", page, StringComparison.Ordinal);
        Assert.Contains("I reviewed the redacted conflict evidence and confirm this decision.", page,
            StringComparison.Ordinal);
        Assert.Contains("SafeConflictReview", page, StringComparison.Ordinal);
        Assert.Contains("ConflictDecisionReady", page, StringComparison.Ordinal);
        Assert.Contains("RESOLUTION_CONFIRMATION_REQUIRED", page, StringComparison.Ordinal);
        Assert.Contains("RESOLUTION_REASON_REQUIRED", page, StringComparison.Ordinal);
        Assert.Contains("_selected.ConflictServerVersion is not > 0", page, StringComparison.Ordinal);
        Assert.Contains("baseVersion = _selected.ConflictServerVersion", page, StringComparison.Ordinal);
        Assert.DoesNotContain("Current base version for REAPPLY", page, StringComparison.Ordinal);
        Assert.DoesNotContain("_reapplyBaseVersion", page, StringComparison.Ordinal);
        Assert.DoesNotContain("_serverOrigin", page, StringComparison.Ordinal);
        Assert.Contains("CanRetryFailedOperations", page, StringComparison.Ordinal);
        Assert.Contains("CanResolveConflicts", page, StringComparison.Ordinal);
        Assert.Contains("SafeCode(exception)", page, StringComparison.Ordinal);
        Assert.Contains("OPERATION_FAILED", page, StringComparison.Ordinal);
        Assert.DoesNotContain("exception.Message", page, StringComparison.Ordinal);
        Assert.DoesNotContain("PayloadJson", page, StringComparison.Ordinal);
        Assert.DoesNotContain("PayloadHash", page, StringComparison.Ordinal);
        Assert.Contains("SignInAndActivateAsync", page, StringComparison.Ordinal);
        Assert.Contains("Company UUID (required)", page, StringComparison.Ordinal);
        Assert.Contains("Branch UUID (required)", page, StringComparison.Ordinal);
        Assert.Contains("TryRequiredGuid", page, StringComparison.Ordinal);
        Assert.Contains("_password.Text = string.Empty", page, StringComparison.Ordinal);
        Assert.Contains("_deviceCredential.Text = string.Empty", page, StringComparison.Ordinal);

        Assert.Contains("DriverOfflineOperationPermissions operationPermissions", activation,
            StringComparison.Ordinal);
        Assert.Contains("request.OperationPermissions", activation, StringComparison.Ordinal);
        Assert.Contains("request.UserId.ToString(\"N\")", activation, StringComparison.Ordinal);
        Assert.Contains("StateChanged", activation, StringComparison.Ordinal);
        Assert.Contains("OFFLINE_CLOSED", activation, StringComparison.Ordinal);
        Assert.True(
            activation.IndexOf("!request.OfflineRuntimeAuthorized || !featureGate.Allows(bindingContext)",
                StringComparison.Ordinal) <
            activation.IndexOf("DriverDeviceKeyBindingGuard.RequireMatchAsync", StringComparison.Ordinal));

        Assert.Contains("SignInAndActivateAsync", authenticated, StringComparison.Ordinal);
        Assert.Contains("featureGate.Authorize(decision", authenticated, StringComparison.Ordinal);
        Assert.Contains("bindingVerifier.Authorize(decision", authenticated, StringComparison.Ordinal);
        Assert.Contains("ProvisionForAuthorizedEnrollmentAsync", authenticated, StringComparison.Ordinal);
        Assert.Contains("DriverKeyProvisioning.UseExisting", authenticated, StringComparison.Ordinal);
        Assert.Contains("DriverKeyProvisioning.Create", authenticated, StringComparison.Ordinal);
        Assert.Contains("DriverKeyProvisioning.ReplaceForRecovery", authenticated, StringComparison.Ordinal);
        Assert.Contains("ReplaceForAuthorizedRecoveryAsync", authenticated, StringComparison.Ordinal);
        Assert.Contains("DEVICE_KEY_RECOVERY_REAUTHENTICATION_REQUIRED", authenticated,
            StringComparison.Ordinal);
        Assert.DoesNotContain("SecureStorage", authenticated, StringComparison.Ordinal);
        Assert.DoesNotContain("Preferences.", authenticated, StringComparison.Ordinal);

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
        Assert.Contains("\"invalid_dpop_proof\" => \"INVALID_DPOP_PROOF\"", composition,
            StringComparison.Ordinal);
        Assert.Contains("_ => \"INVALID_RESULT_CODE\"", composition, StringComparison.Ordinal);
        Assert.Contains("operation.ConflictReview?.BaseVersion", composition, StringComparison.Ordinal);
        Assert.Contains("BuildRedactedLocalSnapshot(operation.ConflictReview)", composition,
            StringComparison.Ordinal);
        Assert.Contains("BuildRedactedServerSnapshot(operation.ConflictReview)", composition,
            StringComparison.Ordinal);
    }

    [Fact]
    public void Driver_consumes_effective_policy_build_identity_and_queues_a_typed_business_action()
    {
        var page = Read("TransportERP.Mobile.Driver", "MainPage.cs");
        var authenticated = Read("TransportERP.Mobile.Driver", "Offline",
            "DriverAuthenticatedActivationCoordinator.cs");
        var activation = Read("TransportERP.Mobile.Driver", "Offline",
            "DriverOfflineActivationService.cs");
        var composition = Read("TransportERP.Mobile.Driver", "Offline",
            "DriverOfflineComposition.cs");
        var producer = Read("TransportERP.Mobile.Driver", "Offline",
            "DriverOfflineBusinessProducer.cs");
        var sharedProducer = Read("TransportERP.Offline", "OperationalPartyOfflineProducer.cs");

        Assert.Contains("EffectivePolicy(decision)", authenticated, StringComparison.Ordinal);
        Assert.Contains("decision.MaxBatchOperations", authenticated, StringComparison.Ordinal);
        Assert.Contains("decision.ClientTransportMaxRetryCount", authenticated, StringComparison.Ordinal);
        Assert.Contains("decision.LocalSuccessHours", authenticated, StringComparison.Ordinal);
        Assert.Contains("decision.CacheMaxAgeHours", authenticated, StringComparison.Ordinal);
        Assert.Contains("decision.MaximumRequestBodyBytes", authenticated, StringComparison.Ordinal);
        Assert.Contains("decision.MaximumPayloadBytes", authenticated, StringComparison.Ordinal);
        Assert.Contains("decision.ActivationImplementationSha", authenticated, StringComparison.Ordinal);
        Assert.Contains("request.EffectivePolicy.MaxBatchOperations", activation, StringComparison.Ordinal);
        Assert.Contains("request.EffectivePolicy.ClientTransportMaxRetryCount", activation,
            StringComparison.Ordinal);
        Assert.Contains("options.EffectivePolicy.IsValid", composition, StringComparison.Ordinal);
        Assert.Contains("options.EffectivePolicy.LocalSuccessRetention", composition, StringComparison.Ordinal);
        Assert.Contains("READ_CACHE_POLICY_DENIED", composition, StringComparison.Ordinal);

        Assert.Contains("QueueOperationalPartyAsync", producer, StringComparison.Ordinal);
        Assert.Contains("new OperationalPartyOfflineProducer", producer, StringComparison.Ordinal);
        Assert.Contains("new OperationalPartyCreateRequest", sharedProducer, StringComparison.Ordinal);
        Assert.Contains("identity.ClientOperationId", sharedProducer, StringComparison.Ordinal);
        Assert.Contains("runtime.QueueAsync", producer, StringComparison.Ordinal);
        Assert.Contains("Queue encrypted operational party", page, StringComparison.Ordinal);
        Assert.Contains("CreateBusinessProducer().QueueOperationalPartyAsync", page, StringComparison.Ordinal);
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
