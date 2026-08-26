using System.Text;
using System.Text.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TransportERP.Api.Security;
using TransportERP.Api.Sync;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

public sealed class Stage4SyncApiContractTests
{
    [Fact]
    public void Envelope_codec_accepts_exact_web_camel_case_and_rejects_pascal_or_unknown_properties()
    {
        const string camel = """
            {"deviceId":"device-1","protocolVersion":"sync-v1","operations":[]}
            """;
        var parsed = SyncBatchJsonContract.Deserialize(Encoding.UTF8.GetBytes(camel));

        Assert.NotNull(parsed);
        Assert.Equal("device-1", parsed!.DeviceId);
        Assert.True(SyncBatchJsonContract.TryReadDeviceId(Encoding.UTF8.GetBytes(camel), out var deviceId));
        Assert.Equal(parsed.DeviceId, deviceId);

        const string pascal = """
            {"DeviceId":"device-1","ProtocolVersion":"sync-v1","Operations":[]}
            """;
        Assert.Throws<JsonException>(() => SyncBatchJsonContract.Deserialize(Encoding.UTF8.GetBytes(pascal)));
        Assert.False(SyncBatchJsonContract.TryReadDeviceId(Encoding.UTF8.GetBytes(pascal), out _));

        const string unknown = """
            {"deviceId":"device-1","protocolVersion":"sync-v1","operations":[],"extra":true}
            """;
        Assert.Throws<JsonException>(() => SyncBatchJsonContract.Deserialize(Encoding.UTF8.GetBytes(unknown)));
    }

    [Fact]
    public void Success_contract_serializes_attempt_correlation_id_with_its_governed_name()
    {
        var attemptCorrelationId = Guid.NewGuid();
        var response = new SyncBatchResponse("sync-v1", [], DateTimeOffset.UtcNow, attemptCorrelationId);

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        using var document = JsonDocument.Parse(json);

        Assert.Equal(attemptCorrelationId,
            document.RootElement.GetProperty("attemptCorrelationId").GetGuid());
        Assert.False(document.RootElement.TryGetProperty("correlationId", out _));
    }

    [Fact]
    public async Task Shared_authenticator_checks_closed_gate_before_body_read_or_nonce_state()
    {
        var runtime = new RecordingProofRuntime();
        var authenticator = Authenticator(open: false, runtime: runtime);
        var body = new MemoryStream("{}"u8.ToArray());
        var http = Request(body);

        var result = await authenticator.AuthenticateAsync(
            http, "/api/v1/sync/conflicts/aaaaaaaa-bbbb-4ccc-8ddd-eeeeeeeeeeee:resolve", null, default);

        Assert.NotNull(result.Failure);
        Assert.Null(result.Accepted);
        Assert.Equal(0L, body.Position);
        Assert.Equal(0, runtime.IssueCount);
        Assert.Equal(0, runtime.ClaimCount);
    }

    [Fact]
    public async Task Shared_authenticator_hashes_raw_body_boundary_before_issuing_missing_proof_nonce()
    {
        var runtime = new RecordingProofRuntime();
        var authenticator = Authenticator(open: true, runtime: runtime);
        var bytes = "{\"decision\":\"KEEP_SERVER_AND_REJECT_LOCAL\",\"reason\":\"reviewed\"}"u8.ToArray();
        var body = new MemoryStream(bytes);
        var http = Request(body);

        var result = await authenticator.AuthenticateAsync(
            http, "/api/v1/sync/conflicts/aaaaaaaa-bbbb-4ccc-8ddd-eeeeeeeeeeee:resolve", null, default);

        Assert.NotNull(result.Failure);
        Assert.Equal((long)bytes.Length, body.Position);
        Assert.Equal(1, runtime.IssueCount);
        Assert.Equal(0, runtime.ClaimCount);
        Assert.True(http.Response.Headers.ContainsKey("DPoP-Nonce"));
    }

    [Fact]
    public async Task Shared_authenticator_rejects_request_host_spoofing_before_nonce_or_claim_state()
    {
        var runtime = new RecordingProofRuntime();
        var authenticator = Authenticator(open: true, runtime: runtime);
        var body = new MemoryStream("{}"u8.ToArray());
        var http = Request(body);
        http.Request.Host = new HostString("attacker.invalid");

        var result = await authenticator.AuthenticateAsync(
            http, "/api/v1/sync/conflicts/aaaaaaaa-bbbb-4ccc-8ddd-eeeeeeeeeeee:resolve", null, default);

        Assert.NotNull(result.Failure);
        Assert.Equal(body.Length, body.Position);
        Assert.Equal(0, runtime.IssueCount);
        Assert.Equal(0, runtime.ClaimCount);
    }

    private static SyncPopHttpRequestAuthenticator Authenticator(bool open, RecordingProofRuntime runtime)
    {
        var current = new CurrentSecurityContext(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "contract-device", true,
            Guid.NewGuid(), 1);
        var profile = new SyncPopDeploymentProfile(
            true, "https://sync.example.test/api/v1/sync/operations:batch", "sync.example.test", 443,
            false, 1, [], [], []);
        return new SyncPopHttpRequestAuthenticator(
            new StaticSecurityContext(current), new StaticGate(open), new SyncPopProofValidator(), runtime, profile);
    }

    private static DefaultHttpContext Request(Stream body)
    {
        var http = new DefaultHttpContext();
        http.User = new ClaimsPrincipal(new ClaimsIdentity("test"));
        http.Request.Scheme = "https";
        http.Request.Host = new HostString("sync.example.test");
        http.Request.ContentType = "application/json";
        http.Request.ContentLength = body.Length;
        http.Request.Headers["X-Correlation-Id"] = Guid.NewGuid().ToString("D");
        http.Request.Body = body;
        return http;
    }

    private sealed class StaticGate(bool open) : ISyncRuntimeGate
    {
        public Task<bool> IsOpenAsync(Guid companyId, CancellationToken cancellationToken)
            => Task.FromResult(open);
    }

    private sealed class StaticSecurityContext(CurrentSecurityContext current) : ICurrentSecurityContext
    {
        public Task<CurrentSecurityContext?> ResolveAsync(
            ClaimsPrincipal principal,
            CancellationToken cancellationToken = default)
            => Task.FromResult<CurrentSecurityContext?>(current);

        public Task<bool> HasPermissionAsync(
            CurrentSecurityContext context,
            string permissionCode,
            CancellationToken cancellationToken = default)
            => Task.FromResult(true);
    }

    private sealed class RecordingProofRuntime : ISyncProofRuntime
    {
        public int IssueCount { get; private set; }
        public int ClaimCount { get; private set; }

        public Task<IssuedSyncNonce> IssueNonceAsync(
            SyncProofSecurityContext security,
            CancellationToken cancellationToken = default)
        {
            IssueCount++;
            return Task.FromResult(new IssuedSyncNonce(
                Convert.ToBase64String(new byte[32]).TrimEnd('=').Replace('+', '-').Replace('/', '_'),
                DateTimeOffset.UtcNow.AddMinutes(5)));
        }

        public Task<AcceptedSyncProofContext> ClaimAsync(
            SyncProofSecurityContext security,
            VerifiedSyncProofMaterial proof,
            CancellationToken cancellationToken = default)
        {
            ClaimCount++;
            throw new NotSupportedException();
        }
    }
}
