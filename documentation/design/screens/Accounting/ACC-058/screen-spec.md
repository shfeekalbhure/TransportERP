# ACC-058 — ميزان المراجعة التفصيلي — Canonical Screen Specification

**English:** Detailed Trial Balance  
**Module:** Accounting / Reporting  
**Profile / Variant:** `ReportInquiry / Report`  
**Toolbar:** `TB-R`  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**OwnerTeam:** `TEAM-D06`  
**Batch:** `BATCH-10`

## Authority
- Current 57-screen baseline: `CURRENT_TRANSPORTERP_SCREEN_BASELINE_V1.1.csv` — ACC-058 detailed governing screen content.
- Current authority family: Current Approved References V1.26 + Unified Design/Execution V1.3 + W1/W2/W3 current contracts.
- W2 exact actions: Query / DrillDown / Export / Print under `ACC058.View`, `ACC058.DrillDown`, `ACC058.Export`, `ACC058.Print`.
- CoreUI layout: `ReportInquiry` = `Filters(Content) → Summary(Content) → ResultsGrid(Fill) → Pagination(Fixed) → DetailsHost(ReadOnly optional)`.
- Runtime E2E evidence is separate from this design closure and does not replace W3 authority.
- No historical concrete typed ScreenDefinition is claimed as recovered; this canonical file is the bounded design closure over current governing content.

## ANALYSIS — TEAM-D01 PASS
Purpose: provide a read-only detailed trial-balance projection with financial filters, server-calculated debit/credit/balance data, contextual drill-down, export and print.

Data ownership:
- Primary persistence entity: none; this is a read model.
- Read model: `DetailedTrialBalance projection`.
- Governing source entities include JournalEntry / JournalLine / Account only as server-side sources; the client does not recompute ledger balances.

Capabilities:
- View/query → `ACC058.View`.
- DrillDown → `ACC058.DrillDown`.
- Export → `ACC058.Export`.
- Print → `ACC058.Print`.
- No Create/Edit/Delete/Post/Reverse or local financial mutation capability.

Scope:
- Company required and filtered by server permission.
- Branch optional or required by context and server-filtered.
- Export/Print/DrillDown preserve the authorized parent query context; target permission/scope is re-evaluated server-side.

## LAYOUT — TEAM-D02 PASS
CoreUI only:

`TransportScreenHost → TransportToolbarHost(TB-R) → TransportContentHost → Filters(Content) → Summary(Content) → ResultsGrid(Fill) → Pagination(Fixed) → DetailsHost(ReadOnly optional) → Audit/Context hosts as shared policy dictates`.

Current functional areas are preserved as:
1. `معايير التقرير`
2. `النتائج`
3. `الملخص والتفاصيل`

These are functional areas mapped to ReportInquiry roles; no local nested scrolling, pixel heights, colors, fonts, padding, toolbar, grid, pagination or audit implementation is introduced.

## FIELD_GRID — TEAM-D03 PASS
### Query/filter contract
The current governing screen inventory defines the following report criteria. W3 keys below are design aliases and do **not** claim W2 DTO property names:

| W3 design key | Arabic label | UI semantic | Requiredness / authority |
|---|---|---|---|
| `companyRef` | الشركة | Lookup / Reference | required authorized scope |
| `branchRef` | الفرع | Lookup / Reference | optional / context-dependent; server filtered |
| `fromDate` | من تاريخ | Date | W2 date/period rule |
| `toDate` | إلى تاريخ | Date | W2 date/period rule |
| `fiscalPeriodRef` | السنة/الفترة | Lookup / Reference | W2-bound optional/context filter |
| `currencyRef` | العملة | Lookup / Reference | W2-bound filter |
| `accountScopeRef` | الحساب/النطاق | Lookup / Reference | W2-bound filter |
| `costCenterRef` | مركز التكلفة | Lookup / Reference | W2-bound filter |
| `entryStateType` | الحالة/نوع القيد | Enum / typed filter | W2-bound filter |
| `searchText` | البحث | SearchText | shared W2 typed search where supported |

