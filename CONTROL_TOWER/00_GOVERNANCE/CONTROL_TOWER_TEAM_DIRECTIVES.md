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
- Remote governance delivery chain accepted through `85fb92b664a70fab497b60962bf34753a66f7dce`.
- Accepted planning scope: 64/64 findings; both P0s; all governing P1s; 8/8 workstreams `PLANNED`; 20 remediation packages; waves `W0–W8`; all proposed DB changes gated through `DB-GOV-001`.
- Product modification authority exercised by MISSION-02: `NONE`.
- Next permitted action: none unless controlled `REOPEN` is issued.

## MISSION-03

- `CURRENT DIRECTIVE`: `CONTINUE — DB REHEARSAL ENTRY HOLD; RESOLVE DBP-003B/C ↔ DBP-006 ORDER CONFLICT; KEEP MISSION-03 OPEN`.
- MISSION-03 remains `IN PROGRESS — OPEN — NOT SEALED`.
- Product authority remains `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`.
- Current execution branch/head: `codex/mission-03-execution-20260828@5d1352b4fb6d56261dff8b8a622bacb2786f56d9`, tree `00512125311306a43474638195d2cad97b76118e`.
- PR #69 remains comparative unmerged evidence only; no merge is authorized.

### Accepted bounded execution evidence

- W1 REM-100 checkpoint: `069a311b8f0e66f5d1ee3fdcffed13ec13d0a91a`.
- W2-A1/A2/B1/B2A/C1/F1: `ADOPT — REBOUND TO SEALED PLAN`.
- W2-B2B code-only: `ADOPT — EXACT DIFF/RAW CI REVALIDATED` through `cc67ad2bd491ed3ab23c3144f11dff955353c3a4`.
- Final current internal head: `5d1352b4fb6d56261dff8b8a622bacb2786f56d9`.
- Run `33201720896`: `153/153 PASS`, PostgreSQL 18.6, ten existing migrations, no model drift, expected API HTTP 401; Desktop/Mobile probes remain build/scaffold evidence rather than executable runtime proof.
- Run `33201720878`: disposable PostgreSQL backup/restore `PASS`, source/restored migration-history `10/10`.
- No candidate Entity, DbContext model, Migration, Schema, Seed, persistent adapter, Product data or Production configuration delta exists after this baseline.

### Binding owner decisions

- `AUTH-001 = RESOLVED — LOCAL APPLICATION AUTHORITY`.
- `ACC-001 = RESOLVED — OPERATIONAL COLLECTION; GOVERNED SETTLEMENT POSTS THE LEDGER`.
- `OFFLINE-001 = RESOLVED — DEFAULT DENY; EXPLICIT QUEUE FOR BOUNDED OPERATIONAL CAPTURE`.
- `CLIENT-001 = RESOLVED — DESKTOP + THREE ANDROID CLIENTS ARE RELEASE TARGETS; IOS DEFERRED`.
- `DB-BASELINE-001 = RESOLVED — GREENFIELD / NEW / EMPTY TARGET DATABASE`.

### Greenfield DB-GOV post-resubmission revalidation

The exact v1.0 physical design and Greenfield acceptance specification are present. A mission-local review decision nominally approves coordinated disposable/Greenfield non-Production rehearsal, but repository chronology shows the review decision existed at `fc2e28f86b297203be9f857f507d40629d9bbb35` before the exact v1.0 physical resubmission was committed in `8b97d99e481ed2b6f4a7e90a5d4790ebdcac8219`.

Control Tower independently revalidated the current package and recorded:

`CONTROL_TOWER/03_DATABASE_GOVERNANCE/DB_GOV_POST_RESUBMISSION_REVALIDATION_2026-08-29.md`

Current DB-GOV result:

`HOLD AT COORDINATED GREENFIELD REHEARSAL ENTRY — POST-RESUBMISSION DB-GOV REVALIDATION REQUIRED`

The evidence-bound conflict is exact:

- physical design: `DBP-002 → DBP-004 → DBP-003A → DBP-003B/C → DBP-006 → DBP-005`;
- earlier review decision: `DBP-002 → DBP-004 → DBP-003A → DBP-006 → DBP-003B/C → DBP-005`;
- the physical design makes DBP-006 depend on device/proof persistence introduced by DBP-003B/C.

Therefore no candidate Entity/DbContext/Migration/Schema/Seed/persistent-adapter authoring and no candidate migration application is authorized until one corrected post-resubmission dependency decision removes the contradiction.

### Required continuation

MISSION-03 must continue; the analytical chain is not stopped. The immediate delegated work is non-destructive:

1. reconcile DBP-003B/C ↔ DBP-006 ordering by either retaining device/proof before Offline persistence and correcting the review order, or splitting DBP-006 into a physically independent pre-device core plus a later device/proof-bound extension;
2. bind the corrected package to the exact execution parent SHA/tree, candidate-unit identities, FK/index dependencies and acceptance tests;
3. obtain a fresh independent DB-GOV decision after the corrected repository package exists;
4. continue unrelated non-destructive W5/W6/W7 preparation where its own gates permit;
5. keep W8 last and preserve all worktree/stash/local-only evidence before any destructive/global cleanup.

`MISSION-03-GREENFIELD-DBP-RESUBMISSION-v1.1` remains an open historical checkpoint. Because Control Tower directives/registers changed after it, the next worker checkpoint must produce a new manifest and detached SHA-256 set before any later acceptance or seal claim.

Remaining non-DB gates include canonical post-DEPART Shipping/Ticketing/screen authority, real Windows/Android executable runtime and secure-store proof, protected signing custody, Production recovery/RPO-RTO/privacy/KMS/dependency/license/provenance approvals, and complete Git worktree/stash/local-only preservation inventory before destructive/global W8 cleanup.

Do not merge, delete, reset, rewrite, force-push, cherry-pick, mutate Production, commit secrets, or perform Entity/DbContext/Migration/Schema/Seed/Data work while the DB-GOV rehearsal-entry hold is active.

No `OWNER DECISION REQUIRED` is active; the next permitted action is delegated non-destructive DB-GOV correction/revalidation.

## MISSION-04

- `CURRENT DIRECTIVE`: `WAIT`.
- Prerequisite: MISSION-03 must be conclusively sealed and handed off with exact execution SHAs, report/evidence/manifest/detached SHA-256/seal/handoff, preservation/rollback and DB-GOV compliance independently verified.
- MISSION-03 is still open/not sealed; M04 dispatch remains prohibited.
- Independence from MISSION-03 execution remains mandatory.

## MISSION-05

- `CURRENT DIRECTIVE`: `WAIT`.
- Prerequisite: MISSION-04 must be sealed and handed off.

`DB-GOV-001` remains binding throughout. No Database, Schema, Entity, Migration, field, relationship, index, constraint, seed or data change may execute without its required governance, impact, preservation, test/recovery and explicit execution authority.
