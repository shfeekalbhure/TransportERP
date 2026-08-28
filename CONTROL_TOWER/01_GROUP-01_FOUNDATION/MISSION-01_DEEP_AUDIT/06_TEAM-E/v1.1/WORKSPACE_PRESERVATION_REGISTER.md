# TEAM-E Workspace Preservation Register

- Status: `FINAL v1.1 — SEALED`
- Purpose: preserve evidence, source/data semantics, and local/unmerged work through reopen and later planning. This is not a merge recommendation.

| ID | Asset/behavior | Classification | Required state | Risk/authority |
|---|---|---|---|---|
| `E-PRES-001` | TEAM-A/B/C1/D/C2 sealed v1.0 packages | SEALED AUDIT ARTIFACTS | preserve immutable with original hashes | no silent correction; retain provenance defects as history |
| `E-PRES-002` | accepted C1/D/C2 v1.1 packages | GOVERNING SUCCESSORS | retain alongside supersession/reopen links and verified hashes | do not erase v1.0 lineage |
| `E-PRES-003` | TEAM-E sealed review/reopen evidence | SEALED AUDIT WORK PRODUCT | preserve immutable after seal; changes require governed REOPEN/new version | silent edits would invalidate the detached checksum set |
| `E-PRES-004` | `master@2ec6cccf...` assessed product snapshot | CURRENT CANDIDATE / SNAPSHOT | keep exact-SHA baseline; do not call authoritative | authority unknown |
| `E-PRES-005` | listed PR69/WAVE-1/W0/P2-D heads | UNMERGED/MOVING | retain each SHA until semantic review | no blind evidence transfer/merge/delete |
| `E-PRES-006` | local `3bc7f431...`, `7df4743e...`, dirty `06146e0f...` evidence | LOCAL-ONLY P0 | hash/bundle/preserve and assign ownership | owner required before destructive action |
| `E-PRES-007` | migration lineage/model snapshot/manual hardening | DATABASE LINEAGE | no rewrite/reorder/squash; forward-only governed change | DB-GOV-001/data/recovery risk |
| `E-PRES-008` | `Volume` meaning/precision/nullability across contracts/data | DATA CONTRACT P0 | impact + regression + safe-copy data assessment | DB-GOV-001; data repair may require owner |
| `E-PRES-009` | CAS/idempotency/payload hash/serializable/constraints/triggers | POSITIVE CONTROLS | parity tests before refactor/split | weakening creates security/data risks |
| `E-PRES-010` | audit hash lineage/append-only history | VERSIONED COMPLIANCE HISTORY | backward verifier/new version marker | no silent rehash/rewrite |
| `E-PRES-011` | tenant scope predicates/permission codes | PARTIAL POSITIVE CONTROLS | keep until stronger bidirectional parity evidence | avoid regression while closing gaps |
| `E-PRES-012` | Waybill/Shipping behavior/contracts/endpoints/tests | PARTIAL RUNTIME ASSETS | exact-SHA contract/runtime parity before extraction | module moves can break behavior |
| `E-PRES-013` | Desktop forms/screen IDs/RTL/design and Kurrasa versions | PROTOTYPE/VERSIONED CONTRACT ASSETS | canonical crosswalk and supersession evidence | no destructive identity overwrite |
| `E-PRES-014` | exact-SHA CI/test/artifact evidence | SHA-BOUND EVIDENCE | retain SHA/environment/logs; no PASS transfer | release assurance |
| `E-PRES-015` | rejected TEAM-E v1.0 package | SEALED REISSUE PREDECESSOR | preserve all 16 files and original detached hashes unchanged | semantic inconsistency requires v1.1; never silently repair v1.0 |

Every destructive cleanup, Production/data action, or irreversible change remains outside TEAM-E authority.
