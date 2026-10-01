using TransportERP.Contracts.Core;
using TransportERP.Contracts.Waybills;
using TransportERP.Domain.Waybills;

namespace TransportERP.Application.Waybills;

public interface IShippingExecutionStore
{
    Task<ItemQuantityStateResponse> ReleaseItemAsync(OperationContext context, Guid waybillId, Guid itemId, ReleaseItemRequest request, CancellationToken cancellationToken);
    Task<TripResponse> CreateTripAsync(OperationContext context, CreateTripRequest request, CancellationToken cancellationToken);
    Task<TripSegmentsResponse> GetTripSegmentsAsync(OperationContext context, Guid tripId, CancellationToken cancellationToken);
    Task<TripSegmentsResponse> SetTripSegmentsAsync(OperationContext context, Guid tripId, SetTripSegmentsRequest request, CancellationToken cancellationToken);
    Task<AllocationResponse> AllocateAsync(OperationContext context, Guid tripId, AllocateItemRequest request, CancellationToken cancellationToken);
    Task<AllocationResponse> UnallocateAsync(OperationContext context, Guid allocationId, UnallocateRequest request, CancellationToken cancellationToken);
    Task<ManifestResponse> GenerateManifestAsync(OperationContext context, Guid tripId, GenerateManifestRequest request, CancellationToken cancellationToken);
    Task<ManifestLineResponse> LoadManifestLineAsync(OperationContext context, Guid manifestId, Guid lineId, LoadManifestLineRequest request, CancellationToken cancellationToken);
    Task<ManifestResponse> FinalizeManifestAsync(OperationContext context, Guid manifestId, FinalizeManifestRequest request, CancellationToken cancellationToken);
    Task<ManifestResponse> HandoverManifestAsync(OperationContext context, Guid manifestId, HandoverManifestRequest request, CancellationToken cancellationToken);
    Task<TripResponse> StartTripAsync(OperationContext context, Guid tripId, StartTripRequest request, CancellationToken cancellationToken);
    Task<TripSegmentsResponse> AdvanceTripSegmentAsync(OperationContext context, Guid tripId, Guid segmentId, AdvanceTripSegmentRequest request, CancellationToken cancellationToken);
}

public sealed class ShippingExecutionApplicationService(IShippingExecutionStore store)
{
    public Task<ItemQuantityStateResponse> ReleaseItemAsync(
        OperationContext context,
        Guid waybillId,
        Guid itemId,
        ReleaseItemRequest request,
        CancellationToken cancellationToken = default)
    {
        EnsureContext(context);
        EnsureId(waybillId);
        EnsureId(itemId);
        EnsureOperation(request.ClientOperationId);
        if (request.Quantity <= 0m || request.ReleasedAt == default)
            throw new ShippingExecutionApplicationException("QUANTITY_INVALID");
        return store.ReleaseItemAsync(context, waybillId, itemId, request, cancellationToken);
    }

    public Task<TripResponse> CreateTripAsync(
        OperationContext context,
        CreateTripRequest request,
        CancellationToken cancellationToken = default)
    {
        EnsureContext(context);
        EnsureOperation(request.ClientOperationId);
        var stops = (request.Stops ?? []).Select(x => (x.StopNo, x.LocationId)).ToList();
        ShippingExecutionRules.EnsureTripInput(
            request.TripNo, request.VehicleId, request.DriverId, request.OriginId,
            request.DestinationId, request.PlannedDepartAt, stops);
        return store.CreateTripAsync(context, request, cancellationToken);
    }

    public Task<TripSegmentsResponse> GetTripSegmentsAsync(
        OperationContext context,
        Guid tripId,
        CancellationToken cancellationToken = default)
    {
        EnsureContext(context);
        EnsureId(tripId);
        return store.GetTripSegmentsAsync(context, tripId, cancellationToken);
    }

    public Task<TripSegmentsResponse> SetTripSegmentsAsync(
        OperationContext context,
        Guid tripId,
        SetTripSegmentsRequest request,
        CancellationToken cancellationToken = default)
    {
        EnsureContext(context);
        EnsureId(tripId);
        EnsureOperation(request.ClientOperationId);
        EnsureVersion(request.ExpectedVersion);
        EnsureSegments(request.Segments);
        return store.SetTripSegmentsAsync(context, tripId, request, cancellationToken);
    }

    public Task<AllocationResponse> AllocateAsync(
        OperationContext context,
        Guid tripId,
        AllocateItemRequest request,
        CancellationToken cancellationToken = default)
    {
        EnsureContext(context);
        EnsureId(tripId);
        EnsureId(request.WaybillItemId);
        EnsureId(request.ReleaseId);
        EnsureOperation(request.ClientOperationId);
        if (request.Quantity <= 0m)
            throw new ShippingExecutionApplicationException("QUANTITY_INVALID");
        return store.AllocateAsync(context, tripId, request, cancellationToken);
    }

    public Task<AllocationResponse> UnallocateAsync(
        OperationContext context,
        Guid allocationId,
        UnallocateRequest request,
        CancellationToken cancellationToken = default)
    {
        EnsureContext(context);
        EnsureId(allocationId);
        EnsureOperation(request.ClientOperationId);
        if (string.IsNullOrWhiteSpace(request.Reason))
            throw new ShippingExecutionApplicationException("REASON_REQUIRED");
        return store.UnallocateAsync(context, allocationId, request, cancellationToken);
    }

