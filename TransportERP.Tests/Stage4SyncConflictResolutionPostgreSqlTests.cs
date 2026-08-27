using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using TransportERP.Application.Sync;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

[Collection("PostgreSql")]
public sealed class Stage4SyncConflictResolutionPostgreSqlTests
{
    [Fact]
    public void Sync_operation_service_exposes_no_legacy_conflict_resolution_bypass()
        => Assert.Null(typeof(SyncOperationService).GetMethod("ResolveSyncConflictAsync"));

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Keep_server_atomically_rejects_original_resolves_conflict_and_writes_metadata_only_audit()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        ConflictScope scope;
        await using (var db = PostgreSqlTestEnvironment.CreateDbContext(connection))
        {
            await db.Database.MigrateAsync();
            scope = await SeedConflictAsync(db, "KEEP");
        }

        await using (var db = PostgreSqlTestEnvironment.CreateDbContext(connection))
        {
            var result = await ResolveAsync(Service(db), scope, KeepRequest("keep reviewed server value"));
            Assert.Equal("RESOLVED", result.ConflictStatus);
            Assert.Equal("REJECTED", result.OriginalOperationStatus);
            Assert.Equal("KEEP_SERVER", result.OriginalOperationErrorCode);
        }

        await using var verify = PostgreSqlTestEnvironment.CreateDbContext(connection);
        var conflict = await verify.ConflictCases.AsNoTracking().SingleAsync(x => x.Id == scope.ConflictId);
        var operation = await verify.SyncOperations.AsNoTracking().SingleAsync(x => x.Id == scope.OperationId);
        var audit = await verify.AuditEvents.AsNoTracking().SingleAsync(x =>
            x.Action == "SyncConflictResolved" && x.EntityId == scope.ConflictId);
        Assert.Equal(SyncConflictResolutionDecisions.KeepServerAndRejectLocal, conflict.Resolution);
        Assert.Equal(scope.UserId.ToString(), conflict.ResolvedBy);
        Assert.Equal("REJECTED", operation.Status);
        Assert.Equal("KEEP_SERVER", operation.ErrorCode);
        Assert.Equal("keep reviewed server value", audit.Reason);
        Assert.DoesNotContain("PayloadJson", audit.AfterJson ?? string.Empty, StringComparison.Ordinal);
        Assert.DoesNotContain("device-secret", audit.AfterJson ?? string.Empty, StringComparison.Ordinal);
        Assert.Equal(scope.OperationCorrelationId, audit.OperationCorrelationId);
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Concurrent_resolvers_produce_one_transition_and_one_audit()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        ConflictScope scope;
        ResolverFixture firstResolver;
        ResolverFixture secondResolver;
        await using (var db = PostgreSqlTestEnvironment.CreateDbContext(connection))
        {
            await db.Database.MigrateAsync();
            scope = await SeedConflictAsync(db, "RACE");
            firstResolver = await SeedFreshProofAsync(db, scope);
            secondResolver = await SeedFreshProofAsync(db, scope);
        }

        async Task<string> Resolve(ResolverFixture resolver)
        {
            await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
            try
            {
                _ = await Service(db).ResolveAsync(
                    scope.ConflictId, KeepRequest("race decision"), resolver.Context, resolver.Proof);
                return "SUCCESS";
            }
            catch (SyncRuleException exception)
            {
                return exception.Code;
            }
        }

        var outcomes = await Task.WhenAll(
            Task.Run(() => Resolve(firstResolver)), Task.Run(() => Resolve(secondResolver)));
        Assert.Equal(2, outcomes.Count(x => x == "SUCCESS"));

        await using var verify = PostgreSqlTestEnvironment.CreateDbContext(connection);
        Assert.Equal(1, await verify.AuditEvents.CountAsync(x =>
            x.Action == "SyncConflictResolved" && x.EntityId == scope.ConflictId));
        Assert.Equal(1, await verify.AuditEvents.CountAsync(x =>
            x.Action == "SyncConflictResolutionReplayed" && x.EntityId == scope.ConflictId));
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Resolution_and_parent_legal_hold_follow_operation_then_conflict_without_deadlock()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        ConflictScope holdFirst;
        ConflictScope resolveFirst;
        await using (var seed = PostgreSqlTestEnvironment.CreateDbContext(connection))
        {
            await seed.Database.MigrateAsync();
            holdFirst = await SeedConflictAsync(seed, "HOLD-RESOLVE");
            resolveFirst = await SeedConflictAsync(seed, "RESOLVE-HOLD");
        }

        await using (var holdDb = PostgreSqlTestEnvironment.CreateDbContext(connection))
        await using (var holdTransaction = await holdDb.Database.BeginTransactionAsync())
        {
            await holdDb.Database.ExecuteSqlInterpolatedAsync($$"""
                UPDATE transport_erp.sync_operations SET "LegalHold"=TRUE
                WHERE "Id"={{holdFirst.OperationId}}
                """);
            var resolverApplication = $"conflict-resolver-wait-{Guid.NewGuid():N}";
            await using var resolveDb = PostgreSqlTestEnvironment.CreateDbContext(
                WithApplicationName(connection, resolverApplication));
            var resolveTask = ResolveAsync(
                Service(resolveDb), holdFirst, KeepRequest("hold linearized before resolution"));
            await WaitForLockWaiterAsync(connection, resolverApplication);
            await holdTransaction.CommitAsync();
            var resolved = await resolveTask.WaitAsync(TimeSpan.FromSeconds(10));
            Assert.Equal("RESOLVED", resolved.ConflictStatus);
        }

        await using (var resolveDb = PostgreSqlTestEnvironment.CreateDbContext(connection))
            _ = await ResolveAsync(
                Service(resolveDb), resolveFirst, KeepRequest("resolution linearized before hold"));
        await using (var holdDb = PostgreSqlTestEnvironment.CreateDbContext(connection))
            Assert.Equal(1, await holdDb.Database.ExecuteSqlInterpolatedAsync($$"""
                UPDATE transport_erp.sync_operations SET "LegalHold"=TRUE
                WHERE "Id"={{resolveFirst.OperationId}}
                """));

