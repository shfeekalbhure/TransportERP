# CONTROL TOWER TEAM DIRECTIVES

Every team or mission must first read, in order: `CONTROL_TOWER/README.md`, `OWNER_DELEGATION_AND_OPERATING_DIRECTIVE.md`, its own section here, its mission order, its mission-local `CURRENT_DIRECTIVE.md`, and all required sealed predecessor outputs. Only Control Tower changes a `CURRENT DIRECTIVE`. A team at `WAIT`, `HOLD`, or `STOP` must not work. A sealed team must not modify its output unless this file issues `REOPEN` or `RETURN FOR REWORK`.

## Governing owner decision now in force

Authoritative current product line:

`refs/heads/master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`

PR #69 `codex/p1-security-device-sync-offline-20260825@601f2d1cad61d62e590a6714ad84e307eb84fe5f` is `UNMERGED REMEDIATION / FINAL CANDIDATE`, not CURRENT. No merge is authorized.

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

- `CURRENT DIRECTIVE`: `CONTINUE — CODE-ONLY BASELINE ADOPTED; DBP-003 HOLD AT REHEARSAL ENTRY`.
- MISSION-03 remains `IN PROGRESS` and `NOT SEALED`.
- Accepted W1 checkpoint: `codex/mission-03-execution-20260828@069a311b8f0e66f5d1ee3fdcffed13ec13d0a91a`.
- Current bounded execution baseline: `codex/mission-03-execution-20260828@cc67ad2bd491ed3ab23c3144f11dff955353c3a4`, tree `ea940e592cb11f5fff736e68055ebf77d2eece88`.
- DEP-005 is Control Tower revalidated for current-source design/code-only scope; live rows/roles/RLS remain DBP-002-only blockers.
- DEP-006 is Control Tower revalidated for authority-neutral code-only implementation; `AUTH-001 = RESOLVED — LOCAL APPLICATION AUTHORITY`.
- DEP-007 is Control Tower revalidated for bounded owner/lifecycle code-only implementation; registry/PoP/nonce/replay/session-device persistence remains behind DBP-003/006.
- W2-A1/A2/B1/B2A/C1/F1: `ADOPT — REBOUND TO SEALED PLAN` and ready for later independent verification.
- W2-B2B code-only: `ADOPT — EXACT DIFF/RAW CI REVALIDATED` at `cc67ad2...`.
- DBP-003A/W2-B2B persistence: `REVISE BEFORE REHEARSAL`; PasswordHash baseline, PostgreSQL rotation/atomic-audit design and safe-copy package are missing.
- DBP-003B/C and W2-C2: `DEFERRED — DEPENDS ON DBP-002/006`; W2-D/E/F2 remain individually blocked. They do not stop unrelated satisfied packages.
- Exact-head run `33185419917` and decoded logs/artifacts confirm 128/128, ten existing migrations/no drift, API 401, Desktop and Mobile x3 at `9c5b7a1...`; failed run `33184771338` remains historical evidence.
- Raw run `33191269475` and artifacts confirm exact `cc67ad2...` / `ea940e5...`, PostgreSQL 18.6, ten existing migrations/no drift, 146/146 and HTTP 401. Desktop/Mobile x3 builds pass; probes remain Library-mode, not executable-runtime proof.
- No Entity, DbContext model, Migration, schema, seed, data repair or Production configuration change occurred. No DBP-003 package is open for migration authoring.
- The earlier retained-hold decision is preserved and superseded by `CONTROL_TOWER/00_GOVERNANCE/DECISIONS/MISSION_03_W2_BOUNDED_ADOPTION_DECISION_2026-08-28.md`, which records the additional persistent-scope analysis and current owner direction.
- Do not merge, delete, reset, rewrite, force-push, cherry-pick, mutate Production, or perform DB/schema/data work without its independent gate.
- PR #69 remains comparative unmerged evidence only; no merge is authorized.

## MISSION-04

- `CURRENT DIRECTIVE`: `WAIT`.
- Prerequisite: MISSION-03 must be sealed and handed off with exact execution SHAs, tests/evidence, preservation/rollback and DB-GOV compliance verified.
- MISSION-03 is not sealed; the B2B code-only checkpoint is not a final handoff and M04 dispatch remains prohibited.
- Independence from MISSION-03 execution remains mandatory.

## MISSION-05

- `CURRENT DIRECTIVE`: `WAIT`.
- Prerequisite: MISSION-04 must be sealed and handed off.

`DB-GOV-001` remains binding throughout. No Database, Schema, Entity, Migration, field, or relationship change may execute without its required governance, impact, preservation, test/recovery and execution authority.
