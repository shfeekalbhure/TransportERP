using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TransportERP.Api.Identity;
using TransportERP.Api.Security;
using TransportERP.Api.Sync;
using TransportERP.Api.Waybills;
using TransportERP.Infrastructure.Persistence;

var bootstrapRequested = args.Any(x => string.Equals(x, "--bootstrap-admin", StringComparison.Ordinal));
if (bootstrapRequested && args.Any(x => x.Contains("AdminPassword", StringComparison.OrdinalIgnoreCase) ||
                                       x.Contains("TRANSPORTERP_BOOTSTRAP_ADMIN_PASSWORD", StringComparison.OrdinalIgnoreCase)))
    throw new InvalidOperationException("BOOTSTRAP_PASSWORD_COMMAND_LINE_FORBIDDEN");
var hostArgs = args.Where(x => !string.Equals(x, "--bootstrap-admin", StringComparison.Ordinal)).ToArray();
var builder = WebApplication.CreateBuilder(hostArgs);

var syncRuntimePolicy = SyncRuntimePolicyOptions.Load(builder.Configuration);
var syncRuntimePolicyValidation = new SyncRuntimePolicyOptionsValidator().Validate(null, syncRuntimePolicy);
if (syncRuntimePolicyValidation.Failed)
    throw new OptionsValidationException("Sync", typeof(SyncRuntimePolicyOptions),
        syncRuntimePolicyValidation.Failures);

var connectionString = builder.Configuration.GetConnectionString("TransportErp")
    ?? Environment.GetEnvironmentVariable("TRANSPORTERP_CONNECTION_STRING")
    ?? throw new InvalidOperationException("Transport ERP database connection is not configured.");

if (bootstrapRequested)
{
    builder.Services.AddTransportErpPostgreSql(connectionString);
    builder.Services.AddScoped<AuditEventService>();
    builder.Services.AddScoped<BootstrapAdminService>();
    builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
    var bootstrapOptions = BootstrapAdminOptions.FromConfiguration(builder.Configuration);
    var bootstrapHost = builder.Build();
    await using var bootstrapScope = bootstrapHost.Services.CreateAsyncScope();
    var bootstrapDb = bootstrapScope.ServiceProvider.GetRequiredService<TransportErpDbContext>();
    await bootstrapDb.Database.MigrateAsync();
    await bootstrapScope.ServiceProvider.GetRequiredService<BootstrapAdminService>().ExecuteAsync(bootstrapOptions);
    Console.WriteLine("TransportERP one-time administrator bootstrap completed.");
    return;
}

builder.Services.AddTransportErpPostgreSql(connectionString);
builder.Services.AddScoped<AuditEventService>();
builder.Services.AddScoped<BootstrapAdminService>();
builder.Services.AddScoped<ISystemPermissionCatalogVerifier, SystemPermissionCatalogVerifier>();
builder.Services.AddSingleton<IOptions<SyncRuntimePolicyOptions>>(Options.Create(syncRuntimePolicy));
builder.Services.AddSingleton<SyncEffectivePolicyResolver>();
builder.Services.AddScoped<SyncOperationService>(services =>
    new SyncOperationService(
        services.GetRequiredService<TransportErpDbContext>(),
        services.GetRequiredService<AuditEventService>(),
        new SyncRetryPolicy(
            syncRuntimePolicy.ServerExecutionMaxRetryCount!.Value,
            TimeSpan.FromSeconds(syncRuntimePolicy.ServerExecutionBaseSeconds!.Value),
            TimeSpan.FromMinutes(syncRuntimePolicy.ServerExecutionMaxDelayMinutes!.Value))));
builder.Services.AddP2C01AWaybillFoundation();
builder.Services.AddP2C01BWaybillFinance();
builder.Services.AddP2C01CShippingExecution();
builder.Services.AddSyncBusinessExecution(syncRuntimePolicy.ServerExecutionEnabled == true);
builder.Services.AddSyncConflictRuntime();
builder.Services.AddScoped<SyncRetentionCleanupService>();
builder.Services.AddHostedService<SyncRetentionCleanupWorker>();

var configuredMode = builder.Configuration["Auth:Mode"] ?? "LocalSessions";
if (!Enum.TryParse<TransportAuthMode>(configuredMode, true, out var authMode))
    throw new InvalidOperationException("Auth:Mode must be LocalSessions or ExternalAuthority.");
