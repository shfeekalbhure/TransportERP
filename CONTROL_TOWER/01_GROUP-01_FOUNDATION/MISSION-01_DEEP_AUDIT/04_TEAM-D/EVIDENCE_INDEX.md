# TEAM-D Evidence Index

Collection time unless stated otherwise: `2026-08-28T01:52:48Z–2026-08-28T02:02:00Z`. Collector: `TEAM-D coordinator` with recorded bounded read-only reviewers. Product-source evidence is bound to assessed snapshot `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`; authoritative product line remains unknown.

| Evidence ID | Finding(s) | Source ID | Type | Exact location / ref | Result and limit |
|---|---|---|---|---|---|
| D-EV-001 | all A | D-SRC-002 | Artifact integrity | `01_TEAM-A/AUDIT_OUTPUT_MANIFEST.md` and sidecar | 13/13 entries verified; report SHA `e64c66f...`; proves package bytes, not every claim |
| D-EV-002 | all B | D-SRC-003 | Artifact integrity | `02_TEAM-B/AUDIT_OUTPUT_SHA256.txt` | 13/13 entries verified; report SHA `51b92496...`; `BLK-B-001` retained |
| D-EV-003 | all C1 | D-SRC-004 | Artifact integrity | `03_TEAM-C1/TEAM-C1_REPORT_SEAL.md` | 9/9 entries verified; report SHA `ef59d438...` |
| D-EV-004 | temporal/line | D-SRC-005/006 | Git tree | `git diff 8a36f88b..TEAM-D-HEAD -- . :(exclude)CONTROL_TOWER/**`; `master@2ec6cccf...` | no product delta; proves assessed-tree continuity, not authority |
| D-EV-005 | line candidates | D-SRC-007 | Remote Git | `git ls-remote --symref origin` selected refs | default master `2ec6cccf...`; governance `59b3812b...`; PR69 `9c9cfdb7...`; observation only |
| D-EV-006 | A-ARCH-002, TB-F-020 | D-SRC-005 | Direct code | `ConcurrencySafeWaybillRepository.cs:76-87,119-137`; `WaybillAggregate.cs`; `P2WaybillEntities.cs`; `WaybillApiModule.cs` | active save mapper omits Volume after delete/reinsert; runtime/data population not tested |
| D-EV-007 | A-SEC-002, TB-F-003 | D-SRC-005 | Direct code | `SyncOperationService.cs:346-368`; `Program.cs` sync context | active user checked by ID/status, not bound to user Company/Branch; IdP exploitability unknown |
| D-EV-008 | A-SEC-001, TB-F-002 | D-SRC-005 | Direct code | `TransportERP.Api/Program.cs`, Waybill API permission helpers | JWT claim-driven resource-server authorization; external IdP/session controls unknown |
| D-EV-009 | A-DB-003/004, TB-F-003/012 | D-SRC-005/009 | Code/database model | `TransportErpDbContext.cs:125-301`; P1/P2 entities/model customizers | soft-delete filters and partial constraints; systemic tenant DB enforcement/live DB unknown |
| D-EV-010 | A-AUD-006, TB-F-013 | D-SRC-005 | Direct code | `AuditEventService.cs:138-153`; `P1Entities.cs:272-290`; sync/waybill commit paths | hash omits persisted fields; transaction atomicity partial; failure injection not run |
| D-EV-011 | A-DB-005 | D-SRC-005/009 | Code/migration scan | finance interceptor and migrations | application append-only guard found; live DB/roles/raw-SQL behavior inaccessible |
| D-EV-012 | A-ACCDB-007, A-BIZ-005, TB-F-005/012 | D-SRC-005 | Direct code | `VoucherLifecycleService.cs:107-150`; journal mappings; finance application/persistence | voucher post is status-only; complete balanced posting runtime not found |
| D-EV-013 | A-OFF-001/002, TB-F-004 | D-SRC-005 | Direct code | `Program.cs:78-145`; `SyncOperationService.cs`; `P1Entities.cs:293-330`; client inventory | server queue/state foundation; no client outbox/worker/executor/PoP runtime |
| D-EV-014 | A-RUNTIME-001/002, TB-F-001, C1-PROB-005 | D-SRC-005 | Project/source inventory | Desktop/Mobile csproj and source counts | Desktop Library with 5 C# files/no entry point; Mobile has zero C# files |
| D-EV-015 | A-BIZ-001/002, TB-F-006/007 | D-SRC-005 | API/domain scan | Waybill/Shipping API modules; repository-wide ticket/return/claim/customs search | shipping ends at trip start; ticketing and later lifecycle runtime absent on snapshot |
| D-EV-016 | A-QA-001, A-CI-001, TB-F-011 | D-SRC-002/003/008 | CI/environment | sealed GitHub evidence; local `command -v dotnet` | SHA-bound product CI exists; governance SHA no checks; local execution blocked |
| D-EV-017 | A-QA-002, A-SCR-001, TB-F-015/019 | D-SRC-002/003/011 | Documentation/source | acceptance registers, screen queue/specs, Desktop catalog | contracts/design not runtime; cited screen-ID conflict confirmed; latest external authority partial |
| D-EV-018 | A-RELEASE-001, TB-F-009 | D-SRC-005/007/010 | Repository/remote/environment | tags/releases/workflows/package/deploy inventory | no repo release chain; external deployment/recovery inaccessible |
| D-EV-019 | A-SUPPLY-001, TB-F-014/021, C1-PROB-011 | D-SRC-005 | Build configuration | all csproj/workflows; absence of global.json/central packages/lockfile | reproducibility/assurance gaps confirmed; resolved graph/advisories unknown |
| D-EV-020 | A-PRIV-008, TB-F-008 | D-SRC-005/010 | Code/data surface | P1/P2 entities, audit/sync payloads and API | sensitive text/JSON surfaces confirmed; environmental encryption/retention unknown |
| D-EV-021 | A-PRES-001, TB-F-016 | D-SRC-002/006 | Git/preservation | A preservation register/appendix; local objects/refs/worktrees | local-only value/loss risk confirmed; semantic merge disposition unknown |
| D-EV-022 | C1-PROB-001..012 | D-SRC-004/005 | Structural source | `.slnx`, 10 csproj, ProjectReferences, DbContext, stores, forms, tests | all 12 structural problems corroborated; no priority invented |
| D-EV-023 | A-ARCH-005/006/012 | D-SRC-005 | Direct source/layout | Desktop forms, API modules, repository tree | integration, duplication and layout facts confirmed; target design not decided |
| D-EV-024 | A-QA-005 | D-SRC-005 | CI/workflow scan | Tests csproj and `.github/workflows` | no coverage threshold/upload/TRX retention gate; actual coverage unknown |
| D-EV-025 | A-DB-INFO-009 | D-SRC-005 | Direct code/migrations | CAS/idempotency/serializable paths, constraints, audit/shipping triggers | positive controls confirmed statically and marked for preservation |
| D-EV-026 | TB-F-018 / BLK-B-001 | D-SRC-003 | Governance record | TEAM-B formation/seal/handoff/unknown register | single-session limitation confirmed; does not invalidate package integrity |

No copied artifact hash is substituted for a Git SHA. Every runtime/environmental conclusion is bounded by access and execution status.
