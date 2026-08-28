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

- `CURRENT DIRECTIVE`: `CONTINUE — GREENFIELD DB-GOV RE-REVIEW COMPLETE; REVISE PROPOSALS; KEEP MISSION-03 OPEN`.
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
- No Entity, DbContext model, Migration, Schema, Seed, persistent adapter, Product data or Production configuration delta is authorized or claimed by Control Tower.

### Binding owner decisions

- `AUTH-001 = RESOLVED — LOCAL APPLICATION AUTHORITY`.
- `ACC-001 = RESOLVED — OPERATIONAL COLLECTION; GOVERNED SETTLEMENT POSTS THE LEDGER`.
- `OFFLINE-001 = RESOLVED — DEFAULT DENY; EXPLICIT QUEUE FOR BOUNDED OPERATIONAL CAPTURE`.
- `CLIENT-001 = RESOLVED — DESKTOP + THREE ANDROID CLIENTS ARE RELEASE TARGETS; IOS DEFERRED`.
- `DB-BASELINE-001 = RESOLVED — GREENFIELD / NEW / EMPTY TARGET DATABASE`.

### Greenfield DB-GOV re-review

Control Tower completed the second independent Greenfield review in:

`CONTROL_TOWER/03_DATABASE_GOVERNANCE/DB_GOV_GREENFIELD_REREVIEW_DECISION_2026-08-28.md`

Result:

`GREENFIELD LEGACY-DATA BLOCKERS CLEARED — PROPOSAL-SPECIFIC DESIGN GATES REMAIN — NO DB/MIGRATION REHEARSAL AUTHORITY YET`

Therefore legacy target-row/backfill evidence, legacy PasswordHash/verifier/rehash compatibility, legacy audit/accounting row reconciliation, and a safe-copy of a pre-existing target database are no longer MISSION-03 target-database prerequisites.

Current DB-GOV dispositions:

- `DBP-002 = REVISE BEFORE REHEARSAL`.
- `DBP-003A = REVISE BEFORE REHEARSAL`.
- `DBP-003B/C = DEFERRED — DEPENDS ON DBP-002/006`.
- `DBP-004 = REVISE BEFORE REHEARSAL`.
- `DBP-005 = REVISE BEFORE REHEARSAL`.
- `DBP-006 = REVISE BEFORE REHEARSAL`.
- no proposal is currently `APPROVED FOR DISPOSABLE/GREENFIELD NON-PRODUCTION REHEARSAL ONLY`.

### Required continuation

MISSION-03 must continue non-destructive design/governance refinement: exact candidate physical specifications for DBP-002/003A/004/005/006; new-system password hash/verify/lockout policy; shared caller-owned transaction/audit boundary; Greenfield PostgreSQL role/RLS-equivalent bootstrap; retention/legal-hold/cleanup/recovery for device proof, nonce/replay, Offline queue and audit; then re-submit to independent DB-GOV.

Remaining non-DB gates include canonical post-DEPART Shipping/Ticketing/screen authority, real Windows/Android executable runtime and secure-store proof, protected signing custody, Production recovery/RPO-RTO/privacy/KMS/dependency/license/provenance approvals, and complete Git worktree/stash/local-only preservation inventory before destructive/global W8 cleanup.

Do not merge, delete, reset, rewrite, force-push, cherry-pick, mutate Production, commit secrets, or perform Entity/DbContext/Migration/Schema/Seed/Data work without the exact independently opened gate.

No `OWNER DECISION REQUIRED` is active from the Greenfield DB-GOV re-review; the next permitted work is non-destructive.

## MISSION-04

- `CURRENT DIRECTIVE`: `WAIT`.
- Prerequisite: MISSION-03 must be conclusively sealed and handed off with exact execution SHAs, report/evidence/manifest/detached SHA-256/seal/handoff, preservation/rollback and DB-GOV compliance independently verified.
- MISSION-03 is still open/not sealed; M04 dispatch remains prohibited.
- Independence from MISSION-03 execution remains mandatory.

## MISSION-05

- `CURRENT DIRECTIVE`: `WAIT`.
- Prerequisite: MISSION-04 must be sealed and handed off.

`DB-GOV-001` remains binding throughout. No Database, Schema, Entity, Migration, field, relationship, index, constraint, seed or data change may execute without its required governance, impact, preservation, test/recovery and explicit execution authority.
