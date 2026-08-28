# DB-GOV POST-CORRECTION PASS DECISION — 2026-08-29

- Review authority: `INDEPENDENT DB-GOV REVIEW / CONTROL TOWER RECORD`
- Formal recording authorized by owner: `APPROVED — "اعتمد نفذ"`
- Governing rule: `DB-GOV-001`
- Database baseline: `DB-BASELINE-001 = GREENFIELD — NEW — EMPTY — NO LEGACY TABLES / NO LEGACY DATA`
- Correction reviewed: `20608494998e671892ee35abd415158e399c9036`
- Correction title: `DBP-003B/C ↔ DBP-003A ↔ DBP-006 — Physical Dependency Correction v1.1`
- Execution baseline: `codex/mission-03-execution-20260828@5d1352b4fb6d56261dff8b8a622bacb2786f56d9`
- Execution tree: `00512125311306a43474638195d2cad97b76118e`
- Production authorization: `NONE`

## Independent verdict formally recorded

`DB-GOV VERDICT = PASS`

`DEPENDENCY CORRECTION ACCEPTED — NO REMAINING PHYSICAL ORDER BLOCKER IDENTIFIED`

The fresh independent review issued after correction `2060849...` accepted the corrected physical dependency order and found no remaining dependency/physical-design blocker requiring FAIL or another dependency redesign.

This record does not replace the independent review with an owner decision. The owner instruction authorizes formal recording and continuation; the technical PASS originates from the independent DB-GOV review.

## Controlling physical order

The only approved coordinated candidate-unit order is:

`DBP-002 → DBP-004 → DBP-003B/C → DBP-003A → DBP-006 → DBP-005`

Reserved candidate units remain:

| Order | Candidate unit | Proposal |
|---:|---|---|
| 11 | `GreenfieldTenantMembershipIsolation` | `DBP-002` |
| 12 | `GreenfieldAuditV2AndAtomicOutbox` | `DBP-004` |
| 13 | `GreenfieldDeviceRegistryAndProof` | `DBP-003B/C` |
| 14 | `GreenfieldLocalAuthSessions` | `DBP-003A` |
| 15 | `GreenfieldTypedOfflineProtocol` | `DBP-006` |
| 16 | `GreenfieldGovernedSettlement` | `DBP-005` |

The physical dependency graph is acyclic under this sequence:

- `DBP-002` provides tenant/membership physical authority.
- `DBP-004` builds on DBP-002 and provides the shared atomic audit/outbox boundary.
- `DBP-003B/C` may create durable device/proof objects after DBP-002/004.
- `DBP-003A` follows DBP-003B/C so its `(RegisteredDeviceId, CompanyId)` FK can bind to an existing `registered_devices` principal key.
- `DBP-006` follows membership, audit, device/proof and session persistence because its provenance contract depends on them.
- `DBP-005` depends materially on DBP-002/004; placing it last is permitted and introduces no cycle.

## Physical creation versus runtime activation

Physical schema creation does not prematurely enable dependent behavior.

- Device lifecycle behavior requiring session-family revocation stays disabled until DBP-003A passes.
- Device lifecycle behavior requiring Offline quarantine stays disabled until DBP-006 passes.
- `LOGIN ACTIVATION` stays separately blocked until the new-system password hash/verify/lockout tests pass.
- OFFLINE execution eligibility remains governed by OFFLINE-001 default-deny policy.
- Settlement remains governed by ACC-001 and cannot bypass accounting/SoD/period/FX constraints.

## Authority opened by this PASS

The following is now authorized **only on the isolated MISSION-03 execution branch and disposable/Greenfield non-Production PostgreSQL 18.6 rehearsal environments**:

`CANDIDATE AUTHORING + DISPOSABLE/GREENFIELD NON-PRODUCTION REHEARSAL APPLICATION`

This bounded authority includes proposal-scoped:

- Entity authoring;
- DbContext mappings;
- additive forward-only candidate migrations;
- PostgreSQL keys/FKs/checks/indexes/RLS-or-equivalent rehearsal controls;
- persistent adapters required by the exact candidate package;
- generated SQL and model snapshot changes;
- synthetic rehearsal fixtures outside Production seed;
- proposal-specific concurrency, failure-injection, tenant, replay, accounting and audit tests.

## Mandatory pre-apply evidence

Before applying any candidate migration to PostgreSQL, bind and retain:

1. exact candidate Product SHA/tree/parent;
2. changed-file inventory and migration identity/hash;
3. EF model snapshot diff and generated SQL;
4. `dotnet ef migrations has-pending-model-changes` evidence;
5. empty PostgreSQL 18.6 baseline with the existing ten migrations applied unchanged;
6. baseline catalog/FK/index/RLS-equivalent inventory;
7. baseline backup digest and successful restore to a fresh disposable database;
8. proposal-specific test and rollback/forward-recovery plan.

After candidate application, capture the same catalog/migration/model evidence, full regression, proposal-specific negatives, backup/restore and reconciliation.

## Dependency failure gates

- Failure of candidate unit 13 (`DBP-003B/C`) stops units 14 and 15.
- Failure of unit 14 (`DBP-003A`) stops unit 15.
- DBP-005 remains physically independent after successful DBP-002/004, but in the coordinated run it remains ordered last.
- A failed package stops itself and its dependents; it does not authorize reordering around the failure.

## Prohibited scope

This PASS does **not** authorize:

- Production database/data/configuration/credentials;
- real customer/user/accounting data;
- Production signing keys, peppers, private device keys or secrets;
- destructive migration/down-migration reliance;
- editing/deleting/squashing the existing ten migrations;
- master merge;
- rebase;
- cherry-pick;
- force-push;
- history rewrite.

## Post-rehearsal gate

Successful rehearsal is not final DB acceptance. Each candidate package and the coordinated bundle require independent DB-GOV post-rehearsal review of exact-head evidence before Production/bootstrap release authority can be considered.

## Mission disposition

`MISSION-03 = IN PROGRESS — OPEN — NOT SEALED`

`MISSION-04 = WAIT — NOT STARTED`

The previous gate:

`PASS ISSUED — AUTHORING/REHEARSAL HOLD UNTIL THIS DECISION IS FORMALLY RECORDED`

is closed by this record.

Current gate:

`PASS RECORDED — BOUNDED GREENFIELD AUTHORING/REHEARSAL AUTHORIZED`
