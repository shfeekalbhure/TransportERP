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
    private readonly OfflineOperationScope? _scope;

    public OfflineReadCacheStore(string databasePath, ILocalEncryptionKeyProvider keyProvider, TimeProvider? timeProvider = null)
    {
        _connections = new EncryptedSqliteConnectionFactory(databasePath, keyProvider, LocalStorePurpose.ReadCache);
        _timeProvider = timeProvider ?? TimeProvider.System;
    }

    public OfflineReadCacheStore(
        string databasePath,
        ILocalEncryptionKeyProvider keyProvider,
        OfflineOperationScope scope,
        TimeProvider? timeProvider = null)
        : this(databasePath, keyProvider, timeProvider)
    {
        _scope = scope ?? throw new ArgumentNullException(nameof(scope));
        _scope.Validate();
    }

    public string DatabasePath => _connections.DatabasePath;

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (_scope is null)
            throw new OfflineStoreException(
                "READ_CACHE_SCOPE_REQUIRED", "A read-cache database must be bound to one authenticated scope.");
        await using var connection = await _connections.OpenAsync(cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = """
            PRAGMA journal_mode = WAL;
            PRAGMA synchronous = FULL;
            CREATE TABLE IF NOT EXISTS read_cache_scope (
                Id INTEGER PRIMARY KEY NOT NULL CHECK (Id = 1),
                CompanyId TEXT NOT NULL,
                BranchId TEXT NOT NULL,
                UserId TEXT NOT NULL,
                RegisteredDeviceId TEXT NOT NULL
            );
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
        await using var transaction = connection.BeginTransaction(deferred: false);
        await VerifyOrBindScopeAsync(connection, transaction, allowEmptyBinding: true, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
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
        await VerifyOrBindScopeAsync(connection, transaction: null, allowEmptyBinding: false, cancellationToken);
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
        await VerifyOrBindScopeAsync(connection, transaction, allowEmptyBinding: false, cancellationToken);
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

    private async Task VerifyOrBindScopeAsync(
        SqliteConnection connection,
        SqliteTransaction? transaction,
        bool allowEmptyBinding,
        CancellationToken cancellationToken)
    {
        if (_scope is null)
            throw new OfflineStoreException(
                "READ_CACHE_SCOPE_REQUIRED", "A read-cache database must be bound to one authenticated scope.");

        await using var read = connection.CreateCommand();
        read.Transaction = transaction;
        read.CommandText = "SELECT CompanyId, BranchId, UserId, RegisteredDeviceId FROM read_cache_scope WHERE Id = 1;";
        await using var reader = await read.ExecuteReaderAsync(cancellationToken);
        if (await reader.ReadAsync(cancellationToken))
        {
            var matches = string.Equals(reader.GetString(0), _scope.CompanyId.ToString("D"), StringComparison.Ordinal) &&
                          string.Equals(reader.GetString(1), _scope.BranchId.ToString("D"), StringComparison.Ordinal) &&
                          string.Equals(reader.GetString(2), _scope.UserId.ToString("D"), StringComparison.Ordinal) &&
                          string.Equals(reader.GetString(3), _scope.RegisteredDeviceId.ToString("D"), StringComparison.Ordinal);
            if (!matches)
                throw new OfflineStoreException("READ_CACHE_SCOPE_DENIED", "The read cache belongs to another authenticated scope.");
            return;
        }
        await reader.DisposeAsync();

        if (!allowEmptyBinding)
            throw new OfflineStoreException("READ_CACHE_SCOPE_REQUIRED", "The read cache has no authenticated scope binding.");
        await using var count = connection.CreateCommand();
        count.Transaction = transaction;
        count.CommandText = "SELECT COUNT(*) FROM read_cache_entries;";
        if (Convert.ToInt64(await count.ExecuteScalarAsync(cancellationToken),
                System.Globalization.CultureInfo.InvariantCulture) != 0)
            throw new OfflineStoreException(
                "READ_CACHE_SCOPE_MIGRATION_REQUIRED",
                "An existing populated cache cannot be assigned to a scope without verified migration evidence.");

        await using var bind = connection.CreateCommand();
        bind.Transaction = transaction;
        bind.CommandText = """
            INSERT INTO read_cache_scope (Id, CompanyId, BranchId, UserId, RegisteredDeviceId)
            VALUES (1, $companyId, $branchId, $userId, $registeredDeviceId);
            """;
        bind.Parameters.AddWithValue("$companyId", _scope.CompanyId.ToString("D"));
        bind.Parameters.AddWithValue("$branchId", _scope.BranchId.ToString("D"));
        bind.Parameters.AddWithValue("$userId", _scope.UserId.ToString("D"));
        bind.Parameters.AddWithValue("$registeredDeviceId", _scope.RegisteredDeviceId.ToString("D"));
        await bind.ExecuteNonQueryAsync(cancellationToken);
    }
}
