# TransportERP — P2-C01 W0-5 Independent Review Assignment

**Date:** 2026-08-20 UTC+3  
**Phase:** `W0-5 — Shared Kernel`  
**Baseline:** `master@96551dd2f99650f8e58a2df184bbdb95e6b0ff7e`  
**Status:** `ASSIGNED — REVIEW REQUIRED BEFORE MERGE`

## 1. Review scope

The independent reviewer must inspect the complete W0-5 branch and verify that it only introduces provider-neutral shared-kernel assets required by the already-closed W0-3 P2-C01 contracts.

The review covers:

- MoneyAmount / FxSnapshot;
- operational-party snapshots and roles;
- attachment metadata;
- append-only movement envelope;
- Geo address snapshot;
- authoritative NumberReservation boundary;
- W0-5 tests;
- phase-boundary validator and CI evidence.

## 2. Mandatory checks

1. Existing P1 and W0-3 contracts are not reopened or weakened.
2. No Infrastructure migration, physical Waybill schema, runtime API, or production SHP UI is introduced.
3. Money values always carry CurrencyId and FX uses a historical immutable snapshot.
4. Same-currency FX cannot silently use a non-1 rate.
5. Operational party snapshots do not create or imply an accounting account.
6. Attachment metadata requires storage reference, content hash, actor, and timestamp while binary storage stays external.
7. Movement envelope carries company/branch scope, actor, correlation, event time, retry identity, and reversal reference; a movement cannot reverse itself.
8. NumberReservation remains server-authoritative and idempotent, with known RESERVED/COMMITTED/VOID states.
9. Shared-kernel tests pass together with all existing P1/Foundation tests.
10. Phase validator returns PASS and confirms only W0-5-allowed file paths changed.

## 3. Review output

The reviewer must record the exact final head SHA and return one explicit decision: `PASS` or `FAIL`.

Any head change after the review invalidates the decision and requires a fresh review.

## 4. Phase gate

`P2-C01-A MUST NOT START` until W0-5 has final green CI, explicit independent `PASS`, and is merged into `master`.
