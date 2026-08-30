using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using TransportERP.Application.Waybills;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Waybills;
using TransportERP.Domain.Waybills;

namespace TransportERP.Infrastructure.Persistence;

public sealed class EfArrivalExecutionStore(TransportErpDbContext db, IWaybillAuditSink audit) : IArrivalExecutionStore
{
    private DbSet<WaybillEntity> Waybills => db.Set<WaybillEntity>();
    private DbSet<WaybillItemEntity> Items => db.Set<WaybillItemEntity>();
    private DbSet<ItemReleaseEntity> Releases => db.Set<ItemReleaseEntity>();
    private DbSet<TripEntity> Trips => db.Set<TripEntity>();
    private DbSet<TripStopEntity> Stops => db.Set<TripStopEntity>();
    private DbSet<TripAllocationEntity> Allocations => db.Set<TripAllocationEntity>();
    private DbSet<ManifestEntity> Manifests => db.Set<ManifestEntity>();
    private DbSet<ManifestLineEntity> ManifestLines => db.Set<ManifestLineEntity>();
    private DbSet<MovementEventEntity> Movements => db.Set<MovementEventEntity>();
    private DbSet<WaybillHoldEntity> Holds => db.Set<WaybillHoldEntity>();
    private DbSet<ArrivalReceiptEntity> Receipts => db.Set<ArrivalReceiptEntity>();
    private DbSet<ArrivalReceiptLineEntity> ReceiptLines => db.Set<ArrivalReceiptLineEntity>();
    private DbSet<WarehouseHoldingEntity> Holdings => db.Set<WarehouseHoldingEntity>();

