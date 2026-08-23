# TransportERP WAVE-1 — 13 Current-Approved Screen Readiness Evidence

**Branch:** `wave1-screen-readiness-20260822`  
**Rule:** No guessing. `IMPLEMENTED / REVIEW REQUIRED` is not final READY. Final READY requires one exact final SHA with all required CI green, PostgreSQL migration evidence green, and an independent reviewer PASS on that same SHA.

## Authority basis

- Current Approved References V1.26
- Unified Design V1.3
- P0 Authority Decision Record 2026-08-22
- Owner Decisions `OD-W1-01..05` issued 2026-08-23 from explicit owner approval of the remaining-six decision docket.
- Governing identity baseline = 57 Current Approved identities; the 51 master targets remain mapping/recovery aliases.

## Independent review history

TEAM-07 completed the first final independent review on exact SHA:

`1b0efb4ac785be5f90f225a7be62c606df985854`

Verdict:

`INDEPENDENT REVIEW: FAIL ON 1b0efb4ac785be5f90f225a7be62c606df985854`

That SHA is historical and is **not** a delivery candidate anymore. The review produced seven delivery-blocking findings: F-01 through F-07. Issue #61 was closed because that review assignment finished with FAIL, not because delivery passed.

## TEAM-07 remediation state

The implementation remediation was completed and validated before this documentation freeze at:

`9a8dd543cf01553fb3e37e856e46d18c61b53ec9`

Required gates on that implementation SHA:

- P2 C01 W0-3 contract validation — run `32610663251` — **SUCCESS**.
- P2 foundation validation — run `32610663229` — **SUCCESS**.
- WAVE1 PostgreSQL delivery gate — run `32610663248` — **SUCCESS** on PostgreSQL 18.
- Contract validator — **PASS**.
- Runtime/non-PostgreSQL tests — **PASS**.
- Desktop contract surface compilation — **PASS**.
- Governed WAVE-1 migration order execution on PostgreSQL 18 — **PASS**.

Because this evidence file itself is being updated after those runs, a **fresh final CI + PostgreSQL cycle is mandatory on the documentation-freeze head** before assigning the next independent review.

## Finding closure evidence

| Finding | Severity | Remediation | Current disposition |
|---|---|---|---|
| F-01 | HIGH | GEN-013 protected action now binds a governing ApprovalRequest/ApprovalAction, validates target/company/branch/action/target version and writes approval lineage in audit; missing/invalid approval fails with `APPROVAL_STATE_INVALID`. | REMEDIATED — final exact-SHA re-review required |
| F-02 | MEDIUM | Legacy `DocumentType` may seed technical `Code`; historical `ArabicName` remains NULL/unknown until governed reconciliation/touch. No guessed business name. | REMEDIATED — final exact-SHA re-review required |
| F-03 | HIGH | GEN-003 successful Print writes mandatory audit with actor/context/correlation and exact filter payload before returning the print result. | REMEDIATED — final exact-SHA re-review required |
| F-04 | HIGH | ACC-049/050/058/074/075 successful Export routes write mandatory audit with actor/company/branch/correlation/filter payload before returning export output. | REMEDIATED — final exact-SHA re-review required |
| F-05 | HIGH | ACC-074/075 source-document resolution revalidates Company/Branch for ReceiptVoucher, PaymentVoucher, JournalEntry and Waybill; known-type foreign-scope IDs fail closed. | REMEDIATED — final exact-SHA re-review required |
| F-06 | MEDIUM | Delivery traceability `Source Authority` is corrected to V1.26 / Screen-to-Entity V1.2 / Permission V1.2 / API V1.7 / Screen-to-API V1.7; superseded versions are explicitly historical. | REMEDIATED IN DELIVERY WORKBOOK — final package restamp pending final SHA |
| F-07 | MEDIUM | Added PostgreSQL 18 gate executing governed order `Base/P2 → Geo → Reference cleanup → CountryAuthority → NumberingAuthority → AccountingAuthority`; verified legacy unknown values, migration histories, current tables, and absence of rejected legacy tables. | PROVEN on remediation SHA — final exact-SHA rerun required |

## Current WAVE-1 state

All 13 identities remain **IMPLEMENTED / REVIEW REQUIRED**. Authority blockers = **0**. Delivery review blockers are not self-cleared: only the next independent review may grant PASS after final exact-SHA gates are green.

