# BATCH-16 — Cash / Bank / Reconciliation — Design Authority

**Screens:** `ACC-063..ACC-069`  
**Date:** 2026-08-24  
**State:** `INDEPENDENT_REVIEW`

## Current authority
Current 57-screen baseline + current V1.3 screen content + current W2 contracts + CoreUI Transaction/ReportInquiry/ControlApproval foundations.

Unresolved physical field mappings, lookup-provider ids, DTO property names and sort keys are implementation-level `TBD-GATED`; no W1/API/DDL invention is authorized.

## Identities / actions
- ACC-063 Cash Box Transfer — `Transaction / Transfer` — View/Create/Edit/Cancel/Post/Reverse.
- ACC-064 Cash Deposit to Bank — `Transaction / Transfer` — View/Create/Edit/Cancel/Post/Reverse.
- ACC-065 Bank Withdrawal to Cash — `Transaction / Transfer` — View/Create/Edit/Cancel/Post/Reverse.
- ACC-066 Bank Reconciliation — `Transaction / Reconciliation` — View/Create/Edit/Cancel/Match/Finalize/Reopen.
- ACC-067 Cash Box Movement — `ReportInquiry / Statement` — View/DrillDown/Export/Print.
- ACC-068 Bank Movement — `ReportInquiry / Statement` — View/DrillDown/Export/Print.
- ACC-069 Cashier Shift Closing — `ControlApproval / VarianceControl` — View/Execute/Approve/Reject/Return/Reopen.

No screen-specific action outside these sets is authorized. Final posting/reversal/reconciliation/approval/shift-close authority remains server-side.

`TEAM-D01..D05 = PASS`; next gate: TEAM-D06 independent review.