# ACC-035 — دليل الحسابات — Canonical Screen Specification

**English:** Account Tree  
**Profile / Variant:** `TreeMaster / Standard`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**Batch:** `BATCH-08`

## Authority
- Owner issuance: `SRC-061 / OWNER-ACCOUNT-COSTCENTER-TREE-W1-W2-W3-ISSUANCE-001`.
- W2: Account List/Get/Create/Update/Disable/Children/Move; `ACC035.View/Create/Edit/Disable/Move`.
- W3: `ACCOUNT_COSTCENTER_TREE_TYPED_SCREENDEFINITIONS_2026-08-22.md`.
- Test input: `ACCOUNT_COSTCENTER_TREE_ACCEPTANCE_TEST_SPECIFICATIONS_2026-08-22.md` — issued, runtime not run.

## ANALYSIS — TEAM-D01 PASS
Purpose: maintain a company-scoped hierarchical chart of accounts with controlled re-parenting and lazy child retrieval.

Fields:
- `CompanyId` — required Company lookup — Create only.
- `ParentAccountId` — optional tree parent — Create only; null=root; afterward changed only by Move.
- `AccountGroupId` — required reference — Create/Edit — existing ACC036 selector authority only.
- `AccountTypeId` — required reference — Create/Edit — existing ACC036 selector authority only.
- `Code` — required — Create/Edit — unique in Company.
- `ArabicName` — required — Create/Edit.
- `EnglishName` — optional — Create/Edit.
- `IsPostable` — required Boolean — Create/Edit; no leaf/posting semantics inferred.
- `CurrencyPolicy` — hidden diagnostic — must remain null; no currency control.
- `Status` — read-only `Active|Stopped` projection.
- `Version` — hidden expectedVersion token for Update/Disable/Move.

Capabilities: View/Create/Edit/Disable/Move/server paging.

## LAYOUT — TEAM-D02 PASS
Shared `TreeMaster / Standard`:
`TransportScreenHost → Toolbar → ContentHost → TreeMasterContent → SearchPanel + TreeGrid(Fill) → Pagination → AuditPanel`.
RTL, SingleRow, lazy children, server paging. No local Tree/Grid/Pagination/Audit replacement and no AccountBranchScope tab/grid.

## FIELD_GRID — TEAM-D03 PASS
`AutoGenerateColumns=false`, `SelectionPolicy=SingleRow`, `UsesServerPaging=true`, lazy children via issued children contract.
Tree/grid columns:
1. `Code` — الرمز.
2. `ArabicName` — الاسم العربي — primary Fill.
3. `EnglishName` — الاسم الإنجليزي.
4. `Group` — المجموعة.
5. `Type` — النوع.
6. `IsPostable` — قابل للترحيل.
7. `Status` — الحالة — read-only.

Filters: `SearchText`, `Status`, `CompanyId`, `ParentAccountId`, `AccountGroupId`, `AccountTypeId`, `IsPostable`; server allow-listed sort only. No numeric page default/cap is invented.

## UX — TEAM-D04 PASS
- Create root with ParentAccountId=null or child under a valid in-scope parent.
- CompanyId and initial parent are immutable through Update.
- Move is a distinct capability: `newParentId=null` moves to root; valid parent reparents; expectedVersion required.
- self/descendant target displays shared hierarchy-cycle error; invalid parent uses invalid-parent error; client does not try to reconstruct authoritative cycle rules.
- lazy expansion requests children server-side; no full-tree preload.
- Disable requires reason + expectedVersion and does not delete or reparent descendants.
- AccountGroup/AccountType use existing reference selectors; no ACC036 API/DTO is invented here.
- `CurrencyPolicy` has no editor and must remain null; `IsPostable` is not treated as proof of leaf-only posting behavior.
- no AccountBranchScope surface, Print/Export/Delete/Enable/Attachments/offline queue.
- shared loading/error/validation/concurrency presenters only.

## VISUAL — TEAM-D05 PASS
Shared TreeMaster CoreUI owns RTL/DPI, indentation/expand-collapse states, focus/selection, text/lookup/boolean/state rendering, pagination and audit. No local tree graphics, raw colors, pixel widths or component clones.

## Acceptance criteria
1. true TreeMaster with lazy paged children.
2. Company and parent cannot be changed by Update; parent changes only through Move.
3. Move handles root/valid reparent and rejects self/descendant/invalid parent through server contract.
4. seven explicit columns and server paging.
5. CurrencyPolicy remains hidden/null-only; no currency lookup/control.
6. no AccountBranchScope or unissued Print/Export/Delete/Enable/offline capability.
7. no API/DTO/permission/DDL invention.

## INDEPENDENT REVIEW — TEAM-D06 PASS
W2/W3/acceptance cross-check confirmed true lazy TreeMaster behavior, immutable Company/parent on Update, Move-only reparenting with hierarchy errors, null-only CurrencyPolicy, no AccountBranchScope and no prohibited capabilities. Open design findings: `0`.

Runtime tests remain `NOT RUN`; design approval is not runtime PASS.
