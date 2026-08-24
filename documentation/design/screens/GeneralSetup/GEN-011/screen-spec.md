# GEN-011 — الفروع — Canonical Screen Specification

**English:** Branch  
**Profile / Variant:** `MasterData / Standard`  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**OwnerTeam:** `TEAM-D06`  
**Batch:** `BATCH-05`

## Authority
- Owner issuance: `SRC-058 / OWNER-COMPANY-BRANCH-W1-W2-W3-ISSUANCE-001`.
- W2: Branch List/Get/Create/Update/Disable; `GEN011.View/Create/Edit/Disable`.
- W3: `COMPANY_BRANCH_TYPED_SCREENDEFINITIONS_2026-08-22.md`.
- Test input: `COMPANY_BRANCH_ACCEPTANCE_TEST_SPECIFICATIONS_2026-08-22.md` — issued, runtime not run.

## ANALYSIS — TEAM-D01 PASS
Purpose: maintain branch/service-center master data under an authorized company scope while preserving only the geographic references actually issued.

Fields:
- `CompanyId` — required Company lookup — Create only — scope verified.
- `Code` — required — Create/Edit — unique inside Company.
- `ArabicName` — required — Create/Edit.
- `EnglishName` — optional — Create/Edit.
- `CountryId` — optional Country lookup — Create/Edit.
- `GovernorateId` — optional Governorate lookup — Create/Edit.
- `CityId` — optional City lookup — Create/Edit.
- `Status` — read-only `Active|Stopped` projection.
- `Version` — hidden expectedVersion token.

Capabilities: View/Create/Edit/Disable/server paging only. No Print/Export/Delete/Enable/Activate/Move.

## LAYOUT — TEAM-D02 PASS
Shared `MasterData / Standard`: MainData=`Content` max two columns, Search=`Content`, MasterListGrid=`Fill`, shared Pagination/Audit; no Tabs or LocalException.

## FIELD_GRID — TEAM-D03 PASS
`AutoGenerateColumns=false`, `SelectionPolicy=SingleRow`, `UsesServerPaging=true`.
Grid columns: `Company`, `Code`, `ArabicName`, `EnglishName`, `Status`.
Search: `SearchText`, `Status`, `CompanyId`, `CountryId`, `GovernorateId`, `CityId`; server allow-listed sort only.
Lookups consume current Company/Country/Governorate/City reference contracts and permissions; no new lookup routes are created.

## UX — TEAM-D04 PASS
- `CompanyId` is selected on Create and cannot be replaced by Update.
- optional Country/Governorate/City values use server existence/scope validation only; the client does not invent a hierarchy-consistency rule among them.
- create/edit/disable remain permission-bound and server-authoritative.
- disable requires reason + expectedVersion; Status is not directly editable.
- stale version uses shared conflict Reload/Refresh.
- shared validation/loading/error/empty/paging only; no Move or offline path.

## VISUAL — TEAM-D05 PASS
Shared MasterData CoreUI only: RTL/DPI/typography/spacing, lookup/text/state presenters, grid/pagination/audit. No local colors, fixed metrics or component clones.

## Acceptance criteria
1. CompanyId required on Create and immutable afterward.
2. Code unique within Company.
3. Country/Governorate/City remain optional issued references with existence/scope checks only.
4. explicit five-column grid with server paging.
5. no invented geographic-consistency rule or Move/Print/Export/Delete/Enable/offline capability.
6. no API/DTO/permission/DDL invention.

## Handoff
Completed: `ANALYSIS`, `LAYOUT`, `FIELD_GRID`, `UX`, `VISUAL`.  
Current: `INDEPENDENT_REVIEW` — `TEAM-D06`.
