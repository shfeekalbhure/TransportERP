# GEN-005 — المديريات — Canonical Screen Specification

**English:** Directorates  
**Profile / Variant:** `MasterData / Standard`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**Batch:** `BATCH-03`

## Authority
- Owner issuance: `SRC-056 / OWNER-GEOGRAPHY-HIERARCHY-W1-W2-W3-ISSUANCE-001`.
- W1: Directorate with required `GovernorateId` parent; global scope.
- W2: Directorate List/Get/Create/Update/Disable; `GEN005.View/Create/Edit/Disable`.
- W3: `GEOGRAPHY_HIERARCHY_TYPED_SCREENDEFINITIONS_2026-08-22.md`.
- Test input: `GEOGRAPHY_HIERARCHY_ACCEPTANCE_TEST_SPECIFICATIONS_2026-08-22.md` — issued, runtime not run.

## ANALYSIS — TEAM-D01 PASS
Purpose: maintain directorates as children of Governorate in the global reference hierarchy.

Fields:
- `GovernorateId` — المحافظة — parent lookup — required — Create only.
- `Code` — الرمز — required — Create/Edit — unique within Governorate.
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

Search: `SearchText`, `GovernorateId`, `Status`; allow-listed server sort only. Parent lookup uses current Governorate List/Search with `GEN004.View`; no new route/provider.

## UX — TEAM-D04 PASS
- parent Governorate is Create-only and immutable after creation.
- create/edit/disable remain permission-bound; server rechecks authority and parent validity.
- disable requires reason + expectedVersion; no direct Status edit.
- stale version uses shared concurrency Reload/Refresh.
- shared validation/loading/error/empty/paging only; online writes only.

## VISUAL — TEAM-D05 PASS
Shared MasterData CoreUI only: RTL, DPI, typography, spacing, lookup/text/state states, grid/pagination/audit. No local colors, metrics or cloned components.

## Acceptance criteria
1. Governorate→Directorate hierarchy preserved.
2. parent cannot be replaced by Update.
3. explicit four-column grid and server paging.
4. no unissued capabilities or offline path.
5. no API/DTO/permission/DDL invention.

## INDEPENDENT REVIEW — TEAM-D06 PASS
Current W2, atomic trace and acceptance specification confirm parent-aware Create, immutable Governorate parent on Update, expectedVersion conflict handling, reasoned Disable and prohibited Print/Delete/Enable/Move/Offline. Open design findings: `0`.

Runtime tests remain `NOT RUN`; design approval is not runtime PASS.
