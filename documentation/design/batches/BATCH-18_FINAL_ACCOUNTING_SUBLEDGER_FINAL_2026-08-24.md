# BATCH-18 — Final Current Accounting Subledger — Final Design Closure

**Screens:** `ACC-075`, `ACC-076`, `ACC-077`, `ACC-078`  
**Date:** 2026-08-24  
**State:** `DESIGN_APPROVED`

## Independent review
- ACC-075 aging design: `TEAM-D06 PASS / 0 open design findings` — `documentation/design/batches/BATCH-18_INDEPENDENT_REVIEW_2026-08-24.md`.
- ACC-076/077/078: `TEAM-D06 PASS / 0 open design findings` — `documentation/design/batches/BATCH-18B_TRANSACTIONS_INDEPENDENT_REVIEW_2026-08-24.md`.

## Final states
- ACC-075 — ReportInquiry / Aging — 9 criteria + 9 columns — View/DrillDown/Export/Print.
- ACC-076 — Transaction / Allocation — 11 fields + 7 columns — View/Allocate/Unallocate only.
- ACC-077 — Transaction / Note — 15 fields + 7-column allocation grid — View/Create/Edit/Cancel/Post/Reverse.
- ACC-078 — Transaction / Note — 15 fields + 7-column allocation grid — View/Create/Edit/Cancel/Post/Reverse.

## Design boundary
Supplier aging, open balances, allocation validity, remaining balances, differences, taxes, note totals, open-item application, posting and reversal are server/domain/read-model authoritative. No client accounting, tax, allocation or aging formula is promoted.

The current ACC-075 `OPEN_ITEM_SOURCE_RECONCILIATION` implementation/runtime HOLD remains open and is not cleared by design approval.

Unissued W1/DTO/property/lookup/provider/attachment/sort bindings remain `TBD-GATED`. No application code, official Kurrasa, W1/DDL, API, DTO, permission or offline-final-write authority was modified.
