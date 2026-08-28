# TEAM-C2 Migration and DB-Governance Constraints

Status: `PROPOSED CONSTRAINTS — NO DATABASE CHANGE AUTHORIZED`.

## 1. Governing rule

`DB-GOV-001` applies to every proposed Entity, field, relationship, key, index, query filter, RLS policy, trigger, role/grant, DbContext, migration, schema, data correction, and database deployment. The current state and live database are not proven, and the authoritative product line is unknown.

No proposal may move into execution until `DATABASE_CURRENT_STATE_REGISTER.md` and `DATABASE_CHANGE_PROPOSAL_REGISTER.md` contain a reviewed, exact-SHA-bound entry with UI/API/Accounting/Sync impact, preservation, disposable test, rollback/recovery, and authority.

## 2. Mandatory current-state capture before change

1. authoritative ref and full SHA;
2. exact EF model, model snapshot, migration list/order/hashes, manual SQL/triggers;
3. applied migration history and schema drift from an authorized read-only source;
4. table/column/key/index/FK/check/trigger/RLS/role/grant inventory;
5. row counts and targeted integrity queries using safe/redacted evidence;
6. backup/restore evidence and recovery objectives;
7. current application and migration credentials/permissions without exposing secrets;
8. exact build/test/migrate/boot evidence in a disposable environment.

If unavailable: `ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION`; do not infer Production state from migrations.

## 3. Lineage and transition constraints

- Never rewrite, reorder, squash, or silently replace the existing migration lineage or model snapshot.
- Use forward-only transitions after a verified baseline; downgrade scripts alone are not recovery evidence.
- Keep current table/column/ID semantics stable while introducing logical module ownership.
- Do not split the DbContext, migration assembly, or physical schema in one step. First isolate configurations/repositories and prove parity against unchanged tables.
- If multiple DbContexts are later approved, define ownership, transaction scope, history-table strategy, design-time tooling, and cross-context outbox/inbox behavior.
- No distributed transaction assumption is allowed. Cross-module effects must be idempotent and observable.
- Retain current scope predicates, CAS, idempotency, serializable paths, precision/status constraints, and append-only triggers until stronger controls pass regression and negative tests.

## 4. `Volume` P0 controls

Before remediation:

1. register one change proposal bound to the authoritative SHA;
2. preserve the field's meaning, nullability, precision, API/domain/entity/read/allocation semantics;
3. reproduce create→update→reload→allocate behavior in disposable PostgreSQL;
4. run an authorized read-only impact query against a safe copy or approved environment;
5. separate code correction from data repair;
6. if data repair is needed, define source of truth, ambiguity handling, backup, dry run, row-level evidence, rollback/restore, and owner authority;
7. prohibit automatic derivation from dimensions where explicit `Volume` is authoritative.

No data correction is authorized by TEAM-C2.

## 5. Tenant and security constraints

A proposed tenant model must document company/branch/user/device cardinality and every tenant-bearing relationship. Evaluate tenant-consistent composite keys/FKs and RLS/equivalent defenses without assuming either is sufficient alone.

Required evidence:

- A→B and B→A company access tests;
- branch A→B and B→A tests;
- direct-ID/IDOR, background worker, audit, export/report, sync, and raw-SQL paths;
- runtime versus migration role/grant separation;
- session/device revocation and cache invalidation behavior;
- rollback/restore behavior with policies enabled.

Live roles/RLS and external IdP guarantees remain unknown.

## 6. Accounting constraints

- `POSTED` must imply a linked balanced immutable journal created in the same transaction.
- Database and service controls must cover tenant, branch, fiscal period, currency/rounding, actor, permission/SoD, idempotency, and source-document uniqueness.
- Reversal appends linked inverse entries; no update/delete of posted history.
- Finance append-only enforcement must include the approved database/raw-SQL boundary, not EF interception alone.
- Collection/settlement bridges must prevent duplicate posting and support reconciliation.
- Account mapping and subledger semantics require canonical accounting decisions; they are not invented by architecture.

## 7. Audit, privacy, and offline constraints

- Canonical audit hashing is versioned; old events remain verifiable with their original algorithm.
- Business state, audit event, and outbox write are atomic where one use case requires it.
- Sensitive JSON/text fields have classification, size, redaction, encryption/key, retention/legal-hold, export, and deletion/anonymization decisions.
- Offline inbox/outbox keys are tenant/device/operation aware; payloads are typed/versioned/allowlisted.
- Financial, approval, and administration writes remain non-queueable under the current `OFFLINE_WRITE=0 / Can Queue=NO` authority.

## 8. Required disposable verification matrix

| Test | Minimum evidence |
|---|---|
| Fresh database | migrate from zero at exact SHA; model drift/pending changes absent |
| Representative upgrade | restore a versioned pre-change snapshot, migrate forward, compare invariants |
| Failure/rollback | inject failure at each critical step; prove atomicity or successful restore |
| Backup/restore | backup, restore to new disposable instance, verify counts/hashes/invariants |
| Tenant negative | bidirectional company/branch/user/device isolation at API/service/DB/raw SQL |
| Accounting | balanced posting, duplicate/idempotent retry, period/permission denial, reversal |
| Offline | duplicate/reorder/replay/restart/conflict/revocation/schema-version tests |
| P0 Volume | explicit Volume survives create/update/reload/shipping allocation; affected-row assessment |
| Audit | legacy + new hash verification, append-only enforcement, transaction failure |

All logs/artifacts must be exact-SHA-bound and contain no Production secrets or unnecessary personal data.

## 9. Stop conditions

Stop the affected implementation and require authority if it would mutate Production, risk data loss, rewrite migration/Git history, delete preserved assets, require destructive DDL, or cannot demonstrate a recovery path. Analytical/design work may continue with the item recorded as unknown.
