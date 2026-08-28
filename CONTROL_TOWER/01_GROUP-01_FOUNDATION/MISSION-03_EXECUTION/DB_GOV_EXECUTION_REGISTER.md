# DB-GOV Execution Register

`DB-GOV-001` is binding. Production database/schema/entity/migration/data authority remains separate from bounded Greenfield rehearsal authority.

The central proposal register contains DBP-001..009. AUTH-001, ACC-001, OFFLINE-001, CLIENT-001 and DB-BASELINE-001 remain binding owner decisions.

Reviewed MISSION-03 execution baseline:

`codex/mission-03-execution-20260828@5d1352b4fb6d56261dff8b8a622bacb2786f56d9`

Tree:

`00512125311306a43474638195d2cad97b76118e`

## Current DB-GOV state

Physical dependency correction:

`20608494998e671892ee35abd415158e399c9036`

Fresh independent post-correction verdict:

`DB-GOV VERDICT = PASS`

`DEPENDENCY CORRECTION ACCEPTED — NO REMAINING PHYSICAL ORDER BLOCKER IDENTIFIED`

Formal record:

`CONTROL_TOWER/03_DATABASE_GOVERNANCE/DB_GOV_POST_CORRECTION_PASS_DECISION_2026-08-29.md`

The previous state `FRESH INDEPENDENT DB-GOV REQUIRED BEFORE REHEARSAL AUTHORING` is closed.

## Current proposal gates

| Proposal | Relevant REM | Current controlling result |
|---|---|---|
| `DBP-001` | `REM-100` | `CODE-ONLY IMPLEMENTED; GREENFIELD TARGET HAS NO LEGACY POPULATION TO REPAIR` |
| `DBP-002` | `REM-210` | `PASS — CANDIDATE AUTHORING + DISPOSABLE/GREENFIELD NON-PRODUCTION REHEARSAL AUTHORIZED` |
| `DBP-004` | `REM-320` | `PASS — AUTHORIZED AFTER DBP-002` |
| `DBP-003B/C` | `REM-220` | `PASS — AUTHORIZED AFTER DBP-002/004; BEHAVIORAL ACTIVATION STILL DEPENDENCY-GATED` |
| `DBP-003A` | `REM-200` | `PASS — AUTHORIZED AFTER DBP-003B/C; LOGIN ACTIVATION STILL PASSWORD/LOCKOUT TEST-GATED` |
| `DBP-006` | `REM-400` | `PASS — AUTHORIZED AFTER DBP-003A + DBP-003B/C + DBP-002/004` |
| `DBP-005` | `REM-310` | `PASS — AUTHORIZED AFTER DBP-002/004; ORDERED LAST` |
| `DBP-007` | `REM-600` | `BLOCKED — CANONICAL SCOPE REQUIRED` |
| `DBP-008` | `REM-610` | `BLOCKED — CANONICAL TICKETING REQUIREMENTS REQUIRED` |
| `DBP-009` | reporting | `BLOCKED — REPORTING REQUIREMENTS REQUIRED` |

## Only approved coordinated physical order

`DBP-002 → DBP-004 → DBP-003B/C → DBP-003A → DBP-006 → DBP-005`

Reserved candidate units:

1. `GreenfieldTenantMembershipIsolation`.
2. `GreenfieldAuditV2AndAtomicOutbox`.
3. `GreenfieldDeviceRegistryAndProof`.
4. `GreenfieldLocalAuthSessions`.
5. `GreenfieldTypedOfflineProtocol`.
6. `GreenfieldGovernedSettlement`.

No alternate ordering is authorized.

## Bounded authoring authority

MISSION-03 may now author proposal-scoped Entity/DbContext mappings, additive forward-only candidate migrations, persistent adapters, generated SQL/model snapshot deltas, and test/rehearsal controls on its isolated execution branch.

This authority does not allow Product work on master or any Production database/data/configuration/credential.

## Disposable Greenfield rehearsal authority

Candidate application is authorized only to isolated PostgreSQL 18.6 rehearsal databases that start empty.

Required baseline:

1. apply the existing ten migrations unchanged;
2. record migration/catalog/FK/index/RLS-equivalent state;
3. create and hash baseline backup;
4. restore it successfully to a fresh disposable database;
5. apply only the next candidate unit in the approved order.

Every candidate must then pass proposal-specific negatives/concurrency/failure injection, full regression, model-drift check, candidate-state backup/restore and catalog/migration reconciliation.

Exact candidate SHA/tree/parent, changed files, migration identity/hash, generated SQL, model snapshot diff, logs and artifact digests are mandatory evidence.

## Dependency failure gates

- Failure of unit 13 / DBP-003B/C stops DBP-003A and DBP-006.
- Failure of unit 14 / DBP-003A stops DBP-006.
- DBP-005 materially depends on DBP-002/004 but remains ordered last.
- A failed unit does not authorize reordering.

## Runtime activation boundary

- Device actions needing session-family revoke remain disabled until DBP-003A passes.
- Device actions needing Offline quarantine remain disabled until DBP-006 passes.
- `LOGIN ACTIVATION` remains separately blocked until new-system password hash/verify/lockout tests pass.
- OFFLINE-001 default-deny remains binding.
- ACC-001 Settlement/accounting rules remain binding.

## Prior exact-head evidence retained

- W2-B2B code-only: `cc67ad2bd491ed3ab23c3144f11dff955353c3a4`; raw CI `146/146 PASS`, ten existing migrations/no drift.
- Current reviewed execution baseline: `5d1352b4fb6d56261dff8b8a622bacb2786f56d9` / `00512125311306a43474638195d2cad97b76118e`.
- Run `33201720896 = 153/153 PASS`; PostgreSQL 18.6; ten existing migrations; no model drift.
- Run `33201720878 = PASS`; disposable backup/restore and migration reconciliation `10/10`.

These prior runs prove the baseline only; they do not substitute for candidate-head rehearsal evidence.

## Post-rehearsal gate

Every candidate checkpoint and the coordinated bundle require independent post-rehearsal DB-GOV acceptance before any Production/bootstrap release authority can be considered.

## Prohibitions

No Production database/data/configuration/credentials. No secrets. Do not edit/delete/squash the existing ten migrations. No destructive/down-migration recovery reliance. No merge to master, rebase, cherry-pick, force-push or history rewrite.

MISSION-03 remains `IN PROGRESS — OPEN — NOT SEALED`; MISSION-04 remains `WAIT — NOT STARTED`.
