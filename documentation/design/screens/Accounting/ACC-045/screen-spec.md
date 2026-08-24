# ACC-045 — سند التحويل بين الصناديق والبنوك — Canonical Screen Specification

**English:** Cash/Bank Transfer Voucher  
**Module:** Accounting  
**Profile / Variant:** `Transaction / Transfer`  
**CurrentDesignState:** `INDEPENDENT_REVIEW`  
**OwnerTeam:** `TEAM-D06`  
**Batch:** `BATCH-12`

## Authority
- Current 57-screen baseline + Unified Design/Execution V1.3.
- W1 aggregate: `TransferVoucher`; related context Cashbox/BankAccount/Currency/JournalEntry.
- W2 exact actions only: `ACC045.View/Create/Edit/Cancel/Post/Reverse`; List/Get/Create/Update/Cancel/Post/Reverse.
- Owner design-only decision is recorded in the Batch-12 authority file; unresolved field/lookup bindings remain `TBD-GATED`.

## ANALYSIS — TEAM-D01 PASS
Purpose: create and maintain a cash/bank transfer Draft with explicit source/destination semantics, then cancel/post/reverse through server-authoritative commands.

Tabs: البيانات الرئيسية | التفاصيل والحركات | المرفقات والربط بالمستندات | الاعتمادات | سجل العمليات.

No screen-specific Print/Export, attachment mutation, direct Approve/Reject/Return, Delete or offline-write authority is issued. Posted records are immutable.

## LAYOUT — TEAM-D02 PASS
Shared Transaction layout with Transfer semantics only: `Header/MainData(Content) → Tabs/Workspace(Fill) → Lines/Grid(Fill) → Totals/Actions(Content/Fixed) → Audit`.

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

Transfer source/destination validity, period checks, posting rules, exchange-rate semantics and accounting effects remain server/domain-authoritative. The UI does not infer additional transfer types or formulas.

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
- Source and destination are separate presentation roles; the client does not invent routing/accounting rules beyond server validation.
- Post is advisory-enabled in UI but server rechecks period, accounting validity, numbering, approval state, scope and version.
- Cancel requires eligible state and reason/version per W2; no silent transition.
- Reverse applies to eligible posted records only; original remains immutable.
- Concurrency conflicts use shared Refresh/Reload UX; no silent overwrite.
- Approvals tab is read-only status/history/context; no direct approval decision command.
- Attachments tab has no Upload/Download/Delete command until explicit W2 authority exists.
- No Print/Export and no offline write/outbox/replay.

## VISUAL — TEAM-D05 PASS
CoreUI owns RTL, typography, spacing, toolbar, validation/error/loading states, tabs, grid, audit, focus and DPI. Source/destination references are visually distinct labels but use the same shared lookup component; no local provider or business rule is embedded. Amount/rate/accounting amount use shared numeric presentation.

## Remaining technical gates
- FBR-042..043 field-level mapping closure.
- Unissued lookup provider/source/revalidation contracts.
- Exact DTO/line/sort-key bindings and accountingAmount formula/binding.
- Attachment authority if later issued.
- Runtime/acceptance execution.

## TEAM-D06 review input
Verify identity/profile/variant = ACC-045 / Transaction / Transfer; exact five tabs, 11 fields and 8 columns; actions exactly View/Create/Edit/Cancel/Post/Reverse; no Print/Export/attachment/direct-approval/offline action; source/destination semantics do not invent accounting rules; posted immutability; owner decision remains UI-only; technical gaps remain TBD-GATED; no CoreUI duplication or client accounting formula.
