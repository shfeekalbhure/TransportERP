# DBP-003B/C ↔ DBP-003A ↔ DBP-006 — Physical Dependency Correction v1.1

- Status: `CORRECTED PACKAGE — AWAITING FRESH INDEPENDENT DB-GOV REVIEW`
- Supersedes only the dependency/order portions of `DBP-002_003_004_005_006_EXACT_PHYSICAL_DESIGN_RESUBMISSION.md v1.0` and the incompatible ordered sequence in the earlier coordinated DB-GOV decision.
- All other v1.0 physical definitions remain unchanged unless explicitly stated here.
- Product execution baseline: `codex/mission-03-execution-20260828@5d1352b4fb6d56261dff8b8a622bacb2786f56d9`
- Product tree: `00512125311306a43474638195d2cad97b76118e`
- Governance parent reviewed before this correction: `4c99ee79c97b0cf05a5c49fb33db833782b38425`
- Database baseline: `DB-BASELINE-001 = GREENFIELD / NEW / EMPTY / NO LEGACY TABLES OR DATA`
- Rehearsal target after approval only: isolated disposable PostgreSQL `18.6`

## 1. Corrected controlling physical order

The exact candidate-unit order is corrected to:

`DBP-002 → DBP-004 → DBP-003B/C → DBP-003A → DBP-006 → DBP-005`

Reserved candidate identities remain:

| Order | Candidate migration unit | Proposal | Physical dependency |
|---:|---|---|---|
| 11 | `GreenfieldTenantMembershipIsolation` | DBP-002 | existing ten migrations |
| 12 | `GreenfieldAuditV2AndAtomicOutbox` | DBP-004 | DBP-002 |
| 13 | `GreenfieldDeviceRegistryAndProof` | DBP-003B/C | DBP-002/004 |
| 14 | `GreenfieldLocalAuthSessions` | DBP-003A | DBP-002/003B/C/004 |
| 15 | `GreenfieldTypedOfflineProtocol` | DBP-006 | DBP-002/003A/003B/C/004 |
| 16 | `GreenfieldGovernedSettlement` | DBP-005 | DBP-002/004 |

This order is the only order submitted for the next independent DB-GOV review.

## 2. Why the correction is required

The prior v1.0 order correctly placed DBP-003B/C before DBP-006 because `offline_inbox` and Offline authorization carry durable registered-device/session/key provenance and the worker re-resolves device assignment/proof before commit.

The earlier coordinated DB-GOV decision inverted those two units and therefore could not govern the later exact physical package.

A second physical issue is also corrected here: `auth_sessions` in DBP-003A contains nullable `RegisteredDeviceId` plus the composite FK `(RegisteredDeviceId,CompanyId) -> registered_devices(Id,CompanyId)`. A migration that creates that FK cannot precede the migration that creates `registered_devices`. Therefore DBP-003B/C must physically precede DBP-003A unless the FK itself were deferred. This correction chooses the simpler, fully ordered design and does not defer the FK.

## 3. DBP-003B/C dependency refinement

DBP-003B/C physical schema creation depends on DBP-002 and DBP-004 only:

- `registered_devices` depends on Company/User baseline plus tenant controls;
- `device_assignments` depends on `registered_devices` plus DBP-002 membership/tenant FKs;
- `device_proof_keys` depends on `registered_devices`;
- `device_replay_nonces` depends on `registered_devices` and `device_proof_keys`;
- Audit V2 and caller-owned atomic audit/outbox behavior depend on DBP-004.

DBP-003B/C does **not** require an `auth_sessions` table merely to create its durable objects.

Behavioral activation is stricter than physical creation: any device lifecycle command that must revoke session families remains disabled until DBP-003A is installed and its session-revocation path passes. Any device lifecycle command that must quarantine Offline work remains disabled until DBP-006 is installed and passes. This prevents a temporary schema-ordering choice from weakening runtime invariants.

## 4. DBP-003A dependency refinement

DBP-003A is created after DBP-003B/C so that `auth_sessions.RegisteredDeviceId` can be bound to the already-existing device registry in the same candidate unit without an orphan/deferred FK.

The DBP-003A candidate must enforce the v1.0 composite device FK when `RegisteredDeviceId` is non-null and must retain all existing membership/company/branch/session-family constraints.

Login activation remains independently blocked until the new-system password hash/verify/lockout policy and tests pass. Physical rehearsal approval, if later granted, is not login-release authority.

## 5. DBP-006 dependency refinement

DBP-006 remains after DBP-003A and DBP-003B/C because its durable provenance and authorization contract require both:

- `MembershipId/UserId/CompanyId/BranchId` from DBP-002;
- `SessionId` and security/membership version validation from DBP-003A;
- `RegisteredDeviceId`, device assignment, proof-key version and replay state from DBP-003B/C;
- atomic Audit V2/Outbox from DBP-004.

No pre-device DBP-006 split is introduced. This avoids a second temporary Offline schema/protocol and avoids authorizing intake records whose device/proof provenance cannot yet be FK- or policy-enforced.

## 6. Required FK/index/test binding

The next independent DB-GOV review must verify at minimum:

1. DBP-003B/C creates unique `(Id,CompanyId)` on `registered_devices` before DBP-003A creates the composite session-device FK.
2. DBP-003A creates `auth_sessions` after device registry exists and enforces membership, branch/company and device tenant consistency.
3. DBP-006 creates Offline FKs only after membership/session/device/proof durable objects exist.
4. Cross-tenant device/session/offline INSERT/UPDATE attempts fail closed under RLS and composite constraints.
5. Device revoke/lost/replacement cannot activate until session-family revoke support exists; Offline quarantine coupling cannot activate until DBP-006 exists.
6. Candidate unit rollback/recreate tests demonstrate that failure of unit 13 stops 14/15, and failure of unit 14 stops 15, while DBP-005 remains independently gated by DBP-002/004.
7. `dotnet ef migrations has-pending-model-changes`, exact migration SQL, catalog/index/FK manifests, backup/restore and full regression are captured on the exact candidate head only after DB-GOV opens rehearsal authoring.

## 7. Acceptance-spec binding

`GREENFIELD_DB_REHEARSAL_ACCEPTANCE_SPEC.md` remains the controlling test specification, with its ordered-run step interpreted using the corrected unit order in this v1.1 addendum. All DBP-003 device/session negatives and DBP-006 provenance/replay/lease/revoke tests remain mandatory.

## 8. Authority boundary

This file is a design correction, not a DB-GOV approval.

Until a fresh independent DB-GOV decision is recorded **after this file exists**:

- Entity/DbContext/Migration/Schema/Seed/persistent-adapter authoring for the coordinated bundle: `HOLD`;
- applying candidate migrations to disposable PostgreSQL: `HOLD`;
- Production database/data/configuration/credentials: `PROHIBITED`;
- merge/rebase/cherry-pick/force-push/master mutation: `NOT AUTHORIZED BY THIS CORRECTION`.

MISSION-03 remains `IN PROGRESS — OPEN — NOT SEALED`; MISSION-04 remains `WAIT — NOT STARTED`.
