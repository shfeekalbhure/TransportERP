# TransportERP — WAVE-1 Owner Decisions OD-W1-01..05

**Issued:** 2026-08-23
**Owner approval source:** explicit owner instruction in the governing completion conversation: `يعتمد — و حدث الملفات — ونفذ`.
**Pre-decision implementation head:** `e67ac02af757566ca7262412b8b129c888165fee`
**Authority class:** CURRENT OWNER DECISION — supersedes the six listed WAVE-1 authority HOLDs only.

## OD-W1-01 — GEN-003 Country physical promotion
- Promote `ISO2`, `ISO3`, `DialingCode` to W1 physical persistence for Country.
- `ISO2`: required for new/updated records, uppercase two-letter code, globally unique.
- `ISO3`: optional uppercase three-letter code, unique when present.
- `DialingCode`: optional `+` followed by digits; not required to be globally unique.
- Existing legacy rows are not assigned guessed ISO values; they may remain null until explicitly reconciled/touched.
- No other GEN-003 field is promoted by this decision.

## OD-W1-02 — GEN-013 field persistence and LastNumber
- Persist NumberSequence metadata: `Code`, `ArabicName`, `EnglishName`, `Notes`.
- `Notes` is persisted business metadata, not presentation-only metadata.
- Scope remains normalized, not duplicated: Company/Branch are read from the existing sequence, optional FiscalYear metadata is persisted, and DocumentType remains on the sequence. API `Scope` is derived from these dimensions.
- `LastNumber` is the authoritative API/business concept and is derived without loss as:
  `max(NextValue - 1, MAX(NumberReservation.NumberValue), 0)`.
- Legacy `NextValue` remains the internal allocation cursor for compatibility; it is not exposed by GEN-013 API.
- Protected LastNumber action writes the equivalent cursor `NextValue = LastNumber + 1` and may never lower below the maximum allocated historical number.
- Reservation remains server-side, atomic, unique, non-reusable after cancel, versioned and audited.

## OD-W1-03 — ACC-036 separate AccountGroup / AccountType contract
- `AccountGroup` and `AccountType` remain separate physical entities; no merged `account_classifications` table is authorized.
- Both are company-scoped accounting masters for the executable ACC-036 screen.
- API uses a discriminated `Kind = GROUP | TYPE` over the existing ACC-036 route family.
- GROUP owns: `Code`, `ArabicName`, `EnglishName`, `AllowsPostingAccounts`, `ShowInFinancialStatements`, `DisplayOrder`, `IsActive`, `Version`.
- TYPE owns: `Code`, `ArabicName`, `EnglishName`, `FinancialClassification`, `NormalBalance`, `IsActive`, `Version`.
- `FinancialClassification ∈ {ASSET, LIABILITY, EQUITY, REVENUE, EXPENSE}`.
- `NormalBalance ∈ {DEBIT, CREDIT}`.
- Mutations are company-scoped, concurrency-protected and audited.

## OD-W1-04 — ACC-074 / ACC-075 authoritative aging source chain
- Customer/Supplier master is the authoritative party display source. Persist `Code`, `ArabicName`, optional `EnglishName` plus existing financial master references.
- `OpenItem` stores normalized references only: Company/Branch, PartyType + CustomerId/SupplierId, SourceDocumentType/Id, producing JournalEntryId/JournalLineNo, Currency, OriginalAmount, DueDate, Status, Version.
- Do not copy PartyName/PartyCode/DocumentNo/DocumentDate into OpenItem.
- Outstanding balance is `OriginalAmount - SUM(APPLIED PaymentAllocation.Amount)`; reversed allocations are excluded.
- Party name/code are joined from Customer/Supplier.
- Document number/date are resolved from the source document identified by `SourceDocumentType/Id`. Initial executable resolvers are the source aggregates currently present in the repository: ReceiptVoucher, PaymentVoucher, JournalEntry and Waybill. Unknown source types fail closed; they are not guessed.

## OD-W1-05 — ACC-050 Cash Flow classification
- Promote the AP-A4-002 model to CURRENT implementation authority.
- Activities are exactly `OPERATING`, `INVESTING`, `FINANCING`, `UNCLASSIFIED`.
- Default classification source is `CashFlowAccountMapping` keyed by accounting AccountId.
- A controlled movement override may replace the account default for one movement; it requires explicit activity, reason, approver identity, version and audit lineage.
- For a posted receipt/payment movement, override wins. Without override, the linked posted journal lines are inspected; exactly one distinct mapped activity wins. No mapping or conflicting mapped activities yields explicit `UNCLASSIFIED`.
- `ReferenceType` substring/keyword classification is prohibited.
- Cash-flow report remains server-calculated from posted movements only, with branch/currency scope enforcement.

## Release effect
These decisions close the **authority** portion of the six prior HOLDs. Each identity may move only to `IMPLEMENTED / REVIEW REQUIRED` after its implementation and tests pass. Final READY still requires exact-final-SHA green CI and independent review PASS on that same SHA. Merge remains prohibited until those release gates pass.