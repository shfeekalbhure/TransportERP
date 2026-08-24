# ACC-038 — الصناديق — Canonical Screen Specification

**English:** Cash Boxes  
**Module:** Accounting  
**Profile / Variant:** `MasterData / Tabbed`  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**OwnerTeam:** `TEAM-D06`  
**Batch:** `BATCH-11`

## Authority
- Current 57-screen governing baseline + Unified Design/Execution V1.3.
- W2 exact surface: List/Get/Create/Update/Disable; `ACC038.View/Create/Edit/Disable`.
- Primary W1 entity: `Cashbox`.
- Owner design-only decision: `BATCH-11_ACCOUNTING_FINANCIAL_MASTERS_HOLD_2026-08-24.md` — approved 2026-08-24.
- Specialist field review supplies UI type/requiredness evidence but unresolved physical/API/lookup mappings remain `TBD-GATED`; no W1/API/DTO mapping is invented.

## ANALYSIS — TEAM-D01 PASS
Purpose: maintain company/branch-scoped cash boxes as financial master data used by receipt/payment/transfer flows, without adding posting actions to this master screen.

Capabilities are exactly:
- View → `ACC038.View`
- Create → `ACC038.Create`
- Edit → `ACC038.Edit`
- Disable → `ACC038.Disable`

No Print/Export/Post/Reverse/Delete/Enable authority is issued.

## LAYOUT — TEAM-D02 PASS
Use shared `MasterData / Tabbed` CoreUI only.

Functional tabs:
1. البيانات الرئيسية
2. بيانات إضافية
3. الربط المحاسبي
4. الحدود والصلاحيات
5. المرفقات
6. سجل العمليات

CoreUI owns toolbar, RTL, typography, spacing, validation/error/loading states, Search/List/Grid, pagination and audit. No local pixel sizing, toolbar/grid clone or nested scrolling is introduced.

## FIELD_GRID — TEAM-D03 PASS
Owner design-only authority applies. The field keys below are **W3 design aliases** and do not claim DTO/DB property names.

| Order | W3 design key | Arabic label | UI semantic / type | Required | UI mode | Technical binding |
|---:|---|---|---|---:|---|---|
| 1 | `cashboxCode` | رمز الصندوق | Input / Text-Code | yes | Create/Edit when capability/state allows | W1/W2 exact field mapping `TBD-GATED` |
| 2 | `cashboxName` | اسم الصندوق | Input / Text | yes | Create/Edit when allowed | W1/W2 exact field mapping `TBD-GATED` |
| 3 | `companyRef` | الشركة | Lookup / Reference | yes | Create/Edit when allowed | known Cashbox company concept; provider details remain implementation authority |
| 4 | `branchRef` | الفرع | Lookup / Reference | yes | Create/Edit when allowed | known Cashbox branch concept; server scope authoritative |
| 5 | `currencyRef` | العملة | Lookup / Reference | yes | Create/Edit when allowed | known Cashbox currency concept; server validation authoritative |
| 6 | `glAccountRef` | حساب الأستاذ | Lookup / Reference | yes | Create/Edit when allowed | lookup provider/revalidation `TBD-GATED` |
| 7 | `defaultCashierRef` | أمين الصندوق الافتراضي | Lookup / Reference | no | Create/Edit when allowed | lookup provider/revalidation `TBD-GATED` |
| 8 | `maximumBalance` | الحد الأقصى للرصيد | Input / MonetaryAmount | no | Create/Edit when allowed | persistence/enforcement `TBD-GATED`; UI accepts non-negative design value only |
| 9 | `singleTransactionLimit` | حد العملية الواحدة | Input / MonetaryAmount | no | Create/Edit when allowed | persistence/enforcement `TBD-GATED`; UI accepts non-negative design value only |
| 10 | `requiresShiftClose` | يتطلب إقفال وردية | Input / Boolean | yes | Create/Edit when allowed | persistence/policy binding `TBD-GATED` |
| 11 | `status` | الحالة | Status / Enum | response | read-only projection | state changes only through issued Disable command; no direct status editor |

### List/Grid
Current governing list contract is preserved exactly as seven display columns:
1. الرمز
2. الاسم
3. الشركة
4. الفرع/النطاق
5. العملة
6. الحالة
7. آخر تعديل

Grid contract:
- `GridProfile = Display`
- `AutoGenerateColumns = false`
- `Selection = SingleRow`
- server paging through shared MasterData paging contract
- Search contract: code/name/status
- exact server sort-key mapping remains implementation `TBD-GATED`

Width policy is semantic only: Code/content; Name=primary Fill; Company/Branch/Currency=content; Status/LastModified=compact.

## UX — TEAM-D04 PASS
- New/Edit/Disable are capability-driven; server permission/scope is authoritative.
- Disable is the only issued state-change command; no Enable or direct Status mutation is shown.
- Shared TransportLookup presentation is used for Company/Branch/Currency/GL Account/Default Cashier; unissued providers do not become local hardcoded lists.
- Shared validation presenter handles field errors; no MessageBox duplicate validation path.
- Concurrency conflicts use shared reload/refresh handling where returned by W2; no silent overwrite.
- Loading disables conflicting commands and prevents duplicate submission through shared command state.
- No offline write/queue/outbox/replay is introduced.

## VISUAL — TEAM-D05 PASS
- CoreUI owns all RTL, DPI, typography, spacing, semantic states, toolbar, grid, pagination, audit, validation, loading and error presentation.
- Required fields use shared required semantics, not error styling.
- Monetary fields use shared numeric formatting/alignment; no financial computation is performed by Desktop.
- Tabs are functional only; no extra decorative tab is introduced.

## Technical gates retained
The design approval does not resolve Cashbox code/name persistence, GL/default-cashier lookup providers, limit persistence/enforcement, shift-close storage, DTO field mappings, field-level audit mapping or offline classification. These remain explicit implementation `TBD-GATED` items.

## TEAM-D06 review input
Verify:
1. Identity/Profile/Variant = `ACC-038 / MasterData / Tabbed`.
2. Six governing functional tabs retained.
3. Eleven governing design fields explicit with UI metadata only.
4. Seven current list columns preserved exactly.
5. Actions exactly View/Create/Edit/Disable.
6. Status is not directly edited and no Enable/Delete/Print/Export/Post/Reverse is invented.
7. Lookup providers and physical/DTO bindings remain `TBD-GATED` rather than invented.
8. CoreUI owns shared layout/RTL/visual behavior.
9. No offline write authority or accounting formula is created.
