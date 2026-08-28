# DB-GOV Execution Register

`DB-GOV-001` is binding. No database, schema, entity, migration, field, relationship, index, constraint, type, seed, precision or numbering change was executed. Applying the already-committed migration lineage to an empty disposable PostgreSQL database was verification only.

The central proposal register was re-read through governance `6b2d238...`; DBP-001..009 are registered for intake. W2 ADRs and AUTH-001 resolve design dependencies but do not grant schema/data execution authority.

Control Tower independently revalidated and adopted the authority-neutral code-only W2 controls and the B2B code-only head `cc67ad2...`. DBP-002/003/006 remain blocked for every material database/schema/persistence/data action; the adopted code does not activate those proposals.

| Proposal | Relevant REM | Current execution gate | Result |
|---|---|---|---|
| `DBP-001` | `REM-100` | code-only mapper path authorized; no schema/migration/data mutation; disposable PostgreSQL test path passed | `CODE-ONLY IMPLEMENTED; DATA ASSESSMENT/REPAIR REMAINS BLOCKED` |
| `DBP-002` | `REM-210` | current model and ten-migration lineage inventoried; ADR-W2-001 CT-revalidated; disposable DB/model-drift evidence passed; live rows/roles/RLS, complete impact/backfill/forward/recovery and execution authorization absent | `BLOCKED — DB-GOV ENTRY GATE NOT SATISFIED; A1/A2 CODE-ONLY CONTROLS ADOPTED INDEPENDENTLY` |
| `DBP-003A` | `REM-200` | exact code-only diff and run 33191269475 verified; `user_security_state` omits claimed lockout shape/concurrency detail; `auth_sessions` lacks executable PostgreSQL family-lock/single-successor/atomic-audit design; PasswordHash and safe-copy evidence absent | `REVISE BEFORE REHEARSAL — NO ENTITY/DBCONTEXT/MIGRATION/ADAPTER AUTHORITY` |
| `DBP-003B` | `REM-220` | registry/assignment depends on device lifecycle plus explicit membership/cardinality and tenant-consistent keys | `DEFERRED — DEPENDS ON DBP-002/006` |
| `DBP-003C` | `REM-220` | PoP/nonce/replay store, uniqueness scope, retention/legal hold and recovery not established | `DEFERRED — DEPENDS ON DBP-002/006` |
| `DBP-004` | `REM-320` | UoW ADR/legacy sample/live controls absent | `BLOCKED` |
| `DBP-005` | `REM-310` | accounting authority/reconciliation absent | `BLOCKED` |
| `DBP-006` | `REM-400` | offline authority/protocol and DB baseline absent; no nonce/replay/offline persistence authorized | `BLOCKED — DB-GOV ENTRY GATE NOT SATISFIED` |
| `DBP-007` | `REM-600` | canonical scope absent | `BLOCKED` |
| `DBP-008` | `REM-610` | canonical Ticketing requirements absent | `BLOCKED` |
| `DBP-009` | reporting | requirements absent | `BLOCKED` |

No Entity, DbContext, Migration, Seed, Schema, data or Production credential was changed. Applying the existing ten migrations to an empty disposable database was verification only. The proposal is `DBP-003_SESSION_PERSISTENCE_PROPOSAL.md`; the independent disposition and exact evidence are in `DBP-003_DB_GOV_REVIEW_DECISION.md`.

## v0.9 resubmission preparation

`DBP-003A_REHEARSAL_RESUBMISSION.md` addresses the repository-resolvable design
review findings: exact proposed keys/checks/indexes, failure/lockout state,
tenant boundary, serializable family locking and re-read, one-successor
invariants, atomic caller-owned audit, SQLSTATE/constraint retry, ambiguous
commit recovery and failure injection. Read-only inventory/reconciliation SQL
and a safe-copy/backup/restore runbook are prepared.

This does not supersede the independent HOLD. Actual PasswordHash format,
authorized safe-copy outputs/backup restore, live roles/RLS and DBP-002/006
dependencies remain absent. Therefore:

- `DBP-003A = RESUBMITTED DESIGN — AWAITING INDEPENDENT REVIEW; REHEARSAL NOT AUTHORIZED`;
- `DBP-003B/C = DEFERRED — DEPENDS ON DBP-002/006`;
- permitted Entity/DbContext/Migration/persistent-adapter/data execution: `NONE`.

W3/W4/W6 revalidation additionally confirms DBP-004/005/006/007/008 remain
blocked at proposal/external-authority gates. Reporting ownership under DBP-009
is a bounded plan-deviation item, not an execution authority.

## DBP-003 review boundary

- Reviewed execution head/tree: `cc67ad2bd491ed3ab23c3144f11dff955353c3a4` / `ea940e592cb11f5fff736e68055ebf77d2eece88`.
- Exact diff: three new code/contracts/test files, 992 insertions; migration/model/project/Production configuration counts unchanged.
- Raw CI: PostgreSQL 18.6, ten existing migrations, no model drift, 146/146, HTTP 401 and all four client build jobs succeeded. Client probes remain Library-mode, not executable-runtime proof.
- `NO NEW PERSISTENCE CHANGE` in Git; `DISPOSABLE TEST DATABASE MUTATION OCCURRED AS PART OF VALIDATION` in CI.
- Overall: `DBP-003 = HOLD AT REHEARSAL ENTRY`.
- No `OWNER DECISION REQUIRED`; all current next actions are proposal/evidence/rehearsal preparation and are non-destructive.

The v0.9 end-to-end assessment now raises bounded non-DB business decisions for
accounting, Offline actions and client delivery/signing. It does not raise or
self-decide any DB-GOV approval.
