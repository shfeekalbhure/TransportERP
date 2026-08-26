using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TransportERP.Application.Sync;
using TransportERP.Contracts.Geo;
using TransportERP.Contracts.Waybills;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

public sealed class Stage4SyncBusinessExecutorIntegrationTests
{
    [Theory]
    [InlineData("permission", "SCOPE_DENIED")]
    [InlineData("suspended", "DEVICE_NOT_REGISTERED")]
    [InlineData("assignment", "DEVICE_NOT_REGISTERED")]
    [InlineData("assignment-scope", "DEVICE_NOT_REGISTERED")]
    [InlineData("credential", "DEVICE_NOT_REGISTERED")]
    [InlineData("proof-key-rotation-before-execute", "DEVICE_NOT_REGISTERED")]
    [InlineData("expired", "DEVICE_NOT_REGISTERED")]
    [InlineData("inactive", "DEVICE_NOT_REGISTERED")]
    public async Task Mutable_security_state_is_rechecked_before_business_effect(
        string mutation,
        string expectedCode)
    {
        await using var db = CreateDb();
        var fixture = await SeedDeviceAsync(db);
        var permission = new FakePermissionResolver { Allowed = mutation != "permission" };
        var adapters = new FakeBusinessAdapters();
        var audit = new CapturingAuditSink();
        var executor = Executor(db, permission, adapters, audit);
        // The execution claim is immutable provenance captured before mutable
        // device state changes (credential/proof rotation included).
        var claim = Claim(fixture);
        switch (mutation)
        {
            case "suspended": fixture.Device.Status = "SUSPENDED"; break;
            case "assignment": fixture.Assignment.Status = "REVOKED"; break;
            case "assignment-scope":
                db.RegisteredDeviceAssignments.Remove(fixture.Assignment);
                await db.SaveChangesAsync();
                var now = DateTimeOffset.UtcNow;
                db.RegisteredDeviceAssignments.Add(new RegisteredDeviceAssignment
                {
                    Id = Guid.NewGuid(), RegisteredDeviceId = fixture.Device.Id,
                    UserId = fixture.UserId, CompanyId = fixture.CompanyId, BranchId = Guid.NewGuid(),
                    Status = "ACTIVE", AssignedByUserId = fixture.UserId, AssignedAt = now,
                    CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
                });
                break;
            case "credential": fixture.Device.CredentialVersion++; break;
            case "proof-key-rotation-before-execute": fixture.Device.ProofKeyVersion++; break;
            case "expired": fixture.Device.ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(-1); break;
            case "inactive": fixture.Device.LastSeenAt = DateTimeOffset.UtcNow.AddDays(-91); break;
        }
        await db.SaveChangesAsync();

        var result = await executor.ExecuteAsync(claim);

        Assert.Equal(expectedCode, Assert.IsType<SyncActionExecutionOutcome.Failed>(result).ErrorCode);
        Assert.Equal(0, adapters.EffectCount);
        Assert.Single(audit.Records);
    }

    [Fact]
    public async Task Successful_and_nullable_result_mapping_are_catalog_driven_and_unsupported_action_is_closed()
    {
        await using var db = CreateDb();
        var fixture = await SeedDeviceAsync(db);
        var adapters = new FakeBusinessAdapters();
        var audit = new CapturingAuditSink();
        var permission = new FakePermissionResolver { Allowed = true };
        var executor = Executor(db, permission, adapters, audit);

        var successfulClaim = Claim(fixture);
        var success = Assert.IsType<SyncActionExecutionOutcome.Succeeded>(
            await executor.ExecuteAsync(successfulClaim));
        Assert.Equal(successfulClaim.OperationCorrelationId, adapters.LastOperationContextCorrelationId!.Value);
        Assert.Equal(successfulClaim.OperationCorrelationId, adapters.LastOperationCorrelationId!.Value);
        Assert.Equal(successfulClaim.UserId, permission.LastUserId!.Value);
        Assert.Equal(successfulClaim.CompanyId, permission.LastCompanyId!.Value);
        Assert.Equal(successfulClaim.BranchId, permission.LastBranchId!.Value);
        Assert.Equal(SyncActionCatalog.Definitions.Single(x =>
            x.ActionCode == SyncActionCode.CreateOperationalParty).RequiredPermission,
            permission.LastPermissionCode);
        var nullableVersion = Assert.IsType<SyncActionExecutionOutcome.Succeeded>(
            await executor.ExecuteAsync(Claim(fixture, SyncActionCode.LoadAllocatedQuantity)));
        var unsupported = Assert.IsType<SyncActionExecutionOutcome.Failed>(
            await executor.ExecuteAsync(Claim(fixture, SyncActionCode.CreateJournalEntry)));

        Assert.NotEqual(Guid.Empty, success.ResultEntityId);
        Assert.Equal(1L, success.ResultVersion!.Value);
        Assert.Null(nullableVersion.ResultVersion);
        Assert.Equal("ACTION_RUNTIME_UNAVAILABLE", unsupported.ErrorCode);
        Assert.Equal(2, adapters.EffectCount);
    }

