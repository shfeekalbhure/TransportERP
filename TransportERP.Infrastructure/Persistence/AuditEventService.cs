using System.Data;
using System.Globalization;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace TransportERP.Infrastructure.Persistence;

public sealed record AuditEventDraft(
    string Action,
    string Outcome,
    string EntityType,
    Guid? EntityId = null,
    Guid? ActorUserId = null,
    Guid? CompanyId = null,
    Guid? BranchId = null,
    Guid? CorrelationId = null,
    string? DeviceId = null,
    string? BeforeJson = null,
    string? AfterJson = null,
    string? Reason = null,
    string? Ip = null,
    DateTimeOffset? OccurredAt = null);

public sealed record AuditEventQuery(
    Guid? CompanyId = null,
    Guid? BranchId = null,
    string? DeviceId = null,
    string? Action = null,
    string? EntityType = null,
    Guid? EntityId = null,
    DateTimeOffset? From = null,
    DateTimeOffset? To = null,
    int Skip = 0,
    int Take = 100);

public sealed record AuditChainVerificationResult(
    bool IsValid,
    int EventCount,
    Guid? FirstInvalidEventId,
    string? FailureReason,
    string? StreamKey = null);

public sealed class AuditEventService(TransportErpDbContext db)
{
    public async Task<AuditEvent> AppendAuditEventAsync(
        AuditEventDraft draft,
        CancellationToken cancellationToken = default)
    {
        ValidateDraft(draft);

        for (var attempt = 1; ; attempt++)
        {
            try
            {
                return await AppendOnceAsync(draft, cancellationToken);
            }
            catch (Exception ex) when (IsSerializationFailure(ex) && attempt < 6)
            {
                db.ChangeTracker.Clear();
                await Task.Delay(TimeSpan.FromMilliseconds(25 * attempt), cancellationToken);
            }
        }
    }

    public async Task<IReadOnlyList<AuditEvent>> GetAuditEventsAsync(
        AuditEventQuery query,
        CancellationToken cancellationToken = default)
    {
        if (query.Skip < 0) throw new ArgumentOutOfRangeException(nameof(query.Skip));
        if (query.Take is < 1 or > 1000) throw new ArgumentOutOfRangeException(nameof(query.Take));

        IQueryable<AuditEvent> events = db.AuditEvents.AsNoTracking();
        if (query.CompanyId.HasValue) events = events.Where(x => x.CompanyId == query.CompanyId);
        if (query.BranchId.HasValue) events = events.Where(x => x.BranchId == query.BranchId);
        if (query.DeviceId is not null) events = events.Where(x => x.DeviceId == query.DeviceId);
        if (!string.IsNullOrWhiteSpace(query.Action)) events = events.Where(x => x.Action == query.Action);
        if (!string.IsNullOrWhiteSpace(query.EntityType)) events = events.Where(x => x.EntityType == query.EntityType);
        if (query.EntityId.HasValue) events = events.Where(x => x.EntityId == query.EntityId);
        if (query.From.HasValue) events = events.Where(x => x.OccurredAt >= NormalizePostgreSqlTimestamp(query.From.Value));
        if (query.To.HasValue) events = events.Where(x => x.OccurredAt < NormalizePostgreSqlTimestamp(query.To.Value));

        return await events
            .OrderBy(x => x.OccurredAt)
            .ThenBy(x => x.Id)
            .Skip(query.Skip)
            .Take(query.Take)
            .ToListAsync(cancellationToken);
    }

    public async Task<long> CountAuditEventsAsync(
        AuditEventQuery query,
        CancellationToken cancellationToken = default)
    {
        if (query.Skip < 0) throw new ArgumentOutOfRangeException(nameof(query.Skip));
        if (query.Take is < 1 or > 1000) throw new ArgumentOutOfRangeException(nameof(query.Take));
        IQueryable<AuditEvent> events = ApplyFilters(db.AuditEvents.AsNoTracking(), query);
        return await events.LongCountAsync(cancellationToken);
    }

    public async Task<AuditChainVerificationResult> VerifyHashChainAsync(
        Guid? companyId = null,
        Guid? branchId = null,
        string? deviceId = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<AuditEvent> query = db.AuditEvents.AsNoTracking();
        if (companyId.HasValue) query = query.Where(x => x.CompanyId == companyId);
        if (branchId.HasValue) query = query.Where(x => x.BranchId == branchId);
        if (deviceId is not null) query = query.Where(x => x.DeviceId == deviceId);

        var events = await query
            .OrderBy(x => x.OccurredAt)
            .ThenBy(x => x.Id)
            .ToListAsync(cancellationToken);

        if (companyId.HasValue || branchId.HasValue || deviceId is not null)
            return VerifyStream(events, GetStreamKey(companyId, branchId, deviceId));

        var total = events.Count;
        foreach (var group in events.GroupBy(x => GetStreamKey(x.CompanyId, x.BranchId, x.DeviceId)))
        {
            var result = VerifyStream(group.OrderBy(x => x.OccurredAt).ThenBy(x => x.Id).ToList(), group.Key);
            if (!result.IsValid)
                return result with { EventCount = total };
        }

        return new(true, total, null, null, null);
    }

