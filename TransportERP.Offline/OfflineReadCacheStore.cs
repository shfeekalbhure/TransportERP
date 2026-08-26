using Microsoft.Data.Sqlite;

namespace TransportERP.Offline;

public sealed class OfflineReadCacheStore
{
    private static readonly HashSet<string> PermittedCacheKinds = new(StringComparer.Ordinal)
    {
        "SearchOperationalParties",
        "ReadBasicWaybillCache"
    };

    private readonly EncryptedSqliteConnectionFactory _connections;
    private readonly TimeProvider _timeProvider;

    public OfflineReadCacheStore(string databasePath, ILocalEncryptionKeyProvider keyProvider, TimeProvider? timeProvider = null)
    {
        _connections = new EncryptedSqliteConnectionFactory(databasePath, keyProvider, LocalStorePurpose.ReadCache);
        _timeProvider = timeProvider ?? TimeProvider.System;
    }

    public string DatabasePath => _connections.DatabasePath;

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await using var connection = await _connections.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = """
            PRAGMA journal_mode = WAL;
            PRAGMA synchronous = FULL;
            CREATE TABLE IF NOT EXISTS read_cache_entries (
                CacheKind TEXT NOT NULL CHECK (CacheKind IN ('SearchOperationalParties','ReadBasicWaybillCache')),
                CacheKey TEXT NOT NULL,
                PayloadJson TEXT NOT NULL,
                CachedAt TEXT NOT NULL,
                ExpiresAt TEXT NOT NULL,
                PRIMARY KEY (CacheKind, CacheKey)
            );
            """;
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task PutAsync(string cacheKind, string cacheKey, string payloadJson, TimeSpan lifetime, CancellationToken cancellationToken = default)
    {
        if (!PermittedCacheKinds.Contains(cacheKind) || string.IsNullOrWhiteSpace(cacheKey) || string.IsNullOrWhiteSpace(payloadJson) ||
            lifetime <= TimeSpan.Zero || lifetime > TimeSpan.FromHours(24))
        {
            throw new OfflineStoreException("READ_CACHE_INVALID", "Only an approved read-cache kind, key, payload, and lifetime no longer than 24 hours are accepted.");
        }

        OfflineOperationIntegrity.ValidatePayload(payloadJson);
        var now = _timeProvider.GetUtcNow();
        await using var connection = await _connections.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO read_cache_entries (CacheKind, CacheKey, PayloadJson, CachedAt, ExpiresAt)
            VALUES ($kind, $key, $payload, $now, $expires)
            ON CONFLICT (CacheKind, CacheKey) DO UPDATE SET PayloadJson = excluded.PayloadJson,
                CachedAt = excluded.CachedAt, ExpiresAt = excluded.ExpiresAt;
            """;
        command.Parameters.AddWithValue("$kind", cacheKind);
        command.Parameters.AddWithValue("$key", cacheKey);
        command.Parameters.AddWithValue("$payload", payloadJson);
        command.Parameters.AddWithValue("$now", now.ToString("O"));
        command.Parameters.AddWithValue("$expires", (now + lifetime).ToString("O"));
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<string?> GetAsync(string cacheKind, string cacheKey, CancellationToken cancellationToken = default)
    {
        if (!PermittedCacheKinds.Contains(cacheKind) || string.IsNullOrWhiteSpace(cacheKey))
        {
            throw new OfflineStoreException("READ_CACHE_INVALID", "Only approved read-cache kinds can be queried.");
        }

        var now = _timeProvider.GetUtcNow();
        await using var connection = await _connections.OpenAsync(cancellationToken);
        await using var transaction = connection.BeginTransaction(deferred: false);
        await using var delete = connection.CreateCommand();
        delete.Transaction = transaction;
        delete.CommandText = "DELETE FROM read_cache_entries WHERE ExpiresAt <= $now;";
        delete.Parameters.AddWithValue("$now", now.ToString("O"));
        await delete.ExecuteNonQueryAsync(cancellationToken);
        await using var get = connection.CreateCommand();
        get.Transaction = transaction;
        get.CommandText = "SELECT PayloadJson FROM read_cache_entries WHERE CacheKind = $kind AND CacheKey = $key;";
        get.Parameters.AddWithValue("$kind", cacheKind);
        get.Parameters.AddWithValue("$key", cacheKey);
        var result = await get.ExecuteScalarAsync(cancellationToken) as string;
        await transaction.CommitAsync(cancellationToken);
        await using var checkpoint = connection.CreateCommand();
        checkpoint.CommandText = "PRAGMA wal_checkpoint(TRUNCATE);";
        await checkpoint.ExecuteNonQueryAsync(cancellationToken);
        return result;
    }
}
