# TEAM-C1 Architecture Inventory

**Version:** `1.1 — CORRECTED REOPEN PACKAGE`

**Supersession:** Corrects and supersedes v1.0 for downstream use; v1.0 is preserved unchanged.

**Correction scope:** Exact source behavior of `TransportErpDbContextFactory` plus register/manifest conformance only.

**Baseline:** `governance/control-tower-20260828` @ `8a36f88b56a43cd5b47277b645ba2030ed3da4f1`

**Counting rule:** a project is counted only when a current `.csproj` exists and is listed in the authoritative `.slnx`.

## 1. Project inventory — 10 projects

| # | Project | SDK / evaluated type | TFM | Direct project references | Function proven from code | Current status |
|---:|---|---|---|---|---|---|
| 1 | `TransportERP.Api` | Web / executable | `net10.0` | Application, Contracts, Infrastructure | Sole startup/composition root; JWT validation; sync and audit routes; Waybill foundation/finance/shipping routes | **Current code-reachable runtime**; boot not executed |
| 2 | `TransportERP.Application` | Class library | `net10.0` | Domain, Contracts | Active Waybill use cases and ports; separate in-memory P1 behavior model | **Mixed:** Waybill services runtime-reachable; P1Baseline test/prototype-only |
| 3 | `TransportERP.Contracts` | Class library | `net10.0` | None | Cross-boundary DTOs/primitives for Waybills, Geo, Party, Numbering, Tracking, Attachments, Core | **Mixed:** active Waybill contracts plus unintegrated foundation surfaces |
| 4 | `TransportERP.Desktop` | WinForms class library under current files | `net10.0-windows` | Contracts | RTL/code-built Waybill forms and screen catalog metadata | **Foundation/prototype only:** no entry point or current service wiring |
| 5 | `TransportERP.Infrastructure` | Class library | `net10.0` | Domain, Application, Contracts | EF Core/Npgsql persistence, repositories, audit, sync, vouchers, model configuration, migrations | **Current runtime for registered services**, plus non-wired services |
| 6 | `TransportERP.Mobile.Admin` | Class library under current files | `net10.0` | None | Conditional MAUI project definition only | **Empty placeholder/foundation** |
| 7 | `TransportERP.Mobile.Customer` | Class library under current files | `net10.0` | None | Conditional MAUI project definition only | **Empty placeholder/foundation** |
| 8 | `TransportERP.Mobile.Driver` | Class library under current files | `net10.0` | None | Conditional MAUI project definition only | **Empty placeholder/foundation** |
| 9 | `TransportERP.Tests` | xUnit test class library | `net10.0` | Application, Contracts, Infrastructure, API | Unit, API-contract, EF in-memory, and PostgreSQL integration suites | **Test project; NOT RUN at exact SHA** |
| 10 | `TransportERP.Domain` (`TransportERP/`) | Class library | `net10.0` | None | Waybill aggregate, financial rules, shipping execution rules | **Current runtime dependency** |

The Desktop project becomes `WinExe` only if `Program.cs` exists; it does not. Each Mobile project becomes a MAUI executable only when its declared scaffold/resources exist; none of those projects contains a C# file at this baseline.

## 2. Direct NuGet/package configuration

| Project | Direct package references |
|---|---|
| API | `Microsoft.AspNetCore.OpenApi 10.0.10`; `Microsoft.OpenApi 2.7.5`; `Microsoft.AspNetCore.Authentication.JwtBearer 10.0.10`; `Microsoft.EntityFrameworkCore 10.0.0`; `Npgsql.EntityFrameworkCore.PostgreSQL 10.0.0` |
| Infrastructure | `Microsoft.EntityFrameworkCore 10.0.0`; `Npgsql.EntityFrameworkCore.PostgreSQL 10.0.0`; `System.Security.Cryptography.Xml 10.0.10`; `Microsoft.EntityFrameworkCore.Design 10.0.0` as private assets |
| Tests | `coverlet.collector 6.0.4`; `Microsoft.EntityFrameworkCore.InMemory 10.0.0`; `Microsoft.AspNetCore.Mvc.Testing 10.0.0`; `Microsoft.NET.Test.Sdk 17.14.1`; `xunit 2.9.3`; `xunit.runner.visualstudio 3.1.4` |
| Mobile projects | MAUI and Debug Logging references are conditional on an absent MAUI scaffold, so they are not active under the current file state |
| Domain, Application, Contracts, Desktop | No direct package references |

