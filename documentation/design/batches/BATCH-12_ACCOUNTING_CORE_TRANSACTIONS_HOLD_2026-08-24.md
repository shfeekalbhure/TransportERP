# BATCH-12 — Accounting Core Transactions — Final Design Closure

**Screens:** `ACC-042`, `ACC-043`, `ACC-044`, `ACC-045`  
**Date:** 2026-08-24  
**Current state:** `DESIGN_APPROVED`  
**Owner:** `DESIGN-LEAD / ORCHESTRATOR`  
**Independent review:** `TEAM-D06 PASS / 0 open design findings`

## Completed stages
- `TEAM-D01 ANALYSIS = PASS`
- `TEAM-D02 LAYOUT = PASS`
- `TEAM-D03 FIELD_GRID = PASS` under owner-approved design-only authority
- `TEAM-D04 UX = PASS`
- `TEAM-D05 VISUAL = PASS`
- `TEAM-D06 INDEPENDENT_REVIEW = PASS`
- `DESIGN-LEAD CLOSURE = DESIGN_APPROVED`

Independent review evidence: `documentation/design/batches/BATCH-12_INDEPENDENT_REVIEW_2026-08-24.md`.

## Governing identity and exact executable surface
- `ACC-042 — القيد اليومي` = `Transaction / HeaderLines`; W2 actions exactly View/Create/Edit/Cancel/Post/Reverse.
- `ACC-043 — سند القبض` = `Transaction / HeaderLines`; W2 actions exactly View/Create/Edit/Cancel/Post/Reverse.
- `ACC-044 — سند الصرف` = `Transaction / HeaderLines`; W2 actions exactly View/Create/Edit/Cancel/Post/Reverse.
- `ACC-045 — سند التحويل بين الصناديق والبنوك` = `Transaction / Transfer`; W2 actions exactly View/Create/Edit/Cancel/Post/Reverse.

Current W2 exposes seven transaction endpoints per screen: List, Get, Create draft, Update draft, Cancel, Post and Reverse. No screen-specific Print/Export, attachment mutation, or Approve/Reject/Return endpoint/permission is promoted.

## Shared current V1.3 transaction contract
Functional tabs for all four:
1. البيانات الرئيسية
2. التفاصيل والحركات
3. المرفقات والربط بالمستندات
4. الاعتمادات
5. سجل العمليات

The Attachments tab is a governing structural area but does not authorize Upload/Download/Delete without explicit W2 binding. The Approvals tab is a governing structural/read-only area and does not authorize direct approval decisions; approval queue/decision authority remains separate where applicable.

Posted records are immutable. Cancel/Reverse/Post are explicit permission/state-bound commands and server authority is final.

## Current field/grid inventory
### ACC-042
11 fields: رقم المستند | التاريخ المحاسبي | المرجع | الوصف | العملة | سعر الصرف | الحساب | مركز التكلفة | مدين | دائن | الحالة.

Governing line grid: `# | الحساب | مركز التكلفة/الأبعاد | البيان | مدين | دائن | العملة | سعر الصرف | المبلغ المحاسبي`.

### ACC-043 / ACC-044 / ACC-045
11 fields: رقم السند | التاريخ | الطرف | الصندوق/البنك المصدر | الصندوق/البنك الوجهة | العملة | المبلغ | سعر الصرف | الحساب المقابل | البيان | الحالة.

Governing line grid: `# | الطرف/الجهة | الحساب المقابل | البيان | العملة | المبلغ | سعر الصرف | المبلغ المحاسبي`.

## OWNER DECISION — APPROVED 2026-08-24
Owner approved design-only authority to treat the current V1.3 field/tab/grid inventory as the governing UI design contract and allow TEAM-D03 to issue UI-only metadata: ValueType/UI semantic, required/read-only/edit policy, field/grid order, CoreUI width/editor/selection presentation.

Every unresolved W1/DTO/API/permission/DDL/lookup/attachment/print binding remains `TBD-GATED` implementation authority.

This approval did **not** authorize W1/DDL/schema/migrations, API routes or DTO fields, new permissions, approval commands, Print/Export, attachment commands, offline writes, accounting formulas, posting rules, exchange-rate calculations, balancing logic, numbering behavior, application code, or official Kurrasa modification.

## TEAM-D06 final disposition
`PASS` with `0` open design findings.

Review confirmed:
- identity/profile/variant correctness;
- exact tabs, field inventory and grids;
- exact W2 action surface only;
- posted immutability and expected-version/server authority;
- no client accounting formula or local CoreUI duplication;
- no unissued Print/Export/Attachment/Approval/Offline commands;
- technical field/lookup/storage gaps are explicit rather than silently designed around.

## Remaining technical gates — nonblocking for design approval
- FBR-035..043 field-level persistence/read/API/audit/offline mapping closure as applicable per screen.
- Unissued lookup provider/source/search/revalidation bindings.
- Exact DTO property/line/sort-key bindings and accountingAmount server binding.
- Attachment/approval data-source authority if later issued.
- Runtime/acceptance execution.

## Scope statement
No application code, official Kurrasa, W1/DDL, API, DTO or permission contract was modified by this design closure.
