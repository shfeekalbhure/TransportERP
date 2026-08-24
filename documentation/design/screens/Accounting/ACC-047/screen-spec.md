# ACC-047 — كشف الحساب — Canonical Screen Specification

**English:** Account Statement  
**Module:** Accounting / Reporting  
**Profile / Variant:** `ReportInquiry / Statement`  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**OwnerTeam:** `TEAM-D06`  
**Batch:** `BATCH-13`

## Authority
- Current 57-screen baseline + current `MASTER_DEEP_RECONCILIATION_MATRIX` + Field Traceability + W2 report contracts.
- Batch authority: `documentation/design/batches/BATCH-13_ACCOUNTING_REPORTS_AUTHORITY_2026-08-24.md`.
- W2 permissions: `ACC047.View`, `ACC047.DrillDown`, `ACC047.Export`, `ACC047.Print` only.

## ANALYSIS — TEAM-D01 PASS
Purpose: read-only account statement projection with server-calculated running balances and contextual source drill-down.
Functional areas: `معايير التقرير | النتائج | الملخص والتفاصيل`.
No mutation capability exists.

## LAYOUT — TEAM-D02 PASS
CoreUI `ReportInquiry`: `Filters(Content) → Summary(Content) → ResultsGrid(Fill) → Pagination(Fixed) → DetailsHost(ReadOnly optional)`.
No LocalException or local visual/control clone.

## FIELD_GRID — TEAM-D03 PASS
### Query criteria
The governing nine criteria are: الشركة، الفرع، من تاريخ، إلى تاريخ، السنة/الفترة، العملة، الحساب/النطاق، مركز التكلفة، الحالة/نوع القيد.

- Company and date range are required by current Field Traceability.
- Branch/FiscalPeriod/Currency/CostCenter/StateType are optional/contextual.
- AccountScope requiredness is report-dependent.
- All provider identifiers, date/scope validation and exact DTO/sort bindings remain server/W2 authority; unexposed identifiers are `TBD-GATED`.

### ResultsGrid
`GridProfile=Display`, `AutoGenerateColumns=false`, `UsesServerPaging=true`, `SelectionPolicy=SingleRow`.

| # | Design key | Arabic column | Presentation |
|---:|---|---|---|
| 1 | `date` | التاريخ | ReadOnly |
| 2 | `documentNumber` | رقم المستند | ReadOnly |
| 3 | `documentType` | نوع المستند | ReadOnly |
| 4 | `reference` | المرجع | ReadOnly |
| 5 | `description` | البيان | ReadOnly / primary fill |
| 6 | `debit` | مدين | ReadOnly server value |
| 7 | `credit` | دائن | ReadOnly server value |
| 8 | `runningBalance` | الرصيد الجاري | ReadOnly server value |
| 9 | `currency` | العملة | ReadOnly |
| 10 | `branch` | الفرع | ReadOnly |
| 11 | `costCenter` | مركز التكلفة | ReadOnly |

Running balance is server-authoritative; no client accumulation formula is approved.

## UX — TEAM-D04 PASS
Query/DrillDown/Export/Print preserve the server query context and each rechecks permission/scope. Shared error/loading/empty/paging behavior only. No mutation or offline write.

## VISUAL — TEAM-D05 PASS
CoreUI owns RTL/DPI/layout/grid/pagination/details. Running balance and debit/credit use shared server-value presentation; no local financial calculation or color rule.

## TEAM-D06 — INDEPENDENT REVIEW
Pending final disposition. Review must confirm `Statement`, nine criteria, exact 11 columns and four W2 permissions only.

## Remaining technical gates
Exact lookup providers, DTO fields, sort allow-list bindings and runtime/release evidence remain implementation/release gates.
