# BATCH-13 — Accounting Core Reports — Design Authority

**Screens:** `ACC-046`, `ACC-047`, `ACC-048`, `ACC-050`, `ACC-051`, `ACC-052`  
**Date:** 2026-08-24  
**State:** `DESIGN_IN_PROGRESS / INDEPENDENT_REVIEW`

## Governing authority
This batch is derived from the current 57-screen baseline, `MASTER_DEEP_RECONCILIATION_MATRIX.xlsx / SCREEN_ASPECT_MATRIX`, current Field Traceability read-model rows, current W2 Screen→API→Permission traceability and shared CoreUI `ReportInquiry` rules.

The older working intelligence dossier that marked ACC-046 local UI extraction as partial is not used as the release authority because the stronger current reconciliation matrix now supplies exact current tabs, nine filter fields, result-grid columns, actions, permissions and W2 endpoint counts.

## Canonical identities
| Screen | Arabic name | Profile | Variant | Result columns |
|---|---|---|---|---:|
| ACC-046 | ميزان المراجعة | ReportInquiry | Report | 8 |
| ACC-047 | كشف الحساب | ReportInquiry | Statement | 11 |
| ACC-048 | قائمة الدخل | ReportInquiry | Report | 6 |
| ACC-050 | التدفقات النقدية | ReportInquiry | Report | 6 |
| ACC-051 | الأستاذ العام | ReportInquiry | Statement | 12 |
| ACC-052 | دفتر اليومية العام | ReportInquiry | Inquiry | 10 |

## Shared current filter contract
All six screens have nine governing report criteria:
1. الشركة
2. الفرع
3. من تاريخ
4. إلى تاريخ
5. السنة/الفترة
6. العملة
7. الحساب/النطاق
8. مركز التكلفة
9. الحالة/نوع القيد

Field Traceability classifies these as read-model query criteria. Company and date range are required by the current contract; Branch/FiscalPeriod/Currency/CostCenter/StateType are optional/contextual; AccountScope requiredness is report-dependent. Exact lookup provider identifiers and any unexposed DTO/sort-property names remain implementation-owned and are not invented.

## Executable surface
Exactly four W2 capabilities per screen:
- View / Query
- DrillDown
- Export
- Print

Permission families are exactly `ACCxxx.View`, `ACCxxx.DrillDown`, `ACCxxx.Export`, `ACCxxx.Print` for the respective screen. No Create/Edit/Delete/Post/Reverse/Approve or other mutation is authorized.

## Shared design rules
- Read-only server-authoritative report/read-model semantics.
- CoreUI `ReportInquiry` layout: `Filters(Content) → Summary(Content) → ResultsGrid(Fill) → Pagination(Fixed) → DetailsHost(ReadOnly optional)`.
- Result grids are explicit-column Display grids with single-row contextual drill-down and server paging.
- Financial balances, totals, ratios, running balances, comparative values and classifications are server results; no client accounting formula is authorized.
- DrillDown preserves parent query context and rechecks target permission/scope.
- Export/Print preserve current query/filter/sort context and use their W2 permissions.
- No offline mutation/queue/outbox/replay authority.
- No local toolbar/grid/pagination/audit/validation styling or behavior.

## Technical gates retained
- Exact server sort-key allow-list mapping where not exposed to design evidence.
- Exact lookup provider/search/revalidation identifiers where not exposed.
- DTO property names beyond the read-model semantics explicitly present in current evidence.
- Runtime/acceptance/release review remains separate from design approval.

## Handoff
`TEAM-D01 ANALYSIS = PASS`  
`TEAM-D02 LAYOUT = PASS`  
`TEAM-D03 FIELD_GRID = PASS`  
`TEAM-D04 UX = PASS`  
`TEAM-D05 VISUAL = PASS`  
Next: `TEAM-D06 INDEPENDENT_REVIEW`.
