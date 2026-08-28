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

- `CURRENT DIRECTIVE`: `CONTINUE — DBP-002 WORKER EXECUTION EVIDENCED; EXACT-HEAD REHEARSAL/REGRESSION GATES FAILING; FIX AND RERUN; DO NOT ADVANCE TO DBP-004`.
- MISSION-03 remains `IN PROGRESS — OPEN — NOT SEALED`.
- Product authority remains `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`.
- Reviewed pre-authoring execution baseline: `codex/mission-03-execution-20260828@5d1352b4fb6d56261dff8b8a622bacb2786f56d9`, tree `00512125311306a43474638195d2cad97b76118e`.
- Current evidenced worker head: `codex/mission-03-execution-20260828@4cb8a388d65f9bf621feec4fde8ba3ec06bebea1`, tree `948726dfdd61e541a3b36c6e4039d48f23da4cfe`, parent `8e4d6e81104172bab86bb1eb3666c44da20d4ded`.
- PR #69 remains comparative unmerged evidence only; no merge is authorized.

### Binding owner decisions

- `AUTH-001 = RESOLVED — LOCAL APPLICATION AUTHORITY`.
- `ACC-001 = RESOLVED — OPERATIONAL COLLECTION; GOVERNED SETTLEMENT POSTS THE LEDGER`.
- `OFFLINE-001 = RESOLVED — DEFAULT DENY; EXPLICIT QUEUE FOR BOUNDED OPERATIONAL CAPTURE`.
- `CLIENT-001 = RESOLVED — DESKTOP + THREE ANDROID CLIENTS ARE RELEASE TARGETS; IOS DEFERRED`.
- `DB-BASELINE-001 = RESOLVED — GREENFIELD / NEW / EMPTY TARGET DATABASE`.

### Governing DB-GOV decision

The fresh post-correction decision remains `DB-GOV VERDICT = PASS` and authorizes bounded non-Production authoring/rehearsal in this single order only:

`DBP-002 → DBP-004 → DBP-003B/C → DBP-003A → DBP-006 → DBP-005`

The PASS authorizes work; it does not pre-accept any generated candidate.

### DBP-002 current exact-head result

Worker execution is now proven. The current head contains candidate `GreenfieldTenantMembershipIsolation`, an additive Migration 11, model-snapshot/physical-SQL changes, permission-resolver wiring and regression tests. Evidence on exact head `4cb8a388...` proves the original ten migration source/designer files remain unchanged, PostgreSQL 18.6 is used, build succeeds, no pending model changes are reported, and an empty baseline can receive exactly the existing ten migrations.

DBP-002 is **not accepted** because mandatory exact-head gates fail:

1. Full Rehearsal v2 run `33219905514` fails at baseline backup/restore catalog reconciliation before candidate application. The observed differences are textual PostgreSQL canonicalization of semantically equivalent CHECK expressions after `pg_dump/pg_restore`; later candidate/RLS/regression/restore steps are therefore skipped.
2. W0 exact-head run `33219905526` successfully applies all 11 migrations but full regression is `149 passed / 5 failed / 154 total`. Four SyncOperation persistence tests fail with `PERMISSION_DENIED: sync.operations.execute`; the API sync test expects `QUEUED` but receives `REJECTED`. The worker must prove whether this is an intended membership-authority fixture transition or a product integration regression and restore the required regression gate without weakening authorization.
3. Harness-fix run `33219905547` creates a local workflow-only correction commit but cannot push it because the GitHub App lacks permission to update `.github/workflows/*`. This is an execution-mechanism blocker, not owner-reserved authority. Do not broaden permissions or ask for merge/Production authority; land any harness-only correction through an already-authorized worker path and rerun exact-head gates.

### Required continuation

- Keep DBP-002 `IN PROGRESS — NOT ACCEPTED`.
- Fix the catalog reconciliation harness so backup/restore comparison is semantic/deterministic without masking real schema drift.
- Reconcile the five W0 authorization regressions under the accepted DBP-002 membership model without weakening deny/fail-closed behavior.
- Rerun exact-head Full Rehearsal and W0/full regression on a fresh disposable PostgreSQL 18.6 environment.
- Require all mandatory DBP-002 steps to complete, retain exact artifacts/digests, and submit the resulting exact-head package for independent post-rehearsal DB-GOV.
- `DBP-004 = HOLD — DEPENDS ON DBP-002 ACCEPTANCE`; no later DBP may be advanced by reordering.
- Continue unrelated W5/W6/W7 work only where its own gates remain independent.
- W8 stays last; no destructive/global cleanup before preservation gates.

### Prohibitions

No Production database/data/configuration/credentials. No Production secrets. Do not edit/delete/squash the existing ten migrations. No destructive migration/down-migration reliance. No merge to master, PR #69 merge, rebase, cherry-pick, force-push or history rewrite.

No `OWNER DECISION REQUIRED` is active for this exact-head failure set; the next permitted actions are ordinary isolated-branch harness/test/candidate correction and rerun.

## MISSION-04

- `CURRENT DIRECTIVE`: `WAIT`.
- Prerequisite: MISSION-03 must be conclusively sealed and handed off with exact execution SHAs, report/evidence/manifest/detached SHA-256/seal/handoff, preservation/rollback and DB-GOV compliance independently verified.
- MISSION-03 is still open/not sealed; M04 dispatch remains prohibited.
- Independence from MISSION-03 execution remains mandatory.

## MISSION-05

- `CURRENT DIRECTIVE`: `WAIT`.
- Prerequisite: MISSION-04 must be sealed and handed off.

`DB-GOV-001` remains binding throughout. No Production Database, Schema, Entity, Migration, field, relationship, index, constraint, seed or data change may execute without its required governance, impact, preservation, test/recovery and explicit Production authority.
