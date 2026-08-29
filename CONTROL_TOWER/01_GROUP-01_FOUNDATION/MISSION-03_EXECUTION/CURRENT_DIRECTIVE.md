# CURRENT DIRECTIVE — MISSION-03

`CONTINUE — DBP-002 EXACT-HEAD TECHNICAL PASS EVIDENCED; START INDEPENDENT POST-REHEARSAL DB-GOV REVIEW; DO NOT ADVANCE TO DBP-004`

## Current execution basis

- MISSION-03: `IN PROGRESS — OPEN — NOT SEALED`.
- Product authority: `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`.
- Reviewed pre-authoring baseline: `codex/mission-03-execution-20260828@5d1352b4fb6d56261dff8b8a622bacb2786f56d9`, tree `00512125311306a43474638195d2cad97b76118e`.
- Current evidenced worker head: `codex/mission-03-execution-20260828@ffdf1087ab4a6435cd1f2b19c5ab9ff58ce206ce`, tree `e828941817432bdc73f3e6fc31e74219e74fcf33`, parent `f128d24dce7baf76a6ac8af4e62a331b80447311`.
- Database baseline: `DB-BASELINE-001 = GREENFIELD — NEW — EMPTY — NO LEGACY TABLES / NO LEGACY DATA`.
- PR #69 remains `OPEN / DRAFT / UNMERGED — EVIDENCE ONLY` at `601f2d1cad61d62e590a6714ad84e307eb84fe5f`.

## Governing authority

The fresh post-correction pre-authoring DB-GOV decision remains:

`DB-GOV VERDICT = PASS`

It authorizes bounded candidate authoring/rehearsal only in this order:

`DBP-002 → DBP-004 → DBP-003B/C → DBP-003A → DBP-006 → DBP-005`

The PASS authorizes the workstream; it does not pre-accept a generated candidate.

## Material worker transition now verified

The worker correction cycle has advanced to a new exact head after the authoritative fixture correction:

`ffdf1087ab4a6435cd1f2b19c5ab9ff58ce206ce`

The preceding `f128d24d...` commit updated only `TransportERP.Tests/ApiAuthenticationAndAuditTests.cs` and `TransportERP.Tests/SyncOperationPersistenceTests.cs` to seed the authoritative `UserMembership` / `UserRoleGrant` / `UserPermissionGrant` model while preserving an explicit DENY path. `ffdf1087...` then triggered a fresh exact-head rehearsal/regression cycle.

## Exact-head technical gate evidence

The current head has materially improved from the previous red checkpoint:

1. `MISSION-03 DBP-002 Full Rehearsal v3` run `33222541097` is `SUCCESS` on `ffdf1087...`.
2. W0 run `33222541108` is `SUCCESS` on `ffdf1087...`; both the Desktop Windows job and the Core/PostgreSQL/API/Mobile job pass. The Linux job passes contract validation, build, migration lineage/application, complete test suite and API boundary probe.
3. W7 PostgreSQL backup/restore run `33222541109` is `SUCCESS` on `ffdf1087...`.
4. The corrected v3 path binds evidence to exact SHA/tree/parent, uses PostgreSQL 18.6, proves original-ten migration preservation, verifies no model drift and generated SQL, performs structural baseline backup/restore reconciliation, exercises generated-SQL and EF candidate application on independent databases, validates RLS/catalog/role/ACL/FK properties and negative isolation behavior, runs full regression, and captures candidate backup/restore evidence.

However, `MISSION-03 DBP-002 Full Rehearsal v2` run `33222541073` on the same head remains `FAIL` at `Baseline catalog backup restore reconciliation`; all later candidate stages in that legacy workflow are skipped. The known v2 failure is consistent with its raw/canonical textual catalog comparison, but it cannot be silently waived by Control Tower.

## Current DBP-002 disposition

`DBP-002 = TECHNICAL EXACT-HEAD CANDIDATE PASSED V3 + W0 — AWAITING INDEPENDENT POST-REHEARSAL DB-GOV ACCEPTANCE — NOT ACCEPTED`

`DBP-002 POST-REHEARSAL DB-GOV REVIEW = START AUTHORIZED — WAITING FOR WORKER SESSION`

An independent DB-GOV reviewer must now verify and record, from the exact-head evidence package, that:

1. v3 is the valid corrected/superseding rehearsal path for DBP-002;
2. the red v2 result is a superseded harness artifact and not a physical-design defect;
3. all v3 evidence is complete and bound to `ffdf1087...` / tree `e8289418...` / parent `f128d24d...`;
4. original migration preservation, candidate hashes, generated SQL, RLS/ACL/catalog checks, fail-closed/cross-tenant negatives, full regression and recovery evidence satisfy the approved DBP-002 gate;
5. the DBP-002 checkpoint has report + evidence + manifest + SHA-256 integrity evidence before acceptance.

Only after a fresh independent DB-GOV PASS for this post-rehearsal checkpoint may DBP-002 become accepted and DBP-004 be authorized.

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
