# ACC-055 — قيود التسوية والإقفال السنوي — Canonical Screen Specification

**English:** Adjustment & Year-End Closing Entries  
**Module:** Accounting  
**Profile / Variant:** `Transaction / Closing`  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**OwnerTeam:** `TEAM-D06`  
**Batch:** `BATCH-14`

## Authority
- Current57 closed execution register + current baseline V1.1 + current V1.3 field/tab/grid crosswalk.
- W2: `ACC055.View/Create/Edit/Cancel/Post/Reverse`; seven endpoints: List/Get/Create/Update/Cancel/Post/Reverse.
- W1 root: `ClosingRun`; secondary `JournalEntry/JournalLine/FiscalPeriod`.
- Unresolved field physical/lookup mappings remain `TBD-GATED`; no W1/API/DTO invention.

## ANALYSIS — TEAM-D01 PASS
Purpose: create and maintain closing/adjustment draft records, then post or reverse through server-authoritative lifecycle commands.

Tabs:
1. البيانات الرئيسية
2. التفاصيل والحركات
3. المرفقات والربط بالمستندات
4. الاعتمادات
5. سجل العمليات

Capabilities exactly View/Create/Edit/Cancel/Post/Reverse. No screen-specific Print/Export, direct approval command, attachment mutation, Delete or offline final-write authority.

## LAYOUT — TEAM-D02 PASS
Shared Transaction host:
`Toolbar → Header/MainData(Content) → Tabs/Workspace(Fill) → Detail/Grid(Fill) → Totals/Actions(Content/Fixed) → Audit`.
No local visual/control architecture.

## FIELD_GRID — TEAM-D03 PASS
### Governing fields
| # | UI design key | Arabic label | Type | Requiredness | Edit policy |
|---:|---|---|---|---|---|
| 1 | `documentNumber` | رقم المستند | Display/Text | Automatic | ReadOnly / server generated |
| 2 | `accountingDate` | التاريخ المحاسبي | Date | Required | Draft-editable; open-period rule server-side |
| 3 | `reference` | المرجع | Text | Optional | Draft-editable |
| 4 | `description` | الوصف | Text | Required | Draft-editable |
| 5 | `currencyRef` | العملة | Lookup | Required | Draft-editable; provider TBD-GATED |
| 6 | `exchangeRate` | سعر الصرف | Decimal | Required | Draft-editable; server/domain validation |
| 7 | `accountRef` | الحساب | Lookup/Grid | Required per line | Draft line editor |
| 8 | `costCenterRef` | مركز التكلفة | Lookup/Grid | Conditional per account | Draft line editor |
| 9 | `debit` | مدين | Decimal/Grid | Conditional | Draft line editor; >=0 domain rule |
| 10 | `credit` | دائن | Decimal/Grid | Conditional | Draft line editor; >=0 domain rule |
| 11 | `state` | الحالة | Workflow state | Automatic | ReadOnly |

### ClosingLinesGrid
`AutoGenerateColumns=false`; primary workspace Fill. Exactly **9 display columns**:
`# | الحساب | مركز التكلفة/الأبعاد | البيان | مدين | دائن | العملة | سعر الصرف | المبلغ المحاسبي`.

The first is ordinal. Account/cost center/debit/credit/currency/rate follow issued draft-line semantics. `المبلغ المحاسبي` is read-only/server-derived. Grid-only `البيان` is W3-governing; exact persistence/requiredness remains `TBD-GATED`.

Balancing, posting eligibility, year-end logic, exchange-rate validity and accounting amount remain server/domain-authoritative; no client formula.

## UX — TEAM-D04 PASS
- Create/Edit only for eligible draft/current-version state.
- Post performs server checks for period, balance, account eligibility, approval state, scope, numbering/idempotency and concurrency.
- Cancel is an explicit lifecycle command; no silent state mutation.
- Reverse applies only through `ACC055.Reverse` to eligible posted results; original remains immutable and reversal linking is server-authoritative.
- Approvals tab is structural/read-only unless a separate approval contract is invoked elsewhere; ACC-055 exposes no direct Approve/Reject/Return command.
- Attachments tab exposes no mutation command without W2.
- Shared errors/conflict/loading/focus/audit only; no offline final write.

## VISUAL — TEAM-D05 PASS
Shared Transaction RTL/DPI/tab/grid/numeric/state presentation only. No local accounting formulas, colors, spacing, toolbar or validation architecture.

## TEAM-D06 — INDEPENDENT REVIEW
Pending final disposition. Must confirm 11 fields, five tabs, 9 display columns, exact View/Create/Edit/Cancel/Post/Reverse surface and server-authoritative closing/posting/reversal semantics.

## Remaining technical gates
Exact field persistence, lookup providers, DTO/line mappings, sort keys and runtime/release evidence remain separate.
