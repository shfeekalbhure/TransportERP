# BATCH-14 — Accounting Lifecycle & Control Screens — Final Design Closure

**Screens:** `ACC-053`, `ACC-054`, `ACC-055`, `ACC-056`, `ACC-057`  
**Date:** 2026-08-24  
**State:** `DESIGN_APPROVED`  
**Independent review:** `TEAM-D06 PASS — 0 open design findings`

## Current governing authority
- `TransportERP_FINAL_EXECUTION_REGISTER_V1.0.xlsx / Current57` — specification closed in unified candidate.
- `CURRENT_TRANSPORTERP_SCREEN_BASELINE_V1.1.csv` — exact current fields/tabs/grid/actions/profile/variant.
- `SCREEN_FIELD_ACTION_WORKFLOW_CROSSWALK.xlsx` — current field QA, tabs and grid inventory; current V1.3 elements are classified `ALREADY_PRESENT`.
- `Field_Traceability_Matrix_TransportERP_V1_CANDIDATE.xlsx` — UI type/requiredness/edit/read-only semantics; unresolved physical mappings are implementation gates only.
- `Screen_to_API_and_Permission_Traceability_TransportERP_V1.7.xlsx` + API Contract V1.7 — exact routes, permissions, concurrency/idempotency/posting/reversal/approval/SoD contracts.
- CoreUI Transaction / ControlApproval shared foundations.

Older per-screen intelligence dossiers marked PARTIAL are working records and do not override the stronger current closed execution register and crosswalk.

## Canonical identities
| Screen | Arabic | Profile | Variant | Fields | Screen-specific grid |
|---|---|---|---|---:|---|
| ACC-053 | الأرصدة الافتتاحية للحسابات ومراكز التكلفة | Transaction | Opening | 10 | 9 display columns (1 ordinal + 8 data) |
| ACC-054 | إغلاق وفتح الفترات المحاسبية | ControlApproval | PeriodAction | 9 | N/A — no concrete screen grid |
| ACC-055 | قيود التسوية والإقفال السنوي | Transaction | Closing | 11 | 9 display columns (1 ordinal + 8 data) |
| ACC-056 | القيود العكسية وربط القيد الأصلي بالقيد العاكس | Transaction | Reversal | 10 | 9 display columns, read-only details |
| ACC-057 | طلبات الاعتماد المحاسبية | ControlApproval | ApprovalQueue | 11 | shared Queue Grid/DetailsHost; no screen-specific columns |

## Transaction line/detail columns — ACC-053 / ACC-055 / ACC-056
Exactly: `# | الحساب | مركز التكلفة/الأبعاد | البيان | مدين | دائن | العملة | سعر الصرف | المبلغ المحاسبي`.

The ordinal `#` is presentation-only. Exact DTO property names, persistence mappings, lookup-provider ids and server sort keys remain implementation-owned. For ACC-056 the detail grid is read-only because no line-edit field/action is issued and reversal creation is server-authoritative.

## W2 executable surface
- ACC-053: `View | Create | Edit | Cancel | Post` — six endpoints including list/get.
- ACC-054: `View | Execute | Approve | Reject | Return | Reopen` — seven endpoints; Approval Contract + SoD; expected versions and target-state recheck.
- ACC-055: `View | Create | Edit | Cancel | Post | Reverse` — seven endpoints; lifecycle/posting/reversal/idempotency/concurrency.
- ACC-056: `View | Create | Edit | Cancel | Reverse` — six endpoints; reversal route is server-authoritative.
- ACC-057: `View | Execute | Approve | Reject | Return` — six endpoints; Approval Contract + SoD.

No screen-specific Print/Export/Delete/Disable/Enable/attachment mutation is authorized for this batch.

## Layout authority
Transaction variants reuse the shared Transaction host. ControlApproval uses shared decision/history/Queue Details foundations. `ACC-054` has no concrete screen grid. `ACC-057` has no screen-specific business column inventory to invent.

## Design-only boundary
Current V1.3 fields/tabs/grids are governing UI semantics. Field Trace rows that remain `NEEDS_REVIEW` / `LOOKUP_REVIEW` do not block design and do not authorize physical W1 columns, DTO properties, API changes or lookup-provider inventions. They remain `TBD-GATED` for implementation.

## Pre-PASS correction
TEAM-D06 required one correction in ACC-057: existing ApprovalRequest target/context values (`نوع المستند`, `المستند المرتبط`, and request-note context) are read-only in the approval queue because W2 issues no ApprovalRequest Create/Edit action on this screen. The correction changed UI editability only and introduced no API/DTO/permission authority.

## Final gates
- `TEAM-D01 ANALYSIS = PASS`
- `TEAM-D02 LAYOUT = PASS`
- `TEAM-D03 FIELD_GRID = PASS`
- `TEAM-D04 UX = PASS`
- `TEAM-D05 VISUAL = PASS`
- `TEAM-D06 INDEPENDENT_REVIEW = PASS`
- `DESIGN-LEAD = DESIGN_APPROVED`

Independent review evidence: `documentation/design/batches/BATCH-14_INDEPENDENT_REVIEW_2026-08-24.md`.

## Runtime boundary
Runtime/acceptance/release evidence was not executed by this design closure. `DESIGN_APPROVED` is not a runtime PASS claim.
