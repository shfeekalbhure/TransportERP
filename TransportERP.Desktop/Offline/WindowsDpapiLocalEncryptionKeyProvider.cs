using System.Security.Cryptography;
using System.Text;
using TransportERP.Offline;

namespace TransportERP.Desktop.Offline;

/// <summary>
/// Persists only DPAPI-protected SQLCipher keys. The write outbox and read cache
/// deliberately use different blobs and different optional entropy.
/// </summary>
public sealed class WindowsDpapiLocalEncryptionKeyProvider : ILocalEncryptionKeyProvider
{
    private const int KeySizeBytes = 32;
    private readonly string _keyDirectory;

    public WindowsDpapiLocalEncryptionKeyProvider(string keyDirectory)
    {
        if (string.IsNullOrWhiteSpace(keyDirectory))
            throw new ArgumentException("A protected-key directory is required.", nameof(keyDirectory));

        _keyDirectory = Path.GetFullPath(keyDirectory);
    }

    public static WindowsDpapiLocalEncryptionKeyProvider ForCurrentUser()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        if (string.IsNullOrWhiteSpace(localAppData))
            throw new OfflineStoreException("LOCAL_SECURE_STORAGE_UNAVAILABLE", "Windows local application data is unavailable.");

        return new WindowsDpapiLocalEncryptionKeyProvider(
            Path.Combine(localAppData, "TransportERP", "Offline", "keys"));
    }

    public ValueTask<byte[]> GetKeyAsync(
        LocalStorePurpose purpose,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        EnsureWindows();

        var path = Path.Combine(_keyDirectory, FileName(purpose));
        var entropy = PurposeEntropy(purpose);
        try
        {
            Directory.CreateDirectory(_keyDirectory);
            if (File.Exists(path))
                return ValueTask.FromResult(UnprotectRequired(path, entropy));

            var key = RandomNumberGenerator.GetBytes(KeySizeBytes);
            byte[]? protectedKey = null;
            try
            {
                protectedKey = ProtectedData.Protect(key, entropy, DataProtectionScope.CurrentUser);
                PersistProtectedBlobOnce(path, protectedKey);

                // A competing process may have won creation. Always return the
                // persisted identity so both processes open the same database.
                var persisted = UnprotectRequired(path, entropy);
                CryptographicOperations.ZeroMemory(key);
                return ValueTask.FromResult(persisted);
            }
            finally
            {
                CryptographicOperations.ZeroMemory(key);
                if (protectedKey is not null)
                    CryptographicOperations.ZeroMemory(protectedKey);
            }
        }
        catch (OfflineStoreException)
        {
            throw;
        }
        catch (Exception exception) when (exception is CryptographicException or IOException or UnauthorizedAccessException)
        {
            throw new OfflineStoreException(
                "LOCAL_SECURE_STORAGE_UNAVAILABLE",
                "The protected local database key could not be loaded. Plaintext fallback is prohibited.",
                exception);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(entropy);
        }
    }

    private static void PersistProtectedBlobOnce(string path, byte[] protectedKey)
    {
        var temporaryPath = $"{path}.{Guid.NewGuid():N}.tmp";
        try
        {
            using var stream = new FileStream(
                temporaryPath,
                FileMode.CreateNew,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 4096,
                FileOptions.WriteThrough);
            stream.Write(protectedKey);
            stream.Flush(flushToDisk: true);
            File.Move(temporaryPath, path, overwrite: false);
        }
        catch (IOException) when (File.Exists(path))
        {
            // Another instance created the protected identity first.
        }
        finally
        {
            if (File.Exists(temporaryPath))
                File.Delete(temporaryPath);
        }
    }

    private static byte[] UnprotectRequired(string path, byte[] entropy)
    {
        var protectedKey = File.ReadAllBytes(path);
        try
        {
            if (protectedKey.Length == 0)
                throw new OfflineStoreException("LOCAL_ENCRYPTION_KEY_INVALID", "The protected local database key is empty.");

            var key = ProtectedData.Unprotect(protectedKey, entropy, DataProtectionScope.CurrentUser);
            if (key.Length != KeySizeBytes)
            {
                CryptographicOperations.ZeroMemory(key);
                throw new OfflineStoreException("LOCAL_ENCRYPTION_KEY_INVALID", "The protected local database key has an invalid length.");
            }

            return key;
        }
        finally
        {
            CryptographicOperations.ZeroMemory(protectedKey);
        }
    }

    private static byte[] PurposeEntropy(LocalStorePurpose purpose) =>
        SHA256.HashData(Encoding.UTF8.GetBytes($"TransportERP|offline-sqlcipher|v1|{purpose}"));

    private static string FileName(LocalStorePurpose purpose) => purpose switch
    {
        LocalStorePurpose.WriteOutbox => "write-outbox.v1.dpapi",
        LocalStorePurpose.ReadCache => "read-cache.v1.dpapi",
        _ => throw new OfflineStoreException("LOCAL_STORE_PURPOSE_INVALID", "The local store purpose is not approved.")
    };

    private static void EnsureWindows()
    {
        if (!OperatingSystem.IsWindows())
            throw new OfflineStoreException("LOCAL_SECURE_STORAGE_UNAVAILABLE", "DPAPI secure storage is available only on Windows.");
    }
}
