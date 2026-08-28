# DBP-003A — Revised Rehearsal Entry Resubmission

- Revision: `v0.9`
- Product baseline: `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`
- Execution baseline: `codex/mission-03-execution-20260828@cc67ad2bd491ed3ab23c3144f11dff955353c3a4`
- Execution tree: `ea940e592cb11f5fff736e68055ebf77d2eece88`
- Prior DB-GOV decision: `DBP-003A = REVISE BEFORE REHEARSAL`
- Resubmission state: `DESIGN REVISION COMPLETE — EXTERNAL EVIDENCE GATES OPEN — NOT AUTHORIZED FOR REHEARSAL`
- Product/DB mutation: `NONE`

This resubmission closes the repository-resolvable design defects identified in
`DBP-003_DB_GOV_REVIEW_DECISION.md`. It does not manufacture the missing
PasswordHash, safe-copy, live-role, key-custody or tenant-population evidence,
and it does not request Entity, DbContext, Migration, persistent-adapter or data
authoring before an independent DB-GOV decision.

## 1. Revalidated current state

- The current model has a required `users.PasswordHash varchar(500)` and no
  password verifier, format/version marker, failure counter, lockout state,
  application session, refresh family or security-version persistence.
- Repository fixtures use only non-authoritative values such as `test-only`.
- The W1 contract explicitly records `AUTH-REUSE-PENDING`, requires a separate
  password/identity decision before import, and forbids automatic reuse.
- Referenced AlTayerERP identity source files are absent from every reachable
  Git object.
- PR #69 selects ASP.NET `PasswordHasher<User>` and adds session/device schema,
  but remains unmerged comparative evidence. It does not establish the format
  of any current or legacy stored value.
- User has independent Company and nullable Branch foreign keys. It has no
  tenant-consistent composite user key and no explicit membership persistence.
- The existing audit service owns its own transaction and is not an atomic
  session-audit Unit of Work.

## 2. Proposed DBP-003A physical design

The definitions below are review specifications, not executable DDL.

### `user_security_state`

| Field / rule | Proposed invariant |
|---|---|
| `UserId uuid` | primary key and FK to `users(Id)` with `ON DELETE RESTRICT` |
| `SecurityVersion bigint` | required, default `1`, check `>= 1` |
| `AccessFailedCount integer` | required, default `0`, check `>= 0` |
| `LockoutEnd timestamptz` | nullable; a future value denies login |
| `LastFailedAt timestamptz` | nullable diagnostic/audit anchor |
| `ConcurrencyVersion bigint` | required, default `1`, check `>= 1`; increment on every mutation |
| timestamps | required created/updated UTC timestamps |

Initialization must be deterministic and race-safe. The rehearsal migration may
use an additive `INSERT ... SELECT ... ON CONFLICT DO NOTHING` for existing
users, while every future user creation must create its security row in the same
Unit of Work. Security-state mutation uses a row lock or checked compare-and-swap
and never infers a password format.

### `auth_sessions`

| Field / rule | Proposed invariant |
|---|---|
| `Id uuid` | primary key |
| `FamilyId uuid` | immutable refresh-family identifier |
| `Generation integer` | required, check `>= 0` |
| `UserId uuid` | FK to `users(Id)`, restrict delete |
| `CompanyId uuid` | FK to `companies(Id)`, restrict delete |
| `BranchId uuid` | nullable; composite FK `(BranchId, CompanyId)` to `branches(Id, CompanyId)` |
| `DeviceId varchar(120)` | bounded selector only; not proof of registry trust |
| `SecurityVersionAtIssue bigint` | required, check `>= 1` |
| `RefreshTokenDigest bytea` | required, exactly 32 bytes (SHA-256 digest); raw token prohibited |
| `LifecycleState` | bounded `ACTIVE`, `ROTATED`, or `REVOKED` |
| issue/expiry fields | `IssuedAt < AccessExpiresAt <= RefreshExpiresAt` |
| consumption/revoke fields | nullable `ConsumedAt`, `RevokedAt`, bounded `RevokeReason` |
| `ReplacedBySessionId uuid` | nullable self-FK; immutable after transition |
| `CreatedByOperationId uuid` | required correlation/idempotency key for ambiguous-commit reconciliation |
| `ConcurrencyVersion bigint` | required, check `>= 1` |

