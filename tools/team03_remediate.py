from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]

def read(rel):
    return (ROOT / rel).read_text(encoding="utf-8-sig")

def write(rel, text):
    p = ROOT / rel
    p.parent.mkdir(parents=True, exist_ok=True)
    p.write_text(text, encoding="utf-8", newline="\n")

def replace_once(text, old, new, label):
    n = text.count(old)
    if n != 1:
        raise RuntimeError(f"{label}: expected 1 match, found {n}")
    return text.replace(old, new, 1)

def replace_between(text, start, end, replacement, label):
    i = text.find(start)
    if i < 0:
        raise RuntimeError(f"{label}: start marker not found")
    j = text.find(end, i + len(start))
    if j < 0:
        raise RuntimeError(f"{label}: end marker not found")
    return text[:i] + replacement + text[j:]

def section(payload, name, next_name):
    start = f"@@{name}@@\n"
    end = f"@@{next_name}@@\n"
    i = payload.index(start) + len(start)
    j = payload.index(end, i)
    return payload[i:j]

payload = read("tools/team03_store_parts.txt")
parts = {
    "GENERATE": section(payload, "GENERATE", "LOAD"),
    "LOAD": section(payload, "LOAD", "FINALIZE"),
    "FINALIZE": section(payload, "FINALIZE", "HANDOVER"),
    "HANDOVER": section(payload, "HANDOVER", "START"),
    "START": section(payload, "START", "HELPERS"),
    "HELPERS": section(payload, "HELPERS", "END"),
}

# Preserve TEAM-02 contract/idempotency checks while TEAM-03 hardens persistence.
parts["GENERATE"] = replace_once(parts["GENERATE"],
    "            if (replay.TripId != tripId)\n                throw new WaybillPersistenceException(\"IDEMPOTENCY_CONFLICT\");",
    "            if (!ManifestCreateReplayMatches(replay, tripId, request))\n                throw new WaybillPersistenceException(\"IDEMPOTENCY_CONFLICT\");",
    "generate replay contract")
parts["GENERATE"] = replace_once(parts["GENERATE"],
    "            if (replay is not null && replay.TripId == tripId)\n                return await ManifestResponseOf(context, replay.Id, cancellationToken);",
    "            if (replay is not null && ManifestCreateReplayMatches(replay, tripId, request))\n                return await ManifestResponseOf(context, replay.Id, cancellationToken);",
    "generate unique replay contract")
parts["GENERATE"] = replace_once(parts["GENERATE"],
    "                null, JsonSerializer.Serialize(new { manifest.TripId, manifest.ManifestNo, LineCount = candidates.Count }),\n                null, cancellationToken);",
    "                null, JsonSerializer.Serialize(new\n                {\n                    manifest.TripId,\n                    manifest.ManifestNo,\n                    LineCount = candidates.Count,\n                    SourceAllocations = candidates.Select(x => new\n                    {\n                        x.Allocation.Id,\n                        x.Allocation.WaybillItemId,\n                        x.Allocation.Quantity\n                    }).OrderBy(x => x.Id).ToArray()\n                }), null, cancellationToken);",
    "generate audit evidence")

parts["LOAD"] = replace_once(parts["LOAD"],
    "            if (replay.ManifestId != manifestId || replay.ManifestLineId != lineId || replay.Quantity != request.Quantity)\n                throw new WaybillPersistenceException(\"IDEMPOTENCY_CONFLICT\");",
    "            if (!LoadReplayMatches(replay, manifestId, lineId, request))\n                throw new WaybillPersistenceException(\"IDEMPOTENCY_CONFLICT\");",
    "load replay contract")
parts["LOAD"] = replace_once(parts["LOAD"],
    "            await EnsureNoActiveHold(context, line.WaybillId, cancellationToken);",
    "            _ = await RequireActiveAllocationForLine(context, manifest, line, cancellationToken);\n            await EnsureNoActiveHold(context, line.WaybillId, cancellationToken);",
    "load active allocation")
parts["LOAD"] = replace_once(parts["LOAD"],
    "                OccurredAt = request.OccurredAt, RecordedAt = DateTimeOffset.UtcNow, RecordedBy = context.UserId,\n                ClientOperationId = storedOperationId",
    "                OccurredAt = request.OccurredAt, RecordedAt = DateTimeOffset.UtcNow, RecordedBy = context.UserId,\n                ReasonCode = ResourceConstraintEvidence(request.ResourceConstraintConfirmed),\n                ClientOperationId = storedOperationId",
    "load evidence")
parts["LOAD"] = replace_once(parts["LOAD"],
    "                    movement.WaybillItemId, movement.Quantity, movement.OccurredAt\n                }), null, cancellationToken);",
    "                    movement.WaybillItemId, movement.Quantity, movement.OccurredAt,\n                    request.ResourceConstraintConfirmed\n                }), null, cancellationToken);",
    "load audit evidence")
