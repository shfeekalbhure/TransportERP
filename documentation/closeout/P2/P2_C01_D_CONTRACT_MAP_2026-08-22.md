# P2-C01-D — W1 / W2 / W3 Contract Map

Date: 2026-08-22
Baseline: `master@5d58a42046e07166e6db76bcb893f32b1d8f2ec7`
Phase: `P2-C01-D — Arrival / Transit / Warehouse`

Effective-contract precedence: closed base registers + supplements, with RR1 overrides replacing the same W2/W3 IDs where present.

## Capability map

| Capability | W1 authority | Effective W2 | Effective W3 | Core invariant |
|---|---|---|---|---|
| Record destination/stop arrival | 010 Trip; 011 TripStop; 015 MovementEvent; 016 ArrivalReceipt; 017 ArrivalReceiptLine | 022 RecordArrival | 026 SHP-031 RR1; 027 SHP-032; 029 SHP-034 RR1 | Departed trip + valid receiving location; retry creates one logical arrival |
| Partial/full unload | 014 ManifestLine; 015 MovementEvent; 017 ArrivalReceiptLine; 018 WarehouseHolding | 023 RecordUnload | 027 SHP-032; 028 SHP-033; 029 SHP-034 RR1 | unload <= current in-transit; movement + holding update atomic |
| Transit reallocation | 012 TripAllocation; 015 MovementEvent; 018 WarehouseHolding; 026 hold blocker dependency | 024 ReallocateTransitQuantity | 027 SHP-032 | allocate <= AVAILABLE holding; route compatible; previous trip history preserved |
| Finalize receiving receipt | 016 ArrivalReceipt; 017 ArrivalReceiptLine; 018 WarehouseHolding | 040 FinalizeArrivalReceipt RR1 | 029 SHP-034 RR1 | all lines validated/evidence complete; partial receipt never implies false full arrival |
| Operational trip close | 010 Trip; 013 Manifest; 014 ManifestLine; 015 MovementEvent; 025 exception blocker dependency | 041 CloseTrip RR1 | 026 SHP-031 RR1 | ARRIVED -> CLOSED only after custody reconciliation and blockers cleared |
| Waybill movement inquiry | 002 Waybill; 015 MovementEvent | 035 GetWaybillMovement | 014 SHP-018 | immutable chronological ledger timeline; read-only |
| Item movement inquiry | 004 WaybillItem; 015 MovementEvent | 036 GetItemMovement | 013 SHP-017 | original/released/allocated/loaded/arrived/delivered/remaining derived from evidence |

## W1 details

### W1-P2C01-010 Trip
D consumes the existing C Trip record and governs only the post-departure lifecycle needed here: actual arrival and operational closure. `RowVersion`/version semantics remain mandatory for stale-writer protection.

### W1-P2C01-011 TripStop
Intermediate stops retain unique sequence and location identity. D may record actual arrival/departure status according to the governed stop lifecycle; planned routing remains historical context.

### W1-P2C01-015 MovementEvent
This remains the authoritative operational history. D extends accepted history with arrival/unload/transit-reallocation evidence but must not update/delete previously accepted LOAD/DEPART or any D event. Corrections use compensating evidence only where an effective action contract permits it.

### W1-P2C01-016 ArrivalReceipt
Fields: `Id; TripId; ManifestId; LocationId; ReceivedAt; ReceivedBy; Status; RowVersion`.
Scope: receiving branch/location. Lifecycle: `DRAFT -> FINALIZED`.

### W1-P2C01-017 ArrivalReceiptLine
Fields from the closed contract: `Id; ArrivalReceiptId; ManifestLineId; WaybillItemId; ExpectedQty; ActualQty; DifferenceType; DamageQty; Notes`.
It records expected-vs-actual physical receipt evidence and becomes immutable through the finalized parent receipt.

### W1-P2C01-018 WarehouseHolding
Location-scoped operational balance/projection for transit/destination quantities. It never replaces MovementEvent as history. Direct CRUD/manual quantity editing is not an authorized D capability.

