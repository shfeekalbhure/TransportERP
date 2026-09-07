# DB-GOV Execution Register

`DB-GOV-001` is binding. Production database/schema/entity/migration/data authority remains separate from bounded Greenfield rehearsal authority.

The central proposal register contains DBP-001..009. AUTH-001, ACC-001, OFFLINE-001, CLIENT-001 and DB-BASELINE-001 remain binding owner decisions.

Reviewed pre-authoring MISSION-03 execution baseline:

`codex/mission-03-execution-20260828@5d1352b4fb6d56261dff8b8a622bacb2786f56d9`

Tree:

`00512125311306a43474638195d2cad97b76118e`

Frozen DBP-002 post-rehearsal review target:

`codex/mission-03-execution-20260828@ffdf1087ab4a6435cd1f2b19c5ab9ff58ce206ce`

Tree / parent:

`e828941817432bdc73f3e6fc31e74219e74fcf33` / `f128d24dce7baf76a6ac8af4e62a331b80447311`

## Current DB-GOV state

Physical dependency correction:

`20608494998e671892ee35abd415158e399c9036`

Fresh independent post-correction pre-authoring verdict:

`DB-GOV VERDICT = PASS`

`DEPENDENCY CORRECTION ACCEPTED — NO REMAINING PHYSICAL ORDER BLOCKER IDENTIFIED`

Formal record:

`CONTROL_TOWER/03_DATABASE_GOVERNANCE/DB_GOV_POST_CORRECTION_PASS_DECISION_2026-08-29.md`

This pre-authoring PASS remains valid for bounded non-Production work. It does not pre-accept any generated candidate.

## Current proposal gates

| Proposal | Relevant REM | Current controlling result |
|---|---|---|
| `DBP-001` | `REM-100` | `CODE-ONLY IMPLEMENTED; GREENFIELD TARGET HAS NO LEGACY POPULATION TO REPAIR` |
| `DBP-002` | `REM-210` | `AUTHORING/REHEARSAL AUTHORITY EXERCISED; FROZEN TECHNICAL CANDIDATE NOT POST-REHEARSAL ACCEPTED` |
| `DBP-004` | `REM-320` | `HOLD/STOP — EARLY EXECUTION DETECTED BEFORE DBP-002 ACCEPTANCE` |
| `DBP-003B/C` | `REM-220` | `PASS — AUTHORIZED AFTER ACCEPTED DBP-002/004; BEHAVIORAL ACTIVATION STILL DEPENDENCY-GATED` |
| `DBP-003A` | `REM-200` | `PASS — AUTHORIZED AFTER DBP-003B/C; LOGIN ACTIVATION STILL PASSWORD/LOCKOUT TEST-GATED` |
| `DBP-006` | `REM-400` | `PASS — AUTHORIZED AFTER DBP-003A + DBP-003B/C + ACCEPTED DBP-002/004` |
| `DBP-005` | `REM-310` | `PASS — AUTHORIZED AFTER ACCEPTED DBP-002/004; ORDERED LAST` |
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

## DBP-002 post-rehearsal intake

Control Tower independently reverified the frozen exact-head run identity:

- Full Rehearsal v3 `33222541097 = SUCCESS` at `ffdf1087...`.
- W0 `33222541108 = SUCCESS` at `ffdf1087...`.
- W7 disposable recovery `33222541109 = SUCCESS` at `ffdf1087...`.
- Full Rehearsal v2 `33222541073 = FAILURE` at `ffdf1087...`; failure is at `Baseline catalog backup restore reconciliation`, and the later candidate/RLS/regression/recovery stages are skipped in that workflow.

The technical results are retained, but governance acceptance cannot be issued because the authoritative mission-local post-rehearsal package is absent. The current manifest/hash/test/master-report package remains bound to `5d1352b4...`, not `ffdf1087...`, and no DBP-002 post-rehearsal evidence package exists under `EVIDENCE/`.

Formal intake record:

`CONTROL_TOWER/01_GROUP-01_FOUNDATION/MISSION-03_EXECUTION/DBP-002_POST_REHEARSAL_DB_GOV_INTAKE_DECISION_2026-09-07.md`

Verdict:

`DBP-002 POST-REHEARSAL DB-GOV INTAKE = FAIL / RETURN FOR EVIDENCE PACKAGING`

`DBP-002 = FROZEN TECHNICAL CANDIDATE — TECHNICAL RUNS RETAINED — GOVERNANCE ACCEPTANCE BLOCKED — NOT ACCEPTED`

This is an evidence-integrity/intake failure and is not a technical rejection of the frozen candidate.

`DBP-002 EVIDENCE PACKAGING = START AUTHORIZED — WAITING FOR WORKER SESSION`

A new exact-head report + repository-local evidence + new versioned manifest + detached SHA-256 must be published and stabilized for `ffdf1087...` before independent DB-GOV re-review.

## DBP-004 containment

Before DBP-002 post-rehearsal acceptance existed, the execution branch advanced through DBP-004 commits `1750fe82...` and `c3f2b7b4...`.

`DBP-004 = HOLD/STOP — UNAUTHORIZED EARLY EXECUTION DETECTED — PRESERVE COMMITS AS UNACCEPTED CANDIDATE EVIDENCE — NO FURTHER PRODUCT MODIFICATION`

The early commits receive no retroactive acceptance and must not be deleted, reverted, squashed, rebased, cherry-picked, force-pushed or history-rewritten by supervision.

## Bounded authoring authority

The original pre-authoring PASS does not authorize bypassing candidate acceptance sequencing. No further DBP-004 Product authoring is allowed until DBP-002 receives a fresh independent post-rehearsal PASS and Control Tower explicitly releases DBP-004.

## Disposable Greenfield rehearsal authority

Candidate application remains restricted to isolated PostgreSQL 18.6 rehearsal databases that start empty. No Production database/data/configuration/credential authority exists.

## Dependency failure gates

- DBP-002 acceptance is now the immediate gate for DBP-004 release.
- Failure of DBP-003B/C stops DBP-003A and DBP-006.
- Failure of DBP-003A stops DBP-006.
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
- Reviewed pre-authoring baseline: `5d1352b4fb6d56261dff8b8a622bacb2786f56d9` / `00512125311306a43474638195d2cad97b76118e`.
- Run `33201720896 = 153/153 PASS`; PostgreSQL 18.6; ten existing migrations; no model drift.
- Run `33201720878 = PASS`; disposable backup/restore and migration reconciliation `10/10`.

These prior runs prove only their own checkpoints; they do not substitute for a complete frozen-head DBP-002 acceptance package.

## Post-rehearsal gate

Every candidate checkpoint and the coordinated bundle require independent post-rehearsal DB-GOV acceptance. For DBP-002, report + evidence + manifest + detached SHA-256 + exact SHA/tree/parent must be coherent and independently verified before PASS.

## Prohibitions

No Production database/data/configuration/credentials. No secrets. Do not edit/delete/squash the existing ten migrations. No destructive/down-migration recovery reliance. No merge to master, PR #69 merge, rebase, cherry-pick, force-push or history rewrite.

MISSION-03 remains `IN PROGRESS — OPEN — NOT SEALED`; MISSION-04 remains `WAIT — NOT STARTED`. No `OWNER DECISION REQUIRED` is active.