        await using var verify = PostgreSqlTestEnvironment.CreateDbContext(connection);
        foreach (var scope in new[] { holdFirst, resolveFirst })
        {
            var operation = await verify.SyncOperations.AsNoTracking()
                .SingleAsync(item => item.Id == scope.OperationId);
            var conflict = await verify.ConflictCases.AsNoTracking()
                .SingleAsync(item => item.Id == scope.ConflictId);
            Assert.True(operation.LegalHold);
            Assert.True(conflict.ParentLegalHold);
            Assert.Equal("RESOLVED", conflict.Status);
            Assert.Equal("REJECTED", operation.Status);
        }
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Scope_permission_reason_decision_and_repeat_resolution_fail_closed()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedConflictAsync(db, "DENY");

        var oldContext = Context(scope);
        var oldProofError = await Assert.ThrowsAsync<SyncRuleException>(() => Service(db).ResolveAsync(
            scope.ConflictId, KeepRequest("old request must not resolve"), oldContext,
            Proof(scope, oldContext) with { ReplayId = scope.OriginalReplayId }));
        Assert.Equal("invalid_dpop_proof", oldProofError.Code);

        var forgedProofError = await Assert.ThrowsAsync<SyncRuleException>(() => Service(db).ResolveAsync(
            scope.ConflictId, KeepRequest("unpersisted request must not resolve"), oldContext,
            Proof(scope, oldContext) with { ReplayId = Guid.NewGuid() }));
        Assert.Equal("invalid_dpop_proof", forgedProofError.Code);

        var scopeError = await Assert.ThrowsAsync<SyncRuleException>(() => ResolveAsync(
            Service(db), scope, KeepRequest("reason"), Context(scope) with { BranchId = Guid.NewGuid() }));
        Assert.Equal("SCOPE_DENIED", scopeError.Code);

        db.ChangeTracker.Clear();
        var permissionError = await Assert.ThrowsAsync<SyncRuleException>(() => ResolveAsync(
            Service(db, allowOriginal: false), scope, KeepRequest("reason")));
        Assert.Equal("PERMISSION_DENIED", permissionError.Code);

        var decisionError = await Assert.ThrowsAsync<SyncRuleException>(() => ResolveAsync(
            Service(db), scope, new ResolveSyncConflictRequest("USE_DEVICE_OVERWRITE", "reason")));
        Assert.Equal("RESOLUTION_INVALID", decisionError.Code);
        var reasonError = await Assert.ThrowsAsync<SyncRuleException>(() => ResolveAsync(
            Service(db), scope, KeepRequest(" ")));
        Assert.Equal("REASON_REQUIRED", reasonError.Code);
        foreach (var unsafeReason in new[]
                 {
                     "Bearer eyJhbGciOiJFUzI1NiJ9.payload.signature",
                     "credential=QWxhZGRpbjpvcGVuIHNlc2FtZQ==",
                     "proof\u0001control"
                 })
        {
            var unsafeReasonError = await Assert.ThrowsAsync<SyncRuleException>(() => ResolveAsync(
                Service(db), scope, KeepRequest(unsafeReason)));
            Assert.Equal("REASON_INVALID", unsafeReasonError.Code);
        }

        _ = await ResolveAsync(Service(db), scope, KeepRequest("final reason"));
        db.ChangeTracker.Clear();
        var replayProof = await SeedFreshProofAsync(db, scope);
        db.ChangeTracker.Clear();
        var replay = await Service(db).ResolveAsync(
            scope.ConflictId, KeepRequest("final reason"), replayProof.Context, replayProof.Proof);
        Assert.Equal("RESOLVED", replay.ConflictStatus);
        var changedReasonProof = await SeedFreshProofAsync(db, scope);
        db.ChangeTracker.Clear();
        var changedReason = await Assert.ThrowsAsync<SyncRuleException>(() => Service(db).ResolveAsync(
            scope.ConflictId, KeepRequest("different decision evidence"),
            changedReasonProof.Context, changedReasonProof.Proof));
        Assert.Equal("CONFLICT_ALREADY_RESOLVED", changedReason.Code);
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Conflict_relations_are_tenant_and_branch_safe_under_direct_PostgreSQL_writes()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var companyA = await SeedConflictAsync(db, "TENANT-A");
        var companyB = await SeedConflictAsync(db, "TENANT-B");

        Assert.Equal(1, await db.Database.SqlQuery<int>($"""
            SELECT count(*)::int AS "Value"
            FROM transport_erp.conflict_cases c
            JOIN transport_erp.sync_operations o
              ON o."Id"=c."SyncOperationId" AND o."CompanyId"=c."CompanyId"
             AND o."BranchId" IS NOT DISTINCT FROM c."BranchId"
            WHERE c."Id"={companyA.ConflictId}
            """).SingleAsync());

        await AssertTenantGuardAsync(() => db.Database.ExecuteSqlInterpolatedAsync($"""
            UPDATE transport_erp.conflict_cases
            SET "SyncOperationId"={companyB.OperationId}
            WHERE "Id"={companyA.ConflictId}
            """));
        await AssertTenantGuardAsync(() => db.Database.ExecuteSqlInterpolatedAsync($"""
            UPDATE transport_erp.conflict_cases
            SET "ReplacedByOperationId"={companyB.OperationId}
            WHERE "Id"={companyA.ConflictId}
            """));

