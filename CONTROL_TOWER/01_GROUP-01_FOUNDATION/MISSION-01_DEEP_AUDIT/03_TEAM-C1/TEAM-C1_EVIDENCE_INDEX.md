# TEAM-C1 Evidence Index

All paths are relative to the repository root. “Finding” means a statement supported by the cited source at SHA `8a36f88b56a43cd5b47277b645ba2030ed3da4f1` unless explicitly marked unmerged.

| Evidence ID | Evidence | Supported finding |
|---|---|---|
| C1-BASE-001 | `git rev-parse`, `git status`, remote tracking refs | Authoritative ref/SHA and clean starting baseline |
| C1-BASE-002 | `git diff --name-status origin/master...HEAD` and four-commit log | Current product source equals master; branch delta is governance-only |
| C1-SOL-001 | `TransportERP.slnx` | Exactly 10 listed projects; flat solution; no Solution Folder nodes |
| C1-PROJ-001 | All ten `*.csproj` files | SDK, target framework, output type conditions, project/package references |
| C1-PKG-001 | Repository-wide configuration search | No `global.json`, central package props, `NuGet.config`, or package lock file in the current tree |
| C1-DEP-001 | All `ProjectReference` elements | Exact project-to-project dependency graph |
| C1-CIRC-001 | Directed graph derived from C1-DEP-001 | No project-reference cycle is present |
| C1-RUN-001 | `TransportERP.Api/Program.cs` | API is the only current startup/composition root; persistence/auth/sync/audit wiring |
| C1-RUN-002 | `TransportERP.Api/Waybills/*.cs` | 21 Waybill routes plus two routes in `Program.cs`, for 23 mapped endpoints total |
| C1-DOM-001 | `TransportERP/Waybills/*.cs` | Current Domain project contains Waybill aggregate and financial/shipping rules only |
| C1-APP-001 | `TransportERP.Application/Waybills/*.cs` | Active Waybill foundation, finance, and shipping application services/ports |
| C1-APP-002 | `TransportERP.Application/P1Baseline/P1InMemoryBaseline.cs` plus repository reference search | 664-line in-memory P1 implementation is referenced by its behavior tests, not API composition |
| C1-CON-001 | All 14 Contract source files plus reference search | Active Waybill contracts and additional shared/foundation-only contract surfaces |
| C1-DATA-001 | `TransportErpDbContext.cs`, P1/P2 entity/model/customizer files | One DbContext spans P1 and P2; P2 model is composed by custom model customizers |
| C1-MIG-001 | `Persistence/Migrations/*` | 10 migration classes, 9 designer companions, and one model snapshot in one location |
| C1-INF-001 | Persistence service/repository files and API DI registration | Infrastructure implements active API persistence and additional non-wired P1 services |
| C1-UI-001 | Desktop csproj and absence of `Program.cs` | Desktop currently evaluates to `Library`, not `WinExe` |
| C1-UI-002 | Five Desktop source files and repository-wide event reference search | 16 concrete WinForms/19 catalog IDs; events have no current subscribers or API-client wiring |
| C1-MOB-001 | Three Mobile csproj files and directory contents | Each Mobile project contains zero C# files and evaluates to a plain `net10.0` library |
| C1-TEST-001 | Test csproj and all 22 test source files | One xUnit test project; 101 `[Fact]` and 2 `[Theory]` declarations |
| C1-CI-001 | `.github/workflows/*`, GitHub exact-SHA checks, missing local `dotnet` | CI definitions exist, but exact-SHA build/test evidence is absent and local execution was blocked |
| C1-ARCH-001 | P1 in-memory source and P1 persistence entities/services | Parallel in-memory and EF responsibility surfaces exist; only the EF side is API-composed |
| C1-ARCH-002 | `ShippingExecutionPersistence.cs` | `EfShippingExecutionStore` is a 1,212-line, multi-workflow persistence/application-orchestration concentration |
| C1-ARCH-003 | `TransportErpDbContext.cs` | 400-line DbContext owns configuration across organization, identity, settings, accounting, audit, sync, and model composition |
| C1-ARCH-004 | Three API Waybill modules | Repeated `TryContext`, `TryGuid`, `HasPermission`, and endpoint error-execution patterns |
| C1-ARCH-005 | `AuditEventService.cs` and `Program.cs` | HTTP request/response/scope/API helper types sit in Persistence while live endpoint mapping is independently in API |
| C1-SHARED-001 | Contract declarations plus repository usage search | `IBusinessAuditWriter`, `OperationalPartySnapshot`, and `MovementEnvelope` are not part of current API composition; parallel active representations exist |
| C1-LOOKUP-001 | API routes, Contract namespaces, Desktop source | Operational-party search is the only proven current lookup route; Geo is contracts-only; no reusable common-dialog implementation found |
| C1-NS-001 | All production namespace declarations | Namespaces generally follow project/folder placement; `AuditEventApi` is a semantic placement inconsistency inside Persistence |
| C1-HIST-001 | Current/master commit graph | Current source architecture is the master source plus governance-only commits |
| C1-UNMERGED-001 | PR #69 metadata and unmerged `.slnx`/csproj diffs | The 13-project Offline/Desktop-E2E architecture is unmerged and not current |

## Evidence qualification

- File/reference searches establish use inside this repository only; they cannot exclude reflection, external consumers, or deployment-specific loading.
- Build, test, migration application, and runtime statements are not inferred from source shape. Where no execution evidence exists, the report says `NOT RUN` or `UNKNOWN — REQUIRES VERIFICATION`.
- Historical or pull-request sources are never used to redefine the current tree.

