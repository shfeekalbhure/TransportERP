# ACC-038 — الصناديق — Canonical Screen Specification

**English:** Cash Boxes  
**Module:** Accounting  
**Profile / Variant:** `MasterData / Tabbed`  
**CurrentDesignState:** `FIELD_GRID_HOLD_AUTHORITY`  
**OwnerTeam:** `DESIGN-LEAD / TEAM-D03`  
**Batch:** `BATCH-11`

## Authority
- Current 57-screen governing baseline + Unified Design/Execution V1.3.
- W2 exact surface: List/Get/Create/Update/Disable; `ACC038.View/Create/Edit/Disable`.
- Primary W1 entity: `Cashbox`.
- Current specialist field review remains authoritative for unresolved field-level mappings; no W1/API/DTO mapping is invented.

## ANALYSIS — TEAM-D01 PASS
Purpose: maintain company/branch-scoped cash boxes as financial master data used by receipt/payment/transfer flows, without adding posting actions to this master screen.

Current functional tabs:
1. البيانات الرئيسية
2. بيانات إضافية
3. الربط المحاسبي
4. الحدود والصلاحيات
5. المرفقات
6. سجل العمليات

Current design field inventory (11):
- رمز الصندوق
- اسم الصندوق
- الشركة
- الفرع
- العملة
- حساب الأستاذ
- أمين الصندوق الافتراضي
- الحد الأقصى للرصيد
- حد العملية الواحدة
- يتطلب إقفال وردية
- الحالة

Capabilities are exactly View/Create/Edit/Disable. There is no current Print/Export/Post/Reverse/Delete/Enable authority.

## LAYOUT — TEAM-D02 PASS
Use shared `MasterData / Tabbed` CoreUI only. Functional tabs are the current V1.3 set above. Shared MasterData sizing, Search/List, pagination, audit, RTL, typography, spacing and validation/error/loading states remain CoreUI-owned; no local pixel dimensions or visual duplication.

## FIELD_GRID — TEAM-D03 HOLD_AUTHORITY
Current list/grid inventory has seven display columns:
1. الرمز
2. الاسم
3. الشركة
4. الفرع/النطاق
5. العملة
6. الحالة
7. آخر تعديل

However the current specialist review still marks multiple field-level bindings as blocking, including رمز الصندوق, اسم الصندوق, الحد الأقصى للرصيد, حد العملية الواحدة and يتطلب إقفال وردية. Exact persisted/read/API/permission/audit/offline mappings are not sufficiently evidenced.

### Non-invention rule
- Do not create W1 columns/DDL.
- Do not create DTO properties/routes/permissions.
- Do not infer financial formulas or balance-limit enforcement beyond current issued semantics.
- Unissued lookup-provider identifiers remain `TBD-GATED`.

### Required owner decision
See `documentation/design/batches/BATCH-11_ACCOUNTING_FINANCIAL_MASTERS_HOLD_2026-08-24.md`.

If approved, TEAM-D03 may close only UI-design metadata while all unresolved technical mappings remain explicit implementation gates.
