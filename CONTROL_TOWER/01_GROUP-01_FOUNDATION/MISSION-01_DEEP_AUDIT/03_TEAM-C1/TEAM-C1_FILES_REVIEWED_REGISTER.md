# TEAM-C1 Files Reviewed Register

**Baseline:** `governance/control-tower-20260828` @ `8a36f88b56a43cd5b47277b645ba2030ed3da4f1`

| Scope | Review depth | Result/use |
|---|---|---|
| `CONTROL_TOWER/README.md` | FULL | Mandatory operating model |
| `CONTROL_TOWER/00_GOVERNANCE/**/*` | FULL | All governance decisions, orders, registers, and output rules present at baseline |
| `MISSION-01_DEEP_AUDIT/00_COMMAND/TRANSPORTERP_MASTER_DEEP_AUDIT_COMMAND_2026-08-28_AR_FINAL.md` | FULL | Governing mission command |
| `03_TEAM-C1/START_ORDER.md` | FULL | TEAM-C1 scope and prohibitions |
| `TransportERP.slnx` | FULL | Exact solution membership/tree |
| All 10 current `*.csproj` | FULL | Project type, TFM, output type, references, packages, build conditions |
| `.github/workflows/*` | FULL for architecture/trigger/build intent | CI definitions and target branches/jobs |
| `TransportERP/Waybills/*.cs` (3 files) | FULL for type/rule architecture | Domain placement and scope |
| `TransportERP.Application/Waybills/*.cs` (3 files) | FULL for ports, services, dependencies, public operations | Active application layer |
| `TransportERP.Application/P1Baseline/P1InMemoryBaseline.cs` | STRUCTURAL FULL; behavioral spot checks | In-memory P1 aggregate responsibility and repository usage |
| `TransportERP.Contracts/**/*.cs` (14 files) | FULL for declarations, namespaces, and repository usage | Contract inventory and active/foundation classification |
| `TransportERP.Api/Program.cs` | FULL | Startup, DI, middleware, sync/audit endpoints |
| `TransportERP.Api/Waybills/*.cs` (3 files) | FULL for endpoint map and repeated helpers | Route/UI boundary and service composition |
| `TransportERP.Desktop/**/*.cs` (5 files) | FULL for forms, catalog IDs, dependencies, and wiring | Screen inventory and disconnected-state proof |
| `TransportERP.Infrastructure/Persistence/TransportErpDbContext.cs` | FULL for DbSets/configuration/model scope | Database integration and responsibility concentration |
| `TransportERP.Infrastructure/Persistence/P1Entities.cs` | FULL for entity inventory | P1 organization/security/settings/accounting/audit/sync data scope |
| `TransportERP.Infrastructure/Persistence/P2WaybillEntities.cs` | FULL for entity inventory | Waybill/numbering/finance entity placement |
| `TransportERP.Infrastructure/Persistence/P2ShippingEntities.cs` | FULL for entity inventory | Shipping entity placement |
| P2 model/customizer and persistence-extension files | FULL | P2 model composition and DI integration |
| Waybill repository/application persistence files | FULL for interfaces implemented, methods, transactions, and composition | Active persistence implementations |
| `ShippingExecutionPersistence.cs` | FULL for class/method/responsibility map; critical paths inspected | Multi-workflow concentration finding |
| `AuditEventService.cs`, `SyncOperationService.cs`, `VoucherLifecycleService.cs` | FULL for public types/methods and composition references | Audit/sync runtime; voucher non-wiring; misplaced HTTP types |
| `Persistence/Migrations/*` (20 files) | FULL inventory; migration bodies reviewed by class/operation surface, not database execution | 10 migration locations/order and snapshot placement |
| `TransportERP.Tests/**/*.cs` (22 files) | FULL test declaration/suite inventory; representative implementation paths inspected | Test coverage map and counts, not a passing-test claim |
| `documentation/architecture/**/*`, `documentation/closeout/**/*`, `documentation/design/**/*` | Targeted architecture/contract context only | Historical/documented context; not used over code evidence |
| `artifacts/**/*` | Inventory and targeted read | Historical evidence only; not current runtime proof |
| Current Git history, refs, branch diffs, PR/check metadata | FULL for stated baseline questions | Current/unmerged separation |
| Unmerged PR #69 `.slnx`, added project files, and top-level diff | TARGETED | Unmerged comparison only |

## Coverage note

The repository's current product code was exhaustively inventoried by project, source file, declared type, namespace, project reference, package reference, endpoint, form, migration, and test declaration. Detailed business correctness of every method and every migration SQL statement is outside TEAM-C1's architecture-only mandate and is not claimed.

