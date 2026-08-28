# DB-BASELINE-001 — Greenfield Target Database Authority

Decision date: 2026-08-28
Owner approval: `EXPLICITLY APPROVED`
Decision status: `RESOLVED — BINDING`

## Decision

`TARGET DATABASE = GREENFIELD — NEW — EMPTY — NO LEGACY TABLES / NO LEGACY DATA`

The owner confirms that the TransportERP target database is a new database. There is no existing legacy/Production database whose tables, users, password hashes, accounting rows, audit rows, business rows or schema must be migrated or preserved as the source population for this mission.

## Governing consequences

1. The repository's current migration lineage remains the schema bootstrap authority. The existing ten committed migrations are not removed, renamed or bypassed; a new PostgreSQL target starts empty and is built by applying the governed migration lineage.
2. `LEGACY DATABASE PRESERVATION = NOT APPLICABLE` for MISSION-03 target-database migration planning.
3. `LEGACY PASSWORD HASH INVENTORY / LEGACY VERIFIER / LEGACY REHASH COMPATIBILITY = NOT APPLICABLE` because no legacy users/password rows exist in the target database.
4. A new password-hashing/lockout policy is still required before login activation, but it is a new-system security design/test requirement, not a legacy-data discovery gate.
5. Legacy audit/accounting row reconciliation is not a prerequisite for the new target database. New-system accounting/audit invariants, configured account roles, FX/rounding behavior, append-only audit and reconciliation tests remain required.
6. A copy of an existing live/legacy database is not required for DB-GOV rehearsal because no such target population exists. DB-GOV may use a named isolated non-Production Greenfield rehearsal database created empty, apply the current ten-migration lineage, then rehearse any separately authorized candidate migrations.
7. Backup/restore and recovery proof remain required for operational readiness, but as Greenfield deployment/recovery evidence rather than legacy-preservation evidence.
8. PostgreSQL roles, extensions, RLS/equivalent controls, indexes, constraints and new DBP-002/003/004/005/006 physical changes must still be explicitly designed, reviewed and rehearsed. Absence of legacy data does not waive DB-GOV-001.
9. No Entity, DbContext, Migration, Schema, Seed, Data or Production mutation is authorized by this owner decision alone.
10. W8 preservation of Git worktrees/stashes/local-only artifacts is separate from database preservation and remains governed by its own inventory gate.

## DB-GOV routing

Control Tower must re-review `DBP-002/003/004/005/006` against this Greenfield fact pattern and remove blockers that depended only on unknown legacy/live row shape, legacy password formats, legacy audit/accounting populations or the existence of a prior target database.

The next permitted decision may be bounded, for example:

`APPROVED FOR DISPOSABLE/GREENFIELD NON-PRODUCTION REHEARSAL ONLY`

only after the proposal-specific design, rollback/recovery and test gates are independently satisfied. Production application remains a separate authorization.

## Historical interpretation

Any earlier MISSION-03 or DB-GOV statement requiring a legacy PasswordHash inventory, legacy target-data reconciliation or a safe-copy of an existing target database is superseded **only to the extent that it assumed a pre-existing target database/data population**. Other security, DB-GOV, runtime, canonical business-authority, signing, privacy, recovery and preservation gates remain in force.