    public async Task<ArrivalReceiptResponse> RecordArrivalAsync(
        OperationContext context, Guid tripId, RecordArrivalRequest request, CancellationToken ct)
    {
        const string action = "ARRIVAL_RECORD";
        var operationId = request.ClientOperationId.Trim();
        var fingerprint = Fingerprint(action, new { tripId, request.ManifestId, request.LocationId, request.ReceivedAt });
        var replay = await TryReplayAsync<ArrivalReceiptResponse>(context, action, operationId, "Trip", tripId, fingerprint, ct);
        if (replay is not null) return replay;

        try
        {
            await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct);
            replay = await TryReplayAsync<ArrivalReceiptResponse>(context, action, operationId, "Trip", tripId, fingerprint, ct);
            if (replay is not null) { await tx.CommitAsync(ct); return replay; }

            await EnsureActiveBranch(context, ct);
            var trip = await Trips.SingleOrDefaultAsync(x => x.Id == tripId && x.CompanyId == context.CompanyId, ct)
                ?? throw new WaybillPersistenceException("NOT_FOUND");
            var stop = await Stops.SingleOrDefaultAsync(x => x.TripId == tripId && x.LocationId == request.LocationId, ct);
            ArrivalExecutionRules.EnsureRecordArrival(trip.Status, trip.DestinationId, request.LocationId, stop is not null);
            if (stop is not null && stop.Status is not "PLANNED" and not "DEPARTED")
                throw new WaybillPersistenceException("INVALID_STATE");

            var manifest = await Manifests.AsNoTracking().SingleOrDefaultAsync(x =>
                    x.Id == request.ManifestId && x.TripId == tripId && x.CompanyId == context.CompanyId &&
                    x.Status == "ACCEPTED", ct)
                ?? throw new WaybillPersistenceException("INVALID_STATE");
            if (await Receipts.AsNoTracking().AnyAsync(x =>
                x.TripId == tripId && x.ManifestId == manifest.Id && x.LocationId == request.LocationId, ct))
                throw new WaybillPersistenceException("DUPLICATE_OPERATION");

            var manifestLines = await ManifestLines.AsNoTracking().Where(x =>
                    x.ManifestId == manifest.Id && x.LoadedQuantity > 0m)
                .ToListAsync(ct);
            if (manifestLines.Count == 0) throw new WaybillPersistenceException("INVALID_STATE");

            var now = DateTimeOffset.UtcNow;
            var receipt = new ArrivalReceiptEntity
            {
                Id = Guid.NewGuid(), CompanyId = context.CompanyId, ReceivingBranchId = context.BranchId,
                TripId = trip.Id, ManifestId = manifest.Id, LocationId = request.LocationId,
                ReceivedAt = request.ReceivedAt, ReceivedBy = context.UserId,
                Status = ArrivalExecutionStatuses.Receipt.Draft,
                CreateClientOperationId = operationId, LastClientOperationId = operationId,
                CreatedAt = now, UpdatedAt = now, Version = 1
            };
            Receipts.Add(receipt);

            foreach (var manifestLine in manifestLines)
            {
                var priorUnloaded = await Movements.AsNoTracking().Where(x =>
                        x.CompanyId == context.CompanyId && x.ManifestLineId == manifestLine.Id && x.EventType == "UNLOAD")
                    .SumAsync(x => x.Quantity ?? 0m, ct);
                var expected = manifestLine.LoadedQuantity - priorUnloaded;
                if (expected <= 0m) continue;

                var line = new ArrivalReceiptLineEntity
                {
                    Id = Guid.NewGuid(), ArrivalReceiptId = receipt.Id, ManifestLineId = manifestLine.Id,
                    WaybillItemId = manifestLine.WaybillItemId, ExpectedQty = expected, ActualQty = 0m,
                    DifferenceType = ArrivalExecutionStatuses.Difference.Unvalidated, DamageQty = 0m
                };
                ReceiptLines.Add(line);
                Movements.Add(new MovementEventEntity
                {
                    Id = Guid.NewGuid(), CompanyId = context.CompanyId, BranchId = context.BranchId,
                    WaybillId = manifestLine.WaybillId, WaybillItemId = manifestLine.WaybillItemId,
                    AllocationId = manifestLine.AllocationId, ManifestLineId = manifestLine.Id,
                    EventType = "ARRIVE", Quantity = expected, TripId = trip.Id, ManifestId = manifest.Id,
                    ToLocationId = request.LocationId, OccurredAt = request.ReceivedAt, RecordedAt = now,
                    RecordedBy = context.UserId,
                    ClientOperationId = ScopedMovementId(context.BranchId, operationId, manifestLine.Id, "arrive")
                });
            }

            if (!db.ChangeTracker.Entries<ArrivalReceiptLineEntity>().Any(x => x.State == EntityState.Added))
                throw new WaybillPersistenceException("INVALID_STATE");

            if (request.LocationId == trip.DestinationId)
            {
                trip.Status = "ARRIVED";
                trip.ActualArriveAt = request.ReceivedAt;
            }
            else if (stop is not null)
            {
                stop.Status = "ARRIVED";
                stop.ArrivedAt = request.ReceivedAt;
            }
            trip.LastClientOperationId = operationId;
            trip.Version++;
            trip.UpdatedAt = now;

            await Save(ct);
            var response = await ReceiptResponseOf(context, receipt.Id, ct);
            await PersistOutcomeAsync(context, action, operationId, "Trip", tripId, fingerprint, response, ct);
            await audit.WriteAsync(context, "ArrivalRecord", "SUCCESS", "ArrivalReceipt", receipt.Id,
                null, JsonSerializer.Serialize(new { tripId, manifest.Id, request.LocationId, request.ReceivedAt, LineCount = response.Lines.Count }), null, ct);
            await tx.CommitAsync(ct);
            return response;
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            db.ChangeTracker.Clear();
            replay = await TryReplayAsync<ArrivalReceiptResponse>(context, action, operationId, "Trip", tripId, fingerprint, ct);
            if (replay is not null) return replay;
            throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT", ex);
        }
        catch (PostgresException ex) when (ex.SqlState == "23505")
        {
            db.ChangeTracker.Clear();
            replay = await TryReplayAsync<ArrivalReceiptResponse>(context, action, operationId, "Trip", tripId, fingerprint, ct);
            if (replay is not null) return replay;
            throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT", ex);
        }
        catch (Exception ex) when (IsSerializationFailure(ex))
        {
            throw new WaybillPersistenceException("CONCURRENCY_CONFLICT", ex);
        }
    }

    public async Task<ArrivalReceiptResponse> RecordUnloadAsync(
        OperationContext context, Guid arrivalId, RecordUnloadRequest request, CancellationToken ct)
    {
        const string action = "ARRIVAL_UNLOAD";
        var operationId = request.ClientOperationId.Trim();
        var fingerprint = Fingerprint(action, new
        {
            arrivalId, request.OccurredAt,
            Lines = request.Lines.OrderBy(x => x.ManifestLineId).Select(x => new
            { x.ManifestLineId, x.ActualQuantity, x.DamageQuantity, x.DifferenceType, x.EvidenceAttachmentId, x.Notes })
        });
        var replay = await TryReplayAsync<ArrivalReceiptResponse>(context, action, operationId, "ArrivalReceipt", arrivalId, fingerprint, ct);
        if (replay is not null) return replay;

        try
        {
            await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct);
            replay = await TryReplayAsync<ArrivalReceiptResponse>(context, action, operationId, "ArrivalReceipt", arrivalId, fingerprint, ct);
            if (replay is not null) { await tx.CommitAsync(ct); return replay; }

            var receipt = await RequireReceipt(context, arrivalId, ct);
            if (receipt.Status != ArrivalExecutionStatuses.Receipt.Draft)
                throw new WaybillPersistenceException("INVALID_STATE");
            var trip = await Trips.AsNoTracking().SingleAsync(x => x.Id == receipt.TripId && x.CompanyId == context.CompanyId, ct);
            var now = DateTimeOffset.UtcNow;

            foreach (var input in request.Lines)
            {
                var line = await ReceiptLines.SingleOrDefaultAsync(x =>
                        x.ArrivalReceiptId == receipt.Id && x.ManifestLineId == input.ManifestLineId, ct)
                    ?? throw new WaybillPersistenceException("NOT_FOUND");
                var manifestLine = await ManifestLines.AsNoTracking().SingleAsync(x => x.Id == line.ManifestLineId, ct);
                var remaining = line.ExpectedQty - line.ActualQty;
                ArrivalExecutionRules.EnsureUnload(remaining, input.ActualQuantity, input.DamageQuantity);

                var newActual = line.ActualQty + input.ActualQuantity;
                var newDamage = line.DamageQty + input.DamageQuantity;
                string difference;
                if (newActual < line.ExpectedQty - 0.0001m && string.IsNullOrWhiteSpace(input.DifferenceType) && newDamage <= 0.0001m)
                    difference = ArrivalExecutionStatuses.Difference.Unvalidated;
                else
                    difference = ArrivalExecutionRules.DifferenceType(line.ExpectedQty, newActual, newDamage, input.DifferenceType);
                if (difference != ArrivalExecutionStatuses.Difference.Unvalidated)
                    ArrivalExecutionRules.EnsureDifferenceEvidence(difference, input.EvidenceAttachmentId ?? line.EvidenceAttachmentId);

                line.ActualQty = newActual;
                line.DamageQty = newDamage;
                line.DifferenceType = difference;
                if (input.EvidenceAttachmentId.HasValue) line.EvidenceAttachmentId = input.EvidenceAttachmentId;
                if (input.Notes is not null) line.Notes = input.Notes.Trim();

                Guid? availableHoldingId = null;
                Guid? damageHoldingId = null;
                var availableQty = input.ActualQuantity - input.DamageQuantity;
                var holdingType = receipt.LocationId == trip.DestinationId
                    ? ArrivalExecutionStatuses.Holding.Destination
                    : ArrivalExecutionStatuses.Holding.Transit;
                if (availableQty > 0m)
                {
                    var holding = NewHolding(context, manifestLine.WaybillItemId, receipt.LocationId,
                        availableQty, holdingType, ArrivalExecutionStatuses.Holding.Available, operationId, now);
                    Holdings.Add(holding);
                    availableHoldingId = holding.Id;
                }
                if (input.DamageQuantity > 0m)
                {
                    var damaged = NewHolding(context, manifestLine.WaybillItemId, receipt.LocationId,
                        input.DamageQuantity, holdingType, ArrivalExecutionStatuses.Holding.Exception, operationId, now);
                    Holdings.Add(damaged);
                    damageHoldingId = damaged.Id;
                }

                if (input.ActualQuantity > 0m)
                {
                    Movements.Add(new MovementEventEntity
                    {
                        Id = Guid.NewGuid(), CompanyId = context.CompanyId, BranchId = context.BranchId,
                        WaybillId = manifestLine.WaybillId, WaybillItemId = manifestLine.WaybillItemId,
                        AllocationId = manifestLine.AllocationId, ManifestLineId = manifestLine.Id,
                        EventType = "UNLOAD", Quantity = input.ActualQuantity,
                        TripId = receipt.TripId, ManifestId = receipt.ManifestId,
                        ToLocationId = receipt.LocationId, OccurredAt = request.OccurredAt, RecordedAt = now,
                        RecordedBy = context.UserId,
                        ReasonCode = HoldingReason(availableHoldingId, damageHoldingId, difference),
                        ClientOperationId = ScopedMovementId(context.BranchId, operationId, manifestLine.Id, "unload")
                    });
                }
            }

            receipt.LastClientOperationId = operationId;
            receipt.Version++;
            receipt.UpdatedAt = now;
            await Save(ct);
            var response = await ReceiptResponseOf(context, receipt.Id, ct);
            await PersistOutcomeAsync(context, action, operationId, "ArrivalReceipt", receipt.Id, fingerprint, response, ct);
            await audit.WriteAsync(context, "ArrivalUnload", "SUCCESS", "ArrivalReceipt", receipt.Id,
                null, JsonSerializer.Serialize(new
                {
                    receipt.LocationId, request.OccurredAt,
                    Lines = request.Lines.Select(x => new { x.ManifestLineId, x.ActualQuantity, x.DamageQuantity, x.DifferenceType, x.EvidenceAttachmentId })
                }), null, ct);
            await tx.CommitAsync(ct);
            return response;
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            db.ChangeTracker.Clear();
            replay = await TryReplayAsync<ArrivalReceiptResponse>(context, action, operationId, "ArrivalReceipt", arrivalId, fingerprint, ct);
            if (replay is not null) return replay;
            throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT", ex);
        }
        catch (PostgresException ex) when (ex.SqlState == "23505")
        {
            db.ChangeTracker.Clear();
            replay = await TryReplayAsync<ArrivalReceiptResponse>(context, action, operationId, "ArrivalReceipt", arrivalId, fingerprint, ct);
            if (replay is not null) return replay;
            throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT", ex);
        }
        catch (Exception ex) when (IsSerializationFailure(ex))
        {
            throw new WaybillPersistenceException("CONCURRENCY_CONFLICT", ex);
        }
    }

    public async Task<AllocationResponse> ReallocateTransitAsync(
        OperationContext context, Guid holdingId, ReallocateTransitRequest request, CancellationToken ct)
    {
        const string action = "TRANSIT_REALLOCATE";
        var operationId = request.ClientOperationId.Trim();
        var fingerprint = Fingerprint(action, new { holdingId, request.NextTripId, request.Quantity });
        var replay = await TryReplayAsync<AllocationResponse>(context, action, operationId, "WarehouseHolding", holdingId, fingerprint, ct);
        if (replay is not null) return replay;

        try
        {
            await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct);
            replay = await TryReplayAsync<AllocationResponse>(context, action, operationId, "WarehouseHolding", holdingId, fingerprint, ct);
            if (replay is not null) { await tx.CommitAsync(ct); return replay; }

            var holding = await Holdings.SingleOrDefaultAsync(x =>
                    x.Id == holdingId && x.CompanyId == context.CompanyId && x.BranchId == context.BranchId, ct)
                ?? throw new WaybillPersistenceException("NOT_FOUND");
            ArrivalExecutionRules.EnsureReallocate(holding.Status, holding.HoldingType, holding.Quantity, request.Quantity);

            var nextTrip = await Trips.SingleOrDefaultAsync(x =>
                    x.Id == request.NextTripId && x.CompanyId == context.CompanyId && x.BranchId == context.BranchId, ct)
                ?? throw new WaybillPersistenceException("NOT_FOUND");
            if (nextTrip.Status is not "DRAFT" and not "READY")
                throw new WaybillPersistenceException("INVALID_STATE");
            var stopLocations = await Stops.AsNoTracking().Where(x => x.TripId == nextTrip.Id).Select(x => x.LocationId).ToListAsync(ct);
            ArrivalExecutionRules.EnsureRouteCompatible(holding.LocationId, nextTrip.OriginId, stopLocations);

            var itemScope = await (from item in Items.AsNoTracking()
                                   join waybill in Waybills.AsNoTracking() on item.WaybillId equals waybill.Id
                                   where item.Id == holding.WaybillItemId && waybill.CompanyId == context.CompanyId
                                   select new { Item = item, Waybill = waybill }).SingleOrDefaultAsync(ct)
                ?? throw new WaybillPersistenceException("NOT_FOUND");
            if (await Holds.AsNoTracking().AnyAsync(x =>
                x.CompanyId == context.CompanyId && x.WaybillId == itemScope.Waybill.Id && x.Status == "ACTIVE", ct))
                throw new WaybillPersistenceException("HOLD_BLOCKED");

            var marker = $"H:{holding.Id:N}";
            var sourceMovement = await Movements.AsNoTracking().Where(x =>
                    x.CompanyId == context.CompanyId && x.WaybillItemId == holding.WaybillItemId &&
                    x.EventType == "UNLOAD" && x.ReasonCode != null && x.ReasonCode.Contains(marker) && x.AllocationId != null)
                .OrderByDescending(x => x.RecordedAt).FirstOrDefaultAsync(ct)
                ?? throw new WaybillPersistenceException("INVALID_STATE");
            var sourceAllocation = await Allocations.AsNoTracking().SingleOrDefaultAsync(x =>
                    x.Id == sourceMovement.AllocationId && x.CompanyId == context.CompanyId, ct)
                ?? throw new WaybillPersistenceException("INVALID_STATE");

            var allocation = new TripAllocationEntity
            {
                Id = Guid.NewGuid(), CompanyId = context.CompanyId, BranchId = context.BranchId,
                WaybillItemId = holding.WaybillItemId, ReleaseId = sourceAllocation.ReleaseId,
                TripId = nextTrip.Id, Quantity = request.Quantity, AllocatedAt = DateTimeOffset.UtcNow,
                ClientOperationId = operationId, Status = "ALLOCATED",
                Reason = $"TRANSIT_REALLOCATION:{holding.Id:N}"
            };
            Allocations.Add(allocation);

            holding.Quantity -= request.Quantity;
            holding.Status = holding.Quantity <= 0.0001m ? ArrivalExecutionStatuses.Holding.Released : ArrivalExecutionStatuses.Holding.Available;
            holding.Version++;
            holding.UpdatedAt = DateTimeOffset.UtcNow;

            Movements.Add(new MovementEventEntity
            {
                Id = Guid.NewGuid(), CompanyId = context.CompanyId, BranchId = context.BranchId,
                WaybillId = itemScope.Waybill.Id, WaybillItemId = holding.WaybillItemId,
                AllocationId = allocation.Id, EventType = "REALLOCATE", Quantity = request.Quantity,
                TripId = nextTrip.Id, FromLocationId = holding.LocationId, ToLocationId = nextTrip.DestinationId,
                OccurredAt = allocation.AllocatedAt, RecordedAt = DateTimeOffset.UtcNow, RecordedBy = context.UserId,
                ReasonCode = marker,
                ClientOperationId = ScopedMovementId(context.BranchId, operationId, allocation.Id, "reallocate")
            });

            await Save(ct);
            var response = new AllocationResponse(allocation.Id, allocation.WaybillItemId, allocation.ReleaseId,
                allocation.TripId, allocation.Quantity, allocation.Status, allocation.ReversalOfId, context.CorrelationId);
            await PersistOutcomeAsync(context, action, operationId, "WarehouseHolding", holdingId, fingerprint, response, ct);
            await audit.WriteAsync(context, "TransitReallocate", "SUCCESS", "WarehouseHolding", holding.Id,
                null, JsonSerializer.Serialize(new { holding.LocationId, request.NextTripId, request.Quantity, allocation.Id }), null, ct);
            await tx.CommitAsync(ct);
            return response;
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            db.ChangeTracker.Clear();
            replay = await TryReplayAsync<AllocationResponse>(context, action, operationId, "WarehouseHolding", holdingId, fingerprint, ct);
            if (replay is not null) return replay;
            throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT", ex);
        }
        catch (PostgresException ex) when (ex.SqlState == "23505")
        {
            db.ChangeTracker.Clear();
            replay = await TryReplayAsync<AllocationResponse>(context, action, operationId, "WarehouseHolding", holdingId, fingerprint, ct);
            if (replay is not null) return replay;
            throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT", ex);
        }
        catch (Exception ex) when (IsSerializationFailure(ex))
        {
            throw new WaybillPersistenceException("CONCURRENCY_CONFLICT", ex);
        }
    }

    public async Task<ArrivalReceiptResponse> FinalizeArrivalAsync(
        OperationContext context, Guid arrivalId, FinalizeArrivalRequest request, CancellationToken ct)
    {
        const string action = "ARRIVAL_FINALIZE";
        var operationId = request.ClientOperationId.Trim();
        var fingerprint = Fingerprint(action, new { arrivalId, request.ExpectedVersion });
        var replay = await TryReplayAsync<ArrivalReceiptResponse>(context, action, operationId, "ArrivalReceipt", arrivalId, fingerprint, ct);
        if (replay is not null) return replay;

        try
        {
            await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct);
            replay = await TryReplayAsync<ArrivalReceiptResponse>(context, action, operationId, "ArrivalReceipt", arrivalId, fingerprint, ct);
            if (replay is not null) { await tx.CommitAsync(ct); return replay; }

            var receipt = await RequireReceipt(context, arrivalId, ct);
            if (receipt.Version != request.ExpectedVersion) throw new WaybillPersistenceException("CONCURRENCY_CONFLICT");
            var lines = await ReceiptLines.Where(x => x.ArrivalReceiptId == receipt.Id).ToListAsync(ct);
            ArrivalExecutionRules.EnsureFinalize(receipt.Status, lines.Select(x => (x.DifferenceType, x.ExpectedQty, x.ActualQty)));
            foreach (var line in lines)
                ArrivalExecutionRules.EnsureDifferenceEvidence(line.DifferenceType, line.EvidenceAttachmentId);

            receipt.Status = ArrivalExecutionStatuses.Receipt.Finalized;
            receipt.LastClientOperationId = operationId;
            receipt.Version++;
            receipt.UpdatedAt = DateTimeOffset.UtcNow;
            await Save(ct);
            var response = await ReceiptResponseOf(context, receipt.Id, ct);
            await PersistOutcomeAsync(context, action, operationId, "ArrivalReceipt", receipt.Id, fingerprint, response, ct);
            await audit.WriteAsync(context, "ArrivalFinalize", "SUCCESS", "ArrivalReceipt", receipt.Id,
                null, JsonSerializer.Serialize(new
                {
                    receipt.TripId, receipt.ManifestId, receipt.LocationId,
                    Expected = lines.Sum(x => x.ExpectedQty), Actual = lines.Sum(x => x.ActualQty),
                    Damage = lines.Sum(x => x.DamageQty), Differences = lines.Count(x => x.DifferenceType != "NONE")
                }), null, ct);
            await tx.CommitAsync(ct);
            return response;
        }
        catch (PostgresException ex) when (ex.SqlState == "23505")
        {
            db.ChangeTracker.Clear();
            replay = await TryReplayAsync<ArrivalReceiptResponse>(context, action, operationId, "ArrivalReceipt", arrivalId, fingerprint, ct);
            if (replay is not null) return replay;
            throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT", ex);
        }
        catch (Exception ex) when (IsSerializationFailure(ex))
        {
            throw new WaybillPersistenceException("CONCURRENCY_CONFLICT", ex);
        }
    }

    public async Task<TripResponse> CloseTripAsync(
        OperationContext context, Guid tripId, CloseTripRequest request, CancellationToken ct)
    {
        const string action = "TRIP_CLOSE";
        var operationId = request.ClientOperationId.Trim();
        var fingerprint = Fingerprint(action, new { tripId, request.ClosedAt, request.ExpectedVersion });
        var replay = await TryReplayAsync<TripResponse>(context, action, operationId, "Trip", tripId, fingerprint, ct);
        if (replay is not null) return replay;

        try
        {
            await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct);
            replay = await TryReplayAsync<TripResponse>(context, action, operationId, "Trip", tripId, fingerprint, ct);
            if (replay is not null) { await tx.CommitAsync(ct); return replay; }

            var trip = await Trips.SingleOrDefaultAsync(x =>
                    x.Id == tripId && x.CompanyId == context.CompanyId && x.BranchId == context.BranchId, ct)
                ?? throw new WaybillPersistenceException("NOT_FOUND");
            if (trip.Version != request.ExpectedVersion) throw new WaybillPersistenceException("CONCURRENCY_CONFLICT");
            var departed = await Movements.AsNoTracking().Where(x =>
                    x.CompanyId == context.CompanyId && x.TripId == tripId && x.EventType == "DEPART")
                .SumAsync(x => x.Quantity ?? 0m, ct);
            var accounted = await Movements.AsNoTracking().Where(x =>
                    x.CompanyId == context.CompanyId && x.TripId == tripId && x.EventType == "UNLOAD")
                .SumAsync(x => x.Quantity ?? 0m, ct);
            var custodyOpen = departed - accounted > 0.0001m;
            var exceptionBlocked = await db.Set<ShipmentExceptionEntity>().AsNoTracking().AnyAsync(x =>
                x.CompanyId == context.CompanyId && x.TripId == tripId && x.Status == "OPEN" && x.Severity == "BLOCKING", ct);
            ArrivalExecutionRules.EnsureTripClose(trip.Status, departed, accounted, custodyOpen, exceptionBlocked);

            var manifests = await Manifests.Where(x => x.TripId == tripId && x.CompanyId == context.CompanyId).ToListAsync(ct);
            foreach (var manifest in manifests)
            {
                if (manifest.Status == "ACCEPTED")
                {
                    manifest.Status = "CLOSED";
                    manifest.Version++;
                    manifest.UpdatedAt = request.ClosedAt;
                }
            }
            trip.Status = "CLOSED";
            trip.LastClientOperationId = operationId;
            trip.Version++;
            trip.UpdatedAt = request.ClosedAt;
            await Save(ct);
            var response = await TripResponseOf(context, trip, ct);
            await PersistOutcomeAsync(context, action, operationId, "Trip", tripId, fingerprint, response, ct);
            await audit.WriteAsync(context, "TripClose", "SUCCESS", "Trip", trip.Id,
                null, JsonSerializer.Serialize(new { trip.TripNo, ClosedAt = request.ClosedAt, Departed = departed, Accounted = accounted, ManifestCount = manifests.Count }), null, ct);
            await tx.CommitAsync(ct);
            return response;
        }
        catch (PostgresException ex) when (ex.SqlState == "23505")
        {
            db.ChangeTracker.Clear();
            replay = await TryReplayAsync<TripResponse>(context, action, operationId, "Trip", tripId, fingerprint, ct);
            if (replay is not null) return replay;
            throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT", ex);
        }
        catch (Exception ex) when (IsSerializationFailure(ex))
        {
            throw new WaybillPersistenceException("CONCURRENCY_CONFLICT", ex);
        }
    }

    public async Task<WaybillMovementResponse> GetWaybillMovementAsync(
        OperationContext context, Guid waybillId, MovementQueryRequest request, CancellationToken ct)
    {
        await EnsureWaybillVisible(context, waybillId, ct);
        var query = Movements.AsNoTracking().Where(x => x.CompanyId == context.CompanyId && x.WaybillId == waybillId);
        if (request.From.HasValue) query = query.Where(x => x.OccurredAt >= request.From.Value);
        if (request.To.HasValue) query = query.Where(x => x.OccurredAt < request.To.Value);
        var rows = await query.OrderBy(x => x.OccurredAt).ThenBy(x => x.RecordedAt).ThenBy(x => x.Id).ToListAsync(ct);
        return new WaybillMovementResponse(waybillId, rows.Select(MovementResponseOf).ToList(), context.CorrelationId);
    }

    public async Task<ItemMovementResponse> GetItemMovementAsync(
        OperationContext context, Guid waybillId, Guid itemId, MovementQueryRequest request, CancellationToken ct)
    {
        await EnsureWaybillVisible(context, waybillId, ct);
        var item = await Items.AsNoTracking().SingleOrDefaultAsync(x => x.Id == itemId && x.WaybillId == waybillId, ct)
            ?? throw new WaybillPersistenceException("NOT_FOUND");
        var query = Movements.AsNoTracking().Where(x =>
            x.CompanyId == context.CompanyId && x.WaybillId == waybillId && x.WaybillItemId == itemId);
        if (request.From.HasValue) query = query.Where(x => x.OccurredAt >= request.From.Value);
        if (request.To.HasValue) query = query.Where(x => x.OccurredAt < request.To.Value);
        var timeline = await query.OrderBy(x => x.OccurredAt).ThenBy(x => x.RecordedAt).ThenBy(x => x.Id).ToListAsync(ct);

        var releaseRows = await Releases.AsNoTracking().Where(x => x.CompanyId == context.CompanyId && x.WaybillItemId == itemId).ToListAsync(ct);
        var released = releaseRows.Sum(x => x.Status == "ACTIVE" ? x.Quantity : -x.Quantity);
        var allocationRows = await Allocations.AsNoTracking().Where(x =>
            x.CompanyId == context.CompanyId && x.WaybillItemId == itemId &&
            (x.Reason == null || !x.Reason.StartsWith("TRANSIT_REALLOCATION:"))).ToListAsync(ct);
        var allocated = allocationRows.Sum(x => x.Status == "ALLOCATED" ? x.Quantity : -x.Quantity);
        var depart = await Movements.AsNoTracking().Where(x => x.CompanyId == context.CompanyId && x.WaybillItemId == itemId && x.EventType == "DEPART").SumAsync(x => x.Quantity ?? 0m, ct);
        var unload = await Movements.AsNoTracking().Where(x => x.CompanyId == context.CompanyId && x.WaybillItemId == itemId && x.EventType == "UNLOAD").SumAsync(x => x.Quantity ?? 0m, ct);
        var inTransit = Math.Max(0m, depart - unload);
        var arrived = await Holdings.AsNoTracking().Where(x =>
                x.CompanyId == context.CompanyId && x.WaybillItemId == itemId && x.HoldingType == "DESTINATION" &&
                (x.Status == "AVAILABLE" || x.Status == "EXCEPTION"))
            .SumAsync(x => x.Quantity, ct);
        var transitHolding = await Holdings.AsNoTracking().Where(x =>
                x.CompanyId == context.CompanyId && x.WaybillItemId == itemId && x.HoldingType == "TRANSIT" &&
                (x.Status == "AVAILABLE" || x.Status == "EXCEPTION" || x.Status == "RESERVED"))
            .SumAsync(x => x.Quantity, ct);
        const decimal delivered = 0m;
        var remaining = Math.Max(0m, item.Quantity - inTransit - transitHolding - arrived - delivered);

        return new ItemMovementResponse(waybillId, itemId, item.Quantity, released, allocated, inTransit,
            arrived, delivered, remaining, timeline.Select(MovementResponseOf).ToList(), context.CorrelationId);
    }

    private async Task<ArrivalReceiptEntity> RequireReceipt(OperationContext context, Guid id, CancellationToken ct)
        => await Receipts.SingleOrDefaultAsync(x =>
               x.Id == id && x.CompanyId == context.CompanyId && x.ReceivingBranchId == context.BranchId, ct)
           ?? throw new WaybillPersistenceException("NOT_FOUND");

    private async Task EnsureActiveBranch(OperationContext context, CancellationToken ct)
    {
        if (!await db.Set<Branch>().AsNoTracking().AnyAsync(x =>
            x.Id == context.BranchId && x.CompanyId == context.CompanyId && x.Status == "ACTIVE", ct))
            throw new WaybillPersistenceException("SCOPE_DENIED");
    }

    private async Task EnsureWaybillVisible(OperationContext context, Guid waybillId, CancellationToken ct)
    {
        var visible = await Waybills.AsNoTracking().AnyAsync(x =>
            x.Id == waybillId && x.CompanyId == context.CompanyId &&
            (x.BranchId == context.BranchId || ReceiptLines.Any(line =>
                line.WaybillItem!.WaybillId == waybillId &&
                line.ArrivalReceipt!.CompanyId == context.CompanyId &&
                line.ArrivalReceipt.ReceivingBranchId == context.BranchId)), ct);
        if (!visible) throw new WaybillPersistenceException("NOT_FOUND");
    }

    private async Task<ArrivalReceiptResponse> ReceiptResponseOf(OperationContext context, Guid id, CancellationToken ct)
    {
        var receipt = await Receipts.AsNoTracking().SingleAsync(x =>
            x.Id == id && x.CompanyId == context.CompanyId && x.ReceivingBranchId == context.BranchId, ct);
        var lines = await ReceiptLines.AsNoTracking().Where(x => x.ArrivalReceiptId == id).OrderBy(x => x.Id)
            .Select(x => new ArrivalReceiptLineResponse(x.Id, x.ManifestLineId, x.WaybillItemId,
                x.ExpectedQty, x.ActualQty, x.DifferenceType, x.DamageQty, x.EvidenceAttachmentId, x.Notes))
            .ToListAsync(ct);
        return new ArrivalReceiptResponse(receipt.Id, receipt.TripId, receipt.ManifestId, receipt.LocationId,
            receipt.ReceivingBranchId, receipt.ReceivedAt, receipt.ReceivedBy, receipt.Status, receipt.Version,
            lines, context.CorrelationId);
    }

    private async Task<TripResponse> TripResponseOf(OperationContext context, TripEntity trip, CancellationToken ct)
    {
        var stops = await Stops.AsNoTracking().Where(x => x.TripId == trip.Id).OrderBy(x => x.StopNo)
            .Select(x => new TripStopResponse(x.Id, x.StopNo, x.LocationId, x.StopType, x.PlannedAt, x.Status))
            .ToListAsync(ct);
        return new TripResponse(trip.Id, trip.CompanyId, trip.BranchId, trip.TripNo, trip.VehicleId, trip.DriverId,
            trip.OriginId, trip.DestinationId, trip.PlannedDepartAt, trip.ActualDepartAt, trip.Status, trip.Version,
            stops, context.CorrelationId);
    }

    private static WarehouseHoldingEntity NewHolding(OperationContext context, Guid itemId, Guid locationId,
        decimal quantity, string type, string status, string operationId, DateTimeOffset now)
        => new()
        {
            Id = Guid.NewGuid(), CompanyId = context.CompanyId, BranchId = context.BranchId,
            WaybillItemId = itemId, LocationId = locationId, Quantity = quantity,
            HoldingType = type, Status = status, SourceClientOperationId = operationId,
            CreatedAt = now, UpdatedAt = now, Version = 1
        };

    private static MovementEventResponse MovementResponseOf(MovementEventEntity x)
        => new(x.Id, x.WaybillId, x.WaybillItemId, x.EventType, x.Quantity, x.TripId, x.ManifestId,
            x.FromLocationId, x.ToLocationId, x.OccurredAt, x.RecordedAt, x.RecordedBy, x.ReasonCode);

    private static string HoldingReason(Guid? available, Guid? damage, string difference)
    {
        var parts = new List<string>();
        if (available.HasValue) parts.Add($"H:{available.Value:N}");
        if (damage.HasValue) parts.Add($"D:{damage.Value:N}");
        parts.Add(difference);
        var value = string.Join(';', parts);
        return value.Length <= 80 ? value : value[..80];
    }

    private async Task<T?> TryReplayAsync<T>(OperationContext context, string action, string operationId,
        string aggregateType, Guid aggregateId, string requestFingerprint, CancellationToken ct) where T : class
    {
        var outcome = await ReadOutcomeAsync(context, action, operationId, ct);
        if (outcome is null) return null;
        if (!string.Equals(outcome.AggregateType, aggregateType, StringComparison.Ordinal) ||
            outcome.AggregateId != aggregateId || outcome.RequestFingerprint != requestFingerprint)
            throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT");
        return JsonSerializer.Deserialize<T>(outcome.ResponseJson)
            ?? throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT");
    }

    private async Task<CommandOutcome?> ReadOutcomeAsync(OperationContext context, string action, string operationId, CancellationToken ct)
    {
        var connection = db.Database.GetDbConnection();
        var close = connection.State != ConnectionState.Open;
        if (close) await connection.OpenAsync(ct);
        try
        {
            await using var command = connection.CreateCommand();
            command.Transaction = db.Database.CurrentTransaction?.GetDbTransaction();
            command.CommandText = """
                SELECT "AggregateType", "AggregateId", "RequestFingerprint", "ResponseJson"
                FROM transport_erp.shipping_command_outcomes
                WHERE "CompanyId"=@company AND "BranchId"=@branch AND "Action"=@action AND "ClientOperationId"=@operation
                """;
            command.Parameters.Add(new NpgsqlParameter("company", context.CompanyId));
            command.Parameters.Add(new NpgsqlParameter("branch", context.BranchId));
            command.Parameters.Add(new NpgsqlParameter("action", action));
            command.Parameters.Add(new NpgsqlParameter("operation", operationId));
            await using var reader = await command.ExecuteReaderAsync(ct);
            if (!await reader.ReadAsync(ct)) return null;
            return new CommandOutcome(reader.GetString(0), reader.GetGuid(1), reader.GetString(2), reader.GetString(3));
        }
        finally { if (close) await connection.CloseAsync(); }
    }

    private async Task PersistOutcomeAsync<T>(OperationContext context, string action, string operationId,
        string aggregateType, Guid aggregateId, string fingerprint, T response, CancellationToken ct)
    {
        var tx = db.Database.CurrentTransaction ?? throw new InvalidOperationException("Outcome requires transaction.");
        var connection = db.Database.GetDbConnection();
        await using var command = connection.CreateCommand();
        command.Transaction = tx.GetDbTransaction();
        command.CommandText = """
            INSERT INTO transport_erp.shipping_command_outcomes
            ("Id","CompanyId","BranchId","Action","ClientOperationId","AggregateType","AggregateId","RequestFingerprint","ResponseJson","CreatedAt")
            VALUES (@id,@company,@branch,@action,@operation,@type,@aggregate,@fingerprint,@response,@created)
            """;
        command.Parameters.Add(new NpgsqlParameter("id", Guid.NewGuid()));
        command.Parameters.Add(new NpgsqlParameter("company", context.CompanyId));
        command.Parameters.Add(new NpgsqlParameter("branch", context.BranchId));
        command.Parameters.Add(new NpgsqlParameter("action", action));
        command.Parameters.Add(new NpgsqlParameter("operation", operationId));
        command.Parameters.Add(new NpgsqlParameter("type", aggregateType));
        command.Parameters.Add(new NpgsqlParameter("aggregate", aggregateId));
        command.Parameters.Add(new NpgsqlParameter("fingerprint", fingerprint));
        command.Parameters.Add(new NpgsqlParameter("response", JsonSerializer.Serialize(response)));
        command.Parameters.Add(new NpgsqlParameter("created", DateTimeOffset.UtcNow));
        await command.ExecuteNonQueryAsync(ct);
    }

    private static string Fingerprint(string action, object payload)
    {
        var json = JsonSerializer.Serialize(new { action, payload });
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(json))).ToLowerInvariant();
    }

    private static string ScopedMovementId(Guid branchId, string operationId, Guid sourceId, string kind)
    {
        var raw = $"{branchId:N}:{operationId}:{sourceId:N}:{kind}";
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(raw))).ToLowerInvariant();
        return $"d:{branchId:N}:{hash}";
    }

    private async Task Save(CancellationToken ct)
    {
        try { await db.SaveChangesAsync(ct); }
        catch (DbUpdateConcurrencyException ex) { throw new WaybillPersistenceException("CONCURRENCY_CONFLICT", ex); }
    }

    private static bool IsUniqueViolation(Exception ex)
    {
        for (var current = ex; current is not null; current = current.InnerException)
            if (current is PostgresException { SqlState: "23505" }) return true;
        return false;
    }

    private static bool IsSerializationFailure(Exception ex)
    {
        for (var current = ex; current is not null; current = current.InnerException)
            if (current is PostgresException { SqlState: "40001" or "40P01" }) return true;
        return false;
    }

    private sealed record CommandOutcome(string AggregateType, Guid AggregateId, string RequestFingerprint, string ResponseJson);
}
