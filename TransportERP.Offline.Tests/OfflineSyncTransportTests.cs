using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using TransportERP.Application.Sync;
using TransportERP.Offline.Transport;

namespace TransportERP.Offline.Tests;

public sealed class OfflineSyncTransportTests : IDisposable
{
    private static readonly Uri Endpoint = new("https://sync.example.test/api/v1/sync/operations:batch");
    private static readonly Guid CompanyId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid BranchId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    private static readonly Guid UserId = Guid.Parse("33333333-3333-3333-3333-333333333333");
    private static readonly Guid RegisteredDeviceId = Guid.Parse("44444444-4444-4444-4444-444444444444");
    private static readonly BuildIdentityV1 TestBuildIdentity = new(
        BuildIdentityV1.DesktopWindowsPlatform, new string('a', 64));
    private readonly string _directory = Path.Combine(Path.GetTempPath(), "transporterp-transport-tests", Guid.NewGuid().ToString("N"));
    private readonly byte[] _outboxKey = RandomNumberGenerator.GetBytes(32);
    private readonly byte[] _cacheKey = RandomNumberGenerator.GetBytes(32);

    [Fact]
    public async Task Effective_payload_ceiling_rejects_locally_before_any_HTTP_call()
    {
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 27, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        var queued = await EnqueueRequestAsync(store, Request());
        using var key = new TestSigningKey();
        var calls = 0;
        using var http = new HttpClient(new DelegateHandler((_, _) =>
        {
            calls++;
            throw new InvalidOperationException("The narrowed client policy must reject before transport.");
        }));
        var options = new OfflineSyncTransportOptions(
            Endpoint, "desktop-device-1", RegisteredDeviceId, CompanyId, BranchId, UserId, "policy-worker",
            MaximumRequestBodyBytes: 1024,
            MaximumPayloadBytes: 8,
            BuildIdentity: TestBuildIdentity);
        var client = new OfflineSyncTransportClient(
            http, store, new FixedBearerProvider("token"), key, options, clock);

        var result = await client.ProcessNextBatchAsync();
        var persisted = await store.GetAsync(queued.Operation.LocalOperationId, Scope());

        Assert.Equal(0, calls);
        Assert.Equal(1, result.Rejected);
        Assert.Equal(OfflineOperationStatus.Rejected, persisted!.Status);
        Assert.Equal("PAYLOAD_TOO_LARGE", persisted.ResultCode);
    }

    [Fact]
    public async Task Nonce_challenge_is_followed_by_a_fresh_cryptographically_valid_proof_over_exact_body()
    {
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 26, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        var queued = await EnqueueRequestAsync(store, Request());
        var nonce = Base64Url(RandomNumberGenerator.GetBytes(32));
        const string bearer = "volatile-session-token";
        byte[]? challengeBody = null;
        Guid? challengeCorrelation = null;
        CapturedRequest? signed = null;
        using var key = new TestSigningKey();
        var calls = 0;
        using var http = new HttpClient(new DelegateHandler(async (request, cancellationToken) =>
        {
            calls++;
            var captured = await CaptureAsync(request, cancellationToken);
            if (calls == 1)
            {
                challengeBody = captured.Body;
                challengeCorrelation = captured.AttemptCorrelationId;
                Assert.Null(captured.Proof);
                return Challenge(nonce, captured.AttemptCorrelationId);
            }
            signed = captured;
            return Success(captured, "SUCCEEDED", resultEntityId: Guid.NewGuid(), resultVersion: 9);
        }));

        var client = Client(http, store, key, clock, bearer);
        var result = await client.ProcessNextBatchAsync();

        Assert.Equal(2, calls);
        Assert.Equal(1, result.Succeeded);
        Assert.Equal(challengeBody, signed!.Body);
        Assert.Equal(bearer, signed.Bearer);
        Assert.Equal("application/json", signed.MediaType);
        Assert.NotNull(signed.Proof);
        Assert.NotEqual(challengeCorrelation, signed.AttemptCorrelationId);
        AssertExactJsonOnlyWireContract(signed.Body);
        Assert.Equal(TestBuildIdentity, ReadBatch(signed.Body).BuildIdentity);
        VerifyProof(signed.Proof!, signed.Body, bearer, nonce, signed.AttemptCorrelationId, key);
        var persisted = await store.GetAsync(queued.Operation.LocalOperationId, Scope());
        Assert.Equal(OfflineOperationStatus.Succeeded, persisted!.Status);
        Assert.Equal(9, persisted.ResultVersion);
    }

    [Fact]
    public async Task Signed_nonce_refresh_creates_a_new_jti_and_signature_for_the_same_http_attempt_body()
    {
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 26, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        await EnqueueRequestAsync(store, Request());
        using var key = new TestSigningKey();
        var firstNonce = Base64Url(RandomNumberGenerator.GetBytes(32));
        var refreshedNonce = Base64Url(RandomNumberGenerator.GetBytes(32));
        var signed = new List<CapturedRequest>();
        var call = 0;
        using var http = new HttpClient(new DelegateHandler(async (request, cancellationToken) =>
        {
            call++;
            var captured = await CaptureAsync(request, cancellationToken);
            if (call == 1) return Challenge(firstNonce, captured.AttemptCorrelationId);
            signed.Add(captured);
            if (call == 2) return Challenge(refreshedNonce, captured.AttemptCorrelationId);
            return Success(captured, "SUCCEEDED");
        }));

        var outcome = await Client(http, store, key, clock, "token").ProcessNextBatchAsync();

        Assert.Equal(1, outcome.Succeeded);
        Assert.Equal(3, call);
        Assert.Equal(signed[0].Body, signed[1].Body);
        Assert.NotEqual(signed[0].AttemptCorrelationId, signed[1].AttemptCorrelationId);
        Assert.NotEqual(signed[0].Proof, signed[1].Proof);
        var firstClaims = ProofClaims(signed[0].Proof!);
        var refreshedClaims = ProofClaims(signed[1].Proof!);
        Assert.Equal(firstNonce, firstClaims.GetProperty("nonce").GetString());
        Assert.Equal(refreshedNonce, refreshedClaims.GetProperty("nonce").GetString());
        Assert.NotEqual(firstClaims.GetProperty("jti").GetString(), refreshedClaims.GetProperty("jti").GetString());
    }

    [Fact]
    public async Task Timeout_after_send_replays_stable_business_identity_with_new_attempt_and_proof()
    {
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 26, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        var queued = await EnqueueRequestAsync(store, Request());
        using var key = new TestSigningKey();
        var signedRequests = new List<CapturedRequest>();
        var call = 0;
        using var http = new HttpClient(new DelegateHandler(async (request, cancellationToken) =>
        {
            call++;
            var captured = await CaptureAsync(request, cancellationToken);
            if (call is 1 or 3 or 5) return Challenge(Base64Url(RandomNumberGenerator.GetBytes(32)), captured.AttemptCorrelationId);
            signedRequests.Add(captured);
            if (call == 2) throw new TaskCanceledException("simulated timeout after server acceptance");
            return Success(captured, call == 4 ? "QUEUED" : "SUCCEEDED",
                resultEntityId: call == 6 ? Guid.NewGuid() : null,
                resultVersion: call == 6 ? 1 : null);
        }));
        var client = Client(http, store, key, clock, "memory-only-token");

        var firstRun = await client.ProcessNextBatchAsync();
        var failed = await store.GetAsync(queued.Operation.LocalOperationId, Scope());
        Assert.Equal(1, firstRun.RetryScheduled);
        Assert.Equal(OfflineOperationStatus.Failed, failed!.Status);
        var firstAttempt = failed.AttemptCorrelationId;

        clock.Advance(TimeSpan.FromSeconds(5));
        var secondRun = await client.ProcessNextBatchAsync();
        var accepted = await store.GetAsync(queued.Operation.LocalOperationId, Scope());

        Assert.Equal(0, secondRun.Succeeded);
        Assert.Equal(1, secondRun.AcceptedPending);
        Assert.Equal(OfflineOperationStatus.Queued, accepted!.Status);
        Assert.NotNull(accepted.ServerOperationId);
        Assert.Equal(1, accepted.ClientTransportRetryCount);

        clock.Advance(TimeSpan.FromSeconds(5));
        var thirdRun = await client.ProcessNextBatchAsync();
        var completed = await store.GetAsync(queued.Operation.LocalOperationId, Scope());

        Assert.Equal(1, thirdRun.Succeeded);
        Assert.Equal(OfflineOperationStatus.Succeeded, completed!.Status);
        Assert.NotEqual(firstAttempt, completed.AttemptCorrelationId);
        Assert.Equal(queued.Operation.ClientOperationId, completed.ClientOperationId);
        Assert.Equal(queued.Operation.OperationCorrelationId, completed.OperationCorrelationId);
        Assert.Equal(signedRequests[0].Body, signedRequests[1].Body);
        Assert.NotEqual(signedRequests[0].Proof, signedRequests[1].Proof);
        var firstClaims = ProofClaims(signedRequests[0].Proof!);
        var secondClaims = ProofClaims(signedRequests[1].Proof!);
        Assert.NotEqual(firstClaims.GetProperty("jti").GetString(), secondClaims.GetProperty("jti").GetString());
        Assert.NotEqual(firstClaims.GetProperty("cid").GetString(), secondClaims.GetProperty("cid").GetString());
    }