parts["LOAD"] = replace_once(parts["LOAD"],
    "            if (replay is not null && replay.ManifestId == manifestId &&\n                replay.ManifestLineId == lineId && replay.Quantity == request.Quantity)\n                return await ManifestLineResponseOf(context, lineId, cancellationToken);",
    "            if (replay is not null && LoadReplayMatches(replay, manifestId, lineId, request))\n                return await ManifestLineResponseOf(context, lineId, cancellationToken);",
    "load unique replay contract")

parts["FINALIZE"] = replace_once(parts["FINALIZE"],
    "            ShippingExecutionRules.EnsureManifestCanFinalize(\n                lines.Select(x => (x.Quantity, x.LoadedQuantity)).ToList());\n\n            var trip = await RequireTrip(context, manifest.TripId, cancellationToken);",
    "            ShippingExecutionRules.EnsureManifestCanFinalize(\n                lines.Select(x => (x.Quantity, x.LoadedQuantity)).ToList());\n\n            var activeAllocations = await ActiveTripAllocations(context, manifest.TripId, cancellationToken);\n            ShippingExecutionRules.EnsureManifestAllocationCoverage(\n                lines.Select(x => (x.AllocationId, x.WaybillItemId, x.Quantity)).ToList(),\n                activeAllocations.Select(x => (x.Id, x.WaybillItemId, x.Quantity)).ToList());\n\n            var trip = await RequireTrip(context, manifest.TripId, cancellationToken);",
    "finalize allocation coverage")
parts["FINALIZE"] = replace_once(parts["FINALIZE"],
    "                null, JsonSerializer.Serialize(new { manifest.ManifestNo, LineCount = lines.Count, trip.Status }),\n                null, cancellationToken);",
    "                null, JsonSerializer.Serialize(new\n                {\n                    manifest.ManifestNo,\n                    LineCount = lines.Count,\n                    FinalTotals = new\n                    {\n                        Quantity = lines.Sum(x => x.Quantity),\n                        LoadedQuantity = lines.Sum(x => x.LoadedQuantity),\n                        Weight = lines.Sum(x => x.Weight),\n                        Volume = lines.Sum(x => x.Volume)\n                    },\n                    SourceAllocations = lines.OrderBy(x => x.AllocationId).Select(x => new\n                    {\n                        x.AllocationId, x.WaybillId, x.WaybillItemId, x.Quantity\n                    }).ToArray(),\n                    trip.Status\n                }), null, cancellationToken);",
    "finalize audit evidence")

parts["START"] = replace_once(parts["START"],
    "            if (lines.Count == 0 ||\n                lines.Any(x => x.LoadedQuantity <= 0m ||\n                               Math.Abs(x.LoadedQuantity - x.Quantity) > 0.0001m))\n                throw new WaybillPersistenceException(\"MANIFEST_NOT_ACCEPTED\");\n\n            trip.Status = ShippingExecutionStatuses.Trip.Departed;",
    "            if (lines.Count == 0 ||\n                lines.Any(x => x.LoadedQuantity <= 0m ||\n                               Math.Abs(x.LoadedQuantity - x.Quantity) > 0.0001m))\n                throw new WaybillPersistenceException(\"MANIFEST_NOT_ACCEPTED\");\n\n            var activeAllocations = await ActiveTripAllocations(context, tripId, cancellationToken);\n            try\n            {\n                ShippingExecutionRules.EnsureManifestAllocationCoverage(\n                    lines.Select(x => (x.AllocationId, x.WaybillItemId, x.Quantity)).ToList(),\n                    activeAllocations.Select(x => (x.Id, x.WaybillItemId, x.Quantity)).ToList());\n            }\n            catch (ShippingExecutionRuleException ex) when (ex.Code == \"MANIFEST_LINE_INVALID\")\n            {\n                throw new WaybillPersistenceException(\"MANIFEST_NOT_ACCEPTED\", ex);\n            }\n\n            trip.Status = ShippingExecutionStatuses.Trip.Departed;",
    "start allocation coverage")

active_helpers = '''    private async Task<TripAllocationEntity> RequireActiveAllocationForLine(
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

'''
parts["HELPERS"] = replace_once(parts["HELPERS"],
    "    private async Task EnsureNoActiveHold(OperationContext context, Guid waybillId, CancellationToken ct)",
    active_helpers + "    private async Task EnsureNoActiveHold(OperationContext context, Guid waybillId, CancellationToken ct)",
    "active allocation helpers")

