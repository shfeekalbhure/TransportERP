# GEN-012 — السنوات المالية — Canonical Screen Specification

**English:** Fiscal Year  
**Profile / Variant:** `ControlApproval / Standard`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**Batch:** `BATCH-07`

## Authority
- Owner issuance: `SRC-060 / OWNER-FISCAL-YEAR-PERIOD-W1-W2-W3-ISSUANCE-001`.
- W2: List/Get/Request protected action/Approve/Reject/Return/Reopen; `GEN012.View/Execute/Approve/Reject/Return/Reopen`.
- W3: `FISCAL_YEAR_PERIOD_TYPED_SCREENDEFINITIONS_2026-08-22.md`.
- Test input: `FISCAL_YEAR_PERIOD_ACCEPTANCE_TEST_SPECIFICATIONS_2026-08-22.md` — issued, runtime not run.

## ANALYSIS — TEAM-D01 PASS
Purpose: provide a controlled fiscal-year lifecycle surface over existing FiscalYear records; no direct Create/Edit route is issued by this screen contract.

Display/filter fields:
- `CompanyId` — authorized Company scope, filter/display only.
- `Code` — display only, unique inside Company.
- `StartDate`, `EndDate` — display only fiscal range.
- `Status` — display-only lifecycle state.
- `Version` — hidden technical token for protected action/Reopen.

Capabilities:
- View
- Execute protected action
- Approve
- Reject
- Return
- Reopen
- server paging

## LAYOUT — TEAM-D02 PASS
Shared `ControlApproval / Standard` contract:
`ScreenHost → Toolbar → ContentHost → SearchPanel(Content) → ControlListGrid(Fill) → Pagination → ApprovalHistoryHost → AuditPanel`.
No tabs or local toolbar/grid/pagination/audit implementation.

## FIELD_GRID — TEAM-D03 PASS
`AutoGenerateColumns=false`, `SelectionPolicy=SingleRow`, `UsesServerPaging=true`.
Grid columns: `Code`, `StartDate`, `EndDate`, `Status`.
Filters: authorized Company scope, `Status`, date range and action context; sort allow-list only `code|startDate|endDate|status`. No default page-size value is invented.

## UX — TEAM-D04 PASS
- Selecting a fiscal year exposes read-only target data and server-authorized control actions.
- `Request protected action` sends only an action chosen from server-authorized choices plus target expectedVersion/reason as required; client does not create an action-code allow-list.
- Approve/Reject/Return act on the current `ApprovalRequestDto` through shared approval history and current request Version.
- target state is rechecked by server at decision time; SoD/self-decision prohibition remains server-authoritative.
- Reopen requires reason + current target Version and uses the issued route only.
- direct Status edit, Create/Edit/Delete/Disable/Enable, direct Close/Open/Lock commands, Print/Export and offline actions are absent.
- concurrency/approval-state/transition errors use shared presenters; no silent overwrite.

## VISUAL — TEAM-D05 PASS
Use shared ControlApproval CoreUI: RTL/DPI, search/filter presentation, read-only control grid, action/approval-history presenters, loading/error/validation/audit. No local colors, dimensions or approval UI clone.

## Acceptance criteria
1. W1 fields are read-only in this screen.
2. state transition is only through issued protected action or Reopen.
3. approval decisions respect current request Version and SoD.
4. explicit four-column grid and server paging.
5. no invented action codes or direct Close/Open/Lock control.
6. no Create/Edit/Delete/Print/Export/Disable/Enable/Offline capability.
7. no API/DTO/permission/DDL invention.

## INDEPENDENT REVIEW — TEAM-D06 PASS
W2/W3/acceptance cross-check confirmed read-only fiscal-year data, protected-action-only state control, ApprovalRequest version/state recheck, SoD, Reopen reason/version, no direct lifecycle command invention and no unissued CRUD/print/offline actions. Open design findings: `0`.

Runtime tests remain `NOT RUN`; design approval is not runtime PASS.