    [Theory]
    [InlineData("SUCCEEDED", OfflineOperationStatus.Succeeded, null)]
    [InlineData("CONFLICT", OfflineOperationStatus.Conflict, "BASE_VERSION_CONFLICT")]
    [InlineData("FAILED", OfflineOperationStatus.Rejected, "ACTION_EXECUTION_FAILED")]
    public async Task Server_acceptance_is_polled_until_a_real_terminal_outcome_without_spending_transport_retry(
        string terminalStatus,
        OfflineOperationStatus expectedLocalStatus,
        string? terminalError)
    {
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 26, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        var queued = await EnqueueRequestAsync(store, Request());
        using var key = new TestSigningKey();
        var signed = new List<CapturedRequest>();
        var call = 0;
        using var http = new HttpClient(new DelegateHandler(async (request, cancellationToken) =>
        {
            call++;
            var captured = await CaptureAsync(request, cancellationToken);
            if (call is 1 or 3)
                return Challenge(Base64Url(RandomNumberGenerator.GetBytes(32)), captured.AttemptCorrelationId);
            signed.Add(captured);
            return Success(captured, call == 2 ? "QUEUED" : terminalStatus,
                call == 4 ? terminalError : null,
                call == 4 && terminalStatus == "SUCCEEDED" ? Guid.NewGuid() : null,
                call == 4 && terminalStatus == "SUCCEEDED" ? 1 : null);
        }));
        var client = Client(http, store, key, clock, "token");

        var acceptedRun = await client.ProcessNextBatchAsync();
        var accepted = (await store.GetAsync(queued.Operation.LocalOperationId, Scope()))!;

        Assert.Equal(1, acceptedRun.AcceptedPending);
        Assert.Equal(OfflineOperationStatus.Queued, accepted.Status);
        Assert.Equal("QUEUED", accepted.ResultCode);
        Assert.NotNull(accepted.ServerOperationId);
        Assert.Null(accepted.AcknowledgedAt);
        Assert.Equal(0, accepted.ClientTransportRetryCount);
        var acceptedAttempt = accepted.AttemptCorrelationId;

        clock.Advance(TimeSpan.FromSeconds(5));
        await client.ProcessNextBatchAsync();
        var terminal = (await store.GetAsync(queued.Operation.LocalOperationId, Scope()))!;

        Assert.Equal(expectedLocalStatus, terminal.Status);
        Assert.Equal(0, terminal.ClientTransportRetryCount);
        Assert.NotEqual(acceptedAttempt, terminal.AttemptCorrelationId);
        Assert.Equal(accepted.ServerOperationId, terminal.ServerOperationId);
        Assert.Equal(signed[0].Body, signed[1].Body);
        Assert.NotEqual(signed[0].Proof, signed[1].Proof);
    }

    [Fact]
    public async Task Supervisor_reopens_the_encrypted_queue_waits_for_connectivity_and_drains_accepted_work_once()
    {
        Directory.CreateDirectory(_directory);
        var path = Path.Combine(_directory, "supervised-outbox.db");
        var firstStore = new OfflineOperationStore(path, new FixedKeyProvider(_outboxKey, _cacheKey));
        await firstStore.InitializeAsync();
        var queued = await EnqueueRequestAsync(firstStore, Request());

        var reopened = new OfflineOperationStore(path, new FixedKeyProvider(_outboxKey, _cacheKey));
        await reopened.InitializeAsync();
        using var key = new TestSigningKey();
        var signedCalls = 0;
        using var http = new HttpClient(new DelegateHandler(async (request, cancellationToken) =>
        {
            var captured = await CaptureAsync(request, cancellationToken);
            if (captured.Proof is null)
                return Challenge(Base64Url(RandomNumberGenerator.GetBytes(32)), captured.AttemptCorrelationId);
            signedCalls++;
            return Success(captured, signedCalls == 1 ? "QUEUED" : "SUCCEEDED",
                resultEntityId: signedCalls == 2 ? Guid.NewGuid() : null,
                resultVersion: signedCalls == 2 ? 1 : null);
        }));
        var transport = new OfflineSyncTransportClient(http, reopened, new FixedBearerProvider("token"), key,
            new OfflineSyncTransportOptions(Endpoint, "desktop-device-1", RegisteredDeviceId,
                CompanyId, BranchId, UserId, "supervised-worker",
                AcceptedPollInterval: TimeSpan.FromMilliseconds(20), BuildIdentity: TestBuildIdentity));
        var connectivity = new ManualConnectivity(initiallyOnline: false);
        var supervisor = new OfflineSyncSupervisor(reopened, transport, connectivity,
            new OfflineSyncSupervisorOptions(10, TimeSpan.FromMilliseconds(20)));
        using var stop = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        var run = supervisor.RunAsync(stop.Token);

        await Task.Delay(30);
        Assert.Equal(0, signedCalls);
        connectivity.SetOnline();
        await WaitUntilAsync(async () =>
            (await reopened.GetAsync(queued.Operation.LocalOperationId, Scope()))?.Status == OfflineOperationStatus.Succeeded,
            stop.Token);
        stop.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => run);

