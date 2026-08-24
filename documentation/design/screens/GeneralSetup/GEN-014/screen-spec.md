# GEN-014 — اللغات — Canonical Screen Specification

**English:** Language  
**Profile / Variant:** `MasterData / Standard`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**Batch:** `BATCH-04`

## Authority
- Owner issuance: `SRC-057 / OWNER-CURRENCY-LANGUAGE-W1-W2-W3-ISSUANCE-001`.
- W2: Language List/Get/Create/Update/Disable; `GEN014.View/Create/Edit/Disable`.
- W3: `CURRENCY_LANGUAGE_TYPED_SCREENDEFINITIONS_2026-08-22.md`.
- Test input: `CURRENCY_LANGUAGE_ACCEPTANCE_TEST_SPECIFICATIONS_2026-08-22.md` — issued, runtime not run.

## ANALYSIS — TEAM-D01 PASS
Purpose: maintain supported language/culture references and their declared text direction.

Fields:
- `Code` required, globally unique, Create/Edit.
- `CultureCode` required, globally unique, Create/Edit.
- `Direction` required enum `RTL|LTR`, Create/Edit.
- `Status` read-only `Active|Stopped` projection.
- `Version` hidden expectedVersion token.

Capabilities: View/Create/Edit/Disable/server paging only. No Print/Export/Delete/Enable/Activate/Move.

## LAYOUT — TEAM-D02 PASS
Shared `MasterData / Standard`: MainData=`Content`, Search=`Content`, MasterListGrid=`Fill`, shared Pagination/Audit, no Tabs or LocalException.

## FIELD_GRID — TEAM-D03 PASS
`AutoGenerateColumns=false`, `SelectionPolicy=SingleRow`, `UsesServerPaging=true`.
Grid columns: `Code`, `CultureCode`, `Direction`, `Status`.
Search: `SearchText`, `Status`, `Direction`; allow-listed sort: code/cultureCode/direction/status.

## UX — TEAM-D04 PASS
- only issued Code/CultureCode/Direction fields are editable; no ArabicName/EnglishName fields are invented.
- `Direction` is a data value for the language record; it does not locally override the current screen's CoreUI direction.
- disable requires reason + expectedVersion; Status is not directly editable.
- stale version uses shared concurrency Reload/Refresh.
- shared validation/loading/error/paging only; online authoritative writes only.

## VISUAL — TEAM-D05 PASS
The screen itself follows shared CoreUI RTL/DPI/typography/spacing regardless of the selected record's Direction value. Grid/pagination/audit and state presenters remain central; no local styling.

## Acceptance criteria
1. exact fields are Code, CultureCode, Direction, Status, Version.
2. Direction accepts RTL or LTR only.
3. four explicit grid columns with server paging.
4. no invented display names or Print/Export/Delete/Enable/Move/offline behavior.
5. no API/DTO/permission/DDL invention.

## INDEPENDENT REVIEW — TEAM-D06 PASS
Acceptance/W2/W3 cross-check confirmed exact Language fields, unique Code/CultureCode, RTL/LTR Direction, reasoned Disable, concurrency handling and absence of unissued display names or Print/Export/Delete/Enable/Offline. Open design findings: `0`.

Runtime tests remain `NOT RUN`; design approval is not runtime PASS.
