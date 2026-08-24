# ACC-058 — ميزان المراجعة التفصيلي — Canonical Screen Specification

**English:** Detailed Trial Balance  
**Module:** Accounting / Reporting  
**Profile / Variant:** `ReportInquiry / Report`  
**Toolbar:** `TB-R`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**CompletedStages:** `ANALYSIS, LAYOUT, FIELD_GRID, UX, VISUAL, INDEPENDENT_REVIEW`  
**Batch:** `BATCH-10`

## Authority
- Current 57-screen baseline: `CURRENT_TRANSPORTERP_SCREEN_BASELINE_V1.1.csv` — ACC-058 detailed governing screen content.
- Current authority family: Current Approved References V1.26 + Unified Design/Execution V1.3 + current W1/W2/W3 contracts.
- W2 exact actions: Query / DrillDown / Export / Print under `ACC058.View`, `ACC058.DrillDown`, `ACC058.Export`, `ACC058.Print`.
- CoreUI `ReportInquiry` layout: `Filters(Content) → Summary(Content) → ResultsGrid(Fill) → Pagination(Fixed) → DetailsHost(ReadOnly optional)`.
- No historical separate typed ScreenDefinition is claimed as recovered; this file is the canonical design closure over current governing content.
- Runtime E2E evidence remains separate from design/release authority.

## ANALYSIS — TEAM-D01 PASS
Purpose: read-only server-authoritative Detailed Trial Balance projection with financial criteria, server-calculated debit/credit/balance data, contextual drill-down, export and print.

- Primary persistence entity: none; `DetailedTrialBalance projection` read model.
- Server source domain may include JournalEntry / JournalLine / Account; the client does not recompute ledger balances.
- Capabilities: View, DrillDown, Export, Print only.
- Company scope is required/server-filtered; Branch is optional or context-required/server-filtered.
- No Create/Edit/Delete/Post/Reverse or local financial mutation capability.

## LAYOUT — TEAM-D02 PASS
CoreUI only:

`TransportScreenHost → TransportToolbarHost(TB-R) → TransportContentHost → Filters(Content) → Summary(Content) → ResultsGrid(Fill) → Pagination(Fixed) → DetailsHost(ReadOnly optional) → shared Audit/Context hosts`.

Current functional areas preserved:
1. `معايير التقرير`
2. `النتائج`
3. `الملخص والتفاصيل`

No local nested scrolling, pixel sizes, fonts, colors, padding, toolbar, grid, pagination or audit implementation.

## FIELD_GRID — TEAM-D03 PASS
### Query/filter design
W3 keys below are design aliases, not claims about W2 DTO property names.

| W3 design key | Arabic label | UI semantic | Authority |
|---|---|---|---|
| `companyRef` | الشركة | Lookup / Reference | required authorized scope |
| `branchRef` | الفرع | Lookup / Reference | optional/context-dependent; server filtered |
| `fromDate` | من تاريخ | Date | W2 date/period rule |
| `toDate` | إلى تاريخ | Date | W2 date/period rule |
| `fiscalPeriodRef` | السنة/الفترة | Lookup / Reference | W2-bound filter |
| `currencyRef` | العملة | Lookup / Reference | W2-bound filter |
| `accountScopeRef` | الحساب/النطاق | Lookup / Reference | W2-bound filter |
| `costCenterRef` | مركز التكلفة | Lookup / Reference | W2-bound filter |
| `entryStateType` | الحالة/نوع القيد | Enum / typed filter | W2-bound filter |
| `searchText` | البحث | SearchText | shared W2 typed search where supported |

Filter validation, date/period rules and authorization are server-authoritative. Page/PageSize use server paging. Exact DTO property names and exact server sort-key mapping are not invented.

### ResultsGrid
- `GridProfile = Display`; read-only.
- `AutoGenerateColumns = false`.
- `Selection = SingleRow` for contextual DrillDown.
- `UsesServerPaging = true`.
- No client financial recomputation.

| Order | W3 design key | Arabic column | Display semantic | Width policy |
|---:|---|---|---|---|
| 1 | `accountNumber` | رقم الحساب | Code/Text | content-sized |
| 2 | `accountName` | اسم الحساب | DisplayText | primary fill |
| 3 | `entryNumber` | رقم القيد | Reference/Text | content-sized |
| 4 | `entryDate` | التاريخ | Date | compact date |
| 5 | `description` | البيان | DisplayText | primary fill |
| 6 | `debit` | مدين | MonetaryAmount | compact numeric |
| 7 | `credit` | دائن | MonetaryAmount | compact numeric |
| 8 | `balance` | الرصيد | MonetaryAmount | compact numeric |
| 9 | `branch` | الفرع | Reference/Text | content-sized |
| 10 | `costCenter` | مركز التكلفة | Reference/Text | content-sized |
| 11 | `currency` | العملة | Reference/Text | compact reference |

Exact server sort-key mapping remains `TBD-GATED` where not explicitly exposed by current evidence; binding must use the W2 allow-list only. This is nonblocking for design.

DrillDown uses the selected server result key plus parent query context and returns shared read-only details; no local detail DTO/route/extra columns are invented.

## UX — TEAM-D04 PASS
- UI permission state is advisory; server enforces View/scope.
- Query uses shared loading/error/empty/double-submit prevention behavior.
- Debit/credit totals and balances are read-only server results.
- DrillDown preserves parent context and rechecks permission/scope; `DRILLDOWN_NOT_ALLOWED` uses shared error UX.
- Export preserves current query context under `ACC058.Export`; `EXPORT_TOO_LARGE` / `EXPORT_FAILED` are shared errors.
- Print preserves current query context under `ACC058.Print`; `PRINT_FAILED` is a shared error.
- No New/Save/Edit/Delete commands.
- No offline write/queue/outbox/retry/replay.

## VISUAL — TEAM-D05 PASS
- CoreUI owns RTL, typography, spacing, dimensions, focus, loading/error/validation, TB-R, grid, pagination, summary and details visuals.
- Filters=`Content`; Summary=`Content`; ResultsGrid=`Fill`; Pagination=`Fixed`.
- Debit/credit/balance values use shared numeric formatting/alignment; no local visual semantics.

## INDEPENDENT_REVIEW — TEAM-D06 PASS
Review record: `documentation/design/reviews/BATCH-10_ACC049_ACC058_INDEPENDENT_REVIEW_2026-08-24.md`.

Result:
- Identity/Profile/Variant PASS.
- Exact current eleven result columns PASS.
- Governing criteria family PASS.
- W2 View/DrillDown/Export/Print binding PASS.
- Server-authoritative financial semantics PASS.
- CoreUI layout/visual ownership PASS.
- No mutation/offline/local formula invention PASS.
- Historical typed-file absence disclosed PASS.
- Runtime/design/release separation PASS.
- **Open design findings: 0.**

## Runtime evidence boundary
A separate WAVE-1 E2E package reports runtime coverage for posted ledger, reversal, branch/currency isolation, drill-down, export/print and PageSize cap 200 on an exact implementation SHA. This design approval does **not** declare the release/delivery independent-review gate closed.

## DESIGN-LEAD closure
`ACC-058 = DESIGN_APPROVED`.
