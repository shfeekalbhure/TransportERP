# TransportERP WAVE-1 — 13 Current-Approved Screen Readiness Evidence

**Branch:** `wave1-screen-readiness-20260822`  
**Rule:** No guessing. `IMPLEMENTED / REVIEW REQUIRED` is not final READY. Final READY requires one exact final SHA with green required CI and an independent reviewer PASS on that same SHA.

## Authority basis

- Current Approved References V1.26
- Unified Design V1.3
- P0 Authority Decision Record 2026-08-22
- Owner Decisions `OD-W1-01..05` issued 2026-08-23 from explicit owner approval of the remaining-six decision docket.
- Governing identity baseline = 57 Current Approved identities; the 51 master targets remain mapping/recovery aliases.

## Current WAVE-1 state

All 13 identities are now **IMPLEMENTED / REVIEW REQUIRED**. Authority blockers = **0**.

| Screen | Current state | Closure basis | Remaining gate |
|---|---|---|---|
| GEN-003 | IMPLEMENTED / REVIEW REQUIRED | OD-W1-01 + Country ISO2/ISO3/DialingCode physical promotion + exact W2 routes/print | Exact-final-SHA CI + independent review |
| GEN-004 | IMPLEMENTED / REVIEW REQUIRED | Current geography implementation | Independent review |
| GEN-005 | IMPLEMENTED / REVIEW REQUIRED | Current geography implementation | Independent review |
| GEN-006 | IMPLEMENTED / REVIEW REQUIRED | Current geography implementation | Independent review |
| GEN-007 | IMPLEMENTED / REVIEW REQUIRED | Current geography implementation | Independent review |
| GEN-013 | IMPLEMENTED / REVIEW REQUIRED | OD-W1-02 + persisted metadata + normalized scope + lossless LastNumber derivation over allocation cursor/history | Exact-final-SHA CI + independent review |
| GEN-014 | IMPLEMENTED / REVIEW REQUIRED | Dedicated Wave1LanguageService and exact W2 behavior | Independent review |
| ACC-036 | IMPLEMENTED / REVIEW REQUIRED | OD-W1-03; separate AccountGroup/AccountType + Kind-discriminated exact route family | Exact-final-SHA CI + independent review |
| ACC-074 | IMPLEMENTED / REVIEW REQUIRED | OD-W1-04 normalized Customer/OpenItem/PaymentAllocation/source-document joins | Exact-final-SHA CI + independent review |
| ACC-075 | IMPLEMENTED / REVIEW REQUIRED | OD-W1-04 normalized Supplier/OpenItem/PaymentAllocation/source-document joins | Exact-final-SHA CI + independent review |
| ACC-049 | IMPLEMENTED / REVIEW REQUIRED | Posted/reversal/branch/currency/drill-down/export/print/cap E2E | Independent review |
| ACC-050 | IMPLEMENTED / REVIEW REQUIRED | OD-W1-05 account mapping + controlled movement override + explicit UNCLASSIFIED; no ReferenceType heuristic | Exact-final-SHA CI + independent review |
| ACC-058 | IMPLEMENTED / REVIEW REQUIRED | Posted/reversal/branch/currency/drill-down/export/print/cap E2E | Independent review |

## Owner-decision implementation details

### GEN-003
- `ISO2`: uppercase 2 letters, required for new/updated records, unique.
- `ISO3`: optional uppercase 3 letters, unique when present.
- `DialingCode`: optional `+digits`.
- Legacy records are not assigned guessed ISO values.

### GEN-013
- `Code`, `ArabicName`, `EnglishName`, `Notes`, optional FiscalYear metadata are persisted separately from the existing allocation cursor.
- API `Scope` is derived from configured Company/Branch/FiscalYear/DocumentType dimensions.
- API/business `LastNumber = max(NextValue - 1, MAX(NumberReservation.NumberValue), 0)`.
- Protected LastNumber cannot be lowered below allocated history.
- Reserve remains atomic/idempotent and cancelled numbers are not reused.

### ACC-036
- No merged `account_classifications` table is reintroduced.
- `account_groups` and `account_types` are distinct company-scoped entities.
- API discriminator `Kind=GROUP|TYPE` preserves the exact ACC-036 route family.

### ACC-074 / ACC-075
- `OpenItem` stores normalized references; it does not persist PartyName/PartyCode/DocumentNo/DocumentDate copies.
- Party display comes from Customer/Supplier masters.
- Document identity/display comes from `SourceDocumentType/Id` resolver.
- Outstanding = OriginalAmount - applied allocations; reversed allocations do not reduce outstanding.
- Unknown source types fail closed.

### ACC-050
- Activities are `OPERATING`, `INVESTING`, `FINANCING`, `UNCLASSIFIED`.
- Controlled movement override wins; otherwise linked posted journal-line account mappings determine the activity only if exactly one distinct mapping exists.
- No mapping or conflicting mappings => explicit `UNCLASSIFIED`.
- `ReferenceType` keywords are not classification authority.

## Historical containment retained

- `Wave1ReferenceService`, `Wave1FinancialReportService`, and old `Wave1NumberingService` remain unregistered.
- Legacy denormalized `Wave1AccountClassificationEntity` and `Wave1AccountingOpenItemEntity` remain excluded from the active Wave1Reference EF model.
- Cleanup migration `20260822172500_Wave1HeldArtifactsCleanup` remains lineage evidence and prevents silent resurrection of the rejected old tables.

## Release gate

**Authority blockers = 0.**  
**Independent Review = pending until the new exact implementation SHA has green required CI.**  
**PR #58 remains Draft / NOT READY / DO NOT MERGE until independent PASS on the same exact final SHA.**
