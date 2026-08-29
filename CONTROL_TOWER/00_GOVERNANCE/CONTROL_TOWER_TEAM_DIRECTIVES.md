# CONTROL TOWER TEAM DIRECTIVES

Every team or mission must first read, in order: `CONTROL_TOWER/README.md`, `OWNER_DELEGATION_AND_OPERATING_DIRECTIVE.md`, its own section here, its mission order, its mission-local `CURRENT_DIRECTIVE.md`, and all required sealed predecessor outputs. Only Control Tower changes a `CURRENT DIRECTIVE`. A team at `WAIT`, `HOLD`, or `STOP` must not work. A sealed team must not modify its output unless this file issues `REOPEN` or `RETURN FOR REWORK`.

## Governing owner decisions now in force

Authoritative current product line:

`refs/heads/master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`

PR #69 `codex/p1-security-device-sync-offline-20260825@601f2d1cad61d62e590a6714ad84e307eb84fe5f` is `UNMERGED REMEDIATION / FINAL CANDIDATE`, not CURRENT. No merge is authorized.

Target database authority:

`DB-BASELINE-001 = GREENFIELD — NEW — EMPTY — NO LEGACY TABLES / NO LEGACY DATA`.

## MISSION-01 TEAMS

- TEAM-A: `STOP — SEALED — DELIVERED TO CONTROL TOWER`.
- TEAM-B: `STOP — SEALED — DELIVERED TO CONTROL TOWER`; `BLK-B-001` retained.
- TEAM-C1: `STOP — v1.1 SEALED`; v1.0 preserved/superseded.
- TEAM-D: `STOP — v1.1 SEALED`; v1.0 preserved/superseded.
- TEAM-C2: `STOP — v1.1 SEALED`; v1.0 preserved/superseded.
- TEAM-E: `STOP — v1.1 SEALED`; v1.0 preserved/rejected for downstream use.
- MASTER/GATE: `STOP — v2.0 SEALED — READY FOR REMEDIATION PLANNING`; v1.0 preserved as historical sealed evidence.

## MISSION-02

- `CURRENT DIRECTIVE`: `STOP`.
- Recorded disposition: `MISSION-02-v1.2 — SEALED — DELIVERED TO CONTROL TOWER — READY FOR MISSION-03`.
- Accepted planning scope: 64/64 findings; both P0s; all governing P1s; 8/8 workstreams `PLANNED`; 20 remediation packages; waves `W0–W8`; all proposed DB changes gated through `DB-GOV-001`.
- Product modification authority exercised by MISSION-02: `NONE`.

## MISSION-03

- `CURRENT DIRECTIVE`: `CONTINUE — DBP-002 EXACT-HEAD TECHNICAL PASS EVIDENCED; START INDEPENDENT POST-REHEARSAL DB-GOV REVIEW; DO NOT ADVANCE TO DBP-004`.
- MISSION-03 remains `IN PROGRESS — OPEN — NOT SEALED`.
- Product authority remains `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`.
- Reviewed pre-authoring execution baseline: `codex/mission-03-execution-20260828@5d1352b4fb6d56261dff8b8a622bacb2786f56d9`, tree `00512125311306a43474638195d2cad97b76118e`.
- Current evidenced worker head: `codex/mission-03-execution-20260828@ffdf1087ab4a6435cd1f2b19c5ab9ff58ce206ce`, tree `e828941817432bdc73f3e6fc31e74219e74fcf33`, parent `f128d24dce7baf76a6ac8af4e62a331b80447311`.
- PR #69 remains comparative unmerged evidence only; no merge is authorized.

### Binding owner decisions

- `AUTH-001 = RESOLVED — LOCAL APPLICATION AUTHORITY`.
- `ACC-001 = RESOLVED — OPERATIONAL COLLECTION; GOVERNED SETTLEMENT POSTS THE LEDGER`.
- `OFFLINE-001 = RESOLVED — DEFAULT DENY; EXPLICIT QUEUE FOR BOUNDED OPERATIONAL CAPTURE`.
- `CLIENT-001 = RESOLVED — DESKTOP + THREE ANDROID CLIENTS ARE RELEASE TARGETS; IOS DEFERRED`.
- `DB-BASELINE-001 = RESOLVED — GREENFIELD / NEW / EMPTY TARGET DATABASE`.