Required keys and indexes:

1. unique global `RefreshTokenDigest`;
2. unique `(FamilyId, Generation)`;
3. unique non-null `ReplacedBySessionId`;
4. partial unique `(FamilyId)` where state is `ACTIVE`;
5. unique `CreatedByOperationId`;
6. indexes `(FamilyId, Id)`, `(UserId, SecurityVersionAtIssue)`,
   `(UserId, LifecycleState, RefreshExpiresAt)`,
   `(CompanyId, BranchId, UserId)`, and the expiry cleanup path.

State rules:

- `ACTIVE`: no consumed/revoked timestamp and no successor.
- `ROTATED`: consumed timestamp and exactly one successor are present.
- `REVOKED`: revoked timestamp/reason are present and no active successor can be
  created by the same command.
- Successor must have the same family, user, company, branch and device, and
  generation `predecessor + 1`. These cross-row facts require transactional
  validation/trigger review; a plain row check is insufficient.

### Tenant consistency boundary

DBP-003A does not reinterpret null scope as a wildcard. A bounded rehearsal may
propose a separately reviewed singular-model trigger requiring:

`users.CompanyId = auth_sessions.CompanyId` and
`users.BranchId IS NOT DISTINCT FROM auth_sessions.BranchId`.

Production activation with explicit multi-membership remains dependent on
DBP-002. No membership table or backfill is bundled into DBP-003A.

## 3. PostgreSQL refresh transaction

The current storage-neutral interface is insufficient for durable correctness
because lookup precedes rotation, family revoke is separate, and audit is not in
the same Unit of Work. A future adapter therefore requires a single atomic
command such as `RotateOrRevokeFamily(command, auditDraft)` and a separate
atomic login command.

The required transaction is:

1. Begin a PostgreSQL `SERIALIZABLE` transaction.
2. A digest lookup may obtain only the immutable user/family selectors.
3. Lock in deterministic order: user security-state row, then every family row
   ordered by `Id FOR UPDATE`, then the audit stream head.
4. Re-read the presented digest from the locked family. No decision uses the
   pre-lock snapshot.
5. For one valid active generation, mark the predecessor `ROTATED`, insert one
   `ACTIVE` successor, and append the rotation audit in the same transaction.
6. For consumed/revoked/expired/device/scope/security-version mismatch, revoke
   every active family row and append the denial/reuse audit in the same
   transaction.
7. Commit before returning the successor raw token. Raw tokens never enter the
   database or audit.

Retry and failure classification:

- SQLSTATE `40001` or `40P01`: discard transaction and DbContext, then retry the
  entire Unit of Work with bounded attempts and jitter.
- refresh-digest `23505`: generate a new random raw token and retry the entire
  command; never weaken digest uniqueness.
- named active-family/generation/successor conflicts: begin a new transaction,
  re-read the family, classify reuse/race, revoke the family and audit denial.
- every other constraint or SQLSTATE fails hard and is retained as evidence.
- an aborted transaction is never queried.
- ambiguous commit is reconciled only through `CreatedByOperationId`; a second
  successor is prohibited. If successor delivery cannot be proved, revoke the
  family and require reauthentication.

Audit append must accept a caller-owned transaction. The current autonomous
`AuditEventService` path cannot be used for DBP-003A without the separately
reviewed DBP-004-compatible enlistment design.

## 4. Failure-injection and concurrency acceptance

The rehearsal must inject failure:

