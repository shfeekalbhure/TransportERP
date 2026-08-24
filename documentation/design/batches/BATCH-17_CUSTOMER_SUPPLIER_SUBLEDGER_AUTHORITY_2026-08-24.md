# BATCH-17 — Customer & Supplier Subledger — Design Authority

**Screens:** `ACC-070`, `ACC-071`, `ACC-072`, `ACC-073`  
**State:** `INDEPENDENT_REVIEW`

## Governing authority
Current baseline V1.1 + Unified Execution V1.3 + current W2 + CoreUI MasterData/ReportInquiry.

- ACC-070 = `MasterData / Tabbed`, 13 fields, 6 tabs, 7 list columns, actions exactly `View/Create/Edit/Disable`.
- ACC-071 = `MasterData / Tabbed`, 13 fields, 6 tabs, 7 list columns, actions exactly `View/Create/Edit/Disable`.
- ACC-072 = `ReportInquiry / Statement`, 9 criteria, 10 result columns, actions exactly `View/DrillDown/Export/Print`.
- ACC-073 = `ReportInquiry / Statement`, 9 criteria, 10 result columns, actions exactly `View/DrillDown/Export/Print`.

Current V1.3 UI field/tab/grid inventory is design-governing. Unissued physical W1/DTO/provider/sort/attachment mappings remain `TBD-GATED`; no schema/API invention.

`ACC-074` and `ACC-075` are excluded from this batch because the current P0 change log retains them on HOLD.

TEAM-D01..D05 = PASS; next gate TEAM-D06.
