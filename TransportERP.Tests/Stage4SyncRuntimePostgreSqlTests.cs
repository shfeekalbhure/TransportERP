using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using TransportERP.Api.Sync;
using TransportERP.Application.Sync;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

[Collection("PostgreSql")]
public sealed class Stage4SyncRuntimePostgreSqlTests
{
    private const string Htu = "https://sync.example.test/api/v1/sync/operations:batch";

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Nonce_claim_and_business_replay_are_atomic_and_idempotent()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "FLOW");
        var proofRuntime = new SyncProofRuntimeService(db, new AuditEventService(db));

        var firstNonce = await proofRuntime.IssueNonceAsync(scope.Security);
        var secondNonce = await proofRuntime.IssueNonceAsync(scope.Security);
        var jti = Guid.NewGuid().ToString("D");
        // Issuing a newer nonce does not invalidate the still-live previous nonce.
        var firstClaim = await proofRuntime.ClaimAsync(scope.Security,
            Proof(firstNonce.Value, jti, scope.Thumbprint));
        var operationService = CreateOperationService(db);
        var command = Command("client-" + Guid.NewGuid().ToString("N"), "{\"amount\":10}");
        var first = await operationService.EnqueueAcceptedSyncOperationAsync(command, firstClaim);

        var secondClaim = await proofRuntime.ClaimAsync(scope.Security,
            Proof(secondNonce.Value, Guid.NewGuid().ToString("D"), scope.Thumbprint));
        var replay = await operationService.EnqueueAcceptedSyncOperationAsync(command, secondClaim);

        Assert.Equal(first.Id, replay.Id);
        Assert.Equal(first.OperationCorrelationId, replay.OperationCorrelationId);
        Assert.Single(await db.SyncOperations.Where(x => x.ClientOperationId == command.ClientOperationId).ToListAsync());
        Assert.Single(await db.AuditEvents.Where(x => x.Action == "SyncOperationQueued" &&
            x.OperationCorrelationId == command.OperationCorrelationId).ToListAsync());
        Assert.Equal(2, await db.SyncProofReplays.CountAsync(x => x.RegisteredDeviceId == scope.DeviceId));
        var nonceBytes = DecodeBase64Url(firstNonce.Value);
        var persistedHashes = await db.SyncProofNonces.Where(x => x.RegisteredDeviceId == scope.DeviceId)
            .Select(x => x.NonceHash).ToListAsync();
        var expectedHash = SHA256.HashData(nonceBytes);
        Assert.Contains(persistedHashes, value => value.SequenceEqual(expectedHash));

        var changed = command with
        {
            PayloadJson = "{\"amount\":11}",
            PayloadHash = Hash("{\"amount\":11}")
        };
        var mismatch = await Assert.ThrowsAsync<SyncRuleException>(() =>
            operationService.EnqueueAcceptedSyncOperationAsync(changed, secondClaim));
        Assert.Equal("IDEMPOTENCY_MISMATCH", mismatch.Code);

        var duplicateProof = await Assert.ThrowsAsync<SyncProofRuntimeException>(() =>
            proofRuntime.ClaimAsync(scope.Security, Proof(firstNonce.Value, jti, scope.Thumbprint)));
        Assert.Equal("invalid_dpop_proof", duplicateProof.Code);
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Concurrent_claim_across_contexts_accepts_exactly_one_jti()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using (var setup = PostgreSqlTestEnvironment.CreateDbContext(connection))
        {
            await setup.Database.MigrateAsync();
            var seeded = await SeedAsync(setup, "RACE");
            var issuer = new SyncProofRuntimeService(setup, new AuditEventService(setup));
            var nonce = await issuer.IssueNonceAsync(seeded.Security);
            var proof = Proof(nonce.Value, Guid.NewGuid().ToString("D"), seeded.Thumbprint);

            await using var firstDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
            await using var secondDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
            var firstRuntime = new SyncProofRuntimeService(firstDb, new AuditEventService(firstDb));
            var secondRuntime = new SyncProofRuntimeService(secondDb, new AuditEventService(secondDb));
            var first = Record.ExceptionAsync(() => firstRuntime.ClaimAsync(seeded.Security, proof));
            var second = Record.ExceptionAsync(() => secondRuntime.ClaimAsync(seeded.Security, proof));
            var outcomes = await Task.WhenAll(first, second);

            Assert.Single(outcomes, x => x is null);
            var rejected = Assert.Single(outcomes, x => x is not null);
            Assert.Equal("invalid_dpop_proof", Assert.IsType<SyncProofRuntimeException>(rejected).Code);
        }
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Concurrent_business_replay_converges_and_same_client_key_is_isolated_by_tenant()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        TestScope firstScope;
        AcceptedSyncProofContext firstProof;
        AcceptedSyncProofContext secondProof;
        await using (var setup = PostgreSqlTestEnvironment.CreateDbContext(connection))
        {
            await setup.Database.MigrateAsync();
            firstScope = await SeedAsync(setup, "BUSINESS-RACE");
            var runtime = new SyncProofRuntimeService(setup, new AuditEventService(setup));
            var firstNonce = await runtime.IssueNonceAsync(firstScope.Security);
            firstProof = await runtime.ClaimAsync(firstScope.Security,
                Proof(firstNonce.Value, Guid.NewGuid().ToString("D"), firstScope.Thumbprint));
            var secondNonce = await runtime.IssueNonceAsync(firstScope.Security);
            secondProof = await runtime.ClaimAsync(firstScope.Security,
                Proof(secondNonce.Value, Guid.NewGuid().ToString("D"), firstScope.Thumbprint));
        }

        var clientOperationId = "shared-client-" + Guid.NewGuid().ToString("N");
        var command = Command(clientOperationId, "{\"shared\":true}");
        await using (var firstDb = PostgreSqlTestEnvironment.CreateDbContext(connection))
        await using (var secondDb = PostgreSqlTestEnvironment.CreateDbContext(connection))
        {
            var firstTask = CreateOperationService(firstDb).EnqueueAcceptedSyncOperationAsync(command, firstProof);
            var secondTask = CreateOperationService(secondDb).EnqueueAcceptedSyncOperationAsync(command, secondProof);
            var results = await Task.WhenAll(firstTask, secondTask);
            Assert.Equal(results[0].Id, results[1].Id);
        }

        await using (var verify = PostgreSqlTestEnvironment.CreateDbContext(connection))
        {
            Assert.Single(await verify.SyncOperations.Where(x => x.CompanyId == firstScope.Security.CompanyId &&
                x.ClientOperationId == clientOperationId).ToListAsync());
            Assert.Single(await verify.AuditEvents.Where(x => x.CompanyId == firstScope.Security.CompanyId &&
                x.Action == "SyncOperationQueued" && x.OperationCorrelationId == command.OperationCorrelationId)
                .ToListAsync());
        }

        await using (var otherTenantDb = PostgreSqlTestEnvironment.CreateDbContext(connection))
        {
            var otherScope = await SeedAsync(otherTenantDb, "BUSINESS-OTHER-TENANT");
            var runtime = new SyncProofRuntimeService(otherTenantDb, new AuditEventService(otherTenantDb));
            var nonce = await runtime.IssueNonceAsync(otherScope.Security);
            var proof = await runtime.ClaimAsync(otherScope.Security,
                Proof(nonce.Value, Guid.NewGuid().ToString("D"), otherScope.Thumbprint));
            var other = await CreateOperationService(otherTenantDb)
                .EnqueueAcceptedSyncOperationAsync(command, proof);
            Assert.Equal(otherScope.Security.CompanyId, other.CompanyId);
            Assert.NotEqual(firstScope.Security.CompanyId, other.CompanyId);
        }
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Execution_claim_race_allows_exactly_one_worker()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        Guid operationId;
        await using (var setup = PostgreSqlTestEnvironment.CreateDbContext(connection))
        {
            await setup.Database.MigrateAsync();
            var scope = await SeedAsync(setup, "EXECUTION-RACE");
            var runtime = new SyncProofRuntimeService(setup, new AuditEventService(setup));
            var nonce = await runtime.IssueNonceAsync(scope.Security);
            var proof = await runtime.ClaimAsync(scope.Security,
                Proof(nonce.Value, Guid.NewGuid().ToString("D"), scope.Thumbprint));
            operationId = (await CreateOperationService(setup).EnqueueAcceptedSyncOperationAsync(
                Command("execution-race-" + Guid.NewGuid().ToString("N"), "{}"), proof)).Id;
            await RejectOtherExecutionCandidatesAsync(setup, operationId);
        }

        await using var firstDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await using var secondDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
        var claimAt = Normalize(DateTimeOffset.UtcNow);
        var first = CreateOperationService(firstDb).ClaimNextExecutionAsync(
            TimeSpan.FromMinutes(2), claimAt);
        var second = CreateOperationService(secondDb).ClaimNextExecutionAsync(
            TimeSpan.FromMinutes(2), claimAt);
        var claims = await Task.WhenAll(first, second);

        var winner = Assert.Single(claims, x => x is not null)!;
        Assert.Equal(operationId, winner.OperationId);
        Assert.Single(claims, x => x is null);

        await using var verify = PostgreSqlTestEnvironment.CreateDbContext(connection);
        var persisted = await verify.SyncOperations.SingleAsync(x => x.Id == operationId);
        Assert.Equal("SENDING", persisted.Status);
        Assert.Equal(winner.ClaimToken, persisted.ExecutionClaimToken);
        Assert.Equal(0, persisted.RetryCount);
        Assert.Single(await verify.AuditEvents.Where(x => x.EntityId == operationId &&
            x.Action == "SyncOperationExecutionClaimed").ToListAsync());
        await CreateOperationService(verify).CompleteExecutionFailureAsync(
            operationId, winner.ClaimToken, "TEST_EXECUTION_COMPLETE", claimAt.AddSeconds(1));
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Expired_sending_lease_is_recovered_after_restart_without_consuming_retry()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        Guid operationId;
        SyncOperationExecutionClaim original;
        SyncOperationExecutionClaim recovered;
        var claimedAt = Normalize(DateTimeOffset.UtcNow.AddMinutes(1));
        await using (var firstProcess = PostgreSqlTestEnvironment.CreateDbContext(connection))
        {
            await firstProcess.Database.MigrateAsync();
            var scope = await SeedAsync(firstProcess, "EXECUTION-RESTART");
            var runtime = new SyncProofRuntimeService(firstProcess, new AuditEventService(firstProcess));
            var nonce = await runtime.IssueNonceAsync(scope.Security);
            var proof = await runtime.ClaimAsync(scope.Security,
                Proof(nonce.Value, Guid.NewGuid().ToString("D"), scope.Thumbprint));
            operationId = (await CreateOperationService(firstProcess).EnqueueAcceptedSyncOperationAsync(
                Command("execution-restart-" + Guid.NewGuid().ToString("N"), "{}"), proof)).Id;
            await RejectOtherExecutionCandidatesAsync(firstProcess, operationId);
            original = Assert.IsType<SyncOperationExecutionClaim>(await CreateOperationService(firstProcess)
                .ClaimNextExecutionAsync(TimeSpan.FromMinutes(2), claimedAt));
        }

        await using (var beforeExpiryDb = PostgreSqlTestEnvironment.CreateDbContext(connection))
        {
            var beforeExpiry = await CreateOperationService(beforeExpiryDb).ClaimNextExecutionAsync(
                TimeSpan.FromMinutes(2), claimedAt.AddMinutes(1));
            Assert.Null(beforeExpiry);
        }

        await using (var restartedDb = PostgreSqlTestEnvironment.CreateDbContext(connection))
        {
            var restartedService = CreateOperationService(restartedDb);
            recovered = Assert.IsType<SyncOperationExecutionClaim>(await restartedService
                .ClaimNextExecutionAsync(TimeSpan.FromMinutes(2), claimedAt.AddMinutes(3)));
            Assert.Equal(operationId, recovered.OperationId);
            Assert.True(recovered.RecoveredStaleClaim);
            Assert.NotEqual(original.ClaimToken, recovered.ClaimToken);
            Assert.Equal(0, recovered.ServerRetryCount);
            var staleOwner = await Assert.ThrowsAsync<SyncRuleException>(() => restartedService
                .CompleteExecutionSuccessAsync(operationId, original.ClaimToken,
                    new SyncExecutionSuccess(Guid.NewGuid(), 1), claimedAt.AddMinutes(3).AddSeconds(1)));
            Assert.Equal("EXECUTION_CLAIM_LOST", staleOwner.Code);
        }

        await using var verify = PostgreSqlTestEnvironment.CreateDbContext(connection);
        var persisted = await verify.SyncOperations.SingleAsync(x => x.Id == operationId);
        Assert.Equal(0, persisted.RetryCount);
        Assert.Equal(2, await verify.AuditEvents.CountAsync(x => x.EntityId == operationId &&
            (x.Action == "SyncOperationExecutionClaimed" ||
             x.Action == "SyncOperationExecutionReclaimed")));
        await CreateOperationService(verify).CompleteExecutionFailureAsync(
            operationId, recovered.ClaimToken, "TEST_EXECUTION_COMPLETE", claimedAt.AddMinutes(3).AddSeconds(1));
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Committed_effect_with_pending_completion_is_reclaimed_and_finishes_success_without_repeat()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "EXECUTION-COMPLETION-RECOVERY");
        var proofRuntime = new SyncProofRuntimeService(db, new AuditEventService(db));
        var nonce = await proofRuntime.IssueNonceAsync(scope.Security);
        var proof = await proofRuntime.ClaimAsync(scope.Security,
            Proof(nonce.Value, Guid.NewGuid().ToString("D"), scope.Thumbprint));
        var service = CreateOperationService(db);
        var operation = await service.EnqueueAcceptedSyncOperationAsync(
            Command("completion-recovery-" + Guid.NewGuid().ToString("N"), "{}"), proof);
        await RejectOtherExecutionCandidatesAsync(db, operation.Id);
        var resultEntityId = Guid.NewGuid();
        var executor = new PendingThenRecoveredExecutor(resultEntityId);
        var processor = new SyncExecutionProcessor(service, executor);
        var firstAt = Normalize(DateTimeOffset.UtcNow.AddMinutes(1));

        Assert.True(await processor.ExecuteNextAsync(TimeSpan.FromSeconds(5), firstAt));
        db.ChangeTracker.Clear();
        var pending = await db.SyncOperations.AsNoTracking().SingleAsync(x => x.Id == operation.Id);
        Assert.Equal("FAILED", pending.Status);
        Assert.Equal("COMPLETION_PENDING", pending.ErrorCode);
        Assert.Equal(1, pending.RetryCount);
        Assert.Equal(firstAt.AddSeconds(1), pending.NextRetryAt);
        Assert.Null(pending.ExecutionClaimToken);

        Assert.True(await processor.ExecuteNextAsync(TimeSpan.FromSeconds(5), pending.NextRetryAt));
        db.ChangeTracker.Clear();
        var completed = await db.SyncOperations.AsNoTracking().SingleAsync(x => x.Id == operation.Id);
        Assert.Equal("SUCCEEDED", completed.Status);
        Assert.Equal(resultEntityId, completed.ResultEntityId);
        Assert.Equal(1, completed.ResultVersion);
        Assert.Equal(1, executor.EffectCount);
        Assert.True(await db.AuditEvents.AnyAsync(x => x.EntityId == operation.Id &&
            x.Action == "SyncOperationCompletionRecoveryScheduled"));
        Assert.True(await db.AuditEvents.AnyAsync(x => x.EntityId == operation.Id &&
            x.Action == "SyncOperationExecutionSucceeded"));
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Permanent_completion_ambiguity_exhausts_original_plus_five_without_false_rejection()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "COMPLETION-EXHAUSTION");
        var runtime = new SyncProofRuntimeService(db, new AuditEventService(db));
        var nonce = await runtime.IssueNonceAsync(scope.Security);
        var proof = await runtime.ClaimAsync(scope.Security,
            Proof(nonce.Value, Guid.NewGuid().ToString("D"), scope.Thumbprint));
        var service = new SyncOperationService(db, new AuditEventService(db),
            new SyncRetryPolicy(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(80)));
        var operation = await service.EnqueueAcceptedSyncOperationAsync(
            Command("completion-exhaustion-" + Guid.NewGuid().ToString("N"), "{}"), proof);
        await RejectOtherExecutionCandidatesAsync(db, operation.Id);
        var executor = new PermanentlyPendingExecutor();
        var processor = new SyncExecutionProcessor(service, executor);
        var dueAt = Normalize(DateTimeOffset.UtcNow.AddMinutes(1));

        for (var attempt = 0; attempt <= 5; attempt++)
        {
            Assert.True(await processor.ExecuteNextAsync(TimeSpan.FromMinutes(2), dueAt));
            db.ChangeTracker.Clear();
            operation = await db.SyncOperations.AsNoTracking().SingleAsync(x => x.Id == operation.Id);
            if (attempt < 5)
            {
                Assert.Equal("FAILED", operation.Status);
                Assert.Equal("COMPLETION_PENDING", operation.ErrorCode);
                Assert.Equal(attempt + 1, operation.RetryCount);
                Assert.NotNull(operation.NextRetryAt);
                dueAt = operation.NextRetryAt!.Value;
            }
        }

        Assert.Equal(6, executor.AttemptCount);
        Assert.Equal("FAILED", operation.Status);
        Assert.Equal("COMPLETION_RECOVERY_EXHAUSTED", operation.ErrorCode);
        Assert.Equal(5, operation.RetryCount);
        Assert.Null(operation.NextRetryAt);
        Assert.Null(operation.ExecutionClaimToken);
        Assert.Null(await service.ClaimNextExecutionAsync(
            TimeSpan.FromMinutes(2), dueAt.AddMinutes(10)));
        Assert.Equal(5, await db.AuditEvents.CountAsync(x => x.EntityId == operation.Id &&
            x.Action == "SyncOperationCompletionRecoveryScheduled"));
        Assert.Single(await db.AuditEvents.Where(x => x.EntityId == operation.Id &&
            x.Action == "SyncOperationCompletionRecoveryExhausted").ToListAsync());
        Assert.False(await db.AuditEvents.AnyAsync(x => x.EntityId == operation.Id &&
            x.Action == "SyncOperationExecutionRejected"));
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Permanent_unclassified_executor_failure_is_rejected_once_with_audit()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "EXECUTOR-PERMANENT-FAILURE");
        var runtime = new SyncProofRuntimeService(db, new AuditEventService(db));
        var nonce = await runtime.IssueNonceAsync(scope.Security);
        var proof = await runtime.ClaimAsync(scope.Security,
            Proof(nonce.Value, Guid.NewGuid().ToString("D"), scope.Thumbprint));
        var service = CreateOperationService(db);
        var operation = await service.EnqueueAcceptedSyncOperationAsync(
            Command("executor-permanent-" + Guid.NewGuid().ToString("N"), "{}"), proof);
        await RejectOtherExecutionCandidatesAsync(db, operation.Id);
        var processor = new SyncExecutionProcessor(service, new ThrowingExecutor());
        var attemptAt = Normalize(DateTimeOffset.UtcNow.AddMinutes(1));

        Assert.True(await processor.ExecuteNextAsync(TimeSpan.FromMinutes(2), attemptAt));
        db.ChangeTracker.Clear();
        operation = await db.SyncOperations.AsNoTracking().SingleAsync(x => x.Id == operation.Id);

        Assert.Equal("REJECTED", operation.Status);
        Assert.Equal("ACTION_EXECUTION_FAILED", operation.ErrorCode);
        Assert.Equal(0, operation.RetryCount);
        Assert.Null(operation.NextRetryAt);
        Assert.Single(await db.AuditEvents.Where(x => x.EntityId == operation.Id &&
            x.Action == "SyncOperationExecutionRejected").ToListAsync());
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Actual_rate_limited_failures_alone_consume_budget_and_exhaustion_clears_claim()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "EXECUTION-EXHAUSTION");
        var runtime = new SyncProofRuntimeService(db, new AuditEventService(db));
        var nonce = await runtime.IssueNonceAsync(scope.Security);
        var proof = await runtime.ClaimAsync(scope.Security,
            Proof(nonce.Value, Guid.NewGuid().ToString("D"), scope.Thumbprint));
        var policy = new SyncRetryPolicy(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(80));
        var service = new SyncOperationService(db, new AuditEventService(db), policy);
        var operation = await service.EnqueueAcceptedSyncOperationAsync(
            Command("execution-exhaustion-" + Guid.NewGuid().ToString("N"), "{}"), proof);
        await RejectOtherExecutionCandidatesAsync(db, operation.Id);
        var attemptAt = Normalize(DateTimeOffset.UtcNow.AddMinutes(1));

        var dueAt = attemptAt;
        var expectedDelays = new[] { 5, 10, 20, 40, 80 };
        for (var completedFailures = 0; completedFailures <= expectedDelays.Length; completedFailures++)
        {
            var claim = Assert.IsType<SyncOperationExecutionClaim>(await service.ClaimNextExecutionAsync(
                TimeSpan.FromMinutes(2), dueAt));
            Assert.Equal(completedFailures, claim.ServerRetryCount);
            var failedAt = dueAt.AddSeconds(1);
            operation = await service.CompleteExecutionFailureAsync(
                operation.Id, claim.ClaimToken, "RATE_LIMITED", failedAt);

            if (completedFailures < expectedDelays.Length)
            {
                Assert.Equal("FAILED", operation.Status);
                Assert.Equal(completedFailures + 1, operation.RetryCount);
                Assert.Null(operation.ExecutionClaimToken);
                Assert.Equal(failedAt.AddSeconds(expectedDelays[completedFailures]), operation.NextRetryAt);
                dueAt = operation.NextRetryAt!.Value;
            }
        }

        Assert.Equal("REJECTED", operation.Status);
        Assert.Equal("RETRY_EXHAUSTED", operation.ErrorCode);
        Assert.Equal(5, operation.RetryCount);
        Assert.Null(operation.NextRetryAt);
        Assert.Null(operation.ExecutionClaimToken);
        Assert.Null(operation.ExecutionAttemptStartedAt);
        Assert.Null(operation.ExecutionLeaseExpiresAt);
        Assert.Null(await service.ClaimNextExecutionAsync(
            TimeSpan.FromMinutes(2), dueAt.AddMinutes(10)));
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Worker_uses_effective_scope_retry_budget_and_recomputed_backoff()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "EFFECTIVE-WORKER-RETRY");
        var runtime = new SyncProofRuntimeService(db, new AuditEventService(db));
        var nonce = await runtime.IssueNonceAsync(scope.Security);
        var proof = await runtime.ClaimAsync(scope.Security,
            Proof(nonce.Value, Guid.NewGuid().ToString("D"), scope.Thumbprint));
        var initialEffective = new SyncRetryPolicy(1, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(2));
        var tightenedEffective = new SyncRetryPolicy(1, TimeSpan.FromSeconds(13), TimeSpan.FromMinutes(2));
        var service = new SyncOperationService(
            db, new AuditEventService(db),
            new SyncRetryPolicy(5, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(30)),
            new StaticRetryPolicyResolver(initialEffective));
        var operation = await service.EnqueueAcceptedSyncOperationAsync(
            Command("effective-worker-retry-" + Guid.NewGuid().ToString("N"), "{}"), proof);
        await RejectOtherExecutionCandidatesAsync(db, operation.Id);
        var firstAt = Normalize(DateTimeOffset.UtcNow.AddMinutes(1));

        var firstClaim = Assert.IsType<SyncOperationExecutionClaim>(await service.ClaimNextExecutionAsync(
            TimeSpan.FromMinutes(2), firstAt));
        operation = await service.CompleteExecutionFailureAsync(
            operation.Id, firstClaim.ClaimToken, "RATE_LIMITED", firstAt);

        Assert.Equal(1, operation.RetryCount);
        Assert.Equal(firstAt.AddSeconds(5), operation.NextRetryAt);

        var originalDueAt = operation.NextRetryAt;
        service = new SyncOperationService(
            db, new AuditEventService(db),
            new SyncRetryPolicy(5, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(30)),
            new StaticRetryPolicyResolver(tightenedEffective));
        Assert.Null(await service.ClaimNextExecutionAsync(TimeSpan.FromMinutes(2), originalDueAt));
        db.ChangeTracker.Clear();
        operation = await db.SyncOperations.AsNoTracking().SingleAsync(x => x.Id == operation.Id);
        Assert.Equal(firstAt.AddSeconds(13), operation.NextRetryAt);
        Assert.Single(await db.AuditEvents.Where(x => x.EntityId == operation.Id &&
            x.Action == "SyncOperationExecutionBackoffTightened").ToListAsync());

        var dueAt = operation.NextRetryAt;
        var secondClaim = Assert.IsType<SyncOperationExecutionClaim>(await service.ClaimNextExecutionAsync(
            TimeSpan.FromMinutes(2), dueAt));
        operation = await service.CompleteExecutionFailureAsync(
            operation.Id, secondClaim.ClaimToken, "RATE_LIMITED", dueAt);

        Assert.Equal("REJECTED", operation.Status);
        Assert.Equal("RETRY_EXHAUSTED", operation.ErrorCode);
        Assert.Equal(1, operation.RetryCount);
        Assert.Null(operation.NextRetryAt);
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Worker_policy_lookup_missing_fails_closed_without_business_claim()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "WORKER-POLICY-MISSING");
        var runtime = new SyncProofRuntimeService(db, new AuditEventService(db));
        var nonce = await runtime.IssueNonceAsync(scope.Security);
        var proof = await runtime.ClaimAsync(scope.Security,
            Proof(nonce.Value, Guid.NewGuid().ToString("D"), scope.Thumbprint));
        var service = new SyncOperationService(
            db, new AuditEventService(db),
            new SyncRetryPolicy(5, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(30)),
            new StaticRetryPolicyResolver(null));
        var operation = await service.EnqueueAcceptedSyncOperationAsync(
            Command("worker-policy-missing-" + Guid.NewGuid().ToString("N"), "{}"), proof);
        await RejectOtherExecutionCandidatesAsync(db, operation.Id);

        var claim = await service.ClaimNextExecutionAsync(
            TimeSpan.FromMinutes(2), Normalize(DateTimeOffset.UtcNow.AddMinutes(1)));
        db.ChangeTracker.Clear();
        operation = await db.SyncOperations.AsNoTracking().SingleAsync(x => x.Id == operation.Id);

        Assert.Null(claim);
        Assert.Equal("FAILED", operation.Status);
        Assert.Equal("SYNC_RUNTIME_POLICY_UNAVAILABLE", operation.ErrorCode);
        Assert.Null(operation.ExecutionClaimToken);
        Assert.Null(operation.NextRetryAt);
        Assert.Single(await db.AuditEvents.Where(x => x.EntityId == operation.Id &&
            x.Action == "SyncOperationExecutionPolicyUnavailable").ToListAsync());
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Worker_rejects_action_tightened_after_enqueue_before_business_claim()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "WORKER-ACTION-TIGHTENED");
        var runtime = new SyncProofRuntimeService(db, new AuditEventService(db));
        var nonce = await runtime.IssueNonceAsync(scope.Security);
        var proof = await runtime.ClaimAsync(scope.Security,
            Proof(nonce.Value, Guid.NewGuid().ToString("D"), scope.Thumbprint));
        var service = new SyncOperationService(
            db, new AuditEventService(db),
            new SyncRetryPolicy(5, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(30)),
            new StaticRetryPolicyResolver(
                new SyncRetryPolicy(5, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(30)),
                SyncExecutionPolicyDecision.Denied("SCOPE_DENIED")));
        var operation = await service.EnqueueAcceptedSyncOperationAsync(
            Command("worker-action-tightened-" + Guid.NewGuid().ToString("N"), "{}"), proof);
        await RejectOtherExecutionCandidatesAsync(db, operation.Id);

        var claim = await service.ClaimNextExecutionAsync(
            TimeSpan.FromMinutes(2), Normalize(DateTimeOffset.UtcNow.AddMinutes(1)));
        db.ChangeTracker.Clear();
        operation = await db.SyncOperations.AsNoTracking().SingleAsync(x => x.Id == operation.Id);

        Assert.Null(claim);
        Assert.Equal("REJECTED", operation.Status);
        Assert.Equal("SCOPE_DENIED", operation.ErrorCode);
        Assert.Null(operation.ExecutionClaimToken);
        Assert.Single(await db.AuditEvents.Where(x => x.EntityId == operation.Id &&
            x.Action == "SyncOperationExecutionPolicyDenied" && x.Outcome == "REJECTED").ToListAsync());
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Enqueue_proof_replay_does_not_consume_server_execution_retry_budget()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "EXECUTION-ENQUEUE-REPLAY");
        var runtime = new SyncProofRuntimeService(db, new AuditEventService(db));
        var firstNonce = await runtime.IssueNonceAsync(scope.Security);
        var firstProof = await runtime.ClaimAsync(scope.Security,
            Proof(firstNonce.Value, Guid.NewGuid().ToString("D"), scope.Thumbprint));
        var secondNonce = await runtime.IssueNonceAsync(scope.Security);
        var secondProof = await runtime.ClaimAsync(scope.Security,
            Proof(secondNonce.Value, Guid.NewGuid().ToString("D"), scope.Thumbprint));
        var command = Command("execution-enqueue-replay-" + Guid.NewGuid().ToString("N"), "{}");
        var service = CreateOperationService(db);

        var first = await service.EnqueueAcceptedSyncOperationAsync(command, firstProof);
        var replay = await service.EnqueueAcceptedSyncOperationAsync(command, secondProof);

        Assert.Equal(first.Id, replay.Id);
        Assert.Equal(0, replay.RetryCount);
        Assert.Equal("QUEUED", replay.Status);
        Assert.Null(replay.ExecutionClaimToken);
        Assert.Single(await db.SyncOperations.Where(x => x.Id == first.Id).ToListAsync());
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Stale_base_version_becomes_atomic_typed_conflict_then_keep_server_resolves_it()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "EXECUTION-CONFLICT");
        var proofRuntime = new SyncProofRuntimeService(db, new AuditEventService(db));
        var firstNonce = await proofRuntime.IssueNonceAsync(scope.Security);
        var firstProof = await proofRuntime.ClaimAsync(scope.Security,
            Proof(firstNonce.Value, Guid.NewGuid().ToString("D"), scope.Thumbprint));
        const string privatePayload = "{\"name\":\"private-person\",\"mobile\":\"0500000000\"}";
        var entityId = Guid.NewGuid();
        var operationCorrelationId = Guid.NewGuid();
        var service = CreateOperationService(db);
        var operation = await service.EnqueueAcceptedSyncOperationAsync(
            new EnqueueAcceptedSyncOperationCommand(
                "sync-v1", "UpdateWaybillDraft", "UPDATE", "Waybill", entityId,
                "stale-" + Guid.NewGuid().ToString("N"), privatePayload, Hash(privatePayload),
                DateTimeOffset.UtcNow, operationCorrelationId, 7), firstProof);
        await RejectOtherExecutionCandidatesAsync(db, operation.Id);
        var claimAt = Normalize(DateTimeOffset.UtcNow);
        var claim = Assert.IsType<SyncOperationExecutionClaim>(await service.ClaimNextExecutionAsync(
            TimeSpan.FromMinutes(2), claimAt));

        operation = await service.CompleteExecutionConflictAsync(
            operation.Id, claim.ClaimToken, "CONCURRENCY_CONFLICT");

        Assert.Equal("CONFLICT", operation.Status);
        var conflict = await db.ConflictCases.SingleAsync(x => x.SyncOperationId == operation.Id);
        Assert.Equal(7, conflict.BaseVersion);
        Assert.Contains("RequestedBaseVersion", conflict.DeviceSnapshot, StringComparison.Ordinal);
        Assert.Contains("CurrentVersion", conflict.ServerSnapshot, StringComparison.Ordinal);
        Assert.DoesNotContain("private-person", conflict.DeviceSnapshot + conflict.ServerSnapshot, StringComparison.Ordinal);
        Assert.DoesNotContain("0500000000", conflict.DeviceSnapshot + conflict.ServerSnapshot, StringComparison.Ordinal);
        Assert.Single(await db.AuditEvents.Where(x => x.EntityId == operation.Id &&
            x.Action == "SyncOperationExecutionConflict").ToListAsync());

        var resolutionNonce = await proofRuntime.IssueNonceAsync(scope.Security);
        var resolutionProof = await proofRuntime.ClaimAsync(scope.Security,
            Proof(resolutionNonce.Value, Guid.NewGuid().ToString("D"), scope.Thumbprint));
        var resolutionContext = new SyncConflictResolutionContext(
            scope.Security.UserId, scope.Security.CompanyId, scope.Security.BranchId,
            scope.Security.RegisteredDeviceId, 1, scope.Security.DeviceId,
            resolutionProof.AttemptCorrelationId);
        var resolved = await new SyncConflictResolutionService(
            db, new AuditEventService(db), new AllowPermissionResolver(), service).ResolveAsync(
            conflict.Id,
            new ResolveSyncConflictRequest(
                SyncConflictResolutionDecisions.KeepServerAndRejectLocal, "reviewed version conflict"),
            resolutionContext, resolutionProof);

        Assert.Equal("RESOLVED", resolved.ConflictStatus);
        Assert.Equal("REJECTED", resolved.OriginalOperationStatus);
        Assert.Equal("KEEP_SERVER", resolved.OriginalOperationErrorCode);
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Batch_action_permission_is_denied_before_operation_or_queue_audit_is_persisted()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "ACTION-PERMISSION-DENY");
        var payload = "{}";
        var item = new SyncBatchOperationRequest(
            "CreateOperationalParty", "CREATE", "OperationalParty", null,
            "denied-" + Guid.NewGuid().ToString("N"), payload, Hash(payload),
            DateTimeOffset.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss.ffffff'Z'"), Guid.NewGuid());
        var unavailable = new SyncBatchOperationRequest(
            "CreateJournalEntry", "CREATE", "JournalEntry", null,
            "denied-unavailable-" + Guid.NewGuid().ToString("N"), payload, Hash(payload),
            DateTimeOffset.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss.ffffff'Z'"), Guid.NewGuid());
        var request = new SyncBatchRequest(scope.Security.DeviceId, "sync-v1", [item, unavailable]);
        var body = JsonSerializer.SerializeToUtf8Bytes(request, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        var http = new DefaultHttpContext();
        http.Request.Body = new MemoryStream(body);
        var accepted = new AcceptedSyncHttpRequest(
            new TransportERP.Api.Security.CurrentSecurityContext(
                scope.Security.UserId, scope.Security.CompanyId, scope.Security.BranchId,
                Guid.NewGuid(), scope.Security.DeviceId, true, scope.Security.RegisteredDeviceId, 1),
            scope.Security,
            new AcceptedSyncProofContext(Guid.NewGuid(), scope.Security.UserId, scope.Security.CompanyId,
                scope.Security.BranchId, scope.Security.RegisteredDeviceId, scope.Security.DeviceId,
                1, 1, scope.Thumbprint, Guid.NewGuid()),
            body,
            Guid.NewGuid(),
            new EffectiveSyncPolicy(
                true,
                SyncActionCatalog.Definitions.Select(x => x.ActionCodeValue)
                    .ToHashSet(StringComparer.Ordinal),
                new HashSet<string>(["sync-v1"], StringComparer.Ordinal),
                100, 2_097_152, 16_384, 5, 5, 5, 5, 30, 30, 24, 7, 90, 24, null));
        var rowsBefore = await db.SyncOperations.CountAsync();
        var auditsBefore = await db.AuditEvents.CountAsync(x => x.Action == "SyncOperationQueued");

        var httpResult = await SyncApiModule.HandleBatchAsync(
            http, new AcceptedRequestAuthenticator(accepted), CreateOperationService(db),
            new DenyPermissionResolver(),
            new SyncBatchRejectionAuditSink(new AuditEventService(db)),
            CancellationToken.None);

        var response = Assert.IsType<SyncBatchResponse>(
            Assert.IsAssignableFrom<IValueHttpResult>(httpResult).Value);
        Assert.Equal(2, response.Results.Count);
        Assert.All(response.Results, denied =>
        {
            Assert.Equal("REJECTED", denied.Status);
            Assert.Equal("SCOPE_DENIED", denied.ErrorCode);
            Assert.Null(denied.ServerOperationId);
        });
        Assert.Equal(rowsBefore, await db.SyncOperations.CountAsync());
        Assert.Equal(auditsBefore, await db.AuditEvents.CountAsync(x => x.Action == "SyncOperationQueued"));
        var rejectionAudits = await db.AuditEvents.Where(x =>
            x.Action == "SyncOperationRejected" && x.Outcome == "REJECTED" &&
            x.Reason == "SCOPE_DENIED" && x.CompanyId == scope.Security.CompanyId &&
            x.BranchId == scope.Security.BranchId && x.ActorUserId == scope.Security.UserId &&
            x.DeviceId == scope.Security.DeviceId).ToListAsync();
        Assert.Equal(2, rejectionAudits.Count);
        Assert.All(rejectionAudits, auditEvent =>
        {
            Assert.Null(auditEvent.EntityId);
            Assert.Null(auditEvent.BeforeJson);
            Assert.Null(auditEvent.AfterJson);
            Assert.NotNull(auditEvent.OperationCorrelationId);
        });
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Nonce_and_claim_are_tenant_scoped_and_expiry_is_rechecked_under_lock()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "SCOPE");
        var runtime = new SyncProofRuntimeService(db, new AuditEventService(db));
        var before = await db.SyncProofNonces.CountAsync();

        var tenantError = await Assert.ThrowsAsync<SyncProofRuntimeException>(() =>
            runtime.IssueNonceAsync(scope.Security with { CompanyId = Guid.NewGuid() }));
        Assert.Equal("DEVICE_NOT_REGISTERED", tenantError.Code);
        Assert.Equal(before, await db.SyncProofNonces.CountAsync());

        var nonce = await runtime.IssueNonceAsync(scope.Security);
        var wrongThumbprint = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
            .TrimEnd('=').Replace('+', '-').Replace('/', '_');
        var thumbprintError = await Assert.ThrowsAsync<SyncProofRuntimeException>(() => runtime.ClaimAsync(
            scope.Security, Proof(nonce.Value, Guid.NewGuid().ToString("D"), wrongThumbprint)));
        Assert.Equal("invalid_dpop_proof", thumbprintError.Code);
        Assert.False(await db.SyncProofReplays.AnyAsync(x => x.RegisteredDeviceId == scope.DeviceId));

        var row = await db.SyncProofNonces.SingleAsync(x => x.RegisteredDeviceId == scope.DeviceId);
        row.IssuedAt = DateTimeOffset.UtcNow.AddMinutes(-6);
        row.ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(-1);
        await db.SaveChangesAsync();
        var expired = await Assert.ThrowsAsync<SyncProofRuntimeException>(() => runtime.ClaimAsync(
            scope.Security, Proof(nonce.Value, Guid.NewGuid().ToString("D"), scope.Thumbprint)));
        Assert.Equal("use_dpop_nonce", expired.Code);
        Assert.False(await db.SyncProofReplays.AnyAsync(x => x.RegisteredDeviceId == scope.DeviceId));
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Claim_rechecks_device_assignment_expiry_and_rotated_key_state()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();

        async Task AssertRejectedAsync(
            string suffix,
            Func<TestScope, Task> mutate,
            string expectedCode)
        {
            var scope = await SeedAsync(db, suffix);
            var runtime = new SyncProofRuntimeService(db, new AuditEventService(db));
            var nonce = await runtime.IssueNonceAsync(scope.Security);
            await mutate(scope);
            db.ChangeTracker.Clear();

            var error = await Assert.ThrowsAsync<SyncProofRuntimeException>(() => runtime.ClaimAsync(
                scope.Security, Proof(nonce.Value, Guid.NewGuid().ToString("D"), scope.Thumbprint)));

            Assert.Equal(expectedCode, error.Code);
            Assert.False(await db.SyncProofReplays.AnyAsync(x => x.RegisteredDeviceId == scope.DeviceId));
        }

        await AssertRejectedAsync("CLAIM-DEVICE-SUSPENDED", async scope =>
        {
            await db.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE transport_erp.registered_devices SET "Status"='SUSPENDED'
                WHERE "Id"={scope.DeviceId}
                """);
        }, "DEVICE_NOT_REGISTERED");

        await AssertRejectedAsync("CLAIM-ASSIGNMENT-REVOKED", async scope =>
        {
            await db.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE transport_erp.registered_device_assignments SET "Status"='REVOKED'
                WHERE "RegisteredDeviceId"={scope.DeviceId}
                """);
        }, "DEVICE_NOT_REGISTERED");

        await AssertRejectedAsync("CLAIM-DEVICE-EXPIRED", async scope =>
        {
            await db.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE transport_erp.registered_devices SET "ExpiresAt"={DateTimeOffset.UtcNow.AddMinutes(-1)}
                WHERE "Id"={scope.DeviceId}
                """);
        }, "DEVICE_NOT_REGISTERED");

        await AssertRejectedAsync("CLAIM-KEY-ROTATED", async scope =>
        {
            var rotatedThumbprint = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
                .TrimEnd('=').Replace('+', '-').Replace('/', '_');
            await db.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE transport_erp.registered_devices
                SET "ProofKeyVersion"=2, "ProofKeyThumbprint"={rotatedThumbprint}
                WHERE "Id"={scope.DeviceId}
                """);
        }, "invalid_dpop_proof");
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Third_nonce_hash_collision_is_mapped_without_leaking_db_exception()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "NONCE-COLLISION");
        var runtime = new SyncProofRuntimeService(db, new AuditEventService(db));
        _ = await runtime.IssueNonceAsync(scope.Security);
        var suffix = Guid.NewGuid().ToString("N");
        var function = $"force_nonce_collision_{suffix}";
        var trigger = $"trg_force_nonce_collision_{suffix}";
        await db.Database.ExecuteSqlRawAsync($$"""
            CREATE FUNCTION transport_erp.{{function}}() RETURNS trigger LANGUAGE plpgsql AS $body$
            BEGIN
              SELECT "NonceHash" INTO NEW."NonceHash" FROM transport_erp.sync_proof_nonces ORDER BY "Id" LIMIT 1;
              RETURN NEW;
            END $body$;
            CREATE TRIGGER {{trigger}} BEFORE INSERT ON transport_erp.sync_proof_nonces
              FOR EACH ROW EXECUTE FUNCTION transport_erp.{{function}}();
            """);
        try
        {
            var error = await Assert.ThrowsAsync<SyncProofRuntimeException>(() =>
                runtime.IssueNonceAsync(scope.Security));
            Assert.Equal("NONCE_GENERATION_FAILED", error.Code);
        }
        finally
        {
            await using var cleanup = PostgreSqlTestEnvironment.CreateDbContext(connection);
            await cleanup.Database.ExecuteSqlRawAsync($$"""
                DROP TRIGGER IF EXISTS {{trigger}} ON transport_erp.sync_proof_nonces;
                DROP FUNCTION IF EXISTS transport_erp.{{function}}();
                """);
        }
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Exact_legacy_unique_constraint_is_mapped_to_legacy_idempotency_conflict()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "LEGACY-MAP");
        var proofRuntime = new SyncProofRuntimeService(db, new AuditEventService(db));
        var nonce = await proofRuntime.IssueNonceAsync(scope.Security);
        var accepted = await proofRuntime.ClaimAsync(scope.Security,
            Proof(nonce.Value, Guid.NewGuid().ToString("D"), scope.Thumbprint));
        var suffix = Guid.NewGuid().ToString("N");
        var function = $"force_legacy_unique_{suffix}";
        var trigger = $"trg_force_legacy_unique_{suffix}";
        await db.Database.ExecuteSqlRawAsync($$"""
            CREATE FUNCTION transport_erp.{{function}}() RETURNS trigger LANGUAGE plpgsql AS $body$
            BEGIN
              RAISE unique_violation USING CONSTRAINT='ux_sync_op_legacy_company_device_client';
            END $body$;
            CREATE TRIGGER {{trigger}} BEFORE INSERT ON transport_erp.sync_operations
              FOR EACH ROW EXECUTE FUNCTION transport_erp.{{function}}();
            """);
        try
        {
            var error = await Assert.ThrowsAsync<SyncRuleException>(() => CreateOperationService(db)
                .EnqueueAcceptedSyncOperationAsync(Command("client-" + Guid.NewGuid().ToString("N"), "{}"), accepted));
            Assert.Equal("LEGACY_IDEMPOTENCY_CONFLICT", error.Code);
        }
        finally
        {
            await using var cleanup = PostgreSqlTestEnvironment.CreateDbContext(connection);
            await cleanup.Database.ExecuteSqlRawAsync($$"""
                DROP TRIGGER IF EXISTS {{trigger}} ON transport_erp.sync_operations;
                DROP FUNCTION IF EXISTS transport_erp.{{function}}();
                """);
        }

        var unknownFunction = $"force_unknown_unique_{suffix}";
        var unknownTrigger = $"trg_force_unknown_unique_{suffix}";
        await using var unknownDb = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await unknownDb.Database.ExecuteSqlRawAsync($$"""
            CREATE FUNCTION transport_erp.{{unknownFunction}}() RETURNS trigger LANGUAGE plpgsql AS $body$
            BEGIN
              RAISE unique_violation USING CONSTRAINT='ux_unknown_sync_test';
            END $body$;
            CREATE TRIGGER {{unknownTrigger}} BEFORE INSERT ON transport_erp.sync_operations
              FOR EACH ROW EXECUTE FUNCTION transport_erp.{{unknownFunction}}();
            """);
        try
        {
            var unknown = await Assert.ThrowsAsync<DbUpdateException>(() => CreateOperationService(unknownDb)
                .EnqueueAcceptedSyncOperationAsync(Command("client-" + Guid.NewGuid().ToString("N"), "{}"), accepted));
            Assert.DoesNotContain("LEGACY_IDEMPOTENCY_CONFLICT", unknown.ToString(), StringComparison.Ordinal);
        }
        finally
        {
            await using var cleanup = PostgreSqlTestEnvironment.CreateDbContext(connection);
            await cleanup.Database.ExecuteSqlRawAsync($$"""
                DROP TRIGGER IF EXISTS {{unknownTrigger}} ON transport_erp.sync_operations;
                DROP FUNCTION IF EXISTS transport_erp.{{unknownFunction}}();
                """);
        }
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Cleanup_deletes_expired_replays_before_unreferenced_nonces_and_never_deletes_early()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedAsync(db, "CLEANUP");
        var now = Normalize(DateTimeOffset.UtcNow);
        var cleanup = new SyncProofCleanupService(db);
        // Cleanup is intentionally global. Remove expired evidence left by earlier
        // PostgreSQL fixtures before asserting exact counts for this arrangement.
        _ = await cleanup.CleanupExpiredAsync(now);

        async Task<SyncProofCleanupResult> CleanupWithAccountingAsync(DateTimeOffset cutoff)
        {
            var replaysBefore = await db.SyncProofReplays.CountAsync();
            var noncesBefore = await db.SyncProofNonces.CountAsync();
            var result = await cleanup.CleanupExpiredAsync(cutoff);
            var replaysAfter = await db.SyncProofReplays.CountAsync();
            var noncesAfter = await db.SyncProofNonces.CountAsync();
            Assert.Equal(replaysBefore - replaysAfter, result.DeletedReplays);
            Assert.Equal(noncesBefore - noncesAfter, result.DeletedNonces);
            return result;
        }
        var assignmentId = await db.RegisteredDeviceAssignments
            .Where(x => x.RegisteredDeviceId == scope.DeviceId && x.Status == "ACTIVE")
            .Select(x => x.Id).SingleAsync();
        var expiredReferenced = Nonce(scope, now.AddMinutes(-20), now.AddMinutes(-15));
        var expiredStillReferenced = Nonce(scope, now.AddMinutes(-6), now.AddMinutes(-1));
        var futureUnreferenced = Nonce(scope, now.AddMinutes(-4), now.AddMinutes(1));
        var expiredUnreferenced = Nonce(scope, now.AddMinutes(-6), now.AddMinutes(-1));
        var shortExpiryReferenced = Nonce(scope, now.AddMinutes(-6), now.AddMinutes(-1));
        db.SyncProofNonces.AddRange(expiredReferenced, expiredStillReferenced,
            futureUnreferenced, expiredUnreferenced, shortExpiryReferenced);
        await db.SaveChangesAsync();
        var boundaryReplay = Replay(scope, assignmentId, expiredReferenced.Id,
            now.AddMinutes(-10), now);
        var futureReplay = Replay(scope, assignmentId, expiredStillReferenced.Id,
            now.AddMinutes(-9), now.AddMinutes(1));
        var malformedShortReplay = Replay(scope, assignmentId, shortExpiryReferenced.Id,
            now.AddMinutes(-5), now.AddMinutes(-1));
        db.SyncProofReplays.AddRange(boundaryReplay, futureReplay, malformedShortReplay);
        await db.SaveChangesAsync();

        var first = await CleanupWithAccountingAsync(now);

        Assert.True(first.DeletedReplays >= 1);
        Assert.True(first.DeletedNonces >= 2);
        Assert.False(await db.SyncProofReplays.AnyAsync(x => x.Id == boundaryReplay.Id));
        Assert.False(await db.SyncProofNonces.AnyAsync(x => x.Id == expiredReferenced.Id));
        Assert.False(await db.SyncProofNonces.AnyAsync(x => x.Id == expiredUnreferenced.Id));
        Assert.True(await db.SyncProofReplays.AnyAsync(x => x.Id == futureReplay.Id));
        Assert.True(await db.SyncProofReplays.AnyAsync(x => x.Id == malformedShortReplay.Id));
        Assert.True(await db.SyncProofNonces.AnyAsync(x => x.Id == expiredStillReferenced.Id));
        Assert.True(await db.SyncProofNonces.AnyAsync(x => x.Id == shortExpiryReferenced.Id));
        Assert.True(await db.SyncProofNonces.AnyAsync(x => x.Id == futureUnreferenced.Id));

        var second = await CleanupWithAccountingAsync(now.AddMinutes(2));
        Assert.True(second.DeletedReplays >= 1);
        Assert.True(second.DeletedNonces >= 2);
        Assert.False(await db.SyncProofReplays.AnyAsync(x => x.Id == futureReplay.Id));
        Assert.True(await db.SyncProofReplays.AnyAsync(x => x.Id == malformedShortReplay.Id));
        Assert.True(await db.SyncProofNonces.AnyAsync(x => x.Id == shortExpiryReferenced.Id));

        var third = await CleanupWithAccountingAsync(now.AddMinutes(6));
        Assert.True(third.DeletedReplays >= 1);
        Assert.True(third.DeletedNonces >= 1);
        Assert.False(await db.SyncProofReplays.AnyAsync(x => x.Id == malformedShortReplay.Id));
        Assert.False(await db.SyncProofNonces.AnyAsync(x => x.RegisteredDeviceId == scope.DeviceId));
    }

    private static SyncOperationService CreateOperationService(TransportErpDbContext db)
        => new(db, new AuditEventService(db), new SyncRetryPolicy(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30)));

    private sealed class StaticRetryPolicyResolver(
        SyncRetryPolicy? policy,
        SyncExecutionPolicyDecision? executionDecision = null) : ISyncRetryPolicyResolver
    {
        public ValueTask<SyncRetryPolicy?> ResolveAsync(
            Guid companyId,
            Guid? branchId,
            Guid? registeredDeviceId,
            string? deviceId,
            CancellationToken cancellationToken = default)
            => ValueTask.FromResult(policy);

        public ValueTask<SyncExecutionPolicyDecision> AuthorizeExecutionAsync(
            Guid companyId,
            Guid? branchId,
            Guid? registeredDeviceId,
            string? deviceId,
            string actionCode,
            CancellationToken cancellationToken = default)
            => ValueTask.FromResult(executionDecision ?? SyncExecutionPolicyDecision.Allowed);
    }

    private static Task<int> RejectOtherExecutionCandidatesAsync(
        TransportErpDbContext db,
        Guid retainedOperationId)
        => db.Database.ExecuteSqlInterpolatedAsync($$"""
            UPDATE transport_erp.sync_operations
            SET "Status"='REJECTED',
                "ErrorCode"='TEST_EXECUTION_ISOLATION',
                "NextRetryAt"=NULL,
                "ExecutionClaimToken"=NULL,
                "ExecutionAttemptStartedAt"=NULL,
                "ExecutionLeaseExpiresAt"=NULL,
                "UpdatedAt"={{Normalize(DateTimeOffset.UtcNow)}}
            WHERE "Id"<>{{retainedOperationId}}
              AND "ActionCode" IS NOT NULL
              AND "Status" IN ('QUEUED','FAILED','SENDING')
            """);

    private static EnqueueAcceptedSyncOperationCommand Command(string clientOperationId, string payload)
        => new("sync-v1", "CreateJournalEntry", "CREATE", "JournalEntry", null,
            clientOperationId, payload, Hash(payload), DateTimeOffset.UtcNow,
            Guid.NewGuid(), null);

    private static VerifiedSyncProofMaterial Proof(string nonce, string jti, string thumbprint)
        => new(jti, nonce, thumbprint, DateTimeOffset.UtcNow, Htu, Guid.NewGuid());

    private static string Hash(string payload)
        => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(payload))).ToLowerInvariant();

    private static byte[] DecodeBase64Url(string value)
    {
        var padded = value.Replace('-', '+').Replace('_', '/');
        padded += new string('=', (4 - padded.Length % 4) % 4);
        return Convert.FromBase64String(padded);
    }

    private static SyncProofNonce Nonce(TestScope scope, DateTimeOffset issuedAt, DateTimeOffset expiresAt)
        => new()
        {
            Id = Guid.NewGuid(), CompanyId = scope.Security.CompanyId, RegisteredDeviceId = scope.DeviceId,
            DeviceId = scope.Security.DeviceId, ProofKeyVersion = 1,
            NonceHash = RandomNumberGenerator.GetBytes(32), IssuedAt = issuedAt, ExpiresAt = expiresAt
        };

    private static SyncProofReplay Replay(TestScope scope, Guid assignmentId, Guid nonceId,
        DateTimeOffset firstSeenAt, DateTimeOffset expiresAt)
        => new()
        {
            Id = Guid.NewGuid(), CompanyId = scope.Security.CompanyId, RegisteredDeviceId = scope.DeviceId,
            DeviceId = scope.Security.DeviceId, DeviceAssignmentId = assignmentId,
            UserId = scope.Security.UserId, BranchId = scope.Security.BranchId,
            ProofKeyVersion = 1, ProofKeyThumbprint = scope.Thumbprint,
            JtiHash = RandomNumberGenerator.GetBytes(32), HtuHash = RandomNumberGenerator.GetBytes(32),
            HttpMethod = "POST", NonceRecordId = nonceId, IssuedAt = firstSeenAt,
            FirstSeenAt = firstSeenAt, ExpiresAt = expiresAt, AttemptCorrelationId = Guid.NewGuid()
        };

    private static DateTimeOffset Normalize(DateTimeOffset value)
    {
        var ticks = value.UtcDateTime.Ticks;
        return new DateTimeOffset(new DateTime(
            ticks - ticks % TimeSpan.TicksPerMicrosecond, DateTimeKind.Utc));
    }

    private static async Task<TestScope> SeedAsync(TransportErpDbContext db, string suffix)
    {
        var now = DateTimeOffset.UtcNow;
        var currency = new Currency
        {
            Id = Guid.NewGuid(), Code = await PostgreSqlTestCurrencyCodeAllocator.NextAsync(db),
            NameAr = "عملة Stage4", MinorUnit = 2, IsBase = true, CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var company = new Company
        {
            Id = Guid.NewGuid(), Code = $"R-{suffix}-{Guid.NewGuid():N}"[..18], LegalNameAr = "شركة Stage4",
            BaseCurrencyId = currency.Id, DefaultCalendarId = Guid.NewGuid(), Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var branch = new Branch
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, Code = "MAIN", NameAr = "الفرع الرئيسي",
            Timezone = "Asia/Aden", Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var user = new User
        {
            Id = Guid.NewGuid(), UserName = $"runtime-{Guid.NewGuid():N}",
            NormalizedUserName = $"RUNTIME-{Guid.NewGuid():N}", DisplayName = "مستخدم Runtime",
            PasswordHash = "test-only", SecurityStamp = Guid.NewGuid().ToString("N"), AuthVersion = 1,
            Status = "ACTIVE", CompanyId = company.Id, BranchId = branch.Id,
            CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var deviceIdText = $"runtime-{suffix}-{Guid.NewGuid():N}";
        var thumbprint = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
            .TrimEnd('=').Replace('+', '-').Replace('/', '_');
        var device = new RegisteredDevice
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, DeviceId = deviceIdText, DisplayName = "جهاز Runtime",
            Platform = "TEST", AppVersion = "1.0", RegistrationRequestId = "req-" + Guid.NewGuid().ToString("N"),
            CredentialHash = new string('a', 64), CredentialVersion = 1, Status = "ACTIVE",
            RegisteredByUserId = user.Id, ApprovedByUserId = user.Id, ApprovedAt = now, LastSeenAt = now,
            ProofPublicJwkCanonicalJson = "{\"crv\":\"P-256\",\"kty\":\"EC\",\"x\":\"x\",\"y\":\"y\"}",
            ProofKeyThumbprint = thumbprint, ProofKeyVersion = 1, ProofKeyChangedAt = now,
            ProofKeyChangedByUserId = user.Id, CreatedAt = now, UpdatedAt = now,
            RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        var assignment = new RegisteredDeviceAssignment
        {
            Id = Guid.NewGuid(), RegisteredDeviceId = device.Id, UserId = user.Id,
            CompanyId = company.Id, BranchId = branch.Id, Status = "ACTIVE", AssignedByUserId = user.Id,
            AssignedAt = now, CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
        };
        db.AddRange(currency, company, branch, user, device, assignment);
        await db.SaveChangesAsync();
        return new TestScope(device.Id, thumbprint,
            new SyncProofSecurityContext(user.Id, company.Id, branch.Id, device.Id, deviceIdText));
    }

    private sealed record TestScope(Guid DeviceId, string Thumbprint, SyncProofSecurityContext Security);

    private sealed class AcceptedRequestAuthenticator(AcceptedSyncHttpRequest accepted)
        : ISyncPopHttpRequestAuthenticator
    {
        public Task<SyncHttpAuthenticationResult> AuthenticateAsync(
            HttpContext http,
            string canonicalPath,
            TryReadSyncRequestDeviceId? tryReadBodyDeviceId,
            CancellationToken cancellationToken)
            => Task.FromResult(new SyncHttpAuthenticationResult(accepted, null));
    }

    private sealed class DenyPermissionResolver : IEffectivePermissionResolver
    {
        public Task<bool> HasPermissionAsync(Guid userId, Guid companyId, Guid? branchId,
            string permissionCode, CancellationToken cancellationToken = default)
            => Task.FromResult(false);
    }

    private sealed class AllowPermissionResolver : IEffectivePermissionResolver
    {
        public Task<bool> HasPermissionAsync(Guid userId, Guid companyId, Guid? branchId,
            string permissionCode, CancellationToken cancellationToken = default)
            => Task.FromResult(true);
    }

    private sealed class PendingThenRecoveredExecutor(Guid resultEntityId) : ISyncActionExecutor
    {
        private bool _committed;
        public int EffectCount { get; private set; }

        public Task<SyncActionExecutionOutcome> ExecuteAsync(
            SyncOperationExecutionClaim claim,
            CancellationToken cancellationToken = default)
        {
            if (!_committed)
            {
                _committed = true;
                EffectCount++;
                return Task.FromResult<SyncActionExecutionOutcome>(
                    new SyncActionExecutionOutcome.CompletionPending());
            }
            return Task.FromResult<SyncActionExecutionOutcome>(
                new SyncActionExecutionOutcome.Succeeded(resultEntityId, 1));
        }
    }

    private sealed class PermanentlyPendingExecutor : ISyncActionExecutor
    {
        public int AttemptCount { get; private set; }

        public Task<SyncActionExecutionOutcome> ExecuteAsync(
            SyncOperationExecutionClaim claim,
            CancellationToken cancellationToken = default)
        {
            AttemptCount++;
            return Task.FromResult<SyncActionExecutionOutcome>(
                new SyncActionExecutionOutcome.CompletionPending());
        }
    }

    private sealed class ThrowingExecutor : ISyncActionExecutor
    {
        public Task<SyncActionExecutionOutcome> ExecuteAsync(
            SyncOperationExecutionClaim claim,
            CancellationToken cancellationToken = default)
            => throw new InvalidOperationException("deterministic injected executor failure");
    }
}
