using System.Text.Json;
using TransportERP.Application.Sync;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Geo;
using TransportERP.Contracts.Waybills;
using TransportERP.Application.Waybills;

namespace TransportERP.Tests;

public sealed class Stage4SyncBusinessDispatcherTests
{
    public static IEnumerable<object[]> AllActions()
        => SyncActionCatalog.Definitions.Select(x => new object[]
        {
            x.ActionCodeValue,
            x.DispatcherSupport == SyncActionDispatcherSupport.Supported
        });

    [Theory]
    [MemberData(nameof(AllActions))]
    public async Task Every_governed_action_has_an_explicit_typed_dispatch_decision(
        string actionCode,
        bool supported)
    {
        var adapters = new FakeBusinessAdapters();
        var dispatcher = CreateDispatcher(adapters);
        var definition = Assert.Single(SyncActionCatalog.Definitions, x => x.ActionCodeValue == actionCode);
        var actor = Actor(definition.RequiredPermission);
        var command = Command(actor, definition);

        var result = await dispatcher.DispatchAsync(actor, command);

        Assert.Equal(supported, result.IsSuccess);
        Assert.Equal(supported ? null : "ACTION_RUNTIME_UNAVAILABLE", result.ErrorCode);
        Assert.Equal(supported ? 1 : 0, adapters.EffectCount);
        Assert.Equal(1, adapters.AuditCount);
        if (supported)
        {
            Assert.NotNull(result.ResultEntityId);
            Assert.Equal(definition.ResultVersionRequired ? 1L : null, result.ResultVersion);
            Assert.Equal(command.ClientOperationId, adapters.LastClientOperationId);
            Assert.Equal(actor.RegisteredDeviceId, adapters.LastRegisteredDeviceId);
        }
    }

    [Theory]
    [InlineData("CompanyId")]
    [InlineData("BranchId")]
    [InlineData("UserId")]
    [InlineData("RegisteredDeviceId")]
    public async Task Scope_and_device_mismatch_fail_closed_before_business_execution(string field)
    {
        var definition = Definition(SyncActionCode.CreateWaybillDraft);
        var actor = Actor(definition.RequiredPermission);
        var command = Command(actor, definition);
        command = field switch
        {
            "CompanyId" => command with { CompanyId = Guid.NewGuid() },
            "BranchId" => command with { BranchId = Guid.NewGuid() },
            "UserId" => command with { UserId = Guid.NewGuid() },
            "RegisteredDeviceId" => command with { RegisteredDeviceId = Guid.NewGuid() },
            _ => command
        };
        var adapters = new FakeBusinessAdapters();

        var result = await CreateDispatcher(adapters).DispatchAsync(actor, command);

        Assert.Equal("SCOPE_DENIED", result.ErrorCode);
        Assert.Equal(0, adapters.EffectCount);
    }

    [Theory]
    [MemberData(nameof(AllActions))]
    public async Task Every_action_missing_its_permission_fails_closed_before_payload_or_availability_disclosure(
        string actionCode,
        bool _)
    {
        var definition = Assert.Single(SyncActionCatalog.Definitions, x => x.ActionCodeValue == actionCode);
        var actor = Actor();
        var adapters = new FakeBusinessAdapters();

        var result = await CreateDispatcher(adapters).DispatchAsync(actor, Command(actor, definition));

        Assert.Equal("SCOPE_DENIED", result.ErrorCode);
        Assert.Equal(0, adapters.EffectCount);
    }

