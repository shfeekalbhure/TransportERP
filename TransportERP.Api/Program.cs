using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TransportERP.Api.Wave1;
using TransportERP.Api.Waybills;
using TransportERP.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("TransportErp")
    ?? Environment.GetEnvironmentVariable("TRANSPORTERP_CONNECTION_STRING")
    ?? throw new InvalidOperationException("Transport ERP database connection is not configured.");

builder.Services.AddTransportErpPostgreSql(connectionString);
builder.Services.AddScoped<AuditEventService>();
builder.Services.AddScoped<SyncOperationService>(services =>
    new SyncOperationService(
        services.GetRequiredService<TransportErpDbContext>(),
        services.GetRequiredService<AuditEventService>(),
        new SyncRetryPolicy(
            builder.Configuration.GetValue("Sync:MaxRetryCount", 5),
            TimeSpan.FromSeconds(builder.Configuration.GetValue("Sync:BaseRetrySeconds", 5)),
            TimeSpan.FromMinutes(builder.Configuration.GetValue("Sync:MaxRetryMinutes", 30)))));
builder.Services.AddP2C01AWaybillFoundation();
builder.Services.AddP2C01BWaybillFinance();
builder.Services.AddP2C01CShippingExecution();

var jwtAuthority = builder.Configuration["Auth:Authority"] ?? Environment.GetEnvironmentVariable("TRANSPORTERP_JWT_AUTHORITY");
var jwtIssuer = builder.Configuration["Auth:Issuer"] ?? Environment.GetEnvironmentVariable("TRANSPORTERP_JWT_ISSUER");
var jwtAudience = builder.Configuration["Auth:Audience"] ?? Environment.GetEnvironmentVariable("TRANSPORTERP_JWT_AUDIENCE");
var jwtSigningKey = builder.Configuration["Auth:SigningKey"] ?? Environment.GetEnvironmentVariable("TRANSPORTERP_JWT_SIGNING_KEY");
if (string.IsNullOrWhiteSpace(jwtAudience))
    throw new InvalidOperationException("Auth:Audience or TRANSPORTERP_JWT_AUDIENCE is required.");
if (string.IsNullOrWhiteSpace(jwtAuthority) && (string.IsNullOrWhiteSpace(jwtIssuer) || string.IsNullOrWhiteSpace(jwtSigningKey)))
    throw new InvalidOperationException("Configure Auth:Authority or both Auth:Issuer and Auth:SigningKey.");
if (string.IsNullOrWhiteSpace(jwtAuthority) && jwtSigningKey!.Length < 32)
    throw new InvalidOperationException("Auth:SigningKey must contain at least 32 characters.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
        options.SaveToken = false;
        if (!string.IsNullOrWhiteSpace(jwtAuthority))
        {
            options.Authority = jwtAuthority;
            options.Audience = jwtAudience;
        }
        else
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSigningKey!)),
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,
                ValidateAudience = true,
                ValidAudience = jwtAudience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                NameClaimType = ClaimTypes.NameIdentifier,
                RoleClaimType = "permission"
            };
        }
    });
builder.Services.AddAuthorization(options =>
    options.AddPolicy("Authenticated", policy => policy.RequireAuthenticatedUser()));

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapWave1ScreenCatalog();
app.MapP2C01AWaybillFoundation();
app.MapP2C01BWaybillFinance();
app.MapP2C01CShippingExecution();

