using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TransportERP.Api.Security;
using TransportERP.Api.Sync;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

public sealed class Stage4SyncConflictApiTests
{
    [Fact]
    public async Task Separate_conflict_endpoint_requires_resolve_permission_and_forwards_exact_registered_device_scope()
    {
        var capture = new CapturingConflictService();
        await using var app = await StartAppAsync(capture, registered: true);
        using var client = app.GetTestClient();
        var conflictId = Guid.NewGuid();
        var request = new ResolveSyncConflictRequest(
            SyncConflictResolutionDecisions.KeepServerAndRejectLocal, "reviewed conflict");

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
    }

    [Fact]
    public async Task Endpoint_rejects_session_without_current_registered_device_before_service()
    {
        var capture = new CapturingConflictService();
        await using var app = await StartAppAsync(capture, registered: false);
        using var client = app.GetTestClient();
        client.DefaultRequestHeaders.Add("X-Test-Permission", SyncConflictPermissionCodes.Resolve);

        var response = await client.PostAsJsonAsync($"/api/v1/sync/conflicts/{Guid.NewGuid()}:resolve",
            new ResolveSyncConflictRequest(SyncConflictResolutionDecisions.KeepServerAndRejectLocal, "reason"));

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.Equal("DEVICE_NOT_REGISTERED", body!.ErrorCode);
        Assert.Equal(0, capture.CallCount);
    }

    [Fact]
    public async Task Endpoint_exposes_explicit_reapply_proof_blocker_without_creating_a_success_response()
    {
        var capture = new CapturingConflictService { ErrorCode = "REAPPLY_PROOF_REQUIRED" };
        await using var app = await StartAppAsync(capture, registered: true);
        using var client = app.GetTestClient();
        client.DefaultRequestHeaders.Add("X-Test-Permission", SyncConflictPermissionCodes.Resolve);

        var response = await client.PostAsJsonAsync($"/api/v1/sync/conflicts/{Guid.NewGuid()}:resolve",
            new ResolveSyncConflictRequest(SyncConflictResolutionDecisions.ReapplyAsNew, "reason",
                new SyncReapplyAsNewRequest("new-operation", Guid.NewGuid(), "UpdateWaybillDraft", "UPDATE",
                    "Waybill", Guid.NewGuid(), 2, DateTimeOffset.UtcNow, "{}")));

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.Equal("REAPPLY_PROOF_REQUIRED", body!.ErrorCode);
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
            new ResolveSyncConflictRequest(SyncConflictResolutionDecisions.KeepServerAndRejectLocal, "reason"));

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.Equal("SCOPE_DENIED", body!.ErrorCode);
    }

    private static async Task<WebApplication> StartAppAsync(
        CapturingConflictService service,
        bool registered)
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();
        builder.Services.AddAuthentication("test")
            .AddScheme<AuthenticationSchemeOptions, HeaderAuthenticationHandler>("test", _ => { });
        builder.Services.AddAuthorization(options => options.AddPolicy(
            SecurityPolicies.Permission(SyncConflictPermissionCodes.Resolve),
            policy => policy.RequireAuthenticatedUser().RequireClaim("permission", SyncConflictPermissionCodes.Resolve)));
        builder.Services.AddSingleton<ICurrentSecurityContext>(new StaticCurrentSecurityContext(registered));
        builder.Services.AddSingleton<ISyncConflictResolutionService>(service);

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

        public Task<SyncConflictResolutionResult> ResolveAsync(
            Guid conflictCaseId,
            ResolveSyncConflictRequest request,
            SyncConflictResolutionContext context,
            CancellationToken cancellationToken = default)
        {
            CallCount++;
            Context = context;
            if (ErrorCode is not null) throw new SyncRuleException(ErrorCode, conflictCaseId.ToString());
            return Task.FromResult(new SyncConflictResolutionResult(
                conflictCaseId, Guid.NewGuid(), request.Decision, "RESOLVED", "REJECTED", "KEEP_SERVER",
                null, DateTimeOffset.UtcNow, context.CorrelationId));
        }
    }

    private sealed class StaticCurrentSecurityContext(bool registered) : ICurrentSecurityContext
    {
        public Task<CurrentSecurityContext?> ResolveAsync(
            ClaimsPrincipal principal,
            CancellationToken cancellationToken = default)
            => Task.FromResult<CurrentSecurityContext?>(new CurrentSecurityContext(
                TestIdentity.UserId, TestIdentity.CompanyId, TestIdentity.BranchId, Guid.NewGuid(),
                TestIdentity.DeviceId, true,
                registered ? TestIdentity.RegisteredDeviceId : null,
                registered ? 1 : null));

        public Task<bool> HasPermissionAsync(
            CurrentSecurityContext context,
            string permissionCode,
            CancellationToken cancellationToken = default)
            => Task.FromResult(true);
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