    [Fact]
    public async Task Replayed_business_idempotency_key_returns_prior_result_without_second_effect()
    {
        await using var db = CreateDb();
        var fixture = await SeedDeviceAsync(db);
        var adapters = new FakeBusinessAdapters();
        var executor = Executor(db, new FakePermissionResolver { Allowed = true },
            adapters, new CapturingAuditSink());
        var clientOperationId = "same-business-operation";

        var first = Assert.IsType<SyncActionExecutionOutcome.Succeeded>(await executor.ExecuteAsync(
            Claim(fixture, clientOperationId: clientOperationId)));
        var replay = Assert.IsType<SyncActionExecutionOutcome.Succeeded>(await executor.ExecuteAsync(
            Claim(fixture, clientOperationId: clientOperationId)));

        Assert.Equal(first, replay);
        Assert.Equal(1, adapters.EffectCount);
    }

    [Fact]
    public async Task Committed_effect_with_audit_failure_remains_pending_and_recovery_does_not_repeat_effect()
    {
        await using var db = CreateDb();
        var fixture = await SeedDeviceAsync(db);
        var adapters = new FakeBusinessAdapters();
        var audit = new FailOnceAuditSink();
        var executor = Executor(db, new FakePermissionResolver { Allowed = true }, adapters, audit);
        var claim = Claim(fixture, clientOperationId: "committed-before-audit");

        var interrupted = await executor.ExecuteAsync(claim);
        var recovered = await executor.ExecuteAsync(claim with
        {
            ClaimToken = Guid.NewGuid(),
            RecoveredStaleClaim = true,
            AttemptStartedAt = DateTimeOffset.UtcNow,
            LeaseExpiresAt = DateTimeOffset.UtcNow.AddMinutes(2)
        });

        Assert.IsType<SyncActionExecutionOutcome.CompletionPending>(interrupted);
        Assert.IsType<SyncActionExecutionOutcome.Succeeded>(recovered);
        Assert.Equal(1, adapters.EffectCount);
        Assert.Single(audit.Records);
    }

    [Fact]
    public async Task Unclassified_deterministic_adapter_failure_is_terminal_not_completion_pending()
    {
        await using var db = CreateDb();
        var fixture = await SeedDeviceAsync(db);
        var adapters = new FakeBusinessAdapters
        {
            Failure = new InvalidOperationException("deterministic injected defect")
        };
        var executor = Executor(db, new FakePermissionResolver { Allowed = true },
            adapters, new CapturingAuditSink());

        var failed = Assert.IsType<SyncActionExecutionOutcome.Failed>(
            await executor.ExecuteAsync(Claim(fixture)));

        Assert.Equal("ACTION_EXECUTION_FAILED", failed.ErrorCode);
        Assert.Equal(0, adapters.EffectCount);
    }

    [Fact]
    public async Task Commit_outcome_timeout_is_classified_for_bounded_completion_recovery()
    {
        await using var db = CreateDb();
        var fixture = await SeedDeviceAsync(db);
        var adapters = new FakeBusinessAdapters
        {
            Failure = new TimeoutException("commit acknowledgement was not observed")
        };
        var executor = Executor(db, new FakePermissionResolver { Allowed = true },
            adapters, new CapturingAuditSink());

        Assert.IsType<SyncActionExecutionOutcome.CompletionPending>(
            await executor.ExecuteAsync(Claim(fixture)));
        Assert.Equal(0, adapters.EffectCount);
    }

    [Fact]
    public async Task Audit_sink_persists_metadata_only_with_operation_correlation()
    {
        await using var db = CreateDb();
        var operationCorrelationId = Guid.NewGuid();
        var sink = new SyncBusinessDispatchAuditSink(new AuditEventService(db));

        await sink.WriteAsync(new SyncBusinessDispatchAuditRecord(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), operationCorrelationId,
            "client-operation", "CreateWaybillDraft", null, null, "REJECTED", null, null,
            "SCOPE_DENIED"), CancellationToken.None);