### Read-only blocker dependencies
- `W1-P2C01-025 ShipmentException`: close-trip blocker only; D exposes no ResolveShipmentException command.
- `W1-P2C01-026 WaybillHold`: W2-024 may return HOLD_BLOCKED; D exposes no hold/release-hold command.

## W2 exact action contract

### W2-P2C01-022 — RecordArrival
- `POST /api/v1/trips/{tripId}/arrivals`
- Request: `RecordArrivalRequest`
- Response: `ArrivalReceiptResponse`
- Permission: `arrival.record`
- Scope: `company/receiving-branch`
- Preconditions: Trip departed; destination/stop valid.
- Errors: `NOT_FOUND | INVALID_STATE | LOCATION_INVALID | DUPLICATE_OPERATION`.
- Idempotency: `ClientOperationId`.
- Offline: `OFFLINE_QUEUE_ALLOWED_SERVER_ACCEPTED`.

### W2-P2C01-023 — RecordUnload
- `POST /api/v1/arrivals/{arrivalId}/lines:unload`
- Request: `RecordUnloadRequest`
- Response: `ArrivalReceiptResponse`
- Permission: `arrival.unload`
- Preconditions: arrival open; actual quantity <= in-transit quantity.
- Result: ARRIVE/UNLOAD evidence appended and WarehouseHolding updated.
- Errors: `NOT_FOUND | QUANTITY_EXCEEDS_IN_TRANSIT | INVALID_STATE`.
- Idempotency: `ClientOperationId`.
- Concurrency: serialized physical quantity check.
- Offline: `OFFLINE_QUEUE_ALLOWED_SERVER_ACCEPTED`.

### W2-P2C01-024 — ReallocateTransitQuantity
- `POST /api/v1/holdings/{holdingId}:allocate`
- Request: `ReallocateTransitRequest`
- Response: `AllocationResponse`
- Permission: `waybill.reallocate`
- Preconditions: holding AVAILABLE at transit location; compatible next trip.
- Errors: `NOT_FOUND | QUANTITY_EXCEEDS_AVAILABLE | ROUTE_INCOMPATIBLE | HOLD_BLOCKED`.
- Idempotency: `ClientOperationId`.
- Concurrency: serialized holding balance.
- Offline: `ONLINE_REQUIRED`.

### W2-P2C01-040 — FinalizeArrivalReceipt (RR1)
- `POST /api/v1/arrivals/{arrivalId}:finalize`
- Request: `FinalizeArrivalRequest`
- Response: `ArrivalReceiptResponse`
- Permission: `arrival.finalize`
- Scope: `company/receiving-branch`
- Preconditions: DRAFT receipt; all expected lines validated; required evidence present.
- Transition: `DRAFT -> FINALIZED`.
- Errors: `NOT_FOUND | INVALID_STATE | UNVALIDATED_LINES | DIFFERENCE_REQUIRES_EVIDENCE | CONCURRENCY_CONFLICT`.
- Idempotency: `ClientOperationId`.
- Concurrency: ArrivalReceipt RowVersion.
- Offline: `ONLINE_REQUIRED`.

### W2-P2C01-041 — CloseTrip (RR1)
- `POST /api/v1/trips/{tripId}:close`
- Request: `CloseTripRequest`
- Response: `TripResponse`
- Permission: `trip.close`
- Scope: `company/branch`
- Preconditions: Trip ARRIVED; all manifest custody quantities accounted by arrival/unload/transfer/return/approved exception; no open driver custody.
- Transition: `ARRIVED -> CLOSED`.
- Errors: `NOT_FOUND | INVALID_STATE | CARGO_UNACCOUNTED | CUSTODY_OPEN | EXCEPTION_BLOCKED | CONCURRENCY_CONFLICT`.
- Idempotency: `Idempotency-Key required`.
- Concurrency: Trip RowVersion + serialized custody check.
- Offline: `ONLINE_REQUIRED`.

