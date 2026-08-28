# DATABASE CHANGE PROPOSAL REGISTER

`DB-GOV-001` is binding. Review here does **not** itself authorize Production database/schema/entity/migration execution. Every execution scope remains bounded by the exact independent decision, isolation, test and recovery evidence recorded below.

Owner decision `DB-BASELINE-001` is binding:

`TARGET DATABASE = GREENFIELD — NEW — EMPTY — NO LEGACY TABLES / NO LEGACY DATA`

Accordingly, legacy target-row inventory/backfill, legacy PasswordHash compatibility, legacy accounting/audit row reconciliation and a safe-copy of a pre-existing target database are not target-database prerequisites. The existing ten committed migrations remain the bootstrap lineage for an empty PostgreSQL target.

## Current proposal register

| Proposal ID | Requirement | Proposed Change | Review Status | Execution Status |
|---|---|---|---|---|
| `DBP-001` | `A-ARCH-002 / REM-100` | mapper correction plus any later conditional data repair | `REVIEWED — CODE-ONLY FIX IMPLEMENTED` | `GREENFIELD TARGET HAS NO LEGACY POPULATION TO REPAIR; NO NEW DB/DATA ACTION AUTHORIZED` |
| `DBP-002` | tenant isolation | explicit membership plus tenant-consistent keys/FKs/indexes/checks and reviewed RLS/equivalent | `v1.0 EXACT PHYSICAL DESIGN RESUBMITTED; POST-RESUBMISSION REVALIDATION OPEN` | `HOLD AT COORDINATED GREENFIELD REHEARSAL ENTRY — ORDER CONFLICT WITH DBP-003B/C ↔ DBP-006 MUST BE RESOLVED` |
| `DBP-003A` | auth/session | security-version/lockout state and durable session-family persistence | `v1.0 EXACT PHYSICAL DESIGN RESUBMITTED; POST-RESUBMISSION REVALIDATION OPEN` | `HOLD AT COORDINATED GREENFIELD REHEARSAL ENTRY; LOGIN ACTIVATION ALSO REQUIRES PASSWORD/VERIFY/LOCKOUT TEST EVIDENCE` |
| `DBP-003B` | device registry/assignment | membership-bound device registry and assignment persistence | `v1.0 DEPENDENCY-BOUND PHYSICAL DESIGN RESUBMITTED` | `HOLD — PHYSICAL DESIGN PLACES BEFORE DBP-006; EARLIER REVIEW PLACES AFTER DBP-006` |
| `DBP-003C` | PoP/nonce/replay | proof-key, nonce/JTI and replay persistence | `v1.0 DEPENDENCY-BOUND PHYSICAL DESIGN RESUBMITTED` | `HOLD — PHYSICAL DESIGN PLACES BEFORE DBP-006; EARLIER REVIEW PLACES AFTER DBP-006` |
| `DBP-004` | audit integrity | V2 hash marker/canonicalizer, stream sequence and atomic append boundary | `v1.0 EXACT PHYSICAL DESIGN RESUBMITTED; POST-RESUBMISSION REVALIDATION OPEN` | `HOLD AS PART OF COORDINATED BUNDLE; DESIGN MAY CONTINUE` |
| `DBP-005` | accounting integrity | durable Settlement/journal/source links, uniqueness, reversal, period/currency/SoD constraints | `v1.0 EXACT PHYSICAL DESIGN RESUBMITTED; POST-RESUBMISSION REVALIDATION OPEN` | `HOLD AS PART OF COORDINATED BUNDLE; DESIGN MAY CONTINUE` |
| `DBP-006` | Offline protocol | typed queue/inbox/outbox/result, version/fingerprint, claim/lease, device/proof/retention fields | `v1.0 EXACT PHYSICAL DESIGN RESUBMITTED` | `HOLD — EXACT DESIGN DEPENDS ON DBP-003B/C WHILE EARLIER REVIEW REQUIRES DBP-006 TO PASS FIRST` |
| `DBP-007` | shipping lifecycle | later custody/delivery/settlement/return/claim/customs persistence | `REVIEWED — DEFERRED INTAKE` | `BLOCKED — CANONICAL SCOPE REQUIRED` |
| `DBP-008` | ticketing | Ticketing tables/entities/indexes/numbering after contract authority | `REVIEWED — DEFERRED INTAKE` | `BLOCKED — CANONICAL REQUIREMENTS REQUIRED` |
| `DBP-009` | reporting | read projections/materialized views if approved | `REVIEWED — DEFERRED INTAKE` | `BLOCKED — REPORTING REQUIREMENTS REQUIRED` |

