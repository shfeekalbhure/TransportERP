#if TRANSPORTERP_DEVICE_TESTS
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Java.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Storage;
using TransportERP.Mobile.Driver.Offline;
using TransportERP.Mobile.Driver.Platforms.Android;
using TransportERP.Offline;
using TransportERP.Offline.Transport;

namespace TransportERP.Mobile.Driver.DeviceTesting;

internal static class AndroidDriverRuntimeSelfTest
{
    private const int SchemaVersion = 1;
    private const string AndroidKeyStore = "AndroidKeyStore";
    private const string SigningKeyAlias = "transporterp.driver.device-pop.p256.v1";
    private const string OutboxTestPayloadName = "Android runtime test";
    private static readonly Guid OutboxTestLocalIntentId = Guid.Parse("10000000-0000-4000-8000-000000000001");
    private static readonly Guid OutboxTestCompanyId = Guid.Parse("10000000-0000-4000-8000-000000000002");
    private static readonly Guid OutboxTestBranchId = Guid.Parse("10000000-0000-4000-8000-000000000003");
    private static readonly Guid OutboxTestUserId = Guid.Parse("10000000-0000-4000-8000-000000000004");
    private static readonly Guid OutboxTestRegisteredDeviceId = Guid.Parse("10000000-0000-4000-8000-000000000005");
    private static readonly DateTimeOffset OutboxTestOccurredAt =
        new(2026, 8, 27, 0, 0, 0, TimeSpan.Zero);
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    internal static async Task<DriverDeviceTestResult> RunAsync(
        string phase,
        string statePath,
        CancellationToken cancellationToken)
    {
        return phase switch
        {
            "startup" => await VerifyClosedStartupAsync(statePath, cancellationToken),
            "seed" => await SeedNativeSecurityAsync(statePath, cancellationToken),
            "verify" => await VerifyNativeSecurityAfterRestartAsync(statePath, cancellationToken),
            "loss" => await VerifyMissingSigningAliasFailsClosedAsync(statePath, cancellationToken),
            _ => DriverDeviceTestResult.Failure(phase, "UNKNOWN_PHASE")
        };
    }

    private static async Task<DriverDeviceTestResult> VerifyClosedStartupAsync(
        string statePath,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var checks = ClosedRuntimeChecks();
        checks["test_state_absent"] = !File.Exists(statePath);
        checks["signing_alias_absent"] = !SigningAliasExists();

        // Yield once so this is a real asynchronous Android runtime path without touching native
        // secure storage or creating local databases during the closed-startup phase.
        await Task.Yield();
        return DriverDeviceTestResult.FromChecks("startup", checks);
    }

