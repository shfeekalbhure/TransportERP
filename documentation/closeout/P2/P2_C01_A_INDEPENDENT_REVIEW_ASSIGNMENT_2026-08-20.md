# TransportERP — P2-C01-A Independent Review Assignment

**Date:** 2026-08-20 UTC+3  
**Phase:** `P2-C01-A — Waybill Foundation`  
**Baseline:** `master@4571bba98458f5211982114f366940e04420d4e7`  
**Status:** `ASSIGNED — REVIEW REQUIRED BEFORE MERGE`

## 1. Scope to review

The reviewer must inspect only the first runtime Waybill package authorized after W0-5:

- Waybill draft aggregate and lifecycle;
- operational Sender / Receiver / Payer parties;
- Waybill items and basic measurements/risk flags needed for validation;
- validation, submit, return-for-correction, cancel, and approval;
- atomic server-authoritative official numbering using the W0-5 NumberReservation boundary;
- PostgreSQL persistence and one additive migration for this package only;
- W2 API endpoints required by P2-C01-A;
- Desktop Transaction screen surface required by SHP-005/SHP-006/SHP-007/SHP-008/SHP-014 for this package;
- phase tests, migration evidence, security/scope checks, audit, idempotency, and concurrency.

## 2. Explicit exclusions

The reviewer must FAIL the phase if it introduces runtime work from a later package, including:

- payment plan / collection ledger (`P2-C01-B`);
- Release / Allocation / Trip / Manifest / Load (`P2-C01-C` and later);
- Movement/Arrival/Warehouse runtime flows beyond audit/lifecycle evidence needed for A;
- Customs full runtime domain;
- Delivery/POD/COD;
- commissions, trip settlement, or financial close;
- GPS/Fleet runtime implementation.

## 3. Mandatory checks

1. W0-5 shared types are reused; no duplicate Money, FX, Address, Party snapshot, Attachment, Movement, or NumberReservation contract is invented.
2. Domain rules are implemented in Domain/Application, not Desktop or database triggers alone.
3. Draft has no official Waybill number.
4. Approval requires required parties and at least one valid positive-quantity item.
5. Official numbering is server-authoritative, atomic, idempotent, and committed/voided numbers are never reused.
6. Repeated approve with the same idempotency key returns the same approved result.
7. Optimistic concurrency prevents silent overwrite of stale drafts.
8. Company/Branch scope is enforced in API and persistence queries.
9. Operational party creation does not automatically create an accounting account.
10. Audit records create/update/submit/return/approve/cancel and sensitive identity access where applicable.
11. Migration is additive and does not reopen or rewrite P1 tables/lifecycle.
12. All P1/W0-3/W0-5 regression tests continue to pass.
13. P2-C01-A tests cover happy path, negative validation, concurrency, scope, idempotency, and numbering.
14. Desktop screen uses the governed Transaction/ControlApproval profiles and RTL behavior; no direct DB access from Desktop.
15. No later-phase endpoint, table, or production screen is introduced.

## 4. Required evidence before PASS

- final PR diff on exact final head SHA;
- green P2-C01-A CI on exact final head;
- migration/schema validation result;
- P1/W0-3/W0-5 regression result;
- phase-specific test result;
- changed-file boundary validation;
- explicit final decision `PASS` or `FAIL`.

Any head change after review invalidates the review and requires a new review.

## 5. Gate

`P2-C01-B MUST NOT START` until P2-C01-A receives final green CI, explicit independent PASS, and is merged into `master`.
