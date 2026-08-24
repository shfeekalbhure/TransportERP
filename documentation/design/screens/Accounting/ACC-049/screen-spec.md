# ACC-049 — الميزانية العمومية — Canonical Screen Specification

**English:** Balance Sheet  
**Module:** Accounting / Reporting  
**Profile / Variant:** `ReportInquiry / Report`  
**Toolbar:** `TB-R`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**CompletedStages:** `ANALYSIS, LAYOUT, FIELD_GRID, UX, VISUAL, INDEPENDENT_REVIEW`  
**Batch:** `BATCH-10`

## Authority
- Current 57-screen baseline: `CURRENT_TRANSPORTERP_SCREEN_BASELINE_V1.1.csv` — ACC-049 detailed governing screen content.
- Current authority family: Current Approved References V1.26 + Unified Design/Execution V1.3 + current W1/W2/W3 contracts.
- W2 exact actions: Query / DrillDown / Export / Print under `ACC049.View`, `ACC049.DrillDown`, `ACC049.Export`, `ACC049.Print`.
- CoreUI `ReportInquiry` layout: `Filters(Content) → Summary(Content) → ResultsGrid(Fill) → Pagination(Fixed) → DetailsHost(ReadOnly optional)`.
- No historical separate typed ScreenDefinition is claimed as recovered; this file is the canonical design closure over current governing content.
- Runtime E2E evidence remains separate from design/release authority.

## ANALYSIS — TEAM-D01 PASS
Purpose: read-only server-authoritative Balance Sheet projection with financial criteria, source-calculated balances, contextual drill-down, export and print.

- Primary persistence entity: none; `BalanceSheet projection` read model.
- Server source domain may include JournalEntry / JournalLine / Account / AccountType; the client does not recompute financial results.
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
| 1 | `item` | البند | DisplayText | content-sized |
| 2 | `accountGroup` | الحساب/المجموعة | Reference/Text | primary fill |
| 3 | `currentBalance` | الرصيد الحالي | MonetaryAmount | compact numeric |
| 4 | `comparativeBalance` | الرصيد المقارن | MonetaryAmount | compact numeric |
| 5 | `difference` | الفرق | MonetaryAmount | compact numeric |

Exact server sort-key mapping remains `TBD-GATED` where not explicitly exposed by current evidence; binding must use the W2 allow-list only. This is nonblocking for design.

DrillDown uses the selected server result key plus parent query context and returns shared read-only details; no local detail DTO/route/extra columns are invented.

## UX — TEAM-D04 PASS
- UI permission state is advisory; server enforces View/scope.
- Query uses shared loading/error/empty/double-submit prevention behavior.
- Summary and balances are read-only server results.
- DrillDown preserves parent context and rechecks permission/scope; `DRILLDOWN_NOT_ALLOWED` uses shared error UX.
- Export preserves current query context under `ACC049.Export`; `EXPORT_TOO_LARGE` / `EXPORT_FAILED` are shared errors.
- Print preserves current query context under `ACC049.Print`; `PRINT_FAILED` is a shared error.
- No New/Save/Edit/Delete commands.
- No offline write/queue/outbox/retry/replay.

## VISUAL — TEAM-D05 PASS
- CoreUI owns RTL, typography, spacing, dimensions, focus, loading/error/validation, TB-R, grid, pagination, summary and details visuals.
- Filters=`Content`; Summary=`Content`; ResultsGrid=`Fill`; Pagination=`Fixed`.
- Numeric values use shared numeric formatting/alignment; no local visual semantics.

## INDEPENDENT_REVIEW — TEAM-D06 PASS
Review record: `documentation/design/reviews/BATCH-10_ACC049_ACC058_INDEPENDENT_REVIEW_2026-08-24.md`.

Result:
- Identity/Profile/Variant PASS.
- Exact current five result columns PASS.
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
`ACC-049 = DESIGN_APPROVED`.
