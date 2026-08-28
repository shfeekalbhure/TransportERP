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

- `CURRENT DIRECTIVE`: `CONTINUE` for non-destructive prerequisite/evidence reconciliation only; the affected W2 Product directive remains `HOLD`.
- MISSION-03 remains `IN PROGRESS` and `NOT SEALED`.
- Accepted execution checkpoint: W0 bounded exit plus W1 `REM-100` at `codex/mission-03-execution-20260828@069a311b8f0e66f5d1ee3fdcffed13ec13d0a91a`; exact-head run `33181376288` and retained artifacts were independently reverified by Control Tower.
- W1 disposition: `IMPLEMENTED — READY FOR INDEPENDENT VERIFICATION` later by MISSION-04 after a valid final MISSION-03 seal/handoff.
- Latest received checkpoint: `MISSION-03-W2-REVALIDATION-HOLD-CHECKPOINT-v0.5` at `codex/mission-03-execution-20260828@9c5b7a12e59d2c42e682717b8e90c491f8699b96`.
- Control Tower independently reverified the exact candidate head, the W1→W2 compare, Actions run `33185419917`, both successful jobs and retained artifact digests. The candidate is technically successful evidence and contains no Entity, DbContext, Migration, Seed, schema, data or Production configuration change.
- `W2 AFFECTED DIRECTIVE`: `HOLD — RETAINED AFTER INDEPENDENT REVALIDATION — NO FURTHER W2 PRODUCT MODIFICATION`.
- Control Tower revalidation decision: `CONTROL_TOWER/00_GOVERNANCE/DECISIONS/MISSION_03_W2_REVALIDATION_DECISION_2026-08-28.md`.
- Reason: the sealed MISSION-02 contract still requires live-role evidence for `DEP-005`, IdP mode/config plus DEP-005 for `DEP-006`, and DEP-005/006 for `DEP-007`; W2 entry also requires the recorded tenant/IdP evidence and `DBP-002/003` review state. Current MISSION-03 DB-GOV records still mark DBP-002/003 entry gates unsatisfied. Candidate authorization logic also relies on null/company-wide branch-scope semantics that are not yet proven against authoritative live user/role evidence.
- ADR-W2-001/002/003 are retained as substantive candidate design evidence but do not release their sealed W2 execution gates at this checkpoint.
- Candidate packages `W2-A1/A2/B1/B2A/C1/F1` remain `PRESERVED TECHNICAL CANDIDATE — NOT ADOPTED AS EXECUTION BASELINE`.
- `W2-B2B/C2/D/E/F2` remain blocked by their recorded owner-authority, live-baseline, upstream dependency and/or DB-GOV conditions.
- Preserve all post-W1 commits and exact run evidence. Do not merge, delete, reset, rewrite, force-push, cherry-pick, silently adopt or continue Product implementation from them.
- Permitted work is limited to non-destructive authoritative evidence gathering, live-role/tenant-cardinality reconciliation where access is authorized, IdP authority evidence, ADR/package rebinding, safe test design and DB-GOV impact/preservation/recovery proposal preparation.
- DB/data portions remain separately blocked. No Production or database mutation is authorized.
- Bounded owner items such as `AUTH-001` remain carried forward but do not create an immediate global owner hold while the actual next permitted work is non-destructive prerequisite reconciliation.
- PR #69 remains comparative unmerged evidence only; no merge is authorized.

## MISSION-04

- `CURRENT DIRECTIVE`: `WAIT`.
- Prerequisite: MISSION-03 must be sealed and handed off with exact execution SHAs, tests/evidence, preservation/rollback and DB-GOV compliance verified.
- MISSION-03 is not sealed; the independently revalidated W2 hold remains in force and prevents M04 dispatch.
- Independence from MISSION-03 execution remains mandatory.

## MISSION-05

- `CURRENT DIRECTIVE`: `WAIT`.
- Prerequisite: MISSION-04 must be sealed and handed off.

`DB-GOV-001` remains binding throughout. No Database, Schema, Entity, Migration, field, or relationship change may execute without its required governance, impact, preservation, test/recovery and execution authority.
