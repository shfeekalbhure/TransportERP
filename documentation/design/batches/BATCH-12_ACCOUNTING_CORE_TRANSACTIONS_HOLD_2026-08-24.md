# BATCH-12 — Accounting Core Transactions — Authority Gate

**Screens:** `ACC-042`, `ACC-043`, `ACC-044`, `ACC-045`  
**Date:** 2026-08-24  
**Current state:** `FIELD_GRID / HOLD_AUTHORITY`  
**Owner:** `DESIGN-LEAD / ORCHESTRATOR`

## Completed design stages
- `TEAM-D01 ANALYSIS = PASS`
- `TEAM-D02 LAYOUT = PASS`
- `TEAM-D03 FIELD_GRID = HOLD_AUTHORITY`

## Governing identity and exact executable surface
- `ACC-042 — القيد اليومي` = `Transaction / HeaderLines`; W2 actions exactly View/Create/Edit/Cancel/Post/Reverse.
- `ACC-043 — سند القبض` = `Transaction / HeaderLines`; W2 actions exactly View/Create/Edit/Cancel/Post/Reverse.
- `ACC-044 — سند الصرف` = `Transaction / HeaderLines`; W2 actions exactly View/Create/Edit/Cancel/Post/Reverse.
- `ACC-045 — سند التحويل بين الصناديق والبنوك` = `Transaction / Transfer`; W2 actions exactly View/Create/Edit/Cancel/Post/Reverse.

Current W2 exposes seven transaction endpoints per screen: List, Get, Create draft, Update draft, Cancel, Post and Reverse. No screen-specific Print/Export, attachment mutation, or Approve/Reject/Return endpoint/permission is promoted here.

## Shared current V1.3 transaction contract
Functional tabs for all four:
1. البيانات الرئيسية
2. التفاصيل والحركات
3. المرفقات والربط بالمستندات
4. الاعتمادات
5. سجل العمليات

The Attachments tab does not authorize Upload/Download/Delete without explicit W2 binding. The Approvals tab does not authorize direct approval decisions on these screens; approval queue/decision authority remains separate (`ACC-057`) where applicable.

Posted records are immutable; Cancel/Reverse/Post are explicit permission/state-bound commands and server authority is final.

## Current field/grid inventory
### ACC-042
11 fields: رقم المستند | التاريخ المحاسبي | المرجع | الوصف | العملة | سعر الصرف | الحساب | مركز التكلفة | مدين | دائن | الحالة.

Governing line grid: 9-column contract centered on `# | الحساب | مركز التكلفة/الأبعاد | البيان | مدين | دائن | العملة | سعر الصرف | المبلغ المحاسبي`.

### ACC-043 / ACC-044 / ACC-045
11 fields: رقم السند | التاريخ | الطرف | الصندوق/البنك المصدر | الصندوق/البنك الوجهة | العملة | المبلغ | سعر الصرف | الحساب المقابل | البيان | الحالة.

Governing line grid: 8-column contract `# | الطرف/الجهة | الحساب المقابل | البيان | العملة | المبلغ | سعر الصرف | المبلغ المحاسبي`.

## Why FIELD_GRID is held
Latest specialist review still classifies the following field-level authority gaps as `BLOCKING / REVIEWED — NEEDS FIX`:
- ACC-042: التاريخ المحاسبي, المرجع, الوصف (`FBR-035..037`).
- ACC-043: التاريخ, البيان (`FBR-038..039`).
- ACC-044: التاريخ, البيان (`FBR-040..041`).
- ACC-045: التاريخ, البيان (`FBR-042..043`).

The issue is not whether the fields exist in V1.3; they do. The unresolved item is the exact persisted/read/API/permission/audit/offline field-level mapping. No mapping is invented.

## One owner decision requested
**Recommended design-only decision:** authorize TEAM-D03 to treat the current V1.3 field/tab/grid inventory as the governing UI design contract for ACC-042..045 and to issue UI-only metadata (ValueType/UI semantic, required/read-only/edit policy, field/grid order, CoreUI width/editor/selection presentation), while every unresolved W1/DTO/API/permission/DDL/lookup/attachment/print binding remains explicit `TBD-GATED` implementation authority.

This decision does NOT authorize:
- W1/DDL/schema/migration changes;
- API routes or DTO fields;
- new permissions, approval commands, Print/Export, attachment commands, or offline writes;
- accounting formulas, posting rules, exchange-rate calculations, balancing logic or numbering behavior beyond current server contracts;
- application code or official Kurrasa modification.

If approved, TEAM-D03 resumes immediately and the four screens continue through UX → VISUAL → TEAM-D06 independent review.
