using System.Security.Cryptography;
using Microsoft.Data.Sqlite;

namespace TransportERP.Offline;

public enum LocalStorePurpose
{
    WriteOutbox,
    ReadCache
}

public interface ILocalEncryptionKeyProvider
{
    ValueTask<byte[]> GetKeyAsync(LocalStorePurpose purpose, CancellationToken cancellationToken = default);
}

internal sealed class EncryptedSqliteConnectionFactory
{
    private static readonly Lazy<bool> ProviderInitialization = new(
        static () =>
        {
            SQLitePCL.Batteries_V2.Init();
            return true;
        },
        LazyThreadSafetyMode.ExecutionAndPublication);
    private readonly string _databasePath;
    private readonly ILocalEncryptionKeyProvider _keyProvider;
    private readonly LocalStorePurpose _purpose;

    public EncryptedSqliteConnectionFactory(
        string databasePath,
        ILocalEncryptionKeyProvider keyProvider,
        LocalStorePurpose purpose)
    {
        if (string.IsNullOrWhiteSpace(databasePath))
        {
            throw new ArgumentException("A database path is required.", nameof(databasePath));
        }

        _databasePath = Path.GetFullPath(databasePath);
        _keyProvider = keyProvider ?? throw new ArgumentNullException(nameof(keyProvider));
        _purpose = purpose;
    }

    public string DatabasePath => _databasePath;

    public async ValueTask<SqliteConnection> OpenAsync(CancellationToken cancellationToken = default)
    {
        EnsureProviderInitialized();
        Directory.CreateDirectory(Path.GetDirectoryName(_databasePath)!);

        var key = await _keyProvider.GetKeyAsync(_purpose, cancellationToken);
        if (key is null || key.Length < 32)
        {
            if (key is not null)
            {
                CryptographicOperations.ZeroMemory(key);
            }

            throw new OfflineStoreException("LOCAL_ENCRYPTION_KEY_INVALID", "The local encryption key must contain at least 256 bits.");
        }

        var existedWithContent = File.Exists(_databasePath) && new FileInfo(_databasePath).Length > 0;
        var connection = new SqliteConnection(new SqliteConnectionStringBuilder
        {
            DataSource = _databasePath,
            Mode = SqliteOpenMode.ReadWriteCreate,
            Pooling = false
        }.ToString());

        try
        {
            await connection.OpenAsync(cancellationToken);
            var keyHex = Convert.ToHexString(key);
            await using (var keyCommand = connection.CreateCommand())
            {
                keyCommand.CommandText = $"PRAGMA key = \"x'{keyHex}'\";";
                await keyCommand.ExecuteNonQueryAsync(cancellationToken);
            }

            await using (var cipherCheck = connection.CreateCommand())
            {
                cipherCheck.CommandText = "PRAGMA cipher_version;";
                var cipherVersion = await cipherCheck.ExecuteScalarAsync(cancellationToken) as string;
                if (string.IsNullOrWhiteSpace(cipherVersion))
                {
                    throw new OfflineStoreException(
                        "LOCAL_ENCRYPTION_UNAVAILABLE",
                        "The required SQLCipher provider is unavailable; plaintext fallback is prohibited.");
                }
            }

            await using (var verification = connection.CreateCommand())
            {
                verification.CommandText = "SELECT count(*) FROM sqlite_master;";
                await verification.ExecuteScalarAsync(cancellationToken);
            }

            await using (var hardening = connection.CreateCommand())
            {
                hardening.CommandText = "PRAGMA cipher_memory_security = ON; PRAGMA secure_delete = ON; PRAGMA foreign_keys = ON; PRAGMA busy_timeout = 5000;";
                await hardening.ExecuteNonQueryAsync(cancellationToken);
            }

            return connection;
        }
        catch (Exception exception) when (exception is SqliteException or InvalidOperationException)
        {
            await connection.DisposeAsync();
            throw new OfflineStoreException(
                existedWithContent ? "LOCAL_STORE_DECRYPTION_FAILED" : "LOCAL_STORE_OPEN_FAILED",
                "The encrypted local store could not be opened. It was not recreated or reset.",
                exception);
        }
        catch
        {
            await connection.DisposeAsync();
            throw;
        }
        finally
        {
            CryptographicOperations.ZeroMemory(key);
        }
    }

    private static void EnsureProviderInitialized()
    {
        _ = ProviderInitialization.Value;
    }
}
