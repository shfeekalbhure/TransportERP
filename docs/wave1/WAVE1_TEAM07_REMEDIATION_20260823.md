# TransportERP WAVE-1 — TEAM-07 FAIL Remediation Record

**Date:** 2026-08-23  
**PR:** #58  
**Branch:** `wave1-screen-readiness-20260822`  
**Previous reviewed SHA:** `1b0efb4ac785be5f90f225a7be62c606df985854`  
**Previous independent verdict:** `INDEPENDENT REVIEW: FAIL ON 1b0efb4ac785be5f90f225a7be62c606df985854`

This record does not self-grant PASS. It records the remediation evidence that must be independently re-reviewed on the final exact SHA after all required CI gates are green.

## Findings received

TEAM-07 reported seven delivery-blocking findings: four HIGH and three MEDIUM.

| ID | Severity | Scope |
|---|---|---|
| F-01 | HIGH | GEN-013 protected action Approval binding |
| F-02 | MEDIUM | GEN-013 legacy ArabicName backfill |
| F-03 | HIGH | GEN-003 mandatory Print audit |
| F-04 | HIGH | ACC-049/050/058/074/075 mandatory Export audit |
| F-05 | HIGH | ACC-074/075 source-document Company/Branch scope |
| F-06 | MEDIUM | Delivery workbook Source Authority superseded/current contradiction |
| F-07 | MEDIUM | PostgreSQL migration execution/order not proven |

## F-01 — GEN-013 Approval binding

Remediation:

- Protected request carries `ApprovalRequestId` through the exact `NumberingProtectedActionRequest` contract.
- Runtime requires `GEN013.Override`, valid operation scope, reason and expected target version as before.
- Service loads the governing ApprovalRequest and checks:
  - TargetType = `NumberSequence`.
  - TargetId = target sequence.
  - RequestedAction = `Override/Reset`.
  - Status = `APPROVED`.
  - CompanyId = authenticated company.
  - BranchId = the target sequence branch semantics.
  - TargetExpectedVersion = protected request ExpectedVersion.
- A corresponding append-only ApprovalAction with decision `APPROVE` is required.
- Missing/mismatched/stale approval fails with `APPROVAL_STATE_INVALID`; inactive numbering state fails with `NUMBERING_STATE_INVALID`.
- Successful audit stores reason plus approval request/version/action/approver/time/decision lineage.
- Tests cover missing approval, valid approval and audit trace.

Authority basis: Current GEN-013 W2 row + Approval Contract V1 + SoD rule for Numbering Override/Reset.

## F-02 — no guessed legacy ArabicName

Remediation:

- `Wave1NumberSequenceMetadataRecord.ArabicName` is nullable for historical unresolved data.
- `NumberSequenceDto.ArabicName` is nullable.
- Migration may derive technical Code from legacy DocumentType.
- Migration does **not** infer ArabicName from DocumentType; historical ArabicName is inserted as NULL.
- Runtime DTO does not substitute DocumentType as an Arabic business name.
- Tests assert the unknown value remains unknown until governed reconciliation/touch.

## F-03 / F-04 — mandatory delivery audit

A shared `Wave1DeliveryAuditWriter` was introduced for governed successful print/export access.

Remediated routes:

- GEN-003 Print — `GEN003.Print`.
- ACC-049 Export — `ACC049.Export`.
- ACC-050 Export — `ACC050.Export`.
- ACC-058 Export — `ACC058.Export`.
- ACC-074 Export — `ACC074.Export`.
- ACC-075 Export — `ACC075.Export`.

The audit payload records actor, company/branch context, correlation ID, screen/action and serialized filter/request context. Audit is awaited before the successful response is returned; it is not a best-effort post-response log.

## F-05 — aging source scope

The normalized SourceDocument resolver now receives the authorized company and branch context and revalidates ownership for each supported source aggregate:

- ReceiptVoucher.
- PaymentVoucher.
- JournalEntry.
- Waybill.

Known-type IDs from another company or branch are not resolved. Unknown types remain fail-closed. A negative test seeds a foreign-scope source and proves the report cannot consume it.

## F-06 — Source Authority workbook

The delivery audit workbook `Source Authority` sheet was remediated to current authority:

- Current Approved References V1.26.
- Unified Execution Book V1.3.
- Screen_to_Entity_Traceability V1.2.
- DB_Constraint_Matrix V1.2.
- Permission_Matrix V1.2.
- API_Contract_Matrix V1.7.
- Screen_to_API_and_Permission_Traceability V1.7.

V1.25 / Screen-to-Entity V1.1 / Permission V1.1 / API V1.6 / Screen-to-API V1.6 are explicitly labelled `HISTORICAL / SUPERSEDED` and forbidden for current release gating. Formula error scan returned zero matches.

The workbook must be restamped to the final exact SHA only after final CI completes.

## F-07 — PostgreSQL 18 evidence

A dedicated workflow `.github/workflows/wave1-postgresql-delivery-gate.yml` now runs PostgreSQL 18 and the test `Wave1PostgreSqlMigrationOrderTests`.

The gate executes the governed dependency order:

`Base/P2 → Wave1Geo → Wave1Reference cleanup → Wave1CountryAuthority → Wave1NumberingAuthority → Wave1AccountingAuthority`

It verifies:

- a legacy Country survives physical promotion with ISO2/ISO3/DialingCode still NULL rather than guessed;
- a legacy NumberSequence preserves technical Code while ArabicName remains NULL;
- approval_requests / approval_actions are created;
- account_groups / account_types / open_items / payment_allocations / cash-flow mapping/override tables are created;
- rejected `account_classifications` and `accounting_open_items` are absent;
- separate EF migration-history tables contain the expected migration IDs.

The new gate initially exposed malformed hand-written FK argument ordering. That was remediated in GEN-013 directly and in the compact WAVE-1 accounting migration compatibility path. PostgreSQL 18 then passed on implementation SHA `9a8dd543cf01553fb3e37e856e46d18c61b53ec9`, run `32610663248`.

## Pre-documentation green evidence

Implementation SHA `9a8dd543cf01553fb3e37e856e46d18c61b53ec9`:

- W0-3 run `32610663251` — SUCCESS.
- Foundation run `32610663229` — SUCCESS.
- PostgreSQL 18 delivery gate run `32610663248` — SUCCESS.

These runs prove the remediation implementation before documentation freeze. They do **not** authorize review/merge of a later documentation commit.

## Final gate

After this remediation record and readiness evidence are committed:

1. determine the new exact branch-head SHA;
2. run W0-3, Foundation and WAVE1 PostgreSQL delivery gate on that exact SHA;
3. restamp Word/PDF/Excel/ZIP/library artifacts to that same SHA and record hashes;
4. create a **new** TEAM-07 read-only independent review assignment for that exact SHA;
5. require a literal PASS or FAIL verdict;
6. keep PR #58 Draft / DO NOT MERGE until PASS exists on the same exact SHA.

No previous PASS exists. The previous independent result is FAIL and remains historical evidence.
