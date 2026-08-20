# TransportERP — P2-C01-A Independent Review Assignment

**Date:** 2026-08-20 UTC+3  
**Phase:** `P2-C01-A — Waybill Core`  
**Baseline:** `master@4571bba98458f5211982114f366940e04420d4e7`  
**Status:** `ASSIGNED — REVIEW REQUIRED BEFORE MERGE`

## Scope under review
P2-C01-A implements only:
- OperationalParty persistence/search/create;
- Waybill draft header;
- Sender/Receiver/Payer snapshots;
- Waybill item draft lines;
- validation;
- submit/return/cancel workflow needed by the approval path;
- approval with server-authoritative atomic NumberSequence + NumberReservation;
- minimal API endpoints required by W2-P2C01-001..009 and W2-P2C01-003..007;
- migration and tests for this phase;
- only SHP-005..SHP-014 surfaces required for P2-C01-A if UI is introduced.

## Explicit exclusions
No Release, Allocation, Trip, Manifest, Movement runtime, Arrival, Warehouse, Delivery, POD, Collection settlement, Commissions, Financial Close, GPS, Customs full domain, Ticketing, Maintenance, or P2-C01-B work.

## Mandatory checks
1. W0-5 shared contracts are reused; no duplicate Money/Address/Party/Movement/Numbering definitions.
2. Official WaybillNo is null in draft and issued only in one server transaction during approval.
3. Number reservation retry by Idempotency-Key returns the same reservation/result.
4. COMMITTED and VOID numbers are never reused.
5. Company/Branch scope is enforced on reads and writes.
6. Optimistic concurrency is enforced on mutable draft aggregates.
7. At least one item plus sender and receiver are required before submit/approval.
8. Operational parties do not automatically create accounting accounts.
9. Cancellation retains an issued official number.
10. P1 entities/lifecycle remain unchanged except additive references/configuration required for P2-C01-A.
11. Migration is additive and reversible.
12. API permission checks match the W0-3 security matrix.
13. Tests cover multi-draft numbering order, duplicate approval retry, validation failure, scope denial, concurrency, and cancellation-number retention.
14. No next phase files or runtime behavior are included.

## Review output
The reviewer must inspect the exact final head, final PR diff, migration, tests, and CI evidence and return one explicit result: `PASS` or `FAIL`.

Any head change after review invalidates the decision.

## Gate
`P2-C01-B MUST NOT START` until P2-C01-A receives green CI, explicit final PASS, and is merged into master.
