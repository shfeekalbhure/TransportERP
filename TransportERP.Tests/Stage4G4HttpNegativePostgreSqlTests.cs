using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using TransportERP.Api.Security;
using TransportERP.Api.Sync;
using TransportERP.Application.Sync;
using TransportERP.Contracts.Attachments;
using TransportERP.Contracts.Geo;
using TransportERP.Contracts.Waybills;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

/// <summary>
/// Exact HTTP/PostgreSQL negative evidence for the remaining G4 sync-v1 acceptance cases.
/// The production Offline and server worker switches remain closed; this isolated TestServer
/// opens only the request gate and drives the real processor deterministically.
/// </summary>
[Collection("PostgreSql")]
public sealed class Stage4G4HttpNegativePostgreSqlTests
{
    private const string Issuer = "TransportERP.Stage4.G4.HttpNegative";
    private const string Audience = "TransportERP.Stage4.G4.HttpNegative.Api";
    private const string SigningKey = "transport-erp-stage4-g4-http-negative-signing-key-minimum-32";
    private static readonly Uri PublicOrigin = new("https://sync.example.test");
    private const string BatchPath = "/api/v1/sync/operations:batch";
    private const string BatchHtu = "https://sync.example.test/api/v1/sync/operations:batch";

    [Fact]
    [Trait("Category", "PostgreSQL")]
    [Trait("Category", "HTTP")]
    [Trait("Acceptance", "T-SYNC-003")]
    public async Task T_SYNC_003_signed_request_with_mismatched_payload_hash_is_rejected_audited_and_has_no_business_effect()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var seedDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await seedDb.Database.MigrateAsync();
        using var proofKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var scope = await SeedAsync(seedDb, proofKey, "HASH");
        using var factory = CreateFactory(connection);
        using var client = CreateClient(factory, scope.Bearer);
        var operationCorrelationId = Guid.NewGuid();
        var clientOperationId = $"t-sync-003-{Guid.NewGuid():N}";
        var payload = PartyPayload(clientOperationId, "T-SYNC-003");
        var body = BatchBody(scope.DeviceId,
            Operation("CreateOperationalParty", "CREATE", "OperationalParty", null,
                clientOperationId, operationCorrelationId, payload, new string('0', 64), null));

        using var response = await SendSignedAsync(client, proofKey, scope.Bearer, body);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await SingleResultAsync(response);
        Assert.Equal("REJECTED", result.Status);
        Assert.Equal("HASH_MISMATCH", result.ErrorCode);
        Assert.Null(result.ServerOperationId);

        await using var verify = PostgreSqlTestEnvironment.CreateDbContext(connection);
        Assert.False(await verify.SyncOperations.AsNoTracking().AnyAsync(x =>
            x.CompanyId == scope.CompanyId && x.ClientOperationId == clientOperationId));
        Assert.False(await verify.Set<OperationalPartyEntity>().AsNoTracking().AnyAsync(x =>
            x.CompanyId == scope.CompanyId && x.ClientOperationId == clientOperationId));

        // The acceptance contract requires a metadata-only rejection AuditEvent, not merely the
        // successful proof-claim event.
        var rejectionAudits = await verify.AuditEvents.AsNoTracking().Where(x =>
            x.CompanyId == scope.CompanyId &&
            x.OperationCorrelationId == operationCorrelationId &&
            x.Outcome == "REJECTED").ToListAsync();
        Assert.Contains(rejectionAudits, x =>
            (x.Reason ?? string.Empty).Contains("HASH_MISMATCH", StringComparison.Ordinal));
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    [Trait("Category", "HTTP")]
    [Trait("Acceptance", "T-SYNC-004")]
    public async Task T_SYNC_004_cross_company_cross_branch_and_missing_party_references_are_indistinguishable_and_replay_safe()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var seedDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await seedDb.Database.MigrateAsync();
        using var proofKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var scope = await SeedAsync(seedDb, proofKey, "SCOPE");
        var targets = await SeedCrossScopeTargetsAsync(seedDb, scope);
        using var factory = CreateFactory(connection);
        using var client = CreateClient(factory, scope.Bearer);

        var companyOperation = UpdateOperation(scope, targets, targets.ForeignCompanyPartyId, "COMPANY");
        var branchOperation = UpdateOperation(scope, targets, targets.ForeignBranchPartyId, "BRANCH");
        var missingOperation = UpdateOperation(scope, targets, Guid.NewGuid(), "MISSING");
        var body = BatchBody(scope.DeviceId, companyOperation, branchOperation, missingOperation);

        using (var accepted = await SendSignedAsync(client, proofKey, scope.Bearer, body))
        {
            Assert.Equal(HttpStatusCode.OK, accepted.StatusCode);
            var results = await ResultsAsync(accepted);
            Assert.Equal(3, results.Count);
            Assert.All(results, result =>
            {
                Assert.Equal("QUEUED", result.Status);
                Assert.Null(result.ErrorCode);
                Assert.NotNull(result.ServerOperationId);
            });
        }

        await ExecuteUntilTerminalAsync(factory, connection,
            [companyOperation.ClientOperationId, branchOperation.ClientOperationId, missingOperation.ClientOperationId]);

