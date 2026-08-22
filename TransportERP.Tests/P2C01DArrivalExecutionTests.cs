using TransportERP.Application.Waybills;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Waybills;
using TransportERP.Domain.Waybills;

namespace TransportERP.Tests;

public sealed class P2C01DArrivalExecutionTests
{
    [Fact]
    public void Record_arrival_requires_departed_trip_and_governed_location()
    {
        var destination = Guid.NewGuid();
        ArrivalExecutionRules.EnsureRecordArrival("DEPARTED", destination, destination, false);
        var invalidState = Assert.Throws<ArrivalExecutionRuleException>(() =>
            ArrivalExecutionRules.EnsureRecordArrival("READY", destination, destination, false));
        Assert.Equal("INVALID_STATE", invalidState.Code);
        var invalidLocation = Assert.Throws<ArrivalExecutionRuleException>(() =>
            ArrivalExecutionRules.EnsureRecordArrival("DEPARTED", destination, Guid.NewGuid(), false));
        Assert.Equal("LOCATION_INVALID", invalidLocation.Code);
    }

    [Fact]
    public void Unload_and_reallocation_never_allow_negative_or_overrun_balances()
    {
        ArrivalExecutionRules.EnsureUnload(5m, 5m, 1m);
        Assert.Equal("QUANTITY_EXCEEDS_IN_TRANSIT",
            Assert.Throws<ArrivalExecutionRuleException>(() => ArrivalExecutionRules.EnsureUnload(5m, 5.001m, 0m)).Code);
        Assert.Equal("VALIDATION_ERROR",
            Assert.Throws<ArrivalExecutionRuleException>(() => ArrivalExecutionRules.EnsureUnload(5m, 2m, 3m)).Code);

        ArrivalExecutionRules.EnsureReallocate("AVAILABLE", "TRANSIT", 4m, 4m);
        Assert.Equal("QUANTITY_EXCEEDS_AVAILABLE",
            Assert.Throws<ArrivalExecutionRuleException>(() => ArrivalExecutionRules.EnsureReallocate("AVAILABLE", "TRANSIT", 4m, 4.1m)).Code);
    }

    [Fact]
    public void Difference_and_finalization_require_explicit_evidence_and_validation()
    {
        var evidence = Guid.NewGuid();
        var difference = ArrivalExecutionRules.DifferenceType(10m, 8m, 1m, "SHORT_AND_DAMAGE");
        Assert.Equal("SHORT_AND_DAMAGE", difference);
        ArrivalExecutionRules.EnsureDifferenceEvidence(difference, evidence);
        Assert.Equal("DIFFERENCE_REQUIRES_EVIDENCE",
            Assert.Throws<ArrivalExecutionRuleException>(() => ArrivalExecutionRules.EnsureDifferenceEvidence(difference, null)).Code);

        Assert.Equal("UNVALIDATED_LINES", Assert.Throws<ArrivalExecutionRuleException>(() =>
            ArrivalExecutionRules.EnsureFinalize("DRAFT", new[] { ("UNVALIDATED", 10m, 4m) })).Code);
        ArrivalExecutionRules.EnsureFinalize("DRAFT", new[] { ("SHORT", 10m, 8m), ("NONE", 2m, 2m) });
    }

    [Fact]
    public void Trip_close_requires_arrival_and_full_custody_reconciliation()
    {
        ArrivalExecutionRules.EnsureTripClose("ARRIVED", 10m, 10m, false, false);
        Assert.Equal("CARGO_UNACCOUNTED", Assert.Throws<ArrivalExecutionRuleException>(() =>
            ArrivalExecutionRules.EnsureTripClose("ARRIVED", 10m, 9m, true, false)).Code);
        Assert.Equal("EXCEPTION_BLOCKED", Assert.Throws<ArrivalExecutionRuleException>(() =>
            ArrivalExecutionRules.EnsureTripClose("ARRIVED", 10m, 10m, false, true)).Code);
    }

    [Fact]
    public async Task Application_validates_D_command_shapes_before_store()
    {
        var service = new ArrivalExecutionApplicationService(new NoopArrivalStore());
        var context = new OperationContext(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        var arrivalId = Guid.NewGuid();

        var invalidUnload = await Assert.ThrowsAsync<ArrivalExecutionApplicationException>(() =>
            service.RecordUnloadAsync(context, arrivalId,
                new RecordUnloadRequest([], DateTimeOffset.UtcNow, "op")));
        Assert.Equal("VALIDATION_ERROR", invalidUnload.Code);

        var invalidClose = await Assert.ThrowsAsync<ArrivalExecutionApplicationException>(() =>
            service.CloseTripAsync(context, Guid.NewGuid(), new CloseTripRequest(default, 1, "op")));
        Assert.Equal("VALIDATION_ERROR", invalidClose.Code);
    }

    private sealed class NoopArrivalStore : IArrivalExecutionStore
    {
        private static Task<T> Never<T>() => Task.FromException<T>(new InvalidOperationException("Store should not be called."));
        public Task<ArrivalReceiptResponse> RecordArrivalAsync(OperationContext context, Guid tripId, RecordArrivalRequest request, CancellationToken cancellationToken) => Never<ArrivalReceiptResponse>();
        public Task<ArrivalReceiptResponse> RecordUnloadAsync(OperationContext context, Guid arrivalId, RecordUnloadRequest request, CancellationToken cancellationToken) => Never<ArrivalReceiptResponse>();
        public Task<AllocationResponse> ReallocateTransitAsync(OperationContext context, Guid holdingId, ReallocateTransitRequest request, CancellationToken cancellationToken) => Never<AllocationResponse>();
        public Task<ArrivalReceiptResponse> FinalizeArrivalAsync(OperationContext context, Guid arrivalId, FinalizeArrivalRequest request, CancellationToken cancellationToken) => Never<ArrivalReceiptResponse>();
        public Task<TripResponse> CloseTripAsync(OperationContext context, Guid tripId, CloseTripRequest request, CancellationToken cancellationToken) => Never<TripResponse>();
        public Task<WaybillMovementResponse> GetWaybillMovementAsync(OperationContext context, Guid waybillId, MovementQueryRequest request, CancellationToken cancellationToken) => Never<WaybillMovementResponse>();
        public Task<ItemMovementResponse> GetItemMovementAsync(OperationContext context, Guid waybillId, Guid itemId, MovementQueryRequest request, CancellationToken cancellationToken) => Never<ItemMovementResponse>();
    }
}
