# BATCH-17 — Customer / Vendor / Subledger — Design Authority

**Screens:** `ACC-070`, `ACC-071`, `ACC-072`, `ACC-073`, `ACC-074`  
**Date:** 2026-08-24  
**State:** `INDEPENDENT_REVIEW`

## Governing sources
- Current TransportERP screen baseline V1.1 / current approved V1.25+ content.
- Current W2 screen→API→permission traceability.
- CoreUI MasterData and ReportInquiry foundations.
- Specialist field/lookup review is consumed as implementation-gap evidence only; it does not override the current screen inventory.

## Design-only boundary
Current screen fields/tabs/list/report columns are governing UI semantics. Any unresolved physical W1 column, DTO property, lookup provider/search/scope/revalidation binding, attachment API or exact sort key remains `TBD-GATED` for implementation.

No database/API/DTO/permission/offline authority is created by this design package.

## Canonical screens
- `ACC-070 — العملاء` = `MasterData / Tabbed`; 13 fields; 7-column list; View/Create/Edit/Disable only.
- `ACC-071 — الموردون` = `MasterData / Tabbed`; 13 fields; 7-column list; View/Create/Edit/Disable only.
- `ACC-072 — كشف حساب العميل` = `ReportInquiry / Statement`; 9 criteria; 10 result columns; View/DrillDown/Export/Print only.
- `ACC-073 — كشف حساب المورد` = `ReportInquiry / Statement`; 9 criteria; 10 result columns; View/DrillDown/Export/Print only.
- `ACC-074 — أعمار الديون للعملاء` = `ReportInquiry / Aging`; 9 criteria; 9 result columns; View/DrillDown/Export/Print only.

## Shared boundaries
- Customer/vendor financial balances, open items, credit use, statements and aging are server/read-model authoritative.
- No client aging-bucket, running-balance, credit-availability or accounting formula is authoritative.
- Attachment tabs on master screens create no mutation commands without W2 binding.
- Disable is the only issued lifecycle state mutation for ACC-070/071; no Delete/Enable is invented.
- No offline final write is authorized.

`TEAM-D01..D05 = PASS`; next gate: `TEAM-D06 INDEPENDENT_REVIEW`.
