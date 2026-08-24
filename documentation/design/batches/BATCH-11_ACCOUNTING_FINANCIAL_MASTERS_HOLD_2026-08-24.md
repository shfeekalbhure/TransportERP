# BATCH-11 — Accounting Financial Masters — Authority Gate

**Screens:** `ACC-038`, `ACC-039`, `ACC-040`  
**Date:** 2026-08-24  
**Current state:** `FIELD_GRID / HOLD_AUTHORITY`  
**Owner:** `DESIGN-LEAD / ORCHESTRATOR`

## Completed design stages
- `TEAM-D01 ANALYSIS = PASS`
- `TEAM-D02 LAYOUT = PASS`
- `TEAM-D03 FIELD_GRID = HOLD_AUTHORITY`

## Current governing identity / W2
- `ACC-038 — الصناديق` = `MasterData / Tabbed`; W2 actions: View/Create/Edit/Disable; exact 5 current routes under `ACC038.View/Create/Edit/Disable`.
- `ACC-039 — الحسابات البنكية` = `MasterData / Tabbed`; W2 actions: View/Create/Edit/Disable; exact 5 current routes under `ACC039.View/Create/Edit/Disable`.
- `ACC-040 — طرق الدفع` = `MasterData / Standard`; W2 actions: View/Create/Edit/Disable; exact 5 current routes under `ACC040.View/Create/Edit/Disable`.

## Current W3 screen inventory
### ACC-038
Tabs: البيانات الرئيسية | بيانات إضافية | الربط المحاسبي | الحدود والصلاحيات | المرفقات | سجل العمليات.

11 current design fields: رمز الصندوق | اسم الصندوق | الشركة | الفرع | العملة | حساب الأستاذ | أمين الصندوق الافتراضي | الحد الأقصى للرصيد | حد العملية الواحدة | يتطلب إقفال وردية | الحالة.

Current list/grid inventory is a 7-column display contract: الرمز | الاسم | الشركة | الفرع/النطاق | العملة | الحالة | آخر تعديل.

### ACC-039
Tabs: البيانات الرئيسية | بيانات إضافية | الربط المحاسبي | الحدود والصلاحيات | المرفقات | سجل العمليات.

14 current design fields: رمز الحساب البنكي | اسم البنك | اسم الحساب | رقم الحساب | IBAN | SWIFT/BIC | الشركة | الفرع | العملة | حساب الأستاذ | حساب رسوم البنك | حساب فروق التسوية | حد السحب/التحويل | الحالة.

Current list/grid inventory is a 7-column display contract: الرمز | الاسم | الشركة | الفرع/النطاق | العملة | الحالة | آخر تعديل.

### ACC-040
Tabs: البيانات الرئيسية | الربط المحاسبي | قواعد الاستخدام | سجل العمليات.

9 current design fields: رمز الطريقة | اسم طريقة الدفع | نوع الوسيلة | يتطلب صندوقًا | يتطلب حسابًا بنكيًا | حساب المقاصة | يسمح بالقبض | يسمح بالصرف | الحالة.

Current baseline has no concrete primary grid-column contract (`Columns=0`); TEAM-D03 must not invent one without design authority.

## Why FIELD_GRID is held
The current specialist field review still classifies multiple field-level bindings as `BLOCKING / NEEDS FIX` because exact persisted/read/API/permission/audit/offline mappings are not evidenced. Examples include:
- ACC-038: رمز الصندوق, اسم الصندوق, الحد الأقصى للرصيد, حد العملية الواحدة, يتطلب إقفال وردية.
- ACC-039: رمز الحساب البنكي, اسم البنك, اسم الحساب, SWIFT/BIC, حساب رسوم البنك, حساب فروق التسوية, حد السحب/التحويل; Account Number vs IBAN storage semantics are not fully reconciled.
- ACC-040: رمز الطريقة, اسم طريقة الدفع, نوع الوسيلة, يتطلب صندوقًا, يتطلب حسابًا بنكيًا, حساب المقاصة, يسمح بالقبض, يسمح بالصرف.

No mapping is invented. Current W1/W2 technical binding gaps remain implementation-authority gaps.

## One owner decision requested
**Recommended design-only decision:** authorize TEAM-D03 to treat the current V1.3 screen field/tab inventory as the governing **UI design contract** for ACC-038/039/040 and to define only screen-design metadata (UI semantic/value type, required/read-only/edit policy, display order, CoreUI width policy, selection/editor/lookup presentation and missing ACC-040 list-column presentation where needed), while preserving all unresolved W1/DTO/API/permission/DDL bindings as explicit `TBD-GATED` implementation blockers.

This decision would NOT authorize:
- W1 columns/tables/DDL/migrations;
- API routes or DTO properties;
- new permissions/security scope;
- accounting/business formulas or limits not already issued;
- lookup provider identifiers not already issued;
- application code or official Kurrasa changes.

If approved, TEAM-D03 resumes immediately and the batch continues through UX → VISUAL → INDEPENDENT_REVIEW. If not approved, all three remain `HOLD_AUTHORITY` until field-level W1/W2 evidence is issued.
