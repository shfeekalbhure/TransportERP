# TEAM-E Files Reviewed Register

- Status: `FINAL v1.1 — SEALED`
- Default ref for product files: `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`.

| File ID | Path/group | Project/domain | Reviewer/purpose | Sections | Findings | Coverage / limit |
|---|---|---|---|---|---|---|
| `E-FILE-001` | mandatory governance and full MISSION-01 command | Governance | coordinator; contract/state/reopen/seal rules | full files | all | FULL FOR STATED PURPOSE |
| `E-FILE-002` | TEAM-A/B/C1 manifests, checksums, seals, handoffs, main reports | Audit inputs | governance + domain reviewers | full integrity records; selected report findings | all P0/P1 | FULL for integrity/critical review; not every prose statement |
| `E-FILE-003` | TEAM-D full package | Reconciliation | all reviewers | report, 62-row Crosswalk, sources/evidence/files/unknowns/coverage/preservation/formation/manifest/seal/handoff | all | FULL FOR STATED PURPOSE |
| `E-FILE-004` | TEAM-C2 full package | Target architecture | all reviewers | proposal/tree/maps/26-change crosswalk/DB constraints and registers | all | FULL FOR STATED PURPOSE |
| `E-FILE-005` | `ConcurrencySafeWaybillRepository.cs` | Waybill/DB | DB + architecture; Volume path | save/update/mappers | A-ARCH-002 | FULL FOR STATED PURPOSE |
| `E-FILE-006` | Waybill Domain/contracts/entities/model/migration/tests | Waybill/DB | DB reviewer; Volume semantic chain | Volume declarations/mapping/tests | A-ARCH-002 | FULL FOR STATED PURPOSE; runtime not run |
| `E-FILE-007` | `Program.cs`, Waybill API modules | API/security/sync | security reviewer; JWT/context/permission/routes | auth, sync, audit, helpers | security/offline/P2 duplication | FULL FOR STATED PURPOSE |
| `E-FILE-008` | `SyncOperationService.cs`, sync entities/tests | Offline/security/DB | security reviewer; enqueue/lifecycle/ownership/atomicity | full service critical methods | A-OFF-001/002, TB-F-004 | FULL FOR STATED PURPOSE; lifecycle API exposure absent |
| `E-FILE-009` | `TransportErpDbContext.cs`, P1/P2 model customizers/entities | DB/tenant/accounting | DB/security reviewers; keys/FKs/filters/invariants | relevant configurations/entities | DB/RBAC/accounting | FULL FOR STATED PURPOSE; live DB blocked |
| `E-FILE-010` | `VoucherLifecycleService.cs`, finance persistence/tests | Accounting | accounting reviewer; posting/link/audit | create/approve/post/cancel/collection | accounting P1 | FULL FOR STATED PURPOSE |
| `E-FILE-011` | `AuditEventService.cs`, audit entities/tests/migrations | Audit/compliance | DB/security; hash/append/atomicity | hash/append/query/fields | A-AUD-006, TB-F-013 | FULL FOR STATED PURPOSE |
| `E-FILE-012` | Desktop/Mobile csproj/source inventory | Clients/runtime | architecture/release; executability | project properties/source counts | runtime P1 | FULL FOR STATED PURPOSE; runtime not run |
| `E-FILE-013` | `.github/workflows/*`, csproj/solution/build config | QA/CI/supply/release | architecture/release; evidence chain | restore/build/client/artifact steps | QA/CI/release/supply | FULL for repository config; external controls blocked |
| `E-FILE-014` | shipping API/application/domain/persistence/forms | Shipping | architecture/domain; lifecycle extent | endpoints/states/forms | A-BIZ-001 | PARTIAL representative trace; accepted sealed inventory used for breadth |
| `E-FILE-015` | repository-wide Ticketing/Passenger search and project tree | Ticketing | architecture/domain; existence check | all tracked source names/content search | A-BIZ-002/TB-F-006 | FULL FOR STATED PURPOSE on snapshot |
| `E-FILE-016` | `TransportErpDbContextFactory.cs` and C1 claims | DB/governance | DB reviewer; fallback correction | full factory and exact C1 lines | E-REOPEN-001 | FULL FOR STATED PURPOSE |
| `E-FILE-017` | Git status/refs/worktree list/product diff | Preservation/authority | coordinator/governance; continuity and non-authority | current observable local state | A-PRES-001/line unknown | PARTIAL; external machines/sessions unknown |
| `E-FILE-018` | `03_TEAM-C1/v1.1/` full corrected package | Architecture/governance | coordinator; correction, integrity, supersession | all 15 package files; detached list validates 14 | C1-CORR-001 | FULL FOR STATED PURPOSE |
| `E-FILE-019` | `04_TEAM-D/v1.1/` full corrected package | Reconciliation/governance | coordinator; corrected chronology/schema/new evidence | all 15 package files; full 64-row Crosswalk; detached list validates 14 | all reconciled rows plus D-SEC-SYNC-001 | FULL FOR STATED PURPOSE |
| `E-FILE-020` | `05_TEAM-C2/v1.1/` full corrected package | Target architecture/governance | coordinator; corrected proposal/crosswalk/constraints | all 17 package files; full 27-row crosswalk; detached list validates 16 | all target-change rows incl. C2-TARGET-027 | FULL FOR STATED PURPOSE |
| `E-FILE-021` | `06_TEAM-E/` sealed v1.0 package | TEAM-E assurance/governance | coordinator; immutable baseline and contradiction recheck | all 16 package files; full 39-row P0/P1 and 8-row P2/P3 matrices | E-REOPEN-007 | FULL FOR REISSUE PURPOSE; 15/15 detached hashes pass |

Review breadth is bounded as stated per row; no unexecuted runtime or inaccessible external environment is implied reviewed.
