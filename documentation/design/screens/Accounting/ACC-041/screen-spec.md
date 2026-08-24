# ACC-041 — الفترات المحاسبية — Canonical Screen Specification

**English:** Fiscal Period  
**Profile / Variant:** `ControlApproval / Standard`  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**OwnerTeam:** `TEAM-D06`  
**Batch:** `BATCH-07`

## Authority
- Owner issuance: `SRC-060 / OWNER-FISCAL-YEAR-PERIOD-W1-W2-W3-ISSUANCE-001`.
- W2: List/Get/Request protected action/Approve/Reject/Return/Reopen; `ACC041.View/Execute/Approve/Reject/Return/Reopen`.
- W3: `FISCAL_YEAR_PERIOD_TYPED_SCREENDEFINITIONS_2026-08-22.md`.
- Test input: `FISCAL_YEAR_PERIOD_ACCEPTANCE_TEST_SPECIFICATIONS_2026-08-22.md` — issued, runtime not run.

## ANALYSIS — TEAM-D01 PASS
Purpose: provide controlled fiscal-period lifecycle actions over existing FiscalPeriod records, with parent-year integrity and approval separation.

Display/filter fields:
- `FiscalYearId` — parent FiscalYear scope/reference, filter/display only.
- `PeriodNo` — display-only period number, unique inside FiscalYear.
- `StartDate`, `EndDate` — display-only range; server enforces non-overlap and containment within parent FiscalYear.
- `Status` — display-only lifecycle state.
- `Version` — hidden expectedVersion token for protected action/Reopen.

Capabilities: View, Execute protected action, Approve, Reject, Return, Reopen, server paging.

## LAYOUT — TEAM-D02 PASS
Shared `ControlApproval / Standard`:
`ScreenHost → Toolbar → ContentHost → SearchPanel(Content) → ControlListGrid(Fill) → Pagination → ApprovalHistoryHost → AuditPanel`.
No tabs or local toolbar/grid/pagination/audit implementation.

## FIELD_GRID — TEAM-D03 PASS
`AutoGenerateColumns=false`, `SelectionPolicy=SingleRow`, `UsesServerPaging=true`.
Grid columns: `FiscalYear`, `PeriodNo`, `StartDate`, `EndDate`, `Status`.
Filters: `FiscalYearId`, `Status`, date range, action and authorized derived scope; sort allow-list only `periodNo|startDate|endDate|status`. No default page size or numeric cap is invented.

## UX — TEAM-D04 PASS
- FiscalPeriod data is read-only; no direct Create/Edit or Status editor.
- protected actions are requested only through the issued generic action route with target expectedVersion; client does not invent action codes.
- Approve/Reject/Return act on current ApprovalRequest with expectedVersion and target state recheck.
- SoD/self-decision prohibition remains server-authoritative.
- Reopen requires reason + current target Version.
- period overlap, parent-year containment and unique PeriodNo validation remain server/domain authoritative; no local fiscal calendar engine.
- no separate Close/Open/Lock action or `ACC-054 / PeriodAction` model is created.
- concurrency/state/approval errors use shared presenters; no silent overwrite or offline queue.

## VISUAL — TEAM-D05 PASS
Use shared ControlApproval CoreUI only: RTL/DPI, filters, read-only control grid, approval history/action presenters, validation/loading/error/audit. No local visual architecture.

## Acceptance criteria
1. FiscalYear+PeriodNo uniqueness and date containment are server-authoritative.
2. W1 fields/Status remain read-only.
3. protected action/approval/Reopen only; no direct lifecycle edit.
4. five explicit grid columns with server paging.
5. no invented action codes, direct Close/Open/Lock or ACC-054 model.
6. no Create/Edit/Delete/Print/Export/Disable/Enable/Move/offline capability.
7. no API/DTO/permission/DDL invention.

## Handoff
Completed: `ANALYSIS`, `LAYOUT`, `FIELD_GRID`, `UX`, `VISUAL`.  
Current: `INDEPENDENT_REVIEW` — `TEAM-D06`.
