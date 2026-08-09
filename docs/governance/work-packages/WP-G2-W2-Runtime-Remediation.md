# WP-G2-W2-Runtime-Remediation — TransportERP

- **Owner:** API_SECURITY_REVIEWER
- **Independent reviewer:** QA_TESTING_REVIEWER
- **Target gate:** G2
- **Scope:** Execute only the W2 runtime remediation below; approved W2 contracts are immutable.
- **Gate state:** `G2 = NOT READY` — unchanged.

## Required outcomes
1. `OTS-W2-001`: server-enforced `MaximumPageSize = 200`, including a real inbound API path and executable integration test for requests above the cap.
2. `OTS-W2-002`: server-enforced `MaximumLookupResults = 50`, real lookup/provider path, permission and scope filtering, no full-table lookup, and executable limit/overflow/auth-scope tests.
3. `OTS-W2-005`: an actual `IApiClient` consumer using the approved transport contract: total 30s; per-attempt 10s; three attempts; 2s exponential backoff with jitter; Retry-After; transient errors only; no automatic unsafe retry except explicit allowed Idempotency-Key operations. Add executable tests for every rule.

## Constraints
- Do not edit approved W1/W2/W3 documents.
- Do not change G2 or run final readiness review.
- Do not create GEN-004 or any screen/feature outside these fixes.
- Record exact paths, commit SHA, build/test results, and independent QA result in the G2 verification matrix after review.
