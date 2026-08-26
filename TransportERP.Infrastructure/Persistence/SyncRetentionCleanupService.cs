using System.Data;
using Microsoft.EntityFrameworkCore;

namespace TransportERP.Infrastructure.Persistence;

public sealed record SyncRetentionCleanupResult(
    int RedactedOperations,
    int RedactedConflictCases,
    Guid? AuditCorrelationId);

/// <summary>
/// Bounded, PostgreSQL-only Stage 4 server retention. Payload and conflict
/// snapshots are replaced one way with an empty JSON object after 90 days;
/// hashes, identifiers, timestamps, status, result metadata and audit history
/// are retained. The client-only 24-hour success and 7-day rejected retention
/// policies are intentionally outside this server cleanup.
/// </summary>
public sealed class SyncRetentionCleanupService(
    TransportErpDbContext db,
    AuditEventService audit)
{
    public const int ServerContentRetentionDays = 90;
    public const int DefaultBatchSize = 250;
    public const int MaximumBatchSize = 2_000;

    public async Task<SyncRetentionCleanupResult> CleanupBatchAsync(
        int batchSize = DefaultBatchSize,
        CancellationToken cancellationToken = default)
    {
        if (batchSize is < 1 or > MaximumBatchSize)
            throw new ArgumentOutOfRangeException(nameof(batchSize));
        if (!db.Database.IsNpgsql())
            throw new InvalidOperationException("SYNC_RETENTION_STORE_UNSUPPORTED");

        await using var transaction = await db.Database.BeginTransactionAsync(
            IsolationLevel.ReadCommitted, cancellationToken);
        try
        {
            var redactedOperations = await db.Database.ExecuteSqlInterpolatedAsync($$"""
                WITH candidates AS (
                  SELECT o."Id"
                  FROM transport_erp.sync_operations o
                  WHERE o."RedactedAt" IS NULL
                    AND o."Status" IN ('SUCCEEDED','REJECTED','RESOLVED')
                    AND o."UpdatedAt"<=clock_timestamp()-INTERVAL '90 days'
                  ORDER BY o."UpdatedAt",o."Id"
                  FOR UPDATE OF o SKIP LOCKED
                  LIMIT {{batchSize}}
                )
                UPDATE transport_erp.sync_operations o
                SET "PayloadJson"='{}',"RedactedAt"=clock_timestamp()
                FROM candidates c
                WHERE o."Id"=c."Id" AND o."RedactedAt" IS NULL
                """, cancellationToken);

            var redactedConflicts = await db.Database.ExecuteSqlInterpolatedAsync($$"""
                WITH candidates AS (
                  SELECT c."Id"
                  FROM transport_erp.conflict_cases c
                  JOIN transport_erp.sync_operations o ON o."Id"=c."SyncOperationId"
                  WHERE c."RedactedAt" IS NULL
                    AND c."Status"='RESOLVED'
                    AND c."ResolvedAt" IS NOT NULL
                    AND c."ResolvedAt"<=clock_timestamp()-INTERVAL '90 days'
                    AND o."Status" IN ('SUCCEEDED','REJECTED','RESOLVED')
                    AND o."UpdatedAt"<=clock_timestamp()-INTERVAL '90 days'
                  ORDER BY c."ResolvedAt",c."Id"
                  FOR UPDATE OF c SKIP LOCKED
                  LIMIT {{batchSize}}
                )
                UPDATE transport_erp.conflict_cases c
                SET "DeviceSnapshot"='{}',"ServerSnapshot"='{}',"RedactedAt"=clock_timestamp()
                FROM candidates selected
                WHERE c."Id"=selected."Id" AND c."RedactedAt" IS NULL
                """, cancellationToken);

            Guid? auditCorrelationId = null;
            if (redactedOperations != 0 || redactedConflicts != 0)
            {
                auditCorrelationId = Guid.NewGuid();
                await audit.AppendAuditEventAsync(new AuditEventDraft(
                    "SyncRetentionContentRedacted",
                    "SUCCESS",
                    "SyncRetentionBatch",
                    CorrelationId: auditCorrelationId,
                    Reason: $"RetentionDays={ServerContentRetentionDays};" +
                            $"Operations={redactedOperations};ConflictCases={redactedConflicts}"),
                    cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);
            db.ChangeTracker.Clear();
            return new(redactedOperations, redactedConflicts, auditCorrelationId);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            db.ChangeTracker.Clear();
            throw;
        }
    }
}
