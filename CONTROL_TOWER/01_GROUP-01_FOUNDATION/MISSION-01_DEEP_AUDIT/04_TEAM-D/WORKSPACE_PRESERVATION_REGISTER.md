# TEAM-D Workspace Preservation Register

| Preservation ID | Asset / ref | Classification | Value / linked evidence | Required preservation state | Risk / authority |
|---|---|---|---|---|---|
| D-PRES-001 | sealed TEAM-A package | SEALED AUDIT ARTIFACT | D-EV-001; 29 findings | PRESERVE IMMUTABLY | later edit requires REOPEN/new seal |
| D-PRES-002 | sealed TEAM-B package | SEALED AUDIT ARTIFACT | D-EV-002; 21 findings | PRESERVE IMMUTABLY | retain BLK-B-001 provenance |
| D-PRES-003 | sealed TEAM-C1 package | SEALED AUDIT ARTIFACT | D-EV-003; structural baseline | PRESERVE IMMUTABLY | later edit requires REOPEN/new seal |
| D-PRES-004 | `master@2ec6cccf...` product snapshot | CURRENT CANDIDATE | assessed source tree | KEEP AS SHA-BOUND BASELINE | not automatically authoritative |
| D-PRES-005 | PR69 historical snapshot `939f49fa...` | UNMERGED/HISTORICAL SNAPSHOT | predecessor reports and CI evidence | KEEP UNTIL RECONCILED | newer remote head exists; no evidence transfer |
| D-PRES-006 | PR69 remote head `9c9cfdb7...` | UNMERGED | fresh direct remote observation | KEEP UNTIL RECONCILED | contents/CI unknown |
| D-PRES-007 | WAVE-1 `e3a2fe2e...` | UNMERGED | separate open work line | KEEP UNTIL RECONCILED | no blind merge/delete |
| D-PRES-008 | W0 `31ed28b2...` | UNMERGED | separate work line | KEEP UNTIL RECONCILED | no blind merge/delete |
| D-PRES-009 | P2-D `05ea90b6...` | UNMERGED | later shipping candidate | KEEP UNTIL RECONCILED | no blind merge/delete |
| D-PRES-010 | local head `3bc7f431...` | LOCAL-ONLY | patch-unique work per sealed A evidence | PRESERVE | P0 loss risk; semantic merit unknown |
| D-PRES-011 | local object/head `7df4743e...` | LOCAL-ONLY | patch-unique work per sealed A evidence | PRESERVE | P0 loss risk; owner disposition required before deletion |
| D-PRES-012 | dirty-worktree evidence at `06146e0f...` and tracked PNG hash | LOCAL-ONLY / DIRTY ARTIFACT | sealed A preservation appendix | PRESERVE/HASH | P0 loss risk; current worktree ownership must be verified |
| D-PRES-013 | migration lineage and model snapshot | SNAPSHOT-PRESENT | D-EV-009/011/025 | PRESERVE | DB-GOV-001; no rewrite/reorder without analysis |
| D-PRES-014 | CAS/idempotency/serializable/constraint and audit/shipping trigger behavior | SNAPSHOT-PRESENT | D-EV-025 | PRESERVE WITH REGRESSION EVIDENCE | remediation may accidentally weaken controls |
| D-PRES-015 | audit hash lineage | SNAPSHOT-PRESENT | D-EV-010 | PRESERVE/VERSION | hash expansion requires versioned compatibility |

This register is not a merge recommendation. It forbids destructive cleanup until each asset is reconciled and the required authority is recorded.
