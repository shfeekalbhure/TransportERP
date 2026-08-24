# ACC-037 — مراكز التكلفة — Canonical Screen Specification

**English:** Cost Center Tree  
**Profile / Variant:** `TreeMaster / Standard`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**Batch:** `BATCH-08`

## Authority
- Owner issuance: `SRC-061 / OWNER-ACCOUNT-COSTCENTER-TREE-W1-W2-W3-ISSUANCE-001`.
- W2: CostCenter List/Get/Create/Update/Disable/Children/Move; `ACC037.View/Create/Edit/Disable/Move`.
- W3: `ACCOUNT_COSTCENTER_TREE_TYPED_SCREENDEFINITIONS_2026-08-22.md`.
- Test input: `ACCOUNT_COSTCENTER_TREE_ACCEPTANCE_TEST_SPECIFICATIONS_2026-08-22.md` — issued, runtime not run.

## ANALYSIS — TEAM-D01 PASS
Purpose: maintain a company-scoped hierarchical cost-center tree with optional branch reference and controlled re-parenting.

Fields:
- `CompanyId` — required Company lookup — Create only.
- `BranchId` — optional Branch lookup — Create/Edit; current authority validates reference/scope without issuing an extra cross-company compatibility rule.
- `ParentCostCenterId` — optional tree parent — Create only; null=root; afterward changed only by Move.
- `Code` — required — Create/Edit — unique in Company.
- `ArabicName` — required — Create/Edit.
- `EnglishName` — optional — Create/Edit.
- `Status` — read-only `Active|Stopped` projection.
- `Version` — hidden expectedVersion token for Update/Disable/Move.

Capabilities: View/Create/Edit/Disable/Move/server paging.

## LAYOUT — TEAM-D02 PASS
Shared `TreeMaster / Standard`:
`TransportScreenHost → Toolbar → ContentHost → TreeMasterContent → SearchPanel + TreeGrid(Fill) → Pagination → AuditPanel`.
RTL, SingleRow, lazy children, server paging. No local Tree/Grid/Pagination/Audit replacement.

## FIELD_GRID — TEAM-D03 PASS
`AutoGenerateColumns=false`, `SelectionPolicy=SingleRow`, `UsesServerPaging=true`, lazy children through issued children contract.
Tree/grid columns:
1. `Code` — الرمز.
2. `ArabicName` — الاسم العربي — primary Fill.
3. `EnglishName` — الاسم الإنجليزي.
4. `Branch` — الفرع.
5. `Status` — الحالة — read-only.

Filters: `SearchText`, `Status`, `CompanyId`, `BranchId`, `ParentCostCenterId`; server allow-listed sort only. No numeric page default/cap is invented.

## UX — TEAM-D04 PASS
- Create root with ParentCostCenterId=null or child under a valid in-scope parent.
- CompanyId and initial parent are immutable through Update.
- BranchId may be edited only as issued; server reference/scope validation remains authoritative and the client does not invent an extra cross-company compatibility formula.
- Move is distinct: null target moves to root; valid parent reparents; expectedVersion required.
- self/descendant move maps to hierarchy-cycle error; invalid parent maps to invalid-parent error; no client-side cycle engine.
- lazy tree expansion loads children server-side only.
- Disable requires reason + expectedVersion and does not delete/cascade.
- no CostCenter Statement, JournalLine/posting behavior, Print/Export/Delete/Enable/Attachments/offline queue.
- shared loading/error/validation/concurrency presenters only.

## VISUAL — TEAM-D05 PASS
Shared TreeMaster CoreUI owns RTL/DPI, tree indentation/expand-collapse, selection/focus, lookup/text/state rendering, pagination and audit. No local tree graphics or pixel/style overrides.

## Acceptance criteria
1. true TreeMaster with lazy paged children.
2. Company and parent immutable through Update; parent changes only via Move.
3. optional Branch follows issued reference/scope validation only.
4. five explicit columns and server paging.
5. no CostCenter statement/JournalLine/posting UX.
6. no Print/Export/Delete/Enable/offline capability.
7. no API/DTO/permission/DDL invention.

## INDEPENDENT REVIEW — TEAM-D06 PASS
W2/W3/acceptance cross-check confirmed lazy TreeMaster behavior, immutable Company/parent on Update, Move-only reparenting with hierarchy errors, optional Branch reference without invented cross-company rule and absence of posting/report/offline capabilities. Open design findings: `0`.

Runtime tests remain `NOT RUN`; design approval is not runtime PASS.
