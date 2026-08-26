using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using TransportERP.Offline.Transport;

namespace TransportERP.Offline.Tests;

public sealed class OfflineSyncTransportTests : IDisposable
{
    private static readonly Uri Endpoint = new("https://sync.example.test/api/v1/sync/operations:batch");
    private static readonly Guid CompanyId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid BranchId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    private static readonly Guid UserId = Guid.Parse("33333333-3333-3333-3333-333333333333");
    private static readonly Guid RegisteredDeviceId = Guid.Parse("44444444-4444-4444-4444-444444444444");
    private readonly string _directory = Path.Combine(Path.GetTempPath(), "transporterp-transport-tests", Guid.NewGuid().ToString("N"));
    private readonly byte[] _outboxKey = RandomNumberGenerator.GetBytes(32);
    private readonly byte[] _cacheKey = RandomNumberGenerator.GetBytes(32);

    [Fact]
    public async Task Nonce_challenge_is_followed_by_a_fresh_cryptographically_valid_proof_over_exact_body()
    {
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 26, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        var queued = await store.EnqueueAsync(Request());
        var nonce = Base64Url(RandomNumberGenerator.GetBytes(32));
        const string bearer = "volatile-session-token";
        byte[]? challengeBody = null;
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
        AssertExactJsonOnlyWireContract(signed.Body);
        VerifyProof(signed.Proof!, signed.Body, bearer, nonce, signed.AttemptCorrelationId, key);
        var persisted = await store.GetAsync(queued.Operation.LocalOperationId);
        Assert.Equal(OfflineOperationStatus.Succeeded, persisted!.Status);
        Assert.Equal(9, persisted.ResultVersion);
    }