- before predecessor mutation;
- after predecessor mutation;
- after successor insert;
- during audit append;
- after save and before commit;
- during connection loss/ambiguous commit;
- on serialization/deadlock;
- on refresh digest collision.

Required results:

- definite rollback leaves the predecessor active and produces no successor or
  audit record;
- committed rotation contains predecessor transition, exactly one successor and
  matching audit atomically;
- no successor exists without predecessor transition and audit;
- no consumed token can be resurrected;
- concurrent same-token refresh yields at most one successor, and subsequent
  reuse detection revokes the complete family;
- retry creates no duplicate successor or audit;
- audit-chain verification remains valid.

The current in-memory tests remain useful contract tests but are not durable
PostgreSQL evidence.

## 5. PasswordHash gate

`PASSWORD-HASH BASELINE = ACCESS BLOCKED — UNKNOWN — REQUIRES AUTHORIZED EXTERNAL EVIDENCE`

Required authorized non-Production evidence:

1. sanitized aggregate of hash prefixes, lengths and counts, including null,
   empty and malformed rows, without exposing hashes;
2. authoritative generator/verifier source or identity documentation;
3. one controlled known-password fixture for each active/legacy format;
4. version/salt and pepper-custody metadata, never the pepper itself;
5. approved legacy verification plus opportunistic-rehash or forced-reset
   policy;
6. failure window, maximum failures, lockout duration, reset and administrator
   unlock policy;
7. malformed-hash and concurrent-failure behavior.

Until this evidence is accepted, login persistence and endpoint activation are
prohibited. The proposal deliberately selects no algorithm.

## 6. Safe-copy package

The repository-preparable package is defined by:

- `DBP003A_SAFE_COPY_READONLY_INVENTORY.sql`;
- `DBP003A_RECONCILIATION.sql`;
- `DBP003A_REHEARSAL_RUNBOOK.md`.

The scripts are read-only and contain no candidate DDL or data repair. Actual
rehearsal entry additionally requires a named authorized non-Production copy,
PostgreSQL version, schema-only and custom-format backup digests, successful
restore to a new disposable instance, applied-history/role/extension/RLS
inventory, pre/post reconciliation, and operator/time/environment identity.

## 7. Dependency split

- `DBP-003A`: this revised design is resubmitted; rehearsal remains closed until
  independent DB-GOV accepts the design and the external PasswordHash/safe-copy
  evidence.
- `DBP-003B`: `DEFERRED — DEPENDS ON DBP-002/006` for explicit membership,
  registry/assignment authority and tenant-consistent keys.
- `DBP-003C`: `DEFERRED — DEPENDS ON DBP-002/006` for proof protocol,
  nonce/replay uniqueness and retention/legal-hold/key-recovery policy.

## 8. PR #69 disposition

Useful comparative evidence only: family `FOR UPDATE` ordering, caller-owned
audit intent, rollback and concurrent-refresh test patterns.

Rejected as an adoption unit: bundled audit/user/session/device/migration/seed
changes, absence of the required DB-enforced one-successor design, incomplete
SQLSTATE/constraint retry policy, and an unproved `PasswordHasher<User>` choice.

## 9. Decision requested from independent DB-GOV

The repository-resolvable design defect `DBP003-BLK-001` is addressed in this
revision, and read-only safe-copy tooling is prepared. The following remain open
and cannot be fabricated by MISSION-03:

- `DBP003-BLK-002`: PasswordHash reality;
- `DBP003-BLK-003`: actual named safe-copy/backup/restore/reconciliation evidence;
- `DBP003-BLK-004`: DBP-002 tenant physical decision;
- `DBP003-BLK-005`: DBP-006 device/proof/retention decision;
- `DBP003-BLK-006`: Production key custody, which blocks Production activation.

Requested disposition: review this revision, but keep execution at
`NONE` unless and until the complete rehearsal evidence gate is independently
satisfied. No self-approval is claimed.
