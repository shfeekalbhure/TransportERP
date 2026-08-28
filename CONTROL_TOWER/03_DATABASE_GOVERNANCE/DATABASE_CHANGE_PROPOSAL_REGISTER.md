# DATABASE CHANGE PROPOSAL REGISTER

`DB-GOV-001` is binding. These rows reconcile the sealed MISSION-02 planning proposals into the central register for MISSION-03 intake. Review here does **not** authorize database/schema/entity/migration execution. Execution remains blocked until each row's evidence, disposable-environment tests, preservation, rollback/recovery and any owner-reserved action are satisfied.

| Proposal ID | Requirement | Proposed Change | Impacted UI/API/Accounting/Sync | Preservation | Migration/Test Plan | Review Status | Execution Status |
|---|---|---|---|---|---|---|---|
| `DBP-001` | `A-ARCH-002 / REM-100` | Separate mapper code correction from read-only affected-row assessment and any later conditional data repair | Waybill API / Shipping allocation / stored items | Preserve Volume meaning, precision and nullability; code change isolated from data repair; row backup before any mutation | create/update/reload/allocate tests; read-only impact query; disposable dry-run before any repair | `REVIEWED — MISSION-03 INTAKE` | `CODE-ONLY FIX MAY PROCEED AFTER W0 EXIT; DB/DATA MUTATION BLOCKED PENDING DB-GOV EVIDENCE/AUTHORITY` |
| `DBP-002` | tenant isolation | Tenant-consistent keys/FKs/indexes/checks and reviewed RLS/equivalent | APIs / workers / reporting / accounting / Sync | Preserve IDs/data/current predicates; additive staged transition | cross-tenant negatives + migration/restore rehearsal | `REVIEWED — DESIGN INTAKE` | `BLOCKED — CARDINALITY/LIVE SCHEMA/ROLES UNKNOWN` |
| `DBP-003` | auth/device/session | Split local-authority persistence into `003A` session/security version, `003B` device registry/assignment and `003C` PoP/nonce/replay | Auth / clients / Offline | Preserve permission codes/users/current singular scope; no raw token/password/private key; staged enrollment | exact lineage + safe-copy restore/reconciliation + PostgreSQL atomic rotation/audit + login/device/replay negatives | `INDEPENDENT DB-GOV REVIEW COMPLETE — SPLIT DECISION RECORDED` | `HOLD AT REHEARSAL ENTRY — 003A REVISE; 003B/C DEFERRED; NO MIGRATION AUTHORITY` |
| `DBP-004` | audit integrity | Hash-version marker, append-only controls, outbox/audit atomicity | Audit / export / compliance / all writes | Preserve old events immutable/verifiable; no rehash | legacy/new chain + raw-SQL denial + failure injection | `REVIEWED — DEPENDENT INTAKE` | `BLOCKED — UOW ADR/LEGACY SAMPLE/LIVE CONTROLS REQUIRED` |
| `DBP-005` | accounting integrity | Durable journal/source links, uniqueness, reversal, period/currency/SoD constraints | Accounting / collection / vouchers / reports | Preserve IDs/precision/history; reconciliation before cutover | balance/duplicate/period/reversal/concurrency tests | `REVIEWED — DEPENDENT INTAKE` | `BLOCKED — ACCOUNTING AUTHORITY/RECONCILIATION REQUIRED` |
| `DBP-006` | offline protocol | Typed queue/inbox/outbox, version/result, owner/device/proof/retention fields/constraints | Sync API / workers / clients / audit | Compatible protocol; quarantine; legacy rows retained | replay/reorder/restart/conflict/revocation/schema/owner negatives | `REVIEWED — CANDIDATE INTAKE` | `BLOCKED — AUTHORITY/PROTOCOL/LIVE BASELINE REQUIRED` |
| `DBP-007` | shipping lifecycle | Later custody/delivery/settlement/return/claim/customs persistence | Shipping / accounting / clients / Offline | Preserve movements/status/idempotency; one increment per migration | state/quantity/custody/accounting acceptance | `REVIEWED — DEFERRED INTAKE` | `BLOCKED — CANONICAL SCOPE REQUIRED` |
| `DBP-008` | ticketing | Separate Ticketing tables/entities/indexes/numbering only after contract authority | Ticketing / accounting / Desktop/Mobile | No inferred schema; preserve approved decision IDs | booking/seat/payment/refund/tenant/concurrency | `REVIEWED — DEFERRED INTAKE` | `BLOCKED — CANONICAL REQUIREMENTS REQUIRED` |
| `DBP-009` | reporting | Read projections/materialized views only if approved | Reporting / privacy / accounting | Source truth unchanged; read-only credentials/redaction | as-of/currency/auth/reconciliation | `REVIEWED — DEFERRED INTAKE` | `BLOCKED — REPORTING REQUIREMENTS REQUIRED` |

