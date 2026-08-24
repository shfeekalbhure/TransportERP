# BATCH-18 — Final Current Accounting Subledger — Design Authority

**Screens:** `ACC-075`, `ACC-076`, `ACC-077`, `ACC-078`  
**Date:** 2026-08-24  
**State:** `INDEPENDENT_REVIEW`

## Canonical identities
- ACC-075 — أعمار الالتزامات للموردين — `ReportInquiry / Aging` — 9 criteria + 9 result columns.
- ACC-076 — تخصيص الدفعات وتسوية الأرصدة — `Transaction / Allocation` — 11 fields + 7-column open-item grid.
- ACC-077 — إشعار مدين — `Transaction / Note` — 15 fields + 7-column allocation grid.
- ACC-078 — إشعار دائن — `Transaction / Note` — 15 fields + 7-column allocation grid.

## Exact W2 action surface
- ACC-075: `View | DrillDown | Export | Print`.
- ACC-076: `View | Allocate | Unallocate` only. Current approved ODR-ACC076-001 reduces the surface to these exact permissions/endpoints.
- ACC-077: `View | Create | Edit | Cancel | Post | Reverse`.
- ACC-078: `View | Create | Edit | Cancel | Post | Reverse`.

## Design boundaries
- Supplier aging totals/buckets are server/read-model authoritative. Runtime OPEN_ITEM_SOURCE_RECONCILIATION hold remains separate and does not become a design claim.
- Allocation is atomic/server-authoritative. Open balances, allocated amount validity, remaining balance, settlement difference and reverse/unallocate behavior are not client-authoritative formulas.
- Debit/Credit Note tax/base/total, balance-before/after, open-item allocation, posting and reversal remain server/domain authoritative.
- No screen-specific Print/Export is added to ACC-076/077/078.
- Attachment/context tabs create no mutation commands without W2 authority.
- No offline final write is authorized.
- Any unissued W1/DTO/property/lookup/provider/sort binding remains `TBD-GATED` implementation authority.

`TEAM-D01..D05 = PASS`; next gate: `TEAM-D06 INDEPENDENT_REVIEW`.
