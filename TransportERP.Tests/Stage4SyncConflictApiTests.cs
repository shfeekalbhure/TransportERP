using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TransportERP.Api.Security;
using TransportERP.Api.Sync;
using TransportERP.Application.Sync;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

public sealed class Stage4SyncConflictApiTests
{
    private static readonly BuildIdentityV1 TestBuildIdentity = new(
        BuildIdentityV1.DesktopWindowsPlatform, new string('6', 64));
    [Fact]
    public async Task Separate_conflict_endpoint_requires_resolve_permission_and_forwards_exact_registered_device_scope()
    {
        var capture = new CapturingConflictService();
        await using var app = await StartAppAsync(capture, registered: true);
        using var client = app.GetTestClient();
        var conflictId = Guid.NewGuid();
        var request = new ResolveSyncConflictRequest(
            SyncConflictResolutionDecisions.KeepServerAndRejectLocal, "reviewed conflict",
            BuildIdentity: TestBuildIdentity);

        var forbidden = await client.PostAsJsonAsync($"/api/v1/sync/conflicts/{conflictId}:resolve", request);
        Assert.Equal(HttpStatusCode.Forbidden, forbidden.StatusCode);
        Assert.Equal(0, capture.CallCount);

        client.DefaultRequestHeaders.Add("X-Test-Permission", SyncConflictPermissionCodes.Resolve);
        var allowed = await client.PostAsJsonAsync($"/api/v1/sync/conflicts/{conflictId}:resolve", request);
        Assert.Equal(HttpStatusCode.OK, allowed.StatusCode);
        Assert.Equal(1, capture.CallCount);
        Assert.Equal(TestIdentity.CompanyId, capture.Context!.CompanyId);
        Assert.Equal(TestIdentity.BranchId, capture.Context.BranchId);
        Assert.Equal(TestIdentity.UserId, capture.Context.UserId);
        Assert.Equal(TestIdentity.RegisteredDeviceId, capture.Context.RegisteredDeviceId);
        Assert.Equal(TestIdentity.DeviceId, capture.Context.DeviceId);
        Assert.EndsWith($"/{conflictId:D}:resolve", capture.CanonicalPath, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Endpoint_rejects_session_without_current_registered_device_before_service()
    {
        var capture = new CapturingConflictService();
        await using var app = await StartAppAsync(capture, registered: false);
        using var client = app.GetTestClient();
        client.DefaultRequestHeaders.Add("X-Test-Permission", SyncConflictPermissionCodes.Resolve);

        var response = await client.PostAsJsonAsync($"/api/v1/sync/conflicts/{Guid.NewGuid()}:resolve",
            new ResolveSyncConflictRequest(SyncConflictResolutionDecisions.KeepServerAndRejectLocal, "reason",
                BuildIdentity: TestBuildIdentity));

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.Equal("DEVICE_NOT_REGISTERED", body!.ErrorCode);
        Assert.Equal(0, capture.CallCount);
    }

    [Fact]
    public async Task Endpoint_forwards_fresh_attempt_proof_and_strict_reapply_payload()
    {
        var capture = new CapturingConflictService();
        await using var app = await StartAppAsync(capture, registered: true);
        using var client = app.GetTestClient();
        client.DefaultRequestHeaders.Add("X-Test-Permission", SyncConflictPermissionCodes.Resolve);

        var response = await client.PostAsJsonAsync($"/api/v1/sync/conflicts/{Guid.NewGuid()}:resolve",
            new ResolveSyncConflictRequest(SyncConflictResolutionDecisions.ReapplyAsNew, "reason",
                new SyncReapplyAsNewRequest("new-operation", Guid.NewGuid(), "UpdateWaybillDraft", "UPDATE",
                    "Waybill", Guid.NewGuid(), 2, DateTimeOffset.UtcNow, "{}",
                    Convert.ToHexString(System.Security.Cryptography.SHA256.HashData("{}"u8)).ToLowerInvariant()),
                TestBuildIdentity));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(capture.Proof);
        Assert.Equal(capture.Context!.CorrelationId, capture.Proof!.AttemptCorrelationId);
    }

    [Theory]
    [InlineData("CONFLICT_NOT_FOUND")]
    [InlineData("OPERATION_NOT_FOUND")]
    [InlineData("SCOPE_DENIED")]
    public async Task Endpoint_does_not_disclose_missing_versus_cross_scope_conflict(string serviceError)
    {
        var capture = new CapturingConflictService { ErrorCode = serviceError };
        await using var app = await StartAppAsync(capture, registered: true);
        using var client = app.GetTestClient();
        client.DefaultRequestHeaders.Add("X-Test-Permission", SyncConflictPermissionCodes.Resolve);

        var response = await client.PostAsJsonAsync($"/api/v1/sync/conflicts/{Guid.NewGuid()}:resolve",
            new ResolveSyncConflictRequest(SyncConflictResolutionDecisions.KeepServerAndRejectLocal, "reason",
                BuildIdentity: TestBuildIdentity));

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.Equal("SCOPE_DENIED", body!.ErrorCode);
    }

    [Theory]
    [InlineData("OFFLINE_DISABLED", HttpStatusCode.Forbidden)]
    [InlineData("invalid_dpop_proof", HttpStatusCode.Unauthorized)]
    public async Task Gate_or_proof_failure_never_reaches_conflict_mutation(
        string authenticationError,
        HttpStatusCode expectedStatus)
    {
        var capture = new CapturingConflictService();
        await using var app = await StartAppAsync(
            capture, registered: true, authenticationError: authenticationError);
        using var client = app.GetTestClient();
        client.DefaultRequestHeaders.Add("X-Test-Permission", SyncConflictPermissionCodes.Resolve);

        var response = await client.PostAsJsonAsync($"/api/v1/sync/conflicts/{Guid.NewGuid()}:resolve",
            new ResolveSyncConflictRequest(SyncConflictResolutionDecisions.KeepServerAndRejectLocal, "reason",
                BuildIdentity: TestBuildIdentity));

        Assert.Equal(expectedStatus, response.StatusCode);
        Assert.Equal(0, capture.CallCount);
    }

    [Fact]
    public async Task Strict_raw_body_codec_rejects_unknown_fields_after_proof_without_service_mutation()
    {
        var capture = new CapturingConflictService();
        await using var app = await StartAppAsync(capture, registered: true);
        using var client = app.GetTestClient();
        client.DefaultRequestHeaders.Add("X-Test-Permission", SyncConflictPermissionCodes.Resolve);
        using var content = JsonContent.Create(new
        {
            decision = SyncConflictResolutionDecisions.KeepServerAndRejectLocal,
            reason = "reviewed",
            unexpected = "must fail"
        });

        var response = await client.PostAsync($"/api/v1/sync/conflicts/{Guid.NewGuid()}:resolve", content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(0, capture.CallCount);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Conflict_resolution_rejects_missing_or_different_build_identity_before_mutation(bool missing)
    {
        var capture = new CapturingConflictService();
        await using var app = await StartAppAsync(capture, registered: true);
        using var client = app.GetTestClient();
        client.DefaultRequestHeaders.Add("X-Test-Permission", SyncConflictPermissionCodes.Resolve);
        var identity = missing
            ? null
            : new BuildIdentityV1(BuildIdentityV1.DesktopWindowsPlatform, new string('7', 64));

        var response = await client.PostAsJsonAsync($"/api/v1/sync/conflicts/{Guid.NewGuid()}:resolve",
            new ResolveSyncConflictRequest(
                SyncConflictResolutionDecisions.KeepServerAndRejectLocal, "reviewed", BuildIdentity: identity));

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.Equal("BUILD_IDENTITY_MISMATCH", body!.ErrorCode);
        Assert.Equal(0, capture.CallCount);
    }

    [Fact]
    public async Task Conflict_resolution_accepts_an_exact_Android_identity_from_the_multi_platform_authority()
    {
        var android = new BuildIdentityV1(
            BuildIdentityV1.AndroidPlatform, new string('8', 64), new string('9', 64));
        var capture = new CapturingConflictService();
        await using var app = await StartAppAsync(
            capture, registered: true, authorizedBuilds: [TestBuildIdentity, android]);
        using var client = app.GetTestClient();
        client.DefaultRequestHeaders.Add("X-Test-Permission", SyncConflictPermissionCodes.Resolve);

        var response = await client.PostAsJsonAsync($"/api/v1/sync/conflicts/{Guid.NewGuid()}:resolve",
            new ResolveSyncConflictRequest(
                SyncConflictResolutionDecisions.KeepServerAndRejectLocal, "reviewed",
                BuildIdentity: android));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(1, capture.CallCount);
    }

    private static async Task<WebApplication> StartAppAsync(
        CapturingConflictService service,
        bool registered,
        string? authenticationError = null,
        IReadOnlyList<BuildIdentityV1>? authorizedBuilds = null)
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();
        builder.Services.AddAuthentication("test")
            .AddScheme<AuthenticationSchemeOptions, HeaderAuthenticationHandler>("test", _ => { });
        builder.Services.AddAuthorization(options => options.AddPolicy(
            SecurityPolicies.Permission(SyncConflictPermissionCodes.Resolve),
            policy => policy.RequireAuthenticatedUser().RequireClaim("permission", SyncConflictPermissionCodes.Resolve)));
        builder.Services.AddSingleton<ISyncPopHttpRequestAuthenticator>(
            new FakeAuthenticator(registered, service, authenticationError));
        builder.Services.AddSingleton<ISyncConflictResolutionService>(service);
        builder.Services.AddSingleton<IOptions<SyncRuntimePolicyOptions>>(Options.Create(new SyncRuntimePolicyOptions
        {
            OfflineAuthorizedBuilds = (authorizedBuilds ?? new[] { TestBuildIdentity }).ToArray()
        }));

        var app = builder.Build();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapSyncConflictRuntime();
        await app.StartAsync();
        return app;
    }

    private sealed class CapturingConflictService : ISyncConflictResolutionService
    {
        public int CallCount { get; private set; }
        public string? ErrorCode { get; init; }
        public SyncConflictResolutionContext? Context { get; private set; }
        public AcceptedSyncProofContext? Proof { get; private set; }
        public string? CanonicalPath { get; set; }

        public Task<SyncConflictResolutionResult> ResolveAsync(
            Guid conflictCaseId,
            ResolveSyncConflictRequest request,
            SyncConflictResolutionContext context,
            AcceptedSyncProofContext acceptedProof,
            CancellationToken cancellationToken = default)
        {
            CallCount++;
            Context = context;
            Proof = acceptedProof;
            if (ErrorCode is not null) throw new SyncRuleException(ErrorCode, conflictCaseId.ToString());
            return Task.FromResult(new SyncConflictResolutionResult(
                conflictCaseId, Guid.NewGuid(), request.Decision, "RESOLVED", "REJECTED", "KEEP_SERVER",
                null, DateTimeOffset.UtcNow, context.CorrelationId));
        }
    }

    private sealed class FakeAuthenticator(
        bool registered,
        CapturingConflictService capture,
        string? authenticationError)
        : ISyncPopHttpRequestAuthenticator
    {
        public async Task<SyncHttpAuthenticationResult> AuthenticateAsync(
            HttpContext http,
            string canonicalPath,
            TryReadSyncRequestDeviceId? tryReadBodyDeviceId,
            CancellationToken cancellationToken)
        {
            capture.CanonicalPath = canonicalPath;
            var correlationId = http.Request.Headers.TryGetValue("X-Correlation-Id", out var values) &&
                                Guid.TryParseExact(values.SingleOrDefault(), "D", out var parsed)
                ? parsed : Guid.NewGuid();
            if (authenticationError is not null)
            {
                var status = authenticationError == "OFFLINE_DISABLED"
                    ? StatusCodes.Status403Forbidden : StatusCodes.Status401Unauthorized;
                return new(null, Results.Json(new
                    { ErrorCode = authenticationError, CorrelationId = correlationId }, statusCode: status));
            }
            if (!registered)
                return new(null, Results.Json(new
                    { ErrorCode = "DEVICE_NOT_REGISTERED", CorrelationId = correlationId },
                    statusCode: StatusCodes.Status403Forbidden));
            await using var body = new MemoryStream();
            await http.Request.Body.CopyToAsync(body, cancellationToken);
            var current = new CurrentSecurityContext(
                TestIdentity.UserId, TestIdentity.CompanyId, TestIdentity.BranchId, Guid.NewGuid(),
                TestIdentity.DeviceId, true, TestIdentity.RegisteredDeviceId, 1);
            var security = new SyncProofSecurityContext(
                TestIdentity.UserId, TestIdentity.CompanyId, TestIdentity.BranchId,
                TestIdentity.RegisteredDeviceId, TestIdentity.DeviceId);
            var proof = new AcceptedSyncProofContext(
                Guid.NewGuid(), TestIdentity.UserId, TestIdentity.CompanyId, TestIdentity.BranchId,
                TestIdentity.RegisteredDeviceId, TestIdentity.DeviceId, 1, 1, new string('t', 43), correlationId);
            return new(new AcceptedSyncHttpRequest(current, security, proof, body.ToArray(), correlationId), null);
        }
    }

    private sealed class HeaderAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
    {
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, TestIdentity.UserId.ToString()) };
            if (Request.Headers.TryGetValue("X-Test-Permission", out var permission))
                claims.Add(new Claim("permission", permission.ToString()));
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, Scheme.Name));
            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name)));
        }
    }

    private sealed record ErrorResponse(string ErrorCode, Guid CorrelationId);

    private static class TestIdentity
    {
        public static readonly Guid UserId = Guid.Parse("11111111-1111-4111-8111-111111111111");
        public static readonly Guid CompanyId = Guid.Parse("22222222-2222-4222-8222-222222222222");
        public static readonly Guid BranchId = Guid.Parse("33333333-3333-4333-8333-333333333333");
        public static readonly Guid RegisteredDeviceId = Guid.Parse("44444444-4444-4444-8444-444444444444");
        public const string DeviceId = "conflict-http-device";
    }
}
