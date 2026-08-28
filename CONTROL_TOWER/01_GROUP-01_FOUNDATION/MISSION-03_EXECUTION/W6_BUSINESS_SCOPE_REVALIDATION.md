# W6 — Business Scope Revalidation

- Baseline: `cc67ad2bd491ed3ab23c3144f11dff955353c3a4`
- State: `PREPARATION MAY CONTINUE — NO PRODUCT PACKAGE HAS PASSED ENTRY`
- Product/DB mutation: `NONE`

## REM-600 Shipping

Current Shipping ends at `LOAD`/`DEPART`. Arrival/unload/delivery/POD/claim/
return/customs/settlement routes do not exist, and an exact API contract test
expects those routes to return 404. Dormant `ActualArriveAt` and status values do
not constitute a lifecycle.

The preserved P2-D branch contains an arrival/transit candidate but is an old,
unmerged audit candidate, not authority. It uses claim authority, adds model
without a migration, auto-generates migration work in CI, lacks PostgreSQL
integration/concurrency evidence and does not cover the full later lifecycle.
Disposition: `COMPARE → REIMPLEMENT SELECTIVELY AFTER AUTHORITY/DEPENDENCIES`.

## REM-610 Ticketing

Ticketing has no Product entity, model, migration, API, application service,
client or test. Existing CSV/domain references are `DOCUMENTED_ONLY` or legacy
lineage. The exact Ticketing Library artifacts are accessible and were read.
They contain `DEC-TRV-001..006`, a screen register and detailed `TRV-*`
contracts, but each states that it does not itself grant programming,
DDL/API/DTO/Permission or Offline-write authority. DBP-008 therefore has useful
non-governing input but still lacks a canonical programming package.

## REM-620 Screens/Kurrasa

Current Desktop uses `SHP-*` identities; some are explicitly classified
`NON_GOVERNING_LINEAGE`, while FLOW01 and older P2 registers use conflicting or
review-pending identities. The final alias/supersession crosswalk and several
provider/API/sort identifiers are absent. No rename, route binding or client
implementation is authorized from this collision set.

## Ordered preparation

| Package | Gate/result |
|---|---|
| `W6-G0` | preserve/hash current, P2-D, PR69, screens-workspace and exact Library sources; completed as evidence inventory |
| `W6-620A` | immutable screen/alias/route/DTO/permission/test/supersession inventory; closure requires external authority |
| `W6-600A` | arrival/transit contract comparison only; no runtime activation |
| `DBP-007A` | proposal for arrival/holding/movement tenant keys, audit/UoW, idempotency, safe-copy and recovery |
| `W6-600B+` | code, persistence/API, Offline/clients and later lifecycle increments only after their gates |
| `W6-610A` | recover/version/supersede Ticketing authority package before any DTO/schema/code |
| `DBP-008` | remains blocked until canonical Ticketing W1/W2/W3/UAT package exists |

Reporting/T-620/DBP-009 appears in the sealed DB register without a separate REM
package. This is recorded as
`PLAN DEVIATION — CONTROL TOWER REVALIDATION REQUIRED` for Reporting scope only;
it does not authorize expansion of REM-600/610/620.

## External evidence required

- adoption/supersession authority for the reachable analysis-only Kurrasa and
  screen workspace; that workspace reports `READY FOR PROGRAMMING = 0`;
- accepted post-departure Shipping increment and accounting/Offline binding;
- programming authority that promotes or supersedes the now-readable Ticketing
  artifacts; their current status is design/contract evidence only;
- canonical Ticketing W1/W2/W3/UAT contracts;
- DB-GOV decisions for DBP-007/008 and clarification of DBP-009 remediation
  ownership.
