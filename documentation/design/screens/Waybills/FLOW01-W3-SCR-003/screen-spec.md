# FLOW01-W3-SCR-003 — تتبع البوليصة — Canonical Screen Specification

**Alias:** `SHP-003`  
**Profile / Variant:** `ReportInquiry / Inquiry`  
**CurrentDesignState:** `FIELD_GRID`  
**OwnerTeam:** `TEAM-D03`  
**Batch:** `BATCH-01`

## Authority
- Canonical identity: `CHG-20260818-FLOW01-W3-ID-002`.
- W1: shipment status/event/current custody read model.
- W2: `F01.Shipment.Tracking.Get`.
- Permission: `f01.shipment.tracking.view`.
- Typed definition: `FLOW01-W3-SCR-003_TYPED_SCREENDEFINITION.md`.
- Design test input: `TAE-F01-003` (issued; runtime PASS not claimed).

## ANALYSIS — TEAM-D01 PASS
Purpose: scoped read-only shipment tracking over server-authoritative current state, custody and ordered tracking events.

### Fields / filters / results
- `shipmentRef` — البوليصة — Lookup/UUID — required query target — visible scope.
- `currentState` — الحالة الحالية — Enum/read-only — server result.
- `custodyOwner` — الحيازة الحالية — Reference/read-only — server result.
- `eventRange` — الفترة — DateRange — optional typed filter.
- `eventState` — حالة الحدث — Enum lookup/filter.
- `trackingEvents` — سجل التتبع — ReadGrid/Collection — immutable result view.

No client company/branch scope field is authorized.

## LAYOUT — TEAM-D02 PASS
Current `ReportInquiry` profile governs:
- Filters = `Content`.
- Summary = `Content`.
- ResultsGrid = `Fill / ReadOnly`.
- Pagination = shared `Fixed` region.
- RTL/DPI/spacing/error/loading/empty states = CoreUI.
- No LocalException.

The typed historical `Filters(Fixed)` shorthand is not promoted over the approved shared profile.

## FIELD_GRID — TEAM-D03 IN PROGRESS
Typed result columns:
`occurredAt, eventType, priorState, currentState, sourceCustody, targetCustody, actorDisplay, reason`.

Known rules:
- `AutoGenerateColumns=false`.
- cursor paging is issued.
- results are immutable/read-only.
- filters bind only to the issued tracking query contract.

Required TEAM-D03 closure: explicit per-column labels/types/order/CoreUI width policy, read-only contract and typed filter presentation; no endpoint/provider identifier may be invented.

## Non-inventions
No write command, lifecycle transition, hidden-data inference, API route, DTO, permission, DDL or offline behavior is created by this design.

## Handoff
- Completed: `ANALYSIS`, `LAYOUT`.
- Current: `FIELD_GRID`.
- Next after PASS: `TEAM-D04 / UX`.
