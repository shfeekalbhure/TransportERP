# TEAM-A — Files Reviewed Register

Audit ref: `governance/control-tower-20260828@8a36f88b56a43cd5b47277b645ba2030ed3da4f1` unless a Library version or historical SHA is stated.

`FULL` means the file was read completely for the stated audit purpose. `TARGETED` means the relevant symbols/sections were read completely, while the file was not claimed as exhaustively reviewed for every possible concern. `INVENTORY` means existence, metadata or structure was verified without treating content as implemented behavior.

Version: `A-FILES-v1.0`. Formal ref for every repository row below: `refs/heads/governance/control-tower-20260828@8a36f88b56a43cd5b47277b645ba2030ed3da4f1`.

## Formal important-file index

| File ID | Path | Project / module / domain | Reviewer | Reason | Classes/methods/sections read | Linked Findings | Coverage | Unreviewed remainder |
|---|---|---|---|---|---|---|---|---|
| A-FILE-001 | `TransportERP.slnx` | Solution | Architecture | True project count/tree | Entire file | Architecture inventory | FULL FOR STATED PURPOSE | None |
| A-FILE-002 | all ten `*.csproj` listed below | Build/projects | Architecture + QA | Targets/output/references/packages | Entire files | A-RUNTIME-001/002; A-CI-001; A-SUPPLY-001 | FULL FOR STATED PURPOSE | Runtime build blocked |
| A-FILE-003 | `TransportERP.Api/Program.cs` | API/security/sync/audit | Architecture + DB/security | Composition/routes/auth | Entire file including helpers/records | A-SEC-001/002; A-OFF-001/002; A-PRIV-008 | FULL FOR STATED PURPOSE | External config/IdP unavailable |
| A-FILE-004 | `TransportERP.Api/Waybills/WaybillApiModule.cs` | API/waybill | Architecture + business | Registered repository/routes/context | Entire file | A-ARCH-002; A-ARCH-006 | FULL FOR STATED PURPOSE | Runtime blocked |
| A-FILE-005 | `TransportERP.Api/Waybills/WaybillFinanceApiModule.cs` | API/finance | Architecture + business | Write endpoints/read gap/auth duplication | Entire file | A-BIZ-005; A-ARCH-005/006 | FULL FOR STATED PURPOSE | Runtime blocked |
| A-FILE-006 | `TransportERP.Api/Waybills/ShippingExecutionApiModule.cs` | API/shipping | Business + architecture | Current command boundary | Entire file | A-BIZ-001; A-ARCH-005/006 | FULL FOR STATED PURPOSE | Runtime blocked |
| A-FILE-007 | `TransportERP.Infrastructure/Persistence/ConcurrencySafeWaybillRepository.cs` | Infrastructure/waybill DB | Architecture + evidence lead | Volume/CAS/save path | Entire file; `SaveAsync`, `ToItemEntity` | A-ARCH-002 | FULL FOR STATED PURPOSE | Runtime/data impact blocked |
| A-FILE-008 | `TransportERP/Waybills/WaybillAggregate.cs` | Domain/waybill | Architecture | Domain field/rules | Entire file | A-ARCH-002 | FULL FOR STATED PURPOSE | Runtime blocked |
| A-FILE-009 | `TransportERP.Infrastructure/Persistence/P2WaybillEntities.cs` | Infrastructure/waybill DB | Architecture + DB | Entity fields/PII/finance | Entire file | A-ARCH-002; A-PRIV-008; A-BIZ-005 | FULL FOR STATED PURPOSE | Live data unavailable |
| A-FILE-010 | `TransportERP.Infrastructure/Persistence/TransportErpDbContext.cs` | Infrastructure/database | DB/security | FKs/keys/filters/constraints | Entire model configuration | A-DB-003/004; A-ACCDB-007 | FULL FOR STATED PURPOSE | Live roles/RLS unavailable |
| A-FILE-011 | `TransportERP.Infrastructure/Persistence/P1Entities.cs` | Infrastructure/P1 model | DB/security + privacy | RBAC/accounting/sync/settings data | Entire file | A-SEC-001; A-DB-004; A-PRIV-008 | FULL FOR STATED PURPOSE | Actual data unavailable |
| A-FILE-012 | `TransportErpP2ModelCustomizer.cs`, `TransportErpP2FinanceModel.cs`, `TransportErpP2ShippingModel.cs` | Infrastructure/P2 database | DB/security | Tenant/FK/constraint model | Entire files | A-DB-003/005 | FULL FOR STATED PURPOSE | DB runtime blocked |
| A-FILE-013 | `TransportERP.Infrastructure/Persistence/SyncOperationService.cs` | Infrastructure/sync/security | Offline + DB/security | Tenant/device/version/retry/audit | Entire file; especially `EnsureSecurityAsync` | A-SEC-002; A-OFF-001/002 | FULL FOR STATED PURPOSE | External worker/IdP unavailable |
| A-FILE-014 | `TransportERP.Infrastructure/Persistence/AuditEventService.cs` | Infrastructure/audit | DB/security + privacy | Hash/append/query/transaction | Entire file | A-AUD-006; A-PRIV-008 | FULL FOR STATED PURPOSE | External immutable log unavailable |
| A-FILE-015 | `P2FinanceAppendOnlyInterceptor.cs` + finance migrations | Infrastructure/finance DB | DB/security | Append-only enforcement | Entire interceptor + relevant migration search | A-DB-005 | FULL FOR STATED PURPOSE | Raw-SQL runtime blocked |
| A-FILE-016 | `TransportERP.Infrastructure/Persistence/VoucherLifecycleService.cs` | Infrastructure/accounting | Business + DB/security | Voucher transitions/actor use | Entire file | A-ACCDB-007 | FULL FOR STATED PURPOSE | Posting integration absent |
| A-FILE-017 | `WaybillFinancePersistence.cs` | Infrastructure/finance | Business + DB/security | Collection/reversal/reference/link | Entire relevant service paths | A-BIZ-005 | FULL FOR STATED PURPOSE | External accounting jobs unknown |
| A-FILE-018 | `ShippingExecutionPersistence.cs` | Infrastructure/shipping | Business + architecture | Commands/events/transactions/tenant scope | All cited command paths | A-BIZ-001/002 | FULL FOR STATED PURPOSE | Unrelated incidental helpers not separately assessed |
| A-FILE-019 | all ten migration Up files + nine Designers + snapshot | Infrastructure/migrations | DB/security | Lineage/DDL/triggers | Entire Up bodies/inventory; snapshot targeted | A-DB-003/005; A-ACCDB-007 | FULL FOR STATED PURPOSE | Execution/upgrade blocked |
| A-FILE-020 | all current files under `TransportERP.Desktop/Waybills/` | Desktop/screens | Architecture + business | Forms/integration/duplication | Entire files for stated purpose | A-RUNTIME-001; A-ARCH-005/006; A-SCR-001 | FULL FOR STATED PURPOSE | UI execution unavailable |
| A-FILE-021 | `TransportERP.Desktop/CoreUI/Architecture/TransportScreenProfile.cs` | Desktop/shared UI | Architecture | Shared component inventory | Entire file | A-ARCH-006 | FULL FOR STATED PURPOSE | None |
| A-FILE-022 | three Mobile csproj and directories | Mobile | Offline/mobile + QA | Executable/scaffold reality | Entire project directories | A-RUNTIME-002 | FULL FOR STATED PURPOSE | No runtime exists |
| A-FILE-023 | all 22 `TransportERP.Tests/*.cs` | Tests | QA + relevant specialists | Static count/coverage/behavior | Entire corpus for test inventory and cited methods | A-QA-001/002/005; supporting domain findings | FULL FOR STATED PURPOSE | Discovery/execution blocked |
| A-FILE-024 | `P2C01AWaybillPostgreSqlIntegrationTests.cs` | Tests/waybill DB | Architecture + QA | Volume regression coverage | create/update cited ranges and full test context | A-ARCH-002 | FULL FOR STATED PURPOSE | Not executed |
| A-FILE-025 | `P2C01CPhysicalMeasurePostgreSqlTests.cs` | Tests/physical measures | Architecture + QA | Volume persistence path distinction | cited seed/assert ranges and full context | A-ARCH-002 | FULL FOR STATED PURPOSE | Not executed |
| A-FILE-026 | all seven `.github/workflows/*.yml` | CI/CD/supply chain | Git + QA | Gates/triggers/pins/artifacts | Entire files | A-CI-001; A-SUPPLY-001; A-QA-005 | FULL FOR STATED PURPOSE | Org controls external |
| A-FILE-027 | P1/P2 acceptance registers/assessments listed below | QA/governance | QA + business | Acceptance truth | Entire CSVs/reports for status | A-QA-002 | FULL FOR STATED PURPOSE | Cases not executed |
| A-FILE-028 | screen queue/FLOW01/SHP lineage files listed below | Screens/governance | Business + architecture | Authority identity | Entire cited files | A-SCR-001 | FULL FOR STATED PURPOSE | SHP-015..030 crosswalk unknown |
| A-FILE-029 | `P1InMemoryBaseline.cs` | Application/prototype | Architecture + QA | Parallel prototype classification | Entire file | Architecture/QA context | FULL FOR STATED PURPOSE | Not runtime-registered |
| A-FILE-030 | mandatory Control Tower command/governance files | Governance | TEAM-A lead | Scope/schema/independence | Entire files | All governance fields | FULL FOR STATED PURPOSE | TEAM-B content excluded |

