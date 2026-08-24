# ACC-071 — الموردون — Canonical Screen Specification

**English:** Vendors  
**Profile / Variant:** `MasterData / Tabbed`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**Batch:** `BATCH-17`

## Authority
Current baseline + current W2 + CoreUI MasterData. Root: `Supplier`; context: Account/Currency/Branch/Attachment/OpenItem. Physical/lookup mappings not explicitly closed remain `TBD-GATED`.

## ANALYSIS / LAYOUT — PASS
Purpose: maintain supplier financial master data without creating transaction/posting authority.

Tabs exactly:
1. البيانات الرئيسية
2. العناوين والاتصال
3. الحسابات والربط المحاسبي
4. شروط الدفع والحدود
5. المرفقات
6. سجل العمليات

Shared MasterData/Tabbed host only.

## FIELD_GRID — PASS
Governing fields (13):
1. رمز المورد — Text — required
2. اسم المورد — Text — required
3. نوع المورد — Enum — required
4. الرقم الضريبي — Text
5. الهاتف — Text
6. البريد الإلكتروني — Text
7. العنوان — Text
8. حساب المورد — Lookup/Reference — provider TBD-GATED
9. العملة الافتراضية — Lookup/Reference — provider TBD-GATED
10. شروط الدفع — Policy/Text/Reference presentation; exact provider/contract remains TBD-GATED
11. حد التعامل — Decimal/Money presentation — server policy authoritative
12. طريقة الدفع الافتراضية — Lookup/Reference — provider TBD-GATED
13. الحالة — Status/Enum — server state

List grid: `AutoGenerateColumns=false`, server-paged, exact 7 columns:
`الرمز | الاسم | الشركة | الفرع/النطاق | العملة | الحالة | آخر تعديل`.

Company/branch/list projection values are server-scope derived. No client balance/payment/credit formula.

## UX / VISUAL — PASS
Actions exactly `View/Create/Edit/Disable`. Disable is explicit permission/state-bound server command. No Delete/Enable/Print/Export/attachment mutation/offline final write is invented. Attachments tab is structural until W2 binding exists. Shared CoreUI only.

## TEAM-D06 — PASS
Independent review: `PASS / 0 open design findings`.
Evidence: `documentation/design/batches/BATCH-17_INDEPENDENT_REVIEW_2026-08-24.md`.

## Remaining technical gates
Exact W1/DTO/property/lookup/provider/attachment/sort bindings and runtime/acceptance/release evidence remain separate `TBD-GATED` implementation items.
