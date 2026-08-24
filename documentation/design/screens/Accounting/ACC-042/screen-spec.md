# ACC-042 — القيد اليومي — Canonical Screen Specification

**English:** General Journal Entry  
**Module:** Accounting  
**Profile / Variant:** `Transaction / HeaderLines`  
**Toolbar:** shared Transaction family / capability-bound  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**OwnerTeam:** `TEAM-D06`  
**Batch:** `BATCH-12`

## Authority
- Current 57-screen baseline + Unified Design/Execution V1.3.
- W1 aggregate: `JournalEntry` with JournalLine/Account/CostCenter/Currency/FiscalPeriod/NumberReservation/ApprovalRequest context.
- W2 exact commands: `ACC042.View/Create/Edit/Cancel/Post/Reverse`; seven endpoints only: List/Get/Create/Update/Cancel/Post/Reverse.
- Owner design-only decision recorded in `documentation/design/batches/BATCH-12_ACCOUNTING_CORE_TRANSACTIONS_HOLD_2026-08-24.md`.
- Specialist field gaps remain technical `TBD-GATED`; no W1/API/DTO/lookup mapping is invented.

## ANALYSIS — TEAM-D01 PASS
Purpose: create and maintain a journal draft, then cancel/post/reverse through explicit server-authoritative lifecycle commands.

Functional tabs:
1. البيانات الرئيسية
2. التفاصيل والحركات
3. المرفقات والربط بالمستندات
4. الاعتمادات
5. سجل العمليات

Capabilities are exactly View/Create/Edit/Cancel/Post/Reverse. There is no screen-specific Print/Export, attachment mutation, Approve/Reject/Return, Delete, or offline-write authority.

Lifecycle presentation follows current contract: Draft → optional approval state → Posted → Reversed/Cancelled as permitted. Posted records are immutable; UI command state is advisory and the server rechecks permission, scope, version, period and target state.

## LAYOUT — TEAM-D02 PASS
CoreUI Transaction layout only:

`TransportScreenHost → Toolbar → Header/MainData(Content) → Tabs/Workspace(Fill) → Lines/Grid(Fill) → Totals/Actions(Content/Fixed) → Audit`.

No local dimensions, fonts, colors, RTL overrides, toolbar clone, grid styling clone, pagination clone, validation presenter or audit footer is introduced.

## FIELD_GRID — TEAM-D03 PASS
Owner decision authorizes UI metadata only. Exact field-to-W1/DTO/API mappings and unissued lookup providers remain `TBD-GATED`.

### Field contract
| Order | W3 design key | Arabic label | UI type | Requiredness | Edit policy |
|---:|---|---|---|---|---|
| 1 | `documentNumber` | رقم المستند | Display / Text | server generated | ReadOnly |
| 2 | `accountingDate` | التاريخ المحاسبي | Input / Date | required | Draft-editable when `ACC042.Edit`/Create context allows |
| 3 | `externalReference` | المرجع | Input / Text | optional | Draft-editable |
| 4 | `description` | الوصف | Input / Text | required | Draft-editable |
| 5 | `currencyRef` | العملة | Lookup / Reference | required | Draft-editable; provider binding TBD-GATED |
| 6 | `exchangeRate` | سعر الصرف | Input / Decimal | required | Draft-editable; no client accounting authority |
| 7 | `accountRef` | الحساب | Lookup / Reference | required per line | Draft line editor; provider binding TBD-GATED |
| 8 | `costCenterRef` | مركز التكلفة | Lookup / Reference | conditional per account/dimension rules | Draft line editor; provider binding TBD-GATED |
| 9 | `debit` | مدين | Input / Decimal | conditional line amount | Draft line editor |
| 10 | `credit` | دائن | Input / Decimal | conditional line amount | Draft line editor |
| 11 | `state` | الحالة | State / Enum display | server state | ReadOnly |

Rules:
- Accounting-date open-period validation is server-authoritative.
- Reference/description persistence mapping remains TBD-GATED; their V1.3 UI existence is design-governing.
- Exchange-rate selection/validation and accounting amount calculation remain server/domain-authoritative; no client formula is introduced.
- Debit/credit balancing, postability and accounting rules are not implemented in screen metadata.

