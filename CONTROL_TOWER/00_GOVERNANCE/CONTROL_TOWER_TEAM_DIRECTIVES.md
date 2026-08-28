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

- `CURRENT DIRECTIVE`: `CONTINUE — POST-CORRECTION DB-GOV PASS RECORDED; BOUNDED GREENFIELD AUTHORING/REHEARSAL AUTHORIZED; KEEP MISSION-03 OPEN`.
- MISSION-03 remains `IN PROGRESS — OPEN — NOT SEALED`.
- Product authority remains `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`.
- Reviewed execution baseline: `codex/mission-03-execution-20260828@5d1352b4fb6d56261dff8b8a622bacb2786f56d9`, tree `00512125311306a43474638195d2cad97b76118e`.
- PR #69 remains comparative unmerged evidence only; no merge is authorized.

### Binding owner decisions

- `AUTH-001 = RESOLVED — LOCAL APPLICATION AUTHORITY`.
- `ACC-001 = RESOLVED — OPERATIONAL COLLECTION; GOVERNED SETTLEMENT POSTS THE LEDGER`.
- `OFFLINE-001 = RESOLVED — DEFAULT DENY; EXPLICIT QUEUE FOR BOUNDED OPERATIONAL CAPTURE`.
- `CLIENT-001 = RESOLVED — DESKTOP + THREE ANDROID CLIENTS ARE RELEASE TARGETS; IOS DEFERRED`.
- `DB-BASELINE-001 = RESOLVED — GREENFIELD / NEW / EMPTY TARGET DATABASE`.

### Fresh post-correction DB-GOV decision

Correction:

`20608494998e671892ee35abd415158e399c9036`

Formal record:

`CONTROL_TOWER/03_DATABASE_GOVERNANCE/DB_GOV_POST_CORRECTION_PASS_DECISION_2026-08-29.md`

Verdict:

`DB-GOV VERDICT = PASS`

`DEPENDENCY CORRECTION ACCEPTED — NO REMAINING PHYSICAL ORDER BLOCKER IDENTIFIED`

The prior rehearsal-entry/formal-recording hold is closed.

### Only approved coordinated physical order

`DBP-002 → DBP-004 → DBP-003B/C → DBP-003A → DBP-006 → DBP-005`

Candidate units:

1. `GreenfieldTenantMembershipIsolation`.
2. `GreenfieldAuditV2AndAtomicOutbox`.
3. `GreenfieldDeviceRegistryAndProof`.
4. `GreenfieldLocalAuthSessions`.
5. `GreenfieldTypedOfflineProtocol`.
6. `GreenfieldGovernedSettlement`.

No alternate order is authorized.

### Bounded authority now opened

MISSION-03 may perform candidate Entity/DbContext/persistent-adapter authoring, additive forward-only candidate migrations, generated SQL/model-snapshot changes, and candidate application/testing only on its isolated branch and disposable/Greenfield non-Production PostgreSQL 18.6.

For every candidate checkpoint it must bind exact SHA/tree/parent, changed files, migration/hash/generated SQL, apply the existing ten migrations unchanged to an empty baseline, prove baseline backup/restore, apply only the authorized next candidate, run proposal-specific negatives/concurrency/failure injection plus full regression/model-drift checks, then prove candidate backup/restore/catalog reconciliation and retain artifact digests.

### Dependency and activation gates

- Failure of DBP-003B/C stops DBP-003A and DBP-006.
- Failure of DBP-003A stops DBP-006.
- DBP-005 materially depends on DBP-002/004 but remains ordered last in the coordinated run.
- Device behavior requiring session-family revoke remains disabled until DBP-003A passes.
- Device behavior requiring Offline quarantine remains disabled until DBP-006 passes.
- `LOGIN ACTIVATION` remains blocked until new-system password hash/verify/lockout tests pass.
- OFFLINE remains default-deny under OFFLINE-001.
- Settlement remains governed by ACC-001, configuration-driven account roles, FX/rounding, SoD and fiscal-period controls.

### Required continuation

MISSION-03 must now continue automatically through all enabled work. Do not return after each DBP or Wave.

After every candidate package and the coordinated bundle, submit exact-head rehearsal evidence for independent post-rehearsal DB-GOV. Fix and rerun failed gates before continuing dependent units.

Continue unrelated W5/W6/W7 work in parallel where its own gates permit. Keep W8 last and do not perform destructive/global cleanup before the preservation gate is satisfied.

Remaining non-DB gates include canonical post-DEPART Shipping/Ticketing/screen authority, real Windows/Android executable runtime and secure-store proof, protected signing custody, Production recovery/RPO-RTO/privacy/KMS/dependency/license/provenance approvals, and complete worktree/stash/local-only preservation inventory before W8 cleanup.

### Prohibitions

No Production database/data/configuration/credentials. No Production secrets. Do not edit/delete/squash the existing ten migrations. No destructive migration/down-migration reliance. No merge to master, rebase, cherry-pick, force-push or history rewrite.

No `OWNER DECISION REQUIRED` is active for the DB-GOV dependency package.

## MISSION-04

- `CURRENT DIRECTIVE`: `WAIT`.
- Prerequisite: MISSION-03 must be conclusively sealed and handed off with exact execution SHAs, report/evidence/manifest/detached SHA-256/seal/handoff, preservation/rollback and DB-GOV compliance independently verified.
- MISSION-03 is still open/not sealed; M04 dispatch remains prohibited.
- Independence from MISSION-03 execution remains mandatory.

## MISSION-05

- `CURRENT DIRECTIVE`: `WAIT`.
- Prerequisite: MISSION-04 must be sealed and handed off.

`DB-GOV-001` remains binding throughout. No Production Database, Schema, Entity, Migration, field, relationship, index, constraint, seed or data change may execute without its required governance, impact, preservation, test/recovery and explicit Production authority.
