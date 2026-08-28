# ACC-001 — Accounting Posting Authority

Decision date: 2026-08-28
Owner decision: `APPROVED — RESOLVED`
Owner approval record: `EXPLICIT OWNER APPROVAL — 2026-08-28 — "اعتمد"`

## Decision

`ACC-001 = RESOLVED — OPERATIONAL COLLECTION; GOVERNED SETTLEMENT POSTS THE LEDGER`

TransportERP adopts model 3 from the MISSION-03 completion gate:

1. `CollectionTransaction` is an operational, auditable collection record and does not require a pre-posted voucher.
2. Recording an operational collection does not itself post the General Ledger.
3. A later governed `Settlement` is the accounting boundary. Settlement must create/post its voucher and journal atomically and bind the accepted collection rows to the resulting accounting reference.
4. No accepted collection may be silently deleted or rewritten. Corrections use explicit reversal/settlement correction with immutable audit.

## Evidence basis

The accepted P2 collection contract permits `AccountingReferenceId` to be nullable and defines the collection as an immutable accepted transaction linked to P1 Finance. Current persistence also accepts a collection without an accounting reference and only validates/adds the financial link when a reference is supplied. This decision therefore preserves the current operational boundary instead of forcing a new pre-voucher requirement.

## Accounting mappings

Account IDs remain company configuration and must not be hard-coded. The following account roles are authoritative:

- Direct branch cash collection: `Dr Branch Cashbox` / `Cr Waybill-Customer Receivable Subledger`.
- Direct bank collection: `Dr Bank` / `Cr Waybill-Customer Receivable Subledger`.
- Transfer/remittance not yet cleared: `Dr Transfer/Clearing Account` / `Cr Waybill-Customer Receivable Subledger`; clearing to bank/cash is a separate settlement step.
- Driver/agent collection still in custody: `Dr Driver/Agent Custody Receivable` / `Cr Waybill-Customer Receivable Subledger`; remittance later posts `Dr Cash/Bank` / `Cr Driver/Agent Custody Receivable`.
- Reversal never deletes the original posting; it posts the exact inverse or governed correction and retains the original source link.

The Waybill/Customer receivable subledger is the source-level ownership boundary for collected amounts. Revenue recognition, if required by a separate approved business event, remains a distinct accounting event and must not be inferred from collection alone.

## FX and rounding

- Source currency, source amount and collection exchange rate are immutable after acceptance except by explicit reversal/correction.
- Settlement uses the collection-captured rate for the collected principal.
- Realized differences between the recorded local equivalent and bank/clearing realization post to configured FX Gain/Loss accounts.
- Currency minor-unit precision is configuration-driven; arithmetic uses existing high-precision exchange-rate storage and rounding occurs only at the posting boundary.
- Any residual rounding difference posts to a configured Rounding Difference account; it must never be hidden by changing the source collection amount.

## Segregation of duties

`SoD threshold = 0` for settlement posting: every accounting settlement requires maker-checker separation.

- A collector cannot approve/post their own settlement.
- A settlement maker cannot be its final approver/poster.
- Reversal requires a distinct permission and mandatory reason/audit.
- Administrative override requires explicit authority, reason, correlation ID and immutable audit.

## Fiscal periods

- No posting into a hard-closed period.
- Reversal of a transaction from a closed period posts in the first permitted open period while retaining the original transaction/date reference.
- Automatic reopening is prohibited.
- Reopening a closed period requires explicit Finance Administration authority, reason and immutable audit; any owner-reserved hard-close policy remains separately enforceable.

## Implementation boundary

This owner decision resolves the accounting model only. It does not authorize Entity/DbContext/Migration/Schema/Data/Production mutation. W3/DBP-004/005 must implement and verify the Settlement UoW, account-role mappings, reconciliation, reversals, period rules and atomic audit under DB-GOV.
