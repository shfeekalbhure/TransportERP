using TransportERP.Application.Waybills;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Geo;
using TransportERP.Contracts.Numbering;
using TransportERP.Contracts.Waybills;
using TransportERP.Domain.Waybills;

namespace TransportERP.Tests;

public sealed class P2C01AWaybillFoundationTests
{
    [Fact]
    public void Draft_has_no_official_number_until_approval()
    {
        var draft = NewDraft();

        Assert.Equal(WaybillStatus.Draft, draft.Status);
        Assert.Null(draft.WaybillNo);
        Assert.Equal(1, draft.Version);
    }

    [Fact]
    public void Submit_requires_sender_receiver_and_item()
    {
        var draft = NewDraft();

        var ex = Assert.Throws<WaybillValidationException>(() => draft.SubmitForApproval());

        Assert.Contains("SENDER_REQUIRED", ex.Errors);
        Assert.Contains("RECEIVER_REQUIRED", ex.Errors);
        Assert.Contains("ITEM_REQUIRED", ex.Errors);
        Assert.Equal(WaybillStatus.Draft, draft.Status);
    }

    [Fact]
    public void Duplicate_sender_is_rejected_before_submit()
    {
        var draft = NewDraft();
        var address = NewAddress();
        var sender1 = NewParty(WaybillPartyRole.Sender, "مرسل 1", address);
        var sender2 = NewParty(WaybillPartyRole.Sender, "مرسل 2", address);
        var item = NewItem(1);

        var ex = Assert.Throws<WaybillRuleException>(() => draft.UpdateDraft(
            DateTimeOffset.UtcNow, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1m,
            100m, 0m, "STANDARD", "NORMAL", [sender1, sender2], [item]));

        Assert.Equal("PARTY_ROLE_DUPLICATE", ex.Code);
    }

    [Fact]
    public void Valid_draft_can_submit_and_approve_with_one_official_number()
    {
        var draft = NewDraft();
        var address = NewAddress();
        var sender = NewParty(WaybillPartyRole.Sender, "المرسل", address);
        var receiver = NewParty(WaybillPartyRole.Receiver, "المستلم", address);
        var item = NewItem(1);

        draft.UpdateDraft(DateTimeOffset.UtcNow, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1m,
            100m, 10m, "STANDARD", "NORMAL", [sender, receiver], [item]);
        draft.SubmitForApproval();
        draft.ApplyApproval("WB-00000001");

        Assert.Equal(WaybillStatus.Approved, draft.Status);
        Assert.Equal("WB-00000001", draft.WaybillNo);
        Assert.Equal(90m, draft.NetAmount);
    }

    [Fact]
    public async Task Create_draft_replay_returns_same_waybill_and_audits_once()
    {
        var fixture = new AppFixture();
        var request = new CreateWaybillDraftRequest(
            fixture.Context.BranchId, DateTimeOffset.UtcNow, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            1m, "STANDARD", "NORMAL", "create-001");

        var first = await fixture.Service.CreateDraftAsync(fixture.Context, request);
        var replay = await fixture.Service.CreateDraftAsync(fixture.Context, request);

        Assert.Equal(first.Id, replay.Id);
        Assert.Equal(first.DraftNo, replay.DraftNo);
        Assert.Null(first.WaybillNo);
        Assert.Single(fixture.Audit.Events.Where(x => x.Action == "WaybillDraftCreate"));
    }

    [Fact]
    public async Task Full_A_lifecycle_approves_atomically_and_retry_returns_same_number()
    {
        var fixture = new AppFixture();
        var create = await fixture.Service.CreateDraftAsync(fixture.Context, new CreateWaybillDraftRequest(
            fixture.Context.BranchId, DateTimeOffset.UtcNow, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            1m, "STANDARD", "NORMAL", "create-002"));

        var address = NewAddress();
        var updated = await fixture.Service.UpdateDraftAsync(fixture.Context, create.Id, new UpdateWaybillDraftRequest(
            create.Version, create.WaybillDateTime, create.OriginId, create.DestinationId, create.CurrencyId,
            1m, 150m, 25m, "STANDARD", "NORMAL",
            [
                new WaybillPartyInput("SENDER", null, "المرسل", "777000001", null, null, address),
                new WaybillPartyInput("RECEIVER", null, "المستلم", "777000002", null, null, address)
            ],
            [new WaybillItemInput(null, 1, "GENERAL", "طرود عامة", 2m, 2, 10m, null, null, null, 500m, null, [], null)],
            "update-002"));

        var submitted = await fixture.Service.SubmitAsync(fixture.Context, create.Id,
            new SubmitWaybillRequest(updated.Version, "submit-002"));

        var sequenceId = Guid.NewGuid();
        var approved = await fixture.Service.ApproveAsync(fixture.Context, create.Id,
            new ApproveWaybillRequest(submitted.Version, sequenceId, "approve-002"));
        var retry = await fixture.Service.ApproveAsync(fixture.Context, create.Id,
            new ApproveWaybillRequest(submitted.Version, sequenceId, "approve-002"));

        Assert.Equal("APPROVED", approved.Status);
        Assert.Equal("WB-00000001", approved.WaybillNo);
        Assert.Equal(approved.WaybillNo, retry.WaybillNo);
        Assert.Equal(approved.Id, retry.Id);
        Assert.Equal(1, fixture.Numbering.ReservationCount);
        Assert.Single(fixture.Audit.Events.Where(x => x.Action == "WaybillApprove"));
    }

