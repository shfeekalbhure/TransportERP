# BATCH-16 — Cash & Bank Operations — Design Authority

**Screens:** `ACC-063..ACC-069`  
**Date:** 2026-08-24

## Split disposition
- `ACC-067`, `ACC-068`: ReportInquiry / Statement — ready for full design review.
- `ACC-063`, `ACC-064`, `ACC-065`, `ACC-066`, `ACC-069`: ANALYSIS + LAYOUT PASS; `FIELD_GRID = HOLD_AUTHORITY` because current field/lookup mappings remain explicitly unclosed at implementation layer.

## Governing identities
- ACC-063 Cash Box Transfer = `Transaction / Transfer`, 11 fields, 8 data columns + ordinal, View/Create/Edit/Cancel/Post/Reverse.
- ACC-064 Cash Deposit to Bank = `Transaction / Transfer`, 11 fields, same 8 data columns + ordinal, View/Create/Edit/Cancel/Post/Reverse.
- ACC-065 Bank Withdrawal to Cash = `Transaction / Transfer`, 11 fields, same 8 data columns + ordinal, View/Create/Edit/Cancel/Post/Reverse.
- ACC-066 Bank Reconciliation = `Transaction / Reconciliation`, 7 fields, 9 display columns, View/Create/Edit/Cancel/Match/Finalize/Reopen.
- ACC-067 Cash Box Movement Statement = `ReportInquiry / Statement`, 9 criteria, 10 result columns, View/DrillDown/Export/Print.
- ACC-068 Bank Movement Statement = `ReportInquiry / Statement`, 9 criteria, 10 result columns, View/DrillDown/Export/Print.
- ACC-069 Cashier Shift Closing = `ControlApproval / VarianceControl`, 13 fields, no concrete screen grid, View/Execute/Approve/Reject/Return/Reopen.

## Boundaries
No client financial formulas. Posting, reversal, reconciliation match/finalize/reopen, shift variance/approval, balances and running balances are server/domain authoritative. No unissued attachment/offline/print/mutation actions.

## Authority gate
Current specialist trace still marks transactional lookup/field persistence/provider details as unresolved. No W1/DTO/API/DDL/lookup provider is invented. Those five screens stay `HOLD_AUTHORITY` until an owner design-only decision explicitly permits V1.3 UI inventory to govern presentation metadata while technical mappings remain `TBD-GATED`.