    public Task<ManifestResponse> GenerateManifestAsync(
        OperationContext context,
        Guid tripId,
        GenerateManifestRequest request,
        CancellationToken cancellationToken = default)
    {
        EnsureContext(context);
        EnsureId(tripId);
        EnsureOperation(request.ClientOperationId);
        if (!string.IsNullOrWhiteSpace(request.ManifestNo) && request.ManifestNo.Trim().Length > 100)
            throw new ShippingExecutionApplicationException("VALIDATION_ERROR");
        return store.GenerateManifestAsync(context, tripId, request, cancellationToken);
    }

    public Task<ManifestLineResponse> LoadManifestLineAsync(
        OperationContext context,
        Guid manifestId,
        Guid lineId,
        LoadManifestLineRequest request,
        CancellationToken cancellationToken = default)
    {
        EnsureContext(context);
        EnsureId(manifestId);
        EnsureId(lineId);
        EnsureOperation(request.ClientOperationId);
        if (request.Quantity <= 0m || request.OccurredAt == default)
            throw new ShippingExecutionApplicationException("QUANTITY_INVALID");
        return store.LoadManifestLineAsync(context, manifestId, lineId, request, cancellationToken);
    }

    public Task<ManifestResponse> FinalizeManifestAsync(
        OperationContext context,
        Guid manifestId,
        FinalizeManifestRequest request,
        CancellationToken cancellationToken = default)
    {
        EnsureContext(context);
        EnsureId(manifestId);
        EnsureOperation(request.ClientOperationId);
        EnsureVersion(request.ExpectedVersion);
        return store.FinalizeManifestAsync(context, manifestId, request, cancellationToken);
    }

    public Task<ManifestResponse> HandoverManifestAsync(
        OperationContext context,
        Guid manifestId,
        HandoverManifestRequest request,
        CancellationToken cancellationToken = default)
    {
        EnsureContext(context);
        EnsureId(manifestId);
        EnsureId(request.DriverId);
        EnsureOperation(request.ClientOperationId);
        EnsureVersion(request.ExpectedVersion);
        if (request.AcceptedAt == default)
            throw new ShippingExecutionApplicationException("VALIDATION_ERROR");
        return store.HandoverManifestAsync(context, manifestId, request, cancellationToken);
    }

    public Task<TripResponse> StartTripAsync(
        OperationContext context,
        Guid tripId,
        StartTripRequest request,
        CancellationToken cancellationToken = default)
    {
        EnsureContext(context);
        EnsureId(tripId);
        EnsureOperation(request.ClientOperationId);
        EnsureVersion(request.ExpectedVersion);
        if (request.ActualDepartAt == default)
            throw new ShippingExecutionApplicationException("VALIDATION_ERROR");
        return store.StartTripAsync(context, tripId, request, cancellationToken);
    }

    public Task<TripSegmentsResponse> AdvanceTripSegmentAsync(
        OperationContext context,
        Guid tripId,
        Guid segmentId,
        AdvanceTripSegmentRequest request,
        CancellationToken cancellationToken = default)
    {
        EnsureContext(context);
        EnsureId(tripId);
        EnsureId(segmentId);
        EnsureOperation(request.ClientOperationId);
        EnsureVersion(request.ExpectedTripVersion);
        EnsureVersion(request.ExpectedSegmentVersion);
        if (request.ArrivedAt == default)
            throw new ShippingExecutionApplicationException("VALIDATION_ERROR");
        return store.AdvanceTripSegmentAsync(context, tripId, segmentId, request, cancellationToken);
    }

    private static void EnsureSegments(IReadOnlyList<TripSegmentInput>? segments)
    {
        if (segments is null || segments.Count == 0)
            throw new ShippingExecutionApplicationException("VALIDATION_ERROR");

        var ordered = segments.OrderBy(x => x.SegmentNo).ToList();
        for (var i = 0; i < ordered.Count; i++)
        {
            var segment = ordered[i];
            if (segment.SegmentNo != i + 1 ||
                segment.FromLocationId == Guid.Empty ||
                segment.ToLocationId == Guid.Empty ||
                segment.FromLocationId == segment.ToLocationId ||
                segment.DriverId == Guid.Empty ||
                segment.VehicleId == Guid.Empty ||
                segment.FromStopId == Guid.Empty ||
                segment.ToStopId == Guid.Empty ||
                (segment.PlannedDepartAt.HasValue && segment.PlannedArriveAt.HasValue &&
                 segment.PlannedArriveAt.Value < segment.PlannedDepartAt.Value))
                throw new ShippingExecutionApplicationException("VALIDATION_ERROR");

            if (i > 0 && ordered[i - 1].ToLocationId != segment.FromLocationId)
                throw new ShippingExecutionApplicationException("ROUTE_INCOMPATIBLE");
        }
    }

    private static void EnsureContext(OperationContext context)
    {
        context.EnsureComplete();
    }

    private static void EnsureOperation(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Trim().Length > 160)
            throw new ShippingExecutionApplicationException("CLIENT_OPERATION_REQUIRED");
    }

    private static void EnsureId(Guid id)
    {
        if (id == Guid.Empty)
            throw new ShippingExecutionApplicationException("VALIDATION_ERROR");
    }

    private static void EnsureVersion(long version)
    {
        if (version < 1)
            throw new ShippingExecutionApplicationException("CONCURRENCY_CONFLICT");
    }
}

public sealed class ShippingExecutionApplicationException(string code) : InvalidOperationException(code)
{
    public string Code { get; } = code;
}