    [Fact]
    public async Task Signed_nonce_refresh_creates_a_new_jti_and_signature_for_the_same_http_attempt_body()
    {
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 26, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        await store.EnqueueAsync(Request());
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
        Assert.Equal(signed[0].AttemptCorrelationId, signed[1].AttemptCorrelationId);
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
        var queued = await store.EnqueueAsync(Request());
        using var key = new TestSigningKey();
        var signedRequests = new List<CapturedRequest>();
        var call = 0;
        using var http = new HttpClient(new DelegateHandler(async (request, cancellationToken) =>
        {
            call++;
            var captured = await CaptureAsync(request, cancellationToken);
            if (call is 1 or 3) return Challenge(Base64Url(RandomNumberGenerator.GetBytes(32)), captured.AttemptCorrelationId);
            signedRequests.Add(captured);
            if (call == 2) throw new TaskCanceledException("simulated timeout after server acceptance");
            return Success(captured, "QUEUED");
        }));
        var client = Client(http, store, key, clock, "memory-only-token");

        var firstRun = await client.ProcessNextBatchAsync();
        var failed = await store.GetAsync(queued.Operation.LocalOperationId);
        Assert.Equal(1, firstRun.RetryScheduled);
        Assert.Equal(OfflineOperationStatus.Failed, failed!.Status);
        var firstAttempt = failed.AttemptCorrelationId;

        clock.Advance(TimeSpan.FromSeconds(5));
        var secondRun = await client.ProcessNextBatchAsync();
        var completed = await store.GetAsync(queued.Operation.LocalOperationId);

        Assert.Equal(1, secondRun.Succeeded);
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

    [Fact]
    public async Task No_http_response_is_retryable_and_does_not_become_a_business_rejection()
    {
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 26, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        var queued = await store.EnqueueAsync(Request());
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
        var persisted = await store.GetAsync(queued.Operation.LocalOperationId);

        Assert.Equal(1, outcome.RetryScheduled);
        Assert.Equal(OfflineOperationStatus.Failed, persisted!.Status);
        Assert.Equal("NO_RESPONSE", persisted.ResultCode);
        Assert.Equal(1, persisted.ClientTransportRetryCount);
    }

    [Fact]
    public async Task Partial_batch_results_are_matched_by_both_stable_operation_identities()
    {
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 26, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        var first = await store.EnqueueAsync(Request());
        var second = await store.EnqueueAsync(Request(Guid.NewGuid()) with { PayloadJson = "{\"amount\":43}" });
        var third = await store.EnqueueAsync(Request(Guid.NewGuid()) with { PayloadJson = "{\"amount\":44}" });
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
        var completedFirst = (await store.GetAsync(first.Operation.LocalOperationId))!;
        var completedSecond = (await store.GetAsync(second.Operation.LocalOperationId))!;
        var completedThird = (await store.GetAsync(third.Operation.LocalOperationId))!;
        Assert.Equal(OfflineOperationStatus.Succeeded, completedFirst.Status);
        Assert.Equal(OfflineOperationStatus.Conflict, completedSecond.Status);
        Assert.NotNull(completedSecond.ConflictCaseId);
        Assert.Equal(OfflineOperationStatus.Rejected, completedThird.Status);
        var localAttempts = new[]
        {
            completedFirst.AttemptCorrelationId, completedSecond.AttemptCorrelationId, completedThird.AttemptCorrelationId
        };
        Assert.Equal(3, localAttempts.Distinct().Count());
        Assert.Contains(wireAttempt, localAttempts);
    }

    [Fact]
    public async Task A_claim_from_another_company_branch_user_or_device_is_rejected_before_http()
    {
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 26, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        var queued = await store.EnqueueAsync(Request() with { BranchId = Guid.NewGuid() });
        using var key = new TestSigningKey();
        var calls = 0;
        using var http = new HttpClient(new DelegateHandler((_, _) =>
        {
            calls++;
            throw new InvalidOperationException("Out-of-scope data must not reach HTTP.");
        }));

        var result = await Client(http, store, key, clock, "token").ProcessNextBatchAsync();
        var persisted = await store.GetAsync(queued.Operation.LocalOperationId);

        Assert.Equal(0, calls);
        Assert.Equal(1, result.Rejected);
        Assert.Equal(OfflineOperationStatus.Rejected, persisted!.Status);
        Assert.Equal("LOCAL_SCOPE_INVALID", persisted.ResultCode);
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
        var queued = await store.EnqueueAsync(Request());
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
        var persisted = await store.GetAsync(queued.Operation.LocalOperationId);

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
        var queued = await store.EnqueueAsync(Request());
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
        var persisted = await store.GetAsync(queued.Operation.LocalOperationId);

        Assert.Equal(1, outcome.RetryScheduled);
        Assert.Equal(OfflineOperationStatus.Failed, persisted!.Status);
        Assert.Equal(1, persisted.ClientTransportRetryCount);
        Assert.Equal(errorCode, persisted.ResultCode);
    }

    [Fact]
    public async Task Bearer_nonce_jti_and_proof_are_not_persisted()
    {
        var clock = new MutableTimeProvider(new DateTimeOffset(2026, 8, 26, 10, 0, 0, TimeSpan.Zero));
        var store = await CreateStoreAsync(clock);
        var queued = await store.EnqueueAsync(Request());
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
        var persisted = await store.GetAsync(queued.Operation.LocalOperationId);
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
        var queued = await store.EnqueueAsync(Request());
        var claimed = await store.ClaimNextAsync("worker", TimeSpan.FromMinutes(1));
        var conflictCaseId = Guid.NewGuid();
        await store.MarkConflictAsync(claimed!.LocalOperationId, claimed.AttemptCorrelationId!.Value,
            conflictCaseId, "BASE_VERSION_CONFLICT");
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
            if (call == 2) throw new TaskCanceledException("server committed but response was lost");
            var resolution = JsonSerializer.Deserialize<SyncV1ConflictResolutionRequest>(captured.Body,
                new JsonSerializerOptions(JsonSerializerDefaults.Web))!;
            return Json(HttpStatusCode.OK, new SyncV1ConflictResolutionResponse(
                conflictCaseId, Guid.NewGuid(), resolution.Decision, "RESOLVED", "RESOLVED",
                null, Guid.NewGuid(), clock.GetUtcNow(), captured.AttemptCorrelationId));
        }));
        var options = new OfflineSyncTransportOptions(Endpoint, "desktop-device-1", RegisteredDeviceId,
            queued.Operation.CompanyId, queued.Operation.BranchId, queued.Operation.UserId, "test-worker");
        var client = new OfflineSyncConflictClient(http, store, new FixedBearerProvider("token"), key, options, clock);

        await Assert.ThrowsAsync<TaskCanceledException>(() =>
            client.ResolveAsync(queued.Operation.LocalOperationId, OfflineConflictDecision.Reapply, 12));
        Assert.Equal(OfflineOperationStatus.Conflict,
            (await store.GetAsync(queued.Operation.LocalOperationId))!.Status);
        await client.ResolveAsync(queued.Operation.LocalOperationId, OfflineConflictDecision.Reapply, 12);

        Assert.Equal(signed[0].Body, signed[1].Body);
        Assert.NotEqual(signed[0].Proof, signed[1].Proof);
        var first = JsonSerializer.Deserialize<SyncV1ConflictResolutionRequest>(signed[0].Body,
            new JsonSerializerOptions(JsonSerializerDefaults.Web))!;
        var second = JsonSerializer.Deserialize<SyncV1ConflictResolutionRequest>(signed[1].Body,
            new JsonSerializerOptions(JsonSerializerDefaults.Web))!;
        Assert.Equal(first.Reapply!.ClientOperationId, second.Reapply!.ClientOperationId);
        Assert.Equal(first.Reapply.OperationCorrelationId, second.Reapply.OperationCorrelationId);
        Assert.Equal(OfflineOperationStatus.Resolved,
            (await store.GetAsync(queued.Operation.LocalOperationId))!.Status);
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
            CompanyId, BranchId, UserId, "test-worker"), clock);

    private static OfflineOperationEnqueueRequest Request(Guid? localIntentId = null) => new(
        localIntentId ?? Guid.NewGuid(),
        CompanyId,
        BranchId,
        UserId,
        RegisteredDeviceId,
        "CreateWaybillDraft",
        "CREATE",
        "Waybill",
        null,
        null,
        new DateTimeOffset(2026, 8, 26, 9, 30, 0, TimeSpan.Zero),
        "{\"amount\":42}");

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
        Guid.NewGuid(),
        operation.ActionCode,
        resultEntityId,
        status,
        resultVersion,
        errorCode,
        status == "CONFLICT" ? Guid.NewGuid() : null,
        DateTimeOffset.UtcNow);

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
        using var header = JsonDocument.Parse(DecodeBase64Url(segments[0]));
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
        Assert.Equal(new[] { "deviceId", "protocolVersion", "operations" },
            document.RootElement.EnumerateObject().Select(property => property.Name).ToArray());
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

    private sealed class FixedBearerProvider(string bearer) : IInMemoryBearerTokenProvider
    {
        public ValueTask<string> GetBearerTokenAsync(CancellationToken cancellationToken = default) =>
            ValueTask.FromResult(bearer);
    }

    private sealed class FixedKeyProvider(byte[] outboxKey, byte[] cacheKey) : ILocalEncryptionKeyProvider
    {
        public ValueTask<byte[]> GetKeyAsync(LocalStorePurpose purpose, CancellationToken cancellationToken = default) =>
            ValueTask.FromResult((purpose == LocalStorePurpose.WriteOutbox ? outboxKey : cacheKey).ToArray());
    }

    private sealed class MutableTimeProvider(DateTimeOffset initial) : TimeProvider
    {
        private DateTimeOffset _current = initial;
        public override DateTimeOffset GetUtcNow() => _current;
        public void Advance(TimeSpan duration) => _current += duration;
    }
}
