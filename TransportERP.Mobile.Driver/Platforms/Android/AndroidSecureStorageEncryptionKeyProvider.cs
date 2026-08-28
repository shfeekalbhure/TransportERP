using System.Security.Cryptography;
using Microsoft.Maui.Storage;
using TransportERP.Mobile.Driver.Offline;
using TransportERP.Offline;

namespace TransportERP.Mobile.Driver.Platforms.Android;

/// <summary>
/// Keeps independent SQLCipher keys in Android-backed MAUI SecureStorage. Invalid or unavailable
/// protected storage fails closed; keys are never written to preferences or files in plaintext.
/// </summary>
public sealed class AndroidSecureStorageEncryptionKeyProvider : IDriverNativeEncryptionKeyProvider
{
    private const string OutboxKeyName = "transporterp.driver.sqlcipher.outbox.v1";
    private const string ReadCacheKeyName = "transporterp.driver.sqlcipher.readcache.v1";
    private readonly SemaphoreSlim _gate = new(1, 1);

    public async ValueTask<bool> IsNativeSecureStorageAvailableAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var outbox = await GetKeyAsync(LocalStorePurpose.WriteOutbox, cancellationToken);
            var readCache = await GetKeyAsync(LocalStorePurpose.ReadCache, cancellationToken);
            try
            {
                return outbox.Length == 32 && readCache.Length == 32 &&
                    !CryptographicOperations.FixedTimeEquals(outbox, readCache);
            }
            finally
            {
                CryptographicOperations.ZeroMemory(outbox);
                CryptographicOperations.ZeroMemory(readCache);
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch
        {
            return false;
        }
    }

    public async ValueTask<byte[]> GetKeyAsync(
        LocalStorePurpose purpose,
        CancellationToken cancellationToken = default)
    {
        var keyName = purpose switch
        {
            LocalStorePurpose.WriteOutbox => OutboxKeyName,
            LocalStorePurpose.ReadCache => ReadCacheKeyName,
            _ => throw new ArgumentOutOfRangeException(nameof(purpose))
        };

        await _gate.WaitAsync(cancellationToken);
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            var encoded = await SecureStorage.Default.GetAsync(keyName);
            if (encoded is null)
            {
                var generated = RandomNumberGenerator.GetBytes(32);
                try
                {
                    await SecureStorage.Default.SetAsync(keyName, Convert.ToBase64String(generated));
                }
                finally
                {
                    CryptographicOperations.ZeroMemory(generated);
                }

                encoded = await SecureStorage.Default.GetAsync(keyName);
            }

            cancellationToken.ThrowIfCancellationRequested();
            var key = DecodeKey(encoded);
            return key;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (DriverOfflineUnavailableException)
        {
            throw;
        }
        catch (Exception exception)
        {
            throw new DriverOfflineUnavailableException(
                "NATIVE_SECURE_STORAGE_UNAVAILABLE",
                exception);
        }
        finally
        {
            _gate.Release();
        }
    }

    private static byte[] DecodeKey(string? encoded)
    {
        try
        {
            var key = Convert.FromBase64String(encoded ?? string.Empty);
            if (key.Length == 32)
                return key;
            CryptographicOperations.ZeroMemory(key);
        }
        catch (FormatException)
        {
            // Existing malformed protected state must not be silently replaced.
        }

        throw new DriverOfflineUnavailableException("NATIVE_SECURE_STORAGE_KEY_INVALID");
    }
}