### Governing DB-GOV decision

The fresh post-correction pre-authoring decision remains `DB-GOV VERDICT = PASS` and authorizes bounded non-Production authoring/rehearsal in this single order only:

`DBP-002 → DBP-004 → DBP-003B/C → DBP-003A → DBP-006 → DBP-005`

The PASS authorizes work; it does not pre-accept any generated candidate.

### DBP-002 current exact-head result

The current exact head `ffdf1087...` now has successful corrected technical evidence:

- `MISSION-03 DBP-002 Full Rehearsal v3` run `33222541097 = SUCCESS`.
- W0 run `33222541108 = SUCCESS`; Windows Desktop and Linux Core/PostgreSQL/API/Mobile both pass, including complete tests and API boundary verification.
- W7 PostgreSQL backup/restore run `33222541109 = SUCCESS`.
- v3 binds SHA/tree/parent, uses PostgreSQL 18.6, proves original-ten migration preservation, verifies no model drift and generated SQL, performs structural baseline backup/restore reconciliation, independently applies the candidate through generated SQL and EF, validates RLS/catalog/roles/ACL/FK and negative isolation/fail-closed behavior, runs full regression and captures recovery evidence.

The authoritative fixture correction immediately preceding this head updates the affected Sync/API regression fixtures to `UserMembership` / `UserRoleGrant` / `UserPermissionGrant` semantics while preserving an explicit DENY test path.

DBP-002 is still **not independently accepted**. Full Rehearsal v2 run `33222541073` remains `FAIL` at baseline catalog backup/restore reconciliation and skips the candidate stages. Control Tower will not silently ignore that red result even though v3 was introduced as the structural/semantic corrected rehearsal path.

### Required independent post-rehearsal DB-GOV review

`DBP-002 POST-REHEARSAL DB-GOV REVIEW = START AUTHORIZED — WAITING FOR WORKER SESSION`.

The independent reviewer must:

- confirm whether v3 formally supersedes the raw-text v2 harness and document the disposition of the red v2 result;
- verify exact SHA/tree/parent and the final changed-file scope;
- verify preservation of the original ten migrations;
- verify migration/source/generated-SQL hashes and evidence digests;
- verify PostgreSQL 18.6 candidate apply, RLS/catalog/roles/ACL/FK controls, cross-tenant/fail-closed negatives, W0/full regression and backup/restore evidence;
- verify a DBP-002 report + evidence + manifest + SHA-256 integrity package;
- issue an independent `PASS` or `FAIL` with exact blockers.

Only a fresh independent PASS may change DBP-002 to accepted and release DBP-004.

`DBP-004 = HOLD — DEPENDS ON DBP-002 ACCEPTANCE`.

Continue unrelated W5/W6/W7 work only where its own gates remain independent. W8 stays last; no destructive/global cleanup before preservation gates.

### Prohibitions

No Production database/data/configuration/credentials. No Production secrets. Do not edit/delete/squash the existing ten migrations. No destructive migration/down-migration reliance. No merge to master, PR #69 merge, rebase, cherry-pick, force-push or history rewrite.

No `OWNER DECISION REQUIRED` is active; the next action is an ordinary independent governance review on the isolated exact-head evidence.

## MISSION-04

- `CURRENT DIRECTIVE`: `WAIT`.
- Prerequisite: MISSION-03 must be conclusively sealed and handed off with exact execution SHAs, report/evidence/manifest/detached SHA-256/seal/handoff, preservation/rollback and DB-GOV compliance independently verified.
- MISSION-03 is still open/not sealed; M04 dispatch remains prohibited.
- Independence from MISSION-03 execution remains mandatory.

## MISSION-05

- `CURRENT DIRECTIVE`: `WAIT`.
- Prerequisite: MISSION-04 must be sealed and handed off.

`DB-GOV-001` remains binding throughout. No Production Database, Schema, Entity, Migration, field, relationship, index, constraint, seed or data change may execute without its required governance, impact, preservation, test/recovery and explicit Production authority.
