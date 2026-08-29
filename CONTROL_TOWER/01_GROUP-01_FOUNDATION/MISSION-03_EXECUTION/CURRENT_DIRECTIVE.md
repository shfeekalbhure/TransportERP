# CURRENT DIRECTIVE — MISSION-03

`CONTINUE — DBP-002 CORRECTION HEAD ADVANCED; AUTHORITATIVE FIXTURE PATCH LANDED; RUN NEW EXACT-HEAD GATES; DO NOT ADVANCE TO DBP-004`

## Current execution basis

- MISSION-03: `IN PROGRESS — OPEN — NOT SEALED`.
- Product authority: `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`.
- Reviewed pre-authoring baseline: `codex/mission-03-execution-20260828@5d1352b4fb6d56261dff8b8a622bacb2786f56d9`, tree `00512125311306a43474638195d2cad97b76118e`.
- Current evidenced worker head: `codex/mission-03-execution-20260828@f128d24dce7baf76a6ac8af4e62a331b80447311`, tree `7eb7970cdb2349aaefabfa7b8e2d4bdfa5e50501`, parent `99d2880165c393edf90cfdc833c035ee4b7b552a`.
- Database baseline: `DB-BASELINE-001 = GREENFIELD — NEW — EMPTY — NO LEGACY TABLES / NO LEGACY DATA`.
- PR #69 remains `OPEN / DRAFT / UNMERGED — EVIDENCE ONLY` at `601f2d1cad61d62e590a6714ad84e307eb84fe5f`.

## Governing authority

The fresh post-correction DB-GOV decision remains:

`DB-GOV VERDICT = PASS`

It authorizes bounded candidate authoring/rehearsal only in this order:

`DBP-002 → DBP-004 → DBP-003B/C → DBP-003A → DBP-006 → DBP-005`

The PASS authorizes the workstream; it does not pre-accept a generated candidate.

## Material worker transition now verified

The previously recorded worker head `4cb8a388d65f9bf621feec4fde8ba3ec06bebea1` is no longer current. The isolated execution branch is now 19 commits ahead of that checkpoint at `f128d24d...`.

The correction chain includes new DBP-002 rehearsal/correction workflows, targeted DBP-002 physical-SQL and permission-resolver correction, added authorization coverage, and a worker-generated correction of the failing authorization fixtures. The latest commit `f128d24d...` modifies only:

- `TransportERP.Tests/ApiAuthenticationAndAuditTests.cs`;
- `TransportERP.Tests/SyncOperationPersistenceTests.cs`.

Those fixture changes move the affected tests from legacy role/permission assumptions to the authoritative `UserMembership` / `UserRoleGrant` / `UserPermissionGrant` model while retaining an explicit DENY test path.

This is evidence that the worker correction cycle continued. It is **not** evidence that DBP-002 passed its mandatory gates.

## Exact-head gate status

`DBP-002 = IN PROGRESS — NOT ACCEPTED`.

At the latest verified exact worker head `f128d24d...`, GitHub currently reports no completed exact-head check runs. Therefore DBP-002 cannot be accepted from the fixture correction alone.

The most recent completed predecessor evidence remains red:

1. On `9df4e9440f57ff82e35c18828c05e2d27860d43b`, Full Rehearsal v2 run `33222202212` again reaches application of exactly the original ten migrations, then fails `Baseline catalog backup restore reconciliation`; candidate application, RLS/negative probes, full regression and candidate restore/reconciliation are skipped.
2. On the same `9df4e944...`, W0 run `33222202131` succeeds through migration lineage/application but fails at `Run complete test suite against disposable PostgreSQL`; the Desktop job succeeds while the Linux Core/PostgreSQL/API/Mobile job fails.
3. A newer human-triggered predecessor head `99d2880165c393edf90cfdc833c035ee4b7b552a` has rehearsal runs in flight, but it is superseded for exact-head acceptance by `f128d24d...` because the fixture correction landed afterward.

No predecessor run may be used as a substitute for successful mandatory checks on the final candidate SHA.

## Required next sequence

1. Keep `DBP-002 = IN PROGRESS — NOT ACCEPTED`.
2. Bind the next rehearsal/regression evidence to `f128d24d...` or, if the worker advances again, to that newer exact head.
3. Run Full Rehearsal and W0/full regression on a fresh disposable PostgreSQL 18.6 environment after the fixture correction.
4. Require every mandatory DBP-002 step to complete successfully, including semantic/deterministic baseline backup/restore reconciliation, candidate application, RLS/catalog/ACL checks, cross-tenant and fail-closed negatives, full regression, model-drift check, candidate backup/restore and final reconciliation.
5. Preserve exact SHA/tree/parent, changed-file set, migration/source/generated-SQL hashes, evidence and artifact digests.
6. Submit the successful exact-head DBP-002 package to independent post-rehearsal DB-GOV.
7. Only after independent DBP-002 acceptance may DBP-004 begin.

`DBP-004 = HOLD — DEPENDS ON DBP-002 ACCEPTANCE`.

No alternate ordering is authorized.

## Parallel and later work

Continue unrelated W5/W6/W7 work only where its gates are independent of DBP-002. W8 stays last and no destructive/global cleanup is authorized before preservation gates.

## Prohibitions

No Production database/data/configuration/credentials. No Production secrets. No edit/delete/squash of the existing ten migrations. No destructive migration/down-migration reliance. No merge to master, PR #69 merge, rebase, cherry-pick, force-push or history rewrite.

Control Tower supervision does not modify Product Source, Tests, Migrations, production configuration or databases.

## Return rule

Do not return to the owner after each DBP or Wave. Continue automatically through all enabled MISSION-03 work.

Return only for:

1. `MISSION-03 = COMPLETE — SEALED — DELIVERED TO CONTROL TOWER`; or
2. a genuinely new owner-reserved decision not already covered by current decisions; or
3. a true external-access blocker after all internally permitted work is exhausted.

MISSION-04 remains `WAIT — NOT STARTED` until a valid MISSION-03 report + evidence + manifest + detached SHA-256 + seal + handoff exists.