No `global.json`, `Directory.Build.props`, `Directory.Build.targets`, `Directory.Packages.props`, `NuGet.config`, or `packages.lock.json` exists in the current tree.

## 3. Solution tree

`TransportERP.slnx` contains only ten direct `<Project>` elements. There are no Solution Folders and no `.sln` or `.slnf` files.

```text
TransportERP.slnx
├── TransportERP.Api
├── TransportERP.Application
├── TransportERP.Contracts
├── TransportERP.Desktop
├── TransportERP.Infrastructure
├── TransportERP.Mobile.Admin
├── TransportERP.Mobile.Customer
├── TransportERP.Mobile.Driver
├── TransportERP.Tests
└── TransportERP.Domain
```

## 4. Physical repository tree

The tree below is architectural, not an exhaustive listing of documentation files.

```text
repository root
├── .github/workflows                 CI definitions
├── CONTROL_TOWER                     governance and mission outputs
├── TransportERP/Waybills             Domain code (3 C# files)
├── TransportERP.Api/Waybills         API modules (3) + Program.cs
├── TransportERP.Application
│   ├── P1Baseline                    in-memory P1 model/service (1)
│   └── Waybills                      active application services (3)
├── TransportERP.Contracts
│   ├── Attachments, Core, Geo
│   ├── Numbering, Party, Tracking
│   └── Waybills                      14 C# files total
├── TransportERP.Desktop
│   ├── CoreUI/Architecture           screen profile/catalog
│   └── Waybills                      forms (4 files; 16 concrete forms)
├── TransportERP.Infrastructure/Persistence
│   └── Migrations                    39 C# files total in Infrastructure
├── TransportERP.Mobile.Admin         csproj only
├── TransportERP.Mobile.Customer      csproj only
├── TransportERP.Mobile.Driver        csproj only
├── TransportERP.Tests                22 C# test/support files
├── artifacts                         historical evidence outputs
└── documentation                     architecture/design/closeout records
```

Production C# counts are: Domain 3, API 4, Application 4, Contracts 14, Desktop 5, Infrastructure 39, and zero in each Mobile project. Tests contain 22 C# files.

## 5. Current module/domain placement

| Capability | Proven physical placement | Runtime classification |
|---|---|---|
| Waybill foundation | Domain, Contracts/Waybills, Application/Waybills, API/Waybills, Infrastructure/Persistence, Desktop/Waybills | Server path code-reachable; Desktop not wired |
| Waybill finance | Domain financial rules, Waybill contracts/app/API, Infrastructure finance persistence, Desktop finance forms | Server path code-reachable; Desktop not wired |
| Shipping execution | Domain shipping rules, Waybill contracts/app/API, Infrastructure shipping persistence, Desktop shipping forms | Server path code-reachable; Desktop not wired |
| Numbering | Contract port + EF implementation + Waybill application usage | Code-reachable through Waybill approval |
| Operational parties | Waybill contract/application/repository/API; separate Party contract snapshot | Search/create API code-reachable; parallel Party contract not wired |
| Organization/security/settings/accounting P1 | Infrastructure entities/DbContext; in-memory Application prototype; voucher service | Data foundation; only audit/sync are API-exposed; no full feature API/UI proved |
| Audit | Infrastructure service/entity and API GET endpoint; Waybill audit sink | Code-reachable |
| Sync | Infrastructure enqueue/state service and API batch enqueue endpoint | Server intake code-reachable; no current client outbox/replay worker proved |
| Geo | Contracts only | Foundation-only |
| Attachments | Contract descriptor only | Foundation-only |
| Generic tracking | Contract `MovementEnvelope`; separate shipping movement entity | Contract is foundation/test-used; shipping movement persistence is active |
| Mobile | Three csproj-only projects | Placeholder-only |
| Reporting | No report project/service found; two read-only/status forms are feature UI, not a reporting subsystem | **No current reporting subsystem proved** |
| Ticketing | No production namespace, project, service, contract, screen, or persistence model found | **Not present in current source** |

