# GEN-008 — العملات — Canonical Screen Specification

**English:** Currency  
**Profile / Variant:** `MasterData / Standard`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**Batch:** `BATCH-04`

## Authority
- Owner issuance: `SRC-057 / OWNER-CURRENCY-LANGUAGE-W1-W2-W3-ISSUANCE-001`.
- W2: Currency List/Get/Create/Update/Disable; `GEN008.View/Create/Edit/Disable`.
- W3: `CURRENCY_LANGUAGE_TYPED_SCREENDEFINITIONS_2026-08-22.md`.
- Test input: `CURRENCY_LANGUAGE_ACCEPTANCE_TEST_SPECIFICATIONS_2026-08-22.md` — issued, runtime not run.

## ANALYSIS — TEAM-D01 PASS
Purpose: maintain the global Currency reference master without owning company base-currency selection.

Fields:
- `Code` required, global unique, Create/Edit.
- `ArabicName` required, Create/Edit.
- `EnglishName` optional, Create/Edit.
- `Symbol` optional, Create/Edit.
- `DecimalPlaces` required integer `0..6`; Create default=2.
- `Status` read-only `Active|Stopped` projection.
- `Version` hidden expectedVersion token.

Capabilities: View/Create/Edit/Disable/server paging only. No Print/Export/Delete/Enable/Activate/Move.

## LAYOUT — TEAM-D02 PASS
Shared `MasterData / Standard`: MainData=`Content` two columns maximum, Search=`Content`, MasterListGrid=`Fill`, shared Pagination/Audit, no Tabs or LocalException.

## FIELD_GRID — TEAM-D03 PASS
`AutoGenerateColumns=false`, `SelectionPolicy=SingleRow`, `UsesServerPaging=true`.
Grid columns: `Code`, `ArabicName`, `EnglishName`, `Symbol`, `DecimalPlaces`, `Status`.
Search: `SearchText`, `Status`; allow-listed sort: code/arabicName/englishName/symbol/decimalPlaces/status.

## UX — TEAM-D04 PASS
- create/edit fields are exactly the issued Currency DTO surface.
- disable requires reason + expectedVersion; direct Status editing is prohibited.
- stale version uses shared concurrency Reload/Refresh.
- `IsBaseCurrency` and `Company.BaseCurrencyId` are absent; no company side effect or inference.
- shared validation/loading/error/paging only; online authoritative writes only.

## VISUAL — TEAM-D05 PASS
Shared MasterData CoreUI owns RTL/DPI/typography/spacing/grid/pagination/audit and field states. No local colors, fixed sizes or component clones.

## Acceptance criteria
1. DecimalPlaces constrained to 0..6 with Create default 2.
2. six explicit grid columns and server paging.
3. no IsBaseCurrency/BaseCurrency control or side effect.
4. no Print/Export/Delete/Enable/Move/offline capability.
5. no API/DTO/permission/DDL invention.

## INDEPENDENT REVIEW — TEAM-D06 PASS
Acceptance/W2/W3 cross-check confirmed exact Currency field surface, decimal policy, server paging/sort, reasoned Disable, concurrency handling, absence of IsBaseCurrency/Company side effects and prohibited Print/Export/Delete/Enable/Offline. Open design findings: `0`.

Runtime tests remain `NOT RUN`; design approval is not runtime PASS.
