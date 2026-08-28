using System.Data;
using Microsoft.EntityFrameworkCore;

namespace TransportERP.Infrastructure.Persistence;

public sealed record SyncProofCleanupResult(int DeletedReplays, int DeletedNonces);

/// <summary>
/// PostgreSQL retention boundary for Sync-PoP artifacts. It deletes expired replay
/// rows first, then expired nonce rows that are no longer referenced. It never
/// reads or emits raw nonce, jti, proof, token, or request-body material.
/// </summary>
public sealed class SyncProofCleanupService(TransportErpDbContext db)
{
    public const int DefaultBatchSize = 1_000;

    public async Task<SyncProofCleanupResult> CleanupExpiredAsync(
        DateTimeOffset serverNow,
        int batchSize = DefaultBatchSize,
        CancellationToken cancellationToken = default)
    {
        if (batchSize is < 1 or > 10_000) throw new ArgumentOutOfRangeException(nameof(batchSize));
        var cutoff = NormalizeTimestamp(serverNow);
        var retentionCutoff = NormalizeTimestamp(cutoff - SyncProofRuntimeService.ReplayRetention);
        await using var transaction = await db.Database.BeginTransactionAsync(
            IsolationLevel.ReadCommitted, cancellationToken);
        try
        {
            await db.Database.ExecuteSqlRawAsync(
                "SELECT pg_advisory_xact_lock(hashtextextended('sync-proof-cleanup-v1', 0))",
                cancellationToken);
            var deletedReplays = await db.Database.ExecuteSqlInterpolatedAsync($$"""
                WITH candidates AS (
                  SELECT r."Id"
                  FROM transport_erp.sync_proof_replays r
                  WHERE r."ExpiresAt" <= {{cutoff}}
                    AND r."FirstSeenAt" <= {{retentionCutoff}}
                  ORDER BY r."ExpiresAt", r."Id"
                  FOR UPDATE SKIP LOCKED
                  LIMIT {{batchSize}}
                )
                DELETE FROM transport_erp.sync_proof_replays r
                USING candidates c
                WHERE r."Id" = c."Id"
                """, cancellationToken);
            var deletedNonces = await db.Database.ExecuteSqlInterpolatedAsync($$"""
                WITH candidates AS (
                  SELECT n."Id"
                  FROM transport_erp.sync_proof_nonces n
                  WHERE n."ExpiresAt" <= {{cutoff}}
                    AND NOT EXISTS (
                      SELECT 1 FROM transport_erp.sync_proof_replays r
                      WHERE r."NonceRecordId" = n."Id"
                    )
                  ORDER BY n."ExpiresAt", n."Id"
                  FOR UPDATE SKIP LOCKED
                  LIMIT {{batchSize}}
                )
                DELETE FROM transport_erp.sync_proof_nonces n
                USING candidates c
                WHERE n."Id" = c."Id"
                """, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            db.ChangeTracker.Clear();
            return new SyncProofCleanupResult(deletedReplays, deletedNonces);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            db.ChangeTracker.Clear();
            throw;
        }
    }

    private static DateTimeOffset NormalizeTimestamp(DateTimeOffset value)
    {
        var ticks = value.UtcDateTime.Ticks;
        return new DateTimeOffset(new DateTime(
            ticks - ticks % TimeSpan.TicksPerMicrosecond, DateTimeKind.Utc));
    }
}
