# DOMAIN COVERAGE MATRIX — MISSION-01 CONTROL

| Domain / Area | TEAM-A | TEAM-B | TEAM-C1 | TEAM-D reconciliation | Governing evidence | P0/P1 unresolved | Gate impact |
|---|---|---|---|---|---|---|---|
| Architecture / Solution / Projects | REVIEWED | REVIEWED | REVIEWED | UNKNOWN | Sealed A/B/C1 coverage and evidence registers | YES — conflicting findings require reconciliation | TEAM-D must reconcile |
| Database / EF Core / Migrations | PARTIAL | PARTIAL | REVIEWED | UNKNOWN | Sealed A/B/C1 registers | P1/unknown runtime state | TEAM-D must preserve limitations |
| Security / Authentication / Authorization | PARTIAL | PARTIAL | PARTIAL | UNKNOWN | Sealed A/B/C1 registers | P1/unknown runtime state | TEAM-D must reconcile |
| Multi-Tenant / Company / Branch Isolation | PARTIAL | PARTIAL | PARTIAL | UNKNOWN | Sealed A/B/C1 registers | P1/unknown runtime state | TEAM-D must reconcile |
| Offline / Sync / Device Security | PARTIAL | PARTIAL | REVIEWED | UNKNOWN | Sealed A/B/C1 registers | P1/unknown client runtime | TEAM-D must reconcile |
| Desktop / WinForms / RTL | REVIEWED | REVIEWED | REVIEWED | UNKNOWN | Sealed A/B/C1 registers | P1 runtime gap reported | TEAM-D must reconcile |
| Mobile / MAUI / Android | REVIEWED | REVIEWED | REVIEWED | UNKNOWN | Sealed A/B/C1 registers | P1 runtime gap reported | TEAM-D must reconcile |
| Shipping / Waybills / Warehouse | REVIEWED | PARTIAL | REVIEWED | UNKNOWN | Sealed A/B/C1 registers | P0/P1 conflict and lifecycle gaps | TEAM-D must reconcile |
| Ticketing / Passenger / Trips | REVIEWED | REVIEWED | REVIEWED | UNKNOWN | Sealed A/B/C1 registers | P1 absence/gap reported | TEAM-D must reconcile |
| Accounting / Finance | PARTIAL | PARTIAL | REVIEWED | UNKNOWN | Sealed A/B/C1 registers | P1 invariants/runtime gaps | TEAM-D must reconcile |
| Supply Chain / Dependencies | PARTIAL | PARTIAL | REVIEWED | UNKNOWN | Sealed A/B/C1 registers | P1 assurance gaps | TEAM-D must reconcile |
| CI/CD / Tests | PARTIAL | REVIEWED | REVIEWED | UNKNOWN | Sealed A/B/C1 registers | Exact-SHA/runtime gaps | TEAM-D must reconcile |
| Privacy / Sensitive Data | PARTIAL | PARTIAL | PARTIAL | UNKNOWN | Sealed A/B/C1 registers | P1/access gaps | TEAM-D must reconcile |
| Kurrasa / Governance / Traceability | PARTIAL | REVIEWED | NOT REVIEWED | UNKNOWN | Sealed A/B registers; C1 scope limitation | Authority drift/unknowns | TEAM-D must reconcile |
| Release / Deployment / Recovery | BLOCKED | BLOCKED | PARTIAL | UNKNOWN | Sealed A/B/C1 registers | P1/access gaps | Blocks final readiness, not TEAM-D start |

This matrix is a Control Tower routing summary only. Detailed judgments remain in the immutable sealed team matrices; TEAM-D must independently reverify and replace each `UNKNOWN` reconciliation state with an allowed determination before sealing.
