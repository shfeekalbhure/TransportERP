# CURRENT DIRECTIVE — MISSION-03

`CONTINUE — POST-CORRECTION DB-GOV PASS RECORDED; BOUNDED GREENFIELD AUTHORING/REHEARSAL AUTHORIZED; KEEP MISSION-03 OPEN`

## Current execution basis

- MISSION-03: `IN PROGRESS — OPEN — NOT SEALED`.
- Product authority: `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`.
- Execution branch/head from the reviewed package: `codex/mission-03-execution-20260828@5d1352b4fb6d56261dff8b8a622bacb2786f56d9`.
- Execution tree: `00512125311306a43474638195d2cad97b76118e`.
- Database baseline: `DB-BASELINE-001 = GREENFIELD — NEW — EMPTY — NO LEGACY TABLES / NO LEGACY DATA`.
- PR #69 remains `OPEN / DRAFT / UNMERGED — EVIDENCE ONLY`.

## Binding owner decisions

- `AUTH-001 = RESOLVED — LOCAL APPLICATION AUTHORITY`.
- `ACC-001 = RESOLVED — OPERATIONAL COLLECTION; GOVERNED SETTLEMENT POSTS THE LEDGER`.
- `OFFLINE-001 = RESOLVED — DEFAULT DENY; EXPLICIT QUEUE FOR BOUNDED OPERATIONAL CAPTURE`.
- `CLIENT-001 = RESOLVED — DESKTOP + THREE ANDROID CLIENTS ARE RELEASE TARGETS; IOS DEFERRED`.
- `DB-BASELINE-001 = RESOLVED — GREENFIELD / NEW / EMPTY TARGET DATABASE`.

## Fresh post-correction DB-GOV PASS

The physical dependency correction exists at:

`20608494998e671892ee35abd415158e399c9036`

A fresh independent DB-GOV review issued after that correction returned:

`DB-GOV VERDICT = PASS`

`DEPENDENCY CORRECTION ACCEPTED — NO REMAINING PHYSICAL ORDER BLOCKER IDENTIFIED`

The formal governing record is:

`CONTROL_TOWER/03_DATABASE_GOVERNANCE/DB_GOV_POST_CORRECTION_PASS_DECISION_2026-08-29.md`

The prior formal-recording hold is therefore closed.

## Only approved physical order

`DBP-002 → DBP-004 → DBP-003B/C → DBP-003A → DBP-006 → DBP-005`

Candidate units:

1. `GreenfieldTenantMembershipIsolation` — DBP-002.
2. `GreenfieldAuditV2AndAtomicOutbox` — DBP-004.
3. `GreenfieldDeviceRegistryAndProof` — DBP-003B/C.
4. `GreenfieldLocalAuthSessions` — DBP-003A.
5. `GreenfieldTypedOfflineProtocol` — DBP-006.
6. `GreenfieldGovernedSettlement` — DBP-005.

No alternate order is authorized.

## Authority now opened

MISSION-03 may now perform, on its isolated execution branch only:

- candidate Entity authoring;
- DbContext mapping changes;
- additive forward-only candidate migrations;
- proposal-scoped persistent adapters;
- generated SQL/model-snapshot work;
- PostgreSQL keys/FKs/checks/indexes/RLS-or-equivalent controls;
- synthetic test/rehearsal fixtures outside Production seed;
- disposable/Greenfield PostgreSQL 18.6 candidate application and testing.

This authority is bounded to non-Production rehearsal only.

## Mandatory authoring/rehearsal sequence

For every candidate unit:

1. re-fetch this directive and the latest governance head;
2. bind exact candidate SHA/tree/parent and changed-file inventory;
3. retain migration name/hash, model snapshot diff and generated SQL;
4. start from an empty PostgreSQL 18.6 database;
5. apply the existing ten migrations unchanged;
6. prove baseline backup/restore and catalog/migration reconciliation;
7. apply only the candidate unit allowed by the approved order;
8. run proposal-specific negatives/concurrency/failure-injection tests;
9. run full existing regression and `has-pending-model-changes`;
10. capture post-candidate catalog/FK/index/RLS-equivalent evidence;
11. backup/restore candidate state and reconcile;
12. retain exact artifacts/digests/failures/recovery evidence.

A package failure stops that package and dependents. It does not authorize reordering.

Dependency stop rules:

- DBP-003B/C failure stops DBP-003A and DBP-006.
- DBP-003A failure stops DBP-006.
- DBP-005 depends materially on DBP-002/004 but remains ordered last in the coordinated run.

## Runtime activation boundaries

- Device commands requiring session-family revoke remain disabled until DBP-003A passes.
- Device commands requiring Offline quarantine remain disabled until DBP-006 passes.
- `LOGIN ACTIVATION` remains blocked until new-system password hash/verify/lockout tests pass.
- OFFLINE actions remain governed by OFFLINE-001 default deny.
- Settlement remains governed by ACC-001, configured account roles, FX/rounding, SoD and period controls.

## Post-rehearsal DB-GOV

Successful authoring/rehearsal does not itself authorize Production or final DB acceptance.

After each candidate checkpoint and after the coordinated bundle, submit exact-head evidence to independent DB-GOV for post-rehearsal acceptance. Fix and rerun any failed gate before continuing dependent units.

## Remaining non-DB work

Continue all independently permitted W5/W6/W7 work in parallel. Canonical Shipping/Ticketing/screen authority, executable Windows/Android runtime/secure-store proof, Production signing custody, Production recovery/RPO-RTO/privacy/KMS/dependency/license/provenance approvals and full worktree/stash/local-only preservation inventory remain separate gates.

W8 stays last and no destructive/global cleanup is authorized before its preservation gate.

## Prohibitions

No Production database/data/configuration/credentials. No Production secrets. No edit/delete/squash of the existing ten migrations. No destructive migration/down-migration reliance. No merge to master, rebase, cherry-pick, force-push or history rewrite.

## Return rule

Do not return to the owner after each DBP or Wave. Continue automatically through all enabled MISSION-03 work.

Return only for:

1. `MISSION-03 = COMPLETE — SEALED — DELIVERED TO CONTROL TOWER`; or
2. a genuinely new owner-reserved decision not already covered by current decisions; or
3. a true external-access blocker after all internally permitted work is exhausted.

MISSION-04 remains `WAIT — NOT STARTED` until a valid MISSION-03 seal and handoff exist.