## Greenfield rehearsal model

Any future proposal-specific rehearsal must use a named isolated non-Production PostgreSQL database that:

1. starts empty;
2. applies the existing ten governed migrations unchanged;
3. applies only candidate migrations covered by a valid post-resubmission DB-GOV decision;
4. runs proposal-specific negative/concurrency/failure-injection tests;
5. records schema/model-drift, constraint/index/RLS-equivalent and application-regression evidence;
6. proves backup/restore or forward-recovery appropriate to the Greenfield candidate state;
7. is discarded or preserved as evidence according to the rehearsal decision.

No Production data, endpoint, role, credential, signing secret or private key is authorized.

## Prior evidence retained

- W2 code-only controls through `9c5b7a12e59d2c42e682717b8e90c491f8699b96` were independently adopted without a persistence delta.
- W2-B2B code-only through `cc67ad2bd491ed3ab23c3144f11dff955353c3a4` was independently revalidated; run `33191269475` passed `146/146` against the existing ten-migration disposable PostgreSQL lineage with no model drift.
- MISSION-03 later advanced to `5d1352b4fb6d56261dff8b8a622bacb2786f56d9`; run `33201720896` passed `153/153`, ten existing migrations/no drift, and run `33201720878` passed disposable backup/restore with `10/10` migration-history reconciliation.
- No Entity, DbContext model, Migration, Schema, Seed, persistent adapter, Product data or Production configuration delta exists in that bounded evidence.

Historical DBP-003 decisions that required legacy PasswordHash inventory or a safe-copy of an existing target database are superseded **only** for those legacy assumptions by `DB-BASELINE-001`. Their physical-design, transaction, tenant-consistency, device/proof, security and DB-GOV concerns remain preserved.

## Review chronology and controlling revalidation

The earlier Greenfield re-review is:

`CONTROL_TOWER/03_DATABASE_GOVERNANCE/DB_GOV_GREENFIELD_REREVIEW_DECISION_2026-08-28.md`

MISSION-03 subsequently supplied:

- `DBP-002_003_004_005_006_EXACT_PHYSICAL_DESIGN_RESUBMISSION.md`;
- `GREENFIELD_DB_REHEARSAL_ACCEPTANCE_SPEC.md`.

A mission-local file named `DBP-002_003_004_005_006_GREENFIELD_DB_GOV_REVIEW_DECISION.md` records nominal coordinated rehearsal approval. Repository chronology shows that decision already existed at `fc2e28f86b297203be9f857f507d40629d9bbb35`, while the exact v1.0 resubmission did not yet exist at that ref and was committed later in `8b97d99e481ed2b6f4a7e90a5d4790ebdcac8219`.

Control Tower therefore performed post-resubmission revalidation and recorded the current controlling disposition in:

`CONTROL_TOWER/03_DATABASE_GOVERNANCE/DB_GOV_POST_RESUBMISSION_REVALIDATION_2026-08-29.md`

Current result:

`HOLD AT COORDINATED GREENFIELD REHEARSAL ENTRY — POST-RESUBMISSION DB-GOV REVALIDATION REQUIRED`

The exact design orders DBP-003B/C before DBP-006 and makes DBP-006 depend on those device/proof objects. The earlier review decision orders DBP-006 before DBP-003B/C and conditions DBP-003B/C on a passed DBP-006 baseline. Until one corrected, post-resubmission dependency decision removes this contradiction, candidate Entity/DbContext/Migration/Schema/Seed/persistent-adapter authoring and candidate migration application remain `HOLD`.

This is not an owner-decision condition. Non-destructive design correction and independent DB-GOV re-review are delegated governance work.

Every deletion remains `CANDIDATE FOR REMOVAL` until separately proved and authorized.
