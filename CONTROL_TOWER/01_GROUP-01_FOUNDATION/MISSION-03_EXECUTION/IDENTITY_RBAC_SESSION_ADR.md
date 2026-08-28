# ADR-W2-002 — Identity, RBAC and session authority

Control Tower disposition: `DEP-006 = CONTROL TOWER REVALIDATED FOR AUTHORITY-NEUTRAL CODE-ONLY IMPLEMENTATION` at execution baseline `9c5b7a12e59d2c42e682717b8e90c491f8699b96`. `AUTH-001 = RESOLVED — LOCAL APPLICATION AUTHORITY SELECTED FOR PRODUCTION TARGET`. The code-only lifecycle checkpoint is `cc67ad2bd491ed3ab23c3144f11dff955353c3a4`; persistent adapters remain behind DBP-003.

- Decision date: `2026-08-28`
- Execution baseline: `069a311b8f0e66f5d1ee3fdcffed13ec13d0a91a`
- Governing dependency: `DEP-006`
- Findings: `A-SEC-001`, `A-SEC-002`, `TB-F-002`, `TB-F-003`
- Decision: `DEP-006 = RESOLVED FOR IMPLEMENTATION` for the authority-neutral request pipeline
- Governance acceptance: `CONTROL TOWER REVALIDATED; AUTH-001 OWNER DECISION RECORDED`
- Production authority: `LOCAL APPLICATION AUTHORITY`
- DB execution: `NOT AUTHORIZED — DBP-003 remains gated`

## Reconciliation

| Area | Master state | Determination | PR #69 evidence treatment |
|---|---|---|---|
| JWT validation | Authority or issuer+symmetric key; audience/lifetime checked | `CURRENT — retain as authentication boundary` | options validation is a candidate, not an adopted authority decision |
| User/tenant binding | independent claims; active user check only in Sync | `GAP — REIMPLEMENT` | `CurrentSecurityContext` is a useful candidate but null-company scope selection needs redesign |
| Permission evaluation | literal token `permission`/role claims | `GAP — REIMPLEMENT` | `EffectivePermissionResolver` is a selective candidate requiring key/cardinality tests |
| Login/token issuance | no current endpoint/service | `CODE-ONLY LIFECYCLE IMPLEMENTED; ACTIVATION REQUIRES DBP-003 ADAPTER` | state-machine intent reimplemented without copying PR persistence |
| Refresh rotation/reuse | absent | `GAP IF LOCAL MODE SELECTED` | candidate needs independent concurrency, audit and recovery verification |
| Logout/revoke | absent in application | `GAP` | self-revoke and family revoke are candidates if local mode is selected |
| External IdP revocation | configuration/evidence unavailable | `VERIFY — EXTERNAL` | PR #69 does not prove Production IdP semantics |
| Session/device binding | claim-only device in master | `GAP` | candidate session/device binding depends on DBP-003 and DEP-007 |
| Cache invalidation | no permission cache found | `CURRENT — no stale cache`; future cache must be versioned | no cache adoption without invalidation tests |
| Worker identity | no explicit service/workload identity found | `GAP — DESIGN REQUIRED BEFORE WORKER EXPOSURE` | do not reuse interactive/platform claims |

## Decision

1. Authentication and authorization are separate. JWT validation establishes issuer-bound subject authenticity; it does not grant tenant, permission or device authority.
2. One request pipeline resolves an active user, one active membership/TenantContext, current session status when the selected mode has application sessions, current effective permission and device binding when required.
3. Persistent RBAC is the request-time authority. Token permission/role/platform claims may be hints only and can never widen the server decision.
4. Explicit deny wins. Missing/malformed scope, unknown permission code, inactive role/user/company/branch, ambiguous membership, stale session or unavailable authority fails closed.
5. Permission codes remain API contracts and are preserved. Effective grants are scoped to the resolved membership; scope shape and branch/company consistency are validated before use.
6. No authorization result cache is introduced in W2. A future cache must key by user, membership, permission, session/security version and expire/invalidate on any role, override, membership, user, session or device change.
7. Desktop/Mobile store access/refresh credentials only in platform secure storage, clear them and suspend protected/offline mutations on revoke, and never manufacture tenant/permission claims.
8. Offline operations retain the accepted server authority/provenance. A queued operation is re-authorized at intake/execution; a revoked session/device cannot submit new mutations.

## Session lifecycle by mode

| Concern | External authority mode | Local session mode candidate |
|---|---|---|
| Authoritative identity | configured issuer subject plus server membership | local active user plus server membership |
| Access token | short-lived issuer token | short-lived application token with session/security version |
| Refresh | IdP-owned and evidenced | rotating one-time refresh family; reuse revokes family |
| Revocation | issuer event/introspection/version contract plus local deny/version where needed | DB session revoke, user security version and device binding |
| Logout | client credential deletion plus configured server/IdP revoke | revoke current/family then client deletion |
| Failure | 401 for invalid/stale identity/session; 403 for valid identity lacking scope/permission | same |

## AUTH-001 resolution and bounded implementation

The owner selected local application authority for the Production target. W2-B2B therefore implements the storage-neutral lifecycle contract: login authority adapter, narrow JWT issuance without role/permission grants, short access lifetime, one-time refresh rotation, family revoke on reuse/race, logout/current/family revoke, security-version and current-membership checks, client `ClearAndSuspendOffline`, and Offline mutation denial after revoke.

The API intentionally does not register a test/in-memory session store or expose local endpoints. Production activation requires a durable `ILocalSessionStore`, approved password-hash/current-identity adapter, atomic audit path and DBP-003 migration. Until then the existing authentication configuration remains preserved and local issuance fails closed by absence of registration.

The DBP-003 design, rehearsal and recovery proposal is `DBP-003_SESSION_PERSISTENCE_PROPOSAL.md` and is `READY FOR DB-GOV REVIEW — NOT AUTHORIZED FOR EXECUTION`.

## Negative tests

- Permission claim present but DB grant absent/denied.
- DB grant revoked after token issuance.
- User, company, branch or membership disabled after token issuance.
- Stale/revoked/expired session; rotated refresh reuse; logout then access/refresh.
- Wrong issuer/audience/signature, missing subject, missing/ambiguous scope.
- Tenant/branch selector mismatch and platform claim without explicit platform grant.
- Permission cache invalidation if caching is later introduced.
- Desktop/Mobile credential clearing and protected navigation after revoke.
- Offline submission and worker execution after user/session/device revoke.

## Rollback

- Code-only resolution/permission changes are isolated commits and can be reverted while retaining current predicates and permission codes.
- Local-session or membership persistence cannot be deployed without DBP-003 forward migration, safe-copy rehearsal and recovery. Rollback must preserve users, permission codes, audit and issued-session evidence; destructive downgrade is prohibited.
