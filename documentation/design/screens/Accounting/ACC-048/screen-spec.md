# ACC-048 — قائمة الدخل — Canonical Screen Specification

**English:** Income Statement  
**Module:** Accounting / Reporting  
**Profile / Variant:** `ReportInquiry / Report`  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**OwnerTeam:** `TEAM-D06`  
**Batch:** `BATCH-13`

## Authority
- Current 57-screen baseline + current `MASTER_DEEP_RECONCILIATION_MATRIX` + Field Traceability + W2 report contracts.
- Batch authority: `documentation/design/batches/BATCH-13_ACCOUNTING_REPORTS_AUTHORITY_2026-08-24.md`.
- W2 permissions: `ACC048.View`, `ACC048.DrillDown`, `ACC048.Export`, `ACC048.Print` only.

## ANALYSIS — TEAM-D01 PASS
Purpose: read-only Income Statement with source-calculated current/comparative values and contextual drill-down.
Functional areas: `معايير التقرير | النتائج | الملخص والتفاصيل`.
No mutation authority.

## LAYOUT — TEAM-D02 PASS
CoreUI `ReportInquiry`: `Filters(Content) → Summary(Content) → ResultsGrid(Fill) → Pagination(Fixed) → DetailsHost(ReadOnly optional)`.

## FIELD_GRID — TEAM-D03 PASS
### Query criteria
Nine governing criteria: الشركة، الفرع، من تاريخ، إلى تاريخ، السنة/الفترة، العملة، الحساب/النطاق، مركز التكلفة، الحالة/نوع القيد.
Company and date range are required; remaining criteria are optional/contextual except account scope is report-dependent. Exact providers/DTO/sort fields remain W2/implementation authority.

### ResultsGrid
`GridProfile=Display`, `AutoGenerateColumns=false`, `UsesServerPaging=true`, `SelectionPolicy=SingleRow`.

| # | Design key | Arabic column | Presentation |
|---:|---|---|---|
| 1 | `item` | البند | ReadOnly / content |
| 2 | `accountGroup` | الحساب/المجموعة | ReadOnly / primary fill |
| 3 | `currentPeriod` | الفترة الحالية | ReadOnly server value |
| 4 | `comparativePeriod` | الفترة المقارنة | ReadOnly server value |
| 5 | `difference` | الفرق | ReadOnly server value |
| 6 | `changePercentage` | نسبة التغير | ReadOnly server value |

No client difference/ratio/aggregation formula is authorized.

## UX — TEAM-D04 PASS
Server-authoritative query, summary and drill-down. Export/Print preserve query context under distinct permissions. Shared paging/loading/error/empty behavior only; no mutation/offline write.

## VISUAL — TEAM-D05 PASS
CoreUI owns RTL/DPI/layout/summary/grid/pagination/details. Comparative/difference/percentage values use server-supplied presentation semantics; no local formula or custom financial styling.

## TEAM-D06 — INDEPENDENT REVIEW
Pending final disposition. Review must confirm `Report`, nine criteria, exact six columns and View/DrillDown/Export/Print only.

## Remaining technical gates
Exact provider/DTO/sort mappings and runtime/acceptance/release evidence remain separate.
