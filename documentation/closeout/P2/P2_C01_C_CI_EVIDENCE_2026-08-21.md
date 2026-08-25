# TransportERP — P2-C01-C CI Evidence

**Date:** 2026-08-21 UTC+3  
**Phase:** `P2-C01-C — Release, Allocation, Trip, Manifest & Load`  
**Baseline:** `master@22ee24108b3c682d94e9d8693a566d6b479f19c9`  
**PR:** `#40`  
**Status at issuance:** `FINAL EXACT-HEAD CI RE-RUN REQUIRED`
**Current reconciled status:** `FINAL EXACT-HEAD CI PASS — CLOSED_AFTER_MERGE`
**Final reviewed head:** `0fc0d7446efd66a03b60e4d0f4bf48a0de6d94cc`
**Master merge:** `5d58a42046e07166e6db76bcb893f32b1d8f2ec7`

## 1. EF migrations reviewed

Generated migrations:

- `TransportERP.Infrastructure/Persistence/Migrations/20260821132015_P2C01CShippingExecution.cs`
- `TransportERP.Infrastructure/Persistence/Migrations/20260821141529_P2C01CShippingExecutionHardening.cs`

Structural review result at issuance: `PASS — pending final exact-head CI`.

Final reconciled result: `PASS — final exact-head CI confirmed on 0fc0d7446efd66a03b60e4d0f4bf48a0de6d94cc`.

The initial migration is additive and creates only the C-authorized persistence set:

- `item_releases`
- `trips`
- `trip_stops`
- `trip_allocations`
- `manifests`
- `manifest_lines`
- `movement_events`
- `waybill_holds` as the read-only blocker dependency required by W1-P2C01-026

The hardening migration is EF-generated from the reviewed model change that enforces one governed manifest lifecycle per Trip in C. This closes the lifecycle defect where one DRAFT manifest could move the Trip to READY while another DRAFT manifest remained unable to finalize.

Neither migration introduces Arrival, Warehouse, Delivery, Customs, Commission, Settlement, Fleet or GPS runtime.

## 2. Reviewed safeguards

- release and allocation quantities are positive and reversal-shaped;
- manifest line loaded quantity cannot exceed planned quantity;
- `MovementEvent` is constrained to C-authorized `LOAD` and `DEPART` event types;
- idempotency indexes exist for release, allocation, trip creation, manifest creation and movement operations;
- Trip number remains unique within company scope;
- a unique `TripId` manifest index prevents a second manifest lifecycle from stranding Trip state in C;
- foreign keys to future Fleet runtime are intentionally absent: `VehicleId` and `DriverId` remain reference identifiers per W1-P2C01-010;
- `Down()` removes C tables in dependency-safe order.

## 3. Concurrency evidence added

`TransportERP.Tests/P2C01CConcurrencyPostgreSqlTests.cs` adds real PostgreSQL 18 races using separate DbContexts and synchronized starts:

1. two concurrent Release requests cannot exceed original WaybillItem quantity;
2. two concurrent Allocation requests cannot exceed released quantity;
3. two concurrent Load requests cannot exceed ManifestLine quantity;
4. a Trip cannot acquire a second Manifest lifecycle.

The prior hardening run produced real PostgreSQL serialization failures (`40001`) during competing quantity writes while the tests still completed with controlled outcomes and invariant-preserving persisted balances. Combined with the existing C PostgreSQL/HTTP tests, the C database gate now contains 7 tests.

## 4. Prior hardening run evidence

On the hardening-predecessor head, workflow `P2 C01 C shipping execution` completed successfully with:

- closed-contract validator: PASS;
- C phase boundary: PASS;
- non-database regression: `72/72 PASS`;
- EF hardening migration generation: PASS;
- pending-model check: PASS;
- P1 + A + B + C + C-hardening migration apply on PostgreSQL 18: PASS;
- C PostgreSQL/HTTP/concurrency gate: `7/7 PASS`;
- Desktop RTL: PASS.

Because EF generated and pushed the hardening migration, the PR head moved to the bot-created migration commit. This governance evidence update intentionally creates a user-authored head containing both committed migrations and retriggers all C gates for final exact-head review.

## 5. Final closure rule at issuance

At issuance, no independent PASS or merge was valid until the following all succeeded on the exact final head created by this evidence update:

1. closed-contract validator PASS;
2. C phase-boundary PASS;
3. build PASS;
4. non-database regression PASS;
5. EF pending-model check PASS;
6. full migration apply on PostgreSQL 18 PASS;
7. C PostgreSQL/HTTP/concurrency tests PASS;
8. Desktop RTL PASS.

## 6. Final exact-head evidence

Workflow `P2 C01 C shipping execution` [run 32524128894](https://github.com/shfeekalbhure/TransportERP/actions/runs/32524128894) ran on pull-request head `0fc0d7446efd66a03b60e4d0f4bf48a0de6d94cc` and concluded `SUCCESS`. `Shipping + PostgreSQL + HTTP` and `Shipping Desktop RTL` both concluded successfully. The exact-head review records the W0-3 contract validator PASS, regression `103/103`, PostgreSQL/HTTP/concurrency/hardening `12/12`, EF clean, PostgreSQL 18.6, and Desktop RTL/W3 PASS.

## 7. Closure disposition

[Independent review 4997311857](https://github.com/shfeekalbhure/TransportERP/pull/40#pullrequestreview-4997311857) returned `PASS` on the same SHA. [PR #40](https://github.com/shfeekalbhure/TransportERP/pull/40) merged as `5d58a42046e07166e6db76bcb893f32b1d8f2ec7`. The C CI/review/merge lock is satisfied. Any `P2-C01-D` work still requires its own governing scope and pre-programming gates.
