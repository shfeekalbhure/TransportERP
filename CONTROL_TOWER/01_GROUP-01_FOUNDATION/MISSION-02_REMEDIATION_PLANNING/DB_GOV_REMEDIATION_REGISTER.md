# DB-GOV Remediation Register

`DB-GOV-001` is binding. These are planning proposals, not entries authorizing
execution. MISSION-03 must copy/reconcile an approved version into the central
Database Governance registers before implementation.

| Proposal | Findings | Proposed DB-related scope | Impact | Preservation / migration path | Validation | Rollback/recovery | Status |
|---|---|---|---|---|---|---|---|
| `DBP-001` | `A-ARCH-002` | mapper correction; read-only affected-row assessment; separate conditional data repair | Waybill API, Shipping allocation, stored items | preserve Volume meaning/precision/nullability; mapper code separate from repair; forward-only repair only if approved | create/update/reload/allocate; safe-copy impact and dry run | code revert; row-level backup/restore; owner authority for data mutation | `REQUIRED — P0` |
| `DBP-002` | `A-DB-003/004`, `TB-F-003/012` | tenant-consistent keys/FKs/indexes/checks and reviewed RLS/equivalent; role separation | all APIs, workers, reporting, accounting, Sync | preserve IDs/data and current predicates; additive staged transition | A↔B company/branch/user/device/API/service/DB/raw-SQL negatives | forward compensating migration or restore; lockout rehearsal | `REQUIRED — DESIGN AFTER CARDINALITY ADR` |
| `DBP-003` | `A-SEC-001/002`, `TB-F-002`, `REM-220` | sessions, membership, permission scope, device registry/assignment/PoP tables if selected from PR69 | auth, clients, Offline | preserve permission codes and existing users; staged enrollment; no Production secret | login/refresh/revoke/cache/device/expiry/replay negatives | disable local mode/feature; restore; forward correction | `CANDIDATE — PR69 SELECTIVE REVIEW` |
| `DBP-004` | `A-AUD-006`, `A-DB-005`, `TB-F-013` | hash-version marker, append-only DB controls, outbox/audit atomicity | audit/export/compliance/all writes | old events immutable/verifiable; no rehash | legacy/new chain, raw SQL mutation denial, failure injection | disable new writer while retaining reader; restore; forward fix | `REQUIRED AFTER UOW ADR` |
| `DBP-005` | `A-ACCDB-007`, `A-BIZ-005`, `TB-F-005` | durable journal/source links, uniqueness, reversal, period/currency/SoD constraints | accounting, collection, vouchers, reports | IDs/precision/history preserved; reconciliation before cutover | balance, duplicate, period close, permission, reversal, concurrency | feature toggle; inverse entries; restore/reconciliation | `REQUIRED AFTER ACCOUNTING AUTHORITY` |
| `DBP-006` | `A-OFF-001/002`, `TB-F-004`, `D-SEC-SYNC-001` | typed queue/inbox/outbox, version/result, owner/device/proof/retention fields and constraints | Sync API/workers/clients/audit | compatible protocol, queue quarantine, legacy rows retained | replay/reorder/restart/conflict/revocation/schema/owner negatives | worker kill switch; compatible reader; restore | `CANDIDATE — AUTHORITY + PR69 REVIEW` |
| `DBP-007` | `A-BIZ-001`, `TB-F-007` | later Shipping custody/delivery/settlement/return/claim/customs persistence | Shipping, accounting, clients, Offline | preserve existing movements/status/idempotency; one increment per migration | state/quantity/custody/accounting acceptance | per-increment feature disable, compensation/restore | `DEFERRED UNTIL CANONICAL SCOPE` |
| `DBP-008` | `A-BIZ-002`, `TB-F-006` | separate Ticketing tables/entities/indexes/numbering only after contract authority | Ticketing, accounting, Desktop/Mobile | no inferred schema; preserve approved decision IDs | booking/seat/payment/refund/tenant/concurrency | module feature off + forward compensation/restore | `BLOCKED — CANONICAL REQUIREMENTS` |
| `DBP-009` | reporting gap | read projections/materialized views only if approved | reporting/privacy/accounting | source truth unchanged; read-only credentials/redaction | as-of/currency/auth/reconciliation | drop only newly created projection after recovery proof | `PLANNED — REQUIREMENTS REQUIRED` |

## Mandatory evidence before any DB execution

Exact authoritative SHA; EF model/snapshot/migration hashes; applied migration history; live schema/drift/roles/RLS inventory; safe row counts/integrity queries; backup/restore proof and RPO/RTO; disposable fresh/upgrade/failure/restore tests; non-secret credential separation; reviewed impact on API, Desktop, Mobile, Offline, Accounting and audit.

Any unavailable item is `ACCESS BLOCKED — UNKNOWN` and blocks its DB execution, not the rest of planning.
