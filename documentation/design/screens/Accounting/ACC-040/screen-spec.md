# ACC-040 — طرق الدفع — Canonical Screen Specification

**English:** Payment Methods  
**Module:** Accounting  
**Profile / Variant:** `MasterData / Standard`  
**CurrentDesignState:** `FIELD_GRID_HOLD_AUTHORITY`  
**OwnerTeam:** `DESIGN-LEAD / TEAM-D03`  
**Batch:** `BATCH-11`

## Authority
- Current 57-screen governing baseline + Unified Design/Execution V1.3.
- W2 exact surface: List/Get/Create/Update/Disable; `ACC040.View/Create/Edit/Disable`.
- Primary W1 entity: `PaymentMethod`.
- Current specialist field review remains authoritative for unresolved field-level mappings; no W1/API/DTO mapping is invented.

## ANALYSIS — TEAM-D01 PASS
Purpose: maintain payment-method reference data and settlement/accounting behavior flags consumed by receipt/payment flows, without turning this master screen into a transaction workflow.

Current functional tabs:
1. البيانات الرئيسية
2. الربط المحاسبي
3. قواعد الاستخدام
4. سجل العمليات

Current design field inventory (9):
- رمز الطريقة
- اسم طريقة الدفع
- نوع الوسيلة
- يتطلب صندوقًا
- يتطلب حسابًا بنكيًا
- حساب المقاصة
- يسمح بالقبض
- يسمح بالصرف
- الحالة

Capabilities are exactly View/Create/Edit/Disable. There is no current Print/Export/Pay/Deposit/Post/Reverse/Delete/Enable authority.

## LAYOUT — TEAM-D02 PASS
Use shared `MasterData / Standard` CoreUI only: MainData/Search are Content-sized, primary list workspace owns Fill, pagination/audit/RTL/visual states remain centrally owned. The four functional areas above remain screen content; no local toolbar/grid/pagination/audit implementation is introduced.

## FIELD_GRID — TEAM-D03 HOLD_AUTHORITY
The current baseline does not issue a concrete primary grid-column contract (`Columns=0`). TEAM-D03 must not invent list columns without design authority.

The specialist review also classifies several field-level bindings as blocking, including Method Code, Payment Method Name, Method Type, Requires Cashbox, Requires Bank Account, Clearing Account, Allows Receipt and Allows Payment. Exact persistence/API/permission/audit/offline mappings are not fully evidenced.

### Non-invention rule
- Do not invent the PaymentMethod physical schema.
- Do not infer conditional business formulas from the boolean flags beyond current screen semantics.
- Do not create Clearing Account storage, lookup route, DTO property or permission by assumption.
- Do not create Print/Export or transaction actions.

### Required owner decision
See `documentation/design/batches/BATCH-11_ACCOUNTING_FINANCIAL_MASTERS_HOLD_2026-08-24.md`.

If approved, TEAM-D03 may define UI-only design metadata and a concrete list-column presentation from the current field inventory, while all unresolved W1/W2 bindings stay explicit implementation blockers.
