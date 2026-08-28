# DBP-003 — Local Session Persistence Execution Proposal

- Proposal: `DBP-003`
- REM: `REM-200 / REM-220`
- Authority decision: `AUTH-001 = RESOLVED — LOCAL APPLICATION AUTHORITY SELECTED FOR PRODUCTION TARGET`
- Code-only reference: `codex/mission-03-execution-20260828@cc67ad2bd491ed3ab23c3144f11dff955353c3a4`
- Proposal state: `READY FOR DB-GOV REVIEW — NOT AUTHORIZED FOR EXECUTION`
- Production access/credentials: `NONE`

## Current persistence evidence

At execution baseline `9c5b7a1...`, the committed model has `User.PasswordHash`, `User.Status`, nullable `User.CompanyId/BranchId`, scoped `UserRole`, `RolePermission` and `UserPermissionOverride`. It has no application session, refresh family, security-version, registered-device, assignment, proof-key, nonce or replay persistence. `Program.cs` validates an external authority or a configured local issuer/signing key but exposes no application login/refresh/logout endpoint. The exact committed migration lineage contains ten migrations and its model snapshot reflects that state.

PR #69 contains candidate session/device entities and migrations, but it is unmerged evidence only. None of its schema, entity or migration objects is adopted by this proposal.

## Proposed additive physical model

Names below are logical proposals subject to DB-GOV physical-name review.

| Object | Purpose | Proposed keys and constraints |
|---|---|---|
| `user_security_state` | application-wide invalidation version without rewriting legacy identity history | PK/FK `UserId -> users.Id`; `SecurityVersion bigint >= 1`; timestamps/concurrency token |
| `auth_sessions` | one row per refresh generation; family lineage, request scope and device selector | PK `Id`; indexed `FamilyId`; FK `UserId`; FK `CompanyId`; nullable FK `BranchId`; unique non-null `RefreshTokenHash`; self-FK `ReplacedBySessionId`; expiry ordering check; bounded status/reason; concurrency token |
| `registered_devices` | tenant-owned logical installation and versioned proof-key metadata | PK `Id`; unique `(CompanyId, ExternalDeviceId)`; FK Company; status check; key algorithm/thumbprint/version; expiry/revoke timestamps; no private key material |
| `registered_device_assignments` | active binding to one current `(User, Company, Branch)` membership | PK `Id`; FKs device/user/company/branch; status/effective dates; filtered unique active assignment policy after PostgreSQL feasibility review |
| `device_proof_nonces` | one-time server nonce/JTI replay window | PK `Id`; unique `(RegisteredDeviceId, NonceHash)`; issued/expires/consumed timestamps; bounded outcome; retention index |

The first governed migration must preserve the current singular user cardinality. It binds a session/assignment only where `User.CompanyId == CompanyId` and `User.BranchId` is null or equals `BranchId`. It must not invent a multi-company membership table or infer trust from a null company. Any future cardinality change is a separate DBP-002 design/migration.

Tenant-consistent composite FKs/checks that require new alternate keys are coordinated with DBP-002. DBP-003 must not silently duplicate or bypass that proposal.

## Service and transaction semantics

- Login reads one active user and exact active Company/Branch scope, verifies the approved password-hash format, increments failure/lockout state under reviewed concurrency, creates `user_security_state` if absent, and inserts the first session generation.
- Only a SHA-256 or stronger refresh-token digest is stored; raw refresh tokens, passwords, signing keys and device private keys are never persisted.
- Refresh locks the complete family in deterministic UUID order. Exactly one consumer may replace the active generation. A second/reused consumer atomically revokes every family row and appends audit before returning denial.
- Logout revokes the current session. Administrative or security-context invalidation revokes the family. Incrementing `SecurityVersion` invalidates every stale access/refresh generation.
- User/role/permission/membership/device state is re-evaluated server-side. Token claims select and narrow; they never grant authority.
- Device revoke, transfer, loss, replacement or proof-key recovery revokes bound sessions and blocks new Sync intake in the same governed transaction/outbox boundary.
- Concurrent refresh outcomes are deterministic: one rotation maximum; any observed reuse/race ends fail-closed with family revoke.