    public Task<IReadOnlyList<AuditEvent>> ExportAuditEventsAsync(
        AuditEventQuery query,
        CancellationToken cancellationToken = default)
        => GetAuditEventsAsync(query with { Skip = 0, Take = Math.Min(query.Take, 1000) }, cancellationToken);

    public static string ComputeHash(AuditEvent audit)
    {
        var canonical = string.Join("|",
            audit.Id,
            audit.Action,
            audit.EntityId?.ToString() ?? string.Empty,
            audit.ActorUserId?.ToString() ?? string.Empty,
            audit.CompanyId?.ToString() ?? string.Empty,
            audit.BranchId?.ToString() ?? string.Empty,
            audit.CorrelationId,
            audit.OccurredAt.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture),
            audit.Outcome,
            audit.Reason ?? string.Empty,
            audit.PreviousHash ?? string.Empty);

        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(canonical))).ToLowerInvariant();
    }

    public static string GetStreamKey(Guid? companyId, Guid? branchId, string? deviceId)
        => string.Join("|", companyId?.ToString() ?? string.Empty, branchId?.ToString() ?? string.Empty,
            deviceId?.Trim() ?? string.Empty);

    private async Task<AuditEvent> AppendOnceAsync(AuditEventDraft draft, CancellationToken cancellationToken)
    {
        await using var transaction = await db.Database.BeginTransactionAsync(
            IsolationLevel.Serializable, cancellationToken);

        var occurredAt = NormalizePostgreSqlTimestamp(draft.OccurredAt ?? DateTimeOffset.UtcNow);
        var deviceId = NullIfWhiteSpace(draft.DeviceId);
        var previousHash = await db.AuditEvents
            .AsNoTracking()
            .Where(x => x.CompanyId == draft.CompanyId && x.BranchId == draft.BranchId && x.DeviceId == deviceId)
            .OrderByDescending(x => x.OccurredAt)
            .ThenByDescending(x => x.Id)
            .Select(x => x.Hash)
            .FirstOrDefaultAsync(cancellationToken);

        var audit = new AuditEvent
        {
            Id = Guid.NewGuid(),
            OccurredAt = occurredAt,
            ActorUserId = draft.ActorUserId,
            CompanyId = draft.CompanyId,
            BranchId = draft.BranchId,
            Action = draft.Action.Trim(),
            Outcome = draft.Outcome.Trim(),
            EntityType = draft.EntityType.Trim(),
            EntityId = draft.EntityId,
            CorrelationId = draft.CorrelationId ?? Guid.NewGuid(),
            DeviceId = deviceId,
            BeforeJson = draft.BeforeJson,
            AfterJson = draft.AfterJson,
            Reason = NullIfWhiteSpace(draft.Reason),
            Ip = NullIfWhiteSpace(draft.Ip),
            PreviousHash = previousHash
        };
        audit.Hash = ComputeHash(audit);

        db.AuditEvents.Add(audit);
        await db.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return audit;
    }

    private static IQueryable<AuditEvent> ApplyFilters(IQueryable<AuditEvent> events, AuditEventQuery query)
    {
        if (query.CompanyId.HasValue) events = events.Where(x => x.CompanyId == query.CompanyId);
        if (query.BranchId.HasValue) events = events.Where(x => x.BranchId == query.BranchId);
        if (query.DeviceId is not null) events = events.Where(x => x.DeviceId == query.DeviceId);
        if (!string.IsNullOrWhiteSpace(query.Action)) events = events.Where(x => x.Action == query.Action);
        if (!string.IsNullOrWhiteSpace(query.EntityType)) events = events.Where(x => x.EntityType == query.EntityType);
        if (query.EntityId.HasValue) events = events.Where(x => x.EntityId == query.EntityId);
        if (query.From.HasValue) events = events.Where(x => x.OccurredAt >= NormalizePostgreSqlTimestamp(query.From.Value));
        if (query.To.HasValue) events = events.Where(x => x.OccurredAt < NormalizePostgreSqlTimestamp(query.To.Value));
        return events;
    }

    private static AuditChainVerificationResult VerifyStream(IReadOnlyList<AuditEvent> events, string streamKey)
    {
        string? previousHash = null;
        foreach (var audit in events)
        {
            if (!string.Equals(audit.PreviousHash, previousHash, StringComparison.Ordinal))
                return new(false, events.Count, audit.Id, "PREVIOUS_HASH_MISMATCH", streamKey);
            if (!string.Equals(audit.Hash, ComputeHash(audit), StringComparison.Ordinal))
                return new(false, events.Count, audit.Id, "HASH_MISMATCH", streamKey);
            previousHash = audit.Hash;
        }
        return new(true, events.Count, null, null, streamKey);
    }

    private static void ValidateDraft(AuditEventDraft draft)
    {
        if (string.IsNullOrWhiteSpace(draft.Action)) throw new ArgumentException("Action is required.", nameof(draft));
        if (string.IsNullOrWhiteSpace(draft.Outcome)) throw new ArgumentException("Outcome is required.", nameof(draft));
        if (string.IsNullOrWhiteSpace(draft.EntityType)) throw new ArgumentException("EntityType is required.", nameof(draft));
        if (draft.Action.Length > 120) throw new ArgumentException("Action exceeds 120 characters.", nameof(draft));
        if (draft.Outcome.Length > 40) throw new ArgumentException("Outcome exceeds 40 characters.", nameof(draft));
        if (draft.EntityType.Length > 120) throw new ArgumentException("EntityType exceeds 120 characters.", nameof(draft));
        if (draft.Reason?.Length > 500) throw new ArgumentException("Reason exceeds 500 characters.", nameof(draft));
        if (draft.Ip?.Length > 64) throw new ArgumentException("Ip exceeds 64 characters.", nameof(draft));
        if (draft.DeviceId?.Length > 120) throw new ArgumentException("DeviceId exceeds 120 characters.", nameof(draft));
    }

    private static bool IsSerializationFailure(Exception exception)
    {
        for (var current = exception; current is not null; current = current.InnerException)
        {
            if (current is PostgresException { SqlState: "40001" or "40P01" })
                return true;
        }

        return false;
    }

    private static DateTimeOffset NormalizePostgreSqlTimestamp(DateTimeOffset value)
    {
        var utcTicks = value.UtcDateTime.Ticks;
        var normalizedTicks = utcTicks - (utcTicks % TimeSpan.TicksPerMicrosecond);
        return new DateTimeOffset(new DateTime(normalizedTicks, DateTimeKind.Utc));
    }

    private static string? NullIfWhiteSpace(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}

