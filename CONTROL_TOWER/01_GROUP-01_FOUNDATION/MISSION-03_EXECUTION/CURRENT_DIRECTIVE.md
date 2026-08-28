# CURRENT DIRECTIVE — MISSION-03

`CONTINUE — DBP-002 WORKER EXECUTION EVIDENCED; EXACT-HEAD REHEARSAL/REGRESSION GATES FAILING; FIX AND RERUN; DO NOT ADVANCE TO DBP-004`

## Current execution basis

- MISSION-03: `IN PROGRESS — OPEN — NOT SEALED`.
- Product authority: `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`.
- Reviewed pre-authoring baseline: `codex/mission-03-execution-20260828@5d1352b4fb6d56261dff8b8a622bacb2786f56d9`, tree `00512125311306a43474638195d2cad97b76118e`.
- Current evidenced worker head: `codex/mission-03-execution-20260828@4cb8a388d65f9bf621feec4fde8ba3ec06bebea1`, tree `948726dfdd61e541a3b36c6e4039d48f23da4cfe`, parent `8e4d6e81104172bab86bb1eb3666c44da20d4ded`.
- Database baseline: `DB-BASELINE-001 = GREENFIELD — NEW — EMPTY — NO LEGACY TABLES / NO LEGACY DATA`.
- PR #69 remains `OPEN / DRAFT / UNMERGED — EVIDENCE ONLY`.

## Governing authority

The fresh post-correction DB-GOV decision remains:

`DB-GOV VERDICT = PASS`

It authorizes bounded candidate authoring/rehearsal only in this order:

`DBP-002 → DBP-004 → DBP-003B/C → DBP-003A → DBP-006 → DBP-005`

The PASS authorizes the workstream; it does not pre-accept a generated candidate.

## Material worker transition now verified

The former state `AUTHORING/REHEARSAL START AUTHORIZED — WAITING FOR WORKER SESSION` is closed. Repository evidence now proves actual DBP-002 worker output on the isolated execution branch.

Current head includes:

- additive candidate migration `20260828224241_GreenfieldTenantMembershipIsolation`;
- DBP-002 entity/model/physical SQL authoring;
- model snapshot and persistent permission resolver wiring;
- DBP-002 authorization regression coverage;
- rehearsal/generator/materialization workflows.

Exact-head evidence proves:

- PostgreSQL 18.6 is used;
- the original ten migration source/designer files remain unchanged from `5d1352...`;
- build succeeds;
- EF reports no pending model changes;
- generated Migration 11 SQL exists and includes RLS enable/force controls;
- an empty baseline can receive exactly the existing ten migrations;
- W0 can apply all eleven migrations on a disposable database.

These facts establish worker start, not DBP-002 acceptance.

## Blocking exact-head failures

### 1. Full Rehearsal v2 — baseline reconciliation failure

Run `33219905514` is `FAILURE`.

It succeeds through identity/PostgreSQL 18.6 capture, original-ten preservation, restore/build/no-model-drift/generated SQL, and application of exactly the original ten migrations. It then fails at `Baseline catalog backup restore reconciliation`.

The observed raw catalog diff is caused by PostgreSQL `pg_dump/pg_restore` canonicalization of equivalent CHECK expressions, for example an array cast represented before restore as an outer `::text[]` and after restore as per-element `::text`. Because the harness compares the textual definitions directly, candidate application/RLS/negative/full-regression/candidate-restore steps are skipped.

This gate may not be waived. Correct the harness to compare catalog semantics deterministically without suppressing real drift, then rerun from a fresh empty PostgreSQL 18.6 baseline.

### 2. W0 full regression — five authorization/sync failures

Run `33219905526` applies all 11 migrations and reaches full regression, but exact-head result is:

`149 PASSED / 5 FAILED / 154 TOTAL`.

Failures:

- four `SyncOperationPersistenceTests` reject operations with `PERMISSION_DENIED: sync.operations.execute`;
- `ApiAuthenticationAndAuditTests.Sync_batch_accepts_a_valid_token_and_enforces_claim_scope` expects `QUEUED` but receives `REJECTED`.

The worker must determine and prove whether the failures are stale test/fixture authority assumptions under the accepted membership model or a product integration regression. Correct only the necessary isolated-branch candidate/test-fixture path, preserve fail-closed/deny semantics, and rerun the full suite. Do not make tests pass by bypassing authorization.

### 3. Harness-fix workflow cannot deliver its own workflow edit

Run `33219905547` generated a local workflow-only correction commit but push was rejected because the GitHub App lacks permission to create/update `.github/workflows/mission-03-dbp002-rehearsal.yml` without workflow permission.

Do not broaden permissions, merge, or request Production authority as part of supervision. Land the harness-only correction through an already-authorized worker path, then rerun exact-head gates. This is not an `OWNER DECISION REQUIRED` item.

## Required next sequence

1. Keep `DBP-002 = IN PROGRESS — NOT ACCEPTED`.
2. Correct the deterministic catalog-reconciliation harness.
3. Reconcile the five authorization/sync regression failures without weakening the DBP-002 authority model.
4. Produce a new remote exact head and bind SHA/tree/parent/changed files.
5. Rerun Full Rehearsal and W0/full regression on fresh disposable PostgreSQL 18.6.
6. Require every mandatory DBP-002 candidate step to execute successfully, including proposal negatives/RLS, full regression, model drift, candidate backup/restore and catalog reconciliation.
7. Retain all report/evidence/artifact digests and submit the successful exact-head checkpoint to independent post-rehearsal DB-GOV.
8. Only after DBP-002 independent acceptance may DBP-004 begin.

`DBP-004 = HOLD — DEPENDS ON DBP-002 ACCEPTANCE`.

No alternate ordering is authorized.

## Parallel and later work

Continue unrelated W5/W6/W7 work only where its gates are independent of DBP-002. W8 stays last and no destructive/global cleanup is authorized before preservation gates.

## Prohibitions

No Production database/data/configuration/credentials. No Production secrets. No edit/delete/squash of the existing ten migrations. No destructive migration/down-migration reliance. No merge to master, PR #69 merge, rebase, cherry-pick, force-push or history rewrite.

## Return rule

Do not return to the owner after each DBP or Wave. Continue automatically through all enabled MISSION-03 work.

Return only for:

1. `MISSION-03 = COMPLETE — SEALED — DELIVERED TO CONTROL TOWER`; or
2. a genuinely new owner-reserved decision not already covered by current decisions; or
3. a true external-access blocker after all internally permitted work is exhausted.

MISSION-04 remains `WAIT — NOT STARTED` until a valid MISSION-03 report + evidence + manifest + detached SHA-256 + seal + handoff exists.