## Governance and command — FULL

- `CONTROL_TOWER/README.md`
- every file under `CONTROL_TOWER/00_GOVERNANCE/`, including its decisions/registers
- `CONTROL_TOWER/01_GROUP-01_FOUNDATION/MISSION-01_DEEP_AUDIT/00_COMMAND/TRANSPORTERP_MASTER_DEEP_AUDIT_COMMAND_2026-08-28_AR_FINAL.md`
- `CONTROL_TOWER/01_GROUP-01_FOUNDATION/MISSION-01_DEEP_AUDIT/01_TEAM-A/START_ORDER.md`

TEAM-B content was excluded and not read.

## Solution and projects — FULL

- `TransportERP.slnx`
- `TransportERP.Api/TransportERP.Api.csproj`
- `TransportERP.Application/TransportERP.Application.csproj`
- `TransportERP.Contracts/TransportERP.Contracts.csproj`
- `TransportERP.Desktop/TransportERP.Desktop.csproj`
- `TransportERP.Infrastructure/TransportERP.Infrastructure.csproj`
- `TransportERP.Mobile.Admin/TransportERP.Mobile.Admin.csproj`
- `TransportERP.Mobile.Customer/TransportERP.Mobile.Customer.csproj`
- `TransportERP.Mobile.Driver/TransportERP.Mobile.Driver.csproj`
- `TransportERP.Tests/TransportERP.Tests.csproj`
- `TransportERP/TransportERP.Domain.csproj`