    [Fact]
    public async Task Entity_base_version_and_embedded_operation_id_are_bound_to_the_typed_payload()
    {
        var definition = Definition(SyncActionCode.UpdateWaybillDraft);
        var actor = Actor(definition.RequiredPermission);
        var adapters = new FakeBusinessAdapters();
        var command = Command(actor, definition);
        var payload = JsonSerializer.Deserialize<UpdateWaybillDraftRequest>(command.PayloadJson)!;

        var versionMismatch = await CreateDispatcher(adapters).DispatchAsync(actor, command with
        {
            PayloadJson = JsonSerializer.Serialize(payload with { ExpectedVersion = command.BaseVersion!.Value + 1 })
        });
        var operationMismatch = await CreateDispatcher(adapters).DispatchAsync(actor, command with
        {
            PayloadJson = JsonSerializer.Serialize(payload with { ClientOperationId = "different-operation" })
        });
        var missingEntity = await CreateDispatcher(adapters).DispatchAsync(actor, command with { EntityId = null });

        Assert.Equal("ACTION_CONTRACT_MISMATCH", versionMismatch.ErrorCode);
        Assert.Equal("IDEMPOTENCY_CONFLICT", operationMismatch.ErrorCode);
        Assert.Equal("ACTION_CONTRACT_MISMATCH", missingEntity.ErrorCode);
        Assert.Equal(0, adapters.EffectCount);
    }

    [Fact]
    public async Task Replaying_the_same_business_idempotency_key_returns_the_same_result_without_a_second_effect()
    {
        var definition = Definition(SyncActionCode.RecordCollection);
        var actor = Actor(definition.RequiredPermission);
        var command = Command(actor, definition);
        var adapters = new FakeBusinessAdapters();
        var dispatcher = CreateDispatcher(adapters);

        var first = await dispatcher.DispatchAsync(actor, command);
        var replay = await dispatcher.DispatchAsync(actor, command);

        Assert.True(first.IsSuccess);
        Assert.Equal(first.ResultEntityId, replay.ResultEntityId);
        Assert.Equal(first.ResultVersion, replay.ResultVersion);
        Assert.Equal(1, adapters.EffectCount);
    }

    [Fact]
    public async Task Business_idempotency_is_device_scoped_while_protocol_and_audit_keep_the_original_id()
    {
        var definition = Definition(SyncActionCode.CreateOperationalParty);
        var firstDevice = Actor(definition.RequiredPermission);
        var secondDevice = firstDevice with { RegisteredDeviceId = Guid.NewGuid() };
        var operationId = "shared-client-operation";
        var firstCommand = Command(firstDevice, definition, operationId);
        var secondCommand = Command(secondDevice, definition, operationId);
        var adapters = new FakeBusinessAdapters();
        var dispatcher = CreateDispatcher(adapters);

        var first = await dispatcher.DispatchAsync(firstDevice, firstCommand);
        var firstReplay = await dispatcher.DispatchAsync(firstDevice, firstCommand);
        var second = await dispatcher.DispatchAsync(secondDevice, secondCommand);

        Assert.True(first.IsSuccess);
        Assert.Equal(first.ResultEntityId, firstReplay.ResultEntityId);
        Assert.True(second.IsSuccess);
        Assert.NotEqual(first.ResultEntityId, second.ResultEntityId);
        Assert.Equal(2, adapters.EffectCount);
        Assert.Equal(2, adapters.BusinessIdempotencyKeys.Count);
        Assert.All(adapters.BusinessIdempotencyKeys, key =>
        {
            Assert.StartsWith("sync-device-v1:", key, StringComparison.Ordinal);
            Assert.Equal(79, key.Length);
            Assert.DoesNotContain(operationId, key, StringComparison.Ordinal);
        });
        Assert.Equal(3, adapters.AuditRecords.Count);
        Assert.All(adapters.AuditRecords,
            record => Assert.Equal(operationId, record.ClientOperationId));
    }

