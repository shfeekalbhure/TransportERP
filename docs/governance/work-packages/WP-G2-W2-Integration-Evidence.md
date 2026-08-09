# WP-G2-W2-Integration-Evidence — TransportERP

## Identity
- **Owner:** API_SECURITY_REVIEWER
- **Independent reviewer:** QA_TESTING_REVIEWER
- **Target Gate:** G2
- **Gate state:** `G2 = NOT READY` (unchanged)

## Objective
Close only the QA evidence gaps found at PR #2 head `4e481b5`: HTTP middleware evidence for
OTS-W2-002, and full API-container wiring evidence for OTS-W2-005.

## In scope
- A `WebApplicationFactory`/TestServer HTTP test proving JWT authentication, authorization,
  claim-derived company/branch scope, forged-header rejection, and the endpoint response cap of 50.
- An HTTP endpoint test proving `DownstreamStatusController -> DownstreamStatusService -> IApiClient
  -> SafeReadRetryHandler`, including observable retry and `Retry-After`; DI-level unsafe/idempotent
  request evidence reuses the actual handler without replacing existing unit tests.
- Only the JWT and `Downstream:StatusUrl` settings needed for runnable configuration.

## Out of scope
- Any W1/W3 work, G2 disposition, PR merge, approved W2 policy values, or feature work.

## Acceptance criteria
1. Tests cross ASP.NET Core authentication and authorization middleware with a valid JWT and return
   `403` for a forged header where the token lacks the permission claim.
2. The real lookup endpoint returns no more than 50 scoped records.
3. The endpoint path demonstrates configured downstream target, typed client DI, handler retry, and
   `Retry-After`; existing resilience unit tests remain intact.
4. CI and independent QA are required before either item is marked VERIFIED.
