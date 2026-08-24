# ACC-043 — سند القبض — Canonical Screen Specification

**English:** Receipt Voucher  
**Module:** Accounting  
**Profile / Variant:** `Transaction / HeaderLines`  
**CurrentDesignState:** `DESIGN_APPROVED`  
**OwnerTeam:** `DESIGN-LEAD / ORCHESTRATOR`  
**Batch:** `BATCH-12`  
**ApprovedOn:** `2026-08-24`

## Authority
- Current 57-screen baseline + Unified Design/Execution V1.3.
- W1 aggregate: `ReceiptVoucher`; related context Cashbox/BankAccount/Customer/Currency/JournalEntry/OpenItem.
- W2 exact actions only: `ACC043.View/Create/Edit/Cancel/Post/Reverse`; List/Get/Create/Update/Cancel/Post/Reverse.
- Owner design-only decision is recorded in `documentation/design/batches/BATCH-12_ACCOUNTING_CORE_TRANSACTIONS_HOLD_2026-08-24.md`.
- Independent review: `documentation/design/batches/BATCH-12_INDEPENDENT_REVIEW_2026-08-24.md`.
- Unresolved field/lookup bindings remain `TBD-GATED`; no W1/API/DTO/lookup mapping is invented.

## ANALYSIS — TEAM-D01 PASS
Purpose: create and maintain a receipt Draft, then cancel/post/reverse through server-authoritative commands.

Tabs: البيانات الرئيسية | التفاصيل والحركات | المرفقات والربط بالمستندات | الاعتمادات | سجل العمليات.

No screen-specific Print/Export, attachment mutation, direct Approve/Reject/Return, Delete or offline-write authority is issued. Posted records are immutable.

## LAYOUT — TEAM-D02 PASS
Shared Transaction layout only: `Header/MainData(Content) → Tabs/Workspace(Fill) → Lines/Grid(Fill) → Totals/Actions(Content/Fixed) → Audit`.

## FIELD_GRID — TEAM-D03 PASS
### Field contract
| # | Design key | Arabic | UI type | Requiredness | Edit policy |
|---:|---|---|---|---|---|
| 1 | `voucherNumber` | رقم السند | Display/Text | server generated | ReadOnly |
| 2 | `voucherDate` | التاريخ | Date | required | Draft-editable |
| 3 | `partyRef` | الطرف | Lookup/Reference | context-dependent | Draft-editable; provider TBD-GATED |
| 4 | `sourceCashBankRef` | الصندوق/البنك المصدر | Lookup/Reference | context-dependent | Draft-editable; provider TBD-GATED |
| 5 | `destinationCashBankRef` | الصندوق/البنك الوجهة | Lookup/Reference | context-dependent | Draft-editable; provider TBD-GATED |
| 6 | `currencyRef` | العملة | Lookup/Reference | required | Draft-editable |
| 7 | `amount` | المبلغ | Decimal/Money | required | Draft-editable |
| 8 | `exchangeRate` | سعر الصرف | Decimal/Rate | required | Draft-editable; no client calculation authority |
| 9 | `counterAccountRef` | الحساب المقابل | Lookup/Reference | required | Draft-editable; provider TBD-GATED |
| 10 | `description` | البيان | Text | required | Draft-editable |
| 11 | `state` | الحالة | State/Enum display | server state | ReadOnly |

Server/domain authority owns date/period validation, source/destination validity, posting rules, exchange-rate semantics and accounting effects. Exact field-to-W1/DTO/API bindings stay TBD-GATED where unissued.

### TransactionLines grid
`GridProfile=TransactionLines`, `AutoGenerateColumns=false`, Draft-only edit, CoreUI shared behavior.

| # | Design key | Arabic column | UI semantic | Edit |
|---:|---|---|---|---|
| 1 | `rowNo` | # | RowNumber | read-only |
| 2 | `partyRef` | الطرف/الجهة | Lookup/Reference | Draft |
| 3 | `counterAccountRef` | الحساب المقابل | Lookup/Reference | Draft |
| 4 | `lineDescription` | البيان | Text | Draft |
| 5 | `currencyRef` | العملة | Reference | Draft |
| 6 | `amount` | المبلغ | MonetaryAmount | Draft |
| 7 | `exchangeRate` | سعر الصرف | Decimal/Rate | Draft |
| 8 | `accountingAmount` | المبلغ المحاسبي | Derived MonetaryAmount | read-only |

Primary description owns Fill; reference/numeric columns use shared semantic width policies. Exact lookup providers, DTO fields, sort keys and accountingAmount formula/binding remain TBD-GATED.

## UX — TEAM-D04 PASS
- Create/Edit work only in Draft/eligible states with permission and expected-version semantics.
- Post is advisory-enabled in UI but server rechecks period, accounting validity, numbering, approval state, scope and version.
- Cancel requires eligible state and reason/version per W2; no silent transition.
- Reverse applies to eligible posted records only; original remains immutable.
- Concurrency conflicts use shared Refresh/Reload UX; no silent overwrite.
- Approvals tab is a governing structural/read-only area; it creates no direct decision command, and exact content/source binding remains implementation-owned where not issued.
- Attachments tab is a governing structural area; no Upload/Download/Delete command or data-source contract is created without W2 authority.
- No Print/Export and no offline write/outbox/replay.

## VISUAL — TEAM-D05 PASS
CoreUI owns RTL, typography, spacing, toolbar, validation/error/loading states, tabs, grid, audit, focus and DPI. Amount/rate/accounting amount use shared numeric presentation. No local financial color/formula semantics.

## TEAM-D06 — INDEPENDENT REVIEW PASS
- Result: `PASS`; open design findings: `0`.
- Exact five tabs, 11 fields and 8-column line grid preserved.
- Executable actions remain exactly View/Create/Edit/Cancel/Post/Reverse.
- No Print/Export, attachment mutation, direct approval decision, offline write, local accounting formula or CoreUI duplication.
- Owner decision remains UI-only; technical mappings remain explicit gates.

## Remaining technical gates
- FBR-038..039 field-level mapping closure.
- Unissued lookup provider/source/revalidation contracts.
- Exact DTO/line/sort-key bindings and accountingAmount formula/binding.
- Attachment/approval data-source authority if later issued.
- Runtime/acceptance execution.