    [Fact]
    public async Task Stale_update_without_matching_operation_is_concurrency_conflict()
    {
        var fixture = new AppFixture();
        var create = await fixture.Service.CreateDraftAsync(fixture.Context, new CreateWaybillDraftRequest(
            fixture.Context.BranchId, DateTimeOffset.UtcNow, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            1m, "STANDARD", "NORMAL", "create-003"));

        var req = new UpdateWaybillDraftRequest(
            create.Version, create.WaybillDateTime, create.OriginId, create.DestinationId, create.CurrencyId,
            1m, 10m, 0m, "STANDARD", "NORMAL",
            [
                new WaybillPartyInput("SENDER", null, "س", "1", null, null, NewAddress()),
                new WaybillPartyInput("RECEIVER", null, "م", "2", null, null, NewAddress())
            ],
            [new WaybillItemInput(null, 1, "GENERAL", "x", 1m, 1, null, null, null, null, null, null, [], null)],
            "update-003");

        var updated = await fixture.Service.UpdateDraftAsync(fixture.Context, create.Id, req);

        var stale = req with { ClientOperationId = "different-op" };
        var ex = await Assert.ThrowsAsync<WaybillApplicationException>(() =>
            fixture.Service.UpdateDraftAsync(fixture.Context, create.Id, stale));

        Assert.Equal("CONCURRENCY_CONFLICT", ex.Code);
        Assert.Equal(updated.Version, fixture.Repository.VersionOf(create.Id));
    }

    private static WaybillAggregate NewDraft()
        => WaybillAggregate.CreateDraft(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "D-001",
            DateTimeOffset.UtcNow, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1m);

    private static GeoAddressSnapshot NewAddress()
        => new(null, null, null, null, "العنوان");

    private static WaybillPartyValue NewParty(WaybillPartyRole role, string name, GeoAddressSnapshot address)
        => new(role, null, name, "777000000", null, null,
            address.CountryId, address.GovernorateId, address.CityId, address.AreaId, address.AddressLine);

    private static WaybillItemValue NewItem(int line)
        => new(Guid.NewGuid(), line, "GENERAL", "محتويات", 1m, 1, null, null, null, null, null, null, "[]", null);

    private sealed class AppFixture
    {
        public OperationContext Context { get; } = new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        public FakeWaybillRepository Repository { get; } = new();
        public FakeNumbering Numbering { get; } = new();
        public FakeAudit Audit { get; } = new();
        public WaybillApplicationService Service { get; }

        public AppFixture()
        {
            Service = new WaybillApplicationService(
                Repository,
                new FakePartyRepository(),
                Numbering,
                new InlineUnitOfWork(),
                Audit);
        }
    }

    private sealed class FakeWaybillRepository : IWaybillRepository
    {
        private readonly Dictionary<Guid, WaybillAggregate> _byId = [];
        private readonly Dictionary<string, Guid> _createOps = new(StringComparer.Ordinal);
        private readonly Dictionary<Guid, string> _lastOps = [];
        private readonly Dictionary<Guid, long> _versions = [];

        public long VersionOf(Guid id) => _versions[id];

        public Task<WaybillAggregate?> GetAsync(Guid companyId, Guid branchId, Guid waybillId, CancellationToken cancellationToken)
            => Task.FromResult(_byId.TryGetValue(waybillId, out var x) && x.CompanyId == companyId && x.BranchId == branchId ? x : null);

        public Task<WaybillAggregate?> GetByCreateOperationAsync(Guid companyId, Guid branchId, string clientOperationId, CancellationToken cancellationToken)
        {
            if (_createOps.TryGetValue(clientOperationId, out var id) && _byId.TryGetValue(id, out var x) &&
                x.CompanyId == companyId && x.BranchId == branchId)
                return Task.FromResult<WaybillAggregate?>(x);
            return Task.FromResult<WaybillAggregate?>(null);
        }

