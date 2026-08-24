# ACC-050 — التدفقات النقدية — Canonical Screen Specification

**English:** Cash Flow Statement  
**Module:** Accounting / Reporting  
**Profile / Variant:** `ReportInquiry / Report`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**Batch:** `BATCH-13`

## Authority
- Current 57-screen baseline + current `MASTER_DEEP_RECONCILIATION_MATRIX` + Field Traceability + W2 report contracts.
- Batch authority: `documentation/design/batches/BATCH-13_ACCOUNTING_REPORTS_AUTHORITY_2026-08-24.md`.
- W2 permissions: `ACC050.View`, `ACC050.DrillDown`, `ACC050.Export`, `ACC050.Print` only.

## ANALYSIS — TEAM-D01 PASS
Purpose: read-only Cash Flow projection over authoritative accounting/cash-bank sources with contextual drill-down.
Functional areas: `معايير التقرير | النتائج | الملخص والتفاصيل`.
No mutation authority.

## LAYOUT — TEAM-D02 PASS
CoreUI `ReportInquiry`: `Filters(Content) → Summary(Content) → ResultsGrid(Fill) → Pagination(Fixed) → DetailsHost(ReadOnly optional)`.

## FIELD_GRID — TEAM-D03 PASS
### Query criteria
Nine governing criteria: الشركة، الفرع، من تاريخ، إلى تاريخ، السنة/الفترة، العملة، الحساب/النطاق، مركز التكلفة، الحالة/نوع القيد.
Company and date range are required; remaining criteria are optional/contextual except account scope is report-dependent. Exact providers/DTO/sort fields remain implementation authority.

### ResultsGrid
`GridProfile=Display`, `AutoGenerateColumns=false`, `UsesServerPaging=true`, `SelectionPolicy=SingleRow`.

| # | Design key | Arabic column | Presentation |
|---:|---|---|---|
| 1 | `activity` | النشاط | ReadOnly / content |
| 2 | `item` | البند | ReadOnly / primary fill |
| 3 | `cashInflow` | التدفق الداخل | ReadOnly server value |
| 4 | `cashOutflow` | التدفق الخارج | ReadOnly server value |
| 5 | `netCashFlow` | صافي التدفق | ReadOnly server value |
| 6 | `period` | الفترة | ReadOnly |

Classification and net-flow calculations remain server-authoritative; no client formula is authorized.

## UX — TEAM-D04 PASS
Query/DrillDown/Export/Print preserve query context and recheck permission/scope server-side. Shared loading/error/empty/paging only. No mutation or offline write.

## VISUAL — TEAM-D05 PASS
CoreUI owns RTL/DPI/layout/summary/grid/pagination/details. Inflow/outflow/net values use shared server-value presentation without local financial logic.

## TEAM-D06 — INDEPENDENT REVIEW PASS
- Review: `documentation/design/batches/BATCH-13_INDEPENDENT_REVIEW_2026-08-24.md`.
- `Report`, nine criteria, exact six columns and W2 View/DrillDown/Export/Print surface: PASS.
- Server-authoritative classification/net-flow semantics and no mutation/invention: PASS.
- Open design findings: `0`.

## Remaining technical gates
Exact provider/DTO/sort mappings and runtime/acceptance/release evidence remain separate.

## DESIGN-LEAD closure
`ACC-050 = DESIGN_APPROVED`.
