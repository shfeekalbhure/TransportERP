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

- `CURRENT DIRECTIVE`: `CONTINUE`.
- MISSION-03 remains `IN PROGRESS` and `NOT SEALED`.
- Accepted execution checkpoint: W0 bounded exit plus W1 `REM-100` at `codex/mission-03-execution-20260828@069a311b8f0e66f5d1ee3fdcffed13ec13d0a91a`; exact-head run `33181376288` and retained artifacts were independently reverified by Control Tower.
- W1 disposition: `IMPLEMENTED — READY FOR INDEPENDENT VERIFICATION` later by MISSION-04 after a valid final MISSION-03 seal/handoff.
- `W2 AFFECTED DIRECTIVE`: `HOLD — STOP/REPLAN REQUIRED`.
- Reason: after the accepted W1 checkpoint, the isolated execution branch advanced through W2-scope security/tenant/RBAC/Sync Product changes beginning at `a157c34d6767deeb5544adf456a2a36946a599a9` and later `d1c0a2571bf3d240b9134e8614186acd70a6bd5d`, while the governing MISSION-02/MISSION-03 records still show `DEP-005`, `DEP-006`, `DEP-007`, `DBP-002`, and `DBP-003` unsatisfied/blocked.
- CI success on those candidate commits is evidence only; it does not satisfy or replace missing design, authority, live-baseline, preservation, rollback/recovery, or DB-GOV entry gates.
- Preserve the later commits as `UNACCEPTED ISOLATED CANDIDATE EVIDENCE`. Do not merge, delete, rewrite, force-push, or silently adopt them.
- No further W2 Product modification is authorized until the missing W2 prerequisites are supplied, independently reverified, and the affected package is rebound/replanned through the sealed MISSION-02 contract.
- Non-destructive read-only evidence gathering, ADR preparation, dependency reconciliation, safe test design, and DB-GOV proposal preparation may continue where they do not cross an unmet execution gate.
- DB/data portions remain separately blocked. No Production or database mutation is authorized.
- No owner-decision hold is required at this checkpoint because the candidate is isolated/unmerged and no destructive, Production, irreversible, data-repair, merge, or history-rewrite action is presently required.
- PR #69 remains comparative unmerged evidence only; no merge is authorized.

## MISSION-04

- `CURRENT DIRECTIVE`: `WAIT`.
- Prerequisite: MISSION-03 must be sealed and handed off with exact execution SHAs, tests/evidence, preservation/rollback and DB-GOV compliance verified.
- MISSION-03 is not sealed; the W2 governance hold prevents M04 dispatch.
- Independence from MISSION-03 execution remains mandatory.

## MISSION-05

- `CURRENT DIRECTIVE`: `WAIT`.
- Prerequisite: MISSION-04 must be sealed and handed off.

`DB-GOV-001` remains binding throughout. No Database, Schema, Entity, Migration, field, or relationship change may execute without its required governance, impact, preservation, test/recovery and execution authority.
