using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using TransportERP.Api.Sync;
using TransportERP.Contracts.Identity;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

[Collection("PostgreSql")]
public sealed class ApiAuthenticationAndAuditTests
{
    private const string Issuer = "TransportERP.Test.Identity";
    private const string Audience = "TransportERP.Test.Api";
    private const string SigningKey = "transport-erp-test-signing-key-2026-minimum-32";

    [Fact]
    [Trait("Category", "HTTP")]
    public async Task Sync_batch_requires_a_valid_bearer_token()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();

        await using var db = CreateDb(connection);
        await db.Database.MigrateAsync();
        using var factory = CreateFactory(connection);
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/v1/sync/operations:batch",
            new
            {
                DeviceId = "untrusted-device",
                ProtocolVersion = "P1",
                Operations = Array.Empty<object>()
            });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    [Trait("Category", "HTTP")]
    public async Task Sync_batch_stays_hard_disabled_for_a_stage4_shaped_authenticated_request()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();

        await using var db = CreateDb(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedScopeAsync(db, "HTTPB");
        using var factory = CreateFactory(connection);
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateToken(
            scope.UserId, scope.CompanyId, scope.BranchId, scope.DeviceId,
            "sync.operations.execute"));
        var correlationId = Guid.NewGuid();
        client.DefaultRequestHeaders.Add("X-Correlation-Id", correlationId.ToString());
        client.DefaultRequestHeaders.Add("DPoP", "not-evaluated-while-offline-is-disabled");

        const string payload = "{\"http\":true}";
        var response = await client.PostAsJsonAsync("/api/v1/sync/operations:batch", new
        {
            DeviceId = scope.DeviceId,
            ProtocolVersion = "sync-v1",
            Operations = new[]
            {
                new
                {
                    OperationType = "UPDATE",
                    ActionCode = "UpdateWaybillDraft",
                    EntityType = "Waybill",
                    EntityId = Guid.NewGuid().ToString(),
                    ClientOperationId = $"http-{Guid.NewGuid():N}",
                    OperationCorrelationId = Guid.NewGuid(),
                    PayloadJson = payload,
                    PayloadHash = Sha256(payload),
                    ClientOccurredAt = "2026-08-26T00:00:00.123456Z",
                    BaseVersion = 1L
                }
            }
        });

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        var error = await response.Content.ReadFromJsonAsync<ApiError>();
        Assert.Equal("OFFLINE_DISABLED", error!.ErrorCode);
        Assert.Equal(correlationId, error.CorrelationId);
        Assert.Empty(await db.SyncOperations.Where(x => x.DeviceId == scope.DeviceId).ToListAsync());
        Assert.False(await db.AuditEvents.AnyAsync(x => x.CorrelationId == correlationId));
    }

    [Fact]
    [Trait("Category", "HTTP")]
    public async Task Sync_runtime_gate_can_only_be_opened_by_an_isolated_test_host_override()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = CreateDb(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedScopeAsync(db, "HTTP-GATE");
        using var factory = CreateFactory(connection, openSyncGateForTest: true);
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateToken(
            scope.UserId, scope.CompanyId, scope.BranchId, scope.DeviceId, "sync.operations.execute"));
        client.DefaultRequestHeaders.Add("X-Correlation-Id", Guid.NewGuid().ToString("D"));

        var response = await client.PostAsJsonAsync("/api/v1/sync/operations:batch", new
        {
            DeviceId = scope.DeviceId, ProtocolVersion = "sync-v1", Operations = Array.Empty<object>()
        });

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        var error = await response.Content.ReadFromJsonAsync<ApiError>();
        Assert.Equal("DEVICE_NOT_REGISTERED", error!.ErrorCode);
    }

    [Fact]
    [Trait("Category", "HTTP")]
    public async Task Isolated_gate_open_host_runs_nonce_proof_claim_and_camel_case_envelope_flow()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = CreateDb(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedScopeAsync(db, "HTTP-POP");
        using var proofKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var binding = await AttachProofDeviceAsync(db, scope, proofKey);
        var bearer = CreateToken(scope.UserId, scope.CompanyId, scope.BranchId, scope.DeviceId,
            "sync.operations.execute", registeredDeviceId: binding.RegisteredDeviceId,
            deviceCredentialVersion: binding.CredentialVersion);
        using var factory = CreateFactory(connection, openSyncGateForTest: true);
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://sync.example.test")
        });
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);
        const string payload = "{\"amount\":10}";
        var body = JsonSerializer.Serialize(new
        {
            deviceId = scope.DeviceId,
            protocolVersion = "sync-v1",
            operations = new[]
            {
                new
                {
                    actionCode = "CreateJournalEntry", operationType = "CREATE", entityType = "JournalEntry",
                    entityId = (Guid?)null, clientOperationId = "http-pop-" + Guid.NewGuid().ToString("N"),
                    payloadJson = payload, payloadHash = Sha256(payload),
                    clientOccurredAt = "2026-08-26T00:00:00.123456Z",
                    operationCorrelationId = Guid.NewGuid(), baseVersion = (long?)null
                }
            }
        });

        using (var multipleProofRequest = JsonRequest(body, Guid.NewGuid()))
        {
            multipleProofRequest.Headers.TryAddWithoutValidation("DPoP", new[] { "proof-one", "proof-two" });
            var multipleProofResponse = await client.SendAsync(multipleProofRequest);
            Assert.Equal(HttpStatusCode.Unauthorized, multipleProofResponse.StatusCode);
            Assert.Equal("invalid_dpop_proof",
                (await multipleProofResponse.Content.ReadFromJsonAsync<ApiError>())!.ErrorCode);
        }

        using (var exactBodyRequest = ByteRequest(new byte[SyncApiModule.MaximumRequestBodyBytes], Guid.NewGuid()))
        {
            var exactBodyResponse = await client.SendAsync(exactBodyRequest);
            Assert.Equal(HttpStatusCode.Unauthorized, exactBodyResponse.StatusCode);
            Assert.Equal("use_dpop_nonce", (await exactBodyResponse.Content.ReadFromJsonAsync<ApiError>())!.ErrorCode);
        }
        using (var oversizedBodyRequest = ByteRequest(new byte[SyncApiModule.MaximumRequestBodyBytes + 1], Guid.NewGuid()))
        {
            var oversizedBodyResponse = await client.SendAsync(oversizedBodyRequest);
            Assert.Equal(HttpStatusCode.RequestEntityTooLarge, oversizedBodyResponse.StatusCode);
        }
        using (var chunkedBodyRequest = ByteRequest(
                   new byte[SyncApiModule.MaximumRequestBodyBytes + 1], Guid.NewGuid(), chunked: true))
        {
            var chunkedBodyResponse = await client.SendAsync(chunkedBodyRequest);
            Assert.Equal(HttpStatusCode.RequestEntityTooLarge, chunkedBodyResponse.StatusCode);
        }
        using (var encodedRequest = ByteRequest(Encoding.UTF8.GetBytes(body), Guid.NewGuid(), contentEncoding: "gzip"))
        {
            var encodedResponse = await client.SendAsync(encodedRequest);
            Assert.Equal(HttpStatusCode.UnsupportedMediaType, encodedResponse.StatusCode);
        }

        var nonceCorrelation = Guid.NewGuid();
        using var nonceRequest = JsonRequest(body, nonceCorrelation);
        var nonceResponse = await client.SendAsync(nonceRequest);
        Assert.Equal(HttpStatusCode.Unauthorized, nonceResponse.StatusCode);
        Assert.True(nonceResponse.Headers.TryGetValues("DPoP-Nonce", out var nonceValues));
        var nonce = Assert.Single(nonceValues);

        var attemptCorrelationId = Guid.NewGuid();
        var proof = CreateSyncProof(proofKey, Encoding.UTF8.GetBytes(body), bearer, nonce,
            attemptCorrelationId, DateTimeOffset.UtcNow);
        using var claimedRequest = JsonRequest(body, attemptCorrelationId, proof.CompactProof);
        var claimedResponse = await client.SendAsync(claimedRequest);

        Assert.Equal(HttpStatusCode.OK, claimedResponse.StatusCode);
        using var responseJson = JsonDocument.Parse(await claimedResponse.Content.ReadAsStringAsync());
        Assert.Equal(attemptCorrelationId,
            responseJson.RootElement.GetProperty("attemptCorrelationId").GetGuid());
        var result = Assert.Single(responseJson.RootElement.GetProperty("results").EnumerateArray());
        // Item-level permission is evaluated before runtime availability so an unauthorized
        // caller cannot use the batch response as an action-availability oracle.
        Assert.Equal("SCOPE_DENIED", result.GetProperty("errorCode").GetString());
        var replayRows = await db.SyncProofReplays.Where(x => x.RegisteredDeviceId == binding.RegisteredDeviceId)
            .AsNoTracking().ToListAsync();
        Assert.Single(replayRows.Where(x => x.AttemptCorrelationId == attemptCorrelationId));
        Assert.False(await db.SyncOperations.AnyAsync(x => x.RegisteredDeviceId == binding.RegisteredDeviceId));
        var auditRows = await db.AuditEvents.Where(x => x.CompanyId == scope.CompanyId).AsNoTracking().ToListAsync();
        var nonceRows = await db.SyncProofNonces.Where(x => x.RegisteredDeviceId == binding.RegisteredDeviceId)
            .AsNoTracking().ToListAsync();
        var persistedSecurityState = JsonSerializer.Serialize(new { nonceRows, replayRows, auditRows });
        Assert.DoesNotContain(nonce, persistedSecurityState, StringComparison.Ordinal);
        Assert.DoesNotContain(proof.Jti, persistedSecurityState, StringComparison.Ordinal);
        Assert.DoesNotContain(proof.CompactProof, persistedSecurityState, StringComparison.Ordinal);
        Assert.DoesNotContain(bearer, persistedSecurityState, StringComparison.Ordinal);

        var zeroBatch = await SendClaimedAsync(client, proofKey, bearer, nonce,
            BatchBody(scope.DeviceId, 0));
        Assert.Equal(HttpStatusCode.BadRequest, zeroBatch.StatusCode);
        var hundredBatch = await SendClaimedAsync(client, proofKey, bearer, nonce,
            BatchBody(scope.DeviceId, 100));
        Assert.Equal(HttpStatusCode.OK, hundredBatch.StatusCode);
        using (var hundredJson = JsonDocument.Parse(await hundredBatch.Content.ReadAsStringAsync()))
            Assert.Equal(100, hundredJson.RootElement.GetProperty("results").GetArrayLength());
        var tooManyBatch = await SendClaimedAsync(client, proofKey, bearer, nonce,
            BatchBody(scope.DeviceId, 101));
        Assert.Equal(HttpStatusCode.BadRequest, tooManyBatch.StatusCode);

        var exactPayload = JsonPayload(SyncApiModule.MaximumPayloadBytes);
        var exactPayloadResponse = await SendClaimedAsync(client, proofKey, bearer, nonce,
            BatchBody(scope.DeviceId, 1, exactPayload));
        Assert.Equal("ACTION_RUNTIME_UNAVAILABLE", await FirstItemErrorAsync(exactPayloadResponse));
        var oversizedPayloadResponse = await SendClaimedAsync(client, proofKey, bearer, nonce,
            BatchBody(scope.DeviceId, 1, JsonPayload(SyncApiModule.MaximumPayloadBytes + 1)));
        Assert.Equal("PAYLOAD_TOO_LARGE", await FirstItemErrorAsync(oversizedPayloadResponse));
        var depth32Response = await SendClaimedAsync(client, proofKey, bearer, nonce,
            BatchBody(scope.DeviceId, 1, NestedPayload(32)));
        Assert.Equal("ACTION_RUNTIME_UNAVAILABLE", await FirstItemErrorAsync(depth32Response));
        var depth33Response = await SendClaimedAsync(client, proofKey, bearer, nonce,
            BatchBody(scope.DeviceId, 1, NestedPayload(33)));
        Assert.Equal("PAYLOAD_INVALID", await FirstItemErrorAsync(depth33Response));
    }

    [Fact]
    [Trait("Category", "HTTP")]
    public async Task Audit_read_requires_permission_and_cannot_escape_company_scope()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();

        await using var db = CreateDb(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedScopeAsync(db, "HTTPA");
        var other = await SeedScopeAsync(db, "HTTPZ");
        var audit = new AuditEventService(db);
        await audit.AppendAuditEventAsync(new AuditEventDraft(
            "TestAudit", "SUCCESS", "TestEntity", Guid.NewGuid(), scope.UserId,
            scope.CompanyId, scope.BranchId, Guid.NewGuid(), scope.DeviceId));
        await audit.AppendAuditEventAsync(new AuditEventDraft(
            "OtherAudit", "SUCCESS", "TestEntity", Guid.NewGuid(), other.UserId,
            other.CompanyId, other.BranchId, Guid.NewGuid(), other.DeviceId));

        using var factory = CreateFactory(connection);
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateToken(
            scope.UserId, scope.CompanyId, scope.BranchId, scope.DeviceId,
            "audit.events.read"));

        var denied = await client.GetAsync($"/api/v1/audit/events?companyId={other.CompanyId}");
        Assert.Equal(HttpStatusCode.Forbidden, denied.StatusCode);

        var allowed = await client.GetAsync($"/api/v1/audit/events?companyId={scope.CompanyId}&take=100");
        Assert.Equal(HttpStatusCode.OK, allowed.StatusCode);
        var body = await allowed.Content.ReadFromJsonAsync<PagedAuditEventResponse>();
        Assert.NotNull(body);
        Assert.NotEmpty(body!.Items);
        Assert.All(body.Items, item => Assert.Equal(scope.CompanyId, item.CompanyId));
        Assert.Contains(body.Items, item => item.Action == "TestAudit");
    }

    [Fact]
    [Trait("Category", "HTTP")]
    public async Task Invalid_issuer_token_is_rejected_by_jwt_bearer_provider()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();

        await using var db = CreateDb(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedScopeAsync(db, "HTTPX");
        using var factory = CreateFactory(connection);
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateToken(
            scope.UserId, scope.CompanyId, scope.BranchId, scope.DeviceId,
            "sync.operations.execute", issuer: "untrusted-issuer"));

        var response = await client.GetAsync("/api/v1/audit/events");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    [Trait("Category", "HTTP")]
    public async Task Login_rate_limit_is_generic_and_supplies_retry_after_without_partition_data()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = CreateDb(connection);
        await db.Database.MigrateAsync();
        using var factory = CreateFactory(connection, loginRateLimit: 1);
        using var client = factory.CreateClient();

        var first = await client.PostAsJsonAsync("/api/v1/auth/sessions",
            new CreateIdentitySessionRequest("missing-a", "wrong-password", null, null, "device-a"));
        Assert.Equal(HttpStatusCode.Unauthorized, first.StatusCode);

        var second = await client.PostAsJsonAsync("/api/v1/auth/sessions",
            new CreateIdentitySessionRequest("missing-b", "wrong-password", null, null, "device-b"));
        Assert.Equal((HttpStatusCode)429, second.StatusCode);
        Assert.True(second.Headers.TryGetValues("Retry-After", out var values));
        Assert.True(int.TryParse(values.Single(), out var seconds) && seconds > 0);
        var body = await second.Content.ReadAsStringAsync();
        Assert.Contains("RATE_LIMITED", body, StringComparison.Ordinal);
        Assert.DoesNotContain("missing", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("device", body, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    [Trait("Category", "HTTP")]
    public async Task Device_api_enforces_company_permissions_keeps_correlation_and_never_returns_secret()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = CreateDb(connection);
        await db.Database.MigrateAsync();
        var allowed = await SeedScopeAsync(db, "DEVHTTP", devicePermissions: true);
        var denied = await SeedScopeAsync(db, "DEVNO");
        using var factory = CreateFactory(connection);
        using var client = factory.CreateClient();
        var secret = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        var request = new RegisterDeviceRequest("http-terminal", "HTTP terminal", "WEB", "1.0", null, null,
            "http-request", secret);

        Assert.Equal(HttpStatusCode.Unauthorized,
            (await client.PostAsJsonAsync("/api/v1/devices", request)).StatusCode);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateToken(
            denied.UserId, denied.CompanyId, denied.BranchId, denied.DeviceId, "devices.register"));
        Assert.Equal(HttpStatusCode.Forbidden,
            (await client.PostAsJsonAsync("/api/v1/devices", request)).StatusCode);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateToken(
            allowed.UserId, allowed.CompanyId, allowed.BranchId, allowed.DeviceId, "devices.register"));
        var correlation = Guid.NewGuid();
        client.DefaultRequestHeaders.Remove("X-Correlation-Id");
        client.DefaultRequestHeaders.Add("X-Correlation-Id", correlation.ToString());
        var created = await client.PostAsJsonAsync("/api/v1/devices", request);
        Assert.Equal(HttpStatusCode.OK, created.StatusCode);
        var createdJson = await created.Content.ReadAsStringAsync();
        Assert.DoesNotContain(secret, createdJson, StringComparison.Ordinal);
        Assert.DoesNotContain("CredentialHash", createdJson, StringComparison.OrdinalIgnoreCase);
        Assert.True(await db.AuditEvents.AnyAsync(x => x.Action == "RegisteredDeviceCreated" && x.CorrelationId == correlation));

        var conflictCorrelation = Guid.NewGuid();
        client.DefaultRequestHeaders.Remove("X-Correlation-Id");
        client.DefaultRequestHeaders.Add("X-Correlation-Id", conflictCorrelation.ToString());
        var conflict = await client.PostAsJsonAsync("/api/v1/devices", request with { DisplayName = "Changed" });
        Assert.Equal(HttpStatusCode.Conflict, conflict.StatusCode);
        var error = await conflict.Content.ReadFromJsonAsync<ApiError>();
        Assert.Equal(conflictCorrelation, error!.CorrelationId);
        Assert.Equal("DEVICE_REGISTRATION_CONFLICT", error.ErrorCode);

        var list = await client.GetFromJsonAsync<RegisteredDeviceResponse[]>("/api/v1/devices");
        Assert.Single(list!);
        var current = await client.GetAsync("/api/v1/devices/current");
        Assert.Equal(HttpStatusCode.Forbidden, current.StatusCode);
    }

    private static WebApplicationFactory<Program> CreateFactory(string connection, int? loginRateLimit = null,
        bool openSyncGateForTest = false)
        => new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseSetting("ConnectionStrings:TransportErp", connection);
            builder.UseSetting("Auth:Issuer", Issuer);
            builder.UseSetting("Auth:Audience", Audience);
            builder.UseSetting("Auth:SigningKey", SigningKey);
            builder.UseSetting("Auth:SigningKeyId", "test-current");
            builder.UseSetting("Sync:Proof:PublicOrigin", "https://sync.example.test");
            builder.UseSetting("Sync:Proof:MaximumPastSeconds", "120");
            builder.UseSetting("Sync:Proof:MaximumFutureSeconds", "30");
            builder.UseSetting("Sync:Proof:NonceLifetimeSeconds", "300");
            builder.UseSetting("Sync:Proof:ReplayRetentionSeconds", "600");
            builder.UseSetting("Sync:Proof:MaximumRequestBodyBytes", "2097152");
            builder.UseSetting("Sync:Proof:MaximumPayloadBytes", "16384");
            if (loginRateLimit.HasValue)
                builder.UseSetting("Auth:LoginRateLimitPermitCount", loginRateLimit.Value.ToString());
            if (openSyncGateForTest)
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll<ISyncRuntimeGate>();
                    services.AddScoped<ISyncRuntimeGate, OpenSyncRuntimeGateForTest>();
                });
        });

    private sealed class OpenSyncRuntimeGateForTest : ISyncRuntimeGate
    {
        public Task<bool> IsOpenAsync(Guid companyId, CancellationToken cancellationToken)
            => Task.FromResult(true);
    }

    private static string CreateToken(Guid userId, Guid companyId, Guid branchId, string deviceId,
        string permission, string issuer = Issuer, Guid? registeredDeviceId = null,
        int? deviceCredentialVersion = null)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim("company_id", companyId.ToString()),
            new Claim("branch_id", branchId.ToString()),
            new Claim("device_id", deviceId),
            new Claim("sid", userId.ToString()),
            new Claim("security_stamp", userId.ToString("N")),
            new Claim("auth_version", "1")
        };
        if (registeredDeviceId.HasValue && deviceCredentialVersion.HasValue)
        {
            claims.Add(new Claim("registered_device_id", registeredDeviceId.Value.ToString("D")));
            claims.Add(new Claim("device_credential_version", deviceCredentialVersion.Value.ToString()));
        }
        var identity = new ClaimsIdentity(claims);
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = identity,
            Issuer = issuer,
            Audience = Audience,
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey)) { KeyId = "test-current" },
                SecurityAlgorithms.HmacSha256)
        };
        return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityTokenHandler().CreateToken(descriptor));
    }

    private static HttpRequestMessage JsonRequest(string body, Guid correlationId, string? proof = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/sync/operations:batch")
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json")
        };
        request.Headers.Add("X-Correlation-Id", correlationId.ToString("D"));
        if (proof is not null) request.Headers.Add("DPoP", proof);
        return request;
    }

    private static HttpRequestMessage ByteRequest(byte[] body, Guid correlationId,
        bool chunked = false, string? contentEncoding = null)
    {
        var content = new ByteArrayContent(body);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        if (contentEncoding is not null) content.Headers.ContentEncoding.Add(contentEncoding);
        if (chunked) content.Headers.ContentLength = null;
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/sync/operations:batch") { Content = content };
        request.Headers.Add("X-Correlation-Id", correlationId.ToString("D"));
        if (chunked) request.Headers.TransferEncodingChunked = true;
        return request;
    }

    private static async Task<HttpResponseMessage> SendClaimedAsync(HttpClient client, ECDsa key,
        string bearer, string nonce, string body)
    {
        var correlationId = Guid.NewGuid();
        var proof = CreateSyncProof(key, Encoding.UTF8.GetBytes(body), bearer, nonce,
            correlationId, DateTimeOffset.UtcNow);
        using var request = JsonRequest(body, correlationId, proof.CompactProof);
        return await client.SendAsync(request);
    }

    private static string BatchBody(string deviceId, int count, string payloadJson = "{}")
        => JsonSerializer.Serialize(new
        {
            deviceId,
            protocolVersion = "sync-v1",
            operations = Enumerable.Range(0, count).Select(_ => new
            {
                actionCode = "CreateJournalEntry", operationType = "CREATE", entityType = "JournalEntry",
                entityId = (Guid?)null, clientOperationId = "http-batch-" + Guid.NewGuid().ToString("N"),
                payloadJson, payloadHash = Sha256(payloadJson),
                clientOccurredAt = "2026-08-26T00:00:00.123456Z",
                operationCorrelationId = Guid.NewGuid(), baseVersion = (long?)null
            }).ToArray()
        });

    private static string JsonPayload(int utf8Length)
    {
        if (utf8Length < 8) throw new ArgumentOutOfRangeException(nameof(utf8Length));
        return "{\"x\":\"" + new string('a', utf8Length - 8) + "\"}";
    }

    private static string NestedPayload(int depth)
        => new string('[', depth) + "0" + new string(']', depth);

    private static async Task<string?> FirstItemErrorAsync(HttpResponseMessage response)
    {
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return document.RootElement.GetProperty("results")[0].GetProperty("errorCode").GetString();
    }

    private static async Task<ProofDeviceBinding> AttachProofDeviceAsync(
        TransportErpDbContext db, TestScope scope, ECDsa key)
    {
        var now = DateTimeOffset.UtcNow;
        var parameters = key.ExportParameters(false);
        var x = Base64Url(parameters.Q.X!);
        var y = Base64Url(parameters.Q.Y!);
        var canonicalJwk = $"{{\"crv\":\"P-256\",\"kty\":\"EC\",\"x\":\"{x}\",\"y\":\"{y}\"}}";
        var thumbprint = Base64Url(SHA256.HashData(Encoding.ASCII.GetBytes(canonicalJwk)));
        var device = new RegisteredDevice
        {
            Id = Guid.NewGuid(), CompanyId = scope.CompanyId, DeviceId = scope.DeviceId,
            DisplayName = "جهاز HTTP PoP", Platform = "TEST", AppVersion = "1.0",
            RegistrationRequestId = "http-pop-" + Guid.NewGuid().ToString("N"),
            CredentialHash = new string('a', 64), CredentialVersion = 1, Status = "ACTIVE",
            RegisteredByUserId = scope.UserId, ApprovedByUserId = scope.UserId, ApprovedAt = now, LastSeenAt = now,
            ProofPublicJwkCanonicalJson = canonicalJwk, ProofKeyThumbprint = thumbprint, ProofKeyVersion = 1,
            ProofKeyChangedAt = now, ProofKeyChangedByUserId = scope.UserId,
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        db.RegisteredDevices.Add(device);
        db.RegisteredDeviceAssignments.Add(new RegisteredDeviceAssignment
        {
            Id = Guid.NewGuid(), RegisteredDeviceId = device.Id, UserId = scope.UserId,
            CompanyId = scope.CompanyId, BranchId = scope.BranchId, Status = "ACTIVE",
            AssignedByUserId = scope.UserId, AssignedAt = now, CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        });
        var session = await db.AuthSessions.SingleAsync(x => x.Id == scope.UserId);
        session.RegisteredDeviceId = device.Id;
        session.DeviceCredentialVersion = device.CredentialVersion;
        session.UpdatedAt = now;
        session.RowVersion = RandomNumberGenerator.GetBytes(16);
        await db.SaveChangesAsync();
        return new ProofDeviceBinding(device.Id, device.CredentialVersion);
    }

    private static SyncProofFixture CreateSyncProof(ECDsa key, byte[] body, string bearer, string nonce,
        Guid correlationId, DateTimeOffset issuedAt)
    {
        var parameters = key.ExportParameters(false);
        var header = JsonSerializer.Serialize(new
        {
            typ = "dpop+jwt", alg = "ES256",
            jwk = new { kty = "EC", crv = "P-256", x = Base64Url(parameters.Q.X!), y = Base64Url(parameters.Q.Y!) }
        }, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
        var jti = Guid.NewGuid().ToString("D");
        var payload = JsonSerializer.Serialize(new
        {
            jti, htm = "POST",
            htu = "https://sync.example.test/api/v1/sync/operations:batch",
            iat = issuedAt.ToUnixTimeSeconds(),
            ath = Base64Url(SHA256.HashData(Encoding.ASCII.GetBytes(bearer))), nonce,
            tbh = Base64Url(SHA256.HashData(body)), cid = correlationId.ToString("D")
        });
        var protectedSegment = Base64Url(Encoding.UTF8.GetBytes(header));
        var payloadSegment = Base64Url(Encoding.UTF8.GetBytes(payload));
        var signingInput = Encoding.ASCII.GetBytes(protectedSegment + "." + payloadSegment);
        var signature = key.SignData(signingInput, HashAlgorithmName.SHA256,
            DSASignatureFormat.IeeeP1363FixedFieldConcatenation);
        return new SyncProofFixture(
            protectedSegment + "." + payloadSegment + "." + Base64Url(signature), jti);
    }

    private static string Base64Url(byte[] value)
        => Convert.ToBase64String(value).TrimEnd('=').Replace('+', '-').Replace('/', '_');

    private static TransportErpDbContext CreateDb(string connection)
        => PostgreSqlTestEnvironment.CreateDbContext(connection);

    private static string Sha256(string payload)
        => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(payload))).ToLowerInvariant();

        private static async Task<TestScope> SeedScopeAsync(TransportErpDbContext db, string suffix,
            bool devicePermissions = false)
    {
        for (var attempt = 0; attempt < 8; attempt++)
        {
            var now = DateTimeOffset.UtcNow;
            var currency = new Currency
            {
                Id = Guid.NewGuid(), Code = await PostgreSqlTestCurrencyCodeAllocator.NextAsync(db),
                NameAr = "عملة اختبار HTTP", MinorUnit = 2, IsBase = true, CreatedAt = now, UpdatedAt = now,
                RowVersion = Guid.NewGuid().ToByteArray()
            };
            var company = new Company
            {
                Id = Guid.NewGuid(),
                Code = $"H-{suffix}-{Guid.NewGuid():N}"[..18], LegalNameAr = "شركة اختبار HTTP",
                BaseCurrencyId = currency.Id, DefaultCalendarId = Guid.NewGuid(), Status = "ACTIVE",
                CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
            };
            var branch = new Branch
            {
                Id = Guid.NewGuid(), CompanyId = company.Id, Code = "MAIN", NameAr = "الفرع الرئيسي",
                Timezone = "Asia/Aden", Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
                RowVersion = Guid.NewGuid().ToByteArray()
            };
            var deviceId = $"http-device-{suffix}-{Guid.NewGuid():N}";
            var user = new User
            {
                Id = Guid.NewGuid(), UserName = $"http-{Guid.NewGuid():N}", NormalizedUserName = $"HTTP{suffix}",
                DisplayName = "مستخدم اختبار HTTP", PasswordHash = "test-only", SecurityStamp = Guid.Empty.ToString("N"), AuthVersion = 1, Status = "ACTIVE",
                CompanyId = company.Id, BranchId = branch.Id, CreatedAt = now, UpdatedAt = now,
                RowVersion = Guid.NewGuid().ToByteArray()
            };
            db.Currencies.Add(currency);
            db.Companies.Add(company);
            db.Branches.Add(branch);
            db.Users.Add(user);
            user.SecurityStamp = user.Id.ToString("N");
            db.AuthSessions.Add(new AuthSession
            {
                Id = user.Id, UserId = user.Id, CompanyId = company.Id, BranchId = branch.Id, DeviceId = deviceId,
                Mode = "LOCAL", SecurityStampAtIssue = user.SecurityStamp, AuthVersionAtIssue = user.AuthVersion,
                RefreshTokenHash = Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(user.Id.ToByteArray())).ToLowerInvariant(),
                RefreshTokenFamilyId = Guid.NewGuid(), IssuedAt = now, AccessTokenExpiresAt = now.AddMinutes(10),
                RefreshTokenExpiresAt = now.AddDays(1), CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
            });
            var permissionCodes = devicePermissions
                ? new[] { "sync.operations.execute", "audit.events.read", "devices.register", "devices.read", "devices.manage" }
                : new[] { "sync.operations.execute", "audit.events.read" };
            var role = new Role { Id = Guid.NewGuid(), Code = $"HTTP-{suffix}-{Guid.NewGuid():N}", NameAr = "دور اختبار",
                CompanyId = company.Id, Status = "ACTIVE", CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray() };
            db.Roles.Add(role);
            db.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id, CompanyId = company.Id, BranchId = branch.Id,
                CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray() });
            foreach (var code in permissionCodes)
            {
                var permissionEntity = await db.Permissions.SingleOrDefaultAsync(x => x.Code == code) ?? new Permission
                {
                    Id = Guid.NewGuid(), Code = code, NameAr = code, Resource = code.Split('.')[0], Action = code.Split('.')[^1],
                    ScopeType = "BRANCH", Status = "ACTIVE", CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
                };
                if (db.Entry(permissionEntity).State == EntityState.Detached) db.Permissions.Add(permissionEntity);
                var permissionScope = code.StartsWith("devices.", StringComparison.Ordinal) ? "COMPANY" : "BRANCH";
                db.RolePermissions.Add(new RolePermission { RoleId = role.Id, PermissionId = permissionEntity.Id,
                    ScopeType = permissionScope, CompanyId = company.Id,
                    BranchId = permissionScope == "BRANCH" ? branch.Id : null, CreatedAt = now, UpdatedAt = now,
                    RowVersion = Guid.NewGuid().ToByteArray() });
            }
            try
            {
                await db.SaveChangesAsync();
                return new TestScope(company.Id, branch.Id, user.Id, deviceId);
            }
            catch (Exception ex) when (IsUniqueViolation(ex) && attempt < 7)
            {
                db.ChangeTracker.Clear();
                await Task.Delay(10 * (attempt + 1));
            }
        }

        throw new InvalidOperationException("Unable to seed a unique HTTP test scope after retries.");
    }

    private static bool IsUniqueViolation(Exception exception)
    {
        for (var current = exception; current is not null; current = current.InnerException)
        {
            if (current is Npgsql.PostgresException { SqlState: "23505" })
                return true;
        }

        return false;
    }

    private sealed record TestScope(Guid CompanyId, Guid BranchId, Guid UserId, string DeviceId);
    private sealed record ProofDeviceBinding(Guid RegisteredDeviceId, int CredentialVersion);
    private sealed record SyncProofFixture(string CompactProof, string Jti);
    private sealed record ApiError(string ErrorCode, Guid CorrelationId);
}
