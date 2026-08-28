# AUTH-001 — Production token/session authority mode

- Decision date: `2026-08-28`
- Authority: `OWNER APPROVED`
- Related mission: `MISSION-03 / W2-B2B`
- Related ADR: `ADR-W2-002 — Identity, RBAC and session authority`
- Execution baseline at decision: `codex/mission-03-execution-20260828@9c5b7a12e59d2c42e682717b8e90c491f8699b96`

## Decision

`AUTH-001 = RESOLVED — LOCAL APPLICATION AUTHORITY SELECTED FOR PRODUCTION TARGET`

TransportERP will target an application-owned local token/session authority rather than making an external OIDC/IdP authority a mandatory Production dependency.

## Evidence basis

The authoritative `master` currently supports two JWT validation modes: external `Auth:Authority`, or local `Auth:Issuer + Auth:SigningKey`. The repository and sealed evidence do not contain an established Production external IdP contract, availability/revocation evidence, or mandatory external authority decision. The product also requires offline-capable clients; therefore the target must not make external IdP reachability a prerequisite for normal TransportERP authentication/session lifecycle.

This decision selects the local application authority as the governed Production target. It does not authorize storing secrets in source control and does not authorize any Production credential, database, schema, migration or data mutation.

## Mandatory implementation boundaries

1. Authentication and authorization remain separate. Issued tokens do not become tenant/permission/device authority.
2. Request-time tenant scope and effective permissions remain server-resolved from persistent authority as established by ADR-W2-001/002.
3. Access tokens are short-lived. Refresh tokens use rotating one-time families; detected reuse revokes the family.
4. Logout/revoke must invalidate the applicable session/family and protected clients must clear/suspend credentials.
5. Session/device binding, membership/session persistence and any refresh-family tables are controlled by `DBP-003`.
6. Device registry/PoP/nonce/replay persistence remains controlled by `DBP-003/006`.
7. Signing keys/secrets must be supplied through secure deployment configuration/secret custody and must never be committed to the repository.
8. No Production deployment or credential activation is authorized by this decision.
9. Any future switch to an external IdP requires a separate governed decision and migration/recovery plan.

## W2 effect

- `W2-B2B`: owner authority-mode blocker is cleared.
- `W2-B2B` implementation that requires session persistence remains `BLOCKED — DBP-003 ENTRY GATE REQUIRED`.
- Authority-neutral/code-only preparation, endpoint contract design, failure behavior and tests may proceed where they require no DB/schema/data mutation.
- `W2-E` remains blocked until DBP-003 current-state, migration, preservation, test and recovery gates are satisfied.
- `W2-C2` remains separately blocked by DBP-003/006 and device/PoP evidence.

## Owner instruction

`APPROVED — EXECUTE ALL NON-DESTRUCTIVE WORK ENABLED BY THIS DECISION; DO NOT CROSS DB-GOV OR PRODUCTION BOUNDARIES.`
