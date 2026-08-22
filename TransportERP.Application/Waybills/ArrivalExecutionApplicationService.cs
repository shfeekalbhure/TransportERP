using TransportERP.Contracts.Core;
using TransportERP.Contracts.Waybills;
using TransportERP.Domain.Waybills;

namespace TransportERP.Application.Waybills;

public interface IArrivalExecutionStore
{
    Task<ArrivalReceiptResponse> RecordArrivalAsync(OperationContext context, Guid tripId, RecordArrivalRequest request, CancellationToken cancellationToken);
    Task<ArrivalReceiptResponse> RecordUnloadAsync(OperationContext context, Guid arrivalId, RecordUnloadRequest request, CancellationToken cancellationToken);
    Task<AllocationResponse> ReallocateTransitAsync(OperationContext context, Guid holdingId, ReallocateTransitRequest request, CancellationToken cancellationToken);
    Task<ArrivalReceiptResponse> FinalizeArrivalAsync(OperationContext context, Guid arrivalId, FinalizeArrivalRequest request, CancellationToken cancellationToken);
    Task<TripResponse> CloseTripAsync(OperationContext context, Guid tripId, CloseTripRequest request, CancellationToken cancellationToken);
    Task<WaybillMovementResponse> GetWaybillMovementAsync(OperationContext context, Guid waybillId, MovementQueryRequest request, CancellationToken cancellationToken);
    Task<ItemMovementResponse> GetItemMovementAsync(OperationContext context, Guid waybillId, Guid itemId, MovementQueryRequest request, CancellationToken cancellationToken);
}

public sealed class ArrivalExecutionApplicationService(IArrivalExecutionStore store)
{
    public Task<ArrivalReceiptResponse> RecordArrivalAsync(OperationContext context, Guid tripId, RecordArrivalRequest request, CancellationToken ct = default)
    {
        EnsureContext(context); EnsureId(tripId); EnsureId(request.ManifestId); EnsureId(request.LocationId);
        EnsureOperation(request.ClientOperationId);
        if (request.ReceivedAt == default) throw new ArrivalExecutionApplicationException("VALIDATION_ERROR");
        return store.RecordArrivalAsync(context, tripId, request, ct);
    }

    public Task<ArrivalReceiptResponse> RecordUnloadAsync(OperationContext context, Guid arrivalId, RecordUnloadRequest request, CancellationToken ct = default)
    {
        EnsureContext(context); EnsureId(arrivalId); EnsureOperation(request.ClientOperationId);
        if (request.OccurredAt == default || request.Lines is null || request.Lines.Count == 0)
            throw new ArrivalExecutionApplicationException("VALIDATION_ERROR");
        if (request.Lines.GroupBy(x => x.ManifestLineId).Any(g => g.Key == Guid.Empty || g.Count() != 1))
            throw new ArrivalExecutionApplicationException("VALIDATION_ERROR");
        foreach (var line in request.Lines)
        {
            ArrivalExecutionRules.EnsureUnload(decimal.MaxValue, line.ActualQuantity, line.DamageQuantity);
            if (line.Notes is { Length: > 1000 }) throw new ArrivalExecutionApplicationException("VALIDATION_ERROR");
        }
        return store.RecordUnloadAsync(context, arrivalId, request, ct);
    }

    public Task<AllocationResponse> ReallocateTransitAsync(OperationContext context, Guid holdingId, ReallocateTransitRequest request, CancellationToken ct = default)
    {
        EnsureContext(context); EnsureId(holdingId); EnsureId(request.NextTripId); EnsureOperation(request.ClientOperationId);
        if (request.Quantity <= 0m) throw new ArrivalExecutionApplicationException("VALIDATION_ERROR");
        return store.ReallocateTransitAsync(context, holdingId, request, ct);
    }

    public Task<ArrivalReceiptResponse> FinalizeArrivalAsync(OperationContext context, Guid arrivalId, FinalizeArrivalRequest request, CancellationToken ct = default)
    {
        EnsureContext(context); EnsureId(arrivalId); EnsureOperation(request.ClientOperationId); EnsureVersion(request.ExpectedVersion);
        return store.FinalizeArrivalAsync(context, arrivalId, request, ct);
    }

    public Task<TripResponse> CloseTripAsync(OperationContext context, Guid tripId, CloseTripRequest request, CancellationToken ct = default)
    {
        EnsureContext(context); EnsureId(tripId); EnsureOperation(request.ClientOperationId); EnsureVersion(request.ExpectedVersion);
        if (request.ClosedAt == default) throw new ArrivalExecutionApplicationException("VALIDATION_ERROR");
        return store.CloseTripAsync(context, tripId, request, ct);
    }

    public Task<WaybillMovementResponse> GetWaybillMovementAsync(OperationContext context, Guid waybillId, MovementQueryRequest request, CancellationToken ct = default)
    {
        EnsureContext(context); EnsureId(waybillId); EnsureFilter(request);
        return store.GetWaybillMovementAsync(context, waybillId, request, ct);
    }

    public Task<ItemMovementResponse> GetItemMovementAsync(OperationContext context, Guid waybillId, Guid itemId, MovementQueryRequest request, CancellationToken ct = default)
    {
        EnsureContext(context); EnsureId(waybillId); EnsureId(itemId); EnsureFilter(request);
        return store.GetItemMovementAsync(context, waybillId, itemId, request, ct);
    }

    private static void EnsureFilter(MovementQueryRequest request)
    {
        if (request.From.HasValue && request.To.HasValue && request.From >= request.To)
            throw new ArrivalExecutionApplicationException("INVALID_FILTER");
    }

    private static void EnsureContext(OperationContext context) => context.EnsureComplete();
    private static void EnsureId(Guid value) { if (value == Guid.Empty) throw new ArrivalExecutionApplicationException("VALIDATION_ERROR"); }
    private static void EnsureOperation(string value) { if (string.IsNullOrWhiteSpace(value) || value.Trim().Length > 160) throw new ArrivalExecutionApplicationException("CLIENT_OPERATION_REQUIRED"); }
    private static void EnsureVersion(long value) { if (value < 1) throw new ArrivalExecutionApplicationException("CONCURRENCY_CONFLICT"); }
}

public sealed class ArrivalExecutionApplicationException(string code) : InvalidOperationException(code)
{
    public string Code { get; } = code;
}
