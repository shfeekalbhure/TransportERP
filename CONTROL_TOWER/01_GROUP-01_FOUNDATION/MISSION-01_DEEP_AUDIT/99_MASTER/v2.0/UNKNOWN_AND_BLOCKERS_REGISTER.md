# MASTER/GATE v2.0 Unknown and Blockers Register

No item below blocks the start of planning. Each blocks only its named later action until resolved.

| ID | Evidence / unknown | Why it does not block MISSION-02 | Can resolve now? | Required non-destructive action | Later gate blocked |
|---|---|---|---|---|---|
| `M2-BLK-001` | local `dotnet` absent; master CI partial | static/current risk portfolio and exact tree are known; M02 can plan the matrix | No in current runtime | disposable exact-SHA restore/build/test/migrate/boot with retained logs | affected implementation/release |
| `M2-BLK-002` | live schema/data/roles/RLS/backups and affected Volume rows inaccessible | plan can separate code correction, impact query, safe copy, and repair decision | No; no authorized DB source | DB-GOV-001 read-only query design, approved execution, safe-copy rehearsal | Volume/data repair and DB implementation |
| `M2-BLK-003` | IdP/session/revocation/device environment unavailable | static security defects and required negatives are known | Partly: source only completed | request redacted config; define exact negative matrix; run in authorized sandbox | security implementation/release |
| `M2-BLK-004` | Production privacy/deploy/rollback/recovery evidence unavailable | plan can require evidence and prohibit release until supplied | No | non-secret topology/config/runbook/artifact/backup evidence and drills | Production/release |
| `M2-BLK-005` | latest canonical Kurrasa/screen/offline authority not established | planning may put authority resolution before affected scope | No from repository alone | register immutable canonical version and requirement/screen/operation crosswalk | affected UI/offline implementation |
| `M2-BLK-006` | local/external workspace ownership and full inventory incomplete | preservation-first planning is possible and mandatory | Partly: registered assets preserved | hash/bundle/inventory without cleanup; owner disposition only before destructive action | delete/cleanup/merge/history rewrite |
| `M2-BLK-007` | accounting mappings, period, SoD, reversal and subledger authority incomplete | M02 can draft alternatives and an authority checklist without choosing by guess | Partly: source constraints known | canonical requirement record + reviewed accounting ADR | accounting implementation |
| `M2-BLK-008` | cross-module transaction/UoW owner unresolved (`E-BLK-013`) | drafting/reviewing the ADR is legitimate first-wave planning work | Yes analytically | produce reviewed ADR preserving module ownership and atomic journal/audit/outbox invariants | affected implementation plan approval |
| `M2-BLK-009` | offline business-operation authority conflicts with version-bound `OFFLINE_WRITE=0`; PR69 enables five candidate actions | candidate and current classifications are known; plan can reconcile authority before adoption | No authority source in repo | operation-by-operation authority matrix; keep Production offline disabled | offline-write implementation/activation |
| `M2-BLK-010` | Sync lifecycle route/caller/override policy incomplete on master | known static P1 can be planned without assuming exposure | Yes for source callers; policy external | enumerate callers/routes; design owner binding or audited override; negative tests | safe Sync exposure |
| `M2-BLK-011` | EF design-time execution not run | source fail-closed fact is known; execution is a planned verification | No local dotnet | disposable controlled connection value and exact-SHA EF command | migration/tooling claim |
| `M2-BLK-012` | PR69 semantic adoption not independently approved | exact candidate is fully identified and preserved | Yes analytically | finding-by-finding delta/adoption matrix; no merge | any adoption/merge proposal |
| `BLK-B-001` | TEAM-B single-session provenance | independently mitigated for audit closure; retained as limitation | Resolved for M01 | retain in planning assurance narrative | none for M02 start |

`OWNER DECISION REQUIRED` is not active. It becomes applicable only if a later proposed action is destructive, Production-affecting, irreversible, data-loss-capable, history-rewriting, or expressly owner-reserved.