        var otherBranch = new Branch
        {
            Id = Guid.NewGuid(), CompanyId = companyA.CompanyId,
            Code = $"B-{Guid.NewGuid():N}"[..12], NameAr = "فرع آخر",
            Timezone = "Asia/Riyadh", Status = "ACTIVE",
            CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        db.Branches.Add(otherBranch);
        await db.SaveChangesAsync();
        await AssertTenantGuardAsync(() => db.Database.ExecuteSqlInterpolatedAsync($"""
            UPDATE transport_erp.conflict_cases
            SET "BranchId"={otherBranch.Id}
            WHERE "Id"={companyA.ConflictId}
            """));
        await AssertTenantGuardAsync(() => db.Database.ExecuteSqlInterpolatedAsync($"""
            UPDATE transport_erp.conflict_cases
            SET "BranchId"=NULL
            WHERE "Id"={companyA.ConflictId}
            """));

        var branchlessOperationId = Guid.NewGuid();
        var historicalPayload = "{\"legacy\":true}";
        var historicalHash = Convert.ToHexString(
            SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(historicalPayload))).ToLowerInvariant();
        await db.Database.ExecuteSqlInterpolatedAsync($"""
            ALTER TABLE transport_erp.sync_operations DISABLE TRIGGER USER;
            INSERT INTO transport_erp.sync_operations
              ("Id","DeviceId","UserId","CompanyId","BranchId","OperationType","EntityType","EntityId",
               "ClientOperationId","PayloadJson","PayloadHash","ClientOccurredAt","ServerReceivedAt","Status",
               "RetryCount","CreatedAt","UpdatedAt","RowVersion")
            VALUES ({branchlessOperationId},{"historical-branchless"},{companyA.UserId},{companyA.CompanyId},NULL,
                    'UPDATE','Historical',NULL,{$"historical-{Guid.NewGuid():N}"},{historicalPayload},
                    {historicalHash},{DateTimeOffset.UtcNow},{DateTimeOffset.UtcNow},'QUEUED',0,
                    {DateTimeOffset.UtcNow},{DateTimeOffset.UtcNow},{RandomNumberGenerator.GetBytes(16)});
            ALTER TABLE transport_erp.sync_operations ENABLE TRIGGER USER
            """);
        const string emptyJson = "{}";
        await AssertTenantGuardAsync(() => db.Database.ExecuteSqlInterpolatedAsync($"""
            INSERT INTO transport_erp.conflict_cases
              ("Id","SyncOperationId","CompanyId","BranchId","BaseVersion","DeviceSnapshot",
               "ServerSnapshot","ConflictReason","Resolution","ResolvedBy","ResolvedAt",
               "ReplacedByOperationId","Status","CreatedAt","UpdatedAt","RowVersion")
            VALUES ({Guid.NewGuid()},{branchlessOperationId},{companyA.CompanyId},{companyA.BranchId},NULL,
                    {emptyJson},{emptyJson},'DIRECT_SCOPE_TEST',NULL,NULL,NULL,NULL,'OPEN',
                    {DateTimeOffset.UtcNow},{DateTimeOffset.UtcNow},{RandomNumberGenerator.GetBytes(16)})
            """));

        await db.Database.ExecuteSqlInterpolatedAsync($"""
            UPDATE transport_erp.conflict_cases
            SET "ReplacedByOperationId"={companyA.OperationId}
            WHERE "Id"={companyA.ConflictId}
            """);
        Assert.Equal(companyA.OperationId, await db.ConflictCases.AsNoTracking()
            .Where(x => x.Id == companyA.ConflictId)
            .Select(x => x.ReplacedByOperationId)
            .SingleAsync());
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Reapply_enqueues_replacement_first_and_atomically_supersedes_original()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedConflictAsync(db, "REAPPLY");
        var reusedIdentity = new ResolveSyncConflictRequest(
            SyncConflictResolutionDecisions.ReapplyAsNew,
            "must use fresh client identity",
            new SyncReapplyAsNewRequest(
                scope.ClientOperationId, scope.OperationCorrelationId, "UpdateWaybillDraft", "UPDATE", "Waybill",
                scope.EntityId, 2, DateTimeOffset.UtcNow, Payload, PayloadHash));
        var identityError = await Assert.ThrowsAsync<SyncRuleException>(() =>
            ResolveAsync(Service(db), scope, reusedIdentity));
        Assert.Equal("REAPPLY_ID_REUSE", identityError.Code);

        var crossEntity = new ResolveSyncConflictRequest(
            SyncConflictResolutionDecisions.ReapplyAsNew,
            "must preserve original entity",
            new SyncReapplyAsNewRequest(
                $"replacement-{Guid.NewGuid():N}", Guid.NewGuid(), "UpdateWaybillDraft", "UPDATE", "Waybill",
                Guid.NewGuid(), 2, DateTimeOffset.UtcNow, Payload, PayloadHash));
        var scopeError = await Assert.ThrowsAsync<SyncRuleException>(() =>
            ResolveAsync(Service(db), scope, crossEntity));
        Assert.Equal("REAPPLY_SCOPE_MISMATCH", scopeError.Code);

        var hashMismatch = new ResolveSyncConflictRequest(
            SyncConflictResolutionDecisions.ReapplyAsNew,
            "reject mismatched payload hash",
            new SyncReapplyAsNewRequest(
                $"replacement-{Guid.NewGuid():N}", Guid.NewGuid(), "UpdateWaybillDraft", "UPDATE", "Waybill",
                scope.EntityId, 2, DateTimeOffset.UtcNow, Payload, new string('0', 64)));
        var hashError = await Assert.ThrowsAsync<SyncRuleException>(() =>
            ResolveAsync(Service(db), scope, hashMismatch));
        Assert.Equal("HASH_MISMATCH", hashError.Code);
        Assert.Equal(1, await db.SyncOperations.CountAsync(x => x.CompanyId == scope.CompanyId));

