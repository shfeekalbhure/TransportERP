# ACC-046 — ميزان المراجعة — Canonical Screen Specification

**English:** Trial Balance  
**Module:** Accounting / Reporting  
**Profile / Variant:** `ReportInquiry / Report`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**Batch:** `BATCH-13`

## Authority
- Current 57-screen baseline + current `MASTER_DEEP_RECONCILIATION_MATRIX` + current Field Traceability + W2 report contracts.
- Batch authority: `documentation/design/batches/BATCH-13_ACCOUNTING_REPORTS_AUTHORITY_2026-08-24.md`.
- W2 permissions: `ACC046.View`, `ACC046.DrillDown`, `ACC046.Export`, `ACC046.Print` only.
- No separate historical typed ScreenDefinition is claimed; this canonical spec closes design from stronger current governing content.

## ANALYSIS — TEAM-D01 PASS
Purpose: read-only server-authoritative Trial Balance with financial criteria, source-calculated balances, contextual drill-down, export and print.

Functional areas: `معايير التقرير | النتائج | الملخص والتفاصيل`.

No business mutation is authorized. Report values, balances and totals are server/read-model results.

## LAYOUT — TEAM-D02 PASS
CoreUI `ReportInquiry` only:
`Filters(Content) → Summary(Content) → ResultsGrid(Fill) → Pagination(Fixed) → DetailsHost(ReadOnly optional)`.

No LocalException, local toolbar/grid/pagination/audit implementation or pixel styling.

## FIELD_GRID — TEAM-D03 PASS
### Query criteria
| # | Design key | Arabic label | UI semantic | Requiredness |
|---:|---|---|---|---|
| 1 | `companyRef` | الشركة | Lookup / Reference | Required / authorized scope |
| 2 | `branchRef` | الفرع | Lookup / Reference | Optional / company-context |
| 3 | `fromDate` | من تاريخ | Date | Required |
| 4 | `toDate` | إلى تاريخ | Date | Required |
| 5 | `fiscalPeriodRef` | السنة/الفترة | Lookup / Reference | Optional |
| 6 | `currencyRef` | العملة | Lookup / Reference | Optional |
| 7 | `accountScopeRef` | الحساب/النطاق | Lookup / Reference | Report-dependent |
| 8 | `costCenterRef` | مركز التكلفة | Lookup / Reference | Optional |
| 9 | `entryStateType` | الحالة/نوع القيد | Typed filter | Optional |

Date ordering, scope, account range/tree semantics and lookup validity are server-authoritative. Exact provider/DTO/sort identifiers remain `TBD-GATED` where not exposed.

### ResultsGrid
`GridProfile=Display`, `AutoGenerateColumns=false`, `UsesServerPaging=true`, `SelectionPolicy=SingleRow` for DrillDown.

| # | Design key | Arabic column | Presentation |
|---:|---|---|---|
| 1 | `accountCode` | رقم الحساب | ReadOnly / content |
| 2 | `accountName` | اسم الحساب | ReadOnly / primary fill |
| 3 | `openingDebit` | الرصيد الافتتاحي مدين | ReadOnly server value |
| 4 | `openingCredit` | الرصيد الافتتاحي دائن | ReadOnly server value |
| 5 | `movementDebit` | حركة مدين | ReadOnly server value |
| 6 | `movementCredit` | حركة دائن | ReadOnly server value |
| 7 | `closingDebit` | الرصيد الختامي مدين | ReadOnly server value |
| 8 | `closingCredit` | الرصيد الختامي دائن | ReadOnly server value |

No local balance formula or recomputation. Exact row DTO/sort-key bindings remain implementation-owned.

## UX — TEAM-D04 PASS
- Query and refresh use shared loading/error/empty behavior; server enforces View and scope.
- DrillDown preserves parent context and rechecks `ACC046.DrillDown`.
- Export/Print preserve current query context under their distinct permissions.
- No Create/Edit/Delete/Post/Reverse/Approve command.
- No offline write/queue/outbox/replay.

## VISUAL — TEAM-D05 PASS
CoreUI owns RTL, DPI, filters, summary, grid, pagination, details, focus and error states. Financial values use shared server-value formatting; no local accounting visual formula.

## TEAM-D06 — INDEPENDENT REVIEW PASS
- Review: `documentation/design/batches/BATCH-13_INDEPENDENT_REVIEW_2026-08-24.md`.
- Identity/Variant, 9 criteria, exact 8 result columns and W2 View/DrillDown/Export/Print surface: PASS.
- Server-authoritative financial semantics and no mutation/invention: PASS.
- Open design findings: `0`.

## Remaining technical gates
Exact provider/DTO/sort-key mappings and runtime/acceptance/release evidence remain separate and nonblocking for design approval.

## DESIGN-LEAD closure
`ACC-046 = DESIGN_APPROVED`.