## 6. Screens and UI

- Desktop contains **16 concrete WinForms classes** and one abstract base (`ShippingRtlForm`).
- Screen metadata exposes **19 screen IDs** because `WaybillDraftForm` represents five tab/catalog entries.
- Foundation: SHP-005/006/007/008/014.
- Finance: SHP-009/010/011/012.
- Shipping: SHP-015/016/019/023/024/025/027/028/029/030.
- UI is built in C#; no `.Designer.cs` or `.resx` files exist in Desktop.
- Forms bind Contract DTOs and raise request events. Repository-wide search found declarations/invocations but no current subscribers, application-service calls, HTTP client, composition root, or `Program.cs`.

## 7. Services, contracts, data, and infrastructure

- Application ports and services are under `TransportERP.Application/Waybills`.
- Active transport DTOs are under `TransportERP.Contracts/Waybills`; shared primitives are under Contracts/Core.
- All EF entities and implementations—P1, Waybill, finance, and shipping—share `TransportERP.Infrastructure.Persistence`.
- `TransportErpDbContext` is the only DbContext. Its public DbSets expose P1 types; P2 types are accessed with `Set<T>()` and composed by `TransportErpP2CombinedModelCustomizer`.
- All migrations are in `TransportERP.Infrastructure/Persistence/Migrations`: **10 migration classes**, **9 designer files**, and **one snapshot**.
- `TransportErpDbContextFactory` is the design-time factory. It reads `TRANSPORTERP_DESIGN_CONNSTR`; if the value is absent or whitespace, `CreateDbContext` throws `InvalidOperationException`. No source-coded local fallback connection string exists at baseline SHA `8a36f88b56a43cd5b47277b645ba2030ed3da4f1`.

## 8. Shared components, lookups, and common dialogs

- Shared code proven active: Contracts/Core (`OperationContext`, money/fx, capability/error primitives), Waybill DTOs/permission codes, and Desktop `TransportScreenProfile`.
- The only current lookup endpoint proved is operational-party search (`GET /api/v1/operational-parties`).
- Geo lookup DTOs exist, but no current Geo API, persistence implementation, or screen was found.
- No reusable current common-dialog/picker implementation was found.
- `ShippingRtlForm` is a feature-local UI base, not a repository-wide dialog layer.

## 9. Runtime, prototype, and foundation classification

“Runtime” below means reachable from the sole source startup root; it does not assert successful execution or deployment.

| Classification | Components |
|---|---|
| Current code-reachable runtime | API; Waybill Domain/Application; registered Infrastructure repositories/stores; audit and sync enqueue services |
| Library supporting runtime | Domain, Application, Contracts, Infrastructure |
| Test-only/prototype | `Application/P1Baseline/P1InMemoryBaseline.cs` |
| UI foundation, not executable | Desktop forms/catalog |
| Empty project foundation | Mobile.Admin, Mobile.Customer, Mobile.Driver |
| Data/service foundation not API-composed | P1 organization/identity/settings/accounting entities and `VoucherLifecycleService` |
| Contract foundation not runtime-integrated | Geo, Attachments, generic Party snapshot, generic Tracking envelope, `IBusinessAuditWriter` |
