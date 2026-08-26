using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
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

        var first = await EnqueueRequestAsync(firstStore, request);
        var duplicate = await EnqueueRequestAsync(firstStore, request);
        var reopened = Store(path);
        await reopened.InitializeAsync();
        var afterRestart = await reopened.GetAsync(first.Operation.LocalOperationId, Scope());

        Assert.True(first.Created);
        Assert.False(duplicate.Created);
        Assert.Equal(first.Operation.LocalOperationId, duplicate.Operation.LocalOperationId);
        Assert.Equal(first.Operation.ClientOperationId, afterRestart!.ClientOperationId);
        Assert.Equal(first.Operation.OperationCorrelationId, afterRestart.OperationCorrelationId);
        Assert.Equal(OfflineOperationStatus.Queued, afterRestart.Status);

        var changed = request with { PayloadJson = "{\"amount\":43}" };
        var payloadReplay = await EnqueueRequestAsync(reopened, changed);
        Assert.False(payloadReplay.Created);
        Assert.Equal(first.Operation.LocalOperationId, payloadReplay.Operation.LocalOperationId);
        Assert.Equal(first.Operation.PayloadHash, payloadReplay.Operation.PayloadHash);
    }

    [Fact]
    public async Task Concurrent_duplicate_enqueue_creates_exactly_one_local_operation()
    {
        var store = Store(OutboxPath());
        await store.InitializeAsync();
        var request = Request();

        var results = await Task.WhenAll(EnqueueRequestAsync(store, request), EnqueueRequestAsync(store, request));

        Assert.Single(results, result => result.Created);
        Assert.Single(results, result => !result.Created);
        Assert.Single(results.Select(result => result.Operation.LocalOperationId).Distinct());
        Assert.Single(results.Select(result => result.Operation.ClientOperationId).Distinct());
        Assert.Single(results.Select(result => result.Operation.OperationCorrelationId).Distinct());
    }

    [Fact]
    public async Task Identity_bound_payload_factory_generates_business_ids_once_and_replay_does_not_reinvoke_it()
    {
        var store = Store(OutboxPath());
        await store.InitializeAsync();
        var template = Template();
        OfflineGeneratedOperationIdentity? generated = null;
        var calls = 0;

        var first = await store.EnqueueAsync(template, identity =>
        {
            calls++;
            generated = identity;
            return $"{{\"clientOperationId\":\"{identity.ClientOperationId}\",\"nameAr\":\"طرف\"}}";
        });
        var replay = await store.EnqueueAsync(template, _ =>
            throw new InvalidOperationException("An idempotent local replay must not regenerate payload identity."));

        Assert.Equal(1, calls);
        Assert.True(first.Created);
        Assert.False(replay.Created);
        Assert.Equal(generated!.ClientOperationId, first.Operation.ClientOperationId);
        Assert.Equal(generated.OperationCorrelationId, first.Operation.OperationCorrelationId);
        Assert.Equal(first.Operation.LocalOperationId, replay.Operation.LocalOperationId);

        var mismatch = await Assert.ThrowsAsync<OfflineStoreException>(() => store.EnqueueAsync(
            Template(Guid.NewGuid()), _ => "{\"clientOperationId\":\"caller-changed\"}"));
        Assert.Equal("LOCAL_PAYLOAD_IDENTITY_MISMATCH", mismatch.Code);
    }

    [Fact]
    public async Task Seven_day_non_terminal_boundary_blocks_new_scope_writes_without_deleting_the_old_queue()
    {
        var clock = new ManualTimeProvider(new DateTimeOffset(2026, 8, 1, 0, 0, 0, TimeSpan.Zero));
        var store = Store(OutboxPath(), clock);
        await store.InitializeAsync();
        var oldest = await EnqueueRequestAsync(store, Request());

        clock.Advance(TimeSpan.FromDays(7) - TimeSpan.FromTicks(1));
        var beforeBoundary = await EnqueueRequestAsync(store, Request(Guid.NewGuid()));
        Assert.True(beforeBoundary.Created);

        clock.Advance(TimeSpan.FromTicks(1));
        var blocked = await Assert.ThrowsAsync<OfflineStoreException>(() =>
            EnqueueRequestAsync(store, Request(Guid.NewGuid())));

        Assert.Equal("OFFLINE_QUEUE_ESCALATION_REQUIRED", blocked.Code);
        Assert.Equal(OfflineOperationStatus.Queued,
            (await store.GetAsync(oldest.Operation.LocalOperationId, Scope()))!.Status);
        Assert.Equal(2, (await store.ListAsync(Scope())).Count);
    }

    [Fact]
    public async Task Unknown_and_generic_delete_actions_fail_before_persistence()
    {
        var store = Store(OutboxPath());
        await store.InitializeAsync();

        var unknown = await Assert.ThrowsAsync<OfflineStoreException>(() =>
            EnqueueRequestAsync(store, Request() with { ActionCode = "UnknownAction" }));
        var delete = await Assert.ThrowsAsync<OfflineStoreException>(() =>
            EnqueueRequestAsync(store, Request(Guid.NewGuid()) with { OperationType = "DELETE" }));

        Assert.Equal("ACTION_RUNTIME_UNAVAILABLE", unknown.Code);
        Assert.Equal("ACTION_RUNTIME_UNAVAILABLE", delete.Code);
        Assert.Null(await store.ClaimNextAsync("worker", TimeSpan.FromMinutes(1), Scope()));
    }

    [Fact]
    public async Task Expired_sending_lease_is_recovered_after_restart_with_new_attempt_identity()
    {
        var clock = new ManualTimeProvider(new DateTimeOffset(2026, 8, 26, 8, 0, 0, TimeSpan.Zero));
        var path = OutboxPath();
        var store = Store(path, clock);
        await store.InitializeAsync();
        var queued = await EnqueueRequestAsync(store, Request());
        var firstAttempt = await store.ClaimNextAsync("worker-a", TimeSpan.FromMinutes(1), Scope());

        clock.Advance(TimeSpan.FromMinutes(2));
        var reopened = Store(path, clock);
        var recovered = await reopened.ClaimNextAsync("worker-b", TimeSpan.FromMinutes(1), Scope());

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
        await EnqueueRequestAsync(store, Request());

        var claims = await Task.WhenAll(
            store.ClaimNextAsync("worker-a", TimeSpan.FromMinutes(1), Scope()),
            store.ClaimNextAsync("worker-b", TimeSpan.FromMinutes(1), Scope()));

        Assert.Single(claims, operation => operation is not null);
        Assert.Single(claims, operation => operation is null);
    }

    [Fact]
    public async Task Every_retry_claim_gets_a_new_attempt_but_preserves_business_identity()
    {
        var clock = new ManualTimeProvider(new DateTimeOffset(2026, 8, 26, 8, 0, 0, TimeSpan.Zero));
        var store = Store(OutboxPath(), clock);
        await store.InitializeAsync();
        var queued = await EnqueueRequestAsync(store, Request());
        var first = await store.ClaimNextAsync("worker", TimeSpan.FromMinutes(1), Scope());
        await store.MarkTransportFailureAsync(first!.LocalOperationId, first.AttemptCorrelationId!.Value, true, "TIMEOUT");

        clock.Advance(TimeSpan.FromSeconds(5));
        var second = await store.ClaimNextAsync("worker", TimeSpan.FromMinutes(1), Scope());

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
        var queued = await EnqueueRequestAsync(store, Request());

        for (var attemptNumber = 0; attemptNumber < 3; attemptNumber++)
        {
            var attempt = await store.ClaimNextAsync("worker", TimeSpan.FromMinutes(1), Scope());
            Assert.NotNull(attempt);
            await store.MarkTransportFailureAsync(attempt!.LocalOperationId, attempt.AttemptCorrelationId!.Value, true, "TIMEOUT");
            clock.Advance(TimeSpan.FromSeconds(4));
        }

        var operation = await store.GetAsync(queued.Operation.LocalOperationId, Scope());
        Assert.Equal(OfflineOperationStatus.Rejected, operation!.Status);
        Assert.Equal("RETRY_EXHAUSTED", operation.ResultCode);
        Assert.Equal(2, operation.ClientTransportRetryCount);
        Assert.Null(await store.ClaimNextAsync("worker", TimeSpan.FromMinutes(1), Scope()));
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
    public void Client_retry_configuration_can_only_tighten_the_global_ceiling()
    {
        _ = Store(OutboxPath(), retryPolicy: new OfflineRetryPolicy(5));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            Store(OutboxPath(), retryPolicy: new OfflineRetryPolicy(6)));
    }

    [Fact]
    public async Task Legacy_payload_enqueue_fails_closed_before_persistence()
    {
        var store = Store(OutboxPath());
        await store.InitializeAsync();
#pragma warning disable CS0618
        var denied = await Assert.ThrowsAsync<OfflineStoreException>(() => store.EnqueueAsync(Request()));
#pragma warning restore CS0618
        Assert.Equal("OFFLINE_IDENTITY_FACTORY_REQUIRED", denied.Code);
        Assert.Empty(await store.ListAsync(Scope()));
    }

    [Fact]
    public async Task Conflict_can_be_resolved_but_stale_attempt_cannot_complete_it()
    {
        var clock = new ManualTimeProvider(new DateTimeOffset(2026, 8, 26, 8, 0, 0, TimeSpan.Zero));
        var store = Store(OutboxPath(), clock);
        await store.InitializeAsync();
        await EnqueueRequestAsync(store, Request());
        var attempt = await store.ClaimNextAsync("worker", TimeSpan.FromMinutes(1), Scope());
        var conflictCaseId = Guid.NewGuid();
        await store.MarkConflictAsync(attempt!.LocalOperationId, attempt.AttemptCorrelationId!.Value,
            conflictCaseId, "BASE_VERSION_CONFLICT", ConflictReview(attempt));
        var encryptedBytes = Directory.EnumerateFiles(_directory).SelectMany(File.ReadAllBytes).ToArray();
        Assert.DoesNotContain("BASE_VERSION_CONFLICT", Encoding.UTF8.GetString(encryptedBytes),
            StringComparison.Ordinal);
        await store.MarkResolvedAsync(attempt.LocalOperationId, "KEEP_SERVER",
            new OfflineConflictResolutionOutcome("KEEP_SERVER_AND_REJECT_LOCAL", "RESOLVED", true,
                clock.GetUtcNow(), null), Scope());

        var operation = await store.GetAsync(attempt.LocalOperationId, Scope());
        Assert.Equal(OfflineOperationStatus.Resolved, operation!.Status);
        Assert.Equal(conflictCaseId, operation.ConflictCaseId);
        await Assert.ThrowsAsync<OfflineStoreException>(() =>
            store.MarkSucceededAsync(attempt.LocalOperationId, attempt.AttemptCorrelationId.Value, null, null));

        clock.Advance(TimeSpan.FromHours(24));
        Assert.Equal(1, await store.RedactExpiredPayloadsAsync());
        var redacted = (await store.GetAsync(attempt.LocalOperationId, Scope()))!;
        Assert.Null(redacted.PayloadJson);
        Assert.Null(redacted.ConflictReview);
        Assert.Equal(conflictCaseId, redacted.ConflictCaseId);
        Assert.Equal("KEEP_SERVER", redacted.ResultCode);
    }

    [Fact]
    public async Task List_and_manual_retry_preserve_identity_and_only_requeue_failed_rows()
    {
        var store = Store(OutboxPath());
        await store.InitializeAsync();
        var queued = await EnqueueRequestAsync(store, Request());
        var attempt = await store.ClaimNextAsync("worker", TimeSpan.FromMinutes(1), Scope());
        await store.MarkTransportFailureAsync(attempt!.LocalOperationId,
            attempt.AttemptCorrelationId!.Value, true, "TIMEOUT");

        await store.RequeueFailedAsync(queued.Operation.LocalOperationId, Scope());
        var listed = Assert.Single(await store.ListAsync(Scope()));

        Assert.Equal(OfflineOperationStatus.Queued, listed.Status);
        Assert.Equal(queued.Operation.ClientOperationId, listed.ClientOperationId);
        Assert.Equal(queued.Operation.OperationCorrelationId, listed.OperationCorrelationId);
        Assert.Null(listed.AttemptCorrelationId);
        Assert.Null(listed.ResultCode);
        var error = await Assert.ThrowsAsync<OfflineStoreException>(() =>
            store.RequeueFailedAsync(listed.LocalOperationId, Scope()));
        Assert.Equal("LOCAL_STATE_CONFLICT", error.Code);
    }

    [Fact]
    public async Task Manual_retry_and_resolution_are_denied_for_every_cross_scope_identity()
    {
        var store = Store(OutboxPath());
        await store.InitializeAsync();
        var failed = await EnqueueRequestAsync(store, Request());
        var failedAttempt = await store.ClaimNextAsync("worker", TimeSpan.FromMinutes(1), Scope());
        await store.MarkTransportFailureAsync(failedAttempt!.LocalOperationId,
            failedAttempt.AttemptCorrelationId!.Value, true, "TIMEOUT");

        var wrongScopes = new[]
        {
            Scope() with { CompanyId = Guid.NewGuid() },
            Scope() with { BranchId = Guid.NewGuid() },
            Scope() with { UserId = Guid.NewGuid() },
            Scope() with { RegisteredDeviceId = Guid.NewGuid() }
        };
        foreach (var wrongScope in wrongScopes)
        {
            var retryDenied = await Assert.ThrowsAsync<OfflineStoreException>(() =>
                store.RequeueFailedAsync(failed.Operation.LocalOperationId, wrongScope));
            Assert.Equal("LOCAL_STATE_CONFLICT", retryDenied.Code);
        }
        Assert.Equal(OfflineOperationStatus.Failed,
            (await store.GetAsync(failed.Operation.LocalOperationId, Scope()))!.Status);

        var conflict = await EnqueueRequestAsync(store, Request(Guid.NewGuid()));
        var conflictAttempt = await store.ClaimNextAsync("worker", TimeSpan.FromMinutes(1), Scope());
        await store.MarkConflictAsync(conflictAttempt!.LocalOperationId,
            conflictAttempt.AttemptCorrelationId!.Value, Guid.NewGuid(), "BASE_VERSION_CONFLICT",
            ConflictReview(conflictAttempt));
        var resolution = new OfflineConflictResolutionOutcome("KEEP_SERVER_AND_REJECT_LOCAL", "RESOLVED",
            true, DateTimeOffset.UtcNow, null);
        foreach (var wrongScope in wrongScopes)
        {
            var resolveDenied = await Assert.ThrowsAsync<OfflineStoreException>(() =>
                store.MarkResolvedAsync(conflict.Operation.LocalOperationId, "KEEP_SERVER", resolution, wrongScope));
            Assert.Equal("LOCAL_STATE_CONFLICT", resolveDenied.Code);
        }
        Assert.Equal(OfflineOperationStatus.Conflict,
            (await store.GetAsync(conflict.Operation.LocalOperationId, Scope()))!.Status);
    }

    [Fact]
    public async Task Operation_get_and_list_require_exact_authenticated_scope()
    {
        var store = Store(OutboxPath());
        await store.InitializeAsync();
        var queued = await EnqueueRequestAsync(store, Request());
        var other = Scope() with { UserId = Guid.NewGuid() };

        Assert.Null(await store.GetAsync(queued.Operation.LocalOperationId, other));
        Assert.Empty(await store.ListAsync(other));
        Assert.NotNull(await store.GetAsync(queued.Operation.LocalOperationId, Scope()));
        var unscoped = await Assert.ThrowsAsync<OfflineStoreException>(() =>
            store.GetAsync(queued.Operation.LocalOperationId));
        Assert.Equal("LOCAL_SCOPE_REQUIRED", unscoped.Code);
    }

    [Fact]
    public async Task Retention_redacts_only_acknowledged_terminal_payloads_at_exact_boundaries()
    {
        var clock = new ManualTimeProvider(new DateTimeOffset(2026, 8, 26, 8, 0, 0, TimeSpan.Zero));
        var store = Store(OutboxPath(), clock);
        await store.InitializeAsync();
        var succeeded = await EnqueueRequestAsync(store, Request());
        var succeededAttempt = await store.ClaimNextAsync("worker", TimeSpan.FromMinutes(1), Scope());
        await store.MarkSucceededAsync(succeededAttempt!.LocalOperationId, succeededAttempt.AttemptCorrelationId!.Value, Guid.NewGuid(), 1);
        var rejected = await EnqueueRequestAsync(store, Request(Guid.NewGuid()));
        var rejectedAttempt = await store.ClaimNextAsync("worker", TimeSpan.FromMinutes(1), Scope());
        await store.MarkRejectedAsync(rejectedAttempt!.LocalOperationId, rejectedAttempt.AttemptCorrelationId!.Value, "SCOPE_DENIED");
        var pending = await EnqueueRequestAsync(store, Request(Guid.NewGuid()));

        clock.Advance(TimeSpan.FromHours(24));
        var concurrentCleanup = await Task.WhenAll(store.RedactExpiredPayloadsAsync(), store.RedactExpiredPayloadsAsync());
        Assert.Equal(1, concurrentCleanup.Sum());
        Assert.Null((await store.GetAsync(succeeded.Operation.LocalOperationId, Scope()))!.PayloadJson);
        Assert.NotNull((await store.GetAsync(rejected.Operation.LocalOperationId, Scope()))!.PayloadJson);
        Assert.NotNull((await store.GetAsync(pending.Operation.LocalOperationId, Scope()))!.PayloadJson);

        clock.Advance(TimeSpan.FromDays(6));
        Assert.Equal(1, await store.RedactExpiredPayloadsAsync());
        Assert.Null((await store.GetAsync(rejected.Operation.LocalOperationId, Scope()))!.PayloadJson);
        Assert.NotNull((await store.GetAsync(pending.Operation.LocalOperationId, Scope()))!.PayloadJson);
        Assert.Equal(0, await store.RedactExpiredPayloadsAsync());
    }

    [Fact]
    public async Task Wrong_key_and_tampered_ciphertext_fail_closed_without_recreating_store()
    {
        var path = OutboxPath();
        var store = Store(path);
        await store.InitializeAsync();
        await EnqueueRequestAsync(store, Request());
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
        var denied = await Assert.ThrowsAsync<OfflineStoreException>(() => EnqueueRequestAsync(store, secretRequest));
        Assert.Equal("TRANSPORT_SECRET_PERSISTENCE_DENIED", denied.Code);

        var businessMarker = "business-payload-plaintext-marker";
        await EnqueueRequestAsync(store, Request(Guid.NewGuid()) with { PayloadJson = $"{{\"note\":\"{businessMarker}\"}}" });
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
        var cache = new OfflineReadCacheStore(cachePath, Keys(), Scope());
        var outbox = Store(outboxPath);
        await cache.InitializeAsync();
        await outbox.InitializeAsync();
        await cache.PutAsync("SearchOperationalParties", "party:1", "{\"name\":\"cached\"}", TimeSpan.FromHours(24));

        Assert.Equal("{\"name\":\"cached\"}", await cache.GetAsync("SearchOperationalParties", "party:1"));
        Assert.Null(await outbox.ClaimNextAsync("worker", TimeSpan.FromMinutes(1), Scope()));
        Assert.NotEqual(Path.GetFullPath(cache.DatabasePath), Path.GetFullPath(outbox.DatabasePath));
        var denied = await Assert.ThrowsAsync<OfflineStoreException>(() =>
            cache.PutAsync("ArbitraryApiResponse", "secret", "{}", TimeSpan.FromHours(1)));
        Assert.Equal("READ_CACHE_INVALID", denied.Code);
    }

    [Fact]
    public async Task Read_cache_database_is_bound_to_one_authenticated_scope_and_cross_scope_open_fails_closed()
    {
        var path = Path.Combine(_directory, "scope-bound-cache.db");
        var first = new OfflineReadCacheStore(path, Keys(), Scope());
        await first.InitializeAsync();
        await first.PutAsync("SearchOperationalParties", "party:1", "{\"name\":\"scope-a\"}", TimeSpan.FromHours(1));

        var otherScope = Scope() with { UserId = Guid.NewGuid() };
        var second = new OfflineReadCacheStore(path, Keys(), otherScope);
        var denied = await Assert.ThrowsAsync<OfflineStoreException>(() => second.InitializeAsync());

        Assert.Equal("READ_CACHE_SCOPE_DENIED", denied.Code);
        Assert.Equal("{\"name\":\"scope-a\"}",
            await first.GetAsync("SearchOperationalParties", "party:1"));
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

    private static OfflineOperationScope Scope() => new(
        Guid.Parse("11111111-1111-1111-1111-111111111111"),
        Guid.Parse("22222222-2222-2222-2222-222222222222"),
        Guid.Parse("33333333-3333-3333-3333-333333333333"),
        Guid.Parse("44444444-4444-4444-4444-444444444444"));

    private static OfflineOperationEnqueueRequest Request(Guid? localIntentId = null) => new(
        localIntentId ?? Guid.NewGuid(),
        Guid.Parse("11111111-1111-1111-1111-111111111111"),
        Guid.Parse("22222222-2222-2222-2222-222222222222"),
        Guid.Parse("33333333-3333-3333-3333-333333333333"),
        Guid.Parse("44444444-4444-4444-4444-444444444444"),
        "UpdateWaybillDraft",
        "UPDATE",
        "Waybill",
        Guid.Parse("55555555-5555-5555-5555-555555555555"),
        1,
        new DateTimeOffset(2026, 8, 26, 7, 0, 0, TimeSpan.Zero),
        "{\"amount\":42}");

    private static Task<OfflineEnqueueResult> EnqueueRequestAsync(
        OfflineOperationStore store,
        OfflineOperationEnqueueRequest request) =>
        store.EnqueueAsync(new OfflineOperationEnqueueTemplate(
                request.LocalIntentId, request.CompanyId, request.BranchId, request.UserId,
                request.RegisteredDeviceId, request.ActionCode, request.OperationType, request.EntityType,
                request.EntityId, request.BaseVersion, request.ClientOccurredAt),
            identity => BindPayloadIdentity(request.PayloadJson, identity.ClientOperationId));

    private static string BindPayloadIdentity(string payloadJson, string clientOperationId)
    {
        var payload = JsonNode.Parse(payloadJson)?.AsObject()
            ?? throw new InvalidOperationException("The test payload must be an object.");
        payload["clientOperationId"] = clientOperationId;
        return payload.ToJsonString();
    }

    private static OfflineConflictReview ConflictReview(OfflineOperation operation) => new(
        operation.BaseVersion ?? 1,
        "BASE_VERSION_CONFLICT",
        new OfflineConflictLocalSnapshot(operation.ActionCode, operation.EntityType, operation.EntityId,
            operation.BaseVersion ?? 1),
        new OfflineConflictServerSnapshot(operation.EntityType, operation.EntityId, true,
            (operation.BaseVersion ?? 1) + 1),
        "OPEN");

    private static OfflineOperationEnqueueTemplate Template(Guid? localIntentId = null) => new(
        localIntentId ?? Guid.NewGuid(),
        Scope().CompanyId,
        Scope().BranchId,
        Scope().UserId,
        Scope().RegisteredDeviceId,
        "CreateOperationalParty",
        "CREATE",
        "OperationalParty",
        null,
        null,
        new DateTimeOffset(2026, 8, 26, 7, 0, 0, TimeSpan.Zero));

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
