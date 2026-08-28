# TEAM-C2 Domain and Architecture Coverage Matrix

- Version: `v1.0`
- Status values describe proposal coverage, not implementation completion.

| Domain / area | Reconciled input status | TEAM-C2 target coverage | Governing evidence | Key target output / remaining gap |
|---|---|---|---|---|
| Architecture / Solution | RECONCILED | COVERED — PROPOSED | C2-EV-003..011 | modular monolith/tree/dependency rules; assembly split ADR remains |
| Projects / Modules / Feature folders | RECONCILED STRUCTURAL | COVERED — PROPOSED | C2-EV-003..011 | target tree/template/crosswalk |
| Database / EF / Migrations | PARTIAL / live blocked | COVERED WITH CONSTRAINTS | C2-EV-004/012/015/017/018/023 | DB-GOV constraints; live state and exact transition unknown |
| Security / Authentication | PARTIAL | COVERED WITH CONSTRAINTS | C2-EV-014/015 | identity/session/device/authorization target; IdP unknown |
| Multi-Tenant / RBAC | STATIC RECONCILED / RUNTIME PARTIAL | COVERED WITH CONSTRAINTS | C2-EV-014/015 | TenantContext + DB defense + negative matrix; cardinality/RLS unknown |
| Offline / Sync | STATIC RECONCILED | COVERED — POLICY GATED | C2-EV-016 | typed outbox/inbox/PoP/pull/conflict; write authority blocks implementation |
| Desktop | RECONCILED PROTOTYPE | COVERED — PROPOSED | C2-EV-007/025 | executable shell/client/shared UI; screen crosswalk unknown |
| Mobile | RECONCILED ABSENT | COVERED — PROPOSED | C2-EV-007/026 | three scoped clients; code/platform scope unknown |
| API / Runtime hosts | RECONCILED PARTIAL | COVERED — PROPOSED | C2-EV-003/008/014 | API composition + optional Worker; runtime not proven |
| Shipping / Waybill | RECONCILED PARTIAL | COVERED — PROPOSED | C2-EV-012/019 | modules/lifecycle/custody; later requirements incomplete |
| Ticketing / Passenger | RECONCILED ABSENT | COVERED — PROPOSED BOUNDARY | C2-EV-019/021 | bounded module only; exact contracts require authority |
| Accounting / Finance | RECONCILED FOUNDATION | COVERED WITH INVARIANTS | C2-EV-018 | atomic balanced posting/reversal; mappings/SoD unknown |
| Reporting | RECONCILED ABSENT | COVERED — PROPOSED | C2-EV-019 | read-model subsystem; report requirements unknown |
| Screens / UX / RTL / Lookups | RECONCILED STATIC | COVERED — PROPOSED | C2-EV-007/011/025 | shell/shared UI/lookups/features; identity conflict remains |
| Audit / Compliance / Privacy | PARTIAL | COVERED WITH CONSTRAINTS | C2-EV-017/024 | versioned hash, atomic outbox, classification/retention; environment unknown |
| Tests / QA / Acceptance | PARTIAL | COVERED — PROPOSED | C2-EV-009/020 | split test topology/exact-SHA gates; outcomes not run |
| CI/CD / Supply Chain | PARTIAL | COVERED — PROPOSED | C2-EV-010/020 | locks/SBOM/SCA/artifacts/provenance; graph/advisories unknown |
| Release / Deployment / Recovery | ACCESS BLOCKED | COVERED AS REQUIRED GATES | C2-EV-020 | artifact/install/upgrade/restore target; external state unknown |
| Kurrasa / Governance | PARTIAL VERSION-BOUND | COVERED AS AUTHORITY GATE | C2-EV-021/025 | crosswalk/supersession required; latest authority unknown |
| Git / PR / Workspace Preservation | RECONCILED FOR LISTED ASSETS | COVERED — NON-DESTRUCTIVE | C2-EV-013/026 | preserve/hash/semantic disposition; external inventory unknown |

All master-command areas have an explicit target treatment. `COVERED` never means implemented, tested, released, or ready.
