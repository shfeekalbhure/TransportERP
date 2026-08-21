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
store = replace_once(store,
    "            if (await Allocations.AsNoTracking().AnyAsync(x => x.ReversalOfId == original.Id, cancellationToken))\n                throw new WaybillPersistenceException(\"INVALID_STATE\");\n            if (await Movements.AsNoTracking().AnyAsync(x =>\n                x.AllocationId == original.Id && x.EventType == \"LOAD\", cancellationToken))\n                throw new WaybillPersistenceException(\"ALREADY_LOADED\");",
    "            if (await Allocations.AsNoTracking().AnyAsync(x =>\n                    x.ReversalOfId == original.Id &&\n                    x.CompanyId == context.CompanyId && x.BranchId == context.BranchId,\n                    cancellationToken))\n                throw new WaybillPersistenceException(\"INVALID_STATE\");\n            if (await Movements.AsNoTracking().AnyAsync(x =>\n                    x.AllocationId == original.Id && x.EventType == \"LOAD\" &&\n                    x.CompanyId == context.CompanyId && x.BranchId == context.BranchId,\n                    cancellationToken))\n                throw new WaybillPersistenceException(\"ALREADY_LOADED\");",
    "unallocate scope")
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
    "    private static void EnsureTripReplay(",
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
