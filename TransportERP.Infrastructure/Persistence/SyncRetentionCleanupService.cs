using System.Data;
using Microsoft.EntityFrameworkCore;
using TransportERP.Application.Sync;

namespace TransportERP.Infrastructure.Persistence;

public sealed record SyncRetentionCleanupResult(
    int RedactedOperations,
    int RedactedConflictCases,
    Guid? AuditCorrelationId);

/// <summary>
/// Bounded, PostgreSQL-only Stage 4 server retention. Payload and conflict
/// snapshots are replaced one way with an empty JSON object after the current
/// effective Global -&gt; Company -&gt; Branch -&gt; Device retention period;
/// hashes, identifiers, timestamps, status, result metadata and audit history
/// are retained. The client-only 24-hour success and 7-day rejected retention
/// policies are intentionally outside this server cleanup.
/// </summary>
public sealed class SyncRetentionCleanupService(
    TransportErpDbContext db,
    AuditEventService audit,
    IEffectiveSyncRetentionPolicyProvider effectivePolicies)
{
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
            var redactedOperations = 0;
            var appliedPolicies = new List<EffectiveSyncRetentionPolicy>();
            var operationCursor = string.Empty;
            while (redactedOperations < batchSize)
            {
                var operationScopes = await OperationScopePageAsync(
                    operationCursor, batchSize, cancellationToken);
                if (operationScopes.Count == 0) break;
                operationCursor = operationScopes[^1].ScopeKey;
                foreach (var scope in operationScopes)
                {
                    var policy = await effectivePolicies.ResolveAsync(
                        scope.CompanyId, scope.BranchId, scope.RegisteredDeviceId,
                        scope.DeviceId, cancellationToken);
                    if (policy is null || scope.OldestAt > DateTimeOffset.UtcNow.AddDays(-policy.ServerPayloadDays))
                        continue;
                    var remaining = batchSize - redactedOperations;
                    var affected = await db.Database.ExecuteSqlInterpolatedAsync($$"""
                        WITH candidates AS (
                          SELECT o."Id"
                          FROM transport_erp.sync_operations o
                          WHERE o."CompanyId"={{scope.CompanyId}}
                            AND o."BranchId" IS NOT DISTINCT FROM {{scope.BranchId}}
                            AND o."RegisteredDeviceId" IS NOT DISTINCT FROM {{scope.RegisteredDeviceId}}
                            AND o."DeviceId"={{scope.DeviceId}}
                            AND NOT o."LegalHold"
                            AND o."RedactedAt" IS NULL
                            AND o."RetentionDaysApplied" IS NULL
                            AND (o."Status" IN ('SUCCEEDED','REJECTED','RESOLVED') OR
                                 (o."Status"='FAILED' AND o."NextRetryAt" IS NULL))
                            AND o."UpdatedAt"<=clock_timestamp()-make_interval(days => {{policy.ServerPayloadDays}})
                          ORDER BY o."UpdatedAt",o."Id"
                          FOR UPDATE OF o SKIP LOCKED
                          LIMIT {{remaining}}
                        )
                        UPDATE transport_erp.sync_operations o
                        SET "PayloadJson"='{}',
                            "RetentionDaysApplied"={{policy.ServerPayloadDays}},
                            "RedactedAt"=clock_timestamp()
                        FROM candidates c
                        WHERE o."Id"=c."Id" AND NOT o."LegalHold" AND o."RedactedAt" IS NULL
                        """, cancellationToken);
                    redactedOperations += affected;
                    if (affected > 0) appliedPolicies.Add(policy);
                    if (redactedOperations >= batchSize) break;
                }
            }

            var redactedConflicts = 0;
            var conflictCursor = string.Empty;
            while (redactedConflicts < batchSize)
            {
                var conflictScopes = await ConflictScopePageAsync(
                    conflictCursor, batchSize, cancellationToken);
                if (conflictScopes.Count == 0) break;
                conflictCursor = conflictScopes[^1].ScopeKey;
                foreach (var scope in conflictScopes)
                {
                    var policy = await effectivePolicies.ResolveAsync(
                        scope.CompanyId, scope.BranchId, scope.RegisteredDeviceId,
                        scope.DeviceId, cancellationToken);
                    if (policy is null || scope.OldestAt > DateTimeOffset.UtcNow.AddDays(-policy.ServerPayloadDays))
                        continue;
                    var remaining = batchSize - redactedConflicts;
                    var affected = await db.Database.ExecuteSqlInterpolatedAsync($$"""
                        WITH locked_operations AS MATERIALIZED (
                          SELECT o."Id"
                          FROM transport_erp.conflict_cases c
                          JOIN transport_erp.sync_operations o ON o."Id"=c."SyncOperationId"
                          WHERE o."CompanyId"={{scope.CompanyId}}
                            AND o."BranchId" IS NOT DISTINCT FROM {{scope.BranchId}}
                            AND o."RegisteredDeviceId" IS NOT DISTINCT FROM {{scope.RegisteredDeviceId}}
                            AND o."DeviceId"={{scope.DeviceId}}
                            AND NOT c."LegalHold" AND NOT c."ParentLegalHold" AND NOT o."LegalHold"
                            AND c."RedactedAt" IS NULL
                            AND c."RetentionDaysApplied" IS NULL
                            AND c."Status"='RESOLVED' AND c."ResolvedAt" IS NOT NULL
                            AND c."ResolvedAt"<=clock_timestamp()-make_interval(days => {{policy.ServerPayloadDays}})
                            AND (o."Status" IN ('SUCCEEDED','REJECTED','RESOLVED') OR
                                 (o."Status"='FAILED' AND o."NextRetryAt" IS NULL))
                            AND o."UpdatedAt"<=clock_timestamp()-make_interval(days => {{policy.ServerPayloadDays}})
                          ORDER BY c."ResolvedAt",c."Id"
                          FOR UPDATE OF o SKIP LOCKED
                          LIMIT {{remaining}}
                        ),
                        candidates AS MATERIALIZED (
                          SELECT c."Id"
                          FROM transport_erp.conflict_cases c
                          JOIN locked_operations selected_o ON selected_o."Id"=c."SyncOperationId"
                          JOIN transport_erp.sync_operations o ON o."Id"=selected_o."Id"
                          WHERE NOT c."LegalHold" AND NOT c."ParentLegalHold" AND NOT o."LegalHold"
                            AND c."RedactedAt" IS NULL
                            AND c."RetentionDaysApplied" IS NULL
                            AND c."Status"='RESOLVED' AND c."ResolvedAt" IS NOT NULL
                            AND c."ResolvedAt"<=clock_timestamp()-make_interval(days => {{policy.ServerPayloadDays}})
                            AND (o."Status" IN ('SUCCEEDED','REJECTED','RESOLVED') OR
                                 (o."Status"='FAILED' AND o."NextRetryAt" IS NULL))
                            AND o."UpdatedAt"<=clock_timestamp()-make_interval(days => {{policy.ServerPayloadDays}})
                          ORDER BY c."ResolvedAt",c."Id"
                          FOR UPDATE OF c SKIP LOCKED
                          LIMIT {{remaining}}
                        )
                        UPDATE transport_erp.conflict_cases c
                        SET "DeviceSnapshot"='{}',"ServerSnapshot"='{}',
                            "RetentionDaysApplied"={{policy.ServerPayloadDays}},
                            "RedactedAt"=clock_timestamp()
                        FROM candidates selected, transport_erp.sync_operations o
                        WHERE c."Id"=selected."Id" AND o."Id"=c."SyncOperationId"
                          AND NOT c."LegalHold" AND NOT c."ParentLegalHold"
                          AND NOT o."LegalHold" AND c."RedactedAt" IS NULL
                        """, cancellationToken);
                    redactedConflicts += affected;
                    if (affected > 0) appliedPolicies.Add(policy);
                    if (redactedConflicts >= batchSize) break;
                }
            }

            Guid? auditCorrelationId = null;
            if (redactedOperations != 0 || redactedConflicts != 0)
            {
                auditCorrelationId = Guid.NewGuid();
                var policySources = appliedPolicies
                    .Select(policy => (policy.SourceVersion, policy.SourceFingerprint))
                    .Distinct().ToArray();
                var minimumDays = appliedPolicies.Min(policy => policy.ServerPayloadDays);
                var maximumDays = appliedPolicies.Max(policy => policy.ServerPayloadDays);
                var policySourceEvidence = policySources.Length == 1
                    ? $"PolicySourceVersion={policySources[0].SourceVersion};" +
                      $"PolicySourceFingerprint={policySources[0].SourceFingerprint};"
                    : $"PolicySources={policySources.Length};";
                await audit.AppendAuditEventAsync(new AuditEventDraft(
                    "SyncRetentionContentRedacted",
                    "SUCCESS",
                    "SyncRetentionBatch",
                    CorrelationId: auditCorrelationId,
                    Reason: $"RetentionPolicy=EFFECTIVE;{policySourceEvidence}" +
                            $"RetentionDaysRange={minimumDays}-{maximumDays};" +
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

    private Task<List<RetentionScopeCandidate>> OperationScopePageAsync(
        string cursor,
        int pageSize,
        CancellationToken cancellationToken)
        => db.Database.SqlQuery<RetentionScopeCandidate>($$"""
            SELECT o."CompanyId" AS "CompanyId",
                   o."BranchId" AS "BranchId",
                   o."RegisteredDeviceId" AS "RegisteredDeviceId",
                   o."DeviceId" AS "DeviceId",
                   MIN(o."UpdatedAt") AS "OldestAt",
                   o."CompanyId"::text || '|' || COALESCE(o."BranchId"::text,'') || '|' ||
                     COALESCE(o."RegisteredDeviceId"::text,'') || '|' || o."DeviceId" AS "ScopeKey"
            FROM transport_erp.sync_operations o
            WHERE NOT o."LegalHold" AND o."RedactedAt" IS NULL
              AND o."RetentionDaysApplied" IS NULL
              AND (o."Status" IN ('SUCCEEDED','REJECTED','RESOLVED') OR
                   (o."Status"='FAILED' AND o."NextRetryAt" IS NULL))
              AND (o."CompanyId"::text || '|' || COALESCE(o."BranchId"::text,'') || '|' ||
                   COALESCE(o."RegisteredDeviceId"::text,'') || '|' || o."DeviceId") > {{cursor}}
            GROUP BY o."CompanyId",o."BranchId",o."RegisteredDeviceId",o."DeviceId"
            ORDER BY "ScopeKey"
            LIMIT {{pageSize}}
            """).ToListAsync(cancellationToken);

    private Task<List<RetentionScopeCandidate>> ConflictScopePageAsync(
        string cursor,
        int pageSize,
        CancellationToken cancellationToken)
        => db.Database.SqlQuery<RetentionScopeCandidate>($$"""
            SELECT o."CompanyId" AS "CompanyId",
                   o."BranchId" AS "BranchId",
                   o."RegisteredDeviceId" AS "RegisteredDeviceId",
                   o."DeviceId" AS "DeviceId",
                   MIN(GREATEST(c."ResolvedAt",o."UpdatedAt")) AS "OldestAt",
                   o."CompanyId"::text || '|' || COALESCE(o."BranchId"::text,'') || '|' ||
                     COALESCE(o."RegisteredDeviceId"::text,'') || '|' || o."DeviceId" AS "ScopeKey"
            FROM transport_erp.conflict_cases c
            JOIN transport_erp.sync_operations o ON o."Id"=c."SyncOperationId"
            WHERE NOT c."LegalHold" AND NOT c."ParentLegalHold" AND NOT o."LegalHold"
              AND c."RedactedAt" IS NULL AND c."RetentionDaysApplied" IS NULL
              AND c."Status"='RESOLVED' AND c."ResolvedAt" IS NOT NULL
              AND (o."Status" IN ('SUCCEEDED','REJECTED','RESOLVED') OR
                   (o."Status"='FAILED' AND o."NextRetryAt" IS NULL))
              AND (o."CompanyId"::text || '|' || COALESCE(o."BranchId"::text,'') || '|' ||
                   COALESCE(o."RegisteredDeviceId"::text,'') || '|' || o."DeviceId") > {{cursor}}
            GROUP BY o."CompanyId",o."BranchId",o."RegisteredDeviceId",o."DeviceId"
            ORDER BY "ScopeKey"
            LIMIT {{pageSize}}
            """).ToListAsync(cancellationToken);


    private sealed class RetentionScopeCandidate
    {
        public Guid CompanyId { get; init; }
        public Guid? BranchId { get; init; }
        public Guid? RegisteredDeviceId { get; init; }
        public string DeviceId { get; init; } = string.Empty;
        public DateTimeOffset OldestAt { get; init; }
        public string ScopeKey { get; init; } = string.Empty;
    }
}