app.MapPost("/api/v1/sync/operations:batch", async (
    SyncBatchRequest request,
    HttpContext httpContext,
    SyncOperationService sync,
    CancellationToken cancellationToken) =>
{
    var correlationId = GetCorrelationId(httpContext);
    if (request is null || request.Operations is null || request.Operations.Count is < 1 or > 100)
        return Results.BadRequest(new { ErrorCode = "BATCH_SIZE_INVALID", CorrelationId = correlationId });
    if (string.IsNullOrWhiteSpace(request.DeviceId) || string.IsNullOrWhiteSpace(request.ProtocolVersion))
        return Results.BadRequest(new { ErrorCode = "BATCH_METADATA_REQUIRED", CorrelationId = correlationId });
    if (httpContext.User.Identity?.IsAuthenticated != true)
        return Results.Unauthorized();

    if (!TryGetGuidClaim(httpContext.User, ClaimTypes.NameIdentifier, "sub", out var userId) ||
        !TryGetGuidClaim(httpContext.User, "company_id", null, out var companyId))
        return Results.Unauthorized();

    Guid? branchId = TryGetGuidClaim(httpContext.User, "branch_id", null, out var parsedBranch) ? parsedBranch : null;
    var claimedDeviceId = httpContext.User.FindFirstValue("device_id");
    var isDeviceRegistered = string.Equals(
        httpContext.User.FindFirstValue("device_registered"), "true", StringComparison.OrdinalIgnoreCase);
    if (!isDeviceRegistered || string.IsNullOrWhiteSpace(claimedDeviceId) ||
        !string.Equals(claimedDeviceId, request.DeviceId.Trim(), StringComparison.Ordinal))
        return Results.Json(new { ErrorCode = "DEVICE_NOT_REGISTERED", CorrelationId = correlationId }, statusCode: StatusCodes.Status403Forbidden);

    var hasPermission = HasPermission(httpContext.User, "sync.operations.execute");
    if (!hasPermission)
        return Results.Json(new { ErrorCode = "SCOPE_DENIED", CorrelationId = correlationId }, statusCode: StatusCodes.Status403Forbidden);

    var security = new SyncSecurityContext(userId, request.DeviceId.Trim(), companyId, branchId, true, true);
    var results = new List<SyncBatchOperationResult>(request.Operations.Count);

    foreach (var item in request.Operations)
    {
        var serverTime = DateTimeOffset.UtcNow;
        try
        {
            if (!Guid.TryParse(item.EntityId, out var entityId))
                throw new SyncRuleException("PAYLOAD_INVALID", item.ClientOperationId ?? "");

            var command = new EnqueueSyncOperationCommand(
                request.DeviceId, userId, companyId, branchId,
                item.OperationType, item.EntityType, entityId, item.ClientOperationId,
                item.PayloadJson, item.PayloadHash, item.ClientOccurredAt, item.BaseVersion);
            var operation = await sync.EnqueueSyncOperationAsync(command, security, cancellationToken);
            results.Add(SyncBatchOperationResult.From(operation, serverTime));
        }
        catch (SyncRuleException ex)
        {
            results.Add(new SyncBatchOperationResult(
                item.ClientOperationId, null, "REJECTED", null, ex.Code, null, serverTime));
        }
        catch (ArgumentException)
        {
            results.Add(new SyncBatchOperationResult(
                item.ClientOperationId, null, "REJECTED", null, "PAYLOAD_INVALID", null, serverTime));
        }
        catch (Exception)
        {
            results.Add(new SyncBatchOperationResult(
                item.ClientOperationId, null, "FAILED", null, "INTERNAL_ERROR", null, serverTime));
        }
    }

    return Results.Ok(new SyncBatchResponse(request.ProtocolVersion, results, DateTimeOffset.UtcNow, correlationId));
}).RequireAuthorization("Authenticated");

