# TEAM-C2 Files Reviewed Register

- Version: `v1.0`
- “FULL FOR STATED PURPOSE” means read for target-design derivation; it does not claim runtime execution.

| File ID | Path / group | Domain | Ref / purpose | Linked targets | Coverage / limit |
|---|---|---|---|---|---|
| C2-FR-001 | `CONTROL_TOWER/README.md` | Governance | workspace rules | all | FULL FOR STATED PURPOSE |
| C2-FR-002 | `00_GOVERNANCE/OWNER_DELEGATION_AND_OPERATING_DIRECTIVE.md` | Governance | authority/seal/handoff/DB rules | all | FULL |
| C2-FR-003 | `00_GOVERNANCE/CONTROL_TOWER_AUTONOMOUS_SUPERVISION_PROTOCOL.md` | Governance | analytical sequence/unknown rules | all | FULL |
| C2-FR-004 | `00_GOVERNANCE/CONTROL_TOWER_TEAM_DIRECTIVES.md` | Governance | TEAM-C2 START; TEAM-E WAIT | all | FULL |
| C2-FR-005 | `00_GOVERNANCE/DECISIONS/DB-GOV-001.md` | Database governance | no DB execution | 002,013,016,018,019 | FULL |
| C2-FR-006 | full MISSION-01 command | Mission contract | sections 1–43 including C2/tree/seal/preservation | all | FULL |
| C2-FR-007 | `05_TEAM-C2/CURRENT_DIRECTIVE.md`, `START_ORDER.md` | Team order | authorized design scope | all | FULL |
| C2-FR-008 | complete `04_TEAM-D/` package | Reconciliation | report, 62 crosswalk, evidence, unknowns, preservation, coverage, source/line, seal/handoff | all | FULL FOR STATED PURPOSE |
| C2-FR-009 | `01_TEAM-A/TEAM-A_INDEPENDENT_DEEP_AUDIT_REPORT.md` + evidence/unknown/preservation registers | Independent audit | P0/P1, current facts, positive controls | 013..025 | FULL FOR TARGET INPUTS |
| C2-FR-010 | `02_TEAM-B/TEAM-B_INDEPENDENT_DEEP_AUDIT_REPORT.md` + evidence/unknown/preservation registers | Independent audit | DB/security/offline/runtime/release and BLK-B-001 | 010..026 | FULL FOR TARGET INPUTS |
| C2-FR-011 | `03_TEAM-C1/TEAM-C1_CURRENT_ARCHITECTURE_ASSESSMENT.md` | Architecture | 10-project/current structure/problems | 001..012 | FULL |
| C2-FR-012 | `03_TEAM-C1/TEAM-C1_ARCHITECTURE_INVENTORY.md` | Architecture | projects/modules/screens/data | 001..012,021..023 | FULL |
| C2-FR-013 | `03_TEAM-C1/TEAM-C1_DEPENDENCY_MAPPING.md` | Dependencies | graph/coupling/circularity | 001..009 | FULL |
| C2-FR-014 | C1 evidence/files/unknown/coverage/source registers | Architecture evidence | trace and limits | 001..012 | FULL FOR STATED PURPOSE |
| C2-FR-015 | `TransportERP.slnx` and ten `.csproj` | Direct structure | project list/references/packages | 001,009,010 | FULL FOR STRUCTURE |
| C2-FR-016 | `ConcurrencySafeWaybillRepository.cs` and Volume domain/entity/contract/read/migration paths | Data integrity | mapper/data-contract trace | 013 | FULL FOR P0 TRACE; no execution |
| C2-FR-017 | `TransportErpDbContext.cs`, model customizers, migration inventory | Database | concentration/lineage | 002,016,018,019 | PARTIAL; live DB blocked |
| C2-FR-018 | `ShippingExecutionPersistence.cs` | Shipping architecture | mixed store responsibility | 003,021 | FULL FOR RESPONSIBILITY TRACE |
| C2-FR-019 | `P1InMemoryBaseline.cs` | Prototype | parallel semantics | 004 | FULL FOR CLASSIFICATION |
| C2-FR-020 | Desktop csproj/forms/catalog files | Desktop/UI | executable status/forms/shared mechanics | 005,012,025 | FULL FOR STATIC PURPOSE; no runtime |
| C2-FR-021 | Mobile project directories/csproj | Mobile | zero-source placeholder classification | 006 | FULL FOR STATIC PURPOSE |
| C2-FR-022 | API `Program.cs` and Waybill modules via sealed evidence/direct symbol inventory | API/Security | host/claims/repeated boundaries | 007,008,015 | PARTIAL DIRECT; runtime/IdP blocked |
| C2-FR-023 | Sync/audit/accounting persistence files via sealed evidence | Security/Offline/Accounting | target constraints | 015..020 | FULL FOR EVIDENCE-BOUND DESIGN; no DB/runtime |
| C2-FR-024 | test/workflow/package inventories in sealed packages | QA/CI/Supply | proposed test/release topology | 010,024 | FULL FOR DOCUMENTARY PURPOSE |
| C2-FR-025 | `03_DATABASE_GOVERNANCE/*.md` | DB governance | current/proposal registers and constraints | 002,013,016,018,019 | FULL; registers contain no execution authority |

No file count is used as a substitute for evidence quality.
