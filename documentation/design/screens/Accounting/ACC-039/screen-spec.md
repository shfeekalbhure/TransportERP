# ACC-039 — الحسابات البنكية — Canonical Screen Specification

**English:** Bank Accounts  
**Module:** Accounting  
**Profile / Variant:** `MasterData / Tabbed`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**Batch:** `BATCH-11`  
**ApprovedOn:** `2026-08-24`

## Authority
- Current 57-screen governing baseline + Unified Design/Execution V1.3.
- W2 exact surface: List/Get/Create/Update/Disable; `ACC039.View/Create/Edit/Disable`.
- Primary W1 entity: `BankAccount`.
- Owner design-only decision: `BATCH-11_ACCOUNTING_FINANCIAL_MASTERS_HOLD_2026-08-24.md` — approved 2026-08-24.
- Independent review: `BATCH-11_INDEPENDENT_REVIEW_2026-08-24.md` — TEAM-D06 PASS / 0 open design findings.
- Specialist field review supplies UI type/requiredness evidence; unresolved physical/API/lookup mappings remain `TBD-GATED`.

## ANALYSIS — TEAM-D01 PASS
Purpose: maintain bank-account master data and accounting/reconciliation references used by financial transaction and reconciliation flows.

Capabilities are exactly:
- View → `ACC039.View`
- Create → `ACC039.Create`
- Edit → `ACC039.Edit`
- Disable → `ACC039.Disable`

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

The `المرفقات` tab is retained because it is part of the current V1.3 screen inventory, but current W2 does not issue attachment Upload/Download/Delete commands. No attachment mutation command/provider is invented; that surface remains `TBD-GATED` until explicit authority exists.

CoreUI owns toolbar, RTL, typography, spacing, validation/error/loading states, Search/List/Grid, pagination and audit. No local styling or nested scrolling is introduced.

## FIELD_GRID — TEAM-D03 PASS
Owner design-only authority applies. The field keys below are W3 design aliases only and do not assert DTO/DB names.

| Order | W3 design key | Arabic label | UI semantic / type | Required | UI mode | Technical binding |
|---:|---|---|---|---:|---|---|
| 1 | `bankAccountCode` | رمز الحساب البنكي | Input / Text-Code | yes | Create/Edit when allowed | physical/API mapping `TBD-GATED` |
| 2 | `bankName` | اسم البنك | Input / Text | yes | Create/Edit when allowed | physical/API mapping `TBD-GATED` |
| 3 | `accountName` | اسم الحساب | Input / Text | yes | Create/Edit when allowed | physical/API mapping `TBD-GATED` |
| 4 | `accountNumber` | رقم الحساب | Input / Text | yes | Create/Edit when allowed | distinct UI field by V1.3; storage semantics `TBD-GATED` |
| 5 | `iban` | IBAN | Input / Text-LTR | no | Create/Edit when allowed | distinct UI field by V1.3; storage semantics `TBD-GATED` |
| 6 | `swiftBic` | SWIFT/BIC | Input / Text-LTR | no | Create/Edit when allowed | storage/API mapping `TBD-GATED`; shared validation surface only |
| 7 | `companyRef` | الشركة | Lookup / Reference | yes | Create/Edit when allowed | known BankAccount company concept; scope server-authoritative |
| 8 | `branchRef` | الفرع | Lookup / Reference | no | Create/Edit when allowed | known optional branch concept; scope server-authoritative |
| 9 | `currencyRef` | العملة | Lookup / Reference | yes | Create/Edit when allowed | known BankAccount currency concept |
| 10 | `glAccountRef` | حساب الأستاذ | Lookup / Reference | yes | Create/Edit when allowed | lookup provider/revalidation `TBD-GATED` |
| 11 | `bankFeesAccountRef` | حساب رسوم البنك | Lookup / Reference | no | Create/Edit when allowed | lookup provider/storage `TBD-GATED` |
| 12 | `reconciliationDifferenceAccountRef` | حساب فروق التسوية | Lookup / Reference | no | Create/Edit when allowed | lookup provider/storage `TBD-GATED` |
| 13 | `withdrawalTransferLimit` | حد السحب/التحويل | Input / MonetaryAmount | no | Create/Edit when allowed | persistence/enforcement `TBD-GATED`; UI design value is non-negative |
| 14 | `status` | الحالة | Status / Enum | response | read-only projection | state changes only through issued Disable command |

### Account Number / IBAN boundary
Both remain distinct visible fields because V1.3 issues both. This design does **not** claim separate persisted columns, does not merge them into one storage field, and does not infer how existing `IBAN/BankReference` evidence maps physically. That remains an implementation authority gate.

### List/Grid
Current governing list contract is preserved as seven display columns:
1. الرمز
2. الاسم
3. الشركة
4. الفرع/النطاق
5. العملة
6. الحالة
7. آخر تعديل

For the list projection, `الاسم` is the shared display-name projection of the bank-account record; this does not decide whether Bank Name or Account Name is the physical source.

Grid contract:
- `GridProfile = Display`
- `AutoGenerateColumns = false`
- `Selection = SingleRow`
- server paging through shared MasterData contract
- Search contract: code/name/status
- exact server sort-key mapping remains implementation `TBD-GATED`

Width policy: Code/content; Name=primary Fill; Company/Branch/Currency=content; Status/LastModified=compact.

## UX — TEAM-D04 PASS
- New/Edit/Disable are capability-driven and server-authorized.
- Disable is the only issued state-change command; no Enable or direct Status editor.
- IBAN/SWIFT technical text uses LTR/BiDi isolation within the RTL screen.
- Company/Branch/Currency/GL/Fees/Reconciliation account fields use shared TransportLookup presentation; missing providers remain `TBD-GATED` and are not replaced by local lists.
- The Attachments tab exposes no local upload/download/delete command without explicit W2 attachment authority.
- Shared validation/loading/error/concurrency behavior applies; no silent overwrite or local validation clone.
- No reconciliation action is executed from this master screen; it only maintains references consumed by reconciliation flows.
- No offline write/queue/outbox/replay is introduced.

## VISUAL — TEAM-D05 PASS
- CoreUI owns RTL, DPI, typography, spacing, semantic states, toolbar, grid, pagination, audit, validation/loading/error surfaces.
- Technical bank identifiers use invariant/LTR rendering without changing overall RTL layout.
- Monetary limit uses shared numeric formatting only; Desktop does not enforce or calculate financial policy beyond UI pre-validation.
- No local style exception is issued.

## INDEPENDENT_REVIEW — TEAM-D06 PASS
- Initial review finding F-01: attachment tab could be misread as attachment command authority.
- Corrected by explicitly retaining the tab while prohibiting unissued attachment commands/providers.
- Re-review result: PASS.
- Open design findings: **0**.

## Technical gates retained
Physical/API mappings for Bank Account Code, Bank Name, Account Name, Account Number vs IBAN, SWIFT/BIC, GL/fees/reconciliation account references, withdrawal/transfer limit, attachment provider/actions, field-level audit and offline classification remain `TBD-GATED` implementation items.

## Final design verdict
`DESIGN_APPROVED` — design only. Runtime/implementation readiness is not claimed.