        var persisted = Assert.Single(await db.AuditEvents.ToListAsync());
        Assert.Equal(operationCorrelationId, persisted.OperationCorrelationId);
        Assert.Null(persisted.BeforeJson);
        Assert.Null(persisted.AfterJson);
        Assert.DoesNotContain("PayloadJson", persisted.Reason ?? string.Empty, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("proof", persisted.Reason ?? string.Empty, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("credential", persisted.Reason ?? string.Empty, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("nonce", persisted.Reason ?? string.Empty, StringComparison.OrdinalIgnoreCase);
    }

    private static TransportErpDbContext CreateDb()
        => new(new DbContextOptionsBuilder<TransportErpDbContext>()
            .UseInMemoryDatabase("sync-executor-" + Guid.NewGuid().ToString("N"))
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options);

    private static SyncBusinessActionExecutor Executor(
        TransportErpDbContext db,
        IEffectivePermissionResolver permissions,
        FakeBusinessAdapters adapters,
        ISyncBusinessDispatchAuditSink audit)
        => new(db, permissions, new SyncBusinessDispatcher(adapters, adapters, adapters, audit), audit);

    private static async Task<DeviceFixture> SeedDeviceAsync(TransportErpDbContext db)
    {
        var now = DateTimeOffset.UtcNow;
        var fixture = new DeviceFixture(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            new RegisteredDevice
            {
                Id = Guid.NewGuid(), CompanyId = Guid.Empty, DeviceId = "device-" + Guid.NewGuid().ToString("N"),
                DisplayName = "device", Platform = "TEST", AppVersion = "1",
                RegistrationRequestId = "request-" + Guid.NewGuid().ToString("N"),
                CredentialHash = new string('a', 64), CredentialVersion = 3, Status = "ACTIVE",
                RegisteredByUserId = Guid.NewGuid(), ApprovedByUserId = Guid.NewGuid(), ApprovedAt = now,
                LastSeenAt = now, ProofPublicJwkCanonicalJson = "{}",
                ProofKeyThumbprint = new string('t', 43), ProofKeyVersion = 4,
                ProofKeyChangedAt = now, ProofKeyChangedByUserId = Guid.NewGuid(),
                CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
            },
            new RegisteredDeviceAssignment());
        fixture.Device.CompanyId = fixture.CompanyId;
        fixture.Assignment.Id = Guid.NewGuid();
        fixture.Assignment.RegisteredDeviceId = fixture.Device.Id;
        fixture.Assignment.UserId = fixture.UserId;
        fixture.Assignment.CompanyId = fixture.CompanyId;
        fixture.Assignment.BranchId = fixture.BranchId;
        fixture.Assignment.Status = "ACTIVE";
        fixture.Assignment.AssignedByUserId = fixture.UserId;
        fixture.Assignment.AssignedAt = now;
        fixture.Assignment.CreatedAt = now;
        fixture.Assignment.UpdatedAt = now;
        fixture.Assignment.RowVersion = Guid.NewGuid().ToByteArray();
        db.AddRange(fixture.Device, fixture.Assignment);
        await db.SaveChangesAsync();
        return fixture;
    }

    private static SyncOperationExecutionClaim Claim(
        DeviceFixture fixture,
        SyncActionCode action = SyncActionCode.CreateOperationalParty,
        string? clientOperationId = null)
    {
        var definition = SyncActionCatalog.Definitions.Single(x => x.ActionCode == action);
        var operationId = clientOperationId ?? "executor-" + Guid.NewGuid().ToString("N");
        Guid? entityId = definition.EntityId == SyncValueRequirement.Required ? Guid.NewGuid() : null;
        long? baseVersion = definition.BaseVersion == SyncValueRequirement.Required ? 1L : null;
        var payload = action switch
        {
            SyncActionCode.CreateOperationalParty => JsonSerializer.Serialize(new OperationalPartyCreateRequest(
                "party", "700000000", null, null,
                new GeoAddressSnapshot(null, null, null, null, "address"), operationId)),
            SyncActionCode.LoadAllocatedQuantity => JsonSerializer.Serialize(new SyncLoadAllocatedQuantityPayload(
                Guid.NewGuid(), new LoadManifestLineRequest(1m, DateTimeOffset.UtcNow, true, operationId))),
            _ => "{}"
        };
        return new SyncOperationExecutionClaim(
            Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddMinutes(2), false,
            fixture.CompanyId, fixture.BranchId, fixture.UserId, fixture.Device.Id,
            fixture.Device.CredentialVersion, fixture.Device.ProofKeyVersion!.Value, fixture.Device.DeviceId,
            "sync-v1", definition.ActionCodeValue, definition.OperationTypeValue, definition.EntityTypeValue,
            entityId, baseVersion, payload, "hash", operationId, Guid.NewGuid(), 0);
    }

    private sealed record DeviceFixture(
        Guid CompanyId,
        Guid BranchId,
        Guid UserId,
        RegisteredDevice Device,
        RegisteredDeviceAssignment Assignment);

    private sealed class FakePermissionResolver : IEffectivePermissionResolver
    {
        public bool Allowed { get; init; }
        public Guid? LastUserId { get; private set; }
        public Guid? LastCompanyId { get; private set; }
        public Guid? LastBranchId { get; private set; }
        public string? LastPermissionCode { get; private set; }
        public Task<bool> HasPermissionAsync(Guid userId, Guid companyId, Guid? branchId,
            string permissionCode, CancellationToken cancellationToken = default)
        {
            LastUserId = userId;
            LastCompanyId = companyId;
            LastBranchId = branchId;
            LastPermissionCode = permissionCode;
            return Task.FromResult(Allowed);
        }
    }

    private sealed class CapturingAuditSink : ISyncBusinessDispatchAuditSink
    {
        public List<SyncBusinessDispatchAuditRecord> Records { get; } = [];
        public Task WriteAsync(SyncBusinessDispatchAuditRecord record, CancellationToken cancellationToken)
        {
            Records.Add(record);
            return Task.CompletedTask;
        }
    }

    private sealed class FailOnceAuditSink : ISyncBusinessDispatchAuditSink
    {
        private bool _failed;
        public List<SyncBusinessDispatchAuditRecord> Records { get; } = [];
        public Task WriteAsync(SyncBusinessDispatchAuditRecord record, CancellationToken cancellationToken)
        {
            if (!_failed)
            {
                _failed = true;
                throw new InvalidOperationException("injected audit failure");
            }
            Records.Add(record);
            return Task.CompletedTask;
        }
    }

    private sealed class FakeBusinessAdapters :
        ISyncWaybillBusinessAdapter,
        ISyncFinanceBusinessAdapter,
        ISyncShippingBusinessAdapter
    {
        private readonly Dictionary<string, SyncBusinessActionResult> _results = [];
        public int EffectCount { get; private set; }
        public Guid? LastOperationCorrelationId { get; private set; }
        public Guid? LastOperationContextCorrelationId { get; private set; }
        public Exception? Failure { get; init; }

        public Task<SyncBusinessActionResult> CreateDraftAsync(SyncBusinessExecutionContext context,
            CreateWaybillDraftRequest request, CancellationToken cancellationToken) => Execute(context, true);
        public Task<SyncBusinessActionResult> UpdateDraftAsync(SyncBusinessExecutionContext context,
            Guid waybillId, UpdateWaybillDraftRequest request, CancellationToken cancellationToken) => Execute(context, true);
        public Task<SyncBusinessActionResult> CreateOperationalPartyAsync(SyncBusinessExecutionContext context,
            OperationalPartyCreateRequest request, CancellationToken cancellationToken) => Execute(context, true);
        public Task<SyncBusinessActionResult> RecordCollectionAsync(SyncBusinessExecutionContext context,
            Guid waybillId, RecordCollectionRequest request, CancellationToken cancellationToken) => Execute(context, true);
        public Task<SyncBusinessActionResult> LoadAllocatedQuantityAsync(SyncBusinessExecutionContext context,
            Guid manifestLineId, SyncLoadAllocatedQuantityPayload payload, CancellationToken cancellationToken)
            => Execute(context, false);

        private Task<SyncBusinessActionResult> Execute(SyncBusinessExecutionContext context, bool versioned)
        {
            if (Failure is not null)
                throw Failure;
            LastOperationCorrelationId = context.OperationCorrelationId;
            LastOperationContextCorrelationId = context.Operation.CorrelationId;
            if (!_results.TryGetValue(context.ClientOperationId, out var result))
            {
                result = new SyncBusinessActionResult(Guid.NewGuid(), versioned ? 1L : null);
                _results.Add(context.ClientOperationId, result);
                EffectCount++;
            }
            return Task.FromResult(result);
        }
    }
}