        var request = new ResolveSyncConflictRequest(
            SyncConflictResolutionDecisions.ReapplyAsNew,
            "reapply reviewed draft",
            new SyncReapplyAsNewRequest(
                $"replacement-{Guid.NewGuid():N}", Guid.NewGuid(), "UpdateWaybillDraft", "UPDATE", "Waybill",
                scope.EntityId, 2, DateTimeOffset.UtcNow, Payload, PayloadHash));

        var result = await ResolveAsync(Service(db), scope, request);
        Assert.Equal("RESOLVED", result.ConflictStatus);
        Assert.Equal("RESOLVED", result.OriginalOperationStatus);
        Assert.Equal("SUPERSEDED", result.OriginalOperationErrorCode);
        Assert.NotNull(result.ReplacedByOperationId);

        db.ChangeTracker.Clear();
        var conflict = await db.ConflictCases.AsNoTracking().SingleAsync(x => x.Id == scope.ConflictId);
        var operation = await db.SyncOperations.AsNoTracking().SingleAsync(x => x.Id == scope.OperationId);
        var replacement = await db.SyncOperations.AsNoTracking()
            .SingleAsync(x => x.Id == conflict.ReplacedByOperationId);
        Assert.Equal("RESOLVED", conflict.Status);
        Assert.Equal("RESOLVED", operation.Status);
        Assert.Equal("SUPERSEDED", operation.ErrorCode);
        Assert.Equal("QUEUED", replacement.Status);
        Assert.Equal(scope.CompanyId, replacement.CompanyId);
        Assert.Equal(scope.BranchId, replacement.BranchId);
        Assert.Equal(scope.UserId, replacement.UserId);
        Assert.Equal(scope.RegisteredDeviceId, replacement.RegisteredDeviceId);
        Assert.Equal(scope.ReplayId, replacement.AcceptedProofReplayId);
        Assert.Equal(2, await db.SyncOperations.CountAsync(x => x.CompanyId == scope.CompanyId));
        Assert.True(await db.AuditEvents.AnyAsync(x =>
            x.Action == "SyncOperationQueued" && x.EntityId == replacement.Id));
        var resolutionAudit = await db.AuditEvents.AsNoTracking().SingleAsync(x =>
            x.Action == "SyncConflictResolved" && x.EntityId == scope.ConflictId);
        Assert.DoesNotContain(Payload, resolutionAudit.AfterJson ?? string.Empty, StringComparison.Ordinal);
        Assert.DoesNotContain(PayloadHash, resolutionAudit.AfterJson ?? string.Empty, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Proof", resolutionAudit.AfterJson ?? string.Empty, StringComparison.OrdinalIgnoreCase);
        Assert.Equal(scope.OperationCorrelationId, resolutionAudit.OperationCorrelationId);

        var replayProof = await SeedFreshProofAsync(db, scope);
        db.ChangeTracker.Clear();
        var replay = await Service(db).ResolveAsync(
            scope.ConflictId, request, replayProof.Context, replayProof.Proof);
        Assert.Equal(result.ReplacedByOperationId, replay.ReplacedByOperationId);
        Assert.Equal(2, await db.SyncOperations.CountAsync(x => x.CompanyId == scope.CompanyId));
        Assert.Single(await db.AuditEvents.Where(x =>
            x.Action == "SyncConflictResolutionReplayed" && x.EntityId == scope.ConflictId).ToListAsync());
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Exact_reapply_replay_remains_idempotent_after_retention_redacts_raw_payload()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedConflictAsync(db, "REAPPLY-REDACTED-REPLAY");
        var request = new ResolveSyncConflictRequest(
            SyncConflictResolutionDecisions.ReapplyAsNew,
            "reviewed reapply retained by fingerprint",
            new SyncReapplyAsNewRequest(
                $"replacement-{Guid.NewGuid():N}", Guid.NewGuid(), "UpdateWaybillDraft", "UPDATE", "Waybill",
                scope.EntityId, 2, DateTimeOffset.UtcNow, Payload, PayloadHash));

        var cutoff = DateTimeOffset.UtcNow.AddDays(-91);
        var resolved = await ResolveAsync(Service(db, timeProvider: new FixedTimeProvider(cutoff)), scope, request);
        var replacement = await db.SyncOperations.SingleAsync(x => x.Id == resolved.ReplacedByOperationId);
        replacement.Status = "SUCCEEDED";
        replacement.ResultEntityId = scope.EntityId;
        replacement.ResultVersion = 3;
        replacement.UpdatedAt = cutoff;
        await db.SaveChangesAsync();

        var cleanup = await new SyncRetentionCleanupService(
                db, new AuditEventService(db), FixedRetentionPolicyProvider.Instance)
            .CleanupBatchAsync();
        Assert.True(cleanup.RedactedOperations >= 2);
        db.ChangeTracker.Clear();
        var redactedReplacement = await db.SyncOperations.AsNoTracking()
            .SingleAsync(x => x.Id == resolved.ReplacedByOperationId);
        Assert.Equal("{}", redactedReplacement.PayloadJson);
        Assert.NotNull(redactedReplacement.RedactedAt);

        var replayProof = await SeedFreshProofAsync(db, scope);
        db.ChangeTracker.Clear();
        var replay = await Service(db).ResolveAsync(
            scope.ConflictId, request, replayProof.Context, replayProof.Proof);

        Assert.Equal(resolved.ReplacedByOperationId, replay.ReplacedByOperationId);
        Assert.Equal(2, await db.SyncOperations.CountAsync(x => x.CompanyId == scope.CompanyId));
        Assert.Single(await db.AuditEvents.Where(x =>
            x.Action == "SyncConflictResolutionReplayed" && x.EntityId == scope.ConflictId).ToListAsync());
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Accepted_enqueue_inside_caller_transaction_neither_commits_nor_clears_caller_state()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedConflictAsync(db, "CALLER-TX");
        var context = Context(scope);
        var clientOperationId = $"caller-owned-{Guid.NewGuid():N}";
        await using var transaction = await db.Database.BeginTransactionAsync();

        var replacement = await OperationService(db).EnqueueAcceptedSyncOperationAsync(
            new EnqueueAcceptedSyncOperationCommand(
                "sync-v1", "UpdateWaybillDraft", "UPDATE", "Waybill", scope.EntityId,
                clientOperationId, Payload, PayloadHash, DateTimeOffset.UtcNow, Guid.NewGuid(), 2),
            Proof(scope, context));

        Assert.NotNull(db.Database.CurrentTransaction);
        Assert.Equal("QUEUED", replacement.Status);
        Assert.True(db.ChangeTracker.Entries<SyncOperation>().Any(x => x.Entity.Id == replacement.Id));
        await transaction.RollbackAsync();
        db.ChangeTracker.Clear();
        Assert.False(await db.SyncOperations.AsNoTracking().AnyAsync(x => x.Id == replacement.Id));
        Assert.Equal(1, await db.SyncOperations.CountAsync(x => x.CompanyId == scope.CompanyId));
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Concurrent_reapply_resolvers_create_exactly_one_replacement_and_one_resolution_audit()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        ConflictScope scope;
        ResolverFixture firstResolver;
        ResolverFixture secondResolver;
        await using (var seed = PostgreSqlTestEnvironment.CreateDbContext(connection))
        {
            await seed.Database.MigrateAsync();
            scope = await SeedConflictAsync(seed, "REAPPLY-RACE");
            firstResolver = await SeedFreshProofAsync(seed, scope);
            secondResolver = await SeedFreshProofAsync(seed, scope);
        }

        async Task<string> Resolve(string suffix, ResolverFixture resolver)
        {
            await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
            var request = new ResolveSyncConflictRequest(
                SyncConflictResolutionDecisions.ReapplyAsNew, "concurrent reviewed reapply",
                new SyncReapplyAsNewRequest(
                    $"replacement-{suffix}-{Guid.NewGuid():N}", Guid.NewGuid(), "UpdateWaybillDraft",
                    "UPDATE", "Waybill", scope.EntityId, 2, DateTimeOffset.UtcNow, Payload, PayloadHash));
            try
            {
                _ = await Service(db).ResolveAsync(
                    scope.ConflictId, request, resolver.Context, resolver.Proof);
                return "SUCCESS";
            }
            catch (SyncRuleException exception)
            {
                return exception.Code;
            }
        }

        var outcomes = await Task.WhenAll(
            Task.Run(() => Resolve("a", firstResolver)),
            Task.Run(() => Resolve("b", secondResolver)));
        Assert.Single(outcomes, x => x == "SUCCESS");
        Assert.Single(outcomes, x => x == "CONFLICT_ALREADY_RESOLVED");

        await using var verify = PostgreSqlTestEnvironment.CreateDbContext(connection);
        var conflict = await verify.ConflictCases.AsNoTracking().SingleAsync(x => x.Id == scope.ConflictId);
        Assert.NotNull(conflict.ReplacedByOperationId);
        Assert.Equal(2, await verify.SyncOperations.CountAsync(x => x.CompanyId == scope.CompanyId));
        Assert.Equal(1, await verify.AuditEvents.CountAsync(x =>
            x.Action == "SyncConflictResolved" && x.EntityId == scope.ConflictId));
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Authorized_supervisor_on_live_second_device_can_resolve_conflict_from_revoked_origin_device()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedConflictAsync(db, "ORPHAN-SUPERVISOR");
        var originDevice = await db.RegisteredDevices.SingleAsync(x => x.Id == scope.RegisteredDeviceId);
        originDevice.Status = "REVOKED";
        originDevice.RevokedAt = DateTimeOffset.UtcNow;
        var originAssignment = await db.RegisteredDeviceAssignments.SingleAsync(x => x.Id == scope.AssignmentId);
        originAssignment.Status = "REVOKED";
        originAssignment.RemovedAt = DateTimeOffset.UtcNow;
        originAssignment.RemovedByUserId = scope.UserId;
        await db.SaveChangesAsync();
        var resolver = await SeedResolverAsync(db, scope.CompanyId, scope.BranchId);
        db.ChangeTracker.Clear();

        var result = await Service(db).ResolveAsync(
            scope.ConflictId, KeepRequest("supervisor kept current server state"),
            resolver.Context, resolver.Proof);

        Assert.Equal("RESOLVED", result.ConflictStatus);
        Assert.Equal("REJECTED", result.OriginalOperationStatus);
        db.ChangeTracker.Clear();
        var persisted = await db.ConflictCases.AsNoTracking().SingleAsync(x => x.Id == scope.ConflictId);
        Assert.Equal(resolver.Context.UserId.ToString(), persisted.ResolvedBy);
    }

