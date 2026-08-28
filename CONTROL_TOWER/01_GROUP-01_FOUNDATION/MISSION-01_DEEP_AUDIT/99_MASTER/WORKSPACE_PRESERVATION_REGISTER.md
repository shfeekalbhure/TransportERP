# MASTER/GATE Workspace Preservation Register

| ID | Asset/behavior | Classification | Preservation requirement | Authority/risk |
|---|---|---|---|---|
| M-PRES-001 | A/B/C1/D/C2/E sealed packages and v1.0 lineage | SEALED AUDIT EVIDENCE | immutable bytes, manifests, hashes, seals, handoffs, supersession chain | later change requires REOPEN/new version |
| M-PRES-002 | this Master/Gate package | SEALED MASTER EVIDENCE | immutable after Control Tower acceptance | later change requires REOPEN/new version |
| M-PRES-003 | `master@2ec6cccf...` assessed snapshot | CURRENT CANDIDATE / SNAPSHOT | retain exact SHA; do not call authoritative | authority unresolved |
| M-PRES-004 | PR69/WAVE-1/W0/P2-D heads | UNMERGED/MOVING | retain exact observed SHAs pending semantic review | no blind merge/delete/evidence transfer |
| M-PRES-005 | `3bc7f431...`, `7df4743e...`, dirty evidence `06146e0f...` | LOCAL-ONLY P0 | hash/bundle/preserve, identify owner and intent | owner required before destructive action |
| M-PRES-006 | migration lineage, model snapshot, manual hardening | DATABASE LINEAGE | no rewrite/reorder/squash; forward governed changes only | DB-GOV-001 |
| M-PRES-007 | `Volume` meaning/precision/nullability | DATA CONTRACT P0 | safe-copy impact analysis and parity regression | data repair/change requires authority |
| M-PRES-008 | CAS/idempotency/payload hashes/transactions/constraints/triggers | POSITIVE CONTROLS | exact-SHA parity before refactor/split | do not weaken while closing gaps |
| M-PRES-009 | audit hash lineage and append-only history | COMPLIANCE HISTORY | backward verifier and version marker | no silent rehash/rewrite |
| M-PRES-010 | tenant predicates and permission codes | PARTIAL POSITIVE CONTROLS | retain until stronger control passes negative parity | security regression risk |
| M-PRES-011 | Waybill/Shipping behavior, contracts, endpoints, tests | PARTIAL RUNTIME ASSETS | dependency/contract/runtime parity before extraction | no big-bang rewrite |
| M-PRES-012 | Desktop forms, 19 screen IDs, RTL/design evidence | PROTOTYPE/VERSIONED ASSETS | canonical crosswalk and parity before consolidation | no destructive identity overwrite |
| M-PRES-013 | Kurrasa/document versions | VERSIONED REQUIREMENTS EVIDENCE | retain exact versions and supersession metadata | no silent authority promotion |
| M-PRES-014 | exact-SHA CI/test/artifact logs | SHA-BOUND EVIDENCE | retain SHA, environment, logs; no PASS transfer | release assurance |

Preservation is not merge, release, or implementation approval. Delete/rebase/force-push/destructive cleanup, Production/data changes, and irreversible actions require explicit owner authority.
