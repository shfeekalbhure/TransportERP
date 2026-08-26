#if TRANSPORTERP_DEVICE_TESTS
using System.Security.Cryptography;
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

        var services = Services();
        var encryptionKeys = services.GetRequiredService<AndroidSecureStorageEncryptionKeyProvider>();
        var signingKey = services.GetRequiredService<AndroidKeystoreDeviceSigningKey>();
        byte[]? outboxKey = null;
        byte[]? readCacheKey = null;
        try
        {
            outboxKey = await encryptionKeys.GetKeyAsync(LocalStorePurpose.WriteOutbox, cancellationToken);
            readCacheKey = await encryptionKeys.GetKeyAsync(LocalStorePurpose.ReadCache, cancellationToken);
            var publicJwk = await signingKey.GetPublicJwkAsync(cancellationToken);
            var signatureValid = await CreateAndVerifySignatureAsync(signingKey, publicJwk, cancellationToken);

            var checks = ClosedRuntimeChecks();
            checks["outbox_key_is_32_bytes"] = outboxKey.Length == 32;
            checks["read_cache_key_is_32_bytes"] = readCacheKey.Length == 32;
            checks["purpose_keys_are_distinct"] = outboxKey.Length == readCacheKey.Length &&
                !CryptographicOperations.FixedTimeEquals(outboxKey, readCacheKey);
            checks["p256_p1363_signature_verified"] = signatureValid;
            checks["private_key_non_exportable"] = PrivateSigningKeyIsNonExportable();
            checks["offline_storage_still_absent"] = !OfflineStorageDirectoryExists();

            if (checks.Values.Any(value => !value))
                return DriverDeviceTestResult.FromChecks("seed", checks);

            var state = new DriverDeviceTestState(
                SchemaVersion,
                CreateSealedProbe(outboxKey),
                CreateSealedProbe(readCacheKey),
                publicJwk.X,
                publicJwk.Y);
            await WriteStateAtomicallyAsync(statePath, state, cancellationToken);
            checks["sealed_restart_state_written"] = File.Exists(statePath);
            return DriverDeviceTestResult.FromChecks("seed", checks);
        }
        finally
        {
            if (outboxKey is not null)
                CryptographicOperations.ZeroMemory(outboxKey);
            if (readCacheKey is not null)
                CryptographicOperations.ZeroMemory(readCacheKey);
        }
    }

    private static async Task<DriverDeviceTestResult> VerifyNativeSecurityAfterRestartAsync(
        string statePath,
        CancellationToken cancellationToken)
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
        byte[]? outboxKey = null;
        byte[]? readCacheKey = null;
        try
        {
            outboxKey = await encryptionKeys.GetKeyAsync(LocalStorePurpose.WriteOutbox, cancellationToken);
            readCacheKey = await encryptionKeys.GetKeyAsync(LocalStorePurpose.ReadCache, cancellationToken);
            var publicJwk = await signingKey.GetPublicJwkAsync(cancellationToken);
            var checks = ClosedRuntimeChecks();
            checks["outbox_key_survived_restart"] = VerifySealedProbe(outboxKey, state.OutboxProbe);
            checks["read_cache_key_survived_restart"] = VerifySealedProbe(readCacheKey, state.ReadCacheProbe);
            checks["purpose_keys_remain_distinct"] = outboxKey.Length == readCacheKey.Length &&
                !CryptographicOperations.FixedTimeEquals(outboxKey, readCacheKey);
            checks["signing_public_key_survived_restart"] =
                Base64UrlEquals(publicJwk.X, state.PublicKeyX) &&
                Base64UrlEquals(publicJwk.Y, state.PublicKeyY);
            checks["p256_p1363_signature_verified_after_restart"] =
                await CreateAndVerifySignatureAsync(signingKey, publicJwk, cancellationToken);
            checks["private_key_still_non_exportable"] = PrivateSigningKeyIsNonExportable();
            checks["offline_storage_still_absent"] = !OfflineStorageDirectoryExists();

            var result = DriverDeviceTestResult.FromChecks("verify", checks);
            if (result.Passed)
                File.Delete(statePath);
            return result;
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
        return new(StringComparer.Ordinal)
        {
            ["activation_is_inactive"] = activation.Active is null,
            ["default_feature_gate_is_closed"] = !featureGate.IsOfflineRuntimeAuthorized,
            ["offline_storage_absent"] = !OfflineStorageDirectoryExists()
        };
    }

    private static IServiceProvider Services() =>
        IPlatformApplication.Current?.Services ??
        throw new InvalidOperationException("DEVICE_TEST_SERVICES_UNAVAILABLE");

    private static bool OfflineStorageDirectoryExists() =>
        Directory.Exists(Path.Combine(FileSystem.AppDataDirectory, "offline-v1"));

    private static bool SigningAliasExists()
    {
        using var keyStore = KeyStore.GetInstance(AndroidKeyStore)
            ?? throw new InvalidOperationException("DEVICE_TEST_KEYSTORE_UNAVAILABLE");
        keyStore.Load(null);
        return keyStore.ContainsAlias(SigningKeyAlias);
    }

    private static bool PrivateSigningKeyIsNonExportable()
    {
        using var keyStore = KeyStore.GetInstance(AndroidKeyStore)
            ?? throw new InvalidOperationException("DEVICE_TEST_KEYSTORE_UNAVAILABLE");
        keyStore.Load(null);
        using var privateKey = keyStore.GetKey(SigningKeyAlias, null) as IPrivateKey;
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

    private static async Task<bool> CreateAndVerifySignatureAsync(
        AndroidKeystoreDeviceSigningKey signingKey,
        DevicePublicP256Jwk publicJwk,
        CancellationToken cancellationToken)
    {
        var challenge = RandomNumberGenerator.GetBytes(48);
        byte[]? signature = null;
        try
        {
            signature = await signingKey.SignEs256Async(challenge, cancellationToken);
            if (signature.Length != 64)
                return false;

            var x = DecodeBase64Url(publicJwk.X);
            var y = DecodeBase64Url(publicJwk.Y);
            try
            {
                if (x.Length != 32 || y.Length != 32)
                    return false;
                using var verifier = ECDsa.Create(new ECParameters
                {
                    Curve = ECCurve.NamedCurves.nistP256,
                    Q = new ECPoint { X = x, Y = y }
                });
                return verifier.VerifyData(
                    challenge,
                    signature,
                    HashAlgorithmName.SHA256,
                    DSASignatureFormat.IeeeP1363FixedFieldConcatenation);
            }
            finally
            {
                CryptographicOperations.ZeroMemory(x);
                CryptographicOperations.ZeroMemory(y);
            }
        }
        finally
        {
            CryptographicOperations.ZeroMemory(challenge);
            if (signature is not null)
                CryptographicOperations.ZeroMemory(signature);
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
}

internal sealed record DriverDeviceTestState(
    int SchemaVersion,
    DriverDeviceTestSealedProbe OutboxProbe,
    DriverDeviceTestSealedProbe ReadCacheProbe,
    string PublicKeyX,
    string PublicKeyY);

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
