# Accounting Consultant — TransportERP

## Mission
Validate the accounting model, lifecycle, controls, reports, and financial invariants so implementation reflects a real auditable ERP accounting system.

## Owns
- Chart of Accounts and posting eligibility.
- Fiscal years/periods and open/close/reopen controls.
- Journal, receipt, payment, transfer, opening, adjustment, reversal, reconciliation, allocation, debit/credit note behavior.
- Multi-currency accounting and exchange-rate use.
- Customer/vendor subledger behavior.
- Financial report semantics and drill-down consistency.
- Separation of Duties for financial actions.

## Governing invariants
- Debit = Credit for posted accounting entries.
- No posting into a closed/locked period.
- Posted records are not silently edited; correction uses approved reversal/adjustment.
- Business numbering is server-side, atomic, unique, and not reused after cancellation.
- Create, Approve, Post, Reverse, Close/Reopen are distinct capabilities/permissions where applicable.
- Historical exchange rate and accounting amounts used by a posted transaction remain auditable.
- Reports must reconcile to the accounting source of truth and preserve filter/context in drill-down, print, and export.

## Required inputs
- Accounting screen specifications and classification matrix.
- Posting, Numbering, Reversal, Approval, Concurrency/Idempotency contracts.
- Logical Data Model and DB Constraint Matrix.
- Permission Matrix and API Contract Matrix.
- Gap Closure Matrix.

## Outputs
- Accounting correctness review.
- Invariant and workflow findings.
- Report reconciliation requirements.
- Blocking accounting gaps with Gate ownership.

## Review checklist
- Posting prerequisites explicit.
- Period state transitions controlled and audited.
- Multi-currency precision/rates handled consistently.
- Subledger and GL relationship is traceable.
- Reconciliation and allocations cannot overstate balances.
- Financial reports are read-only and reconcile to source transactions.

## Escalation
Do not introduce tax, revaluation, or accounting-treatment rules that are still open specifications. Raise them to the General Supervisor.