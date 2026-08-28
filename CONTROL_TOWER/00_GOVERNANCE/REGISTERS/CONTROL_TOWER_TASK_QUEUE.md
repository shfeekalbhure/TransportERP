# CONTROL TOWER TASK QUEUE

| Order | Mission | Team | Prerequisite | Required Output | State | Blocker |
|---:|---|---|---|---|---|---|
| 1 | MISSION-01 | TEAM-A/TEAM-B/TEAM-C1 | Audit baseline + assignments | Independent reports + current architecture | SEALED | A/B sealed; C1 v1.1 accepted; historical versions preserved |
| 2 | MISSION-01 | TEAM-D | A/B/C1 accepted | Complete reconciliation package | SEALED | D v1.1 accepted; historical v1.0 preserved |
| 3 | MISSION-01 | TEAM-C2 | TEAM-D sealed | Target architecture proposal | SEALED | C2 v1.1 accepted |
| 4 | MISSION-01 | TEAM-E | TEAM-C2 sealed | Multidisciplinary advisory package | SEALED | E v1.1 accepted; `BLK-B-001` retained in assurance narrative |
| 5 | MISSION-01 | MASTER/GATE REVALIDATION | Owner-authorized authoritative line | Revalidated Master/Gate package on exact authoritative SHA | SEALED — STOP | v2.0 complete; all 14 hashes verified; gate `READY FOR REMEDIATION PLANNING`; v1.0 preserved |
| 6 | MISSION-02 | Planning Team | Revalidated MISSION-01 gate = `READY FOR REMEDIATION PLANNING` | Remediation plan | SEALED — DELIVERED — STOP | v1.2 remotely delivered; 64/64 findings; 8/8 workstreams PLANNED; 20 packages; W0–W8; DB-GOV paths retained |
| 7 | MISSION-03 | Execution Team | MISSION-02 sealed | Implemented changes/evidence | IN PROGRESS — OPEN — NOT SEALED — DBP-002 IN PROGRESS — EXACT-HEAD GATES FAILING | Worker execution is evidenced at `4cb8a388...` / tree `948726df...`. Full Rehearsal v2 `33219905514` fails at baseline backup/restore catalog reconciliation before candidate application; W0 `33219905526` applies all 11 migrations but full regression is 149/154 with five authorization/sync failures. Harness-fix run `33219905547` cannot push its workflow-only correction because the GitHub App lacks workflow-update permission. Fix and rerun DBP-002; do not advance to DBP-004. Production, master/PR69 merge and destructive Git remain prohibited. |
| 8 | MISSION-04 | Verification Team | MISSION-03 sealed | Independent verification | WAITING | MISSION-03 remains open/not sealed; no final exact-head report/evidence/manifest/detached SHA-256/seal/handoff exists |
| 9 | MISSION-05 | Closure Team | MISSION-04 sealed | Final closure package | WAITING | MISSION-04 not sealed |

## Governing line decision

Authoritative current product line:

`refs/heads/master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`

MISSION-03 reviewed pre-authoring baseline:

`codex/mission-03-execution-20260828@5d1352b4fb6d56261dff8b8a622bacb2786f56d9`, tree `00512125311306a43474638195d2cad97b76118e`.

Current evidenced worker head:

`codex/mission-03-execution-20260828@4cb8a388d65f9bf621feec4fde8ba3ec06bebea1`, tree `948726dfdd61e541a3b36c6e4039d48f23da4cfe`.

PR #69 at `601f2d1cad61d62e590a6714ad84e307eb84fe5f` remains `UNMERGED REMEDIATION / FINAL CANDIDATE`; no merge is authorized by Control Tower.

## Current MISSION-03 priority

The governing post-correction DB-GOV PASS remains valid and the only approved physical order remains:

`DBP-002 → DBP-004 → DBP-003B/C → DBP-003A → DBP-006 → DBP-005`

Worker execution has started, so the former `START AUTHORIZED — WAITING FOR WORKER SESSION` state is closed. DBP-002 is now `IN PROGRESS — NOT ACCEPTED`.

Required next actions:

1. Correct the baseline backup/restore catalog-comparison harness so PostgreSQL canonicalization does not create false textual drift while real schema differences remain detectable.
2. Reconcile the five exact-head W0 authorization/sync regression failures under the intended DBP-002 membership authority without weakening fail-closed behavior.
3. Land the harness correction through an already-authorized worker route; do not broaden GitHub App permissions as a supervision action.
4. Rerun exact-head Full Rehearsal and W0/full regression on fresh disposable PostgreSQL 18.6.
5. Retain exact SHA/tree/parent, changed files, migration/source/generated-SQL hashes, baseline and candidate backup/restore/catalog evidence, RLS/negative tests, complete regression and artifact digests.
6. Submit successful DBP-002 exact-head evidence to independent post-rehearsal DB-GOV before proceeding to DBP-004.

`DBP-004 = HOLD — DEPENDS ON DBP-002 ACCEPTANCE`.

No `OWNER DECISION REQUIRED` is active for these failures. MISSION-04 remains WAIT until MISSION-03 is conclusively sealed and handed off.