path = "TransportERP.Infrastructure/Persistence/ShippingExecutionPersistence.cs"
store = read(path)
store = replace_once(store,
    "using Microsoft.EntityFrameworkCore;\nusing Npgsql;",
    "using Microsoft.EntityFrameworkCore;\nusing Microsoft.EntityFrameworkCore.Storage;\nusing Npgsql;",
    "storage using")
store = store.replace(
    "var releasedNet = await ReleasedNet(itemId, cancellationToken);",
    "var releasedNet = await ReleasedNet(context, itemId, cancellationToken);")
store = store.replace(
    "var allocatedNet = await AllocationNet(request.ReleaseId, cancellationToken);",
    "var allocatedNet = await AllocationNet(context, request.ReleaseId, cancellationToken);")
store = replace_once(store,
    "            var stops = await Stops.AsNoTracking().Where(x => x.TripId == tripId).OrderBy(x => x.StopNo)\n                .Select(x => x.LocationId).ToListAsync(cancellationToken);",
    "            var stops = await Stops.AsNoTracking()\n                .Where(x => x.TripId == tripId &&\n                            x.Trip!.CompanyId == context.CompanyId &&\n                            x.Trip.BranchId == context.BranchId)\n                .OrderBy(x => x.StopNo)\n                .Select(x => x.LocationId).ToListAsync(cancellationToken);",
    "allocate stops scope")
# Unallocate reversal/load scope was already landed by TEAM-02. Scope the remaining manifest-line anti-join.
store = replace_once(store,
    "            if (await ManifestLines.AsNoTracking().AnyAsync(x => x.AllocationId == original.Id, cancellationToken))",
    "            if (await ManifestLines.AsNoTracking().AnyAsync(x =>\n                x.AllocationId == original.Id &&\n                x.Manifest!.CompanyId == context.CompanyId && x.Manifest.BranchId == context.BranchId,\n                cancellationToken))",
    "unallocate manifest scope")
store = replace_once(store,
    "            }\n            throw new WaybillPersistenceException(\"DUPLICATE_TRIP_NO\", ex);\n        }\n    }\n\n    public async Task<AllocationResponse> AllocateAsync(",
    "            }\n            throw new WaybillPersistenceException(\"DUPLICATE_TRIP_NO\", ex);\n        }\n        catch (Exception ex) when (IsSerializationFailure(ex))\n        {\n            throw new WaybillPersistenceException(\"CONCURRENCY_CONFLICT\", ex);\n        }\n    }\n\n    public async Task<AllocationResponse> AllocateAsync(",
    "create trip serialization")
store = replace_between(store,
    "    public async Task<ManifestResponse> GenerateManifestAsync(",
    "    public async Task<ManifestLineResponse> LoadManifestLineAsync(",
    parts["GENERATE"], "generate")
store = replace_between(store,
    "    public async Task<ManifestLineResponse> LoadManifestLineAsync(",
    "    public async Task<ManifestResponse> FinalizeManifestAsync(",
    parts["LOAD"], "load")
store = replace_between(store,
    "    public async Task<ManifestResponse> FinalizeManifestAsync(",
    "    public async Task<ManifestResponse> HandoverManifestAsync(",
    parts["FINALIZE"], "finalize")
store = replace_between(store,
    "    public async Task<ManifestResponse> HandoverManifestAsync(",
    "    public async Task<TripResponse> StartTripAsync(",
    parts["HANDOVER"], "handover")
store = replace_between(store,
    "    public async Task<TripResponse> StartTripAsync(",
    "    private async Task<(WaybillEntity Waybill, WaybillItemEntity Item)> RequireItem(",
    parts["START"], "start")
store = replace_between(store,
    "    private async Task<(WaybillEntity Waybill, WaybillItemEntity Item)> RequireItem(",
    "    private async Task EnsureTripReplayAsync(",
    parts["HELPERS"], "helpers")
if "ReleasedNet(itemId" in store or "AllocationNet(request.ReleaseId" in store:
    raise RuntimeError("unscoped quantity helper call remains")
if "ManifestLineResponseOf(lineId, cancellationToken)" in store:
    raise RuntimeError("unscoped manifest-line response remains")
write(path, store)

workflow_path = ".github/workflows/p2-c01-c-shipping-execution.yml"
workflow = read(workflow_path)
old = "FullyQualifiedName~P2C01CShippingPostgreSqlIntegrationTests|FullyQualifiedName~P2C01CConcurrencyPostgreSqlTests"
new = old + "|FullyQualifiedName~P2C01CPhysicalMeasurePostgreSqlTests|FullyQualifiedName~P2C01CTeam03PostgreSqlHardeningTests"
workflow = replace_once(workflow, old, new, "PostgreSQL CI filter")
write(workflow_path, workflow)
print("TEAM-03 remediation applied")
