# CURRENT DIRECTIVE — MISSION-03

`CONTINUE — GREENFIELD TARGET DATABASE CONFIRMED; RE-ROUTE DB-GOV; KEEP MISSION-03 OPEN`

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

Decision file:

`CONTROL_TOWER/00_GOVERNANCE/DECISIONS/DB-BASELINE-001_GREENFIELD_TARGET_DATABASE_2026-08-28.md`

## Greenfield consequences

The target database does not contain legacy users, password hashes, accounting rows, audit rows, business rows or schema requiring migration/preservation.

Therefore:

- `LEGACY DATABASE PRESERVATION = NOT APPLICABLE`.
- `LEGACY PASSWORD HASH INVENTORY / LEGACY VERIFIER / LEGACY REHASH COMPATIBILITY = NOT APPLICABLE`.
- `LEGACY ACCOUNTING/AUDIT ROW RECONCILIATION = NOT APPLICABLE` as a target-database prerequisite.
- A copy of an existing target database is not required for rehearsal because no prior target population exists.

The existing ten committed migrations remain the initial schema lineage. A new non-Production rehearsal database must start empty, apply those ten migrations, then apply only candidate migrations separately authorized by DB-GOV.

A new password-hashing and lockout policy is still required before login activation, but it is a **new-system security design/test requirement**, not a legacy evidence gate.

## DB-GOV direction

Owner decisions do not override `DB-GOV-001`.

Control Tower / DB-GOV must now independently re-review:

- `DBP-002` — tenant-consistent physical keys/FKs/indexes/RLS-equivalent;
- `DBP-003A/B/C` — session/security version/device/PoP/nonce/replay persistence;
- `DBP-004` — audit integrity;
- `DBP-005` — accounting integrity / governed Settlement persistence;
- `DBP-006` — Offline queue/inbox/outbox protocol persistence.

The review must remove blockers that depended only on unknown legacy/live target rows, legacy password formats, legacy accounting/audit populations or a prior target database.

Where proposal-specific design/tests are complete, DB-GOV may issue only a bounded next-stage authority such as:

`APPROVED FOR DISPOSABLE/GREENFIELD NON-PRODUCTION REHEARSAL ONLY`

before any separate Production authorization.

No Entity, DbContext, Migration, Schema, Seed, Data or Production mutation is authorized by DB-BASELINE-001 alone.

## Remaining external/non-Greenfield gates

The following are **not** removed by the Greenfield decision:

- new-system password hash/lockout security policy and tests;
- PostgreSQL role/RLS/equivalent design and DB-GOV proof;
- canonical programming authority for post-DEPART Shipping, Ticketing and screen routes;
- executable Windows/Android runtime, secure-store integration and protected signing custody;
- Production recovery/RPO/RTO, privacy/retention, KMS/key custody and dependency/license/provenance approvals;
- complete Git worktree/stash/local-only preservation inventory before W8 cleanup.

## Execution routing

1. Do not ask the owner again for legacy database/password evidence; it is not applicable.
2. Route DBP-002/003/004/005/006 immediately to second independent DB-GOV review using the Greenfield basis.
3. Continue any non-destructive work enabled by that review; do not cross a DB-GOV gate.
4. Preserve execution head `5d1352b...` until a newly authorized Product package is opened.
5. W8 remains last and may not start before W7/preservation gates are satisfied.
6. Do not start MISSION-04 before `MISSION-03 = SEALED — DELIVERED TO CONTROL TOWER`.

## Prohibitions

No merge to master, rebase, cherry-pick, force-push, history rewrite, Production mutation, secret commit or unauthorized database/data change.

This directive supersedes the v1.0 exhaustion directive only where that directive assumed a pre-existing legacy target database/data population. Historical evidence remains preserved in Git.
