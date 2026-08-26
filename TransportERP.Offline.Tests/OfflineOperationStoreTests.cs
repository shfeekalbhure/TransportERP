using System.Security.Cryptography;
using System.Text;
using TransportERP.Offline;

namespace TransportERP.Offline.Tests;

public sealed class OfflineOperationStoreTests : IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), "transporterp-offline-tests", Guid.NewGuid().ToString("N"));
    private readonly byte[] _outboxKey = RandomNumberGenerator.GetBytes(32);
    private readonly byte[] _cacheKey = RandomNumberGenerator.GetBytes(32);

    [Fact]
    public async Task Enqueue_is_durable_and_duplicate_intent_preserves_stable_identities()
    {
        var path = OutboxPath();
        var request = Request();
        var firstStore = Store(path);
        await firstStore.InitializeAsync();

        var first = await firstStore.EnqueueAsync(request);
        var duplicate = await firstStore.EnqueueAsync(request);
        var reopened = Store(path);
        await reopened.InitializeAsync();
        var afterRestart = await reopened.GetAsync(first.Operation.LocalOperationId);

        Assert.True(first.Created);
        Assert.False(duplicate.Created);
        Assert.Equal(first.Operation.LocalOperationId, duplicate.Operation.LocalOperationId);
        Assert.Equal(first.Operation.ClientOperationId, afterRestart!.ClientOperationId);
        Assert.Equal(first.Operation.OperationCorrelationId, afterRestart.OperationCorrelationId);
        Assert.Equal(OfflineOperationStatus.Queued, afterRestart.Status);

        var changed = request with { PayloadJson = "{\"amount\":43}" };
        var error = await Assert.ThrowsAsync<OfflineStoreException>(() => reopened.EnqueueAsync(changed));
        Assert.Equal("LOCAL_IDEMPOTENCY_MISMATCH", error.Code);
    }

    [Fact]
    public async Task Concurrent_duplicate_enqueue_creates_exactly_one_local_operation()
    {
        var store = Store(OutboxPath());
        await store.InitializeAsync();
        var request = Request();

        var results = await Task.WhenAll(store.EnqueueAsync(request), store.EnqueueAsync(request));

        Assert.Single(results.Where(result => result.Created));
        Assert.Single(results.Where(result => !result.Created));
        Assert.Single(results.Select(result => result.Operation.LocalOperationId).Distinct());
        Assert.Single(results.Select(result => result.Operation.ClientOperationId).Distinct());
        Assert.Single(results.Select(result => result.Operation.OperationCorrelationId).Distinct());
    }

    [Fact]
    public async Task Unknown_and_generic_delete_actions_fail_before_persistence()
    {
        var store = Store(OutboxPath());
        await store.InitializeAsync();

        var unknown = await Assert.ThrowsAsync<OfflineStoreException>(() =>
            store.EnqueueAsync(Request() with { ActionCode = "UnknownAction" }));
        var delete = await Assert.ThrowsAsync<OfflineStoreException>(() =>
            store.EnqueueAsync(Request(Guid.NewGuid()) with { OperationType = "DELETE" }));

        Assert.Equal("ACTION_RUNTIME_UNAVAILABLE", unknown.Code);
        Assert.Equal("ACTION_RUNTIME_UNAVAILABLE", delete.Code);
        Assert.Null(await store.ClaimNextAsync("worker", TimeSpan.FromMinutes(1)));
    }

    [Fact]
    public async Task Expired_sending_lease_is_recovered_after_restart_with_new_attempt_identity()
    {
        var clock = new ManualTimeProvider(new DateTimeOffset(2026, 8, 26, 8, 0, 0, TimeSpan.Zero));
        var path = OutboxPath();
        var store = Store(path, clock);
        await store.InitializeAsync();
        var queued = await store.EnqueueAsync(Request());
        var firstAttempt = await store.ClaimNextAsync("worker-a", TimeSpan.FromMinutes(1));

        clock.Advance(TimeSpan.FromMinutes(2));
        var reopened = Store(path, clock);
        var recovered = await reopened.ClaimNextAsync("worker-b", TimeSpan.FromMinutes(1));

        Assert.NotNull(firstAttempt);
        Assert.NotNull(recovered);
        Assert.Equal(queued.Operation.LocalOperationId, recovered!.LocalOperationId);
        Assert.Equal(queued.Operation.ClientOperationId, recovered.ClientOperationId);
        Assert.Equal(queued.Operation.OperationCorrelationId, recovered.OperationCorrelationId);
        Assert.NotEqual(firstAttempt!.AttemptCorrelationId, recovered.AttemptCorrelationId);
        Assert.Equal("worker-b", recovered.LeaseOwner);
    }

    [Fact]
    public async Task Two_workers_cannot_claim_the_same_operation()
    {
        var store = Store(OutboxPath());
        await store.InitializeAsync();
        await store.EnqueueAsync(Request());

        var claims = await Task.WhenAll(
            store.ClaimNextAsync("worker-a", TimeSpan.FromMinutes(1)),
            store.ClaimNextAsync("worker-b", TimeSpan.FromMinutes(1)));

        Assert.Single(claims.Where(operation => operation is not null));
        Assert.Single(claims.Where(operation => operation is null));
    }

    [Fact]
    public async Task Every_retry_claim_gets_a_new_attempt_but_preserves_business_identity()
    {
        var clock = new ManualTimeProvider(new DateTimeOffset(2026, 8, 26, 8, 0, 0, TimeSpan.Zero));
        var store = Store(OutboxPath(), clock);
        await store.InitializeAsync();
        var queued = await store.EnqueueAsync(Request());
        var first = await store.ClaimNextAsync("worker", TimeSpan.FromMinutes(1));
        await store.MarkTransportFailureAsync(first!.LocalOperationId, first.AttemptCorrelationId!.Value, true, "TIMEOUT");

        clock.Advance(TimeSpan.FromSeconds(5));
        var second = await store.ClaimNextAsync("worker", TimeSpan.FromMinutes(1));

        Assert.NotNull(second);
        Assert.NotEqual(first.AttemptCorrelationId, second!.AttemptCorrelationId);
        Assert.Equal(queued.Operation.ClientOperationId, second.ClientOperationId);
        Assert.Equal(queued.Operation.OperationCorrelationId, second.OperationCorrelationId);
        Assert.Equal(1, second.ClientTransportRetryCount);
    }

    [Fact]
    public async Task Retry_budget_exhaustion_rejects_without_an_extra_retry()
    {
        var clock = new ManualTimeProvider(new DateTimeOffset(2026, 8, 26, 8, 0, 0, TimeSpan.Zero));
        var policy = new OfflineRetryPolicy(2, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(4));
        var store = Store(OutboxPath(), clock, policy);
        await store.InitializeAsync();
        await store.EnqueueAsync(Request());

        for (var attemptNumber = 0; attemptNumber < 3; attemptNumber++)
        {
            var attempt = await store.ClaimNextAsync("worker", TimeSpan.FromMinutes(1));
            Assert.NotNull(attempt);
            await store.MarkTransportFailureAsync(attempt!.LocalOperationId, attempt.AttemptCorrelationId!.Value, true, "TIMEOUT");
            clock.Advance(TimeSpan.FromSeconds(4));
        }

        var operation = await store.GetAsync((await store.EnqueueAsync(Request())).Operation.LocalOperationId);
        Assert.Equal(OfflineOperationStatus.Rejected, operation!.Status);
        Assert.Equal("RETRY_EXHAUSTED", operation.ResultCode);
        Assert.Equal(2, operation.ClientTransportRetryCount);
        Assert.Null(await store.ClaimNextAsync("worker", TimeSpan.FromMinutes(1)));
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 10)]
    [InlineData(3, 20)]
    [InlineData(4, 40)]
    [InlineData(5, 80)]
    public void Default_retry_policy_uses_bounded_exponential_backoff(int retryNumber, int expectedSeconds)
    {
        Assert.Equal(TimeSpan.FromSeconds(expectedSeconds), new OfflineRetryPolicy().DelayForRetry(retryNumber));
    }

    [Fact]
    public async Task Conflict_can_be_resolved_but_stale_attempt_cannot_complete_it()
    {
        var store = Store(OutboxPath());
        await store.InitializeAsync();
        await store.EnqueueAsync(Request());
        var attempt = await store.ClaimNextAsync("worker", TimeSpan.FromMinutes(1));
        await store.MarkConflictAsync(attempt!.LocalOperationId, attempt.AttemptCorrelationId!.Value, "BASE_VERSION_CONFLICT");
        await store.MarkResolvedAsync(attempt.LocalOperationId, "KEEP_SERVER");

        var operation = await store.GetAsync(attempt.LocalOperationId);
        Assert.Equal(OfflineOperationStatus.Resolved, operation!.Status);
        await Assert.ThrowsAsync<OfflineStoreException>(() =>
            store.MarkSucceededAsync(attempt.LocalOperationId, attempt.AttemptCorrelationId.Value, null, null));
    }

    [Fact]
    public async Task Retention_redacts_only_acknowledged_terminal_payloads_at_exact_boundaries()
    {
        var clock = new ManualTimeProvider(new DateTimeOffset(2026, 8, 26, 8, 0, 0, TimeSpan.Zero));
        var store = Store(OutboxPath(), clock);
        await store.InitializeAsync();
        var succeeded = await store.EnqueueAsync(Request());
        var succeededAttempt = await store.ClaimNextAsync("worker", TimeSpan.FromMinutes(1));
        await store.MarkSucceededAsync(succeededAttempt!.LocalOperationId, succeededAttempt.AttemptCorrelationId!.Value, Guid.NewGuid(), 1);
        var rejected = await store.EnqueueAsync(Request(Guid.NewGuid()));
        var rejectedAttempt = await store.ClaimNextAsync("worker", TimeSpan.FromMinutes(1));
        await store.MarkRejectedAsync(rejectedAttempt!.LocalOperationId, rejectedAttempt.AttemptCorrelationId!.Value, "SCOPE_DENIED");
        var pending = await store.EnqueueAsync(Request(Guid.NewGuid()));

        clock.Advance(TimeSpan.FromHours(24));
        var concurrentCleanup = await Task.WhenAll(store.RedactExpiredPayloadsAsync(), store.RedactExpiredPayloadsAsync());
        Assert.Equal(1, concurrentCleanup.Sum());
        Assert.Null((await store.GetAsync(succeeded.Operation.LocalOperationId))!.PayloadJson);
        Assert.NotNull((await store.GetAsync(rejected.Operation.LocalOperationId))!.PayloadJson);
        Assert.NotNull((await store.GetAsync(pending.Operation.LocalOperationId))!.PayloadJson);

        clock.Advance(TimeSpan.FromDays(6));
        Assert.Equal(1, await store.RedactExpiredPayloadsAsync());
        Assert.Null((await store.GetAsync(rejected.Operation.LocalOperationId))!.PayloadJson);
        Assert.NotNull((await store.GetAsync(pending.Operation.LocalOperationId))!.PayloadJson);
        Assert.Equal(0, await store.RedactExpiredPayloadsAsync());
    }

    [Fact]
    public async Task Wrong_key_and_tampered_ciphertext_fail_closed_without_recreating_store()
    {
        var path = OutboxPath();
        var store = Store(path);
        await store.InitializeAsync();
        await store.EnqueueAsync(Request());
        var originalLength = new FileInfo(path).Length;

        var wrongKeyStore = new OfflineOperationStore(path, new FixedKeyProvider(RandomNumberGenerator.GetBytes(32), _cacheKey));
        var wrongKey = await Assert.ThrowsAsync<OfflineStoreException>(() => wrongKeyStore.InitializeAsync());
        Assert.Equal("LOCAL_STORE_DECRYPTION_FAILED", wrongKey.Code);
        Assert.Equal(originalLength, new FileInfo(path).Length);

        var bytes = await File.ReadAllBytesAsync(path);
        bytes[Math.Min(bytes.Length - 1, 128)] ^= 0x5a;
        await File.WriteAllBytesAsync(path, bytes);
        var tamperedStore = Store(path);
        var tampered = await Assert.ThrowsAsync<OfflineStoreException>(() => tamperedStore.InitializeAsync());
        Assert.Equal("LOCAL_STORE_DECRYPTION_FAILED", tampered.Code);
    }

    [Fact]
    public async Task Transport_secrets_are_rejected_and_plaintext_is_absent_from_encrypted_files()
    {
        var directory = _directory;
        var store = Store(OutboxPath());
        await store.InitializeAsync();
        var rawSecret = "secret-material-that-must-never-be-persisted";
        var secretRequest = Request() with { PayloadJson = $"{{\"accessToken\":\"{rawSecret}\"}}" };
        var denied = await Assert.ThrowsAsync<OfflineStoreException>(() => store.EnqueueAsync(secretRequest));
        Assert.Equal("TRANSPORT_SECRET_PERSISTENCE_DENIED", denied.Code);

        var businessMarker = "business-payload-plaintext-marker";
        await store.EnqueueAsync(Request(Guid.NewGuid()) with { PayloadJson = $"{{\"note\":\"{businessMarker}\"}}" });
        var bytes = Directory.EnumerateFiles(directory).SelectMany(File.ReadAllBytes).ToArray();
        var fileText = Encoding.UTF8.GetString(bytes);
        Assert.DoesNotContain(rawSecret, fileText, StringComparison.Ordinal);
        Assert.DoesNotContain(businessMarker, fileText, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Read_cache_uses_a_separate_encrypted_database_and_never_creates_outbox_rows()
    {
        var cachePath = Path.Combine(_directory, "read-cache.db");
        var outboxPath = OutboxPath();
        var cache = new OfflineReadCacheStore(cachePath, Keys());
        var outbox = Store(outboxPath);
        await cache.InitializeAsync();
        await outbox.InitializeAsync();
        await cache.PutAsync("SearchOperationalParties", "party:1", "{\"name\":\"cached\"}", TimeSpan.FromHours(24));

        Assert.Equal("{\"name\":\"cached\"}", await cache.GetAsync("SearchOperationalParties", "party:1"));
        Assert.Null(await outbox.ClaimNextAsync("worker", TimeSpan.FromMinutes(1)));
        Assert.NotEqual(Path.GetFullPath(cache.DatabasePath), Path.GetFullPath(outbox.DatabasePath));
        var denied = await Assert.ThrowsAsync<OfflineStoreException>(() =>
            cache.PutAsync("ArbitraryApiResponse", "secret", "{}", TimeSpan.FromHours(1)));
        Assert.Equal("READ_CACHE_INVALID", denied.Code);
    }

    public void Dispose()
    {
        if (Directory.Exists(_directory))
        {
            Directory.Delete(_directory, recursive: true);
        }

        CryptographicOperations.ZeroMemory(_outboxKey);
        CryptographicOperations.ZeroMemory(_cacheKey);
    }

    private string OutboxPath() => Path.Combine(_directory, "outbox.db");

    private OfflineOperationStore Store(string path, TimeProvider? clock = null, OfflineRetryPolicy? retryPolicy = null) =>
        new(path, Keys(), clock, retryPolicy);

    private FixedKeyProvider Keys() => new(_outboxKey, _cacheKey);

    private static OfflineOperationEnqueueRequest Request(Guid? localIntentId = null) => new(
        localIntentId ?? Guid.NewGuid(),
        Guid.Parse("11111111-1111-1111-1111-111111111111"),
        Guid.Parse("22222222-2222-2222-2222-222222222222"),
        Guid.Parse("33333333-3333-3333-3333-333333333333"),
        Guid.Parse("44444444-4444-4444-4444-444444444444"),
        "CreateWaybillDraft",
        "CREATE",
        "Waybill",
        null,
        null,
        new DateTimeOffset(2026, 8, 26, 7, 0, 0, TimeSpan.Zero),
        "{\"amount\":42}");

    private sealed class FixedKeyProvider(byte[] outboxKey, byte[] cacheKey) : ILocalEncryptionKeyProvider
    {
        public ValueTask<byte[]> GetKeyAsync(LocalStorePurpose purpose, CancellationToken cancellationToken = default) =>
            ValueTask.FromResult((purpose == LocalStorePurpose.WriteOutbox ? outboxKey : cacheKey).ToArray());
    }

    private sealed class ManualTimeProvider(DateTimeOffset initial) : TimeProvider
    {
        private DateTimeOffset _current = initial;
        public override DateTimeOffset GetUtcNow() => _current;
        public void Advance(TimeSpan duration) => _current += duration;
    }
}
