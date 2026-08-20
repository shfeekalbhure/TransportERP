# TransportERP — P2-C01-B Independent Review Assignment

**Date:** 2026-08-21 UTC+3  
**Phase:** `P2-C01-B — Payment Plan, Collections & Financial Status`  
**Baseline:** `master@c3f982d3f2c2197267af1bdfe4f0ddcd4df04d60`  
**Status:** `ASSIGNED — REVIEW REQUIRED BEFORE MERGE`

## 1. Scope to review

The reviewer must inspect only the B package authorized after P2-C01-A:

- W1-P2C01-002 financial-status realization only;
- W1-P2C01-007 PaymentPlanLine;
- W1-P2C01-008 CollectionTransaction;
- W1-P2C01-023 FinancialLink only where needed to reference P1 finance documents;
- W2-P2C01-011 SetPaymentPlan;
- W2-P2C01-012 RecordCollection;
- W2-P2C01-013 ReverseCollection;
- SHP-009 / SHP-010 / SHP-011 / SHP-012 surfaces;
- additive B migration, tests, security, audit, idempotency and concurrency.

## 2. Mandatory FAIL conditions

FAIL if the package introduces runtime work from P2-C01-C or later, including Release, Allocation, Trip, Manifest, Load, Movement runtime, Arrival, Warehouse, Customs clearance, Delivery/POD, commissions, TripSettlement/financial close, GPS/Fleet, ticketing, maintenance or HR.

## 3. Mandatory checks

1. A's closed Waybill foundation and numbering behavior are inherited, not rewritten.
2. Payment plan and actual collection are separate concepts.
3. Accepted collection is immutable and cannot be physically deleted.
4. Reversal creates a separate referenced transaction and requires a reason.
5. `ClientOperationId` makes RecordCollection idempotent under retry/concurrency.
6. Currency identity and exchange-rate snapshot are retained for accepted monetary transactions.
7. FinancialStatus is derived from accepted net collections and is not a user-editable field.
8. Company/Branch scope is enforced in API and persistence.
9. Collector/payer/method/amount/currency/correlation are auditable.
10. FinancialLink does not bypass P1 posting/reversal contracts or hard-code accounts.
11. EF migration is additive and does not rewrite P1 or A migrations.
12. P1 and P2-C01-A regression tests remain green.
13. SHP-009/010/011/012 are Arabic RTL and Desktop has no direct DB access.
14. No later-phase table, endpoint or production screen is introduced.

## 4. Required evidence for PASS

- final PR diff on exact final head SHA;
- B scope validator PASS;
- green CI on exact final head;
- clean EF model/migration check;
- PostgreSQL 18 integration evidence;
- P1 + A regression evidence;
- B-specific unit/integration/HTTP evidence;
- Desktop RTL build evidence;
- explicit final `PASS` or `FAIL` decision.

Any head movement after review invalidates the review and requires a new review.

## 5. Gate

`P2-C01-C MUST NOT START` until P2-C01-B receives green exact-head CI, explicit independent PASS, and is merged into master.
