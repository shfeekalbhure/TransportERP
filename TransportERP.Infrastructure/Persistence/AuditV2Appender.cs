using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace TransportERP.Infrastructure.Persistence;

public sealed record AuditV2AppendRequest(
    AuditEventDraft Audit,
    string StreamKey,
    Guid OperationId,
    string RetentionClass,
    string Topic,
    short ContractVersion,
    string PayloadJson,
    DateTimeOffset? AvailableAt = null);

public sealed record AuditV2AppendResult(
    AuditEvent AuditEvent,
    IntegrationOutbox Outbox,
    long StreamSequence,
    byte[] HashV2);

public sealed class AuditV2Appender(TransportErpDbContext db)
{
    public async Task<AuditV2AppendResult> AppendAsync(
        AuditV2AppendRequest request,
        CancellationToken cancellationToken = default)
    {
        if (db.Database.CurrentTransaction is null)
            throw new InvalidOperationException("Audit V2 append requires a caller-owned database transaction.");
        if (!request.Audit.CompanyId.HasValue || request.Audit.CompanyId == Guid.Empty)
            throw new ArgumentException("Audit V2 requires CompanyId.", nameof(request));
        if (string.IsNullOrWhiteSpace(request.StreamKey) || request.StreamKey.Length > 300)
            throw new ArgumentException("StreamKey is required and limited to 300 characters.", nameof(request));
        if (request.OperationId == Guid.Empty) throw new ArgumentException("OperationId is required.", nameof(request));
        if (string.IsNullOrWhiteSpace(request.RetentionClass) || request.RetentionClass.Length > 30)
            throw new ArgumentException("RetentionClass is required and limited to 30 characters.", nameof(request));
        if (string.IsNullOrWhiteSpace(request.Topic) || request.Topic.Length > 160)
            throw new ArgumentException("Topic is required and limited to 160 characters.", nameof(request));
        if (request.ContractVersion < 1) throw new ArgumentOutOfRangeException(nameof(request.ContractVersion));

        var companyId = request.Audit.CompanyId.Value;
        var now = NormalizeTimestamp(request.Audit.OccurredAt ?? DateTimeOffset.UtcNow);
        var streamKey = request.StreamKey.Trim().Normalize(NormalizationForm.FormC);
        var payloadBytes = AuditV2Canonicalizer.CanonicalizeJsonToUtf8(request.PayloadJson);
        var payloadDigest = SHA256.HashData(payloadBytes);
        var newHeadId = Guid.NewGuid();

        await db.Database.ExecuteSqlInterpolatedAsync($$"""
            INSERT INTO transport_erp.audit_stream_heads
                ("Id","CompanyId","BranchId","StreamKey","LastSequence","LastHashV2","CreatedAt","UpdatedAt","ConcurrencyVersion")
            VALUES
                ({{newHeadId}},{{companyId}},{{request.Audit.BranchId}},{{streamKey}},0,NULL,{{now}},{{now}},1)
            ON CONFLICT ("CompanyId","StreamKey") DO NOTHING;
            """, cancellationToken);

        var head = await db.Set<AuditStreamHead>()
            .FromSqlInterpolated($$"""
                SELECT "Id","CompanyId","BranchId","StreamKey","LastSequence","LastHashV2","CreatedAt","UpdatedAt","ConcurrencyVersion"
                FROM transport_erp.audit_stream_heads
                WHERE "CompanyId" = {{companyId}} AND "StreamKey" = {{streamKey}}
                FOR UPDATE
                """)
            .SingleAsync(cancellationToken);

        if (head.BranchId != request.Audit.BranchId)
            throw new InvalidOperationException("Audit stream branch scope is immutable and does not match the append request.");

        var sequence = checked(head.LastSequence + 1);
        var previousHashV2 = head.LastHashV2?.ToArray();
        var deviceId = NullIfWhiteSpace(request.Audit.DeviceId);
        var previousLegacyHash = await db.AuditEvents
            .AsNoTracking()
            .Where(x => x.CompanyId == companyId && x.BranchId == request.Audit.BranchId && x.DeviceId == deviceId)
            .OrderByDescending(x => x.OccurredAt)
            .ThenByDescending(x => x.Id)
            .Select(x => x.Hash)
            .FirstOrDefaultAsync(cancellationToken);

        var audit = new AuditEvent
        {
            Id = Guid.NewGuid(),
            OccurredAt = now,
            ActorUserId = request.Audit.ActorUserId,
            CompanyId = companyId,
            BranchId = request.Audit.BranchId,
            Action = RequireTrimmed(request.Audit.Action, 120, nameof(request.Audit.Action)),
            Outcome = RequireTrimmed(request.Audit.Outcome, 40, nameof(request.Audit.Outcome)),
            EntityType = RequireTrimmed(request.Audit.EntityType, 120, nameof(request.Audit.EntityType)),
            EntityId = request.Audit.EntityId,
            CorrelationId = request.Audit.CorrelationId ?? Guid.NewGuid(),
            DeviceId = deviceId,
            BeforeJson = request.Audit.BeforeJson,
            AfterJson = request.Audit.AfterJson,
            Reason = NullIfWhiteSpace(request.Audit.Reason),
            Ip = NullIfWhiteSpace(request.Audit.Ip),
            PreviousHash = previousLegacyHash
        };
        audit.Hash = AuditEventService.ComputeHash(audit);

        var retentionClass = request.RetentionClass.Trim().Normalize(NormalizationForm.FormC);
        var hashV2 = AuditV2Canonicalizer.ComputeHash(new AuditV2CanonicalInput(
            2,
            1,
            streamKey,
            sequence,
            previousHashV2,
            audit.Id,
            audit.OccurredAt,
            audit.ActorUserId,
            companyId,
            audit.BranchId,
            audit.Action,
            audit.Outcome,
            audit.EntityType,
            audit.EntityId,
            audit.CorrelationId,
            request.OperationId,
            audit.DeviceId,
            audit.BeforeJson,
            audit.AfterJson,
            audit.Reason,
            audit.Ip,
            retentionClass,
            payloadDigest));

        db.AuditEvents.Add(audit);
        var entry = db.Entry(audit);
        entry.Property<short>("HashVersion").CurrentValue = 2;
        entry.Property<short>("CanonicalizerVersion").CurrentValue = 1;
        entry.Property<Guid>("StreamHeadId").CurrentValue = head.Id;
        entry.Property<long>("StreamSequence").CurrentValue = sequence;
        entry.Property<byte[]?>("PreviousHashV2").CurrentValue = previousHashV2;
        entry.Property<byte[]>("HashV2").CurrentValue = hashV2;
        entry.Property<byte[]>("PayloadDigest").CurrentValue = payloadDigest;
        entry.Property<Guid>("OperationId").CurrentValue = request.OperationId;
        entry.Property<string>("RetentionClass").CurrentValue = retentionClass;

        head.LastSequence = sequence;
        head.LastHashV2 = hashV2;
        head.UpdatedAt = now;
        head.ConcurrencyVersion = checked(head.ConcurrencyVersion + 1);

        var outbox = new IntegrationOutbox
        {
            Id = Guid.NewGuid(),
            CompanyId = companyId,
            BranchId = request.Audit.BranchId,
            OperationId = request.OperationId,
            Topic = request.Topic.Trim(),
            ContractVersion = request.ContractVersion,
            PayloadJson = Encoding.UTF8.GetString(payloadBytes),
            PayloadSha256 = payloadDigest,
            OccurredAt = now,
            AvailableAt = NormalizeTimestamp(request.AvailableAt ?? now),
            Status = "PENDING",
            AttemptCount = 0,
            CreatedAt = now,
            UpdatedAt = now,
            ConcurrencyVersion = 1
        };
        if (outbox.AvailableAt < now) throw new ArgumentException("AvailableAt cannot precede OccurredAt.", nameof(request));
        db.Set<IntegrationOutbox>().Add(outbox);

        return new AuditV2AppendResult(audit, outbox, sequence, hashV2);
    }

    private static string RequireTrimmed(string value, int maxLength, string name)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException($"{name} is required.", name);
        value = value.Trim().Normalize(NormalizationForm.FormC);
        if (value.Length > maxLength) throw new ArgumentException($"{name} exceeds {maxLength} characters.", name);
        return value;
    }

    private static string? NullIfWhiteSpace(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim().Normalize(NormalizationForm.FormC);

    private static DateTimeOffset NormalizeTimestamp(DateTimeOffset value)
    {
        var utcTicks = value.UtcDateTime.Ticks;
        var normalizedTicks = utcTicks - (utcTicks % TimeSpan.TicksPerMicrosecond);
        return new DateTimeOffset(new DateTime(normalizedTicks, DateTimeKind.Utc));
    }
}
