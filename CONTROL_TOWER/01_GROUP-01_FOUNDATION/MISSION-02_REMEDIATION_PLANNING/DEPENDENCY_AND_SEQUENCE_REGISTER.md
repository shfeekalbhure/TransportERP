# Dependency and Sequence Register

| ID | Work item | Depends on | Blocks | Parallel preparation allowed | Gate result |
|---|---|---|---|---|---|
| `DEP-001` | Immutable baseline/preservation | none | every destructive or implementation action | yes, inventory only | mandatory first |
| `DEP-002` | Exact-SHA quality baseline | DEP-001 | code/DB acceptance claims | no product change until measured | mandatory first |
| `DEP-003` | Volume mapper correction | DEP-002, DBP-001 | data integrity and Shipping confidence | code/test design yes | W1 |
| `DEP-004` | Volume data impact/repair | DEP-001, DBP-001, safe copy, owner if repair | release/data closure | query design yes | may remain separate from code fix |
| `DEP-005` | Tenant hierarchy/cardinality ADR | canonical authority, live-role evidence | DB keys/RLS/device/accounting scope | analysis yes | W2 entry |
| `DEP-006` | Identity/RBAC/session design | IdP mode/config and DEP-005 | client/offline exposure | PR69 review yes | W2 |
| `DEP-007` | Device registry/PoP and lifecycle owner policy | DEP-005/006 | Offline and mobile activation | negative-test design yes | W2 |
| `DEP-008` | Cross-module UoW ADR | current DbContext and module ownership map | Accounting/Audit/Outbox implementation | ADR drafting yes | W3 entry |
| `DEP-009` | Canonical accounting decisions | periods, SoD, mappings, reversal/subledger authority | posting/collection bridge | alternatives only | W3 entry |
| `DEP-010` | Audit version/append-only design | DEP-008/009, legacy chain sample | Accounting and Offline atomicity | verifier design yes | W3 |
| `DEP-011` | Per-operation offline authority | canonical Kurrasa/owner decision | any business offline write | candidate comparison yes | W4 entry |
| `DEP-012` | Typed Sync adoption | DEP-006/007/008/011 | Desktop/Mobile offline | test harness prep yes | W4 |
| `DEP-013` | Canonical screen/route registry | Kurrasa version, screen supersession | client composition and business scope | inventory yes | W5/W6 entry |
| `DEP-014` | Executable Desktop/Mobile scope | DEP-006/007/012/013 | client acceptance/release | packaging design yes | W5 |
| `DEP-015` | Shipping/Ticketing increments | DEP-005/008/009/013; DEP-012 if offline | release and reporting | requirements analysis yes | W6 |
| `DEP-016` | Supply/release/recovery | stable candidate from prior wave | Production/release | tooling design yes | W7 |
| `DEP-017` | Structural cleanup | all behavior/data/release gates | final maintainability closure | no destructive preparation | W8 last |

No circular dependency is accepted. If module ownership and atomicity conflict, `DEP-008` decides the boundary before code exists.
