# GEN-010 — الشركات — Canonical Screen Specification

**English:** Company  
**Profile / Variant:** `MasterData / Standard`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**Batch:** `BATCH-05`

## Authority
- Owner issuance: `SRC-058 / OWNER-COMPANY-BRANCH-W1-W2-W3-ISSUANCE-001`.
- W2: Company List/Get/Create/Update/Disable; `GEN010.View/Create/Edit/Disable`.
- W3: `COMPANY_BRANCH_TYPED_SCREENDEFINITIONS_2026-08-22.md`.
- Test input: `COMPANY_BRANCH_ACCEPTANCE_TEST_SPECIFICATIONS_2026-08-22.md` — issued, runtime not run.

## ANALYSIS — TEAM-D01 PASS
Purpose: maintain company/legal-entity master data and its selected base-currency/country references without absorbing Branch management into the screen.

Fields:
- `Code` — required — Create/Edit — globally unique.
- `ArabicName` — required — Create/Edit.
- `EnglishName` — optional — Create/Edit.
- `BaseCurrencyId` — required Currency lookup — Create/Edit — server validated via current Currency reference authority.
- `CountryId` — optional Country lookup — Create/Edit — server validated.
- `Status` — read-only `Active|Stopped` projection.
- `Version` — hidden expectedVersion token for Update/Disable.

Capabilities: View/Create/Edit/Disable/server paging only. No Print/Export/Delete/Enable/Activate/Move.

## LAYOUT — TEAM-D02 PASS
Shared `MasterData / Standard`: MainData=`Content` with max two columns, Search=`Content`, MasterListGrid=`Fill`, shared Pagination/Audit. No Tabs, no child Branch grid, no LocalException.

## FIELD_GRID — TEAM-D03 PASS
`AutoGenerateColumns=false`, `SelectionPolicy=SingleRow`, `UsesServerPaging=true`.
Grid columns: `Code`, `ArabicName`, `EnglishName`, `Status`.
Search: `SearchText`, `Status`, `CountryId`, `BaseCurrencyId`; server allow-listed sort only.
`BaseCurrencyId` uses current Currency List/Search with `GEN008.View`; `CountryId` uses Country List/Search with `GEN003.View`. No new lookup route/provider is created.

## UX — TEAM-D04 PASS
- Company base-currency selection is edited through `BaseCurrencyId` only; the design does not expose Currency-side `IsBaseCurrency`.
- no Branch child-management surface or side effect is introduced.
- create/edit/disable remain permission-bound and server-authoritative.
- disable requires reason + expectedVersion; Status is not directly editable.
- stale version uses shared conflict Reload/Refresh; no silent overwrite.
- shared validation/loading/error/empty/paging behavior only; online authoritative writes only.

## VISUAL — TEAM-D05 PASS
Shared MasterData CoreUI only: RTL/DPI, typography, spacing, lookup/text/state presenters, grid, pagination and audit. No local colors, dimensions or component clones.

## Acceptance criteria
1. BaseCurrencyId is required and server validated.
2. CountryId remains optional and server validated.
3. explicit four-column grid with server paging.
4. no child Branch tab/grid or Currency IsBaseCurrency control.
5. no Print/Export/Delete/Enable/Move/offline capability.
6. no API/DTO/permission/DDL invention.

## INDEPENDENT REVIEW — TEAM-D06 PASS
W2/W3/acceptance cross-check confirmed required BaseCurrencyId, optional CountryId, no Currency-side IsBaseCurrency control, no Branch child-management surface, controlled Disable and server-authoritative scope. Open design findings: `0`.

Runtime tests remain `NOT RUN`; design approval is not runtime PASS.