The repository was also inventoried for absent solution/build/package files: `.sln`, `.slnf`, `global.json`, `Directory.Build.*`, `Directory.Packages.props`, `NuGet.config`, and `packages.lock.json`.

## API, application, contracts and domain

FULL for their audit purpose:

- `TransportERP.Api/Program.cs`
- `TransportERP.Api/Waybills/WaybillApiModule.cs`
- `TransportERP.Api/Waybills/WaybillFinanceApiModule.cs`
- `TransportERP.Api/Waybills/ShippingExecutionApiModule.cs`
- all current files under `TransportERP.Application/Waybills/`
- `TransportERP.Application/P1Baseline/P1InMemoryBaseline.cs`
- all current files under `TransportERP.Contracts/`
- all current files under `TransportERP/Waybills/`
- `TransportERP.Infrastructure/Persistence/VoucherLifecycleService.cs`

## Infrastructure, database, migrations and security

FULL or FULL-for-purpose:

- `TransportERP.Infrastructure/Persistence/TransportErpDbContext.cs`
- `TransportERP.Infrastructure/Persistence/P1Entities.cs`
- `TransportERP.Infrastructure/Persistence/TransportErpPersistenceExtensions.cs`
- `TransportERP.Infrastructure/Persistence/TransportErpDbContextFactory.cs`
- `TransportERP.Infrastructure/Persistence/TransportErpP2ModelCustomizer.cs`
- `TransportERP.Infrastructure/Persistence/TransportErpP2CombinedModelCustomizer.cs`
- `TransportERP.Infrastructure/Persistence/TransportErpP2FinanceModel.cs`
- `TransportERP.Infrastructure/Persistence/TransportErpP2ShippingModel.cs`
- `TransportERP.Infrastructure/Persistence/P2WaybillEntities.cs`
- `TransportERP.Infrastructure/Persistence/P2ShippingEntities.cs`
- `TransportERP.Infrastructure/Persistence/SyncOperationService.cs`
- `TransportERP.Infrastructure/Persistence/AuditEventService.cs`
- `TransportERP.Infrastructure/Persistence/P2FinanceAppendOnlyInterceptor.cs`
- `TransportERP.Infrastructure/Persistence/P2ShippingAppendOnlyInterceptor.cs`
- `TransportERP.Infrastructure/Persistence/WaybillPersistenceServices.cs`
- `TransportERP.Infrastructure/Persistence/ConcurrencySafeWaybillRepository.cs`
- `TransportERP.Infrastructure/Persistence/WaybillFinancePersistence.cs`
- `TransportERP.Infrastructure/Persistence/ShippingExecutionPersistence.cs` (FULL for the shipping command/tenant/transaction paths cited)
- all ten migration `Up` files, the nine migration Designers, and the current model snapshot (FULL/INVENTORY for DDL lineage and cited constraints)

