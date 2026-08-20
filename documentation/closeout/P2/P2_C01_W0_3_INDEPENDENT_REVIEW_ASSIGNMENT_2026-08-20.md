# TransportERP — P2-C01 W0-3 Independent Review Assignment

**Date:** 2026-08-20 UTC+3  
**Phase:** `W0-3 — Contract Reconciliation`  
**Release:** `P2-C01-WAYBILL-SHIPPING-2026-08`  
**Baseline:** `master@545d9ed8f0859e313c78ef13f6ef6edbdeb3c11c`  
**Review status:** `ASSIGNED — REVIEW REQUIRED BEFORE MERGE`

## 1. Purpose

This assignment authorizes an independent review of the P2-C01 contract package only. It does not authorize physical schema, migrations, runtime API implementation, or production UI implementation.

The reviewer must return one explicit decision only: `PASS` or `FAIL`. `PASS_WITH_NOTES` is not sufficient for merge authorization.

## 2. Review scope

The reviewer must verify the complete W0-3 package:

- 27 W1 data contracts: `W1-P2C01-001..027`.
- 36 W2 action/API contracts: `W2-P2C01-001..036`.
- 43 W3 screen contracts: `W3-P2C01-001..043` / SHP-005..SHP-048 in the approved scope.
- 35 UAT scenarios: `UAT-P2C01-001..035`.
- 40 governing business rules: `BR-SHP-001..040`.
- Security and isolation matrix.
- Offline / Sync policy.
- P2-C01 owner scope decision.
- Automated validator and CI workflow.

## 3. Mandatory review checks

1. Every W1/W2/W3/UAT/BR identifier is unique and sequential within its declared range.
2. Every W2 W1 reference resolves to an existing W1 contract.
3. Every W3 action reference resolves to W2 and every W3 data reference resolves to W1.
4. Every UAT and BR cross-reference resolves to the declared W1/W2/W3/UAT set.
5. Every W2 and W3 permission is present in the security/isolation matrix.
6. Every W3 contract requires RTL and defines fields, states, validation, empty/load/error behavior, accessibility, audit, and offline policy.
7. Every W1/W2/W3 row has a canonical contract test identifier.
8. W1 remains `CONTRACT_ONLY`; there is no premature physical implementation authorization.
9. No P1 entity, lifecycle, migration, or closed P1 behavior is changed by this phase.
10. No hidden runtime implementation is introduced under the guise of contract reconciliation.
11. Official numbering remains server-authoritative, atomic, and idempotent.
12. Quantity constraints prevent over-release, over-allocation, over-load, and over-delivery.
13. Operational and financial states remain independent.
14. Movement and accepted collection history remain append-only / reversal-based.
15. Driver/vehicle commissions derive from actual Trip/Manifest execution, not the entire waybill.
16. Offline operations remain queued/captured only where authorized; approval, official numbering, and financial close/reopen stay server-authoritative.

## 4. Required evidence

Before issuing `PASS`, the reviewer must inspect:

- the final PR diff;
- `validate_p2_c01_contracts.py` result;
- `P2 C01 W0-3 contract validation` workflow result;
- `P2 foundation validation` workflow result;
- confirmation that the PR is based on `master@545d9ed8f0859e313c78ef13f6ef6edbdeb3c11c` or a later master that only contains already-closed predecessor work;
- confirmation that no later phase files were added.

## 5. Output

The independent review report must record:

- reviewed head SHA;
- PR number;
- validator result;
- CI results;
- findings and corrective actions, if any;
- explicit final decision: `PASS` or `FAIL`.

If the reviewed head changes after the report is issued, the review is invalidated and must be rerun against the new head.

## 6. Phase gate

`W0-5` MUST NOT START until W0-3 receives final independent `PASS` on the final head and the W0-3 PR is merged into `master`.
