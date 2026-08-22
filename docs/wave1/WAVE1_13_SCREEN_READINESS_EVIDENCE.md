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
| GEN-003 | HOLD | W2/W3 specification exists, but W1 physical persistence does not authorize ISO2/ISO3/DialingCode. | `W1_PHYSICAL_PROMOTION`. Keep runtime/DDL withheld unless a current governing W1 physical promotion is issued. |
| GEN-004 | IMPLEMENTED / REVIEW REQUIRED | Governorate implementation is present under the current geography hierarchy. | Exact-SHA runtime, negative/security and independent review evidence. |
| GEN-005 | IMPLEMENTED / REVIEW REQUIRED | Directorate implementation is present. | Exact-SHA runtime, negative/security and independent review evidence. |
| GEN-006 | IMPLEMENTED / REVIEW REQUIRED | City implementation is present. | Exact-SHA runtime, negative/security and independent review evidence. |
| GEN-007 | IMPLEMENTED / REVIEW REQUIRED | Area implementation is present. | Exact-SHA runtime, negative/security and independent review evidence. |
| GEN-013 | HOLD | W1 NumberSequence persists CompanyId/BranchId/FiscalYearId/DocumentType/NextValue/ResetRule/format/IsActive/Version. | `W1_W3_FIELD_SEMANTICS`: Code/ArabicName/EnglishName/Notes are not proven persistent; Scope binding is unresolved; LastNumber must not be inferred from NextValue. Runtime and DI service exposure are withheld. |
| GEN-014 | IMPLEMENTED / REVIEW REQUIRED | Current W1/W2 contract is implemented as Id/Code/CultureCode/Direction/IsActive/Version with List/Get/Create/Update/Disable only; translation/display-name persistence was removed. | Exact-final-SHA CI and independent review. |
| ACC-036 | HOLD | Current W1 defines separate AccountGroup and AccountType entities; W2 route/DTO type names exist. | `W1_W2_ENTITY_DTO_RECONCILIATION`: exact field-level DTO schema/entity discrimination and physical implementation contract are NOT PROVEN. |
| ACC-074 | HOLD | W1 source chain requires Customer/OpenItem/PaymentAllocation with authoritative joins. | `OPEN_ITEM_SOURCE_RECONCILIATION`: exact physical mapping and customer/source-document joins remain NOT PROVEN. |
| ACC-075 | HOLD | W1 source chain requires Supplier/OpenItem/PaymentAllocation with authoritative joins. | Same `OPEN_ITEM_SOURCE_RECONCILIATION` gate as ACC-074. |
| ACC-049 | HOLD | Balance sheet source-of-truth is posted JournalEntry/JournalLine/Account/AccountType. | `ACCOUNTING_E2E_RECONCILIATION`: exact-SHA posted/reversal/branch/currency/drill-down/export evidence. |
| ACC-050 | HOLD | W2 Query/Export/Print/DrillDown surfaces exist. | `OTS_W1_005_CASH_FLOW_CLASSIFICATION`: candidate/heuristic classification does not authorize implementation-ready closure. |
| ACC-058 | HOLD | Detailed trial balance source-of-truth is posted JournalEntry/JournalLine/Account and W2 Query/DrillDown/Export/Print is exact. | `ACCOUNTING_E2E_RECONCILIATION`: exact-SHA period/currency/reversal/scope/drill-down/export/print evidence. Runtime remains withheld while the gate is open. |

## Runtime containment verified on this branch

1. `MapWave1ScreenCatalog` registers only non-HOLD WAVE-1 runtime surfaces.
2. GEN-003 is withheld inside geography registration.
3. GEN-013 numbering routes are not registered and its DI service registration is removed while the HOLD remains.
4. ACC-036 and WAVE-1 financial report routes are not registered; financial report DI service registration is removed while those HOLDs remain.
5. Migration `20260822172500_Wave1HeldArtifactsCleanup` drops the non-governing `account_classifications` and `accounting_open_items` tables and refuses rollback recreation without new governing authority.
6. Held WAVE-1 report modules `ACC-049`, `ACC-050`, `ACC-074`, `ACC-075`, and `ACC-058` enforce the authoritative server paging cap `PageSize <= 200`; this source correction does not promote any held report to READY.

## Structural corrections already applied

1. WAVE-1 catalog uses the 13 Current Approved IDs, not composite target IDs.
2. Report permission identities are exact: ACC074 / ACC075 / ACC049 / ACC050 / ACC058.
3. UI aliases are not represented as duplicate W2 endpoint bindings.
4. W3 visual definitions are bound to current V1.26/V1.3 and scoped approved decisions; historical/benchmark material is not promoted by name or date.
5. `Wave1ReadinessCatalog` prevents silent READY claims while evidence-backed HOLD gates remain.

## Hard gates

`CI GREEN != SCREEN READY`.

PR #58 must remain **Draft / NOT READY / DO NOT MERGE** while any `Wave1ReadinessCatalog` HOLD remains. READY/Merge requires blockers=0, required CI green on one exact final SHA, all review threads resolved, documentation snapshot updated, and an independent reviewer PASS on that same exact SHA.
