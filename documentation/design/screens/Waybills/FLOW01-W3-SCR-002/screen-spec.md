# FLOW01-W3-SCR-002 — تخصيص الشحنة — Canonical Screen Specification

**Alias:** `SHP-002`  
**Profile / Variant:** `Transaction / Allocation`  
**CurrentDesignState:** `FIELD_GRID`  
**OwnerTeam:** `TEAM-D03`  
**Batch:** `BATCH-01`

## Authority
- Canonical identity: `CHG-20260818-FLOW01-W3-ID-002`.
- W1: `ShipmentTripAllocation`, `FreightTrip`, `FreightManifestItem`.
- W2: `F01.Allocation.Create`, `F01.Allocation.Release`, `F01.Allocation.List`.
- Typed definition: `FLOW01-W3-SCR-002_TYPED_SCREENDEFINITION.md`.
- Design test input: `TAE-F01-002` (issued; runtime execution not claimed).
- CoreUI/Profile: current `Transaction / Allocation` authority.

## ANALYSIS — TEAM-D01 PASS
Business purpose: allocate confirmed/active shipment items/packages to a loadable freight trip, expose available/capacity status, and support controlled release without inventing operational or accounting effects.

### Capabilities
| Capability | Action | Permission | State rule |
|---|---|---|---|
| تخصيص | `F01.Allocation.Create` | `f01.allocation.create` | Confirmed/Active + loadable trip |
| Release/عكس | `F01.Allocation.Release` | `f01.allocation.release` | controlled; reason required |
| عرض | `F01.Allocation.List` | `f01.allocation.view` | scoped read |

### Fields
- `shipmentRef` — البوليصة — Lookup/UUID — required — read-only after selection — visible scope.
- `tripRef` — رحلة الشحن — Lookup/UUID — required — valid trip state.
- `allocationLines` — عناصر التخصيص — AllocationGrid/Collection — required — no over-allocation.
- `availableQuantity` — المتاح — Decimal — required/read-only — server calculated.
- `capacityStatus` — السعة — Enum/read-only — capacity/measurement authority.
- `releaseReason` — سبب Release — String — conditional — required on release.
- `expectedVersion` — Integer hidden/read-only token — required on change.

## LAYOUT — TEAM-D02 PASS
Current shared CoreUI architecture governs historical sizing shorthand. No LocalException.

`Transaction / Allocation`:
- Header/MainData = `Content`.
- Tabs/Workspace = `Fill`.
- AllocationGrid = `Fill`.
- Totals/status/actions/audit = shared CoreUI Content/Fixed regions as applicable.
- RTL/DPI/spacing/toolbar/grid rendering = CoreUI only.

Functional tabs retained from typed definition: `Available | Allocations | Audit`.

## FIELD_GRID — TEAM-D03 IN PROGRESS
Typed semantic columns:
`itemOrPackageRef, availableQuantity, proposedQuantity, allocatedQuantity, measurementStatus, allocationState`.

Known rules:
- `AutoGenerateColumns=false`.
- server paging is issued.
- allocation may not exceed available quantity.
- sensitive actions remain W2 permission/state bound.

Required TEAM-D03 closure work: explicit per-column labels/types/required-readonly/edit policy/order/CoreUI width policy/editor/selection policy and lookup UI bindings without inventing API provider identifiers.

## Non-inventions
No API route, DTO field, permission, DDL, accounting effect, local capacity formula, or offline-write authority is created here.

## Handoff
- Completed: `ANALYSIS`, `LAYOUT`.
- Current: `FIELD_GRID`.
- Next after PASS: `TEAM-D04 / UX`.
