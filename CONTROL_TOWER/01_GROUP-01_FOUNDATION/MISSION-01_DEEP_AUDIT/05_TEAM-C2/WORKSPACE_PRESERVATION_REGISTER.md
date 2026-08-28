# TEAM-C2 Workspace Preservation Register

- Version: `v1.0`
- Purpose: carry sealed preservation requirements into every proposed architecture transition. This is not a merge or cleanup recommendation.

| Preservation ID | Asset / behavior | Classification | Linked evidence / target | Required state before implementation | Authority / risk |
|---|---|---|---|---|---|
| C2-PRES-001 | sealed TEAM-A/B/C1/D packages | SEALED AUDIT ARTIFACTS | C2-EV-001/002 | preserve immutable; later change requires REOPEN/new version/hash/seal | Control Tower; evidence lineage |
| C2-PRES-002 | `master@2ec6cccf...` assessed product snapshot | CURRENT CANDIDATE / SNAPSHOT | C2-EV-026/027 | keep SHA-bound baseline; do not call authoritative | repository/owner authority required |
| C2-PRES-003 | PR69 sealed and latest observed heads | UNMERGED / MOVING | C2-EV-026 | preserve each SHA; no evidence transfer; inspect before reuse | no blind merge/delete |
| C2-PRES-004 | WAVE-1, W0, P2-D and other listed unmerged lines | UNMERGED | D preservation register | keep until semantic disposition | owner for merge/delete |
| C2-PRES-005 | local heads/objects `3bc7f431...`, `7df4743e...` | LOCAL-ONLY P0 | C2-EV-013 | hash/bundle/preserve before any cleanup or tree move | owner; loss of valuable work |
| C2-PRES-006 | dirty-worktree evidence at `06146e0f...` and tracked PNG hash | LOCAL-ONLY / DIRTY P0 | C2-EV-013 | preserve/hash and identify ownership | owner; irreversible loss risk |
| C2-PRES-007 | existing migration chain/model snapshot/manual hardening | SNAPSHOT-PRESENT | C2-EV-023 | no rewrite/reorder/squash; forward-only governed transition | DB-GOV-001 |
| C2-PRES-008 | stored IDs/data meanings/precision, especially Waybill item `Volume` | DATA CONTRACT / P0 | C2-EV-012/028 | impact analysis, safe-copy evidence, semantic and regression parity | DB-GOV-001; data repair may require owner |
| C2-PRES-009 | CAS/idempotency/payload hash/serializable/concurrency controls | POSITIVE CONTROL | C2-EV-023 | regression evidence before refactor/split | security/data review |
| C2-PRES-010 | append-only audit/movement/finance mechanisms and triggers | POSITIVE CONTROL | C2-EV-023 | preserve or replace only with stronger DB/runtime parity | DB-GOV-001 |
| C2-PRES-011 | audit hash lineage | VERSIONED HISTORY | C2-EV-017/023 | backward verifier and version marker; no silent rehash | compliance/DB governance |
| C2-PRES-012 | tenant scope predicates/permission codes | CURRENT PARTIAL CONTROL | C2-EV-014/015 | retain until new server+DB controls pass bidirectional parity | security approval |
| C2-PRES-013 | Waybill/Shipping domain rules, contracts, 23 endpoints, tests | PARTIAL RUNTIME ASSET | C2-EV-019/023 | exact-SHA behavior/contract compatibility before module extraction | remediation review |
| C2-PRES-014 | Desktop forms, 19 screen IDs, RTL/design evidence | PROTOTYPE / CONTRACT ASSET | C2-EV-007/025 | canonical crosswalk, UI parity, no identity overwrite | screen authority/owner for destructive changes |
| C2-PRES-015 | Kurrasa v72 and historical/design versions | VERSION-BOUND GOVERNANCE | C2-EV-021/025 | preserve version/supersession and current offline guardrail | owner/authority update required |
| C2-PRES-016 | exact-SHA CI/test evidence and artifacts | SHA-BOUND EVIDENCE | C2-EV-020 | retain with SHA/environment; never transfer PASS | QA/release governance |
| C2-PRES-017 | apparently unused generic contracts/prototype types | UNKNOWN CONSUMERS | C2-EV-006/008 | consumer scan and compatibility/supersession plan before removal | `CANDIDATE FOR REMOVAL` only |

No preserved asset is automatically approved for merge, runtime use, or final architecture.
