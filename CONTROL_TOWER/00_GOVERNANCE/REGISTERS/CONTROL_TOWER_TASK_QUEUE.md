# CONTROL TOWER TASK QUEUE

| Order | Mission | Team | Prerequisite | Required Output | State | Blocker |
|---:|---|---|---|---|---|---|
| 1 | MISSION-01 | TEAM-A/TEAM-B/TEAM-C1 | Audit baseline + assignments | Independent reports + current architecture | SEALED | A/B sealed; C1 v1.1 accepted; historical versions preserved |
| 2 | MISSION-01 | TEAM-D | A/B/C1 accepted | Complete reconciliation package | SEALED | D v1.1 accepted; historical v1.0 preserved |
| 3 | MISSION-01 | TEAM-C2 | TEAM-D sealed | Target architecture proposal | SEALED | C2 v1.1 accepted |
| 4 | MISSION-01 | TEAM-E | TEAM-C2 sealed | Multidisciplinary advisory package | SEALED | E v1.1 accepted; `BLK-B-001` retained in assurance narrative |
| 5 | MISSION-01 | MASTER/GATE REVALIDATION | Owner-authorized authoritative line | Revalidated Master/Gate package on exact authoritative SHA | SEALED — STOP | v2.0 complete; all 14 hashes verified; gate `READY FOR REMEDIATION PLANNING`; v1.0 preserved |
| 6 | MISSION-02 | Planning Team | Revalidated MISSION-01 gate = `READY FOR REMEDIATION PLANNING` | Remediation plan | SEALED — DELIVERED — STOP | v1.2 remotely delivered; 64/64 findings; 8/8 workstreams PLANNED; 20 packages; W0–W8; DB-GOV paths retained |
| 7 | MISSION-03 | Execution Team | MISSION-02 sealed | Implemented changes/evidence | IN PROGRESS — OPEN — NOT SEALED — DBP-002 CORRECTION CYCLE ACTIVE — EXACT-HEAD PASS NOT YET EVIDENCED | Current worker head is `f128d24dce7baf76a6ac8af4e62a331b80447311` / tree `7eb7970c...`, 19 commits ahead of the prior `4cb8a388...` checkpoint. The latest fixture correction has landed in two regression test files, but there are currently no completed check runs bound to `f128d24d...`. Predecessor exact-head evidence remains red: v2 rehearsal `33222202212` fails baseline backup/restore catalog reconciliation and W0 `33222202131` fails the Linux full test-suite step. Run mandatory gates on the final exact head and obtain independent DBP-002 acceptance before DBP-004. Production, master/PR69 merge and destructive Git remain prohibited. |
| 8 | MISSION-04 | Verification Team | MISSION-03 sealed | Independent verification | WAITING | MISSION-03 remains open/not sealed; no final exact-head report/evidence/manifest/detached SHA-256/seal/handoff exists |
| 9 | MISSION-05 | Closure Team | MISSION-04 sealed | Final closure package | WAITING | MISSION-04 not sealed |

## Governing line decision

Authoritative current product line:

`refs/heads/master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`

MISSION-03 reviewed pre-authoring baseline:

`codex/mission-03-execution-20260828@5d1352b4fb6d56261dff8b8a622bacb2786f56d9`, tree `00512125311306a43474638195d2cad97b76118e`.

Current evidenced worker head:

`codex/mission-03-execution-20260828@f128d24dce7baf76a6ac8af4e62a331b80447311`, tree `7eb7970cdb2349aaefabfa7b8e2d4bdfa5e50501`, parent `99d2880165c393edf90cfdc833c035ee4b7b552a`.

PR #69 at `601f2d1cad61d62e590a6714ad84e307eb84fe5f` remains `UNMERGED REMEDIATION / FINAL CANDIDATE`; no merge is authorized by Control Tower.

## Current MISSION-03 priority

The governing post-correction DB-GOV PASS remains valid and the only approved physical order remains:

`DBP-002 → DBP-004 → DBP-003B/C → DBP-003A → DBP-006 → DBP-005`

DBP-002 remains `IN PROGRESS — NOT ACCEPTED`.

Material progress since the previous Control Tower checkpoint is verified: the worker branch advanced from `4cb8a388...` to `f128d24d...`; DBP-002 rehearsal/correction tooling was extended; targeted DBP-002 physical-SQL/permission-resolver corrections and additional authorization coverage were added; and the latest bot commit updated the two failing Sync/API authority fixtures to seed the authoritative membership/grant model.

That correction does not close the gate. At the current `f128d24d...` head, no completed exact-head check run is yet available. The most recent completed predecessor runs still show the mandatory rehearsal and Linux regression path red, and any in-flight run on a superseded predecessor SHA cannot establish acceptance of the final head.

Required next actions:

1. Bind the next mandatory Full Rehearsal and W0/full-regression runs to `f128d24d...` or to a later final candidate SHA if the worker advances again.
2. Require fresh disposable PostgreSQL 18.6 execution and successful semantic baseline backup/restore reconciliation.
3. Require candidate application, RLS/catalog/ACL validation, cross-tenant/fail-closed negatives, full regression, no model drift, candidate backup/restore and final reconciliation to all pass on one exact final SHA.
4. Retain exact SHA/tree/parent, changed files, migration/source/generated-SQL hashes and artifact digests.
5. Submit the successful exact-head DBP-002 package to independent post-rehearsal DB-GOV before proceeding.

`DBP-004 = HOLD — DEPENDS ON DBP-002 ACCEPTANCE`.

No `OWNER DECISION REQUIRED` is active for this correction cycle. MISSION-04 remains WAIT until MISSION-03 is conclusively sealed and handed off.
