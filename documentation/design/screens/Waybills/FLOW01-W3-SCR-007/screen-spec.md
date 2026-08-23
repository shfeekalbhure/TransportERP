# FLOW01-W3-SCR-007 — رحلة الشحن — Canonical Screen Specification

**Alias:** `TRIP-001`  
**Profile / Variant:** `Transaction / HeaderLines`  
**CurrentDesignState:** `FIELD_GRID`  
**OwnerTeam:** `TEAM-D03`  
**Batch:** `BATCH-01`

## Authority
- Canonical identity: `CHG-20260818-FLOW01-W3-ID-002`.
- W1: `FreightTrip`.
- W2: `F01.FreightTrip.Create`, `F01.FreightTrip.UpdateDraft`, `F01.FreightTrip.Transition`, `F01.FreightTrip.Get/Search`.
- Typed definition: `FLOW01-W3-SCR-007_TYPED_SCREENDEFINITION.md`.
- Design test input: `TAE-F01-006` (issued; runtime PASS not claimed).

## ANALYSIS — TEAM-D01 PASS
Purpose: create/edit a freight trip Draft, expose scoped trip data, and request only legal lifecycle transitions through the issued transition capability.

### Capabilities
| Capability | Action | Permission | State rule |
|---|---|---|---|
| إنشاء | `F01.FreightTrip.Create` | `f01.trip.create` | new Draft |
| تعديل | `F01.FreightTrip.UpdateDraft` | `f01.trip.edit` | Draft + expectedVersion |
| انتقال حالة | `F01.FreightTrip.Transition` | `f01.trip.transition` | legal lifecycle edge only |
| عرض/بحث | `F01.FreightTrip.Get/Search` | `f01.trip.view` | scoped read |

### Fields
- `tripId` — رقم الرحلة — UUID/read-only — server assigned.
- `vehicleRef` — المركبة — Lookup/Reference — required — Draft only — scope valid.
- `driverRef` — السائق — Lookup/Reference — conditional — Draft only — scope valid.
- `originRef` — المصدر — Lookup/Reference — required — Draft only.
- `destinationRef` — الوجهة — Lookup/Reference — required — Draft only.
- `scheduledAt` — الموعد — DateTime/Instant — Draft only — date validation.
- `tripState` — الحالة — Enum/read-only — server state.
- `targetState` — الانتقال المطلوب — Enum lookup — conditional — legal edge only.
- `expectedVersion` — Integer hidden/read-only token — required on update/transition.

## LAYOUT — TEAM-D02 PASS
Current shared Transaction profile governs:
- Header/MainData = `Content`.
- Tabs/Workspace = `Fill`.
- TripAllocationGrid = `Fill`.
- Actions/Audit/shared presenters = CoreUI.
- No LocalException; RTL/DPI/shared sizing remain central.

Functional tabs retained: `General | Load | Route | Audit`.

## FIELD_GRID — TEAM-D03 IN PROGRESS
Typed columns:
`allocationRef, shipmentRef, quantityOrPackage, loadState, capacityContribution`.

Known rules:
- `AutoGenerateColumns=false`.
- server paging is issued.
- state transition remains permission + legal-edge bound.
- capacity/business calculations are not recreated in the client.

Required TEAM-D03 closure: explicit column labels/types/read-only-edit rules/order/CoreUI width policy/selection/editor presentation and lookup UI contracts without inventing providers.

## Non-inventions
No lifecycle graph, capacity formula, route formula, API/DTO/permission, DDL, or offline transition authority is created.

## Handoff
- Completed: `ANALYSIS`, `LAYOUT`.
- Current: `FIELD_GRID`.
- Next after PASS: `TEAM-D04 / UX`.
