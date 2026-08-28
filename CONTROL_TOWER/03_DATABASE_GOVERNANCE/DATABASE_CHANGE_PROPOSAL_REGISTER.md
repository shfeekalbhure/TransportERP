# DATABASE CHANGE PROPOSAL REGISTER

`DB-GOV-001` is binding. Review here does **not** itself authorize database/schema/entity/migration execution. Execution remains blocked until the exact proposal gate is independently opened with impact, isolation, test and rollback/recovery evidence.

Owner decision `DB-BASELINE-001` is binding:

`TARGET DATABASE = GREENFIELD — NEW — EMPTY — NO LEGACY TABLES / NO LEGACY DATA`

Accordingly, legacy target-row inventory/backfill, legacy PasswordHash compatibility, legacy accounting/audit row reconciliation and a safe-copy of a pre-existing target database are not target-database prerequisites. The existing ten committed migrations remain the bootstrap lineage for an empty PostgreSQL target.

## Current proposal register

| Proposal ID | Requirement | Proposed Change | Review Status | Execution Status |
|---|---|---|---|---|
| `DBP-001` | `A-ARCH-002 / REM-100` | mapper correction plus any later conditional data repair | `REVIEWED — CODE-ONLY FIX IMPLEMENTED` | `GREENFIELD TARGET HAS NO LEGACY POPULATION TO REPAIR; NO NEW DB/DATA ACTION AUTHORIZED` |
| `DBP-002` | tenant isolation | explicit membership plus tenant-consistent keys/FKs/indexes/checks and reviewed RLS/equivalent | `v1.0 EXACT PHYSICAL DESIGN RESUBMITTED` | `AWAITING INDEPENDENT DB-GOV DECISION — NO REHEARSAL AUTHORITY` |
| `DBP-003A` | auth/session | security-version/lockout state and durable session-family persistence | `v1.0 EXACT PHYSICAL DESIGN RESUBMITTED` | `AWAITING INDEPENDENT DB-GOV DECISION — NO REHEARSAL AUTHORITY` |
| `DBP-003B` | device registry/assignment | membership-bound device registry and assignment persistence | `v1.0 DEPENDENCY-BOUND PHYSICAL DESIGN RESUBMITTED` | `AWAITING INDEPENDENT DB-GOV DECISION — NO REHEARSAL AUTHORITY` |
| `DBP-003C` | PoP/nonce/replay | proof-key, nonce/JTI and replay persistence | `v1.0 DEPENDENCY-BOUND PHYSICAL DESIGN RESUBMITTED` | `AWAITING INDEPENDENT DB-GOV DECISION — NO REHEARSAL AUTHORITY` |
| `DBP-004` | audit integrity | V2 hash marker/canonicalizer, stream sequence and atomic append boundary | `v1.0 EXACT PHYSICAL DESIGN RESUBMITTED` | `AWAITING INDEPENDENT DB-GOV DECISION — NO REHEARSAL AUTHORITY` |
| `DBP-005` | accounting integrity | durable Settlement/journal/source links, uniqueness, reversal, period/currency/SoD constraints | `v1.0 EXACT PHYSICAL DESIGN RESUBMITTED` | `AWAITING INDEPENDENT DB-GOV DECISION — NO REHEARSAL AUTHORITY` |
| `DBP-006` | Offline protocol | typed queue/inbox/outbox/result, version/fingerprint, claim/lease, device/proof/retention fields | `v1.0 EXACT PHYSICAL DESIGN RESUBMITTED` | `AWAITING INDEPENDENT DB-GOV DECISION — NO REHEARSAL AUTHORITY` |
| `DBP-007` | shipping lifecycle | later custody/delivery/settlement/return/claim/customs persistence | `REVIEWED — DEFERRED INTAKE` | `BLOCKED — CANONICAL SCOPE REQUIRED` |
| `DBP-008` | ticketing | Ticketing tables/entities/indexes/numbering after contract authority | `REVIEWED — DEFERRED INTAKE` | `BLOCKED — CANONICAL REQUIREMENTS REQUIRED` |
| `DBP-009` | reporting | read projections/materialized views if approved | `REVIEWED — DEFERRED INTAKE` | `BLOCKED — REPORTING REQUIREMENTS REQUIRED` |

## Greenfield rehearsal model

A future proposal-specific rehearsal must use a named isolated non-Production PostgreSQL database that:

1. starts empty;
2. applies the existing ten governed migrations;
3. applies only the exact candidate migrations separately authorized by DB-GOV;
4. runs proposal-specific negative/concurrency/failure-injection tests;
5. records schema/model-drift, constraint/index/RLS-equivalent and application-regression evidence;
6. proves backup/restore or forward-recovery appropriate to the Greenfield candidate state;
7. is discarded or preserved as evidence according to the rehearsal decision.

No Production data or secrets are authorized.

## Prior evidence retained

- W2 code-only controls through `9c5b7a12e59d2c42e682717b8e90c491f8699b96` were independently adopted without a persistence delta.
- W2-B2B code-only through `cc67ad2bd491ed3ab23c3144f11dff955353c3a4` was independently revalidated; run `33191269475` passed `146/146` against the existing ten-migration disposable PostgreSQL lineage with no model drift.
- MISSION-03 later advanced to `5d1352b4fb6d56261dff8b8a622bacb2786f56d9`; run `33201720896` passed `153/153`, ten existing migrations/no drift, and run `33201720878` passed disposable backup/restore with `10/10` migration-history reconciliation.
- No Entity, DbContext model, Migration, Schema, Seed, persistent adapter, Product data or Production configuration delta exists in that bounded evidence.

Historical DBP-003 decisions that required legacy PasswordHash inventory or a safe-copy of an existing target database are superseded **only** for those legacy assumptions by `DB-BASELINE-001`. Their physical-design, transaction, tenant-consistency, device/proof, security and DB-GOV concerns remain preserved.

## Current DB-GOV decision

The controlling second review is:

`CONTROL_TOWER/03_DATABASE_GOVERNANCE/DB_GOV_GREENFIELD_REREVIEW_DECISION_2026-08-28.md`

Current result:

`GREENFIELD LEGACY-DATA BLOCKERS CLEARED — PROPOSAL-SPECIFIC DESIGN GATES REMAIN — NO DB/MIGRATION REHEARSAL AUTHORITY YET`

No proposal currently has `APPROVED FOR DISPOSABLE/GREENFIELD NON-PRODUCTION REHEARSAL ONLY` status.

## v1.0 exact physical-design resubmission

MISSION-03 has supplied the exact proposal-specific design requested by the
controlling re-review in:

- `DBP-002_003_004_005_006_EXACT_PHYSICAL_DESIGN_RESUBMISSION.md`;
- `GREENFIELD_DB_REHEARSAL_ACCEPTANCE_SPEC.md`.

The bundle fixes migration order, membership/grant/RLS physical shapes,
new-system password and lockout policy, session/family atomic rotation,
device/assignment/PoP/replay persistence, caller-owned Audit V2/Outbox UoW,
Settlement constraints and typed Offline inbox/queue/result/lease, plus
retention/legal-hold/cleanup/recovery. It requests but does not grant bounded
Greenfield rehearsal authority. Material execution remains `NONE` until an
independent DB-GOV decision is recorded.

Every deletion remains `CANDIDATE FOR REMOVAL` until separately proved and authorized.
