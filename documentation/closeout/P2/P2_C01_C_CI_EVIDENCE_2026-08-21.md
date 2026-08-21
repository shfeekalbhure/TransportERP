# TransportERP — P2-C01-C CI Evidence

**Date:** 2026-08-21 UTC+3  
**Phase:** `P2-C01-C — Release, Allocation, Trip, Manifest & Load`  
**Baseline:** `master@22ee24108b3c682d94e9d8693a566d6b479f19c9`  
**PR:** `#40`  
**Status:** `FINAL EXACT-HEAD CI RE-RUN REQUIRED`

## 1. EF migrations reviewed

Generated migrations:

- `TransportERP.Infrastructure/Persistence/Migrations/20260821132015_P2C01CShippingExecution.cs`
- `TransportERP.Infrastructure/Persistence/Migrations/20260821141529_P2C01CShippingExecutionHardening.cs`

Structural review result: `PASS — pending final exact-head CI`.

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

## 5. Final closure rule

No independent PASS or merge is valid until the following all succeed on the exact final head created by this evidence update:

1. closed-contract validator PASS;
2. C phase-boundary PASS;
3. build PASS;
4. non-database regression PASS;
5. EF pending-model check PASS;
6. full migration apply on PostgreSQL 18 PASS;
7. C PostgreSQL/HTTP/concurrency tests PASS;
8. Desktop RTL PASS.

## 6. Next-phase lock

`Arrival / Transit / Warehouse MUST NOT START` before P2-C01-C receives exact-head green CI, independent review PASS, and merge to master.
