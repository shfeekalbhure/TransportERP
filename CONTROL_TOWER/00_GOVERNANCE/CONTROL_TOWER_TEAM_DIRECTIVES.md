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

- `CURRENT DIRECTIVE`: `CONTINUE — DBP-002 CORRECTION HEAD ADVANCED; AUTHORITATIVE FIXTURE PATCH LANDED; RUN NEW EXACT-HEAD GATES; DO NOT ADVANCE TO DBP-004`.
- MISSION-03 remains `IN PROGRESS — OPEN — NOT SEALED`.
- Product authority remains `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`.
- Reviewed pre-authoring execution baseline: `codex/mission-03-execution-20260828@5d1352b4fb6d56261dff8b8a622bacb2786f56d9`, tree `00512125311306a43474638195d2cad97b76118e`.
- Current evidenced worker head: `codex/mission-03-execution-20260828@f128d24dce7baf76a6ac8af4e62a331b80447311`, tree `7eb7970cdb2349aaefabfa7b8e2d4bdfa5e50501`, parent `99d2880165c393edf90cfdc833c035ee4b7b552a`.
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

Worker correction has materially advanced beyond the prior Control Tower checkpoint. `f128d24d...` is 19 commits ahead of `4cb8a388...`. The intervening chain adds/updates DBP-002 rehearsal and semantic-reconciliation tooling, targeted DBP-002 physical-SQL and permission-resolver corrections, authorization coverage, and finally the authoritative membership/grant fixture correction in:

- `TransportERP.Tests/ApiAuthenticationAndAuditTests.cs`;
- `TransportERP.Tests/SyncOperationPersistenceTests.cs`.

The latest fixture patch replaces the affected legacy authority assumptions with `UserMembership`, `UserRoleGrant`, and `UserPermissionGrant` setup while preserving an explicit DENY path.

DBP-002 is **not accepted**. There is not yet a successful mandatory gate set bound to the current exact head `f128d24d...`; at the latest verification GitHub reports no completed check runs on that SHA.

The latest completed predecessor evidence is still red:

1. Full Rehearsal v2 run `33222202212` on `9df4e944...` fails at baseline backup/restore catalog reconciliation after successfully applying the original ten migrations. Candidate application, RLS/negative/full-regression/candidate-restore stages are skipped.
2. W0 run `33222202131` on `9df4e944...` succeeds through disposable PostgreSQL migration application but fails at the Linux complete-test-suite step; Desktop succeeds.
3. Runs started on predecessor `99d2880165...` cannot establish exact-head acceptance after `f128d24d...` landed.

### Required continuation

- Keep DBP-002 `IN PROGRESS — NOT ACCEPTED`.
- Trigger/rerun the mandatory Full Rehearsal and W0/full regression on `f128d24d...` or on a later final exact head if the branch advances again.
- Use fresh disposable PostgreSQL 18.6 and require semantic/deterministic baseline backup/restore reconciliation to pass without masking real schema drift.
- Require candidate apply, RLS/catalog/ACL checks, cross-tenant and fail-closed negatives, full regression, model-drift check, candidate backup/restore and final reconciliation to all pass on the same final SHA.
- Preserve exact SHA/tree/parent, changed files, migration/source/generated-SQL hashes, evidence and artifact digests.
- Submit the successful exact-head package to independent post-rehearsal DB-GOV.
- `DBP-004 = HOLD — DEPENDS ON DBP-002 ACCEPTANCE`; no later DBP may be advanced by reordering.
- Continue unrelated W5/W6/W7 work only where its own gates remain independent.
- W8 stays last; no destructive/global cleanup before preservation gates.

### Prohibitions

No Production database/data/configuration/credentials. No Production secrets. Do not edit/delete/squash the existing ten migrations. No destructive migration/down-migration reliance. No merge to master, PR #69 merge, rebase, cherry-pick, force-push or history rewrite.

No `OWNER DECISION REQUIRED` is active for this exact-head correction cycle; the next permitted actions remain ordinary isolated-branch correction/rerun and independent DB-GOV verification.

## MISSION-04

- `CURRENT DIRECTIVE`: `WAIT`.
- Prerequisite: MISSION-03 must be conclusively sealed and handed off with exact execution SHAs, report/evidence/manifest/detached SHA-256/seal/handoff, preservation/rollback and DB-GOV compliance independently verified.
- MISSION-03 is still open/not sealed; M04 dispatch remains prohibited.
- Independence from MISSION-03 execution remains mandatory.

## MISSION-05

- `CURRENT DIRECTIVE`: `WAIT`.
- Prerequisite: MISSION-04 must be sealed and handed off.

`DB-GOV-001` remains binding throughout. No Production Database, Schema, Entity, Migration, field, relationship, index, constraint, seed or data change may execute without its required governance, impact, preservation, test/recovery and explicit Production authority.
