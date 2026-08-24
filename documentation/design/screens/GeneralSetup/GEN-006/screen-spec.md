# GEN-006 — المدن — Canonical Screen Specification

**English:** Cities  
**Profile / Variant:** `MasterData / Standard`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**Batch:** `BATCH-03`

## Authority
- Owner issuance: `SRC-056 / OWNER-GEOGRAPHY-HIERARCHY-W1-W2-W3-ISSUANCE-001`.
- W1: City with required `DirectorateId` parent; global scope.
- W2: City List/Get/Create/Update/Disable; `GEN006.View/Create/Edit/Disable`.
- W3: `GEOGRAPHY_HIERARCHY_TYPED_SCREENDEFINITIONS_2026-08-22.md`.
- Test input: `GEOGRAPHY_HIERARCHY_ACCEPTANCE_TEST_SPECIFICATIONS_2026-08-22.md` — issued, runtime not run.

## ANALYSIS — TEAM-D01 PASS
Purpose: maintain cities as children of Directorate in the global reference hierarchy.

Fields:
- `DirectorateId` — المديرية — parent lookup — required — Create only.
- `Code` — الرمز — required — Create/Edit — unique within Directorate.
- `ArabicName` — الاسم العربي — required — Create/Edit.
- `EnglishName` — الاسم الإنجليزي — optional — Create/Edit.
- `Status` — الحالة — read-only `Active|Stopped` projection.
- `Version` — hidden technical token for Update/Disable.

Capabilities: View, Create, Edit, Disable, server paging only. No Print/Export/Delete/Enable/Activate/Move.

## LAYOUT — TEAM-D02 PASS
Shared `MasterData / Standard`: MainData=`Content`, Search=`Content`, MasterListGrid=`Fill`, shared Pagination/Audit, no Tabs, no LocalException.

## FIELD_GRID — TEAM-D03 PASS
`AutoGenerateColumns=false`, `SelectionPolicy=SingleRow`, `UsesServerPaging=true`.

Grid columns:
1. `Code` — الرمز — text/code — content.
2. `ArabicName` — الاسم العربي — text — primary Fill.
3. `EnglishName` — الاسم الإنجليزي — text — Fill/content.
4. `Status` — الحالة — enum/read-only — content state.

Search: `SearchText`, `DirectorateId`, `Status`; allow-listed server sort only. Parent lookup uses current Directorate List/Search with `GEN005.View`; no new route/provider.

## UX — TEAM-D04 PASS
- parent Directorate is Create-only and immutable afterward.
- create/edit/disable are permission-bound; server validates hierarchy and scope.
- disable requires reason + expectedVersion; Status is not directly editable.
- stale version uses shared concurrency Reload/Refresh; shared validation/loading/error/paging only.
- online authoritative writes only; no queue/outbox.

## VISUAL — TEAM-D05 PASS
Shared MasterData CoreUI only: central RTL/DPI/typography/spacing, lookup/text/state presenters, grid/pagination/audit. No local styling or cloned components.

## Acceptance criteria
1. Directorate→City hierarchy preserved.
2. parent immutable after Create.
3. explicit four-column grid with server paging.
4. no unissued Print/Export/Delete/Enable/Activate/Move/offline capability.
5. no API/DTO/permission/DDL invention.

## INDEPENDENT REVIEW — TEAM-D06 PASS
Current W2, atomic trace and acceptance specification confirm immutable Directorate parent on Update, stale-version conflict handling, reasoned Disable and absence of Print/Delete/Enable/Move/Offline. Open design findings: `0`.

Runtime tests remain `NOT RUN`; design approval is not runtime PASS.
