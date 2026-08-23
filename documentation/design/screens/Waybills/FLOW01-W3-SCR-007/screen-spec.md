# FLOW01-W3-SCR-007 — رحلة الشحن — Canonical Screen Specification

**Alias:** `TRIP-001`  
**Profile / Variant:** `Transaction / HeaderLines`  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**OwnerTeam:** `TEAM-D06`  
**Batch:** `BATCH-01`

## Authority
- Canonical identity: `CHG-20260818-FLOW01-W3-ID-002`.
- W1: `FreightTrip`.
- W2: `F01.FreightTrip.Create`, `F01.FreightTrip.UpdateDraft`, `F01.FreightTrip.Transition`, `F01.FreightTrip.Get/Search`.
- Permissions: `f01.trip.create`, `f01.trip.edit`, `f01.trip.transition`, `f01.trip.view`.
- Typed definition: `FLOW01-W3-SCR-007_TYPED_SCREENDEFINITION.md`.
- Design test input: `TAE-F01-006` (issued; runtime PASS not claimed).
- FIELD_GRID authority: `documentation/design/decisions/2026-08-24_BATCH-01_FIELD_GRID_DESIGN_AUTHORITY_DECISION.md`.

## ANALYSIS — TEAM-D01 PASS
Purpose: create/edit a freight trip Draft, expose scoped trip data, and request only legal lifecycle transitions through the issued transition capability.

Fields:
- `tripId` — رقم الرحلة — UUID/read-only — server assigned.
- `vehicleRef` — المركبة — Lookup/Reference — required — Draft only.
- `driverRef` — السائق — Lookup/Reference — conditional — Draft only.
- `originRef` — المصدر — Lookup/Reference — required — Draft only.
- `destinationRef` — الوجهة — Lookup/Reference — required — Draft only.
- `scheduledAt` — الموعد — DateTime/Instant — Draft only.
- `tripState` — الحالة — Enum/read-only/server state.
- `targetState` — الانتقال المطلوب — Enum lookup — conditional; legal edge only.
- `expectedVersion` — hidden Integer/read-only token — update/transition.

## LAYOUT — TEAM-D02 PASS
Shared `Transaction / HeaderLines` authority:
- Header/MainData = `Content`.
- Tabs/Workspace = `Fill`.
- TripAllocationGrid = `Fill`.
- shared actions/audit/presenters = CoreUI.
- tabs: `General | Load | Route | Audit`.
- no LocalException; central RTL/DPI/sizing only.

## FIELD_GRID — TEAM-D03 PASS
`GridProfile=Display`, `AutoGenerateColumns=false`, `UsesServerPaging=true`, `SelectionPolicy=SingleRow`. This screen has no allocation-edit capability, so the allocation grid is read-only display.

| # | Key | Arabic Header | UI Type | Edit | Width policy |
|---:|---|---|---|---|---|
| 1 | `allocationRef` | مرجع التخصيص | Reference | ReadOnly | content/reference |
| 2 | `shipmentRef` | البوليصة | Reference | ReadOnly | content/reference |
| 3 | `quantityOrPackage` | الكمية/الطرد | String display | ReadOnly | primary Fill |
| 4 | `loadState` | حالة التحميل | Enum | ReadOnly | content state |
| 5 | `capacityContribution` | مساهمة السعة | Decimal | ReadOnly | content numeric |

Exact provider/sort identifiers remain `TBD-GATED`; no allocation mutation is invented inside the trip screen.

## UX — TEAM-D04 PASS
- Draft fields are editable only under the issued edit permission/state.
- transition action is enabled only under `f01.trip.transition` and current server state; the client does not derive or persist a lifecycle graph.
- target-state choices must be limited to server/contract-authorized legal edges; no unsupported transition is shown as executable.
- `expectedVersion` is required for protected update/transition; conflict uses shared Reload/Refresh and never overwrites silently.
- loading state prevents duplicate submit and conflicting mutations.
- capacity contribution/allocation state are display-only server results; no local capacity formula.
- lookups are shared server-side/debounced/Id-bound/scope-authoritative; provider identifiers may remain gated.
- all issued FLOW01 actions are online-only; no offline transition/queue is exposed.

## VISUAL — TEAM-D05 PASS
- shared Transaction CoreUI tokens only: central typography, spacing, state colors, toolbar, grids, tabs, RTL, DPI, validation/loading/error/audit.
- no local route/capacity graphics, fixed sizes, raw colors or custom lifecycle styling.

## Acceptance criteria
1. create/edit/transition/view are permission/state bound exactly as issued.
2. client does not invent legal transition edges or capacity formulas.
3. explicit five-column read-only allocation grid with server paging.
4. Draft-only editability and concurrency token are preserved.
5. no invented API/DTO/Permission/DDL/offline behavior.

## Handoff
- Completed: `ANALYSIS`, `LAYOUT`, `FIELD_GRID`, `UX`, `VISUAL`.
- Current: `INDEPENDENT_REVIEW`.
- Reviewer: `TEAM-D06`.
