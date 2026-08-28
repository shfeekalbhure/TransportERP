# TEAM-C1 Evidence Index — v1.1

**Version / scope:** `1.1 / corrected current-architecture package`

**Default evidence ref:** `refs/heads/governance/control-tower-20260828` @ full SHA `8a36f88b56a43cd5b47277b645ba2030ed3da4f1` unless a row explicitly says UNMERGED.

**Inherited collection time:** v1.0 recorded only `2026-08-28 UTC`; exact instants were not captured and are `UNKNOWN`, not reconstructed. `C1-DATA-002` was collected at `2026-08-28T02:20:30Z` / `2026-08-28T05:20:30+03:00`.

| Evidence ID | Finding ID(s) | Source ID | Type | Exact location / artifact | Ref / branch / full SHA | Collection time | Result and proof boundary | Artifact SHA-256 | Collector/reviewer role |
|---|---|---|---|---|---|---|---|---|---|
| C1-BASE-001 | Baseline identity | C1-SRC-001 | Git | `git rev-parse`; `git status`; tracking refs | Default evidence ref | `2026-08-28`, exact time UNKNOWN | Original ref/SHA/worktree state only | N/A | Evidence role |
| C1-BASE-002 | Current/master separation | C1-SRC-003/004 | Git history | name-status diff and original four-commit log | Default evidence ref; master `2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5` | `2026-08-28`, exact time UNKNOWN | Original governance-only delta claim at snapshot | N/A | Evidence role |
| C1-SOL-001 | Solution count/tree | C1-SRC-001 | Code/config | `TransportERP.slnx` | Default evidence ref | `2026-08-28`, exact time UNKNOWN | 10 project elements; flat logical tree | N/A | Architecture role |
| C1-PROJ-001 | Project inventory | C1-SRC-001 | Code/config | All 10 current `*.csproj` | Default evidence ref | `2026-08-28`, exact time UNKNOWN | SDK/TFM/output/reference/package declarations | N/A | Architecture role |
| C1-PKG-001 | C1-PROB-011 | C1-SRC-001 | Search | Root build/package configuration | Default evidence ref | `2026-08-28`, exact time UNKNOWN | Listed central config/lock files not found; transitive graph unproved | N/A | Architecture role |
| C1-DEP-001 | Dependency graph | C1-SRC-001 | Code/config | All `ProjectReference` elements | Default evidence ref | `2026-08-28`, exact time UNKNOWN | Exact direct project-reference graph | N/A | Architecture role |
| C1-CIRC-001 | No proven project cycle | C1-SRC-001 | Derived static evidence | Graph derived from C1-DEP-001 | Default evidence ref | `2026-08-28`, exact time UNKNOWN | No declared ProjectReference cycle; not runtime proof | N/A | Architecture + evidence roles |
| C1-RUN-001 | Startup/composition | C1-SRC-001 | Direct code | `TransportERP.Api/Program.cs` | Default evidence ref | `2026-08-28`, exact time UNKNOWN | Only host/composition root found; not boot proof | N/A | Application architecture role |
| C1-RUN-002 | API surface | C1-SRC-001 | Direct code | `TransportERP.Api/Waybills/*.cs`; `Program.cs` | Default evidence ref | `2026-08-28`, exact time UNKNOWN | 23 endpoints counted statically | N/A | API architecture role |
| C1-DOM-001 | Domain placement | C1-SRC-001 | Direct code | `TransportERP/Waybills/*.cs` | Default evidence ref | `2026-08-28`, exact time UNKNOWN | Current Domain source scope | N/A | Domain architecture role |
| C1-APP-001 | Active services | C1-SRC-001 | Direct code | `TransportERP.Application/Waybills/*.cs` | Default evidence ref | `2026-08-28`, exact time UNKNOWN | Ports/services and static composition | N/A | Application architecture role |
| C1-APP-002 | C1-PROB-004 | C1-SRC-001 | Code + references | `TransportERP.Application/P1Baseline/P1InMemoryBaseline.cs` | Default evidence ref | `2026-08-28`, exact time UNKNOWN | Test-referenced in-memory implementation; external use not excluded | N/A | Application architecture role |
| C1-CON-001 | Contract placement | C1-SRC-001 | Code + references | `TransportERP.Contracts/**/*.cs` | Default evidence ref | `2026-08-28`, exact time UNKNOWN | Active vs foundation-only inside repository | N/A | Contract architecture role |
| C1-DATA-001 | C1-PROB-001/002 | C1-SRC-001 | Direct code | `TransportErpDbContext.cs`; P1/P2 entities/model/customizers | Default evidence ref | `2026-08-28`, exact time UNKNOWN | One DbContext spans P1/P2; model composition | N/A | Data architecture role |
| C1-DATA-002 | C1-CORR-001 | C1-SRC-014 | Direct code | `TransportERP.Infrastructure/Persistence/TransportErpDbContextFactory.cs:8-18` | Default evidence ref; byte-identical at `e2843caff509d34509146f9dfe2e748dea22df7e` | `2026-08-28T02:20:30Z` / `2026-08-28T05:20:30+03:00` | Lines 10-15 read `TRANSPORTERP_DESIGN_CONNSTR` and throw when absent/whitespace; lines 16-18 use only the supplied value. Proves no source-coded local fallback in this file, not runtime environment state. | `d5c331d2180258fde574484de5f41a6ba78648743e0c7e3df68502620766c74c` | Data architecture + evidence roles |
| C1-MIG-001 | Migration placement | C1-SRC-001 | Code/config | `Persistence/Migrations/*` | Default evidence ref | `2026-08-28`, exact time UNKNOWN | 10 migrations, 9 designers, one snapshot; not applied-state proof | N/A | Data architecture role |
| C1-INF-001 | Infrastructure composition | C1-SRC-001 | Code + references | Persistence services/repositories and API DI | Default evidence ref | `2026-08-28`, exact time UNKNOWN | Active/non-wired service placement | N/A | Infrastructure role |
| C1-UI-001 | C1-PROB-005 | C1-SRC-001 | Code/config | Desktop csproj; absent `Program.cs` | Default evidence ref | `2026-08-28`, exact time UNKNOWN | Desktop evaluates to library | N/A | Desktop architecture role |
| C1-UI-002 | Desktop inventory/wiring | C1-SRC-001 | Code + references | `TransportERP.Desktop/**/*.cs` | Default evidence ref | `2026-08-28`, exact time UNKNOWN | 16 forms/19 IDs; no in-repository subscriber/client wiring | N/A | Desktop architecture role |
| C1-MOB-001 | Mobile placeholders | C1-SRC-001 | Code/config | Three Mobile csproj/directories | Default evidence ref | `2026-08-28`, exact time UNKNOWN | Zero C# files; current library evaluation | N/A | Mobile architecture role |
| C1-TEST-001 | Test inventory | C1-SRC-001 | Test source | Test csproj and 22 source files | Default evidence ref | `2026-08-28`, exact time UNKNOWN | Structure/declaration counts only; not run | N/A | Test architecture role |
| C1-CI-001 | Exact-SHA gap | C1-SRC-005/008 | CI/tooling | `.github/workflows/*`; checks; local CLI | Default evidence ref | `2026-08-28`, exact time UNKNOWN | Definitions exist; no exact-SHA run; local execution blocked in v1.0 | N/A | CI/evidence role |
| C1-ARCH-001 | Parallel P1 surfaces | C1-SRC-001 | Code + references | P1 in-memory and persistence sources | Default evidence ref | `2026-08-28`, exact time UNKNOWN | Parallel responsibilities; only EF side statically API-composed | N/A | Architecture role |
| C1-ARCH-002 | C1-PROB-003 | C1-SRC-001 | Direct code | `ShippingExecutionPersistence.cs` | Default evidence ref | `2026-08-28`, exact time UNKNOWN | 1,212-line multi-workflow concentration | N/A | Architecture role |
| C1-ARCH-003 | C1-PROB-002 | C1-SRC-001 | Direct code | `TransportErpDbContext.cs` | Default evidence ref | `2026-08-28`, exact time UNKNOWN | 400-line multi-capability configuration | N/A | Architecture role |
| C1-ARCH-004 | C1-PROB-006 | C1-SRC-001 | Direct code | Three API Waybill modules | Default evidence ref | `2026-08-28`, exact time UNKNOWN | Repeated helper/error patterns | N/A | API architecture role |
| C1-ARCH-005 | C1-PROB-007 | C1-SRC-001 | Direct code | `AuditEventService.cs`; `Program.cs` | Default evidence ref | `2026-08-28`, exact time UNKNOWN | API types in Persistence; separate API mapping | N/A | Architecture role |
| C1-SHARED-001 | Shared/parallel surfaces | C1-SRC-001 | Code + references | Audit/party/tracking contracts | Default evidence ref | `2026-08-28`, exact time UNKNOWN | No in-repository runtime composition; external use unknown | N/A | Contract architecture role |
| C1-LOOKUP-001 | Lookup placement | C1-SRC-001 | Code + search | API routes; Geo/Party contracts; Desktop | Default evidence ref | `2026-08-28`, exact time UNKNOWN | Operational-party route only; no common dialog found | N/A | UI/API architecture role |
| C1-NS-001 | Namespace placement | C1-SRC-001 | Direct code | Production namespaces | Default evidence ref | `2026-08-28`, exact time UNKNOWN | General alignment; semantic exception for `AuditEventApi` | N/A | Architecture role |
| C1-HIST-001 | History separation | C1-SRC-004 | Git history | Current/master graph | Default evidence ref | `2026-08-28`, exact time UNKNOWN | Original source equality/delta claim at snapshot | N/A | Evidence role |
| C1-UNMERGED-001 | Unmerged comparison | C1-SRC-007 | Git/GitHub/config | PR #69 metadata and unmerged `.slnx`/csproj diff | `origin/codex/p1-security-device-sync-offline-20260825` @ `939f49fa9c2ae57fa532ad55f67461c5f3f256f3` | `2026-08-28`, exact time UNKNOWN | 13-project candidate is UNMERGED, not current | N/A | Architecture + evidence roles |

## Qualification

- Static searches establish use only inside the inspected repository.
- No build, test, migration, API boot, live database, or Production claim is inferred.
- `C1-DATA-002` is the only substantive factual correction introduced by v1.1; other evidence preserves v1.0 conclusions while exposing missing original timestamps.
