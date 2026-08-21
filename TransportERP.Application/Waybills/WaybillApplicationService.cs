using System.Text.Json;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Geo;
using TransportERP.Contracts.Numbering;
using TransportERP.Contracts.Waybills;
using TransportERP.Domain.Waybills;

namespace TransportERP.Application.Waybills;

public interface IWaybillRepository
{
    Task<WaybillAggregate?> GetAsync(Guid companyId, Guid branchId, Guid waybillId, CancellationToken cancellationToken);
    Task<WaybillAggregate?> GetByCreateOperationAsync(Guid companyId, Guid branchId, string clientOperationId, CancellationToken cancellationToken);
    Task<bool> WasLastOperationAsync(Guid companyId, Guid branchId, Guid waybillId, string clientOperationId, CancellationToken cancellationToken);
    Task<WaybillAggregate> AddOrGetAsync(WaybillAggregate aggregate, string clientOperationId, CancellationToken cancellationToken);
    Task SaveAsync(WaybillAggregate aggregate, long expectedVersion, string clientOperationId, CancellationToken cancellationToken);
    Task LinkNumberReservationAsync(Guid companyId, Guid branchId, Guid waybillId, Guid reservationId, CancellationToken cancellationToken);
}

public sealed record OperationalPartyRecord(
    Guid Id,
    Guid CompanyId,
    Guid? BranchId,
    string PartyNo,
    string Name,
    string Mobile,
    string? IdentityType,
    string? IdentityNo,
    GeoAddressSnapshot Address,
    string Status,
    long Version);

public interface IOperationalPartyRepository
{
    Task<(IReadOnlyList<OperationalPartyRecord> Items, long Total)> SearchAsync(
        Guid companyId, Guid branchId, string? query, int skip, int take, CancellationToken cancellationToken);
    Task<OperationalPartyRecord?> GetByClientOperationAsync(Guid companyId, string clientOperationId, CancellationToken cancellationToken);
    Task<OperationalPartyRecord> CreateAsync(
        Guid companyId, Guid branchId, string partyNo, OperationalPartyCreateRequest request, CancellationToken cancellationToken);
}

public interface IWaybillUnitOfWork
{
    Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> action, CancellationToken cancellationToken);
}

public interface IWaybillAuditSink
{
    Task WriteAsync(
        OperationContext context,
        string action,
        string outcome,
        string entityType,
        Guid entityId,
        string? beforeJson,
        string? afterJson,
        string? reason,
        CancellationToken cancellationToken);
}