var securityOptions = new TransportSecurityOptions
{
    Mode = authMode,
    Authority = builder.Configuration["Auth:Authority"] ?? Environment.GetEnvironmentVariable("TRANSPORTERP_JWT_AUTHORITY"),
    Issuer = builder.Configuration["Auth:Issuer"] ?? Environment.GetEnvironmentVariable("TRANSPORTERP_JWT_ISSUER") ?? "",
    Audience = builder.Configuration["Auth:Audience"] ?? Environment.GetEnvironmentVariable("TRANSPORTERP_JWT_AUDIENCE") ?? "",
    SigningKey = builder.Configuration["Auth:SigningKey"] ?? Environment.GetEnvironmentVariable("TRANSPORTERP_JWT_SIGNING_KEY") ?? "",
    SigningKeyId = builder.Configuration["Auth:SigningKeyId"] ?? Environment.GetEnvironmentVariable("TRANSPORTERP_JWT_SIGNING_KEY_ID") ?? "",
    PreviousSigningKeys = builder.Configuration.GetSection("Auth:PreviousSigningKeys").GetChildren()
        .Where(x => x.Value is not null).ToDictionary(x => x.Key, x => x.Value!, StringComparer.Ordinal),
    AccessTokenMinutes = builder.Configuration.GetValue("Auth:AccessTokenMinutes", 15),
    RefreshTokenDays = builder.Configuration.GetValue("Auth:RefreshTokenDays", 30),
    MaxFailures = builder.Configuration.GetValue("Auth:MaxFailures", 5),
    LockoutMinutes = builder.Configuration.GetValue("Auth:LockoutMinutes", 15),
    LoginRateLimitPermitCount = builder.Configuration.GetValue("Auth:LoginRateLimitPermitCount", 10),
    RefreshRateLimitPermitCount = builder.Configuration.GetValue("Auth:RefreshRateLimitPermitCount", 20),
    RateLimitWindowSeconds = builder.Configuration.GetValue("Auth:RateLimitWindowSeconds", 60),
    RateLimiterMode = builder.Configuration["Auth:RateLimiterMode"] ?? "SingleNode",
    ApplicationInstanceCount = builder.Configuration.GetValue("Auth:ApplicationInstanceCount", 1)
};
var validation = new TransportSecurityOptionsValidator().Validate(null, securityOptions);
if (validation.Failed) throw new OptionsValidationException("Auth", typeof(TransportSecurityOptions), validation.Failures);
builder.Services.AddSingleton<IOptions<TransportSecurityOptions>>(Options.Create(securityOptions));
builder.Services.AddScoped<IEffectivePermissionResolver, EffectivePermissionResolver>();
builder.Services.AddScoped<TenantScopeResolver>();
builder.Services.AddScoped<ICurrentSecurityContext, CurrentSecurityContextService>();
builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddSingleton<IdentityPasswordSentinel>();
builder.Services.AddScoped<IdentitySessionService>();
builder.Services.AddScoped<RegisteredDeviceService>();
builder.Services.AddScoped<OfflineSyncPolicyService>();
builder.Services.AddScoped<ISyncRuntimeGate, ClosedSyncRuntimeGate>();
builder.Services.AddScoped<SyncProofRuntimeService>();
builder.Services.AddScoped<ISyncProofRuntime>(services =>
    services.GetRequiredService<SyncProofRuntimeService>());
