# BATCH-16 — Accounting Cash & Bank — Final Design Closure

**Screens:** `ACC-063`, `ACC-064`, `ACC-065`, `ACC-066`, `ACC-067`, `ACC-068`, `ACC-069`  
**Date:** 2026-08-24  
**State:** `DESIGN_APPROVED`

## Independent review
- `ACC-067`, `ACC-068`: `TEAM-D06 PASS / 0 open design findings` — evidence `documentation/design/batches/BATCH-16A_REPORTS_INDEPENDENT_REVIEW_2026-08-24.md`.
- `ACC-063`, `ACC-064`, `ACC-065`, `ACC-066`, `ACC-069`: `TEAM-D06 PASS / 0 open design findings` — evidence `documentation/design/batches/BATCH-16B_TRANSACTIONS_CONTROLS_INDEPENDENT_REVIEW_2026-08-24.md`.

## Final screen states
- ACC-063 — Transaction / Transfer — 11 fields + 8-column grid — View/Create/Edit/Cancel/Post/Reverse.
- ACC-064 — Transaction / Transfer — 11 fields + 8-column grid — View/Create/Edit/Cancel/Post/Reverse.
- ACC-065 — Transaction / Transfer — 11 fields + 8-column grid — View/Create/Edit/Cancel/Post/Reverse.
- ACC-066 — Transaction / Reconciliation — 7 fields + 9-column governed detail grid — View/Create/Edit/Cancel/Match/Finalize/Reopen.
- ACC-067 — ReportInquiry / Statement — 9 criteria + 10 columns — View/DrillDown/Export/Print.
- ACC-068 — ReportInquiry / Statement — 9 criteria + 10 columns — View/DrillDown/Export/Print.
- ACC-069 — ControlApproval / VarianceControl — 13 fields, no concrete local grid — View/Execute/Approve/Reject/Return/Reopen.

## Design boundary
All balances, accounting amounts, exchange-rate validity, posting/reversal, reconciliation matching/finalization/reopen, movement balances, shift book/count/variance and approval/SoD decisions remain server/domain/read-model authoritative. No local accounting/reconciliation formula is promoted.

No application code, official Kurrasa, W1/DDL, API, DTO, permission or offline-final-write authority was modified by this closure. Unissued physical/provider/property/sort/action-code bindings remain `TBD-GATED` implementation items. Runtime/acceptance/release evidence remains separate.
