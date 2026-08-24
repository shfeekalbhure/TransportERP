# ACC-052 — دفتر اليومية العام — Canonical Screen Specification

**English:** General Journal  
**Module:** Accounting / Reporting  
**Profile / Variant:** `ReportInquiry / Inquiry`  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**OwnerTeam:** `TEAM-D06`  
**Batch:** `BATCH-13`

## Authority
- Current 57-screen baseline + current `MASTER_DEEP_RECONCILIATION_MATRIX` + Field Traceability + W2 report contracts.
- Batch authority: `documentation/design/batches/BATCH-13_ACCOUNTING_REPORTS_AUTHORITY_2026-08-24.md`.
- W2 permissions: `ACC052.View`, `ACC052.DrillDown`, `ACC052.Export`, `ACC052.Print` only.
- Current governing Variant is `Inquiry`; generic collected `Report` wording is superseded by the current reconciliation resolution.

## ANALYSIS — TEAM-D01 PASS
Purpose: read-only General Journal inquiry over authoritative journal/read-model data with contextual drill-down.
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
| 1 | `date` | التاريخ | ReadOnly |
| 2 | `journalNumber` | رقم القيد | ReadOnly |
| 3 | `journalType` | نوع القيد | ReadOnly |
| 4 | `reference` | المرجع | ReadOnly |
| 5 | `description` | الوصف | ReadOnly / primary fill |
| 6 | `totalDebit` | إجمالي المدين | ReadOnly server value |
| 7 | `totalCredit` | إجمالي الدائن | ReadOnly server value |
| 8 | `state` | الحالة | ReadOnly server state |
| 9 | `createdBy` | أنشئ بواسطة | ReadOnly |
| 10 | `postedBy` | رحّل بواسطة | ReadOnly |

Totals and state are server-authoritative; no client balancing or total formula is approved.

## UX — TEAM-D04 PASS
Query/DrillDown/Export/Print preserve parent query context and recheck permission/scope. Shared loading/error/empty/paging only. No mutation/offline write.

## VISUAL — TEAM-D05 PASS
CoreUI owns RTL/DPI/layout/summary/grid/pagination/details. Totals use shared server-value presentation without local accounting logic.

## TEAM-D06 — INDEPENDENT REVIEW
Pending final disposition. Review must confirm `Inquiry`, nine criteria, exact 10 columns and W2 four-action surface.

## Remaining technical gates
Exact provider/DTO/sort bindings and runtime/acceptance/release evidence remain separate.
