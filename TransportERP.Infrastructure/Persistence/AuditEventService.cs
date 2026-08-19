using System.Data;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

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
    string? FailureReason);

public sealed class AuditEventService(TransportErpDbContext db)
{
    public async Task<AuditEvent> AppendAuditEventAsync(
        AuditEventDraft draft,
        CancellationToken cancellationToken = default)
    {
        ValidateDraft(draft);

        await using var transaction = await db.Database.BeginTransactionAsync(
            IsolationLevel.Serializable, cancellationToken);

        var previousHash = await db.AuditEvents
            .AsNoTracking()
            .OrderByDescending(x => x.OccurredAt)
            .ThenByDescending(x => x.Id)
            .Select(x => x.Hash)
            .FirstOrDefaultAsync(cancellationToken);

        var audit = new AuditEvent
        {
            Id = Guid.NewGuid(),
            OccurredAt = NormalizePostgreSqlTimestamp(draft.OccurredAt ?? DateTimeOffset.UtcNow),
            ActorUserId = draft.ActorUserId,
            CompanyId = draft.CompanyId,
            BranchId = draft.BranchId,
            Action = draft.Action.Trim(),
            Outcome = draft.Outcome.Trim(),
            EntityType = draft.EntityType.Trim(),
            EntityId = draft.EntityId,
            CorrelationId = draft.CorrelationId ?? Guid.NewGuid(),
            DeviceId = NullIfWhiteSpace(draft.DeviceId),
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

    public async Task<IReadOnlyList<AuditEvent>> GetAuditEventsAsync(
        AuditEventQuery query,
        CancellationToken cancellationToken = default)
    {
        if (query.Skip < 0) throw new ArgumentOutOfRangeException(nameof(query.Skip));
        if (query.Take is < 1 or > 1000) throw new ArgumentOutOfRangeException(nameof(query.Take));

        IQueryable<AuditEvent> events = db.AuditEvents.AsNoTracking();
        if (query.CompanyId.HasValue) events = events.Where(x => x.CompanyId == query.CompanyId);
        if (query.BranchId.HasValue) events = events.Where(x => x.BranchId == query.BranchId);
        if (!string.IsNullOrWhiteSpace(query.Action)) events = events.Where(x => x.Action == query.Action);
        if (!string.IsNullOrWhiteSpace(query.EntityType)) events = events.Where(x => x.EntityType == query.EntityType);
        if (query.EntityId.HasValue) events = events.Where(x => x.EntityId == query.EntityId);
        if (query.From.HasValue) events = events.Where(x => x.OccurredAt >= query.From);
        if (query.To.HasValue) events = events.Where(x => x.OccurredAt < query.To);

        return await events
            .OrderBy(x => x.OccurredAt)
            .ThenBy(x => x.Id)
            .Skip(query.Skip)
            .Take(query.Take)
            .ToListAsync(cancellationToken);
    }

    public async Task<AuditChainVerificationResult> VerifyHashChainAsync(
        Guid? companyId = null,
        CancellationToken cancellationToken = default)
    {
        var events = await db.AuditEvents
            .AsNoTracking()
            .OrderBy(x => x.OccurredAt)
            .ThenBy(x => x.Id)
            .ToListAsync(cancellationToken);

        string? previousHash = null;
        foreach (var audit in events)
        {
            if (!string.Equals(audit.PreviousHash, previousHash, StringComparison.Ordinal))
                return new(false, events.Count, audit.Id, "PREVIOUS_HASH_MISMATCH");

            if (!string.Equals(audit.Hash, ComputeHash(audit), StringComparison.Ordinal))
                return new(false, events.Count, audit.Id, "HASH_MISMATCH");

            previousHash = audit.Hash;
        }

        if (companyId.HasValue && !events.Any(x => x.CompanyId == companyId))
            return new(false, events.Count, null, "COMPANY_EVENTS_NOT_FOUND");

        return new(true, events.Count, null, null);
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

    private static DateTimeOffset NormalizePostgreSqlTimestamp(DateTimeOffset value)
    {
        var utcTicks = value.UtcDateTime.Ticks;
        var normalizedTicks = utcTicks - (utcTicks % TimeSpan.TicksPerMicrosecond);
        return new DateTimeOffset(new DateTime(normalizedTicks, DateTimeKind.Utc));
    }

    private static string? NullIfWhiteSpace(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}