## Migration and preservation path

1. Capture exact source/model snapshot, ordered migration hashes, disposable PostgreSQL version, and a sanitized schema/data-shape inventory.
2. Take a recoverable safe-copy snapshot in the authorized non-Production rehearsal environment and prove restore before applying the candidate migration.
3. Add tables/indexes/checks/FKs only; do not delete or rename current user/RBAC columns, permission codes, audit rows, Sync operations or migration history.
4. Seed no accounts, roles, permissions, tokens, devices, keys or secrets. Initialize `user_security_state` deterministically to version `1` only through the reviewed migration path.
5. Deploy schema while local login endpoints remain disabled. Verify counts/FKs/indexes/model drift and existing behavior.
6. Enable local session issuance only after the application adapter, audit path, client clearing behavior and complete negative matrix pass on the migrated disposable copy.
7. Enroll devices in a separate governed stage. Existing device strings remain evidence and are never auto-promoted to trusted registrations.

## Rehearsal and upgrade tests

- clean database apply; exact current ten migrations followed by candidate migration;
- upgrade a sanitized safe-copy with users, singular scopes, roles/overrides, Sync/audit history and null-scope edge cases;
- pre/post row-count and referential reconciliation; no unexpected user/RBAC/Sync/audit mutation;
- application start and EF no-pending-model-drift check;
- valid/invalid/disabled/wrong-scope login;
- access expiry, stale security version, role/permission and membership revocation after issuance;
- refresh rotation, reuse/family revoke, logout, device mismatch and concurrent refresh race;
- cross-company direct-SQL inserts/updates rejected by constraints or equivalent reviewed controls;
- device registration/assignment/revoke/lost/replaced/key rotation and nonce/replay negatives;
- API, Offline queue, Desktop and Mobile credential-clearing behavior after revoke;
- backup restore and forward-recovery drill with evidence hashes.

## Rollback and recovery

Rollback is operational and forward-safe: disable local issuance/refresh, reject or revoke outstanding local sessions, preserve all new session/device/audit/replay evidence, restore the previous application version only if it fails closed against local tokens, then apply a corrective forward migration. Destructive table/column removal or migration-history rewrite is prohibited. Restore from the rehearsed safe copy is reserved for a failed isolated rehearsal and requires its own recorded decision.

## Audit and secrets boundary

Login success/failure, refresh rotation/reuse, logout, session/family revoke, security-version change, device enrollment/assignment/transfer/revoke/recovery, nonce replay and override attempts require correlation, actor/subject, tenant/branch, session/device/family identifiers, result and bounded reason. Audit must not contain passwords, raw refresh tokens, signing secrets, device private keys or raw recovery material.

Signing keys, password-hash policy/pepper, encryption keys and device private keys remain outside the database and repository in approved secret/platform custody. Key identifiers and public material may be stored only after custody/rotation/recovery design approval.

## Gate disposition

`READY FOR DB-GOV REVIEW`:

- additive logical model, transaction/concurrency semantics, preservation sequence, forward migration, safe-copy rehearsal, test matrix and non-destructive recovery strategy;
- code-only `ILocalSessionStore` boundary and atomic rotation contract at `cc67ad2...`;
- no current Entity/DbContext/Migration/Schema/Seed/data or Production change.

`ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION`:

- authorized live/sanitized row counts, applied migration history, PostgreSQL roles/extensions/RLS and backup/restore evidence;
- approved password-hash inventory and upgrade policy for existing `PasswordHash` values;
- Production secret/key custody, rotation and recovery operator evidence;
- device platform/MDM/attestation capabilities and nonce/replay/audit retention/legal-hold durations;
- DBP-002 composite tenant-key coordination and execution authorization.

Until those gates pass: `BLOCKED — DBP-003 ENTRY GATE REQUIRED` for every Entity, DbContext, Migration, Schema, Seed, data or persistent adapter change.
