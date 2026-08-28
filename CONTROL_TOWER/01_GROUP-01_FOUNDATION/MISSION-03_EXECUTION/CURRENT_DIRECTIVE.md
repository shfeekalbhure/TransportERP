# CURRENT DIRECTIVE — MISSION-03

`CONTINUE — GREENFIELD DB-GOV RE-REVIEW COMPLETE; REVISE PROPOSALS; KEEP MISSION-03 OPEN`

## Current execution basis

- MISSION-03: `IN PROGRESS — OPEN — NOT SEALED`.
- Product authority: `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`.
- Execution branch/head: `codex/mission-03-execution-20260828@5d1352b4fb6d56261dff8b8a622bacb2786f56d9`.
- Execution tree: `00512125311306a43474638195d2cad97b76118e`.
- PR #69 remains `UNMERGED EVIDENCE ONLY`.
- Exact internal baseline evidence remains: run `33201720896 = 153/153 PASS`; PostgreSQL 18.6; ten existing migrations; no model drift; API HTTP 401; client build probes only. Recovery run `33201720878 = PASS` on disposable PostgreSQL.

## Binding owner decisions

- `AUTH-001 = RESOLVED — LOCAL APPLICATION AUTHORITY`.
- `ACC-001 = RESOLVED — OPERATIONAL COLLECTION; GOVERNED SETTLEMENT POSTS THE LEDGER`.
- `OFFLINE-001 = RESOLVED — DEFAULT DENY; EXPLICIT QUEUE FOR BOUNDED OPERATIONAL CAPTURE`.
- `CLIENT-001 = RESOLVED — DESKTOP + THREE ANDROID CLIENTS ARE RELEASE TARGETS; IOS DEFERRED`.
- `DB-BASELINE-001 = RESOLVED — TARGET DATABASE IS GREENFIELD / NEW / EMPTY / NO LEGACY TABLES OR DATA`.

Greenfield decision file:

`CONTROL_TOWER/00_GOVERNANCE/DECISIONS/DB-BASELINE-001_GREENFIELD_TARGET_DATABASE_2026-08-28.md`

## Greenfield DB-GOV re-review — COMPLETE

Control Tower completed the required second independent DB-GOV review and recorded it in:

`CONTROL_TOWER/03_DATABASE_GOVERNANCE/DB_GOV_GREENFIELD_REREVIEW_DECISION_2026-08-28.md`

Controlling result:

`GREENFIELD LEGACY-DATA BLOCKERS CLEARED — PROPOSAL-SPECIFIC DESIGN GATES REMAIN — NO DB/MIGRATION REHEARSAL AUTHORITY YET`

Cleared as target-database prerequisites:

- legacy target-row/cardinality/backfill inventory;
- legacy PasswordHash/verifier/rehash compatibility;
- legacy audit/accounting row reconciliation;
- safe-copy/backup of a pre-existing target database;
- preservation of a pre-existing target database population.

The existing ten committed migrations remain the bootstrap lineage. A future Greenfield rehearsal database must start empty and apply only those ten migrations before any separately authorized candidate migration.

## Current DBP dispositions

- `DBP-002 = REVISE BEFORE REHEARSAL` — exact physical membership/grant schema, tenant-consistent keys/FKs/checks/indexes, RLS/equivalent bootstrap and rollback/test specification remain required.
- `DBP-003A = REVISE BEFORE REHEARSAL` — final DBP-004-compatible caller-owned audit/UoW boundary, exact candidate persistence mapping and new-system password hash/verify/lockout policy remain required.
- `DBP-003B = DEFERRED — DEPENDS ON DBP-002/006`.
- `DBP-003C = DEFERRED — DEPENDS ON DBP-002/006`; nonce/JTI uniqueness, proof-key persistence, retention/legal-hold/cleanup and recovery remain required.
- `DBP-004 = REVISE BEFORE REHEARSAL` — exact V2 audit schema/canonicalizer/stream sequencing, append-only DB enforcement, transaction enlistment and failure-injection acceptance remain required.
- `DBP-005 = REVISE BEFORE REHEARSAL` — exact Settlement/journal/source-link constraints, account-role/FX/rounding configuration contract, period/SoD and concurrency/reversal acceptance remain required.
- `DBP-006 = REVISE BEFORE REHEARSAL` — exact typed queue/inbox/outbox/result/claim-lease schema, protocol/version/fingerprint constraints, retention/legal-hold and device/proof dependencies remain required.

No DBP currently has:

`APPROVED FOR DISPOSABLE/GREENFIELD NON-PRODUCTION REHEARSAL ONLY`.

## Authorized next work

MISSION-03 must continue automatically with non-destructive repository/governance work enabled by the re-review:

1. Produce exact candidate physical specifications for DBP-002/003A/004/005/006 and explicitly order cross-proposal dependencies.
2. Define and test-specify the new-system password hash/verify/lockout policy; do not request legacy compatibility evidence.
3. Finalize the shared caller-owned transaction/audit boundary across session, audit, Settlement and Offline persistence.
4. Define Greenfield PostgreSQL roles/RLS-equivalent bootstrap plus negative-test requirements.
5. Define retention/legal-hold/cleanup/recovery for device proof, nonce/replay, Offline queue and audit records.
6. Re-submit proposal packages for independent DB-GOV decision. Do not author or execute candidate migrations until the exact proposal receives bounded rehearsal authority.
7. Continue unrelated non-destructive W5/W6/W7 preparation where existing gates permit.
8. Keep W8 last; no destructive/global cleanup before its preservation gate is satisfied.

A blocked DBP must not stop unrelated satisfied packages.

## Remaining non-DB / external gates

The Greenfield decision does not remove:

- canonical programming authority for post-DEPART Shipping, Ticketing and governed screen routes;
- real Windows/Android executable runtime and secure-store proof;
- protected Production signing custody;
- Production recovery/RPO/RTO, privacy/retention, KMS/key custody and dependency/license/provenance approvals;
- complete Git worktree/stash/local-only preservation inventory before any W8 destructive/global cleanup.

## Mission transition boundary

MISSION-03 is not sealed. No final exact-head acceptance package/seal/handoff exists. MISSION-04 remains:

`WAIT — NOT STARTED`

Do not change MISSION-04 to START until MISSION-03 is conclusively sealed and handed off with exact SHAs, evidence, manifest, detached SHA-256, seal, preservation/rollback and DB-GOV compliance verified.

## Prohibitions

No merge to master, rebase, cherry-pick, force-push, history rewrite, Production mutation, signing-secret commit, Entity/DbContext/Migration/Schema/Seed/Data change or unauthorized database action.

No `OWNER DECISION REQUIRED` is active from this re-review. The immediate next work is non-destructive design/governance work within delegated Control Tower/MISSION-03 authority.