### TransactionLines grid
- `GridProfile = TransactionLines`.
- `AutoGenerateColumns = false`.
- Primary workspace = `Fill`.
- Draft-only editing; Posted/Cancelled/Reversed presentation is read-only.
- Shared CoreUI keyboard/focus/error/loading behavior only.

| Order | W3 design key | Arabic column | UI semantic | Edit | Width policy |
|---:|---|---|---|---|---|
| 1 | `rowNo` | # | RowNumber | read-only | compact |
| 2 | `accountRef` | الحساب | Lookup/Reference | Draft | content |
| 3 | `costCenterRef` | مركز التكلفة/الأبعاد | Lookup/Reference | Draft / conditional | content |
| 4 | `lineDescription` | البيان | Text | Draft | primary fill |
| 5 | `debit` | مدين | MonetaryAmount | Draft | compact numeric |
| 6 | `credit` | دائن | MonetaryAmount | Draft | compact numeric |
| 7 | `currencyRef` | العملة | Reference | Draft | compact reference |
| 8 | `exchangeRate` | سعر الصرف | Decimal/Rate | Draft | compact numeric |
| 9 | `accountingAmount` | المبلغ المحاسبي | Derived MonetaryAmount | read-only | compact numeric |

Exact lookup providers, line DTO property names, server sort keys and accountingAmount formula/binding remain `TBD-GATED` implementation items.

## UX — TEAM-D04 PASS
- Create opens an editable Draft under `ACC042.Create`; update requires Draft/eligible state plus expected version under `ACC042.Edit`.
- Post is shown/enabled only when capability/permission/state are advisory-valid; server authoritatively checks period, balance, accounts, numbering, approval state, scope and expected version.
- Cancel is an explicit eligible-state command requiring current version/reason as W2 dictates; no silent state rewrite.
- Reverse is available only for posted eligible records under `ACC042.Reverse`; original remains immutable and reversal is server-generated/linked.
- Concurrency conflict uses shared conflict UX with Refresh/Reload; no silent overwrite or client merge.
- The Approvals tab is read-only status/history/context. No direct Approve/Reject/Return command is created; approval decision authority remains outside this screen.
- The Attachments tab remains a governed functional area, but no Upload/Download/Delete command is created without W2 binding.
- No Print/Export surface is created.
- No offline final write, queue, outbox or replay is created.
- Validation/loading/error/empty/audit/focus/keyboard behavior is shared CoreUI only.

## VISUAL — TEAM-D05 PASS
- Header fields use central Transaction MainData spacing and maximum-two-column rules where applicable.
- Tabs use central RTL/right-origin behavior; Workspace and line grid own remaining space.
- Required, read-only, disabled, error and focus states use CoreUI semantic presentation only.
- Debit/credit/rate/accounting amount use shared numeric alignment/formatting; no local colors or accounting-sign formulas.
- Posted/Cancelled/Reversed read-only state is semantic, not a local styling override.
- Toolbar actions follow shared order/style and capability visibility; UI visibility is never authorization authority.
- DPI/resize/Arabic clipping and accessible focus metadata remain CoreUI-owned.

## Remaining technical gates — nonblocking for design approval
- FBR-035..037 exact field-level persistence/read/API/audit/offline mappings.
- Lookup provider/source/revalidation bindings for account/cost center and any other unissued lookup detail.
- Exact DTO property names, line binding and server sort keys.
- Attachment APIs/permissions if later issued.
- Runtime/acceptance execution.

## TEAM-D06 review input
Verify:
1. Identity/Profile/Variant = ACC-042 / Transaction / HeaderLines.
2. Exact five functional tabs and exact 11-field inventory preserved.
3. Exact 9-column line grid preserved.
4. Executable actions remain exactly View/Create/Edit/Cancel/Post/Reverse.
5. No Print/Export/attachment mutation/direct approval command invented.
6. Posted immutability, expected-version conflict behavior and server posting authority preserved.
7. Owner design-only decision has not been converted into W1/DTO/API/DDL authority.
8. Lookup and field-mapping gaps remain explicit TBD-GATED implementation items.
9. No local CoreUI duplication or client accounting formula.
