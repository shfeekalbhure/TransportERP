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

public sealed class EfShippingExecutionStore(TransportErpDbContext db, IWaybillAuditSink audit) : IShippingExecutionStore
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

    public async Task<ItemQuantityStateResponse> ReleaseItemAsync(
        OperationContext context,
        Guid waybillId,
        Guid itemId,
        ReleaseItemRequest request,
        CancellationToken cancellationToken)
    {
        var operationId = request.ClientOperationId.Trim();
        var replay = await Releases.AsNoTracking().SingleOrDefaultAsync(x =>
            x.CompanyId == context.CompanyId && x.BranchId == context.BranchId && x.ClientOperationId == operationId,
            cancellationToken);
        if (replay is not null)
        {
            if (replay.WaybillItemId != itemId || replay.Quantity != request.Quantity)
                throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT");
            return await ItemState(context, waybillId, itemId, cancellationToken);
        }

        try
        {
            await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            var scoped = await RequireItem(context, waybillId, itemId, cancellationToken);
            if (scoped.Waybill.Status != "APPROVED")
                throw new WaybillPersistenceException("INVALID_STATE");
            await EnsureNoActiveHold(context, scoped.Waybill.Id, cancellationToken);

            var releasedNet = await ReleasedNet(context, itemId, cancellationToken);
            ShippingExecutionRules.EnsureRelease(scoped.Item.Quantity, releasedNet, request.Quantity);

            var entity = new ItemReleaseEntity
            {
                Id = Guid.NewGuid(),
                CompanyId = context.CompanyId,
                BranchId = context.BranchId,
                WaybillItemId = itemId,
                Quantity = request.Quantity,
                ReleasedAt = request.ReleasedAt,
                ReleasedBy = context.UserId,
                ClientOperationId = operationId,
                Status = ShippingExecutionStatuses.Release.Active
            };
            Releases.Add(entity);
            await Save(cancellationToken);
            await audit.WriteAsync(context, "WaybillItemRelease", "SUCCESS", "ItemRelease", entity.Id,
                null, JsonSerializer.Serialize(new { waybillId, itemId, entity.Quantity, entity.ReleasedAt }), null, cancellationToken);
            await tx.CommitAsync(cancellationToken);
            return new ItemQuantityStateResponse(waybillId, itemId, scoped.Item.Quantity,
                releasedNet + request.Quantity, scoped.Item.Quantity - releasedNet - request.Quantity, context.CorrelationId);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            db.ChangeTracker.Clear();
            replay = await Releases.AsNoTracking().SingleOrDefaultAsync(x =>
                x.CompanyId == context.CompanyId && x.BranchId == context.BranchId && x.ClientOperationId == operationId,
                cancellationToken);
            if (replay is not null && replay.WaybillItemId == itemId && replay.Quantity == request.Quantity)
                return await ItemState(context, waybillId, itemId, cancellationToken);
            throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT", ex);
        }
        catch (Exception ex) when (IsSerializationFailure(ex))
        {
            throw new WaybillPersistenceException("CONCURRENCY_CONFLICT", ex);
        }
    }

    public async Task<TripResponse> CreateTripAsync(
        OperationContext context,
        CreateTripRequest request,
        CancellationToken cancellationToken)
    {
        var operationId = request.ClientOperationId.Trim();
        var replay = await Trips.AsNoTracking().SingleOrDefaultAsync(x =>
            x.CompanyId == context.CompanyId && x.BranchId == context.BranchId && x.CreateClientOperationId == operationId,
            cancellationToken);
        if (replay is not null)
        {
            await EnsureTripReplayAsync(replay, request, cancellationToken);
            return await TripResponseOf(context, replay.Id, cancellationToken);
        }

        try
        {
            await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            if (await Trips.AsNoTracking().AnyAsync(x =>
                x.CompanyId == context.CompanyId && x.TripNo == request.TripNo.Trim(), cancellationToken))
                throw new WaybillPersistenceException("DUPLICATE_TRIP_NO");

            var now = DateTimeOffset.UtcNow;
            var trip = new TripEntity
            {
                Id = Guid.NewGuid(),
                CompanyId = context.CompanyId,
                BranchId = context.BranchId,
                TripNo = request.TripNo.Trim(),
                VehicleId = request.VehicleId,
                DriverId = request.DriverId,
                OriginId = request.OriginId,
                DestinationId = request.DestinationId,
                PlannedDepartAt = request.PlannedDepartAt,
                Status = ShippingExecutionStatuses.Trip.Draft,
                CreateClientOperationId = operationId,
                LastClientOperationId = operationId,
                CreatedAt = now,
                UpdatedAt = now,
                Version = 1
            };
            Trips.Add(trip);
            foreach (var input in request.Stops ?? [])
            {
                Stops.Add(new TripStopEntity
                {
                    Id = Guid.NewGuid(),
                    TripId = trip.Id,
                    StopNo = input.StopNo,
                    LocationId = input.LocationId,
                    StopType = input.StopType.Trim().ToUpperInvariant(),
                    PlannedAt = input.PlannedAt,
                    Status = "PLANNED"
                });
            }

            await Save(cancellationToken);
            await audit.WriteAsync(context, "TripCreate", "SUCCESS", "Trip", trip.Id,
                null, JsonSerializer.Serialize(new
                {
                    trip.TripNo, trip.VehicleId, trip.DriverId, trip.OriginId, trip.DestinationId,
                    trip.PlannedDepartAt, Stops = (request.Stops ?? []).Select(x => new { x.StopNo, x.LocationId, x.StopType, x.PlannedAt })
                }), null, cancellationToken);
            await tx.CommitAsync(cancellationToken);
            return await TripResponseOf(context, trip.Id, cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            db.ChangeTracker.Clear();
            replay = await Trips.AsNoTracking().SingleOrDefaultAsync(x =>
                x.CompanyId == context.CompanyId && x.BranchId == context.BranchId && x.CreateClientOperationId == operationId,
                cancellationToken);
            if (replay is not null)
            {
                await EnsureTripReplayAsync(replay, request, cancellationToken);
                return await TripResponseOf(context, replay.Id, cancellationToken);
            }
            throw new WaybillPersistenceException("DUPLICATE_TRIP_NO", ex);
        }
        catch (Exception ex) when (IsSerializationFailure(ex))
        {
            throw new WaybillPersistenceException("CONCURRENCY_CONFLICT", ex);
        }
    }

    public async Task<AllocationResponse> AllocateAsync(
        OperationContext context,
        Guid tripId,
        AllocateItemRequest request,
        CancellationToken cancellationToken)
    {
        var operationId = request.ClientOperationId.Trim();
        var replay = await Allocations.AsNoTracking().SingleOrDefaultAsync(x =>
            x.CompanyId == context.CompanyId && x.BranchId == context.BranchId && x.ClientOperationId == operationId,
            cancellationToken);
        if (replay is not null)
        {
            if (replay.TripId != tripId || replay.WaybillItemId != request.WaybillItemId ||
                replay.ReleaseId != request.ReleaseId || replay.Quantity != request.Quantity || replay.ReversalOfId is not null)
                throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT");
            return AllocationResponseOf(context, replay);
        }

        try
        {
            await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            var trip = await RequireTrip(context, tripId, cancellationToken);
            if (trip.Status != ShippingExecutionStatuses.Trip.Draft)
                throw new WaybillPersistenceException("INVALID_STATE");

            var release = await Releases.AsNoTracking().SingleOrDefaultAsync(x =>
                x.Id == request.ReleaseId && x.CompanyId == context.CompanyId && x.BranchId == context.BranchId &&
                x.WaybillItemId == request.WaybillItemId && x.Status == ShippingExecutionStatuses.Release.Active,
                cancellationToken) ?? throw new WaybillPersistenceException("NOT_FOUND");

            var scoped = await RequireItem(context, null, request.WaybillItemId, cancellationToken);
            if (scoped.Waybill.Status != "APPROVED")
                throw new WaybillPersistenceException("INVALID_STATE");
            await EnsureNoActiveHold(context, scoped.Waybill.Id, cancellationToken);

            var stops = await Stops.AsNoTracking()
                .Where(x => x.TripId == tripId &&
                            x.Trip!.CompanyId == context.CompanyId &&
                            x.Trip.BranchId == context.BranchId)
                .OrderBy(x => x.StopNo)
                .Select(x => x.LocationId).ToListAsync(cancellationToken);
            ShippingExecutionRules.EnsureRouteCompatible(
                scoped.Waybill.OriginId, scoped.Waybill.DestinationId,
                trip.OriginId, trip.DestinationId, stops);

            var allocatedNet = await AllocationNet(context, request.ReleaseId, cancellationToken);
            ShippingExecutionRules.EnsureAllocation(release.Quantity, allocatedNet, request.Quantity);

            var allocation = new TripAllocationEntity
            {
                Id = Guid.NewGuid(), CompanyId = context.CompanyId, BranchId = context.BranchId,
                WaybillItemId = request.WaybillItemId, ReleaseId = request.ReleaseId, TripId = tripId,
                Quantity = request.Quantity, AllocatedAt = DateTimeOffset.UtcNow,
                ClientOperationId = operationId, Status = ShippingExecutionStatuses.Allocation.Allocated
            };
            Allocations.Add(allocation);
            await Save(cancellationToken);
            await audit.WriteAsync(context, "WaybillItemAllocate", "SUCCESS", "TripAllocation", allocation.Id,
                null, JsonSerializer.Serialize(new { allocation.WaybillItemId, allocation.ReleaseId, allocation.TripId, allocation.Quantity }),
                null, cancellationToken);
            await tx.CommitAsync(cancellationToken);
            return AllocationResponseOf(context, allocation);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            db.ChangeTracker.Clear();
            replay = await Allocations.AsNoTracking().SingleOrDefaultAsync(x =>
                x.CompanyId == context.CompanyId && x.BranchId == context.BranchId && x.ClientOperationId == operationId,
                cancellationToken);
            if (replay is not null && replay.TripId == tripId && replay.WaybillItemId == request.WaybillItemId &&
                replay.ReleaseId == request.ReleaseId && replay.Quantity == request.Quantity && replay.ReversalOfId is null)
                return AllocationResponseOf(context, replay);
            throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT", ex);
        }
        catch (Exception ex) when (IsSerializationFailure(ex))
        {
            throw new WaybillPersistenceException("CONCURRENCY_CONFLICT", ex);
        }
    }

    public async Task<AllocationResponse> UnallocateAsync(
        OperationContext context,
        Guid allocationId,
        UnallocateRequest request,
        CancellationToken cancellationToken)
    {
        var operationId = request.ClientOperationId.Trim();
        var replay = await Allocations.AsNoTracking().SingleOrDefaultAsync(x =>
            x.CompanyId == context.CompanyId && x.BranchId == context.BranchId && x.ClientOperationId == operationId,
            cancellationToken);
        if (replay is not null)
        {
            if (replay.ReversalOfId != allocationId ||
                !string.Equals(replay.Reason?.Trim(), request.Reason.Trim(), StringComparison.Ordinal))
                throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT");
            return AllocationResponseOf(context, replay);
        }

        try
        {
            await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            var original = await Allocations.AsNoTracking().SingleOrDefaultAsync(x =>
                x.Id == allocationId && x.CompanyId == context.CompanyId && x.BranchId == context.BranchId &&
                x.Status == ShippingExecutionStatuses.Allocation.Allocated && x.ReversalOfId == null,
                cancellationToken) ?? throw new WaybillPersistenceException("NOT_FOUND");

            if (await Allocations.AsNoTracking().AnyAsync(x =>
                x.CompanyId == context.CompanyId && x.BranchId == context.BranchId && x.ReversalOfId == original.Id,
                cancellationToken))
                throw new WaybillPersistenceException("INVALID_STATE");
            if (await Movements.AsNoTracking().AnyAsync(x =>
                x.CompanyId == context.CompanyId && x.BranchId == context.BranchId &&
                x.AllocationId == original.Id && x.EventType == "LOAD", cancellationToken))
                throw new WaybillPersistenceException("ALREADY_LOADED");
            if (await ManifestLines.AsNoTracking().AnyAsync(x =>
                x.AllocationId == original.Id &&
                x.Manifest!.CompanyId == context.CompanyId && x.Manifest.BranchId == context.BranchId,
                cancellationToken))
                throw new WaybillPersistenceException("INVALID_STATE");

            var reversal = new TripAllocationEntity
            {
                Id = Guid.NewGuid(), CompanyId = original.CompanyId, BranchId = original.BranchId,
                WaybillItemId = original.WaybillItemId, ReleaseId = original.ReleaseId, TripId = original.TripId,
                Quantity = original.Quantity, AllocatedAt = DateTimeOffset.UtcNow, ClientOperationId = operationId,
                Status = ShippingExecutionStatuses.Allocation.Reversed, ReversalOfId = original.Id,
                Reason = request.Reason.Trim()
            };
            Allocations.Add(reversal);
            await Save(cancellationToken);
            await audit.WriteAsync(context, "WaybillItemUnallocate", "SUCCESS", "TripAllocation", reversal.Id,
                JsonSerializer.Serialize(new { original.Id, original.Quantity }),
                JsonSerializer.Serialize(new { reversal.Id, reversal.ReversalOfId, reversal.Quantity }),
                request.Reason.Trim(), cancellationToken);
            await tx.CommitAsync(cancellationToken);
            return AllocationResponseOf(context, reversal);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            db.ChangeTracker.Clear();
            replay = await Allocations.AsNoTracking().SingleOrDefaultAsync(x =>
                x.CompanyId == context.CompanyId && x.BranchId == context.BranchId && x.ClientOperationId == operationId,
                cancellationToken);
            if (replay is not null && replay.ReversalOfId == allocationId &&
                string.Equals(replay.Reason?.Trim(), request.Reason.Trim(), StringComparison.Ordinal))
                return AllocationResponseOf(context, replay);
            throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT", ex);
        }
        catch (Exception ex) when (IsSerializationFailure(ex))
        {
            throw new WaybillPersistenceException("CONCURRENCY_CONFLICT", ex);
        }
    }

    public async Task<ManifestResponse> GenerateManifestAsync(
        OperationContext context,
        Guid tripId,
        GenerateManifestRequest request,
        CancellationToken cancellationToken)
    {
        var operationId = request.ClientOperationId.Trim();
        var replay = await Manifests.AsNoTracking().SingleOrDefaultAsync(x =>
            x.CompanyId == context.CompanyId && x.BranchId == context.BranchId &&
            x.CreateClientOperationId == operationId,
            cancellationToken);
        if (replay is not null)
        {
            if (!ManifestCreateReplayMatches(replay, tripId, request))
                throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT");
            return await ManifestResponseOf(context, replay.Id, cancellationToken);
        }

        try
        {
            await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            var trip = await RequireTrip(context, tripId, cancellationToken);
            if (trip.Status != ShippingExecutionStatuses.Trip.Draft)
                throw new WaybillPersistenceException("INVALID_STATE");

            var candidates = await (
                from allocation in Allocations.AsNoTracking()
                join item in Items.AsNoTracking() on allocation.WaybillItemId equals item.Id
                join waybill in Waybills.AsNoTracking() on item.WaybillId equals waybill.Id
                where allocation.TripId == tripId &&
                      allocation.CompanyId == context.CompanyId &&
                      allocation.BranchId == context.BranchId &&
                      waybill.CompanyId == context.CompanyId &&
                      waybill.BranchId == context.BranchId &&
                      allocation.Status == ShippingExecutionStatuses.Allocation.Allocated &&
                      allocation.ReversalOfId == null &&
                      !Allocations.Any(r =>
                          r.ReversalOfId == allocation.Id &&
                          r.CompanyId == context.CompanyId &&
                          r.BranchId == context.BranchId) &&
                      !ManifestLines.Any(l =>
                          l.AllocationId == allocation.Id &&
                          l.Manifest!.CompanyId == context.CompanyId &&
                          l.Manifest.BranchId == context.BranchId)
                orderby waybill.Id, item.LineNo
                select new { Allocation = allocation, Item = item, Waybill = waybill })
                .ToListAsync(cancellationToken);

            if (candidates.Count == 0)
                throw new WaybillPersistenceException("NO_ALLOCATIONS");

            var now = DateTimeOffset.UtcNow;
            var autoManifestNo = $"MF-{trip.TripNo}-{Guid.NewGuid():N}";
            var manifestNo = string.IsNullOrWhiteSpace(request.ManifestNo)
                ? autoManifestNo[..Math.Min(100, autoManifestNo.Length)]
                : request.ManifestNo.Trim();
            var manifest = new ManifestEntity
            {
                Id = Guid.NewGuid(), CompanyId = context.CompanyId, BranchId = context.BranchId,
                TripId = tripId, ManifestNo = manifestNo, Status = ShippingExecutionStatuses.Manifest.Draft,
                CreateClientOperationId = operationId, LastClientOperationId = operationId,
                CreatedAt = now, UpdatedAt = now, Version = 1
            };
            Manifests.Add(manifest);

            foreach (var candidate in candidates)
            {
                var (weight, volume) = ShippingExecutionRules.AllocatePhysicalMeasures(
                    candidate.Item.Quantity,
                    candidate.Allocation.Quantity,
                    candidate.Item.Weight,
                    candidate.Item.Length,
                    candidate.Item.Width,
                    candidate.Item.Height,
                    candidate.Item.Volume);

                ManifestLines.Add(new ManifestLineEntity
                {
                    Id = Guid.NewGuid(), ManifestId = manifest.Id, AllocationId = candidate.Allocation.Id,
                    WaybillId = candidate.Waybill.Id, WaybillItemId = candidate.Item.Id,
                    Quantity = candidate.Allocation.Quantity, LoadedQuantity = 0m,
                    Weight = weight, Volume = volume,
                    LoadStatus = ShippingExecutionStatuses.Load.Planned
                });
            }

            await Save(cancellationToken);
            await audit.WriteAsync(context, "ManifestGenerate", "SUCCESS", "Manifest", manifest.Id,
                null, JsonSerializer.Serialize(new
                {
                    manifest.TripId,
                    manifest.ManifestNo,
                    LineCount = candidates.Count,
                    SourceAllocations = candidates.Select(x => new
                    {
                        x.Allocation.Id,
                        x.Allocation.WaybillItemId,
                        x.Allocation.Quantity
                    }).OrderBy(x => x.Id).ToArray()
                }), null, cancellationToken);
            await tx.CommitAsync(cancellationToken);
            return await ManifestResponseOf(context, manifest.Id, cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            db.ChangeTracker.Clear();
            replay = await Manifests.AsNoTracking().SingleOrDefaultAsync(x =>
                x.CompanyId == context.CompanyId && x.BranchId == context.BranchId &&
                x.CreateClientOperationId == operationId,
                cancellationToken);
            if (replay is not null && ManifestCreateReplayMatches(replay, tripId, request))
                return await ManifestResponseOf(context, replay.Id, cancellationToken);
            throw new WaybillPersistenceException("DUPLICATE_OPERATION", ex);
        }
        catch (Exception ex) when (IsSerializationFailure(ex))
        {
            throw new WaybillPersistenceException("CONCURRENCY_CONFLICT", ex);
        }
    }

    public async Task<ManifestLineResponse> LoadManifestLineAsync(
        OperationContext context,
        Guid manifestId,
        Guid lineId,
        LoadManifestLineRequest request,
        CancellationToken cancellationToken)
    {
        var operationId = request.ClientOperationId.Trim();
        var storedOperationId = ScopedMovementOperationId(context.BranchId, operationId);
        var replay = await Movements.AsNoTracking().SingleOrDefaultAsync(x =>
            x.CompanyId == context.CompanyId && x.BranchId == context.BranchId &&
            x.ClientOperationId == storedOperationId && x.EventType == "LOAD",
            cancellationToken);
        if (replay is not null)
        {
            if (!LoadReplayMatches(replay, manifestId, lineId, request))
                throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT");
            return await ManifestLineResponseOf(context, lineId, cancellationToken);
        }

        try
        {
            await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            var manifest = await RequireManifest(context, manifestId, cancellationToken);
            if (manifest.Status != ShippingExecutionStatuses.Manifest.Draft)
                throw new WaybillPersistenceException("INVALID_STATE");

            var line = await ManifestLines.SingleOrDefaultAsync(x =>
                    x.Id == lineId && x.ManifestId == manifestId &&
                    x.Manifest!.CompanyId == context.CompanyId &&
                    x.Manifest.BranchId == context.BranchId,
                    cancellationToken)
                ?? throw new WaybillPersistenceException("NOT_FOUND");

            var item = await Items.AsNoTracking().SingleOrDefaultAsync(x =>
                    x.Id == line.WaybillItemId &&
                    x.Waybill!.CompanyId == context.CompanyId &&
                    x.Waybill.BranchId == context.BranchId,
                    cancellationToken)
                ?? throw new WaybillPersistenceException("NOT_FOUND");

            _ = await RequireActiveAllocationForLine(context, manifest, line, cancellationToken);
            await EnsureNoActiveHold(context, line.WaybillId, cancellationToken);
            ShippingExecutionRules.EnsureResourceConstraint(item.RiskFlagsJson, request.ResourceConstraintConfirmed);

            var loadedNet = await Movements.AsNoTracking().Where(x =>
                    x.ManifestLineId == line.Id && x.EventType == "LOAD" &&
                    x.CompanyId == context.CompanyId && x.BranchId == context.BranchId)
                .SumAsync(x => x.Quantity ?? 0m, cancellationToken);
            ShippingExecutionRules.EnsureLoad(line.Quantity, loadedNet, request.Quantity);

            var movement = new MovementEventEntity
            {
                Id = Guid.NewGuid(), CompanyId = context.CompanyId, BranchId = context.BranchId,
                WaybillId = line.WaybillId, WaybillItemId = line.WaybillItemId,
                AllocationId = line.AllocationId, ManifestLineId = line.Id,
                EventType = "LOAD", Quantity = request.Quantity, TripId = manifest.TripId, ManifestId = manifest.Id,
                OccurredAt = request.OccurredAt, RecordedAt = DateTimeOffset.UtcNow, RecordedBy = context.UserId,
                ReasonCode = ResourceConstraintEvidence(request.ResourceConstraintConfirmed),
                ClientOperationId = storedOperationId
            };
            Movements.Add(movement);
            line.LoadedQuantity = loadedNet + request.Quantity;
            line.LoadStatus = Math.Abs(line.LoadedQuantity - line.Quantity) <= 0.0001m
                ? ShippingExecutionStatuses.Load.Loaded
                : ShippingExecutionStatuses.Load.Partial;
            await Save(cancellationToken);
            await audit.WriteAsync(context, "ManifestLineLoad", "SUCCESS", "MovementEvent", movement.Id,
                null, JsonSerializer.Serialize(new
                {
                    movement.ManifestId, movement.ManifestLineId, movement.AllocationId,
                    movement.WaybillItemId, movement.Quantity, movement.OccurredAt,
                    request.ResourceConstraintConfirmed
                }), null, cancellationToken);
            await tx.CommitAsync(cancellationToken);
            return ManifestLineResponseOf(line);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            db.ChangeTracker.Clear();
            replay = await Movements.AsNoTracking().SingleOrDefaultAsync(x =>
                x.CompanyId == context.CompanyId && x.BranchId == context.BranchId &&
                x.ClientOperationId == storedOperationId && x.EventType == "LOAD",
                cancellationToken);
            if (replay is not null && LoadReplayMatches(replay, manifestId, lineId, request))
                return await ManifestLineResponseOf(context, lineId, cancellationToken);
            throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT", ex);
        }
        catch (Exception ex) when (IsSerializationFailure(ex))
        {
            throw new WaybillPersistenceException("CONCURRENCY_CONFLICT", ex);
        }
    }

    public async Task<ManifestResponse> FinalizeManifestAsync(
        OperationContext context,
        Guid manifestId,
        FinalizeManifestRequest request,
        CancellationToken cancellationToken)
    {
        const string action = "MANIFEST_FINALIZE";
        var operationId = request.ClientOperationId.Trim();
        var fingerprint = CommandFingerprint(action, new { manifestId, request.ExpectedVersion });
        var replay = await TryReplayCommandAsync<ManifestResponse>(
            context, action, operationId, "Manifest", manifestId, fingerprint, cancellationToken);
        if (replay is not null) return replay;

        try
        {
            await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            replay = await TryReplayCommandAsync<ManifestResponse>(
                context, action, operationId, "Manifest", manifestId, fingerprint, cancellationToken);
            if (replay is not null)
            {
                await tx.CommitAsync(cancellationToken);
                return replay;
            }

            var manifest = await RequireManifest(context, manifestId, cancellationToken);
            if (manifest.Version != request.ExpectedVersion)
                throw new WaybillPersistenceException("CONCURRENCY_CONFLICT");
            if (manifest.Status != ShippingExecutionStatuses.Manifest.Draft)
                throw new WaybillPersistenceException("INVALID_STATE");

            var lines = await ManifestLines.Where(x =>
                    x.ManifestId == manifestId &&
                    x.Manifest!.CompanyId == context.CompanyId &&
                    x.Manifest.BranchId == context.BranchId)
                .ToListAsync(cancellationToken);
            foreach (var waybillId in lines.Select(x => x.WaybillId).Distinct())
                await EnsureNoActiveHold(context, waybillId, cancellationToken);
            ShippingExecutionRules.EnsureManifestCanFinalize(
                lines.Select(x => (x.Quantity, x.LoadedQuantity)).ToList());

            var activeAllocations = await ActiveTripAllocations(context, manifest.TripId, cancellationToken);
            ShippingExecutionRules.EnsureManifestAllocationCoverage(
                lines.Select(x => (x.AllocationId, x.WaybillItemId, x.Quantity)).ToList(),
                activeAllocations.Select(x => (x.Id, x.WaybillItemId, x.Quantity)).ToList());

            var trip = await RequireTrip(context, manifest.TripId, cancellationToken);
            if (trip.Status != ShippingExecutionStatuses.Trip.Draft)
                throw new WaybillPersistenceException("INVALID_STATE");

            var now = DateTimeOffset.UtcNow;
            manifest.Status = ShippingExecutionStatuses.Manifest.Finalized;
            manifest.LastClientOperationId = operationId;
            manifest.Version++;
            manifest.UpdatedAt = now;
            trip.Status = ShippingExecutionStatuses.Trip.Ready;
            trip.LastClientOperationId = operationId;
            trip.Version++;
            trip.UpdatedAt = now;
            await Save(cancellationToken);

            var response = await ManifestResponseOf(context, manifest.Id, cancellationToken);
            await PersistCommandOutcomeAsync(
                context, action, operationId, "Manifest", manifest.Id, fingerprint, response, cancellationToken);
            await audit.WriteAsync(context, "ManifestFinalize", "SUCCESS", "Manifest", manifest.Id,
                null, JsonSerializer.Serialize(new
                {
                    manifest.ManifestNo,
                    LineCount = lines.Count,
                    FinalTotals = new
                    {
                        Quantity = lines.Sum(x => x.Quantity),
                        LoadedQuantity = lines.Sum(x => x.LoadedQuantity),
                        Weight = lines.Sum(x => x.Weight),
                        Volume = lines.Sum(x => x.Volume)
                    },
                    SourceAllocations = lines.OrderBy(x => x.AllocationId).Select(x => new
                    {
                        x.AllocationId, x.WaybillId, x.WaybillItemId, x.Quantity
                    }).ToArray(),
                    trip.Status
                }), null, cancellationToken);
            await tx.CommitAsync(cancellationToken);
            return response;
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            db.ChangeTracker.Clear();
            replay = await TryReplayCommandAsync<ManifestResponse>(
                context, action, operationId, "Manifest", manifestId, fingerprint, cancellationToken);
            if (replay is not null) return replay;
            throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT", ex);
        }
        catch (PostgresException ex) when (ex.SqlState == "23505")
        {
            db.ChangeTracker.Clear();
            replay = await TryReplayCommandAsync<ManifestResponse>(
                context, action, operationId, "Manifest", manifestId, fingerprint, cancellationToken);
            if (replay is not null) return replay;
            throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT", ex);
        }
        catch (Exception ex) when (IsSerializationFailure(ex))
        {
            throw new WaybillPersistenceException("CONCURRENCY_CONFLICT", ex);
        }
    }

    public async Task<ManifestResponse> HandoverManifestAsync(
        OperationContext context,
        Guid manifestId,
        HandoverManifestRequest request,
        CancellationToken cancellationToken)
    {
        const string action = "MANIFEST_HANDOVER";
        var operationId = request.ClientOperationId.Trim();
        var fingerprint = CommandFingerprint(action,
            new { manifestId, request.DriverId, request.AcceptedAt, request.ExpectedVersion });
        var replay = await TryReplayCommandAsync<ManifestResponse>(
            context, action, operationId, "Manifest", manifestId, fingerprint, cancellationToken);
        if (replay is not null) return replay;

        try
        {
            await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            replay = await TryReplayCommandAsync<ManifestResponse>(
                context, action, operationId, "Manifest", manifestId, fingerprint, cancellationToken);
            if (replay is not null)
            {
                await tx.CommitAsync(cancellationToken);
                return replay;
            }

            var manifest = await RequireManifest(context, manifestId, cancellationToken);
            if (manifest.Version != request.ExpectedVersion)
                throw new WaybillPersistenceException("CONCURRENCY_CONFLICT");
            if (manifest.Status != ShippingExecutionStatuses.Manifest.Finalized)
                throw new WaybillPersistenceException("INVALID_STATE");
            var trip = await RequireTrip(context, manifest.TripId, cancellationToken);
            if (trip.DriverId != request.DriverId)
                throw new WaybillPersistenceException("DRIVER_MISMATCH");

            manifest.HandoverAt = request.AcceptedAt;
            manifest.DriverAcceptedAt = request.AcceptedAt;
            manifest.Status = ShippingExecutionStatuses.Manifest.Accepted;
            manifest.LastClientOperationId = operationId;
            manifest.Version++;
            manifest.UpdatedAt = DateTimeOffset.UtcNow;
            await Save(cancellationToken);

            var response = await ManifestResponseOf(context, manifest.Id, cancellationToken);
            await PersistCommandOutcomeAsync(
                context, action, operationId, "Manifest", manifest.Id, fingerprint, response, cancellationToken);
            await audit.WriteAsync(context, "ManifestHandover", "SUCCESS", "Manifest", manifest.Id,
                null, JsonSerializer.Serialize(new { manifest.TripId, request.DriverId, request.AcceptedAt }),
                null, cancellationToken);
            await tx.CommitAsync(cancellationToken);
            return response;
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            db.ChangeTracker.Clear();
            replay = await TryReplayCommandAsync<ManifestResponse>(
                context, action, operationId, "Manifest", manifestId, fingerprint, cancellationToken);
            if (replay is not null) return replay;
            throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT", ex);
        }
        catch (PostgresException ex) when (ex.SqlState == "23505")
        {
            db.ChangeTracker.Clear();
            replay = await TryReplayCommandAsync<ManifestResponse>(
                context, action, operationId, "Manifest", manifestId, fingerprint, cancellationToken);
            if (replay is not null) return replay;
            throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT", ex);
        }
        catch (Exception ex) when (IsSerializationFailure(ex))
        {
            throw new WaybillPersistenceException("CONCURRENCY_CONFLICT", ex);
        }
    }

    public async Task<TripResponse> StartTripAsync(
        OperationContext context,
        Guid tripId,
        StartTripRequest request,
        CancellationToken cancellationToken)
    {
        const string action = "TRIP_START";
        var operationId = request.ClientOperationId.Trim();
        var fingerprint = CommandFingerprint(action, new { tripId, request.ActualDepartAt, request.ExpectedVersion });
        var replay = await TryReplayCommandAsync<TripResponse>(
            context, action, operationId, "Trip", tripId, fingerprint, cancellationToken);
        if (replay is not null) return replay;

        try
        {
            await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            replay = await TryReplayCommandAsync<TripResponse>(
                context, action, operationId, "Trip", tripId, fingerprint, cancellationToken);
            if (replay is not null)
            {
                await tx.CommitAsync(cancellationToken);
                return replay;
            }

            var trip = await RequireTrip(context, tripId, cancellationToken);
            if (trip.Version != request.ExpectedVersion)
                throw new WaybillPersistenceException("CONCURRENCY_CONFLICT");
            if (trip.Status != ShippingExecutionStatuses.Trip.Ready)
                throw new WaybillPersistenceException("INVALID_STATE");

            var manifests = await Manifests.AsNoTracking().Where(x =>
                    x.TripId == tripId &&
                    x.CompanyId == context.CompanyId &&
                    x.BranchId == context.BranchId)
                .ToListAsync(cancellationToken);
            if (manifests.Count == 0 ||
                manifests.Any(x => x.Status != ShippingExecutionStatuses.Manifest.Accepted))
                throw new WaybillPersistenceException("MANIFEST_NOT_ACCEPTED");

            var manifestIds = manifests.Select(x => x.Id).ToList();
            var lines = await ManifestLines.AsNoTracking().Where(x =>
                    manifestIds.Contains(x.ManifestId) &&
                    x.Manifest!.CompanyId == context.CompanyId &&
                    x.Manifest.BranchId == context.BranchId)
                .ToListAsync(cancellationToken);
            if (lines.Count == 0 ||
                lines.Any(x => x.LoadedQuantity <= 0m ||
                               Math.Abs(x.LoadedQuantity - x.Quantity) > 0.0001m))
                throw new WaybillPersistenceException("MANIFEST_NOT_ACCEPTED");

            var activeAllocations = await ActiveTripAllocations(context, tripId, cancellationToken);
            try
            {
                ShippingExecutionRules.EnsureManifestAllocationCoverage(
                    lines.Select(x => (x.AllocationId, x.WaybillItemId, x.Quantity)).ToList(),
                    activeAllocations.Select(x => (x.Id, x.WaybillItemId, x.Quantity)).ToList());
            }
            catch (ShippingExecutionRuleException ex) when (ex.Code == "MANIFEST_LINE_INVALID")
            {
                throw new WaybillPersistenceException("MANIFEST_NOT_ACCEPTED", ex);
            }

            trip.Status = ShippingExecutionStatuses.Trip.Departed;
            trip.ActualDepartAt = request.ActualDepartAt;
            trip.LastClientOperationId = operationId;
            trip.Version++;
            trip.UpdatedAt = DateTimeOffset.UtcNow;

            foreach (var line in lines)
            {
                var manifest = manifests.Single(x => x.Id == line.ManifestId);
                Movements.Add(new MovementEventEntity
                {
                    Id = Guid.NewGuid(), CompanyId = context.CompanyId, BranchId = context.BranchId,
                    WaybillId = line.WaybillId, WaybillItemId = line.WaybillItemId,
                    AllocationId = line.AllocationId, ManifestLineId = line.Id,
                    EventType = "DEPART", Quantity = line.LoadedQuantity,
                    TripId = trip.Id, ManifestId = manifest.Id,
                    FromLocationId = trip.OriginId, ToLocationId = trip.DestinationId,
                    OccurredAt = request.ActualDepartAt, RecordedAt = DateTimeOffset.UtcNow,
                    RecordedBy = context.UserId,
                    ClientOperationId = ScopedMovementOperationId(
                        context.BranchId, DerivedMovementOperationId(operationId, line.Id))
                });
            }

            await Save(cancellationToken);
            var response = await TripResponseOf(context, trip.Id, cancellationToken);
            await PersistCommandOutcomeAsync(
                context, action, operationId, "Trip", trip.Id, fingerprint, response, cancellationToken);
            await audit.WriteAsync(context, "TripStart", "SUCCESS", "Trip", trip.Id,
                null, JsonSerializer.Serialize(new { trip.TripNo, trip.ActualDepartAt, DepartLineCount = lines.Count }),
                null, cancellationToken);
            await tx.CommitAsync(cancellationToken);
            return response;
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            db.ChangeTracker.Clear();
            replay = await TryReplayCommandAsync<TripResponse>(
                context, action, operationId, "Trip", tripId, fingerprint, cancellationToken);
            if (replay is not null) return replay;
            throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT", ex);
        }
        catch (PostgresException ex) when (ex.SqlState == "23505")
        {
            db.ChangeTracker.Clear();
            replay = await TryReplayCommandAsync<TripResponse>(
                context, action, operationId, "Trip", tripId, fingerprint, cancellationToken);
            if (replay is not null) return replay;
            throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT", ex);
        }
        catch (Exception ex) when (IsSerializationFailure(ex))
        {
            throw new WaybillPersistenceException("CONCURRENCY_CONFLICT", ex);
        }
    }

    private async Task<(WaybillEntity Waybill, WaybillItemEntity Item)> RequireItem(
        OperationContext context,
        Guid? waybillId,
        Guid itemId,
        CancellationToken ct)
    {
        var scoped = await (
            from item in Items.AsNoTracking()
            join waybill in Waybills.AsNoTracking() on item.WaybillId equals waybill.Id
            where item.Id == itemId &&
                  (!waybillId.HasValue || waybill.Id == waybillId.Value) &&
                  waybill.CompanyId == context.CompanyId &&
                  waybill.BranchId == context.BranchId
            select new { Waybill = waybill, Item = item })
            .SingleOrDefaultAsync(ct)
            ?? throw new WaybillPersistenceException("NOT_FOUND");
        return (scoped.Waybill, scoped.Item);
    }

    private async Task<TripEntity> RequireTrip(OperationContext context, Guid tripId, CancellationToken ct)
        => await Trips.SingleOrDefaultAsync(x =>
               x.Id == tripId && x.CompanyId == context.CompanyId && x.BranchId == context.BranchId, ct)
           ?? throw new WaybillPersistenceException("NOT_FOUND");

    private async Task<ManifestEntity> RequireManifest(OperationContext context, Guid manifestId, CancellationToken ct)
        => await Manifests.SingleOrDefaultAsync(x =>
               x.Id == manifestId && x.CompanyId == context.CompanyId && x.BranchId == context.BranchId, ct)
           ?? throw new WaybillPersistenceException("NOT_FOUND");

    private async Task<TripAllocationEntity> RequireActiveAllocationForLine(
        OperationContext context,
        ManifestEntity manifest,
        ManifestLineEntity line,
        CancellationToken ct)
    {
        var allocation = await Allocations.AsNoTracking().SingleOrDefaultAsync(x =>
            x.Id == line.AllocationId && x.CompanyId == context.CompanyId && x.BranchId == context.BranchId &&
            x.TripId == manifest.TripId && x.WaybillItemId == line.WaybillItemId &&
            x.Status == ShippingExecutionStatuses.Allocation.Allocated && x.ReversalOfId == null,
            ct) ?? throw new WaybillPersistenceException("INVALID_STATE");

        if (await Allocations.AsNoTracking().AnyAsync(x =>
            x.CompanyId == context.CompanyId && x.BranchId == context.BranchId && x.ReversalOfId == allocation.Id, ct))
            throw new WaybillPersistenceException("INVALID_STATE");
        if (Math.Abs(allocation.Quantity - line.Quantity) > 0.0001m)
            throw new WaybillPersistenceException("MANIFEST_LINE_INVALID");
        return allocation;
    }

    private Task<List<TripAllocationEntity>> ActiveTripAllocations(
        OperationContext context,
        Guid tripId,
        CancellationToken ct)
        => Allocations.AsNoTracking().Where(x =>
                x.TripId == tripId && x.CompanyId == context.CompanyId && x.BranchId == context.BranchId &&
                x.Status == ShippingExecutionStatuses.Allocation.Allocated && x.ReversalOfId == null &&
                !Allocations.Any(r =>
                    r.CompanyId == context.CompanyId && r.BranchId == context.BranchId && r.ReversalOfId == x.Id))
            .ToListAsync(ct);

    private async Task EnsureNoActiveHold(OperationContext context, Guid waybillId, CancellationToken ct)
    {
        if (await Holds.AsNoTracking().AnyAsync(x =>
            x.WaybillId == waybillId && x.CompanyId == context.CompanyId &&
            x.BranchId == context.BranchId && x.Status == "ACTIVE", ct))
            throw new WaybillPersistenceException("HOLD_BLOCKED");
    }

    private async Task<decimal> ReleasedNet(OperationContext context, Guid itemId, CancellationToken ct)
    {
        var entries = await Releases.AsNoTracking().Where(x =>
                x.WaybillItemId == itemId &&
                x.CompanyId == context.CompanyId &&
                x.BranchId == context.BranchId)
            .ToListAsync(ct);
        return entries.Sum(x =>
            x.Status == ShippingExecutionStatuses.Release.Active ? x.Quantity : -x.Quantity);
    }

    private async Task<decimal> AllocationNet(OperationContext context, Guid releaseId, CancellationToken ct)
    {
        var entries = await Allocations.AsNoTracking().Where(x =>
                x.ReleaseId == releaseId &&
                x.CompanyId == context.CompanyId &&
                x.BranchId == context.BranchId)
            .ToListAsync(ct);
        return entries.Sum(x =>
            x.Status == ShippingExecutionStatuses.Allocation.Allocated ? x.Quantity : -x.Quantity);
    }

    private async Task<ItemQuantityStateResponse> ItemState(
        OperationContext context, Guid waybillId, Guid itemId, CancellationToken ct)
    {
        var scoped = await RequireItem(context, waybillId, itemId, ct);
        var released = await ReleasedNet(context, itemId, ct);
        return new ItemQuantityStateResponse(
            waybillId, itemId, scoped.Item.Quantity,
            released, scoped.Item.Quantity - released, context.CorrelationId);
    }

    private async Task<TripResponse> TripResponseOf(OperationContext context, Guid tripId, CancellationToken ct)
    {
        var trip = await Trips.AsNoTracking().SingleAsync(x =>
            x.Id == tripId && x.CompanyId == context.CompanyId && x.BranchId == context.BranchId, ct);
        var stops = await Stops.AsNoTracking()
            .Where(x => x.TripId == tripId &&
                        x.Trip!.CompanyId == context.CompanyId &&
                        x.Trip.BranchId == context.BranchId)
            .OrderBy(x => x.StopNo)
            .Select(x => new TripStopResponse(
                x.Id, x.StopNo, x.LocationId, x.StopType, x.PlannedAt, x.Status))
            .ToListAsync(ct);
        return new TripResponse(
            trip.Id, trip.CompanyId, trip.BranchId, trip.TripNo, trip.VehicleId, trip.DriverId,
            trip.OriginId, trip.DestinationId, trip.PlannedDepartAt, trip.ActualDepartAt,
            trip.Status, trip.Version, stops, context.CorrelationId);
    }

    private async Task<ManifestResponse> ManifestResponseOf(
        OperationContext context, Guid manifestId, CancellationToken ct)
    {
        var manifest = await Manifests.AsNoTracking().SingleAsync(x =>
            x.Id == manifestId &&
            x.CompanyId == context.CompanyId &&
            x.BranchId == context.BranchId, ct);
        var tripVersion = await Trips.AsNoTracking().Where(x =>
                x.Id == manifest.TripId &&
                x.CompanyId == context.CompanyId &&
                x.BranchId == context.BranchId)
            .Select(x => x.Version).SingleAsync(ct);
        var lines = await ManifestLines.AsNoTracking().Where(x =>
                x.ManifestId == manifestId &&
                x.Manifest!.CompanyId == context.CompanyId &&
                x.Manifest.BranchId == context.BranchId)
            .OrderBy(x => x.Id)
            .Select(x => new ManifestLineResponse(
                x.Id, x.AllocationId, x.WaybillId, x.WaybillItemId,
                x.Quantity, x.LoadedQuantity, x.Weight, x.Volume, x.LoadStatus))
            .ToListAsync(ct);
        return new ManifestResponse(
            manifest.Id, manifest.TripId, manifest.ManifestNo, manifest.CreatedAt,
            manifest.HandoverAt, manifest.DriverAcceptedAt, manifest.Status, manifest.Version,
            tripVersion, lines, context.CorrelationId);
    }

    private Task<ManifestLineResponse> ManifestLineResponseOf(
        OperationContext context, Guid lineId, CancellationToken ct)
        => ManifestLines.AsNoTracking().Where(x =>
                x.Id == lineId &&
                x.Manifest!.CompanyId == context.CompanyId &&
                x.Manifest.BranchId == context.BranchId)
            .Select(x => new ManifestLineResponse(
                x.Id, x.AllocationId, x.WaybillId, x.WaybillItemId,
                x.Quantity, x.LoadedQuantity, x.Weight, x.Volume, x.LoadStatus))
            .SingleAsync(ct);

    private static ManifestLineResponse ManifestLineResponseOf(ManifestLineEntity x)
        => new(x.Id, x.AllocationId, x.WaybillId, x.WaybillItemId, x.Quantity,
            x.LoadedQuantity, x.Weight, x.Volume, x.LoadStatus);

    private static AllocationResponse AllocationResponseOf(
        OperationContext context, TripAllocationEntity x)
        => new(x.Id, x.WaybillItemId, x.ReleaseId, x.TripId, x.Quantity,
            x.Status, x.ReversalOfId, context.CorrelationId);

    private async Task<TResponse?> TryReplayCommandAsync<TResponse>(
        OperationContext context,
        string action,
        string operationId,
        string aggregateType,
        Guid aggregateId,
        string requestFingerprint,
        CancellationToken ct) where TResponse : class
    {
        var outcome = await ReadCommandOutcomeAsync(context, action, operationId, ct);
        if (outcome is null) return null;
        if (!string.Equals(outcome.AggregateType, aggregateType, StringComparison.Ordinal) ||
            outcome.AggregateId != aggregateId ||
            !string.Equals(outcome.RequestFingerprint, requestFingerprint, StringComparison.Ordinal))
            throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT");

        return JsonSerializer.Deserialize<TResponse>(outcome.ResponseJson)
            ?? throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT");
    }

    private async Task<CommandOutcomeSnapshot?> ReadCommandOutcomeAsync(
        OperationContext context,
        string action,
        string operationId,
        CancellationToken ct)
    {
        var connection = db.Database.GetDbConnection();
        var closeWhenDone = connection.State != ConnectionState.Open;
        if (closeWhenDone) await connection.OpenAsync(ct);
        try
        {
            await using var command = connection.CreateCommand();
            command.Transaction = db.Database.CurrentTransaction?.GetDbTransaction();
            command.CommandText = """
                SELECT "AggregateType", "AggregateId", "RequestFingerprint", "ResponseJson"
                FROM transport_erp.shipping_command_outcomes
                WHERE "CompanyId" = @company
                  AND "BranchId" = @branch
                  AND "Action" = @action
                  AND "ClientOperationId" = @operation
                """;
            command.Parameters.Add(new NpgsqlParameter("company", context.CompanyId));
            command.Parameters.Add(new NpgsqlParameter("branch", context.BranchId));
            command.Parameters.Add(new NpgsqlParameter("action", action));
            command.Parameters.Add(new NpgsqlParameter("operation", operationId));

            await using var reader = await command.ExecuteReaderAsync(ct);
            if (!await reader.ReadAsync(ct)) return null;
            return new CommandOutcomeSnapshot(
                reader.GetString(0),
                reader.GetGuid(1),
                reader.GetString(2),
                reader.GetString(3));
        }
        finally
        {
            if (closeWhenDone) await connection.CloseAsync();
        }
    }

    private async Task PersistCommandOutcomeAsync<TResponse>(
        OperationContext context,
        string action,
        string operationId,
        string aggregateType,
        Guid aggregateId,
        string requestFingerprint,
        TResponse response,
        CancellationToken ct)
    {
        var transaction = db.Database.CurrentTransaction
            ?? throw new InvalidOperationException("Idempotency outcome requires an active transaction.");
        var connection = db.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
            throw new InvalidOperationException("Idempotency outcome requires the active EF connection.");

        await using var command = connection.CreateCommand();
        command.Transaction = transaction.GetDbTransaction();
        command.CommandText = """
            INSERT INTO transport_erp.shipping_command_outcomes
                ("Id", "CompanyId", "BranchId", "Action", "ClientOperationId",
                 "AggregateType", "AggregateId", "RequestFingerprint", "ResponseJson", "CreatedAt")
            VALUES
                (@id, @company, @branch, @action, @operation,
                 @aggregate_type, @aggregate_id, @fingerprint, @response, @created_at)
            """;
        command.Parameters.Add(new NpgsqlParameter("id", Guid.NewGuid()));
        command.Parameters.Add(new NpgsqlParameter("company", context.CompanyId));
        command.Parameters.Add(new NpgsqlParameter("branch", context.BranchId));
        command.Parameters.Add(new NpgsqlParameter("action", action));
        command.Parameters.Add(new NpgsqlParameter("operation", operationId));
        command.Parameters.Add(new NpgsqlParameter("aggregate_type", aggregateType));
        command.Parameters.Add(new NpgsqlParameter("aggregate_id", aggregateId));
        command.Parameters.Add(new NpgsqlParameter("fingerprint", requestFingerprint));
        command.Parameters.Add(new NpgsqlParameter("response", JsonSerializer.Serialize(response)));
        command.Parameters.Add(new NpgsqlParameter("created_at", DateTimeOffset.UtcNow));
        await command.ExecuteNonQueryAsync(ct);
    }

    private static string CommandFingerprint(string action, object payload)
    {
        var json = JsonSerializer.Serialize(new { action, payload });
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(json));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    private static string ScopedMovementOperationId(Guid branchId, string operationId)
    {
        var bytes = SHA256.HashData(
            Encoding.UTF8.GetBytes($"{branchId:N}:{operationId}"));
        return $"branch:{branchId:N}:{Convert.ToHexString(bytes).ToLowerInvariant()}";
    }

    private sealed record CommandOutcomeSnapshot(
        string AggregateType,
        Guid AggregateId,
        string RequestFingerprint,
        string ResponseJson);

    private async Task EnsureTripReplayAsync(TripEntity replay, CreateTripRequest request, CancellationToken ct)
    {
        if (!string.Equals(replay.TripNo, request.TripNo.Trim(), StringComparison.Ordinal) ||
            replay.VehicleId != request.VehicleId || replay.DriverId != request.DriverId ||
            replay.OriginId != request.OriginId || replay.DestinationId != request.DestinationId ||
            !SameInstant(replay.PlannedDepartAt, request.PlannedDepartAt))
            throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT");

        var persistedStops = await Stops.AsNoTracking().Where(x => x.TripId == replay.Id).OrderBy(x => x.StopNo)
            .Select(x => new { x.StopNo, x.LocationId, x.StopType, x.PlannedAt }).ToListAsync(ct);
        var requestedStops = (request.Stops ?? []).OrderBy(x => x.StopNo).ToList();
        if (persistedStops.Count != requestedStops.Count)
            throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT");

        for (var i = 0; i < persistedStops.Count; i++)
        {
            var persisted = persistedStops[i];
            var requested = requestedStops[i];
            if (persisted.StopNo != requested.StopNo || persisted.LocationId != requested.LocationId ||
                !string.Equals(persisted.StopType, requested.StopType.Trim().ToUpperInvariant(), StringComparison.Ordinal) ||
                !SameInstant(persisted.PlannedAt, requested.PlannedAt))
                throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT");
        }
    }

    private static bool ManifestCreateReplayMatches(ManifestEntity replay, Guid tripId, GenerateManifestRequest request)
    {
        if (replay.TripId != tripId)
            return false;
        return string.IsNullOrWhiteSpace(request.ManifestNo) ||
               string.Equals(replay.ManifestNo, request.ManifestNo.Trim(), StringComparison.Ordinal);
    }

    private static bool LoadReplayMatches(
        MovementEventEntity replay,
        Guid manifestId,
        Guid lineId,
        LoadManifestLineRequest request)
        => replay.ManifestId == manifestId && replay.ManifestLineId == lineId && replay.Quantity == request.Quantity &&
           SameInstant(replay.OccurredAt, request.OccurredAt) &&
           string.Equals(replay.ReasonCode, ResourceConstraintEvidence(request.ResourceConstraintConfirmed), StringComparison.Ordinal);

    private static string ResourceConstraintEvidence(bool confirmed)
        => confirmed ? "RESOURCE_CONSTRAINT_CONFIRMED" : "RESOURCE_CONSTRAINT_NOT_CONFIRMED";

    private static bool SameInstant(DateTimeOffset left, DateTimeOffset right)
        => Math.Abs((left.ToUniversalTime() - right.ToUniversalTime()).Ticks) <= 10;

    private static bool SameInstant(DateTimeOffset? left, DateTimeOffset right)
        => left.HasValue && SameInstant(left.Value, right);

    private static bool SameInstant(DateTimeOffset? left, DateTimeOffset? right)
        => left.HasValue == right.HasValue && (!left.HasValue || SameInstant(left.Value, right!.Value));

    private async Task Save(CancellationToken ct)
    {
        try
        {
            await db.SaveChangesAsync(ct);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new WaybillPersistenceException("CONCURRENCY_CONFLICT", ex);
        }
    }

    private static string DerivedMovementOperationId(string operationId, Guid lineId)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes($"{operationId}:{lineId:N}"));
        return $"depart:{Convert.ToHexString(bytes).ToLowerInvariant()}";
    }

    private static bool IsUniqueViolation(Exception exception)
    {
        for (var current = exception; current is not null; current = current.InnerException)
            if (current is PostgresException { SqlState: "23505" }) return true;
        return false;
    }

    private static bool IsSerializationFailure(Exception exception)
    {
        for (var current = exception; current is not null; current = current.InnerException)
            if (current is PostgresException { SqlState: "40001" or "40P01" }) return true;
        return false;
    }
}
