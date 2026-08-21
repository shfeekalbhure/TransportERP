# TransportERP — P2-C01-C CI Evidence

**Date:** 2026-08-21 UTC+3  
**Phase:** `P2-C01-C — Release, Allocation, Trip, Manifest & Load`  
**Baseline:** `master@22ee24108b3c682d94e9d8693a566d6b479f19c9`  
**PR:** `#40`  
**Status:** `CI RE-RUN REQUIRED ON THIS COMMIT`

## 1. EF migration reviewed

Generated migration:

`TransportERP.Infrastructure/Persistence/Migrations/20260821132015_P2C01CShippingExecution.cs`

Structural review result: `PASS — pending exact-head CI`.

The migration is additive and creates only the C-authorized persistence set:

- `item_releases`
- `trips`
- `trip_stops`
- `trip_allocations`
- `manifests`
- `manifest_lines`
- `movement_events`
- `waybill_holds` as the read-only blocker dependency required by W1-P2C01-026

It does not alter P1, P2-C01-A or P2-C01-B migration files or introduce Arrival, Warehouse, Delivery, Customs, Commission, Settlement, Fleet or GPS tables.

## 2. Reviewed safeguards

- release and allocation quantities are positive and reversal-shaped;
- manifest line loaded quantity cannot exceed planned quantity;
- `MovementEvent` is constrained to C-authorized `LOAD` and `DEPART` event types;
- idempotency indexes exist for release, allocation, trip creation, manifest creation and movement operations;
- Trip number remains unique within company scope;
- W1 allows multiple manifests per trip as long as `TripId + ManifestNo` is unique; C does not invent a one-manifest-only restriction;
- foreign keys to future Fleet runtime are intentionally absent: `VehicleId` and `DriverId` remain reference identifiers per W1-P2C01-010;
- `Down()` removes the C tables in dependency-safe order.

## 3. CI policy

The prior successful workflow generated and pushed the migration, which moved the PR head. The bot-created migration head did not produce usable exact-head CI evidence. This governance commit intentionally retriggers all C gates on a user-authored head that already contains the committed migration.

Required before independent review:

1. closed-contract validator PASS;
2. C phase-boundary PASS;
3. build PASS;
4. non-database regression PASS;
5. EF pending-model check PASS;
6. full migration apply on PostgreSQL 18 PASS;
7. C PostgreSQL/HTTP tests PASS;
8. Desktop RTL PASS.

No independent PASS or merge is valid until all of the above succeed on the exact final head.

## 4. Next-phase lock

`Arrival / Transit / Warehouse MUST NOT START` before P2-C01-C receives exact-head green CI, independent review PASS, and merge to master.
