# ADR-W2-002 — Identity, RBAC and session authority

- Decision date: `2026-08-28`
- Execution baseline: `069a311b8f0e66f5d1ee3fdcffed13ec13d0a91a`
- Governing dependency: `DEP-006`
- Findings: `A-SEC-001`, `A-SEC-002`, `TB-F-002`, `TB-F-003`
- Decision: `DEP-006 = RESOLVED FOR IMPLEMENTATION` for the authority-neutral request pipeline
- Governance acceptance: `PENDING CONTROL TOWER INDEPENDENT REVALIDATION/REBIND UNDER c274f9a HOLD`
- Bounded owner item: `AUTH-001 — select Production token/session authority mode`
- DB execution: `NOT AUTHORIZED — DBP-003 remains gated`

## Reconciliation

| Area | Master state | Determination | PR #69 evidence treatment |
|---|---|---|---|
| JWT validation | Authority or issuer+symmetric key; audience/lifetime checked | `CURRENT — retain as authentication boundary` | options validation is a candidate, not an adopted authority decision |
| User/tenant binding | independent claims; active user check only in Sync | `GAP — REIMPLEMENT` | `CurrentSecurityContext` is a useful candidate but null-company scope selection needs redesign |
| Permission evaluation | literal token `permission`/role claims | `GAP — REIMPLEMENT` | `EffectivePermissionResolver` is a selective candidate requiring key/cardinality tests |
| Login/token issuance | no current endpoint/service | `CURRENT ABSENCE / OWNER-BOUNDED` | local session implementation is a candidate only; not assumed required |
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

## Bounded owner decision AUTH-001

The repository and sealed evidence do not establish whether Production must use an external OIDC authority or the PR69-style local issuer. Owner/Control Tower must select and register exactly one Production mode with recovery, secret/key custody, availability and revocation evidence.

`OWNER DECISION REQUIRED — BOUNDED ITEM AUTH-001`

This blocks local session schema/endpoints and Production IdP integration, but does not block code-only server membership binding, DB-backed permission evaluation, fail-closed error behavior, or negative tests on the current isolated execution branch.

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