## Desktop, Mobile and screens

- all current files under `TransportERP.Desktop/Waybills/` — FULL
- `TransportERP.Desktop/CoreUI/Architecture/TransportScreenProfile.cs` — FULL
- all three Mobile project directories — FULL inventory; each contained only its csproj
- `documentation/design/04_SCREEN_WORK_QUEUE.csv` — FULL for status/identity reconciliation
- `documentation/design/screens/Waybills/FLOW01-W3-SCR-001/screen-spec.md` — FULL
- `documentation/design/screens/Waybills/SHP-005/screen-spec.md` — FULL
- `documentation/design/reviews/2026-08-24_LEGACY_WAYBILL_QUEUE_LINEAGE_RECONCILIATION.md` — FULL
- remaining screen folders/images — INVENTORY or TARGETED; an image's existence was never treated as runtime proof

## Tests and acceptance evidence

- all 22 C# files under `TransportERP.Tests/` — FULL for static test inventory and cited behavior
- `documentation/closeout/P1/P1_ACCEPTANCE_TEST_REGISTER.csv` — FULL
- `documentation/closeout/P1/P1_ACCEPTANCE_EXECUTION_ASSESSMENT.csv` and associated report — FULL
- `documentation/closeout/P2/P2_C01_ACCEPTANCE_TEST_REGISTER.csv` — FULL
- `documentation/closeout/P2/P2_C01_ACCEPTANCE_TEST_SUPPLEMENT_RR1.csv` — FULL
- `documentation/closeout/P1/test-results/P1_Baseline.trx` — FULL metadata/counter review
- `documentation/closeout/validate_p0_p1.py` and `documentation/closeout/P2/validate_p2_c01_contracts.py` — FULL and executed in isolated clone
- W0-5 validator — FULL for execution diagnosis; result classified environment/not-applicable, not source FAIL

## CI/CD, supply chain and release

- every file under `.github/workflows/` — FULL
- repository inventory for Docker/compose/publish/package/installer/signing/release/SBOM/lock/security policy files — FULL inventory
- GitHub exact-SHA checks, workflow runs/jobs, repository ruleset, PR/ref/release/tag metadata — remote evidence, TARGETED/FULL for cited fields

## Git history, refs and alternative copies

- full reachable log/inventory for the audited clone — FULL for baseline/history purpose
- root, initial-project, reset, master and governance commits — TARGETED diff/stat/tree review
- 50 remote branch refs and 10 open PR metadata — FULL inventory at query snapshot
- alternative repositories under `/workspace/scratch/143c66febc8c`, `/workspace/scratch/263a0f4a787d`, `/workspace/scratch/4c170dbb8858`, plus older PR69 copies — read-only Git status/head/ancestry/patch-equivalence inventory; source content was not promoted into current-state findings

## Kurrasa / Library

- `/الكراسة التنفيذية الأساسية الرسمية لمشروع TransportERP/الكراسة التنفيذية الأساسية الرسمية لمشروع TransportERP.md` — `libfile_5c67e0b643108191981df763c58df3d5`, version 72 — FULL
- `/.../08_النقل_والشحن_والبوالص_والرحلات/00_مسودة_دورة_حياة_البوليصة_والأصناف_والعهدة_2026-08-23.md` — `libfile_e2f84cd2057c8191aaf5dc35182ca696`, version 9 — FULL
- ticketing decision/register/booking-contract documents identified in `EVIDENCE_INDEX.md` — FULL/TARGETED for authority and implementation classification
- broader Kurrasa corpus — INVENTORY/TARGETED only; no exhaustive full-corpus claim is made

## Explicit exclusions

- TEAM-B reports, findings, evidence, assessments and recommendations: NOT OPENED.
- Production systems/data: NOT ACCESSED.
- Source/test/migration/database modification: NOT PERFORMED.
