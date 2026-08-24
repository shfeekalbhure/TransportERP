# GEN-007 — المناطق — Canonical Screen Specification

**English:** Areas  
**Profile / Variant:** `MasterData / Standard`  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**OwnerTeam:** `TEAM-D06`  
**Batch:** `BATCH-03`

## Authority
- Owner issuance: `SRC-056 / OWNER-GEOGRAPHY-HIERARCHY-W1-W2-W3-ISSUANCE-001`.
- W1: Area with required `CityId` parent; global scope.
- W2: Area List/Get/Create/Update/Disable; `GEN007.View/Create/Edit/Disable`.
- W3: `GEOGRAPHY_HIERARCHY_TYPED_SCREENDEFINITIONS_2026-08-22.md`.
- Test input: `GEOGRAPHY_HIERARCHY_ACCEPTANCE_TEST_SPECIFICATIONS_2026-08-22.md` — issued, runtime not run.

## ANALYSIS — TEAM-D01 PASS
Purpose: maintain areas as children of City in the global reference hierarchy.

Fields:
- `CityId` — المدينة — parent lookup — required — Create only.
- `Code` — الرمز — required — Create/Edit — unique within City.
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

Search: `SearchText`, `CityId`, `Status`; allow-listed server sort only. Parent lookup uses current City List/Search with `GEN006.View`; no new route/provider.

## UX — TEAM-D04 PASS
- parent City is Create-only and immutable after creation.
- create/edit/disable are permission-bound; server validates hierarchy and scope.
- disable requires reason + expectedVersion; no direct Status edit.
- stale version uses shared concurrency Reload/Refresh; shared validation/loading/error/paging only.
- online authoritative writes only; no queue/outbox.

## VISUAL — TEAM-D05 PASS
Shared MasterData CoreUI only: RTL, DPI, typography, spacing, lookup/text/state presenters, grid/pagination/audit. No local colors, sizes, toolbar/grid clones or LocalException.

## Acceptance criteria
1. City→Area hierarchy preserved.
2. parent immutable after Create.
3. explicit four-column grid with server paging.
4. no unissued Print/Export/Delete/Enable/Activate/Move/offline capability.
5. no API/DTO/permission/DDL invention.

## Handoff
Completed: `ANALYSIS`, `LAYOUT`, `FIELD_GRID`, `UX`, `VISUAL`.  
Current: `INDEPENDENT_REVIEW` — `TEAM-D06`.