    [Theory]
    [InlineData("PENDING")]
    [InlineData("SUSPENDED")]
    [InlineData("REVOKED")]
    [InlineData("STALE")]
    [InlineData("ASSIGNMENT_REMOVED")]
    [Trait("Category", "PostgreSQL")]
    public async Task Inactive_or_stale_device_binding_fails_closed_before_resolution(string state)
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedConflictAsync(db, $"BIND-{state}");
        var now = DateTimeOffset.UtcNow;

        if (state == "ASSIGNMENT_REMOVED")
        {
            var assignment = await db.RegisteredDeviceAssignments.SingleAsync(x => x.Id == scope.AssignmentId);
            assignment.Status = "REVOKED";
            assignment.RemovedAt = now;
            assignment.RemovedByUserId = scope.UserId;
        }
        else
        {
            var device = await db.RegisteredDevices.SingleAsync(x => x.Id == scope.RegisteredDeviceId);
            switch (state)
            {
                case "PENDING":
                    device.Status = "PENDING";
                    device.ApprovedAt = null;
                    device.ApprovedByUserId = null;
                    break;
                case "SUSPENDED":
                    device.Status = "SUSPENDED";
                    device.SuspendedAt = now;
                    break;
                case "REVOKED":
                    device.Status = "REVOKED";
                    device.RevokedAt = now;
                    break;
                case "STALE":
                    device.ApprovedAt = now.AddDays(-100);
                    device.LastSeenAt = now.AddDays(-91);
                    break;
            }
        }
        await db.SaveChangesAsync();
        db.ChangeTracker.Clear();

