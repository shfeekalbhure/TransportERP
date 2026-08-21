# P2-C01-D — Independent Review Assignment

Date: 2026-08-22
Phase: `P2-C01-D — Arrival / Transit / Warehouse`
Baseline: `master@5d58a42046e07166e6db76bcb893f32b1d8f2ec7`
Branch: `feature/p2-c01-d-arrival-transit-warehouse-20260822`

## Independence rule

The independent reviewer does not author D runtime code or fix D findings. Review begins only after the final exact head has passed the full D CI gate. Any subsequent head movement invalidates both the CI certificate and the review.

## Exact-head entry condition

Review is authorized only after a recorded certificate in the form:

`CI-GATE: PASS ON <exact-sha>`

The reviewer must verify that the PR head is still exactly that SHA before reviewing.

## Mandatory review scope

1. Full PR diff, not only primary implementation files.
2. W1 data-contract fidelity for Trip/TripStop/Manifest/ManifestLine/MovementEvent dependencies and new ArrivalReceipt/ArrivalReceiptLine/WarehouseHolding entities.
3. Effective W2 fidelity for 022, 023, 024, 035, 036, 040 and 041.
4. Effective W3 fidelity for SHP-017/018/031/032/033/034, including RR1 overrides for W3-026 and W3-029.
5. Receiving-company/branch/location isolation and non-disclosing cross-scope behavior.
6. RecordArrival/RecordUnload/Reallocate/FinalizeArrival/CloseTrip idempotency and complete command fingerprint semantics.
7. Real concurrency evidence that unload/reallocation cannot over-consume in-transit/holding balances.
8. Arrival receipt lifecycle and partial-arrival correctness; no false full-arrival state.
9. PostgreSQL-level append-only enforcement for accepted movement history and absence of direct mutation paths.
10. Atomicity between movement evidence and WarehouseHolding projection changes.
11. Trip close custody reconciliation: cargo accounted, no open driver custody, blocker dependency honored.
12. No TripSettlement/commission/accounting/financial-close side effects.
13. Transit re-dispatch preserves previous trip history and creates no new Waybill revenue.
14. Movement inquiry reconstructs history/balances from accepted ledger evidence and is read-only.
15. API routes, permissions, Company/Branch context, error mapping and offline policy exactly match effective W2.
16. Desktop Arabic RTL, text-first blockers/accessibility and no direct persistence dependency.
17. Migration is additive relative to the C-closed baseline and does not alter closed P1/A/B/C migrations.
18. No Delivery/POD/COD, notification, customs, commission/settlement, GPS/Fleet, exception-resolution or other later-phase runtime leakage.

## Required verdict

If no blocking finding remains:

`INDEPENDENT REVIEW: PASS ON <exact-sha>`

Otherwise:

`INDEPENDENT REVIEW: FAIL ON <exact-sha>`

with severity, contract ID, file/behavior evidence and required remediation.

No conditional PASS, waiver, merge or phase transition is permitted from the independent-review role.
