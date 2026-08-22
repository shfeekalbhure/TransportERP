# TransportERP WAVE-1 — 13 Current-Approved Screen Readiness Evidence

**Branch:** `wave1-screen-readiness-20260822`  
**Rule:** No guessing. A screen is not READY merely because code compiles or CI is green. Any unclosed authority, W1/W2/W3, accounting, visual, or exact-SHA evidence gap remains an explicit gate.

## Authority baseline

WAVE-1 is governed by these 13 Current Approved identities:

`GEN-003`, `GEN-004`, `GEN-005`, `GEN-006`, `GEN-007`, `GEN-013`, `GEN-014`, `ACC-036`, `ACC-074`, `ACC-075`, `ACC-049`, `ACC-050`, `ACC-058`.

Legacy targets such as `SET-001`, `SET-002`, `SET-011`, `FIN-028`, and `FIN-055` are crosswalk/navigation aliases only.

## Per-screen evidence state

| Screen | Current state | Proven basis | Remaining gate |
|---|---|---|---|
| GEN-003 | HOLD | SRC-055 closes the full logical/W2/W3 specification and exact actions including Print. | Current W1 physical contract does not yet carry ISO2/ISO3/DialingCode. Do not promote candidate DB columns without controlled W1 physical approval. |
| GEN-004 | IMPLEMENTED / REVIEW REQUIRED | SRC-056 closes Governorate as an independent MasterData/Standard screen; current geography hierarchy is Country→Governorate→Directorate→City→Area. | Exact-SHA runtime + independent review. |
| GEN-005 | IMPLEMENTED / REVIEW REQUIRED | SRC-056 closes Directorate independently. | Exact-SHA runtime + independent review. |
| GEN-006 | IMPLEMENTED / REVIEW REQUIRED | SRC-056 closes City independently. | Exact-SHA runtime + independent review. |
| GEN-007 | IMPLEMENTED / REVIEW REQUIRED | SRC-056 closes Area independently. | Exact-SHA runtime + independent review. |
| GEN-013 | HOLD | Current W2 lifecycle is View/Edit/Reserve/Commit/Cancel/Override. | Current W1 does not persist Code/ArabicName/EnglishName/Notes; Scope needs explicit binding; LastNumber must not be silently equated with NextValue. |
| GEN-014 | HOLD | Current W1 physical fields are Id/Code/CultureCode/Direction/IsActive/Version and current W2 exposes List/Get/Create/Update/Disable only. | Existing branch implementation still uses ArabicName/EnglishName/IsRtl plus translation storage; Contract↔Code↔migration parity must be corrected before READY. |
| ACC-036 | HOLD | Current W1 physical contract defines separate AccountGroup and AccountType entities; W2 exact endpoints are List/Get/Create/Update/Disable. | Existing branch merges both into one AccountClassification entity. Exact DTO field schema / entity discrimination must be proven before safe rewrite. |
| ACC-074 | HOLD | W1 defines Customer/OpenItem/PaymentAllocation source chain and OpenItem physical schema. | Branch OpenItem schema differs materially from W1 physical fields and has no proven posting/allocation producer reconciliation. |
| ACC-075 | HOLD | W1 defines Supplier/OpenItem/PaymentAllocation source chain and OpenItem physical schema. | Same OpenItem physical/source reconciliation gate as ACC-074. |
| ACC-049 | HOLD | W1 source-of-truth is JournalEntry/JournalLine/Account/AccountType and reporting must be posted-only. | Exact-SHA end-to-end accounting reconciliation, period/currency/reversal and drill-down evidence. |
| ACC-050 | BLOCKED | W2 routes/actions exist. | `OTS-W1-005 Cash Flow classification` remains gate-bound before ACC-050. Heuristic source classification cannot earn READY. |
| ACC-058 | HOLD | W1 source-of-truth is JournalEntry/JournalLine/Account and W2 Query/DrillDown/Export/Print is exact. | Exact-SHA posted-only accounting reconciliation and drill-down/export/print evidence. |

## Verified structural corrections already applied on this branch

1. WAVE-1 catalog uses the 13 Current Approved IDs, not ten composite target IDs.
2. Report permission identities are exact: ACC074 / ACC075 / ACC049 / ACC050 / ACC058.
3. UI aliases are no longer represented as duplicate W2 endpoint bindings in `Wave1ScreenCatalog`.
4. W3 visual definitions are bound to current V1.25/V1.3 and scoped owner decisions; GEN-003/013/014 no longer inherit unissued fields from the prior generic shell.
5. `Wave1ReadinessCatalog` prevents silent READY claims while evidence-backed HOLD gates remain.

## Hard rule

`CI GREEN != SCREEN READY`.

Merge is prohibited while `Wave1ReadinessCatalog.HasMergeBlockers == true` or until an independent reviewer verifies every required closure on the same exact head SHA.
