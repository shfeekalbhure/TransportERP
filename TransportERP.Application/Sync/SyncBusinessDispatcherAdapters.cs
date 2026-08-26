using TransportERP.Application.Waybills;
using TransportERP.Contracts.Waybills;

namespace TransportERP.Application.Sync;

/// <summary>
/// Typed business adapters. They deliberately contain no routing or persistence; server composition
/// can register them once the execution worker is ready to invoke the dispatcher.
/// </summary>
public sealed class SyncWaybillBusinessAdapter(WaybillApplicationService service) : ISyncWaybillBusinessAdapter
{
    public async Task<SyncBusinessActionResult> CreateDraftAsync(
        SyncBusinessExecutionContext context, CreateWaybillDraftRequest request, CancellationToken cancellationToken)
    {
        var result = await service.CreateDraftAsync(context.Operation, request, cancellationToken);
        return new(result.Id, result.Version);
    }

    public async Task<SyncBusinessActionResult> UpdateDraftAsync(
        SyncBusinessExecutionContext context,
        Guid waybillId,
        UpdateWaybillDraftRequest request,
        CancellationToken cancellationToken)
    {
        var result = await service.UpdateDraftAsync(context.Operation, waybillId, request, cancellationToken);
        return new(result.Id, result.Version);
    }

    public async Task<SyncBusinessActionResult> CreateOperationalPartyAsync(
        SyncBusinessExecutionContext context,
        OperationalPartyCreateRequest request,
        CancellationToken cancellationToken)
    {
        var result = await service.CreatePartyAsync(context.Operation, request, cancellationToken);
        return new(result.Id, result.Version);
    }
}

public sealed class SyncFinanceBusinessAdapter(WaybillFinanceApplicationService service) : ISyncFinanceBusinessAdapter
{
    public async Task<SyncBusinessActionResult> RecordCollectionAsync(
        SyncBusinessExecutionContext context,
        Guid waybillId,
        RecordCollectionRequest request,
        CancellationToken cancellationToken)
    {
        var collection = await service.RecordCollectionAsync(
            context.Operation, waybillId, request, cancellationToken);
        return new(collection.Id, null);
    }
}

public sealed class SyncShippingBusinessAdapter(ShippingExecutionApplicationService service) : ISyncShippingBusinessAdapter
{
    public async Task<SyncBusinessActionResult> LoadAllocatedQuantityAsync(
        SyncBusinessExecutionContext context,
        Guid manifestLineId,
        SyncLoadAllocatedQuantityPayload payload,
        CancellationToken cancellationToken)
    {
        var result = await service.LoadManifestLineAsync(
            context.Operation, payload.ManifestId, manifestLineId, payload.Request, cancellationToken);
        if (!result.MovementEventId.HasValue || result.MovementEventId == Guid.Empty)
            throw new ShippingExecutionApplicationException("BUSINESS_RESULT_INVALID");
        return new(result.MovementEventId.Value, null);
    }
}
