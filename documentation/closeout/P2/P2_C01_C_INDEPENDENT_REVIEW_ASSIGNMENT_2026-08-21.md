# TransportERP — P2-C01-C Independent Review Assignment

**Date:** 2026-08-21 UTC+3  
**Phase:** `P2-C01-C — Release, Allocation, Trip, Manifest & Load`  
**Baseline:** `master@22ee24108b3c682d94e9d8693a566d6b479f19c9`  
**Status:** `ASSIGNED — REVIEW REQUIRED BEFORE MERGE`

## 1. Review scope

The reviewer must inspect only the first shipping-execution package after P2-C01-B:

- ItemRelease;
- Trip and planned TripStop data;
- TripAllocation and allocation reversal;
- Manifest / ManifestLine;
- LoadAllocatedQuantity;
- FinalizeManifest;
- HandoverManifestToDriver;
- StartTrip;
- append-only LOAD and DEPART movement evidence required by the above actions;
- WaybillHold only as a blocking dependency, with no hold command surface;
- additive PostgreSQL schema, APIs and governed RTL screens for this package.

## 2. Mandatory checks

1. Release net never exceeds original WaybillItem quantity.
2. Allocation net never exceeds released remaining quantity.
3. Split allocation of one item across several trips is supported without losing history.
4. Allocation reversal is a separate ledger record and cannot rewrite accepted load history.
5. Load net never exceeds allocation remaining quantity.
6. All quantity writes are idempotent and concurrency-safe on PostgreSQL 18.
7. Company/Branch scope is enforced in API and persistence.
8. Active WaybillHold blocks the governed actions that declare `HOLD_BLOCKED`.
9. Trip/Manifest state transitions match W2, especially `DRAFT -> FINALIZED -> HANDED_OVER/ACCEPTED` and Trip start only after custody acceptance.
10. `FinalizeManifest` from RR1 is implemented before handover.
11. Vehicle/Driver stay reference IDs; no Fleet runtime is invented.
12. LOAD/DEPART movement evidence is append-only and does not broaden into arrival/unload/delivery runtime.
13. Trip execution creates no additional Waybill revenue.
14. Migration is additive and does not rewrite P1, A or B migrations.
15. Regression for all previously closed packages remains green.
16. Desktop surfaces are Arabic RTL and have no direct database access.
17. No P2-C01-D/later endpoint, table or production screen leaks into C except the explicitly allowed WaybillHold blocker storage.

## 3. Mandatory FAIL conditions

FAIL if C introduces:

- arrival/finalize-arrival/unload/reallocate runtime;
- warehouse balance runtime;
- customs clearance;
- delivery/POD/COD;
- commissions/TripSettlement/financial close;
- GPS/Fleet domain entities;
- mutable accepted quantity or movement ledger rows;
- client-side authoritative numbering or direct Desktop persistence access.

## 4. Required evidence for PASS

- exact final PR head SHA;
- phase-boundary validation PASS;
- EF migration committed and `has-pending-model-changes` clean;
- non-database regression PASS;
- PostgreSQL 18 tests for release/allocation/load concurrency and idempotency;
- Trip/Manifest lifecycle integration tests;
- hold-block test;
- API permission/scope tests;
- RTL Desktop build/screen contract check;
- PR diff inspection showing no later-phase leakage;
- explicit final decision `PASS` or `FAIL`.

Any head movement after review invalidates the review.

## 5. Gate

Arrival/transit/warehouse implementation MUST NOT START until C receives final green CI, explicit independent PASS and merge into `master`.
