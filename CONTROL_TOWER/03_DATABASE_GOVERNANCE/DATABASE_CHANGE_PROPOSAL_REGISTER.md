# DATABASE CHANGE PROPOSAL REGISTER

`DB-GOV-001` is binding. Review here does **not** authorize Production database/schema/entity/migration execution. Every execution scope remains bounded by the exact independent decision, isolation, test and recovery evidence recorded below.

Owner decision `DB-BASELINE-001` is binding:

`TARGET DATABASE = GREENFIELD — NEW — EMPTY — NO LEGACY TABLES / NO LEGACY DATA`

The existing ten committed migrations remain the immutable bootstrap lineage for an empty PostgreSQL target.

## Current proposal register

| Proposal ID | Requirement | Review Status | Bounded Execution Status |
|---|---|---|---|
| `DBP-001` | `A-ARCH-002 / REM-100` | `REVIEWED — CODE-ONLY FIX IMPLEMENTED` | `NO LEGACY GREENFIELD DATA REPAIR REQUIRED` |
| `DBP-002` | tenant isolation | `POST-CORRECTION DB-GOV PASS` | `APPROVED FOR CANDIDATE AUTHORING + DISPOSABLE/GREENFIELD NON-PRODUCTION REHEARSAL` |
| `DBP-004` | audit integrity / atomic outbox | `POST-CORRECTION DB-GOV PASS` | `APPROVED AFTER DBP-002 IN COORDINATED ORDER` |
| `DBP-003B/C` | device registry/assignment/PoP/nonce/replay | `POST-CORRECTION DB-GOV PASS` | `APPROVED AFTER DBP-002 + DBP-004; BEHAVIORAL ACTIVATION REMAINS DEPENDENCY-GATED` |
| `DBP-003A` | auth/session/security-version | `POST-CORRECTION DB-GOV PASS` | `APPROVED AFTER DBP-003B/C; LOGIN ACTIVATION SEPARATELY PASSWORD-TEST GATED` |
| `DBP-006` | typed Offline/Sync | `POST-CORRECTION DB-GOV PASS` | `APPROVED AFTER DBP-003A + DBP-003B/C + DBP-002/004` |
| `DBP-005` | governed Settlement/accounting integrity | `POST-CORRECTION DB-GOV PASS` | `APPROVED AFTER DBP-002/004; ORDERED LAST IN COORDINATED RUN` |
| `DBP-007` | shipping lifecycle | `REVIEWED — DEFERRED INTAKE` | `BLOCKED — CANONICAL SCOPE REQUIRED` |
| `DBP-008` | ticketing | `REVIEWED — DEFERRED INTAKE` | `BLOCKED — CANONICAL REQUIREMENTS REQUIRED` |
| `DBP-009` | reporting | `REVIEWED — DEFERRED INTAKE` | `BLOCKED — REPORTING REQUIREMENTS REQUIRED` |

## Controlling post-correction decision

Correction:

`20608494998e671892ee35abd415158e399c9036`

Formal independent PASS record:

`CONTROL_TOWER/03_DATABASE_GOVERNANCE/DB_GOV_POST_CORRECTION_PASS_DECISION_2026-08-29.md`

Verdict:

`DB-GOV VERDICT = PASS`

`DEPENDENCY CORRECTION ACCEPTED — NO REMAINING PHYSICAL ORDER BLOCKER IDENTIFIED`

The previous conflict/recording hold is superseded by this later post-correction review record.

## Only approved coordinated physical order

`DBP-002 → DBP-004 → DBP-003B/C → DBP-003A → DBP-006 → DBP-005`

Candidate units:

| Order | Candidate migration unit | Proposal |
|---:|---|---|
| 11 | `GreenfieldTenantMembershipIsolation` | DBP-002 |
| 12 | `GreenfieldAuditV2AndAtomicOutbox` | DBP-004 |
| 13 | `GreenfieldDeviceRegistryAndProof` | DBP-003B/C |
| 14 | `GreenfieldLocalAuthSessions` | DBP-003A |
| 15 | `GreenfieldTypedOfflineProtocol` | DBP-006 |
| 16 | `GreenfieldGovernedSettlement` | DBP-005 |

No alternate order is authorized.

## Greenfield rehearsal model

Each candidate rehearsal must:

1. use isolated non-Production PostgreSQL 18.6;
2. start empty;
3. apply the existing ten migrations unchanged;
4. capture baseline catalog/migration evidence and backup digest;
5. restore baseline successfully to a second fresh disposable database;
6. apply only the candidate unit allowed by the controlling order;
7. run proposal-specific tenant/security/concurrency/failure/replay/accounting/audit negatives;
8. run the full existing regression and EF model-drift checks;
9. capture post-candidate FK/index/check/RLS-equivalent/catalog evidence;
10. backup/restore the candidate state and reconcile it;
11. retain exact SHA/tree/parent, generated SQL, snapshot diff and artifact hashes;
12. submit exact-head evidence to independent post-rehearsal DB-GOV.

No Production data, endpoint, role, credential, signing secret, pepper or private key is authorized.

## Dependency failure gates

- Failure of unit 13 stops 14 and 15.
- Failure of unit 14 stops 15.
- DBP-005 materially depends on DBP-002/004 but remains ordered last in the coordinated run.
- Failure does not authorize reordering.

## Activation gates retained

- Device lifecycle operations needing session-family revoke stay disabled until DBP-003A passes.
- Device lifecycle operations needing Offline quarantine stay disabled until DBP-006 passes.
- `LOGIN ACTIVATION` remains blocked until the new-system password hash/verify/lockout policy and tests pass.
- OFFLINE actions remain default-deny under OFFLINE-001.
- Settlement remains governed by ACC-001, configured account roles, FX/rounding, maker-checker/SoD and fiscal-period rules.

## Prior evidence retained

- W2 code-only controls through `9c5b7a12e59d2c42e682717b8e90c491f8699b96` were independently adopted without a persistence delta.
- W2-B2B through `cc67ad2bd491ed3ab23c3144f11dff955353c3a4` passed independent exact-diff/raw-CI review; run `33191269475 = 146/146 PASS` against the existing ten migrations with no model drift.
- MISSION-03 execution baseline reviewed for this coordinated package is `5d1352b4fb6d56261dff8b8a622bacb2786f56d9`, tree `00512125311306a43474638195d2cad97b76118e`.
- Run `33201720896 = 153/153 PASS`; PostgreSQL 18.6; ten migrations; no model drift.
- Run `33201720878 = PASS`; disposable backup/restore and migration reconciliation `10/10`.

## Production boundary

This register does not authorize Production bootstrap/release, master merge, real data, credentials, destructive migration, rebase, cherry-pick, force-push or history rewrite.

Every candidate bundle remains subject to independent post-rehearsal DB-GOV before any Production authority can be considered.

Every deletion remains `CANDIDATE FOR REMOVAL` until separately proved and authorized.
