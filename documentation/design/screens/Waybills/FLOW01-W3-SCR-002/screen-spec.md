# FLOW01-W3-SCR-002 — تخصيص الشحنة — Canonical Screen Specification

**Alias:** `SHP-002`  
**Profile / Variant:** `Transaction / Allocation`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**Batch:** `BATCH-01`

## Authority
- Canonical identity: `CHG-20260818-FLOW01-W3-ID-002`.
- W1: `ShipmentTripAllocation`, `FreightTrip`, `FreightManifestItem`.
- W2: `F01.Allocation.Create`, `F01.Allocation.Release`, `F01.Allocation.List`.
- Permissions: `f01.allocation.create`, `f01.allocation.release`, `f01.allocation.view`.
- Typed definition: `FLOW01-W3-SCR-002_TYPED_SCREENDEFINITION.md`.
- Design test input: `TAE-F01-002` (issued; runtime PASS not claimed).
- FIELD_GRID authority: `documentation/design/decisions/2026-08-24_BATCH-01_FIELD_GRID_DESIGN_AUTHORITY_DECISION.md`.

## ANALYSIS — TEAM-D01 PASS
Purpose: allocate confirmed/active shipment items/packages to a loadable freight trip, expose server-authoritative availability/capacity state, and support controlled release.

Fields:
- `shipmentRef` — البوليصة — Lookup/UUID — required — read-only after selection.
- `tripRef` — رحلة الشحن — Lookup/UUID — required — valid target trip state.
- `allocationLines` — عناصر التخصيص — AllocationGrid/Collection — required.
- `availableQuantity` — المتاح — Decimal — read-only/server calculated.
- `capacityStatus` — السعة — Enum/read-only — server/domain result.
- `releaseReason` — سبب Release — String — conditional; required on release.
- `expectedVersion` — hidden Integer/read-only token — required on protected change.

## LAYOUT — TEAM-D02 PASS
Shared `Transaction / Allocation` authority governs:
- Header/MainData = `Content`.
- Tabs/Workspace = `Fill`.
- AllocationGrid = `Fill`.
- Shared actions/status/audit regions = CoreUI Content/Fixed as applicable.
- Tabs: `Available | Allocations | Audit`.
- No LocalException; no local sizing/style.

## FIELD_GRID — TEAM-D03 PASS
`GridProfile=TransactionLines`, `AutoGenerateColumns=false`, `UsesServerPaging=true`, `SelectionPolicy=SingleRow`.

| # | Key | Arabic Header | UI Type | Required | Edit policy | Width policy | Editor |
|---:|---|---|---|---|---|---|---|
| 1 | `itemOrPackageRef` | الصنف/الطرد | Reference | Yes for new allocation line | editable in allocation input context | primary Fill | `TransportLookup`; provider `TBD-GATED` |
| 2 | `availableQuantity` | المتاح | Decimal | No input | ReadOnly | content numeric | display |
| 3 | `proposedQuantity` | الكمية المقترحة | Decimal | Yes for quantity allocation | editable before submit | content numeric | numeric editor |
| 4 | `allocatedQuantity` | المخصص | Decimal | No input | ReadOnly | content numeric | display |
| 5 | `measurementStatus` | حالة القياس | Enum | No input | ReadOnly | content state | state display |
| 6 | `allocationState` | حالة التخصيص | Enum | No input | ReadOnly | content state | state display |

Rules:
- UI may present quantities but does not calculate authoritative available quantity or capacity.
- over-allocation remains server/domain validation.
- exact lookup provider and server sort-key identifiers remain `TBD-GATED` and nonblocking.
- no bulk capability exists; multi-selection is not introduced.

## UX — TEAM-D04 PASS
- Create is enabled only when capability/permission/state allow and required allocation input is present.
- Release requires the issued release capability and a nonempty reason; no local reversal semantics are invented.
- `TransportLoadingState` disables conflicting state-changing commands and prevents UI double-submit.
- `TransportValidationPresenter` renders field/summary errors; no local MessageBox validation path.
- concurrency conflict uses shared Refresh/Reload behavior; no silent overwrite or local merge.
- lookups are server-side, debounced, Id-bound, scope filtered, max 50 results; exact provider identifiers remain gated.
- server result replaces local availability/capacity/allocation state after any command.
- no offline write/queue behavior is exposed.

## VISUAL — TEAM-D05 PASS
- CoreUI typography, semantic colors, spacing/sizing, RTL, DPI, tabs, grid, toolbar, validation/loading/error/audit only.
- required/read-only/focus/error states use shared semantic tokens.
- no raw color, local font, fixed pixel width/height, or local toolbar/grid styling.
- Arabic labels must not clip across governed DPI scales.

## Independent review — TEAM-D06 PASS
- Review artifact: `documentation/design/batches/BATCH-01_INDEPENDENT_REVIEW_2026-08-24.md`.
- Open design findings: `0`.
- Runtime test `TAE-F01-002`: issued, not run; DESIGN APPROVAL does not claim runtime PASS.

## Acceptance criteria
1. `Transaction / Allocation` only; no local profile/variant invention.
2. Header Content; workspace/grid Fill; no LocalException.
3. explicit six grid columns; AutoGenerate=false; server paging.
4. Create/Release/View remain W2 permission/state bound.
5. no client capacity or availability authority.
6. release reason is required by the issued release contract.
7. no invented API/DTO/Permission/DDL/offline behavior.
8. unresolved provider/sort identifiers remain technical nonblocking gates.

## Handoff
- Completed: `ANALYSIS`, `LAYOUT`, `FIELD_GRID`, `UX`, `VISUAL`, `INDEPENDENT_REVIEW`.
- Final: `DESIGN_APPROVED`.
