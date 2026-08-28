# TEAM-C2 Target Architecture Maps

All diagrams are `PROPOSED — NOT IMPLEMENTED`. They express target responsibilities, not current runtime.

## 1. Context map

```mermaid
flowchart LR
    ORG[Organization & Master Data]
    IAM[Identity & Access]
    ACC[Accounting]
    WB[Waybills]
    SHIP[Shipping]
    TKT[Ticketing]
    SYNC[Offline & Sync]
    AUD[Audit & Compliance]
    REP[Reporting]

    IAM --> ORG
    WB --> ORG
    SHIP --> WB
    SHIP --> ORG
    TKT --> ORG
    WB --> ACC
    SHIP --> ACC
    TKT --> ACC
    SYNC --> IAM
    SYNC --> WB
    SYNC --> SHIP
    SYNC --> TKT
    ORG --> AUD
    IAM --> AUD
    ACC --> AUD
    WB --> AUD
    SHIP --> AUD
    TKT --> AUD
    ORG --> REP
    ACC --> REP
    WB --> REP
    SHIP --> REP
    TKT --> REP
```

Arrows indicate contract/event consumption, not direct entity/table access.

## 2. Runtime/container map

```mermaid
flowchart TB
    D[Desktop Client]
    MA[Mobile Admin]
    MC[Mobile Customer]
    MD[Mobile Driver]
    API[API Host]
    W[Background Worker]
    MOD[Business Modules]
    PG[(PostgreSQL)]
    IDP[External/Approved Identity Authority]
    OBS[Logs/Metrics/Traces]

    D -->|TLS + typed contracts| API
    MA -->|TLS + typed contracts| API
    MC -->|TLS + typed contracts| API
    MD -->|TLS + typed contracts| API
    API -->|validate subject/session/device| IDP
    API --> MOD
    W --> MOD
    MOD --> PG
    API --> OBS
    W --> OBS
```

The IdP/device/Production details remain unknown. The map is conditional on approved evidence and configuration.

## 3. Request security flow

```text
Token/PoP proof
  -> issuer/session validation
  -> server lookup of user + company + branch + device status
  -> TenantContext construction
  -> permission/SoD decision
  -> module command/query
  -> tenant-consistent persistence guard
  -> atomic audit/outbox
  -> redacted response
```

Independent claims are not accepted as a substitute for server-side membership and device validation.

## 4. Accounting posting flow

```text
Approved source document
  -> validate actor/tenant/branch/period/currency/idempotency
  -> resolve governed posting rule
  -> create balanced Journal + Lines
  -> link source document
  -> mark POSTED
  -> append canonical Audit + Outbox
  -> commit one transaction
```

If any step fails, no `POSTED` state may survive. Reversal creates linked inverse evidence; it never rewrites posted history.

## 5. Offline flow

```mermaid
sequenceDiagram
    participant C as Authorized Client
    participant L as Encrypted Local Store
    participant A as API/Inbox
    participant H as Typed Handler
    participant DB as PostgreSQL
    participant P as Pull/Conflict Projection

    C->>L: local transaction + typed outbox
    C->>A: PoP push (tenant/device/op/schema/hash)
    A->>A: validate session/device/permission/idempotency
    A->>H: allowlisted command
    H->>DB: domain change + audit + outbox atomically
    DB-->>A: result version/status
    A-->>C: acknowledged/result/conflict
    C->>P: cursor pull
    P-->>C: ordered changes/conflicts
    C->>L: deterministic apply
```

This flow is disabled for business writes until the governing offline authority permits each operation.

## 6. Data ownership map

| Owner | Writes | May read via | Forbidden shortcut |
|---|---|---|---|
| Organization | companies, branches, master data | owned repository/contracts | other modules mutating master tables |
| IdentityAccess | users, roles, permissions, sessions, devices | authorization service | claim-only tenant/device trust |
| Accounting | vouchers, journals, periods, ledger links | posting/query contracts | operational module writing journal tables |
| Waybills | waybill aggregate/items/parties | owned commands/queries | shipping rewriting item meanings/Volume |
| Shipping | trip/custody/allocation/manifest/movement | Waybill reference/contracts | direct Waybill persistence mutation |
| Ticketing | booking/seat/passenger/trip-ticket state | approved master/accounting contracts | reuse of shipping tables by name similarity |
| OfflineSync | inbox/outbox/sync/conflict/cursors | typed module handlers | generic arbitrary entity/payload execution |
| AuditCompliance | canonical append-only events/policies | controlled query/export | business code mutating historical events |
| Reporting | projections only | events/read contracts | operational writes or bypassed authorization |

## 7. Migration coexistence map

```text
Existing single DbContext + migration lineage
  -> inventory and lock baseline
  -> introduce logical module mappings without table movement
  -> verify fresh/upgrade/drift/restore parity
  -> optionally introduce module DbContexts against unchanged ownership
  -> forward-only schema ownership transitions
  -> retire legacy mapping only after dual-read/parity evidence
```

No step is authorized by this diagram; all database steps are governed by DB-GOV-001.