public sealed record AuditQueryRequest(
    Guid? CompanyId = null,
    Guid? BranchId = null,
    string? DeviceId = null,
    string? Action = null,
    string? EntityType = null,
    Guid? EntityId = null,
    DateTimeOffset? From = null,
    DateTimeOffset? To = null,
    int Skip = 0,
    int Take = 100,
    Guid? CorrelationId = null);

public sealed record AuditEventResponse(
    Guid Id,
    DateTimeOffset OccurredAt,
    Guid? ActorUserId,
    Guid? CompanyId,
    Guid? BranchId,
    string Action,
    string Outcome,
    string EntityType,
    Guid? EntityId,
    Guid CorrelationId,
    string? DeviceId,
    string? BeforeJson,
    string? AfterJson,
    string? Reason,
    string? Ip,
    string Hash,
    string? PreviousHash)
{
    public static AuditEventResponse From(AuditEvent x) => new(x.Id, x.OccurredAt, x.ActorUserId,
        x.CompanyId, x.BranchId, x.Action, x.Outcome, x.EntityType, x.EntityId, x.CorrelationId,
        x.DeviceId, x.BeforeJson, x.AfterJson, x.Reason, x.Ip, x.Hash, x.PreviousHash);
}

public sealed record PagedAuditEventResponse(
    IReadOnlyList<AuditEventResponse> Items,
    long TotalCount,
    int Skip,
    int Take,
    Guid CorrelationId);

public sealed record AuditScope(
    Guid? CompanyId,
    Guid? BranchId,
    bool IsPlatformScope,
    string? DeviceId);

public static class AuditErrorCodes
{
    public const string ScopeDenied = "SCOPE_DENIED";
    public const string InvalidFilter = "INVALID_FILTER";
    public const string PermissionDenied = "PERMISSION_DENIED";
}

public static class AuditEventApi
{
    public static AuditScope? ResolveScope(ClaimsPrincipal user, AuditQueryRequest request)
    {
        var platform = string.Equals(user.FindFirst("scope")?.Value, "platform", StringComparison.OrdinalIgnoreCase) ||
                       string.Equals(user.FindFirst("platform_access")?.Value, "true", StringComparison.OrdinalIgnoreCase);
        var claimCompany = TryGetGuid(user, "company_id");
        var claimBranch = TryGetGuid(user, "branch_id");
        if (!platform && !claimCompany.HasValue) return null;
        if (!platform && request.CompanyId.HasValue && request.CompanyId != claimCompany) return null;
        if (!platform && request.BranchId.HasValue && request.BranchId != claimBranch) return null;
        if (!platform && claimBranch.HasValue && request.BranchId.HasValue == false)
            request = request with { BranchId = claimBranch };
        return new AuditScope(platform ? request.CompanyId : claimCompany,
            platform ? request.BranchId : request.BranchId ?? claimBranch,
            platform,
            user.FindFirst("device_id")?.Value);
    }

    private static Guid? TryGetGuid(ClaimsPrincipal user, string type)
        => Guid.TryParse(user.FindFirst(type)?.Value, out var value) ? value : null;
}