        var completed = (await reopened.GetAsync(queued.Operation.LocalOperationId, Scope()))!;
        Assert.Equal(2, signedCalls);
        Assert.Equal(0, completed.ClientTransportRetryCount);
        Assert.NotNull(completed.ServerOperationId);
    }

    [Fact]
    public async Task A_second_supervisor_cannot_drain_the_same_local_outbox()
    {
        var store = await CreateStoreAsync(TimeProvider.System);
        await EnqueueRequestAsync(store, Request());
        using var key = new TestSigningKey();
        var signedEntered = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var releaseSigned = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        using var http = new HttpClient(new DelegateHandler(async (request, cancellationToken) =>
        {
            var captured = await CaptureAsync(request, cancellationToken);
            if (captured.Proof is null)
                return Challenge(Base64Url(RandomNumberGenerator.GetBytes(32)), captured.AttemptCorrelationId);
            signedEntered.TrySetResult();
            await releaseSigned.Task.WaitAsync(cancellationToken);
            return Success(captured, "SUCCEEDED", resultEntityId: Guid.NewGuid(), resultVersion: 1);
        }));
        var transport = Client(http, store, key, TimeProvider.System, "token");
        var first = new OfflineSyncSupervisor(store, transport, new AlwaysOnlineSyncConnectivity());
        var second = new OfflineSyncSupervisor(store, transport, new AlwaysOnlineSyncConnectivity());
        using var stop = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        var firstRun = first.RunAsync(stop.Token);
        await signedEntered.Task.WaitAsync(stop.Token);

        var error = await Assert.ThrowsAsync<OfflineStoreException>(() => second.RunAsync(stop.Token));
        Assert.Equal("LOCAL_SYNC_SUPERVISOR_ALREADY_RUNNING", error.Code);

        releaseSigned.TrySetResult();
        stop.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => firstRun);
    }

    [Fact]
    public async Task Manual_sync_request_joins_the_single_supervisor_owner_without_duplicate_send_or_stale_lease()
    {
        var store = await CreateStoreAsync(TimeProvider.System);
        var queued = await EnqueueRequestAsync(store, Request());
        using var key = new TestSigningKey();
        var signedCalls = 0;
        var signedEntered = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var releaseSigned = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        using var http = new HttpClient(new DelegateHandler(async (request, cancellationToken) =>
        {
            var captured = await CaptureAsync(request, cancellationToken);
            if (captured.Proof is null)
                return Challenge(Base64Url(RandomNumberGenerator.GetBytes(32)), captured.AttemptCorrelationId);
            Interlocked.Increment(ref signedCalls);
            signedEntered.TrySetResult();
            await releaseSigned.Task.WaitAsync(cancellationToken);
            return Success(captured, "SUCCEEDED", resultEntityId: Guid.NewGuid(), resultVersion: 1);
        }));
        var transport = Client(http, store, key, TimeProvider.System, "single-owner-worker");
        var supervisor = new OfflineSyncSupervisor(
            store,
            transport,
            new AlwaysOnlineSyncConnectivity(),
            new OfflineSyncSupervisorOptions(
                MaximumBatchOperations: 1,
                IdleWakeInterval: TimeSpan.FromMilliseconds(20)));
        using var stop = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        var run = supervisor.RunAsync(stop.Token);
        await signedEntered.Task.WaitAsync(stop.Token);

        var manual = supervisor.SynchronizeNowAsync(1, stop.Token);
        Assert.False(manual.IsCompleted);
        releaseSigned.TrySetResult();
        var manualResult = await manual;

        await WaitUntilAsync(async () =>
            (await store.GetAsync(queued.Operation.LocalOperationId, Scope()))?.Status ==
                OfflineOperationStatus.Succeeded,
            stop.Token);
        var terminal = await store.GetAsync(queued.Operation.LocalOperationId, Scope());
        Assert.Equal(1, signedCalls);
        Assert.Equal(0, manualResult.Claimed);
        Assert.Equal(OfflineOperationStatus.Succeeded, terminal!.Status);
        Assert.Null(terminal.LeaseOwner);
        Assert.Null(terminal.LeaseExpiresAt);

        stop.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => run);
    }

    [Fact]
    public async Task Manual_sync_fails_fast_before_start_and_after_shutdown()
    {
        var store = await CreateStoreAsync(TimeProvider.System);
        using var key = new TestSigningKey();
        using var http = new HttpClient(new DelegateHandler((_, _) =>
            throw new InvalidOperationException("An empty lifecycle test must not call HTTP.")));
        var supervisor = new OfflineSyncSupervisor(
            store,
            Client(http, store, key, TimeProvider.System, "token"),
            new ManualConnectivity(initiallyOnline: false));

        var beforeStart = await Assert.ThrowsAsync<OfflineStoreException>(() => supervisor.SynchronizeNowAsync());
        Assert.Equal("LOCAL_SYNC_SUPERVISOR_NOT_RUNNING", beforeStart.Code);

        using var stop = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        var run = supervisor.RunAsync(stop.Token);
        await Task.Delay(20, stop.Token);
        var pending = supervisor.SynchronizeNowAsync();
        stop.Cancel();

        var whileStopping = await Assert.ThrowsAsync<OfflineStoreException>(() => pending);
        Assert.Equal("LOCAL_SYNC_SUPERVISOR_STOPPING", whileStopping.Code);
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => run);
        var afterStop = await Assert.ThrowsAsync<OfflineStoreException>(() => supervisor.SynchronizeNowAsync());
        Assert.Equal("LOCAL_SYNC_SUPERVISOR_STOPPING", afterStop.Code);
    }

    [Fact]
    public async Task Canceled_coalesced_manual_waiters_leave_no_pending_cycle_or_transport_work()
    {
        var store = await CreateStoreAsync(TimeProvider.System);
        using var key = new TestSigningKey();
        var httpCalls = 0;
        using var http = new HttpClient(new DelegateHandler((_, _) =>
        {
            Interlocked.Increment(ref httpCalls);
            throw new InvalidOperationException("An empty canceled request must not call HTTP.");
        }));
        var connectivity = new ManualConnectivity(initiallyOnline: false);
        var supervisor = new OfflineSyncSupervisor(
            store,
            Client(http, store, key, TimeProvider.System, "token"),
            connectivity,
            new OfflineSyncSupervisorOptions(IdleWakeInterval: TimeSpan.FromMilliseconds(20)));
        using var stop = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        var run = supervisor.RunAsync(stop.Token);
        await Task.Delay(20, stop.Token);

        var canceledWaiters = Enumerable.Range(0, 50).Select(async _ =>
        {
            using var cancel = new CancellationTokenSource();
            var request = supervisor.SynchronizeNowAsync(cancellationToken: cancel.Token);
            cancel.Cancel();
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => request);
        });
        await Task.WhenAll(canceledWaiters);

        connectivity.SetOnline();
        var fresh = await supervisor.SynchronizeNowAsync(cancellationToken: stop.Token);
        Assert.Equal(0, fresh.Claimed);
        Assert.Equal(0, httpCalls);

        stop.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => run);
    }

    [Fact]
    public async Task Supervisor_runs_local_retention_even_without_transport_work()
    {
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 26, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        var queued = await EnqueueRequestAsync(store, Request());
        var claimed = await store.ClaimNextAsync("worker", TimeSpan.FromMinutes(1), Scope());
        await store.MarkSucceededAsync(claimed!.LocalOperationId, claimed.AttemptCorrelationId!.Value,
            Guid.NewGuid(), 1);
        clock.Advance(TimeSpan.FromHours(24));
        using var key = new TestSigningKey();
        using var http = new HttpClient(new DelegateHandler((_, _) =>
            throw new InvalidOperationException("Retention-only supervision must not call HTTP.")));
        var transport = Client(http, store, key, clock, "token");
        var supervisor = new OfflineSyncSupervisor(store, transport, new AlwaysOnlineSyncConnectivity(),
            new OfflineSyncSupervisorOptions(RetentionPolicy: new OfflineRetentionPolicy()), clock);
        using var stop = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        var run = supervisor.RunAsync(stop.Token);

        await WaitUntilAsync(async () =>
            (await store.GetAsync(queued.Operation.LocalOperationId, Scope()))?.PayloadJson is null,
            stop.Token);
        stop.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => run);
    }

    [Fact]
    public async Task Supervisor_recovers_after_unexpected_iteration_failure_and_drains_the_operation()
    {
        var store = await CreateStoreAsync(TimeProvider.System);
        var queued = await EnqueueRequestAsync(store, Request());
        using var key = new TestSigningKey();
        var calls = 0;
        using var http = new HttpClient(new DelegateHandler(async (request, cancellationToken) =>
        {
            calls++;
            var captured = await CaptureAsync(request, cancellationToken);
            return captured.Proof is null
                ? Challenge(Base64Url(RandomNumberGenerator.GetBytes(32)), captured.AttemptCorrelationId)
                : Success(captured, "SUCCEEDED", resultEntityId: Guid.NewGuid(), resultVersion: 1);
        }));
        var transport = new OfflineSyncTransportClient(
            http,
            store,
            new FixedBearerProvider("token"),
            key,
            new OfflineSyncTransportOptions(
                Endpoint,
                "desktop-device-1",
                RegisteredDeviceId,
                CompanyId,
                BranchId,
                UserId,
                "resilient-worker",
                BuildIdentity: TestBuildIdentity),
            TimeProvider.System);
        var supervisor = new OfflineSyncSupervisor(
            store,
            transport,
            new OneShotFailingConnectivity(),
            new OfflineSyncSupervisorOptions(
                MaximumBatchOperations: 1,
                IdleWakeInterval: TimeSpan.FromMilliseconds(20),
                FailureRecoveryBaseDelay: TimeSpan.FromMilliseconds(30),
                FailureRecoveryMaxDelay: TimeSpan.FromMilliseconds(30)));
        using var stop = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        var run = supervisor.RunAsync(stop.Token);

        await WaitUntilAsync(async () =>
            (await store.GetAsync(queued.Operation.LocalOperationId, Scope()))?.Status ==
                OfflineOperationStatus.Succeeded,
            stop.Token);
        stop.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => run);

        var completed = (await store.GetAsync(queued.Operation.LocalOperationId, Scope()))!;
        Assert.Equal(2, calls);
        Assert.Equal(0, completed.ClientTransportRetryCount);
        Assert.Equal("SUPERVISOR_ITERATION_FAILED", supervisor.LastObservedFailure?.Code);
        Assert.Equal(1, supervisor.LastObservedFailure?.ConsecutiveFailureCount);
    }

    [Fact]
    public async Task Batch_claim_never_reclaims_the_same_operation_after_its_lease_expires()
    {
        var clock = new AdvancingTimeProvider(
            new DateTimeOffset(2026, 8, 26, 10, 0, 0, TimeSpan.Zero),
            TimeSpan.FromSeconds(2));
        var store = await CreateStoreAsync(clock);
        var queued = await EnqueueRequestAsync(store, Request());
        using var key = new TestSigningKey();
        var calls = 0;
        using var http = new HttpClient(new DelegateHandler(async (request, cancellationToken) =>
        {
            calls++;
            var captured = await CaptureAsync(request, cancellationToken);
            return captured.Proof is null
                ? Challenge(Base64Url(RandomNumberGenerator.GetBytes(32)), captured.AttemptCorrelationId)
                : Success(captured, "SUCCEEDED", resultEntityId: Guid.NewGuid(), resultVersion: 1);
        }));
        var transport = new OfflineSyncTransportClient(
            http,
            store,
            new FixedBearerProvider("token"),
            key,
            new OfflineSyncTransportOptions(
                Endpoint,
                "desktop-device-1",
                RegisteredDeviceId,
                CompanyId,
                BranchId,
                UserId,
                "short-lease-worker",
                LeaseDuration: TimeSpan.FromSeconds(1),
                BuildIdentity: TestBuildIdentity),
            clock);

        var result = await transport.ProcessNextBatchAsync();
        var completed = await store.GetAsync(queued.Operation.LocalOperationId, Scope());

        Assert.Equal(1, result.Claimed);
        Assert.Equal(1, result.Succeeded);
        Assert.Equal(2, calls);
        Assert.Equal(OfflineOperationStatus.Succeeded, completed?.Status);
    }

    [Fact]
    public async Task No_http_response_is_retryable_and_does_not_become_a_business_rejection()
    {
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 26, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        var queued = await EnqueueRequestAsync(store, Request());
        using var key = new TestSigningKey();
        var call = 0;
        using var http = new HttpClient(new DelegateHandler(async (request, cancellationToken) =>
        {
            call++;
            var captured = await CaptureAsync(request, cancellationToken);
            if (call == 1) return Challenge(Base64Url(RandomNumberGenerator.GetBytes(32)), captured.AttemptCorrelationId);
            throw new HttpRequestException("simulated connection loss");
        }));

        var outcome = await Client(http, store, key, clock, "token").ProcessNextBatchAsync();
        var persisted = await store.GetAsync(queued.Operation.LocalOperationId, Scope());

        Assert.Equal(1, outcome.RetryScheduled);
        Assert.Equal(OfflineOperationStatus.Failed, persisted!.Status);
        Assert.Equal("NO_RESPONSE", persisted.ResultCode);
        Assert.Equal(1, persisted.ClientTransportRetryCount);
    }

    [Fact]
    public async Task Unexpected_bearer_provider_failure_is_retryable_and_exposes_only_a_fixed_stage_code()
    {
        const string secret = "fake-bearer|fake-pfx-password|D:\\private\\bearer";
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 27, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        var queued = await EnqueueRequestAsync(store, Request());
        using var key = new TestSigningKey();
        var calls = 0;
        using var http = new HttpClient(new DelegateHandler((_, _) =>
        {
            calls++;
            throw new InvalidOperationException("HTTP must not run after bearer access fails.");
        }));
        var client = new OfflineSyncTransportClient(
            http, store, new ThrowingBearerProvider(new InvalidOperationException(secret)), key,
            new OfflineSyncTransportOptions(Endpoint, "desktop-device-1", RegisteredDeviceId,
                CompanyId, BranchId, UserId, "test-worker", BuildIdentity: TestBuildIdentity), clock);

        var outcome = await client.ProcessNextBatchAsync();
        var persisted = await store.GetAsync(queued.Operation.LocalOperationId, Scope());

        Assert.Equal(0, calls);
        Assert.Equal(1, outcome.RetryScheduled);
        Assert.Equal(OfflineOperationStatus.Failed, persisted!.Status);
        Assert.Equal("CLIENT_BEARER_FAILURE", persisted.ResultCode);
        Assert.DoesNotContain(secret, JsonSerializer.Serialize(persisted), StringComparison.Ordinal);
    }

    [Fact]
    public async Task Unexpected_nonce_http_adapter_failure_is_retryable_and_exposes_only_a_fixed_stage_code()
    {
        const string secret = "fake-bearer|fake-pfx-password|D:\\private\\nonce-send";
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 27, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        var queued = await EnqueueRequestAsync(store, Request());
        using var key = new TestSigningKey();
        var calls = 0;
        using var http = new HttpClient(new DelegateHandler((_, _) =>
        {
            calls++;
            throw new InvalidOperationException(secret);
        }));

        var outcome = await Client(http, store, key, clock, "token").ProcessNextBatchAsync();
        var persisted = await store.GetAsync(queued.Operation.LocalOperationId, Scope());

        Assert.Equal(1, calls);
        Assert.Equal(1, outcome.RetryScheduled);
        Assert.Equal(OfflineOperationStatus.Failed, persisted!.Status);
        Assert.Equal("CLIENT_NONCE_FAILURE", persisted.ResultCode);
        Assert.DoesNotContain(secret, JsonSerializer.Serialize(persisted), StringComparison.Ordinal);
    }

    [Fact]
    public async Task Invalid_success_response_json_is_retryable_with_a_fixed_stage_code()
    {
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 27, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        var queued = await EnqueueRequestAsync(store, Request());
        using var key = new TestSigningKey();
        var calls = 0;
        using var http = new HttpClient(new DelegateHandler(async (request, cancellationToken) =>
        {
            var captured = await CaptureAsync(request, cancellationToken);
            if (++calls == 1)
                return Challenge(Base64Url(RandomNumberGenerator.GetBytes(32)),
                    captured.AttemptCorrelationId);
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("not-json", Encoding.UTF8, "application/json")
            };
        }));

        var outcome = await Client(http, store, key, clock, "token").ProcessNextBatchAsync();
        var persisted = await store.GetAsync(queued.Operation.LocalOperationId, Scope());

        Assert.Equal(2, calls);
        Assert.Equal(1, outcome.RetryScheduled);
        Assert.Equal(OfflineOperationStatus.Failed, persisted!.Status);
        Assert.Equal("CLIENT_SIGNED_JSON_INVALID", persisted.ResultCode);
    }

    [Fact]
    public async Task Error_response_correlation_mismatch_is_retryable_with_a_fixed_stage_code()
    {
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 27, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        var queued = await EnqueueRequestAsync(store, Request());
        using var key = new TestSigningKey();
        var calls = 0;
        using var http = new HttpClient(new DelegateHandler(async (request, cancellationToken) =>
        {
            var captured = await CaptureAsync(request, cancellationToken);
            if (++calls == 1)
                return Challenge(Base64Url(RandomNumberGenerator.GetBytes(32)),
                    captured.AttemptCorrelationId);
            return Json(HttpStatusCode.Forbidden, new
            {
                ErrorCode = "SCOPE_DENIED",
                CorrelationId = Guid.NewGuid()
            });
        }));

        var outcome = await Client(http, store, key, clock, "token").ProcessNextBatchAsync();
        var persisted = await store.GetAsync(queued.Operation.LocalOperationId, Scope());

        Assert.Equal(2, calls);
        Assert.Equal(1, outcome.RetryScheduled);
        Assert.Equal(OfflineOperationStatus.Failed, persisted!.Status);
        Assert.Equal("CLIENT_HTTP_CORRELATION_MISMATCH", persisted.ResultCode);
    }

    [Fact]
    public async Task Unexpected_platform_signing_failure_releases_the_claim_and_schedules_retry()
    {
        const string secret = "fake-bearer|fake-pfx-password|D:\\private\\device-key";
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 27, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        var queued = await EnqueueRequestAsync(store, Request());
        using var key = new TestSigningKey();
        var failingKey = new FailingSigningKey(key, new InvalidOperationException(secret));
        var calls = 0;
        using var http = new HttpClient(new DelegateHandler(async (request, cancellationToken) =>
        {
            calls++;
            var captured = await CaptureAsync(request, cancellationToken);
            return Challenge(Base64Url(RandomNumberGenerator.GetBytes(32)), captured.AttemptCorrelationId);
        }));

        var outcome = await Client(http, store, failingKey, clock, "token").ProcessNextBatchAsync();
        var persisted = await store.GetAsync(queued.Operation.LocalOperationId, Scope());

        Assert.Equal(1, calls);
        Assert.Equal(1, outcome.Claimed);
        Assert.Equal(1, outcome.RetryScheduled);
        Assert.Equal(OfflineOperationStatus.Failed, persisted!.Status);
        Assert.Equal("CLIENT_PROOF_CREATE_FAILURE", persisted.ResultCode);
        Assert.Equal(1, persisted.ClientTransportRetryCount);
        Assert.NotNull(persisted.NextRetryAt);
        Assert.Null(persisted.LeaseOwner);
        Assert.Null(persisted.LeaseExpiresAt);
        Assert.DoesNotContain(secret, JsonSerializer.Serialize(persisted), StringComparison.Ordinal);
    }

    [Fact]
    public async Task Cryptographic_signing_failure_keeps_the_existing_nonretryable_mapping()
    {
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 27, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        var queued = await EnqueueRequestAsync(store, Request());
        using var key = new TestSigningKey();
        var failingKey = new FailingSigningKey(
            key, new CryptographicException("synthetic cryptographic provider detail"));
        var calls = 0;
        using var http = new HttpClient(new DelegateHandler(async (request, cancellationToken) =>
        {
            calls++;
            var captured = await CaptureAsync(request, cancellationToken);
            return Challenge(Base64Url(RandomNumberGenerator.GetBytes(32)),
                captured.AttemptCorrelationId);
        }));

        var outcome = await Client(http, store, failingKey, clock, "token").ProcessNextBatchAsync();
        var persisted = await store.GetAsync(queued.Operation.LocalOperationId, Scope());

        Assert.Equal(1, calls);
        Assert.Equal(1, outcome.Claimed);
        Assert.Equal(0, outcome.RetryScheduled);
        Assert.Equal(1, outcome.Rejected);
        Assert.Equal(OfflineOperationStatus.Rejected, persisted!.Status);
        Assert.Equal("DEVICE_PROOF_KEY_INVALID", persisted.ResultCode);
        Assert.Null(persisted.LeaseOwner);
        Assert.Null(persisted.LeaseExpiresAt);
    }

    [Fact]
    public async Task Governed_signing_failure_keeps_its_sync_transport_code_and_retry_decision()
    {
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 27, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        var queued = await EnqueueRequestAsync(store, Request());
        using var key = new TestSigningKey();
        var failingKey = new FailingSigningKey(
            key, new SyncTransportException("DEVICE_KEY_REBIND_REQUIRED", retryable: false));
        var calls = 0;
        using var http = new HttpClient(new DelegateHandler(async (request, cancellationToken) =>
        {
            calls++;
            var captured = await CaptureAsync(request, cancellationToken);
            return Challenge(Base64Url(RandomNumberGenerator.GetBytes(32)),
                captured.AttemptCorrelationId);
        }));

        var outcome = await Client(http, store, failingKey, clock, "token").ProcessNextBatchAsync();
        var persisted = await store.GetAsync(queued.Operation.LocalOperationId, Scope());

        Assert.Equal(1, calls);
        Assert.Equal(1, outcome.Claimed);
        Assert.Equal(0, outcome.RetryScheduled);
        Assert.Equal(1, outcome.Rejected);
        Assert.Equal(OfflineOperationStatus.Rejected, persisted!.Status);
        Assert.Equal("DEVICE_KEY_REBIND_REQUIRED", persisted.ResultCode);
        Assert.Null(persisted.LeaseOwner);
        Assert.Null(persisted.LeaseExpiresAt);
    }

    [Fact]
    public async Task Unexpected_signed_http_adapter_failure_releases_the_claim_without_leaking_details()
    {
        const string secret = "fake-bearer|fake-pfx-password|D:\\private\\signed-send";
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 27, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        var queued = await EnqueueRequestAsync(store, Request());
        using var key = new TestSigningKey();
        var calls = 0;
        using var http = new HttpClient(new DelegateHandler(async (request, cancellationToken) =>
        {
            calls++;
            var captured = await CaptureAsync(request, cancellationToken);
            if (calls == 1)
                return Challenge(Base64Url(RandomNumberGenerator.GetBytes(32)),
                    captured.AttemptCorrelationId);
            throw new InvalidOperationException(secret);
        }));

        var outcome = await Client(http, store, key, clock, "token").ProcessNextBatchAsync();
        var persisted = await store.GetAsync(queued.Operation.LocalOperationId, Scope());

        Assert.Equal(2, calls);
        Assert.Equal(1, outcome.Claimed);
        Assert.Equal(1, outcome.RetryScheduled);
        Assert.Equal(OfflineOperationStatus.Failed, persisted!.Status);
        Assert.Equal("CLIENT_SIGNED_SEND_FAILURE", persisted.ResultCode);
        Assert.Equal(1, persisted.ClientTransportRetryCount);
        Assert.NotNull(persisted.NextRetryAt);
        Assert.Null(persisted.LeaseOwner);
        Assert.Null(persisted.LeaseExpiresAt);
        Assert.DoesNotContain(secret, JsonSerializer.Serialize(persisted), StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(HttpStatusCode.RequestTimeout)]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.ServiceUnavailable)]
    public async Task Http_408_and_5xx_are_retryable_even_when_the_error_body_is_missing_or_not_retryable(
        HttpStatusCode statusCode)
    {
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 26, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        var queued = await EnqueueRequestAsync(store, Request());
        using var key = new TestSigningKey();
        using var http = new HttpClient(new DelegateHandler((request, _) => Task.FromResult(
            statusCode == HttpStatusCode.InternalServerError
                ? Json(statusCode, new { ErrorCode = "SCOPE_DENIED", CorrelationId = Guid.Parse(
                    request.Headers.GetValues("X-Correlation-Id").Single()) })
                : new HttpResponseMessage(statusCode))));

        var outcome = await Client(http, store, key, clock, "token").ProcessNextBatchAsync();
        var operation = (await store.GetAsync(queued.Operation.LocalOperationId, Scope()))!;

        Assert.Equal(1, outcome.RetryScheduled);
        Assert.Equal(0, outcome.Rejected);
        Assert.Equal(OfflineOperationStatus.Failed, operation.Status);
        Assert.Equal(1, operation.ClientTransportRetryCount);
    }

    [Fact]
    public async Task Exhausted_transport_failure_is_reported_as_rejected_not_retry_scheduled()
    {
        Directory.CreateDirectory(_directory);
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 26, 10, 0, 0, TimeSpan.Zero));
        var store = new OfflineOperationStore(Path.Combine(_directory, "exhausted.db"),
            new FixedKeyProvider(_outboxKey, _cacheKey), clock,
            new OfflineRetryPolicy(0, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1)));
        await store.InitializeAsync();
        var queued = await EnqueueRequestAsync(store, Request());
        using var key = new TestSigningKey();
        using var http = new HttpClient(new DelegateHandler((_, _) =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable))));

        var outcome = await Client(http, store, key, clock, "token").ProcessNextBatchAsync();
        var operation = (await store.GetAsync(queued.Operation.LocalOperationId, Scope()))!;

        Assert.Equal(0, outcome.RetryScheduled);
        Assert.Equal(1, outcome.Rejected);
        Assert.Equal(OfflineOperationStatus.Rejected, operation.Status);
        Assert.Equal("RETRY_EXHAUSTED", operation.ResultCode);
    }

    [Fact]
    public async Task Malformed_business_success_without_result_entity_fails_closed_and_retries()
    {
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 26, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        var queued = await EnqueueRequestAsync(store, Request());
        using var key = new TestSigningKey();
        var call = 0;
        using var http = new HttpClient(new DelegateHandler(async (request, cancellationToken) =>
        {
            var captured = await CaptureAsync(request, cancellationToken);
            if (++call == 1) return Challenge(Base64Url(RandomNumberGenerator.GetBytes(32)), captured.AttemptCorrelationId);
            var wire = Assert.Single(ReadBatch(captured.Body).Operations);
            var malformed = OperationResult(wire, "SUCCEEDED", resultVersion: 2) with { ResultEntityId = null };
            return Json(HttpStatusCode.OK, new SyncV1BatchResponse(
                "sync-v1", [malformed], clock.GetUtcNow(), captured.AttemptCorrelationId));
        }));

        var outcome = await Client(http, store, key, clock, "token").ProcessNextBatchAsync();
        var persisted = (await store.GetAsync(queued.Operation.LocalOperationId, Scope()))!;

        Assert.Equal(0, outcome.Succeeded);
        Assert.Equal(1, outcome.RetryScheduled);
        Assert.Equal(OfflineOperationStatus.Failed, persisted.Status);
        Assert.Equal("CLIENT_SUCCESS_RESULT_INVALID", persisted.ResultCode);
        Assert.Null(persisted.ResultEntityId);
    }

    [Fact]
    public async Task Conflict_without_complete_redacted_review_fails_closed_and_retries()
    {
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 26, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        var queued = await EnqueueRequestAsync(store, Request());
        using var key = new TestSigningKey();
        var call = 0;
        using var http = new HttpClient(new DelegateHandler(async (request, cancellationToken) =>
        {
            var captured = await CaptureAsync(request, cancellationToken);
            if (++call == 1) return Challenge(Base64Url(RandomNumberGenerator.GetBytes(32)), captured.AttemptCorrelationId);
            var wire = Assert.Single(ReadBatch(captured.Body).Operations);
            var malformed = OperationResult(wire, "CONFLICT", "BASE_VERSION_CONFLICT") with
                { ConflictReview = null };
            return Json(HttpStatusCode.OK, new SyncV1BatchResponse(
                "sync-v1", [malformed], clock.GetUtcNow(), captured.AttemptCorrelationId));
        }));

        var outcome = await Client(http, store, key, clock, "token").ProcessNextBatchAsync();
        var persisted = (await store.GetAsync(queued.Operation.LocalOperationId, Scope()))!;

        Assert.Equal(0, outcome.Conflicted);
        Assert.Equal(1, outcome.RetryScheduled);
        Assert.Equal(OfflineOperationStatus.Failed, persisted.Status);
        Assert.Equal("CLIENT_CONFLICT_RESULT_INVALID", persisted.ResultCode);
        Assert.Null(persisted.ConflictCaseId);
        Assert.Null(persisted.ConflictReview);
    }

    [Fact]
    public async Task Partial_batch_results_are_matched_by_both_stable_operation_identities()
    {
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 26, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        var first = await EnqueueRequestAsync(store, Request());
        var second = await EnqueueRequestAsync(store, Request(Guid.NewGuid()) with { PayloadJson = "{\"amount\":43}" });
        var third = await EnqueueRequestAsync(store, Request(Guid.NewGuid()) with { PayloadJson = "{\"amount\":44}" });
        using var key = new TestSigningKey();
        var call = 0;
        Guid? wireAttempt = null;
        using var http = new HttpClient(new DelegateHandler(async (request, cancellationToken) =>
        {
            call++;
            var captured = await CaptureAsync(request, cancellationToken);
            if (call == 1) return Challenge(Base64Url(RandomNumberGenerator.GetBytes(32)), captured.AttemptCorrelationId);
            wireAttempt = captured.AttemptCorrelationId;
            var batch = ReadBatch(captured.Body);
            var byClientId = batch.Operations.ToDictionary(operation => operation.ClientOperationId);
            var results = new[]
            {
                OperationResult(byClientId[third.Operation.ClientOperationId], "REJECTED", "SCOPE_DENIED"),
                OperationResult(byClientId[first.Operation.ClientOperationId], "SUCCEEDED", resultVersion: 12),
                OperationResult(byClientId[second.Operation.ClientOperationId], "CONFLICT", "BASE_VERSION_CONFLICT")
            };
            return Json(HttpStatusCode.OK, new SyncV1BatchResponse(
                "sync-v1", results, clock.GetUtcNow(), captured.AttemptCorrelationId));
        }));

        var outcome = await Client(http, store, key, clock, "token").ProcessNextBatchAsync(3);

        Assert.Equal(1, outcome.Succeeded);
        Assert.Equal(1, outcome.Conflicted);
        Assert.Equal(1, outcome.Rejected);
        var completedFirst = (await store.GetAsync(first.Operation.LocalOperationId, Scope()))!;
        var completedSecond = (await store.GetAsync(second.Operation.LocalOperationId, Scope()))!;
        var completedThird = (await store.GetAsync(third.Operation.LocalOperationId, Scope()))!;
        Assert.Equal(OfflineOperationStatus.Succeeded, completedFirst.Status);
        Assert.Equal(OfflineOperationStatus.Conflict, completedSecond.Status);
        Assert.NotNull(completedSecond.ConflictCaseId);
        Assert.True(completedSecond.ConflictReview?.IsDecisionReady == true);
        Assert.Equal(second.Operation.BaseVersion, completedSecond.ConflictReview!.BaseVersion);
        Assert.Equal(second.Operation.EntityId, completedSecond.ConflictReview.ServerSnapshot.EntityId);
        Assert.Equal(OfflineOperationStatus.Rejected, completedThird.Status);
        var localAttempts = new[]
        {
            completedFirst.AttemptCorrelationId, completedSecond.AttemptCorrelationId, completedThird.AttemptCorrelationId
        };
        Assert.Equal(3, localAttempts.Distinct().Count());
        Assert.DoesNotContain(wireAttempt, localAttempts);
    }

    [Fact]
    public async Task A_queue_from_another_scope_is_not_claimed_or_mutated_by_this_transport()
    {
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 26, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        var queued = await EnqueueRequestAsync(store, Request() with { BranchId = Guid.NewGuid() });
        using var key = new TestSigningKey();
        var calls = 0;
        using var http = new HttpClient(new DelegateHandler((_, _) =>
        {
            calls++;
            throw new InvalidOperationException("Out-of-scope data must not reach HTTP.");
        }));

        var result = await Client(http, store, key, clock, "token").ProcessNextBatchAsync();
        var invisible = await store.GetAsync(queued.Operation.LocalOperationId, Scope());
        var operationScope = new OfflineOperationScope(
            queued.Operation.CompanyId,
            queued.Operation.BranchId,
            queued.Operation.UserId,
            queued.Operation.RegisteredDeviceId);
        var persisted = await store.GetAsync(queued.Operation.LocalOperationId, operationScope);

        Assert.Equal(0, calls);
        Assert.Equal(0, result.Claimed);
        Assert.Equal(0, result.Rejected);
        Assert.Null(invisible);
        Assert.Equal(OfflineOperationStatus.Queued, persisted!.Status);
        Assert.Null(persisted.ResultCode);
    }

    [Theory]
    [InlineData("DEVICE_NOT_REGISTERED", false)]
    [InlineData("SESSION_REVOKED", false)]
    [InlineData("invalid_dpop_proof", true)]
    [InlineData("DEVICE_PROOF_KEY_REQUIRED", false)]
    public async Task Key_rotation_device_suspension_and_session_rejection_fail_closed(
        string errorCode,
        bool rejectAfterProof)
    {
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 26, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        var queued = await EnqueueRequestAsync(store, Request());
        using var key = new TestSigningKey();
        var call = 0;
        using var http = new HttpClient(new DelegateHandler(async (request, cancellationToken) =>
        {
            call++;
            var captured = await CaptureAsync(request, cancellationToken);
            if (rejectAfterProof && call == 1)
                return Challenge(Base64Url(RandomNumberGenerator.GetBytes(32)), captured.AttemptCorrelationId);
            return Json(HttpStatusCode.Forbidden, new
            {
                ErrorCode = errorCode,
                CorrelationId = captured.AttemptCorrelationId
            });
        }));

        var outcome = await Client(http, store, key, clock, "token").ProcessNextBatchAsync();
        var persisted = await store.GetAsync(queued.Operation.LocalOperationId, Scope());

        Assert.Equal(1, outcome.Rejected);
        Assert.Equal(0, outcome.RetryScheduled);
        Assert.Equal(OfflineOperationStatus.Rejected, persisted!.Status);
        Assert.Equal(errorCode, persisted.ResultCode);
        Assert.Equal(rejectAfterProof ? 2 : 1, call);
    }

    [Theory]
    [InlineData("INTERNAL_ERROR")]
    [InlineData("RATE_LIMITED")]
    public async Task Only_governed_server_error_codes_consume_the_client_retry_budget(string errorCode)
    {
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 26, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        var queued = await EnqueueRequestAsync(store, Request());
        using var key = new TestSigningKey();
        var call = 0;
        using var http = new HttpClient(new DelegateHandler(async (request, cancellationToken) =>
        {
            call++;
            var captured = await CaptureAsync(request, cancellationToken);
            if (call == 1) return Challenge(Base64Url(RandomNumberGenerator.GetBytes(32)), captured.AttemptCorrelationId);
            return Success(captured, "FAILED", errorCode);
        }));

        var outcome = await Client(http, store, key, clock, "token").ProcessNextBatchAsync();
        var persisted = await store.GetAsync(queued.Operation.LocalOperationId, Scope());

        Assert.Equal(1, outcome.RetryScheduled);
        Assert.Equal(OfflineOperationStatus.Failed, persisted!.Status);
        Assert.Equal(1, persisted.ClientTransportRetryCount);
        Assert.Equal(errorCode, persisted.ResultCode);
    }

    [Theory]
    [InlineData("CLIENT_PROOF_CREATE_FAILURE", "SERVER_ERROR_CODE_RESERVED")]
    [InlineData("CLIENT_SIGNED_SEND_FAILURE", "SERVER_ERROR_CODE_RESERVED")]
    [InlineData("NO_RESPONSE", "NO_RESPONSE")]
    [InlineData("TIMEOUT", "TIMEOUT")]
    public async Task Remote_operation_codes_cannot_impersonate_client_failures_or_force_retry(
        string remoteCode,
        string expectedStoredCode)
    {
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 27, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        var queued = await EnqueueRequestAsync(store, Request());
        using var key = new TestSigningKey();
        var call = 0;
        using var http = new HttpClient(new DelegateHandler(async (request, cancellationToken) =>
        {
            var captured = await CaptureAsync(request, cancellationToken);
            if (++call == 1)
                return Challenge(Base64Url(RandomNumberGenerator.GetBytes(32)),
                    captured.AttemptCorrelationId);
            return Success(captured, "FAILED", remoteCode);
        }));

        var outcome = await Client(http, store, key, clock, "token").ProcessNextBatchAsync();
        var persisted = await store.GetAsync(queued.Operation.LocalOperationId, Scope());

        Assert.Equal(0, outcome.RetryScheduled);
        Assert.Equal(1, outcome.Rejected);
        Assert.Equal(OfflineOperationStatus.Rejected, persisted!.Status);
        Assert.Equal(expectedStoredCode, persisted.ResultCode);
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest, false)]
    [InlineData(HttpStatusCode.ServiceUnavailable, true)]
    public async Task Remote_top_level_client_code_is_reserved_and_http_status_alone_controls_retry(
        HttpStatusCode statusCode,
        bool expectedRetry)
    {
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 27, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        var queued = await EnqueueRequestAsync(store, Request());
        using var key = new TestSigningKey();
        using var http = new HttpClient(new DelegateHandler(async (request, cancellationToken) =>
        {
            var captured = await CaptureAsync(request, cancellationToken);
            return Json(statusCode, new
            {
                ErrorCode = "CLIENT_NONCE_FAILURE",
                CorrelationId = captured.AttemptCorrelationId
            });
        }));

        var outcome = await Client(http, store, key, clock, "token").ProcessNextBatchAsync();
        var persisted = await store.GetAsync(queued.Operation.LocalOperationId, Scope());

        Assert.Equal(expectedRetry ? 1 : 0, outcome.RetryScheduled);
        Assert.Equal(expectedRetry ? 0 : 1, outcome.Rejected);
        Assert.Equal(
            expectedRetry ? OfflineOperationStatus.Failed : OfflineOperationStatus.Rejected,
            persisted!.Status);
        Assert.Equal("SERVER_ERROR_CODE_RESERVED", persisted.ResultCode);
    }

    [Fact]
    public void Client_local_diagnostic_codes_are_unique_safe_and_bounded()
    {
        var codes = new[]
        {
            "CLIENT_BEARER_FAILURE",
            "CLIENT_NONCE_FAILURE",
            "CLIENT_PIPELINE_FAILURE",
            "CLIENT_PROOF_CREATE_FAILURE",
            "CLIENT_SIGNED_SEND_FAILURE",
            "CLIENT_SIGNED_JSON_INVALID",
            "CLIENT_RESPONSE_ENVELOPE_INVALID",
            "CLIENT_RESULT_SET_INVALID",
            "CLIENT_RESULT_MISSING",
            "CLIENT_PENDING_RESULT_INVALID",
            "CLIENT_SUCCESS_RESULT_INVALID",
            "CLIENT_CONFLICT_RESULT_INVALID",
            "CLIENT_CONFLICT_ID_MISSING",
            "CLIENT_HTTP_CORRELATION_MISMATCH"
        };

        Assert.Equal(codes.Length, codes.Distinct(StringComparer.Ordinal).Count());
        Assert.All(codes, code =>
        {
            Assert.InRange(code.Length, 1, 40);
            Assert.Matches("^[A-Z0-9_]+$", code);
        });
    }

    [Fact]
    public async Task Bearer_nonce_jti_and_proof_are_not_persisted()
    {
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 26, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        var queued = await EnqueueRequestAsync(store, Request());
        using var key = new TestSigningKey();
        const string bearer = "high-entropy-volatile-bearer-marker";
        var nonce = Base64Url(RandomNumberGenerator.GetBytes(32));
        CapturedRequest? signed = null;
        var call = 0;
        using var http = new HttpClient(new DelegateHandler(async (request, cancellationToken) =>
        {
            call++;
            var captured = await CaptureAsync(request, cancellationToken);
            if (call == 1) return Challenge(nonce, captured.AttemptCorrelationId);
            signed = captured;
            return Success(captured, "SUCCEEDED");
        }));

        await Client(http, store, key, clock, bearer).ProcessNextBatchAsync();

        var jti = ProofClaims(signed!.Proof!).GetProperty("jti").GetString()!;
        var persisted = await store.GetAsync(queued.Operation.LocalOperationId, Scope());
        var fileBytes = Directory.EnumerateFiles(_directory).SelectMany(File.ReadAllBytes).ToArray();
        var fileText = Encoding.UTF8.GetString(fileBytes);
        var persistedJson = JsonSerializer.Serialize(persisted);
        foreach (var secret in new[] { bearer, nonce, jti, signed.Proof! })
        {
            Assert.DoesNotContain(secret, fileText, StringComparison.Ordinal);
            Assert.DoesNotContain(secret, persistedJson, StringComparison.Ordinal);
        }
    }

    [Fact]
    public async Task Conflict_reapply_survives_timeout_with_stable_replacement_identity_and_fresh_proof()
    {
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 26, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        var queued = await EnqueueRequestAsync(store, Request());
        var claimed = await store.ClaimNextAsync("worker", TimeSpan.FromMinutes(1), Scope());
        var conflictCaseId = Guid.NewGuid();
        await store.MarkConflictAsync(claimed!.LocalOperationId, claimed.AttemptCorrelationId!.Value,
            conflictCaseId, "BASE_VERSION_CONFLICT", ConflictReview(claimed),
            StableServerOperationId(queued.Operation.ClientOperationId));
        using var key = new TestSigningKey();
        var signed = new List<CapturedRequest>();
        var wireCorrelations = new List<Guid>();
        var call = 0;
        using var http = new HttpClient(new DelegateHandler(async (request, cancellationToken) =>
        {
            call++;
            var captured = await CaptureAsync(request, cancellationToken);
            wireCorrelations.Add(captured.AttemptCorrelationId);
            if (call is 1 or 3)
                return Challenge(Base64Url(RandomNumberGenerator.GetBytes(32)), captured.AttemptCorrelationId);
            signed.Add(captured);
            if (call == 2) throw new TaskCanceledException("server committed but response was lost");
            var resolution = JsonSerializer.Deserialize<SyncV1ConflictResolutionRequest>(captured.Body,
                new JsonSerializerOptions(JsonSerializerDefaults.Web))!;
            Assert.Equal(TestBuildIdentity, resolution.BuildIdentity);
            return Json(HttpStatusCode.OK, new SyncV1ConflictResolutionResponse(
                conflictCaseId, StableServerOperationId(queued.Operation.ClientOperationId),
                resolution.Decision, "RESOLVED", "RESOLVED",
                null, Guid.NewGuid(), clock.GetUtcNow(), captured.AttemptCorrelationId));
        }));
        var options = new OfflineSyncTransportOptions(Endpoint, "desktop-device-1", RegisteredDeviceId,
            queued.Operation.CompanyId, queued.Operation.BranchId, queued.Operation.UserId, "test-worker",
            BuildIdentity: TestBuildIdentity);
        var client = new OfflineSyncConflictClient(http, store, new FixedBearerProvider("token"), key, options, clock);

        await Assert.ThrowsAsync<TaskCanceledException>(() =>
            client.ResolveAsync(queued.Operation.LocalOperationId, OfflineConflictDecision.Reapply,
                "إعادة تطبيق بعد مراجعة الإصدار الحالي", 12));
        Assert.Equal(OfflineOperationStatus.Conflict,
            (await store.GetAsync(queued.Operation.LocalOperationId, Scope()))!.Status);
        await client.ResolveAsync(queued.Operation.LocalOperationId, OfflineConflictDecision.Reapply,
            "إعادة تطبيق بعد مراجعة الإصدار الحالي", 12);

        Assert.Equal(signed[0].Body, signed[1].Body);
        Assert.NotEqual(signed[0].Proof, signed[1].Proof);
        Assert.Equal(4, wireCorrelations.Distinct().Count());
        Assert.NotEqual(signed[0].AttemptCorrelationId, signed[1].AttemptCorrelationId);
        var first = JsonSerializer.Deserialize<SyncV1ConflictResolutionRequest>(signed[0].Body,
            new JsonSerializerOptions(JsonSerializerDefaults.Web))!;
        var second = JsonSerializer.Deserialize<SyncV1ConflictResolutionRequest>(signed[1].Body,
            new JsonSerializerOptions(JsonSerializerDefaults.Web))!;
        Assert.Equal(first.Reapply!.ClientOperationId, second.Reapply!.ClientOperationId);
        Assert.Equal(first.Reapply.OperationCorrelationId, second.Reapply.OperationCorrelationId);
        Assert.Equal("إعادة تطبيق بعد مراجعة الإصدار الحالي", first.Reason);
        Assert.DoesNotContain(first.Reason, JsonSerializer.Serialize(
            await store.GetAsync(queued.Operation.LocalOperationId, Scope())), StringComparison.Ordinal);
        Assert.Equal(OfflineOperationStatus.Resolved,
            (await store.GetAsync(queued.Operation.LocalOperationId, Scope()))!.Status);
    }

    [Theory]
    [InlineData("")]
    [InlineData("contains access_token=secret")]
    [InlineData("line\nbreak")]
    public async Task Conflict_reason_must_be_explicit_bounded_safe_text(string reason)
    {
        var store = await CreateStoreAsync(TimeProvider.System);
        var queued = await EnqueueRequestAsync(store, Request());
        var claimed = await store.ClaimNextAsync("worker", TimeSpan.FromMinutes(1), Scope());
        await store.MarkConflictAsync(claimed!.LocalOperationId, claimed.AttemptCorrelationId!.Value,
            Guid.NewGuid(), "BASE_VERSION_CONFLICT", ConflictReview(claimed));
        using var key = new TestSigningKey();
        using var http = new HttpClient(new DelegateHandler((_, _) =>
            throw new InvalidOperationException("An invalid reason must fail before HTTP.")));
        var client = new OfflineSyncConflictClient(http, store, new FixedBearerProvider("token"), key,
            new OfflineSyncTransportOptions(Endpoint, "desktop-device-1", RegisteredDeviceId,
                CompanyId, BranchId, UserId, "test-worker", BuildIdentity: TestBuildIdentity));

        var error = await Assert.ThrowsAsync<OfflineStoreException>(() => client.ResolveAsync(
            queued.Operation.LocalOperationId, OfflineConflictDecision.KeepServer, reason));

        Assert.Equal("CONFLICT_REASON_INVALID", error.Code);
    }

    public void Dispose()
    {
        if (Directory.Exists(_directory)) Directory.Delete(_directory, recursive: true);
        CryptographicOperations.ZeroMemory(_outboxKey);
        CryptographicOperations.ZeroMemory(_cacheKey);
    }

    private async Task<OfflineOperationStore> CreateStoreAsync(TimeProvider clock)
    {
        Directory.CreateDirectory(_directory);
        var store = new OfflineOperationStore(Path.Combine(_directory, "outbox.db"),
            new FixedKeyProvider(_outboxKey, _cacheKey), clock,
            new OfflineRetryPolicy(5, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(1)));
        await store.InitializeAsync();
        return store;
    }

    private static OfflineSyncTransportClient Client(
        HttpClient http,
        OfflineOperationStore store,
        IDeviceProofSigningKey key,
        TimeProvider clock,
        string bearer) => new(http, store, new FixedBearerProvider(bearer), key,
        new OfflineSyncTransportOptions(Endpoint, "desktop-device-1", RegisteredDeviceId,
            CompanyId, BranchId, UserId, "test-worker", BuildIdentity: TestBuildIdentity), clock);

    private static OfflineOperationEnqueueRequest Request(Guid? localIntentId = null) => new(
        localIntentId ?? Guid.NewGuid(),
        CompanyId,
        BranchId,
        UserId,
        RegisteredDeviceId,
        "UpdateWaybillDraft",
        "UPDATE",
        "Waybill",
        Guid.Parse("55555555-5555-5555-5555-555555555555"),
        1,
        new DateTimeOffset(2026, 8, 26, 9, 30, 0, TimeSpan.Zero),
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

    private static OfflineOperationScope Scope() => new(CompanyId, BranchId, UserId, RegisteredDeviceId);

    private static HttpResponseMessage Challenge(string nonce, Guid correlationId)
    {
        var response = Json(HttpStatusCode.Unauthorized, new
        {
            ErrorCode = "use_dpop_nonce",
            CorrelationId = correlationId
        });
        response.Headers.TryAddWithoutValidation("DPoP-Nonce", nonce);
        response.Headers.WwwAuthenticate.ParseAdd("DPoP error=\"use_dpop_nonce\"");
        return response;
    }

    private static HttpResponseMessage Success(
        CapturedRequest request,
        string status,
        string? errorCode = null,
        Guid? resultEntityId = null,
        long? resultVersion = null)
    {
        var batch = ReadBatch(request.Body);
        var results = batch.Operations.Select(operation => OperationResult(
            operation, status, errorCode, resultEntityId, resultVersion)).ToArray();
        return Json(HttpStatusCode.OK, new SyncV1BatchResponse(
            "sync-v1", results, DateTimeOffset.UtcNow, request.AttemptCorrelationId));
    }

    private static SyncV1OperationResult OperationResult(
        SyncV1OperationRequest operation,
        string status,
        string? errorCode = null,
        Guid? resultEntityId = null,
        long? resultVersion = null) => new(
        operation.ClientOperationId,
        operation.OperationCorrelationId,
        StableServerOperationId(operation.ClientOperationId),
        operation.ActionCode,
        resultEntityId ?? (status == "SUCCEEDED" ? StableResultEntityId(operation.ClientOperationId) : null),
        status,
        resultVersion,
        errorCode,
        status == "CONFLICT" ? Guid.NewGuid() : null,
        DateTimeOffset.UtcNow,
        status == "CONFLICT" ? new SyncV1ConflictReview(
            operation.BaseVersion,
            errorCode ?? "BASE_VERSION_CONFLICT",
            new SyncV1ConflictLocalSnapshot(operation.ActionCode, operation.EntityType, operation.EntityId,
                operation.BaseVersion),
            new SyncV1ConflictServerSnapshot(operation.EntityType, operation.EntityId, true,
                operation.BaseVersion + 1),
            "OPEN", null, false, null, null) : null);

    private static Guid StableServerOperationId(string clientOperationId)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes("server-operation|" + clientOperationId));
        return new Guid(hash.AsSpan(0, 16));
    }

    private static Guid StableResultEntityId(string clientOperationId)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes("result-entity|" + clientOperationId));
        return new Guid(hash.AsSpan(0, 16));
    }

    private static async Task WaitUntilAsync(Func<Task<bool>> condition, CancellationToken cancellationToken)
    {
        while (!await condition())
            await Task.Delay(10, cancellationToken);
    }

    private static HttpResponseMessage Json(HttpStatusCode status, object value) => new(status)
    {
        Content = new StringContent(
            JsonSerializer.Serialize(value, new JsonSerializerOptions(JsonSerializerDefaults.Web)),
            Encoding.UTF8,
            "application/json")
    };

    private static SyncV1BatchRequest ReadBatch(byte[] body) =>
        JsonSerializer.Deserialize<SyncV1BatchRequest>(body,
            new JsonSerializerOptions(JsonSerializerDefaults.Web))!;

    private static async Task<CapturedRequest> CaptureAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var body = await request.Content!.ReadAsByteArrayAsync(cancellationToken);
        var correlation = Guid.ParseExact(request.Headers.GetValues("X-Correlation-Id").Single(), "D");
        var proof = request.Headers.TryGetValues("DPoP", out var proofs) ? proofs.Single() : null;
        return new(body, correlation, proof, request.Headers.Authorization?.Parameter,
            request.Content.Headers.ContentType?.MediaType);
    }

    private static void VerifyProof(
        string proof,
        byte[] body,
        string bearer,
        string nonce,
        Guid correlationId,
        TestSigningKey expectedKey)
    {
        var segments = proof.Split('.');
        Assert.Equal(3, segments.Length);
        var rawHeader = DecodeBase64Url(segments[0]);
        Assert.StartsWith("{\"typ\":\"dpop+jwt\"", Encoding.UTF8.GetString(rawHeader), StringComparison.Ordinal);
        Assert.DoesNotContain("\\u002B", Encoding.UTF8.GetString(rawHeader), StringComparison.OrdinalIgnoreCase);
        using var header = JsonDocument.Parse(rawHeader);
        using var payload = JsonDocument.Parse(DecodeBase64Url(segments[1]));
        Assert.Equal("dpop+jwt", header.RootElement.GetProperty("typ").GetString());
        Assert.Equal("ES256", header.RootElement.GetProperty("alg").GetString());
        Assert.Equal("EC", header.RootElement.GetProperty("jwk").GetProperty("kty").GetString());
        Assert.Equal("P-256", header.RootElement.GetProperty("jwk").GetProperty("crv").GetString());
        Assert.Equal("POST", payload.RootElement.GetProperty("htm").GetString());
        Assert.Equal(Endpoint.AbsoluteUri, payload.RootElement.GetProperty("htu").GetString());
        Assert.Equal(new DateTimeOffset(2026, 8, 26, 10, 0, 0, TimeSpan.Zero).ToUnixTimeSeconds(),
            payload.RootElement.GetProperty("iat").GetInt64());
        Assert.Equal(nonce, payload.RootElement.GetProperty("nonce").GetString());
        Assert.Equal(correlationId.ToString("D"), payload.RootElement.GetProperty("cid").GetString());
        Assert.Equal(Base64Url(SHA256.HashData(Encoding.ASCII.GetBytes(bearer))),
            payload.RootElement.GetProperty("ath").GetString());
        Assert.Equal(Base64Url(SHA256.HashData(body)), payload.RootElement.GetProperty("tbh").GetString());
        var jti = Guid.ParseExact(payload.RootElement.GetProperty("jti").GetString()!, "D");
        Assert.Equal(4, jti.Version);
        Assert.True(expectedKey.Verify(
            Encoding.ASCII.GetBytes(segments[0] + "." + segments[1]), DecodeBase64Url(segments[2])));
    }

    private static void AssertExactJsonOnlyWireContract(byte[] body)
    {
        using var document = JsonDocument.Parse(body);
        Assert.Equal(new[] { "deviceId", "protocolVersion", "operations", "buildIdentity" },
            document.RootElement.EnumerateObject().Select(property => property.Name).ToArray());
        Assert.Equal(new[] { "platform", "artifactSha256", "signerCertificateSha256" },
            document.RootElement.GetProperty("buildIdentity").EnumerateObject()
                .Select(property => property.Name).ToArray());
        var operation = document.RootElement.GetProperty("operations")[0];
        Assert.Equal(new[]
        {
            "actionCode", "operationType", "entityType", "entityId", "clientOperationId",
            "payloadJson", "payloadHash", "clientOccurredAt", "operationCorrelationId", "baseVersion"
        }, operation.EnumerateObject().Select(property => property.Name).ToArray());
        Assert.DoesNotContain(operation.EnumerateObject(), property =>
            property.Name.Contains("attachment", StringComparison.OrdinalIgnoreCase) ||
            property.Name.Contains("binary", StringComparison.OrdinalIgnoreCase));
    }

    private static JsonElement ProofClaims(string proof)
    {
        using var document = JsonDocument.Parse(DecodeBase64Url(proof.Split('.')[1]));
        return document.RootElement.Clone();
    }

    private static string Base64Url(ReadOnlySpan<byte> value) =>
        Convert.ToBase64String(value).TrimEnd('=').Replace('+', '-').Replace('/', '_');

    private static byte[] DecodeBase64Url(string value)
    {
        var padded = value.Replace('-', '+').Replace('_', '/');
        padded += new string('=', (4 - padded.Length % 4) % 4);
        return Convert.FromBase64String(padded);
    }

    private sealed record CapturedRequest(
        byte[] Body,
        Guid AttemptCorrelationId,
        string? Proof,
        string? Bearer,
        string? MediaType);

    private sealed class DelegateHandler(
        Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> callback) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken) => callback(request, cancellationToken);
    }

    private sealed class TestSigningKey : IDeviceProofSigningKey, IDisposable
    {
        private readonly ECDsa _key = ECDsa.Create(ECCurve.NamedCurves.nistP256);

        public ValueTask<DevicePublicP256Jwk> GetPublicJwkAsync(CancellationToken cancellationToken = default)
        {
            var parameters = _key.ExportParameters(includePrivateParameters: false);
            return ValueTask.FromResult(new DevicePublicP256Jwk(
                Base64Url(parameters.Q.X!), Base64Url(parameters.Q.Y!)));
        }

        public ValueTask<byte[]> SignEs256Async(
            ReadOnlyMemory<byte> signingInput,
            CancellationToken cancellationToken = default) => ValueTask.FromResult(
            _key.SignData(signingInput.Span, HashAlgorithmName.SHA256,
                DSASignatureFormat.IeeeP1363FixedFieldConcatenation));

        public bool Verify(byte[] signingInput, byte[] signature) => _key.VerifyData(
            signingInput, signature, HashAlgorithmName.SHA256,
            DSASignatureFormat.IeeeP1363FixedFieldConcatenation);

        public void Dispose() => _key.Dispose();
    }

    private sealed class FailingSigningKey(
        IDeviceProofSigningKey inner,
        Exception failure) : IDeviceProofSigningKey
    {
        public ValueTask<DevicePublicP256Jwk> GetPublicJwkAsync(
            CancellationToken cancellationToken = default) =>
            inner.GetPublicJwkAsync(cancellationToken);

        public ValueTask<byte[]> SignEs256Async(
            ReadOnlyMemory<byte> signingInput,
            CancellationToken cancellationToken = default) =>
            throw failure;
    }

    private sealed class FixedBearerProvider(string bearer) : IInMemoryBearerTokenProvider
    {
        public ValueTask<string> GetBearerTokenAsync(CancellationToken cancellationToken = default) =>
            ValueTask.FromResult(bearer);
    }

    private sealed class ThrowingBearerProvider(Exception failure) : IInMemoryBearerTokenProvider
    {
        public ValueTask<string> GetBearerTokenAsync(CancellationToken cancellationToken = default) =>
            throw failure;
    }

    private sealed class FixedKeyProvider(byte[] outboxKey, byte[] cacheKey) : ILocalEncryptionKeyProvider
    {
        public ValueTask<byte[]> GetKeyAsync(LocalStorePurpose purpose, CancellationToken cancellationToken = default) =>
            ValueTask.FromResult((purpose == LocalStorePurpose.WriteOutbox ? outboxKey : cacheKey).ToArray());
    }

    private sealed class ManualConnectivity(bool initiallyOnline) : IOfflineSyncConnectivity
    {
        private volatile bool _online = initiallyOnline;
        private TaskCompletionSource _onlineSignal = NewSignal();

        public ValueTask<bool> IsOnlineAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return ValueTask.FromResult(_online);
        }

        public Task WaitUntilOnlineAsync(CancellationToken cancellationToken = default) =>
            _online ? Task.CompletedTask : _onlineSignal.Task.WaitAsync(cancellationToken);

        public void SetOnline()
        {
            _online = true;
            _onlineSignal.TrySetResult();
        }

        private static TaskCompletionSource NewSignal() =>
            new(TaskCreationOptions.RunContinuationsAsynchronously);
    }

    private sealed class OneShotFailingConnectivity : IOfflineSyncConnectivity
    {
        private int _checks;

        public ValueTask<bool> IsOnlineAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (Interlocked.Increment(ref _checks) == 1)
                throw new InvalidOperationException("simulated one-shot connectivity adapter failure");
            return ValueTask.FromResult(true);
        }

        public Task WaitUntilOnlineAsync(CancellationToken cancellationToken = default) =>
            Task.CompletedTask;
    }

    private sealed class MutableTimeProvider(DateTimeOffset initial) : TimeProvider
    {
        private DateTimeOffset _current = initial;
        public override DateTimeOffset GetUtcNow() => _current;
        public void Advance(TimeSpan duration) => _current += duration;
    }

    private sealed class AdvancingTimeProvider(DateTimeOffset initial, TimeSpan step) : TimeProvider
    {
        private long _utcTicks = initial.UtcDateTime.Ticks - step.Ticks;

        public override DateTimeOffset GetUtcNow() =>
            new(Interlocked.Add(ref _utcTicks, step.Ticks), TimeSpan.Zero);
    }
}
