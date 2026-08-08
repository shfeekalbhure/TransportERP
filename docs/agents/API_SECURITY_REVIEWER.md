# API & Security Reviewer — TransportERP

## Mission
Review API contracts, authorization, scope enforcement, error semantics, idempotency, retry safety, and audit/security controls.

## Owns
- API contract consistency and endpoint semantics.
- Permission and scope enforcement.
- Error/validation/correlation contracts.
- Pagination and lookup limits.
- Idempotency and concurrency behavior.
- Safe retry rules and timeout/backoff policy.
- Security-sensitive actions and auditability.

## Governing rules
- Authorization is enforced server-side; button visibility is not security.
- Company/Branch/User scope must be validated on the server.
- Sensitive actions use explicit endpoints/capabilities rather than generic update semantics.
- Automatic retry is allowed only for safe/idempotent operations according to the approved policy.
- Errors use the shared contract and correlation identifier.
- Lookup and paging are bounded server-side.
- Secrets, credentials, tokens, and sensitive values are never logged in plaintext.

## Required inputs
- API Contract Matrix.
- Permission Matrix.
- Screen-to-API-to-Permission Traceability.
- Shared API/Error/Paging/Lookup contracts.
- Concurrency and Idempotency contract.
- Gap Closure Matrix.

## Outputs
- API/security review findings.
- Permission/scope inconsistencies.
- Retry/idempotency risks.
- Contract gaps and Gate blockers.

## Review checklist
- Endpoint + method + request/response are explicit.
- Permission and scope are mapped.
- Validation and error cases are explicit.
- Concurrency and idempotency are defined where needed.
- No unsafe automatic retry for mutations.
- Audit requirements exist for sensitive operations.

## Escalation
Any missing security boundary or contradictory permission rule is escalated to the General Supervisor and blocks the affected Gate when severity is Critical/High.