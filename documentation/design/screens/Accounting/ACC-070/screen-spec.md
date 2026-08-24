# ACC-070 — العملاء — Canonical Screen Specification

**English:** Customers  
**Profile / Variant:** `MasterData / Tabbed`  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**OwnerTeam:** `TEAM-D06`  
**Batch:** `BATCH-17`

## Authority
Current baseline + current W2 + CoreUI MasterData. Root: `Customer`; context: Account/Currency/Branch/Attachment/OpenItem. Physical/lookup mappings not explicitly closed remain `TBD-GATED`.

## ANALYSIS / LAYOUT — PASS
Purpose: maintain customer financial master data without creating transaction/posting authority.

Tabs exactly:
1. البيانات الرئيسية
2. العناوين والاتصال
3. الحسابات والربط المحاسبي
4. الائتمان والحدود
5. المرفقات
6. سجل العمليات

Shared MasterData/Tabbed host only. No local toolbar/grid/RTL/DPI/validation/audit architecture.

## FIELD_GRID — PASS
Governing fields (13):
1. رمز العميل — Text — required — create/edit eligible state
2. اسم العميل — Text — required
3. نوع العميل — Enum — required
4. الرقم الضريبي — Text — policy/requiredness per current contract
5. الهاتف — Text
6. البريد الإلكتروني — Text
7. العنوان — Text
8. حساب العميل — Lookup/Reference — provider TBD-GATED
9. العملة الافتراضية — Lookup/Reference — provider TBD-GATED
10. حد الائتمان — Decimal/Money presentation — server policy authoritative
11. أيام الائتمان — Integer
12. طريقة الدفع الافتراضية — Lookup/Reference — provider TBD-GATED
13. الحالة — Status/Enum — server state

List grid: `AutoGenerateColumns=false`, server-paged, exact 7 columns:
`الرمز | الاسم | الشركة | الفرع/النطاق | العملة | الحالة | آخر تعديل`.

Company/branch/list projection values are server-scope derived. No client credit-availability formula.

## UX / VISUAL — PASS
Actions exactly `View/Create/Edit/Disable`. Disable is explicit permission/state-bound server command. No Delete/Enable/Print/Export/attachment mutation/offline final write is invented. Attachments tab is structural until W2 binding exists. Shared CoreUI lookup/loading/error/conflict/audit behavior only.

## TEAM-D06
Pending independent review. Confirm 13 fields, 6 tabs, exact 7-column list, View/Create/Edit/Disable only, and no client credit/accounting authority.
