# MASTER/GATE v2.0 Workspace Preservation Register

| ID | Asset | Classification | Required preservation | Forbidden without later authority |
|---|---|---|---|---|
| `M2-PRES-001` | authoritative `master@2ec6cccf...` | CURRENT BASELINE | retain ref/SHA/tree in evidence | silent baseline substitution |
| `M2-PRES-002` | MASTER/GATE v1.0 | HISTORICAL SEALED | retain all bytes/hashes | overwrite/delete |
| `M2-PRES-003` | accepted A/B/C1v1.1/Dv1.1/C2v1.1/Ev1.1 | SEALED INPUTS | retain full lineage | edit/reseal silently |
| `M2-PRES-004` | PR69 `601f2d1c...` | UNMERGED FINAL CANDIDATE | preserve exact ref/tree and evidence | merge/delete/rebase/force-push/adopt blindly |
| `M2-PRES-005` | WAVE-1/W0/P2-D and registered local objects/dirty evidence | UNMERGED / LOCAL-ONLY | hash/bundle/inventory before any cleanup | delete/prune/reset/history rewrite |
| `M2-PRES-006` | migration lineage/model snapshot/live data meaning | DB PRESERVATION | DB-GOV-001 impact/recovery proof | reorder/drop/mutate/data repair |
| `M2-PRES-007` | Volume semantics and affected data | P0 DATA SCOPE | separate code fix, impact inventory, safe-copy recovery | derive/overwrite/repair by assumption |
| `M2-PRES-008` | CAS/idempotency/constraints/triggers/audit hashes | CONTROL SCOPE | regression and backward compatibility evidence | weaken/remove during remediation |

Preservation is not merge, adoption, release, or implementation approval.
