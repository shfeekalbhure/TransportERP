# CURRENT DIRECTIVE — MISSION-03

`CONTINUE — AUTH-001 RESOLVED; EXECUTE ALL NON-DESTRUCTIVE W2 WORK ENABLED BY LOCAL AUTHORITY DECISION`

## Accepted execution basis

- MISSION-02 package: `MISSION-02-v1.2 — SEALED — DELIVERED TO CONTROL TOWER`.
- Governing product baseline: `refs/heads/master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`.
- Execution branch: `codex/mission-03-execution-20260828`.
- Accepted W1 checkpoint: `069a311b8f0e66f5d1ee3fdcffed13ec13d0a91a`.
- Current bounded execution baseline: `9c5b7a12e59d2c42e682717b8e90c491f8699b96`, tree `452b37f1e2c68d9f3dae6e18f1cf1b67645105af`.
- PR #69: `601f2d1cad61d62e590a6714ad84e307eb84fe5f — UNMERGED EVIDENCE ONLY`; no merge, bulk copy, cherry-pick, or transferred CI status is authorized.

## Accepted W2 packages

- `DEP-005 = CONTROL TOWER REVALIDATED`.
- `DEP-006 = CONTROL TOWER REVALIDATED FOR AUTHORITY-NEUTRAL CODE-ONLY IMPLEMENTATION`.
- `DEP-007 = CONTROL TOWER REVALIDATED FOR BOUNDED CODE-ONLY IMPLEMENTATION`.
- `W2-A1`, `W2-A2`, `W2-B1`, `W2-B2A`, `W2-C1`, `W2-F1`: `ADOPT — REBOUND TO SEALED PLAN`.
- Exact-head run `33185419917`: `128/128 PASS`; ten existing migrations on disposable PostgreSQL 18.6; no model drift; API HTTP 401 boundary; Desktop and Mobile Admin/Customer/Driver probes PASS.

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

- `W2-B2B`: `AUTH-001 RESOLVED — LOCAL MODE SELECTED; PERSISTENCE PORTION BLOCKED BY DBP-003`.
- `W2-C2`: blocked by DBP-003/006 plus client key, retention, registry, PoP, revoke, replay/nonce, and override evidence.
- `W2-D`: `BLOCKED — DB-GOV ENTRY GATE NOT SATISFIED` for DBP-002.
- `W2-E`: `BLOCKED — DB-GOV ENTRY GATE NOT SATISFIED` for DBP-003; AUTH-001 is no longer a blocker.
- `W2-F2`: remains blocked by the persistence/device/offline/direct-DB/client portions that depend on C2/D/E and the persistence portion of B2B.
- DBP-002, DBP-003, and DBP-006 authorize no database/schema/persistence/data mutation at this checkpoint.

## Execution direction

Continue immediately from `9c5b7a12e59d2c42e682717b8e90c491f8699b96` into every independently satisfied non-destructive package. Prepare/reconcile DBP-003 evidence and proposal requirements in parallel, but do not implement any Entity, DbContext, Migration, schema, seed, data repair or Production mutation until DB-GOV entry gates are independently satisfied.

Before each material Product commit, re-fetch the latest governance `CURRENT_DIRECTIVE.md` and branch head to avoid another stale-directive plan deviation.

Do not merge to master. Do not rebase, cherry-pick, force-push, rewrite history, mutate Production, or start MISSION-04. MISSION-03 remains `IN PROGRESS — OPEN — NOT SEALED`; MISSION-04 remains `WAIT` until a valid final MISSION-03 seal and handoff.
