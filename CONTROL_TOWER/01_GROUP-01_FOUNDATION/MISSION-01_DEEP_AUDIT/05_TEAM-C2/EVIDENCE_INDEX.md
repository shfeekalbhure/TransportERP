# TEAM-C2 Evidence Index

- Version: `v1.0`
- Collection: `2026-08-28T02:05:10Z–2026-08-28T02:19:00Z`
- Assessed product snapshot: `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`
- Governing line status: `UNKNOWN — REQUIRES OWNER/REPOSITORY VERIFICATION`

| Evidence ID | Target item(s) | Source ID | Type / exact location | Result and limit |
|---|---|---|---|---|
| C2-EV-001 | all | C2-SRC-005 | `04_TEAM-D/AUDIT_OUTPUT_MANIFEST.md`, hashes, seal, handoff | 62-record reconciliation package centrally accepted; does not resolve authority line |
| C2-EV-002 | all current facts | C2-SRC-005 | D report §§1–11, D Crosswalk | governing reconciled facts and allowed determinations |
| C2-EV-003 | C2-TARGET-001/009 | C2-SRC-008/009 | C1 solution/dependency reports; `TransportERP.slnx`; all csproj | 10 flat projects and acyclic direct reference graph confirmed on snapshot |
| C2-EV-004 | C2-TARGET-002 | C2-SRC-005/008 | `C1-PROB-001/002`, `D-EV-022` | broad persistence/DbContext concentration confirmed statically |
| C2-EV-005 | C2-TARGET-003 | C2-SRC-005/008 | `C1-PROB-003`, `ShippingExecutionPersistence.cs` | mixed shipping store responsibility confirmed; no refactor authorized |
| C2-EV-006 | C2-TARGET-004 | C2-SRC-005/008 | `C1-PROB-004`, `P1InMemoryBaseline.cs` | parallel prototype semantics confirmed; external consumers unknown |
| C2-EV-007 | C2-TARGET-005/006 | C2-SRC-005/008 | `D-EV-014`, C1 inventory | Desktop disconnected Library; Mobile source-empty placeholders |
| C2-EV-008 | C2-TARGET-007/008 | C2-SRC-005/008 | `C1-PROB-006/007`, `D-EV-022/023` | repeated API mechanics and misplaced HTTP/persistence types confirmed |
| C2-EV-009 | C2-TARGET-010 | C2-SRC-005/008 | `C1-PROB-009`, tests project inventory | one mixed test assembly; exact-target execution unknown |
| C2-EV-010 | C2-TARGET-011 | C2-SRC-005/008 | `C1-PROB-011`, `D-EV-019` | SDK/package/source locks and supply assurance incomplete |
| C2-EV-011 | C2-TARGET-012 | C2-SRC-005/008 | `C1-PROB-012`, Desktop forms inventory | multi-form source and repeated UI mechanics confirmed |
| C2-EV-012 | C2-TARGET-013 | C2-SRC-005/009 | `D-EV-006`; `ConcurrencySafeWaybillRepository.cs:76-87,119-137` | mapper omission of `Volume` confirmed static P0; affected rows/runtime unknown |
| C2-EV-013 | C2-TARGET-014 | C2-SRC-005/006 | `D-EV-021`, D/A preservation registers | local-only value/loss risk confirmed; semantic merge merit unknown |
| C2-EV-014 | C2-TARGET-015 | C2-SRC-005 | `D-EV-007/008`, A-SEC-001/002, TB-F-002/003 | claim-driven auth and incomplete user/tenant/device binding confirmed |
| C2-EV-015 | C2-TARGET-016 | C2-SRC-005 | `D-EV-009`, A-DB-003/004 | tenant DB/RBAC enforcement incomplete; live roles/RLS unknown |
| C2-EV-016 | C2-TARGET-017 | C2-SRC-005 | `D-EV-013`, A-OFF-001/002, TB-F-004 | server queue foundation only; end-to-end offline absent on snapshot |
| C2-EV-017 | C2-TARGET-018 | C2-SRC-005 | `D-EV-010`, A-AUD-006, TB-F-013 | audit hash/atomicity incomplete; runtime failure injection unknown |
| C2-EV-018 | C2-TARGET-019/020 | C2-SRC-005 | `D-EV-012`, A-ACCDB-007/A-BIZ-005, TB-F-005/012 | status-only posting and incomplete ledger bridge confirmed |
| C2-EV-019 | C2-TARGET-021/022/023 | C2-SRC-005 | `D-EV-015`, A-BIZ-001/002, TB-F-006/007 | shipping partial; Ticketing and reporting absent on snapshot |
| C2-EV-020 | C2-TARGET-024 | C2-SRC-005 | `D-EV-016/018/024`; release/CI records | exact-target/release/recovery evidence incomplete or blocked |
| C2-EV-021 | C2-TARGET-025 | C2-SRC-005/012 | D source/line register; D-BLK-001/006 | current line and latest authority/crosswalk unresolved |
| C2-EV-022 | C2-TARGET-026 | C2-SRC-005/007 | `D-EV-026`, TB-F-018, BLK-B-001 | single-session assurance limitation preserved |
| C2-EV-023 | preservation | C2-SRC-005 | `D-EV-025`, D-PRES-013..015 | CAS/idempotency/serializable/constraints/triggers/migration/hash lineage to preserve |
| C2-EV-024 | data/privacy | C2-SRC-005 | `D-EV-020`, A-PRIV-008, TB-F-008 | sensitive surfaces confirmed; infrastructure controls unknown |
| C2-EV-025 | screen/UI | C2-SRC-005/006/007 | `D-EV-017`, A-SCR-001, TB-F-015 | design/runtime separation and screen-ID conflict confirmed |
| C2-EV-026 | temporal | C2-SRC-005 | D source/line register | default master is current candidate only; PR69 moving/unmerged; local assets preservation-only |
| C2-EV-027 | direct continuity | C2-SRC-009 | `git diff 2ec6cccf... -- . :(exclude)CONTROL_TOWER/**` | no product delta in working snapshot; proves continuity, not authority |
| C2-EV-028 | direct P0 trace | C2-SRC-009 | Domain/entity/contract/migration/read paths containing `Volume` vs mapper omission | corroborates preservation of Volume across target data contract; no runtime executed |

Recommendations in TEAM-C2 outputs are not evidence of implementation.
