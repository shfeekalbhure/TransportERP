# ACC-053 — الأرصدة الافتتاحية للحسابات ومراكز التكلفة — Canonical Screen Specification

**English:** Opening Balances  
**Module:** Accounting  
**Profile / Variant:** `Transaction / Opening`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**Batch:** `BATCH-14`

## Authority
- Current57 closed execution register + current baseline V1.1 + current V1.3 field/tab/grid crosswalk.
- W2: `ACC053.View/Create/Edit/Cancel/Post`; six endpoints: List/Get/Create/Update/Cancel/Post.
- W1 root: `OpeningBalanceBatch`; secondary `OpeningBalanceLine/Account/CostCenter/Currency/JournalEntry`.
- Batch authority: `documentation/design/batches/BATCH-14_ACCOUNTING_LIFECYCLE_CONTROLS_AUTHORITY_2026-08-24.md`.
- Physical/DTO/lookup mappings not explicitly closed remain `TBD-GATED`; no implementation authority is invented.

## ANALYSIS — TEAM-D01 PASS
Purpose: maintain an opening-balance batch and post it through the issued server lifecycle.

Functional tabs:
1. الأرصدة الافتتاحية
2. تفاصيل الحسابات والأبعاد
3. التحقق والموازنة
4. الاعتماد والترحيل
5. سجل العمليات

Capabilities are exactly View/Create/Edit/Cancel/Post. There is no Reverse, Print, Export, direct approval-decision command, attachment mutation, Delete or offline final-write authority.

## LAYOUT — TEAM-D02 PASS
Shared Transaction host only:
`Toolbar → Header/MainData(Content) → Tabs/Workspace(Fill) → Detail/Grid(Fill) → Validation/Actions(Content/Fixed) → Audit`.

No local toolbar/grid/RTL/DPI/validation/audit architecture.

## FIELD_GRID — TEAM-D03 PASS
### Governing field contract
| # | UI design key | Arabic label | Type | Requiredness | Edit policy |
|---:|---|---|---|---|---|
| 1 | `openingBatch` | دفعة الأرصدة الافتتاحية | Text/Code | Required | Draft-editable |
| 2 | `fiscalYearRef` | السنة المالية | Lookup/Reference | Required | Draft-editable; provider TBD-GATED |
| 3 | `accountRef` | الحساب | Lookup/Grid | Required per line | Draft line editor; provider TBD-GATED |
| 4 | `costCenterDimensions` | مركز التكلفة/الأبعاد | Lookup/Grid | Conditional per account | Draft line editor; provider TBD-GATED |
| 5 | `currencyRef` | العملة | Lookup/Grid | Required per line | Draft line editor |
| 6 | `debitOpening` | رصيد مدين | Decimal/Grid | Conditional | Draft line editor; >=0 server/domain rule |
| 7 | `creditOpening` | رصيد دائن | Decimal/Grid | Conditional | Draft line editor; >=0 server/domain rule |
| 8 | `exchangeRate` | سعر الصرف | Decimal/Grid | Conditional by currency | Draft line editor; >0 server/domain rule |
| 9 | `accountingAmount` | المبلغ المحاسبي | Derived amount | Automatic | ReadOnly / server-authoritative |
| 10 | `batchState` | حالة الدفعة | Workflow state | Automatic | ReadOnly |

The screen does not promote candidate-only extra fields. Exact physical/DTO/provider bindings remain implementation-owned.

### OpeningLinesGrid
`AutoGenerateColumns=false`; primary workspace Fill. Exactly **9 display columns**:
1. `#` — ordinal / read-only
2. الحساب
3. مركز التكلفة/الأبعاد
4. البيان
5. مدين
6. دائن
7. العملة
8. سعر الصرف
9. المبلغ المحاسبي

`المبلغ المحاسبي` is read-only/server-derived. The grid-only `البيان` exists by current W3 grid authority; exact persistence/requiredness remains `TBD-GATED`.

No client accounting formula, balancing engine or early exchange-rate rounding is authorized.

## UX — TEAM-D04 PASS
- Create/Edit operate only in eligible draft state under server permission/scope/version checks.
- Post is explicit and server-authoritative; validation of fiscal year, postable accounts, dimensions, balancing, currency/rate and posting state remains server/domain authority.
- Cancel is an explicit W2 lifecycle command; no silent state rewrite.
- The approval/posting tab is structurally governing but does not create an Approve/Reject/Return command not issued by ACC-053 W2.
- Attachments are not given mutation actions because W2 does not issue them.
- Concurrency/error/loading/focus/validation use shared CoreUI; no offline final write/outbox/replay.

## VISUAL — TEAM-D05 PASS
Use shared Transaction spacing, RTL/DPI, tabs, grid, numeric/reference presentation and lifecycle/read-only states only. Monetary/rate/accounting-amount formatting is centrally governed; no local formulas or colors.

## TEAM-D06 — INDEPENDENT REVIEW PASS
`PASS — 0 open design findings` per `documentation/design/batches/BATCH-14_INDEPENDENT_REVIEW_2026-08-24.md`.

Confirmed: exact 10 fields, five tabs, nine display columns, exact `View/Create/Edit/Cancel/Post` surface, server-authoritative financial/posting semantics, and no W1/API/DTO/provider invention.

## Remaining technical gates
Exact W1 physical mapping for unresolved fields, lookup providers, DTO/line properties, sort keys, runtime acceptance and release evidence remain separate and nonblocking for design approval.
