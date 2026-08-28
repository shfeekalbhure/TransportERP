# TEAM-C2 Target Architecture Proposal

## 1. Status, authority, and boundary

- Project: `TransportERP`
- Mission: `MISSION-01 — TEAM-C2`
- Version: `v1.0`
- Artifact class: `PROPOSED — NOT IMPLEMENTED`
- Assessed source snapshot: `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`
- Sealed audit anchor: `governance/control-tower-20260828@8a36f88b56a43cd5b47277b645ba2030ed3da4f1`
- TEAM-C2 governance input base: detached worktree parent `432cded27a4bad1f42e7912328a195be297d3678` (`governance: seal team D and start team C2`)
- Governing temporal determination: `AUTHORITATIVE CURRENT LINE: UNKNOWN — REQUIRES OWNER/REPOSITORY VERIFICATION`

TEAM-C2 proposes a target architecture from the sealed TEAM-D reconciliation and the sealed A/B/C1 inputs. It does not promote the assessed snapshot to the authoritative product line, claim that the proposed architecture exists, authorize remediation, or modify Source, Tests, Migrations, Database, or Production.

`DB-GOV-001` is binding. Every database, entity, relationship, DbContext, migration, field, RLS, trigger, or data correction described here is a proposal requiring a separately registered and reviewed change, impact analysis, preservation requirements, disposable test/recovery evidence, and explicit execution authority.

## 2. Evidence-bounded current architecture

The reconciled snapshot contains ten projects in a flat `.slnx`. `TransportERP.Api` is the only source-composed executable. Application depends on Domain and Contracts; Infrastructure depends on Domain, Application, and Contracts; API depends on Application, Contracts, and Infrastructure; Desktop depends only on Contracts; Tests span API and lower layers; the three Mobile projects are isolated and source-empty. No ProjectReference cycle was proven. These are snapshot facts, not an authoritative-current-line judgment (`D-EV-004`, `D-EV-014`, `D-EV-022`, `D-EV-023`).

The following reconciled constraints govern this proposal:

- `A-ARCH-002 = CONFIRMED P0`: the registered Waybill repository deletes/reinserts items while `ToItemEntity` omits `Volume`; runtime reproduction and affected stored rows remain unknown (`D-EV-006`, `D-BLK-009`).
- `A-PRES-001 = CONFIRMED LOCAL-ONLY P0`: local-only heads/objects and dirty evidence must be preserved; their merge merit is unknown (`D-EV-021`, `D-BLK-007`).
- Identity is a claim-driven resource-server foundation; persistent request-time RBAC, session lifecycle, device registry, revocation, and Proof-of-Possession are unproven (`D-EV-007..009`).
- Tenant isolation is manual/path-dependent and is not proven systemically at the database boundary (`D-EV-007`, `D-EV-009`).
- Offline/Sync is server enqueue/state foundation, not an end-to-end offline product (`D-EV-013`).
- Accounting posting, balanced journal creation, atomic audit, and complete database invariants are not implemented or proven (`D-EV-010..012`).
- Desktop is a disconnected WinForms library; Mobile projects are placeholders (`D-EV-014`).
- Shipping stops at partial execution; Ticketing and a reporting subsystem are absent on the assessed snapshot (`D-EV-015`).
- Exact-target runtime, live database, IdP/device, Production, deployment, recovery, latest Kurrasa authority, latest PR #69, and external workspaces remain unknown (`D-BLK-001..010`).
- `BLK-B-001` remains an assurance limitation and must be assessed by TEAM-E; it does not invalidate the source-bound proposal.

## 3. Target architectural style

`PROPOSED`: use a **modular monolith with explicit bounded-module ownership**, one primary API host, a separately hosted background worker when authorized, executable clients, and one PostgreSQL deployment initially. This is preferred over immediate microservices because the snapshot is a partial foundation, cross-module transaction requirements are substantial, deployment/recovery evidence is absent, and a distributed split would add unproven operational risk.

The target separates boundaries before splitting physical deployment:

1. Business modules own use cases, domain rules, contracts, persistence mappings, and tests.
2. Hosts compose modules but do not own business rules or provider-specific persistence.
3. Cross-module interaction uses explicit commands/events/contracts; direct table/entity reach-through is prohibited.
4. Database ownership is logical first. Physical schema/DbContext split occurs only through a forward-only, DB-GOV-001-controlled transition.
5. Existing proven behavior and migration lineage remain protected until parity evidence exists.

The candidate Visual Studio and physical trees are defined in `TEAM-C2_TARGET_SOLUTION_AND_REPOSITORY_TREE.md`. Exact assembly count is an architectural decision to be validated before implementation; logical module boundaries are required even if a first remediation wave keeps some modules in shared assemblies.

## 4. Proposed module boundaries

| Module | Proposed responsibility | Current evidence boundary | Key constraints |
|---|---|---|---|
| Organization & Master Data | companies, branches, geo, parties, settings, numbering | partial entities/contracts; no full runtime | tenant hierarchy/cardinality and Kurrasa authority must be resolved |
| Identity & Access | users, roles, permissions, sessions, devices, authorization decisions | JWT resource-server foundation | bind user/company/branch/device; external IdP guarantees unknown |
| Accounting | COA, periods, vouchers, journals, posting, reversal, subledgers, reconciliation | data/service foundation only | atomic balanced posting; tenant/period/actor/SoD; no POSTED without GL |
| Waybills | draft, items, parties, validation, approval, cancellation, finance intent | partial server path | `Volume` P0; contract compatibility and data meanings preserved |
| Shipping | release, allocation, manifest, trip, custody, arrival, unload, delivery, POD/COD, returns/claims/customs | partial through departure | later lifecycle remains proposed; custody/audit/idempotency invariants required |
| Ticketing | routes, schedules, booking, seats, passenger, payment/refund, boarding, transfer, settlement | absent in snapshot | create only from approved canonical requirements; no inferred schema/API |
| Offline & Sync | protocol, client outbox, dispatcher, device trust, conflict lifecycle | server enqueue/state only | `OFFLINE_WRITE=0 / Can Queue=NO` remains binding until superseded authority |
| Audit & Compliance | canonical events, versioned hash, retention/legal hold/export controls | partial audit and append-only controls | preserve legacy hash verification and append-only lineage |
| Reporting | read models, operational/accounting reports, exports | no subsystem proven | read-only projections; authorization/redaction; source-to-report traceability |
| Platform | clock, IDs, transactions/outbox, crypto adapters, observability, provider wiring | responsibilities scattered | no domain semantics; minimal stable abstractions only |

Feature folders inside each module should use the vertical pattern `Commands`, `Queries`, `Domain`, `Contracts`, `Persistence`, and `Tests` only where those concerns actually exist. Empty ceremonial layers are prohibited.

## 5. Dependency rules

The proposed dependency direction is:

```text
Clients/Hosts -> public Contracts + composition abstractions
Hosts -> module composition only
Modules.Application -> Modules.Domain + module/public Contracts
Modules.Infrastructure -> module application ports + module domain + Platform
Modules.Domain -> SharedKernel only
Platform.Infrastructure -> SharedKernel/platform abstractions
Tests -> the exact layer/host under test
```

Rules:

- Domain code must not reference EF, ASP.NET, WinForms, MAUI, or HTTP types.
- API request/response, scope, and error types belong to API/public contracts, not Persistence.
- Provider ownership (EF/Npgsql) belongs to Infrastructure/module persistence; API should not carry provider packages unless a documented host-only need is proven.
- No module reads another module's EF entities or tables directly. Cross-module data needs use stable IDs/contracts, controlled projections, or domain/integration events.
- A transaction that must remain atomic stays within one module boundary. Cross-module effects use a transactional outbox/inbox and idempotent consumers, except where a reviewed design proves a single database transaction is required.
- Dependency and architecture tests must enforce the acyclic graph and forbidden references.

`TEAM-C2_ARCHITECTURE_MAPS.md` records the proposed context, runtime, and data flows.

## 6. Runtime and host model

### API