Rules:
- Filter validation, date/period consistency and scope decisions remain server-authoritative.
- Exact DTO property names and exact sort-key allow-list mapping are not invented here.
- Page/PageSize are server-paged under the shared ReportInquiry contract; effective PageSize is server-authoritative.

### ResultsGrid
- `GridProfile = Display` / read-only.
- `AutoGenerateColumns = false`.
- `Selection = SingleRow` for contextual DrillDown.
- `UsesServerPaging = true`.
- Debit, credit and balance values are server-calculated; no client recomputation.

| Order | W3 design key | Arabic column | Display semantic | Edit | Width policy |
|---:|---|---|---|---|---|
| 1 | `accountNumber` | رقم الحساب | Code/Text | read-only | content-sized |
| 2 | `accountName` | اسم الحساب | DisplayText | read-only | primary fill |
| 3 | `entryNumber` | رقم القيد | Reference/Text | read-only | content-sized |
| 4 | `entryDate` | التاريخ | Date | read-only | compact date |
| 5 | `description` | البيان | DisplayText | read-only | primary fill |
| 6 | `debit` | مدين | MonetaryAmount | read-only | compact numeric |
| 7 | `credit` | دائن | MonetaryAmount | read-only | compact numeric |
| 8 | `balance` | الرصيد | MonetaryAmount | read-only | compact numeric |
| 9 | `branch` | الفرع | Reference/Text | read-only | content-sized |
| 10 | `costCenter` | مركز التكلفة | Reference/Text | read-only | content-sized |
| 11 | `currency` | العملة | Reference/Text | read-only | compact reference |

Exact server sort-key binding remains `TBD-GATED` where the W2 allow-list does not expose a named key in current evidence; this is nonblocking for the visual/design contract.

### DrillDown details
DrillDown uses the selected server result key plus the parent query context and returns read-only details through the shared DetailsHost. No local detail DTO, extra columns or navigation route is invented.

## UX — TEAM-D04 PASS
- Initial query/filter surface is permission-advisory only; server enforces `ACC058.View` and scope.
- Query submission uses shared loading/error/empty states and prevents duplicate submit through shared command state.
- Result data, debit/credit totals and balance values are read-only server results.
- DrillDown preserves parent filter/sort/scope context; permission is re-evaluated and `DRILLDOWN_NOT_ALLOWED` is handled through shared error UX.
- Export uses the exact current result context and `ACC058.Export`; `EXPORT_TOO_LARGE` / `EXPORT_FAILED` use shared error handling.
- Print uses the exact current result context and `ACC058.Print`; `PRINT_FAILED` uses shared error handling.
- Validation/scope/permission/not-found/conflict/concurrency errors use shared error contracts; no local error vocabulary is created.
- No New/Save/Edit/Delete commands are presented.
- No offline write, queue, outbox, retry or replay is introduced.

## VISUAL — TEAM-D05 PASS
- CoreUI owns RTL, typography, spacing, dimensions, focus, validation/error/loading states, TB-R, grid, pagination, summary and details visuals.
- Filters are Content-sized; Summary is Content-sized; ResultsGrid owns Fill; Pagination is Fixed.
- Debit/credit/balance columns use shared numeric alignment/formatting; no local color or font semantics are created.
- Print/Export/DrillDown visibility follows capability binding and is never treated as authorization authority.

## Runtime evidence boundary
A separate WAVE-1 runtime E2E package reports ACC-058 coverage for posted ledger, reversal, branch/currency isolation, drill-down, export/print and PageSize cap 200 on an exact implementation SHA. That evidence is **not** used to claim delivery/release approval here; release-independent review remains a separate gate.

## TEAM-D06 review input
Verify at minimum:
1. Identity/Profile/Variant = ACC-058 / ReportInquiry / Report.
2. Current functional areas and exact 11 result columns are preserved.
3. Current criteria family is preserved without inventing DTO property names.
4. W2 capabilities are exactly View/DrillDown/Export/Print.
5. Financial values are server-authoritative/read-only.
6. ReportInquiry vertical sizing is CoreUI-owned.
7. No unsupported local styles, mutation actions, offline authority or financial formulas.
8. Historical absence of a separate typed ScreenDefinition file is stated rather than concealed.
9. Runtime evidence is not conflated with design or release approval.
