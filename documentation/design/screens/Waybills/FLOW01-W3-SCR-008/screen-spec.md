# FLOW01-W3-SCR-008 — مانيفست الرحلة — Canonical Screen Specification

**Alias:** `TRIP-002`  
**Profile / Variant:** `Transaction / HeaderLines`  
**CurrentDesignState:** `FIELD_GRID`  
**OwnerTeam:** `TEAM-D03`  
**Batch:** `BATCH-01`

## Authority
- Canonical identity: `CHG-20260818-FLOW01-W3-ID-002`.
- W1: `FreightManifest`, `FreightManifestItem`, `ShipmentTripAllocation`.
- W2: `F01.Manifest.Create`, `F01.Manifest.Update`, `F01.Manifest.Load`, `F01.Manifest.Get`.
- Typed definition: `FLOW01-W3-SCR-008_TYPED_SCREENDEFINITION.md`.
- Design test input: `TAE-F01-007` (issued; runtime PASS not claimed).

## ANALYSIS — TEAM-D01 PASS
Purpose: create/update a freight manifest before dispatch, validate allocations/capacity through authoritative services, and execute the issued load action only when allowed.

### Capabilities
| Capability | Action | Permission | State rule |
|---|---|---|---|
| إنشاء/تعديل | `F01.Manifest.Create/Update` | `f01.manifest.create` / `f01.manifest.edit` | pre-dispatch |
| تحميل | `F01.Manifest.Load` | `f01.manifest.load` | capacity/allocation valid |
| عرض | `F01.Manifest.Get` | `f01.manifest.view` | scoped read |

### Fields
- `tripRef` — الرحلة — Lookup/UUID — required — read-only after create — loadable trip.
- `manifestId` — رقم المانيفست — UUID/read-only — server assigned.
- `manifestLines` — البنود — LineGrid/Collection — required — pre-load only — allocated/available.
- `capacitySummary` — ملخص السعة — Status/Object — read-only/server calculated.
- `loadState` — حالة التحميل — State/Enum — read-only/server state.
- `loadReason` — سبب التحميل — TextArea/String — conditional — required on load.
- `expectedVersion` — Integer hidden/read-only token — update/load.

## LAYOUT — TEAM-D02 PASS
Current shared Transaction profile governs:
- Header/MainData = `Content`.
- Tabs/Workspace = `Fill`.
- ManifestLinesGrid = `Fill`.
- capacity summary/actions/audit = shared CoreUI Content/Fixed regions.
- No LocalException; RTL/DPI/shared styling are CoreUI-owned.

Functional tabs retained: `Lines | Load Check | Audit`.

## FIELD_GRID — TEAM-D03 IN PROGRESS
Typed columns:
`allocationRef, shipmentRef, itemOrPackageRef, allocatedQuantity, loadedQuantity, measurementStatus, exception`.

Known rules:
- `AutoGenerateColumns=false`.
- paging is issued.
- editability ends according to pre-load/load state.
- capacity/measurement result is authoritative; no local recalculation.

Required TEAM-D03 closure: explicit per-column labels/types/required-readonly-edit rules/order/CoreUI width policy/selection/editor presentation and reference lookup UI contracts without inventing providers.

## Non-inventions
No capacity formula, allocation algorithm, API/DTO/permission, DDL, posting/accounting behavior, or offline load authority is created.

## Handoff
- Completed: `ANALYSIS`, `LAYOUT`.
- Current: `FIELD_GRID`.
- Next after PASS: `TEAM-D04 / UX`.