    private static async Task<DriverDeviceTestResult> SeedNativeSecurityAsync(
        string statePath,
        CancellationToken cancellationToken)
    {
        if (File.Exists(statePath))
            return DriverDeviceTestResult.Failure("seed", "TEST_STATE_ALREADY_EXISTS");

        byte[]? outboxKey = null;
        byte[]? readCacheKey = null;
        var failureCode = "DEVICE_TEST_SERVICES_RESOLUTION_FAILED";
        try
        {
            var services = Services();
            var encryptionKeys = services.GetRequiredService<AndroidSecureStorageEncryptionKeyProvider>();
            var signingKey = services.GetRequiredService<AndroidKeystoreDeviceSigningKey>();
            failureCode = "NATIVE_SECURE_STORAGE_INITIALIZATION_FAILED";
            if (!await encryptionKeys.IsNativeSecureStorageAvailableAsync(cancellationToken))
                return DriverDeviceTestResult.Failure("seed", failureCode);

            failureCode = "NATIVE_SECURE_STORAGE_OUTBOX_KEY_FAILED";
            outboxKey = await encryptionKeys.GetKeyAsync(LocalStorePurpose.WriteOutbox, cancellationToken);
            failureCode = "NATIVE_SECURE_STORAGE_READ_CACHE_KEY_FAILED";
            readCacheKey = await encryptionKeys.GetKeyAsync(LocalStorePurpose.ReadCache, cancellationToken);
            failureCode = "NATIVE_SIGNING_KEY_TEST_PROVISION_FAILED";
            await signingKey.ProvisionFreshKeyForDeviceTestAsync(cancellationToken);
            failureCode = "NATIVE_SIGNING_PUBLIC_KEY_READ_FAILED";
            var publicJwk = await signingKey.GetPublicJwkAsync(cancellationToken);
            failureCode = "NATIVE_SIGNING_P1363_VERIFY_FAILED";
            var signatureOutcome = await CreateAndVerifySignatureAsync(
                signingKey, publicJwk, cancellationToken);
            if (signatureOutcome.FailureCode is not null)
                return DriverDeviceTestResult.Failure("seed", signatureOutcome.FailureCode);
            var signatureDiagnostics = signatureOutcome.Diagnostics!;
            var bindingContext = TestBindingContext();

            failureCode = "DEVICE_KEY_BINDING_SELF_TEST_FAILED";
            var checks = ClosedRuntimeChecks();
            checks["outbox_key_is_32_bytes"] = outboxKey.Length == 32;
            checks["read_cache_key_is_32_bytes"] = readCacheKey.Length == 32;
            checks["purpose_keys_are_distinct"] = outboxKey.Length == readCacheKey.Length &&
                !CryptographicOperations.FixedTimeEquals(outboxKey, readCacheKey);
            checks["native_secure_storage_initialized"] = true;
            checks["java_der_signature_verified"] = signatureDiagnostics.JavaDerSignatureVerified;
            checks["der_p1363_round_trip_verified"] = signatureDiagnostics.DerP1363RoundTripVerified;
            checks["public_jwk_matches_keystore_certificate"] =
                signatureDiagnostics.PublicJwkMatchesCertificate;
            checks["p256_p1363_signature_verified"] = signatureDiagnostics.ProductionP1363Verified;
            checks["private_key_non_exportable"] = PrivateSigningKeyIsNonExportable();
            checks["matching_registered_binding_accepted"] = await BindingAcceptedAsync(
                bindingContext,
                signingKey,
                new ExactTestDeviceKeyBindingVerifier(publicJwk),
                cancellationToken);
            checks["mismatched_registered_binding_requires_rotation"] = await BindingRejectedWithCodeAsync(
                bindingContext,
                signingKey,
                new ExactTestDeviceKeyBindingVerifier(DifferentPublicKey(publicJwk)),
                "DEVICE_KEY_ROTATION_REQUIRED",
                cancellationToken);
            checks["missing_registered_binding_requires_rebind"] = await BindingRejectedWithCodeAsync(
                bindingContext,
                signingKey,
                new FixedTestDeviceKeyBindingVerifier(
                    DriverDeviceKeyBindingDecision.RegisteredBindingMissing),
                "DEVICE_KEY_REBIND_REQUIRED",
                cancellationToken);
            checks["unavailable_binding_verification_fails_closed"] = await BindingRejectedWithCodeAsync(
                bindingContext,
                signingKey,
                new DriverClosedDeviceKeyBindingVerifier(),
                "DEVICE_KEY_BINDING_VERIFICATION_REQUIRED",
                cancellationToken);
            checks["offline_storage_still_absent"] = !OfflineStorageDirectoryExists();

            if (checks.Values.Any(value => !value))
                return DriverDeviceTestResult.FromChecks("seed", checks);

            failureCode = "SQLCIPHER_OUTBOX_INITIALIZATION_FAILED";
            var outboxStore = new OfflineOperationStore(DeviceTestOutboxPath(), encryptionKeys);
            await outboxStore.InitializeAsync(cancellationToken);
            failureCode = "SQLCIPHER_OUTBOX_ENQUEUE_FAILED";
            var queued = await outboxStore.EnqueueAsync(
                OutboxTestTemplate(),
                identity => TestOutboxPayload(identity.ClientOperationId),
                cancellationToken);
            checks["sqlcipher_outbox_initialized"] = File.Exists(DeviceTestOutboxPath());
            checks["sqlcipher_outbox_enqueued_once"] =
                queued.Created && queued.Operation.Status == OfflineOperationStatus.Queued;
            checks["sqlcipher_outbox_payload_hash_created"] = string.Equals(
                queued.Operation.PayloadHash,
                ComputePayloadHash(TestOutboxPayload(queued.Operation.ClientOperationId)),
                StringComparison.Ordinal);

            failureCode = "DEVICE_TEST_STATE_BUILD_FAILED";
            var state = new DriverDeviceTestState(
                SchemaVersion,
                CreateSealedProbe(outboxKey),
                CreateSealedProbe(readCacheKey),
                publicJwk.X,
                publicJwk.Y,
                queued.Operation.LocalOperationId,
                queued.Operation.ClientOperationId,
                queued.Operation.OperationCorrelationId);
            failureCode = "DEVICE_TEST_STATE_WRITE_FAILED";
            await WriteStateAtomicallyAsync(statePath, state, cancellationToken);
            checks["sealed_restart_state_written"] = File.Exists(statePath);
            return DriverDeviceTestResult.FromChecks("seed", checks);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (DriverOfflineUnavailableException exception)
        {
            return DriverDeviceTestResult.Failure("seed", exception.Code);
        }
        catch
        {
            return DriverDeviceTestResult.Failure("seed", failureCode);
        }
        finally
        {
            if (outboxKey is not null)
                CryptographicOperations.ZeroMemory(outboxKey);
            if (readCacheKey is not null)
                CryptographicOperations.ZeroMemory(readCacheKey);
        }
    }

    private static async Task<DriverDeviceTestResult> VerifyMissingSigningAliasFailsClosedAsync(
        string statePath,
        CancellationToken cancellationToken)
    {
        var failureCode = "SIGNING_ALIAS_LOSS_CHECK_FAILED";
        try
        {
            var services = Services();
            var signingKey = services.GetRequiredService<AndroidKeystoreDeviceSigningKey>();
            var checks = ClosedRuntimeChecks();
            checks["restart_state_was_cleaned"] = !File.Exists(statePath);
            checks["sqlcipher_test_outbox_was_cleaned"] = !File.Exists(DeviceTestOutboxPath());
            checks["signing_alias_existed_before_loss"] = SigningAliasExists();
            DeleteSigningAlias();
            checks["signing_alias_removed"] = !SigningAliasExists();
            checks["missing_alias_reports_unavailable"] =
                !await signingKey.IsNativeSigningKeyAvailableAsync(cancellationToken);
            checks["missing_alias_requires_rebind"] = await MissingAliasRequiresRebindAsync(
                signingKey,
                cancellationToken);
            checks["missing_alias_was_not_reprovisioned"] = !SigningAliasExists();
            checks["offline_storage_still_absent"] = !OfflineStorageDirectoryExists();
            return DriverDeviceTestResult.FromChecks("loss", checks);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (DriverOfflineUnavailableException exception)
        {
            return DriverDeviceTestResult.Failure("loss", exception.Code);
        }
        catch
        {
            return DriverDeviceTestResult.Failure("loss", failureCode);
        }
    }

    private static async Task<DriverDeviceTestResult> VerifyNativeSecurityAfterRestartAsync(
        string statePath,
        CancellationToken cancellationToken)
    {
        var failureCode = "DEVICE_TEST_STATE_READ_FAILED";
        byte[]? outboxKey = null;
        byte[]? readCacheKey = null;
        try
        {
            if (!File.Exists(statePath))
                return DriverDeviceTestResult.Failure("verify", "TEST_STATE_MISSING");

            var stateJson = await File.ReadAllTextAsync(statePath, cancellationToken);
            var state = JsonSerializer.Deserialize<DriverDeviceTestState>(stateJson, JsonOptions);
            if (state is null || state.SchemaVersion != SchemaVersion)
                return DriverDeviceTestResult.Failure("verify", "TEST_STATE_INVALID");

            var services = Services();
            var encryptionKeys = services.GetRequiredService<AndroidSecureStorageEncryptionKeyProvider>();
            var signingKey = services.GetRequiredService<AndroidKeystoreDeviceSigningKey>();
            failureCode = "NATIVE_SECURE_STORAGE_RESTART_INITIALIZATION_FAILED";
            if (!await encryptionKeys.IsNativeSecureStorageAvailableAsync(cancellationToken))
                return DriverDeviceTestResult.Failure("verify", failureCode);

            failureCode = "NATIVE_SECURE_STORAGE_RESTART_OUTBOX_KEY_FAILED";
            outboxKey = await encryptionKeys.GetKeyAsync(LocalStorePurpose.WriteOutbox, cancellationToken);
            failureCode = "NATIVE_SECURE_STORAGE_RESTART_READ_CACHE_KEY_FAILED";
            readCacheKey = await encryptionKeys.GetKeyAsync(LocalStorePurpose.ReadCache, cancellationToken);
            failureCode = "NATIVE_SIGNING_KEY_RESTART_READ_FAILED";
            var publicJwk = await signingKey.GetPublicJwkAsync(cancellationToken);
            var checks = ClosedRuntimeChecks();
            checks["outbox_key_survived_restart"] = VerifySealedProbe(outboxKey, state.OutboxProbe);
            checks["read_cache_key_survived_restart"] = VerifySealedProbe(readCacheKey, state.ReadCacheProbe);
            checks["purpose_keys_remain_distinct"] = outboxKey.Length == readCacheKey.Length &&
                !CryptographicOperations.FixedTimeEquals(outboxKey, readCacheKey);
            checks["signing_public_key_survived_restart"] =
                Base64UrlEquals(publicJwk.X, state.PublicKeyX) &&
                Base64UrlEquals(publicJwk.Y, state.PublicKeyY);
            var restartSignatureOutcome = await CreateAndVerifySignatureAsync(
                signingKey, publicJwk, cancellationToken);
            if (restartSignatureOutcome.FailureCode is not null)
                return DriverDeviceTestResult.Failure("verify", restartSignatureOutcome.FailureCode);
            var restartSignatureDiagnostics = restartSignatureOutcome.Diagnostics!;
            checks["java_der_signature_verified_after_restart"] =
                restartSignatureDiagnostics.JavaDerSignatureVerified;
            checks["der_p1363_round_trip_verified_after_restart"] =
                restartSignatureDiagnostics.DerP1363RoundTripVerified;
            checks["public_jwk_matches_certificate_after_restart"] =
                restartSignatureDiagnostics.PublicJwkMatchesCertificate;
            checks["p256_p1363_signature_verified_after_restart"] =
                restartSignatureDiagnostics.ProductionP1363Verified;
            checks["private_key_still_non_exportable"] = PrivateSigningKeyIsNonExportable();
            checks["offline_storage_still_absent"] = !OfflineStorageDirectoryExists();

            failureCode = "SQLCIPHER_OUTBOX_REOPEN_FAILED";
            var reopenedStore = new OfflineOperationStore(DeviceTestOutboxPath(), encryptionKeys);
            await reopenedStore.InitializeAsync(cancellationToken);
            var persisted = await reopenedStore.GetAsync(
                state.OutboxLocalOperationId,
                OutboxTestScope(),
                cancellationToken);
            var expectedPayload = TestOutboxPayload(state.OutboxClientOperationId);
            checks["sqlcipher_outbox_operation_survived_restart"] = persisted is not null;
            checks["sqlcipher_outbox_identity_preserved"] = persisted is not null &&
                persisted.LocalOperationId == state.OutboxLocalOperationId &&
                string.Equals(persisted.ClientOperationId, state.OutboxClientOperationId,
                    StringComparison.Ordinal) &&
                persisted.OperationCorrelationId == state.OutboxOperationCorrelationId;
            checks["sqlcipher_outbox_status_preserved"] =
                persisted?.Status == OfflineOperationStatus.Queued;
            checks["sqlcipher_outbox_payload_hash_preserved"] = persisted is not null &&
                string.Equals(persisted.PayloadHash, ComputePayloadHash(expectedPayload),
                    StringComparison.Ordinal) &&
                string.Equals(persisted.PayloadJson, expectedPayload, StringComparison.Ordinal);

            failureCode = "SQLCIPHER_OUTBOX_REPLAY_CHECK_FAILED";
            var replay = await reopenedStore.EnqueueAsync(
                OutboxTestTemplate(),
                _ => throw new InvalidOperationException("Persisted identity must prevent payload regeneration."),
                cancellationToken);
            var listed = await reopenedStore.ListAsync(OutboxTestScope(), cancellationToken);
            checks["sqlcipher_outbox_replay_did_not_duplicate"] =
                !replay.Created &&
                replay.Operation.LocalOperationId == state.OutboxLocalOperationId &&
                listed.Count == 1 &&
                listed[0].LocalOperationId == state.OutboxLocalOperationId;

            var result = DriverDeviceTestResult.FromChecks("verify", checks);
            if (result.Passed)
            {
                File.Delete(statePath);
                DeleteDeviceTestOutbox();
            }
            return result;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch
        {
            return DriverDeviceTestResult.Failure("verify", failureCode);
        }
        finally
        {
            if (outboxKey is not null)
                CryptographicOperations.ZeroMemory(outboxKey);
            if (readCacheKey is not null)
                CryptographicOperations.ZeroMemory(readCacheKey);
        }
    }

    private static SortedDictionary<string, bool> ClosedRuntimeChecks()
    {
        var services = Services();
        var activation = services.GetRequiredService<DriverOfflineActivationService>();
        var featureGate = services.GetRequiredService<IDriverOfflineFeatureGate>();
        var bindingVerifier = services.GetRequiredService<IDriverDeviceKeyBindingVerifier>();
        return new(StringComparer.Ordinal)
        {
            ["activation_is_inactive"] = activation.Active is null,
            ["default_feature_gate_is_closed"] = !featureGate.Allows(TestBindingContext()),
            ["default_device_key_binding_verifier_is_closed"] =
                bindingVerifier is DriverServerDeviceKeyBindingVerifier,
            ["offline_storage_absent"] = !OfflineStorageDirectoryExists()
        };
    }

    private static IServiceProvider Services() =>
        IPlatformApplication.Current?.Services ??
        throw new InvalidOperationException("DEVICE_TEST_SERVICES_UNAVAILABLE");

    private static bool OfflineStorageDirectoryExists() =>
        Directory.Exists(Path.Combine(FileSystem.AppDataDirectory, "offline-v1"));

    private static string DeviceTestOutboxPath() =>
        Path.Combine(FileSystem.AppDataDirectory, "device-tests", "t-sync-010-outbox.db");

    private static OfflineOperationScope OutboxTestScope() => new(
        OutboxTestCompanyId,
        OutboxTestBranchId,
        OutboxTestUserId,
        OutboxTestRegisteredDeviceId);

    private static OfflineOperationEnqueueTemplate OutboxTestTemplate() => new(
        OutboxTestLocalIntentId,
        OutboxTestCompanyId,
        OutboxTestBranchId,
        OutboxTestUserId,
        OutboxTestRegisteredDeviceId,
        "CreateOperationalParty",
        "CREATE",
        "OperationalParty",
        null,
        null,
        OutboxTestOccurredAt);

    private static string TestOutboxPayload(string clientOperationId) =>
        $"{{\"clientOperationId\":\"{clientOperationId}\",\"nameAr\":\"{OutboxTestPayloadName}\"}}";

    private static string ComputePayloadHash(string payload)
    {
        var bytes = Encoding.UTF8.GetBytes(payload);
        byte[]? hash = null;
        try
        {
            hash = SHA256.HashData(bytes);
            return Convert.ToHexString(hash).ToLowerInvariant();
        }
        finally
        {
            CryptographicOperations.ZeroMemory(bytes);
            if (hash is not null)
                CryptographicOperations.ZeroMemory(hash);
        }
    }

    private static void DeleteDeviceTestOutbox()
    {
        var path = DeviceTestOutboxPath();
        File.Delete(path);
        File.Delete(path + "-wal");
        File.Delete(path + "-shm");
        var directory = Path.GetDirectoryName(path);
        if (directory is not null && Directory.Exists(directory) &&
            !Directory.EnumerateFileSystemEntries(directory).Any())
        {
            Directory.Delete(directory);
        }
    }

    private static bool SigningAliasExists()
    {
        using var keyStore = KeyStore.GetInstance(AndroidKeyStore)
            ?? throw new InvalidOperationException("DEVICE_TEST_KEYSTORE_UNAVAILABLE");
        keyStore.Load(null);
        return keyStore.ContainsAlias(SigningKeyAlias);
    }

    private static void DeleteSigningAlias()
    {
        using var keyStore = KeyStore.GetInstance(AndroidKeyStore)
            ?? throw new InvalidOperationException("DEVICE_TEST_KEYSTORE_UNAVAILABLE");
        keyStore.Load(null);
        if (keyStore.ContainsAlias(SigningKeyAlias))
            keyStore.DeleteEntry(SigningKeyAlias);
    }

    private static bool PrivateSigningKeyIsNonExportable()
    {
        using var keyStore = KeyStore.GetInstance(AndroidKeyStore)
            ?? throw new InvalidOperationException("DEVICE_TEST_KEYSTORE_UNAVAILABLE");
        keyStore.Load(null);
        using var privateKeyEntry = keyStore.GetEntry(SigningKeyAlias, null) as KeyStore.PrivateKeyEntry;
        using var privateKey = privateKeyEntry?.PrivateKey;
        if (privateKey is null)
            return false;

        var encoded = privateKey.GetEncoded();
        try
        {
            return encoded is null;
        }
        finally
        {
            if (encoded is not null)
                CryptographicOperations.ZeroMemory(encoded);
        }
    }

    private static async Task<(
        AndroidKeystoreDeviceSigningKey.AndroidSignatureDiagnostics? Diagnostics,
        string? FailureCode)>
        CreateAndVerifySignatureAsync(
        AndroidKeystoreDeviceSigningKey signingKey,
        DevicePublicP256Jwk publicJwk,
        CancellationToken cancellationToken)
    {
        var challenge = RandomNumberGenerator.GetBytes(48);
        byte[]? signature = null;
        var failureCode = "NATIVE_SIGNING_PRODUCTION_SIGN_FAILED";
        try
        {
            signature = await signingKey.SignEs256Async(challenge, cancellationToken);
            if (signature.Length != 64)
                return (null, "NATIVE_SIGNING_P1363_LENGTH_INVALID");

            failureCode = "NATIVE_SIGNING_PUBLIC_JWK_DECODE_FAILED";
            var x = DecodeBase64Url(publicJwk.X);
            var y = DecodeBase64Url(publicJwk.Y);
            try
            {
                if (x.Length != 32 || y.Length != 32)
                    return (null, "NATIVE_SIGNING_PUBLIC_JWK_LENGTH_INVALID");
                failureCode = "NATIVE_SIGNING_DIAGNOSTICS_FAILED";
                var diagnostics = await signingKey.DiagnoseSignatureForDeviceTestAsync(
                    challenge, signature, publicJwk, cancellationToken);
                return (diagnostics, null);
            }
            finally
            {
                CryptographicOperations.ZeroMemory(x);
                CryptographicOperations.ZeroMemory(y);
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (DriverOfflineUnavailableException exception)
        {
            return (null, exception.Code);
        }
        catch
        {
            return (null, failureCode);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(challenge);
            if (signature is not null)
                CryptographicOperations.ZeroMemory(signature);
        }
    }

    private static DriverDeviceKeyBindingContext TestBindingContext() => new(
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid(),
        "driver-device-test");

    private static async Task<bool> BindingAcceptedAsync(
        DriverDeviceKeyBindingContext context,
        IDriverNativeDeviceSigningKey signingKey,
        IDriverDeviceKeyBindingVerifier verifier,
        CancellationToken cancellationToken)
    {
        try
        {
            await DriverDeviceKeyBindingGuard.RequireMatchAsync(
                context,
                signingKey,
                verifier,
                cancellationToken);
            return true;
        }
        catch (DriverOfflineUnavailableException)
        {
            return false;
        }
    }

    private static async Task<bool> BindingRejectedWithCodeAsync(
        DriverDeviceKeyBindingContext context,
        IDriverNativeDeviceSigningKey signingKey,
        IDriverDeviceKeyBindingVerifier verifier,
        string expectedCode,
        CancellationToken cancellationToken)
    {
        try
        {
            await DriverDeviceKeyBindingGuard.RequireMatchAsync(
                context,
                signingKey,
                verifier,
                cancellationToken);
            return false;
        }
        catch (DriverOfflineUnavailableException exception)
        {
            return string.Equals(exception.Code, expectedCode, StringComparison.Ordinal);
        }
    }

    private static async Task<bool> MissingAliasRequiresRebindAsync(
        IDriverNativeDeviceSigningKey signingKey,
        CancellationToken cancellationToken)
    {
        try
        {
            await signingKey.GetPublicJwkAsync(cancellationToken);
            return false;
        }
        catch (DriverOfflineUnavailableException exception)
        {
            return string.Equals(exception.Code, "DEVICE_KEY_REBIND_REQUIRED", StringComparison.Ordinal);
        }
    }

    private static DevicePublicP256Jwk DifferentPublicKey(DevicePublicP256Jwk publicKey)
    {
        var x = DecodeBase64Url(publicKey.X);
        try
        {
            x[0] ^= 0x01;
            return new(EncodeBase64Url(x), publicKey.Y);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(x);
        }
    }

    private static DriverDeviceTestSealedProbe CreateSealedProbe(byte[] key)
    {
        var expected = RandomNumberGenerator.GetBytes(32);
        var nonce = RandomNumberGenerator.GetBytes(12);
        var ciphertext = new byte[expected.Length];
        var tag = new byte[16];
        try
        {
            using var aes = new AesGcm(key, tag.Length);
            aes.Encrypt(nonce, expected, ciphertext, tag);
            return new(
                Convert.ToBase64String(nonce),
                Convert.ToBase64String(ciphertext),
                Convert.ToBase64String(tag),
                Convert.ToBase64String(expected));
        }
        finally
        {
            CryptographicOperations.ZeroMemory(expected);
            CryptographicOperations.ZeroMemory(nonce);
            CryptographicOperations.ZeroMemory(ciphertext);
            CryptographicOperations.ZeroMemory(tag);
        }
    }

    private static bool VerifySealedProbe(byte[] key, DriverDeviceTestSealedProbe probe)
    {
        var nonce = Convert.FromBase64String(probe.Nonce);
        var ciphertext = Convert.FromBase64String(probe.Ciphertext);
        var tag = Convert.FromBase64String(probe.Tag);
        var expected = Convert.FromBase64String(probe.Expected);
        var plaintext = new byte[ciphertext.Length];
        try
        {
            using var aes = new AesGcm(key, tag.Length);
            aes.Decrypt(nonce, ciphertext, tag, plaintext);
            return plaintext.Length == expected.Length &&
                CryptographicOperations.FixedTimeEquals(plaintext, expected);
        }
        catch (CryptographicException)
        {
            return false;
        }
        finally
        {
            CryptographicOperations.ZeroMemory(nonce);
            CryptographicOperations.ZeroMemory(ciphertext);
            CryptographicOperations.ZeroMemory(tag);
            CryptographicOperations.ZeroMemory(expected);
            CryptographicOperations.ZeroMemory(plaintext);
        }
    }

    private static async Task WriteStateAtomicallyAsync(
        string statePath,
        DriverDeviceTestState state,
        CancellationToken cancellationToken)
    {
        var temporaryPath = statePath + ".tmp";
        var json = JsonSerializer.Serialize(state, JsonOptions);
        await File.WriteAllTextAsync(temporaryPath, json, cancellationToken);
        File.Move(temporaryPath, statePath, overwrite: true);
    }

    private static byte[] DecodeBase64Url(string value)
    {
        var normalized = value.Replace('-', '+').Replace('_', '/');
        normalized = normalized.PadRight((normalized.Length + 3) / 4 * 4, '=');
        return Convert.FromBase64String(normalized);
    }

    private static string EncodeBase64Url(ReadOnlySpan<byte> value) =>
        Convert.ToBase64String(value).TrimEnd('=').Replace('+', '-').Replace('/', '_');

    private static bool Base64UrlEquals(string left, string right)
    {
        var leftBytes = DecodeBase64Url(left);
        var rightBytes = DecodeBase64Url(right);
        try
        {
            return leftBytes.Length == rightBytes.Length &&
                CryptographicOperations.FixedTimeEquals(leftBytes, rightBytes);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(leftBytes);
            CryptographicOperations.ZeroMemory(rightBytes);
        }
    }

    private sealed class ExactTestDeviceKeyBindingVerifier(DevicePublicP256Jwk expected)
        : IDriverDeviceKeyBindingVerifier
    {
        public ValueTask<DriverDeviceKeyBindingDecision> VerifyAsync(
            DriverDeviceKeyBindingContext context,
            DevicePublicP256Jwk currentPublicKey,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var matches = Base64UrlEquals(expected.X, currentPublicKey.X) &&
                Base64UrlEquals(expected.Y, currentPublicKey.Y);
            return ValueTask.FromResult(matches
                ? DriverDeviceKeyBindingDecision.Match
                : DriverDeviceKeyBindingDecision.Mismatch);
        }
    }

    private sealed class FixedTestDeviceKeyBindingVerifier(DriverDeviceKeyBindingDecision decision)
        : IDriverDeviceKeyBindingVerifier
    {
        public ValueTask<DriverDeviceKeyBindingDecision> VerifyAsync(
            DriverDeviceKeyBindingContext context,
            DevicePublicP256Jwk currentPublicKey,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return ValueTask.FromResult(decision);
        }
    }
}

internal sealed record DriverDeviceTestState(
    int SchemaVersion,
    DriverDeviceTestSealedProbe OutboxProbe,
    DriverDeviceTestSealedProbe ReadCacheProbe,
    string PublicKeyX,
    string PublicKeyY,
    Guid OutboxLocalOperationId,
    string OutboxClientOperationId,
    Guid OutboxOperationCorrelationId);

internal sealed record DriverDeviceTestSealedProbe(
    string Nonce,
    string Ciphertext,
    string Tag,
    string Expected);

internal sealed record DriverDeviceTestResult(
    int SchemaVersion,
    string Phase,
    bool Passed,
    string Code,
    IReadOnlyDictionary<string, bool> Checks)
{
    internal static DriverDeviceTestResult FromChecks(
        string phase,
        IReadOnlyDictionary<string, bool> checks)
    {
        var passed = checks.Count > 0 && checks.Values.All(value => value);
        return new(1, phase, passed, passed ? "PASS" : "CHECK_FAILED", checks);
    }

    internal static DriverDeviceTestResult Failure(string phase, string code) =>
        new(1, phase, false, code, new SortedDictionary<string, bool>(StringComparer.Ordinal));
}
#endif