        var error = await Assert.ThrowsAsync<SyncRuleException>(() => ResolveAsync(
            Service(db), scope, KeepRequest("must fail before mutation")));
        Assert.Equal("DEVICE_NOT_REGISTERED", error.Code);

        db.ChangeTracker.Clear();
        Assert.Equal("OPEN", (await db.ConflictCases.AsNoTracking()
            .SingleAsync(x => x.Id == scope.ConflictId)).Status);
        Assert.Equal("CONFLICT", (await db.SyncOperations.AsNoTracking()
            .SingleAsync(x => x.Id == scope.OperationId)).Status);
        Assert.False(await db.AuditEvents.AnyAsync(x =>
            x.Action == "SyncConflictResolved" && x.EntityId == scope.ConflictId));
    }

    private static ResolveSyncConflictRequest KeepRequest(string reason)
        => new(SyncConflictResolutionDecisions.KeepServerAndRejectLocal, reason);

    private static SyncConflictResolutionContext Context(ConflictScope scope)
        => new(scope.UserId, scope.CompanyId, scope.BranchId, scope.RegisteredDeviceId,
            1, scope.DeviceId, scope.FreshAttemptCorrelationId);

    private static AcceptedSyncProofContext Proof(ConflictScope scope, SyncConflictResolutionContext context)
        => new(scope.ReplayId, context.UserId, context.CompanyId, context.BranchId,
            context.RegisteredDeviceId, context.DeviceId, context.RegisteredDeviceCredentialVersion,
            1, new string('t', 43), context.CorrelationId);

    private static Task<SyncConflictResolutionResult> ResolveAsync(
        SyncConflictResolutionService service,
        ConflictScope scope,
        ResolveSyncConflictRequest request,
        SyncConflictResolutionContext? context = null)
    {
        var actualContext = context ?? Context(scope);
        return service.ResolveAsync(scope.ConflictId, request, actualContext, Proof(scope, actualContext));
    }

    private static SyncConflictResolutionService Service(
        TransportErpDbContext db,
        bool allowOriginal = true,
        TimeProvider? timeProvider = null)
        => new(db, new AuditEventService(db), new TestPermissionResolver(allowOriginal),
            OperationService(db), timeProvider);

    private static SyncOperationService OperationService(TransportErpDbContext db)
        => new(db, new AuditEventService(db), new SyncRetryPolicy(5,
            TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(5)));

    private static string WithApplicationName(string connection, string applicationName)
    {
        var builder = new Npgsql.NpgsqlConnectionStringBuilder(connection)
        {
            ApplicationName = applicationName
        };
        return builder.ConnectionString;
    }

    private static async Task WaitForLockWaiterAsync(string connection, string applicationName)
    {
        await using var observer = new Npgsql.NpgsqlConnection(connection);
        await observer.OpenAsync();
        for (var attempt = 0; attempt < 200; attempt++)
        {
            await using var command = new Npgsql.NpgsqlCommand("""
                SELECT EXISTS (
                  SELECT 1 FROM pg_stat_activity
                  WHERE application_name=@applicationName AND wait_event_type='Lock')
                """, observer);
            command.Parameters.AddWithValue("applicationName", applicationName);
            if (await command.ExecuteScalarAsync() is true) return;
            await Task.Delay(25);
        }
        throw new Xunit.Sdk.XunitException(
            "Conflict resolution did not reach the expected operation lock barrier.");
    }

    private const string Payload = "{\"FreightTotal\":100}";
    private static readonly string PayloadHash = Convert.ToHexString(
        SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(Payload))).ToLowerInvariant();

    private static async Task<ConflictScope> SeedConflictAsync(TransportErpDbContext db, string suffix)
    {
        var now = DateTimeOffset.UtcNow;
        var currency = new Currency
        {
            Id = Guid.NewGuid(), Code = await PostgreSqlTestCurrencyCodeAllocator.NextAsync(db),
            NameAr = "عملة تعارض", MinorUnit = 2, IsBase = true, Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var company = new Company
        {
            Id = Guid.NewGuid(), Code = $"CF-{suffix}-{Guid.NewGuid():N}"[..20], LegalNameAr = "شركة تعارض",
            BaseCurrencyId = currency.Id, DefaultCalendarId = Guid.NewGuid(), Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var branch = new Branch
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, Code = $"B-{Guid.NewGuid():N}"[..12], NameAr = "فرع تعارض",
            Timezone = "Asia/Riyadh", Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var userName = $"conflict-{Guid.NewGuid():N}";
        var user = new User
        {
            Id = Guid.NewGuid(), UserName = userName, NormalizedUserName = userName.ToUpperInvariant(),
            DisplayName = "Conflict resolver", PasswordHash = "test", SecurityStamp = Guid.NewGuid().ToString("N"),
            AuthVersion = 1, Status = "ACTIVE", CompanyId = company.Id, BranchId = branch.Id,
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        db.AddRange(currency, company, branch, user);
        await db.SaveChangesAsync();

        var deviceId = $"conflict-device-{Guid.NewGuid():N}";
        var device = new RegisteredDevice
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, DeviceId = deviceId, DisplayName = "Conflict device",
            Platform = "TEST", AppVersion = "1", RegistrationRequestId = $"req-{Guid.NewGuid():N}",
            CredentialHash = new string('d', 64), CredentialVersion = 1, Status = "ACTIVE",
            RegisteredByUserId = user.Id, ApprovedByUserId = user.Id, ApprovedAt = now, LastSeenAt = now,
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var assignment = new RegisteredDeviceAssignment
        {
            Id = Guid.NewGuid(), RegisteredDeviceId = device.Id, UserId = user.Id, CompanyId = company.Id,
            BranchId = branch.Id, Status = "ACTIVE", AssignedByUserId = user.Id, AssignedAt = now,
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        db.AddRange(device, assignment);
        await db.SaveChangesAsync();

        var nonce = new SyncProofNonce
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, RegisteredDeviceId = device.Id, DeviceId = deviceId,
            ProofKeyVersion = 1, NonceHash = RandomNumberGenerator.GetBytes(32), IssuedAt = now,
            ExpiresAt = now.AddMinutes(5)
        };
        db.SyncProofNonces.Add(nonce);
        await db.SaveChangesAsync();
        var proof = new SyncProofReplay
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, RegisteredDeviceId = device.Id, DeviceId = deviceId,
            DeviceAssignmentId = assignment.Id, UserId = user.Id, BranchId = branch.Id, ProofKeyVersion = 1,
            ProofKeyThumbprint = new string('t', 43), JtiHash = RandomNumberGenerator.GetBytes(32),
            HtuHash = RandomNumberGenerator.GetBytes(32), HttpMethod = "POST", NonceRecordId = nonce.Id,
            IssuedAt = now, FirstSeenAt = now, ExpiresAt = now.AddMinutes(4), AttemptCorrelationId = Guid.NewGuid()
        };
        db.SyncProofReplays.Add(proof);
        await db.SaveChangesAsync();
        var freshProof = new SyncProofReplay
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, RegisteredDeviceId = device.Id, DeviceId = deviceId,
            DeviceAssignmentId = assignment.Id, UserId = user.Id, BranchId = branch.Id, ProofKeyVersion = 1,
            ProofKeyThumbprint = new string('t', 43), JtiHash = RandomNumberGenerator.GetBytes(32),
            HtuHash = RandomNumberGenerator.GetBytes(32), HttpMethod = "POST", NonceRecordId = nonce.Id,
            IssuedAt = now, FirstSeenAt = now, ExpiresAt = now.AddMinutes(4), AttemptCorrelationId = Guid.NewGuid()
        };
        db.SyncProofReplays.Add(freshProof);
        await db.SaveChangesAsync();

        var entityId = Guid.NewGuid();
        var operationCorrelationId = Guid.NewGuid();
        var operation = new SyncOperation
        {
            Id = Guid.NewGuid(), DeviceId = deviceId, UserId = user.Id, CompanyId = company.Id, BranchId = branch.Id,
            OperationType = "UPDATE", EntityType = "Waybill", EntityId = entityId,
            ClientOperationId = $"conflict-{Guid.NewGuid():N}", PayloadJson = "{\"device-secret\":true}",
            PayloadHash = new string('a', 64), ClientOccurredAt = now, ServerReceivedAt = now, BaseVersion = 1,
            Status = "CONFLICT", RetryCount = 0, RegisteredDeviceId = device.Id,
            RegisteredDeviceCredentialVersion = 1, ActionCode = "UpdateWaybillDraft", ProtocolVersion = "sync-v1",
            OperationCorrelationId = operationCorrelationId, RequestFingerprintVersion = "fp-v1",
            RequestFingerprintHash = RandomNumberGenerator.GetBytes(32), ProofKeyVersion = 1,
            ProofKeyThumbprint = new string('t', 43), AcceptedProofReplayId = proof.Id,
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        db.SyncOperations.Add(operation);
        await db.SaveChangesAsync();
        var conflict = new ConflictCase
        {
            Id = Guid.NewGuid(), SyncOperationId = operation.Id, CompanyId = company.Id, BranchId = branch.Id,
            BaseVersion = 1, DeviceSnapshot = "{\"redacted\":true}", ServerSnapshot = "{\"version\":2}",
            ConflictReason = "BASE_VERSION_CONFLICT", Status = "OPEN", CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        db.ConflictCases.Add(conflict);
        await db.SaveChangesAsync();
        return new(company.Id, branch.Id, user.Id, device.Id, assignment.Id, proof.Id, freshProof.Id,
            freshProof.AttemptCorrelationId, deviceId, operation.Id, conflict.Id,
            entityId, operation.ClientOperationId, operationCorrelationId);
    }

    private static async Task<ResolverFixture> SeedResolverAsync(
        TransportErpDbContext db,
        Guid companyId,
        Guid branchId)
    {
        var now = DateTimeOffset.UtcNow;
        var userName = $"supervisor-{Guid.NewGuid():N}";
        var user = new User
        {
            Id = Guid.NewGuid(), UserName = userName, NormalizedUserName = userName.ToUpperInvariant(),
            DisplayName = "Conflict supervisor", PasswordHash = "test", SecurityStamp = Guid.NewGuid().ToString("N"),
            AuthVersion = 1, Status = "ACTIVE", CompanyId = companyId, BranchId = branchId,
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var deviceId = $"supervisor-device-{Guid.NewGuid():N}";
        var device = new RegisteredDevice
        {
            Id = Guid.NewGuid(), CompanyId = companyId, DeviceId = deviceId, DisplayName = "Supervisor device",
            Platform = "TEST", AppVersion = "1", RegistrationRequestId = $"req-{Guid.NewGuid():N}",
            CredentialHash = new string('s', 64), CredentialVersion = 1, Status = "ACTIVE",
            RegisteredByUserId = user.Id, ApprovedByUserId = user.Id, ApprovedAt = now, LastSeenAt = now,
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var assignment = new RegisteredDeviceAssignment
        {
            Id = Guid.NewGuid(), RegisteredDeviceId = device.Id, UserId = user.Id, CompanyId = companyId,
            BranchId = branchId, Status = "ACTIVE", AssignedByUserId = user.Id, AssignedAt = now,
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        db.AddRange(user, device, assignment);
        await db.SaveChangesAsync();
        var nonce = new SyncProofNonce
        {
            Id = Guid.NewGuid(), CompanyId = companyId, RegisteredDeviceId = device.Id, DeviceId = deviceId,
            ProofKeyVersion = 1, NonceHash = RandomNumberGenerator.GetBytes(32), IssuedAt = now,
            ExpiresAt = now.AddMinutes(5)
        };
        db.SyncProofNonces.Add(nonce);
        await db.SaveChangesAsync();
        var correlationId = Guid.NewGuid();
        var replay = new SyncProofReplay
        {
            Id = Guid.NewGuid(), CompanyId = companyId, RegisteredDeviceId = device.Id, DeviceId = deviceId,
            DeviceAssignmentId = assignment.Id, UserId = user.Id, BranchId = branchId, ProofKeyVersion = 1,
            ProofKeyThumbprint = new string('t', 43), JtiHash = RandomNumberGenerator.GetBytes(32),
            HtuHash = RandomNumberGenerator.GetBytes(32), HttpMethod = "POST", NonceRecordId = nonce.Id,
            IssuedAt = now, FirstSeenAt = now, ExpiresAt = now.AddMinutes(4), AttemptCorrelationId = correlationId
        };
        db.SyncProofReplays.Add(replay);
        await db.SaveChangesAsync();
        var context = new SyncConflictResolutionContext(
            user.Id, companyId, branchId, device.Id, 1, deviceId, correlationId);
        var proof = new AcceptedSyncProofContext(
            replay.Id, user.Id, companyId, branchId, device.Id, deviceId, 1, 1,
            new string('t', 43), correlationId);
        return new ResolverFixture(context, proof);
    }

    private static async Task<ResolverFixture> SeedFreshProofAsync(
        TransportErpDbContext db,
        ConflictScope scope)
    {
        var now = DateTimeOffset.UtcNow;
        var nonce = new SyncProofNonce
        {
            Id = Guid.NewGuid(), CompanyId = scope.CompanyId,
            RegisteredDeviceId = scope.RegisteredDeviceId, DeviceId = scope.DeviceId,
            ProofKeyVersion = 1, NonceHash = RandomNumberGenerator.GetBytes(32),
            IssuedAt = now, ExpiresAt = now.AddMinutes(5)
        };
        db.SyncProofNonces.Add(nonce);
        await db.SaveChangesAsync();
        var correlationId = Guid.NewGuid();
        var replay = new SyncProofReplay
        {
            Id = Guid.NewGuid(), CompanyId = scope.CompanyId,
            RegisteredDeviceId = scope.RegisteredDeviceId, DeviceId = scope.DeviceId,
            DeviceAssignmentId = scope.AssignmentId, UserId = scope.UserId, BranchId = scope.BranchId,
            ProofKeyVersion = 1, ProofKeyThumbprint = new string('t', 43),
            JtiHash = RandomNumberGenerator.GetBytes(32), HtuHash = RandomNumberGenerator.GetBytes(32),
            HttpMethod = "POST", NonceRecordId = nonce.Id, IssuedAt = now,
            FirstSeenAt = now, ExpiresAt = now.AddMinutes(4), AttemptCorrelationId = correlationId
        };
        db.SyncProofReplays.Add(replay);
        await db.SaveChangesAsync();
        var context = new SyncConflictResolutionContext(
            scope.UserId, scope.CompanyId, scope.BranchId, scope.RegisteredDeviceId,
            1, scope.DeviceId, correlationId);
        var proof = new AcceptedSyncProofContext(
            replay.Id, scope.UserId, scope.CompanyId, scope.BranchId,
            scope.RegisteredDeviceId, scope.DeviceId, 1, 1,
            new string('t', 43), correlationId);
        return new ResolverFixture(context, proof);
    }

    private static async Task AssertTenantGuardAsync(Func<Task> action)
    {
        var failure = await Assert.ThrowsAsync<Npgsql.PostgresException>(action);
        Assert.Equal("P0001", failure.SqlState);
        Assert.Contains("tenant scope mismatch", failure.MessageText, StringComparison.Ordinal);
    }

    private sealed class TestPermissionResolver(bool allowOriginal) : IEffectivePermissionResolver
    {
        public Task<bool> HasPermissionAsync(Guid userId, Guid companyId, Guid? branchId, string permissionCode,
            CancellationToken cancellationToken = default)
            => Task.FromResult(permissionCode == SyncConflictPermissionCodes.Resolve ||
                (allowOriginal && permissionCode == "waybill.edit"));
    }

    private sealed record ConflictScope(
        Guid CompanyId,
        Guid BranchId,
        Guid UserId,
        Guid RegisteredDeviceId,
        Guid AssignmentId,
        Guid OriginalReplayId,
        Guid ReplayId,
        Guid FreshAttemptCorrelationId,
        string DeviceId,
        Guid OperationId,
        Guid ConflictId,
        Guid EntityId,
        string ClientOperationId,
        Guid OperationCorrelationId);

    private sealed record ResolverFixture(
        SyncConflictResolutionContext Context,
        AcceptedSyncProofContext Proof);

    private sealed class FixedTimeProvider(DateTimeOffset value) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => value;
    }

    private sealed class FixedRetentionPolicyProvider : IEffectiveSyncRetentionPolicyProvider
    {
        public static FixedRetentionPolicyProvider Instance { get; } = new();

        public ValueTask<EffectiveSyncRetentionPolicy?> ResolveAsync(
            Guid companyId,
            Guid? branchId,
            Guid? registeredDeviceId,
            string? deviceId,
            CancellationToken cancellationToken = default)
            => ValueTask.FromResult<EffectiveSyncRetentionPolicy?>(
                new(90, "retention-test-policy-v1", new string('a', 64)));
    }
}
