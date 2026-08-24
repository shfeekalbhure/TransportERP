# ACC-051 — الأستاذ العام — Canonical Screen Specification

**English:** General Ledger  
**Module:** Accounting / Reporting  
**Profile / Variant:** `ReportInquiry / Statement`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**Batch:** `BATCH-13`

## Authority
- Current 57-screen baseline + current `MASTER_DEEP_RECONCILIATION_MATRIX` + Field Traceability + W2 report contracts.
- Batch authority: `documentation/design/batches/BATCH-13_ACCOUNTING_REPORTS_AUTHORITY_2026-08-24.md`.
- W2 permissions: `ACC051.View`, `ACC051.DrillDown`, `ACC051.Export`, `ACC051.Print` only.
- Current governing Variant is `Statement`; collected `Report` wording is superseded by the current reconciliation resolution.

## ANALYSIS — TEAM-D01 PASS
Purpose: read-only General Ledger statement with server-calculated balances and authoritative source drill-down.
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
| 3 | `lineNumber` | رقم السطر | ReadOnly |
| 4 | `account` | الحساب | ReadOnly / content |
| 5 | `description` | البيان | ReadOnly / primary fill |
| 6 | `debit` | مدين | ReadOnly server value |
| 7 | `credit` | دائن | ReadOnly server value |
| 8 | `balance` | الرصيد | ReadOnly server value |
| 9 | `currency` | العملة | ReadOnly |
| 10 | `branch` | الفرع | ReadOnly |
| 11 | `costCenter` | مركز التكلفة | ReadOnly |
| 12 | `source` | المصدر | ReadOnly |

Balance and all accounting values are server-authoritative; no client accumulation or recomputation.

## UX — TEAM-D04 PASS
Query/DrillDown/Export/Print preserve parent query context and recheck permission/scope. Shared loading/error/empty/paging only. No mutation/offline write.

## VISUAL — TEAM-D05 PASS
CoreUI owns RTL/DPI/layout/summary/grid/pagination/details. Debit/credit/balance use shared server-value presentation without local accounting logic.

## TEAM-D06 — INDEPENDENT REVIEW PASS
- Review: `documentation/design/batches/BATCH-13_INDEPENDENT_REVIEW_2026-08-24.md`.
- `Statement`, nine criteria, exact 12 columns and W2 View/DrillDown/Export/Print surface: PASS.
- Server-authoritative ledger balance and no mutation/invention: PASS.
- Open design findings: `0`.

## Remaining technical gates
Exact provider/DTO/sort bindings and runtime/acceptance/release evidence remain separate.

## DESIGN-LEAD closure
`ACC-051 = DESIGN_APPROVED`.