| Screen | Current state | Closure basis | Remaining gate |
|---|---|---|---|
| GEN-003 | IMPLEMENTED / REVIEW REQUIRED | OD-W1-01 + ISO2/ISO3/DialingCode physical promotion + governed Print audit | Final exact-SHA CI/PostgreSQL + independent review |
| GEN-004 | IMPLEMENTED / REVIEW REQUIRED | Current geography implementation | Final exact-SHA CI/PostgreSQL + independent review |
| GEN-005 | IMPLEMENTED / REVIEW REQUIRED | Current geography implementation | Final exact-SHA CI/PostgreSQL + independent review |
| GEN-006 | IMPLEMENTED / REVIEW REQUIRED | Current geography implementation | Final exact-SHA CI/PostgreSQL + independent review |
| GEN-007 | IMPLEMENTED / REVIEW REQUIRED | Current geography implementation | Final exact-SHA CI/PostgreSQL + independent review |
| GEN-013 | IMPLEMENTED / REVIEW REQUIRED | OD-W1-02 + metadata/scope + safe LastNumber + approval-bound protected action | Final exact-SHA CI/PostgreSQL + independent review |
| GEN-014 | IMPLEMENTED / REVIEW REQUIRED | Dedicated Wave1LanguageService and exact W2 behavior | Final exact-SHA CI/PostgreSQL + independent review |
| ACC-036 | IMPLEMENTED / REVIEW REQUIRED | OD-W1-03; separate AccountGroup/AccountType | Final exact-SHA CI/PostgreSQL + independent review |
| ACC-074 | IMPLEMENTED / REVIEW REQUIRED | Normalized aging + scoped source resolver + export audit | Final exact-SHA CI/PostgreSQL + independent review |
| ACC-075 | IMPLEMENTED / REVIEW REQUIRED | Normalized aging + scoped source resolver + export audit | Final exact-SHA CI/PostgreSQL + independent review |
| ACC-049 | IMPLEMENTED / REVIEW REQUIRED | Posted/reversal/branch/currency/drill/export/print + mandatory export audit | Final exact-SHA CI/PostgreSQL + independent review |
| ACC-050 | IMPLEMENTED / REVIEW REQUIRED | Account mapping + controlled override + UNCLASSIFIED + mandatory export audit | Final exact-SHA CI/PostgreSQL + independent review |
| ACC-058 | IMPLEMENTED / REVIEW REQUIRED | Posted/reversal/branch/currency/drill/export/print + mandatory export audit | Final exact-SHA CI/PostgreSQL + independent review |

## Owner-decision implementation invariants retained

### GEN-003
- `ISO2`: uppercase 2 letters, required for new/updated records, unique.
- `ISO3`: optional uppercase 3 letters, unique when present.
- `DialingCode`: optional `+digits`.
- Legacy records are not assigned guessed ISO values.

### GEN-013
- `Code`, `ArabicName`, `EnglishName`, `Notes`, optional FiscalYear metadata are persisted separately from the allocation cursor.
- Unknown legacy ArabicName is preserved as unknown; no DocumentType→ArabicName business-data inference.
- API `Scope` derives from Company/Branch/FiscalYear/DocumentType dimensions.
- `LastNumber = max(NextValue - 1, MAX(NumberReservation.NumberValue), 0)`.
- Protected LastNumber cannot be lowered below allocated history.
- Protected override/reset requires the governed Approval binding, reason, expected target version and full audit lineage.
- Reserve remains atomic/idempotent and cancelled numbers are never reused.

### ACC-036
- No merged `account_classifications` table is current authority.
- `account_groups` and `account_types` are distinct company-scoped entities.

### ACC-074 / ACC-075
- `OpenItem` stores normalized references; it does not persist PartyName/PartyCode/DocumentNo/DocumentDate copies.
- Party display comes from Customer/Supplier masters.
- Source document identity/display is resolved only after Company/Branch scope is revalidated.
- Outstanding = OriginalAmount - applied allocations; reversed allocations do not reduce outstanding.
- Unknown or foreign-scope source references fail closed.

### ACC-050
- Activities are `OPERATING`, `INVESTING`, `FINANCING`, `UNCLASSIFIED`.
- Controlled movement override wins; otherwise linked posted journal-line account mappings determine activity only when exactly one distinct mapping exists.
- No mapping or conflicting mappings => `UNCLASSIFIED`.
- `ReferenceType` keywords are not classification authority.

## Historical containment retained

- `Wave1ReferenceService`, `Wave1FinancialReportService`, and old `Wave1NumberingService` remain unregistered.
- Legacy denormalized `Wave1AccountClassificationEntity` and `Wave1AccountingOpenItemEntity` remain excluded from the active Wave1Reference model.
- Cleanup migration `20260822172500_Wave1HeldArtifactsCleanup` remains lineage evidence and removes rejected legacy tables.

## Release gate

**Authority blockers = 0.**  
**Previous TEAM-07 verdict = FAIL on historical SHA `1b0efb4a...`.**  
**Remediation = implemented; PostgreSQL evidence = proven on `9a8dd543...`.**  
**Final documentation-freeze head still requires fresh CI/PostgreSQL before a new independent review assignment.**  
**PR #58 remains Draft / NOT READY / DO NOT MERGE until a new independent PASS on that same final SHA.**
