# W3 — Unit of Work, Accounting and Audit Preparation

- Baseline: `cc67ad2bd491ed3ab23c3144f11dff955353c3a4`
- Product modification: `NONE`
- State: `DEP-008/010 DESIGN PREPARED; DEP-009 OWNER/ACCOUNTING DECISION REQUIRED — BOUNDED`

## Revalidated defects

- Voucher posting currently changes `APPROVED` to `POSTED` and saves without a
  journal, period/SoD/permission check, audit or outbox.
- Current journal persistence does not enforce debit equals credit, source
  uniqueness, reversal uniqueness or posted-history immutability at the DB.
- Waybill collection can reference an approved voucher but does not create or
  prove a posted ledger effect.
- Audit V1 hash omits persisted `EntityType`, `DeviceId`, `BeforeJson`,
  `AfterJson` and `Ip`, and has no persisted hash-version marker.
- Several Sync lifecycle writes commit business state before a separately
  committed audit append.
- PR #69 leaves status-only voucher posting and the incomplete audit canonical
  hash unchanged; it is not a W3 solution.

## DEP-008 proposed decision

`DEP-008 = READY FOR CONTROL TOWER ADR APPROVAL`

Use a modular monolith with the existing single PostgreSQL deployment and one
`TransportErpDbContext`. One application orchestration boundary owns the
infrastructure transaction and calls Accounting, source module, Audit and
Outbox through ports. Journal, source state/link, audit and outbox are committed
atomically. Retry encloses the entire orchestration and uses a stable
idempotency key. Nested autonomous audit transactions and a physical DbContext
split are prohibited in W3.

## DEP-009 invariant boundary

The sealed evidence proves these mandatory invariants:

- no `POSTED` state without a linked balanced immutable journal;
- the accounting period is open;
- permission and separation-of-duties are evaluated server-side;
- journal, source and tenant scope are company/branch consistent;
- currency/rate/precision inputs are retained as governed snapshots;
- one source produces at most one posting for an idempotency key;
- reversal uses a linked inverse entry and never mutates history;
- journal, source link/state, audit and outbox share one Unit of Work.

The following materially different business choices are not selected by source,
history, sealed requirements or PR #69:

1. collection requires a pre-posted voucher;
2. collection atomically creates/posts its voucher and journal;
3. collection remains operational and a later governed settlement posts it.

Debit/credit mappings, cash/bank/clearing accounts, subledger authority, FX
rounding policy, SoD roles/thresholds and period reopen/override authority are
also absent. Therefore:

`DEP-009 = OWNER/ACCOUNTING DECISION REQUIRED — BOUNDED ITEM`

Status-only posting must remain unavailable until that choice and mapping pack
are authoritative. No guessed mapping is permitted.

## DEP-010 proposed audit design

Preserve every existing byte and verification rule as `V1`. Define an inactive
`V2` canonicalizer covering every persisted semantic field with explicit
field order, invariant UTC/culture/number formatting, length framing and null
representation. A future persisted `HashVersion` and stream sequence/lock are
DBP-004 work. Mixed V1/V2 verification must never rehash historical V1 rows.

`DEP-010 = DESIGN PREPARED — ACTIVATION BLOCKED BY DBP-004 AND SANITIZED LEGACY SAMPLE`

## Ordered packages

| Package | Current result |
|---|---|
| `W3-A / REM-300` | governance ADR prepared; requires Control Tower approval before Product code |
| `W3-B / REM-310` | invariant matrix prepared; accounting choices isolated to bounded owner decision |
| `W3-C` | fail-closed status-only posting guard is planned, but cannot precede approved DEP-008 entry |
| `W3-D` | orchestration/ledger/mapping/audit/outbox contracts planned; mapping stays injected authority |
| `W3-E` | payload-complete Waybill finance idempotency hardening planned after entry |
| `W3-F/G / REM-320` | inactive V2 canonicalizer and transaction-aware audit planned after DEP-008 approval |
| `W3-H / DBP-004` | proposal requires legacy sample, stream ordering, append-only DB controls and safe-copy evidence |
| `W3-I / DBP-005` | physical accounting proposal waits for DEP-009 decision and safe-copy evidence |

## Required tests

Code-only tests must reject unbalanced, missing-mapping, closed-period,
permission/SoD, cross-tenant and invalid-currency posting; prove status-only post
has zero partial effects; compare complete idempotency payloads; preserve V1
known vectors; make every V2 persisted field hash-significant; and inject
failure at every orchestration stage.

PostgreSQL tests after DB-GOV must prove one journal under concurrent same-source
posting, atomic source/journal/audit/outbox, duplicate source/reversal denial,
raw-SQL posted-history protection, mixed V1/V2 verification and safe-copy
backup/restore/reconciliation.

## Preservation and rollback

No posted journal, collection, financial link or legacy audit row may be edited
or deleted. Code-only increments use isolated normal revert. Database work is
forward-only after authorized rehearsal; recovery is forward correction or the
verified safe-copy restore. Any imbalance, partial POSTED state, unaudited write,
cross-tenant link or reconciliation mismatch stops the affected package.

## External evidence gates

- authoritative accounting mapping/subledger/FX/SoD/period decision;
- sanitized legacy audit vectors;
- authorized non-Production applied lineage, roles/triggers and reconciliation
  population;
- recoverable backup digest and restore proof;
- independent DB-GOV decisions for DBP-004/005.
