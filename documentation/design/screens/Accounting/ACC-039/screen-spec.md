# ACC-039 — الحسابات البنكية — Canonical Screen Specification

**English:** Bank Accounts  
**Module:** Accounting  
**Profile / Variant:** `MasterData / Tabbed`  
**CurrentDesignState:** `FIELD_GRID_HOLD_AUTHORITY`  
**OwnerTeam:** `DESIGN-LEAD / TEAM-D03`  
**Batch:** `BATCH-11`

## Authority
- Current 57-screen governing baseline + Unified Design/Execution V1.3.
- W2 exact surface: List/Get/Create/Update/Disable; `ACC039.View/Create/Edit/Disable`.
- Primary W1 entity: `BankAccount`.
- Current specialist field review remains authoritative for unresolved field-level mappings; no W1/API/DTO mapping is invented.

## ANALYSIS — TEAM-D01 PASS
Purpose: maintain bank-account master data and accounting/reconciliation references used by financial transaction and reconciliation flows.

Current functional tabs:
1. البيانات الرئيسية
2. بيانات إضافية
3. الربط المحاسبي
4. الحدود والصلاحيات
5. المرفقات
6. سجل العمليات

Current design field inventory (14):
- رمز الحساب البنكي
- اسم البنك
- اسم الحساب
- رقم الحساب
- IBAN
- SWIFT/BIC
- الشركة
- الفرع
- العملة
- حساب الأستاذ
- حساب رسوم البنك
- حساب فروق التسوية
- حد السحب/التحويل
- الحالة

Capabilities are exactly View/Create/Edit/Disable. There is no current Print/Export/Post/Reverse/Delete/Enable authority.

## LAYOUT — TEAM-D02 PASS
Use shared `MasterData / Tabbed` CoreUI only. Functional tabs are the current V1.3 set above. Shared MasterData layout, search/list behavior, pagination, audit, RTL and visual states remain centrally owned.

## FIELD_GRID — TEAM-D03 HOLD_AUTHORITY
Current list/grid inventory has seven display columns:
1. الرمز
2. الاسم
3. الشركة
4. الفرع/النطاق
5. العملة
6. الحالة
7. آخر تعديل

The specialist review still marks important bindings as blocking. Current W1 evidence does not fully establish separate persisted/read/API semantics for Bank Account Code, Bank Name, Account Name, Account Number vs IBAN, SWIFT/BIC, Bank Fees Account, Reconciliation Difference Account and Withdrawal/Transfer Limit. The current W1 `IBAN/BankReference` evidence is insufficient to silently split Account Number and IBAN.

### Non-invention rule
- Do not merge or split Account Number/IBAN storage semantics by assumption.
- Do not create W1 columns/DDL or extra GL-account links.
- Do not create DTO properties/routes/permissions.
- Unissued lookup providers and revalidation rules remain `TBD-GATED`.

### Required owner decision
See `documentation/design/batches/BATCH-11_ACCOUNTING_FINANCIAL_MASTERS_HOLD_2026-08-24.md`.

If approved, TEAM-D03 may close only UI-design metadata while unresolved persistence/API mapping remains an implementation blocker.
