# FLOW01-W3-SCR-008 — مانيفست الرحلة — Canonical Screen Specification

**Alias:** `TRIP-002`  
**Profile / Variant:** `Transaction / HeaderLines`  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**OwnerTeam:** `TEAM-D06`  
**Batch:** `BATCH-01`

## Authority
- Canonical identity: `CHG-20260818-FLOW01-W3-ID-002`.
- W1: `FreightManifest`, `FreightManifestItem`, `ShipmentTripAllocation`.
- W2: `F01.Manifest.Create`, `F01.Manifest.Update`, `F01.Manifest.Load`, `F01.Manifest.Get`.
- Permissions: `f01.manifest.create`, `f01.manifest.edit`, `f01.manifest.load`, `f01.manifest.view`.
- Typed definition: `FLOW01-W3-SCR-008_TYPED_SCREENDEFINITION.md`.
- Design test input: `TAE-F01-007` (issued; runtime PASS not claimed).
- FIELD_GRID authority: `documentation/design/decisions/2026-08-24_BATCH-01_FIELD_GRID_DESIGN_AUTHORITY_DECISION.md`.

## ANALYSIS — TEAM-D01 PASS
Purpose: create/update a freight manifest before dispatch, validate allocations/capacity through authoritative services, and execute the issued load action only when allowed.

Fields:
- `tripRef` — الرحلة — Lookup/UUID — required — read-only after create.
- `manifestId` — رقم المانيفست — UUID/read-only — server assigned.
- `manifestLines` — البنود — LineGrid/Collection — required — pre-load only.
- `capacitySummary` — ملخص السعة — Status/Object — read-only/server calculated.
- `loadState` — حالة التحميل — State/Enum — read-only/server state.
- `loadReason` — سبب التحميل — TextArea/String — conditional — required on load.
- `expectedVersion` — hidden Integer/read-only token — update/load.

## LAYOUT — TEAM-D02 PASS
Shared `Transaction / HeaderLines` authority:
- Header/MainData = `Content`.
- Tabs/Workspace = `Fill`.
- ManifestLinesGrid = `Fill`.
- capacity summary/actions/audit = shared CoreUI Content/Fixed regions.
- tabs: `Lines | Load Check | Audit`.
- no LocalException; central RTL/DPI/shared styling only.

## FIELD_GRID — TEAM-D03 PASS
`GridProfile=TransactionLines`, `AutoGenerateColumns=false`, `UsesServerPaging=true` as issued, `SelectionPolicy=SingleRow`.

| # | Key | Arabic Header | UI Type | Required | Edit policy | Width policy | Editor |
|---:|---|---|---|---|---|---|---|
| 1 | `allocationRef` | مرجع التخصيص | Reference | Yes for included line | ReadOnly | content/reference | display |
| 2 | `shipmentRef` | البوليصة | Reference | Yes for included line | ReadOnly | content/reference | display |
| 3 | `itemOrPackageRef` | الصنف/الطرد | Reference | Yes for included line | ReadOnly | primary Fill | display |
| 4 | `allocatedQuantity` | الكمية المخصصة | Decimal | No input | ReadOnly | content numeric | display |
| 5 | `loadedQuantity` | الكمية المحملة | Decimal | Yes when quantity-based load is entered | editable pre-load only | content numeric | numeric editor |
| 6 | `measurementStatus` | حالة القياس | Enum | No input | ReadOnly | content state | state display |
| 7 | `exception` | الاستثناء | String | No input | ReadOnly | primary Fill | display |

Rules:
- editability ends when the issued pre-load/load state no longer allows manifest update.
- capacity/measurement/allocation validity remains server/domain authority; no client recalculation.
- exact provider/sort identifiers remain `TBD-GATED` and nonblocking.

## UX — TEAM-D04 PASS
- create/update require issued permissions and trip/pre-dispatch state.
- Load is available only with `f01.manifest.load`, valid server state, and required reason where the contract requires it.
- UI does not infer capacity validity; it renders server capacity/measurement state and returned validation errors.
- loading prevents double-submit/conflicting mutation; server idempotency remains authoritative.
- stale version uses shared conflict + Reload/Refresh, never silent overwrite.
- after successful load, returned manifest state/resource replaces local values and pre-load editors become read-only when required.
- no local dispatch/posting/accounting side effect is implied.
- writes remain online-authoritative only.

## VISUAL — TEAM-D05 PASS
- shared Transaction CoreUI only: toolbar, tabs, line grid, semantic Required/ReadOnly/Error/Focus states, capacity status presentation, validation/loading/error/audit, RTL and DPI.
- no local capacity colors/formulas, raw styling, fixed pixel sizes or custom grid rendering.

## Acceptance criteria
1. manifest actions remain permission/state bound exactly as issued.
2. explicit seven-column grid; AutoGenerate=false; server paging.
3. only `loadedQuantity` is an editable line value where pre-load contract permits; authoritative allocation/capacity fields stay read-only.
4. no client capacity/measurement algorithm.
5. no invented API/DTO/Permission/DDL/posting/offline behavior.

## Handoff
- Completed: `ANALYSIS`, `LAYOUT`, `FIELD_GRID`, `UX`, `VISUAL`.
- Current: `INDEPENDENT_REVIEW`.
- Reviewer: `TEAM-D06`.
