# TransportERP WAVE-1 — 13 Current-Approved Screen Readiness Evidence

**Branch:** `wave1-screen-readiness-20260822`  
**Rule:** No guessing. A screen is not READY merely because code compiles or CI is green. Any unclosed authority, W1/W2/W3, accounting, visual, or exact-SHA evidence gap remains an explicit gate.

## Current authority freeze

Current authority is **Current Approved References V1.26 + Unified Design V1.3 + P0 Authority Decision Record 2026-08-22**. The governing identity baseline is **57 Current Approved identities**; the 51 master catalog targets remain mapping/recovery/navigation targets and do not replace the 57 identities.

WAVE-1 is governed by these 13 Current Approved identities:

`GEN-003`, `GEN-004`, `GEN-005`, `GEN-006`, `GEN-007`, `GEN-013`, `GEN-014`, `ACC-036`, `ACC-074`, `ACC-075`, `ACC-049`, `ACC-050`, `ACC-058`.

Legacy targets such as `SET-001`, `SET-002`, `SET-011`, `FIN-028`, and `FIN-055` are crosswalk/navigation aliases only.

## Per-screen evidence state

| Screen | Current state | Proven basis | Remaining gate |
|---|---|---|---|
| GEN-003 | HOLD | SRC-055 closes full logical/W2/W3 specification design only and explicitly grants no DDL/code/runtime authority. | `W1_PHYSICAL_PROMOTION` for ISO2/ISO3/DialingCode; runtime/DDL withheld. |
| GEN-004 | IMPLEMENTED / REVIEW REQUIRED | Governorate implementation is present under the current geography hierarchy. | Exact-final-SHA independent review. |
| GEN-005 | IMPLEMENTED / REVIEW REQUIRED | Directorate implementation is present. | Exact-final-SHA independent review. |
| GEN-006 | IMPLEMENTED / REVIEW REQUIRED | City implementation is present. | Exact-final-SHA independent review. |
| GEN-007 | IMPLEMENTED / REVIEW REQUIRED | Area implementation is present. | Exact-final-SHA independent review. |
| GEN-013 | HOLD | Owner/business numbering semantics are partially resolved. | `W1_W3_FIELD_SEMANTICS`: NextValue→LastNumber supersession/migration and physical scope/FK/approval/concurrency evidence remain open; runtime and DI exposure withheld. |
| GEN-014 | IMPLEMENTED / REVIEW REQUIRED | Current W1/W2 contract is Id/Code/CultureCode/Direction/IsActive/Version with exact List/Get/Create/Update/Disable runtime. | Exact-final-SHA independent review. |
| ACC-036 | HOLD | W1 requires separate AccountGroup and AccountType. | `W1_W2_ENTITY_DTO_RECONCILIATION`: exact field-level DTO/entity discrimination and authorized physical implementation contract are NOT PROVEN. |
| ACC-074 | HOLD | W1/W2 require authoritative OpenItem/PaymentAllocation and customer/source-document joins. | `OPEN_ITEM_SOURCE_RECONCILIATION` remains NOT PROVEN. |
| ACC-075 | HOLD | W1/W2 require authoritative OpenItem/PaymentAllocation and supplier/source-document joins. | `OPEN_ITEM_SOURCE_RECONCILIATION` remains NOT PROVEN. |
| ACC-049 | IMPLEMENTED / REVIEW REQUIRED | Dedicated runtime uses posted JournalEntry/JournalLine/Account data. Tests prove original+reversal semantics, branch/currency isolation, drill-down, export/print and cap=200. | Exact-final-SHA CI then independent review. |
| ACC-050 | HOLD | Current implementation source still contains ReferenceType heuristic classification. | `OTS_W1_005_CASH_FLOW_CLASSIFICATION`; candidate/heuristic classification cannot authorize runtime. |
| ACC-058 | IMPLEMENTED / REVIEW REQUIRED | Dedicated runtime uses posted JournalEntry/JournalLine/Account data. Tests prove opening/period/closing, original+reversal semantics, branch/currency isolation, drill-down, export/print and cap=200. | Exact-final-SHA CI then independent review. |

## Runtime containment / exposure

1. `MapWave1ScreenCatalog` exposes only non-HOLD runtime identities.
2. GEN-003 is withheld inside geography registration.
3. GEN-013 numbering routes and DI service are withheld.
4. ACC-036 / ACC-050 / ACC-074 / ACC-075 routes and their held shared report DI service remain withheld.
5. ACC-049 is exposed through the dedicated `Wave1BalanceSheetService` and exact W2 API module only.
6. ACC-058 is exposed through the dedicated `Wave1DetailedTrialBalanceService` and exact W2 API module only.
7. ACC-049/058 exact action bindings remain Query/View, DrillDown/DrillDown, Export/Export, Print/Print over POST routes defined by W2.
8. All WAVE-1 report paging validators use authoritative `PageSize <= 200`.
9. Cleanup migration `20260822172500_Wave1HeldArtifactsCleanup` continues to remove non-governing `account_classifications` and `accounting_open_items` physical tables without inventing replacement authority.

## Accounting E2E reconciliation evidence

For ACC-049 and ACC-058 the new exact tests establish:

- source-of-truth is `Status == POSTED` journal entries/lines;
- draft entries are ignored;
- branch scope is isolated;
- currency scope is isolated;
- reversal is represented as a separate posted entry with `ReversalOfId` and inverse lines: the original remains in drill-down and the accounting effect nets correctly;
- drill-down returns both original and reversal when in scope;
- export and print payload generation is exercised;
- server/service paging cap is 200;
- `Wave1ReadinessCatalog` promotes only ACC-049/058 from HOLD to `IMPLEMENTED / REVIEW REQUIRED`; remaining authority gaps stay HOLD.

## Remaining governing blockers

1. `GEN-003 — W1_PHYSICAL_PROMOTION`
2. `GEN-013 — W1_W3_FIELD_SEMANTICS`
3. `ACC-036 — W1_W2_ENTITY_DTO_RECONCILIATION`
4. `ACC-074 — OPEN_ITEM_SOURCE_RECONCILIATION`
5. `ACC-075 — OPEN_ITEM_SOURCE_RECONCILIATION`
6. `ACC-050 — OTS_W1_005_CASH_FLOW_CLASSIFICATION`

These six blockers require governing authority/reconciliation that is not present in the Current source set. They must remain OPEN/HOLD/NOT PROVEN; no runtime promotion by inference is allowed.

## Hard gates

`CI GREEN != SCREEN READY`.

PR #58 remains **Draft / NOT READY / DO NOT MERGE** while any HOLD remains. Independent Review does not start until blockers=0. READY/Merge requires blockers=0, required CI green on one exact final SHA, all review threads resolved, documentation/traceability current, and independent reviewer PASS on that same exact SHA.
