# TEAM-D Files Reviewed Register

Coverage `FULL FOR STATED PURPOSE` means the relevant file/sections were read for reconciliation; it does not claim runtime execution.

| File ID | Path / source | Domain | Ref/SHA | Purpose / sections | Linked findings | Coverage / not reviewed |
|---|---|---|---|---|---|---|
| D-FR-001 | `CONTROL_TOWER/README.md` | Governance | local TEAM-D snapshot | governing workspace rules | all | FULL FOR STATED PURPOSE |
| D-FR-002 | `00_GOVERNANCE/OWNER_DELEGATION_AND_OPERATING_DIRECTIVE.md` | Governance | local | authority, seal, transition, cadence | all | FULL |
| D-FR-003 | `00_GOVERNANCE/CONTROL_TOWER_AUTONOMOUS_SUPERVISION_PROTOCOL.md` | Governance | local | reconciliation/authority rules | all | FULL |
| D-FR-004 | `00_GOVERNANCE/CONTROL_TOWER_TEAM_DIRECTIVES.md` | Governance | local | TEAM-D CONTINUE boundary | all | TEAM-D section FULL |
| D-FR-005 | MISSION-01 master command | Audit governance | sealed Control Tower | all 43 sections, especially baseline/crosswalk/seal/gate | all | FULL |
| D-FR-006 | `04_TEAM-D/CURRENT_DIRECTIVE.md`, `START_ORDER.md` | TEAM-D | local | scope/output constraints | all | FULL |
| D-FR-007 | `01_TEAM-A/TEAM-A_INDEPENDENT_DEEP_AUDIT_REPORT.md` | A audit | sealed `8a36f88b` | all 29 findings, priorities, evidence limits | all A | FULL |
| D-FR-008 | `01_TEAM-A/EVIDENCE_INDEX.md` | A evidence | sealed | 30 evidence records | all A | FULL |
| D-FR-009 | `01_TEAM-A/FILES_REVIEWED_REGISTER.md` | A coverage | sealed | source depth/provenance | all A | FULL |
| D-FR-010 | `01_TEAM-A/UNKNOWN_AND_BLOCKERS_REGISTER.md` | A unknowns | sealed | 16 unknowns | all A | FULL |
| D-FR-011 | `01_TEAM-A/DOMAIN_COVERAGE_MATRIX.md` | A coverage | sealed | domain coverage | all A | FULL |
| D-FR-012 | A baseline/source/preservation/formation/manifest/seal/handoff files | A package | sealed | integrity, authority, preservation, independence | all A | FULL; 13/13 hashes verified |
| D-FR-013 | `02_TEAM-B/TEAM-B_INDEPENDENT_DEEP_AUDIT_REPORT.md` | B audit | sealed `8a36f88b` | all 21 findings and roll-up | all B | FULL |
| D-FR-014 | `02_TEAM-B/EVIDENCE_INDEX.md`, `FILES_REVIEWED_REGISTER.md` | B evidence | sealed | 25 evidence records and review scope | all B | FULL |
| D-FR-015 | B unknown/coverage/source/preservation/formation/baseline/manifest/seal/handoff files | B package | sealed | limits, BLK-B-001, integrity, temporal scope | all B | FULL; 13/13 hashes verified |
| D-FR-016 | `03_TEAM-C1/TEAM-C1_CURRENT_ARCHITECTURE_ASSESSMENT.md` | Architecture | sealed | full current architecture assessment | all C1 | FULL |
| D-FR-017 | C1 inventory/dependency/evidence/files/unknown/coverage/manifest/seal/handoff files | Architecture | sealed | structural corroboration and package integrity | all C1 | FULL; 9/9 hashes verified |
| D-FR-018 | `TransportERP.slnx` and all 10 `.csproj` | Solution/build | `2ec6cccf...` tree | projects, output types, targets, packages, references | client/C1/supply | FULL FOR STRUCTURAL PURPOSE |
| D-FR-019 | `TransportERP.Api/Program.cs` | API/security/sync | `2ec6cccf...` tree | auth, DI, sync endpoints, audit mapping | A-SEC/OFF; TB-F-002/004 | FULL FOR STATED PURPOSE |
| D-FR-020 | three Waybill API modules | API | same | endpoint surface, repeated helpers, repository registration | A-BIZ/ARCH; C1-PROB-006 | FULL |
| D-FR-021 | `ConcurrencySafeWaybillRepository.cs` | Persistence | same | SaveAsync and item mapper | A-ARCH-002, TB-F-020 | FULL |
| D-FR-022 | `WaybillAggregate.cs`, `P2WaybillEntities.cs`, Waybill contracts/application | Domain/data | same | Volume definition/read/write lineage | A-ARCH-002 | RELEVANT SECTIONS FULL |
| D-FR-023 | `SyncOperationService.cs`, `P1Entities.cs` | Sync/security | same | security binding, queue/version/conflict/audit | A-SEC-002, A-OFF, TB-F-003/004 | RELEVANT SECTIONS FULL |
| D-FR-024 | `TransportErpDbContext.cs`, P2 model customizers/entities | Database/tenant/accounting | same | filters, keys, constraints, model breadth | A-DB/ACCDB; TB-F-012; C1 | RELEVANT SECTIONS FULL; live DB not reviewed |
| D-FR-025 | `AuditEventService.cs` | Audit | same | hash and transaction behavior | A-AUD-006, TB-F-013 | RELEVANT SECTIONS FULL |
| D-FR-026 | `VoucherLifecycleService.cs`, finance application/persistence | Accounting | same | post/collection/accounting effect | A-ACCDB/BIZ-005; TB-F-005 | RELEVANT SECTIONS FULL |
| D-FR-027 | shipping application/persistence/API/domain files | Shipping | same | lifecycle endpoint/command boundary and structure | A-BIZ-001; TB-F-007; C1-PROB-003 | FULL FOR LIFECYCLE INVENTORY |
| D-FR-028 | all Desktop C# files and Desktop csproj | Desktop/screens | same | entry point, wiring, forms/catalog, duplication | runtime/screen/architecture | FULL STRUCTURAL; Windows runtime not run |
| D-FR-029 | three Mobile csproj/directories | Mobile | same | source/runtime inventory | A-RUNTIME-002, TB-F-001 | FULL STRUCTURAL |
| D-FR-030 | all 22 Tests C# files and Tests csproj | QA | same | test inventory, references, coverage configuration | QA/CI/C1-PROB-009 | FULL STRUCTURAL; tests not run |
| D-FR-031 | `.github/workflows/*` | CI/supply/release | same | triggers, matrices, artifacts, coverage/security gates | A-CI/QA/SUPPLY/RELEASE; TB-F-009/011/014 | FULL STATIC |
| D-FR-032 | P1/P2 acceptance/design/closeout registers and screen queue cited by A/B | Governance/UI/QA | same + v72 refs | authority, acceptance, screen identity | A-QA-002/SCR/KUR; TB-F-010/015/019 | FULL FOR CITED PURPOSE; external latest Library not reviewed |
| D-FR-033 | local Git refs/log/worktree list and remote selected refs | Git/preservation | inspection snapshot | temporal/authority/preservation reconciliation | A-PRES, TB-F-016 | FULL FOR LISTED REFS; external workspaces not exhaustive |

The register intentionally lists predecessor register groups where every package file was read for a common integrity/provenance purpose; individual hashes remain in their sealed manifests and TEAM-D evidence records.
