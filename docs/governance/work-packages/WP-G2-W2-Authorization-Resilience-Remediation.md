# WP-G2-W2-Authorization-Resilience-Remediation — TransportERP

- **Owner:** API_SECURITY_REVIEWER
- **Independent reviewer:** QA_TESTING_REVIEWER
- **Target gate:** G2
- **Gate state:** `G2 = NOT READY` — unchanged by this package.

## Objective
Remediate only the remaining executable W2 gaps. Approved W2 values and documents are
immutable: lookup results remain capped at 50, and outbound resilience remains 30s total,
10s per attempt, three attempts, 2s exponential backoff with jitter.

## Scope

### OTS-W2-002 — authorization-bound lookup
- Authorization derives from an authenticated principal and server-issued claims only.
- `lookup.read` is evaluated by an ASP.NET Core authorization requirement/handler.
- Company/branch scope is taken from trusted claims and an explicitly requested scope is
  rejected when it differs; client headers never grant role, permission, company, or branch.
- The real lookup endpoint applies permission and scope before materialising at most 50 rows.
- Tests cover authorized, forbidden, forged-header, cross-company/branch, and capped results.

### OTS-W2-005 — observable outbound resilience
- A real API operational path consumes `IApiClient`.
- The delegating handler is exercised with a controllable HTTP handler and verifies retry count,
  attempt/total cancellation, 408/429/5xx retries, Retry-After, permanent failures, unsafe
  writes, Idempotency-Key writes, and caller cancellation.

## Exclusions
- Do not modify W1/W2/W3 approved documents, OTS-W2-001, desktop screens, or G2 state.
- No final G2 readiness review is authorized.

## Acceptance
`dotnet build` and `dotnet test` pass; CI is recorded; QA independently reviews the committed
implementation and tests. Candidate status is not `VERIFIED` until QA records that review.