`TransportERP.Api` remains the primary HTTP composition host, but business endpoint behavior moves behind module commands/queries. Shared endpoint concerns become tested API filters/middleware: authenticated operation context, tenant binding, permission decision, validation, canonical error mapping, idempotency, correlation, audit, and redaction.

### Background worker

`TransportERP.Worker` is proposed only when durable background execution is authorized. It hosts outbox publication, sync apply/pull processing, notification jobs, and long-running reconciliations. It must share no implicit in-memory state with the API and must use leases/CAS/idempotency and observable retry/dead-letter semantics.

### Desktop

The target Desktop is an executable WinForms host with a composition root, authenticated session, typed API client, navigation shell, permission-aware screen registry, error handling, localization/RTL resources, secure local storage adapter, and explicit online/offline capability policy. Existing forms remain prototype assets until mapped to canonical screen IDs and connected through tested commands/queries.

### Mobile

Admin, Customer, and Driver remain separate target applications only if their approved use cases justify separate trust and deployment profiles. Shared client code is limited to typed contracts, UI-neutral validation, security primitives, and the authorized offline client engine. No mobile project is called implemented until platform scaffolding, secure key storage, signing, API integration, offline policy, and exact-SHA E2E evidence exist.

## 7. Security and tenant architecture

`PROPOSED` controls:

1. One documented identity/session authority: issuer-bound subject, short-lived access token, rotating/revocable session or refresh credential, security/session version, and auditable bootstrap/recovery.
2. Server-derived `TenantContext` that binds authenticated user, company, branch, role/permission set, device, and operation. The API must not trust independent claim values that are not reconciled against authoritative membership.
3. Persistent authorization decision at request time, with cache invalidation/versioning if used. Endpoint permission names are contracts; database tables alone do not grant access.
4. Device registry with per-device public key or platform-backed key, enrollment, rotation, revocation, attestation/PoP policy, and audit. `device_registered=true` claims alone are insufficient.
5. Defense in depth at service and database boundaries: tenant-consistent keys/FKs and an explicitly reviewed RLS or equivalent strategy. Exact choice is an ADR and DB-GOV-001 item because live roles/schema are unknown.
6. Bidirectional negative tests for company A/B and branch A/B across API, service, database, audit, export, reporting, sync, and background worker paths.
7. Sensitive-data classification, minimization, response redaction, export/print permissions, key-managed encryption where required, retention/legal hold rules, and log/telemetry scrubbing.

No external IdP, encryption, retention, Production, or legal control is assumed present.

## 8. Offline and synchronization architecture

The target offline model is **policy-gated and typed**, not a generic JSON executor.

```text
Client command
  -> encrypted local transaction
  -> typed Outbox record (tenant, device, operation ID, schema version, dependency IDs)
  -> authenticated + PoP push
  -> server Inbox/idempotency check
  -> allowlisted typed handler
  -> atomic domain change + audit + server outbox
  -> result version/status
  -> cursor-based pull
  -> deterministic local apply/conflict UI
```

Required properties:

- tenant-aware idempotency key and payload hash;
- schema/version/size/clock validation;
- encrypted local database and key-store-backed keys;
- durable retry, dependency ordering, restart recovery, dead-letter/repair workflow;
- server-assigned versions and result versions;
- allowlisted operation handlers with per-operation authorization;
- conflict strategies declared per aggregate, never global last-write-wins by default;
- atomic business/audit/outbox commit and idempotent consumers;
- network loss, duplicate, reorder, partial failure, revocation, replay, clock skew, and restart tests.

Financial posting, approval, security administration, and any operation marked `Can Queue=NO` remain server-authoritative and non-queueable until a newer governing authority explicitly changes that rule. The latest PR #69 is an uninspected moving candidate and is not adopted by this design.

## 9. Data and database architecture

The target uses a single PostgreSQL deployment initially with explicit module ownership. A future schema or DbContext split is conditional, not assumed.

Proposed controls:

- module-owned mapping configurations and tables/schema inventory;
- tenant-consistent identifiers and relationship constraints;
- explicit transaction boundary per use case;
- outbox/inbox tables for non-atomic cross-module delivery;
- optimistic concurrency/CAS and idempotency retained where present;
- precision, timezone, status, and append-only constraints retained and expanded only through reviewed proposals;
- least-privilege runtime/migration roles and a reviewed RLS/equivalent decision;
- versioned canonical audit hash with backward verification of prior hash lineage;
- classified payload columns, encryption/redaction/retention decisions, and size limits;
- read-only reporting projections that cannot mutate operational aggregates.

The `Volume` field and its semantic meaning are a mandatory data-contract invariant across Domain, API contracts, persistence mappings, migrations, read models, allocation, and tests. Before any implementation, the P0 requires a DB-GOV-001-controlled impact query on a safe copy and a regression contract; this report does not run or prescribe a data mutation.

Migration rules are detailed in `TEAM-C2_MIGRATION_AND_DB_GOVERNANCE_CONSTRAINTS.md`.

## 10. Accounting architecture

The Accounting module owns the ledger and posting authority. A target posting transaction must:

1. validate tenant, branch, fiscal period, actor, permissions/SoD, source-document state, currency/rate, and idempotency;
2. create a balanced journal header and immutable lines where total debit equals total credit under governed precision/rounding;
3. link the voucher/collection/settlement to the journal;
4. transition operational state to `POSTED` only after journal persistence succeeds;
5. append a canonical audit event and outbox record atomically;
6. reverse through a linked reversal, not mutation/deletion;
7. expose reconciliation and period-close evidence.

`POSTED` without a linked balanced journal is prohibited by the target contract. Exact database enforcement, posting rules, account mapping, and cross-module settlement require canonical requirements and registered DB changes; they are not inferred here.

## 11. Business lifecycle architecture

### Waybills and Shipping

Preserve the existing partial Waybill and shipping behaviors as versioned inputs. Separate aggregate/use-case orchestration from EF mapping, audit, and idempotency. Extend the lifecycle only through approved increments: arrival, unload, transit/warehouse custody, transfer, delivery/POD/COD, settlement, returns, claims, customs, and exceptions. Every custody transition requires actor/time/location/source/destination, concurrency, idempotency, authorization, audit, and accounting impact rules.

### Ticketing

Ticketing is a new target module, not a rename of shipping routes. Its proposed boundaries include schedules/trips, capacity/seat inventory, booking, passenger identity, fare/payment/refund, boarding/manifest, transfer/disruption, driver/agent custody, and settlement. Exact entities/endpoints/permissions/screens are gated by canonical decisions and must not be created from folder names or drafts.

### Reporting

Reporting uses explicit read models fed by operational/accounting sources. Reports and exports inherit tenant/branch permissions, PII redaction, retention, currency/rounding, and exact-as-of semantics. Operational status forms do not by themselves constitute a reporting subsystem.

## 12. Screens, shared components, and lookups

The target UI organization is feature-first under each client, with a small shared presentation layer:

- Shell: startup, login/session, navigation, feature flags/capabilities, permission evaluation, error boundary.
- Shared UI: RTL/layout tokens, localization/resources, validation summary, loading/error states, confirmation, attachment viewer, audit display.
- Shared Lookups: account, party/customer/vendor/agent, company/branch, currency, cash/bank, cost center, geo, trip, vehicle, driver, warehouse/location. Each lookup uses a typed query contract and tenant-aware authorization.
- Feature screens: grouped by Setup, Master Data, Accounting, Waybills, Shipping, Ticketing, Operations, and Reporting.

Before wiring or renaming any existing screen, the screen-ID authority conflict must be resolved by a canonical crosswalk. Existing source forms and design artifacts are preserved as versioned inputs; duplicate behavior is consolidated only after parity tests and evidence mapping.

## 13. Tests, CI, supply chain, and release

The target separates test responsibilities:

- Domain/Application unit tests.
- Architecture/dependency tests.
- Contract/schema compatibility tests.
- Infrastructure/PostgreSQL integration and migration tests.
- API authentication/authorization/tenant negative tests.
- Offline protocol/replay/conflict/restart tests.
- Desktop component/smoke/E2E tests on Windows.
- Mobile platform/E2E/security tests.
- End-to-end business/accounting/recovery acceptance tests.

