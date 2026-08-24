# ACC-040 — طرق الدفع — Canonical Screen Specification

**English:** Payment Methods  
**Module:** Accounting  
**Profile / Variant:** `MasterData / Standard`  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**OwnerTeam:** `TEAM-D06`  
**Batch:** `BATCH-11`

## Authority
- Current 57-screen governing baseline + Unified Design/Execution V1.3.
- W2 exact surface: List/Get/Create/Update/Disable; `ACC040.View/Create/Edit/Disable`.
- Primary W1 entity: `PaymentMethod`.
- Owner design-only decision: `BATCH-11_ACCOUNTING_FINANCIAL_MASTERS_HOLD_2026-08-24.md` — approved 2026-08-24.
- Specialist field review supplies UI type/requiredness evidence; unresolved physical/API/lookup mappings remain `TBD-GATED`.

## ANALYSIS — TEAM-D01 PASS
Purpose: maintain payment-method reference data and settlement/accounting behavior flags consumed by receipt/payment flows, without turning this master screen into a transaction workflow.

Capabilities are exactly:
- View → `ACC040.View`
- Create → `ACC040.Create`
- Edit → `ACC040.Edit`
- Disable → `ACC040.Disable`

No Print/Export/Pay/Deposit/Post/Reverse/Delete/Enable authority is issued.

## LAYOUT — TEAM-D02 PASS
Use shared `MasterData / Standard` CoreUI only.

Functional areas:
1. البيانات الرئيسية
2. الربط المحاسبي
3. قواعد الاستخدام
4. سجل العمليات

MainData/Search are Content-sized; primary list workspace owns Fill; pagination/audit/RTL/validation/loading/error visuals remain CoreUI-owned. No local toolbar/grid/pagination/audit implementation is introduced.

## FIELD_GRID — TEAM-D03 PASS
Owner design-only authority applies. The field keys below are W3 design aliases only and do not assert DTO/DB names.

| Order | W3 design key | Arabic label | UI semantic / type | Required | UI mode | Technical binding |
|---:|---|---|---|---:|---|---|
| 1 | `methodCode` | رمز الطريقة | Input / Text-Code | yes | Create/Edit when allowed | physical/API mapping `TBD-GATED` |
| 2 | `methodName` | اسم طريقة الدفع | Input / Text | yes | Create/Edit when allowed | physical/API mapping `TBD-GATED` |
| 3 | `methodType` | نوع الوسيلة | Input / Enum | yes | Create/Edit when allowed | enum persistence/API mapping `TBD-GATED` |
| 4 | `requiresCashbox` | يتطلب صندوقًا | Input / Boolean | yes | Create/Edit when allowed | persistence/business enforcement `TBD-GATED` |
| 5 | `requiresBankAccount` | يتطلب حسابًا بنكيًا | Input / Boolean | yes | Create/Edit when allowed | persistence/business enforcement `TBD-GATED` |
| 6 | `clearingAccountRef` | حساب المقاصة | Lookup / Reference | no | Create/Edit when allowed | lookup provider/storage/revalidation `TBD-GATED` |
| 7 | `allowsReceipt` | يسمح بالقبض | Input / Boolean | yes | Create/Edit when allowed | persistence/business enforcement `TBD-GATED` |
| 8 | `allowsPayment` | يسمح بالصرف | Input / Boolean | yes | Create/Edit when allowed | persistence/business enforcement `TBD-GATED` |
| 9 | `status` | الحالة | Status / Enum | response | read-only projection | state changes only through issued Disable command |

No client-side business formula is inferred from the boolean flags. They are displayed/edited as issued settings only; server behavior remains authoritative once exact implementation binding exists.

### List/Grid — owner-issued UI-only closure
The previous baseline had `Columns=0`. Under the approved Batch-11 design-only decision, TEAM-D03 issues this bounded eight-column list projection directly from the current field inventory:
1. الرمز
2. اسم طريقة الدفع
3. نوع الوسيلة
4. يتطلب صندوقًا
5. يتطلب حسابًا بنكيًا
6. يسمح بالقبض
7. يسمح بالصرف
8. الحالة

`حساب المقاصة` is intentionally not duplicated into the primary list because its provider/storage is unresolved; it remains available in the edit/detail surface only.

Grid contract:
- `GridProfile = Display`
- `AutoGenerateColumns = false`
- `Selection = SingleRow`
- server paging through shared MasterData contract
- Search contract: code/name/status
- exact server sort-key mapping remains implementation `TBD-GATED`

Width policy: Code/content; Name=primary Fill; Type/content; Boolean columns compact; Status compact.

## UX — TEAM-D04 PASS
- New/Edit/Disable are capability-driven and server-authorized.
- Disable is the only issued state-change command; no Enable or direct Status editor.
- `requiresCashbox`, `requiresBankAccount`, `allowsReceipt`, `allowsPayment` do not cause locally invented workflow/actions. They are configuration fields only.
- Clearing Account uses shared TransportLookup presentation; provider/endpoint/revalidation remain `TBD-GATED`.
- Shared validation/loading/error/concurrency behavior applies; no silent overwrite or local validation clone.
- No Pay/Deposit/Post/Reverse workflow is exposed on this MasterData screen.
- No offline write/queue/outbox/replay is introduced.

## VISUAL — TEAM-D05 PASS
- CoreUI owns RTL, DPI, typography, spacing, semantic states, toolbar, grid, pagination, audit, validation/loading/error presentation.
- Boolean fields use shared checkbox/toggle semantics without introducing business-state colors.
- Required fields use shared required semantics; Status uses shared read-only status presentation.
- No local visual exception is issued.

## Technical gates retained
Physical/API mapping for Method Code/Name/Type, the boolean flags, Clearing Account lookup/storage, field-level audit mapping and offline classification remain explicit implementation `TBD-GATED` items.

## TEAM-D06 review input
Verify:
1. Identity/Profile/Variant = `ACC-040 / MasterData / Standard`.
2. Four governing functional areas and nine governing fields retained.
3. The new eight-column list is justified only by the owner-approved design-only decision and does not invent persistence.
4. Clearing Account remains detail-only with provider/storage `TBD-GATED`.
5. Actions exactly View/Create/Edit/Disable; no Pay/Deposit/Post/Reverse/Print/Export/Delete/Enable action invented.
6. Boolean flags do not create local business formulas/workflows.
7. CoreUI owns shared visuals/RTL/paging/audit.
8. No offline write authority is introduced.
