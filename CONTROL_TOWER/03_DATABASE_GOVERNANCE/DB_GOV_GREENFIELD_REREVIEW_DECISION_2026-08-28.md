# DB-GOV GREENFIELD RE-REVIEW DECISION — 2026-08-28

- Review authority: `CONTROL TOWER / DB-GOV-001`
- Authoritative product: `refs/heads/master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`
- MISSION-03 execution head reviewed: `codex/mission-03-execution-20260828@5d1352b4fb6d56261dff8b8a622bacb2786f56d9`
- Execution tree: `00512125311306a43474638195d2cad97b76118e`
- Owner database authority: `DB-BASELINE-001 = GREENFIELD / NEW / EMPTY / NO LEGACY TABLES OR DATA`
- Product/Tests/Migrations/Production/Database mutation by this review: `NONE`

## Precedence / reconciliation

This decision is later and controlling. It **supersedes** the earlier mission-local file:

`CONTROL_TOWER/01_GROUP-01_FOUNDATION/MISSION-03_EXECUTION/DBP-002_003_004_005_006_GREENFIELD_DB_GOV_REVIEW_DECISION.md`

only where that earlier file granted `APPROVED FOR ... REHEARSAL`. The earlier file remains preserved as historical review evidence, but it grants **no current execution authority**. Current authority is exactly the result below and the synchronized `CURRENT_DIRECTIVE.md` / central proposal register.

## Controlling result

`GREENFIELD LEGACY-DATA BLOCKERS CLEARED — PROPOSAL-SPECIFIC DESIGN GATES REMAIN — NO DB/MIGRATION REHEARSAL AUTHORITY YET`

This is the required second independent DB-GOV review after `DB-BASELINE-001`. It supersedes only blockers that depended on a pre-existing target database, legacy target rows, legacy password hashes, legacy accounting/audit populations, or a safe-copy of such a database. `DB-GOV-001` remains fully binding.

## Evidence re-read

The review re-read the Greenfield owner decision, current MISSION-03 directive/completion assessment, the existing ten-migration lineage evidence, `TENANT_CARDINALITY_ADR.md`, `DEVICE_LIFECYCLE_POP_ADR.md`, `DBP-003A_REHEARSAL_RESUBMISSION.md`, `DBP-002_004_005_006_REVIEW_PREPARATION.md`, `W3_UOW_ACCOUNTING_AUDIT_PREPARATION.md`, `W4_OFFLINE_SYNC_PREPARATION.md`, the prior DBP-003 decision, and the current DB-GOV registers.

Internal runtime evidence remains bounded to the existing lineage: run `33201720896 = 153/153 PASS`, PostgreSQL 18.6, ten existing migrations, no model drift; run `33201720878 = PASS` for disposable backup/restore and `10/10` migration-history reconciliation. No candidate DBP migration exists at the reviewed execution head.

## Greenfield corrections

The following prior gates are no longer valid prerequisites for the target database:

- legacy target row/cardinality/backfill inventory;
- legacy PasswordHash format/verifier/rehash compatibility;
- legacy audit/accounting row reconciliation;
- safe-copy/backup of a pre-existing target database;
- preservation of a pre-existing target database population.

The Greenfield rehearsal model is now: create a named isolated non-Production PostgreSQL database empty, apply the ten governed migrations, then apply only separately authorized candidate migrations and run proposal-specific tests/recovery.

## Proposal decisions

| Proposal | Greenfield DB-GOV decision | Exact remaining gate |
|---|---|---|
| `DBP-002` | `REVISE BEFORE REHEARSAL` | legacy/backfill uncertainty cleared, but the proposal still lacks an exact candidate physical membership/grant schema, tenant-consistent key/FK/check/index set, reviewed RLS/equivalent role model, and complete bootstrap/rollback test specification |
| `DBP-003A` | `REVISE BEFORE REHEARSAL` | legacy hash/safe-copy blockers cleared; the resubmission materially improves keys, session lineage, locking/retry and failure injection, but durable activation still requires a final DBP-004-compatible caller-owned audit/UoW boundary, exact candidate persistence mapping, and a documented new-system password-hash/verify/lockout policy before login activation |
| `DBP-003B` | `DEFERRED — DEPENDS ON DBP-002/006` | exact membership-bound registry/assignment persistence and lifecycle constraints are not yet jointly specified |
| `DBP-003C` | `DEFERRED — DEPENDS ON DBP-002/006` | nonce/JTI uniqueness scope, proof-key persistence, retention/legal-hold/cleanup and recovery policy remain unresolved |
| `DBP-004` | `REVISE BEFORE REHEARSAL` | legacy V1 population/sample requirement cleared for the empty target, but exact V2 schema/canonicalizer/stream sequence, append-only DB enforcement, caller-owned transaction enlistment and raw-SQL/failure-injection acceptance remain to be finalized |
| `DBP-005` | `REVISE BEFORE REHEARSAL` | legacy reconciliation cleared and ACC-001 fixes the accounting boundary, but exact Settlement/journal/source-link constraints, account-role/FX/rounding configuration contract, period/SoD enforcement and concurrency/reversal acceptance must be finalized |
| `DBP-006` | `REVISE BEFORE REHEARSAL` | live-baseline/safe-copy requirement cleared and OFFLINE-001 fixes authority, but exact typed queue/inbox/outbox/result/claim-lease schema, protocol/version/fingerprint constraints, retention/legal-hold and DBP-002/003B/C device/proof dependencies remain incomplete |

## Authority boundary

No proposal in this re-review is yet granted:

`APPROVED FOR DISPOSABLE/GREENFIELD NON-PRODUCTION REHEARSAL ONLY`

because each still has a proposal-specific physical-design or dependent-policy gap listed above. Therefore this review authorizes only non-destructive design/specification refinement, evidence preparation and re-submission. It does **not** authorize Entity, DbContext, Migration, Schema, Seed, persistent-adapter, data, Production credential or Production database changes.

## Required next MISSION-03 work

1. Produce exact candidate physical specifications for DBP-002/003A/004/005/006, resolving cross-proposal dependencies explicitly rather than bundling them implicitly.
2. Define the new-system password hash/verify/lockout policy; no legacy compatibility requirement exists.
3. Finalize the shared caller-owned transaction/audit boundary for DBP-003A/004/005/006.
4. Define Greenfield PostgreSQL roles/RLS-equivalent bootstrap and negative-test policy.
5. Define retention/legal-hold/cleanup for device proof, nonce/replay, Offline queue and audit records.
6. Re-submit each proposal independently or as an explicitly dependency-ordered bundle for the next DB-GOV decision.

A proposal may then receive a bounded disposable/Greenfield non-Production rehearsal authority if its exact physical design, test, isolation, rollback/recovery and dependency gates are satisfied.

## Mission disposition

- `MISSION-03 = IN PROGRESS — OPEN — NOT SEALED`.
- `MISSION-04 = WAIT — NOT STARTED`.
- No `OWNER DECISION REQUIRED` is created by this review: the next permitted work is non-destructive design/governance work and does not touch Production or preserved work/data.
