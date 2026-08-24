# FLOW01-W3-SCR-011 — تسجيل التحصيل — Canonical Screen Specification

**Alias:** `COD-001`  
**Profile / Variant:** `Transaction / HeaderLines`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**Batch:** `BATCH-02`

## Authority
- Current FLOW01 owner issuance `SRC-053`; historical generic Variant gate is superseded by current `Transaction / HeaderLines` typed definition.
- W1: `CashCollection`, `CollectionReference`, `CommercialCharge` reference only.
- W2: `F01.CashCollection.Record`, `F01.CashCollection.Get/Search`.
- Permissions: `f01.cash.collection.record`, `f01.cash.collection.view`.
- Typed definition: `FLOW01-W3-SCR-011_TYPED_SCREENDEFINITION.md`.
- Design test input: `TAE-F01-010` (issued; runtime not run).
- FIELD_GRID authority: `documentation/design/decisions/2026-08-24_BATCH-02_FIELD_GRID_DESIGN_AUTHORITY_DECISION.md`.

## ANALYSIS — TEAM-D01 PASS
Purpose: record a scoped collection event against an eligible shipment/delivery reference in one currency, preserve audit context, and expose collection/reference state without posting accounting entries.

Fields:
- `collectionId` — رقم التحصيل — UUID/read-only/server assigned.
- `shipmentOrDeliveryRef` — مرجع الشحنة/التسليم — Lookup/Reference — required/eligible.
- `currencyRef` — العملة — Lookup/Reference — required/single currency.
- `amount` — المبلغ — Decimal Currency — required/positive/precision governed.
- `collectedAt` — وقت التحصيل — Instant — required/audited.
- `collectionLines` — المراجع — LineGrid/Collection — optional editable reference allocation.
- `financialStatus` — الحالة المالية — Enum/read-only/external contract only; no posting in FLOW01.
- `reason` — السبب — String conditional by exception/audit contract.

## LAYOUT — TEAM-D02 PASS
Shared `Transaction / HeaderLines` authority:
- Header/MainData = `Content`.
- Tabs/Workspace = `Fill`.
- CollectionReferenceGrid = `Fill`.
- summary/actions/audit and presenters = shared CoreUI.
- tabs: `Collection | References | Audit`.
- no LocalException/local style.

## FIELD_GRID — TEAM-D03 PASS
`GridProfile=TransactionLines`, `AutoGenerateColumns=false`, `UsesServerPaging=true`, `SelectionPolicy=SingleRow`.

| # | Key | Arabic Header | UI Type | Required | Edit policy | Width / editor |
|---:|---|---|---|---|---|---|
| 1 | `referenceType` | نوع المرجع | Enum | Yes | Pre-record | content / enum editor |
| 2 | `referenceId` | المرجع | Reference | Yes | Pre-record | primary Fill / `TransportLookup` |
| 3 | `amount` | المبلغ | Decimal | Yes | Pre-record | content numeric |
| 4 | `currency` | العملة | Reference | Yes | ReadOnly/inherits collection currency | content/reference |
| 5 | `linkageState` | حالة الربط | Enum | Yes | ReadOnly/server result | content state |

The issued `sum = amount` relationship remains domain/API validation; no competing authoritative client formula is created. Provider/sort identifiers remain `TBD-GATED`.

## UX — TEAM-D04 PASS
- record is permitted only for an eligible delivered/controlled collection event and issued permission/scope.
- one-currency rule and positive/precision validation remain contract/domain authority.
- line linkage errors use shared validation presenter; financial status never implies posting from this screen.
- loading prevents double-submit; server result is authoritative.
- no accounting journal, revenue recognition or settlement is created here.
- permission/scope denial leaks no hidden financial/reference data.
- all writes online-only.

## VISUAL — TEAM-D05 PASS
CoreUI Transaction visual system only: RTL/DPI, central currency/reference states, shared grid/tabs/validation/loading/error/audit. No local financial colors, totals logic, dimensions or toolbar clones.

## Independent review — TEAM-D06 PASS
Review report: `documentation/design/reviews/2026-08-24_BATCH-02_INDEPENDENT_REVIEW.md`. Open design findings: `0`. Runtime `TAE-F01-010` not run.

## Acceptance criteria
1. five explicit reference grid columns.
2. one currency and total/linkage validation remain authoritative outside presentation.
3. `financialStatus` is display-only and no posting is implied.
4. no API/DTO/Permission/DDL/offline/accounting invention.

## Handoff
Completed: `ANALYSIS`, `LAYOUT`, `FIELD_GRID`, `UX`, `VISUAL`, `INDEPENDENT_REVIEW`.  
Final: `DESIGN_APPROVED`.
