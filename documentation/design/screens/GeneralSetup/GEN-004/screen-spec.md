# GEN-004 — المحافظات — Canonical Screen Specification

**English:** Governorates  
**Profile / Variant:** `MasterData / Standard`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**Batch:** `BATCH-03`

## Authority
- Owner issuance: `SRC-056 / OWNER-GEOGRAPHY-HIERARCHY-W1-W2-W3-ISSUANCE-001`.
- W1: Governorate with required `CountryId` parent; global scope.
- W2: Governorate List/Get/Create/Update/Disable; `GEN004.View/Create/Edit/Disable`.
- W3: `GEOGRAPHY_HIERARCHY_TYPED_SCREENDEFINITIONS_2026-08-22.md`.
- Test input: `GEOGRAPHY_HIERARCHY_ACCEPTANCE_TEST_SPECIFICATIONS_2026-08-22.md` — issued, runtime not run.

## ANALYSIS — TEAM-D01 PASS
Purpose: maintain governorates as children of Country in the global reference hierarchy.

Fields:
- `CountryId` — الدولة — parent lookup — required — Create only.
- `Code` — الرمز — required — Create/Edit — unique within Country.
- `ArabicName` — الاسم العربي — required — Create/Edit.
- `EnglishName` — الاسم الإنجليزي — optional — Create/Edit.
- `Status` — الحالة — read-only `Active|Stopped` projection.
- `Version` — hidden technical token for Update/Disable.

Capabilities: View, Create, Edit, Disable, server paging only. No Print/Export/Delete/Enable/Activate/Move.

## LAYOUT — TEAM-D02 PASS
Shared `MasterData / Standard`:
- MainData `Content`, two columns maximum.
- SearchPanel `Content`.
- MasterListGrid `Fill`.
- shared Pagination + Audit.
- no Tabs, no LocalException, no local CoreUI replacement.

## FIELD_GRID — TEAM-D03 PASS
`AutoGenerateColumns=false`, `SelectionPolicy=SingleRow`, `UsesServerPaging=true`.

Grid columns:
1. `Code` — الرمز — text/code — content.
2. `ArabicName` — الاسم العربي — text — primary Fill.
3. `EnglishName` — الاسم الإنجليزي — text — Fill/content.
4. `Status` — الحالة — enum/read-only — content state.

Search: `SearchText`, `CountryId`, `Status`; allow-listed server sort only. `CountryId` lookup uses current Country List/Search with `GEN003.View`; no new route/provider is created.

## UX — TEAM-D04 PASS
- parent Country is selected on Create and is immutable afterward; Update cannot replace parent.
- create/edit/disable are permission-bound; API remains authoritative.
- disable requires reason + current expectedVersion; no direct Status edit.
- stale version uses shared concurrency Reload/Refresh, never silent overwrite.
- shared validation/loading/error/empty/paging behavior only.
- online authoritative writes only; no queue/outbox.

## VISUAL — TEAM-D05 PASS
Use shared MasterData CoreUI typography, spacing, RTL/DPI, lookup/text/state presenters, grid/pagination/audit and focus/error states. No local colors, fixed metrics, toolbar/grid clones or visual LocalException.

## Acceptance criteria
1. Country→Governorate parent relation preserved.
2. parent is Create-only and server validated.
3. explicit four-column grid with server paging.
4. no Print/Export/Delete/Enable/Activate/Move/offline capability.
5. no API/DTO/permission/DDL invention.

## INDEPENDENT REVIEW — TEAM-D06 PASS
Current W2, atomic trace and acceptance specification confirm parent-aware Create, Code/names-only Update, immutable Country parent, reasoned Disable, server paging/allow-listed sort and prohibited Print/Delete/Enable/Move/Offline. Open design findings: `0`.

Runtime tests remain `NOT RUN`; design approval is not runtime PASS.