builder.Services.AddScoped<SyncProofCleanupService>();
builder.Services.AddHostedService<SyncProofCleanupWorker>();
builder.Services.AddSingleton<SyncPopProofValidator>();
builder.Services.AddScoped<ISyncPopHttpRequestAuthenticator, SyncPopHttpRequestAuthenticator>();
builder.Services.AddScoped<ProofKeyLifecycleService>();
builder.Services.AddSingleton<ProofKeyChangeProofValidator>();
var syncPopDeployment = SyncPopDeploymentProfile.Load(builder.Configuration);
builder.Services.AddSingleton(syncPopDeployment);
builder.Services.Configure<ForwardedHeadersOptions>(syncPopDeployment.ConfigureForwardedHeaders);
builder.Services.AddSingleton<IdentityRateLimiter>();
builder.Services.AddScoped<IAuthorizationHandler, SecurityAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, TransportAuthorizationMiddlewareResultHandler>();
builder.Services.AddScoped<IDeviceTrustResolver, RegisteredDeviceTrustResolver>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
        options.SaveToken = false;
        if (securityOptions.Mode == TransportAuthMode.ExternalAuthority)
        {
            options.Authority = securityOptions.Authority;
            options.Audience = securityOptions.Audience;
        }
        else
        {
            var signingKeys = securityOptions.PreviousSigningKeys
                .Append(new KeyValuePair<string, string>(securityOptions.SigningKeyId, securityOptions.SigningKey))
                .ToDictionary(x => x.Key, x => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(x.Value)) { KeyId = x.Key }, StringComparer.Ordinal);
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKeyResolver = (_, _, kid, _) => kid is not null && signingKeys.TryGetValue(kid, out var key)
                    ? new[] { key }
                    : Array.Empty<SecurityKey>(),
                ValidateIssuer = true,
                ValidIssuer = securityOptions.Issuer,
                ValidateAudience = true,
                ValidAudience = securityOptions.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                NameClaimType = ClaimTypes.NameIdentifier
            };
        }
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(SecurityPolicies.Authenticated, policy =>
        policy.RequireAuthenticatedUser().AddRequirements(new ActiveSecurityContextRequirement()));
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser().AddRequirements(new ActiveSecurityContextRequirement()).Build();
});

var app = builder.Build();
await using (var catalogScope = app.Services.CreateAsyncScope())
{
    await catalogScope.ServiceProvider.GetRequiredService<ISystemPermissionCatalogVerifier>().VerifyAsync();
}
app.UseForwardedHeaders();
app.UseAuthentication();
app.UseAuthorization();
app.MapIdentitySessions(securityOptions.Mode);
app.MapRegisteredDevices();
app.MapRegisteredDeviceProofKeys();
app.MapP2C01AWaybillFoundation();
app.MapP2C01BWaybillFinance();
app.MapP2C01CShippingExecution();
app.MapTransportSync();
app.MapSyncConflictRuntime();

app.MapGet("/api/v1/audit/events", async (
    [AsParameters] AuditQueryRequest request,
    HttpContext httpContext,
    ICurrentSecurityContext currentSecurity,
    AuditEventService audit,
    CancellationToken cancellationToken) =>
{
    var correlationId = GetCorrelationId(httpContext);
    var current = await currentSecurity.ResolveAsync(httpContext.User, cancellationToken);
    if (current is null) return Results.Unauthorized();
    var queryCompanyId = request.CompanyId ?? current.CompanyId;
    var queryBranchId = request.BranchId ?? current.BranchId;
    if (request.CompanyId.HasValue && request.CompanyId != current.CompanyId)
        return Results.Json(new { ErrorCode = "SCOPE_DENIED", CorrelationId = correlationId }, statusCode: StatusCodes.Status403Forbidden);
    if (current.BranchId.HasValue && request.BranchId.HasValue && request.BranchId != current.BranchId)
        return Results.Json(new { ErrorCode = "SCOPE_DENIED", CorrelationId = correlationId }, statusCode: StatusCodes.Status403Forbidden);
    if (request.Take is < 1 or > 1000 || request.Skip < 0 || (request.From.HasValue && request.To.HasValue && request.From >= request.To))
        return Results.BadRequest(new { ErrorCode = "INVALID_FILTER", CorrelationId = correlationId });

    var effectiveQuery = new AuditEventQuery(queryCompanyId, queryBranchId, request.DeviceId, request.Action,
        request.EntityType, request.EntityId, request.From, request.To, request.Skip, request.Take);
    await audit.AppendAuditEventAsync(new AuditEventDraft(
        "AuditEventsRead", "SUCCESS", nameof(AuditEvent), null,
        current.UserId,
        queryCompanyId, queryBranchId, correlationId,
        current.DeviceId, Reason: "ReadAuditEvents"), cancellationToken);
    var items = await audit.GetAuditEventsAsync(effectiveQuery, cancellationToken);
    var total = await audit.CountAuditEventsAsync(effectiveQuery, cancellationToken);
    return Results.Ok(new PagedAuditEventResponse(items.Select(AuditEventResponse.From).ToList(), total,
        request.Skip, request.Take, correlationId));
}).RequireAuthorization(SecurityPolicies.Permission("audit.events.read"));

app.Run();

static Guid GetCorrelationId(HttpContext context)
    => Guid.TryParse(context.Request.Headers["X-Correlation-Id"].FirstOrDefault(), out var id) ? id : Guid.NewGuid();

public partial class Program { }