        // Two independently composed hosts replay the same governed business identities against
        // one PostgreSQL store. Per-device advisory locking must converge without a duplicate
        // operation or business-transition audit. A separately imported signer avoids sharing ECDSA state.
        using var secondFactory = CreateFactory(connection);
        using var secondClient = CreateClient(secondFactory, scope.Bearer);
        using var secondProofKey = ECDsa.Create(proofKey.ExportParameters(true));
        var replayResponses = await Task.WhenAll(
            SendSignedAsync(client, proofKey, scope.Bearer, body),
            SendSignedAsync(secondClient, secondProofKey, scope.Bearer, body));
        var replayTexts = new List<string>();
        foreach (var replay in replayResponses)
        {
            using (replay)
            {
                Assert.Equal(HttpStatusCode.OK, replay.StatusCode);
                var replayText = await replay.Content.ReadAsStringAsync();
                replayTexts.Add(replayText);
                var replayResults = JsonSerializer.Deserialize<SyncBatchResponse>(replayText,
                    new JsonSerializerOptions(JsonSerializerDefaults.Web))!.Results;
                Assert.Equal(3, replayResults.Count);
                Assert.All(replayResults, result =>
                {
                    Assert.Equal("REJECTED", result.Status);
                    Assert.Equal("SCOPE_DENIED", result.ErrorCode);
                });
            }
        }
        var replayWire = string.Join("\n", replayTexts);
        Assert.DoesNotContain(targets.ForeignCompanyPartyId.ToString("D"), replayWire, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain(targets.ForeignBranchPartyId.ToString("D"), replayWire, StringComparison.OrdinalIgnoreCase);

        await using var verify = PostgreSqlTestEnvironment.CreateDbContext(connection);
        var operations = await verify.SyncOperations.AsNoTracking().Where(x =>
            x.CompanyId == scope.CompanyId &&
            new[] { companyOperation.ClientOperationId, branchOperation.ClientOperationId, missingOperation.ClientOperationId }
                .Contains(x.ClientOperationId)).ToListAsync();
        Assert.Equal(3, operations.Count);
        Assert.All(operations, operation =>
        {
            Assert.Equal("REJECTED", operation.Status);
            Assert.Equal("SCOPE_DENIED", operation.ErrorCode);
            Assert.Equal(0, operation.RetryCount);
            Assert.Null(operation.NextRetryAt);
        });
        var waybill = await verify.Set<WaybillEntity>().AsNoTracking().SingleAsync(x => x.Id == targets.LocalWaybillId);
        Assert.Equal(1, waybill.Version);
        Assert.False(await verify.Set<WaybillPartyEntity>().AsNoTracking()
            .AnyAsync(x => x.WaybillId == targets.LocalWaybillId));

        var operationIds = operations.Select(x => x.Id).ToArray();
        var audits = await verify.AuditEvents.AsNoTracking()
            .Where(x => x.EntityId.HasValue && operationIds.Contains(x.EntityId.Value)).ToListAsync();
        Assert.Equal(3, audits.Count(x => x.Action == "SyncOperationQueued"));
        Assert.Equal(3, audits.Count(x => x.Action == "SyncOperationExecutionRejected"));
        var auditWire = JsonSerializer.Serialize(audits.Select(x => new
        {
            x.Action, x.Outcome, x.Reason, x.CompanyId, x.BranchId, x.OperationCorrelationId
        }));
        Assert.DoesNotContain(targets.ForeignCompanyPartyId.ToString("D"), auditWire, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain(targets.ForeignBranchPartyId.ToString("D"), auditWire, StringComparison.OrdinalIgnoreCase);

        // A replay mutation keeps the original business identity but swaps the target reference.
        // It is rejected and audited as a fingerprint mismatch without creating a fourth operation.
        var mutated = companyOperation with
        {
            PayloadJson = branchOperation.PayloadJson,
            PayloadHash = branchOperation.PayloadHash
        };
        var rejectionAuditCount = await verify.AuditEvents.CountAsync(x =>
            x.CompanyId == scope.CompanyId &&
            x.OperationCorrelationId == companyOperation.OperationCorrelationId &&
            x.Outcome == "REJECTED");
        using var mutationResponse = await SendSignedAsync(
            client, proofKey, scope.Bearer, BatchBody(scope.DeviceId, mutated));
        var mutationResult = await SingleResultAsync(mutationResponse);
        Assert.Equal("IDEMPOTENCY_CONFLICT", mutationResult.ErrorCode);
        var mutationAudits = await verify.AuditEvents.AsNoTracking().Where(x =>
            x.CompanyId == scope.CompanyId &&
            x.OperationCorrelationId == companyOperation.OperationCorrelationId &&
            x.Outcome == "REJECTED").ToListAsync();
        Assert.Equal(rejectionAuditCount + 1, mutationAudits.Count);
        Assert.Contains(mutationAudits, x =>
            (x.Reason ?? string.Empty).Contains("IDEMPOTENCY_CONFLICT", StringComparison.Ordinal));
        Assert.Equal(3, await verify.SyncOperations.CountAsync(x =>
            x.CompanyId == scope.CompanyId &&
            new[] { companyOperation.ClientOperationId, branchOperation.ClientOperationId, missingOperation.ClientOperationId }
                .Contains(x.ClientOperationId)));
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    [Trait("Category", "HTTP")]
    [Trait("Acceptance", "T-SYNC-006")]
    public async Task T_SYNC_006_posting_and_unavailable_accounting_actions_are_rejected_before_enqueue_and_audited()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var seedDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await seedDb.Database.MigrateAsync();
        using var proofKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var scope = await SeedAsync(seedDb, proofKey, "POST");
        using var factory = CreateFactory(connection);
        using var client = CreateClient(factory, scope.Bearer);
        var postCorrelation = Guid.NewGuid();
        var createCorrelation = Guid.NewGuid();
        var postedEntryId = Guid.NewGuid();
        var body = BatchBody(scope.DeviceId,
            Operation("PostJournalEntry", "COMMAND", "JournalEntry", postedEntryId,
                $"post-entry-{Guid.NewGuid():N}", postCorrelation, "{}", Sha256("{}"), null),
            Operation("CreateJournalEntry", "CREATE", "JournalEntry", null,
                $"create-entry-{Guid.NewGuid():N}", createCorrelation, "{}", Sha256("{}"), null));

        using var response = await SendSignedAsync(client, proofKey, scope.Bearer, body);
        var results = await ResultsAsync(response);
        Assert.Equal("ONLINE_REQUIRED", results[0].ErrorCode);
        Assert.Equal("ACTION_RUNTIME_UNAVAILABLE", results[1].ErrorCode);
        Assert.All(results, result =>
        {
            Assert.Equal("REJECTED", result.Status);
            Assert.Null(result.ServerOperationId);
        });

        await using var verify = PostgreSqlTestEnvironment.CreateDbContext(connection);
        Assert.False(await verify.SyncOperations.AsNoTracking().AnyAsync(x =>
            x.OperationCorrelationId == postCorrelation || x.OperationCorrelationId == createCorrelation));
        var rejectionAudits = await verify.AuditEvents.AsNoTracking().Where(x =>
            x.CompanyId == scope.CompanyId && x.Outcome == "REJECTED" &&
            (x.OperationCorrelationId == postCorrelation || x.OperationCorrelationId == createCorrelation))
            .ToListAsync();
        Assert.Contains(rejectionAudits, x =>
            x.OperationCorrelationId == postCorrelation &&
            (x.Reason ?? string.Empty).Contains("ONLINE_REQUIRED", StringComparison.Ordinal));
        Assert.Contains(rejectionAudits, x =>
            x.OperationCorrelationId == createCorrelation &&
            (x.Reason ?? string.Empty).Contains("ACTION_RUNTIME_UNAVAILABLE", StringComparison.Ordinal));
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    [Trait("Category", "HTTP")]
    public async Task Attachment_and_pod_contracts_remain_metadata_only_and_unapproved_binary_never_reaches_sync_storage()
    {
        var contractProperties = typeof(AttachmentDescriptor).GetProperties();
        Assert.DoesNotContain(contractProperties, property =>
            property.PropertyType == typeof(byte[]) ||
            typeof(Stream).IsAssignableFrom(property.PropertyType) ||
            property.Name.Contains("Base64", StringComparison.OrdinalIgnoreCase) ||
            property.Name.Contains("Binary", StringComparison.OrdinalIgnoreCase));

        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var seedDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await seedDb.Database.MigrateAsync();
        using var proofKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var scope = await SeedAsync(seedDb, proofKey, "BINARY");
        using var factory = CreateFactory(connection);
        using var client = CreateClient(factory, scope.Bearer);

        var rawCorrelation = Guid.NewGuid();
        using (var request = new HttpRequestMessage(HttpMethod.Post, BatchPath)
        {
            Content = new ByteArrayContent(RandomNumberGenerator.GetBytes(128))
        })
        {
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            request.Headers.Add("X-Correlation-Id", rawCorrelation.ToString("D"));
            using var rawResponse = await client.SendAsync(request);
            Assert.Equal(HttpStatusCode.UnsupportedMediaType, rawResponse.StatusCode);
        }

        var binary = Convert.ToBase64String(RandomNumberGenerator.GetBytes(96));
        var attachmentCorrelation = Guid.NewGuid();
        var podCorrelation = Guid.NewGuid();
        var attachmentOperation = Operation("AddWaybillAttachment", "CREATE", "Waybill", Guid.NewGuid(),
            $"attachment-binary-{Guid.NewGuid():N}", attachmentCorrelation,
            JsonSerializer.Serialize(new { storageRef = "local://pending", contentHash = Sha256(binary), binaryBase64 = binary }),
            null, null);
        attachmentOperation = attachmentOperation with { PayloadHash = Sha256(attachmentOperation.PayloadJson) };
        var podOperation = Operation("RecordProofOfDelivery", "CREATE", "Delivery", Guid.NewGuid(),
            $"pod-binary-{Guid.NewGuid():N}", podCorrelation,
            JsonSerializer.Serialize(new { storageRef = "local://pending", contentHash = Sha256(binary), signatureBinary = binary }),
            null, null);
        podOperation = podOperation with { PayloadHash = Sha256(podOperation.PayloadJson) };

        using var response = await SendSignedAsync(client, proofKey, scope.Bearer,
            BatchBody(scope.DeviceId, attachmentOperation, podOperation));
        var results = await ResultsAsync(response);
        Assert.All(results, result =>
        {
            Assert.Equal("REJECTED", result.Status);
            Assert.Equal("ACTION_RUNTIME_UNAVAILABLE", result.ErrorCode);
            Assert.Null(result.ServerOperationId);
        });

        await using var verify = PostgreSqlTestEnvironment.CreateDbContext(connection);
        Assert.False(await verify.SyncOperations.AsNoTracking().AnyAsync(x =>
            x.OperationCorrelationId == attachmentCorrelation || x.OperationCorrelationId == podCorrelation));
        var persisted = JsonSerializer.Serialize(new
        {
            Operations = await verify.SyncOperations.AsNoTracking().Where(x => x.CompanyId == scope.CompanyId).ToListAsync(),
            Audits = await verify.AuditEvents.AsNoTracking().Where(x => x.CompanyId == scope.CompanyId).ToListAsync()
        });
        Assert.DoesNotContain(binary, persisted, StringComparison.Ordinal);
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    [Trait("Category", "HTTP")]
    [Trait("Acceptance", "G4-END-TO-END")]
    public async Task Authenticated_sync_activation_returns_only_scoped_policy_and_public_key_binding()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var seedDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await seedDb.Database.MigrateAsync();
        using var proofKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var scope = await SeedAsync(seedDb, proofKey, "ACTIVATION");
        using var factory = CreateFactory(connection);
        using var client = CreateClient(factory, scope.Bearer);

        using var response = await client.GetAsync("/api/v1/sync/activation");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(body);
        var root = document.RootElement;
        Assert.True(root.GetProperty("enabled").GetBoolean());
        Assert.Equal(scope.CompanyId, root.GetProperty("companyId").GetGuid());
        Assert.Equal(scope.BranchId, root.GetProperty("branchId").GetGuid());
        Assert.Equal(scope.UserId, root.GetProperty("userId").GetGuid());
        Assert.Equal(scope.RegisteredDeviceId, root.GetProperty("registeredDeviceId").GetGuid());
        Assert.Equal(scope.SessionId, root.GetProperty("sessionId").GetGuid());
        Assert.Equal(scope.DeviceId, root.GetProperty("deviceId").GetString());
        Assert.Equal(BatchHtu, root.GetProperty("batchEndpoint").GetString());
        Assert.Equal(1, root.GetProperty("proofKeyVersion").GetInt32());
        Assert.Equal("EC", root.GetProperty("proofPublicJwk").GetProperty("kty").GetString());
        Assert.True(root.GetProperty("canRetryFailedOperations").GetBoolean());
        Assert.True(root.GetProperty("canResolveConflicts").GetBoolean());
        Assert.False(root.GetProperty("keyEnrollmentAllowed").GetBoolean());
        Assert.True(root.GetProperty("keyRecoveryAllowed").GetBoolean());
        Assert.True(root.GetProperty("allowedActions").GetArrayLength() > 0);
        Assert.Equal(25, root.GetProperty("maxBatchOperations").GetInt32());
        Assert.Equal(2_097_152, root.GetProperty("maximumRequestBodyBytes").GetInt32());
        Assert.Equal(16_384, root.GetProperty("maximumPayloadBytes").GetInt32());
        Assert.Equal(2, root.GetProperty("clientTransportMaxRetryCount").GetInt32());
        Assert.Equal(10, root.GetProperty("clientTransportBaseSeconds").GetInt32());
        Assert.Equal(20, root.GetProperty("clientTransportMaxDelayMinutes").GetInt32());
        Assert.Equal(12, root.GetProperty("localSuccessHours").GetInt32());
        Assert.Equal(4, root.GetProperty("localRejectedDays").GetInt32());
        Assert.Equal(45, root.GetProperty("serverPayloadDays").GetInt32());
        Assert.Equal(8, root.GetProperty("cacheMaxAgeHours").GetInt32());
        Assert.Equal(JsonValueKind.Null, root.GetProperty("activationImplementationSha").ValueKind);
        Assert.Equal("test-policy-open-v1", root.GetProperty("policySourceVersion").GetString());
        Assert.Equal(new string('a', 64), root.GetProperty("policySourceFingerprint").GetString());
        Assert.DoesNotContain("credentialHash", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("rawBearer", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("nonce", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("jti", body, StringComparison.OrdinalIgnoreCase);

        using var anonymousClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            { BaseAddress = PublicOrigin });
        using var anonymous = await anonymousClient.GetAsync("/api/v1/sync/activation");
        Assert.Equal(HttpStatusCode.Unauthorized, anonymous.StatusCode);
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    [Trait("Category", "HTTP")]
    [Trait("Acceptance", "G4-END-TO-END")]
    public async Task Governed_activation_authorizes_explicit_key_enrollment_without_opening_write_runtime()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var seedDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await seedDb.Database.MigrateAsync();
        using var proofKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var scope = await SeedAsync(seedDb, proofKey, "ENROLL", bindProofKey: false);
        using var factory = CreateFactory(connection);
        using var client = CreateClient(factory, scope.Bearer);

        using var response = await client.GetAsync("/api/v1/sync/activation");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var root = document.RootElement;
        Assert.True(root.GetProperty("enabled").GetBoolean());
        Assert.Equal(JsonValueKind.Null, root.GetProperty("proofPublicJwk").ValueKind);
        Assert.Equal(JsonValueKind.Null, root.GetProperty("proofKeyVersion").ValueKind);
        Assert.True(root.GetProperty("keyEnrollmentAllowed").GetBoolean());
        Assert.False(root.GetProperty("keyRecoveryAllowed").GetBoolean());
        Assert.Equal("PROOF_KEY_BINDING_REQUIRED", root.GetProperty("closedReason").GetString());
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    [Trait("Category", "HTTP")]
    public async Task Batch_handler_reapplies_narrowed_effective_body_limit_before_envelope_processing()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var seedDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await seedDb.Database.MigrateAsync();
        using var proofKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var scope = await SeedAsync(seedDb, proofKey, "NARROW-BODY");
        const int narrowedLimit = 1_024;
        var marker = "must-not-enter-audit-" + Guid.NewGuid().ToString("N");
        var body = "{\"padding\":\"" + marker + new string('x', narrowedLimit) + "\"}";
        Assert.True(Encoding.UTF8.GetByteCount(body) > narrowedLimit);
        Assert.True(Encoding.UTF8.GetByteCount(body) < SyncApiModule.MaximumRequestBodyBytes);

        using var factory = CreateFactory(connection).WithWebHostBuilder(builder =>
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<ISyncPopHttpRequestAuthenticator>();
                services.AddSingleton<ISyncPopHttpRequestAuthenticator>(
                    new AcceptedRawBodyAuthenticator(scope, NarrowedBodyPolicy(narrowedLimit)));
            }));
        using var client = CreateClient(factory, scope.Bearer);
        using var request = new HttpRequestMessage(HttpMethod.Post, BatchPath)
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json")
        };
        request.Headers.Add("X-Correlation-Id", Guid.NewGuid().ToString("D"));

        using var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.RequestEntityTooLarge, response.StatusCode);
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal("REQUEST_BODY_TOO_LARGE",
            document.RootElement.GetProperty("errorCode").GetString());
        await using var verify = PostgreSqlTestEnvironment.CreateDbContext(connection);
        Assert.False(await verify.SyncOperations.AsNoTracking()
            .AnyAsync(x => x.CompanyId == scope.CompanyId));
        var rejection = Assert.Single(await verify.AuditEvents.AsNoTracking().Where(x =>
            x.CompanyId == scope.CompanyId && x.Action == "SyncOperationRejected" &&
            x.Reason == "REQUEST_BODY_TOO_LARGE").ToListAsync());
        Assert.Null(rejection.OperationCorrelationId);
        Assert.Null(rejection.BeforeJson);
        Assert.Null(rejection.AfterJson);
        Assert.DoesNotContain(marker, JsonSerializer.Serialize(rejection), StringComparison.Ordinal);
    }

