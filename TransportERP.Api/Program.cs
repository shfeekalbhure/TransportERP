using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using TransportERP.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("TransportErp")
    ?? Environment.GetEnvironmentVariable("TRANSPORTERP_CONNECTION_STRING")
    ?? throw new InvalidOperationException("Transport ERP database connection is not configured.");

builder.Services.AddDbContext<TransportErpDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<AuditEventService>();
builder.Services.AddScoped<SyncOperationService>(services =>
    new SyncOperationService(
        services.GetRequiredService<TransportErpDbContext>(),
        services.GetRequiredService<AuditEventService>(),
        new SyncRetryPolicy(
            builder.Configuration.GetValue("Sync:MaxRetryCount", 5),
            TimeSpan.FromSeconds(builder.Configuration.GetValue("Sync:BaseRetrySeconds", 5)),
            TimeSpan.FromMinutes(builder.Configuration.GetValue("Sync:MaxRetryMinutes", 30)))));
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/api/v1/sync/operations:batch", async (
    SyncBatchRequest request,
    HttpContext httpContext,
    SyncOperationService sync,
    CancellationToken cancellationToken) =>
{
    if (request is null || request.Operations is null || request.Operations.Count is < 1 or > 100)
        return Results.BadRequest(new { ErrorCode = "BATCH_SIZE_INVALID" });
    if (string.IsNullOrWhiteSpace(request.DeviceId) || string.IsNullOrWhiteSpace(request.ProtocolVersion))
        return Results.BadRequest(new { ErrorCode = "BATCH_METADATA_REQUIRED" });

    if (!TryGetGuidClaim(httpContext.User, ClaimTypes.NameIdentifier, "sub", out var userId) ||
        !TryGetGuidClaim(httpContext.User, "company_id", null, out var companyId))
        return Results.Unauthorized();

    Guid? branchId = TryGetGuidClaim(httpContext.User, "branch_id", null, out var parsedBranch) ? parsedBranch : null;
    var isDeviceRegistered = string.Equals(
        httpContext.User.FindFirstValue("device_registered"), "true", StringComparison.OrdinalIgnoreCase);
    var hasPermission = httpContext.User.Claims.Any(x =>
        (x.Type is ClaimTypes.Role or "permission") &&
        string.Equals(x.Value, "sync.operations.execute", StringComparison.OrdinalIgnoreCase));
    if (!hasPermission)
        hasPermission = string.Equals(httpContext.User.FindFirstValue("sync_operations_execute"), "true", StringComparison.OrdinalIgnoreCase);

    var security = new SyncSecurityContext(userId, request.DeviceId.Trim(), companyId, branchId,
        isDeviceRegistered, hasPermission);
    var results = new List<SyncBatchOperationResult>(request.Operations.Count);

    foreach (var item in request.Operations)
    {
        var serverTime = DateTimeOffset.UtcNow;
        try
        {
            if (!Guid.TryParse(item.EntityId, out var entityId))
                throw new SyncRuleException("ENTITY_ID_INVALID", item.ClientOperationId ?? "");

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

    return Results.Ok(new SyncBatchResponse(request.ProtocolVersion, results, DateTimeOffset.UtcNow));
});

app.Run();

static bool TryGetGuidClaim(ClaimsPrincipal principal, string firstType, string? secondType, out Guid value)
{
    var raw = principal.FindFirstValue(firstType) ?? (secondType is null ? null : principal.FindFirstValue(secondType));
    return Guid.TryParse(raw, out value);
}


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
    DateTimeOffset ServerTime);

public partial class Program { }
