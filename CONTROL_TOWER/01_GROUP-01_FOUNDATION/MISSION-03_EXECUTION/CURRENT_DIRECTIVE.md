# CURRENT DIRECTIVE — MISSION-03

## v0.9 execution checkpoint routing

`MISSION-03 OPEN — REPOSITORY-RESOLVABLE PREPARATION EXHAUSTED; ROUTE BOUNDED OWNER DECISIONS AND AUTHORIZED EXTERNAL EVIDENCE`

Execution remains bound to `cc67ad2bd491ed3ab23c3144f11dff955353c3a4` /
tree `ea940e592cb11f5fff736e68055ebf77d2eece88`. No new Product baseline is
proposed. The revised DBP-003A design is submitted for independent review, but
rehearsal remains unauthorized. W3–W7 preparation is complete at the evidence/
design level; their Product exits remain blocked by the exact owner/external
items in `MISSION03_COMPLETION_GATE_ASSESSMENT.md`. W8 is not entered.

This routing note does not supersede any DB-GOV hold, grant Product/DB/
Production authority, seal MISSION-03 or start MISSION-04. The detailed prior
bounded execution directive remains below as preserved operational context.

`CONTINUE — CODE-ONLY BASELINE ADOPTED; DBP-003 HOLD AT REHEARSAL ENTRY`

## Accepted execution basis

- MISSION-02 package: `MISSION-02-v1.2 — SEALED — DELIVERED TO CONTROL TOWER`.
- Governing product baseline: `refs/heads/master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`.
- Execution branch: `codex/mission-03-execution-20260828`.
- Accepted W1 checkpoint: `069a311b8f0e66f5d1ee3fdcffed13ec13d0a91a`.
- Previous bounded execution baseline: `9c5b7a12e59d2c42e682717b8e90c491f8699b96`, tree `452b37f1e2c68d9f3dae6e18f1cf1b67645105af`.
- Current bounded code-only execution baseline: `cc67ad2bd491ed3ab23c3144f11dff955353c3a4`, tree `ea940e592cb11f5fff736e68055ebf77d2eece88`.
- PR #69: `601f2d1cad61d62e590a6714ad84e307eb84fe5f — UNMERGED EVIDENCE ONLY`; no merge, bulk copy, cherry-pick, or transferred CI status is authorized.

## Accepted W2 packages

- `DEP-005 = CONTROL TOWER REVALIDATED`.
- `DEP-006 = CONTROL TOWER REVALIDATED FOR AUTHORITY-NEUTRAL CODE-ONLY IMPLEMENTATION`.
- `DEP-007 = CONTROL TOWER REVALIDATED FOR BOUNDED CODE-ONLY IMPLEMENTATION`.
- `W2-A1`, `W2-A2`, `W2-B1`, `W2-B2A`, `W2-C1`, `W2-F1`: `ADOPT — REBOUND TO SEALED PLAN`.
- Exact-head run `33185419917`: `128/128 PASS`; ten existing migrations on disposable PostgreSQL 18.6; no model drift; API HTTP 401 boundary; Desktop and Mobile Admin/Customer/Driver probes PASS.
- `W2-B2B CODE-ONLY`: `ADOPT — EXACT DIFF AND RAW RUN 33191269475 REVALIDATED`; 146/146, existing ten migrations/no drift on disposable PostgreSQL 18.6, HTTP 401 and client build surfaces passed. Desktop/Mobile probes truthfully remain Library-mode, not executable-runtime proof.

## AUTH-001 owner decision

Owner decision is recorded at:

`CONTROL_TOWER/00_GOVERNANCE/DECISIONS/AUTH-001_PRODUCTION_AUTHORITY_MODE_2026-08-28.md`

Decision:

`AUTH-001 = RESOLVED — LOCAL APPLICATION AUTHORITY SELECTED FOR PRODUCTION TARGET`