public sealed class WaybillApplicationService(
    IWaybillRepository waybills,
    IOperationalPartyRepository parties,
    INumberReservationService numbering,
    IWaybillUnitOfWork unitOfWork,
    IWaybillAuditSink audit)
{
    public async Task<WaybillResponse> CreateDraftAsync(
        OperationContext context,
        CreateWaybillDraftRequest request,
        CancellationToken cancellationToken = default)
    {
        context.EnsureComplete();
        EnsureBranch(context, request.BranchId);
        RequireClientOperation(request.ClientOperationId);
        var operationId = request.ClientOperationId.Trim();

        var replay = await waybills.GetByCreateOperationAsync(context.CompanyId, context.BranchId, operationId, cancellationToken);
        if (replay is not null)
            return ToResponse(replay, context.CorrelationId);

        var candidate = WaybillAggregate.CreateDraft(
            Guid.NewGuid(), context.CompanyId, context.BranchId, NewDraftNo(), request.WaybillDateTime,
            request.OriginId, request.DestinationId, request.CurrencyId, request.ExchangeRate,
            request.ServiceType ?? "STANDARD", request.Priority ?? "NORMAL");

        var persisted = await waybills.AddOrGetAsync(candidate, operationId, cancellationToken);
        if (persisted.Id == candidate.Id)
        {
            await audit.WriteAsync(context, "WaybillDraftCreate", "SUCCESS", "Waybill", persisted.Id,
                null, Snapshot(persisted), null, cancellationToken);
        }
        return ToResponse(persisted, context.CorrelationId);
    }

    public async Task<WaybillResponse> UpdateDraftAsync(
        OperationContext context,
        Guid waybillId,
        UpdateWaybillDraftRequest request,
        CancellationToken cancellationToken = default)
    {
        context.EnsureComplete();
        RequireClientOperation(request.ClientOperationId);
        var operationId = request.ClientOperationId.Trim();
        var aggregate = await RequireWaybill(context, waybillId, cancellationToken);
        if (await IsReplay(context, aggregate, request.ExpectedVersion, operationId, cancellationToken))
            return ToResponse(aggregate, context.CorrelationId);
        var before = Snapshot(aggregate);

        aggregate.UpdateDraft(
            request.WaybillDateTime, request.OriginId, request.DestinationId, request.CurrencyId,
            request.ExchangeRate, request.FreightTotal, request.DiscountTotal, request.ServiceType,
            request.Priority, request.Parties.Select(ToDomainParty), request.Items.Select(ToDomainItem));

        await waybills.SaveAsync(aggregate, request.ExpectedVersion, operationId, cancellationToken);
        await audit.WriteAsync(context, "WaybillDraftUpdate", "SUCCESS", "Waybill", aggregate.Id,
            before, Snapshot(aggregate), null, cancellationToken);
        return ToResponse(aggregate, context.CorrelationId);
    }

    public async Task<WaybillValidationResponse> ValidateAsync(
        OperationContext context,
        Guid waybillId,
        ValidateWaybillRequest request,
        CancellationToken cancellationToken = default)
    {
        context.EnsureComplete();
        var aggregate = await RequireWaybill(context, waybillId, cancellationToken);
        if (request.ExpectedVersion.HasValue)
            EnsureVersion(aggregate, request.ExpectedVersion.Value);
        var errors = aggregate.ValidateForApproval();
        await audit.WriteAsync(context, "WaybillValidate", errors.Count == 0 ? "SUCCESS" : "REJECTED",
            "Waybill", aggregate.Id, null, JsonSerializer.Serialize(errors), null, cancellationToken);
        return new WaybillValidationResponse(aggregate.Id, errors.Count == 0, errors, aggregate.Version, context.CorrelationId);
    }

    public async Task<WaybillResponse> SubmitAsync(
        OperationContext context,
        Guid waybillId,
        SubmitWaybillRequest request,
        CancellationToken cancellationToken = default)
    {
        context.EnsureComplete();
        RequireClientOperation(request.ClientOperationId);
        var operationId = request.ClientOperationId.Trim();
        var aggregate = await RequireWaybill(context, waybillId, cancellationToken);
        if (await IsReplay(context, aggregate, request.ExpectedVersion, operationId, cancellationToken))
            return ToResponse(aggregate, context.CorrelationId);
        var before = Snapshot(aggregate);
        aggregate.SubmitForApproval();
        await waybills.SaveAsync(aggregate, request.ExpectedVersion, operationId, cancellationToken);
        await audit.WriteAsync(context, "WaybillSubmit", "SUCCESS", "Waybill", aggregate.Id,
            before, Snapshot(aggregate), null, cancellationToken);
        return ToResponse(aggregate, context.CorrelationId);
    }

    public async Task<WaybillResponse> ReturnForCorrectionAsync(
        OperationContext context,
        Guid waybillId,
        ReturnWaybillRequest request,
        CancellationToken cancellationToken = default)
    {
        context.EnsureComplete();
        RequireReason(request.Reason);
        RequireClientOperation(request.ClientOperationId);
        var operationId = request.ClientOperationId.Trim();
        var aggregate = await RequireWaybill(context, waybillId, cancellationToken);
        if (await IsReplay(context, aggregate, request.ExpectedVersion, operationId, cancellationToken))
            return ToResponse(aggregate, context.CorrelationId);
        var before = Snapshot(aggregate);
        aggregate.ReturnForCorrection();
        await waybills.SaveAsync(aggregate, request.ExpectedVersion, operationId, cancellationToken);
        await audit.WriteAsync(context, "WaybillReturnForCorrection", "SUCCESS", "Waybill", aggregate.Id,
            before, Snapshot(aggregate), request.Reason.Trim(), cancellationToken);
        return ToResponse(aggregate, context.CorrelationId);
    }

    public async Task<WaybillResponse> CancelAsync(
        OperationContext context,
        Guid waybillId,
        CancelWaybillRequest request,
        CancellationToken cancellationToken = default)
    {
        context.EnsureComplete();
        RequireReason(request.Reason);
        RequireClientOperation(request.ClientOperationId);
        var operationId = request.ClientOperationId.Trim();
        var aggregate = await RequireWaybill(context, waybillId, cancellationToken);
        if (await IsReplay(context, aggregate, request.ExpectedVersion, operationId, cancellationToken))
            return ToResponse(aggregate, context.CorrelationId);
        var before = Snapshot(aggregate);
        aggregate.Cancel();
        await waybills.SaveAsync(aggregate, request.ExpectedVersion, operationId, cancellationToken);
        await audit.WriteAsync(context, "WaybillCancel", "SUCCESS", "Waybill", aggregate.Id,
            before, Snapshot(aggregate), request.Reason.Trim(), cancellationToken);
        return ToResponse(aggregate, context.CorrelationId);
    }

    public Task<WaybillResponse> ApproveAsync(
        OperationContext context,
        Guid waybillId,
        ApproveWaybillRequest request,
        CancellationToken cancellationToken = default)
    {
        context.EnsureComplete();
        if (request.NumberSequenceId == Guid.Empty)
            throw new WaybillApplicationException("NUMBERING_UNAVAILABLE");
        if (string.IsNullOrWhiteSpace(request.IdempotencyKey))
            throw new WaybillApplicationException("IDEMPOTENCY_KEY_REQUIRED");

        return unitOfWork.ExecuteAsync(async ct =>
        {
            var aggregate = await RequireWaybill(context, waybillId, ct);

            if (aggregate.Status == WaybillStatus.Approved)
            {
                var existing = await numbering.ReserveAsync(context,
                    new NumberReservationRequest(request.NumberSequenceId, request.IdempotencyKey, "WAYBILL_APPROVAL_RETRY"), ct);
                existing.EnsureValid();
                if (existing.State == NumberReservationStates.Reserved)
                    existing = await numbering.CommitAsync(context,
                        new NumberReservationTransitionRequest(existing.Id, request.IdempotencyKey, "WAYBILL_APPROVAL_RETRY"), ct);
                if (!string.Equals(existing.RenderedNumber, aggregate.WaybillNo, StringComparison.Ordinal))
                    throw new WaybillApplicationException("IDEMPOTENCY_CONFLICT");
                return ToResponse(aggregate, context.CorrelationId);
            }

            EnsureVersion(aggregate, request.ExpectedVersion);
            var before = Snapshot(aggregate);
            var reservation = await numbering.ReserveAsync(context,
                new NumberReservationRequest(request.NumberSequenceId, request.IdempotencyKey, "WAYBILL_APPROVAL"), ct);
            reservation.EnsureValid();

            try
            {
                await waybills.LinkNumberReservationAsync(context.CompanyId, context.BranchId, aggregate.Id, reservation.Id, ct);
                aggregate.ApplyApproval(reservation.RenderedNumber);
                await waybills.SaveAsync(aggregate, request.ExpectedVersion, request.IdempotencyKey.Trim(), ct);
                var committed = await numbering.CommitAsync(context,
                    new NumberReservationTransitionRequest(reservation.Id, request.IdempotencyKey, "WAYBILL_APPROVED"), ct);
                if (committed.State != NumberReservationStates.Committed)
                    throw new WaybillApplicationException("NUMBERING_COMMIT_FAILED");
                await audit.WriteAsync(context, "WaybillApprove", "SUCCESS", "Waybill", aggregate.Id,
                    before, Snapshot(aggregate), $"ReservationId={committed.Id}", ct);
                return ToResponse(aggregate, context.CorrelationId);
            }
            catch
            {
                try
                {
                    await numbering.VoidAsync(context,
                        new NumberReservationTransitionRequest(reservation.Id, request.IdempotencyKey, "APPROVAL_FAILED"), ct);
                }
                catch { }
                throw;
            }
        }, cancellationToken);
    }

    public async Task<PagedOperationalPartyResponse> SearchPartiesAsync(
        OperationContext context,
        OperationalPartySearchRequest request,
        CancellationToken cancellationToken = default)
    {
        context.EnsureComplete();
        if (request.Skip < 0 || request.Take is < 1 or > 100)
            throw new WaybillApplicationException("INVALID_FILTER");
        var result = await parties.SearchAsync(context.CompanyId, context.BranchId, request.Query, request.Skip, request.Take, cancellationToken);
        return new PagedOperationalPartyResponse(result.Items.Select(ToPartyResponse).ToList(), result.Total,
            request.Skip, request.Take, context.CorrelationId);
    }

    public async Task<OperationalPartyResponse> CreatePartyAsync(
        OperationContext context,
        OperationalPartyCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        context.EnsureComplete();
        RequireClientOperation(request.ClientOperationId);
        request.Address.EnsureUsable();
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Mobile))
            throw new WaybillApplicationException("VALIDATION_ERROR");
        if (!string.IsNullOrWhiteSpace(request.IdentityNo) && string.IsNullOrWhiteSpace(request.IdentityType))
            throw new WaybillApplicationException("IDENTITY_TYPE_REQUIRED");

        var existing = await parties.GetByClientOperationAsync(context.CompanyId, request.ClientOperationId.Trim(), cancellationToken);
        if (existing is not null)
            return ToPartyResponse(existing);

        var created = await parties.CreateAsync(context.CompanyId, context.BranchId, NewPartyNo(), request, cancellationToken);
        await audit.WriteAsync(context, "OperationalPartyCreate", "SUCCESS", "OperationalParty", created.Id,
            null, JsonSerializer.Serialize(new { created.PartyNo, created.Name, created.Mobile }), null, cancellationToken);
        return ToPartyResponse(created);
    }

    private async Task<bool> IsReplay(
        OperationContext context,
        WaybillAggregate aggregate,
        long expectedVersion,
        string clientOperationId,
        CancellationToken ct)
    {
        if (expectedVersion >= 1 && aggregate.Version == expectedVersion)
            return false;
        if (await waybills.WasLastOperationAsync(context.CompanyId, context.BranchId, aggregate.Id, clientOperationId, ct))
            return true;
        throw new WaybillApplicationException("CONCURRENCY_CONFLICT");
    }

    private async Task<WaybillAggregate> RequireWaybill(OperationContext context, Guid id, CancellationToken ct)
        => await waybills.GetAsync(context.CompanyId, context.BranchId, id, ct)
            ?? throw new WaybillApplicationException("NOT_FOUND");

    private static WaybillPartyValue ToDomainParty(WaybillPartyInput input)
    {
        if (!Enum.TryParse<WaybillPartyRole>(input.Role, true, out var role))
            throw new WaybillApplicationException("PARTY_ROLE_INVALID");
        input.Address.EnsureUsable();
        return new WaybillPartyValue(role, input.OperationalPartyId, input.Name, input.Mobile,
            input.IdentityType, input.IdentityNo,
            input.Address.CountryId, input.Address.GovernorateId, input.Address.CityId, input.Address.AreaId, input.Address.AddressLine);
    }

    private static WaybillItemValue ToDomainItem(WaybillItemInput input)
        => new(input.Id.GetValueOrDefault(Guid.NewGuid()), input.LineNo, input.ItemType, input.Contents,
            input.Quantity, input.Pieces, input.Weight, input.Length, input.Width, input.Height,
            input.DeclaredValue, input.OriginCountryId,
            JsonSerializer.Serialize(input.RiskFlags ?? Array.Empty<string>()), input.Notes, input.Volume);

    public static WaybillResponse ToResponse(WaybillAggregate aggregate, Guid correlationId)
        => new(
            aggregate.Id, aggregate.DraftNo, aggregate.WaybillNo, aggregate.CompanyId, aggregate.BranchId,
            aggregate.WaybillDateTime, aggregate.OriginId, aggregate.DestinationId, aggregate.CurrencyId,
            aggregate.ExchangeRate, aggregate.FreightTotal, aggregate.DiscountTotal, aggregate.NetAmount,
            aggregate.ServiceType, aggregate.Priority, aggregate.Status.ToString().ToUpperInvariant(), aggregate.Version,
            aggregate.Parties.Select(x => new WaybillPartyResponse(
                x.Role.ToString().ToUpperInvariant(), x.OperationalPartyId, x.Name, x.Mobile, x.IdentityType,
                MaskIdentity(x.IdentityNo), new GeoAddressSnapshot(x.CountryId, x.GovernorateId, x.CityId, x.AreaId, x.AddressText))).ToList(),
            aggregate.Items.Select(x => new WaybillItemResponse(
                x.Id, x.LineNo, x.ItemType, x.Contents, x.Quantity, x.Pieces, x.Weight, x.Length, x.Width, x.Height,
                x.DeclaredValue, x.OriginCountryId,
                JsonSerializer.Deserialize<string[]>(x.RiskFlagsJson) ?? Array.Empty<string>(), x.Notes, x.Volume)).ToList(),
            correlationId);

    private static OperationalPartyResponse ToPartyResponse(OperationalPartyRecord party)
        => new(party.Id, party.PartyNo, party.Name, party.Mobile, party.IdentityType, MaskIdentity(party.IdentityNo),
            party.Address, party.Status, party.Version);

    private static string? MaskIdentity(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return value;
        if (value.Length <= 4) return new string('*', value.Length);
        return new string('*', value.Length - 4) + value[^4..];
    }

    private static void EnsureBranch(OperationContext context, Guid branchId)
    {
        if (branchId == Guid.Empty || branchId != context.BranchId)
            throw new WaybillApplicationException("SCOPE_DENIED");
    }

    private static void EnsureVersion(WaybillAggregate aggregate, long expectedVersion)
    {
        if (expectedVersion < 1 || aggregate.Version != expectedVersion)
            throw new WaybillApplicationException("CONCURRENCY_CONFLICT");
    }

    private static void RequireClientOperation(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new WaybillApplicationException("CLIENT_OPERATION_ID_REQUIRED");
    }

    private static void RequireReason(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new WaybillApplicationException("REASON_REQUIRED");
    }

    private static string NewDraftNo() => $"D-{DateTimeOffset.UtcNow:yyyyMMdd}-{Guid.NewGuid():N}"[..27].ToUpperInvariant();
    private static string NewPartyNo() => $"P-{Guid.NewGuid():N}"[..14].ToUpperInvariant();
    private static string Snapshot(WaybillAggregate aggregate) => JsonSerializer.Serialize(ToResponse(aggregate, Guid.Empty));
}

public sealed class WaybillApplicationException(string code) : InvalidOperationException(code)
{
    public string Code { get; } = code;
}
