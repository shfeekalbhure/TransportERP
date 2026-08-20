# TransportERP — W0-2B Selective Recovery Manifest

**Baseline:** `master@e8e22de26b4faa5040f53582ab2c8934d43216f0`  
**Integration branch:** `integration/p2-c01-foundation-20260820`  
**Date:** 2026-08-20 UTC+3  
**Rule:** no bulk merge from diverged branches.

## 1. Recovered assets

| Recovery ID | Source branch | Source asset | Target | Decision |
|---|---|---|---|---|
| PORT-001A | feature/p1-closeout-p2-preparation-20260819 | P1 master baseline record | documentation/closeout/P1/P1_MASTER_BASELINE_2026-08-19.md | PORTED |
| PORT-001B | feature/p1-closeout-p2-preparation-20260819 | P2 scope gate | documentation/closeout/P2/P2_SCOPE_GATE_AND_OWNER_DECISION_2026-08-19.md | PORTED |
| PORT-002A | impl/w1-platform | OperationContext | TransportERP.Contracts/Core/OperationContext.cs | PORTED |
| PORT-002B | impl/w1-platform | TransportError | TransportERP.Contracts/Core/TransportError.cs | PORTED |
| PORT-002C | impl/w1-platform | CapabilityState | TransportERP.Contracts/Core/CapabilityState.cs | PORTED |
| PORT-002D | impl/w1-platform | BusinessAuditEvent | TransportERP.Contracts/Core/BusinessAuditEvent.cs | PORTED |
| PORT-003A | g2/w3-coreui-remediation-current-spec-v3 | TransportScreenProfile | TransportERP.Desktop/CoreUI/Architecture/TransportScreenProfile.cs | PORTED |
| PORT-004A | impl/w1-setup-geo | GeoContracts | TransportERP.Contracts/Geo/GeoContracts.cs | PORTED CONTRACT ONLY |
| PORT-005A | impl/w1-setup-org | NumberReservation concept | TransportERP.Contracts/Numbering/NumberingContracts.cs | PORTED / NORMALIZED |

## 2. Assets intentionally not bulk-ported in W0-2B

### CoreUI runtime shell and controls

The W3 branch contains a large connected UI framework. It is not safe to copy individual runtime controls without their metrics, themes, dependencies, profile rules, and tests. Therefore W0-2B recovers only the governed profile taxonomy. Runtime CoreUI integration is deferred to `W0-4` after W3 reconciliation.

### Geo persistence/migrations

The old setup-geo branch carries a separate historical DbContext and migrations. P1 master already has a closed PostgreSQL physical baseline. Therefore only the provider-neutral contracts are recovered now. Geo persistence must be rebuilt against the current P1 DbContext after W1 approval.

### Org persistence and Company/Branch

P1 already owns Company and Branch. W0-2B does not import a second organizational model. Only the number-reservation contract is extracted because P2-C01 requires server-authoritative atomic numbering.

### Security/OpenAPI branch

`security/sec-rem-001-openapi` is highly diverged and overlaps newer platform/W3 work. No runtime file is ported in W0-2B. Security deltas are re-evaluated after W0-3 contract reconciliation.

### POC branches

Offline and database POC branches are retained as evidence/reference only. Production P1 Sync/Audit behavior on master supersedes the runtime POC code.

## 3. Validation added

- `TransportERP.Tests` now references `TransportERP.Contracts` explicitly.
- `P2FoundationContractTests.cs` validates OperationContext, NumberReservation idempotency metadata, geo hierarchy DTOs, structured errors, and capability-state rules.
- CI workflow for the integration branch must restore/test the test project and build the Desktop project before W0-2B closure.

## 4. Closure conditions

W0-2B closes only when:

1. recovered contract files compile;
2. existing P1 tests plus new foundation tests pass;
3. Desktop profile build passes;
4. recovery manifest is present;
5. no P1 migration or P1 entity lifecycle is changed;
6. recovery branch is reviewed through a PR before merge.

Until those checks pass, `W0-3` must not start.
