# ACC-042 — القيد اليومي — Canonical Screen Specification

**English:** General Journal Entry  
**Module:** Accounting  
**Profile / Variant:** `Transaction / HeaderLines`  
**CurrentDesignState:** `FIELD_GRID_HOLD_AUTHORITY`  
**OwnerTeam:** `DESIGN-LEAD / TEAM-D03`  
**Batch:** `BATCH-12`

## Authority
- Current 57-screen baseline + Unified Design/Execution V1.3.
- W1 aggregate: `JournalEntry` with JournalLine/Account/CostCenter/Currency/FiscalPeriod/NumberReservation/ApprovalRequest context.
- W2 exact commands: `ACC042.View/Create/Edit/Cancel/Post/Reverse`; seven endpoints only: List/Get/Create/Update/Cancel/Post/Reverse.
- Specialist field review FBR-035..037 remains blocking for exact field-level mappings of accounting date/reference/description.

## ANALYSIS — TEAM-D01 PASS
Purpose: create and maintain a journal draft, then cancel/post/reverse through explicit server-authoritative lifecycle commands.

Current fields (11):
1. رقم المستند
2. التاريخ المحاسبي
3. المرجع
4. الوصف
5. العملة
6. سعر الصرف
7. الحساب
8. مركز التكلفة
9. مدين
10. دائن
11. الحالة

Functional tabs:
- البيانات الرئيسية
- التفاصيل والحركات
- المرفقات والربط بالمستندات
- الاعتمادات
- سجل العمليات

Lifecycle: Draft → optional approval state → Posted → Reversed; Posted records immutable. No direct approval decision, Print/Export, attachment mutation, Delete or offline-write action is issued on this screen.

## LAYOUT — TEAM-D02 PASS
CoreUI Transaction layout only:
`Header/MainData(Content) → Tabs/Workspace(Fill) → Lines/Grid(Fill) → Totals/Actions(Content/Fixed) → Audit`.
No local sizes/styles/RTL/grid/toolbar/paging/audit implementation.

## FIELD_GRID — TEAM-D03 HOLD_AUTHORITY
Current governing line grid is the explicit 9-column transaction-lines presentation:
`# | الحساب | مركز التكلفة/الأبعاد | البيان | مدين | دائن | العملة | سعر الصرف | المبلغ المحاسبي`.

The screen field/grid inventory is design-governing, but exact field-level persisted/read/API/audit/offline mappings for `التاريخ المحاسبي`, `المرجع`, `الوصف` remain BLOCKING in the current specialist review. No W1/DTO mapping is inferred.

See `documentation/design/batches/BATCH-12_ACCOUNTING_CORE_TRANSACTIONS_HOLD_2026-08-24.md` for the single owner design-authority decision.
