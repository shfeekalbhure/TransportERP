# FLOW01-W3-SCR-012 — عهدة التحصيل — Canonical Screen Specification

**Alias:** `COD-002`  
**Profile / Variant:** `Transaction / HeaderLines`  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**OwnerTeam:** `TEAM-D06`  
**Batch:** `BATCH-02`

## Authority
- Current FLOW01 owner issuance `SRC-053`; historical generic Variant gate is superseded by current `Transaction / HeaderLines` typed definition.
- W1: `DriverCustody`, `DriverHandover`, `CashCollection`.
- W2: `F01.DriverCustody.Record`, `F01.DriverHandover.Record`, `F01.DriverCustody.Get`.
- Permissions: `f01.cash.custody.record`, `f01.cash.handover.record`, `f01.cash.custody.view`.
- Typed definition: `FLOW01-W3-SCR-012_TYPED_SCREENDEFINITION.md`.
- Design test input: `TAE-F01-011` (issued; runtime not run).
- FIELD_GRID authority: `documentation/design/decisions/2026-08-24_BATCH-02_FIELD_GRID_DESIGN_AUTHORITY_DECISION.md`.

## ANALYSIS — TEAM-D01 PASS
Purpose: record driver custody from eligible linked collections, record a distinct acknowledged handover, and display custody balances/state inside authorized scope.

Fields:
- `custodyId` — رقم العهدة — UUID/read-only/server assigned.
- `driverRef` — السائق — Lookup/Reference — required/scope valid.
- `collectionRefs` — التحصيلات — LookupMulti/UUID Collection — required/eligible unsettled.
- `custodyAmount` — مبلغ العهدة — Decimal Currency — required/read-only/server calculated.
- `handoverRecipientRef` — مستلم التوريد — Lookup/Reference — conditional on handover.
- `handoverAt` — وقت التوريد — Instant — conditional/audited.
- `handoverReason` — سبب التوريد — String — conditional/required on handover.

## LAYOUT — TEAM-D02 PASS
Shared `Transaction / HeaderLines` authority:
- Header/MainData = `Content`.
- Tabs/Workspace = `Fill`.
- CustodyLinesGrid = `Fill`.
- actions/audit/validation/loading/error = shared CoreUI.
- tabs: `Custody | Collections | Handovers | Audit`.
- no LocalException/local styling.

## FIELD_GRID — TEAM-D03 PASS
The selected collections are controlled from the issued `collectionRefs` input; the custody grid is a server-authoritative display of linked collection state. `GridProfile=Display`, `AutoGenerateColumns=false`, `UsesServerPaging=true`, `SelectionPolicy=SingleRow`.

| # | Key | Arabic Header | UI Type | Edit policy | Width policy |
|---:|---|---|---|---|---|
| 1 | `collectionRef` | التحصيل | Reference | ReadOnly | primary Fill |
| 2 | `currency` | العملة | Reference | ReadOnly | content/reference |
| 3 | `amount` | المبلغ | Decimal | ReadOnly | content numeric |
| 4 | `collectedAt` | وقت التحصيل | Instant | ReadOnly | content datetime |
| 5 | `custodyState` | حالة العهدة | Enum | ReadOnly | content state |
| 6 | `handoverState` | حالة التوريد | Enum | ReadOnly | content state |

Provider/sort identifiers remain `TBD-GATED`; no custody calculation is invented.

## UX — TEAM-D04 PASS
- custody record requires eligible linked collections, driver scope and issued permission.
- displayed custody amount is server-calculated; client does not recompute authoritative balance.
- handover requires the issued recipient acknowledgement/context; no silent handover.
- while recording custody/handover, loading prevents duplicate mutation.
- server response becomes authoritative custody/handover state.
- permission/scope denial reveals no hidden collection/custody data.
- all writes online-only; no offline cash custody queue.

## VISUAL — TEAM-D05 PASS
Shared Transaction/CoreUI tokens only: RTL/DPI, central currency/read-only/lookup states, grid/tabs/audit/validation/loading/error. No local balance color, raw sizes or toolbar clone.

## Acceptance criteria
1. six explicit read-only custody grid columns.
2. custody amount is server-calculated.
3. handover requires distinct issued acknowledgement/context.
4. no API/DTO/Permission/DDL/offline/accounting invention.

## Handoff
Completed: `ANALYSIS`, `LAYOUT`, `FIELD_GRID`, `UX`, `VISUAL`.  
Current: `INDEPENDENT_REVIEW` — `TEAM-D06`.
