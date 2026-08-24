# BATCH-17 — Customer / Vendor / Subledger — Final Design Closure

**Screens:** `ACC-070`, `ACC-071`, `ACC-072`, `ACC-073`, `ACC-074`  
**Date:** 2026-08-24  
**State:** `DESIGN_APPROVED`  
**Independent review:** `TEAM-D06 PASS / 0 open design findings`

## Final states
- ACC-070 — MasterData / Tabbed — 13 fields + exact 7-column list — View/Create/Edit/Disable.
- ACC-071 — MasterData / Tabbed — 13 fields + exact 7-column list — View/Create/Edit/Disable.
- ACC-072 — ReportInquiry / Statement — 9 criteria + exact 10 result columns — View/DrillDown/Export/Print.
- ACC-073 — ReportInquiry / Statement — 9 criteria + exact 10 result columns — View/DrillDown/Export/Print.
- ACC-074 — ReportInquiry / Aging — 9 criteria + exact 9 result columns — View/DrillDown/Export/Print.

Independent review evidence: `documentation/design/batches/BATCH-17_INDEPENDENT_REVIEW_2026-08-24.md`.

## Design boundary
Customer/vendor balances, open items, credit exposure/availability, running balances, due classification and aging buckets remain server/read-model authoritative. No local financial or aging formula is promoted.

Unresolved W1/DTO/property/lookup/provider/attachment/sort bindings remain `TBD-GATED` implementation items. No application code, official Kurrasa, W1/DDL, API, DTO, permission or offline-final-write authority was modified.
