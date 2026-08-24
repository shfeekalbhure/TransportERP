# BATCH-18 — Aging Reports — Design Authority

**Screens:** `ACC-074`, `ACC-075`  
**State:** `INDEPENDENT_REVIEW`

## Boundary
Current baseline V1.1 + Unified Execution V1.3 + current W2 + CoreUI ReportInquiry are sufficient for design. The current P0 change log retains ACC-074/075 on HOLD at implementation/runtime level; this design batch does not clear or reinterpret that technical gate.

- ACC-074 = `ReportInquiry / Aging`; 9 criteria; 9 result columns; actions exactly `View/DrillDown/Export/Print`.
- ACC-075 = `ReportInquiry / Aging`; 9 criteria; 9 result columns; actions exactly `View/DrillDown/Export/Print`.

Aging balances, due/overdue classification and bucket assignment are server/read-model authoritative. No client aging formula or mutation authority.

TEAM-D01..D05 = PASS; next TEAM-D06 independent review.