    [Fact]
    public void Device_scoped_key_is_deterministic_and_changes_for_every_scope_component()
    {
        var company = Guid.NewGuid();
        var branch = Guid.NewGuid();
        var device = Guid.NewGuid();
        const string operation = "operation-X";
        var key = SyncBusinessIdempotencyKey.Create(company, branch, device, operation);

        Assert.Equal(key, SyncBusinessIdempotencyKey.Create(company, branch, device, $" {operation} "));
        Assert.NotEqual(key, SyncBusinessIdempotencyKey.Create(Guid.NewGuid(), branch, device, operation));
        Assert.NotEqual(key, SyncBusinessIdempotencyKey.Create(company, Guid.NewGuid(), device, operation));
        Assert.NotEqual(key, SyncBusinessIdempotencyKey.Create(company, branch, Guid.NewGuid(), operation));
        Assert.NotEqual(key, SyncBusinessIdempotencyKey.Create(company, branch, device, operation + "-changed"));
    }

    [Fact]
    public async Task Domain_concurrency_is_returned_as_typed_conflict_not_rejection()
    {
        var definition = Definition(SyncActionCode.UpdateWaybillDraft);
        var actor = Actor(definition.RequiredPermission);
        var adapters = new FakeBusinessAdapters { ErrorCodeToThrow = "CONCURRENCY_CONFLICT" };

        var result = await CreateDispatcher(adapters).DispatchAsync(actor, Command(actor, definition));

        Assert.True(result.IsConflict);
        Assert.Equal("CONFLICT", result.Status);
        Assert.Equal("CONCURRENCY_CONFLICT", result.ErrorCode);
    }

    [Fact]
    public async Task Strict_payload_reader_rejects_unknown_fields_without_invoking_an_adapter()
    {
        var definition = Definition(SyncActionCode.CreateOperationalParty);
        var actor = Actor(definition.RequiredPermission);
        var adapters = new FakeBusinessAdapters();
        var command = Command(actor, definition) with
        {
            PayloadJson = "{\"Name\":\"party\",\"Mobile\":\"700000000\",\"Address\":{},\"ClientOperationId\":\"op-1\",\"Unexpected\":true}",
            ClientOperationId = "op-1"
        };

        var result = await CreateDispatcher(adapters).DispatchAsync(actor, command);

        Assert.Equal("PAYLOAD_INVALID", result.ErrorCode);
        Assert.Equal(0, adapters.EffectCount);
    }

    private static SyncBusinessDispatcher CreateDispatcher(FakeBusinessAdapters adapters)
        => new(adapters, adapters, adapters, adapters);

    private static SyncActionDefinition Definition(SyncActionCode actionCode)
        => Assert.Single(SyncActionCatalog.Definitions, x => x.ActionCode == actionCode);