    private static HttpClient CreateClient(WebApplicationFactory<Program> factory, string bearer)
    {
        var client = factory.CreateClient(new WebApplicationFactoryClientOptions { BaseAddress = PublicOrigin });
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);
        return client;
    }

    private static EffectiveSyncPolicy NarrowedBodyPolicy(int maximumRequestBodyBytes)
    {
        var actions = SyncActionCatalog.Definitions.Select(x => x.ActionCodeValue)
            .ToHashSet(StringComparer.Ordinal);
        return new EffectiveSyncPolicy(
            true, actions, new HashSet<string>(["sync-v1"], StringComparer.Ordinal),
            25, maximumRequestBodyBytes, Math.Min(512, maximumRequestBodyBytes),
            2, 5, 10, 5, 20, 30, 12, 4, 45, 8,
            null, "device-narrow-body-v1", new string('b', 64));
    }

    private static WebApplicationFactory<Program> CreateFactory(string connection)
        => new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseSetting("ConnectionStrings:TransportErp", connection);
            builder.UseSetting("Auth:Mode", "LocalSessions");
            builder.UseSetting("Auth:Issuer", Issuer);
            builder.UseSetting("Auth:Audience", Audience);
            builder.UseSetting("Auth:SigningKey", SigningKey);
            builder.UseSetting("Auth:SigningKeyId", "stage4-g4-http-current");
            builder.UseSetting("Sync:Proof:PublicOrigin", PublicOrigin.ToString().TrimEnd('/'));
            builder.UseSetting("AllowedHosts", PublicOrigin.Host);
            builder.UseSetting("Sync:ServerExecution:Enabled", "false");
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<ISyncRuntimeGate>();
                services.AddScoped<ISyncRuntimeGate, IsolatedOpenSyncRuntimeGate>();
                services.RemoveAll<IEffectiveSyncPolicyProvider>();
                services.AddScoped<IEffectiveSyncPolicyProvider, IsolatedOpenEffectivePolicyProvider>();
                services.RemoveAll<ISyncRetryPolicyResolver>();
                services.AddSingleton<ISyncRetryPolicyResolver>(new FixedSyncRetryPolicyResolver(
                    new SyncRetryPolicy(5, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(30))));
            });
        });

    private static async Task<HttpResponseMessage> SendSignedAsync(
        HttpClient client,
        ECDsa key,
        string bearer,
        string body)
    {
        using var nonceRequest = JsonRequest(body, Guid.NewGuid());
        using var nonceResponse = await client.SendAsync(nonceRequest);
        Assert.Equal(HttpStatusCode.Unauthorized, nonceResponse.StatusCode);
        Assert.True(nonceResponse.Headers.TryGetValues("DPoP-Nonce", out var nonceValues));
        var nonce = Assert.Single(nonceValues);
        var correlationId = Guid.NewGuid();
        var proof = CreateProof(key, Encoding.UTF8.GetBytes(body), bearer, nonce, correlationId);
        using var request = JsonRequest(body, correlationId, proof);
        return await client.SendAsync(request);
    }

    private static HttpRequestMessage JsonRequest(string body, Guid correlationId, string? proof = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, BatchPath)
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json")
        };
        request.Headers.Add("X-Correlation-Id", correlationId.ToString("D"));
        if (proof is not null) request.Headers.Add("DPoP", proof);
        return request;
    }

    private static string CreateProof(
        ECDsa key,
        byte[] body,
        string bearer,
        string nonce,
        Guid correlationId)
    {
        var parameters = key.ExportParameters(false);
        var header = JsonSerializer.Serialize(new
        {
            typ = "dpop+jwt",
            alg = "ES256",
            jwk = new
            {
                kty = "EC", crv = "P-256", x = Base64Url(parameters.Q.X!), y = Base64Url(parameters.Q.Y!)
            }
        }, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
        var payload = JsonSerializer.Serialize(new
        {
            jti = Guid.NewGuid().ToString("D"), htm = "POST", htu = BatchHtu,
            iat = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            ath = Base64Url(SHA256.HashData(Encoding.ASCII.GetBytes(bearer))), nonce,
            tbh = Base64Url(SHA256.HashData(body)), cid = correlationId.ToString("D")
        });
        var headerSegment = Base64Url(Encoding.UTF8.GetBytes(header));
        var payloadSegment = Base64Url(Encoding.UTF8.GetBytes(payload));
        var signingInput = Encoding.ASCII.GetBytes(headerSegment + "." + payloadSegment);
        var signature = key.SignData(signingInput, HashAlgorithmName.SHA256,
            DSASignatureFormat.IeeeP1363FixedFieldConcatenation);
        return headerSegment + "." + payloadSegment + "." + Base64Url(signature);
    }

    private static string BatchBody(string deviceId, params TestOperation[] operations)
        => JsonSerializer.Serialize(new
        {
            deviceId,
            protocolVersion = "sync-v1",
            operations = operations.Select(x => new
            {
                actionCode = x.ActionCode,
                operationType = x.OperationType,
                entityType = x.EntityType,
                entityId = x.EntityId,
                clientOperationId = x.ClientOperationId,
                payloadJson = x.PayloadJson,
                payloadHash = x.PayloadHash,
                clientOccurredAt = "2026-08-26T00:00:00.123456Z",
                operationCorrelationId = x.OperationCorrelationId,
                baseVersion = x.BaseVersion
            }).ToArray()
        });

    private static TestOperation Operation(
        string actionCode,
        string operationType,
        string entityType,
        Guid? entityId,
        string clientOperationId,
        Guid operationCorrelationId,
        string payloadJson,
        string? payloadHash,
        long? baseVersion)
        => new(actionCode, operationType, entityType, entityId, clientOperationId,
            operationCorrelationId, payloadJson, payloadHash ?? Sha256(payloadJson), baseVersion);

    private static TestOperation UpdateOperation(
        TestScope scope,
        CrossScopeTargets targets,
        Guid partyId,
        string suffix)
    {
        var operationId = $"t-sync-004-{suffix.ToLowerInvariant()}-{Guid.NewGuid():N}";
        var payload = JsonSerializer.Serialize(new UpdateWaybillDraftRequest(
            1, targets.WaybillDateTime, targets.OriginId, targets.DestinationId, scope.CurrencyId,
            1m, 10m, 0m, "STANDARD", "NORMAL",
            [new WaybillPartyInput("SENDER", partyId, "scope-neutral", "700000001", null, null,
                new GeoAddressSnapshot(null, null, null, null, "scope-neutral"))],
            [new WaybillItemInput(null, 1, "GENERAL", "scope-neutral", 1m, 1,
                null, null, null, null, null, null, [], null)],
            operationId));
        return Operation("UpdateWaybillDraft", "UPDATE", "Waybill", targets.LocalWaybillId,
            operationId, Guid.NewGuid(), payload, Sha256(payload), 1);
    }

    private static string PartyPayload(string operationId, string suffix)
        => JsonSerializer.Serialize(new OperationalPartyCreateRequest(
            $"Party {suffix}", "700000001", null, null,
            new GeoAddressSnapshot(null, null, null, null, "metadata only"), operationId));

    private static async Task<SyncBatchOperationResult> SingleResultAsync(HttpResponseMessage response)
        => Assert.Single(await ResultsAsync(response));

    private static async Task<IReadOnlyList<SyncBatchOperationResult>> ResultsAsync(HttpResponseMessage response)
    {
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        var envelope = JsonSerializer.Deserialize<SyncBatchResponse>(json,
            new JsonSerializerOptions(JsonSerializerDefaults.Web));
        Assert.NotNull(envelope);
        return envelope!.Results;
    }

    private static async Task ExecuteUntilTerminalAsync(
        WebApplicationFactory<Program> factory,
        string connection,
        IReadOnlyCollection<string> operationIds)
    {
        for (var attempt = 0; attempt < 30; attempt++)
        {
            await using (var scope = factory.Services.CreateAsyncScope())
                _ = await scope.ServiceProvider.GetRequiredService<SyncExecutionProcessor>()
                    .ExecuteNextAsync(TimeSpan.FromSeconds(30));

            await using var verify = PostgreSqlTestEnvironment.CreateDbContext(connection);
            var statuses = await verify.SyncOperations.AsNoTracking()
                .Where(x => operationIds.Contains(x.ClientOperationId))
                .Select(x => x.Status).ToListAsync();
            if (statuses.Count == operationIds.Count && statuses.All(x => x is "SUCCEEDED" or "REJECTED" or "CONFLICT"))
                return;
        }
        throw new Xunit.Sdk.XunitException("The governed operations did not reach terminal states.");
    }

    private static async Task<TestScope> SeedAsync(
        TransportErpDbContext db,
        ECDsa proofKey,
        string suffix,
        bool bindProofKey = true)
    {
        var now = DateTimeOffset.UtcNow;
        var currency = new Currency
        {
            Id = Guid.NewGuid(), Code = await PostgreSqlTestCurrencyCodeAllocator.NextAsync(db),
            NameAr = "عملة G4 HTTP", MinorUnit = 2, IsBase = true, Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var company = new Company
        {
            Id = Guid.NewGuid(), Code = $"G4H-{suffix}-{Guid.NewGuid():N}"[..18],
            LegalNameAr = "شركة G4 HTTP", BaseCurrencyId = currency.Id,
            DefaultCalendarId = Guid.NewGuid(), Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var branch = new Branch
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, Code = "MAIN", NameAr = "الفرع الرئيسي",
            Timezone = "Asia/Riyadh", Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var user = new User
        {
            Id = Guid.NewGuid(), UserName = $"g4-http-{Guid.NewGuid():N}",
            NormalizedUserName = $"G4HTTP{Guid.NewGuid():N}", DisplayName = "مستخدم G4 HTTP",
            PasswordHash = "test-only", SecurityStamp = Guid.NewGuid().ToString("N"), AuthVersion = 1,
            Status = "ACTIVE", CompanyId = company.Id, BranchId = branch.Id,
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var role = new Role
        {
            Id = Guid.NewGuid(), Code = $"G4H-{suffix}-{Guid.NewGuid():N}", NameAr = "دور G4 HTTP",
            CompanyId = company.Id, Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var sessionId = Guid.NewGuid();
        var registeredDeviceId = Guid.NewGuid();
        var deviceId = $"g4-http-{suffix.ToLowerInvariant()}-{Guid.NewGuid():N}";
        db.AddRange(currency, company, branch, user, role);
        db.UserRoles.Add(new UserRole
        {
            UserId = user.Id, RoleId = role.Id, CompanyId = company.Id, BranchId = branch.Id,
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        });
        foreach (var code in new[]
                 {
                     "sync.operations.execute", "party.create", "waybill.create", "waybill.edit",
                     "accounting.journal.create", "waybill.attachment.add", "waybill.pod.capture",
                     "sync.conflicts.resolve", "devices.manage"
                 })
        {
            var permission = await db.Permissions.SingleOrDefaultAsync(x => x.Code == code);
            if (permission is null)
            {
                permission = new Permission
                {
                    Id = Guid.NewGuid(), Code = code, NameAr = code, Resource = code.Split('.')[0],
                    Action = code.Split('.')[^1], ScopeType = "BRANCH", Status = "ACTIVE",
                    CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
                };
                db.Permissions.Add(permission);
            }
            db.RolePermissions.Add(new RolePermission
            {
                RoleId = role.Id, PermissionId = permission.Id, ScopeType = permission.ScopeType,
                CompanyId = permission.ScopeType == "PLATFORM" ? null : company.Id,
                BranchId = permission.ScopeType == "BRANCH" ? branch.Id : null,
                CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
            });
        }

        var publicParameters = proofKey.ExportParameters(false);
        var x = Base64Url(publicParameters.Q.X!);
        var y = Base64Url(publicParameters.Q.Y!);
        var canonicalJwk = $"{{\"crv\":\"P-256\",\"kty\":\"EC\",\"x\":\"{x}\",\"y\":\"{y}\"}}";
        var thumbprint = Base64Url(SHA256.HashData(Encoding.ASCII.GetBytes(canonicalJwk)));
        db.RegisteredDevices.Add(new RegisteredDevice
        {
            Id = registeredDeviceId, CompanyId = company.Id, DeviceId = deviceId,
            DisplayName = "G4 HTTP device", Platform = "TEST", AppVersion = "1.0",
            RegistrationRequestId = "g4-http-" + Guid.NewGuid().ToString("N"),
            CredentialHash = new string('a', 64), CredentialVersion = 1, Status = "ACTIVE",
            RegisteredByUserId = user.Id, ApprovedByUserId = user.Id, ApprovedAt = now, LastSeenAt = now,
            ProofPublicJwkCanonicalJson = bindProofKey ? canonicalJwk : null,
            ProofKeyThumbprint = bindProofKey ? thumbprint : null,
            ProofKeyVersion = bindProofKey ? 1 : null,
            ProofKeyChangedAt = bindProofKey ? now : null,
            ProofKeyChangedByUserId = bindProofKey ? user.Id : null,
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        });
        db.RegisteredDeviceAssignments.Add(new RegisteredDeviceAssignment
        {
            Id = Guid.NewGuid(), RegisteredDeviceId = registeredDeviceId, UserId = user.Id,
            CompanyId = company.Id, BranchId = branch.Id, Status = "ACTIVE", AssignedByUserId = user.Id,
            AssignedAt = now, CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        });
        db.AuthSessions.Add(new AuthSession
        {
            Id = sessionId, UserId = user.Id, CompanyId = company.Id, BranchId = branch.Id,
            DeviceId = deviceId, RegisteredDeviceId = registeredDeviceId, DeviceCredentialVersion = 1,
            Mode = "LOCAL", SecurityStampAtIssue = user.SecurityStamp, AuthVersionAtIssue = 1,
            RefreshTokenHash = Convert.ToHexString(SHA256.HashData(sessionId.ToByteArray())).ToLowerInvariant(),
            RefreshTokenFamilyId = Guid.NewGuid(), IssuedAt = now, AccessTokenExpiresAt = now.AddMinutes(15),
            RefreshTokenExpiresAt = now.AddDays(1), CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        });
        await db.SaveChangesAsync();

        return new TestScope(company.Id, branch.Id, user.Id, sessionId, registeredDeviceId,
            deviceId, currency.Id, CreateToken(user, company.Id, branch.Id, sessionId, registeredDeviceId, deviceId));
    }

    private static async Task<CrossScopeTargets> SeedCrossScopeTargetsAsync(
        TransportErpDbContext db,
        TestScope scope)
    {
        var now = DateTimeOffset.UtcNow;
        var foreignBranch = new Branch
        {
            Id = Guid.NewGuid(), CompanyId = scope.CompanyId, Code = $"B{Guid.NewGuid():N}"[..12],
            NameAr = "فرع آخر", Timezone = "Asia/Riyadh", Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var foreignCompany = new Company
        {
            Id = Guid.NewGuid(), Code = $"FC-{Guid.NewGuid():N}"[..18], LegalNameAr = "شركة أخرى",
            BaseCurrencyId = scope.CurrencyId, DefaultCalendarId = Guid.NewGuid(), Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var foreignCompanyBranch = new Branch
        {
            Id = Guid.NewGuid(), CompanyId = foreignCompany.Id, Code = "MAIN", NameAr = "فرع الشركة الأخرى",
            Timezone = "Asia/Riyadh", Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var branchParty = Party(scope.CompanyId, foreignBranch.Id, "BRANCH", now);
        var companyParty = Party(foreignCompany.Id, foreignCompanyBranch.Id, "COMPANY", now);
        var waybillDateTime = now;
        var originId = Guid.NewGuid();
        var destinationId = Guid.NewGuid();
        var waybill = new WaybillEntity
        {
            Id = Guid.NewGuid(), CompanyId = scope.CompanyId, BranchId = scope.BranchId,
            DraftNo = "D-G4-" + Guid.NewGuid().ToString("N"), WaybillDateTime = waybillDateTime,
            ServiceType = "STANDARD", Priority = "NORMAL", OriginId = originId,
            DestinationId = destinationId, CurrencyId = scope.CurrencyId, ExchangeRate = 1,
            FreightTotal = 0, DiscountTotal = 0, Status = "DRAFT", FinancialStatus = "UNPAID",
            CreateClientOperationId = "seed-" + Guid.NewGuid().ToString("N"),
            LastClientOperationId = "seed-" + Guid.NewGuid().ToString("N"),
            Version = 1, CreatedAt = now, UpdatedAt = now
        };
        db.AddRange(foreignBranch, foreignCompany, foreignCompanyBranch, branchParty, companyParty, waybill);
        await db.SaveChangesAsync();
        return new CrossScopeTargets(waybill.Id, branchParty.Id, companyParty.Id,
            waybillDateTime, originId, destinationId);
    }

    private static OperationalPartyEntity Party(Guid companyId, Guid branchId, string suffix, DateTimeOffset now)
        => new()
        {
            Id = Guid.NewGuid(), CompanyId = companyId, BranchId = branchId,
            PartyNo = $"P-{suffix}-{Guid.NewGuid():N}"[..30], Name = $"Party {suffix}",
            Mobile = "700000001", Status = "ACTIVE", ClientOperationId = $"party-{suffix}-{Guid.NewGuid():N}",
            Version = 1, CreatedAt = now, UpdatedAt = now
        };

    private static string CreateToken(
        User user,
        Guid companyId,
        Guid branchId,
        Guid sessionId,
        Guid registeredDeviceId,
        string deviceId)
    {
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString("D")),
                new Claim("company_id", companyId.ToString("D")),
                new Claim("branch_id", branchId.ToString("D")),
                new Claim("sid", sessionId.ToString("D")),
                new Claim("device_id", deviceId),
                new Claim("registered_device_id", registeredDeviceId.ToString("D")),
                new Claim("device_credential_version", "1"),
                new Claim("security_stamp", user.SecurityStamp!),
                new Claim("auth_version", "1")
            ]),
            Issuer = Issuer,
            Audience = Audience,
            Expires = DateTime.UtcNow.AddMinutes(10),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey))
                    { KeyId = "stage4-g4-http-current" }, SecurityAlgorithms.HmacSha256)
        };
        return new JwtSecurityTokenHandler().WriteToken(
            new JwtSecurityTokenHandler().CreateToken(descriptor));
    }

    private static string Sha256(string value)
        => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value))).ToLowerInvariant();

    private static string Base64Url(ReadOnlySpan<byte> value)
        => Convert.ToBase64String(value).TrimEnd('=').Replace('+', '-').Replace('/', '_');

    private sealed record TestOperation(
        string ActionCode,
        string OperationType,
        string EntityType,
        Guid? EntityId,
        string ClientOperationId,
        Guid OperationCorrelationId,
        string PayloadJson,
        string PayloadHash,
        long? BaseVersion);

    private sealed record TestScope(
        Guid CompanyId,
        Guid BranchId,
        Guid UserId,
        Guid SessionId,
        Guid RegisteredDeviceId,
        string DeviceId,
        Guid CurrencyId,
        string Bearer);

    private sealed class AcceptedRawBodyAuthenticator(
        TestScope scope,
        EffectiveSyncPolicy effectivePolicy) : ISyncPopHttpRequestAuthenticator
    {
        public async Task<SyncHttpAuthenticationResult> AuthenticateAsync(
            HttpContext http,
            string canonicalPath,
            TryReadSyncRequestDeviceId? tryReadBodyDeviceId,
            CancellationToken cancellationToken)
        {
            await using var buffer = new MemoryStream();
            await http.Request.Body.CopyToAsync(buffer, cancellationToken);
            var attemptCorrelationId = Guid.TryParse(
                http.Request.Headers["X-Correlation-Id"].FirstOrDefault(), out var supplied)
                ? supplied
                : Guid.NewGuid();
            var current = new CurrentSecurityContext(
                scope.UserId, scope.CompanyId, scope.BranchId, scope.SessionId,
                scope.DeviceId, true, scope.RegisteredDeviceId, 1);
            var security = new SyncProofSecurityContext(
                scope.UserId, scope.CompanyId, scope.BranchId,
                scope.RegisteredDeviceId, scope.DeviceId);
            var proof = new AcceptedSyncProofContext(
                Guid.NewGuid(), scope.UserId, scope.CompanyId, scope.BranchId,
                scope.RegisteredDeviceId, scope.DeviceId, 1, 1,
                new string('t', 43), attemptCorrelationId);
            return new SyncHttpAuthenticationResult(
                new AcceptedSyncHttpRequest(
                    current, security, proof, buffer.ToArray(), attemptCorrelationId, effectivePolicy),
                null);
        }
    }

    private sealed class IsolatedOpenEffectivePolicyProvider : IEffectiveSyncPolicyProvider
    {
        public Task<EffectiveSyncPolicy> ResolveAsync(
            CurrentSecurityContext current,
            CancellationToken cancellationToken = default)
        {
            var actions = SyncActionCatalog.Definitions.Select(x => x.ActionCodeValue)
                .ToHashSet(StringComparer.Ordinal);
            return Task.FromResult(new EffectiveSyncPolicy(
                true, actions, new HashSet<string>(["sync-v1"], StringComparer.Ordinal),
                25, 2_097_152, 16_384, 2, 5, 10, 5, 20, 30, 12, 4, 45, 8,
                null, "test-policy-open-v1", new string('a', 64)));
        }
    }

    private sealed record CrossScopeTargets(
        Guid LocalWaybillId,
        Guid ForeignBranchPartyId,
        Guid ForeignCompanyPartyId,
        DateTimeOffset WaybillDateTime,
        Guid OriginId,
        Guid DestinationId);

    private sealed class IsolatedOpenSyncRuntimeGate : ISyncRuntimeGate
    {
        public Task<EffectiveSyncPolicy> ResolveAsync(
            CurrentSecurityContext current,
            CancellationToken cancellationToken)
            => Task.FromResult(new EffectiveSyncPolicy(
                true,
                new HashSet<string>(SyncActionCatalog.Definitions.Select(x => x.ActionCodeValue), StringComparer.Ordinal),
                new HashSet<string>(["sync-v1"], StringComparer.Ordinal),
                100, 2_097_152, 16_384, 5, 5, 5, 5, 30, 30, 24, 7, 90, 24, null));
    }
}