        public Task<bool> WasLastOperationAsync(Guid companyId, Guid branchId, Guid waybillId, string clientOperationId, CancellationToken cancellationToken)
            => Task.FromResult(_byId.TryGetValue(waybillId, out var x) && x.CompanyId == companyId && x.BranchId == branchId &&
                               _lastOps.TryGetValue(waybillId, out var op) && op == clientOperationId);

        public Task<WaybillAggregate> AddOrGetAsync(WaybillAggregate aggregate, string clientOperationId, CancellationToken cancellationToken)
        {
            if (_createOps.TryGetValue(clientOperationId, out var existingId))
                return Task.FromResult(_byId[existingId]);
            _byId[aggregate.Id] = aggregate;
            _createOps[clientOperationId] = aggregate.Id;
            _lastOps[aggregate.Id] = clientOperationId;
            _versions[aggregate.Id] = aggregate.Version;
            return Task.FromResult(aggregate);
        }

        public Task SaveAsync(WaybillAggregate aggregate, long expectedVersion, string clientOperationId, CancellationToken cancellationToken)
        {
            if (!_versions.TryGetValue(aggregate.Id, out var current) || current != expectedVersion)
                throw new WaybillApplicationException("CONCURRENCY_CONFLICT");
            _versions[aggregate.Id] = aggregate.Version;
            _lastOps[aggregate.Id] = clientOperationId;
            return Task.CompletedTask;
        }

        public Task LinkNumberReservationAsync(Guid companyId, Guid branchId, Guid waybillId, Guid reservationId, CancellationToken cancellationToken)
            => Task.CompletedTask;
    }

    private sealed class FakePartyRepository : IOperationalPartyRepository
    {
        public Task<(IReadOnlyList<OperationalPartyRecord> Items, long Total)> SearchAsync(Guid companyId, Guid branchId, string? query, int skip, int take, CancellationToken cancellationToken)
            => Task.FromResult(((IReadOnlyList<OperationalPartyRecord>)Array.Empty<OperationalPartyRecord>(), 0L));

        public Task<OperationalPartyRecord?> GetByClientOperationAsync(Guid companyId, string clientOperationId, CancellationToken cancellationToken)
            => Task.FromResult<OperationalPartyRecord?>(null);

        public Task<OperationalPartyRecord> CreateAsync(Guid companyId, Guid branchId, string partyNo, OperationalPartyCreateRequest request, CancellationToken cancellationToken)
            => Task.FromResult(new OperationalPartyRecord(Guid.NewGuid(), companyId, branchId, partyNo, request.Name, request.Mobile,
                request.IdentityType, request.IdentityNo, request.Address, "ACTIVE", 1));
    }

    private sealed class InlineUnitOfWork : IWaybillUnitOfWork
    {
        public Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> action, CancellationToken cancellationToken)
            => action(cancellationToken);
    }

    private sealed class FakeAudit : IWaybillAuditSink
    {
        public List<(string Action, string Outcome, Guid EntityId)> Events { get; } = [];

        public Task WriteAsync(OperationContext context, string action, string outcome, string entityType, Guid entityId,
            string? beforeJson, string? afterJson, string? reason, CancellationToken cancellationToken)
        {
            Events.Add((action, outcome, entityId));
            return Task.CompletedTask;
        }
    }

    private sealed class FakeNumbering : INumberReservationService
    {
        private readonly Dictionary<string, NumberReservationDto> _byKey = new(StringComparer.Ordinal);
        public int ReservationCount => _byKey.Count;

        public ValueTask<NumberReservationDto> ReserveAsync(OperationContext context, NumberReservationRequest request, CancellationToken cancellationToken = default)
        {
            if (_byKey.TryGetValue(request.IdempotencyKey, out var existing))
                return ValueTask.FromResult(existing);
            var dto = new NumberReservationDto(Guid.NewGuid(), request.SequenceId, 1, "WB-00000001", NumberReservationStates.Reserved);
            _byKey[request.IdempotencyKey] = dto;
            return ValueTask.FromResult(dto);
        }

        public ValueTask<NumberReservationDto> CommitAsync(OperationContext context, NumberReservationTransitionRequest request, CancellationToken cancellationToken = default)
        {
            var kv = _byKey.Single(x => x.Value.Id == request.ReservationId);
            var committed = kv.Value with { State = NumberReservationStates.Committed };
            _byKey[kv.Key] = committed;
            return ValueTask.FromResult(committed);
        }

        public ValueTask<NumberReservationDto> VoidAsync(OperationContext context, NumberReservationTransitionRequest request, CancellationToken cancellationToken = default)
        {
            var kv = _byKey.Single(x => x.Value.Id == request.ReservationId);
            var voided = kv.Value with { State = NumberReservationStates.Void };
            _byKey[kv.Key] = voided;
            return ValueTask.FromResult(voided);
        }
    }
}