    private static SyncBusinessActorContext Actor(params string[] permissions)
        => new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            new HashSet<string>(permissions, StringComparer.Ordinal), Guid.NewGuid());

    private static SyncBusinessDispatchCommand Command(
        SyncBusinessActorContext actor,
        SyncActionDefinition definition,
        string? suppliedOperationId = null)
    {
        var operationId = suppliedOperationId ?? $"dispatch-{Guid.NewGuid():N}";
        Guid? entityId = definition.EntityId == SyncValueRequirement.Required ? Guid.NewGuid() : null;
        long? baseVersion = definition.BaseVersion == SyncValueRequirement.Required ? 7L : null;
        var payload = definition.ActionCode switch
        {
            SyncActionCode.CreateWaybillDraft => JsonSerializer.Serialize(new CreateWaybillDraftRequest(
                actor.BranchId, DateTimeOffset.UtcNow, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                1m, "STANDARD", "NORMAL", operationId)),
            SyncActionCode.UpdateWaybillDraft => JsonSerializer.Serialize(new UpdateWaybillDraftRequest(
                baseVersion!.Value, DateTimeOffset.UtcNow, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                1m, 100m, 0m, "STANDARD", "NORMAL", [], [], operationId)),
            SyncActionCode.CreateOperationalParty => JsonSerializer.Serialize(new OperationalPartyCreateRequest(
                "party", "700000000", null, null,
                new GeoAddressSnapshot(null, null, null, null, "address"), operationId)),
            SyncActionCode.RecordCollection => JsonSerializer.Serialize(new RecordCollectionRequest(
                "SENDER", null, "CASH", new MoneyAmount(Guid.NewGuid(), 10m), 1m,
                "USER", actor.UserId, DateTimeOffset.UtcNow, operationId)),
            SyncActionCode.LoadAllocatedQuantity => JsonSerializer.Serialize(new SyncLoadAllocatedQuantityPayload(
                Guid.NewGuid(), new LoadManifestLineRequest(1m, DateTimeOffset.UtcNow, true, operationId))),
            _ => "{}"
        };
        return new(actor.CompanyId, actor.BranchId, actor.UserId, actor.RegisteredDeviceId,
            "sync-v1", definition.ActionCodeValue, definition.OperationTypeValue, definition.EntityTypeValue,
            entityId, baseVersion, operationId, Guid.NewGuid(), payload);
    }

    private sealed class FakeBusinessAdapters :
        ISyncWaybillBusinessAdapter,
        ISyncFinanceBusinessAdapter,
        ISyncShippingBusinessAdapter,
        ISyncBusinessDispatchAuditSink
    {
        private readonly Dictionary<(string Action, string OperationId), SyncBusinessActionResult> _results = [];
        public int EffectCount { get; private set; }
        public string? LastClientOperationId { get; private set; }
        public Guid? LastRegisteredDeviceId { get; private set; }
        public int AuditCount { get; private set; }
        public List<string> BusinessIdempotencyKeys { get; } = [];
        public List<SyncBusinessDispatchAuditRecord> AuditRecords { get; } = [];
        public string? ErrorCodeToThrow { get; init; }

        public Task<SyncBusinessActionResult> CreateDraftAsync(
            SyncBusinessExecutionContext context, CreateWaybillDraftRequest request, CancellationToken cancellationToken)
            => Execute("CreateWaybillDraft", context);

        public Task<SyncBusinessActionResult> UpdateDraftAsync(
            SyncBusinessExecutionContext context, Guid waybillId, UpdateWaybillDraftRequest request, CancellationToken cancellationToken)
            => Execute("UpdateWaybillDraft", context);

        public Task<SyncBusinessActionResult> CreateOperationalPartyAsync(
            SyncBusinessExecutionContext context, OperationalPartyCreateRequest request, CancellationToken cancellationToken)
            => Execute("CreateOperationalParty", context);

        public Task<SyncBusinessActionResult> RecordCollectionAsync(
            SyncBusinessExecutionContext context, Guid waybillId, RecordCollectionRequest request, CancellationToken cancellationToken)
            => Execute("RecordCollection", context);

        public Task<SyncBusinessActionResult> LoadAllocatedQuantityAsync(
            SyncBusinessExecutionContext context,
            Guid manifestLineId,
            SyncLoadAllocatedQuantityPayload payload,
            CancellationToken cancellationToken)
            => Execute("LoadAllocatedQuantity", context);

        public Task WriteAsync(
            SyncBusinessDispatchAuditRecord record,
            CancellationToken cancellationToken)
        {
            AuditCount++;
            AuditRecords.Add(record);
            return Task.CompletedTask;
        }

        private Task<SyncBusinessActionResult> Execute(string action, SyncBusinessExecutionContext context)
        {
            if (ErrorCodeToThrow is not null)
                throw new WaybillApplicationException(ErrorCodeToThrow);
            LastClientOperationId = context.ClientOperationId;
            LastRegisteredDeviceId = context.RegisteredDeviceId;
            var key = (action, context.BusinessIdempotencyKey);
            if (!_results.TryGetValue(key, out var result))
            {
                result = new SyncBusinessActionResult(Guid.NewGuid(),
                    action is "LoadAllocatedQuantity" or "RecordCollection" ? null : 1L);
                _results.Add(key, result);
                EffectCount++;
                BusinessIdempotencyKeys.Add(context.BusinessIdempotencyKey);
            }
            return Task.FromResult(result);
        }
    }
}