This resolves the Production authority-mode choice. TransportERP will target an application-owned local token/session authority. Authentication does not become tenant/RBAC/device authority; request-time scope and permissions remain server-resolved and fail closed.

Secrets/signing keys must remain outside source control and no Production credential activation is authorized here.

## W2-B2B direction

The owner-decision blocker on `W2-B2B` is cleared.

MISSION-03 must now continue all non-destructive B2B work enabled by the decision, including issuer-specific contract/design preparation, endpoint/failure behavior, refresh/revoke/logout semantics, secure-client behavior and tests that do not require an unapproved persistence change.

Any B2B implementation requiring memberships, sessions, refresh-family persistence, device/session binding, schema/entity/DbContext/migration/data changes remains behind `DBP-003` and must not cross that gate.

## Remaining bounded gates

- `W2-B2B`: `CODE-ONLY ADOPTED AT cc67ad2...; LOGIN/PERSISTENT ADAPTER ACTIVATION BLOCKED BY DBP-003A AND UNKNOWN PASSWORD-HASH BASELINE`.
- `W2-C2`: blocked by DBP-003/006 plus client key, retention, registry, PoP, revoke, replay/nonce, and override evidence.
- `W2-D`: `BLOCKED — DB-GOV ENTRY GATE NOT SATISFIED` for DBP-002.
- `W2-E`: `BLOCKED — DBP-003A REVISE BEFORE REHEARSAL`; AUTH-001 is no longer a blocker.
- `W2-F2`: remains blocked by the persistence/device/offline/direct-DB/client portions that depend on C2/D/E and the persistence portion of B2B.
- `DBP-003A`: `REVISE BEFORE REHEARSAL`.
- `DBP-003B`: `DEFERRED — DEPENDS ON DBP-002/006`.
- `DBP-003C`: `DEFERRED — DEPENDS ON DBP-002/006`.
- `DBP-003 = HOLD AT REHEARSAL ENTRY`; DBP-002/003/006 authorize no database/schema/persistence/data mutation at this checkpoint.

## Execution direction

Continue immediately from `cc67ad2bd491ed3ab23c3144f11dff955353c3a4` into independently satisfied non-destructive work only.

For DBP-003, the only current directive is proposal/evidence revision:

1. Revise DBP-003A with exact PostgreSQL keys/checks/indexes, lock/re-read transaction, one-successor invariant, atomic audit, serialization retry and rollback/failure-injection behavior.
2. Establish the authorized sanitized `PasswordHash` algorithm/format/salt/legacy/rehash/failure/lockout baseline. Until then: `PASSWORD-HASH BASELINE = UNKNOWN — BLOCKS LOGIN PERSISTENCE ACTIVATION`.
3. Prepare an operational safe-copy package: pre-schema snapshot, data-shape inventory, recoverable backup digest, restore-before-apply proof, pre/post counts, FK/index reconciliation, model drift, boot/regression/new-session tests and forward-recovery evidence.
4. Keep DBP-003B/C deferred behind DBP-002/006. Do not couple device/PoP/nonce objects to the session migration.

No DBP-003 package is open for Entity/DbContext/Migration/schema/persistent-adapter authoring. Permitted new DBP-003 migration environment: `NONE AT THIS CHECKPOINT`. Existing ten-migration disposable baseline verification may be repeated but grants no proposal authority. Resubmit the revised package to DB-GOV; any later approval can be only `APPROVED FOR DISPOSABLE/SAFE-COPY REHEARSAL ONLY` before separate Production review.

Before each material Product commit, re-fetch the latest governance `CURRENT_DIRECTIVE.md` and branch head to avoid another stale-directive plan deviation.

Do not merge to master. Do not rebase, cherry-pick, force-push, rewrite history, mutate Production, author a DBP-003 migration, or start MISSION-04. MISSION-03 remains `IN PROGRESS — OPEN — NOT SEALED`; MISSION-04 remains `WAIT` until a valid final MISSION-03 seal and handoff. No `OWNER DECISION REQUIRED` is raised by this review.