app.MapGet("/api/v1/audit/events", async (
    [AsParameters] AuditQueryRequest request,
    HttpContext httpContext,
    AuditEventService audit,
    CancellationToken cancellationToken) =>
{
    var correlationId = GetCorrelationId(httpContext);
    if (httpContext.User.Identity?.IsAuthenticated != true)
        return Results.Unauthorized();
    if (!HasPermission(httpContext.User, "audit.events.read"))
        return Results.Json(new { ErrorCode = "SCOPE_DENIED", CorrelationId = correlationId }, statusCode: StatusCodes.Status403Forbidden);

    var platform = IsPlatformScope(httpContext.User);
    var claimCompanyId = TryGetGuidClaim(httpContext.User, "company_id", null, out var companyId) ? companyId : (Guid?)null;
    var claimBranchId = TryGetGuidClaim(httpContext.User, "branch_id", null, out var branchId) ? branchId : (Guid?)null;
    var queryCompanyId = request.CompanyId ?? (!platform ? claimCompanyId : null);
    var queryBranchId = request.BranchId ?? (!platform ? claimBranchId : null);
    if (!platform && !claimCompanyId.HasValue)
        return Results.Json(new { ErrorCode = "SCOPE_DENIED", CorrelationId = correlationId }, statusCode: StatusCodes.Status403Forbidden);
    if (!platform && request.CompanyId.HasValue && request.CompanyId != claimCompanyId)
        return Results.Json(new { ErrorCode = "SCOPE_DENIED", CorrelationId = correlationId }, statusCode: StatusCodes.Status403Forbidden);
    if (!platform && claimBranchId.HasValue && request.BranchId.HasValue && request.BranchId != claimBranchId)
        return Results.Json(new { ErrorCode = "SCOPE_DENIED", CorrelationId = correlationId }, statusCode: StatusCodes.Status403Forbidden);
    if (request.Take is < 1 or > 1000 || request.Skip < 0 || (request.From.HasValue && request.To.HasValue && request.From >= request.To))
        return Results.BadRequest(new { ErrorCode = "INVALID_FILTER", CorrelationId = correlationId });

    var effectiveQuery = new AuditEventQuery(queryCompanyId, queryBranchId, request.DeviceId, request.Action,
        request.EntityType, request.EntityId, request.From, request.To, request.Skip, request.Take);
    await audit.AppendAuditEventAsync(new AuditEventDraft(
        "AuditEventsRead", "SUCCESS", nameof(AuditEvent), null,
        TryGetGuidClaim(httpContext.User, ClaimTypes.NameIdentifier, "sub", out var actorId) ? actorId : null,
        queryCompanyId, queryBranchId, correlationId,
        httpContext.User.FindFirstValue("device_id"), Reason: "ReadAuditEvents"), cancellationToken);
    var items = await audit.GetAuditEventsAsync(effectiveQuery, cancellationToken);
    var total = await audit.CountAuditEventsAsync(effectiveQuery, cancellationToken);
    return Results.Ok(new PagedAuditEventResponse(items.Select(AuditEventResponse.From).ToList(), total,
        request.Skip, request.Take, correlationId));
}).RequireAuthorization("Authenticated");

app.Run();

static bool TryGetGuidClaim(ClaimsPrincipal principal, string firstType, string? secondType, out Guid value)
{
    var raw = principal.FindFirstValue(firstType) ?? (secondType is null ? null : principal.FindFirstValue(secondType));
    return Guid.TryParse(raw, out value);
}

static bool HasPermission(ClaimsPrincipal principal, string permission)
    => principal.Claims.Any(x => x.Type is "permission" or ClaimTypes.Role &&
        string.Equals(x.Value, permission, StringComparison.OrdinalIgnoreCase));

static bool IsPlatformScope(ClaimsPrincipal principal)
    => string.Equals(principal.FindFirstValue("scope"), "platform", StringComparison.OrdinalIgnoreCase) ||
       string.Equals(principal.FindFirstValue("platform_access"), "true", StringComparison.OrdinalIgnoreCase);

static Guid GetCorrelationId(HttpContext context)
    => Guid.TryParse(context.Request.Headers["X-Correlation-Id"].FirstOrDefault(), out var id) ? id : Guid.NewGuid();

public sealed record SyncBatchRequest(
    string DeviceId,
    string ProtocolVersion,
    IReadOnlyList<SyncBatchOperationRequest> Operations);

public sealed record SyncBatchOperationRequest(
    string OperationType,
    string EntityType,
    string EntityId,
    string ClientOperationId,
    string PayloadJson,
    string PayloadHash,
    DateTimeOffset ClientOccurredAt,
    long? BaseVersion = null);

public sealed record SyncBatchOperationResult(
    string? ClientOperationId,
    Guid? ServerOperationId,
    string Status,
    long? ResultVersion,
    string? ErrorCode,
    Guid? ConflictCaseId,
    DateTimeOffset ServerTime)
{
    public static SyncBatchOperationResult From(SyncOperation operation, DateTimeOffset serverTime)
        => new(operation.ClientOperationId, operation.Id, operation.Status, operation.ResultVersion,
            operation.ErrorCode, operation.ConflictCase?.Id, serverTime);
}

public sealed record SyncBatchResponse(
    string ProtocolVersion,
    IReadOnlyList<SyncBatchOperationResult> Results,
    DateTimeOffset ServerTime,
    Guid CorrelationId);

public partial class Program { }
