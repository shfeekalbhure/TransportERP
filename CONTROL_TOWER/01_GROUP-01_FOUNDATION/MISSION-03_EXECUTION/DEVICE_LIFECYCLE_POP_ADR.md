# ADR-W2-003 — Device lifecycle, ownership and proof of possession

Control Tower disposition: `DEP-007 = CONTROL TOWER REVALIDATED FOR BOUNDED CODE-ONLY IMPLEMENTATION` at execution baseline `9c5b7a12e59d2c42e682717b8e90c491f8699b96`. Registry/PoP/nonce/replay/session-device persistence remains behind DBP-003/006 and is not claimed as implemented.

- Decision date: `2026-08-28`
- Execution baseline: `069a311b8f0e66f5d1ee3fdcffed13ec13d0a91a`
- Governing dependency: `DEP-007`
- Findings: `A-SEC-002`, `A-OFF-002`, `TB-F-004`, `D-SEC-SYNC-001`
- Decision: `DEP-007 = RESOLVED FOR IMPLEMENTATION`
- Governance acceptance: `CONTROL TOWER REVALIDATED FOR BOUNDED CODE-ONLY IMPLEMENTATION`
- DB execution: `NOT AUTHORIZED — DBP-003/006 remain gated`

## Current evidence

- Master defines a device only as a `DeviceId` string in claims, request data and `SyncOperation`; `device_registered=true` is a claim. There is no registry, assignment, lifecycle, public key, nonce or replay table/service.
- The Sync batch endpoint compares the request device string with the token device string. It does not prove possession or query a device authority.
- Duplicate enqueue checks stored `UserId + DeviceId`; transition, retry, conflict creation and conflict resolution currently check tenant only. This is the confirmed `D-SEC-SYNC-001` gap.
- PR #69 adds registry/assignment, local-session binding, credential/proof-key lifecycle, nonce/replay persistence and extensive tests. It is a selective candidate, but legacy lifecycle methods still require explicit owner binding and its migrations are not authorized.

## Policy decision

### Definition and ownership

- A device is a server-registered logical installation identified by immutable registry ID plus company-scoped external device ID and a versioned proof public key. Hardware metadata is descriptive, not identity authority.
- The tenant owns the registry record. A user does not own the device globally; an active device assignment binds the device to one explicit user membership `(User, Company, Branch)` for use.
- A device may have multiple historical assignments but only governed active assignments. Assignment never widens user membership.

### Lifecycle authority

| Mutation | Permitted actor | Required controls |
|---|---|---|
| registration request | authenticated active member with `devices.register` | company fixed from server context; pending only |
| approve/activate | active tenant administrator with `devices.manage` | separation from untrusted request, reason/correlation and audit |
| assign | tenant administrator with `devices.manage` | target active membership and branch/company consistency |
| transfer | remove old assignment then add new one atomically or governed two-step | old/new scope validation, session revoke, immutable audit |
| suspend/reactivate | tenant administrator | reason, version change, session/offline effect, audit |
| revoke/lost/replaced | tenant administrator; self-report may only request action | immediate fail-closed trust withdrawal; session revoke and audit |
| proof-key bind/rotate | active assigned device plus current PoP; administrator permission where policy requires | nonce, exact endpoint/method/body/token binding, anti-replay |
| proof-key recovery | named recovery administrator | explicit reason, step-up/out-of-band evidence, revoke sessions/old key, immutable audit |

### Proof of possession and replay

- Sync write and protected key-lifecycle requests require a platform-backed asymmetric key where supported and a signed proof bound to access token, canonical HTTPS target, HTTP method, exact body hash, correlation ID, issued time and server nonce.
- Accepted nonce/JTI is one-time and persisted for the replay window. Algorithm, key version, thumbprint and assignment must match the active registry state.
- Missing, stale, duplicated, malformed or mismatched proof fails closed without a business mutation.

### Revocation and Offline behavior

- Revoke/suspend/assignment removal/key recovery invalidates active bound sessions and prevents new Sync intake immediately at the server.
- Clients erase or quarantine credentials as appropriate, stop protected navigation and freeze the outbound queue. Queued payloads are retained locally under policy for user recovery/export; they are not silently discarded or transmitted.
- Server-accepted queued work is re-authorized before execution. Revocation prevents new business execution unless the operation was already atomically committed; there is no partial replay.

### Sync lifecycle ownership and override

- Every transition, retry, conflict creation/resolution and replacement link must match the stored operation `CompanyId`, `BranchId`, `UserId` and `DeviceId`.
- Default behavior is deny non-owner. No generic administrator bypass is created.
- A future override requires a separately named permission, explicit reason, target operation, actor, prior/next state and immutable audit in the same transaction. Until that authority and atomic audit path are approved, override is unavailable.

## Negative tests

- device string/registry/session/assignment mismatch;
- pending, suspended, revoked, expired or inactive device;
- wrong user/company/branch assignment;
- stale credential/key version and lost/replaced device;
- missing/invalid signature, wrong key/algorithm/HTU/method/body/token/correlation;
- expired/future proof, missing nonce, reused nonce/JTI and concurrent replay;
- unauthorized approve/assign/transfer/revoke/recover and unaudited override attempt;
- same-tenant different user/device lifecycle mutation;
- Offline submission and worker execution after revoke/assignment removal;
- replacement operation from a different owner or tenant.

## DB and rollback implications

- Registry, assignment, session binding, proof key, nonce and replay persistence require DBP-003/006. No schema/data/migration change is authorized here.
- Migration must be additive with staged enrollment; existing device strings/operations remain evidence. Unknown historical rows are quarantined, not inferred as trusted.
- Code-only owner checks are independently revertible. Registry/PoP rollback must disable new intake safely while preserving registry, audit, proof replay and queued-operation provenance; destructive downgrade is prohibited.

## Bounded unknowns

| Unknown | Classification | Blocks |
|---|---|---|
| Production MDM/attestation and platform key capabilities | external evidence | attestation strength and client release, not owner checks |
| Retention duration for nonce/replay/audit and legal hold | owner/legal policy | DBP-003/006 physical design |
| Emergency non-owner override role | owner/security authority | override only; default deny remains executable |

## C2 preparation checkpoint

`W2_C2_PREPARATION.md` now defines the non-destructive enrollment/assignment/PoP envelope, fail-closed lifecycle, client revoke contract and negative matrix. No runtime registry endpoint or persistence was added. Registry/assignment, proof key, nonce/replay and session-device storage remain behind DBP-003/006.