## MISSION-03 execution boundary

- Repository-side proposal review is complete for intake.
- No live database/schema/roles/RLS/applied-history truth is inferred from source files.
- `DBP-001` must remain split: the mapper code defect can be corrected as a code-only change after W0 exit; affected-row assessment is read-only; any data repair remains a separate DB-GOV-controlled action.
- All database/data mutations remain prohibited until their exact execution gates are satisfied.

## DBP-003 AUTH-001 preparation note — 2026-08-28

AUTH-001 selected local application authority. MISSION-03 prepared `DBP-003_SESSION_PERSISTENCE_PROPOSAL.md` with the current persistence inventory, proposed additive objects/constraints, tenant/session/device relationships, atomic refresh-reuse semantics, preservation, safe-copy upgrade rehearsal, tests and forward recovery. This advances the proposal to review readiness only. No Entity, DbContext, Migration, Schema, Seed, data, secret or Production change was executed.

## MISSION-03 W2 Control Tower revalidation note — 2026-08-28

Control Tower adopted W2-A1/A2/B1/B2A/C1/F1 only as code-only/test packages at execution SHA `9c5b7a12e59d2c42e682717b8e90c491f8699b96`. The exact W1→W2 diff contains no Entity, DbContext model, Migration, schema, seed, data repair, or Production configuration mutation. This adoption does not activate a database proposal.

- `DBP-002` remains `BLOCKED — DB-GOV ENTRY GATE NOT SATISFIED` for tenant-consistent physical keys/FKs/checks/indexes/RLS-equivalent changes and live data impact.
- `DBP-003` remains `BLOCKED — DB-GOV ENTRY GATE NOT SATISFIED` for membership/session/device/PoP persistence.
- `DBP-006` remains `BLOCKED — DB-GOV ENTRY GATE NOT SATISFIED` for offline nonce/replay/queue persistence.

The code-only controls do not authorize database/schema/persistence/data work and must not be used as evidence that those material gates are satisfied.

## DBP-003 independent DB-GOV decision — 2026-08-28

Control Tower independently inspected exact diff `9c5b7a1...cc67ad2...`, source/model/migration lineage, decoded raw jobs `98917044706`/`98917044568`, run artifacts, PostgreSQL behavior and proposal dependencies. The review is bound in `DBP-003_DB_GOV_REVIEW_DECISION.md`.

- `cc67ad2...` is accepted as a bounded **code-only** MISSION-03 baseline: `NO NEW PERSISTENCE CHANGE`.
- Run `33191269475` applied the existing ten migrations and test mutations only to its disposable PostgreSQL service: `DISPOSABLE TEST DATABASE MUTATION OCCURRED AS PART OF VALIDATION`.
- `DBP-003A — session/security-version persistence`: `REVISE BEFORE REHEARSAL`.
- `DBP-003B — device registry/assignment`: `DEFERRED — DEPENDS ON DBP-002/006`.
- `DBP-003C — PoP/nonce/replay`: `DEFERRED — DEPENDS ON DBP-002/006`.
- `PASSWORD-HASH BASELINE = UNKNOWN — BLOCKS LOGIN PERSISTENCE ACTIVATION`.
- `DBP-003 = HOLD AT REHEARSAL ENTRY`; no Entity, DbContext, Migration, schema, persistent adapter, Production credential or data action is authorized.

كل حذف يبقى `CANDIDATE FOR REMOVAL` حتى يثبت ويعتمد ضمن المسار الحاكم.