### W2-P2C01-035 — GetWaybillMovement
- `GET /api/v1/waybills/{waybillId}/movement`
- Permission: `waybill.movement.view`
- Scope: `company/branch`
- Errors: `NOT_FOUND | SCOPE_DENIED | INVALID_FILTER`.
- Read-only safe retry; timeline across all trips/locations.

### W2-P2C01-036 — GetItemMovement
- `GET /api/v1/waybills/{waybillId}/items/{itemId}/movement`
- Permission: `waybill.item.movement.view`
- Scope: `company/branch`
- Errors: `NOT_FOUND | SCOPE_DENIED | INVALID_FILTER`.
- Response must derive original/released/allocated/loaded/arrived/delivered/remaining without manual overrides.

## W3 exact screen contract

### W3-P2C01-013 — SHP-017 حركة الصنف
Fields: `Waybill; Item; Original; Released; Allocated; Loaded; Arrived; Delivered; Remaining; Timeline; Trips; Locations`.
Action: W2-036. Read-only, RTL timeline/numeric summary, source-ID drilldown.

### W3-P2C01-014 — SHP-018 حركة البوليصة
Fields: `Waybill; OperationalStatus; FinancialStatus; Timeline; EventType; DateTime; Trip; From; To; User; Reason`.
Action: W2-035. Read-only chronological RTL timeline.

### W3-P2C01-026 RR1 — SHP-031 تتبع وإغلاق الرحلة
Fields: `Trip; Status; Stops; LastOperationalEvent; WaybillCount; Manifest; CustodyBalance; OpenExceptions; ETAPlaceholder`.
Actions: W2-022, W2-035, W2-041.
Permissions: `trip.view; waybill.movement.view; trip.close`.
Trip closes only after custody quantities are accounted and blockers are cleared. Custody balance/blockers must be textual; GPS remains a separate future source.

### W3-P2C01-027 — SHP-032 محطة وسيطة وترانزيت
Fields: `Trip; Stop; Waybill; Item; Expected; Actual; Holding; NextTrip; Status`.
Actions: W2-022, W2-023, W2-024.
Permissions: `arrival.record; arrival.unload; waybill.reallocate`.

### W3-P2C01-028 — SHP-033 تفريغ جزئي
Fields: `ManifestLine; ExpectedQty; UnloadQty; RemainingInTransit; DifferenceType; DamageQty; Notes`.
Action: W2-023. Server-accepted offline queue is allowed; remaining quantity must stay explicit.

### W3-P2C01-029 RR1 — SHP-034 استلام واعتماد فرع الوصول
Fields: `Trip; Manifest; Location; ReceivedAt; Waybill; Item; Expected; Actual; Difference; Evidence; ReceiptStatus`.
Actions: W2-022, W2-023, W2-040.
Permissions: `arrival.record; arrival.unload; arrival.finalize`.
Finalization blockers and expected-vs-actual evidence must be explicit in Arabic RTL.

## Required D tests mapped to contracts

- Full arrival; partial arrival; shortage; damage/evidence; invalid stop/location.
- unload > in-transit rejection.
- finalize with unvalidated line/evidence missing rejection.
- valid partial finalization preserves remaining-in-transit state.
- reallocate <= available; over-available and route-incompatible rejection.
- idempotent duplicate/offline retry for arrival/unload; immutable operation fingerprints for finalization/close.
- true concurrent unload and holding-allocation races with persisted-invariant verification.
- same-company cross-branch and cross-company negative access tests.
- PostgreSQL raw UPDATE/DELETE rejection for accepted D MovementEvent history.
- atomic movement + holding projection persistence.
- trip-close blockers for unaccounted cargo/open custody/open exception.
- movement inquiries reconstruct C + D history and balance totals.
- Desktop RTL/W3 contract tests for all six D screens.
- negative phase-boundary checks proving Delivery/POD/COD and later domains remain absent.