Every governing result is bound to an exact ref/full SHA and environment. Proposed CI stages include SDK/package lock verification, restore/build, static/security checks, migration fresh+upgrade+drift tests, PostgreSQL integration, API/client matrices, coverage/TRX artifacts, SBOM/SCA/license review, signed artifact provenance, install/upgrade/rollback/restore drills, and release approval. Existing SHA-bound tests and positive controls are preserved; no historical PASS transfers to a different SHA.

## 14. Transition strategy

No big-bang rewrite is proposed. Subject to separate authorization, use these gated waves:

1. **Authority and preservation gate:** name authoritative ref/SHA; hash and preserve every local/unmerged asset; resolve screen/Kurrasa authority; create exact-SHA baseline evidence.
2. **P0 safety gate:** register the `Volume` DB/code impact proposal, assess affected data safely, prove regression; prohibit destructive cleanup of local assets.
3. **Boundary-enforcement gate:** add architecture tests and logical module folders/contracts while preserving runtime and migration lineage; no physical DB split.
4. **Security/tenant gate:** implement and negatively test authoritative user/company/branch/device binding and the approved DB defense strategy.
5. **Accounting/audit gate:** establish atomic balanced posting, immutable reversal, canonical audit/outbox, and backward hash verification.
6. **Executable-client gate:** create Desktop composition and authorized Mobile scaffolds; connect only approved screens/use cases.
7. **Offline gate:** only after policy authorization, deliver typed device-bound end-to-end sync and recovery evidence.
8. **Domain increments:** close shipping, then implement approved Ticketing/Reporting increments with accounting and custody traceability.
9. **Release gate:** prove exact-SHA artifact, install/deploy, upgrade/rollback, backup/restore, observability, and operational runbooks.

Each wave has its own plan, evidence, rollback/recovery, and approval. The order does not authorize implementation.

## 15. Change/preservation control

The full matrix is `TEAM-C2_CHANGE_AND_PRESERVATION_CROSSWALK.md`. The non-negotiable preservation envelope includes:

- migration chain, model snapshot, manual hardening, stored IDs, data meanings, and `Volume`;
- Waybill/Shipping partial runtime, API contracts, 23 endpoint behavior, and screen contract assets;
- CAS, idempotency, payload hashes, serializable paths, precision/status constraints, and append-only triggers;
- audit hash lineage and historical verification;
- tenant scope predicates until a stronger control passes parity tests;
- exact-SHA tests/evidence and immutable sealed audit packages;
- every unmerged/local-only asset until semantic disposition and owner authority;
- Kurrasa/version/screen lineage with supersession metadata.

No `Move`, `Merge`, `Split`, `Rename`, `Refactor`, branch cleanup, migration rewrite, data repair, or schema change may proceed without its preservation condition being satisfied and evidenced.

## 16. Open decisions and blockers

The proposal is complete within TEAM-C2's design scope, but it does not resolve evidence outside that scope. `UNKNOWN_AND_BLOCKERS_REGISTER.md` carries the eleven TEAM-D blockers plus target-design decisions. In particular:

- MASTER/GATE cannot assert final current state or readiness without an authoritative ref/full SHA.
- exact-target runtime, live DB, affected `Volume` rows, IdP/device, Production, privacy, deployment, and recovery evidence remain unknown.
- latest Kurrasa/screen authority, latest PR #69, and external workspace inventory remain unresolved.
- TEAM-E must assess all P0/P1, this proposal, and `BLK-B-001`.

## 17. TEAM-C2 determination

`TARGET ARCHITECTURE PROPOSAL: COMPLETE FOR SEALED RECONCILED INPUTS — PROPOSED, NOT IMPLEMENTED`

This proposal provides a bounded target, dependency model, module/data/security/offline/accounting/runtime/test strategy, and preservation-aware transition sequence. It does not issue the MISSION-01 readiness gate, start TEAM-E, resolve line authority, or grant technical execution authority.
