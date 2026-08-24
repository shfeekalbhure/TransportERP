# BATCH-19 — Payment Allocation & Debit/Credit Notes — Design Authority

**Screens:** `ACC-076`, `ACC-077`, `ACC-078`  
**State:** `INDEPENDENT_REVIEW`

## Current authority
- ACC-076 = `Transaction / Allocation`; 11 governing fields; 7-column allocation grid. Current P0 `ODR-ACC076-001` supersedes old draft lifecycle. Executable surface exactly `View | Allocate | Unallocate`; endpoints exactly List/Get/Atomic Allocate/Reverse Allocation.
- ACC-077 = `Transaction / Note`; 15 fields; 6-column linked-document/allocation grid; actions exactly `View/Create/Edit/Cancel/Post/Reverse`.
- ACC-078 = `Transaction / Note`; 15 fields; 6-column linked-document/allocation grid; actions exactly `View/Create/Edit/Cancel/Post/Reverse`.

Shared Transaction/CoreUI only. Allocation/posting/reversal/open-item/tax/balance values remain server/domain-authoritative. No Print/Export/direct approval/attachment mutation/offline final write unless separately issued.

Unissued W1/DTO/provider/sort/attachment mappings remain `TBD-GATED` implementation items.

TEAM-D01..D05 = PASS; next TEAM-D06.
